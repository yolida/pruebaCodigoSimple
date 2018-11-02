using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class TaxExemptionReasonCode: Attributes
    {
        public string Value { get; set; } // Código de tipo de afectación del IGV an2

        public TaxExemptionReasonCode()
        {
            ListName        = /*"SUNAT:Codigo de Tipo de Afectación del IGV"*/ "Afectacion del IGV";
            ListAgencyName  = "PE:SUNAT";
            ListURI         = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07";
        }
    }
}