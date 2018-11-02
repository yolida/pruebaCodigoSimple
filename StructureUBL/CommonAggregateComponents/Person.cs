using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Person
    {
        public PersonId PersonId { get; set; } // Paquete turístico – Documento identidad del huésped

        /// Paquete turístico – Número de documento identidad de huésped
        /// an..100
        public string FirstName { get; set; }

        public Person()
        {
            PersonId    = new PersonId();
        }
    }
}
