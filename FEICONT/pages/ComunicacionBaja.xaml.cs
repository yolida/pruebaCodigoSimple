using BusinessLayer;
using DataLayer;
using DataLayer.CRUD;
using FEICONT.ayuda;
using FEICONT.CustomDialog;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml.Schema;
using CheckBox = System.Windows.Controls.CheckBox;

namespace FEICONT.pages
{
    /// <summary>
    /// Interaction logic for GenerarComunicacionBaja.xaml
    /// </summary>
    public partial class GenerarComunicacionBaja : Page
    {

        List<ComboBoxPares> tipos_comprobante = new List<ComboBoxPares>();

        ReadGeneralData readGeneralData = new ReadGeneralData();
        private readonly IGetDataAsync _Documentos;
        Data_DatosFox data_DatosFox;
        Data_Usuario data_Usuario;
        Data_Contribuyente data_Contribuyente;
        List<Data_Documentos> data_Documentos = new List<Data_Documentos>();
        Data_Log data_Log;
        private Window padre;
        public GenerarComunicacionBaja(Window parent, Data_Usuario usuario)
        {
            InitializeComponent();
            padre = parent;
            data_Usuario    =   usuario;

            Data_Documentos documentos  =   new Data_Documentos();
            _Documentos     =   (IGetDataAsync)documentos;

            data_DatosFox   =   new Data_DatosFox(data_Usuario.IdDatosFox);
            data_DatosFox.Read_DatosFox();

            data_Contribuyente  =   new Data_Contribuyente(data_DatosFox.IdEmisor);
            data_Contribuyente.Read_Contribuyente();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor    =   System.Windows.Input.Cursors.Wait;

                DataTable dataTable     =   readGeneralData.GetDataTable("[dbo].[Read_TipoDocumentos]");
                DataRow row             =   dataTable.NewRow();
                row["Descripcion"]      =   "Todos los documentos";
                row["IdTipoDocumento"]  =   0;
                dataTable.Rows.Add(row);

                var items           =   (dataTable as IListSource).GetList();
                lstTipoDocumento.ItemsSource        =   items;
                lstTipoDocumento.DisplayMemberPath  =   "Descripcion";
                lstTipoDocumento.SelectedValuePath  =   "IdTipoDocumento";
                lstTipoDocumento.SelectedIndex      =   dataTable.Rows.Count - 1;

                datePick_inicio.Text    =   DateTime.Now.Date.ToString();
                datePick_fin.Text       =   DateTime.Now.Date.AddDays(1).ToString();

                LoadGrid();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"La base de datos ha sido alterada, contacte con soporte. Detalle: {ex}", "Base de datos alterada", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        public async void LoadGrid()
        {
            dgDocumentos.ItemsSource    =   null;
            dgDocumentos.Items.Clear();

            try
            {
                Mouse.OverrideCursor        =   System.Windows.Input.Cursors.Wait;
                data_Documentos             =   await GetDocumentos();
                dgDocumentos.ItemsSource    =   data_Documentos;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ha ocurrido un error al cargar los registros, detalle del error: {ex}",
                    "Error al cargar los datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally {
                Mouse.OverrideCursor = null;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)  => LoadGrid();

        public async Task<List<Data_Documentos>> GetDocumentos()
        {
            var listDocumentos  =   new List<Data_Documentos>();
            try
            {
                string estadoDocumento  =   lstEstadoDocumento.SelectionBoxItem.ToString();
                bool? deBaja            =   null;
                switch (estadoDocumento)
                {
                    case "Todos los documentos":
                        deBaja  =   null;
                        break;
                    case "Dados de baja":
                        deBaja  =   true;
                        break;
                    case "Sin dar de baja":
                        deBaja  =   false;
                        break;
                    default:
                        deBaja  =   null;
                        break;
                }

                listDocumentos  =   await _Documentos.GetListFiltered(data_DatosFox.IdDatosFox, DateTime.Parse(datePick_inicio.SelectedDate.ToString()),
                                    DateTime.Parse(datePick_fin.SelectedDate.ToString()), int.Parse(lstTipoDocumento.SelectedValue.ToString()), deBaja);
            }
            catch (Exception ex)
            {
                listDocumentos  =   new List<Data_Documentos>();
            }

            return listDocumentos;
        }

        private void chkCell_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox               =   (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow         =   VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            Data_Documentos data_Documentos =   (Data_Documentos)dataGridRow.DataContext;
            data_Documentos.Selectable      =   true;
        }

        private void chkCell_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox               =   (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow         =   VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            Data_Documentos data_Documentos =   (Data_Documentos)dataGridRow.DataContext;
            data_Documentos.Selectable      =   false;
        }
        
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data_Documentos.Count > 0)
                {
                    foreach (Data_Documentos data_Documento in data_Documentos)
                        data_Documento.Selectable   =   true;

                    dgDocumentos.ItemsSource    =   null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource    =   data_Documentos;
                }
                else
                {
                    e.Handled           =   false;
                    chkAll.IsChecked    =   false;
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al seleccionar todo, detalle del error: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data_Documentos.Count > 0)
                {
                    foreach (Data_Documentos data_Documento in data_Documentos)
                        data_Documento.Selectable   =   false;

                    dgDocumentos.ItemsSource    =   null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource    =   data_Documentos;
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al deseleccionar todo, detalle del error: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("10");
                ayuda.ShowDialog();
            }
        }

