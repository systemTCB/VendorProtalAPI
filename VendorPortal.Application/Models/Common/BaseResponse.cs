namespace VendorPortal.Application.Models.Common
{
    public class BaseResponse
    {
        public Status Status { get; set; }

    }
    public class Status
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}