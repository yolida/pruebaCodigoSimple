using System;
using System.Collections.Generic;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class TaxTotal
    {
        /// Monto de la operación
        /// @currencyID	=> Código de tipo de moneda del monto de la operación
        public PayableAmount TaxableAmount { get; set; }

        /// <summary>
        /// Monto de tributo del ítem
        /// @currencyID =>	Código de tipo de moneda del monto de tributo del ítem
        /// Listado de errores: 
        /// Si el Tag UBL existe, el monto total de impuestos por línea es diferente a la sumatoria de 
        /// 'Monto de tributo por línea' (cbc:TaxAmount de los tributos '1000', '1016','2000' y '9999') (con una tolerancia + -1) |
        /// OBSERV	4293 | El importe total de impuestos por línea no coincide con la sumatoria de los impuestos por línea.
        /// <summary>
        public PayableAmount TaxAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TaxSubtotal> TaxSubtotals { get; set; }

        public TaxTotal()
        {
            TaxableAmount   = new PayableAmount();
            TaxAmount       = new PayableAmount();
            TaxSubtotals    = new List<TaxSubtotal>();
        }
    }
}
