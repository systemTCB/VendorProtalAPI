using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesResponse : ResponseBasePage
    {
        public List<CompainesData> Data { get; set; }
        // Add properties and methods here
    }
    public class CompainesData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Request_date { get; set; }
        public string Request_status { get; set; }
        public CompanyContract Company_contact { get; set; }   
    }
}