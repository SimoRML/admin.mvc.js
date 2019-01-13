using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FAIS.Models.VForm
{
    public class SelectDataModel
    {
        public string Value { get; set; }
        public string Display { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
    public class SelectSourceModel
    {
        public string Source { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public string Filter { get; set; }

        private string sqlQuery
        {
            get
            {
                // TODO : prevent SQL injection
                if (this.Filter == null) this.Filter = "";
                return String.Format("Select convert(varchar,{0}) as value, {1} as display,* from {2} {3}", this.Value, this.Display, this.Source, this.Filter.Trim() == "" ? "" : " where " + this.Filter);
            }
        }

        //public async System.Threading.Tasks.Task<List<SelectDataModel>> GetAsync(FAISEntities db)
        //{
        //    return await db.Database.SqlQuery<SelectDataModel>(this.sqlQuery).ToListAsync();
        //}

        public List<SelectDataModel> Get()
        {
            SGBD s = new SGBD();
            List<SelectDataModel> sources = new List<SelectDataModel>();


            DataTable brute = s.Cmd(this.sqlQuery);


            foreach (DataRow row in brute.Rows)
            {

                sources.Add(new SelectDataModel()
                {
                    Value = row["value"].ToString(),
                    Display = row["display"].ToString(),
                    Attributes = new Dictionary<string, object>()
                });

                var attributes = sources.Last().Attributes;
                foreach (DataColumn coll in brute.Columns)
                {
                    attributes.Add(coll.ColumnName, row[coll.ColumnName].ToString());

                }

            }

            return sources;
        }
    }

    public class FilterModel
    {
        public int MetaBoID { get; set; }
        public List<FilterItemModel> Items { get; set; }
        public Dictionary<string, object> mapping
        {
            get
            {
                Dictionary<string, object> mapp = new Dictionary<string, object>();
                foreach (var item in Items)
                {
                    mapp.Add(item.Field, item.Value);
                }
                return mapp;
            }
        }

        public string Format()
        {
            string where = "";
            foreach (var item in Items)
            {
                where += item.Format();
            }

            return where;
        }
    }
    public class FilterItemModel
    {
        public string Logic { get; set; }
        public string Field { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }

        public string Format()
        {
            return string.Format(" {0} {1} {2} @{3} ", Logic, Field, Condition, Field);
        }

    }

    public class CrudModel : BORepository
    {
        public int MetaBoID { get; set; }
        public int BO_ID { get; set; }
        // public string MetaBoNAME { get; set; }
        public Dictionary<string, object> Items { get; set; }

        public string FormatInsert()
        {
            string fields = "", values = "";
            foreach (var item in Items)
            {
                fields += ",[" + item.Key + "]";
                values += ",@" + item.Key;
            }
            if (fields != "") fields = fields.Remove(0, 1);
            if (values != "") values = values.Remove(0, 1);
            return string.Format("insert into {0} ({1}) values ({2}) ", MetaBO.BO_DB_NAME, fields, values);
        }

        public string FormatUpdate()
        {
            string Field_Values = "";
            foreach (var item in Items)
            {
                Field_Values += ",[" + item.Key + "]=@" + item.Key;
            }
            if (Field_Values != "") Field_Values = Field_Values.Remove(0, 1);

            return string.Format("Update {0} set {1}  where BO_ID=@BO_ID", MetaBO.BO_DB_NAME, Field_Values);

        }

        public string FormatDelete()
        {
            string Field_Values = "";

            return string.Format("delete from {0} where BO_ID = {1}  where BO_ID=@BO_ID", MetaBO.BO_DB_NAME, Field_Values);

        }

        public bool Insert()
        {
            // TODO : call repo validator
            return ExecInsert(FormatInsert(), Items);
        }

        public string Update()
        {
            return ExecUpdate(FormatUpdate(), Items);
        }



    }
}