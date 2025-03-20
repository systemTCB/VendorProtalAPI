using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class MasterCompanyResponse : BaseResponse
    {
        public List<MasterCompanyData> Data { get; set; }
    }

    public class MasterCompanyData
    {
        // Add properties and methods as needed
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string zip_code { get; set; }
        public string branch { get; set; }
        public string tax_number { get; set; }
        public string contract_first_name { get; set; }
        public string contract_last_name { get; set; }
        public string contract_email { get; set; }
        public string contract_phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}