using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string request_date { get; set; }
        public string request_status { get; set; }
        public CompanyContract company_contact { get; set; }   
    }
}