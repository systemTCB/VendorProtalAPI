using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_PURCHASE_ORDER_DETAIL
    {
        
        public string Id { get; set; }
        public string Code { get; set; }
        public QuotationData Quotation { get; set; }
        public string Company_Nameame { get; set; }
        public string Description { get; set; }
        public string purchase_type_name { get; set; }
        public string Category_Namee { get; set; }
        public string Order_datete { get; set; }
        public string Require_Date { get; set; }
        public string Status { get; set; }
        public CompanyAddressData Company_address { get; set; }
        public CompanyContractData Company_contract { get; set; }
        public string Sub_totoal { get; set; }
        public string Discount { get; set; }
        public string Total_amount { get; set; }
        public string Vat_amount { get; set; }
        public string Net_amount { get; set; }
        public List<Line> Lines { get; set; }
        public List<Document> Documents { get; set; }
        public string Ship_to { get; set; }
        public string Payment_condition { get; set; }
        public string Remark { get; set; }
        public string Cancel_reason { get; set; }
        public string Cancel_description { get; set; }

        public class QuotationData
        {
            public string Code { get; set; }
        }

        public class CompanyAddressData
        {
            public string Address_1 { get; set; }
            public string Address_2 { get; set; }
            public string Sub_District { get; set; }
            public string District { get; set; }
            public string Province { get; set; }
            public string Zip_code { get; set; }
            public string Branch { get; set; }
            public string Tax_number { get; set; }
        }

        public class CompanyContractData
        {
            public string First_name { get; set; }
            public string Last_name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        public class Line
        {
            public string Id { get; set; }
            public string Line_number { get; set; }
            public string Item_code { get; set; }
            public string Item_name { get; set; }
            public string Uom_name { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string Unit_price { get; set; }
            public string Vat_rate { get; set; }
            public string Vat_amount { get; set; }
            public string Total_amount { get; set; }
        }

        public class Document
        {
            public string Name { get; set; }
            public string File_url { get; set; }
        }
    }
}