using FAIS.Models;
using FAIS.Models.Authorize;
using FAIS.Models.Repository;
using FAIS.Models.VForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/MetaBo")]
    public class MetaBoController : ApiController
    {
        int BO_Insert_return = 0;
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaBo
        public async Task<IHttpActionResult> GetMETA_BOAsync()
        {
            return Ok(await new MetaBoRepo().GetMETAListOnlyAsync());
        }

        // GET: api/MetaBo/5
        [ResponseType(typeof(META_BO))]
        public async Task<IHttpActionResult> GetMETA_BO(long id)
        {
            META_BO mETA_BO = await db.META_BO.FindAsync(id);
            if (mETA_BO == null)
            {
                return NotFound();
            }

            return Ok(mETA_BO);
        }

        // GET: api/MetaBo/5
        [Route("byName/{name}")]
        public async Task<IHttpActionResult> GetMETA_BOByName(string name)
        {
            long META_BO_ID = await db.META_BO.Where(x=> x.BO_DB_NAME == name)
                .Select(x=>x.META_BO_ID)
                .FirstOrDefaultAsync();

            return Ok(META_BO_ID);
        }
        [ResponseType(typeof(List<META_FIELD>))]
        [Route("GetDefinition/{id}")]
        public async Task<IHttpActionResult> GetDefinition(string id)
        {
            META_BO mETA_BO = await new MetaBoRepo().GetMETAAsync(id);
            if (mETA_BO == null)
            {
                return NotFound();
            }
            return Ok(mETA_BO);
        }
        [HttpPost]
        [Route("SelectSource")]
        [ResponseType(typeof(List<SelectDataModel>))]
        public async Task<IHttpActionResult> SelectSource(SelectSourceModel model)
        {
            var data = model.Get();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetDataSources")]
        public async Task<IHttpActionResult> GetDataSources()
        {
            return Ok(MetaBoRepo.GetDataSources());
        }
        // PUT: api/MetaBo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMETA_BO(long id, META_BO mETA_BO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            META_BO meta = await db.META_BO.FindAsync(id);
            if (meta == null)
            {
                return BadRequest("META_BO NOT FOUND !");
            }

            meta.BO_NAME = mETA_BO.BO_NAME;
            meta.TYPE = mETA_BO.TYPE;
            meta.JSON_DATA = mETA_BO.JSON_DATA;

            meta.UPDATED_BY = User.Identity.Name;
            meta.UPDATED_DATE = DateTime.Now;

            db.Entry(meta).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!META_BOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MetaBo
        [ResponseType(typeof(META_BO))]
        public async Task<IHttpActionResult> PostMETA_BO(META_BO mETA_BO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //mETA_BO.BO_DB_NAME += "_BO_";
            //mETA_BO.VERSION = 1;
            //mETA_BO.CREATED_BY = User.Identity.Name;
            //mETA_BO.UPDATED_BY = User.Identity.Name;
            //mETA_BO.CREATED_DATE = DateTime.Now;
            //mETA_BO.UPDATED_DATE = DateTime.Now;
            //db.META_BO.Add(mETA_BO);

            //// int lastVersion = db.VERSIONS.Where(x => x.META_BO_ID == mETA_BO.META_BO_ID).Max(x => x.NUM);
            //db.VERSIONS.Add(new VERSIONS()
            //{
            //    META_BO_ID = mETA_BO.META_BO_ID,
            //    NUM = 1,
            //    SQLQUERY = Helper.GetSQL("CreateTable.sql"),
            //    STATUS = "PENDING",
            //    CREATED_BY = User.Identity.Name,
            //    UPDATED_BY = User.Identity.Name,
            //});

            //await db.SaveChangesAsync();
            mETA_BO = await new MetaBoRepo().CreateAndSaveAsync(mETA_BO, User.Identity.Name);
            return CreatedAtRoute("DefaultApi", new { id = mETA_BO.META_BO_ID }, mETA_BO);
        }

        // DELETE: api/MetaBo/5
        [ResponseType(typeof(META_BO))]
        public async Task<IHttpActionResult> DeleteMETA_BO(long id)
        {
            META_BO mETA_BO = await db.META_BO.FindAsync(id);
            if (mETA_BO == null)
            {
                return NotFound();
            }

            db.META_BO.Remove(mETA_BO);
            await db.SaveChangesAsync();

            return Ok(mETA_BO);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool META_BOExists(long id)
        {
            return db.META_BO.Count(e => e.META_BO_ID == id) > 0;
        }


        [HttpPost]
        [Route("Filter")]
        public async Task<IHttpActionResult> Filter(FilterModel model)
        {
            var meta = await db.META_BO.FindAsync(model.MetaBoID);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            if (meta == null)
            {
                return BadRequest();
            }


            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            string reqSelect = Gen.GenSelect(meta.BO_DB_NAME) + " where 1=1 " + model.Format();
            var dt = s.Cmd(reqSelect, model.mapping);


            return Ok(dt);
            //return Ok();
        }
        [HttpPost]
        [Route("Filter/includesubform")]
        public async Task<IHttpActionResult> FilterIncludeSubForm(FilterModel model)
        {
            var meta = await db.META_BO.FindAsync(model.MetaBoID);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            if (meta == null)
            {
                return BadRequest();
            }


            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            string reqSelect = Gen.GenSelectIncludeSubForm(meta, db.GetSubForm((int)meta.META_BO_ID).FirstOrDefault()) + " where 1=1 " + model.Format();
            var dt = s.Cmd(reqSelect, model.mapping);


            return Ok(dt);
            //return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("test/{key}")]
        public async Task<IHttpActionResult> test(string key)
        {
            return Ok(SessionVar.Get<object>(key));
        }

        // INSERT BO multi
        [HttpPost]
        [Route("CrudMultiple/{id}")]
        public async Task<IHttpActionResult> InsertMultiple(int id, List<Dictionary<string, object>> Items)
        {
            var meta = await db.META_BO.FindAsync(id);
            if (meta == null) return NotFound();

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            List<object> result = new List<object>();
            string insert = "-";
            bool deleted = false;
            foreach (var ligne in Items)
            {
                var model = new CrudModel()
                {
                    MetaBoID = id,
                    Items = ligne
                };

                BO bo_model = new BO()
                {
                    CREATED_BY = User.Identity.Name,
                    CREATED_DATE = DateTime.Now,
                    UPDATED_BY = User.Identity.Name,
                    UPDATED_DATE = DateTime.Now,
                    STATUS = "1",
                    BO_TYPE = model.MetaBoID.ToString(),
                    VERSION = meta.VERSION
                };

                db.BO.Add(bo_model);
                await db.SaveChangesAsync();
                int id_ = (int)bo_model.BO_ID;

                model.MetaBO = meta;
                model.BO_ID = id_;
                model.Items.Add("BO_ID", id_);
                insert = model.Insert();

                result.Add(new { inserted = insert == "", BO_ID = id_ });
                if (insert != "")
                {
                    deleted = true;
                    db.Entry(bo_model).State = EntityState.Deleted;
                }
            }
            if (deleted) await db.SaveChangesAsync();
            return Ok(result);
        }

        // INSERT BO
        [HttpPost]
        [Route("Crud/{id}")]
        public async Task<IHttpActionResult> Insert(int id, Dictionary<string, object> Items)
        {
            var model = new CrudModel()
            {
                MetaBoID = id,
                Items = Items
            };
            // var meta = await db.META_BO.FindAsync(model.MetaBoID);
            var meta = db.META_BO.Find(model.MetaBoID);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            BO bo_model = new BO()
            {
                CREATED_BY = User.Identity.Name,
                CREATED_DATE = DateTime.Now,
                UPDATED_BY = User.Identity.Name,
                UPDATED_DATE = DateTime.Now,
                STATUS = "1",
                BO_TYPE = model.MetaBoID.ToString(),
                VERSION = meta.VERSION
            };

            db.BO.Add(bo_model);
            await db.SaveChangesAsync();
            int id_ = (int)bo_model.BO_ID;
            BO_Insert_return = id_;
            model.MetaBO = meta;
            model.BO_ID = id_;
            model.Items.Add("BO_ID", model.BO_ID);
            
            //return Ok(model.FormatInsert());
            string insert = model.Insert();

            // Workflow executer begin 
            var s = new SGBD();

            var db_workflow = s.Cmd(" select * from WORKFLOW CROSS APPLY OPENJSON(ITEMS) with(type varchar(50) '$.type',   " +
                "  precedent varchar(50) '$.precedent',     [index] int '$.index',    val nvarchar(500) '$.value.value') as jsonValues where ACTIVE = 1 and jsonValues.val = " + id +
                "and jsonValues.type = 'bo'");
            dynamic _JSON;
            var where = "";
            foreach (DataRow item in db_workflow.Rows)
            {
                _JSON = System.Web.Helpers.Json.Decode(item["ITEMS"].ToString());
                int level = 0;

                for (var i = (int)item["index"] + 1; i < _JSON.Length; i++)
                {

                    var elm = _JSON[i];
                    if (elm["status"] != "deleted")
                    {
                        where = "";
                        level++;
                        if (elm["type"] == "validation")
                        {

                            foreach (var rule in elm.value.rules)
                            {
                                var value = rule["value"];
                                if (!"int,float,decimal,nvarchar(MAX)".Contains(rule["field"]["DB_TYPE"]))
                                {
                                    value = "'" + value + "'";
                                }
                                where += " " + rule["logic"] + " " + rule["field"]["DB_NAME"] + " " + rule["condition"] + " " + value;
                            }

                            var check_validation = s.Cmd("select * from " + meta.BO_DB_NAME + " where BO_ID=" + model.BO_ID + "  " + where);


                            if (check_validation.Rows.Count > 0)
                            {
                                TASK valid = new TASK()
                                {
                                    BO_ID = id_,
                                    JSON_DATA = System.Web.Helpers.Json.Encode(elm.value.validators),
                                    CREATED_BY = User.Identity.Name,
                                    CREATED_DATE = DateTime.Now,
                                    STATUS = elm.value.status,
                                    ETAT = 0,
                                    TASK_LEVEL = level,
                                    TASK_TYPE = "VALIDATION"
                                };

                                db.TASK.Add(valid);

                                foreach (var _validator in elm.value.validators)
                                {
                                    NOTIF notification = new NOTIF()
                                    {
                                        VALIDATOR = _validator["email"],
                                        CREATED_DATE = DateTime.Now,
                                        ETAT = 0
                                    };
                                    db.NOTIF.Add(notification);
                                }

                                db.SaveChanges();

                            }

                        }
                        else if (elm["type"] == "bo")
                        {
                            TASK valid = new TASK()
                            {
                                BO_ID = id_,
                                JSON_DATA = System.Web.Helpers.Json.Encode(elm.value),
                                CREATED_BY = User.Identity.Name,
                                CREATED_DATE = DateTime.Now,
                                ETAT = 0,
                                TASK_LEVEL = level,
                                TASK_TYPE = "BO"
                            };
                            db.TASK.Add(valid);

                            db.SaveChanges();
                            var task_id = (int)valid.TASK_ID;
                            // await Insert_Bo_Using_Mapping(id, meta.BO_DB_NAME, model.BO_ID, task_id);

                        }
                    }
                }


            }

            if (insert == "")
            {
                BO_Insert_return = id_;
                return Ok(model);
            }
            else
                return InternalServerError(new Exception(insert));
        }

        //Insert data automaticly using the Mapping on the WORKFLOWs
        public async Task<IHttpActionResult> Insert_Bo_Using_Mapping(int id, string BO_DB_NAME, int bo_id, int taks_id)
        {
            var s = new SGBD();

            dynamic _JSON_MAPP;
            // DATA 
            var mapping = s.Cmd("select * from TASK t where TASK_TYPE='BO' and TASK_ID=" + taks_id + " and bo_id not in (select bo_id from task where  TASK_TYPE='VALIDATION' and etat=0 and BO_ID=t.BO_ID)");
            if (mapping.Rows.Count == 0) return NotFound();

            _JSON_MAPP = System.Web.Helpers.Json.Decode(mapping.Rows[0]["JSON_DATA"].ToString());
            Dictionary<string, object> JSONA_STRING = new Dictionary<string, object>();
            Dictionary<string, object> JSONA_STRING_SUBFORM = new Dictionary<string, object>();
            foreach (var item in _JSON_MAPP["MyMapping"]["mapping"])
            {
                var originalField = s.Cmd("select DB_NAME from META_FIELD where FORM_NAME='" + item["parent"].ToString() + "' and META_BO_ID=" + id);

                var value = s.Cmd("select " + originalField.Rows[0][0].ToString() + " from " + BO_DB_NAME + " where BO_ID =" + bo_id);


                JSONA_STRING.Add(item.child_to_db.ToString(), value.Rows[0][0].ToString());


            }



            // DAT SUB-FORM
            var parentData = await Insert((int)_JSON_MAPP["value"], JSONA_STRING);
            //////foreach (var item_sub_form in _JSON_MAPP["MyMapping"]["mapping_sub_form"])
            //////{
            //////    var originalField = s.Cmd("select DB_NAME from META_FIELD where FORM_NAME='" + item_sub_form["parent"].ToString() + "' and META_BO_ID=" + item_sub_form["id_subform_p"]);
            //////    var tableName = s.Cmd("select BO_DB_NAME from META_bo where META_BO_ID=" + item_sub_form["id_subform_p"]);


            //////    var value_s = s.Cmd("select  " + originalField.Rows[0][0].ToString() + " from " + tableName.Rows[0][0].ToString() + "  where BO_ID in (select BO_CHILD_ID from BO_CHILDS where BO_PARENT_ID = " + bo_id + ")");


            //////    foreach (var v in value_s.Rows)
            //////    {
            //////        JSONA_STRING_SUBFORM = new Dictionary<string, object>();
            //////        JSONA_STRING_SUBFORM.Add(item_sub_form.child_to_db_sb.ToString(), ((System.Data.DataRow)v).ItemArray[0].ToString());
            //////        await InsertChilds((int)item_sub_form["id_subform_c"], (long)BO_Insert_return, JSONA_STRING_SUBFORM);
            //////    }


            //////}

            // TEST BEGIN

            var dt = s.Cmd("select BO_CHILD_ID from BO_CHILDS where BO_PARENT_ID = " + bo_id);
            int id_subform_c = 0;
            foreach (var r in dt.Rows)
            {
                JSONA_STRING_SUBFORM = new Dictionary<string, object>();
                foreach (var item_sub_form in _JSON_MAPP["MyMapping"]["mapping_sub_form"])
                {
                    var originalField = s.Cmd("select DB_NAME from META_FIELD where FORM_NAME='" + item_sub_form["parent"].ToString() + "' and META_BO_ID=" + item_sub_form["id_subform_p"]);
                    var tableName = s.Cmd("select BO_DB_NAME from META_bo where META_BO_ID=" + item_sub_form["id_subform_p"]);
                    var value_s = s.Cmd("select  " + originalField.Rows[0][0].ToString() + " from " + tableName.Rows[0][0].ToString() + "  where BO_ID =" + ((System.Data.DataRow)r).ItemArray[0].ToString());

                    id_subform_c = (int)item_sub_form["id_subform_c"];
                    JSONA_STRING_SUBFORM.Add(item_sub_form.child_to_db_sb.ToString(), value_s.Rows[0][0].ToString());

                }
                await InsertChilds(id_subform_c, (long)BO_Insert_return, JSONA_STRING_SUBFORM);
            }

            //TEST FIN


            s.Cmd("update task  set ETAT=1 where task_id=" + taks_id);
            return Ok(BO_DB_NAME);
        }

        // INSERT BO CHILDS
        [HttpPost]
        [Route("Crud/{id}/{parentId}")]
        public async Task<IHttpActionResult> InsertChilds(int id, long parentId, Dictionary<string, object> Items)
        {
            var model = new CrudModel()
            {
                MetaBoID = id,
                Items = Items
            };
            var meta = await db.META_BO.FindAsync(model.MetaBoID);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            BO bo_model = new BO()
            {
                CREATED_BY = User.Identity.Name,
                CREATED_DATE = DateTime.Now,
                UPDATED_BY = User.Identity.Name,
                UPDATED_DATE = DateTime.Now,
                STATUS = "1",
                BO_TYPE = model.MetaBoID.ToString(),
                VERSION = meta.VERSION
            };
            db.BO.Add(bo_model);
            await db.SaveChangesAsync();

            db.BO_CHILDS.Add(new BO_CHILDS()
            {
                BO_PARENT_ID = parentId,
                BO_CHILD_ID = bo_model.BO_ID,
                RELATION = "1..*"
            });
            await db.SaveChangesAsync();

            int id_ = (int)bo_model.BO_ID;

            model.MetaBO = meta;
            model.Items.Add("BO_ID", id_);
            string insert = model.Insert();
            if (insert == "")
                return Ok(model);
            else
                return InternalServerError(new Exception(insert));
        }

        // INSERT BO CHILDS multi
        [HttpPost]
        [Route("CrudMultipleChilds/{id}")]
        public async Task<IHttpActionResult> InsertMultipleChilds(int id, List<Dictionary<string, object>> Items)
        {
            var meta = await db.META_BO.FindAsync(id);
            if (meta == null) return NotFound();

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            List<object> result = new List<object>();
            string insert = "-";
            bool deleted = false;
            foreach (var ligne in Items)
            {
                BO bo_model = new BO()
                {
                    CREATED_BY = User.Identity.Name,
                    CREATED_DATE = DateTime.Now,
                    UPDATED_BY = User.Identity.Name,
                    UPDATED_DATE = DateTime.Now,
                    STATUS = "1",
                    BO_TYPE = id.ToString(),
                    VERSION = meta.VERSION
                };
                db.BO.Add(bo_model);
                await db.SaveChangesAsync();

                db.BO_CHILDS.Add(new BO_CHILDS()
                {
                    BO_PARENT_ID = (long)ligne["parentId"],
                    BO_CHILD_ID = bo_model.BO_ID,
                    RELATION = "1..*"
                });
                await db.SaveChangesAsync();

                int id_ = (int)bo_model.BO_ID;

                ligne.Remove("parentId");
                var model = new CrudModel()
                {
                    MetaBoID = id,
                    Items = ligne,
                    MetaBO = meta
                };
                model.Items.Add("BO_ID", id_);
                insert = model.Insert();
                result.Add(new { inserted = insert, BO_ID = id_ });

                if (insert != "")
                {
                    deleted = true;
                    db.Entry(bo_model).State = EntityState.Deleted;
                }
            }
            if (deleted) await db.SaveChangesAsync();
            return Ok(result);
        }

        // UPDATE BO
        [HttpPut]
        [Route("Crud/{id}/{bo_id}")]
        public async Task<IHttpActionResult> Update(long id, long bo_id, Dictionary<string, object> Items)
        {
            var model = new CrudModel()
            {
                MetaBoID = (int)id,
                Items = Items
            };
            var meta = await db.META_BO.FindAsync(id);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var BO_ToUpdate = db.BO.SingleOrDefault(bo => bo.BO_ID == bo_id);

            BO_ToUpdate.UPDATED_BY = User.Identity.Name;
            BO_ToUpdate.UPDATED_DATE = DateTime.Now;
            model.MetaBO = meta;
            await db.SaveChangesAsync();

            model.Items.Add("BO_ID", BO_ToUpdate.BO_ID);
            // db.MoveBoToCurrentVersion(BO_ToUpdate.BO_ID);
            string update_ = model.Update();
            if (update_ == "true")
                return Ok(model);
            else
                return InternalServerError(new Exception(update_));
        }

        // UPDATE BO CHILDS
        [HttpPut]
        [Route("Crud/{id}/{parentId}/{bo_id}")]
        public async Task<IHttpActionResult> UpdateChilds(long id, long parentId, long bo_id, Dictionary<string, object> Items)
        {
            return await Update(id, bo_id, Items);
        }

        // SELECT FROM BO
        [HttpGet]
        [Route("Select/{Tname}")]
        public async Task<IHttpActionResult> Select(string Tname)
        {
            var meta = await db.META_BO.Where(x => x.BO_DB_NAME == Tname).FirstOrDefaultAsync();

            if (meta == null)
            {
                return BadRequest();
            }

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelect(meta.BO_DB_NAME));

            return Ok(dt);
        }

        // SELECT FROM BO
        [HttpPost]
        [Route("SelectFields/{Tname}")]
        public async Task<IHttpActionResult> SelectFields(string Tname, List<string> fields)
        {
            //return Ok("c." + fields.Aggregate((a, b) =>  a + ", c." + b));
            var meta = await db.META_BO.Where(x => x.BO_DB_NAME == Tname).FirstOrDefaultAsync();

            if (meta == null)
            {
                return BadRequest();
            }

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelectFields(meta.BO_DB_NAME, fields));

            return Ok(dt);
        }


        // SELECT FROM BO CHILDS
        [HttpGet]
        [Route("SelectChilds/{Tname}/{parentId}")]
        public async Task<IHttpActionResult> SelectChilds(string Tname, long parentId)
        {
            var meta = await db.META_BO.Where(x => x.BO_DB_NAME == Tname).FirstOrDefaultAsync();
            if (meta == null)
            {
                return BadRequest();
            }
            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelectChilds(meta.BO_DB_NAME, parentId));

            return Ok(dt);
        }

        // SELECT ONE FROM BO
        [HttpGet]
        [Route("Crud/{id}/{param}")]
        public async Task<IHttpActionResult> GetOne(long id, long param)
        {
            var meta = await db.META_BO.FindAsync(id);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyRead(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            Dictionary<string, object> bind = new Dictionary<string, object>();
            bind.Add("BO_ID", param);
            var dt = s.Cmd(Gen.GenSelectOne(meta.BO_DB_NAME), bind);


            return Ok(dt);
        }

        // INSERT MASS META_BO
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("Mass")]
        public IHttpActionResult Mass(List<BoBulk> models)
        {
            foreach (var model in models)
            {
                model.CreateMetaBo(User.Identity.Name);
            }

            return Ok("CREATED");
        }

        // RUN WORKFLOW
        [HttpGet]
        [Route("validateWorkflow/{id}")]
        public async Task<IHttpActionResult> ValidateWokflow(int id)
        {


            var s = new SGBD();
            var tasks = s.Cmd("select top 1 * from Task where bo_id=" + id + " and etat=0 order by task_id  ");

            if (tasks.Rows.Count > 0)
            {
                if (tasks.Rows[0]["task_type"].ToString() == "VALIDATION")
                {
                    bool mine = false;
                    var listV = System.Web.Helpers.Json.Decode(tasks.Rows[0]["JSON_DATA"].ToString());
                    foreach (var v in listV)
                    {
                        if (v.email == User.Identity.Name)
                        {
                            mine = true;
                        }
                    }
                    if (mine) return Ok(new { success = true, task_id = tasks.Rows[0]["task_id"], type = tasks.Rows[0]["task_type"], status = tasks.Rows[0]["STATUS"] });
                    else return Ok(new { success = false });
                }
                else
                {
                    var dt = s.Cmd("select * from META_BO where META_BO_ID = (select bo_type from BO where BO_ID = " + id + ")");

                    await Insert_Bo_Using_Mapping(int.Parse(dt.Rows[0]["META_BO_ID"].ToString()), dt.Rows[0]["BO_DB_NAME"].ToString(), id, int.Parse(tasks.Rows[0]["task_id"].ToString()));
                    return Ok(new { success = true, type = "BO", bo = dt.Rows[0]["BO_NAME"].ToString() });
                }
            }


            return Ok(new { success = false });
        }

        // VALIDATE WORKFLOW TASK
        [HttpGet]
        [Route("valider/{id}")]
        public async Task<IHttpActionResult> valider(int id, string status, int boid)
        {
            var s = new SGBD();
            s.Cmd("update Task set etat = 1 where task_id=" + id);
            s.Cmd("update bo set status='" + status + "' where BO_ID=" + boid);

            return Ok(new { success = true, status });
        }

        // DELETE BO
        [HttpDelete]
        [Route("Crud/{id}/{bo_id}")]
        public async Task<IHttpActionResult> Delete(long id, long bo_id)
        {
            var meta = await db.META_BO.FindAsync(id);

            /* ACCESS RIGHTS */
            try
            {
                UserRoleManager.Instance.VerifyWrite(meta.BO_DB_NAME);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Content(HttpStatusCode.Forbidden, ex.Message);
            }
            /* FIN ACCESS RIGHTS */

            var BO_ToUpdate = db.BO.SingleOrDefault(bo => bo.BO_ID == bo_id);

            BO_ToUpdate.UPDATED_BY = User.Identity.Name;
            BO_ToUpdate.UPDATED_DATE = DateTime.Now;
            BO_ToUpdate.STATUS = "deleted";
            try
            {
                await db.SaveChangesAsync();
                return Ok(BO_ToUpdate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}