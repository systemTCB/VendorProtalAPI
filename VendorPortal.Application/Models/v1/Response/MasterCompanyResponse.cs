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
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Zip_Code { get; set; }
        public string Branch { get; set; }
        public string Tax_number { get; set; }
        public string Contract_first_name { get; set; }
        public string Contract_last_name { get; set; }
        public string Contract_Email { get; set; }
        public string Contract_Phone { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}