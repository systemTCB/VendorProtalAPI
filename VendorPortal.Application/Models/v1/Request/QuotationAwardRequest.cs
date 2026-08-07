using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class QuotationAwardRequest
    {
        public string quotation_id { get; set; }
        public string purchase_order_number { get; set; }
        public string order_date { get; set; }
        public string require_date { get; set; }
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();
    }
}