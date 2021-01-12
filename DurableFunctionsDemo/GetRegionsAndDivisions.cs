using System.IO;
using System.Threading.Tasks;
using DurableFunctionsDemo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace DurableFunctionsDemo
{
    public static class GetRegionsAndDivisions
    {
        [FunctionName("GetRegionsAndDivisions")]
        public static async Task<SalesAggregateInput> Run([ActivityTrigger] IDurableActivityContext context,  [Blob("input/cf-input.json", FileAccess.Read, Connection = "input-storage-connection-string")] string regionsAndDivisionsString)
        {
            var regionsAndDivisions = JsonConvert.DeserializeObject<SalesAggregateInput>(regionsAndDivisionsString);

            return await Task.FromResult(regionsAndDivisions);
        }
    }
}