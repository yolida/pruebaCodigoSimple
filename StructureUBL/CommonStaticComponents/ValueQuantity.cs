using System;

namespace StructureUBL.CommonStaticComponents
{
    public class ValueQuantity: Attributes
    {
        /// <summary>
        /// Cantidad de la Especie vendida | n(12,2)
        /// </summary>
        public decimal Value { get; set; }

        public ValueQuantity()
        {
            UnitCode = "TNE";
        }
    }
}
