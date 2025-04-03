using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Services.v1;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Infrastructure.Repositories.WolfApprove.v1;
using Serilog;
using Microsoft.Extensions.Logging;
using FluentValidation.AspNetCore;
using Namespace.Application.Models.v1.ValidationRequest;
using FluentValidation;
namespace VendorPortal.Infrastructure.IoC
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // //Application
            // services.AddSingleton<RabbitMQService>();
            services.AddSingleton<AppConfigHelper>();
            // services.AddSingleton<RabbitConfigHelper>();
            /* --------Service v1--------*/
            services.AddScoped<IWolfApproveService, WolfApproveService>();
            services.AddScoped<IAuthenticationService , AuthenticationService>();
            services.AddScoped<IMasterDataService, MasterDataService>();

            // services.AddSingleton<IApiWarmer, ApiWarmer>();
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            
            // add your validators here
            services.AddValidatorsFromAssemblyContaining<CancelQuotationValidation>();

            // services.AddSingleton<SensitiveDataHelper>();
            services.AddSingleton<DbContext>();
            //Infrastructure
            services.AddHttpClient();
            services.AddScoped<IWolfApproveRepository , WolfApproveRepository>();
            services.AddScoped<IAuthenticationRepository , AuthenticationRepository>();
            services.AddScoped<IMasterDataRepository, MasterDataRepository>();
            return services;
        }
    }
}