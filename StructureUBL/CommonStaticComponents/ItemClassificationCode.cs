using System;

namespace StructureUBL.CommonStaticComponents
{
    public class ItemClassificationCode: Attributes // Código de producto (SUNAT)
    {
        /// <summary>
        /// Código de producto (SUNAT) n8
        /// Se dejó el formato en string, debido a que SUNAT aun esta evaluando otras alternativas
        /// para este campo
        /// </summary>
        public string Value { get; set; }

        public ItemClassificationCode()
        {
            ListID          = "UNSPSC";
            ListAgencyName  = "GS1 US";
            ListName        = "Item Classification";
        }
    }
}
