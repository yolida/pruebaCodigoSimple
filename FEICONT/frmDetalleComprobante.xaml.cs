using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para frmDetalleComprobante.xaml
    /// </summary>
    public partial class frmDetalleComprobante : Window
    {
        private string IdComprobante;
        private List<clsEntityDocument_Line> lista_items = new List<clsEntityDocument_Line>();
        private clsEntityDocument_Line item_lista;
        private clsEntityDatabaseLocal localDB;
        public frmDetalleComprobante(string idComprobante, clsEntityDatabaseLocal local)
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
            clsEntityDocument cabecera = new clsEntityDocument(localDB);
            cabecera.cs_fxObtenerUnoPorId(IdComprobante);

            txtTipoComprobante.Text = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(cabecera.Cs_tag_InvoiceTypeCode);
            txtRuc.Text = cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;
            txtFechaEmision.Text = cabecera.Cs_tag_IssueDate;
            txtRazonSocial.Text = cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
            txtSerieNumero.Text = cabecera.Cs_tag_ID;

            List<List<string>> registros = new clsEntityDocument_Line(localDB).cs_pxObtenerTodoPorId(IdComprobante);

            if (registros!=null)
            {
                foreach (var item in registros)
                {
                    decimal vardecimal = 0;
                    List<List<string>> descipcion_item = new clsEntityDocument_Line_Description(localDB).cs_pxObtenerTodoPorId(item[0]);
                    if (item[5].ToString().Trim() == "")
                    {
                        vardecimal = 0;
                    }
                    else
                    {
                        vardecimal = decimal.Parse(item[5].ToString().Trim());
                    }
                    item_lista = new clsEntityDocument_Line(localDB);
                    item_lista.Cs_tag_InvoiceLine_ID = item[0].ToString().Trim();
                    item_lista.Cs_tag_Item_SellersItemIdentification = item[4].ToString().Trim();
                    item_lista.Cs_tag_LineExtensionAmount_currencyID = Convert.ToString(vardecimal);

                    if (descipcion_item.Count > 0 && descipcion_item != null)
                    {
                        foreach (var items in descipcion_item)
                        {
                            item_lista.Cs_tag_invoicedQuantity += items[2] + " ";
                        }                                                         
                    }

                    if (item_lista.Cs_tag_invoicedQuantity==null)
                    {
                        //buscar descripcion del mismo codigo
                        string idNuevo =  new clsEntityDocument_Line(localDB).cs_pxObtenerIdRelacionadoDescripcionUltimo(item[0].ToString().Trim(), item[8].ToString().Trim());
                        List<List<string>> descipcion_item2 = new clsEntityDocument_Line_Description(localDB).cs_pxObtenerTodoPorId(idNuevo);
                        if (descipcion_item2.Count > 0 && descipcion_item2 != null)
                        {
                            foreach (var itemss in descipcion_item2)
                            {
                                item_lista.Cs_tag_invoicedQuantity += itemss[2];
                            }

                        }
                    }

                    lista_items.Add(item_lista);                    
                }

                //Agregar total

                clsEntityDocument_Line monto_total = new clsEntityDocument_Line();
                monto_total.Cs_tag_invoicedQuantity = "IMPORTE TOTAL";
                monto_total.Cs_tag_LineExtensionAmount_currencyID = cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID;

                lista_items.Add(monto_total);
            }

            dgEmpresas.ItemsSource = lista_items;
        }

        private void btnXMLEnvio_Click(object sender, RoutedEventArgs e)
        {
            try{
                clsEntityDocument cabecera = new clsEntityDocument(localDB);
                cabecera.cs_fxObtenerUnoPorId(IdComprobante);
                if (cabecera != null)
                {
                    System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
                    sfdDescargar.FileName = cabecera.Cs_tag_ID;  
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

                            if (cabecera.Cs_pr_XML.Trim().Length > 0)
                            {
                                xml = cabecera.Cs_pr_XML;
                            }
                            else
                            {
                                switch (cabecera.Cs_tag_InvoiceTypeCode)
                                {
                                    case "01":
                                        xml = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "03":
                                        xml = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "07":
                                        xml = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "08":
                                        xml = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                }
                            }
                            StreamWriter sw1 = new StreamWriter(File.Open(file, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(xml); sw1.Close(); xml = string.Empty;
                            /*  StreamWriter sw0 = new StreamWriter(file);
                              sw0.Write(xml);
                              sw0.Close();*/
                 
                            System.Windows.Forms.MessageBox.Show("Se ha descargado el XML de envio en la ruta seleccionada.", "Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
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
                clsEntityDocument cabecera = new clsEntityDocument(localDB);
                cabecera.cs_fxObtenerUnoPorId(IdComprobante);
                if (cabecera != null)
                {
                    System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
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
                            sw0.Write(cabecera.Cs_pr_CDR);
                            sw0.Close();
                            System.Windows.Forms.MessageBox.Show("Se ha descargado el XML de recepcion en la ruta seleccionada.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void btnRepresentacionImpresa_Click(object sender, RoutedEventArgs e)
        {
            //Descargar representacion impresa.
            try
            {
                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                string currentDirectory = Environment.CurrentDirectory;
                string pathImage = currentDirectory +"\\"+declarante.Cs_pr_Ruc+ "\\logo.png";
                string pathDatos = currentDirectory + "\\"+ declarante.Cs_pr_Ruc + "\\informacionImpreso.txt";
                if (File.Exists(pathImage) && File.Exists(pathDatos))
                {
                   
                    StreamReader readDatos = new StreamReader(pathDatos);
                    string datosImpresa = readDatos.ReadToEnd();
                    readDatos.Close();
                    clsEntityDocument cabecera = new clsEntityDocument(localDB);
                    cabecera.cs_fxObtenerUnoPorId(IdComprobante);
                    if (cabecera != null)
                    {
                        string[] partes = cabecera.Cs_tag_ID.Split('-');

                        System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
                        sfdDescargar.FileName = cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "_" + partes[0] + "_" + partes[1] + ".pdf";
                        DialogResult result = sfdDescargar.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            string fileName_original = sfdDescargar.FileName;
                            string fileName_falso = "";
                            if (fileName_original.Substring(fileName_original.Length - 4) != ".pdf")
                            {
                                fileName_original = fileName_original + ".pdf";
                            }

                            if (fileName_original.Substring(fileName_original.Length - 4)== ".pdf")
                            {
                                fileName_falso = fileName_original.Replace(".pdf","_Prueba.pdf");
                            }

                            bool procesado = false;
                            if (cabecera.Cs_pr_XML.Trim() != "")
                            {
                                procesado = RepresentacionImpresa.getRepresentacionImpresa(fileName_falso, cabecera, cabecera.Cs_pr_XML, datosImpresa, pathImage, localDB);
                                //Cristhian|06/02/2018|FEI2-596
                                /*Se invoca el metodo para agregar elnumero de página*/
                                /*NUEVO INICIO*/
                                procesado = RepresentacionImpresa.Agregar_Numero_Pagina(fileName_falso,fileName_original);
                                /*NUEVO FIN*/
                            }
                            else
                            {
                                //generar xml 
                                string xml = string.Empty;

                                switch (cabecera.Cs_tag_InvoiceTypeCode)
                                {
                                    case "01":
                                        xml = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "03":
                                        xml = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "07":
                                        xml = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                    case "08":
                                        xml = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                                        break;
                                }
                                procesado = RepresentacionImpresa.getRepresentacionImpresa(fileName_falso, cabecera, xml, datosImpresa, pathImage,localDB);

                                //Cristhian|06/02/2018|FEI2-596
                                /*Se invoca el metodo para agregar elnumero de página*/
                                /*NUEVO INICIO*/
                                procesado = RepresentacionImpresa.Agregar_Numero_Pagina(fileName_falso, fileName_original);
                                /*NUEVO FIN*/
                            }

                            if (procesado)
                            {
                                System.Diagnostics.Process.Start(fileName_original);                             
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

        private void btnReporteErrorXML_Click(object sender, RoutedEventArgs e)
        {
            string XMLValidado = string.Empty;

            try
            {
                clsEntityDocument cabecera = new clsEntityDocument(localDB);
                cabecera.cs_fxObtenerUnoPorId(IdComprobante);
                if (cabecera != null)
                {
                    System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
                    sfdDescargar.FileName = cabecera.Cs_tag_ID;
                    DialogResult result = sfdDescargar.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string file = sfdDescargar.FileName;
                        if (file.Substring(file.Length - 4) != ".txt")
                        {
                            file = file + ".txt";
                        }
                        try
                        {
                            clsNegocioValidar validar = new clsNegocioValidar();
                            XMLValidado = validar.cs_pxGenerarReporteParaGuardarArchivo(IdComprobante, localDB);

                            //if (cabecera.Cs_pr_XML.Trim().Length > 0)
                            //{
                            //    XMLValidado = cabecera.Cs_pr_XML;
                            //}
                            //else
                            //{
                            //    switch (cabecera.Cs_tag_InvoiceTypeCode)
                            //    {
                            //        case "01":
                            //            XMLValidado = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                            //            break;
                            //        case "03":
                            //            XMLValidado = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                            //            break;
                            //        case "07":
                            //            XMLValidado = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                            //            break;
                            //        case "08":
                            //            XMLValidado = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(cabecera.Cs_pr_Document_Id);
                            //            break;
                            //    }
                            //}
                            StreamWriter sw1 = new StreamWriter(File.Open(file, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(XMLValidado); sw1.Close(); XMLValidado = string.Empty;
                            /*  StreamWriter sw0 = new StreamWriter(file);
                              sw0.Write(xml);
                              sw0.Close();*/

                            System.Windows.Forms.MessageBox.Show("Se ha descargado la Validación XML de envio en la ruta seleccionada.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            clsBaseLog.cs_pxRegistarAdd("generar xml Validación " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("xml Validación gen " + ex.ToString());
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
        
    }
}
