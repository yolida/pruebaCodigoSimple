using FEICONT.classes;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para CertificadoDigital.xaml
    /// </summary>
    public partial class CertificadoDigital : Page
    {
        public CertificadoDigital()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Certificado> certs = new List<Certificado>();
            Certificado cert = null;
            List<clsEntityDeclarant> declarantes =  new clsEntityDeclarant().cs_pxObtenerTodos();
            foreach (clsEntityDeclarant dec in declarantes)
            {


                try
                {
                    X509Certificate2 certi = new X509Certificate2(File.ReadAllBytes(dec.Cs_pr_Rutacertificadodigital), dec.Cs_pr_Parafrasiscertificadodigital);

                    cert = new Certificado();
                    cert.Id = dec.Cs_pr_Declarant_Id;
                    cert.Serie = certi.GetSerialNumberString();
                    cert.ValidoDesde = certi.NotBefore.ToShortDateString();
                    cert.ValidoHasta = certi.NotAfter.ToShortDateString();
                    cert.AlertaDias = dec.Cs_pr_Alerta_Dias;
                    certs.Add(cert);
                }
                catch (Exception)
                {

                }



            }

            dgCertificados.ItemsSource = certs;
        }

        private void dgCertificados_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGridRow item = e.Row;
            Certificado comprobante = (Certificado)item.DataContext;
            clsEntityDeclarant dec = new clsEntityDeclarant().cs_pxObtenerUnoPorId(comprobante.Id);
            dec.Cs_pr_Alerta_Dias = comprobante.AlertaDias;
            dec.cs_pxActualizar(false);
        }
    }
}
