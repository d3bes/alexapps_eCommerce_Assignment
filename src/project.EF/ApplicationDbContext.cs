using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project.Core.Models;


namespace project.EF
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }      
        
        //   public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Merchant> merchants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>(entity => entity.ToTable("Users"));
            builder.Entity<Merchant>(entity => entity.ToTable("Merchants"));
            builder.Entity<Cart>(entity => entity.ToTable("Carts"));
            builder.Entity<CartItem>(entity => entity.ToTable("CartItems"));
            builder.Entity<Product>(entity => entity.ToTable("Products"));

        }
    }
}