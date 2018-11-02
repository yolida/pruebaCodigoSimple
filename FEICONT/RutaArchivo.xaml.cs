using FEICONT.ayuda;
using Extension.Base;
//using SERCE;
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

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para RutaArchivo.xaml
    /// </summary>
    public partial class RutaArchivo : Window
    {
        public RutaArchivo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string rutaString = new clsRegistry().Read("RUTA");
            ruta.Content = rutaString;
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            //frmSelect f = new frmSelect();
            //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    MessageBox.Show("Se cerrará el programa para aplicar los cambios realizados.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            //    System.Windows.Application.Current.Shutdown();
            //}
        }

        private void btnResetear_Click(object sender, RoutedEventArgs e)
        {
            bool ruta = new clsRegistry().DeleteKey("RUTA");
            if (ruta)
            {
                MessageBox.Show("Se ha eliminado el registro correctamente. La aplicacion de cerrará para aplicar los cambios.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
            int compare2 = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare2 == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("28");
                ayuda.ShowDialog();
            }
        }
    }
}
