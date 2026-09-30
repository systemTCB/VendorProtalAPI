namespace VendorPortal.Application.Models.v1.Request
{
    public class BlockStatusRequest
    {
        public string supplierAnswerID { get; set; }
        public string status { get; set; }
        public string remark { get; set; }

    }
    public class BlockStatusByEmailRequest
    {
        public string email { get; set; }
        public string company_id { get; set; }
        public string status { get; set; }
        public string remark { get; set; }

    }

}