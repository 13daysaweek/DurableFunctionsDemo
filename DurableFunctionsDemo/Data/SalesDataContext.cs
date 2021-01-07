using DurableFunctionsDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DurableFunctionsDemo.Data
{
    public class SalesDataContext : DbContext
    {
        public SalesDataContext(DbContextOptions<SalesDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public DbSet<SalesDataItem> SalesData { get; set; }
    }
}
