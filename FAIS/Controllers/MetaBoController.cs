using FAIS.Models;
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

namespace FAIS.Controllers
{
    
    [RoutePrefix("api/MetaBo")]
    public class MetaBoController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaBo
        public async Task<IHttpActionResult> GetMETA_BOAsync()
        {
            return Ok(await db.META_BO.Where(x => x.STATUS == "1").ToListAsync());
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
            // META
            META_BO mETA_BO = await db.META_BO.Where(x => x.BO_NAME == id).FirstOrDefaultAsync();
            if (mETA_BO == null)
            {
                return NotFound();
            }

            //// Entity
            //int boId = -1;
            //META_BO entity = null;
            //if(int.TryParse(id, out boId))
            //{
            //    // TODO : call 
            //    entity = await db.META_BO.Where(x => x.META_BO_ID == boId).FirstOrDefaultAsync();
            //}

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
            /*
            if (id != mETA_BO.META_BO_ID)
            {
                return BadRequest();
            }
            */

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

            db.META_BO.Add(mETA_BO);

            // int lastVersion = db.VERSIONS.Where(x => x.META_BO_ID == mETA_BO.META_BO_ID).Max(x => x.NUM);
            db.VERSIONS.Add(new VERSIONS()
            {
                META_BO_ID = mETA_BO.META_BO_ID,
                NUM = 1,
                SQLQUERY = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/SQL/CreateTable.sql")),
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
                BO_TYPE = model.MetaBoID.ToString()
            };

            db.BO.Add(bo_model);
            await db.SaveChangesAsync();
            int id_ = (int)bo_model.BO_ID;

            model.MetaBO = meta;
            model.Items.Add("BO_ID", id_);
            bool insert = model.Insert();
            if (insert != false)
            {
                return Ok(model);
            }
            else
            {
                return Ok(model);

            }
        }
        [HttpPut]
        [Route("Crud/{id}")]
        public async Task<IHttpActionResult> Update(long id, CrudModel model)
        {
            var meta = await db.META_BO.FindAsync(model.MetaBoID);

            var BO_ToUpdate = db.BO.SingleOrDefault(bo => bo.BO_ID == id);

            BO_ToUpdate.UPDATED_BY = User.Identity.Name;
            BO_ToUpdate.UPDATED_DATE = DateTime.Now;
            model.MetaBO = meta;
            await db.SaveChangesAsync();
            bool update = model.Update();
            return Ok(model);
        }

        [HttpGet]
        [Route("Select/{Tname}")]
        public async Task<IHttpActionResult> Select(string Tname)
        {
            var s = new SGBD();
            var Gen = new BORepositoryGenerator();
            var dt = s.Cmd(Gen.GenSelect(Tname));


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
            var dt = s.Cmd(Gen.GenSelectOne(meta.BO_NAME), bind);


            return Ok(dt);
        }

    }
}