using FFBStats.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YahooFantasyWrapper.Client;
using YahooFantasyWrapper.Configuration;

namespace FFBStats.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureOAuth(services);
            ConfigureIoC(services);
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureOAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    //handled by authentication controller, middleware takes over requests to these urls
                    options.LoginPath = "/login"; 
                    options.LogoutPath = "/signout";
                })
                .AddYahoo(options =>
                {
                    options.ClientId = Configuration["YahooClientID"];
                    options.ClientSecret = Configuration["YahooClientSecret"];
                    options.SaveTokens = true;
                });
        }

        private void ConfigureIoC(IServiceCollection services)
        {
            services.AddSingleton<IYahooFFBClient, YahooFFBClient>();
            services.AddSingleton<IYahooWebRequestComposer, YahooWebRequestComposer>();
            services.Configure<YahooConfiguration>(Configuration.GetSection("YahooConfiguration"));
            services.AddSingleton<IYahooFantasyClient, YahooFantasyClient>();
        }
    }
}
