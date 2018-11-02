using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Descuento
    {
        /// <summary>
        /// /Invoice/cac:AllowanceCharge/cbc:ChargeIndicator (Indicador de cargo)
        /// Si valor del tag es diferente 'true' para código de cargo igual a '45'
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public bool Indicador { get; set; } // cbc:ChargeIndicator

        /// <summary>
        /// cbc:AllowanceChargeReasonCode
        /// Codigo en la tabla MotivoDescuento
        /// Catálogo No. 53: Códigos de cargos o descuentos
        /// Código del motivo del cargo
        /// -------------- Para percepciones -------------
        /// /Invoice/cac:AllowanceCharge/cbc:AllowanceChargeReasonCode
        /// Código de motivo de cargo/descuento: Código de régimen de percepción
        /// Reglas: Si 'Tipo de operación' es '2001 - Operación Sujeta a Percepción', debe existir código de cargo/descuento igual a '51' o '52' o '53'
        /// -------------- Para percepciones -------------
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string CodigoMotivo { get; set; }

        /// <summary>
        /// cbc:MultiplierFactorNumeric | n(3,5)
        /// Factor de cargo/descuento
        /// Si el Tag UBL existe, el formato del Tag UBL es diferente de decimal positivo de 3 enteros y hasta 5 decimales y diferente de cero
        /// Factor de cargo/descuento: Tasa percepción expresado como factor
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal Factor { get; set; }

        /// <summary>
        /// /Invoice/cac:AllowanceCharge/cbc:Amount
        /// Monto del cargo/descuento
        /// -------------- Para percepciones -------------
        /// Monto de la percepción
        /// -------------- Para percepciones -------------
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal Monto { get; set; } //cbc:Amount
        
        [JsonProperty(Required = Required.AllowNull)]
        public string Moneda { get; set; } // @currencyID

        /// <summary>
        /// /Invoice/cac:AllowanceCharge/cbc:BaseAmount
        /// Monto base del cargo/descuento
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal MontoBase { get; set; } // cbc:BaseAmount

        [JsonProperty(Required = Required.AllowNull)]
        public string MonedaBase { get; set; } // @currencyID
    }
}
