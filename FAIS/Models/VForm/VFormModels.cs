using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAIS.Models.VForm
{
    public class SelectDataModel
    {
        public string Value { get; set; }
        public string Display { get; set; }
    }
    public class SelectSourceModel
    {
        public string Source { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public string Filter { get; set; }
        private string sqlQuery {
            get
            {
                // TODO : prevent SQL injection
                return String.Format("Select convert(varchar,{0}) as value, {1} as display from {2} {3}", this.Value, this.Display, this.Source, this.Filter.Trim() == "" ? "" : " where " + this.Filter);
            }
        }

        public async System.Threading.Tasks.Task<List<SelectDataModel>> GetAsync(FAISEntities db)
        {
            return await db.Database.SqlQuery<SelectDataModel>(this.sqlQuery).ToListAsync();
        }
    }
}