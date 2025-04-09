using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendorPortal.Application.Services.SyncExternalData;

namespace VendorPortal.API.Controllers.v1
{
    public class SyncDataController : ControllerBase
    {
        private readonly ILogger<SyncDataController> _logger;
        private readonly KubbossService _kubbossService;

        public SyncDataController(ILogger<SyncDataController> logger, KubbossService kubbossService)
        {
            _logger = logger;
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
                _logger.LogError(ex, "Error occurred while syncing data.");
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