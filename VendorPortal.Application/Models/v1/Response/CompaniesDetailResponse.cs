using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesDetailResponse : BaseResponse
    {
        
        public CompaniesDetailData Data { get; set; }
    }

    public class CompaniesDetailData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string request_date { get; set; }
        public string request_status { get; set; }
        public string website { get; set; }
        public CompanyAddress company_address { get; set; }
        public CompanyContract company_contract { get; set; }
        
    }
}