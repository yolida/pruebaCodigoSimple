using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class ContractDocumentReference // Según la referencia de oasis es de 0 a *
    {
        public string Id { get; set; }

        public ContractDocumentReference_DocumentTypeCode DocumentTypeCode { get; set; }

        public LocaleCode LocaleCode { get; set; }

        public DocumentStatusCode DocumentStatusCode { get; set; }
    }
}
