using System.Collections.Generic;
using Microsoft.Identity.Client;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class MasterCompanySyncUpdateResponse : BaseResponse
    {
        public List<MasterCompanyData> data { get; set; } = new List<MasterCompanyData>();
    }
}