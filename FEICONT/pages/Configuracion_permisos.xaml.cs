using FEICONT.ayuda;
using Extension.Base;
using Extension.Datos;
//using FEICONT.ProgressDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Configuracion_permisos.xaml
    /// </summary>
    public partial class Configuracion_permisos : Page
    {
        private List<ReportePermiso> lista_permisos;
        private List<clsEntityAccount> lista_cuentas;
        private clsEntityUsers usuario;
        private clsEntityAccount cuenta;
        private clsEntityPermisos entidad_permiso;
        private ReportePermiso permiso;
        private string idEmpresa;
        private Window padre;
        delegate bool D();
        public Configuracion_permisos(string id_empresa, Window parent)
        {
            InitializeComponent();
            idEmpresa = id_empresa;
            padre = parent;
            cs_pxCargarUsuariosLista();
           // cs_pxCargarEmpresasGrid(Id);
        }
        //Metodo de carga de listado de empresas en el combobox
        private void cs_pxCargarUsuariosLista()
        {
            //Obtener la lista de empresas registradas.
            List<clsEntityAccount> usuarios_empresa = new clsEntityAccount().cs_pxListaUsuariosPorEmpresa(idEmpresa);
            List<clsEntityUsers> usuarios = new clsEntityUsers().cs_pxObtenerLista();
            lista_permisos = new List<ReportePermiso>();
            if (usuarios_empresa.Count > 0 || usuarios_empresa != null)
            {
                lista_cuentas = new List<clsEntityAccount>();
                foreach (clsEntityAccount item in usuarios_empresa)
                {
                    if (item.Cs_pr_Users_Id.Equals("01") == false && item.Cs_pr_Users_Id.Equals("02") == false )
                    {
                        usuario = new clsEntityUsers().cs_pxObtenerUnoPorId(item.Cs_pr_Users_Id);

                        if (usuario != null)
                        {
                            cuenta = new clsEntityAccount();
                            cuenta.Cs_pr_Account_Id = item.Cs_pr_Account_Id;
                            cuenta.Cs_pr_Declarant_Id = item.Cs_pr_Declarant_Id;
                            cuenta.Cs_pr_Users_Id = usuario.Cs_pr_User;
                            lista_cuentas.Add(cuenta);
                        }
                    }                                    
                }

                cboUsuarios.ItemsSource = lista_cuentas;
                cboUsuarios.DisplayMemberPath = "Cs_pr_Users_Id";
                cboUsuarios.SelectedValuePath = "Cs_pr_Account_Id";
                cboUsuarios.SelectedIndex = 0;
            }
        }
        private void cs_pxCargarPermisos()
        {
            lista_permisos = new List<ReportePermiso>();
            dgUsuarios.ItemsSource = null;
            dgUsuarios.Items.Clear();
            string cuenta = cboUsuarios.SelectedValue.ToString();
            if (cuenta != "")
            {
                //buscar todos los modulos de la base de datos
                List<clsEntityModulo> modulos = new clsEntityModulo().cs_pxObtenerTodo();
                if (modulos != null)
                {
                   

                    foreach (clsEntityModulo item in modulos)
                    {
                        permiso = new ReportePermiso();
                        //buscar permiso por cuenta y modulo
                        entidad_permiso = new clsEntityPermisos().cs_pxObtenerPorCodigoCuenta(item.Cs_pr_Modulo_Id,cuenta);
                        if (entidad_permiso != null)
                        {
                            permiso.Id = entidad_permiso.Cs_pr_Permisos_Id;                       
                            permiso.Modulo = item.Cs_pr_Modulo;
                            permiso.ModuloPadre = item.Cs_pr_Modulo_Padre;                        
                            permiso.Permitido = entidad_permiso.Cs_pr_Permitido;
                            if (permiso.Permitido == "1")
                            {
                                permiso.Check = true;
                            }
                            else
                            {
                                permiso.Check = false;
                            }
                        }else
                        {
                            permiso.Id = Guid.NewGuid().ToString();
                            permiso.Check  = false;
                            permiso.Modulo = item.Cs_pr_Modulo;
                            permiso.ModuloPadre = item.Cs_pr_Modulo_Padre;
                            permiso.Usuario = item.Cs_pr_Modulo_Id;
                            permiso.Permitido = cuenta;
                        }

                        lista_permisos.Add(permiso);

                    }
                }             
                dgUsuarios.ItemsSource = lista_permisos;
            }
        }
        private void actualizarPermisos(string IdCuenta)
        {
            //eliminar
            new clsEntityPermisos().cs_pxEliminarPermisos(IdCuenta);                 
            //asociar modulos a usuario
            List<clsEntityModulo> modulos= new clsEntityModulo().cs_pxObtenerTodo();
            if (modulos != null)
            {
                foreach(clsEntityModulo item in modulos)
                {
                    clsEntityPermisos permiso = new clsEntityPermisos();
                    permiso.Cs_pr_Permisos_Id = Guid.NewGuid().ToString();
                    permiso.Cs_pr_Modulo = item.Cs_pr_Modulo_Id;
                    permiso.Cs_pr_Cuenta = IdCuenta;
                    permiso.Cs_pr_Permitido = "0";
                    permiso.cs_pxInsertar(false);
                }
            }
          
        }
        private void chkDiscontinue_Checked(object sender, RoutedEventArgs e)
        {
            //Obtner elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccionado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReportePermiso doc = (ReportePermiso)dataGridRow.DataContext;
            if ((bool)checkBox.IsChecked)
            {
                doc.Check = true;
            }
            e.Handled = true;
        }
        //Evento Uncheck de los items en la grilla.
        private void chkDiscontinue_Unchecked(object sender, RoutedEventArgs e)
        {
            //Obtener elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener objeto asociado a elemento seleccinado
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            ReportePermiso doc = (ReportePermiso)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked == false)
            {
                doc.Check = false;
            }
            e.Handled = true;
        }

        private void cboUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cs_pxCargarPermisos();
        }
        void AsignarPermisos()
        {
           try
           {
               clsEntityPermisos permiso;
               if (lista_permisos.Count > 0)
               {
                   //leer permisos asignados y modificar  
                   foreach (ReportePermiso item in lista_permisos)
                   {
                       if (item.Check == true)
                       {
                           permiso = new clsEntityPermisos().cs_pxObtenerUnoPorId(item.Id);
                           if (permiso != null)
                           {
                               permiso.Cs_pr_Permitido = "1";
                               permiso.cs_pxActualizar(false);
                           }
                           else
                           {
                               clsEntityPermisos nuevo = new clsEntityPermisos();
                               nuevo.Cs_pr_Permisos_Id = item.Id;
                               nuevo.Cs_pr_Modulo = item.Usuario;
                               nuevo.Cs_pr_Cuenta = item.Permitido;
                               nuevo.Cs_pr_Permitido = "1";
                               nuevo.cs_pxInsertar(false);
                           }
                       }
                       else
                       {
                           permiso = new clsEntityPermisos().cs_pxObtenerUnoPorId(item.Id);
                           if (permiso != null)
                           {
                               permiso.Cs_pr_Permitido = "0";
                               permiso.cs_pxActualizar(false);
                           }
                           else
                           {
                               clsEntityPermisos nuevo = new clsEntityPermisos();
                               nuevo.Cs_pr_Permisos_Id = item.Id;
                               nuevo.Cs_pr_Modulo = item.Usuario;
                               nuevo.Cs_pr_Cuenta = item.Permitido;
                               nuevo.Cs_pr_Permitido = "0";
                               nuevo.cs_pxInsertar(false);
                           }
                       }
                   }

                   MessageBox.Show("Asignación exitosa.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Asterisk);
               }
               else
               {
                   MessageBox.Show("Debe seleccionar un usuario", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Warning);
               }

           }
           catch (Exception ex)
           {
               clsBaseLog.cs_pxRegistarAdd("permisos->" + ex.ToString());
               MessageBox.Show("Ha ocurrido un error al guardar los cambios. Para mayor informacion revise el archivo de errores.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
           }   

        }
        private void btnAsignar_click(object sender, RoutedEventArgs e)
        {
            ProgressDialogResult result = ProgressWindow.Execute(padre, "Procesando...", () => {
                AsignarPermisos();
            });
        }

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_permisos.Count > 0)
                {
                    //checkall
                    foreach (ReportePermiso item in lista_permisos)
                    {
                        item.Check = true;
                    }
                    dgUsuarios.ItemsSource = null;
                    dgUsuarios.Items.Clear();
                    dgUsuarios.ItemsSource = lista_permisos;
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
                if (lista_permisos.Count > 0)
                {
                    ////uncheckall
                    foreach (ReportePermiso item in lista_permisos)
                    {
                        item.Check = false;
                    }
                    dgUsuarios.ItemsSource = null;
                    dgUsuarios.Items.Clear();
                    dgUsuarios.ItemsSource = lista_permisos;
                }
            }
            catch
            {

            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("25");
                ayuda.ShowDialog();
            }
        }
    }
}
