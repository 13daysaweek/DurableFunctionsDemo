using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Data;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;

namespace DurableFunctionsDemo
{
    public static class ProcessData
    {
        public static async Task Run([ActivityTrigger] ProcessDataInput input)
        {
            var rng = new Random();
            var randomResult = Convert.ToDecimal(rng.NextDouble() * rng.Next());

            var anomaly = new SalesAnomaly
            {
                AnomalyCaclulationResult = randomResult,
                CustomerId = input.CustomerId,
                Division = input.Division,
                Region = input.Region,
                RunIdentifier = input.RunIdentifier
            };

            await Task.Delay(TimeSpan.FromSeconds(15));
            
            var connectionString = Environment.GetEnvironmentVariable("sales-history-sql-connection-string");
            
            var optionsBuilder = new DbContextOptionsBuilder<SalesDataContext>();
            optionsBuilder.UseSqlServer(connectionString);


            await using var context = new SalesDataContext(optionsBuilder.Options);
            await context.SalesAnomalies.AddAsync(anomaly);
            await context.SaveChangesAsync();
        }
    }
}