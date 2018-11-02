using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class ResumenDiarioNuevo : DocumentoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public List<GrupoResumenNuevo> Resumenes { get; set; }
    }
}
