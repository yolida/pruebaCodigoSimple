using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class DocumentCurrencyCode // Código de tipo de moneda en la cual se emite la factura electrónica
    {
        public string ListID { get; set; }

        public string ListName { get; set; }

        public string ListAgencyName { get; set; }

        public string Value { get; set; }

        public DocumentCurrencyCode()
        {
            ListID          = "ISO 4217 Alpha";
            ListName        = "Currency";
            ListAgencyName  = "United Nations Economic Commission for Europe";
        }
    }
}
