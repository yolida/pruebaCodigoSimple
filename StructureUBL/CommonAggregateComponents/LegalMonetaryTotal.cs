using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class LegalMonetaryTotal // Total valor de venta
    {
        /// <summary>
        /// currencyID => Código de tipo de moneda del total valor de venta an3
        /// Total valor de venta n(12,2)
        /// </summary>
        public PayableAmount LineExtensionAmount { get; set; }

        /// <summary>
        /// currencyID => Código de tipo de moneda del total precio de venta (incluye impuestos) an3
        /// Total valor de venta n(12,2)
        /// </summary>
        public PayableAmount TaxInclusiveAmount { get; set; }

        /// <summary>
        /// currencyID => Código de tipo de moneda del monto total de descuentos globales del comprobante an3
        /// Monto total de descuentos del comprobante n(12,2)
        /// </summary>
        public PayableAmount AllowanceTotalAmount { get; set; }

        /// <summary>
        /// currencyID => Código de tipo de moneda del monto total de otros cargos del comprobante an3
        /// Monto total de otros cargos del comprobante n(12,2)
        /// </summary>
        public PayableAmount ChargeTotalAmount { get; set; }

        /// <summary>
        /// currencyID => Código de tipo de moneda del monto total de anticipos del comprobante an3
        /// Monto total de anticipos del comprobante n(12,2)
        /// </summary>
        public PayableAmount PrepaidAmount { get; set; }

        /// <summary>
        /// Monto para Redondeo del Importe Total
        /// </summary>
        public PayableAmount PayableRoundingAmount { get; set; }

        /// <summary>
        /// currencyID => Código tipo de moneda del importe total de la venta, cesión en uso o del servicio prestado an3
        /// Importe total de la venta, cesión en uso o del servicio prestado n(12,2)
        /// </summary>
        public PayableAmount PayableAmount { get; set; }

        public LegalMonetaryTotal()
        {
            LineExtensionAmount     = new PayableAmount();
            TaxInclusiveAmount      = new PayableAmount();
            AllowanceTotalAmount    = new PayableAmount();
            ChargeTotalAmount       = new PayableAmount();
            PrepaidAmount           = new PayableAmount();
            PayableAmount           = new PayableAmount();
        }
    }
}
