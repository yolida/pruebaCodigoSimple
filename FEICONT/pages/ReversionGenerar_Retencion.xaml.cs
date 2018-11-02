using FEICONT.ayuda;
using FEICONT.Base;
using FEICONT.CustomDialog;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para ReversionGenerar_Retencion.xaml
    /// </summary>
    public partial class ReversionGenerar_Retencion : Page
    {
        List<ComboBoxPares> estados_scc = new List<ComboBoxPares>();
        List<ComboBoxPares> estados_sunat = new List<ComboBoxPares>();
        List<clsEntityRetention> registros;
        List<ReporteRetention> lista_reporte;
        ReporteRetention itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        private clsEntityDatabaseLocal localDB;
        private Window ventanaPrincipal;
        public ReversionGenerar_Retencion(clsEntityDatabaseLocal local, Window ventana)
        {
            InitializeComponent();
            localDB = local;
            ventanaPrincipal = ventana;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores a combobox de tipo reporte
           
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
        private void cs_pxCargarDgvComprobanteselectronicos(string estadocomprobantescc, string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            dgComprobantesFactura.ItemsSource = null;
            dgComprobantesFactura.Items.Clear();
            bool AddToList=false;
            //Obtener los comprobantes de factura.
            registros = new clsEntityRetention(localDB).cs_pxObtenerFiltroPrincipalReversion(estadocomprobantesunat, estadocomprobantescc, fechainicio, fechafin);
            lista_reporte = new List<ReporteRetention>();
            //lista_reporte = new ObservableCollection<Reporte>();
            if (registros != null)
            {
                //Recorrer los registros para llenar la grilla.
                foreach (var item in registros)
                {
                   AddToList = false;
                   
                   if (item.Cs_pr_EstadoSUNAT == "0")
                   {
                      AddToList = true;
                   }

                    if (AddToList == true)
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
            }
            dgComprobantesFactura.ItemsSource = lista_reporte;
        }
        private void actualizarGrilla()
        {
            //Obtener objetos asociados de los valores seleccionados en los combobox.
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
        //Evento click para Filtrar los comprobantes.
        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            actualizarGrilla();
        }
       
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtner elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccionado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteRetention doc = (ReporteRetention)dataGridRow.DataContext;
            if ((bool)checkBox.IsChecked)
            {
                doc.Check = true;
            }
            e.Handled = true;
        }
        //Evento Uncheck de los items en la grilla.
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccinado
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteRetention doc = (ReporteRetention)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                doc.Check = false;
            }
            e.Handled = true;
        }
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (ReporteRetention item in lista_reporte)
                    {
                        item.Check = true;
                    }
                    dgComprobantesFactura.ItemsSource = null;
                    dgComprobantesFactura.Items.Clear();
                    dgComprobantesFactura.ItemsSource = lista_reporte;
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
                    foreach (ReporteRetention item in lista_reporte)
                    {
                        item.Check = false;
                    }
                    dgComprobantesFactura.ItemsSource = null;
                    dgComprobantesFactura.Items.Clear();
                    dgComprobantesFactura.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_procesados = string.Empty;
                //Recorrer las grilla para obtener los elementos seleccionados.
                List<string> seleccionados = new List<string>();
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        if (item.Reversion == "")
                        {
                            seleccionados.Add(item.Id);
                        }
                        else
                        {
                            no_procesados += item.SerieNumero + "\n";
                        }

                    }
                }
                if (no_procesados.Trim().Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Los siguientes comprobantes no sera procesados. Ya fueron agregados a resumen de reversión\n" + no_procesados, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (seleccionados.Count > 0)
                {
                    //Confirmacion para enviar a comunicacion de baja los documentos seleccionados.
                    if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea enviar a resumen de reversión los documentos seleccionados?", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string resultado = string.Empty;
                        ProgressDialogResult result = ProgressWindow.Execute(ventanaPrincipal, "Procesando...", () => {
                            resultado = ProcesarReversion(seleccionados);
                        });
                        if (resultado.Trim().Length > 0)
                        { //Se agregaron 
                            CustomDialogWindow obj = new CustomDialogWindow();
                            obj.AdditionalDetailsText = "Los comprobantes no agregados son los siguientes:\n" + resultado;
                            obj.Buttons = CustomDialogButtons.OK;
                            obj.Caption = "Mensaje";
                            obj.DefaultButton = CustomDialogResults.OK;
                            // obj.FooterIcon = CustomDialogIcons.Shield;
                            // obj.FooterText = "This is a secure program";
                            obj.InstructionHeading = "Documentos no agregados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Algunos documentos no se agregaron a su respectivo resumen de reversión. Verifique el archivo de errores.";
                            CustomDialogResults objResults = obj.Show();

                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Los documentos se agregaron a su respectivo resumen de reversión.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           // System.Windows.Forms.MessageBox.Show("Ocurrio un error al procesar los documentos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        actualizarGrilla();
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("btnAgregar " + ex.ToString());
            }
        }
        private string ProcesarReversion(List<string> seleccionados)
        {
            string resultado = string.Empty;
            try
            {
                resultado = new clsNegocioCEComunicacionBaja(localDB).cs_pxProcesarComunicacionBaja(seleccionados, "1");
            }
            catch
            {
                resultado = string.Empty;
            }
            return resultado;
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("19");
                ayuda.ShowDialog();
            }
        }
    }
}
