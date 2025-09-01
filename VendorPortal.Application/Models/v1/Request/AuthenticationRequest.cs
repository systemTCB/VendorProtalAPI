namespace VendorPortal.Application.Models.v1.Request
{
    public class AuthenticationRequest
    {
        public string token { get; set; }
        public string channel { get; set; }
    }
}