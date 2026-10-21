using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorPortal.Application.Models.v1.Request
{
    public class CheckSupplierRequest
    {
        public string email { get; set; } = string.Empty;
        public string supplier_id { get; set; } = string.Empty;
    }
}
