using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class IdentificacionPais
    {
        public int IdIdentificacionPais { get; set; }

        public string CodigoNumerico { get; set; }

        public string  Alfa2 { get; set; }

        public string Alfa3 { get; set; }

        public string NombreCorto { get; set; }

        public string Nacionalidad { get; set; }
    }
}
