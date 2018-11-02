using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class TaxCategoryId: Attributes
    {
        public string Value { get; set; } // Categoría de impuestos

        public TaxCategoryId()
        {
            SchemeID            = "UN/ECE 5305";
            SchemeName          = "Tax Category Identifier";
            SchemeAgencyName    = "United Nations Economic Commission for Europe";
            SchemeAgencyID      = "6";
        }
    }
}
