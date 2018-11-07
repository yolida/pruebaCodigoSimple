using Newtonsoft.Json;

namespace Models.Modelos
{
    public abstract class DocumentoResumenDetalle
    {
        /// <summary>
        /// En comunicación de baja -> Número de ítem
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TipoDocumento { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Serie { get; set; }
    }
}
