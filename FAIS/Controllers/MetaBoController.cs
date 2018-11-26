using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FAIS.Models;
using FAIS.Models.VForm;

namespace FAIS.Controllers
{
    [Authorize]
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
            db.VERSIONS.Add(new VERSIONS() {
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
    }
}