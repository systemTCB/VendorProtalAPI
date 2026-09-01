using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.v1;
// using DisburseData.Application.Interfaces;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;
namespace VendorPortal.API.Controllers.v1
{
    [ApiController]
    public class VendorPortalController : ControllerBase
    {
        private readonly IWolfApproveService _wolfApproveService;
        public VendorPortalController(IWolfApproveService wolfApproveService)
        {
            _wolfApproveService = wolfApproveService;
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/JobDocument")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Job Document V1" }, Summary = "Job Document", Description = "Job Document By ID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDocumentResponse))]
        public async Task<IActionResult> GetJobDocumentByID(JobDocumentRequest request)
        {
            JobDocumentResponse responseJobDocumentByID = new();
            try
            {
                responseJobDocumentByID = await _wolfApproveService.GetJobDocuments(request);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Get JobDocument By ID ERROR");
                responseJobDocumentByID = new JobDocumentResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return Ok(responseJobDocumentByID);
        }
    }
}