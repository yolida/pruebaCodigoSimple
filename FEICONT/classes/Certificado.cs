using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT.classes
{
    public class Certificado : INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string serie;
        private string validodesde;
        private string validohasta;
        private string alertadias;
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

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }
        public string ValidoDesde
        {
            get { return validodesde; }
            set { validodesde = value; }
        }
        public string ValidoHasta
        {
            get { return validohasta; }
            set { validohasta = value; }
        }
        public string AlertaDias
        {
            get { return alertadias; }
            set { alertadias = value; }
        }
        //Metodo constructor
        public Certificado()
        {
            Id = "";
            Serie = "";
            ValidoDesde = "";
            ValidoHasta = "";
            AlertaDias = "";
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
