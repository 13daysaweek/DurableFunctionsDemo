﻿using DurableFunctionsDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DurableFunctionsDemo.Data.Configuration
{
    public class SalesAnomalyConfiguration : IEntityTypeConfiguration<SalesAnomaly>
    {
        public void Configure(EntityTypeBuilder<SalesAnomaly> builder)
        {
            builder.ToTable("SalesAnomalyResults", "dbo");

            builder.HasKey(table => table.SalesAnomalyId);

            builder.Property(table => table.Region)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(table => table.Division)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(table => table.CustomerId)
                .IsRequired();

            builder.Property(table => table.AnomalyCaclulationResult)
                .HasColumnType("decimal(18,4)")
                .IsRequired();
        }
    }
    
    public class SalesDataItemConfiguration : IEntityTypeConfiguration<SalesDataItem>
    {
        public void Configure(EntityTypeBuilder<SalesDataItem> builder)
        {
            builder.ToTable("SalesData", "dbo");
            
            builder.HasKey(table => table.SalesDataId);
            
            builder.Property(table => table.Region)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(table => table.Division)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(table => table.CustomerId)
                .IsRequired();

            builder.Property(table => table.TransactionDate)
                .IsRequired();

            builder.Property(table => table.TransactionAmount)
                .HasColumnType("decimal(18,4)")
                .IsRequired();
        }
    }
}
