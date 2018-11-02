using FEICONT.ayuda;
using Extension.Base;
using System.Windows;
using System.Windows.Forms;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para Licencia.xaml
    /// </summary>
    public partial class Licencia : Window
    {
        private string codigoPeticion;
        private string fecha;
        private string tipoLicencia;
        private string codigoActivacion;
        private string version;
        public Licencia()
        {
            InitializeComponent();
        }

        public Licencia(string CcodigoActivacion,string Cfecha, string CtipoLicencia, string CcodigoPeticion,string versionLicencia)
        {
            InitializeComponent();
            this.fecha = Cfecha;
            this.tipoLicencia = new clsBaseLicencia().getTipoLicencia(CtipoLicencia);
            this.codigoPeticion = CcodigoPeticion;
            this.codigoActivacion = CcodigoActivacion;
            this.version = versionLicencia;
            lblPeticion.Content = codigoPeticion;
            lblFechaVencimiento.Content = fecha;
            lblCodigoActivacion.Content = codigoActivacion;
            TipoLicencia.Content = tipoLicencia;
            lblVersion.Content = version;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            //agregar confirmacion de eliminar licencia
            if (System.Windows.Forms.MessageBox.Show("¿Está seguro que desea remover la licencia?", "¿Está seguro?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            {
                new clsBaseLicencia().removeLicence();
                Close();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
            int compare2 = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare2 == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("30");
                ayuda.ShowDialog();
            }
        }
    }
}
