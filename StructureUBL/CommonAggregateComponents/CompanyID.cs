namespace StructureUBL.CommonAggregateComponents
{
    /// <summary>
    /// Catálogo No. 06: Códigos de tipos de documentos de identidad
    /// </summary>
    public class CompanyID
    {
        public string SchemeID { get; set; } // Tipo de Documento de Identidad del Emisor (Se debe definir)

        public string SchemeName { get; set; }

        public string SchemeAgencyName { get; set; }

        public string SchemeURI { get; set; }

        public string Value { get; set; }

        public CompanyID()
        {
            SchemeName          = "SUNAT:Identificador de Documento de Identidad";
            SchemeAgencyName    = "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
        }
    }
}
