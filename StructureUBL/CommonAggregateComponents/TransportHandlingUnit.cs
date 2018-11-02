using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    /// <summary>
    /// [0..*] según Oasis pero se tomará como [0..1]
    /// </summary>
    [Serializable]
    public class TransportHandlingUnit
    {
        public List<TransportEquipment> TransportEquipments { get; set; }

        public TransportHandlingUnit()
        {
            TransportEquipments = new List<TransportEquipment>();
        }
    }
}
