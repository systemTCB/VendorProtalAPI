using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VendorPortal.Domain.Interfaces.SyncExternalData;
using VendorPortal.Logging;

namespace VendorPortal.Infrastructure.Repositories.Kubboss.v1
{
    public class KubbossRepository : IKubbossRepository
    {
        public async Task<bool> SyncVendorFromKubboss(DateTime dateTime)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://api.kubboss.com/"); // Replace with actual Kubboss API base URL
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                //fixed Model to send to Kubboss API
                // wait for moove to DataBase
                var jModel = new
                {
                    last_updated = dateTime.ToString("yyyy-MM-ddTHH:mm:ss")
                };

                var contnt = new StringContent(JsonConvert.SerializeObject(jModel));
                var result = await client.PostAsync("sync/vendor", contnt);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    // Process the response content as needed
                    return true; // Return true if the sync was successful
                }
                else
                {
                    Logger.LogError(new Exception($"Failed to sync vendor from Kubboss. Status code: {result.StatusCode}"), "SyncVendorFromKubboss");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SyncVendorFromKubboss");
            }
            finally
            {
                // Cleanup or finalization code (if needed)
            }

            return false;
        }
    }
}