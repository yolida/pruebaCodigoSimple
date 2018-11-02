using FEICONT.ayuda;
using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.Xml;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Reportes de facturas.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_Factura.xaml
    /// </summary>
    public partial class Reporte_Factura : Page
    {
        List<ComboBoxPares> tipos_comprobante = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_scc = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();
        List<clsEntityDocument> registros;
        List<ReporteDocumento> lista_reporte;
        ReporteDocumento itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor
        public Reporte_Factura(clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //Evento de carga de la ventana.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores a combobox de tipo reporte
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;
            //Agregar valores a combobox de tipo de comprobante
            tipos_comprobante.Add(new ComboBoxPares("", "Seleccione"));
            tipos_comprobante.Add(new ComboBoxPares("01", "Factura Electronica"));
            tipos_comprobante.Add(new ComboBoxPares("07", "Nota de Credito"));
            tipos_comprobante.Add(new ComboBoxPares("08", "Nota de Debito"));
            cboTipoComprobante.DisplayMemberPath = "_Value";
            cboTipoComprobante.SelectedValuePath = "_Key";
            cboTipoComprobante.SelectedIndex = 0;
            cboTipoComprobante.ItemsSource = tipos_comprobante;
            //Agregar valores a combobox de tipo estados de SCC
            estados_scc.Add(new ComboBoxPares("", "Seleccione"));
            estados_scc.Add(new ComboBoxPares("0", "Enviado"));
            //estados_scc.Add(new ComboBoxPares("1", "Pendiente (Errores)"));
            estados_scc.Add(new ComboBoxPares("2", "Pendiente (Correcto)"));
            cboEstadoSCC.DisplayMemberPath = "_Value";
            cboEstadoSCC.SelectedValuePath = "_key";
            cboEstadoSCC.SelectedIndex = 0;
            cboEstadoSCC.ItemsSource = estados_scc;
            //Agregar estados sunat a combobox.
            estados_sunat.Add(new ComboBoxPares("", "Seleccione"));
            estados_sunat.Add(new ComboBoxPares("0", "Aceptado"));
            estados_sunat.Add(new ComboBoxPares("1", "Rechazado"));
            estados_sunat.Add(new ComboBoxPares("2", "Sin estado"));
            estados_sunat.Add(new ComboBoxPares("3", "De Baja"));

            cboEstadoSunat.DisplayMemberPath = "_Value";
            cboEstadoSunat.SelectedValuePath = "_key";
            cboEstadoSunat.SelectedIndex = 0;
            cboEstadoSunat.ItemsSource = estados_sunat;
            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();

            ComboBoxPares cbpTC = (ComboBoxPares)cboTipoComprobante.SelectedItem;
            ComboBoxPares cbpESCC = (ComboBoxPares)cboEstadoSCC.SelectedItem;
            ComboBoxPares cbpES = (ComboBoxPares)cboEstadoSunat.SelectedItem;

            if (datePick_inicio.SelectedDate != null)
            {
                DateTime fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            if (datePick_fin.SelectedDate != null)
            {
                DateTime fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            cs_pxCargarDgvComprobanteselectronicos(cbpTC._Id, cbpESCC._Id, cbpES._Id, fecha_inicio_formato, fecha_fin_formato);
        }
        //Metodo de carga de comprobantes en la grilla
        private void cs_pxCargarDgvComprobanteselectronicos(string tipo, string estadocomprobantescc, string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            //Obtener los comprobantes de factura.
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroPrincipal(tipo, estadocomprobantesunat, estadocomprobantescc, fechainicio, fechafin, "'01','07','08'", false);//FEICONT2-Malo, no 03
            lista_reporte = new List<ReporteDocumento>();
            //lista_reporte = new ObservableCollection<Reporte>();
            if (registros != null)
            {
                //Recorrer los registros para llenar la grilla.
                foreach (var item in registros)
                {
                    itemRow = new ReporteDocumento();
                    itemRow.Id = item.Cs_pr_Document_Id;
                    itemRow.Tipo = item.Cs_tag_InvoiceTypeCode;
                    itemRow.SerieNumero = item.Cs_tag_ID;
                    itemRow.FechaEmision = item.Cs_tag_IssueDate;
                    itemRow.FechaEnvio = item.comprobante_fechaenviodocumento;
                    itemRow.Ruc = item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;
                    itemRow.RazonSocial = item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
                    itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                    itemRow.ComunicacionBaja = item.Cs_pr_ComunicacionBaja;
                    itemRow.ResumenDiario = item.Cs_pr_Resumendiario;
                    itemRow.DocumentoReferencia = item.Cs_tag_BillingReference_DocumentTypeCode;
                    itemRow.EstadoSunatCodigo = item.Cs_pr_EstadoSUNAT;
                    itemRow.TipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_InvoiceTypeCode);
                    itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    lista_reporte.Add(itemRow);
                }
            }

            dgComprobantes.ItemsSource = lista_reporte;
        }

        private void cargarGrilla()
        {
            ComboBoxPares cbpTC = (ComboBoxPares)cboTipoComprobante.SelectedItem;
            ComboBoxPares cbpESCC = (ComboBoxPares)cboEstadoSCC.SelectedItem;
            ComboBoxPares cbpES = (ComboBoxPares)cboEstadoSunat.SelectedItem;

            if (datePick_inicio.SelectedDate != null)
            {

                DateTime fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");

            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }

            if (datePick_fin.SelectedDate != null)
            {
                DateTime fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            cs_pxCargarDgvComprobanteselectronicos(cbpTC._Id, cbpESCC._Id, cbpES._Id, fecha_inicio_formato, fecha_fin_formato);

        }
        //Evento para filtra los comprobantes.
        private void btnFiltro_Click(object sender, RoutedEventArgs e)
        {
            cargarGrilla();
        }
        //Evento para descargar reporte.
        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            if (lista_reporte.Count > 0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF(lista_reporte, configuracion.cs_prRutareportesPDF + "\\FACTURAS-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV(lista_reporte, configuracion.cs_prRutareportesCSV + "\\FACTURAS-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                        break;

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsg("Error al generar reporte", "Debe existir datos listados para generar el reporte.");
            }

        }

        private void DetalleItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReporteDocumento item = (ReporteDocumento)dgComprobantes.SelectedItem;
                if (item != null)
                {
                    frmDetalleComprobante Formulario = new frmDetalleComprobante(item.Id, localDB);
                    Formulario.ShowDialog();
                    if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
                    {
                        //cargarDataGrid();
                    }
                    // refrescarGrillaDocumentos(item.ComunicacionBaja);
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("detalle item " + ex.ToString());
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
                if (compare == 0)
                {
                    AyudaPrincipal ayuda = new AyudaPrincipal("4");
                    ayuda.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }

        }

        private void btnDescargaCDR_Click(object sender, RoutedEventArgs e)
        {
        //    btncdr.IsEnabled = false;
        //    clsBaseConfiguracion conf = new clsBaseConfiguracion();
        //    //Obtener el usuario seleccionado.
        //    ReporteDocumento item = (ReporteDocumento)dgComprobantes.SelectedItem;
        //    //Si existe el item seleccionado.
        //    if (item != null)
        //    {
        //       /* if (item.EstadoSunatCodigo == "0" || item.EstadoSunatCodigo == "1" || item.EstadoSunatCodigo == "3")
        //        {*/
        //            clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);

        //            string nombreCarpeta = declarante.Cs_pr_Ruc + "-" + item.Tipo + "-" + item.SerieNumero + ".zip-dc";
        //            string[] dirs = Directory.GetDirectories(conf.cs_prRutadocumentosrecepcion, "*" + nombreCarpeta);

        //            if (dirs.Length > 0)
        //            {
        //                string rutaOpen = string.Empty;
        //                foreach (string dir in dirs)
        //                {
        //                    rutaOpen = dir;
        //                }
        //                //existe el cdr
        //                Process.Start("explorer.exe", rutaOpen);

        //            }
        //            else
        //            {
        //                try
        //                {
        //                    //descargar el cdr
        //                    string[] partes = item.SerieNumero.Split('-');
        //                    string serie = partes[0];
        //                    int numero = Convert.ToInt32(partes[1]);
        //                    byte[] comprobante_electronico_bytes = null;
        //                    SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
        //                    ServicioCDR.billServiceClient bsc = new ServicioCDR.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress("https://www.sunat.gob.pe:443/ol-it-wsconscpegem/billConsultService"));
        //                    bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
        //                    bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
        //                    ServicioCDR.statusResponse sr = new ServicioCDR.statusResponse();
        //                    bsc.Open();
        //                    sr = bsc.getStatusCdr(declarante.Cs_pr_Ruc, item.Tipo, serie, numero);
        //                    string code = sr.statusCode;
        //                    if (sr.statusCode == "0004")
        //                    {
        //                        comprobante_electronico_bytes = sr.content;
        //                    }
        //                    bsc.Close();
        //                    if (comprobante_electronico_bytes != null)
        //                    {
        //                        XmlDocument documentoXML = new XmlDocument();
        //                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + item.Tipo + "-" + item.SerieNumero + ".zip";

        //                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
        //                        string CDR = conf.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;

        //                        FileStream fs = new FileStream(CDR, FileMode.Create);
        //                        fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
        //                        fs.Close();
        //                        //string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
        //                        //Verificar el contenido del archivo
        //                        ZipFile.ExtractToDirectory(conf.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, conf.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
        //                        documentoXML.Load(conf.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-" + item.Tipo + "-" + item.SerieNumero + ".xml");

        //                        clsEntityDocument ce = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item.Id);

        //                        ce.Cs_pr_EstadoSCC = "0";

        //                        if (documentoXML.OuterXml.Contains("ha sido aceptad"))
        //                        {
        //                            ce.Cs_pr_EstadoSUNAT = "0";
        //                            ce.Cs_pr_CDR = documentoXML.OuterXml;
        //                        }
        //                        else
        //                        {
        //                            ce.Cs_pr_EstadoSUNAT = "1";
        //                            ce.Cs_pr_CDR = documentoXML.OuterXml;
        //                        }
                                
        //                        string cadena_xml = @documentoXML.OuterXml;
        //                        cadena_xml = cadena_xml.Replace("ext:", "");
        //                        cadena_xml = cadena_xml.Replace("cbc:", "");
        //                        cadena_xml = cadena_xml.Replace("cac:", "");
        //                        cadena_xml = cadena_xml.Replace("ds:", "");
        //                        cadena_xml = cadena_xml.Replace("ar:", "");
        //                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
        //                        if (cadena_xml.Trim().Length > 0)
        //                        {
        //                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">", "<ApplicationResponse>");
        //                            XmlDocument d = new XmlDocument();
        //                            d.LoadXml(cadena_xml);
        //                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
        //                        }
        //                        ce.cs_pxActualizar(false, false);
        //                        System.Windows.Forms.MessageBox.Show("El CDR correspondiente se ha descargado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                        cargarGrilla();
        //                        Process.Start("explorer.exe", conf.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
        //                    }
        //                    else
        //                    {
        //                        System.Windows.Forms.MessageBox.Show("No se ha podido descargar el CDR correspondiente de la Sunat. Intente nuevamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    clsBaseLog.cs_pxRegistarAdd("descrgar cdr" + ex.ToString());
        //                    System.Windows.Forms.MessageBox.Show("No se ha podido descargar el CDR correspondiente. Revise el archivo de errores para mayor información.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                }

        //            }
        //     /*   }
        //        else
        //        {
        //            System.Windows.Forms.MessageBox.Show("Solo se puede descargar el CDR de comprobantes que hayan sido enviados a Sunat", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }*/
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Seleccione un comprobante", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //    btncdr.IsEnabled = true;

        }

        //Cristhian|14/02/2018|FEICONT2-487
        /*NUEVO INICIO*/
        /// <summary>
        /// Verifica si existe un documetno duplicado y si el documento seleccionado esta aceptado por SUNAT.
        /// Dependiendo de la verificación se habilitara y deshabilitara el botón de Descarga CDR.
        /// Si no tiene duplicado y esta aceptado por SUNAT se habilita el boton de descarga de CDR.
        /// Si tiene duplicado(s) y no esta aceptado por SUNAT se dehabilita el botón de descarga de CDR.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_Verificar_Duplicidad(object sender, RoutedEventArgs e)
        {
            clsBaseConfiguracion conf = new clsBaseConfiguracion();
            
            /*Se intenta realizar el proceso*/
            try
            {
                /*Se obtinene los datos del item Seleccionado*/
                ReporteDocumento item = (ReporteDocumento)dgComprobantes.SelectedItem;

                /*Si no tiene seleccionado no se realiza nada*/
                if (item != null)
                {
                    /*Se obtiene los datos de conexion de la base de datos*/            
                    clsEntityDocument ce = new clsEntityDocument(localDB);

                    /*Se envia los datos a la funcion "cs_Buscar_DocumentoDuplicado" que nos devolvera True or False*/
                    btncdr.IsEnabled = ce.cs_Buscar_DocumentoDuplicado(item.Id,item.SerieNumero,item.FechaEmision,item.Tipo);
                }
            }
            /*Si existe algun error se registra en el archivo log*/
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Buscar Duplicado Reporte Factura: " + ex.ToString());
            }
        }
        /*NUEVO FIN*/
    }
}
