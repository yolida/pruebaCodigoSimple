using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT
{
    public class ReportePermiso : INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string modulo;
        private string moduloPadre;
        private string usuario;
        private string permitido;
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

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public string Modulo
        {
            get { return modulo; }
            set { modulo = value; }
        }
        public string ModuloPadre
        {
            get { return moduloPadre; }
            set { moduloPadre = value; }
        }
        public string Permitido
        {
            get { return permitido; }
            set { permitido = value; }
        }
        //Metodo constructor
        public ReportePermiso()
        {
            Id = "";
            Usuario = "";
            Modulo = "";
            Permitido = "";
            ModuloPadre = "";
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
