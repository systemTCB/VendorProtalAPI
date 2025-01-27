using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.v1;
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
        [Route("api/v1/wolf-approve/rfq-list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "")]
        [ProducesResponseType(StatusCodes.Status200OK , Type = typeof(RFQResponse))]
        public async Task<IActionResult> GetRFQList(string q, string supplier_id, string number, string start_date, string end_date, string purchase_type_id, string status_id, string category_id, string page, string per_page, string order_direction, string order_by)
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
        [Route("api/v1/wolf-approve/rfq-show")]
        // [Description("Create By Peetisook || ใช้ค้นหาทั้ง ชื่อโครงการ, รายเอียดโครง, บริษัท")]
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
        [Route("")]
        public async Task<IActionResult> GetPOList()
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

    }
}