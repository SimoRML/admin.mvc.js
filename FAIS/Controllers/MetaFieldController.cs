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

namespace FAIS.Controllers
{
    public class MetaFieldController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaField
        public IQueryable<META_FIELD> GetMETA_FIELD()
        {
            return db.META_FIELD;
        }

        // GET: api/MetaField/5
        [ResponseType(typeof(META_FIELD))]
        public async Task<IHttpActionResult> GetMETA_FIELD(long id)
        {
            META_FIELD mETA_FIELD = await db.META_FIELD.FindAsync(id);
            if (mETA_FIELD == null)
            {
                return NotFound();
            }

            return Ok(mETA_FIELD);
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