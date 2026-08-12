using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Usuario : EntityBase
    {
        [Required]
        [StringLength(45, MinimumLength = 3)]
        public string Nombre { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Contraseña { get; set; }
        [StringLength(15)]
        public string Tel { get; set; }
    }
}
