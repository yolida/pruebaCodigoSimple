using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class InvoiceTypeCode
    {
        public string ListAgencyName { get; set; }

        /// <summary>
        /// En la documentación del 2018/07/26 este atributo tiene el valor de @name, probablemente sea error de Sunat, por lo que 
        /// se esta dejando la etiqueta de acuerdo al estandar de oasis @listName
        /// </summary>
        public string ListName { get; set; }

        public string ListURI { get; set; }

        public string ListSchemeURI { get; set; }

        /// <summary>
        /// (catálogo 51) según el 'Tipo de documento'
        ///  Código de tipo de operación
        /// </summary>
        public string ListID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// cbc:InvoiceTypeCode
        /// CATALOGO No. 01
        /// Código de tipo de documento autorizado para efectos tributarios
        /// --------- Cambios despues del 2018/06/30 -----------
        /// Catálogo No. 04: Códigos de países
        /// ISO 3166-1: 2006 – Codes for the representation of names of countries and their subdivisions Part. 1: country codes
        /// </summary>
        public string Value { get; set; }

        public InvoiceTypeCode()
        {
            ListAgencyName  =   /*"SUNAT:Identificador de Tipo de Operación"*/ "PE:SUNAT";
            ListName        =   /*"Tipo de Documento"*/ "Tipo de Documento";
            ListURI         =   "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01";
            ListSchemeURI   =   "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo51"; // Nuevo
            Name            =   "Tipo de Operacion";
        }
    }
}
