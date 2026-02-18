using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_COMPANY_API
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string EndpointAPI { get; set; }
        public DateTime URLCreatedDate { get; set; }
        public string KB_API_PATH { get; set; }
        public string ApiPath { get; set; }
        public string HttpMethod { get; set; }
        public string Model { get; set; }
        public DateTime PathCreatedDate { get; set; }

    }
}