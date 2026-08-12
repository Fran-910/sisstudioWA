using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Carrito : EntityBase
    {
        [Required]
        public int idUsuario { get; set; }
    }
}
