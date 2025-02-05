using System;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class AuthenticationResponse : BaseResponse
    {
        public AuthenticationData Data { get; set; }
    }
    public class AuthenticationData
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}