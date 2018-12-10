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
using FAIS.Models.Repository;

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
            List<VERSIONS> vERSIONS = await db.VERSIONS.Where(x => x.META_BO_ID == id).OrderByDescending(x=>x.VERSIONS_ID).ToListAsync();
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

            META_BO mETA_BO = await new MetaBoRepo().GetMETAForCommitAsync(vERSIONS.META_BO_ID.Value); // await db.META_BO.FindAsync(vERSIONS.META_BO_ID);
            if (mETA_BO.META_FIELD.Count <= 0)
            {
                return BadRequest("No meta field found !");
            }

            var fields = "";
            foreach (var f in mETA_BO.META_FIELD)
            {
                fields += string.Format(" [{0}] {1} {2} , "
                    , f.DB_NAME
                    , f.DB_TYPE
                    , f.DB_NULL == 0 ? " NOT NULL " : " NULL "
                    );
            }

            var sqlQuery = Helper.GetSQL("CreateTable.sql");
                sqlQuery = string.Format(sqlQuery,
                vERSIONS.VERSIONS_ID.ToString()
                , mETA_BO.BO_DB_NAME + vERSIONS.NUM
                , fields
                , User.Identity.Name
                , mETA_BO.META_BO_ID.ToString()
                , vERSIONS.NUM
                , mETA_BO.BO_DB_NAME);
                
            
            sqlQuery = sqlQuery.Replace("[SQLQUERY]", sqlQuery.Replace("'","''"));

            var s = new SGBD();
            s.Cmd(sqlQuery);
            s.Cmd(new VersionsModels().GenerateView(mETA_BO.BO_DB_NAME));

            return Ok();
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