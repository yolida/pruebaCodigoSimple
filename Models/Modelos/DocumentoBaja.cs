using Newtonsoft.Json;

namespace Models.Modelos
{
    public class DocumentoBaja : DocumentoResumenDetalle
    {
        [JsonProperty(Required = Required.Always)]
        public string Correlativo { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string MotivoBaja { get; set; }
    }
}
