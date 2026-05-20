using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;
namespace VendorPortal.Application.Services.v1
{
    public class BuyerRouteService : IBuyerRouteService
    {
        private readonly IWolfApproveRepository _repo;

        public BuyerRouteService(IWolfApproveRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<SP_GET_Buyer_Code>> GetActiveBuyerRoute(string buyerCode)
        {
            return await _repo.SP_GET_Buyer_Code(buyerCode);
        }

    }
}