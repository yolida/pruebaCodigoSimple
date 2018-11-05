using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Discrepancia
    {
        public int IdDiscrepanciaDocumento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Tipo { get; set; }

        /// <summary>
        /// Motivo o Sustento | /CreditNote/cac:DiscrepancyResponse/cbc:Description
        /// discrepancia.Descripcion | Description
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Descripcion { get; set; }
    }
}
