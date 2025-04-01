using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;

namespace VendorPortal.Application.Services.SyncExternalData
{
    public class KubbossService : IHostedService, IDisposable
    {
        private readonly ILogger<KubbossService> _logger;
        private readonly DbContext _dbContext;
        
        private Timer? _timer = null;
        private int executionCount = 0;
        public KubbossService(ILogger<KubbossService> logger , DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing KubbossService.");
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("Timed Hosted Service is working.");
            var count = Interlocked.Increment(ref executionCount);
            _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);


            DateTime currentTime = DateTime.Now;
            var apilist = _dbContext.ExcuteStoreQueryListAsync<List<SYSConfigEndPoint>>("SP_GET_API_LIST").Result;
            _logger.LogInformation(JsonConvert.SerializeObject(apilist));

            // Add your logic here to call the Kubboss API and process the response
            // For example, you can use HttpClient to make a request to the Kubboss API
            // and handle the response accordingly.
            // var response = await _httpClient.GetAsync("https://api.kubboss.com/endpoint");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("KubbossService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}