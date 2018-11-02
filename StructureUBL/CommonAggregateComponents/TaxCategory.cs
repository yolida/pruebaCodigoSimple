using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class TaxCategory
    {
        public TaxExemptionReasonCode TaxExemptionReasonCode { get; set; }
        public string           TierRange { get; set; } // Código de tipo de sistema de ISC an2
        public TaxScheme        TaxScheme { get; set; }
        public string           Id { get; set; }
        public TaxCategoryId    TaxCategoryId { get; set; }
        public decimal          Percent { get; set; }

        public TaxCategory()
        {
            TaxScheme       = new TaxScheme();
            TaxCategoryId   = new TaxCategoryId();
        }
    }
}
