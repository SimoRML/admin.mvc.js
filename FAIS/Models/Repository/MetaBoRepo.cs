using FAIS.Models.VForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Z.EntityFramework.Plus;

namespace FAIS.Models.Repository
{
    public class MetaBoRepo
    {
        private FAISEntities db = new FAISEntities();
        public async Task<META_BO> GetMETAAsync(string dbName)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW" && !f.STATUS.Contains("[deleted]")).OrderBy(y => y.META_FIELD_ID))
               .FirstOrDefaultAsync();

            return meta;
        }
        public META_BO GetMETA(string dbName)
        {
            META_BO meta = db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW" && !f.STATUS.Contains("[deleted]")))
               .FirstOrDefault();

            return meta;
        }
        public async Task<META_BO> GetMETAAsync(long id)
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


        public async Task<List<META_BO>> GetMETAListAsync()
        {
            return await db.META_BO
               .Where(x => /*x.TYPE == "form" && */x.STATUS != "-1")
               .IncludeFilter(x => x.META_FIELD.Where(f => f.STATUS != "NEW"))
               .ToListAsync();
        }

        public async Task<List<META_BO>> GetMETAListOnlyAsync()
        {
            return await db.Database.SqlQuery<META_BO>("select * from META_BO where STATUS<>'-1' or STATUS is null").ToListAsync();
        }

        // FOR COMMIT
        public async Task<META_BO> GetMETAForCommitAsync(string dbName)
        {
            META_BO meta = await db.META_BO
               .Where(x => x.BO_DB_NAME == dbName)
               .IncludeFilter(x => x.META_FIELD.Where(f => !f.FORM_TYPE.Contains("subform-")))
               .FirstOrDefaultAsync();

            return meta;
        }
        public async Task<META_BO> GetMETAForCommitAsync(long id)
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
                        Attributes = new Dictionary<string, object>()
                    });
                }
                var attributes = sources.Last().Attributes;
                attributes.Add(row["Field"].ToString(), new { primary = row["Key"].ToString(), FORM_SOURCE = row["FORM_SOURCE"].ToString() });
            }

            return sources;
        }

        public async Task<META_BO> CreateAndSaveAsync(META_BO mETA_BO, string userName)
        {
            mETA_BO = Create(mETA_BO, userName);
            db.META_BO.Add(mETA_BO);

            VERSIONS version = new VersionsRepo().Create(mETA_BO.META_BO_ID, userName);
            db.VERSIONS.Add(version);

            await db.SaveChangesAsync();
            return mETA_BO;
        }
        public META_BO Create(META_BO mETA_BO, string userName)
        {
            mETA_BO.BO_DB_NAME = Helper.cleanDBName(mETA_BO.BO_DB_NAME == null ? mETA_BO.BO_NAME : mETA_BO.BO_DB_NAME) + "_BO_";
            mETA_BO.VERSION = 1;
            mETA_BO.CREATED_BY = userName;
            mETA_BO.UPDATED_BY = userName;
            mETA_BO.CREATED_DATE = DateTime.Now;
            mETA_BO.UPDATED_DATE = DateTime.Now;

            return mETA_BO;
        }

        public async Task<META_BO_EX> FindMetaBoExAsync(long id)
        {
            META_BO_EX res = await db.Database.SqlQuery<META_BO_EX>("select JSON_VALUE(JSON_DATA, '$.TITLE') TITLE, JSON_VALUE(JSON_DATA, '$.GROUPE') GROUPE, * from META_BO WHERE META_BO_ID = " + id).FirstAsync();
            return res;
        }

        public async Task<List<META_BO_EX>> GetMetaBoExAsync(string where)
        {
            return await db.Database.SqlQuery<META_BO_EX>("select JSON_VALUE(JSON_DATA, '$.TITLE') TITLE, JSON_VALUE(JSON_DATA, '$.GROUPE') GROUPE, * from META_BO  " + where + "  AND JSON_VALUE(JSON_DATA, '$.MENU') = 1 ").ToListAsync();
        }

        public async Task<List<META_BO_EX>> GetPagesAsync(string where)
        {
            return await db.Database.SqlQuery<META_BO_EX>("select BO_ID as META_BO_ID, 'PAGE' as type, JSON_VALUE(LAYOUT, '$.page.title') TITLE, JSON_VALUE(LAYOUT, '$.page.groupe') GROUPE from [PAGE] WHERE JSON_VALUE(LAYOUT, '$.page.title') <> '' AND JSON_VALUE(LAYOUT, '$.page.groupe') <> '' AND STATUS = 'public' " + where).ToListAsync();
        }
        //public async Task<META_BO_EX> FindMetaBoExAsync(long id)
        //{
        //    META_BO_EX res = await db.Database.SqlQuery<META_BO_EX>("select * from META_BO WHERE META_BO_ID = " + id).FirstAsync();
        //    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(res.JSON_DATA);
        //    res.TITLE = values["TITLE"];
        //    res.GROUPE = values["GROUPE"];
        //    return res;
        //}

        //public async Task<List<META_BO_EX>> GetMetaBoExAsync(string where)
        //{
        //    List<META_BO_EX> liste = await db.Database.SqlQuery<META_BO_EX>("select * from META_BO " + where).ToListAsync();
        //    foreach (var res in liste)
        //    {
        //        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(res.JSON_DATA);
        //        res.TITLE = values["TITLE"];
        //        res.GROUPE = values["GROUPE"];
        //    }

        //    return liste;
        //}
    }

    public class BoBulk
    {
        private FAISEntities db = new FAISEntities();
        public string BoName { get; set; }
        public List<FieldsBulk> Fields { get; set; }

        public void CreateMetaBo(string userName)
        {
            userName = "api_" + userName;
            MetaBoRepo borepo = new MetaBoRepo();
            VersionsRepo vrepo = new VersionsRepo();

            META_BO mbo = new META_BO
            {
                BO_NAME = BoName,
                BO_DB_NAME = BoName,
                TYPE = "form"
            };

            mbo = borepo.Create(mbo, userName);
            db.META_BO.Add(mbo);

            VERSIONS version = vrepo.Create(mbo.META_BO_ID, userName);
            db.VERSIONS.Add(version);

            // db.SaveChanges();

            foreach (var f in Fields)
            {
                db.META_FIELD.Add(new MetaFieldRepo().Create(f.MetaField(mbo.META_BO_ID, userName), userName));
            }
            db.SaveChanges();

            Task.Run(async () => await vrepo.CommitAsync(version.VERSIONS_ID, userName));
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

    public class META_BO_EX : META_BO
    {
        public string TITLE { get; set; }
        public string GROUPE { get; set; }
    }
}