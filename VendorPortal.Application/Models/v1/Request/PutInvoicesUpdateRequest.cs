namespace VendorPortal.Application.Models.v1.Request
{
    public class PutInvoicesUpdateRequest
    {
        public string id { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string lang { get; set; }
    }
}