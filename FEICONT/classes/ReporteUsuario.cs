using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Clase para definir los objetos tipo usuario.
/// </summary>
namespace FEICONT
{
    public class ReporteUsuario:INotifyPropertyChanged
    {
        private string id;
        private bool check;
        private string usuario;
        private string contrasenia;
        private string rol;
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
        public string Contrasenia
        {
            get { return contrasenia; }
            set { contrasenia = value; }
        }
        public string Rol
        {
            get { return rol; }
            set { rol = value; }
        }
        //Metodo constructor
        public ReporteUsuario()
        {
            Id = "";
            Usuario = "";
            Contrasenia = "";
            Rol = "";
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
