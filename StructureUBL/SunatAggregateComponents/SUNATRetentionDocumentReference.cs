using System;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class SunatRetentionDocumentReference
    {
        public PartyIdentificationId Id { get; set; }

        public string IssueDate { get; set; }

        public PayableAmount TotalInvoiceAmount { get; set; }

        public Payment Payment { get; set; }

        public SunatRetentionInformation SunatRetentionInformation { get; set; }

        public SunatRetentionDocumentReference()
        {
            Id = new PartyIdentificationId();
            TotalInvoiceAmount = new PayableAmount();
            Payment = new Payment();
            SunatRetentionInformation = new SunatRetentionInformation();
        }
    }
}
