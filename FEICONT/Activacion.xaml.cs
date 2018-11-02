using FEICONT.ayuda;
using Extension.Base;
//using Servicios;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para Activacion.xaml
    /// </summary>
    public partial class Activacion : Window
    {
        private string Code;
        private string Version;
        public Activacion(string codigoMaquina, string versionCompilado)
        {
            InitializeComponent();
            Code                = codigoMaquina;
            Version             = versionCompilado;
            labelCodigo.Content = Code;
        }
        private void btnActivar_Click(object sender, RoutedEventArgs e)
        {
            string text     = txtCodigos.Text;
            string text2    = text.Trim();
            //
            if (text2 == ""||text2==null)
            {
                MessageBox.Show("Ingrese Código de Activación.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string pEntrada     = new clsBaseLicencia().fx_CodificarBase64(text.Trim());
                string Peticion     = new clsBaseLicencia().fx_CodificarBase64(Code);
                string pEntrada2    = new clsBaseLicencia().fx_CodificarBase64("SFEI");
                string soapResult   = "";
                //MessageBox.Show(pEntrada2);
                string directorio = Directory.GetCurrentDirectory();
                try
                {
                    //if (File.Exists(directorio + "\\71c03801.txt"))
                    //{
                    //    soapResult = WebServiceALCdemo.Execute_wsLicencia(pEntrada, Peticion, pEntrada2);
                    //}
                    //else if (File.Exists(directorio + "\\71c03802.txt"))
                    //{
                    //    soapResult = WebServiceALCdemo.Execute_wsLicencia(pEntrada, Peticion, pEntrada2);
                    //}
                    //else
                    //{
                        //soapResult = WebServiceALC.Execute_wsLicencia(pEntrada, Peticion, pEntrada2); // Falta el API 
                    //}

                    string text3 = captura_wslicencia(soapResult);
                    int length = text3.Length;
                   // this.txtLicencia.Text = text3;
                    bool flag2 = length > 1;
                    if (flag2)
                    {
                        bool activar= new clsBaseLicencia().activarLicencia(Version,"SFEI",Code,text2,text3);
                        if (activar)
                        {
                            bool flag3 = new clsBaseLicencia().saveLicence(text3);
                            if (flag3)
                            {
                                MessageBox.Show("Su producto fue activado con éxito.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
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

                        if (text3 == "X")
                        {
                            MessageBox.Show("No se pudo activar su producto, código de activación ó producto no válido.","Error", MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                        if (text3 == "A")
                        {
                            MessageBox.Show("No se pudo activar su producto, el código de activación ya se encuentra registrado. Por favor contáctese con su proveedor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo activar su producto, por favor verifique su conexión a Internet e intente nuevamente. ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                }
            }
        }
        public string captura_wslicencia(string xml)
        {
            string text = "<T2 xmlns=\"UXMQ09K34J8\">";
            string text2 = "</T2>";
            int num = xml.Count<char>();
            int num2 = text.Count<char>();
            int num3 = text2.Count<char>();
            int num4 = num2 + xml.IndexOf(text);
            int num5 = xml.IndexOf(text2);
            return xml.Substring(num4, num5 - num4);
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnActivacionManual_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ActivacionManual actManual = new ActivacionManual(Code,Version);
            actManual.ShowDialog();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
