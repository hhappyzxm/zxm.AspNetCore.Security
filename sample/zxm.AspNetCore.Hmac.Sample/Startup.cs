using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using zxm.AspNetCore.Authentication.Hmac;
using zxm.AspNetCore.WebApi.Result.Abstractions;
using zxm.AspNetCore.WebApi.ResultExtenstion;
using Microsoft.AspNetCore.Http.Internal;
using zxm.AspNetCore.Authentication.Hmac.Identity;
using zxm.AspNetCore.Authorization.Hmac;

namespace zxm.AspNetCore.Hmac.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User",
                      policy => policy.Requirements.Add(new UserRequirement()));
            });

            services.AddMvc((options =>
            {
                options.Filters.Add(new ResultActionFilterAttributer());
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseHmacAuthentication(new HmacOptions
            {
                TryGetClientSecret = clientId =>
                {
                    if (clientId == "123456")
                    {
                        return "123456";
                    }
                    else
                    {
                        return string.Empty;
                    }
                },
                VerifyUserAccessToken = s => s == "abc" ? new HmacIdentity("tester") : null,
                RequestTimeInterval = 30
            });
            
            app.UseMvc();
        }
    }
}
