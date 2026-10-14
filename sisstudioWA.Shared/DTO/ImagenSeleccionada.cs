using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;


namespace sisstudioWA.Shared.DTO
{
    public class ImagenSeleccionada
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string url { get; set; } = string.Empty;
        public IBrowserFile archivo { get; set; } = default!;
    }
}
