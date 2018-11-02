using Newtonsoft.Json;

namespace Models.Intercambio
{
    public class EnviarDocumentoRequest : EnvioDocumentoComun
    {
        [JsonProperty(Required = Required.Always)]
        public string TramaXmlFirmado { get; set; }
    }
}
