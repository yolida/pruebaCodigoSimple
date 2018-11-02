using FEICONT.ayuda;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_Retencion.xaml
    /// </summary>
    public partial class Reporte_Retencion : Page
    {
        List<ComboBoxPares> estados_scc = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();
        List<clsEntityRetention> registros;
        List<ReporteRetention> lista_reporte;
        ReporteRetention itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        private clsEntityDatabaseLocal localDB;
        public Reporte_Retencion(clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores a combobox de tipo reporte
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;
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
            cs_pxCargarDgvComprobanteselectronicos(cbpESCC._Id, cbpES._Id, fecha_inicio_formato, fecha_fin_formato);

        }

        //Cristhian|30/10/2017|FEICONT2-410
        /*Se obtine la lista de los comprobantes de retención*/
        /*NUEVO INICIO*/
        private void cs_pxCargarDgvComprobanteselectronicos(string estadocomprobantescc, string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantesFactura.ItemsSource = null;
            dgComprobantesFactura.Items.Clear();
            //Obtener los comprobantes de factura.
            registros = new clsEntityRetention(localDB).cs_pxObtenerFiltroPrincipal(estadocomprobantesunat, estadocomprobantescc, fechainicio, fechafin);
            lista_reporte = new List<ReporteRetention>();
            if (registros != null)
            {
                //Recorrer los registros para llenar la grilla.
                foreach (var item in registros)
                {
                    itemRow = new ReporteRetention();
                    itemRow.Id = item.Cs_pr_Retention_id;
                    itemRow.SerieNumero = item.Cs_tag_Id;
                    itemRow.FechaEmision = item.Cs_tag_IssueDate;
                    itemRow.FechaEnvio = item.Cs_pr_FechaEnvio;
                    itemRow.Ruc = item.Cs_tag_ReceiveParty_PartyIdentification_Id;
                    itemRow.RazonSocial = item.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName;
                    itemRow.Comentario = item.Cs_pr_ComentarioSUNAT;
                    itemRow.Reversion = item.Cs_pr_Reversion;
                    itemRow.ReversionAnterior = item.Cs_pr_Reversion_Anterior;
                    itemRow.MontoRetencion = item.Cs_tag_TotalInvoiceAmount;
                    itemRow.MontoTotal = item.Cs_tag_TotalPaid;
                    itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    lista_reporte.Add(itemRow);
                }
            }
            dgComprobantesFactura.ItemsSource = lista_reporte;
        }
        /*NUEVO FIN*/
        private void btnFiltro_Click(object sender, RoutedEventArgs e)
        {
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
            cs_pxCargarDgvComprobanteselectronicos(cbpESCC._Id, cbpES._Id, fecha_inicio_formato, fecha_fin_formato);
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
                        clsBaseReporte.cs_pxReportePDF_Retencion(lista_reporte, configuracion.cs_prRutareportesPDF + "\\RETENCION-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_Retencion(lista_reporte, configuracion.cs_prRutareportesCSV + "\\RETENCION-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
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
                ReporteRetention item = (ReporteRetention)dgComprobantesFactura.SelectedItem;
                if (item != null)
                {
                    frmDetallePR Formulario = new frmDetallePR(item.Id, localDB);               
                    Formulario.ShowDialog();
                    if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
                    {
                    // cargarDataGrid();
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
                ReporteRetention item = (ReporteRetention)dgComprobantesFactura.SelectedItem;
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
                            string xml = string.Empty;
                            xml = new clsNegocioCERetention(localDB).cs_pxGenerarXMLAString(item.Id);                            
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
                ReporteRetention item = (ReporteRetention)dgComprobantesFactura.SelectedItem;
                if (item != null)
                {
                    clsEntityRetention cabecera = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(item.Id);
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
                AyudaPrincipal ayuda = new AyudaPrincipal("8");
                ayuda.ShowDialog();
            }
        }
    }
}
