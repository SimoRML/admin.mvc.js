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
using FAIS.Models.Authorize;

namespace FAIS.Controllers
{
    [RoutePrefix("api/BoRole")]
    public class BoRoleController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/BoRole
        public IQueryable<BO_ROLE> GetBO_ROLE()
        {
            return db.BO_ROLE;
        }

        // GET: api/BoRole/5
        [ResponseType(typeof(BO_ROLE))]
        public async Task<IHttpActionResult> GetBO_ROLE(long id)
        {
            BO_ROLE bO_ROLE = await db.BO_ROLE.FindAsync(id);
            if (bO_ROLE == null)
            {
                return NotFound();
            }

            return Ok(bO_ROLE);
        }

        // PUT: api/BoRole/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBO_ROLE(long id, BO_ROLE bO_ROLE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bO_ROLE.BO_ROLE_ID)
            {
                return BadRequest();
            }

            db.Entry(bO_ROLE).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BO_ROLEExists(id))
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

        // POST: api/BoRole
        [ResponseType(typeof(BO_ROLE))]
        public async Task<IHttpActionResult> PostBO_ROLE(BO_ROLE bO_ROLE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BO_ROLE.Add(bO_ROLE);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bO_ROLE.BO_ROLE_ID }, bO_ROLE);
        }

        // DELETE: api/BoRole/5
        [ResponseType(typeof(BO_ROLE))]
        public async Task<IHttpActionResult> DeleteBO_ROLE(long id)
        {
            BO_ROLE bO_ROLE = await db.BO_ROLE.FindAsync(id);
            if (bO_ROLE == null)
            {
                return NotFound();
            }

            db.BO_ROLE.Remove(bO_ROLE);
            await db.SaveChangesAsync();

            return Ok(bO_ROLE);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BO_ROLEExists(long id)
        {
            return db.BO_ROLE.Count(e => e.BO_ROLE_ID == id) > 0;
        }
    }
}