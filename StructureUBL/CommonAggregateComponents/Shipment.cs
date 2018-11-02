using System;
using System.Collections.Generic;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Shipment
    {
        public ShipmentId           Id { get; set; }
        public HandlingCode         HandlingCode { get; set; }
        public string               Information { get; set; }
        public bool                 SplitConsignmentIndicator { get; set; }

        /// <summary>
        /// Peso bruto total de la Factura
        /// Catálogo No. 03: Códigos de tipo de unidad de medida comercial
        /// 0..1
        /// </summary>
        public InvoicedQuantity     GrossWeightMeasure { get; set; }
        public int                  TotalTransportHandlingUnitQuantity { get; set; }
        public List<ShipmentStage>  ShipmentStages { get; set; }
        public PostalAddress        DeliveryAddress { get; set; }

        /// <summary>
        /// Información de vehículos secundarios
        /// [0..*] | an..8 | [0..*] según Oasis pero se tomará como [0..1]
        /// </summary>
        public TransportHandlingUnit TransportHandlingUnit { get; set; }
        //public PostalAddress OriginAddress { get; set; }
        public OriginAddress        OriginAddress { get; set; }
        public string               FirstArrivalPortLocationId { get; set; }
        public Delivery             Delivery { get; set; } // Delivery

        public Shipment()
        {
            Id                      = new ShipmentId();
            GrossWeightMeasure      = new InvoicedQuantity();
            ShipmentStages          = new List<ShipmentStage>();
            DeliveryAddress         = new PostalAddress();
            TransportHandlingUnit   = new TransportHandlingUnit();
            OriginAddress           = new OriginAddress();
        }
    }
}
