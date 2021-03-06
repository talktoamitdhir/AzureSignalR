﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRServer.Hubs;
using Swashbuckle.AspNetCore.Swagger;

namespace SignalRServer
{
    public class Startup
    {
        private const string connectionString = "Endpoint=https://gabsignal.service.signalr.net;AccessKey=xDxsLSG/exYCW4DM0p7gIzqV1MLYPsDN+t7tqtywwUc=;Version=1.0;";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            //services.AddSignalR();

            services.AddSignalR().AddAzureSignalR(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                );

            //app.UseSignalR(routes => routes.MapHub<FlightSimulationHub>("/flightsimulationhub"));

            //app.UseFileServer();

            app.UseAzureSignalR(routes => routes.MapHub<FlightSimulationHub>("/flightsimulationhub"));

            app.UseMvc();
        }
    }
}
