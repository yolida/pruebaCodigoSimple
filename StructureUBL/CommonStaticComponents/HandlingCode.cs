using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class HandlingCode: Attributes
    {
        public string Value { get; set; }

        public HandlingCode()
        {
            ListName        = "SUNAT:Indicador de Motivo de Traslado";
            ListAgencyName  = "PE:SUNAT";
            ListURI         = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo20";
        }
    }
}
