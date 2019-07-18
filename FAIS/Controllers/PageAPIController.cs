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
    public class PageAPIController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/PageAPI
        public IQueryable<PAGE> GetPAGE()
        {
            return db.PAGE;
        }

        // GET: api/PageAPI/5
        [ResponseType(typeof(PAGE))]
        public async Task<IHttpActionResult> GetPAGE(long id)
        {
            PAGE pAGE = await db.PAGE.FindAsync(id);
            if (pAGE == null)
            {
                return NotFound();
            }

            return Ok(pAGE);
        }

        // PUT: api/PageAPI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPAGE(long id, PAGE pAGE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pAGE.BO_ID)
            {
                return BadRequest();
            }


            pAGE.UPDATED_DATE = DateTime.Now;
            pAGE.UPDATED_BY = User.Identity.Name;
            
            db.Entry(pAGE).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PAGEExists(id))
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

        // POST: api/PageAPI
        [ResponseType(typeof(PAGE))]
        public async Task<IHttpActionResult> PostPAGE(PAGE pAGE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pAGE.CREATED_DATE = DateTime.Now;
            pAGE.UPDATED_DATE = DateTime.Now;
            pAGE.CREATED_BY = User.Identity.Name;
            pAGE.UPDATED_BY = User.Identity.Name;
            pAGE.STATUS = "New";
            db.PAGE.Add(pAGE);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pAGE.BO_ID }, pAGE);
        }

        // DELETE: api/PageAPI/5
        [ResponseType(typeof(PAGE))]
        public async Task<IHttpActionResult> DeletePAGE(long id)
        {
            PAGE pAGE = await db.PAGE.FindAsync(id);
            if (pAGE == null)
            {
                return NotFound();
            }

            db.PAGE.Remove(pAGE);
            await db.SaveChangesAsync();

            return Ok(pAGE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PAGEExists(long id)
        {
            return db.PAGE.Count(e => e.BO_ID == id) > 0;
        }
    }
}