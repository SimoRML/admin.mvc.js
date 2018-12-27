using FAIS.Models.VForm;
using System;
using System.Collections.Generic;
using System.Data;
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
               .Where(x => /*x.TYPE == "form" && */x.STATUS != "-1")
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
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

        public static List<SelectDataModel> GetDataSources()
        {
            SGBD s = new SGBD();
            List<SelectDataModel> sources = new List<SelectDataModel>();

            DataTable brute = s.Cmd(Helper.GetSQL("SelectDataSources.sql"));

            var last = string.Empty;
            foreach (DataRow row in brute.Rows)
            {
                if (row["TABLE_NAME"].ToString() != last)
                {
                    last = row["TABLE_NAME"].ToString();
                    sources.Add(new SelectDataModel()
                    {
                        Value = last,
                        Display = last.Replace("VIEW_", "").Replace("_BO_", ""),
                        Attributes = new Dictionary<string, string>()
                    });
                }
                var attributes = sources.Last().Attributes;
                attributes.Add(row["Field"].ToString(), row["Key"].ToString());
            }

            return sources;
        }

        public META_BO Create(META_BO mETA_BO, string userName)
        {
            mETA_BO.BO_DB_NAME += "_BO_";
            mETA_BO.VERSION = 1;
            mETA_BO.CREATED_BY = userName;
            mETA_BO.UPDATED_BY = userName;
            mETA_BO.CREATED_DATE = DateTime.Now;
            mETA_BO.UPDATED_DATE = DateTime.Now;
            db.META_BO.Add(mETA_BO);

            // int lastVersion = db.VERSIONS.Where(x => x.META_BO_ID == mETA_BO.META_BO_ID).Max(x => x.NUM);
            db.VERSIONS.Add(new VERSIONS()
            {
                META_BO_ID = mETA_BO.META_BO_ID,
                NUM = 1,
                SQLQUERY = Helper.GetSQL("CreateTable.sql"),
                STATUS = "PENDING",
                CREATED_BY = userName,
                UPDATED_BY = userName,
            });

            db.SaveChanges();
            return mETA_BO;
        }

    }

    public class BoBulk
    {
        private FAISEntities db = new FAISEntities();
        public string BoName { get; set; }
        public List<FieldsBulk> Fields { get; set; }

        public void CreateMetaBo(string userName)
        {
            userName = "api_" + userName;
            MetaBoRepo repo = new MetaBoRepo();

            META_BO mbo = new META_BO
            {
                BO_NAME = BoName
            };

            mbo = repo.Create(mbo, userName);

            foreach (var f in Fields)
            {
                META_FIELD field = f.MetaField(mbo.META_BO_ID, userName);

                db.META_FIELD.Add(field);
            }
            db.SaveChanges();
        }
    }

    public class FieldsBulk
    {
        public string Nom { get; set; }
        public string Type { get; set; }

        public META_FIELD MetaField(long metaBoId, string userName)
        {
            return new META_FIELD()
            {
                META_FIELD_ID = 0,
                META_BO_ID = metaBoId,
                DB_NAME = Nom,
                CREATED_BY = userName,
                CREATED_DATE = DateTime.Now,
                UPDATED_BY = userName,
                UPDATED_DATE = DateTime.Now,
                DB_NULL = 1,
                DB_TYPE = "varchar(100)",
                FORM_DEFAULT = "",
                FORM_FORMAT = "",
                FORM_NAME = Nom,
                FORM_OPTIONAL = 1,
                FORM_SHOW = 1,
                FORM_SOURCE = "",
                FORM_TYPE = Type,
                GRID_FORMAT = "",
                GRID_NAME = Nom,
                GRID_SHOW = 1,
                IS_FILTER = 0,
                STATUS = "NEW"
            };
        }
    }
}