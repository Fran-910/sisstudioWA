using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class DetalleCarrito : EntityBase
    {
        [Required]
        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
