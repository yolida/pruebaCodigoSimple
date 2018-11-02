using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class PartyIdentificationId
    {
        /// <summary>
        /// Para DriverPerson es n11, debería ser int 
        /// Invoice/cac:InvoiceLine/cac:Delivery/cac:DeliveryParty/cac:PartyIdentification => Código de tipo de documento de identidad del huesped
        /// Catálogo No. 06: Códigos de tipos de documentos de identidad
        /// <remarks>
        /// <para>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cac:PartyIdentification/cbc:ID@schemeID (Tipo de documento de identidad)</para>
        /// /Invoice/cac:AdditionalDocumentReference/cac:IssuerParty/cac:PartyIdentification/cbc:ID@schemeID (Tipo de documento del emisor del anticipo)
        /// </remarks>
        /// </summary>
        public string SchemeId { get; set; }

        public string SchemeName { get; set; } // Nuevo

        public string SchemeAgencyName { get; set; } // Nuevo

        public string SchemeURI { get; set; } // Nuevo
        
        public string schemeAgencyID { get; set; } // Nuevo

        /// <summary>
        /// <para> Numero de documento de identidad, RUC del transportista. Dato obligatorio si Transporte Público = "01". </para>
        /// /Invoice/cac:AdditionalDocumentReference/cac:IssuerParty/cac:PartyIdentification/cbc:ID (Número de documento del emisor del anticipo)
        /// <para> Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Número de documento de identidad </para>
        /// <para>DriverPerson: Datos de conductores - Número de documento de identidad</para>
        /// </summary>
        public string Value { get; set; }

        public PartyIdentificationId()
        {   
            SchemeName          = "Documento de Identidad";
            SchemeAgencyName    = "PE:SUNAT";
            SchemeURI           = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06";
            /*----------------------------------------------------------------------*/
            //SchemeId = "6"; // Valor  dinamico, algunas validaciones requieren que sea siempre el valor 6, pero este dato se obtiene de la tabla TipoDocumentoIdentidad
            SchemeName          = "Documento de Identidad";
        }
    }
}