using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionsDemo.Models
{
    public class SalesDataItem
    {
        public int SalesDataId { get; set; }
        
        public string Region { get; set; }
        
        public string Division { get; set; }
        
        public Guid CustomerId { get; set; }
        
        public DateTime TransactionDate { get; set; }
        
        public decimal TransactionAmount { get; set; }
    }
}
