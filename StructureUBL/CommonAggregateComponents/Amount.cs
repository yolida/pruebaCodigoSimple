using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Amount
    {
        public string CurrencyID { get; set; }

        public decimal Value { get; set; }
    }
}
