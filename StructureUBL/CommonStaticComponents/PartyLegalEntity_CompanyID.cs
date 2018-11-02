using System;

namespace StructureUBL.CommonStaticComponents
{
    public class PartyLegalEntity_CompanyID: Attributes
    {
        public int Value { get; set; } // Número de documento de identidad del destinatario

        public PartyLegalEntity_CompanyID()
        {
            SchemeName          = "SUNAT:Identificador de Documento de Identidad";
            SchemeAgencyName    = "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
        }
    }
}
