using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class UbicacionCoordinada
    {
        [JsonProperty(Required = Required.AllowNull)]
        public string Latitud { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Longitud { get; set; }
    }
}
