using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class PaymentTerms
    {
        /// <summary>
        /// Código del bien o producto sujeto a detracción 
        /// </summary>
        public PaymentTermsId   PaymentTermsId  { get; set; }

        /// <summary>
        /// Porcentaje de la detracción | n(3,2)
        /// </summary>
        public decimal          PaymentPercent  { get; set; }

        /// <summary>
        /// Monto de la detracción
        /// </summary>
        public decimal          Amount          { get; set; }

        public PaymentTerms()
        {
            PaymentTermsId = new PaymentTermsId();
        }
    }
}
