using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class TaxSubtotal
    {
        /// Monto las operaciones gravadas/exoneradas/inafectas del impuesto
        /// => Código de tipo de moneda del monto de las operaciones gravadas/exoneradas/inafectas del impuesto
        public PayableAmount TaxableAmount { get; set; }

        /// Monto total del impuesto
        /// => Código de tipo de moneda del monto total del impuesto
        public PayableAmount TaxAmount { get; set; }

        public TaxCategory TaxCategory { get; set; } // Categoría de impuestos

        public decimal Percent { get; set; }

        public TaxSubtotal()
        {
            TaxableAmount   = new PayableAmount();
            TaxAmount       = new PayableAmount();
            TaxCategory     = new TaxCategory();
        }
    }
}
