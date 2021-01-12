using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace DurableFunctionsDemo
{
    public static class Manager
    {
        [FunctionName("Manager")]
        public static async Task RunManager([ActivityTrigger] IDurableOrchestrationContext context, [Blob("input/cf-input.json", FileAccess.Read, Connection = "input-storage-connection-string")] string regionsAndDivisions)
        {
            var tasks = new List<Task<IEnumerable<GetSalesDataOutput>>>();
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

            var processDataResults = new List<Task>();
            var runId = $"{DateTime.UtcNow:MMddyyyy-HHmmss}";
            // Results is an IEnumerable<IEnumerable<T>>, so flatten it with SelectMany
            foreach (var result in results.SelectMany(x => x))
            {
                var processDataInput = new ProcessDataInput
                {
                    CustomerId = result.CustomerId,
                    Division = result.Division,
                    Region = result.Region,
                    RunIdentifier = runId,
                    MonthlySalesTotal = result.MonthlySalesTotal
                };
                processDataResults.Add(context.CallActivityAsync("ProcessData", processDataInput));
            }
        }
    }
}