using Extension.Datos;
using System;
using System.Windows;
/// <summary>
/// Jordy Amaro 12-01-17 FEI2-4
///  Cambio de interfaz - Ventana de motivo de baja de comprobante.Sera llamado desde otra interfaz.
/// </summary>
namespace FEICONT
{
    /// <summary>
    /// Lógica de interacción para BajaMotivo.xaml
    /// </summary>
    public partial class BajaMotivo : Window
    {
        string Id;
        clsEntityVoidedDocuments_VoidedDocumentsLine documento;
        private clsEntityDatabaseLocal localDB;
        //Constructor de la clase
        public BajaMotivo(string Id,clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
            this.Id = Id;
            //Obtener los documentos asociados a la comunicacion de baja.
            documento = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB).cs_fxObtenerUnoPorId(this.Id);
            txtMotivo.Text = documento.Cs_tag_VoidReasonDescription;
        }
        //Cierre de Ventana 
        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }
        //Evento Guardar motivo de baja para los documentos.
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            documento.Cs_tag_VoidReasonDescription = txtMotivo.Text.Trim();
            documento.cs_pxActualizar(true,true);
            Close();
        }
    }
}
