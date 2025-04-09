using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;

namespace VendorPortal.Application.Services.SyncExternalData
{
    public class KubbossService : IKubbossService
    {
        public KubbossService()
        {

        }
        public Task<bool> SyncVendorFromKubboss(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

    }
}