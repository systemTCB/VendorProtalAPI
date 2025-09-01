using System;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQCreateResponse : BaseResponse
    {
        public RFQCreateData data { get; set; } = new RFQCreateData();
    }

    public class RFQCreateData
    {
        public string rfq_id { get; set; } = string.Empty;
        public string rfq_number { get; set; } = string.Empty;
        public int company_id { get; set; } 
        public string company_name { get; set; } = string.Empty;
        public DateTime created_date { get; set; }
    }
}