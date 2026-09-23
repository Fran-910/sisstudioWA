using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class KitProducto : EntityBase
    {
        [Required]
        public int KitId { get; set; }
        public Producto Kit { get; set; }
        [Required]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
