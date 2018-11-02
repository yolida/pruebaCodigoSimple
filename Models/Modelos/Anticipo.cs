using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Anticipo
    {
        public int IdAnticipo { get; set; }

        /// <summary>
        /// cbc:ID
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string ComprobanteAnticipo { get; set; }

        /// <summary>
        /// @schemeID
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string TipoDocumento { get; set; }

        /// <summary>
        /// cbc:PaidAmount
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal Monto { get; set; }

        /// <summary>
        /// @currencyID
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string Moneda { get; set; }

        /// <summary>
        /// /Invoice/cac:PrepaidPayment/cbc:PaidDate
        /// Fecha de Pago
        /// </summary>
        public DateTime FechaPago { get; set; }
    }
}
