using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using DataLayer;
using DataLayer.CRUD;
using FEICONT.ayuda;

namespace FEICONT
{
    /// <summary>
    /// Interaction logic for Registro_Empresa_Declarante.xaml
    /// </summary>
    public partial class Registro_Empresa_Declarante : Window
    {
        ReadGeneralData readGeneralData =   new ReadGeneralData();
        List<EmpresaDeclarante> empresaDeclarantes  =   new List<EmpresaDeclarante>();
        Data_Usuario data_Usuario   =   new Data_Usuario();
        public Registro_Empresa_Declarante(Data_Usuario usuario)
        {
            InitializeComponent();
            data_Usuario = usuario;
        }

        public class EmpresaDeclarante
        {
            public Int16      _IdDatosFox     { get; set; }
            public string   _NombreModulo   { get; set; }
            public string   _CodigoEmpresa  { get; set; }
            public int      _IdEmisor       { get; set; }
            public string   _NroDocumento   { get; set; }
            public string   _NombreLegal    { get; set; }
            public bool     _Selectable      { get; set; }
        }

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            int cantidadSeleccionados   =   0;
            EmpresaDeclarante empresaSeleccionada   =   new EmpresaDeclarante();

            foreach (var empresaDeclarante in empresaDeclarantes)
            {
                if (empresaDeclarante._Selectable == true)
                    cantidadSeleccionados++;
            }

            switch (cantidadSeleccionados)
            {
                case 0:
                    MessageBox.Show("Debe seleccionar una empresa.", "Ninguna selección detectada", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    foreach (var empresaDeclarante in empresaDeclarantes)
                    {
                        if (empresaDeclarante._Selectable == true)
                        {
                            DataTable dataTable     =   readGeneralData.GetDataTable("[dbo].[Verify_User_Empresa]", "@IdDatosFox", 
                                empresaDeclarante._IdDatosFox, "@IdUsuario", data_Usuario.IdUsuario);

                            if (dataTable.Rows.Count > 0)
                            {
                                MessageBox.Show(
                                    $" Ya has registrado datos de acceso a Sunat con esta empresa, y con este mismo usuario: {data_Usuario.IdUsuario}, " +
                                    $" en caso de querer actualizar estos datos, debe hacerlo directamente en la opción de 'Configuración de sistema ->" +
                                    $" Información del declarante' seleccione la empresa, y pulse 'Actualizar accesos sunat'",
                                    "Duplicado de empresa", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                Declarante declarante   =   new Declarante(empresaDeclarante._IdEmisor, empresaDeclarante._IdDatosFox, data_Usuario);
                                declarante.Show();
                            }
                            break;  // Bandera al encontrar la empresa seleccionada
                        }
                    }
                    break;
                default:    // Mayor a una selección
                    MessageBox.Show("Debe seleccionar sólo una empresa a la vez.", "Más de una selección detectada", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
            }
        }

        public List<EmpresaDeclarante> GetEmpresas()
        {
            dgEmpresas.ItemsSource  =   null;
            dgEmpresas.Items.Clear();
            
            DataTable dataTable =   readGeneralData.GetDataTable("[sysfox].[Read_List_DatosFox]");  // Obtener SOLO la lista de empresas de DatosFox
            DataRow row;

            EmpresaDeclarante empresaDeclarante;

            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    row = dataTable.Rows[i];
                    empresaDeclarante   = new EmpresaDeclarante() {
                        _Selectable     =   false,
                        _IdDatosFox     =   Convert.ToInt16(row["IdDatosFox"].ToString()),
                        _NombreModulo   =   row["NombreModulo"].ToString(),
                        _CodigoEmpresa  =   row["CodigoEmpresa"].ToString(),
                        _IdEmisor       =   Convert.ToInt32(row["IdEmisor"].ToString()),
                        _NroDocumento   =   row["NroDocumento"].ToString(),
                        _NombreLegal    =   row["NombreLegal"].ToString()
                    };
                    empresaDeclarantes.Add(empresaDeclarante);
                }
            }

            return empresaDeclarantes;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e) => Close();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dgEmpresas.ItemsSource = null;
            dgEmpresas.Items.Clear();
            dgEmpresas.ItemsSource = GetEmpresas();
        }

        private void chbEmpresaPorCelda_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox                   =   (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow             =   VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            EmpresaDeclarante empresaDeclarante =   (EmpresaDeclarante)dataGridRow.DataContext;
            empresaDeclarante._Selectable       =   true;
        }

        private void chbEmpresaPorCelda_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox                   =   (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow             =   VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            EmpresaDeclarante empresaDeclarante =   (EmpresaDeclarante)dataGridRow.DataContext;
            empresaDeclarante._Selectable       =   false;
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
    }
}
