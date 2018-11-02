using Newtonsoft.Json;
using System;

namespace Models.Modelos
{
    public class DocumentoContrato
    {
        /// <summary>
        /// Número de suministro/Número de teléfono
        /// an..9
        /// [1..1] SUNAT
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string IdNumero { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public Int16 TipoServicioPublico { get; set; }  // Tipo de Servicio Público DocumentTypeCode

        [JsonProperty(Required = Required.AllowNull)]
        public string ServicioTelecomunicaciones { get; set; }  // Código de Servicios de Telecomunicaciones (De corresponder) LocaleCode

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoTarifaContratada { get; set; } // Código de Tipo de Tarifa contratada DocumentStatusCode
    }
}
