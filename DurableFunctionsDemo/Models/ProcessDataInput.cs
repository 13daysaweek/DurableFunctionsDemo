using System;
using System.Collections.Generic;

namespace DurableFunctionsDemo.Models
{
    public class ProcessDataInput
    {
        public string Region { get; set; }
        
        public string Division { get; set; }
        
        public Guid CustomerId { get; set; }
        
        public string RunIdentifier { get; set; }
        
        public decimal MonthlySalesTotal { get; set; }
    }
}
