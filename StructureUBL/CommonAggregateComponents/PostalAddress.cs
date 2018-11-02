using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PostalAddress
    {
        public string Id { get; set; }

        /// <summary>
        /// Dirección completa y detallada
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Urbanización
        /// </summary>
        public string CitySubdivisionName { get; set; }

        /// <summary>
        /// Provincia
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Código de ubigeo
        /// </summary>
        public string PostalZone { get; set; }

        /// <summary>
        /// Departamento
        /// </summary>
        public string CountrySubentity { get; set; }

        /// <summary>
        /// Distrito
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Código de país
        /// </summary>
        public Country Country { get; set; }

        public string CountrySubentityCode { get; set; } // Direccion del punto de partida (Código de ubigeo)

        public PostalAddress()
        {
            Country = new Country();
        }
    }
}
