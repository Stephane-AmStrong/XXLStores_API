using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Persistence.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Persistence
{
    public static class SharedServiceExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            //var emailConfig = _config
            //    .GetSection("MailSettings")
            //    .Get<MailSettings>();
            //services.AddSingleton(emailConfig);

            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
