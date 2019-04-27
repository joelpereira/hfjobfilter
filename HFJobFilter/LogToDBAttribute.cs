using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HFJobFilter
{
	//public class LogToDbAttribute : JobFilterAttribute,
	public class LogToDbAttribute : TypeFilterAttribute
	{
		private readonly HttpClient _httpClient;
		public LogToDbAttribute(HttpClient client) : base(typeof(LogToDbImpl))
		{
			_httpClient = client;
		}



		private class LogToDbImpl : JobFilterAttribute,
	IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
		{
			private readonly HttpClient _httpClient;

			public LogToDbImpl(HttpClient client)
			{
				_httpClient = client;
			}

			public void OnCreating(CreatingContext context)
			{
				// log to secondary database..
			}

			public void OnCreated(CreatedContext context)
			{
				string name = context.BackgroundJob.Job.Method.Name;
			}

			public void OnPerforming(PerformingContext context)
			{
			}

			public void OnPerformed(PerformedContext context)
			{
			}

			public void OnStateElection(ElectStateContext context)
			{
			}

			public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
			{
			}

			public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
			{
			}
		}
	}
}
