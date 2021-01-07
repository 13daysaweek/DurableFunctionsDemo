using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DurableFunctionsDemo.Data;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;

namespace DurableFunctionsDemo
{
    public static class GetSalesData
    {
        [FunctionName("GetSalesData")]
        public static async Task<IEnumerable<GetSalesDataOutput>> Run([ActivityTrigger] GetSalesDataInput input)
        {
            IEnumerable<GetSalesDataOutput> output = null;
            
            var connectionString = Environment.GetEnvironmentVariable("sales-history-sql-connection-string");
            
            var optionsBuilder = new DbContextOptionsBuilder<SalesDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            IList<SalesDataItem> materializedData = null;
            
            using (var context = new SalesDataContext(optionsBuilder.Options))
            {
                var dataForRegion = context.SalesData
                    .Where(table => table.Region == input.Region && table.Division == input.Division);

                materializedData = await dataForRegion.ToListAsync();
            }

            if (materializedData.Any())
            {
                output = materializedData.GroupBy(table => table.CustomerId)
                    .Select(group => new GetSalesDataOutput
                    {
                        CustomerId = group.Key,
                        Division = input.Division,
                        Region = input.Region,
                        MonthlySalesTotal = group.Sum(item => item.TransactionAmount)
                    })
                    .ToList();

            }

            return output;
        }
    }
}