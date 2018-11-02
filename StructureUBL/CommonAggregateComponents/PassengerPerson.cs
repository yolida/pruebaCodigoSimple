using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class PassengerPerson
    {
        public PassengerPersonId PassengerPersonId { get; set; } // Paquete turístico – Documento identidad del huésped

        /// Nombres y apellidos del pasajero
        /// an..100
        public string FirstName { get; set; }

        public PassengerPerson()
        {
            PassengerPersonId   = new PassengerPersonId();
        }
    }
}
