using Newtonsoft.Json;
using System;

namespace Models.Modelos
{
    public class PeriodoFactura
    {
        [JsonProperty(Required = Required.Always)]
        public DateTime FechaInicio { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime FechaFin { get; set; }
    }
}
