using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonStaticComponents;
using System;
using System.Collections.Generic;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class DeliveryLocation
    {
        public List<LocationCoordinate> LocationsCoordinate { get; set; } // n..*

        /// <summary>
        /// cac:Address | [0..1] 
        /// </summary>
        public Address Address { get; set; }

        public DeliveryLocation()
        {
            LocationsCoordinate = new List<LocationCoordinate>();
            Address             = new Address();
        }
    }
}