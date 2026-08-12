using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Producto : EntityBase
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(100)]
        public string Subtitulo { get; set; }
        [StringLength(250)]
        public string Descripcion { get; set; }
        public TipoProd tipoProd { get; set; } = TipoProd.Producto;//Enum: tipo_producto, ej: "Kit, Producto individual"
        [Required(ErrorMessage = "El precio del producto es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio del producto debe ser un valor positivo.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El stock del producto es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock del producto debe ser un valor positivo.")]
        public int Stock { get; set; }
    }
}
