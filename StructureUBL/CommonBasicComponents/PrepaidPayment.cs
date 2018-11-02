using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class PrepaidPayment
    {
        public PrepaidPaymentId PrepaidPaymentId { get; set; }

        public PaidAmount       PaidAmount { get; set; }

        public InstructionID    InstructionID { get; set; }

        /// <summary>
        /// Fecha de pago
        /// </summary>
        public DateTime         PaidTime { get; set; }

        public PrepaidPayment()
        {
            PrepaidPaymentId    = new PrepaidPaymentId();
            PaidAmount          = new PaidAmount();
            InstructionID       = new InstructionID();
        }
    }
}
