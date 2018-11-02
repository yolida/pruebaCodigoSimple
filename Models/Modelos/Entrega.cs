using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Entrega
    {
        /// <summary>
        /// cbc:ID | n1
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string                       Codigo { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public int                          Cantidad { get; set; } // n..10

        [JsonProperty(Required = Required.AllowNull)]
        public int                          MaximaCantidad { get; set; } // n..5

        public List<UbicacionCoordinada>    UbicacionesCoordinadas { get; set; } // The geographical coordinates of this location.

        public Envio                        Envio { get; set; } // Shipment

        public EntregaDetalle               EntregaDetalle { get; set; }

        /// <summary>
        /// <para>Dirección completa y detallada</para>
        /// /Invoice/cac:Delivery/cac:DeliveryLocation/cac:Address/cac:AddressLine/cbc:Line 
        /// <para>Información Adicional - Sustento de traslado de mercaderias</para>
        /// <para>Dirección punto de llegada - Dirección completa y detallada</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryAddress/cac:AddressLine/cbc:Line
        /// </summary>
        public string Direccion     { get; set; }

        /// <summary>
        /// /Invoice/cac:Delivery/cac:DeliveryLocation/cac:Address/cbc:CitySubdivisionName (Urbanización)
        /// </summary>
        public string Urbanizacion  { get; set; }

        public string Provincia     { get; set; }

        public string Ubigeo        { get; set; }

        public string Departamento  { get; set; }

        public string Distrito      { get; set; }

        public string Pais          { get; set; }

        public Entrega()
        {
            UbicacionesCoordinadas  = new List<UbicacionCoordinada>();
            Envio                   = new Envio();
            EntregaDetalle          = new EntregaDetalle();
        }
    }
}
