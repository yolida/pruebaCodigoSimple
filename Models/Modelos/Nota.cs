using Newtonsoft.Json;

namespace Models.Modelos
{
    public class Nota
    {
        public int IdNota { get; set; }

        /// <summary>
        /// /Invoice/cbc:Note  (Descripción de la leyenda)
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string Descripcion { get; set; }

        /// <summary>
        /// /Invoice/cbc:Note@languageLocaleID (Código de la leyenda)
        /// Catálogo No. 52 | Códigos de leyendas
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string Codigo { get; set; }
    }
}
