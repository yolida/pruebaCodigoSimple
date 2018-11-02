using System.ComponentModel;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir los objetos tipo documento .
/// </summary>
namespace FEICONT
{
    public class ReporteDocumento:INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string tipo;
        private string estadoSCC;
        private string estadoSunat;
        private string estadoSunatCodigo;
        private string serieNumero;
        private string fechaEmision;
        private string ruc; // este
        private string razonSocial;
        private string fechaEnvio;
        private string comentario;
        private string resumenDiario;
        private string resumenDiarioTexto;
        private string resumenDiarioFechaEnvio;
        private string resumenDiarioTicket;
        private string comunicacionBaja;
        private string comunicacionBajaMotivo;
        private string documentoReferencia;
        private string tipoTexto;
        private string idAnterior;
        private string FechaDeBaja;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdAnterior
        {
            get { return idAnterior; }
            set { idAnterior = value; }
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
        public string ResumenDiario
        {
            get { return resumenDiario; }
            set { resumenDiario = value; }
        }

        public string ComunicacionBaja
        {
            get { return comunicacionBaja; }
            set { comunicacionBaja = value; }
        }
        public string ComunicacionBajaMotivo
        {
            get { return comunicacionBajaMotivo; }
            set { comunicacionBajaMotivo = value; }
        }
        public string DocumentoReferencia
        {
            get { return documentoReferencia; }
            set { documentoReferencia = value; }
        }
        public string TipoTexto
        {
            get { return tipoTexto; }
            set { tipoTexto = value; }
        }
        public string ResumenDiarioTexto
        {
            get { return resumenDiarioTexto; }
            set { resumenDiarioTexto = value; }
        }
        public string ResumenDiarioFechaEnvio
        {
            get { return resumenDiarioFechaEnvio; }
            set { resumenDiarioFechaEnvio = value; }
        }
        public string ResumenDiarioTicket
        {
            get { return resumenDiarioTicket; }
            set { resumenDiarioTicket = value; }
        }

        public string fechadebaja
        {
            get { return FechaDeBaja; }
            set { FechaDeBaja = value; }
        }
        //Metodo constructor
        public ReporteDocumento()
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
            ResumenDiario = "";
            ResumenDiarioTexto = "";
            ResumenDiarioFechaEnvio = "";
            ResumenDiarioTicket = "";
            ComunicacionBaja = "";
            ComunicacionBajaMotivo = "";
            DocumentoReferencia = "";
            TipoTexto = "";
            IdAnterior = "";
            fechadebaja = "";
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
