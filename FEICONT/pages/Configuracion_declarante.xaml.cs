using FEICONT.ayuda;
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
using DataLayer;
using System.Data;
using System.ComponentModel;
using DataLayer.CRUD;

namespace FEICONT.pages
{
    public partial class Configuracion_declarante : Page
    {
        ReadGeneralData readGeneralData =   new ReadGeneralData();
        Data_Usuario data_Usuario = new Data_Usuario();

        public Configuracion_declarante(Data_Usuario usuario)
        {
            InitializeComponent();
            data_Usuario    =   usuario;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await GetAccessosSunat();
        }

        //Metodo para cargar el grid de declarantes
        private async Task GetAccessosSunat()
        {
            dgEmpresasRegistradas.ItemsSource = null;
            dgEmpresasRegistradas.Items.Clear();

            DataTable dataTable =   readGeneralData.GetDataTable("[dbo].[Read_List_User_Empresa]");

            if (dataTable.Rows.Count > 0)
            {
                var items   =   (dataTable as IListSource).GetList();
                dgEmpresasRegistradas.ItemsSource  =   items;
            }   
        }
        //Evento click para crear nuevo declarante.
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Registro_Empresa_Declarante registro_Empresa_Declarante = new Registro_Empresa_Declarante(data_Usuario);
            registro_Empresa_Declarante.ShowDialog();
            if (registro_Empresa_Declarante.DialogResult.HasValue && registro_Empresa_Declarante.DialogResult.Value)
            {
                //cargarDataGrid();
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("23");
                ayuda.ShowDialog();
            }
        }


        private void btnElimnar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnUpdateEmisor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdateAccesoSunat_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
