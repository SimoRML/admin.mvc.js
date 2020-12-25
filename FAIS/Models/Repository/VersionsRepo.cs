using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAIS.Models.Repository
{
    public class VersionsRepo
    {
        private FAISEntities db = new FAISEntities();
        public VERSIONS Create(long metaBoId, string userName)
        {
            return new VERSIONS()
            {
                META_BO_ID = metaBoId,
                NUM = 1,
                SQLQUERY = Helper.GetSQL("CreateTable.sql"),
                STATUS = "PENDING",
                CREATED_BY = userName,
                UPDATED_BY = userName,
            };
        }

        public async System.Threading.Tasks.Task<VERSIONS> CreateAndSaveAsync(long metaBoId, string userName)
        {
            VERSIONS v = Create(metaBoId, userName);
            db.VERSIONS.Add(v);
            await db.SaveChangesAsync();
            return v;
        }

        public async System.Threading.Tasks.Task CommitAsync(long id, string userName)
        {
            VERSIONS vERSIONS = await db.VERSIONS.FindAsync(id);
            if (vERSIONS == null)
            {
                throw new Exception("VERSION NOT FOUND");
            }

            META_BO mETA_BO = await new MetaBoRepo().GetMETAForCommitAsync(vERSIONS.META_BO_ID.Value); // await db.META_BO.FindAsync(vERSIONS.META_BO_ID);
            if (mETA_BO.META_FIELD.Count <= 0)
            {
                throw new Exception("No meta field found !");
            }

            var fields = "";
            foreach (var f in mETA_BO.META_FIELD)
            {
                fields += string.Format(" [{0}] {1} {2} , "
                    , f.DB_NAME
                    , f.DB_TYPE == "DateTime" ? " varchar(20) " : f.DB_TYPE
                    , f.DB_NULL == 0 ? " NOT NULL " : " NULL "
                    );
            }

            var sqlQuery = Helper.GetSQL("CreateTable.sql");
            sqlQuery = string.Format(sqlQuery,
            vERSIONS.VERSIONS_ID.ToString()
            , mETA_BO.BO_DB_NAME
            , fields
            , userName
            , mETA_BO.META_BO_ID.ToString()
            , vERSIONS.NUM
            , mETA_BO.BO_DB_NAME);


            sqlQuery = sqlQuery.Replace("[SQLQUERY]", sqlQuery.Replace("'", "''"));

            var s = new SGBD();
            s.Cmd(sqlQuery);
            // s.Cmd(new VersionsModels().GenerateView(mETA_BO.BO_DB_NAME));
        }
    }
}