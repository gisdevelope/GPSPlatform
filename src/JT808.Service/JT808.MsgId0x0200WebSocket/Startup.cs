using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JT808.MsgId0x0200WebSocket.Hubs;
using JT808.MsgId0x0200WebSocket.Services;
using JT808.MsgIdExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JT808.MsgId0x0200WebSocket
{
    /// <summary>
    /// 
    /// signalr����ע�룺https://docs.microsoft.com/en-us/aspnet/signalr/overview/advanced/dependency-injection
    /// signalr hubcontext��https://docs.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-2.1
    /// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/background-tasks-with-ihostedservice
    /// </summary>
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                  builder =>
                  {
                      builder.AllowAnyMethod()
                             .AllowAnyHeader()
                             .AllowAnyOrigin()
                             //.WithOrigins("http://localhost:55830")
                             .AllowCredentials();
            }));
            var host = Configuration.GetSection("KafkaOptions").GetValue<string>("bootstrap.servers");
            services.AddSingleton(new JT808_0x0200_Consumer(new Dictionary<string, object>
                        {
                            { "group.id", "JT808_0x0200_WebSocket_Alarm" },
                            { "enable.auto.commit", true },
                            { "bootstrap.servers", host }
                        }));
            services.AddSignalR();
            services.AddSingleton<IHostedService, MsgId0x0200Service>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
                routes.MapHub<AlarmHub>("/alarmHub");
            });
            app.UseMvc();
        }
    }
}
