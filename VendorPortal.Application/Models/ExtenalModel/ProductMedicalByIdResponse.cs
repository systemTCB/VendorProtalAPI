using System;
using System.Collections.Generic;
using System.Net.Mail;
using VendorPortal.Application.Models.Common;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.ExtenalModel
{

    public class ProductMedicalByIdResponse : BaseResponse
    {
        public ProductMedicalById data { get; set; }

    }

    public class ProductMedicalById
    {
        public string id { get; set; }
        public string sku { get; set; }
        public string name { get; set; }
        public string type_service_code { get; set; }
        public string category_code { get; set; }
        public string sub_category_code { get; set; }
        public string description { get; set; }
        public string unit_price { get; set; }
        public string fixed_price { get; set; }
        public DateTime until_date { get; set; }
        public string warranty { get; set; }
        public bool green_product { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Image image { get; set; }
        public Attachment attachment { get; set; }
        public Supplier supplier { get; set; }
    }

    public class Attachment
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class Image
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }
    public class Supplier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
        public string supplier_email { get; set; }
    }


}