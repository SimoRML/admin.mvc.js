﻿using System.Data;

namespace FAIS.Models
{
    public class VersionsModels
    {
        //TODO Function (met)

        public string GenerateView(string tableName)
        {
            int nbrFields = 0;
            bool first = true;
            SGBD s = new SGBD();
            string select_ = "";
            var dt = s.Cmd("SELECT * FROM sys.tables WHERE sys.tables.name  like '" + tableName.Replace("'", "''") + "%' order by create_date desc ");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    var fields = s.Cmd(s.DESCRIBE(item["name"].ToString()) + " and column_name not in ('CREATED_BY','CREATED_DATE','UPDATED_BY','UPDATED_DATE','STATUS')");
                    if (first)
                    {
                        select_ = "select * from " + item["name"] + " ";
                        nbrFields = fields.Rows.Count;
                        first = false;
                    }
                    else
                    {
                        select_ += "union select * ,";
                        if (nbrFields != fields.Rows.Count)
                        {
                            for (int i = 0; i < (nbrFields - fields.Rows.Count); i++)
                            {
                                select_ += "null,";
                            }
                            select_ = select_.Remove(select_.Length - 1) + " from " + item["name"] + " ";
                        }
                    }
                }

            }
            return string.Format(Helper.GetSQL("CreateView.sql"), tableName, select_);
        }
    }
}