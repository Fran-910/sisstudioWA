using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Producto : EntityBase
    {
        public string nombre { get; set; }
        public string subtitulo { get; set; }
        public string descripcion { get; set; } 
        public string tipo { get; set; } //Enum: tipo_producto, ej: "Kit, Producto individual"
        public decimal precio { get; set; } = 0;
        public int stock { get; set; } = 0;
    }
}
