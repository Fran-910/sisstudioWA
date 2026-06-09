using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class DetalleCarrito : EntityBase
    {
        public int idCarrito { get; set; }
        public int idProducto { get; set; }
        public int cantidad { get; set; }
    }
}
