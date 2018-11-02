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

namespace FEICONT.ayuda
{
    /// <summary>
    /// Lógica de interacción para AyudaPrincipal.xaml
    /// </summary>
    public partial class AyudaPrincipal : Window
    {
        private string seleccionado;
        public AyudaPrincipal(string numero)
        {
            InitializeComponent();
            seleccionado = numero;
        }
        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            /*Page1 pa = new Page1();
            frame.Navigate(pa);*/
            this.frame.Source = new Uri("Page1.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
        }
        public void seleccionarMenu(string item)
        {
            /*for (int i = 0; i <= 30; i++)
             {*/
            try
            {
                string lblNameItem = "lblItem" + item.ToString();
                TreeViewItem objecto = (TreeViewItem)FindName(lblNameItem);
                objecto.IsSelected = true;
                objecto.Focus();
                //objecto.Foreground =Brushes.Blue;
                //objecto. = Brushes.Blue;
                TreeViewItem padre = (TreeViewItem)objecto.Parent;
                padre.IsExpanded = true;
            }
            catch
            {
                string lblNameItem = "lblItem0";
                TreeViewItem objecto = (TreeViewItem)FindName(lblNameItem);
                objecto.IsSelected = true;
                objecto.Focus();
                //objecto.Foreground =Brushes.Blue;
                //objecto. = Brushes.Blue;
                TreeViewItem padre = (TreeViewItem)objecto.Parent;
                padre.IsExpanded = true;
            }
                
          /*  }*/
        }
        private void TreeViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Page2.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Dirigido(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Dirigido.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_FormasPlazos(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Formasyplazos.xaml", UriKind.RelativeOrAbsolute);
        } 

        private void TreeViewItem_MotivoRechazo(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("MotivoRechazo.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_InicioSesion(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("InicioSesionAyuda.xaml", UriKind.RelativeOrAbsolute);
        }
        private void TreeViewItem_OpcionesPrograma(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("OpcionesPrograma.xaml", UriKind.RelativeOrAbsolute);
        }
        private void TreeViewItem_Reportes(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Reportes.xaml", UriKind.RelativeOrAbsolute);
        }
         
        private void TreeViewItem_ReporteFE(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("ReporteFE.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_ReporteRD(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("ReporteRD.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_ReporteBV(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("ReporteBV.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_ReporteRA(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("ReporteRA.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_ReporteCRE(object sender, RoutedEventArgs e)
        {

        }
        private void TreeViewItem_ReporteGeneral(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("ReporteGeneral.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_FE(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("FE.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_FEAlerta(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("FEAlerta.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RD(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RD.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RDSunat(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RDSunat.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RDAlerta(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RDAlerta.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RA(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RA.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RASunat(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RASunat.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RegistroLocal(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RegistroLocal.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RegistroWeb(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RegistroWeb.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Declarante(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Declarante.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Usuarios(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Usuarios.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Permisos(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("PermisosUsuario.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_GenerarBackup(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("GenerarBackup.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RestaurarBackup(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RestaurarBackup.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_RutaArchivos(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("RutaArchivos.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Estructura(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Actualizacion.xaml", UriKind.RelativeOrAbsolute);
        }

        private void TreeViewItem_Licencia(object sender, RoutedEventArgs e)
        {
            this.frame.Source = new Uri("Activacion.xaml", UriKind.RelativeOrAbsolute);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            seleccionarMenu(seleccionado);
        }
    }
}
