using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class DetallePedido : EntityBase
    {
        [Required]
        public int idProducto { get; set; }
        [Required]
        public int idPedido { get; set; }
        [Required]
        public int Cantidad { get; set; } = 0;
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor o igual a cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio_Unitario { get; set; }
    }
}
