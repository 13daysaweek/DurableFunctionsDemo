using System;

namespace DurableFunctionsDemo.Models
{
    public class GetSalesDataOutput
    {
        public string Region { get; set; }
        
        public string Division { get; set; }
        
        public Guid CustomerId { get; set; }
        
        public decimal MonthlySalesTotal { get; set; }
    }
}