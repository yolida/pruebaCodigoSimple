using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class TransitPeriod
    {
        /// <summary>
        /// Fecha de inicio del traslado o fecha de entrega de bienes al transportista
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
