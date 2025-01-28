using Microsoft.SqlServer.Server;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderDetailResponse : BaseResponse
    {
        // Add properties and methods here
        public PurchaseOrderDetailData Data { get; set; }
    }

    public class PurchaseOrderDetailData
    {
        // Add properties and methods here
    }
}