using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class ProductoPublicoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Subtitulo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public List<string> Imagenes { get; set; } = new();
        public decimal Precio { get; set; }
        public bool HayStock { get; set; }
        public TipoProd tipoProd { get; set; } = TipoProd.Producto;
        public List<int>? idProductosKit { get; set; } = new();
    }
}
