using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.Shared.DTO
{
    public class KitProdModelDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Id { get; set; }
        public int CantItems { get; set; }
    }
}
