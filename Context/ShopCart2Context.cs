using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShoppingList4.Models;

namespace ShoppingList4.Context
{
    public partial class ShopCart2Context : DbContext
    {
        public ShopCart2Context()
        {
        }

        public ShopCart2Context(DbContextOptions<ShopCart2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ShopCart2;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItems>(entity =>
            {
                entity.HasKey(e => e.CartItemId);

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartItems_Carts");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_CartItems_Products");
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.HasKey(e => e.CartId);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Products__B40CC6EDDBA6470B");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.QuantityPerUnit)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.UnitPrice).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
