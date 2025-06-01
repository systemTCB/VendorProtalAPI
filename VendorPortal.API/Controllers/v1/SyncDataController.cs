using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Response;
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
                if (dateSync == null)
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

        [HttpGet]
        [Route("sync-data/kub-boss/v1/quotation/{supplier_id}/{rfq_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API สำหรับ Get Quotation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuotationResponse))]
        public async Task<IActionResult> GetQuotation(string supplier_id, string rfq_id)
        {
            QuotationResponse response = new();
            try
            {
                response = await _kubbossService.SyncQuotationFromKubboss(supplier_id, rfq_id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetQuotation", $"rfq_id:{rfq_id} , supplier_id:{supplier_id}");
                response = new QuotationResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return Ok(response);
        }



        // [HttpGet("sync-data/kub-boss/v1/check-vendor")]
        // public async Task<IActionResult> CheckVendor()
        // {
        //     BaseResponse response = new BaseResponse();
        //     try
        //     {
        //         response = await _kubbossService.CheckVendor();
        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.LogError(ex, "CheckVendor");
        //         response = new BaseResponse()
        //         {
        //             status = new Status()
        //             {
        //                 code = ResponseCode.InternalServerError.Text(),
        //                 message = ResponseCode.InternalServerError.Description()
        //             }
        //         };
        //         return Ok(response);
        //     }
        // }


    }
}