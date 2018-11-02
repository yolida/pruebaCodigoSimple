using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT
{
    public class DocumentoValidar : INotifyPropertyChanged
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
        private string rucEmisor;
        private string razonSocial;
        private string comentario;
        private string tipoTexto;
        private string monto;
        private string numeroRelacionado;
        private string tipoRelacionado;
        private string cargovalidar;
        private string cargoverificar;

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
        public string RucEmisor
        {
            get { return rucEmisor; }
            set { rucEmisor = value; }
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
        public string CargoValidar
        {
            get { return cargovalidar; }
            set { cargovalidar = value; }
        }
        public string CargoVerificar
        {
            get { return cargoverificar; }
            set { cargoverificar = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DocumentoValidar()
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
            CargoValidar = "";
            CargoVerificar = "";
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

