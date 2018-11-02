using System;

namespace Models.Modelos
{
    public class PrecioAlternativo
    {
        public int IdPrecioAlternativo { get; set; }

        /// <summary>
        /// cbc:PriceAmount
        /// Precio de venta unitario/ Valor referencial unitario en operaciones no onerosas
        /// n(12,10)
        /// </summary>
        public decimal Monto { get; set; }

        /// <summary>
        /// @currencyID
        /// Código de tipo de moneda del precio de venta unitario o valor referencial unitario
        /// an3
        /// </summary>
        public string TipoMoneda { get; set; }

        /// <summary>
        /// cbc:PriceTypeCode > Value
        /// Código de tipo de precio | an2
        /// Catálogo No. 16: Códigos – Tipo de precio de venta unitario
        /// NombreTributo de tabla en la db: TipoPrecio
        /// </summary>
        public string TipoPrecio { get; set; }
    }
}