using System;
using StructureUBL.CommonBasicComponents;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PartyIdentification
    {
        /// <summary>
        /// cbc:ID [1..1]    An identifier for the party.
        /// </summary>
        public PartyIdentificationId Id { get; set; }

        /// <summary>
        /// Número de constancia de inscripcion del vehiculo o certificado de
        /// habilitación vehicular => Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty
        /// </summary>
        public string RegistrationNationalityID { get; set; }

        public PartyIdentification()
        {
            Id = new PartyIdentificationId();
        }
    }
}
