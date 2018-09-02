using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using soverance.com.Models;

namespace soverance.com
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

            services.Configure<SecretConfig>(Configuration.GetSection("SecretConfig"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Database")));
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
                        
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {

            //routes.MapRoute(
            //    name: "default",
            //    template: "{action}/{id?}",
            //    defaults: new { controller = "Home", action = "Index" });

            //routes.MapRoute(
            //    name: "Team",
            //    template: "{area:exists}/{action}/{id?}",
            //    defaults: new { controller = "Team", action = "Index" });

            //routes.MapRoute(
            //    name: "Post",
            //    template: "blog/{slug?}",
            //    defaults: new { controller = "Blog", action = "ViewPost" });

            //routes.MapRoute(
            //    name: "Blog",
            //    template: "blog/{action}/{id?}",
            //    defaults: new { controller = "Blog", action = "Index" });

            //routes.MapRoute(
            //    name: "Ethereal",
            //    template: "{area:exists}/{action}/{id?}",
            //    defaults: new { controller = "Ethereal", action = "Index" });
            });
        }
    }
}
