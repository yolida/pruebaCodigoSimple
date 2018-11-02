using System;

namespace StructureUBL.CommonStaticComponents
{
    public class DurationMeasure: Attributes
    {
        public string Value { get; set; }

        public DurationMeasure()
        {
            UnitCode    = "DAY";
        }
    }
}
