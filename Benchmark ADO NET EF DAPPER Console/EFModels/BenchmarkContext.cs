using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Benchmark_ADO_NET_EF_DAPPER_Console.EFModels
{
    public partial class BenchmarkContext : DbContext
    {
        public string ConnectionString { get; set; }

        public BenchmarkContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public BenchmarkContext()
        {
            ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=Benchmark; Integrated Security=true;";
        }

        public BenchmarkContext(DbContextOptions<BenchmarkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Human> Humans { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Human>(entity =>
            {
                entity.ToTable("Human");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
