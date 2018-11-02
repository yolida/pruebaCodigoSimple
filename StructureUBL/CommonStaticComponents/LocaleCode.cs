using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class LocaleCode: Attributes // Código de Servicios de Telecomunicaciones (De corresponder)
    {
        public string Value { get; set; }
        public LocaleCode()
        {
            ListAgencyName  =   "PE:SUNAT";
            ListName        =   "SUNAT:Identificador de Servicio de Telecomunicación";
            ListURI         =   "urn:pe:gob:sunat:cpe:see:gem: catalogos: catalogo57";
        }
    }
}
