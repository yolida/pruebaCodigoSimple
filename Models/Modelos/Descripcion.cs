using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Descripcion
    {
        public int IdDescripcion { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Detalle { get; set; }
    }
}
