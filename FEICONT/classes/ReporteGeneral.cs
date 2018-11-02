using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir los objetos tipo reporte general .
/// </summary>
namespace FEICONT
{
    public class ReporteGeneral:INotifyPropertyChanged
    {
        private string tipo;
        private string tipoTexto;
        private string aceptado;
        private string rechazado;
        private string sinestado;
        private string debaja;
        private string emitidos;
        private bool check;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        public string TipoTexto
        {
            get { return tipoTexto; }
            set { tipoTexto = value; }
        }
        public string Aceptado
        {
            get { return aceptado; }
            set { aceptado = value; }
        }
        public string Rechazado
        {
            get { return rechazado; }
            set { rechazado = value; }
        }
        public string SinEstado
        {
            get { return sinestado; }
            set { sinestado = value; }
        }
        public string DeBaja
        {
            get { return debaja; }
            set { debaja = value; }
        }
        public string Emitidos
        {
            get { return emitidos; }
            set { emitidos = value; }
        }
        public bool Check
        {
            get { return check; }
            set { check = value; }
        }
         
        //Metodo constructor
        public ReporteGeneral()
        {
            Tipo = "";
            TipoTexto = "";
            Aceptado = "0";
            Rechazado = "0";
            SinEstado = "0";
            DeBaja = "0";
            Emitidos = "0";
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
