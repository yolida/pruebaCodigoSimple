using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT
{
    public class ReporteRetention : INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string tipo;
        private string estadoSCC;
        private string estadoSunat;
        private string estadoSunatCodigo;
        private string serieNumero;
        private string fechaEmision;
        private string ruc;
        private string razonSocial;
        private string fechaEnvio;
        private string comentario;
        private string reversion;
        private string reversionTexto;
        private string reversionFechaEnvio;
        private string reversionAnterior;
        private string serieNumeroRelacionado;
        private string tipoTextoRelacionado;
        private string tipoRelacionado;
        private string montoRetencion;
        private string porcentajeRetencion;
        private string montoPago;
        private string montoTotal;
        private string motivoReversion;
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
        public string TipoRelacionado
        {
            get { return tipoRelacionado; }
            set { tipoRelacionado = value; }
        }
        public string TipoTextoRelacionado
        {
            get { return tipoTextoRelacionado; }
            set { tipoTextoRelacionado = value; }
        }
        public string EstadoSCC
        {
            get { return estadoSCC; }
            set { estadoSCC = value; }
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
        public string FechaEnvio
        {
            get { return fechaEnvio; }
            set { fechaEnvio = value; }
        }
        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; }
        }
        public string Reversion
        {
            get { return reversion; }
            set { reversion = value; }
        }

        public string ReversionTexto
        {
            get { return reversionTexto; }
            set { reversionTexto = value; }
        }
        public string ReversionFechaEnvio
        {
            get { return reversionFechaEnvio; }
            set { reversionFechaEnvio = value; }
        }
        public string ReversionAnterior
        {
            get { return reversionAnterior; }
            set { reversionAnterior = value; }
        }
        public string SerieNumeroRelacionado
        {
            get { return serieNumeroRelacionado; }
            set { serieNumeroRelacionado = value; }
        }      
        public string MontoRetencion
        {
            get { return montoRetencion; }
            set { montoRetencion = value; }
        }
        public string PorcentajeRetencion
        {
            get { return porcentajeRetencion; }
            set { porcentajeRetencion = value; }
        }
        public string MontoPago
        {
            get { return montoPago; }
            set { montoPago = value; }
        }
        public string MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }
        public string MotivoReversion
        {
            get { return motivoReversion; }
            set { motivoReversion = value; }
        }
        //Metodo constructor
        public ReporteRetention()
        {
            Id = "";
            Tipo = "";
            EstadoSCC = "";
            EstadoSunat = "";
            EstadoSunatCodigo = "";
            SerieNumero = "";
            FechaEmision = "";
            Ruc = "";
            RazonSocial = "";
            FechaEnvio = "";
            Comentario = "";
            Reversion = "";
            ReversionTexto = "";
            ReversionFechaEnvio = "";
            ReversionAnterior = "";
            SerieNumeroRelacionado = "";
            TipoRelacionado = "";
            TipoTextoRelacionado = "";
            MontoRetencion = "";
            PorcentajeRetencion = "";
            MontoPago = "";
            MontoTotal = "";
            MotivoReversion = "";
            Check = false;
        }
        //Evento de cambio de propiedad
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
