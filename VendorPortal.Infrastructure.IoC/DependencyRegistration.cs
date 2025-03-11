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
        //     services.AddFluentValidation(options =>
        //    {
        //        // v1
        //        options.RegisterValidatorsFromAssemblyContaining<GetTxnInitiateRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<InsertTxnConfirmRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<InsertTxnInitialRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateTransactionPendingRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateAccountRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateTransactionInformationRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateHoldTransactionRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<InsertTransactionHistoryHoldRequestValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateLockTxnValidator>();
        //        options.RegisterValidatorsFromAssemblyContaining<UpdateTransactionStatusRequestValidator>();
        //        // v2
        //        options.RegisterValidatorsFromAssemblyContaining<InsertTxnInitialSapiRequestValidator>();
        //    });

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