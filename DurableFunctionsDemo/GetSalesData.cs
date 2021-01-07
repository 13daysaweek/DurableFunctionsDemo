using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableFunctionsDemo
{
    public static class GetSalesData
    {
        [FunctionName("GetSalesData")]
        public static async Task Run([ActivityTrigger] string region)
        {

        }
    }
}