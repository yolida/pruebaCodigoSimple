using System;
using System.IO;
using System.Windows;
using DataLayer.CRUD;
using Microsoft.Win32;

namespace FEICONT
{
    public partial class Declarante : Window
    {
        public int IdEmisor;
        public Int16 Id_DatosFox;
        public Data_Usuario data_Usuario   =   new Data_Usuario();
        public Declarante(int idEmisor, Int16 idDatosFox, Data_Usuario usuario)
        {
            InitializeComponent();
            IdEmisor        =   idEmisor;
            Id_DatosFox     =   idDatosFox;
            data_Usuario    =   usuario;
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
            if (!string.IsNullOrEmpty(txtCertificadoDigital.Text.ToString().Trim()) && !string.IsNullOrEmpty(txtCertificadoDigitalClave.Password.ToString().Trim()) &&
                !string.IsNullOrEmpty(txtUsuarioSol.Text.ToString().Trim()) && !string.IsNullOrEmpty(txtClaveSol.Password.ToString().Trim()))
            {
                string claveSol     =   string.Empty;
                string usuarioSol   =   string.Empty;
                string certificadoDigital   =   string.Empty;
                if (txtUsuarioSol.Text.ToString().Trim().ToUpper().Equals("MODDATOS"))
                {
                    usuarioSol  =   txtUsuarioSol.Text.ToString().Trim().ToUpper();
                    claveSol    =   txtClaveSol.Password.ToString().Trim().ToUpper();
                }
                else
                {
                    usuarioSol  =   txtUsuarioSol.Text.ToString().Trim();
                    claveSol    =   txtClaveSol.Password.ToString().Trim();
                }

                bool errorCertificado   =   false;
                try
                {
                    certificadoDigital  =   Convert.ToBase64String(File.ReadAllBytes(txtCertificadoDigital.Text.ToString().Trim()));
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puede leer el certificado digital.", "Archivo irreconosible", MessageBoxButton.OK, MessageBoxImage.Information);
                    errorCertificado    =   true;
                }

                if (errorCertificado == false)
                {
                    Data_AccesosSunat data_AccesosSunat = new Data_AccesosSunat(IdEmisor)
                    {
                        CertificadoDigital  =   certificadoDigital,
                        ClaveCertificado    =   txtCertificadoDigitalClave.Password.ToString().Trim(),
                        UsuarioSol          =   usuarioSol,
                        ClaveSol            =   claveSol,
                        IdDatosFox          =   Id_DatosFox,
                        IdUsuario           =   data_Usuario.IdUsuario
                    };

                    if (data_AccesosSunat.Create_AccesosSunat())
                    {
                        MessageBox.Show("Los datos se han registrado con éxito, para ver los cambios es necesario reiniciar FEICONT.",
                            "Registro exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                        try
                        {
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            //System.Windows.Forms.Application.Restart();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Tenemos problemas para reiniciar, por favor cierre y vuelva a abrir la aplicación.",
                                "Es necesario reiniciar la aplicación", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un problema en el registro de datos, contacte con soporte.", "Registro fallido", MessageBoxButton.OK, MessageBoxImage.Error);
                        Data_Log data_Log = new Data_Log()
                        {
                            DetalleError    =   $"No se registro el usuario",
                            Comentario      =   "Ha ocurrido un problema en el registro de datos",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }
                }
            }
            else
                MessageBox.Show("Antes de continuar debe rellenar todos los campos.", "Campos en blanco", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog   =   new OpenFileDialog();
            openFileDialog.Multiselect      =   false;
            openFileDialog.Filter           =   "Certificados|*.pfx|Todos los archivos|*.*";
            openFileDialog.DefaultExt       =   "*.pfx";
            openFileDialog.Title            =   "Selección de certificado digital";

            Nullable<bool> dialogOk   =   openFileDialog.ShowDialog();

            if (dialogOk  == true)
            {
                string  ruta    =   openFileDialog.FileName;
                txtCertificadoDigital.Text = ruta;
            }
        }
    }
}
