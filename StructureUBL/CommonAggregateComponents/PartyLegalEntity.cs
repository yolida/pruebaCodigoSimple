using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PartyLegalEntity
    {
        /// <summary>
        /// <remarks> Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Apellidos y nombres o razón social </remarks>
        /// <para>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cacPartyLegalEntity/cbc:RegistrationName</para>
        /// </summary>
        public string RegistrationName { get; set; }

        /// <summary>
        /// Número de documento de identidad del destinatario
        /// </summary>
        public CompanyID CompanyID { get; set; }

        public string Name { get; set; } // Apellidos y Nombres o denominacion o razon social del transportista

        public RegistrationAddress RegistrationAddress { get; set; }

        public List<ShareholderParty> ShareholderParties { get; set; }

        public PartyLegalEntity() {
            CompanyID           = new CompanyID();
            RegistrationAddress = new RegistrationAddress();
            ShareholderParties = new List<ShareholderParty>();
        }
    }
}
