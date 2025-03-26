using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
// using DisburseData.Application.Interfaces;
using VendorPortal.Application.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using VendorPortal.Application.Interfaces.v1;
using Microsoft.AspNetCore.Authorization;

namespace VendorPortal.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AliveController : ControllerBase
    {
        private readonly IAuthenticationService _service;
        public AliveController(IAuthenticationService service)
        {
            _service = service;
        }


        [HttpGet]
        [Description("Peetisook.S | Check alive api")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับตรวจสอบสถานะของ Service")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AliveResponse))]
        [Route("alive")]
        public async Task<ActionResult> Get()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            AliveResponse _result = new AliveResponse();
            _result.alive = true;
            _result.version = "1.0.0.1";
            _result.connection = await _service.CheckConnection() ? "Already to Connect" : "Connection Failed !!";
            return Ok(_result);
        }
    }
}