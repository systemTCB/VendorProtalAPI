using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesDetailResponse : BaseResponse
    {
        // Add properties and methods specific to CompaniesDetailResponse here
        public CompaniesDetailData Data { get; set; }
    }

    public class CompaniesDetailData
    {
        // Add properties and methods specific to CompaniesDetailData here
    }
}