using FEICONT.ayuda;
using FEICONT.CustomDialog;
using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
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
    /// Lógica de interacción para Utilitario_RestauracionBackup.xaml
    /// </summary>
    public partial class Utilitario_RestauracionBackup : Page
    {
        private Window VentanaPrincipal;
        private clsEntityDeclarant declarante;
        private List<ReporteEmpresa> lista_reporte;
        private List<clsEntityDeclarant> entidades;
        private ReporteEmpresa itemRow;
        public Utilitario_RestauracionBackup(clsEntityDeclarant empresa, Window parent)
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

        private void btnSeleccionarUnico_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "BAK Files (*.BAK)|*.BAK|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRutaUnico.Text = openFileDialog1.FileName;
                if (txtRutaUnico.Text.Substring(txtRutaUnico.Text.Length - 4) != ".BAK")
                {
                    txtRutaUnico.Text = "";
                }
            }
        }

        private void btnRestauracionUnico_Click(object sender, RoutedEventArgs e)
        {
            if (txtRutaUnico.Text.Trim().Length > 0)
            {
                string rutaArchivo = txtRutaUnico.Text;
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarante.Cs_pr_Declarant_Id);
                //clsBaseConexion cn = new clsBaseConexion();
                string cadenaServidor = local.cs_prConexioncadenaservidor();
                clsBaseLog.cs_pxRegistarAdd(cadenaServidor);
                string resultado = string.Empty;
                ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () =>
                {
                    resultado = restaurarUnico(local, cadenaServidor, rutaArchivo);
                });
                if (resultado.Trim().Length <= 0)
                {
                    System.Windows.Forms.MessageBox.Show("El backup se ha restaurado correctamente.\n", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                   // System.Windows.Forms.MessageBox.Show("Se ha producido un error al procesar el backup.\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CustomDialogWindow obj = new CustomDialogWindow();
                    obj.AdditionalDetailsText = resultado;
                    obj.Buttons = CustomDialogButtons.OK;
                    obj.Caption = "Mensaje";
                    obj.DefaultButton = CustomDialogResults.OK;
                    obj.InstructionHeading = "Restauración fallida.";
                    obj.InstructionIcon = CustomDialogIcons.Warning;
                    obj.InstructionText = "Se ha producido un error al procesar el backup. Revise los detalles para mayor información";
                    CustomDialogResults objResults = obj.Show();
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Seleccione un archivo para restaurar el backup", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private string restaurarUnico(clsEntityDatabaseLocal local, string cadenaConexionServidor, string rutaArchivo)
        {
            string retorno = string.Empty;
            try
            {
                clsBaseConfiguracion conf = new clsBaseConfiguracion();
                //string[] Partes_Cadena_Conexion = cadenaConexionServidor.Split(';');
                //cadenaConexionServidor = Partes_Cadena_Conexion[1] + ";" + Partes_Cadena_Conexion[2] + ";" + Partes_Cadena_Conexion[3] + ";";
                //SqlConnection sqlcon = new SqlConnection(cadenaConexionServidor);
                OdbcConnection sqlcon = new OdbcConnection(cadenaConexionServidor);
                sqlcon.Open();
                string UseMaster = "USE master";
                string DatabaseFullPath = local.Cs_pr_DBName;
                string backUpPath = rutaArchivo;
                try
                {
                    //SqlCommand UseMasterCommand = new SqlCommand(UseMaster,sqlcon);
                    OdbcCommand UseMasterCommand = new OdbcCommand(UseMaster, sqlcon);
                    UseMasterCommand.ExecuteNonQuery();
                }
                catch(Exception ex1)
                {
                    retorno = ex1.Message;
                }

                string Alter1 = @"ALTER DATABASE
                   " + DatabaseFullPath + " SET Single_User WITH Rollback Immediate";

                try
                {
                    //SqlCommand Alter1Cmd = new SqlCommand(Alter1,sqlcon);
                    OdbcCommand Alter1Cmd = new OdbcCommand(Alter1, sqlcon);
                    Alter1Cmd.ExecuteNonQuery();
                }
                catch (Exception ex2)
                {
                    retorno = ex2.Message;
                }

                List<string> tabla_contenidos = new List<string>();

                //Cristhian|23/01/2018|FEICONT2-533
                /*Se cambia la consulta SQL para traer el nombre de los archivos .mdf y .ldf de la base de datos*/
                /*INICIO MODIFICACIóN*/
                /*Se declara la sentencia SQL que obtiene el nombre los archivos .mdf y .ldf*/
                string Alter10 = "RESTORE FILELISTONLY FROM DISK = '" + backUpPath +"' ";
                DataTable tabla_datos_archivos = new DataTable();
                try
                {
                    //SqlCommand Alter10Cmd = new SqlCommand(Alter10,sqlcon);
                    OdbcCommand Alter10Cmd = new OdbcCommand(Alter10, sqlcon);
                    /*Se ejecuta el comando DataAdapter y para que los datos se guarden en un data table*/
                    //SqlDataAdapter TableAdapt = new SqlDataAdapter(Alter10, sqlcon);
                    OdbcDataAdapter TableAdapt = new OdbcDataAdapter(Alter10, sqlcon);
                    /*Se llena de datos el datatable*/
                    TableAdapt.Fill(tabla_datos_archivos);
                }
                catch (Exception ex3)
                {
                    /*Si no se cumple, se obtiene el mensaje de error para registrarlo*/
                    retorno = ex3.Message;
                }
                string[] UbicacionAnterior = tabla_datos_archivos.Rows[0][0].ToString().Split('\\');
                int numeroItems_UA = UbicacionAnterior.Count();
                string[] NombreArchivo = UbicacionAnterior[UbicacionAnterior.Count()-1].Split('.');
                try
                {
                    /*Antes de modificar la cadena de restauracion, se debe verificar el archivo log del FEICONT*/
                    /*Aqui esta la sentencia SQL para restaurar la base de datos, en el cual tambien movemos el archivo .mdf y ldf hacia nuestra carpeta de las bases de datos*/
                    string Restore = @"RESTORE DATABASE
                    [" + DatabaseFullPath + "] FROM DISK = N'" +
                    backUpPath + @"' WITH FILE = 1, MOVE N'" + tabla_datos_archivos.Rows[0][0].ToString() +
                    "' TO N'" + conf.cs_prRutainstalacion + "\\BackUp\\" + DatabaseFullPath + ".mdf', MOVE  N'" + tabla_datos_archivos.Rows[1][0].ToString() +
                    "' TO N'" + conf.cs_prRutainstalacion + "\\BackUp\\" + DatabaseFullPath + "_log.ldf',  NOUNLOAD, REPLACE, STATS = 5";
                    /*Se ejecuta el comando SQL Server*/

                    //SqlCommand RestoreCmd = new SqlCommand(Restore, sqlcon);
                    OdbcCommand RestoreCmd = new OdbcCommand(Restore, sqlcon);
                    RestoreCmd.ExecuteNonQuery();
                    retorno = "";
                }
                catch (Exception restore)
                {
                    if (NombreArchivo[0]!= DatabaseFullPath)
                    {
                        retorno = "La base de datos "+ DatabaseFullPath+ " no coincide con la base de datos que se quiere restaurar, que es: "+ NombreArchivo[0];
                    }
                    else
                    {
                        retorno = restore.Message;
                    }
                }
                /*FIN MODIFICACIóN*/

                // the below query change the database back to multiuser
                string Alter2 = @"ALTER DATABASE
                   " + DatabaseFullPath + " SET Multi_User";
                try
                {
                    //SqlCommand Alter2Cmd = new SqlCommand(Alter2,sqlcon);
                    OdbcCommand Alter2Cmd = new OdbcCommand(Alter2, sqlcon);
                    Alter2Cmd.ExecuteNonQuery();
                }
                catch (Exception ex4)
                {
                    retorno = ex4.Message;
                }

                sqlcon.Close();             
            }
            catch (Exception ex)
            {
                retorno = ex.Message;              
                clsBaseLog.cs_pxRegistarAdd("restback" + ex.ToString());
                //System.Windows.Forms.MessageBox.Show("Se ha producido un error al procesar la restauración de backup.\n", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return retorno;
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

        private void btnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            //Buscar todas las empresas y restaurar uno por uno
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
                    string resultado = string.Empty;
                    string baseLocation = txtRutaTodos.Text;
                    ProgressDialogResult result = ProgressWindow.Execute(VentanaPrincipal, "Procesando...", () =>
                    {
                        resultado = procesarBackMultiple(seleccionados, baseLocation);
                    });

                    //System.Windows.Forms.MessageBox.Show("Resumen backups.\n" + resultado, "Resumen", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CustomDialogWindow obj = new CustomDialogWindow();
                    obj.AdditionalDetailsText = "Resumen de archivos procesados:\n" + resultado;
                    obj.Buttons = CustomDialogButtons.OK;
                    obj.Caption = "Mensaje";
                    obj.DefaultButton = CustomDialogResults.OK;
                    // obj.FooterIcon = CustomDialogIcons.Shield;
                    // obj.FooterText = "This is a secure program";
                    obj.InstructionHeading = "Backup procesados";
                    obj.InstructionIcon = CustomDialogIcons.Information;
                    obj.InstructionText = "Se han procesado los archivos de la carpeta seleccionada.";
                    CustomDialogResults objResults = obj.Show();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione las empresas para restaurar los backups", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private string procesarBackMultiple(List<ReporteEmpresa> seleccionados, string baseLocation)
        {
            string retornar = string.Empty;
            string procesados = string.Empty;
            string no_procesados = string.Empty;
            string no_ruta = string.Empty;
            foreach (var it in seleccionados)
            {
                string nombreArchivo = "FEICONTBACKUP_" + it.Ruc;
                string rutaFullArchivo = baseLocation + "\\" + nombreArchivo + ".BAK";
                //buscar archivo en la ruta proporcionada:
                bool existe = File.Exists(rutaFullArchivo);
                //
                if (existe)
                {
                    clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(it.Id);
                    string cadenaServidor = "Driver={" + local.Cs_pr_DBMSDriver + "};Server=" + local.Cs_pr_DBMSServername + "," + local.Cs_pr_DBMSServerport + ";Uid=" + local.Cs_pr_DBUser + ";Pwd=" + local.Cs_pr_DBPassword + ";";
                    string resultado = string.Empty;
                    resultado = procesarBackup(cadenaServidor, local.Cs_pr_DBName.Trim().ToString(), rutaFullArchivo);
                    if (resultado == "1")
                    {
                        procesados += "  "+it.RazonSocial + "\n";
                    }
                    else
                    {
                        no_procesados += "  " + it.RazonSocial + "\n";
                    }
                }
                else
                {
                    no_ruta += "  " + it.RazonSocial + "\n";
                }                                     
            }

            if (procesados.Length > 0)
            {
                retornar += "►Backups procesados:\n" + procesados;
            }
            if (no_procesados.Length > 0)
            {
                retornar += "►Backups no procesados:\n" + no_procesados;
            }
            if (no_ruta.Length > 0)
            {
                retornar += "►Backups no encontrados:\n" + no_ruta;
            }

            return retornar;         
        }
        private string procesarBackup(string cadenaConexionServidor,string dbName,string rutaArchivo )
        {
            string retorno = string.Empty;
            try
            {
                clsBaseConfiguracion conf = new clsBaseConfiguracion();
                OdbcConnection sqlcon = new OdbcConnection(cadenaConexionServidor);
                sqlcon.Open();
                string UseMaster = "USE master";
                string DatabaseFullPath = dbName;
                string backUpPath = rutaArchivo;
                try
                {
                    OdbcCommand UseMasterCommand = new OdbcCommand(UseMaster, sqlcon);
                    UseMasterCommand.ExecuteNonQuery();
                }
                catch
                {
                    retorno = "2";
                }
              
                // The below query will rollback any transaction which is running on that database and brings SQL Server database in a single user mode.
                string Alter1 = @"ALTER DATABASE
                   " + DatabaseFullPath + " SET Single_User WITH Rollback Immediate";

                try
                {
                    OdbcCommand Alter1Cmd = new OdbcCommand(Alter1, sqlcon);
                    Alter1Cmd.ExecuteNonQuery();
                }
                catch 
                {
                    retorno = "2";
                }

                //Cristhian|23/01/2018|FEICONT2-533
                /*Se cambia la consulta SQL para traer el nombre de los archivos .mdf y .ldf de la base de datos*/
                /*INICIO MODIFICACIóN*/
                /*Se declara la sentencia SQL que obtiene el nombre los archivos .mdf y .ldf*/
                string Alter10 = "RESTORE FILELISTONLY FROM DISK = '" + backUpPath + "' ";
                DataTable tabla_datos_archivos = new DataTable();
                try
                {
                    OdbcCommand Alter10Cmd = new OdbcCommand(Alter10, sqlcon);
                    /*Se ejecuta el comando DataAdapter y para que los datos se guarden en un data table*/
                    OdbcDataAdapter TableAdapt = new OdbcDataAdapter(Alter10, sqlcon);
                    /*Se llena de datos el datatable*/
                    TableAdapt.Fill(tabla_datos_archivos);
                }
                catch
                {
                    /*Si no se cumple, se obtiene el mensaje de error para registrarlo*/
                    retorno = "2";
                }

                try
                {
                    /*Antes de modificar la cadena de restauracion, se debe verificar el archivo log del FEICONT*/
                    /*Aqui esta la sentencia SQL para restaurar la base de datos, en el cual tambien movemos el archivo .mdf y ldf hacia nuestra carpeta de las bases de datos*/
                    string Restore = @"RESTORE DATABASE
                    [" + DatabaseFullPath + "] FROM DISK = N'" +
                    backUpPath + @"' WITH FILE = 1, MOVE N'" + tabla_datos_archivos.Rows[0][0].ToString() + "' TO N'" + conf.cs_prRutainstalacion + "\\" + DatabaseFullPath + ".mdf', MOVE  N'" + tabla_datos_archivos.Rows[1][0].ToString() + "' TO N'" + conf.cs_prRutainstalacion + "\\" + DatabaseFullPath + "_log.ldf',  NOUNLOAD, REPLACE, STATS = 5";
                    /*Se ejecuta el comando SQL Server*/
                    OdbcCommand RestoreCmd = new OdbcCommand(Restore, sqlcon);
                    RestoreCmd.ExecuteNonQuery();
                    retorno = "1";
                }
                catch
                {
                    retorno = "2";
                }
                /*FIN MODIFICACIóN*/

                // the below query change the database back to multiuser
                string Alter2 = @"ALTER DATABASE
                   " + DatabaseFullPath + " SET Multi_User";
                try
                {
                    OdbcCommand Alter2Cmd = new OdbcCommand(Alter2, sqlcon);
                    Alter2Cmd.ExecuteNonQuery();

                }
                catch
                {
                    retorno = "2";
                }
                          
                sqlcon.Close();
                //retorno = "1";
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("restback " + ex.ToString());
                retorno = "2";
            }
            return retorno;
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

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("27");
                ayuda.ShowDialog();
            }
        }
    }
}
