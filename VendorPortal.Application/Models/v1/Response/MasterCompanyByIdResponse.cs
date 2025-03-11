using Microsoft.Extensions.Logging;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class MasterCompanyByIdResponse : BaseResponse
    {
        public MasterCompanyByIdData Data { get; set; }
    }

    public class MasterCompanyByIdData
    {
        public int Company_id { get; set; }
        public string Company_name { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Zip_Code { get; set; }
        public string Branch { get; set; }
        public string Tax_number { get; set; }
        public string Contract_first_name { get; set; }
        public string Contract_last_name { get; set; }
        public string Contract_Email { get; set; }
        public string Contract_Phone { get; set; }
    }
}