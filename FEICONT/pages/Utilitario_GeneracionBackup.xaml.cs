using FEICONT.ayuda;
using Extension.Base;
using Extension.Datos;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
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
    /// Lógica de interacción para Utilitario_GeneracionBackup.xaml
    /// </summary>
    public partial class Utilitario_GeneracionBackup : Page
    {
        private Window VentanaPrincipal;
        private clsEntityDeclarant declarante;
        private List<clsEntityDeclarant> entidades;
        private List<ReporteEmpresa> lista_reporte;
        private ReporteEmpresa itemRow;
       
        public Utilitario_GeneracionBackup(clsEntityDeclarant empresa,Window parent)
        {
            InitializeComponent();
            VentanaPrincipal = parent;
            declarante = empresa;
        }
        private void cargarDataGrid()
        {
            //Limpiar la grilla.
            dgEmpresas.ItemsSource = null;
            dgEmpresas.Items.Clear();
            //Obtener todos los declarantes registrados
            entidades = new clsEntityDeclarant().cs_pxObtenerTodo();
            lista_reporte = new List<ReporteEmpresa>();
            if (entidades.Count > 0)
            {
                //Recorrer los registro de declarantes para rellenra la grilla
                foreach (var item in entidades)
                {
                    itemRow = new ReporteEmpresa();
                    itemRow.Id = item.Cs_pr_Declarant_Id;
                    itemRow.RazonSocial = item.Cs_pr_RazonSocial;
                    itemRow.Ruc = item.Cs_pr_Ruc;
                    itemRow.RutaCertificadoDigital = item.Cs_pr_Rutacertificadodigital;
                    lista_reporte.Add(itemRow);
                }
                dgEmpresas.ItemsSource = lista_reporte;
             
            }

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RUC.Content = declarante.Cs_pr_Ruc;
            NombreEmpresa.Content = declarante.Cs_pr_RazonSocial;
            cargarDataGrid();
        }
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReporteEmpresa comprobante = (ReporteEmpresa)dataGridRow.DataContext;

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
            ReporteEmpresa comprobante = (ReporteEmpresa)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                comprobante.Check = false;
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
                    foreach (ReporteEmpresa item in lista_reporte)
                    {
                        item.Check = true;
                    }
                    dgEmpresas.ItemsSource = null;
                    dgEmpresas.Items.Clear();
                    dgEmpresas.ItemsSource = lista_reporte;
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
                    foreach (ReporteEmpresa item in lista_reporte)
                    {
                        item.Check = false;
                    }
                    dgEmpresas.ItemsSource = null;
                    dgEmpresas.Items.Clear();
                    dgEmpresas.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }
      

        private void btnSeleccionar_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            // BOTON DE NUEVO FOLDER ACTIVADO
            folderBrowserDlg.ShowNewFolderButton = true;
            // MOSTRAR CUADRO DE DIALOGO
            DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult.Equals(DialogResult.OK))
            {
                // MOSTRAR CARPETA ELEGIDA EN CUADRO DE TEXTO;
                txtRutaTodos.Text = folderBrowserDlg.SelectedPath;
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }
        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            List<ReporteEmpresa> seleccionados = new List<ReporteEmpresa>();
            foreach (ReporteEmpresa item in lista_reporte)
            {
                if (item.Check == true)
                {
                    seleccionados.Add(item);
                }
            }
            if (seleccionados.Count > 0)
            {
                if (txtRutaTodos.Text.Trim().Length > 0)
                {

                    bool procesar = false;
                    bool password = false;
                    if (chkPasswordTodos.IsChecked == true)
                    {
                        if (txtPasswordTodos.Text.Trim().Length >= 3)
                        {
                            procesar = true;
                            password = true;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Debe ingresar el password, minimo 3 caracteres.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            procesar = false;
                        }
                    }
                    else
                    {
                        procesar = true;
                    }

                    if (procesar)
                    {
                        string resultado = string.Empty;
                        string baseLocation = txtRutaTodos.Text;
                        string txtPasswordString = txtPasswordTodos.Text;
                        ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () =>
                        {
                           resultado = procesarBackMultiple(seleccionados, password, baseLocation, txtPasswordString);
                        });

                        System.Windows.Forms.MessageBox.Show("Se procesaron los backups.\n"+resultado, "Resumen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ///////
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione una ruta para generar los archivos backups", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Seleccione las empresas para generar los backups", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }      
        private string procesarBackMultiple(List<ReporteEmpresa> seleccionados, bool password, string baseLocation, string txtPasswordString)
        {
            string retornar = string.Empty;
            string procesados = string.Empty;
            string no_procesados = string.Empty;
            foreach (var it in seleccionados)
            {
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(it.Id);
                string cadenaConexion = "Driver={" + local.Cs_pr_DBMSDriver + "};Server=" + local.Cs_pr_DBMSServername + "," + local.Cs_pr_DBMSServerport + ";Database=" + local.Cs_pr_DBName + ";Uid=" + local.Cs_pr_DBUser + ";Pwd=" + local.Cs_pr_DBPassword + ";";
                string resultado = string.Empty;

                //string nameBack = "FEICONTBACKUP_" + it.Ruc + "_" + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString();
                string nameBack = "FEICONTBACKUP_" + it.Ruc;

                string fullName = baseLocation + "\\" + nameBack + ".BAK";
                string targetDirectory = baseLocation + "\\" + nameBack + ".zip";
                resultado = procesarBackupUnico(cadenaConexion, password, baseLocation, nameBack, fullName, targetDirectory, txtPasswordString);

                if (resultado == "1")
                {
                    procesados += it.RazonSocial+"\n";
                    //System.Windows.Forms.MessageBox.Show("Se ha generado correctamente el backup.\n", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    no_procesados += it.RazonSocial + "\n";
                    //System.Windows.Forms.MessageBox.Show("Se ha producido un error al procesar el backup.\n" + resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (procesados.Length > 0)
            {
                retornar += "Backups procesados:\n"+procesados;
            }
            if (no_procesados.Length > 0)
            {
                retornar += "Backups no procesados:\n"+no_procesados;
            }
            
            return retornar;
        }
        private void btnSeleccionarUnico_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            // BOTON DE NUEVO FOLDER ACTIVADO
            folderBrowserDlg.ShowNewFolderButton = true;
            // MOSTRAR CUADRO DE DIALOGO
            DialogResult dlgResult = folderBrowserDlg.ShowDialog();
            if (dlgResult.Equals(DialogResult.OK))
            {
                // MOSTRAR CARPETA ELEGIDA EN CUADRO DE TEXTO;
                txtRutaUnico.Text = folderBrowserDlg.SelectedPath;
                //Environment.SpecialFolder rootFolder = folderBrowserDlg.RootFolder;
            }
        }
        private void btnGenerarUnico_Click(object sender, RoutedEventArgs e)
        {
            clsEntityDatabaseLocal localDB = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarante.Cs_pr_Declarant_Id);
            if (txtRutaUnico.Text.Trim().Length > 0)
            {
                bool procesar = false;
                bool password = false;
                if (chkPasswordUnico.IsChecked == true)
                {
                    if (txtPasswordUnico.Text.Trim().Length >= 3)
                    {
                        procesar = true;
                        password = true; 
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Debe ingresar el password, minimo 3 caracteres.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        procesar = false;
                    }
                }
                else
                {
                    procesar = true;
                }

                if (procesar)
                {
                    clsBaseConexion cn = new clsBaseConexion(localDB);
                    string resultado = string.Empty;
                    string baseLocation = txtRutaUnico.Text;
                   // string nameBack = "FEICONTBACKUP_" + declarante.Cs_pr_Ruc + "_" + DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString();
                    string nameBack = "FEICONTBACKUP_" + declarante.Cs_pr_Ruc;

                    string fullName = baseLocation + "\\" + nameBack + ".BAK";
                    string targetDirectory = baseLocation + "\\" + nameBack + ".zip";
                    string txtPasswordString = txtPasswordUnico.Text;
                    ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () => {
                      resultado = procesarBackupUnico(localDB.cs_prConexioncadenabasedatos(), password,baseLocation,nameBack,fullName,targetDirectory,txtPasswordString);
                    });
                    if (resultado == "1")
                    {
                        System.Windows.Forms.MessageBox.Show("Se ha generado correctamente el backup.\n","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Se ha producido un error al procesar el backup.\n"+resultado,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }

                }
                ///////
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Seleccione una ruta para generar el archivo backup", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private string  procesarBackupUnico(string conexion,bool password,string baseLocation,string nameBack, string fullName, string targetDirectory,string txtPasswordString)
        {
            string retorno=string.Empty;
            try
            {                          
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(conexion);
                OdbcCommand cmd = new OdbcCommand("{call BackupDB (?,?,?)}", cs_pxConexion_basedatos);
                OdbcParameter prm = cmd.Parameters.Add("@BaseLocation", OdbcType.VarChar, 1024);
                prm.Value = baseLocation + "\\";

                prm = cmd.Parameters.Add("@NameBack", OdbcType.VarChar, 1024);
                prm.Value = nameBack;

                prm = cmd.Parameters.Add("@BackupType", OdbcType.VarChar, 32);
                prm.Value = "FULL";

                cs_pxConexion_basedatos.Open();
                int resultado = cmd.ExecuteNonQuery();
                cs_pxConexion_basedatos.Close();

                if (resultado == -1)
                {
                    if (password)
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.Password = txtPasswordString;
                            zip.AddFile(fullName);
                            zip.Save(targetDirectory);
                        }
                    }
                    else
                    {
                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddFile(fullName);
                            zip.Save(targetDirectory);
                        }
                    }
                    File.Delete(fullName);
                }
                retorno = "1";
            }
            catch(Exception ex)
            {
                retorno = ex.Message;
                clsBaseLog.cs_pxRegistarAdd("process bkuno"+ex.ToString());
            }
            return retorno;
            
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("26");
                ayuda.ShowDialog();
            }
        }
    }
}
