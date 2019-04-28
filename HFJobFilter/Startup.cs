using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HFJobFilter
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // just sending something via DI, but this would really be the unit of work
            services.AddHttpClient<HfHttpClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:44303");

                // you can set other options for HttpClient as well, such as
                //client.DefaultRequestHeaders;
                //client.Timeout
                //...
            });

            // register the LogToDbAttribute
            services.AddSingleton<LogToDbAttribute>();

            // build the service provider to inject the dependencies in LogDbAttribute
            var serviceProvider = services.BuildServiceProvider();

            services.AddHangfire(config => config
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireDBConnection"))
                .UseFilter(serviceProvider.GetRequiredService<LogToDbAttribute>())
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();

            AddJob();
        }

        private void AddJob()
        {
            string jobId = BackgroundJob.Enqueue(() => SomeJob.SomeJobMethod());
        }
    }
}