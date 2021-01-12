using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace DurableFunctionsDemo.Models
{
    public class ProcessDataResult : TableEntity
    {
        // PartitionKey = Run Id, string formatted as MMddyyyy-HHmmss
        // RowKey = string in format of {region}-{division}-{customerId}
        
        public decimal SalesAnomalyCalculation { get; set; }
        public Guid CustomerId { get; set; }
        public string Division { get; set; }
        public string Region { get; set; }
    }
}