namespace VendorPortal.API.Middleware
{
    using Microsoft.AspNetCore.Builder;
    // Your code goes here
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using System;
    using System.Data.Common;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Threading.Tasks;
    using VendorPortal.Application.Helpers;
    using VendorPortal.Application.Models.Common;
    using VendorPortal.Application.Models.v1.Response;
    using static VendorPortal.Application.Models.Common.AppEnum;

    public class TokenVerificationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// Middleware เช็ค Token ที่ทำการเรียกเข้ามาที่ API ว่าหมดอายุหรือยัง
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            // Excelude paths that do not require token verification
            // You can add more paths to this list as needed
            string[] pathArrayList ={
                "api/v1/wolf-approve/auth",
                "alive",
                "/",
            };
            if (pathArrayList.All(x => !path.Contains(x)))
            {
                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    var token = context.Request.Headers["Authorization"].ToString().Split(" ").Last();
                    var handler = new JwtSecurityTokenHandler();

                    if (!string.IsNullOrEmpty(token))
                    {
                        var dycToken = VendorPortal.Application.Helpers.Utility.DecyptionToken(token);
                        var strExpireDate = dycToken.Split("|Expire:").Last();
                        DateTime ExipreDate;
                        if (!DateTime.TryParse(strExpireDate, out ExipreDate))
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Invalid token expiration date");
                            return;
                        }
                        else
                        {
                            if (ExipreDate < DateTime.Now)
                            {
                                context.Response.StatusCode = 401;
                                BaseResponse response = new BaseResponse()
                                {
                                    status = new Status()
                                    {
                                        code = ResponseCode.Unauthorized.Text(),
                                        message = ResponseCode.Unauthorized.Description()
                                    }
                                };
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                                return;
                            }
                            else
                            {
                                await _next(context);
                            }
                        }

                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid token");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authorization header missing");
                    return;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}