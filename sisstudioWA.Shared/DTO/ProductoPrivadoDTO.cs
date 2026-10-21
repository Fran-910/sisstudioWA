using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class ProductoPrivadoDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
        public string Nombre { get; set; }
        [StringLength(100, ErrorMessage = "El subtítulo no puede superar los 100 caracteres.")]
        public string Subtitulo { get; set; }
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        public string Descripcion { get; set; }
        public List<string>? Imagenes { get; set; }
        public TipoProd tipoProd { get; set; } = TipoProd.Producto;
        [Range(0, double.MaxValue, ErrorMessage = "El precio del producto debe ser un valor positivo.")]
        public decimal Precio { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "El stock del producto debe ser un valor positivo.")]
        public int Stock { get; set; } = 0;
    }
}
