using DataLayer;
using DataLayer.CRUD;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class presentacion : Window
    {
        public presentacion()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            await ValidateSystem();
            Close();
        }

        public async Task ValidateSystem()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                InternalAccess internalAccess = new InternalAccess();
                internalAccess.Read_InternalAccess();
                ReadGeneralData readGeneralData = new ReadGeneralData();

                if (!string.IsNullOrEmpty(internalAccess.Servidor) && !string.IsNullOrEmpty(internalAccess.Usuario) && !string.IsNullOrEmpty(internalAccess.Usuario))
                {   // Comprobar que haya registro en db interna, primera vez que se ejecute el sistema, se comprueba que los valores sean correctos
                    string cadena = $"data source={@internalAccess.Servidor}; initial catalog=master; user id={@internalAccess.Usuario}; password={@internalAccess.Contrasenia}; Connection Timeout=50";

                    Connection connection = new Connection();

                    if (!connection.CheckConnection(cadena))   // Falló la conexión con los datos de InternalDB 
                    {   // Actualizar la cadena de conexión mediante formulario
                        MessageBox.Show("No se ha podido realizar la conexión con la base de datos, este problema suele suceder cuando se han DETENIDO los servicios de SqlServer,    " +
                            " para solucionar esto oprima la tecla Windows y escriba 'Servicios', en el panel mostrado busque 'SQL Server (MSSQLSERVER)' haga clic derecho sobre este " +
                            " elemento y pulse INICIAR (el valor de 'MSSQLSERVER' puede variar según la configuración que se haya realizado), si el problema persiste, asegúrese de   " +
                            " verificar que los accesos no se hayan cambiado, para esto pruebe la conexión en el siguiente formulario. ", "Error de conexión a SqlServer",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        ConfigurarConeccionSQL configurarConeccionSQL = new ConfigurarConeccionSQL(true);
                        configurarConeccionSQL.Show();
                    }
                    else // Aquí ya no se tiene problemas con la base de datos
                    {
                        DataTable dataTable = readGeneralData.GetDataTable("[sysfox].[Read_List_DatosFox]");

                        int cantidadLineas = dataTable.Rows.Count;

                        if (cantidadLineas <= 0)
                        {
                            MessageBox.Show("Aún no tienes registrada ninguna empresa, puedes realizar este registro desde tu sistema Contasis.",
                                "Error de conexión con los módulos Contasis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            Application.Current.Shutdown();
                        }
                        else
                        {
                            DataTable dtUsuarios = readGeneralData.GetDataTable("[dbo].[List_Usuarios]");

                            if (dtUsuarios.Rows.Count == 0)
                            {
                                MessageBox.Show("Aún no tienes registrado ningún usuario, registra tu usuario en el siguiente formulario.",
                                "Debe registrar su usuario", MessageBoxButton.OK, MessageBoxImage.Information);
                                Usuarios usuarios = new Usuarios();
                                usuarios.Show();
                            }
                            else
                            {
                                Acceso acceso = new Acceso();
                                acceso.Show();
                            }
                        }
                    }
                }
                else     // En caso de que no haya registro en db interna  
                {   // Comprobar la conexión con el servidor de SqlServer
                    InternalConnection internalConnection = new InternalConnection();
                    string[] valores = internalConnection.GetDataFile();   // Obtenemos los datos del archivo txt

                    if (valores.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(valores[0]) && !string.IsNullOrEmpty(valores[1]) && !string.IsNullOrEmpty(valores[2]))
                        {
                            Connection connection = new Connection();
                            string cadena = $"data source={valores[0]}; initial catalog=master; user id={valores[1]}; password={valores[2]};";
                            if (connection.CheckConnection(cadena))    //  Verificamos que la cadena de conexión con el Servidor de SQL sea correcta
                            {
                                if (internalConnection.Create_DataAccess(valores))  // Sólo si la cadena de conexión es correcta procedemos a registrar en App.config
                                {
                                    Acceso acceso = new Acceso();
                                    acceso.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Se debe configurar de forma manual la conexión a la base de datos.", "Configuración requerida",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                                    ConfigurarConeccionSQL configurarConeccionSQL = new ConfigurarConeccionSQL(false);
                                    configurarConeccionSQL.Show();
                                }
                            }
                        }
                    }
                    else // Sí no es correcto se pide configurar de forma manual (Formulario)
                    {
                        MessageBox.Show("Se debe configurar de forma manual la conexión a la base de datos.", "Configuración requerida", MessageBoxButton.OK, MessageBoxImage.Information);
                        ConfigurarConeccionSQL configurarConeccionSQL = new ConfigurarConeccionSQL(false);
                        configurarConeccionSQL.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Detalle del error {ex}.", "No se puede iniciar la aplicación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}
