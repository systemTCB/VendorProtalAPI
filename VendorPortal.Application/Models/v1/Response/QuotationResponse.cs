using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class QuotationResponse : BaseResponse
    {
        public SyncQuotationData data { get; set; }
    }
}