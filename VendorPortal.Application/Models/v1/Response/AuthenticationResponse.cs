using System;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class AuthenticationResponse : BaseResponse
    {
        public AuthenticationData data { get; set; }
    }
    public class AuthenticationData
    {
        public string token { get; set; }
        public DateTime? expiration { get; set; }
    }
}