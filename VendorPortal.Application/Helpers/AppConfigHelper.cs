using Microsoft.Extensions.Configuration;

namespace VendorPortal.Application.Helpers
{
    public class AppConfigHelper
    {
        private IConfiguration Configuration { get; set; }

        public AppConfigHelper(IConfiguration Configuration) => this.Configuration = Configuration;

        public string GetConfiguration(string key) => Configuration[key];
        public string GetResponseMessage(string key) => Configuration["ResponseMessage:" + key];
    }
}