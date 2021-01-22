using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsDemo
{
    public static class SalesAggregateOrchestrator
    {
        [FunctionName("SalesAggregateOrchestrator")]
        public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, 
            [DurableClient] IDurableOrchestrationClient durableOrchestrationClient,
            ILogger logger)
        {
            var regionsAndDivisions = await context.CallActivityAsync<SalesAggregateInput>("GetRegionsAndDivisions", null);
            
            var getDataTasks = new List<Task<IEnumerable<GetSalesDataOutput>>>();
                
            foreach (var region in regionsAndDivisions.Regions)
            {
                foreach (var division in regionsAndDivisions.Divisions)
                {
                    var input = new GetSalesDataInput
                    {
                        Division = division,
                        Region = region
                    };
                    
                    getDataTasks.Add(context.CallActivityAsync<IEnumerable<GetSalesDataOutput>>("GetSalesData", input));
                }
            }

            var getSalesDataResults = await Task.WhenAll(getDataTasks);

            // return data (i.e. no data found in db to match criteria).  Filter out null elements and flatten to just IEnumerable<T> with SelectMany
            var salesData = getSalesDataResults.Where(element => element != null)
                .SelectMany(element => element);
            var processDataTasks = new List<Task>();
            var runId = $"{DateTime.UtcNow:MMddyyyy-HHmmss}";
            
            foreach (var customer in salesData)
            {
                var processDataInput = new ProcessDataInput
                {
                    CustomerId = customer.CustomerId,
                    Division = customer.Division,
                    MonthlySalesTotal = customer.MonthlySalesTotal,
                    Region = customer.Region,
                    RunIdentifier = runId
                };
                
                processDataTasks.Add(context.CallActivityAsync("ProcessData", processDataInput));
            }

            await Task.WhenAll(processDataTasks);

            logger.LogInformation($"Calling SaveData function with run identifier {runId}");
            await context.CallActivityAsync("SaveData", runId);
        }
    }
}