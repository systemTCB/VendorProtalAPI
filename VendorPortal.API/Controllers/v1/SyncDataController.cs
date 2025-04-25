using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Services.SyncExternalData;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.API.Controllers.v1
{
    [ApiController]
    public class SyncDataController : ControllerBase
    {
        private readonly IKubbossService _kubbossService;

        public SyncDataController(IKubbossService kubbossService)
        {
            _kubbossService = kubbossService;
        }

        [HttpGet("sync-data/kub-boss/v1/vendor-portal/{dateSync?}")]
        public async Task<IActionResult> SyncData(DateTime? dateSync = null)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                if(dateSync == null)
                {
                    dateSync = DateTime.Now;
                }
                response = await _kubbossService.SyncVendorFromKubboss(dateSync.Value);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SyncData");
                response = new BaseResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                return Ok(response);
            }
        }

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return Ok("Service is running.");
        }
    }
}