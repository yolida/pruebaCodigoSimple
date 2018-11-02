using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonStaticComponents;
using System;
using System.Collections.Generic;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class DeliveryParty
    {
        /// <summary>
        /// [0..*]  
        /// </summary>
        public List<PartyLegalEntity>       PartyLegalEntities { get; set; }

        public PartyLegalEntity_CompanyID   PartyLegalEntity_CompanyID { get; set; }

        public PartyIdentification          PartyIdentification { get; set; }

        public PartyName                    PartyName { get; set; }

        /// <summary>
        /// Código del país de residencia del huesped
        /// </summary>
        public PostalAddress                PostalAddress { get; set; }

        /// <summary>
        /// SUNAT lo considera de 0..1 pero OASIS es de 0..*, se tomará como valido la indicación de SUNAT.
        /// </summary>
        public Person                       Person { get; set; }

        /// <summary>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryParty
        /// Indicador de subcontratación(en caso de Factura Guia Transportista)
        /// <para>Indicador de subcontratación</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryParty/cbc:MarkAttentionIndicator
        /// </summary>
        public bool                         MarkAttentionIndicator { get; set; }

        public DeliveryParty()
        {
            PartyLegalEntities          = new List<PartyLegalEntity>();
            PartyLegalEntity_CompanyID  = new PartyLegalEntity_CompanyID();
            PartyIdentification         = new PartyIdentification();
            Person                      = new Person();
        }
    }
}
