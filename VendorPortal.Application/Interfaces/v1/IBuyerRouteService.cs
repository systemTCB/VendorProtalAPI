using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IBuyerRouteService
    {
        Task<List<SP_GET_Buyer_Code>> GetActiveBuyerRoute(string buyerCode);
    }
}