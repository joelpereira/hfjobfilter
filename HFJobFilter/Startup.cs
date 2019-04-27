using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HFJobFilter
{
	public class Startup
	{
		public static readonly HttpClient httpClient = new HttpClient();
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHangfire(config => config
				.UseSqlServerStorage(Configuration.GetConnectionString("HangfireDBConnection"))
				.UseFilter(new TypeFilterAttribute(typeof(LogToDbAttribute)))
			);

			// just sending something via DI, but this would really be the unit of work
			services.AddSingleton<HttpClient>(httpClient);
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


		void AddJob()
		{
			string jobId = BackgroundJob.Enqueue(() => SomeJob.SomeJobMethod());
		}
	}
}
