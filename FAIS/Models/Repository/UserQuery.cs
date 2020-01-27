using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FAIS.Models.Repository
{
    public class UserQuery
    {
        public string sqlQuery { get; set; }
        public string grant { get; set; }

        public bool validate()
        {
            if (this.sqlQuery.ToLower().Contains("--")) return false;
            if (this.sqlQuery.ToLower().Contains("drop ")) return false;
            if (this.sqlQuery.ToLower().Contains("update ")) return false;            
            if (this.sqlQuery.ToLower().Contains("revoke ")) return false;
            if (this.sqlQuery.ToLower().Contains("grant ")) return false;
            if (this.sqlQuery.ToLower().Contains("create ")) return false;
            if (this.sqlQuery.ToLower().Contains("alter ")) return false;
            if (this.sqlQuery.ToLower().Contains("into ")) return false;
            if (this.sqlQuery.ToLower().Contains("insert ")) return false;
            if (this.sqlQuery.ToLower().Contains("sp_")) return false;
            if (this.sqlQuery.ToLower().Contains("delete ")) return false;

            if (this.sqlQuery.ToLower().Contains("exec "))
            {
                if (this.grant == string.Empty) return false;
                else if(this.grant != "gha 7na !") return false;
            }

            return true;
        }

        public DataTable Execute()
        {
            return new SGBD().Cmd(this.sqlQuery);
        }
    }
}