using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{

    public class UpdatePORequest
    {
        public string purchase_order_number { get; set; }
        public List<LineUpdatePO> lines { get; set; }
        public int? discount { get; set; }
        public bool include_vat { get; set; }
        public int? vat_rate { get; set; }
        public int? vat_amount { get; set; }
        public bool include_withholding_tax { get; set; }
        public int? wht_rate { get; set; }
        public int? wht_amount { get; set; }
        public bool auto_cal_vat { get; set; }
        public bool auto_cal_wht { get; set; }
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();

    }

    public class LineUpdatePO
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public int? quantity { get; set; }
        public int? unit_price { get; set; }
        public int? discount { get; set; }
        public int? vat_rate { get; set; }
        public int? wht_rate { get; set; }
    }

}