using System.Collections.Generic;
using Newtonsoft.Json;

namespace DurableFunctionsDemo.Models
{
    public class SalesAggregateInput
    {
        [JsonProperty("regions")]
        public IEnumerable<string> Regions { get; set; }
        
        [JsonProperty("divisions")]
        public IEnumerable<string> Divisions { get; set; }
    }
}
