using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class ProfileID
    {
        public string SchemeName { get; set; }

        public string SchemeAgencyName { get; set; }

        public string SchemeURI { get; set; }

        public string Value { get; set; }

        public ProfileID()
        {
            SchemeName = "SUNAT:Identificador de Tipo de Operación";
            SchemeAgencyName = "PE:SUNAT";
            SchemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo17";
        }
    }
}
