using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("api/MetaField")]
    public class MetaFieldController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaField
        public IQueryable<META_FIELD> GetMETA_FIELD()
        {
            return db.META_FIELD;
        }

        // GET: api/MetaField/5
        [ResponseType(typeof(List<META_FIELD>))]
        public async Task<IHttpActionResult> GetMETA_FIELD(long id)
        {
            var mETA_FIELDs = await db.META_FIELD.Where(x => x.META_BO.META_BO_ID == id).ToListAsync();
            if (mETA_FIELDs.Count == 0)
            {
                return NotFound();
            }

            return Ok(mETA_FIELDs);
        }


        [ResponseType(typeof(List<META_FIELD>))]
        public async Task<IHttpActionResult> GetMETA_FIELD([FromUri]string bo_name)
        {
            var mETA_FIELDs = await db.META_FIELD.Where(x => x.META_BO.BO_NAME == bo_name && x.STATUS == "ACTIVE").ToListAsync();
            if (mETA_FIELDs.Count == 0)
            {
                return NotFound();
            }

            return Ok(mETA_FIELDs);
        }

        [HttpPost]
        [Route("SelectSource")]
        [ResponseType(typeof(List<SelectDataModel>))]
        public async Task<IHttpActionResult> SelectSource(SelectSourceModel model)
        {
            var data = await model.GetAsync(db);
            return Ok(data);
        }

        // PUT: api/MetaField/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMETA_FIELD(long id, META_FIELD mETA_FIELD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mETA_FIELD.META_FIELD_ID)
            {
                return BadRequest();
            }

            db.Entry(mETA_FIELD).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!META_FIELDExists(id))
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

        // POST: api/MetaField
        [ResponseType(typeof(META_FIELD))]
        public async Task<IHttpActionResult> PostMETA_FIELD(META_FIELD mETA_FIELD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.META_FIELD.Add(mETA_FIELD);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = mETA_FIELD.META_FIELD_ID }, mETA_FIELD);
        }

        // DELETE: api/MetaField/5
        [ResponseType(typeof(META_FIELD))]
        public async Task<IHttpActionResult> DeleteMETA_FIELD(long id)
        {
            META_FIELD mETA_FIELD = await db.META_FIELD.FindAsync(id);
            if (mETA_FIELD == null)
            {
                return NotFound();
            }

            db.META_FIELD.Remove(mETA_FIELD);
            await db.SaveChangesAsync();

            return Ok(mETA_FIELD);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool META_FIELDExists(long id)
        {
            return db.META_FIELD.Count(e => e.META_FIELD_ID == id) > 0;
        }
    }
}