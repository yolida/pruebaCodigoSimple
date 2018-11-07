using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using DataLayer.CRUD;

namespace FEICONT.pages
{
    /// <summary>
    /// Interaction logic for PromtMotivoBaja.xaml
    /// </summary>
    public partial class PromtMotivoBaja : Window
    {
        string IdDocumento = string.Empty;
        Data_Usuario data_Usuario;
        public PromtMotivoBaja(Data_Usuario usuario, string idDocumento)
        {
            InitializeComponent();
            IdDocumento     =   idDocumento;
            data_Usuario    =   usuario;
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                Mouse.OverrideCursor    =   Cursors.Wait;
                Data_Documentos data_Documento = new Data_Documentos(IdDocumento);   //  IdDocumento variable global
                data_Documento.Read_Documento();

                Data_CabeceraDocumento cabeceraDocumento    =   new Data_CabeceraDocumento(data_Documento.IdCabeceraDocumento);
                cabeceraDocumento.Read_CabeceraDocumento();

                Data_Contribuyente Receptor =   new Data_Contribuyente(cabeceraDocumento.IdReceptor);
                Receptor.Read_Contribuyente();

                lblTipoDocumento.Content    =   data_Documento.TDDescripcion;

                if (!string.IsNullOrEmpty(cabeceraDocumento.FechaEmision.ToString()))
                    lblFechaRecepcion.Content   =   cabeceraDocumento.FechaEmision.ToString();
                else
                    lblFechaRecepcion.Content   =   "----/--/--";

                if (!string.IsNullOrEmpty(data_Documento.FechaEmisionSUNAT.ToString()))
                    lblFechaEmision.Content =   data_Documento.FechaEmisionSUNAT.ToString();
                else
                    lblFechaEmision.Content =   "----/--/--";

                lblCliente.Content          =   Receptor.NombreComercial;
                lblTotal.Content            =   cabeceraDocumento.ImporteTotalVenta.ToString() + " " + cabeceraDocumento.DescripcionMoneda;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ha ocurrido un error al cargar los registros, detalle del error: {ex}",
                    "Error al cargar los datos", MessageBoxButton.OK, MessageBoxImage.Error);
                var msg = string.Concat(ex.InnerException?.Message, ex.Message);
                Data_Log data_Log = new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al enviar el documento a sunat desde la interfaz",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
            finally {
                Mouse.OverrideCursor = null;
            }
        }

        private void linkSunatComunicado_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e) => Close();

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Data_Documentos data_Documento  =   new Data_Documentos() { IdDocumento = IdDocumento,   MotivoBaja = txtDetalleMotivoBaja.Text.ToString() };
            if (!data_Documento.Update_Documento_OneColumn("[dbo].[Update_Documento_MotivoBaja]"))
            {
                Data_Log data_Log = new Data_Log()
                {
                    DetalleError    =   "Error al actualizar el motivo de baja del documento",
                    Comentario      =   "Error con la base de datos: [dbo].[Update_Documento_MotivoBaja]",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
            else
                Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
