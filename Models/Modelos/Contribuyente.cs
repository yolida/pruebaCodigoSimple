using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class Contribuyente
    {
        /// <summary>
        /// Número de documento de identificación
        /// </summary>
        [JsonProperty(Order = 1, Required = Required.Always)]
        public string NroDocumento { get; set; }

        /// <summary>
        /// Tipo de documento de identidad
        /// Catálogo No. 06
        /// </summary>
        [JsonProperty(Order = 2, Required = Required.Always)]
        public string TipoDocumento { get; set; }

        /// <summary>
        /// NombreTributo Comercial
        /// </summary>
        [JsonProperty(Order = 4)]
        public string NombreComercial { get; set; }

        /// <summary>
        /// Apellidos y nombres, denominación o razón social
        /// NombreTributo o razón social del emisor
        /// </summary>
        [JsonProperty(Order = 3, Required = Required.Always)]
        public string NombreLegal { get; set; }

        [JsonProperty(Order = 5)]
        public string Ubigeo { get; set; } // n4 

        [JsonProperty(Order = 6)]
        public string Direccion { get; set; }

        [JsonProperty(Order = 7)]
        public string Urbanizacion { get; set; }

        [JsonProperty(Order = 9)]
        public string Provincia { get; set; }

        [JsonProperty(Order = 8)]
        public string Departamento { get; set; }

        [JsonProperty(Order = 10)]
        public string Distrito { get; set; }

        [JsonProperty(Order = 11)]
        public string Pais { get; set; }

        public string CorreoElectronico { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Contribuyente> OtrosParticipantes { get; set; }

        /// <summary>
        /// Se emplea sólo para los transportistas
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string RegistroMTC { get; set; }
    }
}
