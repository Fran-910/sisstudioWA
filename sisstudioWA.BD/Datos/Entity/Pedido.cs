using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Pedido : EntityBase
    {
        [Required]
        public int idUsuario { get; set; }
        [Required]
        public DateTime Fecha { get; set; }

        public TipoEstadoPedido Estado { get; set; } = TipoEstadoPedido.Pendiente; //Enum: estado_pedido, ej: "Pendiente", "Enviado", "Cancelado"
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto_Total { get; set; }

        public TipoEnvio Envio { get; set; } = TipoEnvio.Domicilio; //Enum: tipo_envio, ej: "Domicilio", "Retiro en tienda"   
    }
}
