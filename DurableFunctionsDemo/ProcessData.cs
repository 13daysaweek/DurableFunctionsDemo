using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Data;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DurableFunctionsDemo
{
    public static class ProcessData
    {
        [FunctionName("ProcessData")]
        public static async Task Run([ActivityTrigger] ProcessDataInput input,
            [Table("ProcessDataOutput", Connection = "input-storage-connection-string")] IAsyncCollector<ProcessDataResult> tableOutput,
            ILogger logger)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Spin for 15 seconds to give the feeling that we're doing cool ML stuff here :D
            
            var rng = new Random();
            var randomResult = Convert.ToDecimal(rng.NextDouble() * rng.Next());
            var rowKey = $"{input.Region}-{input.Division}-{input.CustomerId}";
            

            var processDataResult = new ProcessDataResult
            {
                PartitionKey = input.RunIdentifier,
                RowKey = rowKey,
                SalesAnomalyCalculation = randomResult,
                Region = input.Region,
                Division = input.Division,
                CustomerId = input.CustomerId
            };

            await tableOutput.AddAsync(processDataResult);
        }
    }
}