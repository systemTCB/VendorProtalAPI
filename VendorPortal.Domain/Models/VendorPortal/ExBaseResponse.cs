namespace VendorPortal.Domain.Models.KubBoss
{
    public class ExBaseResponseStatus
    {
        public ExStatus status { get; set; }

    }
    public class ExStatus
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}