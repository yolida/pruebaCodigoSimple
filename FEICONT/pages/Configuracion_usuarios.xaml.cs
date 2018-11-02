using FEICONT.ayuda;
using Extension.Datos;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Configuracion de informacion usuarios.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Configuracion_usuarios.xaml
    /// </summary>
    public partial class Configuracion_usuarios : Page
    {
        List<List<string>> entidades;
        List<ReporteUsuario> lista_reporte;
        ReporteUsuario itemRow;
        //Metodo constructor
        public Configuracion_usuarios()
        {
            InitializeComponent();
        }
        //Evento click para mostrar empresas del usuario.
        private void btnEmpresas_Click(object sender, RoutedEventArgs e)
        {
            //Obtener el usuario seleccionado.
            ReporteUsuario item = (ReporteUsuario)dgUsuarios.SelectedItem;
            //Si existe el item seleccionado.
            if (item != null)
            {               
                UsuariosCuentas Formulario = new UsuariosCuentas(item.Id);
                Formulario.ShowDialog();
                if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
                {
                    cargarDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un usuario.");
            }            
        }
        //Evento click para eliminar un usuario
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //Obtener el usuario seleccionado.
            //ReporteUsuario item = (ReporteUsuario)dgUsuarios.SelectedItem;
            ////Si existe el item seleccionado.
            //if (item != null)
            //{          
            //    Usuarios Formulario = new Usuarios("DLT",item.Id);
            //    Formulario.ShowDialog();
            //    if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
            //    {
            //        cargarDataGrid();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Debe seleccionar un usuario");
            //}
        }
        //Evento click para modificar la informacion del usuario.
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            ////Obtener el usuario seleccionado.
            //ReporteUsuario item = (ReporteUsuario)dgUsuarios.SelectedItem;
            ////Si existe el item seleccionado.
            //if (item != null)
            //{
            //    Usuarios Formulario = new Usuarios("UPD",item.Id);
            //    Formulario.ShowDialog();
            //    if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
            //    {
            //        cargarDataGrid();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Debe seleccionar un usuario");
            //}
        }
        //Evento click para crear nuevo usuario.
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            //Cargar formulario de usuarios.
            //Usuarios Formulario = new Usuarios("INS", "");
            //Formulario.ShowDialog();
            //if (Formulario.DialogResult.HasValue && Formulario.DialogResult.Value)
            //{
            //    cargarDataGrid();
            //}
        }
        //Metodo para cargar informacion del grid principal
        private void cargarDataGrid()
        {
            //Limpiar la grilla.
            dgUsuarios.ItemsSource = null;
            dgUsuarios.Items.Clear();
            //Obtener todos los usuarios.
            entidades = new clsEntityUsers().cs_pxObtenerTodo();
            //Recorre los usiarios para rellenar la grilla
            lista_reporte = new List<ReporteUsuario>();
          
            if (entidades.Count > 0)
            {
                foreach (var item in entidades)
                {
                    if (item[3].ToString().Trim() != "MASTER")
                    {
                        itemRow = new ReporteUsuario();
                        itemRow.Id = item[0].ToString().Trim();
                        itemRow.Usuario = item[1].ToString().Trim();
                        itemRow.Contrasenia = item[2].ToString().Trim();
                        itemRow.Rol = item[3].ToString().Trim();
                        lista_reporte.Add(itemRow);
                    }                  
                }
                dgUsuarios.ItemsSource = lista_reporte;
                btnModificar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
            }
            else
            {
                btnModificar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
            }

        }
        //Evento de carga de la ventan principal
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cargarDataGrid();
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("24");
                ayuda.ShowDialog();
            }
        }
    }
}
