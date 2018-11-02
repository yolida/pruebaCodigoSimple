using Newtonsoft.Json;

namespace Models.Modelos
{
    public class TerminosPago
    {
        /// <summary>
        /// Codigo del bien o producto sujeto a detracción
        /// Codigo en la tabla detraciones
        /// Catálogo No. 54: Códigos de bienes y servicios sujetos a detracción
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string Codigo { get; set; }

        /// <summary>
        /// <para>Porcentaje de la detracción</para>
        /// Catálogo 54: Códigos de bienes y servicios sujetos a detracciones (Según documentación 2018/07/26)
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal Porcentaje { get; set; }

        /// <summary>
        /// Monto de la detracción
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal Monto { get; set; }
    }
}
