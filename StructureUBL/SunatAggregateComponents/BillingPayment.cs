using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.SunatAggregateComponents
{
    [Serializable]
    public class BillingPayment
    {
        // Serie y número de comprobante del anticipo (para el caso de reorganización de empresas, incluye el RUC)
        public PartyIdentificationId Id { get; set; }

        public PayableAmount PaidAmount { get; set; }

        public string InstructionId { get; set; }

        public BillingPayment()
        {
            PaidAmount = new PayableAmount();
            Id = new PartyIdentificationId();
        }
    }
}
