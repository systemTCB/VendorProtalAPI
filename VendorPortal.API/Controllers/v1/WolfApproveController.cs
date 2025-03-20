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
using VendorPortal.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using VendorPortal.Application.Models.Common;
using System.Collections.Generic;
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
        #region [RFQ]

        [HttpGet]
        [Route("api/v1/wolf-approve/rfqs/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้ค้นหาทั้ง ชื่อโครงการ, รายเอียดโครง, บริษัท")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RFQDataItem>))]
        public async Task<IActionResult> GetRFQList(string q,
            string company_id,
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
            BaseResponse<List<RFQDataItem>> response;
            try
            {
                response = await _wolfApproveService.GetRFQ_List(
                    pageSize: Convert.ToInt32(per_page),
                    page: Convert.ToInt32(page),
                    company_id: company_id,
                    number: number,
                    start_date: start_date,
                    end_date: end_date,
                    purchase_type_id: purchase_type_id,
                    status_id: status_id,
                    category_id: category_id,
                    order_direction: order_direction,
                    order_by: order_by,
                    q: q
                );
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetRFQList");
                response = new BaseResponse<List<RFQDataItem>>()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("api/v1/wolf-approve/rfqs/{rfq_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้ค้นหา rfq จาก id เพื่อดูรายละเอียด")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RFQShowResponse))]
        public async Task<IActionResult> GetRFQShow(string rfq_id)
        {
            RFQShowResponse response;
            try
            {
                response = await _wolfApproveService.GetRFQ_Show(rfq_id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, nameof(GetRFQShow));
                response = new RFQShowResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }

        #endregion

        #region [Puchase Order]

        [HttpGet]
        [Route("api/v1/wolf-approve/purchases/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseOrderResponse))]
        public async Task<IActionResult> GetPOList(string q,
            string supplier_id,
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string status_id,
            string category_id,
            int page,
            int per_page,
            string order_direction,
            string order_by)
        {
            BaseResponse<List<PurchaseOrderResponse>> response = new();
            try
            {
                response = await _wolfApproveService.GetPurchaseOrderList(q, supplier_id, number, start_date, end_date, purchase_type_id, status_id, category_id, page, per_page, order_direction, order_by);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetPOList");
                response = new BaseResponse<List<PurchaseOrderResponse>>()
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
        [HttpGet]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO จาก PO Code")]
        [Route("api/v1/wolf-approve/purchases/{id}/{supplier_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseOrderDetailResponse))]
        public async Task<IActionResult> GetPOShow(string id, string supplier_id)
        {
            PurchaseOrderDetailResponse response = new();
            try
            {
                response = await _wolfApproveService.GetPurchaseOrderDetail(id, supplier_id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetPOShow", $"id:{id} , supplier_id:{supplier_id}");
                response = new PurchaseOrderDetailResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                    ,
                    Data = null
                };
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("api/v1/wolf-approve/purchases/{id}/confirm-status")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "ใช้สำหรับยืนยันสถานะ PO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseOrderConfirmResponse))]
        public async Task<IActionResult> ConfirmPOStatus(string id, [FromBody] PurchaseOrderConfirmRequest request)
        {
            PurchaseOrderConfirmResponse response = new();
            try
            {
                response = await _wolfApproveService.ConfirmPurchaseOrderStatus(id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmPOStatus", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");
                response = new PurchaseOrderConfirmResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }

        #endregion

        #region [Claim]

        [HttpGet]
        [Route("api/v1/wolf-approve/claim")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับเรียกรายการสินค้าที่ต้องการ claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClaimResponse))]
        public async Task<IActionResult> GetClaimList(string supplier_id,
            string company_id,
            string status,
            string from_date,
            string to_date,
            string page,
            string per_page,
            string order_direction,
            string order_by)
        {
            BaseResponse<List<ClaimResponse>> response = new();
            try
            {
                response = await _wolfApproveService.GetClaimList(supplier_id, company_id, status, from_date, to_date, page, per_page, order_direction, order_by);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClaimDetailResponse))]
        public async Task<IActionResult> GetClaimShow(string id, string supplier_id)
        {
            ClaimDetailResponse response = new();
            try
            {
                response = await _wolfApproveService.GetClaimDetail(id, supplier_id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetClaimShow", $"id:{id} , supplier_id:{supplier_id}");
                response = new ClaimDetailResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                    ,
                    Data = null
                };
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/claim/{id}/confirm-status")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับยืนยันสถานะ claim")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClaimConfirmResponse))]
        public async Task<IActionResult> ConfirmClaimStatus(string id, [FromBody] ClaimConfirmRequest request)
        {
            ClaimConfirmResponse response = new();
            try
            {
                response = await _wolfApproveService.ConfirmClaimStatus(id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmClaimStatus", $"id:{id} ,request :{JsonConvert.SerializeObject(request)}");
                response = new ClaimConfirmResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return Ok(response);
        }
        #endregion

        #region [Compaines]

        [HttpGet]
        [Route("api/v1/wolf-approve/companies/list/{supplier_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับดึงข้อมูลบริษัท")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompaniesResponse))]
        public async Task<IActionResult> GetCompanies(string supplier_id)
        {
            BaseResponse<List<CompaniesResponse>> response = new();
            try
            {
                response = await _wolfApproveService.GetCompaniesList(supplier_id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompanies", $"supplier_id:{supplier_id}");
                response = new BaseResponse<List<CompaniesResponse>>()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/companies/{id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับดึงข้อมูลบริษัท จาก id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompaniesDetailResponse))]
        public async Task<IActionResult> GetCompaniesById(string id)
        {
            CompaniesDetailResponse response = new();
            try
            {
                response = await _wolfApproveService.GetCompaniesDetail(id);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompaniesById", $"id:{id}");
                response = new CompaniesDetailResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("api/v1/wolf-approve/companies/connect-compaines/{supplier_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API ใช้สำหรับเชื่อมต่อบริษัท")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompaniesConnectResponse))]
        public async Task<IActionResult> ConnectCompanies(string supplier_id, [FromBody] CompaniesConnectRequest request)
        {
            CompaniesConnectResponse response = new();
            try
            {
                response = await _wolfApproveService.ConnectCompanies(supplier_id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConnectCompanies", $"supplier_id:{supplier_id}");
                response = new CompaniesConnectResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }

        #endregion

        #region [Count]
        [HttpGet]
        [Route("api/v1/wolf-approve/count/{suppliers}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "VendorPortal V1" }, Summary = "", Description = "API สำหรับนับจำนวน PO , Claim ที่มีสถานะเป็น Pendding")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponse))]
        public async Task<IActionResult> GetCount(string suppliers)
        {
            CountResponse response = new();
            try
            {
                response = await _wolfApproveService.GetCountClaimPo(suppliers);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCount", $"suppliers:{suppliers}");
                response = new CountResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return Ok(response);
        }
        #endregion
    }
}