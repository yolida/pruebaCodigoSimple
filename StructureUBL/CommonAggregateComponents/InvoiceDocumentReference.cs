using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class InvoiceDocumentReference : IEquatable<InvoiceDocumentReference>
    {
        /*-----------------------------------------------------------------------------------------------------
            Id = Número de guía de remisión relacionada con la operación que se factura

            AdditionalDocumentReference
            Id = Número de documento relacionado con la operación que se factura
        -----------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Serie y Número de comprobante que se realizó el anticipo
        /// </summary>
        public string Id { get; set; }

        public DocumentTypeCode DocumentTypeCode { get; set; }

        public DocumentStatusCode DocumentStatusCode { get; set; }

        /// <summary>
        /// cac:IssuerParty [0..1]    The party who issued the referenced document.
        /// </summary>
        public IssuerParty IssuerParty { get; set; }

        public InvoiceDocumentReference()
        {
            DocumentTypeCode    = new DocumentTypeCode();
            DocumentStatusCode  = new DocumentStatusCode();
        }

        public bool Equals(InvoiceDocumentReference other)
        {
            if (other == null) return false;

            if (string.IsNullOrEmpty(Id))
                return false;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(Id))
                return base.GetHashCode();

            return Id.GetHashCode();
        }
    }
}
