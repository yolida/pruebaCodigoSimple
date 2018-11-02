using FEICONT.ayuda;
using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Reportes de resumen de comprobantes.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_Resumen.xaml
    /// </summary>
    public partial class Reporte_Resumen : Page
    {      
        List<clsEntitySummaryDocuments> registros;
        List<ReporteResumen> lista_reporte;
        ReporteResumen itemRow;

        List<clsEntityDocument> documentos;
        List<ReporteDocumento> lista_documentos;
        ReporteDocumento itemComprobante;

        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();

        string fecha_inicio_formato;
        string fecha_fin_formato;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        ComboBoxPares cbpEstadoSunat;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor
        public Reporte_Resumen(clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
        }
        //Evento de carga para la pagina principal.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar items a estados sunat.
            estados_sunat.Add(new ComboBoxPares("", "Seleccione"));
            estados_sunat.Add(new ComboBoxPares("0", "Aceptado"));
            estados_sunat.Add(new ComboBoxPares("1", "Rechazado"));
            estados_sunat.Add(new ComboBoxPares("2", "Sin estado"));
            //estados_sunat.Add(new ComboBoxPares("3", "De Baja"));
            cboEstadoSunat.DisplayMemberPath = "_Value";
            cboEstadoSunat.SelectedValuePath = "_key";
            cboEstadoSunat.SelectedIndex = 0;
            cboEstadoSunat.ItemsSource = estados_sunat;
            //Agregar items a tipos de reporte
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;

            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();

            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;
            //si existe fecha seleccionada inicio.
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            //Si existe fecha seleccionada fin
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }

            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id,fecha_inicio_formato,fecha_fin_formato);
        }
        //Evento para realizar el filtro de comprobantes
        private void btnFiltro_Click(object sender, RoutedEventArgs e)
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
        //Evento de cambio de item seleccionado en el datagrid
        private void dgComprobantesResumen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Obtener elemento seleccionado
            System.Windows.Controls.DataGrid checkBox = (System.Windows.Controls.DataGrid)e.OriginalSource;
            //Obtener objeto asociado a la fila seleccionada
            ReporteResumen item = (ReporteResumen)checkBox.SelectedItem;
            if (item != null)
            {
                //Cargar comprobantes asociados al resumen seleccionado.
                Detalle.Content = "Detalle Resumen | " + item.Archivo;
                cs_pxCargarDgvComprobanteselectronicosPorResumen(item.Id);
            }else
            {
                Detalle.Content = "Detalle Resumen Diario";
            }
           
            //MessageBox.Show(item.Archivo+"-"+item.Id);
        }
        //Metodo para la carga de comprobantes en la grilla.
        private void cs_pxCargarDgvComprobanteselectronicos(string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantesResumen.ItemsSource = null;
            dgComprobantesResumen.Items.Clear();
            //Obtener los resumen diarios
            registros = new clsEntitySummaryDocuments(localDB).cs_pxObtenerFiltroPrincipal(estadocomprobantesunat, fechainicio, fechafin);
            lista_reporte = new List<ReporteResumen>();
            if (registros != null)
            {
                //Recorrer los registros para rellenar la grilla de resumenes
                foreach (var item in registros)
                {

                    itemRow = new ReporteResumen();
                    itemRow.Id = item.Cs_pr_SummaryDocuments_Id;
                    itemRow.FechaEmision = item.Cs_tag_ReferenceDate;
                    itemRow.FechaEnvio = item.Cs_tag_IssueDate;
                    itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                    itemRow.Ticket = item.Cs_pr_Ticket;
                    itemRow.Archivo = item.Cs_tag_ID;
                    itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    lista_reporte.Add(itemRow);
                }

            }
          
            dgComprobantesResumen.ItemsSource = lista_reporte;
        }
        //Metodo para cargar comprobantes segun resumen.
        private void cs_pxCargarDgvComprobanteselectronicosPorResumen(string idResumen)
        {
            //Limpiar la grilla 
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            //Obtener los documento asociados a resumen diario.
            documentos = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario_n(idResumen);
            lista_documentos = new List<ReporteDocumento>();
            //lista_reporte = new ObservableCollection<Reporte>();
            //Recorrer los elementos obtenidos para rellner la grilla de documentos asociados.
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
                    lista_documentos.Add(itemComprobante);
                }
            }
           
            dgComprobantes.ItemsSource = lista_documentos;
        }
        //Metodo para descargar reportes csv y pdf.
        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            //If existe  opcion a reporte
            if (lista_reporte.Count > 0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();

                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF_resumen(lista_reporte, configuracion.cs_prRutareportesPDF + "\\RC-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--"+ DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_resumen(lista_reporte, configuracion.cs_prRutareportesCSV + "\\RC-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--"+ DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                        break;

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsg("Error al generar reporte", "Deben existir datos listados para generar el reporte.");
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

        private void btnReporteDetallado_Click(object sender, RoutedEventArgs e)
        {
            //If existe  opcion a reporte
            if (lista_reporte.Count > 0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();

                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF_resumenDetallado(localDB,lista_reporte, configuracion.cs_prRutareportesPDF + "\\RC-Detallado-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_resumenDetallado(localDB,lista_reporte, configuracion.cs_prRutareportesCSV + "\\RC-Detallado-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                        break;

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsg("Error al generar reporte", "Deben existir datos listados para generar el reporte.");
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("5");
                ayuda.ShowDialog();
            }
        }
    }
}
