using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace maestroDetalle2Pract.AppWebMVC.Models;

public partial class MaestroDetallePract2DbContext : DbContext
{
    public MaestroDetallePract2DbContext()
    {
    }

    public MaestroDetallePract2DbContext(DbContextOptions<MaestroDetallePract2DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetallesVentas> DetallesVenta { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetallesVentas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detalles__3214EC0784938B80");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VentaId).HasColumnName("VentaID");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesVentas)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__DetallesV__Produ__3E52440B");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetallesVentas)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK__DetallesV__Venta__3D5E1FD2");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07F31B91D7");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ventas__3214EC0786B2686C");

            entity.HasIndex(e => e.Correlativo, "UQ__Ventas__25BD776AA8502EB4").IsUnique();

            entity.Property(e => e.Correlativo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
