using System;
using Newtonsoft.Json;

namespace Models.Modelos
{
    public class SubTotalImpuestos
    {
        public int IdTotalImpuestos { get; set; }

        /// <summary>
        /// cbc:TaxableAmount
        /// Total Valor de Venta - Exportación
        /// Total valor de venta - operaciones inafectas
        /// Total valor de venta - operaciones exoneradas
        /// Total valor de venta - operaciones gravadas (IGV o IVAP)
        /// Total Importe IGV o IVAP
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal BaseImponible { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoMonedaBase { get; set; } // @currencyID | Código de tipo de moneda del monto de las operaciones gravadas/exoneradas/inafectas del impuesto

        /// <summary>
        /// cbc:TaxAmount
        /// Monto total del impuesto en particular
        /// Monto de la Sumatoria
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal MontoTotal { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoMonedaTotal { get; set; } // @currencyID | Código de tipo de moneda del monto total del impuesto

        /// <summary>
        /// En la versión antigua de la documentación toma los valores de la columna UN/ECE 5305- Duty or tax or fee category code*
        /// En la nueva versión esta columna se llama "NombreTributo del Tributo"
        /// cac:TaxCategory > cbc:ID
        /// Categoría de impuestos | [0..1]
        /// Esta etiqueta estuvo presente en la documentacióm antigua, pero en la actual ya no se le considera
        /// Ejemplo: <code> "cbc:ID schemeID="UN/ECE 5305" schemeName="Tax Category Identifier" schemeAgencyName="United Nations Economic Commission for Europe">E>/cbc:ID" </code>
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string CategoriaImpuestos { get; set; }

        /// <summary>
        /// Tasa del IGV o  Tasa del IVAP | Tasa del tributo
        /// Percent
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public decimal PorcentajeImp { get; set; }

        /// <summary>
        /// Código de tipo de afectación del IGV
        /// cbc:TaxExemptionReasonCode | 0..1 | an2 | Catálogo 07
        /// Catálogo No. 07: Códigos de tipo de afectación del IGV
        /// Listado de errores:
        /// {{ Si 'Código de tributo por línea' es diferente a '2000' (ISC) o '9999' (Otros tributos), cuyo 'Monto base' es mayor a cero
        /// (cbc:TaxableAmount > 0), y no existe el Tag UBL | ERROR 2371 |El XML no contiene el tag cbc:TaxExemptionReasonCode de Afectacion al IGV },
        /// { Si 'Código de tributo por línea' es igual a '2000' (ISC) o '9999' (Otros tributos), existe el tag UBL | ERROR 3050 | 
        /// Afectación de IGV no corresponde al código de tributo de la linea. },
        /// { Si 'Código de tributo por línea' es diferente a '2000' (ISC) o '9999' (Otros tributos), cuyo 'Monto base' es mayor a cero 
        /// (cbc:TaxableAmount > 0), el valor del Tag UBL es diferente al listado según su código de tributo. | ERROR 2040 | El tipo de afectacion del IGV es incorrecto },
        /// { Si 'Tipo de operación' es exportación '0200', '0201', '0202', '0203', '0204', '0205', '0206', '0207' o '0208', el valor del tag UBL es
        /// diferente a '40'. | ERROR 2642 | Operaciones de exportacion, deben consignar Tipo Afectacion igual a 40 },
        /// { Si 'Afectación al IGV o IVAP' es '17' y  'Monto base' es mayor a cero, y existe otra línea con 'Afectación al IGV o IVAP' diferente 
        /// de '17' y 'Monto base' mayor a cero | ERROR	2644 | Comprobante operacion sujeta IVAP solo debe tener ítems con código de afectación del IGV igual a 17 }
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string TipoAfectacion { get; set; }

        /// <summary>
        /// cbc:TierRange
        /// Código de tipo de sistema de ISC
        /// 0..1 | an2 | Catálogo 08
        /// Se considera irrelevante por parte de operaciones
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string TipoSistemaISC { get; set; }

        /// <summary>
        /// Listado de errores:
        /// {{ El valor del Tag UBL debe tener por lo menos uno de los siguientes valores en el comprobante: '1000' 
        /// (Gravada), '1016' (IVAP), '9995' (Exportacion), '9996' (Gratuita), '9997' (Exonerada), '9998' (Inafecta) |ERROR	3105 | 
        /// El XML debe contener al menos un tributo por linea de afectacion por IGV (Gravada, Exonerada, Inafecta, Exportación)
        /// Código de tributo
        /// cac:TaxScheme > cbc:ID | Código de tributo | [0..1] | an..3
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CodigoTributo { get; set; }

        /// <summary>
        /// Catálogo 5: Código de tipos de tributos y otros conceptos
        /// CodigoInternacional  =  cac:TaxScheme > cbc:TaxTypeCode | [0..1]
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CodigoInternacional { get; set; }

        /// <summary>
        /// Catálogo 5: Código de tipos de tributos y otros conceptos
        /// NombreTributo  =   cac:TaxScheme > cbc:Name | [0..1]
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string NombreTributo { get; set; }
    }
}
