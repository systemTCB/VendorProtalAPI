using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IBuyerApiService
    {
        Task<bool> SendToBuyer(SP_GET_Buyer_Code route, string payloadJson);
    }
}