using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Services.SyncExternalData;

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

        [HttpGet("sync-data/kub-boss/v1/vendor-portal")]
        public async Task<IActionResult> SyncData()
        {
            try
            {
                var response = await _kubbossService.SyncVendorFromKubboss(DateTime.UtcNow);
                return Ok("Sync data completed !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("health-check")]
        public IActionResult HealthCheck()
        {
            return Ok("Service is running.");
        }
    }
}