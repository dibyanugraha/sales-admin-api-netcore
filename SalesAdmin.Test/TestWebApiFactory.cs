using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesAdmin.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SalesAdmin.Test
{
    public class TestWebApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                var appType = typeof(TStartup);
                var appPath = "";

                configurationBuilder.AddJsonFile($"{appPath}", optional: true, reloadOnChange: true);
                configurationBuilder.AddEnvironmentVariables();

            });

            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<UserContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDbInMemory");
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
