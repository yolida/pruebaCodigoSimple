using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    /// <summary>
    /// Para Tipo y número de la guía de remisión relacionada:  DespatchDocumentReference   = Catálogo No. 01 
    /// Para Tipo y número de otro documento relacionado:       AdditionalDocumentReference = Catálogo No. 12
    /// </summary>
    public class DocumentoRelacionado
    {
        public int IdDocumentoRelacionado { get; set; }

        /// <summary>
        /// ID: Número de guía de remisión relacionada con la operación que se factura
        /// </summary>
        [JsonProperty(Order = 1, Required = Required.Always)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// Código de tipo de documento relacionado con la operación que se factura. Catalogo 12. | 
        /// Factura - Emitida por Anticipo = "02" | Boleta - Emitida por Anticipo = "03"
        /// /Invoice/cac:AdditionalDocumentReference/cbc:DocumentTypeCode (Tipo de comprobante que se realizó el anticipo)
        /// Código de tipo de guía de remisión relacionada con la operación que se factura
        /// </summary>
        [JsonProperty(Order = 2, Required = Required.Always)]
        public string TipoDocumentoRelacionado { get; set; }

        /// <summary>
        /// Tipo de documento del documento que modifica
        /// Catalogo 1 | /CreditNote/cac:BillingReference/cac:InvoiceDocumentReference/cbc:DocumentTypeCode
        /// </summary>
        public string TipoDocumento { get; set; }

        /// <summary>
        /// DocumentStatusCode
        /// /Invoice/cac:AdditionalDocumentReference/cbc:DocumentStatusCode (Identificador del pago)
        /// </summary>
        public string IdentificadorPago { get; set; }

        /// <summary>
        /// Temporal para revisión
        /// </summary>
        public string TipoOperacion { get; set; }

        public List<Contribuyente> Identificaciones { get; set; }
    }
}
