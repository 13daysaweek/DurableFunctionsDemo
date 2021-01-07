using System.Collections;
using System.Collections.Generic;
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
            IList<Task<IEnumerable<GetSalesDataOutput>>> tasks = new List<Task<IEnumerable<GetSalesDataOutput>>>();
            
            var inputs = JsonConvert.DeserializeObject<SalesAggregateInput>(regionsAndDivisions);

            foreach (var region in inputs.Regions)
            {
                foreach (var division in inputs.Divisions)
                {
                    var input = new GetSalesDataInput
                    {
                        Division = division,
                        Region = region
                    };
                    
                    tasks.Add(context.CallActivityAsync<IEnumerable<GetSalesDataOutput>>("GetSalesData", input));        
                }
            }
            
            var results = await Task.WhenAll(tasks);
            
            return inputs;
        }
    }
}