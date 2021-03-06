﻿using System;
using CakeExchange.Common.Binders;
using CakeExchange.Common.Settings;
using CakeExchange.Data;
using CakeExchange.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CakeExchange
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ExchangeContext>(options => options.UseSqlServer(connection));

            services.AddHangfire(config => config.UseSqlServerStorage(connection));

            services.AddSingleton(new MakeTransactionService(Configuration));

            services.AddSingleton(Configuration);

            services.AddMvc(config => config.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider()));

            services.Configure<BackgroundJobsSettings>(Configuration.GetSection("AppSettings:BackgroundJobs"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            GlobalConfiguration.Configuration.UseActivator(new ServiceProviderActivator(serviceProvider));

            app.UseHangfireServer();

            RecurringJob.AddOrUpdate(() => serviceProvider.GetService<MakeTransactionService>().ProcessTransactions(),
                Cron.Minutely);

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