using FEICONT.Base;
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
using FEICONT.CustomDialog;
using FEICONT.ayuda;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Generar resumen diario.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para ResumenDiario_Generar.xaml
    /// </summary>
    public partial class ResumenDiario_Generar : Page
    {
        List<clsEntityDocument> registros;
        List<ReporteDocumento> lista_reporte;
        ReporteDocumento itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        private Window padre;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor
        public ResumenDiario_Generar(Window parent,clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            padre = parent;
            localDB = local;
        }
        //Evento de carga de la ventan principal
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();

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
            cs_pxCargarDgvComprobanteselectronicos(fecha_inicio_formato, fecha_fin_formato);
        }
        //Metodo para cargar los comprobantes en la grilla
        private void cs_pxCargarDgvComprobanteselectronicos(string fechainicio, string fechafin)
        {
            //Limpiar la grilla de comprobantes.
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroAgregarResumen("", "", "", fechainicio, fechafin, "'03','07','08'", true);
            lista_reporte = new List<ReporteDocumento>();
            //lista_reporte = new ObservableCollection<Reporte>();
            //Recorrer los registros para rellenar la grilla.
            if (registros != null) {
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
                    itemRow.TipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(item.Cs_tag_InvoiceTypeCode);
                    itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    itemRow.ResumenDiarioTexto = "SIN ESTADO";
                    lista_reporte.Add(itemRow);
                }
            }
          
            dgComprobantes.ItemsSource = lista_reporte;
        }
        //Evento check para los items de la grilla.
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado en la grilla.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtner la fila seleccionada y el objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteDocumento doc = (ReporteDocumento)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked)
            {
                doc.Check = true;
            }
            //e.Handled = true;
        }
        //Evento uncheck para los items de la grilla.
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteDocumento produit = (ReporteDocumento)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                produit.Check = false;
            }
           // e.Handled = true;
        }
        private void actualizarGrid()
        {
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
            cs_pxCargarDgvComprobanteselectronicos(fecha_inicio_formato, fecha_fin_formato);
        }
        //Evento click para consultar el filtro de comprobantes
        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            actualizarGrid();
        }
        //Evento de envio de comprobantes a resumen diario
        private void btnEnviarAResumen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_procesado = String.Empty;
                //Recorrer la grilla para buscar los elementos seleccionados     
                List<string> seleccionados = new List<string>();
                int numero=0;
                //int ComprobanteSinEstado = 0;
                //int ComprobanteDeBaja = 0;
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        numero++;
                        if (item.ResumenDiario.Trim() == "")
                        {
                            seleccionados.Add(item.Id);
                        }
                        else
                        {
                            no_procesado += item.SerieNumero + "\n";
                        }

                        //if (item.EstadoSunat == "DE BAJA") { ComprobanteDeBaja++; } 
                        //else { ComprobanteSinEstado++; }                                          
                    }
                }

                //if (ComprobanteDeBaja==0 || ComprobanteSinEstado==0)
                //{
                    if (numero > 0)
                    {
                        if (no_procesado.Trim().Length > 0)
                        {
                            CustomDialogWindow obj = new CustomDialogWindow();
                            obj.AdditionalDetailsText = "El listado de comprobantes no agregados es el siguiente:\n" + no_procesado;
                            obj.Buttons = CustomDialogButtons.OK;
                            obj.Caption = "Mensaje";
                            obj.DefaultButton = CustomDialogResults.OK;
                            // obj.FooterIcon = CustomDialogIcons.Shield;
                            // obj.FooterText = "This is a secure program";
                            obj.InstructionHeading = "Documentos no procesados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Algunos documentos seleccionados no se procesaran debido a que ya fueron agregados a un resumen diario. Si es sustitutorio / rectificatorio debe liberarlos para poder agregarlos al resumen.";
                            CustomDialogResults objResults = obj.Show();
                        }
                        //Si existen seleccionados a procesar
                        if (seleccionados.Count() > 0)
                        {
                            //Confirmacion para agregar.
                            if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea enviar a resumen diario los documentos seleccionados?", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                string resultado = string.Empty;
                                ProgressDialogResult result = ProgressWindow.Execute(padre, "Procesando...", () => {
                                    resultado = SendItemToRc(seleccionados);
                                });
                                if (resultado.Trim().Length > 0)
                                {
                                    CustomDialogWindow obj = new CustomDialogWindow();
                                    obj.AdditionalDetailsText = "Los comprobantes no agregados son los siguientes:\n" + resultado;
                                    obj.Buttons = CustomDialogButtons.OK;
                                    obj.Caption = "Mensaje";
                                    obj.DefaultButton = CustomDialogResults.OK;
                                    // obj.FooterIcon = CustomDialogIcons.Shield;
                                    // obj.FooterText = "This is a secure program";
                                    obj.InstructionHeading = "Documentos no agregados";
                                    obj.InstructionIcon = CustomDialogIcons.Information;
                                    obj.InstructionText = "Algunos documentos no se agregaron a resumen diario. Verifique la fecha de emision no mayor a 7 dias.";
                                    CustomDialogResults objResults = obj.Show();

                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("Todos los documentos se agregaron a su respectivo resumen diario.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                actualizarGrid();
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Debe seleccionar un item.", "Mensaje", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("Debe seleccionar documentos con el mismo estado de SUNAT", "Mensaje", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}

            }
            catch (Exception ex)
            { 
                clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("Enviar comprobantes a resumen diario" + ex.Message);
            }                          
        }
        private string SendItemToRc(List<string> seleccionados) {

            string resultadoRetorno = string.Empty;
            try
            {
                resultadoRetorno = new clsNegocioCEResumenDiario(localDB).cs_pxProcesarResumenDiario(seleccionados);

            }catch(Exception)
            {
                resultadoRetorno = string.Empty;
            }
            return resultadoRetorno;
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

        private void btnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Verificar si hay elementos seleccionados              
                List<ReporteDocumento> seleccionados = new List<ReporteDocumento>();
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        seleccionados.Add(item);
                    }
                }

                if (seleccionados.Count > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea descartar los documentos seleccionados?\nEstos documentos serán eliminados completamente de la base de datos.", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        string mensaje = string.Empty;
                        foreach (ReporteDocumento row in seleccionados)
                        {
                            bool resultado = new clsEntityDocument(localDB).cs_pxEliminarDocumento(row.Id);
                            if (resultado == true)
                            {
                                mensaje += row.SerieNumero + "\n";
                            }
                        }

                        if (mensaje.Length > 0)
                        {
                            CustomDialogWindow obj = new CustomDialogWindow();
                            obj.AdditionalDetailsText = "Los comprobantes son:\n" + mensaje;
                            obj.Buttons = CustomDialogButtons.OK;
                            obj.Caption = "Mensaje";
                            obj.DefaultButton = CustomDialogResults.OK;
                            obj.InstructionHeading = "Documentos eliminados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Los documentos se eliminaron correctamente de la base de datos.";
                            CustomDialogResults objResults = obj.Show();
                           // System.Windows.Forms.MessageBox.Show("Los siguientes documentos se eliminaron correctamente de la base de datos.\n" + mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        actualizarGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("delete boleta" + ex.ToString());
                //clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("12");
                ayuda.ShowDialog();
            }
        }
    }
}
