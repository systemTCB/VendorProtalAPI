using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Logging;
namespace VendorPortal.Application.Services.v1
{
    public class BuyerApiService : IBuyerApiService
    {
        public IVendorPortalRepository _vendotPortalRepository;
        public IWolfApproveRepository _wolfApproveRepository;
        private readonly AppConfigHelper _appConfigHelper;
        private readonly IBuyerRouteService _routeService;
        public BuyerApiService(IHttpContextAccessor httpContext, AppConfigHelper appConfigHelper, IBuyerRouteService routeService)
        {
            _appConfigHelper = appConfigHelper;
            _routeService = routeService;
        }
        public async Task<bool> SendToBuyer(SP_GET_Buyer_Code route, string payloadJson)
        {
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(route.BaseUrl)
                };

                if (route.AuthType?.ToUpper() == "BEARER")
                {
                    var token = await GetBearerToken(route);
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var content = new StringContent(payloadJson, Encoding.UTF8, route.ContentType ?? "application/json");
                HttpResponseMessage response = route.HttpMethod.ToUpper() switch
                {
                    "POST" => await client.PostAsync(route.Path, content),
                    "PUT" => await client.PutAsync(route.Path, content),
                    _ =>
                    throw new NotSupportedException($"HTTP method {route.HttpMethod} not supported")
                };

                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {

                    Logger.LogInfo("SendToBuyer",
                        $"FAILED | Buyer:{route.BuyerCode} | URL:{route.BaseUrl}{route.Path} | " +
                        $"Status:{(int)response.StatusCode} {response.StatusCode} | Response:{responseBody}");
                }
                else
                {
                    Logger.LogInfo("SendToBuyer",
                        $"SUCCESS | Buyer:{route.BuyerCode} | Status:{(int)response.StatusCode} | Response:{responseBody}");
                }

                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "SendToBuyer");
            }

            return false;
        }
        private async Task<string> GetBearerToken(SP_GET_Buyer_Code route)
        {

            var cacheKey = $"TOKEN_{route.BuyerCode}";

            if (_cache.TryGetValue(cacheKey, out string token))
                return token;

            using var client = new HttpClient
            {
                BaseAddress = new Uri(route.BaseUrl + route.AuthUrlPath)
            };

            object body;

            if (!string.IsNullOrEmpty(route.AuthBodyJson))
            {
                body = JObject.Parse(route.AuthBodyJson);
            }
            else
            {
                body = new
                {
                    username = route.AuthUsername,
                    password = route.AuthPassword
                };
            }

            var response = await client.PostAsync(
                route.AuthUrlPath,
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get token");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            token = json.SelectToken(route.TokenJsonPath ?? "$.Result")?.ToString();

            var expire = TimeSpan.FromMinutes(route.TokenExpireMinutes ?? 30);
            _cache.Set(cacheKey, token, expire);

            return token;
        }

        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());


    }
}