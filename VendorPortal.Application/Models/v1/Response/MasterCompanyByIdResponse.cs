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
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string zip_code { get; set; }
        public string branch { get; set; }
        public string tax_number { get; set; }
        public string contract_first_name { get; set; }
        public string contract_last_name { get; set; }
        public string contract_email { get; set; }
        public string contract_phone { get; set; }
    }
}