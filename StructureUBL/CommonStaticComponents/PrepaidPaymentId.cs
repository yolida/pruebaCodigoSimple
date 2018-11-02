using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class PrepaidPaymentId: Attributes
    { // Serie y número de comprobante del anticipo (para el caso de reorganización de empresas, incluye el RUC)
        public string Value { get; set; }

        public PrepaidPaymentId()
        {
            SchemeName          = /*"SUNAT:Identificador de Documentos Relacionados"*/ "Anticipo";
            SchemeAgencyName    = "PE:SUNAT";
        }
    }
}
