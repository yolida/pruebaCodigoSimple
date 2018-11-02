using System;

namespace StructureUBL.CommonStaticComponents
{
    public class OriginAddressID: Attributes
    {
        /// <summary>
        /// <para>Dirección punto de partida - Código de ubigeo | Catálogo No. 13</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:OriginAddress/cbc:ID
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Dirección punto de partida - Código de ubigeo
        /// </summary>
        public OriginAddressID()
        {
            SchemeAgencyName = "Dirección punto de partida - Código de ubigeo";
            SchemeName      = "Ubigeos";
        }
    }
}
