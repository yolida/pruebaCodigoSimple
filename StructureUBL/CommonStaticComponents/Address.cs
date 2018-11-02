using StructureUBL.CommonAggregateComponents;
using System;

namespace StructureUBL.CommonStaticComponents
{
    /// <summary>
    /// cac:AddressLine | [0..*]
    /// </summary>
    [Serializable]
    public class Address
    {
        /// <summary>
        /// cac:AddressLine | [0..*] 
        /// Dirección completa y detallada
        /// </summary>
        public AddressLine AddressLine { get; set; }

        public string StreetName { get; set; } // Dirección del lugar en el que se entrega el bien (Dirección completa y detallada)

        /// <summary>
        /// Dirección del lugar en el que se entrega el bien (Urbanización)
        /// cbc:CitySubdivisionName | [0..1] 
        /// </summary>
        public string CitySubdivisionName { get; set; }

        /// <summary>
        /// cbc:CityName | [0..1]  
        /// Dirección del lugar en el que se entrega el bien (Provincia)
        /// </summary>
        public string CityName { get; set; }
        
        /// <summary>
        /// /Invoice/cac:Delivery/cac:DeliveryLocation/cac:Address/cbc:ID (Código de ubigeo)
        /// Catálogo No. 13
        /// </summary>
        public IdUbigeo Id { get; set; }

        /// <summary>
        /// Dirección del lugar en el que se entrega el bien (Departamento)
        /// cbc:CountrySubentity | [0..1]
        /// </summary>
        public string CountrySubentity { get; set; }

        /// <summary>
        /// n6 -- Dirección del lugar en el que se entrega el bien (Código de ubigeo)
        /// 
        /// </summary>
        public string CountrySubentityCode { get; set; } // Verificar, por que en la documentación ya no aparece

        public string District { get; set; } // Dirección del lugar en el que se entrega el bien (Distrito)

        /// <summary>
        /// cac:Country | [0..1] 
        /// </summary>
        public Country Country { get; set; }

        public Address()
        {
            Country = new Country();
            AddressLine = new AddressLine();
        }
    }
}
