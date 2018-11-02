using System.Data;
using System.Windows;
using DataLayer;
using DataLayer.CRUD;
using System.ComponentModel;
using System;
using System.Windows.Input;

namespace FEICONT
{
    /// <summary>
    /// Interaction logic for Acceso.xaml
    /// </summary>
    public partial class Acceso : Window
    {
        public Acceso()
        {
            InitializeComponent();
        }

        ReadGeneralData readGeneralData =   new ReadGeneralData();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e) => Ingresar();

        public void Ingresar()
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text.ToString().Trim()) && !string.IsNullOrEmpty(txtPassword.Password.ToString().Trim()))
            {
                Data_Usuario data_Usuario   =   new Data_Usuario() { IdUsuario  =   txtUsuario.Text.ToString().Trim(), Contrasenia  =   txtPassword.Password.ToString().Trim() };
                if (data_Usuario.Security_Authenticate_Usuario())
                {
                    MainWindow mainWindow   =   new MainWindow(Int32.Parse(lstEmpresas.SelectedValue.ToString()), txtUsuario.Text.ToString());
                    mainWindow.Show();
                    try
                    {
                        Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                    MessageBox.Show("Los datos ingresados son incorrectos, vuelva a intentarlo.", "Error de autenticación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Complete los campos antes de continuar.", "Campos en blanco", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void txtUsuario_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    txtPassword.Focus();
                else
                {
                    DataTable dataTable = readGeneralData.GetDataTable("[dbo].[Read_User_Empresa]", "@IdUsuario", txtUsuario.Text.ToString().Trim());

                    lstEmpresas.ItemsSource         = null;
                    lstEmpresas.Items.Clear();

                    if (dataTable.Rows.Count < 1)
                    {
                        DataRow row     =   dataTable.NewRow();
                        row["NombreLegal"]  =   "Sin empresa";
                        row["IdAccesosSunat"]   =   0;
                        dataTable.Rows.Add(row);
                    }
                    else
                        txtPassword.Focus();

                    var items = (dataTable as IListSource).GetList(); //  Lista de empresas

                    lstEmpresas.ItemsSource         = items;
                    lstEmpresas.DisplayMemberPath   = "NombreLegal";
                    lstEmpresas.SelectedValuePath   = "IdAccesosSunat";
                    lstEmpresas.SelectedIndex       = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Detalle del error: {ex}", "A ocurrido un error inesperado", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios   =   new Usuarios();
            usuarios.Show();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Ingresar();
            }
        }
    }
}
