using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DespatchLineItem
    {
        public string Description { get; set; }

        /// <remarks>
        /// cac:SellersItemIdentification/cbc:ID
        /// </remarks>
        public string SellersIdentificationId { get; set; }
    }
}
