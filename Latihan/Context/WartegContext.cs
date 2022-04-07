using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Latihan.Context
{
public class WartegContext : DbContext
    {
        public WartegContext(DbContextOptions<WartegContext> options)
        : base(options)
        {
        }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>().ToTable("Food");
            modelBuilder.Entity<Topping>().ToTable("Topping");
            modelBuilder.Entity<Customer>().ToTable("Customer");
        }
    }
    public class Food
    {
        public int FoodId { get; set; }
        public string? Name { get; set; }
        public int? ToppingId { get; set; }
    }
    public class Topping
    {
        public int ToppingId { get; set; }
        public string? Name { get; set; }
    }
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? CreatedDate { get; set; }
    }


}
