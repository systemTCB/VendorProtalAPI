using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class RFQCancelRequest
    {
        public string rfq_id { get; set; }
        public string rfq_number { get; set; }
        public string remark { get; set; }
    }
}