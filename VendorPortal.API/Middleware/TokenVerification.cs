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
    using VendorPortal.Application.Models.v1.Response;

    public class TokenVerificationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.Value.Contains("api/v1/wolf-approve/auth"))
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
                                await context.Response.WriteAsync("Token has expired");
                                return;
                            }
                            else
                            {
                                await _next(context);
                            }
                        }

                    }
                    // if (handler.CanReadToken(token))
                    // {
                    //     var jwtToken = handler.ReadJwtToken(token);
                    //     var bearerToken = new JwtSecurityToken(token);
                    //     // Example: Check if the token is expired
                    //     if (bearerToken.ValidTo < DateTime.UtcNow)
                    //     {
                    //         context.Response.StatusCode = 401;
                    //         await context.Response.WriteAsync("Token has expired");
                    //         return;
                    //     }

                    //     // Example: Check for a specific claim
                    //     var userIdClaim = bearerToken.Claims.FirstOrDefault(c => c.Type == "userId");
                    //     if (userIdClaim == null)
                    //     {
                    //         context.Response.StatusCode = 401;
                    //         await context.Response.WriteAsync("Token does not contain required claims");
                    //         return;
                    //     }
                    //     // Add your token validation logic here
                    //     // For example, you can check the token's claims, expiration, etc.
                    // }
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

            // await _next(context);
        }
    }
}