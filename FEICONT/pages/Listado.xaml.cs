using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Listado.xaml
    /// </summary>
    public partial class Listado : Window
    {
        string Titulo = "";
        string Descripcion = "";
        List<string> Documentos = new List<string>();
        public Listado(List<string> documentos, string descripcion, string titulo)
        {
            InitializeComponent();
            Titulo = titulo;
            Descripcion = descripcion;
            Documentos = documentos;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Titulo;
            this.TextoDescripcion.Text = Descripcion;
            dgDocumentos.ItemsSource = Documentos;
        }
    }
}
