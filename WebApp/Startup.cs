﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yuffie.WebApp.Models;

namespace WebApp
{
    public static class YuffieApp
    {
        public static void SetConfiguration()
        {
            var configuration = Builder.Build();
            Config = configuration.Get<YuffieConfiguration>();
        }
        public static YuffieConfiguration Config {get;private set;}
        public static IConfigurationBuilder Builder {get; set;}
    }
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            YuffieApp.Builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("yuffieconfig.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = YuffieApp.Builder.Build();

            YuffieApp.SetConfiguration();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services
            //     .AddDbContext<Yuffie.WebApp.Models.AppContext>(options => options.UseSqlite(Configuration["ConnectionStrings:DefaultConnection"]));
            // /Configuration.GetConnectionString("DefaultConnection")
            services.AddMvc();
            services.AddDbContext<Yuffie.WebApp.Models.AppContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
