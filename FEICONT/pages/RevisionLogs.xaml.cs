using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLayer.CRUD;

namespace FEICONT.pages
{
    /// <summary>
    /// Interaction logic for RevisionLogs.xaml
    /// </summary>
    public partial class RevisionLogs : Page
    {
        Data_Usuario data_Usuario   =   new Data_Usuario();
        private readonly IGetDataAsync _data_Log;
        List<Data_Log> logs =   new List<Data_Log>();

        public RevisionLogs(Data_Usuario usuario)
        {
            InitializeComponent();
            data_Usuario    =   usuario;

            Data_Log data_Log   =   new Data_Log();
            _data_Log           =   (IGetDataAsync)data_Log;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datePick_inicio.Text    =   DateTime.Now.Date.ToString();
            datePick_fin.Text       =   DateTime.Now.Date.AddDays(1).ToString();

            LoadGrid();
        }

        public async void LoadGrid()
        {
            dgLogs.ItemsSource    =   null;
            dgLogs.Items.Clear();

            try
            {
                Mouse.OverrideCursor    =   Cursors.Wait;
                logs                    =   await GetLogs();
                dgLogs.ItemsSource      =   logs;

                btnDescargarLog.IsEnabled   =   false;  // hasta habilitarlo
                btnEliminar.IsEnabled       =   false;
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

        public async Task<List<Data_Log>> GetLogs()
        {
            var data_Logs = new List<Data_Log>();
            try
            {
                Data_Log data_Log   =   new Data_Log() {
                    Start_FechaRegistro =   DateTime.Parse(datePick_inicio.SelectedDate.ToString()),
                    End_FechaRegistro   =   DateTime.Parse(datePick_fin.SelectedDate.ToString()),
                    IdUser_Empresa      =   data_Usuario.IdUser_Empresa
                };
                GetDataAsync getDataAsync   = await _data_Log.Read_List_Log(data_Log);
                data_Logs   =   getDataAsync.Data_Logs;
            }
            catch (Exception)
            {
                data_Logs = new List<Data_Log>();
            }

            return data_Logs;
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCell_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCell_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e) => LoadGrid();
    }
}
