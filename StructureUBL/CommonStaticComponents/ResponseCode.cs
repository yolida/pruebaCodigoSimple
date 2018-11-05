namespace StructureUBL.CommonStaticComponents
{
    public class ResponseCode: Attributes
    {
        public string Value { get; set; }

        public ResponseCode()
        {
            ListAgencyName  =   "PE:SUNAT";
            ListURI         =   "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo09";
        }
    }
}