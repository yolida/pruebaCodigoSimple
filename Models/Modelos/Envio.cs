using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models.Modelos
{
    /// <summary>
    /// /Invoice/cac:Delivery/cac:Shipment
    /// cac:Shipment [0..1]  
    /// </summary>
    public class Envio
    {
        /// <summary>
        /// Código de motivo de traslado
        /// Catálogo No. 20: Códigos – Motivos de traslado
        /// /Invoice/cac:Delivery/cac:Shipment/cbc:ID
        /// Código de motivo del traslado >> HandlingCode
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string MotivoTranslado { get; set; }

        /// <summary>
        /// Peso bruto total de la Factura
        /// /Invoice/cac:Delivery/cac:Shipment/cbc:GrossWeightMeasure
        /// </summary>
        public decimal PesoBruto    { get; set; }

        public string UnidadMedida  { get; set; }

        /// <summary>
        /// /Invoice/cac:Delivery/cac:Shipment
        /// cbc:ID > Value
        /// Código de motivo del traslado
        /// Catálogo No. 20: Códigos – Motivos de traslado
        /// NombreTributo de la tabla en la base de datos: [FeiContDB].[dbo].[MotivoTranslados]
        /// </summary>
        public string CodMotivoTraslado { get; set; }

        /// <summary>
        /// ShipmentStage
        /// </summary>
        public List<EtapaEnvio> EtapaEnvios { get; set; }

        /// <summary>
        /// <para>Información de vehículos secundarios</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:TransportHandlingUnit/cac:TransportEquipment/cbc:ID
        /// </summary>
        public List<Vehiculo>   Vehiculos { get; set; }

        public Entrega  Entrega { get; set; }

        /// <summary>
        /// <para>Dirección punto de partida - Código de ubigeo | Catálogo No. 13</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:OriginAddress/cbc:ID
        /// </summary>
        public string PuntoLlegadaUbigeo { get; set; }

        /// <summary>
        /// <para>Dirección punto de llegada - Dirección completa y detallada</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:Delivery/cac:DeliveryAddress/cac:AddressLine/cbc:Line
        /// </summary>
        public string PuntoLlegadaDireccion { get; set; }

        /// <summary>
        /// <para>Dirección punto de partida - Código de ubigeo</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:OriginAddress/cbc:ID
        /// </summary>
        public string PuntoPartidaUbigeo { get; set; }

        /// <summary>
        /// <para>Dirección punto de partida - Dirección completa y detallada</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:OriginAddress/cac:AddressLine/cbc:Line
        /// </summary>
        public string PuntoPartidaDireccion { get; set; }
    }
}