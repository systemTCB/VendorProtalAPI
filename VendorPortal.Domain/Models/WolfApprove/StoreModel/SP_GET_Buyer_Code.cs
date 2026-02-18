using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_Buyer_Code
    {
        public int RouteId { get; set; }
        public string BuyerCode { get; set; }
        public string CompanyName { get; set; }
        public string BaseUrl { get; set; }
        public string Path { get; set; }
        public string HttpMethod { get; set; }
        public string AuthType { get; set; }
        public string AuthUrlPath { get; set; }
        public string ContentType { get; set; }
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}