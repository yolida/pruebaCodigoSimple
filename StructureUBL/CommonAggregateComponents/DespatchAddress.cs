using StructureUBL.CommonStaticComponents;
using System;
using System.Collections.Generic;

namespace StructureUBL.CommonAggregateComponents
{
    public class DespatchAddress
    {
        /// <summary>
        /// Punto de origen - Código de ubigeo - Dirección detallada del origen
        /// </summary>
        public DespatchAddressID ID { get; set; }
        
        public AddressLine AddressLine { get; set; }

        public DespatchAddress()
        {
            ID          = new DespatchAddressID();
            AddressLine = new AddressLine();
        }
    }
}
