using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class Delivery
    {
        public DeliveryId       DeliveryId { get; set; }
        public Quantity         Quantity { get; set; }

        /// <summary>
        /// [0..1] 
        /// </summary>
        public DeliveryParty    DeliveryParty { get; set; }
        public Shipment         Shipment { get; set; }
        public ShipmentStage    ShipmentStage { get; set; }
        public DeliveryAddress  DeliveryAddress { get; set; }
        public MaximumQuantity  MaximumQuantity { get; set; }

        public Despatch         Despatch { get; set; }

        /// <summary>
        /// [0..1] | cac:DeliveryLocation
        /// </summary>
        public DeliveryLocation DeliveryLocation { get; set; }

        public Delivery()
        {
            DeliveryId          = new DeliveryId();
            Quantity            = new Quantity();
            DeliveryParty       = new DeliveryParty();
            Shipment            = new Shipment();
            ShipmentStage       = new ShipmentStage();
            MaximumQuantity     = new MaximumQuantity();
            DeliveryLocation    = new DeliveryLocation();
            Despatch            = new Despatch();
        }
    }
}
