
using FEICONT.ayuda;
using Extension.Base;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Lógica de interacción para ActivacionManual.xaml
    /// </summary>
    public partial class ActivacionManual : Window
    {
        private string codigoMaquina;
        private string versionCompilado;
        public ActivacionManual(string code,string version)
        {
            InitializeComponent();
            codigoMaquina = code;
            versionCompilado = version;
            txtCodigoPeticion.Text = codigoMaquina;
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            //Generar archivo de solicitud de activacion
            if(txtCodigoActivacion.Text.Trim() == ""){
                MessageBox.Show("Ingrese Código de Activación.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string vCodActivacion = new clsBaseLicencia().fx_CodificarBase64(txtCodigoActivacion.Text.Trim());
                string vPeticion = txtCodigoPeticion.Text;
                string app_codmodulo = "SFEI";
                string vCodModulo = new clsBaseLicencia().fx_CodificarBase64(app_codmodulo);
                //vContenido = objBase64.codificar('[' + vCodActivacion + ']' + '[' + vPeticion + ']' + '[' + vCodModulo + ']')
                string vContenido = vCodActivacion + ";" + vPeticion + ";" + vCodModulo;

                string vNombreArchivo = txtCodigoActivacion.Text.Trim().Replace("-", "") + app_codmodulo + ".lic";
                string mis_documentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string vArchivo = mis_documentos + "//" + vNombreArchivo;
                if (!File.Exists(vArchivo))
                {
                    File.Create(vArchivo).Close();
                }
                StreamWriter sw1 = new StreamWriter(vArchivo);
                sw1.Write(vContenido);
                sw1.Close();

                MessageBox.Show("Se ha generado el archivo de autorización: \n " + vNombreArchivo + " \n " + "En la ruta " + mis_documentos,"Mensaje",MessageBoxButton.OK,MessageBoxImage.Information);

            }

        }
        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            if(txtCodigoActivacion.Text.Trim()!="" && txtCodigoLicencia.Text.Trim()!="" && txtCodigoPeticion.Text.Trim() != "")
            {
                bool activar = new clsBaseLicencia().activarLicencia(versionCompilado, "SFEI", txtCodigoPeticion.Text.Trim(), txtCodigoActivacion.Text.Trim(), txtCodigoLicencia.Text.Trim());
                if (activar)
                {
                    bool flag3 = new clsBaseLicencia().saveLicence(txtCodigoLicencia.Text.Trim());
                    if (flag3)
                    {
                        MessageBox.Show("Su producto fue activado con éxito","Mensaje",MessageBoxButton.OK,MessageBoxImage.Information);
                        DialogResult = true;
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("No es posible realizar la Activación del Producto, por favor contáctese con su proveedor.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Los campos deben estar completos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                AyudaPrincipal ayuda = new AyudaPrincipal("30");
                ayuda.ShowDialog();
            }
        }
    }
}
