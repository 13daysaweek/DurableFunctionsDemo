using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionsDemo.Models
{
    public class SalesAnomaly
    {
        public int SalesAnomalyId { get; set; }

        public string RunIdentifier { get; set; }

        public string Region { get; set; }

        public string Division { get; set; }

        public Guid CustomerId { get; set; }

        public decimal AnomalyCalculationResult { get; set; }
    }
}
