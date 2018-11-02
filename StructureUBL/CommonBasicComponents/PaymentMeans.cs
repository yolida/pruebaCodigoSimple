using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class PaymentMeans
    {
        /// <summary>
        /// Servicio de transporte: Número de autorización de la transacción y el sistema de tarjeta de crédito y/o débito 
        /// </summary>
        public string PaymentID { get; set; }

        /// <summary>
        /// Cuenta del banco de la nacion (detraccion)
        /// [0..1]
        /// cbc:ID
        /// </summary>
        public PayeeFinancialAccount PayeeFinancialAccount { get; set; }

        /// <summary>
        /// Catálogo No. 59: Medios de Pago
        /// </summary>
        public PaymentMeansCode PaymentMeansCode { get; set; }

        public PaymentMeans()
        {
            PayeeFinancialAccount   = new PayeeFinancialAccount();
            PaymentMeansCode        = new PaymentMeansCode();
        }
    }
}
