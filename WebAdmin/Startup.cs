using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using MSDev.Work.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using MSDev.DB.Entities;
using Microsoft.AspNetCore.Identity;
using WebAdmin.Services;

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
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthorization(options => options.AddPolicy("admin", policy => policy.RequireRole("admin")));

            services.AddAuthentication(
                o =>
                {
                    o.DefaultChallengeScheme = "MSDevAdmin";
                    o.DefaultSignInScheme = "MSDevAdmin";
                    o.DefaultAuthenticateScheme = "MSDevAdmin";

                })
                .AddCookie("MSDevAdmin",options =>
                {
                    options.AccessDeniedPath = "/Auth/Forbidden/";
                    options.LoginPath = "/Auth/Login/";
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                });

            services.AddTransient<IEmailSender, EmailSender>();
            // Add framework services.

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

            app.UseStaticFiles();
            app.UseAuthentication();
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
