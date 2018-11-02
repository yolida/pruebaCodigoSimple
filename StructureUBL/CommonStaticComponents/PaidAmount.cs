using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class PaidAmount // Monto prepagado o anticipado
    {
        public string CurrencyID { get; set; } // Código de tipo de moneda del monto prepagado o anticipado

        public decimal Value { get; set; } // n(15,2)
    }
}
