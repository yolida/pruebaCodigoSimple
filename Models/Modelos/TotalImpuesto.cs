using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class TotalImpuesto // [0..*]
    {
        public int IdTotalImpuestos { get; set; }

        /// <summary>
        /// cbc:TaxAmount | Monto total del impuestos | n(12,2)
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal MontoTotal { get; set; } // cbc:TaxAmount Monto total del impuestos

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoMonedaTotal { get; set; } // @currencyID Código de tipo de moneda del monto total del tributo

        [JsonProperty(Required = Required.AllowNull)]
        public List<SubTotalImpuestos> SubTotalesImpuestos { get; set; } // [0..*]
    }
}
