using System;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class ActionResultResponse
    {
        public bool success { get; set; }
        public string message { get; set; }

        public static ActionResultResponse Success(string message = "Success")
                  => new ActionResultResponse { success = true, message = message };

        public static ActionResultResponse Fail(string message)
            => new ActionResultResponse { success = false, message = message };

    }

}

