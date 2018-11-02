using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class OrderReference
    {
        /*-----------------------------------------------------------------------------------------------------
        If is Invoice
            Id = Número de la orden de compra
        -----------------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Número de la orden de compra
        /// </summary>
        public string Id { get; set; }

        public OrderTypeCode OrderTypeCode { get; set; }

        public OrderReference()
        {
            OrderTypeCode = new OrderTypeCode();
        }
    }
}
