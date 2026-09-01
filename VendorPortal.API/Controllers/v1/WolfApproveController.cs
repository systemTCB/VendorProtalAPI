using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;
namespace VendorPortal.API.Controllers.v1
{
    [ApiController]
    public class WolfApproveController : ControllerBase
    {
        private readonly IWolfApproveService _wolfApproveService;
        private readonly IKubbossService _kubBossService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public WolfApproveController(IWolfApproveService wolfApproveService, IKubbossService kubBossService, IServiceScopeFactory serviceScopeFactory)
        {
            _wolfApproveService = wolfApproveService;
            _kubBossService = kubBossService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        #region [RFQ]

        [HttpGet]
        [Route("api/v1/wolf-approve/rfqs/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "RFQ V1" }, Summary = "", Description = "ใช้ค้นหาทั้ง ชื่อโครงการ, รายเอียดโครง, บริษัท")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RFQDataItem>))]
        public async Task<IActionResult> GetRFQList(string q,
            string supplier_id,
            string company_id,
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string request_for_type,
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
                    supplier_id: supplier_id,
                    company_id: company_id,
                    number: number,
                    start_date: start_date,
                    end_date: end_date,
                    purchase_type_id: purchase_type_id,
                    request_for_type: request_for_type,
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
        [SwaggerOperation(Tags = new[] { "RFQ V1" }, Summary = "", Description = "ใช้ค้นหา rfq จาก id เพื่อดูรายละเอียด")]
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


        [HttpPost]
        [Route("api/v1/wolf-approve/rfqs/create")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "RFQ V1" }, Summary = "", Description = "ใช้สำหรับสร้าง RFQ ใหม่")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RFQCreateResponse))]
        public async Task<IActionResult> CreateRFQ([FromBody] RFQCreateRequest request)
        {
            RFQCreateResponse response = new();
            try
            {
                var requestHost = HttpContext.Request;
                string domain = $"{requestHost.Scheme}://{requestHost.Host}";

                Logger.LogInfo("CreateRFQ", $"domain: {domain}");

                response = await _wolfApproveService.CreateAndUpdateRFQ(request, domain);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new RFQCreateResponse()
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

        [HttpPost]
        [Route("api/v1/wolf-approve/rfqs/update")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "RFQ V1" }, Summary = "", Description = "ใช้สำหรับสร้าง Update RFQ")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RFQUpdateResponse))]
        public async Task<IActionResult> UpdateRFQ([FromBody] RFQUpdateRequest request)
        {
            RFQUpdateResponse response = new();
            try
            {
                response = await _wolfApproveService.UpdateRFQ(request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new RFQUpdateResponse()
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

        [HttpPost]
        [Route("api/v1/wolf-approve/rfqs/cancel")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "RFQ V1" }, Summary = "", Description = "ใช้สำหรับยกเเลิก RFQ")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RFQCancelResponse))]
        public async Task<IActionResult> RFQCancelDocument([FromBody] RFQCancelRequest request)
        {
            RFQCancelResponse response = new();

            try
            {
                response = await _wolfApproveService.RFQCancelDocument(request);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Cancel RFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new RFQCancelResponse()
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

        #endregion

        #region [Puchase Order]

        [HttpGet]
        [Route("api/v1/wolf-approve/purchases/list")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO")]
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
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับดึงข้อมูล PO จาก PO Code")]
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
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับยืนยันสถานะ PO")]
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

        [HttpPost]
        [Route("api/v1/wolf-approve/purchases/create")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับสร้าง PO ใหม่")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(POCreateResponse))]
        public async Task<IActionResult> CreatePO([FromBody] POCreateRequest request)
        {
            POCreateResponse response = new();
            try
            {
                var requestHost = HttpContext.Request;
                string domain = $"{requestHost.Scheme}://{requestHost.Host}";

                Logger.LogInfo("CreatePO", $"domain: {domain}");

                response = await _wolfApproveService.CreatePO(request, domain);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new POCreateResponse()
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

        [HttpPost]
        [Route("api/v2/wolf-approve/purchases/create")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "PO V2" }, Summary = "", Description = "ใช้สำหรับสร้าง PO ใหม่ เพิ่ม Document Type")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(POCreateResponse))]
        public async Task<IActionResult> CreatePO_V2([FromBody] POCreateV2Request request)
        {
            POCreateV2Response response = new();
            try
            {
                var requestHost = HttpContext.Request;
                string domain = $"{requestHost.Scheme}://{requestHost.Host}";

                Logger.LogInfo("CreatePO", $"domain: {domain}");

                response = await _wolfApproveService.CreatePOV2(request, domain);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new POCreateV2Response()
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

        [HttpPost]
        [Route("api/v1/wolf-approve/purchases/cancel")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับยกเลิก PO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(POCancelResponse))]
        public async Task<IActionResult> CancelPO([FromBody] POCancelRequest request)
        {
            POCancelResponse response = new();
            try
            {
                response = await _wolfApproveService.CancelPO(request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new POCancelResponse()
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


        [HttpPost]
        [Route("api/v1/wolf-approve/purchases/create/by-quotation")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "ใช้สำหรับสร้าง PO เมื่อมีการ Award ใบ Quotation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(POCreateResponse))]
        public async Task<IActionResult> CreatePOByQuotation([FromBody] QuotationAwardRequest request)
        {
            QuotationAwardResponse response = new();
            try
            {
                var requestHost = HttpContext.Request;
                string domain = $"{requestHost.Scheme}://{requestHost.Host}";

                Logger.LogInfo("CreatePO", $"domain: {domain}");

                response = await _wolfApproveService.CreatePOAward(request, domain);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new QuotationAwardResponse()
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

        [HttpPut]
        [Route("api/v1/wolf-approve/purchases/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "PO V1" }, Summary = "", Description = "API สำหรับสร้าง PO ไปยัง WOLF")]
        public async Task<IActionResult> PutPO(string id, [FromBody] CreatePORequest request)
        {
            try
            {
                var result = await _kubBossService.GetPOByID(id, request);
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Create PO", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");

                return Ok(new InvoicesByIDResponse
                {
                    status = new Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                });
            }
        }

        #endregion

        #region [Claim]

        [HttpGet]
        [Route("api/v1/wolf-approve/claim")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "Claim V1" }, Summary = "", Description = "API ใช้สำหรับเรียกรายการสินค้าที่ต้องการ claim")]
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
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetClaimList");
                response = new BaseResponse<List<ClaimResponse>>()
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
        [Route("api/v1/wolf-approve/claim/{id}/{supplier_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "Claim V1" }, Summary = "", Description = "API ใช้สำหรับดูรายละเอียด claim")]
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
        [SwaggerOperation(Tags = new[] { "Claim V1" }, Summary = "", Description = "API ใช้สำหรับยืนยันสถานะ claim")]
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

        #region [Companies]

        [HttpGet]
        [Route("api/v1/wolf-approve/companies/list/{supplier_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "Companies V1" }, Summary = "", Description = "API ใช้สำหรับดึงข้อมูลบริษัท")]
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
        [SwaggerOperation(Tags = new[] { "Companies V1" }, Summary = "", Description = "API ใช้สำหรับดึงข้อมูลบริษัท จาก id")]
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
        [SwaggerOperation(Tags = new[] { "Companies V1" }, Summary = "", Description = "API ใช้สำหรับเชื่อมต่อบริษัท")]
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
        [SwaggerOperation(Tags = new[] { "Count V1" }, Summary = "", Description = "API สำหรับนับจำนวน PO , Claim ที่มีสถานะเป็น Pendding")]
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

        #region [Quotation]
        [HttpPut]
        [Route("api/v1/wolf-approve/quotation/noti/{rfq_id}")]
        [Description("Create By Peetisook")]
        [SwaggerOperation(Tags = new[] { "Quotation V1" }, Summary = "", Description = "API สำหรับ Put Quotation")]
        public async Task<IActionResult> PutQuotation(string rfq_id, [FromBody] PutQuotationRequest request)
        {
            BaseResponse response = new();
            try
            {
                response = await _wolfApproveService.PutQuotation(rfq_id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "PutQuotation", $"rfq_id:{rfq_id} , request:{JsonConvert.SerializeObject(request)}");
                response = new BaseResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/quotation/updateStatus/{rfq_id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Quotation V1" }, Summary = "", Description = "API สำหรับ Update Quotation")]
        public async Task<IActionResult> UpdateStatusQuotation(string rfq_id, [FromBody] PutQuotationRequest request)
        {
            BaseResponse response = new();
            try
            {
                response = await _wolfApproveService.PutQuotation(rfq_id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Update Quotation", $"rfq_id:{rfq_id} , request:{JsonConvert.SerializeObject(request)}");
                response = new BaseResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }


        #endregion

        #region [Vendor Register]
        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/register")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
            Tags = new[] { "Vendor Register V1" },
            Summary = "Register Vendor",
            Description = "Vendor Register by supplier_id"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierRegistrationResponse))]
        public async Task<IActionResult> VendorRegister([FromBody] VendorRegisterRequest request)
        {

            if (string.IsNullOrEmpty(request.buyerCode) || string.IsNullOrEmpty(request.supplier_id))
                return BadRequest("buyerCode or supplier id is required");

            RegisterResponse responseSuppliers = new();

            try
            {

                var supplierId = request.supplier_id;
                var buyerCode = request.buyerCode;
                var docNo = request.docNo;
                bool is_unblock = request.is_unblock;

                //var result = await _kubBossService.RegsiterSuppliersFromKubboss(request.supplier_id, request.buyerCode, request.docNo, request.is_unblock);

                //return result.success
                //    ? Ok(result)
                //    : BadRequest(result);

                responseSuppliers = new RegisterResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = "รับคำขอเรียบร้อย กำลังดำเนินการลงทะเบียนผู้ขาย"
                    },
                    data = null
                };

                _ = Task.Run(async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var kubBossService = scope.ServiceProvider.GetRequiredService<IKubbossService>();

                    try
                    {
                        int delaySeconds = Random.Shared.Next(0, 5);

                        await Logger.LogInfo($"BACKGROUND START | supplier_id:{supplierId} | buyerCode:{buyerCode} | docNo:{docNo} | delaySeconds:{delaySeconds}", "VendorRegister");

                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));

                        await Logger.LogInfo($"BACKGROUND CALL KUBBOSS | supplier_id:{supplierId} | buyerCode:{buyerCode} | docNo:{docNo}", "VendorRegister");

                        var result = await kubBossService.RegsiterSuppliersFromKubboss(supplierId, buyerCode, docNo, is_unblock);

                        await Logger.LogInfo($"BACKGROUND RESULT | supplier_id:{supplierId} | success:{result.success}", "VendorRegister");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "VendorRegister BACKGROUND ERROR", $"supplier_id: {supplierId}");
                    }
                });

          

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "VendorRegister ERROR", $"supplier_id: {request.supplier_id}");

                responseSuppliers = new RegisterResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseSuppliers);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/register-supplier-wolf")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
            Tags = new[] { "Vendor Register V1" },
            Summary = "Register wolf supplier from email and send email register kubboss",
            Description = "Register wolf supplier from email and send email register kubboss"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierRegistrationResponse))]
        public async Task<IActionResult> VendorRegisterSupplier([FromBody] SupplierRegisterRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.email))
                return BadRequest("email is required");

            SupplierRegisterResponse responseSuppliers = new();

            try
            {
                var result = await _kubBossService.RegsiterSuppliersToKubboss(request);
                if (result == null)
                {
                    return StatusCode(500, "Register supplier failed");
                }

                if (result.status?.code != "200")
                {
                    return BadRequest(result);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "VendorRegister ERROR", $"email: {request.email}");

                return StatusCode(StatusCodes.Status500InternalServerError, new SupplierRegisterResponse
                {
                    status = new Application.Models.Common.Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                });
            }
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/register-supplier-trial")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
       Tags = new[] { "Vendor Register V1" },
       Summary = "Register wolf supplier from email and send email register kubboss",
       Description = "Register wolf supplier from email and send email register kubboss"
   )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierRegistrationResponse))]
        public async Task<IActionResult> VendorRegisterSupplierTrial([FromBody] SupplierRegisterRequestTrial request)
        {

            if (string.IsNullOrWhiteSpace(request.email))
                return BadRequest("email is required");

            SupplierRegisterResponse responseSuppliers = new();

            try
            {
                var result = await _kubBossService.RegsiterSuppliersToKubbossTrial(request);
                if (result == null)
                {
                    return StatusCode(500, "Register supplier failed");
                }

                if (result.status?.code != "200")
                {
                    return BadRequest(result);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "VendorRegister ERROR", $"email: {request.email}");

                return StatusCode(StatusCodes.Status500InternalServerError, new SupplierRegisterResponse
                {
                    status = new Application.Models.Common.Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                });
            }
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/questionnaire/answer/update")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
      Tags = new[] { "Vendor Register V1" },
      Summary = "Vendor Register Questionnaire Update",
      Description = "Vendor Register Questionnaire Update"
  )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierRegistrationResponse))]
        public async Task<IActionResult> VendorRegisterQuestionnaireUpdate([FromBody] QuestionnaireUpdateRequest request)
        {

            await Logger.LogInfo($"Answer Update | supplier_answer_id:{request.supplier_answer_id} | status:{request.status}", "VendorRegister");

            if (string.IsNullOrWhiteSpace(request.supplier_answer_id))
                return BadRequest("supplier answer id is required");

            SupplierRegisterResponse responseSuppliers = new();

            try
            {
                var result = await _kubBossService.VendorRegisterQuestionnaireUpdate(request);

                if (result == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Register supplier failed");
                }

                var statusCode = result["status"]?["code"]?.ToString();

                if (statusCode != "200")
                {
                    return BadRequest(result);
                }

                return Content(result.ToString(Newtonsoft.Json.Formatting.None), "application/json");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "VendorRegisterQuestionnaireUpdate ERROR", $"supplier_answer_id: {request.supplier_answer_id}");
                return StatusCode(StatusCodes.Status500InternalServerError, new SupplierRegisterResponse
                {
                    status = new Application.Models.Common.Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                });
            }
        }

        #endregion

        #region RFP
        [HttpPost]
        [Route("api/v1/wolf-approve/rfp/create/{DocumentNo}/{company_id}")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Tags = new[] { "RFP V1" }, Summary = "", Description = "ใช้สำหรับสร้าง create RFP")]
        public async Task<IActionResult> CreateRFP([FromRoute] string DocumentNo, [FromForm] RFPCreateRequest requestData, [FromRoute] int company_id, [FromForm] List<IFormFile> files)
        {
            RFPCreateResponse response = new();

            try
            {
                var request = HttpContext.Request;
                string domain = $"{request.Scheme}://{request.Host}";

                Logger.LogInfo("CreateRFP", $"domain: {domain}");

                response = await _wolfApproveService.CreateRFP(DocumentNo, requestData, company_id, files, domain);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreateRFP");
                response = new RFPCreateResponse()
                {
                    status = new Status()
                    {
                        code = "500",
                        message = ex.Message
                    }
                };
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/document-acknowledge")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Vendor Register V1" }, Summary = "", Description = "ใช้สำหรับบอกว่าได้รับเอกสารหรือเปิดเอกสารอ่านแล้ว")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RFQCreateResponse))]
        public async Task<IActionResult> acknowledgeDocument([FromBody] AcknowledgeDocumentRequest request)
        {
            AcknowledgeDocumentResponse response = new();

            try
            {
                response = await _wolfApproveService.AcknowledgeDocument(request);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request:{JsonConvert.SerializeObject(request)}");
                response = new AcknowledgeDocumentResponse()
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

        #endregion

        #region Siriraj
        [HttpGet]
        [Route("api/v1/wolf-approve/productMedical")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
            Tags = new[] { "Product Medical V1" },
            Summary = "Product Medical",
            Description = "Product Medical"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductMedicalResponse))]
        public async Task<IActionResult> productMedical(string sku, string name, string sortDirection, int page = 1, int per_page = 5)
        {

            ProductMedicalResponse responseProductMedical = new();
            try
            {
                var result = await _kubBossService.GetProductMedical(sku, name, sortDirection, page, per_page);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetProductMedical ERROR");

                responseProductMedical = new ProductMedicalResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseProductMedical);
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/productMedical/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
           Tags = new[] { "Product Medical V1" },
           Summary = "Product Medical",
           Description = "Product Medical"
       )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductMedicalResponse))]
        public async Task<IActionResult> productMedicalByID(string id)
        {

            ProductMedicalByIdResponse responseProductMedicalByID = new();
            try
            {
                var result = await _kubBossService.GetProductMedicalByID(id);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetProductMedical ERROR");

                responseProductMedicalByID = new ProductMedicalByIdResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseProductMedicalByID);
        }

        #endregion

        #region Request Documents
        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/RequestDocuments")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
           Tags = new[] { "Request Documents V1" },
           Summary = "Create Request Documents",
           Description = "Create a request document record from external systems."
       )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> RequestDocuments([FromForm] RequestDocumentRequest request)
        {
            await Logger.LogInfo($"CreateJobDocument: Incoming request | WolfVendorCode:{request?.WolfVendorCode}", "CreateJobDocument", JsonConvert.SerializeObject(request));

            try
            {
                var result = await _kubBossService.RequestDocuments(request);
                if (result == null)
                {
                    return StatusCode(500, "Request Documents Supplier Failed");
                }

                if (result.status?.code != "200")
                {
                    return BadRequest(result);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Request Documents ERROR", $"supplier_id: {request.supplier_id}");

                return StatusCode(StatusCodes.Status500InternalServerError, new SupplierRegisterResponse
                {
                    status = new Application.Models.Common.Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                });
            }
        }

        [HttpGet]
        [Route("api/v1/wolf-approve/RequestDocuments/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
          Tags = new[] { "Request Documents V1" },
          Summary = "Get request document by id",
          Description = "Get request document by id"
      )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> RequestDocuments(string id, [FromBody] PutRequestDocuments request)
        {

            DocumentCreatetResponse responseRequestDocumentsByID = new();
            try
            {
                var result = await _kubBossService.GetRequestDocumentsByID(id, request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetProductMedical ERROR");

                responseRequestDocumentsByID = new DocumentCreatetResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseRequestDocumentsByID);
        }

        [HttpPut]
        [Route("api/v1/wolf-approve/RequestDocuments/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
         Tags = new[] { "Request Documents V1" },
         Summary = "Get request document by id",
         Description = "Get request document by id"
     )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutRequestDocuments(string id, [FromBody] PutRequestDocuments request)
        {

            DocumentCreatetResponse responseRequestDocumentsByID = new();
            try
            {
                var result = await _kubBossService.GetRequestDocumentsByID(id, request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestDocuments ERROR");

                responseRequestDocumentsByID = new DocumentCreatetResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseRequestDocumentsByID);
        }


        [HttpPost]
        [Route("api/v1/wolf-approve/vendor/RequestDocumentsUpdate")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
           Tags = new[] { "Request Documents V1" },
           Summary = "Create Request Documents",
           Description = "Create a request document record from external systems."
       )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentUpdateResponse))]
        public async Task<IActionResult> RequestDocumentsUpdate([FromBody] DocumentUpdateRequest request)
        {

            if (request.id == null)
                return BadRequest("id is required");
            try
            {
                var result = await _kubBossService.RequestDocumentsUpdate(request);
                if (result == null)
                {
                    return StatusCode(500, "Request Documents Update Failed");
                }

                if (result.status?.code != "200")
                {
                    return BadRequest(result);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Request Documents Update ERROR", $"Document ID: {request.id}");

                return StatusCode(StatusCodes.Status500InternalServerError, new SupplierRegisterResponse
                {
                    status = new Application.Models.Common.Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                });
            }
        }


        #endregion

        #region Delivery Orders

        [HttpPost]
        [Route("api/v1/wolf-approve/delivery-orders/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "DeliveryOrders V1" }, Summary = "", Description = "API สำหรับ สร้าง Delivery Orders")]
        public async Task<IActionResult> DeliveryOrders(string id, [FromBody] CreateDORequest request)
        {
            BaseResponse response = new();
            try
            {
                //response = await _wolfApproveService.PutQuotation(id, request);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Create Delivery Orders", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");
                response = new BaseResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(response);
        }

        [HttpPut]
        [Route("api/v1/wolf-approve/delivery-orders/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
        Tags = new[] { "Delivery Orders V1" },
        Summary = "Get request document by id",
        Description = "Get request document by id"
    )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutDeliveryOrders(string id, [FromBody] PutDeliveryOrdersRequest request)
        {

            DeliveryOrdersResponse responseDeliveryOrdersByID = new();
            try
            {
                var result = await _kubBossService.GetDeliveryOrdersByID(id, request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestDocuments ERROR");

                responseDeliveryOrdersByID = new DeliveryOrdersResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseDeliveryOrdersByID);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/delivery-orders/update-status")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
        Tags = new[] { "Delivery Orders V1" },
        Summary = "Get delivery orders by id",
        Description = "Get delivery orders by id"
    )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutDeliveryOrdersUpdateStatus([FromBody] PutDeliveryOrdersUpdateRequest request)
        {

            DeliveryOrdersUpdateResponse responseDeliveryOrdersUpdate = new();

            try
            {
                var result = await _kubBossService.DeliveryOrdersUpdateStatus(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestDocuments ERROR");

                responseDeliveryOrdersUpdate = new DeliveryOrdersUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseDeliveryOrdersUpdate);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/delivery-orders/sap-status")]
        [Description("Create By Triphop")]
        [SwaggerOperation(
            Tags = new[] { "Delivery Orders V1" }, 
            Summary = "Get delivery orders by id", 
            Description = "Updates whether a delivery order has been sent to SAP. When set to Y, the supplier's cancel delivery order action is disabled/hidden; " +
            "when set to N, it becomes available again (e.g. for correction).")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PatchDeliveryOrderSAPStatus([FromBody] PutDeliveryOrdersUpdateRequest request)
        {

            DeliveryOrdersUpdateResponse responseDeliveryOrdersUpdate = new();

            try
            {
                var result = await _kubBossService.DeliveryOrdersUpdateStatus(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RequestDocuments ERROR");

                responseDeliveryOrdersUpdate = new DeliveryOrdersUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseDeliveryOrdersUpdate);
        }

        #endregion

        #region Invoices
        [HttpPut]
        [Route("api/v1/wolf-approve/invoices/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Invoices V1" }, Summary = "", Description = "API สำหรับ สร้าง invoices")]
        public async Task<IActionResult> PutInvoices(string id, [FromBody] CreateInvoiceRequest request)
        {
            try
            {
                var result = await _kubBossService.GetInvoicesByID(id, request);
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Create Invoice", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");

                return Ok(new InvoicesByIDResponse
                {
                    status = new Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                });
            }
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/invoices/update-status")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Invoices V1" }, Summary = "", Description = "API สำหรับ Update invoices")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutInvoicesUpdateStatus([FromBody] PutInvoicesUpdateRequest request)
        {

            InvoicesUpdateResponse responseInvoicesUpdate = new();

            try
            {
                var result = await _kubBossService.InvoicesUpdateStatus(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Invoices Update ERROR");

                responseInvoicesUpdate = new InvoicesUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseInvoicesUpdate);
        }

        #endregion

        #region Subscriptions BLOCK / UN-BLOCK

        [HttpPost]
        [Route("api/v1/wolf-approve/subscriptions/update")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Subscriptions V1" }, Summary = "", Description = "API สำหรับ Subscriptions BLOCK / UN-BLOCK")]
        public async Task<IActionResult> Subscriptions([FromBody] SubscriptionsRequest request)
        {
            try
            {
                var result = await _kubBossService.Subscriptions(request);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Subscriptions", $"email:{request.email} , request:{JsonConvert.SerializeObject(request)}");
                return Ok(new SubscriptionsResponse
                {
                    status = new Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                });
            }
        }
        #endregion

        #region Credit Notes
        [HttpPut]
        [Route("api/v1/wolf-approve/credit-notes/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Credit Notes V1" }, Summary = "", Description = "API สำหรับ สร้าง Credit Notes")]
        public async Task<IActionResult> PutCreditNotes(string id, [FromBody] PutCreditNotesRequest request)
        {
            CreditNoteByIDResponse responseInvoiceByID = new();
            try
            {
                var result = await _kubBossService.GetCreditNotesByID(id, request);
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Create Invoice", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");
                responseInvoiceByID = new CreditNoteByIDResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(responseInvoiceByID);
        }

        [HttpPost]
        [Route("api/v1/wolf-approve/credit-notes/update-status")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Credit Notes V1" }, Summary = "", Description = "API สำหรับ Update Credit Notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutCreditNotesUpdateStatus([FromBody] PutCreditNoteUpdateRequest request)
        {

            CreditNoteUpdateResponse responseCreditNoteUpdate = new();

            try
            {
                var result = await _kubBossService.CreditNoteUpdateStatus(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreditNoteUpdateStatus ERROR");

                responseCreditNoteUpdate = new CreditNoteUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseCreditNoteUpdate);
        }
        #endregion

        #region Debit Notes
        [HttpPut]
        [Route("api/v1/wolf-approve/debit-notes/noti/{id}")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Debit Notes V1" }, Summary = "", Description = "API สำหรับ สร้าง Debit Notes")]
        public async Task<IActionResult> PutDebitNotes(string id, [FromBody] PutDebitNotesRequest request)
        {
            DebitNotesByIDResponse responseInvoiceByID = new();
            try
            {
                var result = await _kubBossService.GetDebitNotesByID(id, request);
                return Ok(result);

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Create Debit Notes", $"id:{id} , request:{JsonConvert.SerializeObject(request)}");
                responseInvoiceByID = new DebitNotesByIDResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return Ok(responseInvoiceByID);
        }


        [HttpPost]
        [Route("api/v1/wolf-approve/debit-notes/update-status")]
        [Description("Create By Triphop")]
        [SwaggerOperation(Tags = new[] { "Debit Notes V1" }, Summary = "", Description = "API สำหรับ Update Debit Notes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentCreatetResponse))]
        public async Task<IActionResult> PutDebitNotesUpdateStatus([FromBody] PutDebitNoteUpdateRequest request)
        {

            DebitNoteUpdateResponse responseDebitNoteUpdate = new();

            try
            {
                var result = await _kubBossService.DebitNoteUpdateStatus(request);

                return Ok(result);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "CreditNoteUpdateStatus ERROR");

                responseDebitNoteUpdate = new DebitNoteUpdateResponse()
                {
                    status = new Application.Models.Common.Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },

                    data = null
                };
            }

            return Ok(responseDebitNoteUpdate);
        }
        #endregion
    }
}