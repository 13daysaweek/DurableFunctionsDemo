using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Data;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;


namespace DurableFunctionsDemo
{
    public static class SaveData
    {
        [FunctionName("SaveData")]
        public static async Task Run([ActivityTrigger] string runIdentifier,
            [Table("ProcessDataOutput", Connection = "input-storage-connection-string")] CloudTable processDataOutputTable)
        {
            var query = new TableQuery<ProcessDataResult>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, runIdentifier));

            TableContinuationToken continuationToken = null;
            var resultsToSave = new List<SalesAnomaly>();
            
            do
            {
                var queryResult = await processDataOutputTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = queryResult.ContinuationToken;
                
                resultsToSave.AddRange(queryResult.Select(result => new SalesAnomaly
                {
                    AnomalyCalculationResult = result.SalesAnomalyCalculation,
                    CustomerId = result.CustomerId,
                    Division = result.Division,
                    Region = result.Region,
                    RunIdentifier = result.PartitionKey
                }));
                
            } while (continuationToken != null);
            
            var connectionString = Environment.GetEnvironmentVariable("sales-history-sql-connection-string");
            var optionsBuilder = new DbContextOptionsBuilder<SalesDataContext>();
            optionsBuilder.UseSqlServer(connectionString,
                options => options.EnableRetryOnFailure());
            
            using (var context = new SalesDataContext(optionsBuilder.Options))
            {
                await context.AddRangeAsync(resultsToSave);
                await context.SaveChangesAsync();
            }
        }
    }
}