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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Request_date { get; set; }
        public string Request_status { get; set; }
        public string Website { get; set; }
        public CompanyAddress Company_address { get; set; }
        public CompanyContract Company_contract { get; set; }
        
    }
}