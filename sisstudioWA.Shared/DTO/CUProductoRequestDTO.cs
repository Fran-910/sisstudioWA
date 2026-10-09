using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class CUProductoRequestDTO
    {
        public ProductoPrivadoDTO DTO { get; set; }
        public List<int> idProductosKit { get; set; } = new List<int>();
    }
}
