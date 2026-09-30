using System.Collections.Generic;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Models.v1.Request
{
    public class awardQuotationRequest
    {
        public List<AwardQuotationID> quotations { get; set; }
        public string lang { get; set; }
    }
    public class AwardQuotationID
    {
        public string quotation_id { get; set; }
    }
}