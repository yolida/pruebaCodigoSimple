using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir los objetos tipo empresa .
/// </summary>
namespace FEICONT
{
    public class ReporteEmpresa : INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string ruc;
        private string razonSocial;
        private string rutaCertificadoDigital;
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
        public string RutaCertificadoDigital
        {
            get { return rutaCertificadoDigital; }
            set { rutaCertificadoDigital = value; }
        }
        //Metodo constructor
        public ReporteEmpresa()
        {
            Id = "";
            Ruc = "";
            RazonSocial = "";
            RutaCertificadoDigital = "";
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
