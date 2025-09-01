using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class ClaimDetailResponse : BaseResponse
    {
        // Add properties and methods here
        public ClaimDetailData Data { get; set; }

    }

    public class ClaimDetailData
    {
        public string id { get; set; }
        public string code { get; set; }
        public string create_date { get; set; }
        public string company_name { get; set; }
        public ClaimPurchaseOrderData purchase_order { get; set; }
        public ClaimStatus status { get; set; }
        public string claim_date { get; set; }
        public List<Line> lines { get; set; }
        public string claim_reason { get; set; }
        public string claim_description { get; set; }
        public string claim_option { get; set; }
        public string claim_return_address { get; set; }
        public List<Document> documents { get; set; }

    }
    public class ClaimStatus
    {
        public List<string> Name { get; set; }
    }
}