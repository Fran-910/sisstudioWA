using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Pedido : EntityBase
    {
        public int idUsuario { get; set; }
        public DateTime fecha { get; set; }
        public string estado { get; set; } //Enum: estado_pedido, ej: 
        public decimal monto_total { get; set; } = 0;
        public string envio { get; set; } //Enum: tipo_envio, ej: "Domicilio", "Retiro en tienda"   
    }
}
