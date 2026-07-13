using System.Threading.Tasks;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IWolfApproveService
    {
        // RFQ
        Task<BaseResponse<List<RFQDataItem>>> GetRFQ_List(int pageSize, int page, string supplier_id, string company_id,
                    string number,
                    string start_date,
                    string end_date,
                    string purchase_type_id,
                    string request_for_type,
                    string status_id,
                    string category_id,
                    string order_direction,
                    string order_by,
                    string q);
        Task<RFQShowResponse> GetRFQ_Show(string rfq_id);

        Task<RFQCreateResponse> CreateAndUpdateRFQ(RFQCreateRequest request, string domain);
        Task<RFQUpdateResponse> UpdateRFQ(RFQUpdateRequest request);

        // Purchase Order
        Task<BaseResponse<List<PurchaseOrderResponse>>> GetPurchaseOrderList(string q, 
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
            string order_by);
        Task<PurchaseOrderDetailResponse> GetPurchaseOrderDetail(string order_id, string supplier_id);
        Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id, PurchaseOrderConfirmRequest request);

        Task<POCreateResponse> CreatePO(POCreateRequest request, string domain);
        Task<POCancelResponse> CancelPO(POCancelRequest request);
        Task<QuotationAwardResponse> CreatePOAward(QuotationAwardRequest request);


        // Claim 
        Task<BaseResponse<List<ClaimResponse>>> GetClaimList(string supplier_id, string company_id, string status, string from_date, string to_date, string page, string per_page, string order_direction, string order_by);
        Task<ClaimDetailResponse> GetClaimDetail(string claim_id, string supplier_id);
        Task<ClaimConfirmResponse> ConfirmClaimStatus(string claim_id, ClaimConfirmRequest request);

        // Companies
        Task<BaseResponse<List<CompaniesResponse>>> GetCompaniesList(string supplier_id);
        Task<CompaniesDetailResponse> GetCompaniesDetail(string company_id);
        Task<CompaniesConnectResponse> ConnectCompanies(string supplier_id, CompaniesConnectRequest request);

        //Count
        Task<CountResponse> GetCountClaimPo(string supplier_id);

        //Quotation
        Task<BaseResponse> PutQuotation(string rfq_id, PutQuotationRequest request);
        Task<QuotationResponse> GetQuotation(string supplier_id, string rfq_id);

        //Vendor Register
        Task<VendorRegisterResponse> CreateSupplierRegistration(string companyID, SupplierRegistrationResponse responseSuppliers);

        //Task<bool> CreateSupplierRegistrationLPN(SupplierRegisterResponse Suppliers, string buyerCode);

        Task<RFPCreateResponse> CreateRFP(string documentNo, RFPCreateRequest requestData, int company_id, List<IFormFile> files, string domain);
        Task<AcknowledgeDocumentResponse> AcknowledgeDocument(AcknowledgeDocumentRequest request);
        Task<RFQCancelResponse> RFQCancelDocument(RFQCancelRequest request);

    }
}