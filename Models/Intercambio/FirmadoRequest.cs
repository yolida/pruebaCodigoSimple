using Newtonsoft.Json;

namespace Models.Intercambio
{
    public class FirmadoRequest
    {
        [JsonProperty(Required = Required.Always)]
        public byte[] CertificadoDigital { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string PasswordCertificado { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TramaXmlSinFirma { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool UnSoloNodoExtension { get; set; }
    }
}
