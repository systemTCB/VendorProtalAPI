using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_COMPANIES_DETAIL
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Request_date { get; set; }
        public string Request_status { get; set; }
        public string Website { get; set; }

        public CompanyAddressData Company_address { get; set; }
        public CompanyContactData Company_contact { get; set; }

        public class CompanyAddressData
        {
            public string Address_1 { get; set; }
            public string Address_2 { get; set; }
            public string Sub_district { get; set; }
            public string District { get; set; }
            public string Province { get; set; }
            public string Zip_code { get; set; }
            public string Branch { get; set; }
            public string Tax_id { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        public class CompanyContactData
        {
            public string First_name { get; set; }
            public string Last_name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
    }
}