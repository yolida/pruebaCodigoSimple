using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PayeeFinancialAccount
    {
        /// <summary>
        /// Número de cta. en el Banco de la Nación (detraccion)
        /// <para>Número de cuenta | an..100</para>
        /// /Invoice/cac:PaymentMeans/cac:PayeeFinancialAccount/cbc:ID
        /// </summary>
        public string Id { get; set; }
    }
}
