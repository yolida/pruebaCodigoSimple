using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Description
    {
        /// <summary>
        /// [1..*]
        /// El detalle de la descripción
        /// El formato del Tag UBL es diferente a alfanumérico de 1 hasta 500 caracteres 
        /// Se considera cualquier carácter, permite "whitespace character": espacio, salto de línea, fin de línea, tab, etc.
        /// </summary>
        public string Detail { get; set; }
    }
}
