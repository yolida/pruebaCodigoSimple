using StructureUBL.CommonAggregateComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class DeliveryTerms
    {
        public string Id { get; set; }

        public Amount Amount { get; set; }

        public DeliveryLocation DeliveryLocation { get; set; }

        public DeliveryTerms()
        {
            Amount              = new Amount();
            DeliveryLocation    = new DeliveryLocation();
        }
    }
}
