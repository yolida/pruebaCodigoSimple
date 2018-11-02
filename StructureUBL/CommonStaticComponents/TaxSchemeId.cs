using System;

namespace StructureUBL.CommonStaticComponents
{
    /// <summary>
    /// Catálogo No. 05: Códigos de tipos de tributos
    /// </summary>
    [Serializable]
    public class TaxSchemeId: Attributes
    {
        public string Value { get; set; } // an3 Código de tributo

        public TaxSchemeId()
        {
            SchemeID            = "UN/ECE 5153";
            SchemeAgencyID      = "6";
            SchemeName          = /*"Tax Scheme Identifier"*/ "Codigo de tributos";
            SchemeAgencyName    = /*"United Nations Economic Commission for Europe"*/ "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05";
        }
    }
}
