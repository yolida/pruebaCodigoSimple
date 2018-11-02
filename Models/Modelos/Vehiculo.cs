using System;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class Vehiculo
    {
        /// <summary>
        /// <para> Número de constancia de inscripcion del vehiculo o certificado de habilitación vehicular </para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cac:TransportMeans/cbc:RegistrationNationalityID
        /// </summary>
        public string Constancia { get; set; }

        /// <summary>
        /// <para>Información de vehículo principal - Número de placa</para>
        /// /Invoice/cac:Delivery/cac:Shipment/cac:ShipmentStage/cacVehcac:RoadTransport/cbc:LicensePlateID
        /// </summary>
        public string Placa { get; set; }
    }
}
