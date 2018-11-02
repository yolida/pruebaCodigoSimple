using System;

namespace StructureUBL.CommonStaticComponents
{
    public class DeliveryAddressId: Attributes
    {
        /// <summary>
        /// <para>Dirección punto de llegada - Código de ubigeo | Catálogo No. 13</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryAddress/cbc:ID
        /// </summary>
        public string Value { get; set; }

        public DeliveryAddressId()
        {
            SchemeAgencyName    = "Dirección punto de partida - Código de ubigeo";
            SchemeName          = "Ubigeos";
        }
    }
}
