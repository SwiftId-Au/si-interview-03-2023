using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Si.Interview.Web.Api.Models;
using Si.Interview.Web.Api.Services;
using System;
using System.Net.Http;

namespace Si.Interview.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddHttpClient(nameof(AsxListedCompaniesService))
                .AddTransientHttpErrorPolicy(policy =>
                    policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30)));

            services.AddSwaggerGen();
            services.AddMemoryCache();
            services.Configure<AsxSettings>(Configuration.GetSection(nameof(AsxSettings)).Bind);
            services.AddScoped<IAsxListedCompaniesService, AsxListedCompaniesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
