using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.ExtenalModel
{
    public class Link
    {
        public string url { get; set; }
        public string label { get; set; }
        public bool active { get; set; }
        public bool disabled { get; set; }
    }

    public class ProductMedicalResponse
    {
        public int total { get; set; }
        public int per_page { get; set; }
        public int current_page { get; set; }
        public int last_page { get; set; }
        public string first_page_url { get; set; }
        public string last_page_url { get; set; }
        public string next_page_url { get; set; }
        public string prev_page_url { get; set; }
        public string path { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public List<ProductMedicalData> data { get; set; } = new List<ProductMedicalData>();
        public List<Link> links { get; set; }
        public Status status { get; set; }
    }

    public class ProductMedicalData
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
    }
    
}