using Newtonsoft.Json;
using Models.Contratos;
using System.Collections.Generic;
using System;

namespace Models.Modelos
{
    public class DocumentoElectronico : IDocumentoElectronico
    {
        [JsonProperty(Required = Required.Always)]
        public string TipoDocumento     { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Contribuyente Emisor     { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Contribuyente Receptor   { get; set; }

        /// <summary>
        /// SellerSupplierParty
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Contribuyente Proveedor  { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime FechaEmision    { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String HoraEmision     { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime FechaVencimiento { get; set; }

        public string OrdenCompra       { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<DocumentoContrato> DocumentoContratos { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Moneda            { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string TipoOperacion     { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int CantidadItems      { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:LineExtensionAmount
        /// Total Valor de Venta
        /// </summary>
        public decimal TotalValorVenta  { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount
        /// Total Precio de Venta
        /// </summary>
        public decimal TotalPrecioVenta { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount
        /// Total Descuentos (Que no afectan la base)
        /// </summary>
        public decimal TotalDescuento   { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount
        /// Total otros Cargos (Que no afectan la base)
        /// </summary>
        public decimal TotalOtrosCargos { get; set; }

        public decimal TotalAnticipos   { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount
        /// Importe total
        /// </summary>
        public decimal ImporteTotalVenta    { get; set; }

        /// <summary>
        /// /Invoice/cac:LegalMonetaryTotal/cbc:PayableRoundingAmount
        /// Monto para Redondeo del Importe Total | Nuevo
        /// </summary>
        public decimal MontoRedondeo { get; set; }

        // Varias entidades que pertenecen a este documento  

        [JsonProperty(Required = Required.Always)]
        public List<DetalleDocumento> DetalleDocumentos { get; set; } // InvoiceLine
        
        [JsonProperty(Required = Required.AllowNull)]
        public List<PeriodoFactura> PeriodosFactura     { get; set; }
        
        [JsonProperty(Required = Required.AllowNull)]
        public List<DocumentoRelacionado> Relacionados  { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<DocumentoRelacionado> OtrosDocumentosRelacionados { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Entrega> Entregas   { get; set; }

        public string SerieCorrelativo { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public TerminosEntrega TerminosEntrega { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<MedioPago> MedioPagos   { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<TerminosPago> TerminosPagos { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Anticipo> Anticipos     { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Descuento> Descuentos   { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<TotalImpuesto> TotalImpuestos { get; set; }

        public decimal DescuentoGlobal  { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<Nota> Notas { get; set; }

        public List<DatoAdicional> DatoAdicionales { get; set; }
        
        [JsonProperty(Required = Required.AllowNull)]
        public List<Discrepancia> Discrepancias { get; set; }

        public DocumentoElectronico()
        {
            Emisor                      = new Contribuyente();
            Receptor                    = new Contribuyente();
            DocumentoContratos          = new List<DocumentoContrato>();
            DetalleDocumentos           = new List<DetalleDocumento>();
            DatoAdicionales             = new List<DatoAdicional>();
            Relacionados                = new List<DocumentoRelacionado>();
            OtrosDocumentosRelacionados = new List<DocumentoRelacionado>();
            Entregas                    = new List<Entrega>();
            TerminosEntrega             = new TerminosEntrega();
            MedioPagos                  = new List<MedioPago>();
            TerminosPagos               = new List<TerminosPago>();
            Anticipos                   = new List<Anticipo>();
            Descuentos                  = new List<Descuento>();
            TotalImpuestos              = new List<TotalImpuesto>();
            Discrepancias               = new List<Discrepancia>();
            Notas                       = new List<Nota>();
        }
    }
}