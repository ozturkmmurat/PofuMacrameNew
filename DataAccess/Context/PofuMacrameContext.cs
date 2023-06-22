using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Context
{
    public class PofuMacrameContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=YAYIN01;Database=PofuMacrame; User ID=emre;Password=123456;Connect Timeout=30;MultiSubnetFailover=False;");
                //optionsBuilder.UseSqlServer("Server=94.102.74.13;Database=WeighbridgeCalculator; User ID=websa;Password=v2qySqsu7MkXL5D;Connect Timeout=30;MultiSubnetFailover=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Entities.Concrete.Attribute> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
