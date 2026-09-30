using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class BlockStatusResponse : BaseResponse
    {
        public BlockStatusData data { get; set; }
    }

    public class CompanyQuestionnaire
    {
        public string id { get; set; }
        public string title { get; set; }
        public string company { get; set; }
        public string company_id { get; set; }
        public string wolf_company_id { get; set; }
    }

    public class BlockStatusData
    {
        public string id { get; set; }
        public int supplier_id { get; set; }
        public string company_questionnaire_id { get; set; }
        public string status { get; set; }
        public int submission_version { get; set; }
        public DateTime? submission_updated_at { get; set; }
        public bool pdpa_accepted { get; set; }
        public string remark { get; set; }
        public DateTime? blocked_at { get; set; }
        public DateTime? unblocked_at { get; set; }
        public string block_remark { get; set; }
        public string lang { get; set; }
        public bool is_unblock { get; set; }
        public Supplier supplier { get; set; }
        public CompanyQuestionnaire company_questionnaire { get; set; }
        public string type { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
   
}