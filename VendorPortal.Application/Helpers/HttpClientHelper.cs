namespace VendorPortal.Application.Helpers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public static class HttpClientHelper
    {
        public static HttpClient CreateClient(string baseUrl, string token = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty", nameof(baseUrl));

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
