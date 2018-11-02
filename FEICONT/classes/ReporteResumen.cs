using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir los objetos tipo resumen diario y comunicacion de baja .
/// </summary>
namespace FEICONT
{
    public class ReporteResumen: INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string estadoSCC;
        private string estadoSunat;
        private string estadoSunatCodigo;
        private string archivo;
        private string fechaEmision;
        private string fechaEnvio;
        private string comentario;
        private string ticket;

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
        public string Archivo
        {
            get { return archivo; }
            set { archivo = value; }
        }
        public string FechaEmision
        {
            get { return fechaEmision; }
            set { fechaEmision = value; }
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
        public string Ticket
        {
            get { return ticket; }
            set { ticket = value; }
        }
        //Metodo constructor 
        public ReporteResumen()
        {
            Id = "";          
            EstadoSCC = "";
            EstadoSunat = "";
            EstadoSunatCodigo = "";
            Archivo = "";
            FechaEmision = "";
            FechaEnvio = "";
            Comentario = "";
            Ticket = "";
            Check = false;
        }
        //Evento para cambio de propiedad
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
