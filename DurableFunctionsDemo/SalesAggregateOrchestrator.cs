using System.IO;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace DurableFunctionsDemo
{
    public static class SalesAggregateOrchestrator
    {
        [FunctionName("SalesAggregateOrchestrator")]
        public static async Task<SalesAggregateInput> RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, 
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            [Blob("input/cf-input.json", FileAccess.Read, Connection = "input-storage-connection-string")] string regionsAndDivisions)
        {
            var inputs = await Task.FromResult(JsonConvert.DeserializeObject<SalesAggregateInput>(regionsAndDivisions));

            return inputs;
        }
    }
}