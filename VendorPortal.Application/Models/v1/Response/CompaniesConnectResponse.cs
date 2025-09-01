using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesConnectResponse : BaseResponse
    {
        // Add properties and methods here
        public CompaniesConnectData Data { get; set; }
    }

    public class CompaniesConnectData
    {
        // Add properties and methods here
        public string status { get; set; }
    }
}