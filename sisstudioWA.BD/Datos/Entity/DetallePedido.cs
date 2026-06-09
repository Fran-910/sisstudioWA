using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class DetallePedido : EntityBase
    {
        public int idProducto { get; set; }
        public int idPedido { get; set; }
        public int cantidad { get; set; } = 0;
        public decimal precio_unitario { get; set; } = 0;
    }
}
