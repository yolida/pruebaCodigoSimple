using System;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class EtapaEnvio
    {
        /// <summary>
        /// <para> Tabla sn bd: ModalidadTransporte
        /// Catálogo No. 18
        /// Modalidad de Transporte. Dato exclusivo para la Factura Guía Remitente (FG Remitente)
        /// </para>
        /// <remarks>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cbc:TransportModeCode</remarks>
        /// </summary>
        public string               ModalidadTransporte { get; set; }

        /// <summary>
        /// <para> Fecha de inicio del traslado o fecha de entrega de bienes al transportista </para>
        /// <remarks> /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:TransitPeriod/cbc:StartDate </remarks>
        /// </summary>
        public DateTime             FechaInicioTraslado { get; set; }

        /// <summary>
        /// <para>Value
        /// Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Número de documento de identidad</para>
        /// <remarks>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cac:PartyIdentification/cbc:ID </remarks>
        /// <para> SchemaID
        /// Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Tipo de documento de identidad</para>
        /// <remarks>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cac:PartyIdentification/cbc:ID@schemeID (Tipo de documento de identidad)</remarks>
        /// <para>Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Apellidos y nombres o razón social </para>
        /// <remarks> /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cacPartyLegalEntity/cbc:RegistrationName</remarks>
        /// <para>Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Registro del MTC </para>
        /// <remarks>/Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:CarrierParty/cacPartyLegalEntity/cbc:CompanyID</remarks>
        public List<Contribuyente>  Transportistas { get; set; }

        /// <summary>
        /// Incluye los datos de las etiquetas:
        /// /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:TransportMeans/cbc:RegistrationNationalityID
        /// /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cacVehcac:RoadTransport/cbc:LicensePlateID
        /// </summary>
        public Vehiculo             Vehiculo { get; set; }

        /// <summary>
        /// Tabla de db: Conductor_EtapaEnvio heredado de la tabla Contribuyentes
        /// </summary>
        public List<Contribuyente> Conductores { get; set; }

        public EtapaEnvio()
        {
            Transportistas  = new List<Contribuyente>();
            Vehiculo        = new Vehiculo();
            Conductores     = new List<Contribuyente>();
        }
    }
}
