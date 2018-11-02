using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    /// <summary>
    /// cbc:CustomizationID | [0..1]
    /// </summary>
    public class CustomizationID: Attributes
    {
        public string Value { get; set; }

        public CustomizationID() {
            SchemeAgencyName    =   "PE:SUNAT";
        }
    }
}
