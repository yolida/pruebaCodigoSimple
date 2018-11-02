using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class OriginAddress
    {
        /// Dirección del punto de partida (Código de ubigeo)
        /// /Invoice/cac:InvoiceLine/cac:Delivery/cac:Shipment/cac:OriginAddress/ => Ciudad o lugar de origen(Código de Ubigeo)
        public string CountrySubentityCode { get; set; }

        public AddressLine AddressLine { get; set; }

        public OriginAddressID Id { get; set; }

        public OriginAddress()
        {
            AddressLine = new AddressLine();
            Id          = new OriginAddressID();
        }
    }
}
