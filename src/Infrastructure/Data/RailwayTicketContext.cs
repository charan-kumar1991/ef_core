using Core.Models;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RailwayTicketContext : DbContext
    {
        public RailwayTicketContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder
                .Entity<Customer>()
                .HasIndex(c => c.Username)
                .IsUnique();

            modelBuilder
                .Entity<Ticket>()
                .HasIndex(t => t.PNR)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Add hook for SQL Server exceptions
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
