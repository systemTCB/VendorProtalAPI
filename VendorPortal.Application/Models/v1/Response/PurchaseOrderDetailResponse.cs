using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.SqlServer.Server;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderDetailResponse : BaseResponse
    {
        // Add properties and methods here
        public PurchaseOrderDetailData Data { get; set; }
    }

    public class PurchaseOrderDetailData
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public QuotationData Quotation { get; set; }
        public string Company_name { get; set; }
        public string Description { get; set; }
        public string Category_name { get; set; }   
        public string Order_date { get; set; }
        public string Request_date { get; set; }
        public string Status { get; set; }
        public CompanyAddress Company_address { get; set; }
        public CompanyContract Company_contract { get; set; }
        public string Sub_totoal { get; set; }
        public string Discount { get; set; }
        public string Total_amount { get; set; }
        public string Vat_amount { get; set; }
        public string Net_amount { get; set; }
        public List<Line> Lines { get; set; }  
        public List<Document> Documents { get; set; }
        public string Payment_condition { get; set; }
        public string Remark { get; set; }
        public string Cancel_reason { get; set; }
        public string Cancel_description { get; set; }

    }

    public class QuotationData
    {
        public string code { get; set; }
    }
}