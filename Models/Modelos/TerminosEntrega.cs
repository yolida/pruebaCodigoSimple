using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class TerminosEntrega
    {
        [JsonProperty(Required = Required.AllowNull)]
        public string NumeroRegistro { get; set; }  // cbc:ID = Numero de registro MTC

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoMoneda { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public decimal Monto { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Direccion { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Urbanizacion { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Provincia { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Departamento { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string Ubigeo { get; set; } // Código de ubigeo, distinto a id de fila, hacer una consulta para obtener el codigo mediante Id

        [JsonProperty(Required = Required.AllowNull)]
        public string Distrito { get; set; }

        public string Alfa2 { get; set; }   // Codigo de país

        [JsonProperty(Required = Required.AllowNull)]
        public IdentificacionPais IdentificacionPais { get; set; }
    }
}
