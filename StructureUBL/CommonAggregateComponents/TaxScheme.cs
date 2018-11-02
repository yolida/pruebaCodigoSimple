using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class TaxScheme // Código de tributo
    {
        public TaxSchemeId TaxSchemeId { get; set; } // Código de tributo

        public string Name { get; set; } // Nombre de tributo an..6

        /// Código internacional tributo an4
        /// Código del tributo
        public string TaxTypeCode { get; set; }

        public TaxScheme()
        {
            TaxSchemeId = new TaxSchemeId();
        }
    }
}
