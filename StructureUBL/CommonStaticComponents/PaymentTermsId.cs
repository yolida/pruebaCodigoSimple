using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class PaymentTermsId: Attributes
    {
        /// <summary>
        /// Catálogo No. 54: Códigos de bienes y servicios sujetos a detracción
        /// </summary>
        public string Value { get; set; }

        public PaymentTermsId()
        {
            SchemeName          = /*"SUNAT:Codigo de detraccion"*/ "Codigo de detraccion";
            SchemeAgencyName    = "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo54";
        }
    }
}
