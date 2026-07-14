using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class RFQUpdateRequest
    {
        public string rfq_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public List<RFQUpdateDocument> documents { get; set; }
        public string modified_by { get; set; }
        public string supplier_id { get; set; }
    }

    public class RFQUpdateDocument
    {
        public string file_name { get; set; }
        public string file_path { get; set; }
        public int file_seq { get; set; }
    }

    public class RFQUpdateStatus
    {
        public string status { get; set; }
        public string supplier_name { get; set; }
        public string supplier_email { get; set; }
        public string doc_number { get; set; }
        public string reason { get; set; }
        public bool cancel { get; set; } = false;
    }
}