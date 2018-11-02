using System;

namespace Models.Modelos
{
    /// <summary>
    /// <para>Información Adicional</para>
    /// En la bd esta en la tabla: [dbo].[PropiedadesAdicionales]
    /// /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty
    /// </summary>
    public class PropiedadAdicional
    {
        /// <summary>
        /// <para>NombreTributo del concepto | Catálogo No. 55 | Código de identificación del concepto tributario</para>
        /// /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty/cbc:Name
        /// <para> NombreTributo del concepto tributario | an..100 | [1..1] </para>
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// <para>Código del concepto | Catálogo No. 55 | Código de identificación del concepto tributario</para>
        /// /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty/cbc:NameCode
        /// <para>Código del concepto tributario (del ítem) | n4 | [0..1] </para>
        /// NombreTributo de columna en la base de datos: IdLeyendaDetraccion
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Aparentemente a esta campo se le puede poner una diversidad de valores
        /// <para>Número de asiento | an..20</para>
        /// /Invoice/cac:InvoiceLine/cac:Item/cac:AdditionalItemProperty/cbc:Value
        /// <para> Valor de la propiedad del ítem | an..100 antes, actualizado es an..20 | [0..1] </para>
        /// <para>Información de manifiesto de pasajero | an..20</para>
        /// <para>Número de documento de identidad del pasajero | an..15</para>
        /// <para>Tipo de documento de identidad del pasajero | Catálogo No. 06 | Código de tipo de documento de identidad | an1</para>
        /// <para>Nombres y apellidos del pasajero</para>
        /// <para>Ciudad o lugar de destino - Código de ubigeo | Catálogo No. 13 | an6 </para>
        /// <para>Ciudad o lugar de destino - Dirección detallada</para>
        /// <para>Ciudad o lugar de origen - Código de ubigeo | Catálogo No. 13 | an6 </para>
        /// <para>Ciudad o lugar de origen - Dirección detallada</para>
        /// </summary>
        public string ValorPropiedad { get; set; }

        /// <summary>
        /// 
        /// cbc:ValueQualifier
        /// Código del concepto del ítem
        /// SUNAT 0..n, pero se tomará como un 0..1
        /// an..4
        /// NombreTributo de columna en la base de datos: IdCodigoConcepto
        /// </summary>
        public string Concepto { get; set; }

        /// <summary>
        /// UsabilityPeriod > cbc:StartDate
        /// Fecha de inicio de la propiedad del ítem
        /// yyyy-mm-dd
        /// 0..1
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// UsabilityPeriod > cbc:EndDate
        /// Fecha de fin de la propiedad del ítem
        /// yyyy-mm-dd
        /// 0..1
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// UsabilityPeriod > cbc:DurationMeasure
        /// Duración (días) de la propiedad del ítem
        /// n..4 smallint
        /// 0..1
        /// </summary>
        public string Duracion { get; set; }

        /// <summary>
        /// n(12,2) | Cantidad de la Especie vendida
        /// </summary>
        public decimal CantidadEspecies { get; set; }
    }
}
