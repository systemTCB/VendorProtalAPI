using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;
using Logger = VendorPortal.Logging.Logger;

namespace VendorPortal.Infrastructure.Repositories.WolfApprove.v1
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        // Add your methods and properties here
        private readonly DbContext _dbContext;
        public AuthenticationRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CHECK_CONNECTION()
        {
            using var connection = _dbContext.CreateConnectionRead();
            bool isSuccess = false;
            try
            {
                connection.Open();
                await Task.Run(() =>
                {
                    isSuccess = connection.State == ConnectionState.Open ? true : false;
                });

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CHECK_CONNECTION");
            }
            finally
            {
                connection.Close();
            }
            return isSuccess;
        }

        public async Task<SP_AUTHENTICATE_CHANNEL> SP_AUTHENTICATE_CHANNEL(string token, string channel)
        {
            SP_AUTHENTICATE_CHANNEL response = new SP_AUTHENTICATE_CHANNEL();
            try
            {
                var store = "SP_AUTHENTICATE_CHANNEL";
                using var connection = _dbContext.CreateConnectionRead();
                connection.Open();
                var param = new DynamicParameters();
                param.Add("@Token", token);
                param.Add("@Channel", channel);
                var result = await connection.QueryFirstOrDefaultAsync<SP_AUTHENTICATE_CHANNEL>(store, param, commandType: System.Data.CommandType.StoredProcedure);
                response = result;
            }
            catch (SqlException ex)
            {

                response = new SP_AUTHENTICATE_CHANNEL()
                {
                    IsAuthenticated = false,
                    Message = ex.Message,
                    Token = string.Empty
                };
                Logger.LogError(ex, "AuthenticationRepository");
            }
            catch (System.Exception ex)
            {
                response = new SP_AUTHENTICATE_CHANNEL()
                {
                    IsAuthenticated = false,
                    Message = ex.Message,
                    Token = string.Empty
                };
                Logger.LogError(ex, "AuthenticationRepository");
            }
            return response;
        }
    }
}