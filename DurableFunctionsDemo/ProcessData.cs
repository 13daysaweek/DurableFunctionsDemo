using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableFunctionsDemo
{
    public static class ProcessData
    {
        public static async Task Run([ActivityTrigger] string region)
        {

        }
    }
}