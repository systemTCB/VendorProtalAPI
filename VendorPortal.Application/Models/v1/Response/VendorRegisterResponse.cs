using System;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class VendorRegisterResponse : BaseResponse
    {
        public VendorRegisterData data { get; set; } = new VendorRegisterData();
    }

    public class VendorRegisterData
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string EndpointAPI { get; set; }
        public DateTime URLCreatedDate { get; set; }

        public string KB_API_PATH { get; set; }
        public string WOLF_API_PATH { get; set; }
        public string Model { get; set; }
        public DateTime PathCreatedDate { get; set; }
    }


}