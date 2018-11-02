namespace StructureUBL.CommonStaticComponents
{
    public class ResponseCode: Attributes
    {
        public string Value { get; set; }

        public ResponseCode()
        {
            ListAgencyName  =   "PE:SUNAT";
            ListName        =   "Tipo de nota de credito";
            ListURI         =   "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo09";
        }
    }
}