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
        public DbSet<DetalleCarrito > DetalleCarritos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<KitProducto> KitProductos { get; set; }
        public DbSet<NotificacionStock> NotificacionStocks { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
