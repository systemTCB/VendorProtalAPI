using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VendorPortal.Logging;

namespace VendorPortal.Infrastructure.Extensions
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("WolfApprove");
        }

        public IDbConnection CreateConnectionRead() => new SqlConnection(_configuration.GetConnectionString("WolfApprove"));
        public IDbConnection CreateConnectionWrite() => new SqlConnection(_configuration.GetConnectionString("WolfApprove"));

        public async Task<bool> ExecuteStoreNonQueryAsync(string store, SqlParameter[] sqlParameter = null)
        {
            SqlConnection sqlConnection = new SqlConnection(_connection);
            try
            {
                sqlConnection.Open();

                SqlCommand command = new SqlCommand(store, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null && sqlParameter.Length != 0)
                {
                    command.Parameters.AddRange(sqlParameter);
                }

                SqlDataReader dr = await command.ExecuteReaderAsync();
                if (dr.Read())
                {
                    var isSuccess = (bool)dr["result"];
                    var msg = (string)dr["Message"];
                    return isSuccess;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Logger.LogError(ex, "ExecuteStoreNonQueryAsync");
                return false;
            }
            finally { sqlConnection.Close(); }
        }
        public async Task<List<T>> ExcuteStoreQueryListAsync<T>(string store, SqlParameter[] sqlParameter = null)
        {
            SqlConnection sqlConnection = new SqlConnection(_connection);
            
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(store, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null && sqlParameter.Length != 0)
                {
                    command.Parameters.AddRange(sqlParameter);
                }
                SqlDataReader dr = await command.ExecuteReaderAsync();
                List<T> list = new List<T>();
                T obj = default;
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = dr[prop.Name] == DBNull.Value ? null : Convert.ChangeType(dr[prop.Name], targetType);
                            prop.SetValue(obj, safeValue, null);
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Logger.LogError(ex, "ExcuteStoreQueryListAsync");
                return new List<T>();
            }
            finally { sqlConnection.Close(); }
        }
        public async Task<T> ExcuteStoreQuerySingleAsync<T>(string store, SqlParameter[] sqlParameter = null)
        {
            SqlConnection sqlConnection = new SqlConnection(_connection);
            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(store, sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null && sqlParameter.Length != 0)
                {
                    command.Parameters.AddRange(sqlParameter);
                }

                SqlDataReader dr = await command.ExecuteReaderAsync();
                T obj = default;
                while (dr.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                Logger.LogError(ex, "ExcuteStoreQuerySingleAsync");
                return default;
            }
            finally { sqlConnection.Close(); }
        }
    }
}