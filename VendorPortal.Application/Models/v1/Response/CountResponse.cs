using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CountResponse : BaseResponse
    {
        public CountData Data { get; set; }
    }

    public class CountData
    {
        public int Count_po {get;set;}
        public int Count_claim {get;set;}
    }
}