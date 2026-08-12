using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class DetalleCarrito : EntityBase
    {
        [Required]
        public int idCarrito { get; set; }
        [Required]
        public int idProducto { get; set; }
        [Required]
        public int Cantidad { get; set; }
    }
}
