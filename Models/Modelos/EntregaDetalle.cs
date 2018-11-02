using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class EntregaDetalle
    {
        /// <summary>
        /// /Invoice/cac:Delivery/cac:DeliveryParty/cac:PartyLegalEntity
        /// cbc:CompanyID | 0..n      
        /// <remarks>
        /// <para> Value   = Número de documento de identidad del destinatario <value>NroDocumento</value></para>
        /// <para> schemeID = Código de tipo de documento de identidad del destinatario <value>TipoDocumento</value></para>
        /// </remarks>
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public List<Contribuyente> NroDocDestinatarios { get; set; }

        /// <summary>
        /// <para>Indicador de subcontratación</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryParty/cbc:MarkAttentionIndicator
        /// </summary>
        public bool IndSubContratacion { get; set; }
    }
}
