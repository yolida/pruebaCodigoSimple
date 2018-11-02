using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class UsabilityPeriod
    {
        /// <summary>
        /// <para>Fecha de inicio de la propiedad del ítem | Formato: yyyy-mm-dd</para>
        /// Fecha de descarga
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Fecha de fin de la propiedad del ítem
        /// Formato: yyyy-mm-dd
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Duración (días) de la propiedad del ítem | n4
        /// </summary>
        public DurationMeasure DurationMeasure { get; set; }

        public UsabilityPeriod()
        {
            DurationMeasure = new DurationMeasure();
        }
    }
}
