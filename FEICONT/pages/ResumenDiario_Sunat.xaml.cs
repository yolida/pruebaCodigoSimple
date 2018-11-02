using FEICONT.ayuda;
using FEICONT.Base;
using FEICONT.CustomDialog;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Enviar  resumen diario a sunat.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para ResumenDiario_Sunat.xaml
    /// </summary>
    public partial class ResumenDiario_Sunat : Page
    {
        List<clsEntitySummaryDocuments> registros;
        List<ReporteResumen> lista_reporte;
        ReporteResumen itemRow;

        List<clsEntityDocument> documentos;
        List<ReporteDocumento> lista_documentos;
        ReporteDocumento itemComprobante;

        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();

        string fecha_inicio_formato;
        string fecha_fin_formato;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        ComboBoxPares cbpEstadoSunat;
        private Window VentanaPrincipal;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor
        public ResumenDiario_Sunat(Window parent,clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            VentanaPrincipal = parent;
            localDB = local;
        }
        //Evento de carga de la ventana principal.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar estados sunat al combobox Estado Sunat.
            estados_sunat.Add(new ComboBoxPares("", "Seleccione"));
            estados_sunat.Add(new ComboBoxPares("0", "Aceptado"));
            estados_sunat.Add(new ComboBoxPares("1", "Rechazado"));
            estados_sunat.Add(new ComboBoxPares("2", "Sin estado"));
            estados_sunat.Add(new ComboBoxPares("4", "En proceso"));
            estados_sunat.Add(new ComboBoxPares("5", "Ticket a Consultar"));
            cboEstadoSunat.DisplayMemberPath = "_Value";
            cboEstadoSunat.SelectedValuePath = "_key";
            cboEstadoSunat.SelectedIndex = 0;
            cboEstadoSunat.ItemsSource = estados_sunat;
            //Inicializar las fechas.
            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();

            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            //Cargar datos en la grilla
            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
        }
        //Metodo para cargar la grilla de comprobantes resumenes diarios
        private void cs_pxCargarDgvComprobanteselectronicos(string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            //Limpiar la grilla.
            dgComprobantesResumen.ItemsSource = null;
            dgComprobantesResumen.Items.Clear();
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            int items = 0;
            //Obtener los registro segun filtro.
            registros = new clsEntitySummaryDocuments(localDB).cs_pxObtenerFiltroSecundario(estadocomprobantesunat, fechainicio, fechafin);
            lista_reporte = new List<ReporteResumen>();
            if (registros != null)
            {
                //Recorrer los registros
                foreach (var item in registros)
                {
                    items = (int)new clsEntitySummaryDocuments(localDB).cs_fxObtenerResumenNumeroItems(item.Cs_pr_SummaryDocuments_Id);
                    //if (items > 0)
                    //{
                        itemRow = new ReporteResumen();
                        itemRow.Id = item.Cs_pr_SummaryDocuments_Id;
                        itemRow.FechaEmision = item.Cs_tag_ReferenceDate;
                        itemRow.FechaEnvio = item.Cs_tag_IssueDate;
                        itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                        itemRow.Ticket = item.Cs_pr_Ticket;
                        itemRow.Archivo = item.Cs_tag_ID;
                        itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                        itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                        itemRow.EstadoSunatCodigo = item.Cs_pr_EstadoSUNAT;
                        lista_reporte.Add(itemRow);
                    //}
                    //else
                    //{
                    //    new clsEntitySummaryDocuments(localDB).cs_pxEliminarDocumento(item.Cs_pr_SummaryDocuments_Id);
                    //}

                }
            }

            dgComprobantesResumen.ItemsSource = lista_reporte;
        }

        //Evento de cambio de seleccion en la grilla de resumenes diarios-Seleccion de documentos del Resumen Diario
        private void dgComprobantesResumen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Obtener el elemento seleccionado en la grilla.
            System.Windows.Controls.DataGrid checkBox = (System.Windows.Controls.DataGrid)e.OriginalSource;
            //Obtner el objeto asociado a la fila seleccionada.
            ReporteResumen item = (ReporteResumen)checkBox.SelectedItem;
            if (item != null)
            {//Si objeto no es nulo Cargar los comprobantes asociados al resumen diario.
                Detalle.Content = "Detalle Resumen | " + item.Archivo;
                cs_pxCargarDgvComprobanteselectronicosPorResumen(item.Id);
            }
            else
            {
                Detalle.Content = "Detalle Resumen Diario";
            }

            //MessageBox.Show(item.Archivo+"-"+item.Id);
        }
        //Evento check para los comprobantes en la grilla de resumenes diarios
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado en la grilla.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener el objeto asociado a la fila seleccionada.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteResumen resumen = (ReporteResumen)dataGridRow.DataContext;
            //Cambiar estado de objeto.
            if ((bool)checkBox.IsChecked)
            {
                resumen.Check = true;
            }
            e.Handled = true;
        }
        //Evento uncheck pra los comprobantes en la grilla de resumenes diarios 
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado en la grilla.
           System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener el objeto asociado a la fila seleccionada.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteResumen resumen = (ReporteResumen)dataGridRow.DataContext;
            //Cambiar estado de objeto.
            if ((bool)checkBox.IsChecked == false)
            {
                resumen.Check = false;
            }
            e.Handled = true;
        }
        //Metodo para cargar los comprobantes dentro del resumen seleccionado
        private void cs_pxCargarDgvComprobanteselectronicosPorResumen(string idResumen)
        {
            //Limpiar grilla de documentos asociados
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            //Obtener los docuemntos asociados al resume selccionado
            documentos = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario_n(idResumen);
            lista_documentos = new List<ReporteDocumento>();
            //Recorrer los documentos para rellenar la grilla
            if (documentos != null)
            {
                foreach (var item in documentos)
                {
                    itemComprobante = new ReporteDocumento();
                    itemComprobante.Id = item.Cs_pr_Document_Id;
                    itemComprobante.Tipo = item.Cs_tag_InvoiceTypeCode;
                    itemComprobante.SerieNumero = item.Cs_tag_ID;
                    itemComprobante.FechaEmision = item.Cs_tag_IssueDate;
                    itemComprobante.FechaEnvio = item.comprobante_fechaenviodocumento;
                    itemComprobante.Ruc = item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;
                    itemComprobante.RazonSocial = item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
                    itemComprobante.Comentario = item.Cs_pr_ComentarioSUNAT;
                    itemComprobante.ComunicacionBaja = item.Cs_pr_ComunicacionBaja;
                    itemComprobante.ResumenDiario = item.Cs_pr_Resumendiario;
                    itemComprobante.DocumentoReferencia = item.Cs_tag_BillingReference_DocumentTypeCode;
                    itemComprobante.TipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_InvoiceTypeCode);
                    itemComprobante.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemComprobante.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    itemComprobante.EstadoSunatCodigo = item.Cs_pr_EstadoSUNAT;
                    lista_documentos.Add(itemComprobante);
                }
            }
            dgComprobantes.ItemsSource = lista_documentos;
        }
        //Evento para filtrar los comprobantes.
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;

            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
        }
        //Evento para refrescar la grilla
        private void btnRefrescar_Click(object sender, RoutedEventArgs e)
        {
            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
        }
        //Evento de envio de consulta de ticket para los comprobantes seleccionados
        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            string no_procesados = "";
            string procesados = "";
            int cantidad_seleccionados = 0;
            try
            {
                //Recorrer los elementos de la grilla para obtener los seleccionados.
                List<ReporteResumen> seleccionados = new List<ReporteResumen>();
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        cantidad_seleccionados++;
                        if ((item.EstadoSunatCodigo == "5" || item.EstadoSunatCodigo=="4" || item.EstadoSunatCodigo == "2") && item.Ticket != "")
                        {
                            seleccionados.Add(item);
                        }
                        else
                        {
                            no_procesados += item.Archivo + "\n";
                        }
                    }
                }
                //Si existen items seleccionados       
                if (cantidad_seleccionados > 0)
                {
                    //Si existen elementos no procesados.
                    if (no_procesados.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Los siguientes resumenes no seran procesados. Verifique ticket de consulta. \n" + no_procesados);
                    }
                    //Si existen items a procesar                
                    if (seleccionados.Count() > 0)
                    {
                        ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                            procesados = consultaTicket(seleccionados);
                        });

                        if (procesados.Trim().Length > 0)
                        {
                            CustomDialogWindow obj = new CustomDialogWindow();
                            obj.AdditionalDetailsText = "Documentos procesados:\n" + procesados;
                            obj.Buttons = CustomDialogButtons.OK;
                            obj.Caption = "Mensaje";
                            obj.DefaultButton = CustomDialogResults.OK;
                            // obj.FooterIcon = CustomDialogIcons.Shield;
                            // obj.FooterText = "This is a secure program";
                            obj.InstructionHeading = "Documentos consultados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Los documentos fueron procesados.Para mayor informacion vea los detalles.";
                            CustomDialogResults objResults = obj.Show();
                        }                      
                        refrescarGrilla();
                    }
                   
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Debe seleccionar un item");
                }

            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Consulta de Ticket" + ex.Message);
            }
        }
        private string consultaTicket(List<ReporteResumen> seleccionados)
        {
            string retornar = string.Empty;
            //Por cada seleccionado enviar la consulta de ticket.
            foreach (var item in seleccionados)
            {
                bool retorno = new clsBaseSunat(localDB).cs_pxConsultarTicketRC(item.Ticket,true);
                if (retorno == true)
                {
                    retornar += item.Archivo + "\n";
                }
            }
            return retornar;
        }
        //Evento para enviar los resumenes diarios a la sunat.
        private void btnSunat_Click(object sender, RoutedEventArgs e)
        {
            string no_procesados = "";
            int cantidad_seleccionados = 0;
            try
            {
                //Recorrer los elementos de la grilla para obtener los seleccionados.
                List<ReporteResumen> seleccionados = new List<ReporteResumen>();
                foreach (var item in lista_reporte)
                {
                    //Si estan seleccionados
                    if (item.Check == true)
                    {
                        cantidad_seleccionados++;
                        if (item.EstadoSunatCodigo == "2")
                        {
                            seleccionados.Add(item);
                        }
                        else
                        {
                            no_procesados += item.Archivo + "\n";
                        }

                    }
                }
                //Si existen items seleccionados.
                if (cantidad_seleccionados > 0)
                {
                    string procesados = String.Empty;
                    //Si existen items a procesar.
                    if (seleccionados.Count > 0)
                    {                      
                        ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                            procesados = sendToSunat(seleccionados);
                        });                       
                    }

                    string comentario = procesados + no_procesados;

                    if (comentario.Trim().Length > 0)
                    {
                        comentario = "";
                        if (procesados.Trim().Length > 0)
                        {
                            comentario += "Documentos procesados correctamente:\n"+procesados;
                        }
                        //En caso existan elementos nno procesados
                        if (no_procesados.Trim().Length > 0)
                        {
                            comentario += "Resumenes no procesados. Estan en proceso o para consulta de ticket:\n"+no_procesados;
                            //System.Windows.Forms.MessageBox.Show("Los siguientes  . \n" + no_procesados);
                        }
                        CustomDialogWindow obj = new CustomDialogWindow();
                        obj.AdditionalDetailsText = comentario;
                        obj.Buttons = CustomDialogButtons.OK;
                        obj.Caption = "Mensaje";
                        obj.DefaultButton = CustomDialogResults.OK;
                        // obj.FooterIcon = CustomDialogIcons.Shield;
                        // obj.FooterText = "This is a secure program";
                        obj.InstructionHeading = "Documentos enviados";
                        obj.InstructionIcon = CustomDialogIcons.Information;
                        if (procesados.Length > 0)
                        {
                            if (no_procesados.Length > 0)
                            {
                                obj.InstructionText = "Existen documentos no procesados. Para mayor informacion vea los detalles.";
                            }
                            else
                            {
                                obj.InstructionText = "Los documentos se enviaron correctamente a SUNAT.";
                            }
                            
                        }
                        else
                        {
                            obj.InstructionText = "Los documentos no fueron procesados.Para mayor informacion vea los detalles.";
                        }
                        
                        CustomDialogResults objResults = obj.Show();
                       
                    }
                    refrescarGrilla();

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Debe seleccionar un item");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Envio a sunat resumen diario" + ex.Message);
            }
        }
        private string sendToSunat(List<ReporteResumen> seleccionados)
        {
            string retorno = string.Empty;
            //Recorrer los elemtos a procesar y enviar a sunat
            foreach (var item in seleccionados)
            {
               bool resultado = new clsBaseSunat(localDB).cs_pxEnviarRC(item.Id,true);
               if (resultado == true) {
                    retorno += item.Archivo+"\n";
               }
            }
            return retorno;
        }
        private void btnLiberar_Click(object sender, RoutedEventArgs e)
        {
            string no_procesados = String.Empty;
            string resumenes_exito = String.Empty;
            string resumenes_rechazo = string.Empty;

            try
            {
                List<string> seleccionados = new List<string>();
                foreach (var item in lista_reporte)
                {
                    //Si estan seleccionados
                    if (item.Check == true)
                    {
                        seleccionados.Add(item.Id);
                    }
                }

                if (seleccionados.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea complementar los resumenes seleccionados?\n Los documentos asociados seran liberados y podra volverlos a agregar a resumen diario.", "¿Está seguro?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (var item in lista_reporte)
                        {
                            
                            //Si estan seleccionados
                            if (item.Check == true)
                            {
                                //Cristhian|07/02/2018|FEICONT2-588
                                /*Se agrega una validación para no liberar los resumenes diarios que han sido aceptados por la SUNAT*/
                                /*INICIO MODIFICACIóN*/
                                /*Se verifica que el codigo de sunat no sea 0 o 5 (Que es para consultar ticket)*/
                                if (item.EstadoSunatCodigo != "0" && item.EstadoSunatCodigo != "5")
                                {
                                    bool exito = new clsEntitySummaryDocuments(localDB).cs_pxLiberarSustitutorioDocumento(item.Id);
                                    if (exito)
                                    {
                                        resumenes_exito += item.Archivo + "\n";
                                    }
                                }
                                /*Caso contrario ya tiene el estado de rechazado o pendiente, y ya no se puede librerar*/
                                else
                                {
                                    /*Se anota el código del resumen diario que no fue liberado*/
                                    resumenes_rechazo += item.Archivo + "\n";
                                }

                                /*para eliminar los items del sumary document*/
                                if (item.EstadoSunatCodigo == "2")
                                {
                                    new clsEntitySummaryDocuments(localDB).cs_pxEliminarDocumento(item.Id, true);
                                }
                                /*FIN MODIFICACIóN*/
                            }

                        }

                        if (resumenes_exito != "")
                        {
                            System.Windows.Forms.MessageBox.Show("Los siguientes resumenes fueron liberados con exito:\n" + resumenes_exito);
                        }

                        //Cristhian|07/02/2018|FEICONT2-588
                        /*Se envia mensaje de rechazo*/
                        /*NUEVO INICIO*/
                        /*Si se tiene algun resumen diario que no fue liberado, entonces se muestra el mensaje*/
                        if (resumenes_rechazo != "")
                        {
                            System.Windows.Forms.MessageBox.Show("Los siguientes resumenes no fueron liberados:\n" + resumenes_rechazo + "Por que estan en proceso o fueron aceptados por SUNAT \n");
                        }
                        /*NUEVO FIN*/

                        refrescarGrilla();
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione al menos un item");
                }
            }catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("liberarRD "+ex.ToString());
            }
           
        }

        private void btnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> seleccionados = new List<string>();
                foreach (var item in lista_reporte)
                {
                    //Si estan seleccionados
                    if (item.Check == true)
                    {
                        seleccionados.Add(item.Id);
                    }
                }

                if (seleccionados.Count > 0)
                {
                    int noEliminados = 0;
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea descartar los resumenes seleccionados?\nEstos documentos serán eliminados completamente de la base de datos.", "¿Está seguro?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (var item in lista_reporte)
                        {
                            if (item.Check == true)
                            {
                                if (item.EstadoSunatCodigo == "2")
                                {
                                    new clsEntitySummaryDocuments(localDB).cs_pxEliminarDocumento(item.Id);
                                }
                                else
                                {
                                    noEliminados++;
                                }
                            }
                        }

                        refrescarGrilla();
                        if (noEliminados > 0)
                        {
                            System.Windows.Forms.MessageBox.Show("No se descarto "+noEliminados+" documento(s) , ya tienen respuesta de Sunat.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione al menos un item.");
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("descartarRD" + ex.ToString());
            }
                  
        }
        private void refrescarGrilla()
        {
            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;
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

            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
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

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("13");
                ayuda.ShowDialog();
            }
        }
    }
}
