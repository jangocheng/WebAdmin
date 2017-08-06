using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MSDev.DB;
using MSDev.Task.Helpers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WebAdmin
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _hostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(Configuration);
            // Add framework services.
            services.AddMvc()
                   .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               );
            services.AddAuthorization(options => options.AddPolicy("admin", policy => policy.RequireRole("admin")));

            services.AddDbContext<AppDbContext>(
                option => option.UseSqlServer(
                    Configuration.GetConnectionString("OnlineConnection"),
                    b =>
                    {
                        b.MigrationsAssembly("WebAdmin");
                        b.EnableRetryOnFailure();
                        
                    }
                )
            );

            services.AddScoped(typeof(ApiHelper));
            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var cookieOption = new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "MSDevAdmin",
                LoginPath = new PathString("/Auth/Login/"),
                AccessDeniedPath = new PathString("/Auth/Forbidden/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ExpireTimeSpan = TimeSpan.FromDays(1)
            };
            app.UseCookieAuthentication(cookieOption);
            app.UseStaticFiles();
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
