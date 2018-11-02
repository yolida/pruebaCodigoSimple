using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonBasicComponents
{
    [Serializable]
    public class InvoicedQuantity // Cantidad de unidades del ítem
    {
        /// <summary>
        /// Código de unidad de medida an3, Código de unidad de medida del ítem
        /// </summary>
        public string UnitCode { get; set; }

        public string UnitCodeListVersionID { get; set; } // Agregar manualente este valor de atributo: UN/ECE rec 20 Revision 4

        public string UnitCodeListID { get; set; }

        public string UnitCodeListAgencyName { get; set; }

        /// <summary>
        /// Peso bruto total n(12,3)
        /// </summary>
        public decimal Value { get; set; }

        public InvoicedQuantity()
        {
            UnitCodeListID          = "UN/ECE rec 20";
            UnitCodeListAgencyName  = "United Nations Economic Commission for Europe";
        }
    }
}
