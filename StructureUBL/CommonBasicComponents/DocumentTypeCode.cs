using System;

namespace StructureUBL.CommonBasicComponents
{
    public class DocumentTypeCode
    {
        public string ListAgencyName { get; set; }
        public string ListName { get; set; }
        public string ListURI { get; set; }

        /// <summary>
        /// Código de tipo de documento relacionado con la operación que se factura. Catalogo 12. | 
        /// Factura - Emitida por Anticipo = "02" | Boleta - Emitida por Anticipo = "03"
        /// </summary>
        public string Value { get; set; }

        public DocumentTypeCode() {
            ListAgencyName = "PE:SUNAT";
        }
    }
}
