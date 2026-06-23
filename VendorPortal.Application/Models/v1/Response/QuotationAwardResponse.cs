using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.v1.Response
{
    public class QuotationAwardResponse : BaseResponse
    {
        public QuotationAwardData data { get; set; }
    }

    public class QuotationAwardData
    {
        public QuotationAward quotation { get; set; }
        public PurchaseOrder purchase_order { get; set; }
    }

    public class QuotationAward
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
    }

    public class PurchaseOrder
    {
        public string id { get; set; }
        public string purchase_order_number { get; set; }
    }

}