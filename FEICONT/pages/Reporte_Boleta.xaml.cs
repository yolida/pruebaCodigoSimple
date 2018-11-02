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
/// Cambio de interfaz - Reportes de boletas de venta.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_Boletas.xaml
    /// </summary>
    public partial class Reporte_Boleta : Page
    {
        clsEntitySummaryDocuments resumen;
        List<ComboBoxPares> tipos_comprobante = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_scc = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();
        List<clsEntityDocument> registros;
        List<ReporteDocumento> lista_reporte;
        ReporteDocumento itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        ComboBoxPares cbpTipoComprobante;
        ComboBoxPares cbpEstadoSCC;
        ComboBoxPares cbpEstadoSunat;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor
        public Reporte_Boleta(clsEntityDatabaseLocal local)
        {
            localDB = local;
            InitializeComponent();
        }     
        //Evento de carga de la ventana.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar elementos al ComboBox de tipos de reporte
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;
            //Agregar elemntos al combobox de tipo de comprobantes
            tipos_comprobante.Add(new ComboBoxPares("", "Seleccione"));
            tipos_comprobante.Add(new ComboBoxPares("03", "Boleta de Venta"));
            tipos_comprobante.Add(new ComboBoxPares("07", "Nota de Credito"));
            tipos_comprobante.Add(new ComboBoxPares("08", "Nota de Debito"));
            cboTipoComprobante.DisplayMemberPath = "_Value";
            cboTipoComprobante.SelectedValuePath = "_Key";
            cboTipoComprobante.SelectedIndex = 0;
            cboTipoComprobante.ItemsSource = tipos_comprobante;
            //Agregar elementos al combobox de estasdo en SCC
            estados_scc.Add(new ComboBoxPares("", "Seleccione"));
            estados_scc.Add(new ComboBoxPares("0", "Enviado"));
           // estados_scc.Add(new ComboBoxPares("1", "Pendiente (Errores)"));
            estados_scc.Add(new ComboBoxPares("2", "Pendiente (Correcto)"));
            estados_scc.Add(new ComboBoxPares("3", "Pendiente (De Baja)"));
            cboEstadoSCC.DisplayMemberPath = "_Value";
            cboEstadoSCC.SelectedValuePath = "_key";
            cboEstadoSCC.SelectedIndex = 0;
            cboEstadoSCC.ItemsSource = estados_scc;
            //Agregar elementos al combobox de estados para sunat
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

            cbpTipoComprobante = (ComboBoxPares)cboTipoComprobante.SelectedItem;
            cbpEstadoSCC = (ComboBoxPares)cboEstadoSCC.SelectedItem;
            cbpEstadoSunat = (ComboBoxPares)cboEstadoSunat.SelectedItem;
            //Si la seleccion de la fecha de inicio es diferente de null.
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            //Si la seleccion de la fecha de fin es diferente de null.
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }
            //Cargar los comprobantes electronicos.
            cs_pxCargarDgvComprobanteselectronicos(cbpTipoComprobante._Id, cbpEstadoSCC._Id, cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);

        }
        //Metodo de cargar de comprobantes electronicaos en la grilla principal.
        private void cs_pxCargarDgvComprobanteselectronicos(string tipo, string estadocomprobantescc, string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            //Limpiar la grilla.
            dgComprobantes.ItemsSource = null;
            dgComprobantes.Items.Clear();
            //Obtener los registros de boletas.
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroPrincipal(tipo, estadocomprobantesunat, estadocomprobantescc, fechainicio, fechafin, "'03','07','08'", true);
            lista_reporte = new List<ReporteDocumento>();
            //lista_reporte = new ObservableCollection<Reporte>();
            //Recorrer los registros para rellenar la grilla.
            if (registros != null)
            {
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
                    if (item.Cs_pr_Resumendiario != "")
                    {
                        resumen = new clsEntitySummaryDocuments(localDB).cs_fxObtenerUnoPorId(item.Cs_pr_Resumendiario);
                        if (resumen != null)
                        {
                            /*Cambiado para que se muestre el estado del comprobante y ya no del resumen diario*/
                            //itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(resumen.Cs_pr_EstadoSUNAT)).ToUpper();
                            itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                            itemRow.ResumenDiarioTexto = resumen.Cs_tag_ID;
                            itemRow.ResumenDiarioFechaEnvio = resumen.Cs_tag_IssueDate;
                            itemRow.ResumenDiarioTicket = resumen.Cs_pr_Ticket;
                        }
                        else
                        {
                            itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                        }
                    }
                    else
                    {
                        itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    }
                    itemRow.fechadebaja = item.Cs_tag_FechaDeBaja;
                    lista_reporte.Add(itemRow);
                }
            }
            
            dgComprobantes.ItemsSource = lista_reporte;
        }
        //Metodo de consulta para filtrar documentos.
        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            cbpTipoComprobante = (ComboBoxPares)cboTipoComprobante.SelectedItem;
            cbpEstadoSCC = (ComboBoxPares)cboEstadoSCC.SelectedItem;
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
            //Cargar los comprobantes electronicos.
            cs_pxCargarDgvComprobanteselectronicos(cbpTipoComprobante._Id, cbpEstadoSCC._Id, cbpEstadoSunat._Id, fecha_inicio_formato, fecha_fin_formato);
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
                        clsBaseReporte.cs_pxReportePDF(lista_reporte, configuracion.cs_prRutareportesPDF + "\\BOLETAS-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV(lista_reporte, configuracion.cs_prRutareportesCSV + "\\BOLETAS-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
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
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("6");
                ayuda.ShowDialog();
            }
        }
    }
}
