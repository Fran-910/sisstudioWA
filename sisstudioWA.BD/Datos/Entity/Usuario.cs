using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Usuario : EntityBase
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public string tel { get; set; }
    }
}
