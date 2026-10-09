using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace sisstudioWA.Shared.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter<TipoProd>))]
    public enum TipoProd
    {
        Kit,
        Producto
    }
}
