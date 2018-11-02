using DataLayer;
using DataLayer.CRUD;
using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.MessageBox;

namespace FEICONT
{
    public partial class Usuarios : Window
    {
        ReadGeneralData readGeneralData =   new ReadGeneralData();
        bool exitoCreacion              =   false;

        public Usuarios()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
        }
        
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            DataTable usuariosByid  =   readGeneralData.GetDataTable("[dbo].[Read_Usuario]", "@IdUsuario", txtUsuario.Text.ToString().Trim());
            if (usuariosByid.Rows.Count > 0)
                System.Windows.MessageBox.Show("Lo sentimos este usuario ya existe.", "Error nombre de usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                if (!string.IsNullOrEmpty(txtUsuario.Text.ToString().Trim()) && !string.IsNullOrEmpty(txtContrasenia.Password.Trim()) && !string.IsNullOrEmpty(txtVerificacionPass.Password.Trim()) )
                {
                    if (txtContrasenia.Password.Trim() == txtVerificacionPass.Password.Trim())
                    {
                        if (txtContrasenia.Password.Trim().Length > 5)
                        {
                            Data_Usuario usuario = new Data_Usuario() { IdUsuario = txtUsuario.Text.ToString().Trim(), Contrasenia = txtContrasenia.Password.Trim(), IdRol = 1 };
                            if (usuario.Alter_Usuario("[dbo].[Create_Usuario]"))
                            {
                                System.Windows.MessageBox.Show($"Has registrado con exito a {txtUsuario.Text.ToString()}.", "Registro correcto",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                                exitoCreacion = true;
                                Close();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show($"Ha ocurrido un error de base de datos, verifique que el servicio " +
                                    $" de SQL Server este activado en 'Servicios => SQLSERVER' clic derecho e INICIAR, sí el error persiste contacte con soporte.", "Error en la base de datos",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                            MessageBox.Show("La cantidad mínima de caracteres para la contraseña es de 6", "Contraseña muy insegura", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                        MessageBox.Show("Las contraseñas no coinciden.", "Error de coincidencia contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                    MessageBox.Show("Antes de continuar debes completar todos los campos.", "Datos incompletos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e) => Close();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg  =   string.Empty;
            MessageBoxResult result;
            if (exitoCreacion == false)
            {
                msg     =   "Aún no se ha podido crear un nuevo usuario, ¿Esta seguro(a) de salir?";
                result  = System.Windows.MessageBox.Show(msg, "No se creó ningún usuario", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    e.Cancel    =   true;   // Sí el usuario no quiere cerrar la aplicación, cancelará el cierre, sino cerrará este único formulario
                else
                {
                    try
                    {
                        Close();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private void txtUsuario_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                MessageBox.Show("No debe incluir espacios en blanco", "Acción no permitida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Handled = false;
            }
        }

        private void txtContrasenia_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                MessageBox.Show("No debe incluir espacios en blanco", "Acción no permitida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Handled = false;
            }
        }

        private void txtVerificacionPass_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                MessageBox.Show("No debe incluir espacios en blanco", "Acción no permitida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Handled = false;
            }
        }
    }
}
