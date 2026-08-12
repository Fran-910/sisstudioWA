using sisstudioWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sisstudioWA.BD.Datos.Entity
{
    public class NotificacionStock : EntityBase
    {
        [Required]
        public int idUsuario { get; set; }
        [Required]
        public int idProducto { get; set; }
        [Required]
        public DateTime Fecha_Solicitud { get; set; }
        public TipoEstadoNotificacion Estado { get; set; } = TipoEstadoNotificacion.Pendiente; //Enum: estado_notificacion, ej: "Pendiente", "Enviada", "Cancelada"
    }
}
