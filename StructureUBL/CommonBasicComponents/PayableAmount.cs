using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class PayableAmount
    {
        /// <summary>
        /// Catálogo No. 02: Códigos de tipo de monedas
        /// </summary>
        public string CurrencyId { get; set; }

        public decimal Value { get; set; }
    }
}
