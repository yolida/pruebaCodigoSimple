using System;

namespace StructureUBL.CommonStaticComponents
{
    public class PassengerPersonId: Attributes
    {
        public string Value { get; set; } // Número de documento de identidad del pasajero

        public PassengerPersonId()
        {
            SchemeName          = "SUNAT:Identificador de Documento de Identidad";
            SchemeAgencyName    = "PE:SUNAT";
        }
    }
}
