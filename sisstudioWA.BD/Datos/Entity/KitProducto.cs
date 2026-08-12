using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class KitProducto : EntityBase
    {
        [Required]
        public int idKit { get; set; }
        [Required]
        public int idProducto { get; set; }
    }
}
