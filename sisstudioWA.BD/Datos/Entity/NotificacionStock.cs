using System;
using System.Collections.Generic;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class NotificacionStock : EntityBase
    {
        public int idUsuario { get; set; }
        public int idProducto { get; set; }
        public DateTime fecha_solicitud { get; set; }
        public string estado { get; set; } //Enum: estado_notificacion, ej: "Pendiente", "Enviada", "Cancelada"
    }
}
