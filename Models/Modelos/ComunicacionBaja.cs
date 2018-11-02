using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class ComunicacionBaja : DocumentoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public List<DocumentoBaja> Bajas { get; set; }
    }
}
