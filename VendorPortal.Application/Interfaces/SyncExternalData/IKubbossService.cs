using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Application.Interfaces.SyncExternalData
{
    public interface IKubbossService
    {
        Task<BaseResponse> SyncVendorFromKubboss(DateTime dateTime);
        Task<QuotationResponse> SyncQuotationFromKubboss(string supplierId, string rfqId);
        Task<ActionResultResponse> RegsiterSuppliersFromKubboss(string supplier_id, string buyerCode, string docNo);
      
        Task<JObject> GetQuotationDetail(HttpClient client, string quoId);
        Task<JObject> GetSuppliersDetail(HttpClient client, string supplier_id);
        Task<ProductMedicalResponse> GetProductMedical(string sku, string name, string sortDirection, int page, int per_page);
        Task<ProductMedicalByIdResponse> GetProductMedicalByID(string id);
    }
}