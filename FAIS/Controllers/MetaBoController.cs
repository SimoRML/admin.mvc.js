using FAIS.Models;
using FAIS.Models.Repository;
using FAIS.Models.VForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Z.EntityFramework.Plus;

namespace FAIS.Controllers
{

    [RoutePrefix("api/MetaBo")]
    public class MetaBoController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaBo
        public async Task<IHttpActionResult> GetMETA_BOAsync()
        {
            return Ok(await db.META_BO.Where(x => x.STATUS != "-1").ToListAsync());
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
            var data = await model.GetAsync(db);
            return Ok(data);
        }

        // PUT: api/MetaBo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMETA_BO(long id, META_BO mETA_BO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            mETA_BO.META_BO_ID = id;
            mETA_BO.UPDATED_BY = User.Identity.Name;
            mETA_BO.UPDATED_DATE = DateTime.Now;

            db.Entry(mETA_BO).State = EntityState.Modified;

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

            mETA_BO.BO_DB_NAME += "_BO_";
            mETA_BO.VERSION = 1;
            mETA_BO.CREATED_BY = User.Identity.Name;
            mETA_BO.UPDATED_BY = User.Identity.Name;
            mETA_BO.CREATED_DATE = DateTime.Now;
            mETA_BO.UPDATED_DATE = DateTime.Now;
            db.META_BO.Add(mETA_BO);

            // int lastVersion = db.VERSIONS.Where(x => x.META_BO_ID == mETA_BO.META_BO_ID).Max(x => x.NUM);
            db.VERSIONS.Add(new VERSIONS()
            {
                META_BO_ID = mETA_BO.META_BO_ID,
                NUM = 1,
                SQLQUERY = Helper.GetSQL("CreateTable.sql"),
                STATUS = "PENDING",
                CREATED_BY = User.Identity.Name,
                UPDATED_BY = User.Identity.Name,
            });

            await db.SaveChangesAsync();

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
            // var BoRepository = new BORepository(meta);

            return Ok();
        }

        [HttpPost]
        [Route("Crud/{id}")]
        public async Task<IHttpActionResult> Insert(int id, Dictionary<string, object> Items)
        {
            var model = new CrudModel()
            {
                MetaBoID = id,
                Items = Items
            };
            var meta = await db.META_BO.FindAsync(model.MetaBoID);

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
            bool insert = model.Insert();
            if (insert)
                return Ok(model);
            else
                return InternalServerError();
        }

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
            bool insert = model.Insert();
            if (insert)
                return Ok(model);
            else
                return InternalServerError();
        }

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

            var BO_ToUpdate = db.BO.SingleOrDefault(bo => bo.BO_ID == bo_id);

            BO_ToUpdate.UPDATED_BY = User.Identity.Name;
            BO_ToUpdate.UPDATED_DATE = DateTime.Now;
            model.MetaBO = meta;
            await db.SaveChangesAsync();

            model.Items.Add("BO_ID", BO_ToUpdate.BO_ID);
            db.MoveBoToCurrentVersion(BO_ToUpdate.BO_ID);
            string update_ = model.Update();
            if (update_ == "true")
                return Ok(model);
            else
                return InternalServerError(new Exception(update_));
        }
        [HttpPut]
        [Route("Crud/{id}/{parentId}/{bo_id}")]
        public async Task<IHttpActionResult> UpdateChilds(long id, long parentId, long bo_id, Dictionary<string, object> Items)
        {
            return await Update(id, bo_id, Items);
        }

        [HttpGet]
        [Route("Select/{Tname}")]
        public async Task<IHttpActionResult> Select(string Tname)
        {
            var meta = await db.META_BO.Where(x => x.BO_DB_NAME == Tname).FirstOrDefaultAsync();
            if (meta == null)
            {
                return BadRequest();
            }

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelect(meta.BO_DB_NAME));

            return Ok(dt);
        }

        [HttpGet]
        [Route("SelectChilds/{Tname}/{parentId}")]
        public async Task<IHttpActionResult> SelectChilds(string Tname, long parentId)
        {
            var meta = await db.META_BO.Where(x => x.BO_DB_NAME == Tname).FirstOrDefaultAsync();
            if (meta == null)
            {
                return BadRequest();
            }

            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelectChilds(meta.BO_DB_NAME, parentId));

            return Ok(dt);
        }

        [HttpGet]
        [Route("Crud/{id}/{param}")]
        public async Task<IHttpActionResult> GetOne(long id, long param)
        {


            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var meta = await db.META_BO.FindAsync(id);
            Dictionary<string, object> bind = new Dictionary<string, object>();
            bind.Add("BO_ID", param);
            var dt = s.Cmd(Gen.GenSelectOne(meta.BO_DB_NAME), bind);


            return Ok(dt);
        }

    }
}