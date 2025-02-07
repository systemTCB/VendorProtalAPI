using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class ClaimResponse : ResponseBasePage
    {
        // Add properties and methods here
        public List<ClaimData> Data { get; set; }
    }
    public class ClaimData
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Create_date { get; set; }
        public string Company_name { get; set; }
        public ClaimPurchaseOrderData Purchase_order { get; set; }
        public string Status { get; set; }
        public string Claim_date { get; set; }
        public string Claim_reason { get; set; }
        public string Claim_description { get; set; }
        public string Claim_option { get; set; }
        public string Claim_return_address { get; set; }
    }

}