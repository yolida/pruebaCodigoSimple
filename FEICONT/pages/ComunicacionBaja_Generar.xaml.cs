using FEICONT.ayuda;
using FEICONT.Base;
using FEICONT.CustomDialog;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using Extension.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para ComunicacionBaja_Generar.xaml
    /// </summary>
    public partial class ComunicacionBaja_Generar : Page
    {
        List<ComboBoxPares> tipos_comprobante = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_scc = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<clsEntityDocument> registros;
        List<ReporteDocumento> lista_reporte;
        ReporteDocumento itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        private Window VentanaPrincipal;
        private clsEntityDatabaseLocal localDB;
        /// <summary>
        /// Metodo constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="local"></param>
        public ComunicacionBaja_Generar(Window parent,clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            VentanaPrincipal = parent;
            localDB = local;
        }
        /// <summary>
        /// Evento de carga de la pagina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores al combobox de tipos de comprobante.
            tipos_comprobante.Add(new ComboBoxPares("", "Seleccione"));
            tipos_comprobante.Add(new ComboBoxPares("01", "Factura Electronica"));
            tipos_comprobante.Add(new ComboBoxPares("07", "Nota de Credito"));
            tipos_comprobante.Add(new ComboBoxPares("08", "Nota de Debito"));
            cboTipoComprobante.DisplayMemberPath = "_Value";
            cboTipoComprobante.SelectedValuePath = "_Key";
            cboTipoComprobante.SelectedIndex = 0;
            cboTipoComprobante.ItemsSource = tipos_comprobante;

            //Agregar valores la combobox de estado SCC
            estados_scc.Add(new ComboBoxPares("", "Seleccione"));
            estados_scc.Add(new ComboBoxPares("0", "Enviado"));
            //estados_scc.Add(new ComboBoxPares("1", "Pendiente (Errores)"));
            estados_scc.Add(new ComboBoxPares("2", "Pendiente (Correcto)"));
            cboEstadoSCC.DisplayMemberPath = "_Value";
            cboEstadoSCC.SelectedValuePath = "_key";
            cboEstadoSCC.SelectedIndex = 0;
            cboEstadoSCC.ItemsSource = estados_scc;
            //Agregar valores al combobox de estados de sunat.
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

            actualizarGrilla();
        }
        /// <summary>
        /// Cargar comprobantes segun filtro
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="estadocomprobantescc"></param>
        /// <param name="estadocomprobantesunat"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        private void cs_pxCargarDgvComprobanteselectronicos(string tipo, string estadocomprobantescc, string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            bool AddToList;
            //Obtener el lista de comprobantes en comunicacion de baja.
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroComunicacionBaja(tipo, estadocomprobantesunat, estadocomprobantescc, fechainicio, fechafin);
            lista_reporte = new List<ReporteDocumento>();
            if (registros!=null)
            {
                //Recorrer los registros para rellenar el grid.
                foreach (var item in registros)
                {
                    AddToList = false;

                    if (item.Cs_tag_InvoiceTypeCode == "03" || item.Cs_tag_BillingReference_DocumentTypeCode == "03")
                    {
                        //si es boleta o nota asociada agregar solo si esta por enviar o aceptado
                        if (item.Cs_pr_EstadoSUNAT == "0" || item.Cs_pr_EstadoSUNAT == "2")
                        {
                            AddToList = true;
                        }
                    }
                    else
                    {
                        if (item.Cs_pr_EstadoSUNAT == "0")
                        {
                            AddToList = true;
                        }
                    }
                    if (AddToList)
                    {
                        itemRow = new ReporteDocumento();
                        itemRow.Id = item.Cs_pr_Document_Id;//
                        itemRow.Tipo = item.Cs_tag_InvoiceTypeCode;//
                        itemRow.SerieNumero = item.Cs_tag_ID;//
                        itemRow.FechaEmision = item.Cs_tag_IssueDate;//
                        itemRow.FechaEnvio = item.comprobante_fechaenviodocumento;
                        itemRow.Ruc = item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;//
                        itemRow.RazonSocial = item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;//
                        itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                        itemRow.ComunicacionBaja = item.Cs_pr_ComunicacionBaja;
                        itemRow.ResumenDiario = item.Cs_pr_Resumendiario;
                        itemRow.DocumentoReferencia = item.Cs_tag_BillingReference_DocumentTypeCode;
                        itemRow.TipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_InvoiceTypeCode);
                        itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                        itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                        lista_reporte.Add(itemRow);
                    }
                }
            }          
            dgComprobantes.ItemsSource = lista_reporte;
        }
        /// <summary>
        /// Evento Check de los items en la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtner elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccionado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteDocumento doc = (ReporteDocumento)dataGridRow.DataContext;
            if ((bool)checkBox.IsChecked)
            {
                doc.Check = true;
            }
            e.Handled = true;
        }
        /// <summary>
        /// Evento Uncheck de los items en la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccinado
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteDocumento doc = (ReporteDocumento)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                doc.Check = false;
            }
            e.Handled = true;
        }
       
        /// <summary>
        /// Evento click para enviar los comprobantes a comunicacion de baja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarSunat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_procesados = string.Empty;
                //Recorrer las grilla para obtener los elementos seleccionados y alamcenarlos en una lista con los ids de los seleccionados.
                List<string> seleccionados = new List<string>();
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true )
                    {
                        //Si el comprobante no esta asigando a una comunicacion de baja se guarda como seleccionado
                        if (item.ComunicacionBaja == "")
                        {
                            seleccionados.Add(item.Id);
                        }
                        else
                        {
                            //En caso este asignado ya a una comunicacion de baja entonces asignar a no procesados.
                            no_procesados += item.SerieNumero+"\n";
                        }
                       
                    }                  
                }
                // Si existen comprobantes no procesados porque ya fueron agregados a comunicaion de baja entonces mostrar menaje al usuario.
                if (no_procesados.Trim().Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Los siguientes comprobantes no sera procesados. Ya fueron agregados a comunicación de baja\n"+ no_procesados, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Si existen comprobantes seleccionados entonces procesar
                if (seleccionados.Count > 0) {
                    //Confirmacion para enviar a comunicacion de baja los documentos seleccionados.
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea enviar a comunicación de baja los documentos seleccionados?", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string resultadoNoAgregados = string.Empty;
                        //
                        ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                            resultadoNoAgregados = ProcesarComunicacionBaja(seleccionados);
                        });
                        if (resultadoNoAgregados.Trim().Length > 0)
                        {   //no se agregaron                           
                            CustomDialogWindow obj = new CustomDialogWindow();
                            obj.AdditionalDetailsText = "Los comprobantes no agregados son los siguientes:\n" + resultadoNoAgregados;
                            obj.Buttons = CustomDialogButtons.OK;
                            obj.Caption = "Mensaje";
                            obj.DefaultButton = CustomDialogResults.OK;
                            // obj.FooterIcon = CustomDialogIcons.Shield;
                            // obj.FooterText = "This is a secure program";
                            obj.InstructionHeading = "Documentos no agregados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Algunos documentos no se agregaron a su comunicacion de baja. Verifique la fecha de emision no mayor a 7 dias.";
                            CustomDialogResults objResults = obj.Show();
                        }
                        else
                        {
                            //Si el resultado es vacio quiere decir que se agregaron todos los comprobantes:
                            System.Windows.Forms.MessageBox.Show("Los documentos se agregaron a su respectiva comunicación de baja.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);                          
                        }
                        actualizarGrilla();
                    }
                }               
            }
            catch(Exception ex )
            {
                clsBaseLog.cs_pxRegistarAdd("btnEnviarSunat "+ ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para procesar las comunicaciones de baja.
        /// </summary>
        /// <param name="seleccionados"></param>
        /// <returns></returns>
        private string ProcesarComunicacionBaja(List<string> seleccionados)
        {
            string resultadoNoAgregados = string.Empty;
            try
            {
                //Enviar los ids de los seleccionados para procesar en la comunicacion de baja
                resultadoNoAgregados = new clsNegocioCEComunicacionBaja(localDB).cs_pxProcesarComunicacionBaja(seleccionados,"0");
            }
            catch
            {            
                resultadoNoAgregados = string.Empty;
            }      
            return resultadoNoAgregados;
        }
        /// <summary>
        /// Metodo para actualizar el contenido de la grilla.
        /// </summary>
        private void actualizarGrilla()
        {   
            //Obtener objetos asociados de los valores seleccionados en los combobox.
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
        /// <summary>
        /// Evento click para Filtrar los comprobantes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            actualizarGrilla();
        }
        /// <summary>
        /// Evento para mostrar detalle de los comprobantes asociados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Evento para descargar el xml de envio para los documentos relacionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XMLEnvio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReporteDocumento item = (ReporteDocumento)dgComprobantes.SelectedItem;
                if (item != null)
                {
                    SaveFileDialog sfdDescargar = new SaveFileDialog();
                    DialogResult result = sfdDescargar.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        string file = sfdDescargar.FileName;
                        if (file.Substring(file.Length - 4) != ".xml")
                        {
                            file = file + ".xml";
                        }
                        try
                        {
                            clsEntityDocument cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item.Id);
                            string xml = string.Empty;

                            switch (cabecera.Cs_tag_InvoiceTypeCode)
                            {
                                case "01":
                                    xml = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(item.Id);
                                    break;
                                case "03":
                                    xml = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(item.Id);
                                    break;
                                case "07":
                                    xml = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(item.Id);
                                    break;
                                case "08":
                                    xml = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(item.Id);
                                    break;
                            }
                            StreamWriter sw0 = new StreamWriter(file);
                            sw0.Write(xml);
                            sw0.Close();
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
        /// <summary>
        /// Evento para descargar el xml de recepcion de los documentos asociados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XMLRecepcion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReporteDocumento item = (ReporteDocumento)dgComprobantes.SelectedItem;
                if (item != null)
                {
                    clsEntityDocument cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item.Id);
                    SaveFileDialog sfdDescargar = new SaveFileDialog();
                    DialogResult result = sfdDescargar.ShowDialog();
                    if (result == DialogResult.OK)
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
        /// <summary>
        /// Evento check para los seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (ReporteDocumento item in lista_reporte)
                    {
                        item.Check = true;
                    }
                    dgComprobantes.ItemsSource = null;
                    dgComprobantes.Items.Clear();
                    dgComprobantes.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// Evento uncheck para todos los seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (ReporteDocumento item in lista_reporte)
                    {
                        item.Check = false;
                    }
                    dgComprobantes.ItemsSource = null;
                    dgComprobantes.Items.Clear();
                    dgComprobantes.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("15");
                ayuda.ShowDialog();
            }
        }
   
    }
}
