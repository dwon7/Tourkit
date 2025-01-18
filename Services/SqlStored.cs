using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace Tourkit.Services
{
    public class SqlStored
    {
        public static string GetProducts(string connectionString, int pageIndex, int pageSize, out int totalCount, string keySearch, int category)
        {
            try
            {
                totalCount = 0;
                string jsonValue = string.Empty;


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (SqlCommand cmd = new SqlCommand("GetProducts", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
                        cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                        cmd.Parameters.Add("@keySearch", SqlDbType.NVarChar, 250).Value = string.IsNullOrEmpty(keySearch) ? DBNull.Value : keySearch;
                        cmd.Parameters.Add("@category", SqlDbType.Int).Value = category;

                        cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@jsonValue", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        totalCount = Convert.ToInt32(cmd.Parameters["@totalCount"].Value);
                        jsonValue = cmd.Parameters["@jsonValue"].Value.ToString();

                        conn.Close();
                    }
                }

                return jsonValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing stored procedure: " + ex.Message);
            }
        }


        public static string GetCategories(string connectionString, int pageIndex, int pageSize, out int totalCount, string keySearch)
        {
            try
            {
                totalCount = 0;
                string jsonValue = string.Empty;


                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    using (SqlCommand cmd = new SqlCommand("GetCategories", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pageIndex", SqlDbType.Int).Value = pageIndex;
                        cmd.Parameters.Add("@pageSize", SqlDbType.Int).Value = pageSize;
                        cmd.Parameters.Add("@keySearch", SqlDbType.NVarChar, 250).Value = string.IsNullOrEmpty(keySearch) ? DBNull.Value : keySearch;

                        cmd.Parameters.Add("@totalCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@jsonValue", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        totalCount = Convert.ToInt32(cmd.Parameters["@totalCount"].Value);
                        jsonValue = cmd.Parameters["@jsonValue"].Value.ToString();

                        conn.Close();
                    }
                }

                return jsonValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing stored procedure: " + ex.Message);
            }
        }
    }
}
