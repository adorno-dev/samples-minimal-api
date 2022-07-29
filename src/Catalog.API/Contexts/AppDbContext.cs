﻿using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Category>()
                .HasKey(c => c.CategoryId);

            mb.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            mb.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(100)
                .IsRequired();

            mb.Entity<Product>()
                .HasKey(c => c.ProductId);
            mb.Entity<Product>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();
            mb.Entity<Product>()
                .Property(c => c.Description)
                .HasMaxLength(150);
            mb.Entity<Product>()
                .Property(c => c.Image)
                .HasMaxLength(100);
            mb.Entity<Product>()
                .Property(c => c.Price)
                .HasPrecision(14, 2);

            mb.Entity<Product>()
                .HasOne<Category>(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.CategoryId);
        }
    }
}
