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

        public Boolean Insert(string sqlQuery, Dictionary<string, object> parametres = null)
        {
            try
            {
                sqlQuery += "; SELECT SCOPE_IDENTITY()";
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

        public Boolean Update(string sqlQuery, Dictionary<string, object> parametres = null)
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
    }
}
