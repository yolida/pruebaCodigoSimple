using System;

namespace StructureUBL.CommonStaticComponents
{
    public class AllowanceChargeReasonCode: Attributes
    {
        /// <summary>
        /// Catálogo No. 53: Códigos de cargos o descuentos
        /// Código del cargo/descuento del ítem | an2
        /// </summary>
        public string Value { get; set; }

        public AllowanceChargeReasonCode() {
            ListAgencyName  =   "PE:SUNAT";
            ListName        =   "Cargo/descuento";
            ListURI         =   "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53";
        }
    }
}
