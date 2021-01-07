using System;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;

namespace DurableFunctionsDemo
{
    public static class GetSalesData
    {
        [FunctionName("GetSalesData")]
        public static async Task Run([ActivityTrigger] string region)
        {
            var connectionString = Environment.GetEnvironmentVariable("sales-history-sql-connection-string");
            
            var optionsBuilder = new DbContextOptionsBuilder<SalesDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var context = new SalesDataContext(optionsBuilder.Options))
            {
                var dataForRegion = context.SalesData
                    .Where(table => table.Region == region);

                var materializedData = dataForRegion.ToList();
            }
        }
    }
}