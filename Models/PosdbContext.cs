using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace POSbackend.Models;

public partial class PosdbContext : DbContext
{
    public PosdbContext()
    {
    }

    public PosdbContext(DbContextOptions<PosdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleDetail> SaleDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Batch>(entity =>
        {
            entity.HasKey(e => e.Batchid).HasName("PK__Batch__78CDDB4BE3062227");

            entity.ToTable("Batch");

            entity.Property(e => e.Batchid).HasColumnName("batchid");
            entity.Property(e => e.Batchnumber).HasColumnName("batchnumber");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__11F2FC6AD02C81A8");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BillImage).HasMaxLength(500);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InsertedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PdfUrl).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Sale).WithMany(p => p.Bills)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK__Bills__SaleId__02FC7413");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("PK__Category__190606233E5DF0B6");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D88F1571E9");

            entity.ToTable("Customer");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38437EC965");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GatewayTransactionId).HasMaxLength(255);
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Sale).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__SaleId__70DDC3D8");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD9C71B404");

            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ManufacturerName).HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(150);
            entity.Property(e => e.SetSize).HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Batch).WithMany(p => p.Products)
                .HasForeignKey(d => d.Batchid)
                .HasConstraintName("FK__Products__Batchi__693CA210");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("FK__Products__Catego__5070F446");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Sales__1EE3C3FF840C13F1");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerNumber).HasMaxLength(20);
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Method).HasMaxLength(20);
            entity.Property(e => e.NetAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Tax)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<SaleDetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PK__SaleDeta__70DB14FE9E3371B0");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetails_Products");

            entity.HasOne(d => d.Sale).WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetails_Sales");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C6DD18BD0");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4FBE5423E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B1D1CE1D").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
