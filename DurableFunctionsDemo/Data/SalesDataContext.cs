using DurableFunctionsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DurableFunctionsDemo.Data
{
    public class SalesDataContext : DbContext
    {
        public SalesDataContext(DbContextOptions<SalesDataContext> options) : base(options)
        {
        }
        
        public DbSet<SalesDataItem> SalesData { get; set; }
    }
}
