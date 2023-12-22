using Core.Entities.Concrete;
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
                //optionsBuilder.UseSqlServer("Server=DESKTOP-K1G3PAC;Database=PofuMacrame;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("Server=YAYIN01;Database=PofuMacrame; User ID=murat;Password=123456;Connect Timeout=30;MultiSubnetFailover=False;");
                //optionsBuilder.UseSqlServer("Server=94.102.74.13;Database=WeighbridgeCalculator; User ID=websa;Password=v2qySqsu7MkXL5D;Connect Timeout=30;MultiSubnetFailover=False;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
              .HasKey(category => category.Id);

            modelBuilder.Entity<PasswordReset>()
              .HasKey(pr => pr.UserId); // Birebir ilişki için UserId'yi anahtar olarak belirle


            modelBuilder.Entity<Category>()
           .HasOne<Category>()
           .WithMany()
           .HasForeignKey(c => c.ParentId) // ParentId, dış anahtar olarak kullanılıyor.
           .IsRequired(false)// ParentId nullable olduğu için IsRequired(false) kullanılıyor.
           .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Entities.Concrete.Attribute> Attributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
        public DbSet<CategoryImage> CategoryImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
    }
}
