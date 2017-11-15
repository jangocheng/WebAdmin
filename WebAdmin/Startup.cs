using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MSDev.DB;
using Newtonsoft.Json;
using WebAdmin.Services;
using MSDev.DB.Entities;
using Microsoft.AspNetCore.Identity;

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

            var communityConnection = Configuration.GetConnectionString("CommunityConnection");
            services.AddEntityFrameworkNpgsql().AddDbContext<CommunityContext>(
                options => options.UseNpgsql(communityConnection,
                b =>
                    {
                        b.MigrationsAssembly("WebAdmin");
                        b.EnableRetryOnFailure();
                    }
                )
            );

            services.AddIdentity<User, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                options.User.RequireUniqueEmail = true;
            });


            services.AddAuthorization(options => options.AddPolicy("admin", policy => policy.RequireRole("admin")));

            services.AddAuthentication(
                o =>
                {
                    o.DefaultChallengeScheme = "MSDevAdmin";
                    o.DefaultSignInScheme = "MSDevAdmin";
                    o.DefaultAuthenticateScheme = "MSDevAdmin";

                })
                .AddCookie("MSDevAdmin", options =>
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
