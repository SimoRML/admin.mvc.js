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
    [Authorize]
    [RoutePrefix("api/MetaField")]
    public class MetaFieldController : ApiController
    {
        private FAISEntities db = new FAISEntities();


        // GET: api/MetaField/5
        [ResponseType(typeof(META_FIELD))]
        public async Task<IHttpActionResult> GetMETA_FIELD(long id)
        {
            META_FIELD mETA_FIELDs = await db.META_FIELD.FindAsync(id);
            if (mETA_FIELDs == null)
            {
                return NotFound();
            }

            return Ok(mETA_FIELDs);
        }
        // GET: api/MetaField/5
        [ResponseType(typeof(META_FIELD))]
        [Route("metabo/{id}")]
        public async Task<IHttpActionResult> GetMetaBoMETA_FIELD(long id)
        {
            List<META_FIELD> mETA_FIELDs = await db.META_FIELD.Where(x => x.META_BO_ID == id).ToListAsync();
            if (mETA_FIELDs == null)
            {
                return NotFound();
            }

            return Ok(mETA_FIELDs);
        }

        // PUT: api/MetaField/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMETA_FIELD(long id, META_FIELD model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            META_FIELD metafeldDB = await db.META_FIELD.FindAsync(id);
            if (metafeldDB == null)
            {
                return NotFound();
            }

            metafeldDB.DB_NULL = model.DB_NULL;
            metafeldDB.GRID_NAME = model.GRID_NAME;
            metafeldDB.FORM_NAME = model.FORM_NAME;
            metafeldDB.FORM_TYPE = model.FORM_TYPE;
            metafeldDB.FORM_SOURCE = model.FORM_SOURCE;
            metafeldDB.FORM_OPTIONAL = model.FORM_OPTIONAL;
            metafeldDB.UPDATED_BY = User.Identity.Name;
            metafeldDB.UPDATED_DATE = DateTime.Now;


            db.Entry(metafeldDB).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
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
            mETA_FIELD.CREATED_BY = User.Identity.Name;
            mETA_FIELD.UPDATED_BY = User.Identity.Name;
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