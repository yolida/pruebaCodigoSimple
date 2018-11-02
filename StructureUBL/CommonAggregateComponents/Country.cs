using StructureUBL.CommonStaticComponents;
using System;

namespace StructureUBL.CommonAggregateComponents
{
    /// <summary>
    /// "Pais del uso, explotación o aprovechamiento del servicio. - Código de país"
    /// cac:Country [0..1]
    /// Catálogo No. 04
    /// </summary>
    [Serializable]
    public class Country
    { // Dirección del lugar en el que se entrega el bien (Código de país)
        public IdentificationCode IdentificationCode { get; set; }
    }
}
