using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Windows;
/// <summary>
/// Jordy Amaro 12-01-17 FEI2-4
/// Cambio de interfaz - cuentas asociadas al usuario
/// </summary>
namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para UsuariosCuentas.xaml
    /// </summary>
    public partial class UsuariosCuentas : Window
    {
        string Id = "";
        ReporteEmpresa itemRow;
        List<ReporteEmpresa> lista_reporte;
        clsEntityDeclarant clsEntityDeclarant;
        //Metodo constructor
        public UsuariosCuentas(string Id)
        {
            InitializeComponent();
            this.Id = Id;
            cs_pxCargarEmpresasLista();
            cs_pxCargarEmpresasGrid(Id);
        }
        //Metodo de carga de listado de empresas en el combobox
        private void cs_pxCargarEmpresasLista()
        {
            //Obtener la lista de empresas registradas.
            List<clsEntityDeclarant> empresas = new clsEntityDeclarant().cs_pxObtenerTodo();
            if (empresas.Count > 0 || empresas != null)
            {
                cboEmpresas.ItemsSource = new clsEntityDeclarant().cs_pxObtenerTodo();
                cboEmpresas.DisplayMemberPath = "Cs_pr_RazonSocial";
                cboEmpresas.SelectedValuePath = "Cs_pr_Declarant_Id";
                cboEmpresas.SelectedIndex = 0;
            }
        }
        //Metodo de carga de empresas en la grilla
        private void cs_pxCargarEmpresasGrid(string Id)
        {
            dgEmpresas.ItemsSource = null;
            //Obtener la lista de empresas asociadas al usuario actual.
            List<clsEntityAccount> cuentas = new clsEntityAccount().dgvEmpresasUsuario(Id);
            lista_reporte = new List<ReporteEmpresa>();
            if (cuentas.Count > 0 || cuentas != null)
            {
                //Recorrer las cuentas asociadas al usuario.
                foreach (var item in cuentas)
                {
                    if (item.Cs_pr_Declarant_Id != "")
                    {
                        clsEntityDeclarant = new clsEntityDeclarant().cs_pxObtenerUnoPorId(item.Cs_pr_Declarant_Id);
                        if (clsEntityDeclarant != null)
                        {
                            itemRow = new ReporteEmpresa();
                            itemRow.Id = item.Cs_pr_Account_Id;
                            itemRow.RazonSocial = clsEntityDeclarant.Cs_pr_RazonSocial;
                            lista_reporte.Add(itemRow);
                        }
                       
                    }
                }
                dgEmpresas.ItemsSource = lista_reporte;
            }

        }
     
        ///Metodo guardar la asociacion de las empresas y usuarios
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
           /* var df=dgEmpresas
            ReporteEmpresa item = (ReporteEmpresa)dgEmpresas.SelectedItem;
            if (item != null)
            {*/
                string declarant = cboEmpresas.SelectedValue.ToString();
                if (new clsEntityAccount().dgvCuentaDuplicada(Id, declarant) == false)
                {
                    clsEntityAccount cuenta = new clsEntityAccount();
                    cuenta.Cs_pr_Account_Id = Guid.NewGuid().ToString();
                    cuenta.Cs_pr_Declarant_Id = declarant;
                    cuenta.Cs_pr_Users_Id = Id;
                    cuenta.cs_pxInsertar(false);
                    dgEmpresas.ItemsSource = null;
                    List<clsEntityModulo> modulos = new clsEntityModulo().cs_pxObtenerTodo();
                    if (modulos != null)
                    {
                       foreach (clsEntityModulo item in modulos)
                       {
                        clsEntityPermisos permiso = new clsEntityPermisos();
                        permiso.Cs_pr_Permisos_Id = Guid.NewGuid().ToString();
                        permiso.Cs_pr_Modulo = item.Cs_pr_Modulo_Id;
                        permiso.Cs_pr_Cuenta = cuenta.Cs_pr_Account_Id;
                        permiso.Cs_pr_Permitido = "0";
                        permiso.cs_pxInsertar(false);
                       }
                    }
                    cs_pxCargarEmpresasLista();
                    cs_pxCargarEmpresasGrid(Id);
                }
           // }
        }
        //Metodo remover asociacion empresas de usuario.
        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            ReporteEmpresa item = (ReporteEmpresa)dgEmpresas.SelectedItem;
            if (item != null)
            {
               clsEntityUsers user= new clsEntityUsers().cs_pxObtenerUnoPorId(Id);
                if (user.Cs_pr_Role_Id.ToUpper() != "ADMIN")
                {
                    clsEntityAccount cuenta = new clsEntityAccount().cs_fxObtenerUnoPorId(item.Id);
                    new clsEntityPermisos().cs_pxEliminarPermisos(cuenta.Cs_pr_Account_Id);
                    cuenta.cs_pxElimnar(false);
                    dgEmpresas.ItemsSource = null;
                    cs_pxCargarEmpresasLista();
                    cs_pxCargarEmpresasGrid(Id);
                }
                else
                {
                    if (user.Cs_pr_User != "admin")
                    {
                        clsEntityAccount cuenta = new clsEntityAccount().cs_fxObtenerUnoPorId(item.Id);
                        new clsEntityPermisos().cs_pxEliminarPermisos(cuenta.Cs_pr_Account_Id);
                        cuenta.cs_pxElimnar(false);
                        dgEmpresas.ItemsSource = null;
                        cs_pxCargarEmpresasLista();
                        cs_pxCargarEmpresasGrid(Id);
                    }
                    else {
                        MessageBox.Show("No se puede eliminar las empresas asociadas al usuario administrador");
                    }
                   
                }
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
        }
    }
}
