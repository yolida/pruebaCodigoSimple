using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para AcercaDe.xaml
    /// </summary>
    public partial class AcercaDe : Window
    {
        public AcercaDe(string versionCompilado, string buildCompilado)
        {
            InitializeComponent();
            version.Content = " Versión: "+versionCompilado;
            Build.Content = " Build: " + buildCompilado;
           // System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
           // FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
           // string build = fvi.FileVersion;

           // Version versionss = Assembly.GetEntryAssembly().GetName().Version;
           // version.Content = "Versión: " + versionss;
           // Build.Content = "Build: " + build;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.Escape);
            if (compare == 0)
            {
                Close();
            }
        }
    }
}
