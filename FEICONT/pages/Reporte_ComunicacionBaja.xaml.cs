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
/// Cambio de interfaz - Reportes de comunicacion de baja.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_ComunicacionBaja.xaml
    /// </summary>
    public partial class Reporte_ComunicacionBaja : Page
    {
        List<clsEntityVoidedDocuments> registros;
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
        public Reporte_ComunicacionBaja(clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
        }   
        //Evento de carga de la ventana  
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores para estados de la sunat.
            estados_sunat.Add(new ComboBoxPares("", "Seleccione"));
            estados_sunat.Add(new ComboBoxPares("0", "Aceptado"));
            estados_sunat.Add(new ComboBoxPares("1", "Rechazado"));
            estados_sunat.Add(new ComboBoxPares("2", "Sin estado"));
            estados_sunat.Add(new ComboBoxPares("3", "De Baja"));
            cboEstadoSunat.DisplayMemberPath = "_Value";
            cboEstadoSunat.SelectedValuePath = "_key";
            cboEstadoSunat.SelectedIndex = 0;
            cboEstadoSunat.ItemsSource = estados_sunat;

            //Agregar valores para tipo de reporte.
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;

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

            cs_pxCargarDgvComprobanteselectronicos(cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
        }
        // Metodo de cargar de comprobantes electronicaos en la grilla principal.
        private void cs_pxCargarDgvComprobanteselectronicos(string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantesBaja.ItemsSource = null;
            dgComprobantesBaja.Items.Clear();
            //Obtener los registros de comunicacion de baja
            registros = new clsEntityVoidedDocuments(localDB).cs_pxObtenerFiltroPrincipal(estadocomprobantesunat, fechainicio, fechafin,"0");
            lista_reporte = new List<ReporteResumen>();
            //Recorrer los registros de comunicacion de baja para rellenar la grilla.
            if (registros != null)
            {
                foreach (var item in registros)
                {
                    itemRow = new ReporteResumen();
                    itemRow.Id = item.Cs_pr_VoidedDocuments_Id;
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
          
            dgComprobantesBaja.ItemsSource = lista_reporte;
        }
        // Metodo de consulta para filtrar documentos.
        private void btnConsultar_Click(object sender, RoutedEventArgs e)
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
        //Evento para descargar los reportes en csv y pdf.
        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            if (lista_reporte.Count > 0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();

                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF_baja(lista_reporte, configuracion.cs_prRutareportesPDF + "\\BAJA-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_baja(lista_reporte, configuracion.cs_prRutareportesCSV + "\\BAJA-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                        break;

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsg("Error al generar reporte", "Deben existir datos listados para generar el reporte.");
            }
        }
        //Evento de cambio de selccion de comprobantes.
        private void dgComprobantesBaja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Obtner el elemento selccionado
            System.Windows.Controls.DataGrid checkBox = (System.Windows.Controls.DataGrid)e.OriginalSource;
            //Obtener el objeto asociado a la seleccion.
            ReporteResumen item = (ReporteResumen)checkBox.SelectedItem;
            if (item != null)
            {
                Detalle.Content = "Detalle Comunicacion de Baja | " + item.Archivo;
                cs_pxCargarDgvComprobanteselectronicosPorBaja(item.Id);
            }
            else
            {
                Detalle.Content = "Detalle Comunicacion de Baja";
            }
        }
        //Cargar comprobantes para cada comunicacion de baja
        private void cs_pxCargarDgvComprobanteselectronicosPorBaja(string IdBaja)
        {
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            //Obtener los documentos asociados a la comunicacion de baja.
            documentos = new clsEntityDocument(localDB).cs_pxObtenerDocumentosPorComunicacionBaja_n(IdBaja);
            lista_documentos = new List<ReporteDocumento>();
            //Recorrer los documentos obtenidos para rellenar la grilla de documentos asociados
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
            if (lista_reporte.Count > 0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();

                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF_bajaDetallado(localDB,lista_reporte, configuracion.cs_prRutareportesPDF + "\\BAJA-DETALLADO-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_bajaDetallado(localDB,lista_reporte, configuracion.cs_prRutareportesCSV + "\\BAJA-DETALLADO-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
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
                AyudaPrincipal ayuda = new AyudaPrincipal("7");
                ayuda.ShowDialog();
            }
        }
    }
}
