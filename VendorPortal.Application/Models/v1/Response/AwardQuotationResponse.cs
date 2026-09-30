using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.v1.Response
{
    public class AwardQuotationResponse : BaseResponse
    {
        public AwardQuotationData data { get; set; }
    }

    public class AwardQuotationData
    {
        public List<AwardedQuotation> awarded { get; set; }
        public List<object> errors { get; set; }
    }
    public class AwardedQuotation
    {
        public QuotationAwarded quotation { get; set; }
    }

    public class QuotationAwarded
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
    }
}