using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para frmDetallePR.xaml
    /// </summary>
    public partial class frmDetallePR : Window
    {
        private string IdComprobante;
        private List<clsEntityRetention_RetentionLine> lista_items = new List<clsEntityRetention_RetentionLine>();
        private clsEntityRetention_RetentionLine item_lista;
        private clsEntityDatabaseLocal localDB;
        public frmDetallePR(string idComprobante,clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            IdComprobante = idComprobante;
            localDB = local;
        }
        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clsEntityRetention cabecera = new clsEntityRetention(localDB);
            cabecera.cs_fxObtenerUnoPorId(IdComprobante);

            txtTipoComprobante.Text ="Comprobante de retención electrónico";
            txtRuc.Text = cabecera.Cs_tag_ReceiveParty_PartyIdentification_Id;
            txtFechaEmision.Text = cabecera.Cs_tag_IssueDate;
            txtRazonSocial.Text = cabecera.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName;
            txtSerieNumero.Text = cabecera.Cs_tag_Id;

            List<clsEntityRetention_RetentionLine> registros = new clsEntityRetention_RetentionLine(localDB).cs_fxObtenerTodoPorCabeceraId(IdComprobante);

            if (registros.Count > 0)
            {
                foreach (var item in registros)
                {
                  
                    item_lista = new clsEntityRetention_RetentionLine(localDB);
                    item_lista.Cs_tag_Id_SchemeId = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_Id_SchemeId);
                    item_lista.Cs_tag_Id = item.Cs_tag_Id;
                    item_lista.Cs_tag_IssueDate = item.Cs_tag_IssueDate;
                    item_lista.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount;
                    item_lista.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid;
                    lista_items.Add(item_lista);
                }
            }

            dgEmpresas.ItemsSource = lista_items;
        }

        private void btnXMLEnvio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsEntityRetention item = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(IdComprobante);
                if (item != null)
                {
                    SaveFileDialog sfdDescargar = new SaveFileDialog();
                    sfdDescargar.FileName = item.Cs_tag_Id;
                    DialogResult result = sfdDescargar.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string file = sfdDescargar.FileName;
                        if (file.Substring(file.Length - 4) != ".xml")
                        {
                            file = file + ".xml";
                        }
                        try
                        {
                            string xml = string.Empty;

                            if (item.Cs_pr_XML.Trim().Length > 0)
                            {
                                xml = item.Cs_pr_XML;
                            }
                            else
                            {
                                xml = new clsNegocioCERetention(localDB).cs_pxGenerarXMLAString(item.Cs_pr_Retention_id);
                              
                            }
                            StreamWriter sw0 = new StreamWriter(file);
                            sw0.Write(xml);
                            sw0.Close();
                            System.Windows.Forms.MessageBox.Show("Se generó correctamente el archivo XML de envío en la ruta seleccionada.","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            clsBaseLog.cs_pxRegistarAdd("generar xml envio " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("xml envio gen " + ex.ToString());
            }
        }

        private void btnXMLRecepcion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsEntityRetention item = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(IdComprobante);
                if (item != null)
                {
                  
                    SaveFileDialog sfdDescargar = new SaveFileDialog();
                    DialogResult result = sfdDescargar.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string file = sfdDescargar.FileName;
                        if (file.Substring(file.Length - 4) != ".xml")
                        {
                            file = file + ".xml";
                        }
                        try
                        {
                            StreamWriter sw0 = new StreamWriter(file);
                            sw0.Write(item.Cs_pr_CDR);
                            sw0.Close();
                            System.Windows.Forms.MessageBox.Show("Se generó correctamente el archivo XML de recepcion en la ruta seleccionada.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (IOException)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("xml recep gen" + ex.ToString());
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
        }

        private void btnRepresentacionImpresa_Click(object sender, RoutedEventArgs e)
        {
            //Descargar representacion impresa.
            try
            {
                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                string currentDirectory = Environment.CurrentDirectory;
                string pathImage = currentDirectory +"\\"+declarante.Cs_pr_Ruc+ "\\logo.png";
                string pathDatos = currentDirectory +"\\"+declarante.Cs_pr_Ruc+ "\\informacionImpreso.txt";
                if (File.Exists(pathImage) && File.Exists(pathDatos))
                {

                    StreamReader readDatos = new StreamReader(pathDatos);
                    string datosImpresa = readDatos.ReadToEnd();
                    readDatos.Close();

                    clsEntityRetention cabecera = new clsEntityRetention(localDB);
                    cabecera.cs_fxObtenerUnoPorId(IdComprobante);
                    if (cabecera != null)
                    {
                        string[] partes = cabecera.Cs_tag_Id.Split('-');

                        System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
                        sfdDescargar.FileName = cabecera.Cs_tag_PartyIdentification_Id + "_" + partes[0] + "_" + partes[1] + ".pdf";
                        DialogResult result = sfdDescargar.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            string fileName = sfdDescargar.FileName;
                            if (fileName.Substring(fileName.Length - 4) != ".pdf")
                            {
                                fileName = fileName + ".pdf";
                            }

                            bool procesado = false;
                            if (cabecera.Cs_pr_XML.Trim() != "")
                            {
                                procesado = RepresentacionImpresa.getRepresentacionImpresaRetencion(fileName, cabecera, cabecera.Cs_pr_XML, datosImpresa, pathImage, localDB);
                            }
                            else
                            {
                                //generar xml 
                                string xml = string.Empty;

                                xml = new clsNegocioCERetention(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Retention_id);
                                                              
                                procesado = RepresentacionImpresa.getRepresentacionImpresaRetencion(fileName, cabecera, xml, datosImpresa, pathImage, localDB);
                            }

                            if (procesado)
                            {
                                System.Diagnostics.Process.Start(fileName);                            
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("Ha ocurrido un error al procesar la representacion impresa.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No se encuentra la imagen del logo y/o la información para la representacion impresa. Verifique la existencia de la imagen 'logo.png' y el archivo 'informacionImpreso.txt'  en la ruta de instalación.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("pdf repimpresa" + ex.ToString());
            }
        }
    }
}
