using Newtonsoft.Json;

namespace Models.Intercambio
{
    public class ConsultaConstanciaRequest : EnvioDocumentoComun
    {
        [JsonProperty(Required = Required.Always)]
        public string Serie { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int Numero { get; set; }
    }
}
