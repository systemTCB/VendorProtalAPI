using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class QuestionnaireUpdateRequest
    {
        public string supplier_answer_id { get; set; }
        public string form_no { get; set; }
        public string vendor_code { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string lang { get; set; }
    }
}