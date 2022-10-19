using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace apinetcore5.Models
{
    public partial class d3rshp0o2v8k4vContext : DbContext
    {
        public d3rshp0o2v8k4vContext()
        {
        }

        public d3rshp0o2v8k4vContext(DbContextOptions<d3rshp0o2v8k4vContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MockDatum> MockData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MockDatum>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("mock_data");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Price)
                    .HasMaxLength(50)
                    .HasColumnName("price");

                 entity.Property(e => e.Lowprices)
                    .HasPrecision(5, 2)
                    .HasColumnName("lowprices");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("product_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
