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

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Versions")]
    public class VersionsController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/Versions
        public IQueryable<VERSIONS> GetVERSIONS()
        {
            return db.VERSIONS;
        }

        // GET: api/Versions/5
        [ResponseType(typeof(List<VERSIONS>))]
        public async Task<IHttpActionResult> GetVERSIONS(long id)
        {
            List<VERSIONS> vERSIONS = await db.VERSIONS.Where(x => x.META_BO_ID == id).ToListAsync();
            if (vERSIONS.Count <= 0)
            {
                return NotFound();
            }

            return Ok(vERSIONS);
        }

        // PUT: api/Versions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVERSIONS(long id, VERSIONS vERSIONS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vERSIONS.VERSIONS_ID)
            {
                return BadRequest();
            }

            db.Entry(vERSIONS).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VERSIONSExists(id))
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

        // POST: api/Versions
        [ResponseType(typeof(VERSIONS))]
        public async Task<IHttpActionResult> PostVERSIONS(VERSIONS vERSIONS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VERSIONS.Add(vERSIONS);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = vERSIONS.VERSIONS_ID }, vERSIONS);
        }

        // DELETE: api/Versions/5
        [ResponseType(typeof(VERSIONS))]
        public async Task<IHttpActionResult> DeleteVERSIONS(long id)
        {
            VERSIONS vERSIONS = await db.VERSIONS.FindAsync(id);
            if (vERSIONS == null)
            {
                return NotFound();
            }

            db.VERSIONS.Remove(vERSIONS);
            await db.SaveChangesAsync();

            return Ok(vERSIONS);
        }

        [HttpPost]
        [Route("Commit/{id}")]
        [ResponseType(typeof(VERSIONS))]
        public async Task<IHttpActionResult> CommitVERSIONS(long id)
        {
            VERSIONS vERSIONS = await db.VERSIONS.FindAsync(id);
            if (vERSIONS == null)
            {
                return NotFound();
            }

            META_BO mETA_BO = await db.META_BO.FindAsync(vERSIONS.META_BO_ID);
            if (mETA_BO.META_FIELD.Count <= 0)
            {
                return BadRequest("No meta field found !");
            }

            var sqlQuery = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/SQL/CreateTable.sql"));

            var fields = "";
            foreach (var f in mETA_BO.META_FIELD)
            {
                fields += string.Format(" [{0}] {1} {2} , "
                    , f.DB_NAME
                    , f.DB_TYPE
                    , f.DB_NULL == 0 ? " NOT NULL " : " NULL "
                    );
            }

            sqlQuery = sqlQuery
                .Replace("{TABLE_NAME}", mETA_BO.BO_DB_NAME)
                .Replace("{FIELDS}", fields)
                .Replace("\r\n", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                ;
            var s = new SGBD();
            s.Cmd(sqlQuery);
            s.Cmd(" update versions set STATUS='OLD' where META_BO_ID=" + vERSIONS.META_BO_ID);
            
            vERSIONS.STATUS = "ACTIVE";
            db.VERSIONS.Add(new VERSIONS()
            {
                META_BO_ID = mETA_BO.META_BO_ID,
                NUM = 1,
                SQLQUERY = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/SQL/CreateTable.sql")),
                STATUS = "PENDING",
                CREATED_BY = User.Identity.Name,
                UPDATED_BY = User.Identity.Name,
            });

            db.SaveChanges();
            return Ok(vERSIONS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VERSIONSExists(long id)
        {
            return db.VERSIONS.Count(e => e.VERSIONS_ID == id) > 0;
        }
    }
}