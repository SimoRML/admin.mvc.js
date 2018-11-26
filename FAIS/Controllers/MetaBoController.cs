using FAIS.Models;
using FAIS.Models.VForm;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/metabo")]
    public class MetaBoController : ApiController
    {
        private FAISEntities db = new FAISEntities();

        // GET: api/MetaBo
        public IQueryable<META_BO> GetMETA_BO()
        {
            return db.META_BO;
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

        // PUT: api/MetaBo/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMETA_BO(long id, META_BO mETA_BO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mETA_BO.META_BO_ID)
            {
                return BadRequest();
            }

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
        [Route("Crud")]
        public async Task<IHttpActionResult> Insert(CrudModel model)
        {
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
            int id = (int)bo_model.BO_ID;

            model.MetaBO = meta;
            model.Items.Add("BO_ID", id);
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

       
    }
}