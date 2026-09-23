using Microsoft.EntityFrameworkCore;
using sisstudioWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos
{
    public class AppDbContext : DbContext
    {
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<DetalleCarrito> DetallesCarritos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<KitProducto> KitsProductos { get; set; }
        public DbSet<NotificacionStock> NotificacionesStocks { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KitProducto>()
                .HasOne(kp => kp.Kit)
                .WithMany()
                .HasForeignKey(kp => kp.KitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KitProducto>()
                .HasOne(kp => kp.Producto)
                .WithMany()
                .HasForeignKey(kp => kp.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
