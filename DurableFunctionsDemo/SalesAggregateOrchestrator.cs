using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            [Blob("input/cf-input.json", FileAccess.Read, Connection = "input-storage-connection-string")] string regionsAndDivisions,
            Binder binder)
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

            if (results.Any())
            {
                var notEmptyResults = results.Where(result => result != null)
                    .SelectMany(result => result);
                
                var jsonResults = JsonConvert.SerializeObject(notEmptyResults);
                var jsonBytes = Encoding.UTF8.GetBytes(jsonResults);

                // Previously we were getting several zero byte blobs per run.  Switching to runtime binding seems to have solved that
                var attributes = new Attribute[]
                {
                    new BlobAttribute("output/{rand-guid}.json", FileAccess.Write),
                    new StorageAccountAttribute("input-storage-connection-string")
                };

                await using var outputStream = await binder.BindAsync<Stream>(attributes);
                await outputStream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
                await outputStream.FlushAsync();
            }
            
            return inputs;
        }
    }
}