using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQUpdateResponse : BaseResponse
    {
        public RFQUpdateData data { get; set; }
    }
    public class RFQUpdateData
    {
        public string update_response { get; set; }
        public string rfq_id { get; set; }
    }
}