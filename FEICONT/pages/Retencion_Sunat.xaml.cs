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
    /// Lógica de interacción para Retencion_Sunat.xaml
    /// </summary>
    public partial class Retencion_Sunat : Page
    {
        List<ComboBoxPares> tipos_comprobante = new List<ComboBoxPares>();
        List<clsEntityRetention> registros;
        List<ReporteRetention> lista_reporte;
        ReporteRetention itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        private clsEntityDatabaseLocal localDB;
        private Window VentanaPrincipal;
        public Retencion_Sunat(clsEntityDatabaseLocal local,Window parent)
        {
            InitializeComponent();
            localDB = local;
            VentanaPrincipal = parent;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();
            refrescarGrilla();
        }
        //Metodo para cargar la grilla del listado de comprobantes.
        private void cs_pxCargarDgvComprobanteselectronicos( string fechainicio, string fechafin)
        {
            dgComprobantesFactura.ItemsSource = null;
            dgComprobantesFactura.Items.Clear();
            //Obtener los registros para facturas.
            registros = new clsEntityRetention(localDB).cs_pxObtenerFiltroPrincipal("2", "0", fechainicio, fechafin);
            lista_reporte = new List<ReporteRetention>();
            //Recorrer los registros para rellenar el grid.
            if (registros != null)
            {
                foreach (var item in registros)
                {
                    clsEntityRetention_RetentionLine RetentionLine = new clsEntityRetention_RetentionLine(localDB).cs_fxObtenerUnoPorCabeceraId(item.Cs_pr_Retention_id);
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
                    itemRow.SerieNumeroRelacionado = RetentionLine.Cs_tag_Id;
                    itemRow.TipoRelacionado = RetentionLine.Cs_tag_Id_SchemeId;
                    itemRow.MontoPago =RetentionLine.Cs_tag_Payment_PaidAmount;
                    itemRow.MontoRetencion =RetentionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount;
                    itemRow.MontoTotal=RetentionLine.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid;
                    itemRow.TipoTextoRelacionado = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(RetentionLine.Cs_tag_Id_SchemeId); 
                    itemRow.EstadoSCC = clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSCC)).ToUpper();
                    itemRow.EstadoSunat = clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(item.Cs_pr_EstadoSUNAT)).ToUpper();
                    lista_reporte.Add(itemRow);
                }
            }

            dgComprobantesFactura.ItemsSource = lista_reporte;
        }
        //Evento check para cada item del listado.
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteRetention comprobante = (ReporteRetention)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked)
            {
                comprobante.Check = true;
            }
            e.Handled = true;
        }
        //Evento uncheck para cada item del listado.
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteRetention comprobante = (ReporteRetention)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                comprobante.Check = false;
            }
            e.Handled = true;
        }
        //Evento click para consulta sobre filtro.
        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            refrescarGrilla();
        }
        private void refrescarGrilla()
        {     
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
            //Cargar comrpbantes segun filtro.
            cs_pxCargarDgvComprobanteselectronicos(fecha_inicio_formato, fecha_fin_formato);
        }
        //Evento envio a sunat de comprobantes.
        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                //Recorrer la grilla para obtener los documentos seleccionados de la grilla.
                List<ReporteRetention> seleccionados = new List<ReporteRetention>();             
                foreach (var item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        seleccionados.Add(item);
                    }
                }
                //If existen los elementos seleccionados.
                if (seleccionados.Count() > 0)
                {
                    string enviados = string.Empty;
                    ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                        enviados = sendToSunat(seleccionados);
                    });
                    if (enviados.Trim().Length > 0)
                    {
                        CustomDialogWindow obj = new CustomDialogWindow();
                        obj.AdditionalDetailsText = "Los comprobantes enviados correctamente son:\n" + enviados;
                        obj.Buttons = CustomDialogButtons.OK;
                        obj.Caption = "Mensaje";
                        obj.DefaultButton = CustomDialogResults.OK;
                        // obj.FooterIcon = CustomDialogIcons.Shield;
                        // obj.FooterText = "This is a secure program";
                        obj.InstructionHeading = "Documentos enviados";
                        obj.InstructionIcon = CustomDialogIcons.Information;
                        obj.InstructionText = "Los comprobantes han sido enviados correctamente.";
                        CustomDialogResults objResults = obj.Show();
                    }
                    refrescarGrilla();

                    //string enviados = String.Empty;
                    //Por cada elemento seleccionado se envia a la sunat.
                   
                    /*if (enviados.Length > 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Los siguientes comprobantes han sido enviados correctamente.\n"+enviados,"Mensaje",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }*/
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
                clsBaseLog.cs_pxRegistarAdd("Enviar retencion sunat" + ex.Message);
            }
        }

        private string sendToSunat(List<ReporteRetention> seleccionados)
        {
            string enviados = string.Empty;
            foreach (var item in seleccionados)
            {
                bool enviado = new clsBaseSunat(localDB).cs_pxEnviarCERetention(item.Id, true);
                if (enviado)
                {
                    enviados += item.SerieNumero + "\n";
                }
            }
            return enviados;
        }

        private void DetalleItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReporteRetention item = (ReporteRetention)dgComprobantesFactura.SelectedItem;
                if (item != null)
                {
                    //frmDetalleComprobante Formulario = new frmDetalleComprobante(item.Id);
                   // Formulario.ShowDialog();
                   // if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
                   // {
                        //cargarDataGrid();
                   // }
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

        private void btnEnviars_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i <= 5; i++)
            {
                clsEntityRetention per = new clsEntityRetention(localDB);
                per.Cs_pr_Retention_id = Guid.NewGuid().ToString();
                per.Cs_tag_Id = "R001-123"+i;
                per.Cs_tag_IssueDate = "2017-03-17";
                per.Cs_tag_PartyIdentificacion_SchemeId = "6";
                per.Cs_tag_PartyIdentification_Id = "20100113612";
                per.Cs_tag_PartyName = "Kg laboratorios";
                per.Cs_tag_PostalAddress_Id = "150114";
                per.Cs_tag_PostalAddress_StreetName = "olivos";
                per.Cs_tag_PostalAddress_CitySubdivisionName = "Urb santa felicia";
                per.Cs_tag_PostalAddress_CityName = "Lima";
                per.Cs_tag_PostalAddress_CountrySubEntity = "Lima";
                per.Cs_tag_PostalAddress_District = "La molina";
                per.Cs_tag_PostalAddress_Country_IdentificationCode = "PE";
                per.Cs_tag_PartyLegalEntity_RegistrationName = "KG Asociados Sac";
                per.Cs_tag_ReceiveParty_PartyIdentification_SchemeId = "6";
                per.Cs_tag_ReceiveParty_PartyIdentification_Id = "20546772439";
                per.Cs_tag_ReceiveParty_PartyName_Name = "Cia de consultoria";
                per.Cs_tag_ReceiveParty_PostalAddress_Id = "150122";
                per.Cs_tag_ReceiveParty_PostalAddress_StreetName = "Olaya";
                per.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName = "URB santa Rosa";
                per.Cs_tag_ReceiveParty_PostalAddress_CityName = "Lima";
                per.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity = "Lima";
                per.Cs_tag_ReceiveParty_PostalAddress_District = "Miaflores";
                per.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode = "PE";
                per.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName = "Cia de consultoria";
                per.Cs_tag_SUNATRetentionSystemCode = "01";
                per.Cs_tag_SUNATRetentionPercent = "3.00";
                per.Cs_tag_Note = "nota";
                per.Cs_tag_TotalInvoiceAmount_CurrencyId = "PEN";
                per.Cs_tag_TotalInvoiceAmount = "35.40";
                per.Cs_tag_TotalPaid_CurrencyId = "PEN";
                per.Cs_tag_TotalPaid = "1144.60";
                per.Cs_pr_EstadoSCC = "2";
                per.Cs_pr_EstadoSUNAT = "2";
                string idRetorno = per.cs_pxInsertar(true, false);

                clsEntityRetention_RetentionLine linea = new clsEntityRetention_RetentionLine(localDB);
                linea.Cs_pr_Retention_RetentionLine_Id = Guid.NewGuid().ToString();
                linea.Cs_pr_Retention_Id = idRetorno;
                linea.Cs_tag_Id_SchemeId = "01";
                linea.Cs_tag_Id = "F001-540"+i;
                linea.Cs_tag_IssueDate = "2016-09-15";
                linea.Cs_tag_TotalInvoiceAmount_CurrencyId = "USD";
                linea.Cs_tag_TotalInvoiceAmount = "1180.00";
                linea.Cs_tag_Payment_PaidDate = "2016-09-15";
                linea.Cs_tag_Payment_Id = "1";
                linea.Cs_tag_Payment_PaidAmount_CurrencyId = "USD";
                linea.Cs_tag_Payment_PaidAmount = "1180.00";
                linea.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId = "PEN";
                linea.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = "35.40";
                linea.Cs_tag_SUNATRetentionInformation_SUNATRetentionDate = "2016-09-15";
                linea.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId = "PEN";
                linea.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = "1140.60";
                linea.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode = "USD";
                linea.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode = "PEN";
                linea.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate = "3.00";
                linea.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date = "2016-09-15";
                linea.cs_pxInsertar(true, true);
            }
            

            /*clsEntityRetention ret = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(idRetorno);
            System.Windows.Forms.MessageBox.Show(ret.Cs_pr_Retention_id);*/
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("17");
                ayuda.ShowDialog();
            }
        }

        private void btnDescartar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Verificar si hay elementos seleccionados              
                List<ReporteRetention> seleccionados = new List<ReporteRetention>();
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
                        foreach (ReporteRetention row in seleccionados)
                        {
                            bool resultado = new clsEntityRetention(localDB).cs_pxEliminarDocumento(row.Id);
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
                            // obj.FooterIcon = CustomDialogIcons.Shield;
                            // obj.FooterText = "This is a secure program";
                            obj.InstructionHeading = "Documentos eliminados";
                            obj.InstructionIcon = CustomDialogIcons.Information;
                            obj.InstructionText = "Los documentos se eliminaron correctamente de la base de datos.";
                            CustomDialogResults objResults = obj.Show();
                            // System.Windows.Forms.MessageBox.Show("Los siguientes documentos se eliminaron correctamente de la base de datos.\n"+mensaje,"Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        refrescarGrilla();

                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("delete factura" + ex.ToString());
                //clsBaseMensaje.cs_pxMsgEr("ERR15", ex.Message);
            }
        }
    }
}
