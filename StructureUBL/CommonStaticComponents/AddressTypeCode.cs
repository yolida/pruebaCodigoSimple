using System;

namespace StructureUBL.CommonStaticComponents
{
    /// <summary>
    /// PDF Anexo doc: Código asignado por SUNAT para el establecimiento anexo declarado en el RUC
    /// EXCEL 30/06/2018:  Código de local anexo donde se realiza la operación | sin detalles de sus atributos
    /// Código asignado por SUNAT para el establecimiento anexo declarado en el RUC. De informar un código distinto a 0000, 
    /// se verifi cará que corresponda al código del establecimiento anexo que SUNAT tiene registrado en sus sistemas. 
    /// El citado código puede ser revisado en la opción consulta de RUC de SUNAT Virtual.
    /// </summary>
    public class AddressTypeCode: Attributes
    {
        public string Value { get; set; }   // Se le esta asignado el valor por defecto ("0000"), ya que no se identifica el catálogo

        public AddressTypeCode()
        {
            SchemeAgencyName    = "PE:SUNAT";
            SchemeName          = "Establecimientos anexos";
            ListAgencyName      = "PE:SUNAT";
            ListName            = "Establecimientos anexos";
        }
    }
}
