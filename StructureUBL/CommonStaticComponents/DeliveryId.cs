using System;
namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class DeliveryId: Attributes
    {
        public string Value { get; set; }

        public DeliveryId() {
            SchemeName          = "SUNAT:Identificador de Tipo de Medidor";
            SchemeAgencyName    = "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo58";
        }
    }
}
