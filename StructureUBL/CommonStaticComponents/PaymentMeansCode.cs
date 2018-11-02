using System;

namespace StructureUBL.CommonStaticComponents
{
    /// <summary>
    /// Número de cta. en el Banco de la Nación
    /// Catálogo No. 59: Medios de Pago
    /// </summary>
    public class PaymentMeansCode: Attributes
    {
        /// <summary>
        /// <para>Medio de pago | n2</para>
        /// /Invoice/cac:PaymentMeans/cbc:PaymentMeansCode
        /// </summary>
        public string Value { get; set; }

        public PaymentMeansCode()
        {  
            ListName        = "Medio de pago";
            ListAgencyName  = "PE:SUNAT";
            ListURI         = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo59";
        }
    }
}
