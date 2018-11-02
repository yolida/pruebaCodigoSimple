using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.Modelos
{
    public class DetalleDocumento
    {
        /// <summary>
        /// 
        /// </summary>
        public int IdDocumentoDetalle { get; set; }

        /// <summary>
        /// cbc:ID el ID de cada línea
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int NumeroOrden { get; set; }

        /// <summary>
        /// Valor de venta por ítem | n(12,2)
        /// Valor de venta total por cada ítem, producto, servicio, pertenece al nodo Item
        /// Si existe en la línea un cac:TaxSubTotal con 'Código de tributo por línea' igual a '9996' cuyo 'Monto base' es mayor a cero  (cbc:TaxableAmount > 0), 
        /// el importe es diferente al resultado de multiplicar el 'Valor referencial unitario por ítem en operaciones no onerosas' por 'Cantidad de unidades por
        /// ítem', menos los descuentos que afecten la base imponible del ítem ('Código de motivo de descuento' igual a '00') más los cargos que afecten la  base 
        /// imponible del ítem ('Código de motivo de cargo' igual a '47'),  con una tolerancia + - 1.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public decimal ValorVenta { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Moneda { get; set; }

        /// <summary>
        /// Cantidad de unidades por ítem
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Unidad de medida por ítem
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string UnidadMedida { get; set; }

        /// <summary>
        /// SellersItemIdentification | Código de producto del ítem
        /// /Invoice/cac:InvoiceLine/cac:Item/cac:SellersItemIdentification
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CodigoItem { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TipoPrecio { get; set; }

        public decimal Descuento { get; set; }

        /// <summary>
        /// LineExtensionAmount Es el valor de venta total por cada línea
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public decimal TotalVenta { get; set; }

        /// <summary>
        /// InvoiceLine > Item > CommodityClassification > ItemClassificationCode
        /// Código de producto (SUNAT)
        /// </summary>
        [JsonProperty(Required = Required.AllowNull)]
        public string CodigoProductoSunat { get; set; }

        [JsonProperty(Required = Required.Always)]
        public List<Descripcion> Descripciones { get; set; }
        
        [JsonProperty(Required = Required.AllowNull)]
        public List<PrecioAlternativo> PreciosAlternativos { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Descuento> Descuentos { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<TotalImpuesto> TotalImpuestos { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<PropiedadAdicional> PropiedadesAdicionales { get; set; }

        public List<Entrega> Entregas { get; set; }
        public List<Nota> Notas { get; set; }

        public DetalleDocumento()
        {
            Descripciones           =   new List<Descripcion>();
            PreciosAlternativos     =   new List<PrecioAlternativo>();
            Descuentos              =   new List<Descuento>();
            TotalImpuestos          =   new List<TotalImpuesto>();
            PropiedadesAdicionales  =   new List<PropiedadAdicional>();
            Entregas                =   new List<Entrega>();
            Notas                   =   new List<Nota>();
        }
    }
}
