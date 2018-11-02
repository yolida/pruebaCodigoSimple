using System;

namespace StructureUBL.CommonAggregateComponents
{
    public class PlannedDepartureTransportEvent
    {
        /// Fecha de inicio programado
        /// Formato: yyyy-mm-dd
        public DateTime OccurrenceDate { get; set; }

        /// Hora de inicio programado
        /// Formato: hh:mm:ss.0z
        public DateTime OccurrenceTime { get; set; }
    }
}
