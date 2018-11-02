using System;

namespace StructureUBL.CommonAggregateComponents
{
    [Serializable]
    public class Note
    {
        public string LanguageLocaleID { get; set; } //Código de leyenda

        public string Value { get; set; } // Leyenda an..100
    }
}
