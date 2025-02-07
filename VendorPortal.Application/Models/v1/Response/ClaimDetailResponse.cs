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
        public string Id { get; set; }
        public string Code { get; set; }
        public string Create_date { get; set; }
        public string Company_name { get; set; }
        public ClaimPurchaseOrderData Purchase_order { get; set; }
        public ClaimStatus Status { get; set; }
        public string Claim_date { get; set; }
        public List<Line> Lines { get; set; }
        public string Claim_reason { get; set; }
        public string Claim_description { get; set; }
        public string Claim_option { get; set; }
        public string Claim_return_address { get; set; }
        public List<Document> Documents { get; set; }

    }
    public class ClaimStatus
    {
        public string Name { get; set; }
    }
}