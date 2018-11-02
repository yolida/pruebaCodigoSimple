using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class PriceTypeCode: Attributes
    {
        public string Value { get; set; } // Código de tipo de precio

        public PriceTypeCode()
        {
            ListName        = /*"SUNAT:Indicador de Tipo de Precio"*/"Tipo de Precio";
            ListAgencyName  = "PE:SUNAT";
            ListURI         = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo16";
        }
    }
}
