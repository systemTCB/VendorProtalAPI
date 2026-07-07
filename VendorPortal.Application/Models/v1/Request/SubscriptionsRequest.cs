namespace VendorPortal.Application.Models.v1.Request
{
    public class SubscriptionsRequest
    {
        public string email { get; set; }
        public string company_id { get; set; }
        public string status { get; set; }
        public string remark { get; set; }

    }

    public class SubscriptionsUpdateRequest
    {
        public string status { get; set; }
        public string remark { get; set; }
    }
}