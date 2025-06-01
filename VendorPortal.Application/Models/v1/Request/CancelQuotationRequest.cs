namespace VendorPortal.Application.Models.v1.Request
{
    public class PutQuotationRequest
    {
        public string quo_number { get; set; } = string.Empty;
        public string quo_id { get; set; } = string.Empty;
        public string reason { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}