using StructureUBL.CommonStaticComponents;
using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DeliveryAddress
    {
        public string CountrySubentityCode { get; set; } // Direccion del punto de llegada (Código de ubigeo) n6

        /// <summary>
        /// [0..*] según Oasis pero se tomará como [0..1]
        /// </summary>
        public AddressLine AddressLine { get; set; }

        /// <summary>
        /// <para>Dirección punto de llegada - Código de ubigeo | Catálogo No. 13</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryAddress/cbc:ID
        /// </summary>
        public DeliveryAddressId Id { get; set; }

        public DeliveryAddress()
        {
            AddressLine = new AddressLine();
            Id          = new DeliveryAddressId();
        }
    }
}
