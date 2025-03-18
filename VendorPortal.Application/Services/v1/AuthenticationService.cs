using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.Application.Services.v1
{
    public class AuthenticationService : IAuthenticationService
    {
        // Add your methods and properties here
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly AppConfigHelper _appConfigHelper;
        public AuthenticationService(IAuthenticationRepository authenticationRepository, AppConfigHelper appConfigHelper)
        {
            _authenticationRepository = authenticationRepository;
            _appConfigHelper = appConfigHelper;
        }

        /// <summary>
        /// ใช้สำหรับ Authenticate ด้วย Token และ Channel
        /// </summary>
        /// <param name="token">Static Token ที่ต้องส่งให้ Vendor และ Vendor ส่งกลับมาให้เราเมื่อต้องการใช้งาน</param>
        /// <param name="channel">Channel ที่ เป็นชื่อของ Vendor </param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> AuthenticateToken(string token, string channel)
        {
            AuthenticationResponse response = new AuthenticationResponse();
            try
            {
                var sp_result = await _authenticationRepository.SP_AUTHENTICATE_CHANNEL(token, channel);
                if (sp_result.IsAuthenticated)
                {

                    var duration = _appConfigHelper.GetConfiguration("Token:duration_minute") ?? "60";
                    // generate token and expire
                    var ExpireDate = DateTime.Now.AddMinutes(Convert.ToDouble(duration));

                    var enctptedToken = Utility.EncyptionToken(token, ExpireDate);
                    response = new AuthenticationResponse()
                    {
                        status = new Models.Common.Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        },
                        Data = new AuthenticationData
                        {
                            Token = enctptedToken,
                            Expiration = ExpireDate
                        }
                    };
                }
                else
                {
                    var message = sp_result.Message ?? ResponseCode.Unauthorized.Description();
                    response = new AuthenticationResponse()
                    {
                        status = new Models.Common.Status()
                        {
                            code = ResponseCode.Unauthorized.Text(),
                            message = message
                        },
                        Data = null
                    };
                    return response;
                }
            }
            catch (ApplicationException ex)
            {
                response = new AuthenticationResponse()
                {
                    status = new Models.Common.Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "AuthenticateToken");
            }
            catch (System.Exception ex)
            {
                response = new AuthenticationResponse()
                {
                    status = new Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "AuthenticateToken");
            }
            return response;
        }
    }
}