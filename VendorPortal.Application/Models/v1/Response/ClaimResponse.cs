using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class ClaimResponse 
    {
        // Add properties and methods here
        public string id { get; set; }
        public string claim_number { get; set; }
        public string create_date { get; set; }
        public string company_name { get; set; }
        public ClaimPurchaseOrderData purchase_order { get; set; }
        public string status { get; set; }
        public string claim_date { get; set; }
        public string claim_reason { get; set; }
        public string claim_description { get; set; }
        public string claim_option { get; set; }
        public string claim_return_address { get; set; }
    }

}