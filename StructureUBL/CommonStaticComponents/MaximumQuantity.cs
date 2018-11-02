using System;

namespace StructureUBL.CommonStaticComponents
{
    public class MaximumQuantity
    {
        public string UnitCode { get; set; } // Potencia contratada (Servicios públicos) n5

        public string UnitCodeListID { get; set; }

        public string UnitCodeListAgencyName { get; set; }

        public int Value { get; set; }

        public MaximumQuantity()
        {
            //UnitCode Potencia contratada (Servicios públicos) n5
            UnitCodeListID = "UN/ECE rec 20";
            UnitCodeListAgencyName = "United Nations Economic Commission for Europe";
        }
    }
}
