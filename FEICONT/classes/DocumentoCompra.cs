using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT.classes
{
    public class DocumentoCompra : INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string tipo;
        private string estadoValidar;
        private string estadoValidarTexto;
        private string estadoVerificar;
        private string estadoVerificarTexto;
        private string estadoSunat;
        private string estadoSunatCodigo;
        private string serieNumero;
        private string fechaEmision;
        private string ruc;
        private string razonSocial;
        private string comentario;
        private string tipoTexto;
        private string monto;
        private string numeroRelacionado;
        private string tipoRelacionado;

        private string idLinea;
        private string movimiento;
        private string serie;
        private string numero;
        private string fechaVencimiento;
        private string fechaVencimientoDos;
        private string pago;
        private string moneda;
        private string condicionPago;
        private string condicionCompra;
        private string tipoCambio;
        private string almacen;
        private string productoCodigo;
        private string productoDescripcion;
        private string productoUnidadMedida;
        private string productoCantidad;
        private string productoPrecioUnitario;
        private string centroCostosUno;
        private string centroCostosDos;
        private string docReferencia;
        private string docReferenciaFecha;
        private string docReferenciaSerie;
        private string docReferenciaNumero;
        private string docReferenciaMotivoEmision;
        private string docReferenciaMotivo;
        private string transGratuitaMotivo;
        private string transGratuitaValorReferencia;

        private string anioEmisionDUA;
        private string numeroComprobantePagoSujetoNoDomiciliado;
        private string constDepDetNumero;
        private string constDepDetFecha;
        private string equivalenteDolares;
        private string ctaContableBaseImponible;
        private string ctaContableOtrosTributos;
        private string ctaContableTotal;
        private string regimenEspecial;
        private string porcentajeRegimenEspecial;
        private string importeRegimenEspecial;
        private string serieDocumentoRegimenEspecial;
        private string numeroDocumentoRegimenEspecial;
        private string fechaDocumentoRegimenEspecial;
        private string codigoPresupuesto;
        private string porcentajeIGV;
        private string glosa;
        private string condicionPercepcion;
        private string tipoDocProveedor;
        private string numeroDocProveedor;
        private string razonSocialProveedor;
        private string adqGravadasBaseImponible;
        private string adqGravadasIGV;
        private string adqGravadasBaseImponibleGravadasNoGravadas;
        private string adqGravadasIGVGravadasNoGravados;
        private string adqGravadasBaseImponibleNoGravados;
        private string adqGravadasIGVNoGravados;
        private string valorAdquisicionNoGravada;
        private string isc;
        private string otrosTributosYCargos;
        private string importeTotal;

        public string AdqGravadasBaseImponibleNoGravados
        {
            get { return adqGravadasBaseImponibleNoGravados; }
            set { adqGravadasBaseImponibleNoGravados = value; }
        }
        public string OtrosTributosYCargos
        {
            get { return otrosTributosYCargos; }
            set { otrosTributosYCargos = value; }
        }
        public string ImporteTotal
        {
            get { return importeTotal; }
            set { importeTotal = value; }
        }
        public string Isc
        {
            get { return isc; }
            set { isc = value; }
        }
        public string ValorAdquisicionNoGravada
        {
            get { return valorAdquisicionNoGravada; }
            set { valorAdquisicionNoGravada = value; }
        }
        public string AdqGravadasIGVNoGravados
        {
            get { return adqGravadasIGVNoGravados; }
            set { adqGravadasIGVNoGravados = value; }
        }
        public string AdqGravadasIGVGravadasNoGravados
        {
            get { return adqGravadasIGVGravadasNoGravados; }
            set { adqGravadasIGVGravadasNoGravados = value; }
        }
        public string AdqGravadasBaseImponibleGravadasNoGravadas
        {
            get { return adqGravadasBaseImponibleGravadasNoGravadas; }
            set { adqGravadasBaseImponibleGravadasNoGravadas = value; }
        }
        public string AdqGravadasBaseImponible
        {
            get { return adqGravadasBaseImponible; }
            set { adqGravadasBaseImponible = value; }
        }
        public string AdqGravadasIGV
        {
            get { return adqGravadasIGV; }
            set { adqGravadasIGV = value; }
        }
        public string Movimiento
        {
            get { return movimiento; }
            set { movimiento = value; }
        }
        public string TipoDocProveedor
        {
            get { return tipoDocProveedor; }
            set { tipoDocProveedor = value; }
        }
        public string NumeroDocProveedor
        {
            get { return numeroDocProveedor; }
            set { numeroDocProveedor = value; }
        }
        public string RazonSocialProveedor
        {
            get { return razonSocialProveedor; }
            set { razonSocialProveedor = value; }
        }
        public string IdLinea
        {
            get { return idLinea; }
            set { idLinea = value; }
        }
        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }
        public string Numero
        {
            get { return numero; }
            set { numero = value; }
        }
        public string FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }
        public string FechaVencimientoDos
        {
            get { return fechaVencimientoDos; }
            set { fechaVencimientoDos = value; }
        }
        public string Pago
        {
            get { return pago; }
            set { pago = value; }
        }
        public string Moneda
        {
            get { return moneda; }
            set { moneda = value; }
        }
        public string CondicionPago
        {
            get { return condicionPago; }
            set { condicionPago = value; }
        }
        public string CondicionCompra
        {
            get { return condicionCompra; }
            set { condicionCompra = value; }
        }
        public string TipoCambio
        {
            get { return tipoCambio; }
            set { tipoCambio = value; }
        }
        public string Almacen
        {
            get { return almacen; }
            set { almacen = value; }
        }
        public string ProductoCodigo
        {
            get { return productoCodigo; }
            set { productoCodigo = value; }
        }
        public string ProductoDescripcion
        {
            get { return productoDescripcion; }
            set { productoDescripcion = value; }
        }
        public string ProductoUnidadMedida
        {
            get { return productoUnidadMedida; }
            set { productoUnidadMedida = value; }
        }
        public string ProductoCantidad
        {
            get { return productoCantidad; }
            set { productoCantidad = value; }
        }
        public string ProductoPrecioUnitario
        {
            get { return productoPrecioUnitario; }
            set { productoPrecioUnitario = value; }
        }
        public string CentroCostosUno
        {
            get { return centroCostosUno; }
            set { centroCostosUno = value; }
        }
        public string CentroCostosDos
        {
            get { return centroCostosDos; }
            set { centroCostosDos = value; }
        }
        public string DocReferencia
        {
            get { return docReferencia; }
            set { docReferencia = value; }
        }
        public string DocReferenciaFecha
        {
            get { return docReferenciaFecha; }
            set { docReferenciaFecha = value; }
        }
        public string DocReferenciaSerie
        {
            get { return docReferenciaSerie; }
            set { docReferenciaSerie = value; }
        }
        public string DocReferenciaNumero
        {
            get { return docReferenciaNumero; }
            set { docReferenciaNumero = value; }
        }
        public string DocReferenciaMotivoEmision
        {
            get { return docReferenciaMotivoEmision; }
            set { docReferenciaMotivoEmision = value; }
        }
        public string DocReferenciaMotivo
        {
            get { return docReferenciaMotivo; }
            set { docReferenciaMotivo = value; }
        }
        public string TransGratuitaMotivo
        {
            get { return transGratuitaMotivo; }
            set { transGratuitaMotivo = value; }
        }
        public string TransGratuitaValorReferencia
        {
            get { return transGratuitaValorReferencia; }
            set { transGratuitaValorReferencia = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public bool Check
        {
            get { return check; }
            set { check = value; }
        }
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        public string Monto
        {
            get { return monto; }
            set { monto = value; }
        }
        public string NumeroRelacionado
        {
            get { return numeroRelacionado; }
            set { numeroRelacionado = value; }
        }
        public string TipoRelacionado
        {
            get { return tipoRelacionado; }
            set { tipoRelacionado = value; }
        }
        public string TipoTexto
        {
            get { return tipoTexto; }
            set { tipoTexto = value; }
        }
        public string EstadoValidar
        {
            get { return estadoValidar; }
            set { estadoValidar = value; }
        }
        public string EstadoValidarTexto
        {
            get { return estadoValidarTexto; }
            set { estadoValidarTexto = value; }
        }
        public string EstadoVerificar
        {
            get { return estadoVerificar; }
            set { estadoVerificar = value; }
        }
        public string EstadoVerificarTexto
        {
            get { return estadoVerificarTexto; }
            set { estadoVerificarTexto = value; }
        }
        public string EstadoSunat
        {
            get { return estadoSunat; }
            set { estadoSunat = value; }
        }
        public string EstadoSunatCodigo
        {
            get { return estadoSunatCodigo; }
            set { estadoSunatCodigo = value; }
        }
        public string SerieNumero
        {
            get { return serieNumero; }
            set { serieNumero = value; }
        }
        public string FechaEmision
        {
            get { return fechaEmision; }
            set { fechaEmision = value; }
        }
        public string Ruc
        {
            get { return ruc; }
            set { ruc = value; }
        }
        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }
        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; }
        }

        public string AnioEmisionDUA
        {
            get { return anioEmisionDUA; }
            set { anioEmisionDUA = value; }
        }

        public string NumeroComprobantePagoSujetoNoDomiciliado
        {
            get { return numeroComprobantePagoSujetoNoDomiciliado; }
            set { numeroComprobantePagoSujetoNoDomiciliado = value; }
        }
        public string ConstDepDetNumero
        {
            get { return constDepDetNumero; }
            set { constDepDetNumero = value; }
        }
        public string ConstDepDetFecha
        {
            get { return constDepDetFecha; }
            set { constDepDetFecha = value; }
        }
        public string EquivalenteDolares
        {
            get { return equivalenteDolares; }
            set { equivalenteDolares = value; }
        }
        public string CtaContableBaseImponible
        {
            get { return ctaContableBaseImponible; }
            set { ctaContableBaseImponible = value; }
        }
        public string CtaContableOtrosTributos
        {
            get { return ctaContableOtrosTributos; }
            set { ctaContableOtrosTributos = value; }
        }
        public string CtaContableTotal
        {
            get { return ctaContableTotal; }
            set { ctaContableTotal = value; }
        }
        public string RegimenEspecial
        {
            get { return regimenEspecial; }
            set { regimenEspecial = value; }
        }
        public string PorcentajeRegimenEspecial
        {
            get { return porcentajeRegimenEspecial; }
            set { porcentajeRegimenEspecial = value; }
        }
        public string ImporteRegimenEspecial
        {
            get { return importeRegimenEspecial; }
            set { importeRegimenEspecial = value; }
        }
        public string SerieDocumentoRegimenEspecial
        {
            get { return serieDocumentoRegimenEspecial; }
            set { serieDocumentoRegimenEspecial = value; }
        }
        public string NumeroDocumentoRegimenEspecial
        {
            get { return numeroDocumentoRegimenEspecial; }
            set { numeroDocumentoRegimenEspecial = value; }
        }
        public string FechaDocumentoRegimenEspecial
        {
            get { return fechaDocumentoRegimenEspecial; }
            set { fechaDocumentoRegimenEspecial = value; }
        }
        public string CodigoPresupuesto
        {
            get { return codigoPresupuesto; }
            set { codigoPresupuesto = value; }
        }
        public string PorcentajeIGV
        {
            get { return porcentajeIGV; }
            set { porcentajeIGV = value; }
        }
        public string Glosa
        {
            get { return glosa; }
            set { glosa = value; }
        }
        public string CondicionPercepcion
        {
            get { return condicionPercepcion; }
            set { condicionPercepcion = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DocumentoCompra()
        {
            Id = "";
            Tipo = "";
            EstadoVerificar = "";
            EstadoVerificarTexto = "";
            EstadoValidar = "";
            EstadoValidarTexto = "";
            EstadoSunat = "";
            EstadoSunatCodigo = "";
            SerieNumero = "";
            FechaEmision = "";
            Ruc = "";
            RazonSocial = "";
            Comentario = "";
            TipoTexto = "";
            Monto = "";
            NumeroRelacionado = "";
            TipoRelacionado = "";
            Check = false;

            IdLinea = "";
            Movimiento = "";
            Serie = "";
            Numero = "";
            FechaVencimiento = "";
            FechaVencimientoDos = "";
            Pago = "";
            Moneda = "";
            CondicionPago = "";
            CondicionCompra = "";
            TipoCambio = "";
            Almacen = "";
            ProductoCodigo = "";
            ProductoDescripcion = "";
            ProductoUnidadMedida = "";
            ProductoCantidad = "";
            ProductoPrecioUnitario = "";
            CentroCostosUno = "";
            CentroCostosDos = "";
            DocReferencia = "";
            DocReferenciaFecha = "";
            DocReferenciaSerie = "";
            DocReferenciaNumero = "";
            DocReferenciaMotivoEmision = "";
            DocReferenciaMotivo = "";
            TransGratuitaMotivo = "";
            TransGratuitaValorReferencia = "";

            AnioEmisionDUA = "";
            TipoDocProveedor = "";
            NumeroDocProveedor = "";
            RazonSocialProveedor = "";
            NumeroComprobantePagoSujetoNoDomiciliado = "";
            ConstDepDetNumero = "";
            ConstDepDetFecha = "";
            EquivalenteDolares = "";
            CtaContableBaseImponible = "";
            CtaContableOtrosTributos = "";
            CtaContableTotal = "";
            RegimenEspecial = "";
            PorcentajeRegimenEspecial = "";
            ImporteRegimenEspecial = "";
            SerieDocumentoRegimenEspecial = "";
            NumeroDocumentoRegimenEspecial = "";
            FechaDocumentoRegimenEspecial = "";
            CodigoPresupuesto = "";
            PorcentajeIGV = "";
            Glosa = "";
            CondicionPercepcion = "";
        }
        /// <summary>
        ///  //Evento de cambio de propiedad
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
