using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.API.Controllers.v1
{
    // Your code goes here
    [ApiController]
    [AllowAnonymous]
    public class  AuthenticationControlller : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationControlller(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/auth")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "Authentication" }, Summary = "", Description = "ใช้สำหรับขอ Token ในการเข้าใช้งาน API ต่างๆ")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        public async Task<IActionResult> Authentication([FromBody] AuthenticationRequest request)
        {
            var response = new AuthenticationResponse();
            try
            {
                response = await _authenticationService.AuthenticateToken(request.token , request.channel);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex , "Authentication");
                response = new AuthenticationResponse()
                {
                    Status = new Application.Models.Common.Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };

            }
            return Ok(response);
        }

    }
}