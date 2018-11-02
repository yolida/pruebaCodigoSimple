using FEICONT.ayuda;
using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Configuracion de base de datos local.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Configuracion_bdlocal.xaml
    /// </summary>
    public partial class Configuracion_bdlocal : Page
    {
        private clsEntityDatabaseLocal entidad_basedatos;
        private clsEntityDeclarant declaranteLocal;
        List<ComboBoxPares> GestorBaseDatos = new List<ComboBoxPares>();
        public bool cs_prConexionEstado=false;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor.
        public Configuracion_bdlocal(clsEntityDeclarant declarant)
        {
            InitializeComponent();
            declaranteLocal = declarant;
            localDB = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
            //Agregar valores la combobox de gestores de base de datos.
            GestorBaseDatos.Add(new ComboBoxPares("1", "Microsoft SQL Server"));
            GestorBaseDatos.Add(new ComboBoxPares("2", "MySQL"));
            GestorBaseDatos.Add(new ComboBoxPares("3", "SQLite"));
            cboGestorBD.DisplayMemberPath = "_Value";
            cboGestorBD.SelectedValuePath = "_key";
            cboGestorBD.ItemsSource = GestorBaseDatos;

            if (cboGestorBD.Text == "")
            {
                cboGestorBD.SelectedIndex = 0;
            }
            //Obtener la configuracion dela base de datos local para la empresa actual.
            entidad_basedatos = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
            cboGestorBD.Text = entidad_basedatos.Cs_pr_DBMS;
            txtDriver.Text = entidad_basedatos.Cs_pr_DBMSDriver;
            txtServidor.Text = entidad_basedatos.Cs_pr_DBMSServername;
            txtPuerto.Text = entidad_basedatos.Cs_pr_DBMSServerport;
            txtNombreBD.Text = entidad_basedatos.Cs_pr_DBName;
            txtUsuario.Text = entidad_basedatos.Cs_pr_DBUser;
            txtContrasenia.Password = entidad_basedatos.Cs_pr_DBPassword;

        }
        ///Evento click para crear la base de datos.
        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            //asignar valores a la entidad base de datos local.
            entidad_basedatos.Cs_pr_DBMS = cboGestorBD.Text;
            entidad_basedatos.Cs_pr_DBMSDriver = txtDriver.Text;
            entidad_basedatos.Cs_pr_DBMSServername = txtServidor.Text;
            entidad_basedatos.Cs_pr_DBMSServerport = txtPuerto.Text;
            entidad_basedatos.Cs_pr_DBName = txtNombreBD.Text;
            entidad_basedatos.Cs_pr_DBUser = txtUsuario.Text;
            entidad_basedatos.Cs_pr_DBPassword = txtContrasenia.Password;
            entidad_basedatos.cs_pxActualizar(false);      
            //Actualizar la configuracion para el sistema.
           /* clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
            configuracion.cs_prDbms = entidad_basedatos.Cs_pr_DBMS;
            configuracion.cs_prDbmsdriver = entidad_basedatos.Cs_pr_DBMSDriver;
            configuracion.cs_prDbmsservidor = entidad_basedatos.Cs_pr_DBMSServername;
            configuracion.cs_prDbmsservidorpuerto = entidad_basedatos.Cs_pr_DBMSServerport;
            configuracion.cs_prDbnombre = entidad_basedatos.Cs_pr_DBName;
            configuracion.cs_prDbusuario = entidad_basedatos.Cs_pr_DBUser;
            configuracion.cs_prDbclave = entidad_basedatos.Cs_pr_DBPassword;
            configuracion.cs_pxActualizar(false);*/
            string resultado = entidad_basedatos.cs_pxCrearBaseDatos();
            if (resultado.Trim().Length > 0)
            {
                clsBaseMensaje.cs_pxMsg("Base de datos - Advertencia", resultado);
            }
            else
            {
                cs_pxActualizarEstado();
                (this.Tag as MainWindow).EstadoMenu(cs_prConexionEstado, "1");
                clsBaseMensaje.cs_pxMsgOk("OKE4");
            }
           
        }
        private int existeCambios(clsEntityDatabaseLocal local)
        {
            int cambios = 0;
            if(local.Cs_pr_DBMS!= cboGestorBD.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBMSDriver != txtDriver.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBMSServername != txtServidor.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBMSServerport != txtPuerto.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBName != txtNombreBD.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBUser != txtUsuario.Text.Trim())
            {
                cambios++;
            }
            if (local.Cs_pr_DBPassword != txtContrasenia.Password.Trim())
            {
                cambios++;
            }

            return cambios;
        }
        //Evento para guardar los cambios.
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if(existeCambios(entidad_basedatos) > 0){

                //Asignar valores para la entidad de base de datos local
                entidad_basedatos.Cs_pr_DBMS = cboGestorBD.Text;
                entidad_basedatos.Cs_pr_DBMSDriver = txtDriver.Text;
                entidad_basedatos.Cs_pr_DBMSServername = txtServidor.Text;
                entidad_basedatos.Cs_pr_DBMSServerport = txtPuerto.Text;
                entidad_basedatos.Cs_pr_DBName = txtNombreBD.Text;
                entidad_basedatos.Cs_pr_DBUser = txtUsuario.Text;
                entidad_basedatos.Cs_pr_DBPassword = txtContrasenia.Password;
                entidad_basedatos.cs_pxActualizar(true);
                cs_pxActualizarEstado();
                (this.Tag as MainWindow).EstadoMenu(cs_prConexionEstado, "1");

                //Actualizar valores para la configuracion de base de datos.
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                configuracion.cs_prDbms = entidad_basedatos.Cs_pr_DBMS;
                configuracion.cs_prDbmsdriver = entidad_basedatos.Cs_pr_DBMSDriver;
                configuracion.cs_prDbmsservidor = entidad_basedatos.Cs_pr_DBMSServername;
                configuracion.cs_prDbmsservidorpuerto = entidad_basedatos.Cs_pr_DBMSServerport;
                configuracion.cs_prDbnombre = entidad_basedatos.Cs_pr_DBName;
                configuracion.cs_prDbusuario = entidad_basedatos.Cs_pr_DBUser;
                configuracion.cs_prDbclave = entidad_basedatos.Cs_pr_DBPassword;
                configuracion.cs_pxActualizar(false);
               
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No se han realizado cambios.", "Advertencia - Guardar", MessageBoxButtons.OK, MessageBoxIcon.Warning);       
            }
        }
        // Metodo para actualizar estado  de la conexion a base de  datos
        private void cs_pxActualizarEstado()
        {
            //Obtener el estado de la conexin a bd
            clsBaseConexion conexion = new clsBaseConexion(entidad_basedatos);
            cs_prConexionEstado = conexion.cs_fxConexionEstado();
        }
        // Evento de cambio de gestor de base de datos.
        private void cboGestorBD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxPares seleccionado=(ComboBoxPares)cboGestorBD.SelectedItem;
            if (seleccionado._Value == "SQLite")
            {
                txtServidor.IsEnabled = false;
                txtPuerto.IsEnabled = false;
                txtUsuario.IsEnabled = false;
                txtContrasenia.IsEnabled = false;
            }
            else
            {
                txtServidor.IsEnabled = true;
                txtPuerto.IsEnabled = true;
                txtUsuario.IsEnabled = true;
                txtContrasenia.IsEnabled = true;
            }

            if (seleccionado._Value == "PostgreSQL")
            {
                TextBoton.Content = "Actualizar base de datos existente";
            }
            else
            {
                TextBoton.Content = "Crear base de datos";
            }

            txtDriver.Text = clsBaseUtil.cs_fxDBMS_Driver(cboGestorBD.SelectedIndex + 1);
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("21");
                ayuda.ShowDialog();
            }
        }
        //Evento para verificar la estructura de la base de datos.

    }
}
