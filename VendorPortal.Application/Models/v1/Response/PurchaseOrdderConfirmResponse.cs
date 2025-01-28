using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderConfirmResponse : BaseResponse
    {
        public PurchaseOrderConfirmData Data { get; set; }
    }
    public class PurchaseOrderConfirmData
    {
        public string status { get; set; }
    }
}