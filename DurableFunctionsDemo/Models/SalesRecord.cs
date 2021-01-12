using System;

namespace DurableFunctionsDemo.Models
{
    public class SalesRecord
    {
        public DateTime TransactionDate { get; set; }
        
        public decimal TransactionAmount { get; set; }
    }
}