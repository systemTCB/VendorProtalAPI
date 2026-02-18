using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class AcknowledgeDocumentRequest
    {
        public string rfq_id { get; set; }
        public string name { get; set; }
        public string supplier_id { get; set; }
        public string supplier_email { get; set; }
        public DateTime acknowledgeDate { get; set; }

    }

}