        private void btnDescargarXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int documentosSinXML    =   0;
                string mensajeFinal     =   string.Empty;

                List<Data_Documentos> selected_data_Documentos = new List<Data_Documentos>();
                foreach (var data_Documento in data_Documentos)
                {
                    if (data_Documento.Selectable   ==  true)
                        selected_data_Documentos.Add(data_Documento);

                    if (data_Documento.EstadoSunat  ==  "Pendiente")
                        documentosSinXML++;
                }

                if (selected_data_Documentos.Count() > 0)
                {
                    if (documentosSinXML > 0)
                    {
                        System.Windows.MessageBox.Show("Ha seleccionado uno o más documentos que aún no se envían a Sunat (Pendiente), por este motivo aún no se ha" +
                            " generado su respectivo XML, envíe el(los) documentos y vuelva a intentarlo", "Documento sin XML", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        string pdfsNoEncontrados    =   string.Empty;
                        string rutaDescarga         =   Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        VistaFolderBrowserDialog dialog =   new VistaFolderBrowserDialog();
                        dialog.Description              =   "Seleccione el directorio donde desea guardar los archivos.";
                        dialog.UseDescriptionForTitle   =   true;


                        if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
                            System.Windows.MessageBox.Show("Estimado usuario estas empleando una versión muy antigua de Windows, algunas funciones están restringidas, " +
                                "tu documento será descargado en el directorio MIS DOCUMENTOS.", "Algunas funciones no están soportadas en tu sistema operativo",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                        {
                            var resultado   =   dialog.ShowDialog();
                            if (resultado.HasValue  &&  resultado.Value)
                            {
                                rutaDescarga    =   dialog.SelectedPath;

                                ProgressDialogResult result = ProgressWindow.Execute(padre, "Procesando...", () => {
                                    foreach (var selected_data_Documento in selected_data_Documentos)
                                    {
                                        var nombreArchivo = 
                                        $"{data_Contribuyente.NroDocumento}-{selected_data_Documento.TipoDocumento}-{selected_data_Documento.SerieCorrelativo}";

                                        if (!Directory.Exists($"{rutaDescarga}\\{nombreArchivo}"))
                                            Directory.CreateDirectory($"{rutaDescarga}\\{nombreArchivo}");

                                        #region PDF
                                        try
                                        {
                                            if (!File.Exists($"{selected_data_Documento.Ruta}\\{nombreArchivo}\\{nombreArchivo}.pdf"))
                                                pdfsNoEncontrados += $"{selected_data_Documento.SerieCorrelativo}, ";
                                            else
                                            {
                                                File.Copy($"{selected_data_Documento.Ruta}\\{nombreArchivo}\\{nombreArchivo}.pdf",
                                                    $"{rutaDescarga}\\{nombreArchivo}\\{nombreArchivo}.pdf", true);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            var msg     =   string.Concat(ex.InnerException?.Message,   ex.Message);
                                            data_Log    =   new Data_Log()
                                            {
                                                DetalleError    =   $"Detalle del error: {msg}",
                                                Comentario      =   "Error al leer o guardar PDF de representación impresa",
                                                IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                            };
                                            data_Log.Create_Log();
                                        }
                                        #endregion PDF

                                        DataTable datosDocumento = 
                                            readGeneralData.GetDataTable("[dbo].[Query_Scalar_GetXml_Documento]", "@IdDocumento", selected_data_Documento.IdDocumento);
                                        DataRow row     =   datosDocumento.Rows[0];

                                        File.WriteAllBytes($"{rutaDescarga}\\{nombreArchivo}\\{nombreArchivo}.xml", Convert.FromBase64String(row["XmlFirmado"].ToString()));

                                        if (!string.IsNullOrEmpty(row["CdrSunat"].ToString()))
                                            File.WriteAllBytes($"{rutaDescarga}\\{nombreArchivo}\\R-{nombreArchivo}.zip", Convert.FromBase64String(row["CdrSunat"].ToString()));

                                        datosDocumento.Clear();
                                    }
                                });

                                if (!string.IsNullOrEmpty(pdfsNoEncontrados))
                                {
                                    mensajeFinal    =   $"El(los) documento(s) xml fueron guardados en :{dialog.SelectedPath} pero no hemos podido encontrar la" +
                                        $" representación impresa de los siguientes documentos: {pdfsNoEncontrados}";
                                }
                                else
                                    mensajeFinal    =   $"El(los) documento(s) fueron guardados en :{dialog.SelectedPath}";

                                CustomDialogWindow customDialogWindow       =   new CustomDialogWindow();
                                customDialogWindow.Buttons                  =   CustomDialogButtons.OK;
                                customDialogWindow.Caption                  =   "Detalle";
                                customDialogWindow.DefaultButton            =   CustomDialogResults.OK;
                                customDialogWindow.InstructionHeading       =   "Resultados de la descarga del documento(s)";
                                customDialogWindow.InstructionIcon          =   CustomDialogIcons.Information;
                                customDialogWindow.InstructionText          =   mensajeFinal;
                                CustomDialogResults customDialogResults     =   customDialogWindow.Show();

                                LoadGrid();
                            }
                        }
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("Debe seleccionar al menos un documento");
            }
            catch (Exception ex)
            {
                var msg     =   string.Concat(ex.InnerException?.Message,   ex.Message);
                System.Windows.MessageBox.Show(msg, "Error al descargar los archivos del documento", MessageBoxButton.OK, MessageBoxImage.Error);
                data_Log    =   new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al descargar los archivos del documento",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
        }

        private void btnMotivoBaja_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Data_Documentos> selected_data_Documentos = new List<Data_Documentos>();
                foreach (var data_Documento in data_Documentos)
                {
                    if (data_Documento.Selectable   ==  true)
                        selected_data_Documentos.Add(data_Documento);
                }

                switch (selected_data_Documentos.Count())
                {
                    case 0:
                        System.Windows.Forms.MessageBox.Show("Debe seleccionar un documento", "Error de elección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 1:
                        if (!selected_data_Documentos[0].ComunicacionBaja)
                        {
                            PromtMotivoBaja promtMotivoBaja =   new PromtMotivoBaja(data_Usuario, selected_data_Documentos[0].IdDocumento);
                            promtMotivoBaja.ShowDialog();
                            LoadGrid();
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Estimado dino-usuario, ya dio de baja a este documento, y ya no puede cambiar el motivo de baja del documento.", 
                                "Documento ya dado de baja", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Debe seleccionar sólo un documento", "Error de elección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                var msg     =   string.Concat(ex.InnerException?.Message,   ex.Message);
                System.Windows.MessageBox.Show(msg, "Error inesperado al elegir el documento", MessageBoxButton.OK, MessageBoxImage.Error);
                data_Log    =   new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error inesperado al elegir el documento",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
        }

        private void btnGenerarBaja_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Data_Documentos> selected_data_Documentos = new List<Data_Documentos>();
                foreach (var data_Documento in data_Documentos)
                {
                    if (data_Documento.Selectable == true)
                        selected_data_Documentos.Add(data_Documento);
                }

                List<Data_Documentos> dadosDeBaja = new List<Data_Documentos>();
                foreach (var selected_data_Documento in selected_data_Documentos)
                {
                    if (selected_data_Documento.ComunicacionBaja == true)
                        dadosDeBaja.Add(selected_data_Documento);
                }

                if (selected_data_Documentos.Count() > 0)
                {
                    string enviados     =   string.Empty;
                    string noEnviados   =   string.Empty;
                    string mensajeFinal =   string.Empty;

                    if (dadosDeBaja.Count == 0)
                    {
                        ProgressDialogResult result     =   ProgressWindow.Execute(padre, "Procesando...", () =>
                        {
                            ProcesarEnvio procesarEnvio =   new ProcesarEnvio(data_Usuario);
                            mensajeFinal = procesarEnvio.Post(selected_data_Documentos);
                        });

                        LoadGrid();

                        CustomDialogWindow customDialogWindow       =   new CustomDialogWindow();
                        customDialogWindow.Buttons                  =   CustomDialogButtons.OK;
                        customDialogWindow.Caption                  =   "Mensaje";
                        customDialogWindow.DefaultButton            =   CustomDialogResults.OK;
                        customDialogWindow.InstructionHeading       =   "Resultados de la comunicación de baja";
                        customDialogWindow.InstructionIcon          =   CustomDialogIcons.Information;
                        customDialogWindow.InstructionText          =   mensajeFinal;
                        CustomDialogResults customDialogResults     =   customDialogWindow.Show();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Estimado usuario, está intentando dar de baja documentos que ya fueron dados de baja previamente. ",
                            "Acción no permitida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("Debe seleccionar al menos un documento", "Ninguna selección detectada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                var msg =   string.Concat(ex.InnerException?.Message,   ex.Message);
                System.Windows.MessageBox.Show(msg, "Error al enviar el documento a sunat", MessageBoxButton.OK, MessageBoxImage.Error);
                data_Log = new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al enviar el documento a sunat desde la interfaz",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
        }
    }
}
