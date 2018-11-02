using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class RegistrationAddress
    {
        /// <summary>
        /// Código del domicilio fiscal o de local anexo del emisor
        /// n4
        /// Se le asignará como string debido a que el base de datos se registró como nvarchar
        /// </summary>
        public AddressTypeCode AddressTypeCode { get; set; }

        public AddressLine AddressLine { get; set; }

        /// <summary>
        /// cbc:CitySubdivisionName | [0..1]
        /// Urbanización
        /// </summary>
        public string CitySubdivisionName { get; set; }

        /// <summary>
        /// cbc:CityName | [0..1]
        /// Provincia
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// cbc:ID | [0..1]
        /// Código de ubigeo
        /// Catálogo No. 13: Ubicación geográfica (UBIGEO)
        /// Descripción: Código de Ubicación Geográfica (UBIGEO)
        /// Código de Ubicación Geográfica (UBIGEO)
        /// </summary>
        public IdUbigeo IdUbigeo { get; set; }

        /// <summary>
        /// cbc:CountrySubentity | [0..1] 
        /// Departamento
        /// </summary>
        public string CountrySubentity { get; set; }

        /// <summary>
        /// cbc:District | [0..1]
        /// Distrito 
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Catálogo No. 04: Códigos de países
        /// ISO 3166-1: 2006 – Codes for the representation of names of countries and their subdivisions Part. 1: country codes
        /// </summary>
        public Country Country { get; set; }

        public RegistrationAddress()
        {
            AddressLine = new AddressLine();
            Country = new Country();
        }
    }
}
