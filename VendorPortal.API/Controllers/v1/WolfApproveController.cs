using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.API.Controllers.v1
{
    [ApiController]
    public class WolfApproveController : ControllerBase
    {
        private readonly IWolfApproveService _wolfApproveService;
        public WolfApproveController(IWolfApproveService wolfApproveService)
        {
            _wolfApproveService = wolfApproveService;
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/rfqs/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้ค้นหาทั้ง ชื่อโครงการ, รายเอียดโครง, บริษัท")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(RFQResponse))]
        public async Task<IActionResult> GetRFQList(string q, 
            string supplier_id, 
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string status_id,
            string category_id,
            string page,
            string per_page,
            string order_direction,
            string order_by)
        {
            RFQResponse response;
            try
            {
                response = await _wolfApproveService.GetRFQ_List();

            }
            catch (System.Exception ex)
            {
                response = new RFQResponse()
                {
                    Status = new Application.Models.Common.Status()
                    {
                        Code = ResponseCode.InternalError.Text(),
                        Message = ResponseCode.InternalError.Description()
                    },
                    Data = null
                };
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("api/v1/wolf-approve/rfqs/{rfq_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้ค้นหา rfq จาก id เพื่อดูรายละเอียด")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(RFQShowResponse))]   
        public async Task<IActionResult> GetRFQShow(string rfq_id)
        {
            RFQShowResponse response;
            try
            {
                response = await _wolfApproveService.GetRFQ_Show(rfq_id);
            }
            catch (System.Exception ex)
            {
                response = new RFQShowResponse()
                {
                    Status = new Application.Models.Common.Status()
                    {
                        Code = ResponseCode.InternalError.Text(),
                        Message = ResponseCode.InternalError.Description()
                    }
                };
            }
            return Ok(response);
        }
    
        [HttpGet]
        [Route("api/v1/wolf-approve/purchases/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PurchaseOrderResponse))]
        public async Task<IActionResult> GetPOList(string q, 
            string supplier_id, 
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string status_id,
            string category_id,
            string page,
            string per_page,
            string order_direction,
            string order_by)
        {
            PurchaseOrderResponse response = new();
            try
            {
                response = await _wolfApproveService.GetPurchaseOrderList();
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }
        [HttpGet]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO จาก id")]
        [Route("api/v1/wolf-approve/purchases/{id}/{supplier_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseOrderDetailResponse))]
        public async Task<IActionResult> GetPOShow(string id, string supplier_id)
        {
            PurchaseOrderDetailResponse response = new();
            try
            {
                response = await _wolfApproveService.GetPurchaseOrderShow(id , supplier_id);
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }
    
        [HttpPut]
        [Route("api/v1/wolf-approve/purchases/{id}/confirm-status")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับยืนยันสถานะ PO")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(PurchaseOrderConfirmResponse))]
        public async Task<IActionResult> ConfirmPOStatus(string id, [FromBody] PurchaseOrderConfirmRequest request)
        {
            PurchaseOrderConfirmResponse response = new();
            try
            {
                response = await _wolfApproveService.ConfirmPurchaseOrderStatus(id , request);
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/claim")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับเรียกรายการสินค้าที่ต้องการ claim")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(ClaimResponse))]
        public async Task<IActionResult> GetClaimList(string q, 
            string supplier_id, 
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string status_id,
            string category_id,
            string page,
            string per_page,
            string order_direction,
            string order_by)
        {
            ClaimResponse response = new();
            try
            {
                response = await _wolfApproveService.GetClaimList();
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/claim/{id}/{supplier_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับดูรายละเอียด claim")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(ClaimDetailResponse))]
        public async Task<IActionResult> GetClaimShow(string id , string supplier_id)
        {
            ClaimDetailResponse response = new();
            try
            {
                response = await _wolfApproveService.GetClaimShow(id , supplier_id);
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("api/v1/wolf-approve/claim/{id}/confirm-status")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับยืนยันสถานะ claim")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(ClaimConfirmResponse))]
        public async Task<IActionResult> ConfirmClaimStatus(string id , [FromBody] ClaimConfirmRequest request)
        {
            ClaimConfirmResponse response = new();
            try
            {
                response = await _wolfApproveService.ConfirmClaimStatus(id , request);
            }
            catch (System.Exception)
            {
                throw;
            }
            return Ok(response);
        }
    }
}