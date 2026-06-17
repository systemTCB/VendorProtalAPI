using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace VendorPortal.Application.Models.v1.Request
{
    public class RequestDocumentRequest
    {
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string document_name { get; set; }
        public string reason { get; set; }
        public string email { get; set; }
        public bool is_require_signature { get; set; }
        public string lang { get; set; }

        public List<IFormFile>? files { get; set; }
    }
}