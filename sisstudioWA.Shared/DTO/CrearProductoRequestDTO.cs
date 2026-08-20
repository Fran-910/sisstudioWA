using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class CrearProductoRequestDTO
    {
        public ProductoPrivadoDTO DTO { get; set; }
        public List<int> idProductosKit { get; set; }
    }
}
