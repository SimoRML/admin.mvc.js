using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FAIS.Models
{
    public class SGBD
    {
        public SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataSet ds;

        // CMD
        public DataTable Cmd(string sqlQuery, Dictionary<string, object> parametres = null)
        {
            try
            {
                using (cmd = new SqlCommand(sqlQuery, cn))
                {
                    if (parametres != null)
                        foreach (var item in parametres)
                            cmd.Parameters.AddWithValue(item.Key, item.Value);

                    using (da = new SqlDataAdapter(cmd))
                    {
                        using (ds = new DataSet())
                        {
                            da.Fill(ds, "tbl");
                            return ds.Tables["tbl"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO LOG EXCEPTION
                throw ex;
            }
        }

        public string Insert(string sqlQuery, Dictionary<string, object> parametres = null)
        {
            try
            {
                sqlQuery += "; SELECT SCOPE_IDENTITY()";
                using (cmd = new SqlCommand(sqlQuery, cn))
                {
                    if (parametres != null)
                    {
                        foreach (var item in parametres)
                        {
                            var value = item.Value;
                            if (item.Value.GetType().ToString() == "Newtonsoft.Json.Linq.JArray") value = item.Value.ToString().Replace("\r", "").Replace("\n", "");
                            cmd.Parameters.AddWithValue(Helper.cleanDBName(item.Key), item.Value == null ? "" : value);
                        }
                    }

                    using (da = new SqlDataAdapter(cmd))
                    {
                        using (ds = new DataSet())
                        {
                            da.Fill(ds, "tbl");

                            return "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO LOG EXCEPTION
                var message = ex.Message;
                return message;
            }
        }

        public string Update(string sqlQuery, Dictionary<string, object> parametres = null)
        {
            try
            {

                using (cmd = new SqlCommand(sqlQuery, cn))
                {
                    if (parametres != null)
                        foreach (var item in parametres)
                        {
                            var value = item.Value;
                            if (item.Value.GetType().ToString() == "Newtonsoft.Json.Linq.JArray") value = item.Value.ToString().Replace("\r", "").Replace("\n", "");
                            cmd.Parameters.AddWithValue(Helper.cleanDBName(item.Key), item.Value == null ? "" : value);
                        }
                    //cmd.Parameters.AddWithValue(item.Key, item.Value);

                    using (da = new SqlDataAdapter(cmd))
                    {
                        using (ds = new DataSet())
                        {
                            da.Fill(ds, "tbl");

                            return "true";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO LOG EXCEPTION
                return ex.Message;
            }
        }

        public Boolean Delete(string sqlQuery, Dictionary<string, object> parametres = null)
        {
            try
            {

                using (cmd = new SqlCommand(sqlQuery, cn))
                {
                    if (parametres != null)
                        foreach (var item in parametres)
                            cmd.Parameters.AddWithValue(item.Key, item.Value);

                    using (da = new SqlDataAdapter(cmd))
                    {
                        using (ds = new DataSet())
                        {
                            da.Fill(ds, "tbl");

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO LOG EXCEPTION
                return false;
            }
        }

        public string DESCRIBE(string table_name)
        {

            return "SELECT column_name AS[Field],DATA_TYPE  AS[Type]," +
                "IS_NULLABLE AS[Null]," +
                   " case when exists(select* FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE" +
                    "                                    WHERE  column_name = c.column_name and table_name = c.table_name" +
                     "                                   and CONSTRAINT_NAME like 'PK_%')" +
                    "then 'PRI'" +
                    " else '' end as [Key]," +
                    " COLUMN_DEFAULT as [Default] " +
                     ",case when COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 " +
                    "then 'auto_increment' else '' end as [Extra] " +

                "FROM INFORMATION_SCHEMA.Columns c " +
                "WHERE table_name = '" + table_name + "'";
        }
    }
}
