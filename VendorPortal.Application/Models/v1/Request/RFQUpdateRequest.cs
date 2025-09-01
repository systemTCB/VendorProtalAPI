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
    }

    public class RFQUpdateDocument
    {
        public string file_name { get; set; }
        public string file_path { get; set; }
        public int file_seq { get; set; }
    }
}