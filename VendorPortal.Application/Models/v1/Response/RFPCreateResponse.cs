using System;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFPCreateResponse : BaseResponse
    {
        public RFPCreateData data { get; set; } = new RFPCreateData();
    }

    public class RFPCreateData
    {
        public string documentNo { get; set; } = string.Empty;
    }
}