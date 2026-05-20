using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class VendorRegisterRequest
    {
        public string supplier_id { get; set; }
        public string buyerCode { get; set; }
        public string docNo { get; set; }
        //public string buyer_code { get; set; }
    }
}