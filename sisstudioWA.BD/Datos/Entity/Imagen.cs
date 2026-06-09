using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class Imagen : EntityBase
    {
        public int idProducto { get; set; }
        public string url { get; set; } //URL de la imagen, puede ser una ruta local o una URL externa
        public int orden { get; set; }
    }
}
