using System.Net.Http;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;

namespace HFJobFilter
{
    //public class LogToDbAttribute : JobFilterAttribute,
    public class LogToDbAttribute : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private HttpClient _httpClient;
        private readonly ILogger<LogToDbAttribute> _logger;

        public LogToDbAttribute(HfHttpClient hfHttpClient, ILogger<LogToDbAttribute> logger)
        {
            _httpClient = hfHttpClient.Client;
            _logger = logger;
        }

        public void OnCreating(CreatingContext context)
        {
            // log to secondary database..
        }

        public void OnCreated(CreatedContext context)
        {
            string name = context.BackgroundJob.Job.Method.Name;

            _logger.LogInformation(name);
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