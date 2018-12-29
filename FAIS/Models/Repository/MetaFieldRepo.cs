using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAIS.Models.Repository
{
    public class MetaFieldRepo
    {
        private FAISEntities db = new FAISEntities();
        public META_FIELD Create(META_FIELD mETA_FIELD, string userName)
        {
            mETA_FIELD.DB_NAME = Helper.cleanDBName(mETA_FIELD.DB_NAME);
            mETA_FIELD.CREATED_BY = userName;
            mETA_FIELD.UPDATED_BY = userName;
            mETA_FIELD.STATUS = "NEW";

            return mETA_FIELD;
        }
        public async System.Threading.Tasks.Task<META_FIELD> CreateAndSaveAsync(META_FIELD mETA_FIELD, string userName)
        {
            mETA_FIELD = Create(mETA_FIELD, userName);
            db.META_FIELD.Add(mETA_FIELD);
            await db.SaveChangesAsync();
            return mETA_FIELD;
        }

    }
}