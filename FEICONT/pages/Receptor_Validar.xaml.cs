using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Tesseract;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Timers;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Receptor_Validar.xaml
    /// </summary>
    public partial class Receptor_Validar : System.Windows.Controls.Page
    {
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();
        List<DocumentoValidar> lista_reporte = new List<DocumentoValidar>();
        List<clsEntidadDocument> registros = new List<clsEntidadDocument>();
        List<string> no_cargados = new List<string>();
        System.Timers.Timer timer1 = new System.Timers.Timer();
        System.Timers.Timer timer2 = new System.Timers.Timer();
        System.Windows.Forms.WebBrowser webBrowser = null;
        DocumentoValidar itemRow;
        private int archivosExistentes = 0;
        private int numProcesados = 0;
        private int numNoProcesados = 0;
        private bool verificarDocs = false;
        private clsEntityDatabaseLocal localDB;
        private int cargoPagina = 0;
        private int docsValidados = 0;

        private string idDoc = "";
        private string tipoDoc = "";
        private string fechaEmisionDoc = "";
        private string serieDoc = "";
        private string numeroDoc = "";
        private string montoDoc = "";
        private string rucEmisorDoc = "";
        private string Ruc = "";
        private Window VentanaPrincipal = null;

        public Receptor_Validar(clsEntityDatabaseLocal local, Window parent,string RUC)
        {
            InitializeComponent();
            localDB = local;
            VentanaPrincipal = parent;
            Ruc = RUC;
        }
        /// <summary>
        /// Seleccion de la carpeta a importar archivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            // BOTON DE NUEVO FOLDER ACTIVADO
            folderBrowserDlg.ShowNewFolderButton = true;
            // MOSTRAR CUADRO DE DIALOGO
            DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult.Equals(DialogResult.OK))
            {
                txtRutaUnico.Text = folderBrowserDlg.SelectedPath;
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }
        /// <summary>
        /// Procesar los archivos de la ruta seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCargarArchivo_Click(object sender, RoutedEventArgs e)
        {
            ArchivosNoCargados.Visibility = Visibility.Hidden;
            lblValidaron.Content = "Se validaron 0 documentos.";
            //leer todos los xml y cargarlos a la base de datos poner estado verificar en no verificado (3) y no validado (4) y procesado en 0
            //contabilizar procesados y no procesados
            if (txtRutaUnico.Text.Trim().Length > 0)
            {
                numProcesados = 0;
                numNoProcesados = 0;
                archivosExistentes = 0;
                List<string> lista_rutas_facturas = new List<string>();
                List<string> lista_rutas_boletas = new List<string>();
                List<string> lista_rutas_notascredito = new List<string>();
                List<string> lista_rutas_notasdebito = new List<string>();
                no_cargados = new List<string>();
                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);

                string[] dirs = Directory.GetFiles(txtRutaUnico.Text, "*.xml");
                archivosExistentes = dirs.Length;

                if (dirs.Length > 0)
                {
                    foreach (string dir in dirs)
                    {
                        string nombreArchivo = System.IO.Path.GetFileName(dir);
                        string contenidoArchivo = File.ReadAllText(dir);
                        string resultado = new clsBaseXML().procesarXML(contenidoArchivo, localDB, declarante.Cs_pr_Ruc);

                        if (resultado.Length > 0)
                        {
                            numProcesados++;
                        }
                        else
                        {
                            numNoProcesados++;
                            no_cargados.Add(nombreArchivo);
                        }
                    }

                    lblInfoCarga.Content = "Se cargaron " + numProcesados + " archivos y no se cargaron " + numNoProcesados + " archivos.";
                    if (numNoProcesados > 0)
                    {
                        ArchivosNoCargados.Visibility = Visibility.Visible;
                    }
                    cs_pxCargarDgvComprobanteselectronicos();
                }
                else
                {
                    lblInfoCarga.Content = "Se cargaron 0 documentos.";
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Seleccione una ruta válida.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        /// <summary>
        /// Creacion de los reportes en excel y csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            string RutaReporte = string.Empty;
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            // BOTON DE NUEVO FOLDER ACTIVADO
            folderBrowserDlg.ShowNewFolderButton = true;
            // MOSTRAR CUADRO DE DIALOGO
            DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult.Equals(DialogResult.OK))
            {
                RutaReporte = folderBrowserDlg.SelectedPath;
            }


            if (lista_reporte.Count > 0)
            {
                if (RutaReporte.Trim().Length > 0)
                {
                    clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                    switch (cboDownload.SelectedIndex)
                    {
                        case 0:
                            clsBaseReporte.cs_pxReportePDF_Validar(lista_reporte, RutaReporte + "\\VALIDAR-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                            break;
                        case 1:
                            clsBaseReporte.cs_pxReporteCSV_Validar(lista_reporte, RutaReporte + "\\VALIDAR-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                            break;

                    }
                }
                else
                {
                    clsBaseMensaje.cs_pxMsgError("Error al generar reporte", "Seleccione una ruta valida.");

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsgError("Error al generar reporte", "Debe existir datos listados para generar el reporte.");
            }
        }
        /// <summary>
        /// Evento de carga de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //Agregar valores a combobox de tipo exportar
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;

            timer1.Interval = 15000;
            timer1.Elapsed += new ElapsedEventHandler(runWorkerTimer);
            // timer1.Start();


            // btnExportar.IsEnabled = false;
            // btnGrabar.IsEnabled = false;
            webBrowser = new System.Windows.Forms.WebBrowser();
            //webBrowser.Url = new Uri("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/ConsValiCpe.htm", UriKind.Absolute);
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
            //cargar los docs del paso anterior buscar en bd los estados activos que aun no se han procesado
            cs_pxCargarDgvComprobanteselectronicos();

        }
        /// <summary>
        /// Metodo que ejecuta el timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runWorkerTimer(object sender, ElapsedEventArgs e)
        {
            webBrowser.Stop();
            timer1.Enabled = false;
            cargoPagina = 2;
        }
        /// <summary>
        /// Metodo para validar el certificado en las consultas de webservice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        /// <summary>
        /// Evento de carga completa del webbrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //clsBaseLog.cs_pxRegistarAdd(numeroDoc);
            //SI ES la primera carga  y se presiono verificar 
            try
            {
                HtmlDocument documentBrowser = webBrowser.Document;
                HtmlElement bodyDocument = documentBrowser.Body;
                if (bodyDocument.InnerText != null)
                {
                    if (bodyDocument.InnerText.Contains("refrescando") && verificarDocs == true)
                    {
                        clsBaseLog.cs_pxRegistarAdd(bodyDocument.InnerText + " - " + numeroDoc);
                        cargoPagina++;
                    }
                    else if (bodyDocument.InnerText.Contains("Resultado") && verificarDocs == true)
                    {
                        cargoPagina++;
                        string comentario = "";
                        //System.Windows.Forms.MessageBox.Show(bodyDocument.InnerText);
                        HtmlElementCollection html_nodo_lista_tables = webBrowser.Document.GetElementsByTagName("table");
                        if (html_nodo_lista_tables.Count > 0)
                        {
                            HtmlElementCollection col = html_nodo_lista_tables[4].Document.GetElementsByTagName("td");
                            comentario = col[4].InnerText;
                        }
                        clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(idDoc);
                        doc.Cs_pr_ComentarioSUNAT = comentario;
                        if (comentario.Contains("válido"))
                        {
                            doc.Cs_pr_EstadoValidar = "0";

                        }
                        else if (comentario.Contains("BAJA"))
                        {
                            doc.Cs_pr_EstadoValidar = "2";
                        }
                        else
                        {
                            doc.Cs_pr_EstadoValidar = "3";
                        }

                        if (tipoDoc == "2" || tipoDoc == "4" || tipoDoc == "5")
                        {
                            if (!comentario.Contains("no"))
                            {
                                //buscar si esta de baja
                                if (comentario.Contains("BAJA"))
                                {
                                    doc.Cs_pr_EstadoValidar = "2";
                                }
                                else
                                {
                                    doc.Cs_pr_EstadoValidar = "0";
                                }

                            }
                            else
                            {
                                doc.Cs_pr_EstadoValidar = "3";
                            }
                        }

                        doc.cs_pxActualizar(false, false);
                        docsValidados++;
                    }
                    else if (webBrowser.DocumentText.Contains("lista") && verificarDocs == true)
                    {
                        cargoPagina++;
                        HtmlElementCollection html_nodo_lista = webBrowser.Document.GetElementsByTagName("form");
                        HtmlElementCollection html_nodo_lista_tables = html_nodo_lista[0].Document.GetElementsByTagName("table");
                        if (html_nodo_lista_tables.Count > 0)
                        {
                            HtmlElementCollection col = html_nodo_lista_tables[4].Document.GetElementsByTagName("input");

                            HtmlElement rucEmisor = col.GetElementsByName("num_ruc")[0];
                            rucEmisor.SetAttribute("value", rucEmisorDoc);

                            HtmlElementCollection selects = html_nodo_lista_tables[4].Document.GetElementsByTagName("select");
                            HtmlElement selectTipoComprobante = selects.GetElementsByName("tipocomprobante")[0];
                            selectTipoComprobante.Children[Convert.ToInt32(tipoDoc)].SetAttribute("selected", "selected");

                            HtmlElement numSerie = col.GetElementsByName("num_serie")[0];
                            numSerie.SetAttribute("value", serieDoc);

                            HtmlElement numComprob = col.GetElementsByName("num_comprob")[0];
                            numComprob.SetAttribute("value", numeroDoc);

                            HtmlElement fecEmision = col.GetElementsByName("fec_emision")[0];
                            fecEmision.SetAttribute("value", fechaEmisionDoc);

                            if (tipoDoc != "2" && tipoDoc != "4" && tipoDoc != "5")
                            {

                                HtmlElement montoDocumento = col.GetElementsByName("cantidad")[0];
                                montoDocumento.SetAttribute("value", montoDoc);
                            }
                            string nombreRandom = RandomString(5);
                            var testImagePath = Directory.GetCurrentDirectory() + "\\imageCaptcha.png";

                            string s = GetGlobalCookies(webBrowser.Document.Url.AbsoluteUri);
                            //clsBaseLog.cs_pxRegistarAdd(s + " - " + numeroDoc);
                            WebClient Client1 = new WebClient();
                            Client1.Headers.Add(HttpRequestHeader.Cookie, s);
                            Client1.DownloadFile("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/captcha?accion=image&nmagic=0", testImagePath);
                            Client1.Dispose();

                            string textoCaptchaIngresar = textoCaptcha("imageCaptcha");
                            if (textoCaptchaIngresar.Contains("'"))
                            {
                                textoCaptchaIngresar = textoCaptchaIngresar.Replace("'", "I");
                            }
                            if (textoCaptchaIngresar.Contains(" "))
                            {
                                textoCaptchaIngresar = textoCaptchaIngresar.Replace(" ", "");
                            }
                            if (textoCaptchaIngresar.Trim().Length < 4)
                            {
                                //VOLVER A DESCARGAR 
                                WebClient Client2 = new WebClient();
                                Client2.Headers.Add(HttpRequestHeader.Cookie, s);
                                Client2.DownloadFile("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/captcha?accion=image&nmagic=0", testImagePath);
                                Client2.Dispose();
                                textoCaptchaIngresar = textoCaptcha("imageCaptcha");
                                if (textoCaptchaIngresar.Contains("'"))
                                {
                                    textoCaptchaIngresar = textoCaptchaIngresar.Replace("'", "I");
                                }
                                if (textoCaptchaIngresar.Contains(" "))
                                {
                                    textoCaptchaIngresar = textoCaptchaIngresar.Replace(" ", "");
                                }
                                if (textoCaptchaIngresar.Trim().Length < 4)
                                {
                                    textoCaptchaIngresar = "AAAA";
                                }
                            }
                            // clsBaseLog.cs_pxRegistarAdd("uno"+textoCaptchaIngresar);
                            HtmlElement codigoCaptcha = col.GetElementsByName("codigo")[0];
                            codigoCaptcha.SetAttribute("value", textoCaptchaIngresar.ToUpper());
                            Thread.Sleep(400);
                            string elementValue = codigoCaptcha.GetAttribute("value");
                            if (elementValue.Length < 4)
                            {
                                codigoCaptcha.SetAttribute("value", textoCaptchaIngresar.ToUpper());
                                string elementValue2 = codigoCaptcha.GetAttribute("value");
                                if (elementValue2.Length <= 0)
                                {
                                    codigoCaptcha.SetAttribute("value", textoCaptchaIngresar.ToUpper());
                                }
                            }
                            //clsBaseLog.cs_pxRegistarAdd(textoCaptchaIngresar);
                            //end Codigo captcha
                            HtmlElementCollection html_nodo_lista_inputenviar = html_nodo_lista.GetElementsByName("wacepta");
                            HtmlElementCollection html_nodo_lista_inputs = html_nodo_lista[0].Document.GetElementsByTagName("input");

                            if (html_nodo_lista_inputs.Count > 8)
                            {
                                HtmlElement inputEnviar = html_nodo_lista_inputs[8];
                                // timer2.Interval = 900; // 'Give it a few seconds to make sure the OpenFileDialog is open
                                // timer2.Elapsed += new ElapsedEventHandler(runWorkerTimerMessageBox);
                                // timer2.Start();
                                inputEnviar.InvokeMember("click");

                            }
                            string elementValue2s = codigoCaptcha.GetAttribute("value");
                            // clsBaseLog.cs_pxRegistarAdd("dos"+elementValue2s);
                            //File.Delete(testImagePath);

                        }
                        // ClickOKButton();
                    }
                    else
                    {
                        if (verificarDocs == true)
                        {
                            clsBaseLog.cs_pxRegistarAdd(bodyDocument.InnerText);
                        }

                    }
                }
                else
                {
                    clsBaseLog.cs_pxRegistarAdd("browser null");
                    cargoPagina = 2;
                }

                //  Mostrar_Formulario_Detalle_Mensaje = false;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                cargoPagina = 2;
                /*No se ejecuta nada para que no detenga el proceso de busqueda de mensajes, ya que esta función
                  es invocada mas de tres veces*/
            }

        }
        /// <summary>
        /// Metodo para aceptar la ventana emergente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runWorkerTimerMessageBox(object sender, ElapsedEventArgs e)
        {

            timer2.Stop();
            try
            {
                // ClickOKButton();
                SendKeys.SendWait("{ENTER}"); // 'Press the Open button
            }
            catch
            {

            }

        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Metodo para realizar clic en el aceptar
        /// </summary>
        private void ClickOKButton()
        {
            try
            {
                IntPtr hwnd = FindWindow("#32770", null);
                hwnd = FindWindowEx(hwnd, IntPtr.Zero, "Button", "Aceptar");
                uint message = 0xf5;
                SendMessage(hwnd, message, IntPtr.Zero, IntPtr.Zero);
            }
            catch
            {

            }

        }
        /// <summary>
        /// Metodo de carga de registros en la grilla
        /// </summary>
        private void cs_pxCargarDgvComprobanteselectronicos()
        {
            dgDocumentos.ItemsSource = null;
            dgDocumentos.Items.Clear();
            //Obtener los comprobantes de factura.
            registros = new clsEntidadDocument(localDB).cs_pxObtenerActivosValidado(Ruc);
            lista_reporte = new List<DocumentoValidar>();
            //lista_reporte = new ObservableCollection<Reporte>();
            if (registros != null)
            {
                //Recorrer los registros para llenar la grilla.
                foreach (var item in registros)
                {
                    itemRow = new DocumentoValidar();
                    itemRow.Id = item.Cs_pr_Document_Id;
                    itemRow.Tipo = item.Cs_tag_InvoiceTypeCode;
                    itemRow.SerieNumero = item.Cs_tag_ID;
                    itemRow.FechaEmision = item.Cs_tag_IssueDate;
                    itemRow.Ruc = item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;
                    itemRow.RucEmisor = item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                    itemRow.RazonSocial = item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                    itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                    itemRow.TipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_InvoiceTypeCode);
                     itemRow.EstadoValidarTexto = clsBaseUtil.cs_fxComprobantesEstadosValidar_Descripcion(Convert.ToInt16(item.Cs_pr_EstadoValidar)).ToUpper();
                    itemRow.EstadoValidar = item.Cs_pr_EstadoValidar;
                    itemRow.Monto = item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID;
                    itemRow.NumeroRelacionado = item.Cs_tag_BillingReference_ID;
                    itemRow.TipoRelacionado = item.Cs_tag_BillingReference_DocumentTypeCode;
                    lista_reporte.Add(itemRow);
                }
            }

            dgDocumentos.ItemsSource = lista_reporte;
            lblNumeroDocumentos.Content = "Existen " + lista_reporte.Count + " documentos listados.";
        }
        /// <summary>
        /// Evento para eliminar el comprobante seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<DocumentoValidar> seleccionados = new List<DocumentoValidar>();
                foreach (var it in lista_reporte)
                {
                    if (it.Check == true)
                    {
                        seleccionados.Add(it);
                    }
                }
                if (seleccionados.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea descartar los documentos seleccionados?\nEstos documentos serán eliminados completamente de la base de datos.", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        string mensaje = string.Empty;
                        foreach (DocumentoValidar row in seleccionados)
                        {                       
                                bool resultado = new clsEntidadDocument(localDB).cs_pxEliminarDocumento(row.Id);
                                if (resultado == false)
                                {
                                    mensaje += row.SerieNumero + "\n";
                                }
                            
                        }
                        if (mensaje.Length > 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Los siguientes documentos no se eliminaron de la base de datos.\n" + mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Los documentos se eliminaron de la base de datos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        cs_pxCargarDgvComprobanteselectronicos();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Debe seleccionar documento(s) a eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("delete docs" + ex.ToString());
                //clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
            }
        }
        /// <summary>
        /// Evento para validar los documentos listados que falten realizar el proceso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidarArchivos_Click(object sender, RoutedEventArgs e)
        {
            if (lista_reporte.Count > 0)
            {
                verificarDocs = true;
                int count = 0;
                //obtener todos los docs sin validar y por cada uno reload la pagina 
                docsValidados = 0;

                foreach (var item in lista_reporte)
                {
                    if (item.EstadoValidar == "4")
                    {
                        count++;
                    }
                }

                if (count > 0)
                {
                    string resultado = string.Empty;
                    ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                        resultado = validarDocs();
                    });
                    liberarVariableConsulta();
                }
                verificarDocs = false;
                lblValidaron.Content = "Se validaron " + docsValidados + " documentos de " + count;
                cs_pxCargarDgvComprobanteselectronicos();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No existen documentos a validar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // btnGrabar.IsEnabled = true;
            // btnExportar.IsEnabled = true;
        }
        /// <summary>
        /// Metodo para procesar pa validacion de los documentos mientras se ejecuta el progress bar
        /// </summary>
        /// <returns></returns>
        public string validarDocs()
        {
            string docs_Validados = string.Empty;
            //Recorrer los registros para llenar la grilla.
            foreach (var item in lista_reporte)
            {
                cargoPagina = 0;
                if (item.EstadoValidar == "4")
                {
                    idDoc = item.Id;
                    if (item.Tipo == "01")
                    {
                        tipoDoc = "1";
                    }
                    else if (item.Tipo == "03")
                    {
                        tipoDoc = "2";
                    }
                    else if (item.Tipo == "07")
                    {
                        if (item.TipoRelacionado == "01")
                        {
                            tipoDoc = "6";
                        }
                        else if (item.TipoRelacionado == "03")
                        {
                            tipoDoc = "4";
                        }

                    }
                    else if (item.Tipo == "08")
                    {
                        if (item.TipoRelacionado == "01")
                        {
                            tipoDoc = "7";
                        }
                        else if (item.TipoRelacionado == "03")
                        {
                            tipoDoc = "5";
                        }
                    }

                    string[] partes = item.SerieNumero.Split('-');
                    serieDoc = partes[0];
                    string primeraLetra = serieDoc.Substring(0, 1);
                    numeroDoc = partes[1];
                    DateTime dt = DateTime.ParseExact(item.FechaEmision, "yyyy-MM-dd", null);
                    fechaEmisionDoc = dt.ToString("dd/MM/yyyy");
                    rucEmisorDoc = item.RucEmisor;
                    montoDoc = item.Monto;
                    /*if (tipoDoc == "1" || tipoDoc == "6" || tipoDoc == "7" )
                    {
                        if (primeraLetra != "E")
                        {
                            //consultar en webservice
                            try
                            {
                                string estadoComprobante = string.Empty;
                                SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                                ServiceValidez.billServiceClient bsc = new ServiceValidez.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress("https://www.sunat.gob.pe:443/ol-it-wsconscpegem/billConsultService"));
                                //bsc.ClientCredentials.UserName.UserName = "20508997567MBENEL93";
                                //bsc.ClientCredentials.UserName.Password = "Zurita93";
                                bsc.ClientCredentials.UserName.UserName = Ruc+UsuarioSol;
                                bsc.ClientCredentials.UserName.Password = ClaveSol;
                                ServiceValidez.statusResponse sr = new ServiceValidez.statusResponse();
                                //sr = bsc.getStatus("20508997567", "01", "F001", 1484);
                                bsc.Open();
                                sr = bsc.getStatus(rucEmisorDoc, item.Tipo, serieDoc, Convert.ToInt32(numeroDoc));
                                string valorEstado = sr.statusCode;
                                string valorComentario = sr.statusMessage;
                                bsc.Close();
                               
                                switch (valorEstado)
                                {
                                    case "0001":
                                        estadoComprobante = "0";
                                        break;
                                    case "0002":
                                        estadoComprobante = "1";
                                        break;
                                    case "0003":
                                        estadoComprobante = "2";
                                        break;
                                    case "0011":
                                        estadoComprobante = "3";
                                        break;
                                    default:
                                        estadoComprobante = "5";
                                        break;
                                }

                                if (estadoComprobante == "3")
                                {
                                    webBrowser.Navigate(new Uri("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/ConsValiCpe.htm", UriKind.Absolute));
                                   // timer1.Enabled = true;
                                   // timer1.Start();
                                    do
                                    {
                                        Thread.Sleep(5);
                                        DoEvents();
                                    } while (cargoPagina < 2);
                                  //  timer1.Stop();
                                    liberarVariableConsulta();
                                    cargoPagina = 0;
                                }
                                else
                                {
                                    clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(idDoc);
                                    doc.Cs_pr_EstadoValidar = estadoComprobante;
                                    doc.Cs_pr_ComentarioSUNAT = valorEstado + " - " + valorComentario;
                                    doc.cs_pxActualizar(false, false);
                                    docsValidados++;
                                }
                              
                            }
                            catch (Exception ex)
                            {
                                clsBaseLog.cs_pxRegistarAdd(ex.Message + " ---- " + ex.ToString());
                                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                            }
                        }else
                        {
                            webBrowser.Navigate(new Uri("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/ConsValiCpe.htm", UriKind.Absolute));
                          //  timer1.Enabled = true;
                          //  timer1.Start();
                            do
                            {
                                Thread.Sleep(5);
                                DoEvents();
                            } while (cargoPagina < 2);
                         //   timer1.Stop();
                            liberarVariableConsulta();
                            cargoPagina = 0;
                        }
                      

                    }
                    else
                    {*/  //conusltar en la web 
                         // webBrowser.Refresh();
                         // webBrowser.Dispose();
                    webBrowser.Navigate(new Uri("http://www.sunat.gob.pe/ol-ti-itconsvalicpe/ConsValiCpe.htm", UriKind.Absolute));
                    //   timer1.Enabled = true;
                    //   timer1.Start();
                    do
                    {
                        Thread.Sleep(5);
                        DoEvents();
                    } while (cargoPagina < 2);
                    //   timer1.Stop();
                    liberarVariableConsulta();
                    cargoPagina = 0;
                    // }
                }
            }
            return docs_Validados;
        }
        /// <summary>
        /// Metodo para liberar las variables globales de consulta en cada navegacion web
        /// </summary>
        public void liberarVariableConsulta()
        {
            idDoc = "";
            tipoDoc = "";
            serieDoc = "";
            numeroDoc = "";
            fechaEmisionDoc = "";
            rucEmisorDoc = "";
            montoDoc = "";
        }
        /// <summary>
        /// Metodo para continuar la ejecucion de procesos en segundo plano
        /// </summary>
        public void DoEvents()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }

        }
        /// <summary>
        /// Metodo para obtner el  texto de una imagen
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public string textoCaptcha(string nombre)
        {
            var testImagePath = Directory.GetCurrentDirectory() + "\\" + nombre + ".png";
            var dataPath = Directory.GetCurrentDirectory() + "\\tesseract-ocr\\tessdata";
            string prueba = string.Empty;
            try
            {
                using (var tEngine = new TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
                {
                    using (Pix img = Pix.LoadFromFile(testImagePath)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                    {
                        using (var page = tEngine.Process(img)) //process the specified image
                        {
                            var text = page.GetText(); //Gets the image's content as plain text.
                            prueba += text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                //Console.WriteLine("Unexpected Error: " + ex.Message);
            }
            return prueba.Trim();
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);
        const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        /// <summary>
        /// Metodo para obtener las cookies globales
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetGlobalCookies(string uri)
        {
            uint size = 0;
            InternetGetCookieEx(uri, null, null, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero);
           // uint datasize = 1024;
            StringBuilder cookieData = new StringBuilder((int)size);
            if (InternetGetCookieEx(uri, null, cookieData, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero)
            && cookieData.Length > 0)
            {
                return cookieData.ToString();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Metodo para guardar los documetnos validad y pasarlos al siguiente paso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (var item in lista_reporte)
            {
                if (item.EstadoValidar != "4")
                {
                    clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(item.Id);
                    doc.Cs_pr_Procesado = "2";
                    doc.cs_pxActualizar(false, false);
                    count++;
                }
            }
            if (count > 0)
            {
                //Mensage de exito de comprobantes y pasar al siguiente paso automaticamente.
                System.Windows.Forms.MessageBox.Show("Sus documentos electronicos validados fueron grabados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cs_pxCargarDgvComprobanteselectronicos();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No existen documentos a procesar.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private static Random random = new Random();
        /// <summary>
        /// Metodo para obtener una cadena de caracteres aleatorios segun numero de longitud
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// <summary>
        /// Evento al culminar la edicion de una celda en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgDocumentos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridRow item = e.Row;
            DocumentoValidar comprobante = (DocumentoValidar)item.DataContext;
            clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(comprobante.Id);
            doc.Cs_pr_ComentarioSUNAT = comprobante.Comentario;
            doc.cs_pxActualizar(false, false);
            // System.Windows.Forms.MessageBox.Show(comprobante.SerieNumero + " s " + comprobante.Comentario);
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (DocumentoValidar item in lista_reporte)
                    {
                        item.Check = true;
                    }
                    dgDocumentos.ItemsSource = null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (DocumentoValidar item in lista_reporte)
                    {
                        item.Check = false;
                    }
                    dgDocumentos.ItemsSource = null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            DocumentoValidar comprobante = (DocumentoValidar)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked)
            {
                comprobante.Check = true;
            }
            e.Handled = true;
        }

        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            DocumentoValidar comprobante = (DocumentoValidar)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                comprobante.Check = false;
            }
            e.Handled = true;
        }
        private void HyperlinkArchivoNoCargados_Click(object sender, RoutedEventArgs e)
        {
            Listado list = new Listado(no_cargados, "Los siguientes archivos no fueron cargados al sistema.", "No cargados");
            list.ShowDialog();
        }
    }
}
