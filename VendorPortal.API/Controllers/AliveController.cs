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
namespace VendorPortal.API.Controllers
{
    [ApiController]
    public class AliveController : ControllerBase
    {
        [HttpGet]
        [Description("Peetisook.S | Check alive api")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับตรวจสอบสถานะของ Service")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AliveResponse))]
        [Route("alive")]
        public ActionResult Get()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            AliveResponse _result = new AliveResponse();
            _result.alive = true;
            _result.version = "1.0.0.0";

            return Ok(_result);
        }
    }
}