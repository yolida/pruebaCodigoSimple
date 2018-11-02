using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class LocationCoordinate
    {
        public string LatitudeDirectionCode { get; set; } //Ubicación geográfica del medidor

        public string LongitudeDirectionCode { get; set; }
    }
}
