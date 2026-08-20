using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class ProductoPrivadoDTO
    {
        public string Nombre { get; set; }
        public string Subtitulo { get; set; }
        public string Descripcion { get; set; }
        public string[] Imagenes { get; set; }
        public TipoProd tipoProd { get; set; } = TipoProd.Producto;
        public decimal Precio { get; set; } = 0;
        public int Stock { get; set; } = 0;
    }
}
