using System;
using System.Collections.Generic;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;
using StructureUBL.SunatAggregateComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class ShipmentStage
    {
        public int                  Id { get; set; }
        public List<CarrierParty>   CarrierParties { get; set; }

        /// <summary>
        /// Fecha de inicio del traslado o fecha de entrega de bienes al transportista
        /// 0..1 | yyyy-mm-dd
        /// </summary>
        public TransitPeriod        TransitPeriod { get; set; }

        /// <summary>
        /// <para>Datos de conductores - Número de documento de identidad</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:DriverPerson/cbc:ID
        /// </summary>
        public List<PartyIdentification>  DriverPersons { get; set; }

        public SubDelivery          SubDelivery { get; set; }
        public TransportModeCode    TransportModeCode { get; set; } // Modalidad de Transporte

        /// <remarks>
        /// cac:TransitPeriod/cbc:StartDate
        /// </remarks>>
        public DateTime             TransitPeriodStartPeriod { get; set; }
        public TransportMeans       TransportMeans { get; set; }

        /// <summary>
        /// Número de Asiento (Transporte de Pasajeros)
        /// schemeID => Información de Manifiesto de pasajero
        /// </summary>
        public ShipmentStageId      ShipmentStageId { get; set; }

        public PlannedDepartureTransportEvent PlannedDepartureTransportEvent { get; set; }
        public PassengerPerson      PassengerPerson { get; set; }

        public ShipmentStage()
        {
            CarrierParties      = new List<CarrierParty>();
            TransitPeriod       = new TransitPeriod();
            TransportModeCode   = new TransportModeCode();
            DriverPersons       = new List<PartyIdentification>(); 
            TransportMeans      = new TransportMeans();
            ShipmentStageId     = new ShipmentStageId();
            SubDelivery         = new SubDelivery();
            PlannedDepartureTransportEvent = new PlannedDepartureTransportEvent();
            PassengerPerson     = new PassengerPerson();
        }
    }
}
