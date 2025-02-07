using Microsoft.Extensions.Configuration;

namespace VendorPortal.Application.Models.Common
{
    public class KubbossCommonModel
    {
        // Add properties and methods here
        public class CompanyAddress
        {
            public string Address_1 { get; set; }
            public string Address_2 { get; set; }
            public string Sub_District { get; set; }
            public string District { get; set; }
            public string Province { get; set; }
            public string Zip_Code { get; set; }
            public string Branch { get; set; }
            public string Tax_Number { get; set; }
        }
        public class CompanyContract
        {
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
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
            public string FileUrl { get; set; }
        }
        public class ClaimPurchaseOrderData
        {
            public string Code { get; set; }
            public string Purchase_date { get; set; }
        }
    }

    public class ResponseBasePage : BaseResponse
    {
        public string Total { get; set; }
        public string Per_page { get; set; }
        public string Current_page { get; set; }
        public string Last_page { get; set; }
        public string First_page_url { get; set; }
        public string Last_page_url { get; set; }
        public string Next_page_url { get; set; }
        public string Prev_page_url { get; set; }
        public string Path { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}