using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class ClaimConfirmResponse : BaseResponse
    {
        public ClaimConfirmData Data { get; set; }
    }

    public class ClaimConfirmData
    {
        public string status { get; set; }
        public int claim_id { get; set; }
    }
}