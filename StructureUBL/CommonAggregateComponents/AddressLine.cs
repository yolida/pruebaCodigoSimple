using System;

namespace StructureUBL.CommonAggregateComponents
{
    /// <summary>
    /// cac:AddressLine | [0..*]  
    /// </summary>
    [Serializable]
    public class AddressLine
    {
        /// <summary>
        /// cbc:Line | [1..1]
        /// Direccion del punto de llegada (Dirección completa y detallada)
        /// <para>Detracciones - Servicio de transporte de Carga</para>
        /// </summary>
        public string Line { get; set; }
    }
}
