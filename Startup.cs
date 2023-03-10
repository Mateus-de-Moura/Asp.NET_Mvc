using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTeste
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
            services.AddControllersWithViews();         

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/login";
                        options.LogoutPath = "/";
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/Forbidden/";
                        options.Cookie.Name = "auth_cookie";
                        
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Login/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

              app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                

                endpoints.MapControllerRoute(
                 name: "datails",
                 pattern: "Contas/{action}/{idconta?}",
                 defaults: new { controller = "Home", Action = "Details" });                 

                endpoints.MapControllerRoute(
                    name: "List",
                    pattern: "Contas/{action}/{mes?}",
                    defaults: new { controller = "Home", Action = "Index" });

                endpoints.MapControllerRoute(
                name: "listar",
                pattern: "Contas/{action}",
                defaults: new { controller = "List", Action = "Index" });

                endpoints.MapControllerRoute(
                 name: "Deletar",
                 pattern: "Contas/{action}/{id?}",
                 defaults: new { controller = "Home", Action = "Delete" });                

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
