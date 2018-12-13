using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Z.EntityFramework.Plus;

namespace FAIS.Models.Repository
{
    public class MetaBoRepo
    {
        private FAISEntities db = new FAISEntities();
        public async System.Threading.Tasks.Task<META_BO> GetMETAAsync(string dbName)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
               .FirstOrDefaultAsync();

            return meta;
        }
        public META_BO GetMETA(string dbName)
        {
            META_BO meta = db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
               .FirstOrDefault();

            return meta;
        }
        public async System.Threading.Tasks.Task<META_BO> GetMETAAsync(long id)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.META_BO_ID == id)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
               .FirstOrDefaultAsync();

            return meta;
        }
        public META_BO GetMETA(long id)
        {
            META_BO meta = db.META_BO
               .Where(x => x.META_BO_ID == id)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
               .FirstOrDefault();

            return meta;
        }


        public async System.Threading.Tasks.Task<List<META_BO>> GetMETAListAsync()
        {
            return await db.META_BO
               .Where(x => x.TYPE == "form" && x.STATUS != "-1")
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW" ))
               .ToListAsync();
        }

        // FOR COMMIT
        public async System.Threading.Tasks.Task<META_BO> GetMETAForCommitAsync(string dbName)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => !f.FORM_TYPE.Contains("subform-")))
               .FirstOrDefaultAsync();

            return meta;
        }
        public async System.Threading.Tasks.Task<META_BO> GetMETAForCommitAsync(long id)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.META_BO_ID == id)
               .IncludeFilter(x => x.META_FIELD.Where(f => !f.FORM_TYPE.Contains("subform-")))
               .FirstOrDefaultAsync();

            return meta;
        }


    }
}