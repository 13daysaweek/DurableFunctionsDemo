using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsDemo
{
    public static class SalesAggregateStarter
    {
        [FunctionName(("SalesAggregateStarter"))]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger logger)
        {
            var instanceId = await starter.StartNewAsync("SalesAggregateOrchestrator");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}