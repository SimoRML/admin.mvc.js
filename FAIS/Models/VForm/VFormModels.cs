using FAIS.Models.Repository;
using Newtonsoft.Json;
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
        private string ignoredTables = "[meta_bo][meta_field]";
        private string sqlQuery
        {
            get
            {
                // TODO : prevent SQL injection
                if (this.Filter == null) this.Filter = "";
                return String.Format(
                    "Select convert(varchar,{0}) as value, {1} as display,* from {2} where 1=1 " +
                    ( this.ignoredTables.Contains("["+ this.Source.ToLower().Trim() +"]") ? "" :
@" AND BO_ID not in (
	select BO.BO_ID
	from BO inner join META_BO on  BO.BO_TYPE = META_BO.META_BO_ID AND META_BO.BO_DB_NAME = '{2}'
	where BO.STATUS = 'deleted'
) "
                    )
 +
" {3} "
                    , this.Value, this.Display, this.Source, this.Filter.Trim() == "" ? "" : " AND " + this.Filter);
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
                    mapp.Add(key: item.Field.Replace(".", ""), value: item.Value);
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
            return string.Format(" {0} {1} {2} @{3} ", Logic, Field, Condition, Field.Replace(".", ""));
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
                values += ",@" + Helper.cleanDBName(item.Key);
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
                Field_Values += ",[" + item.Key + "]=@" + Helper.cleanDBName(item.Key);
            }
            if (Field_Values != "") Field_Values = Field_Values.Remove(0, 1);

            return string.Format("Update {0} set {1}  where BO_ID=@BO_ID", MetaBO.BO_DB_NAME, Field_Values);

        }

        public string FormatDelete()
        {
            string Field_Values = "";

            return string.Format("delete from {0} where BO_ID = {1}  where BO_ID=@BO_ID", MetaBO.BO_DB_NAME, Field_Values);

        }

        public string Insert()
        {
            // TODO : call repo validator
            SetPlusValues();
            return ExecInsert(FormatInsert(), Items);
        }
        public void SetPlusValues()
        {
            try
            {
                //DataTable metaFields = new SGBD().Cmd("select *, JSON_VALUE(JSON_DATA,'$.DEFAULT') as 'DEFAULT' from META_FIELD where META_BO_ID = " + this.MetaBoID
                //                    + " AND JSON_VALUE(JSON_DATA,'$.DEFAULT') like '%[+%]%'");

                DataTable metaFields = new SGBD().Cmd("select * from META_FIELD where META_BO_ID = " + this.MetaBoID
                                    + " AND   JSON_DATA like '%\"DEFAULT\":%[+%]%' ");


                foreach (DataRow mf in metaFields.Rows)
                {
                    if (this.Items.ContainsKey(mf["DB_NAME"].ToString()))
                    {
                        var jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(mf["JSON_DATA"].ToString());

                        var df = new MetaFieldRepo().GetDefaultValue(jsonData["DEFAULT"].ToString(), this.MetaBO.BO_DB_NAME, 1);
                        if (df.type != "error")
                            this.Items[mf["DB_NAME"].ToString()] = df.value;
                    }
                }

            }
            catch (Exception)
            {
            }
        }
        public string Update()
        {
            return ExecUpdate(FormatUpdate(), Items);
        }



    }
}