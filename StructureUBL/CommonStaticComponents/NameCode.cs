using System;

namespace StructureUBL.CommonStaticComponents
{
    [Serializable]
    public class NameCode: Attributes
    {
        /// <summary>
        /// <para>Código del concepto tributario (del ítem) | n4</para>
        /// De existir código de concepto igual '7000' y no existe el tag
        /// Catálogo No. 55 | Código de identificación del concepto tributario 
        /// </summary>
        public string Value { get; set; }

        public NameCode()
        {
            ListName        = /*"SUNAT:Identificador de la propiedad del ítem"*/ "Propiedad del item";
            ListAgencyName  = "PE:SUNAT";
            ListURI         = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55";
        }
    }
}