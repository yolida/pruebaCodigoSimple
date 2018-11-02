using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    /// <summary>
    /// NombreTributo de la tabla: MediosPago
    /// </summary>
    public class MedioPago
    {
        [JsonProperty(Required = Required.AllowNull)]
        public string NroCuenta { get; set; }

        public string CodigoCatalogo { get; set; }

        /// <summary>
        /// Servicio de transporte: Número de autorización de la transacción y el sistema de tarjeta de crédito y/o débito 
        /// </summary>
        public string Autorizacion { get; set; }
    }
}
