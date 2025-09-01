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
        public int count_po {get;set;}
        public int count_claim {get;set;}
    }
}