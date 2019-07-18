using FAIS.Models;
using FAIS.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

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
            List<META_FIELD> mETA_FIELDs = await db.META_FIELD.Where(x => x.META_BO_ID == id && !x.STATUS.Contains("[deleted]")).ToListAsync();
            if (mETA_FIELDs == null)
            {
                return NotFound();
            }

            return Ok(mETA_FIELDs);
        }

        [HttpPost]
        [Route("formtype")]
        public async Task<IHttpActionResult> FormType()
        {
            var formTypes = new[] {
                new { Value= "v-checkbox"   , Display= "Case à cocher" },
                new { Value= "v-datepicker" , Display= "Date" },
                new { Value= "v-email"      , Display= "Email" },
                new { Value= "v-file"       , Display= "Fichier" },
                new { Value= "v-label"      , Display= "Etiquette" },
                new { Value= "v-multi-image"     , Display= "Multi image" },
                new { Value= "v-number"     , Display= "Numeric" },
                new { Value= "v-select"     , Display= "Liste de choix" },
                new { Value= "v-select-multiple"     , Display= "Liste multi selection" },
                new { Value= "v-text"       , Display= "Text" },
                new { Value= "v-textarea"   , Display= "Zone de text" },
            }.ToList();

            var subForms = await db.META_BO.Where(x => x.TYPE == "subform" && x.STATUS != "PENDING").ToListAsync();
            foreach (var oneSubForm in subForms)
            {
                formTypes.Add(new { Value = "subform-" + oneSubForm.BO_DB_NAME, Display = "@" + oneSubForm.BO_NAME });
            }

            return Ok(formTypes);
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

            if (metafeldDB.STATUS.Trim() == "NEW")
            {
                metafeldDB.DB_NAME = model.DB_NAME;
                metafeldDB.DB_NULL = 1;//model.DB_NULL;
            }
            if (metafeldDB.STATUS.Trim() == "NEW" || (metafeldDB.FORM_TYPE != "v-datepicker" & metafeldDB.FORM_TYPE != "v-checkbox"))
                metafeldDB.FORM_TYPE = model.FORM_TYPE;

            metafeldDB.GRID_NAME = model.GRID_NAME;
            metafeldDB.GRID_SHOW = model.GRID_SHOW;
            metafeldDB.GRID_FORMAT = model.GRID_FORMAT;
            metafeldDB.FORM_NAME = model.FORM_NAME;
            metafeldDB.FORM_SOURCE = model.FORM_SOURCE;
            metafeldDB.FORM_OPTIONAL = model.FORM_OPTIONAL;
            metafeldDB.IS_FILTER = model.IS_FILTER;
            metafeldDB.UPDATED_BY = User.Identity.Name;
            metafeldDB.UPDATED_DATE = DateTime.Now;

            if (!string.IsNullOrEmpty(model.JSON_DATA)) metafeldDB.JSON_DATA = model.JSON_DATA;

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
            catch (DbEntityValidationException ex)
            {

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
            mETA_FIELD = await new MetaFieldRepo().CreateAndSaveAsync(mETA_FIELD, User.Identity.Name);
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

            mETA_FIELD.STATUS = "[deleted]" + mETA_FIELD.STATUS;
            db.Entry(mETA_FIELD).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (DbEntityValidationException ex)
            {
                return Ok(new { success = false });
            }
        }


        // DELETE: api/MetaField/GetDefaultValue/
        [ResponseType(typeof(META_FIELD))]
        [Route("GetDefaultValue")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDefaultValue(string format, string boName)
        {
            //string cle = "", formule = "", step = "", type = "";
            //bool inFomule = false;
            //int count = 0;

            //foreach (char car in format)
            //{
            //    if (car == '[')
            //    {
            //        formule += car;
            //        inFomule = true;
            //        continue;
            //    }
            //    if (car == ']')
            //    {
            //        formule += car;
            //        inFomule = false;
            //        continue;
            //    }

            //    if (inFomule)
            //    {
            //        formule += car;
            //        count++;
            //        if (count == 1)
            //        {
            //            switch (car)
            //            {
            //                case '+':
            //                    type = "plus";
            //                    break;
            //                case 'd':
            //                    type = "date";
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            step += car;
            //        }
            //    }
            //    else
            //    {
            //        cle += car;
            //    }
            //}

            //switch (type)
            //{
            //    case "plus":
            //        var rst = db.PlusSequenceNextID(cle, boName, int.Parse(step), 0).ToList()[0];
            //        return Ok(new { type, value = format.Replace(formule, rst.ToString()) });
            //    case "date":
            //        return Ok(new { type, value = step == "" ? DateTime.Now : DateTime.Now.AddDays(int.Parse(step)) });
            //    default:
            //        return BadRequest("Formule non prise en charge !");
            //}
            var df = new MetaFieldRepo().GetDefaultValue(format, boName, 0);
            if (df.type == "error")
                return BadRequest(df.msg);
            else
                return Ok(df);
        }

        // DELETE: api/MetaField/GetDefaultValue/
        [AllowAnonymous]
        [ResponseType(typeof(META_FIELD))]
        [Route("GetSubFormID/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSubFormID(int id)
        {
            var i = 0;
            return Ok(db.GetSubFormId(id));
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