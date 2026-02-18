using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class SupplierQuestionnaireRegistration
    {
        public string id { get; set; }
        public int supplier_id { get; set; }
        public string company_questionnaire_id { get; set; }
        public string status { get; set; }
        public bool pdpa_accepted { get; set; }
        public int submission_version { get; set; }
        public string remark { get; set; }
        public int is_submitted { get; set; }
        public string submission_updated_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public CompanyQuestionnaireSupplier company_questionnaire { get; set; }
        public string details_url { get; set; }
        public JObject answer_questionnaire { get; set; }
    }

    public class SupplierRegistrationResponse : BaseResponse
    {
        public List<SupplierQuestionnaireRegistration> data { get; set; }
    }


}