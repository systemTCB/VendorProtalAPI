using System;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RegisterResponse : BaseResponse
    {
        public object data { get; set; }
    }
}