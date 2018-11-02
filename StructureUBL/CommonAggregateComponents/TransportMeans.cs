using System;

namespace StructureUBL.CommonAggregateComponents
{
    public class TransportMeans
    {
        /// <summary>
        /// Número de constancia de inscripcion del vehiculo o certificado de habilitación vehicular
        ///  [0..1] | an..40
        /// </summary>
        public string RegistrationNationalityID { get; set; }

        /// <summary>
        ///  [0..1] | an..8
        /// </summary>
        public RoadTransport RoadTransport { get; set; }
    }
}
