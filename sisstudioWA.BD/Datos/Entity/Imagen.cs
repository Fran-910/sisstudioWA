using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Imagen : EntityBase
    {
        [Required]
        public int idProducto { get; set; }
        [Required]
        public string Url { get; set; } //URL de la imagen, puede ser una ruta local o una URL externa
        [Required]
        public int Orden { get; set; }
    }
}
