using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ArduinoLightswitcherGateway;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace LightSwitcherWeb
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
            var gatewayType = Configuration["ArduinoGateway"];
            _logger.Information("Hello! It's a startup. Configuring gateway as: {gateway}", gatewayType);
            services.AddSingleton<ILightSwitcherGateway>(
                LightSwitcherGatewayFactory.CreateLightswitcherGateway(gatewayType));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy", builder => 
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            }); 
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowAllPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private readonly ILogger _logger = Log.ForContext<Startup>(); 
    }
}
