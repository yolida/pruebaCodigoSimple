using FEICONT.classes;
using Extension.Base;
using Extension.Datos;
using Extension.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Receptor_Compras.xaml
    /// </summary>
    public partial class Receptor_Compras : Page
    {
        clsEntityDatabaseLocal localDB;
        Window VentanaPrincipal;

        DocumentoCompra itemRow;
        List<DocumentoCompra> lista_reporte = new List<DocumentoCompra>();
        List<clsEntidadDocument> registros = new List<clsEntidadDocument>();
        List<clsEntidadDocument_Line> registros_lineas = new List<clsEntidadDocument_Line>();

        List<ComboBoxPares> lista_meses = new List<ComboBoxPares>();
        List<ComboBoxPares> lista_anios = new List<ComboBoxPares>();
        bool periodoCompras = false;
        string fechaInicioFormato = "";
        string fechaFinFormato = "";
        private int faltantes = 0;
        private string Ruc = "";

        public Receptor_Compras(clsEntityDatabaseLocal local, Window parent,string RUC)
        {
            InitializeComponent();
            localDB = local;
            VentanaPrincipal = parent;
            Ruc = RUC;
            DataContext = new ViewModel();
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
                FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch
            {

            }
        }
        /// <summary>
        /// Evento al termino de la edicion de una celda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgDocumentos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

          /*  DataGridRow item = e.Row;
            DocumentoCompra comprobante = (DocumentoCompra)item.DataContext;
            clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(comprobante.Id);
            DateTime datetime;
            bool valid = DateTime.TryParse(comprobante.FechaVencimiento, out datetime);
            if (valid)
            {
                DateTime dt11 = DateTime.Parse(comprobante.FechaVencimiento);
                doc.Cs_cr_FechaVencimiento = dt11.ToString("yyyy-MM-dd");
            }
            else
            {
                doc.Cs_cr_FechaVencimiento = "";
            }
            bool valid2 = DateTime.TryParse(comprobante.DocReferenciaFecha, out datetime);
            if (valid2)
            {
                DateTime dt21 = DateTime.Parse(comprobante.DocReferenciaFecha);
                doc.Cs_cr_DocReferenciaFecha = dt21.ToString("yyyy-MM-dd");
                // doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
            }
            else
            {
                doc.Cs_cr_DocReferenciaFecha = "";
            }
            bool valid21 = DateTime.TryParse(comprobante.ConstDepDetFecha, out datetime);
            if (valid21)
            {
                DateTime dt211 = DateTime.Parse(comprobante.ConstDepDetFecha);
                doc.Cs_cr_ConstDepDetFecha = dt211.ToString("yyyy-MM-dd");
                // doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
            }
            else
            {
                doc.Cs_cr_ConstDepDetFecha = "";
            }
            bool valid3 = DateTime.TryParse(comprobante.FechaVencimientoDos, out datetime);
            if (valid3)
            {
                DateTime dt31 = DateTime.Parse(comprobante.FechaVencimientoDos);
                doc.Cs_cr_FechaVencimientoDos = dt31.ToString("yyyy-MM-dd");
                //doc.Cs_cr_FechaVencimientoDos = comprobante.FechaVencimientoDos;
            }
            else
            {
                doc.Cs_cr_FechaVencimientoDos = "";
            }

            bool valid4 = DateTime.TryParse(comprobante.FechaDocumentoRegimenEspecial, out datetime);
            if (valid4)
            {
                DateTime dt41 = DateTime.Parse(comprobante.FechaDocumentoRegimenEspecial);
                doc.Cs_cr_FechaDocumentoRegimenEspecial = dt41.ToString("yyyy-MM-dd");
                // doc.Cs_cr_FechaDocumentoRegimenEspecial = comprobante.FechaDocumentoRegimenEspecial;
            }
            else
            {
                doc.Cs_cr_FechaDocumentoRegimenEspecial = "";
            }
            if (comprobante.AnioEmisionDUA.Trim().Length == 4)
            {
                doc.Cs_cr_AnioEmisionDUA = comprobante.AnioEmisionDUA;
            }
            else
            {
                doc.Cs_cr_AnioEmisionDUA = "";
            }

            //doc.Cs_cr_AnioEmisionDUA = comprobante.AnioEmisionDUA;
            float f;
            if (float.TryParse(comprobante.AdqGravadasBaseImponibleGravadasNoGravadas.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = comprobante.AdqGravadasBaseImponibleGravadasNoGravadas.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = "";
            }

            if (float.TryParse(comprobante.AdqGravadasIGVGravadasNoGravados.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_AdqGravadasIGVGravadasNoGravados = comprobante.AdqGravadasIGVGravadasNoGravados.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_AdqGravadasIGVGravadasNoGravados = "";
            }

            if (float.TryParse(comprobante.AdqGravadasBaseImponibleNoGravados.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_AdqGravadasBaseImponibleNoGravados = comprobante.AdqGravadasBaseImponibleNoGravados.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_AdqGravadasBaseImponibleNoGravados = "";
            }

            if (float.TryParse(comprobante.AdqGravadasIGVNoGravados.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_AdqGravadasIGVNoGravados = comprobante.AdqGravadasIGVNoGravados.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_AdqGravadasIGVNoGravados = "";
            }

            if (comprobante.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Length > 20)
            {
                doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Substring(0, 20);

            }
            else
            {
                doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado;
            }
            // doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado;
            if (comprobante.ConstDepDetNumero.Trim().Length > 20)
            {
                doc.Cs_cr_ConstDepDetNumero = comprobante.ConstDepDetNumero.Trim().Substring(0, 20);
                faltantes++;
            }
            else
            {
                doc.Cs_cr_ConstDepDetNumero = comprobante.ConstDepDetNumero;
            }

            // doc.Cs_cr_ConstDepDetFecha = comprobante.ConstDepDetFecha;
            if (float.TryParse(comprobante.TipoCambio.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_TipoCambio = comprobante.TipoCambio.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_TipoCambio = "";
            }

            //doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
            if (float.TryParse(comprobante.EquivalenteDolares.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_EquivalenteDolares = comprobante.EquivalenteDolares.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_EquivalenteDolares = "";
            }

            // doc.Cs_cr_FechaVencimientoDos = comprobante.FechaVencimientoDos;
            doc.Cs_cr_CondicionCompra = comprobante.CondicionCompra;


            if (comprobante.CtaContableBaseImponible.Trim().Length > 10)
            {
                doc.Cs_cr_CtaContableBaseImponible = comprobante.CtaContableBaseImponible.Trim().Substring(0, 10);
            }
            else
            {
                doc.Cs_cr_CtaContableBaseImponible = comprobante.CtaContableBaseImponible;
            }

            if (comprobante.CtaContableOtrosTributos.Trim().Length > 10)
            {
                doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos.Trim().Substring(0, 10);
            }
            else
            {
                doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos;
            }

            //doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos;

            if (comprobante.CtaContableTotal.Trim().Length > 10)
            {
                doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal.Trim().Substring(0, 10);
            }
            else
            {
                doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal;
            }

            // doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal;
            if (comprobante.CentroCostosUno.Trim().Length > 9)
            {
                doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno.Trim().Substring(0, 9);
            }
            else
            {
                doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno.Trim();
            }
            // doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno;
            if (comprobante.CentroCostosDos.Trim().Length > 9)
            {
                doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos.Trim().Substring(0, 9);
            }
            else
            {
                doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos.Trim();
            }
            // doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos;

            //doc.Cs_cr_RegimenEspecial = comprobante.RegimenEspecial;
            //doc.Cs_cr_PorcentajeRegimenEspecial = comprobante.PorcentajeRegimenEspecial;
            //doc.Cs_cr_ImporteRegimenEspecial = comprobante.ImporteRegimenEspecial;

            if (comprobante.SerieDocumentoRegimenEspecial.Trim().Length > 6)
            {
                doc.Cs_cr_SerieDocumentoRegimenEspecial = comprobante.SerieDocumentoRegimenEspecial.Trim().Substring(0, 6);
            }
            else
            {
                doc.Cs_cr_SerieDocumentoRegimenEspecial = comprobante.SerieDocumentoRegimenEspecial;
            }

            if (comprobante.NumeroDocumentoRegimenEspecial.Trim().Length > 13)
            {
                doc.Cs_cr_NumeroDocumentoRegimenEspecial = comprobante.NumeroDocumentoRegimenEspecial.Trim().Substring(0, 13);
            }
            else
            {
                doc.Cs_cr_NumeroDocumentoRegimenEspecial = comprobante.NumeroDocumentoRegimenEspecial;
            }


            //doc.Cs_cr_FechaDocumentoRegimenEspecial = comprobante.FechaDocumentoRegimenEspecial;

            if (comprobante.CodigoPresupuesto.Trim().Length > 10)
            {
                doc.Cs_cr_CodigoPresupuesto = comprobante.CodigoPresupuesto.Trim().Substring(0, 10);
            }
            else
            {
                doc.Cs_cr_CodigoPresupuesto = comprobante.CodigoPresupuesto;
            }

            if (float.TryParse(comprobante.PorcentajeIGV.Trim().Replace(',', '.'), out f))
            {
                doc.Cs_cr_PorcentajeIGV = comprobante.PorcentajeIGV.Replace(',', '.');
            }
            else
            {
                doc.Cs_cr_PorcentajeIGV = "";
            }
            if (comprobante.Glosa.Trim().Length > 60)
            {
                doc.Cs_cr_Glosa = comprobante.Glosa.Trim().Substring(0, 60);
            }
            else
            {
                doc.Cs_cr_Glosa = comprobante.Glosa;
            }

            // doc.Cs_cr_Glosa = comprobante.Glosa;
            doc.Cs_cr_CondicionPercepcion = comprobante.CondicionPercepcion;
            //doc.Cs_cr_ImporteRegimenEspecial = comprobante.ImporteRegimenEspecial;

            doc.cs_pxActualizar(false, false);

            //actualizar en los items
            actualizarGrilla();*/
        }
        /// <summary>
        /// Evento de carga de la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //datePick_inicio.Text = DateTime.Now.Date.ToString();
            //datePick_fin.Text = DateTime.Now.Date.ToString();

            lista_meses.Add(new ComboBoxPares("", "Mes"));
            lista_meses.Add(new ComboBoxPares("01", "Enero"));
            lista_meses.Add(new ComboBoxPares("02", "Febrero"));
            lista_meses.Add(new ComboBoxPares("03", "Marzo"));
            lista_meses.Add(new ComboBoxPares("04", "Abril"));
            lista_meses.Add(new ComboBoxPares("05", "Mayo"));
            lista_meses.Add(new ComboBoxPares("06", "Junio"));
            lista_meses.Add(new ComboBoxPares("07", "Julio"));
            lista_meses.Add(new ComboBoxPares("08", "Agosto"));
            lista_meses.Add(new ComboBoxPares("09", "Setiembre"));
            lista_meses.Add(new ComboBoxPares("10", "Octubre"));
            lista_meses.Add(new ComboBoxPares("11", "Noviembre"));
            lista_meses.Add(new ComboBoxPares("12", "Diciembre"));

            periodoMesRCompras.DisplayMemberPath = "_Value";
            periodoMesRCompras.SelectedValuePath = "_key";
            periodoMesRCompras.SelectedIndex = 0;
            periodoMesRCompras.ItemsSource = lista_meses;

            periodoMesRAsignar.DisplayMemberPath = "_Value";
            periodoMesRAsignar.SelectedValuePath = "_key";
            periodoMesRAsignar.SelectedIndex = 0;
            periodoMesRAsignar.ItemsSource = lista_meses;

            int anioActual = DateTime.Now.Year;
            lista_anios.Add(new ComboBoxPares("", "Año"));
            for (int anio = anioActual; anio >= (anioActual - 10); anio--)
            {
                lista_anios.Add(new ComboBoxPares(anio.ToString(), anio.ToString()));
            }
            periodoAnioRCompras.DisplayMemberPath = "_Value";
            periodoAnioRCompras.SelectedValuePath = "_key";
            periodoAnioRCompras.SelectedIndex = 0;
            periodoAnioRCompras.ItemsSource = lista_anios;

            periodoAnioRAsignar.DisplayMemberPath = "_Value";
            periodoAnioRAsignar.SelectedValuePath = "_key";
            periodoAnioRAsignar.SelectedIndex = 0;
            periodoAnioRAsignar.ItemsSource = lista_anios;

            actualizarGrilla("");
        }
        /// <summary>
        /// Metodo para actualizar la grilla
        /// </summary>
        private void actualizarGrilla(string periodo)
        {
            //btnExportar.IsEnabled = false;
            //if (datePick_inicio.SelectedDate != null)
            //{
            //    DateTime fechaInicio = (DateTime)datePick_inicio.SelectedDate;
            //    fechaInicioFormato = fechaInicio.ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    fechaInicioFormato = string.Empty;
            //}
            //if (datePick_fin.SelectedDate != null)
            //{
            //    DateTime fechaFin = (DateTime)datePick_fin.SelectedDate;
            //    fechaFinFormato = fechaFin.ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    fechaFinFormato = string.Empty;
            //}
            ////cargar los docs del paso anterior buscar en bd los estados activos que aun no se han procesado
            //cs_pxCargarDgvComprobanteselectronicos(fechaInicioFormato, fechaFinFormato,periodo);
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            DocumentoCompra obj = e.Row.Item as DocumentoCompra;
            if (obj != null)
            {
                //see obj properties
            }
        }

        /// <summary>
        /// Metodo para cargar los comprobantes en el datagrid de compras
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>

        private void cs_pxCargarDgvComprobanteselectronicos(string fechaInicio, string fechaFin,string periodo)
        {

            faltantes = 0;
            dgDocumentos.ItemsSource = null;
            dgDocumentos.Items.Clear();
            //Obtener los comprobantes de factura.

            if (periodo != "")
            {
                registros = new clsEntidadDocument(localDB).cs_pxObtenerAceptadosValidado("", "", Ruc, "1", periodo);
            }
            else
            {
                registros = new clsEntidadDocument(localDB).cs_pxObtenerAceptadosValidado(fechaInicio, fechaFin, Ruc, "1", periodo);
            }

            //registros = new clsEntidadDocument(localDB).cs_pxObtenerAceptadosValidado(fechaInicio, fechaFin, Ruc);

            lista_reporte = new List<DocumentoCompra>();
            //lista_reporte = new ObservableCollection<Reporte>();
            if (registros != null)
            {
                //Recorrer los registros para llenar la grilla.
                foreach (var item in registros)
                {
                    //obtener las lineas por cada uno y validarlas
                    itemRow = new DocumentoCompra();
                    itemRow.Id = item.Cs_pr_Document_Id;
                    DateTime dt = DateTime.ParseExact(item.Cs_tag_IssueDate, "yyyy-MM-dd", null);
                    itemRow.FechaEmision = dt.ToString("dd/MM/yyyy");
                    if (item.Cs_cr_FechaVencimiento.Trim().Length > 0)
                    {
                        try
                        {
                            DateTime dt2 = DateTime.ParseExact(item.Cs_cr_FechaVencimiento, "yyyy-MM-dd", null);
                            itemRow.FechaVencimiento = dt2.ToString("dd/MM/yyyy").Trim();
                            //itemRow.FechaVencimiento = item.Cs_cr_FechaVencimiento;
                        }
                        catch
                        {

                        }
                    }

                    /* if (itemRow.FechaVencimiento.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/

                    itemRow.Tipo = item.Cs_tag_InvoiceTypeCode;
                    /*  if (itemRow.Tipo.Trim().Length == 0)
                      {
                          faltantes++;
                      }*/
                    /*itemRow.SerieNumero = item.Cs_tag_ID;
                    if (itemRow.SerieNumero.Trim().Length == 0)
                    {
                        faltantes++;
                    }*/

                    itemRow.Serie = item.Cs_tag_ID.Split('-')[0];
                    /* if (itemRow.Serie.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {*/
                    /*if (itemRow.Serie.Length == 4)
                    {
                        itemRow.Serie = "00" + itemRow.Serie;
                    }*/
                    //comentado a pedido de fabian
                    /* else
                     {
                         faltantes++;
                     }*/
                    // }
                    itemRow.Numero = item.Cs_tag_ID.Split('-')[1];
                    /*  if (itemRow.Numero.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {*/
                    //13 digitos 
                   /* if (itemRow.Numero.Length == 8)
                    {
                        itemRow.Numero = "00000" + itemRow.Numero;
                    }*/
                    //Comentado a pedido de fabian
                    /*  else
                      {
                          faltantes++;
                      }
                  }*/
                    itemRow.AnioEmisionDUA = item.Cs_cr_AnioEmisionDUA;
                    /* if (itemRow.AnioEmisionDUA.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {*/
                    if (itemRow.AnioEmisionDUA.Trim().Length != 4)
                    {
                        // faltantes++;
                        itemRow.AnioEmisionDUA = "";
                    }
                    // }
                    itemRow.TipoDocProveedor = item.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                    /*  if (itemRow.TipoDocProveedor.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.TipoDocProveedor.Trim().Length != 1)
                          {
                              faltantes++;
                          }
                      }*/
                    itemRow.NumeroDocProveedor = item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                    /* if (itemRow.NumeroDocProveedor.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (itemRow.NumeroDocProveedor.Trim().Length != 11)
                         {
                             faltantes++;
                         }
                     }*/
                    itemRow.RazonSocialProveedor = item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                    /* if (itemRow.RazonSocialProveedor.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {*/
                    if (itemRow.RazonSocialProveedor.Trim().Length > 60)
                    {
                        itemRow.RazonSocialProveedor = itemRow.RazonSocialProveedor.Substring(0, 60);
                    }
                    // }
                    /*itemRow.Ruc = item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID;
                    if (itemRow.Ruc.Trim().Length == 0)
                    {
                        faltantes++;
                    }
                    itemRow.RazonSocial = item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
                    if (itemRow.RazonSocial.Trim().Length == 0)
                    {
                        faltantes++;
                    }*/
                    float f;
                    itemRow.AdqGravadasBaseImponible = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("1001", item.Cs_pr_Document_Id, "Cs_tag_PayableAmount");
                    /* if (itemRow.AdqGravadasBaseImponible.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {*/
                    if (float.TryParse(itemRow.AdqGravadasBaseImponible.Trim(), out f))
                    {
                        // success! Use f here
                    }
                    else
                    {
                        itemRow.AdqGravadasBaseImponible = "";
                        //  faltantes++;
                    }
                    // }
                    itemRow.AdqGravadasIGV = new clsEntidadDocument_TaxTotal(localDB).cs_pxObtenerValorPorTagSchemeNameAndDocumentoId("IGV", item.Cs_pr_Document_Id, "Cs_tag_TaxAmount");
                    /*  if (itemRow.AdqGravadasIGV.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {*/
                    if (float.TryParse(itemRow.AdqGravadasIGV.Trim(), out f))
                    {
                        // success! Use f here
                    }
                    else
                    {
                        itemRow.AdqGravadasIGV = "";
                        //  faltantes++;
                    }

                    // }
                    itemRow.AdqGravadasBaseImponibleGravadasNoGravadas = item.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas;
                    /*  if (itemRow.AdqGravadasBaseImponibleGravadasNoGravadas.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {*/
                    if (float.TryParse(itemRow.AdqGravadasBaseImponibleGravadasNoGravadas.Trim(), out f))
                    {
                        // success! Use f here
                    }
                    else
                    {
                        itemRow.AdqGravadasBaseImponibleGravadasNoGravadas = "";
                        //    faltantes++;
                    }

                    //  }
                    itemRow.AdqGravadasIGVGravadasNoGravados = item.Cs_cr_AdqGravadasIGVGravadasNoGravados;
                    /*  if (itemRow.AdqGravadasIGVGravadasNoGravados.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {*/
                    if (float.TryParse(itemRow.AdqGravadasIGVGravadasNoGravados.Trim(), out f))
                    {
                        // success! Use f here
                    }
                    else
                    {
                        itemRow.AdqGravadasIGVGravadasNoGravados = "";
                        //  faltantes++;
                    }

                    // }
                    itemRow.AdqGravadasBaseImponibleNoGravados = item.Cs_cr_AdqGravadasBaseImponibleNoGravados;
                    /* if (itemRow.AdqGravadasBaseImponibleNoGravados.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (float.TryParse(itemRow.AdqGravadasBaseImponibleNoGravados.Trim(), out f))
                         {
                             // success! Use f here
                         }
                         else
                         {
                             itemRow.AdqGravadasBaseImponibleNoGravados = "";
                             faltantes++;
                         }

                     }*/
                    itemRow.AdqGravadasIGVNoGravados = item.Cs_cr_AdqGravadasIGVNoGravados;
                    /*  if (itemRow.AdqGravadasIGVNoGravados.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (float.TryParse(itemRow.AdqGravadasIGVNoGravados.Trim(), out f))
                          {
                              // success! Use f here
                          }
                          else
                          {
                              itemRow.AdqGravadasIGVNoGravados = "";
                              faltantes++;
                          }

                      }*/

                    itemRow.ValorAdquisicionNoGravada = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("1002", item.Cs_pr_Document_Id, "Cs_tag_PayableAmount");
                    /*no  if (itemRow.ValorAdquisicionNoGravada.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (float.TryParse(itemRow.ValorAdquisicionNoGravada.Trim(), out f))
                          {
                              // success! Use f here
                          }
                          else
                          {
                              itemRow.ValorAdquisicionNoGravada = "";
                              faltantes++;
                          }

                      }*/

                    itemRow.Isc = new clsEntidadDocument_TaxTotal(localDB).cs_pxObtenerValorPorTagSchemeNameAndDocumentoId("ISC", item.Cs_pr_Document_Id, "Cs_tag_TaxAmount");
                    /*no if (itemRow.Isc.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (float.TryParse(itemRow.Isc.Trim(), out f))
                         {
                             // success! Use f here
                         }
                         else
                         {
                             itemRow.Isc = "";
                             faltantes++;
                         }

                     }*/

                    itemRow.OtrosTributosYCargos = new clsEntidadDocument_TaxTotal(localDB).cs_pxObtenerValorPorTagSchemeNameAndDocumentoId("OTH", item.Cs_pr_Document_Id, "Cs_tag_TaxAmount");
                    /*no if (itemRow.OtrosTributosYCargos.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (float.TryParse(itemRow.OtrosTributosYCargos.Trim(), out f))
                         {
                             // success! Use f here
                         }
                         else
                         {
                             itemRow.OtrosTributosYCargos = "";
                             faltantes++;
                         }

                     }*/

                    itemRow.ImporteTotal = item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID;
                    /*no if (itemRow.ImporteTotal.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (float.TryParse(itemRow.ImporteTotal.Trim(), out f))
                         {
                             // success! Use f here
                         }
                         else
                         {
                             itemRow.ImporteTotal = "";
                             faltantes++;
                         }

                     }*/


                    itemRow.NumeroComprobantePagoSujetoNoDomiciliado = item.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado;
                    /* if (itemRow.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (itemRow.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Length > 20)
                         {
                             itemRow.NumeroComprobantePagoSujetoNoDomiciliado = itemRow.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Substring(0, 20);
                             faltantes++;
                         }
                     }*/
                    bool isDetraccion = false;
                    string valorDet = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2006", item.Cs_pr_Document_Id, "Cs_tag_Value");
                    if (valorDet.Length > 0)
                    {
                        isDetraccion = true;
                    }

                    itemRow.ConstDepDetNumero = item.Cs_cr_ConstDepDetNumero;
                    if (isDetraccion)
                    {
                        /* if (itemRow.ConstDepDetNumero.Trim().Length == 0)
                         {
                             faltantes++;
                         }
                         else
                         {
                             if (itemRow.ConstDepDetNumero.Trim().Length > 20)
                             {
                                 itemRow.ConstDepDetNumero = itemRow.ConstDepDetNumero.Trim().Substring(0, 20);
                                 faltantes++;
                             }
                         }*/
                    }



                    if (item.Cs_cr_ConstDepDetFecha.Trim().Length > 0)
                    {
                        try
                        {
                            DateTime dt3 = DateTime.ParseExact(item.Cs_cr_ConstDepDetFecha, "yyyy-MM-dd", null);
                            itemRow.ConstDepDetFecha = dt3.ToString("dd/MM/yyyy");
                            // itemRow.DocReferenciaFecha = item.Cs_cr_DocReferenciaFecha;
                        }
                        catch { }
                    }
                    if (isDetraccion)
                    {
                        /* if (itemRow.ConstDepDetFecha.Trim().Length == 0)
                         {
                             faltantes++;
                         }*/
                    }

                    itemRow.TipoCambio = item.Cs_cr_TipoCambio;

                    /*  if (itemRow.TipoCambio.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (float.TryParse(itemRow.TipoCambio.Trim(), out f))
                          {
                              // success! Use f here
                          }
                          else
                          {
                              itemRow.TipoCambio = "";
                              faltantes++;
                          }

                      }*/

                    if (item.Cs_tag_InvoiceTypeCode == "07" || item.Cs_tag_InvoiceTypeCode == "08")
                    {
                        itemRow.NumeroRelacionado = item.Cs_tag_BillingReference_ID;
                        itemRow.TipoRelacionado = item.Cs_tag_BillingReference_DocumentTypeCode;
                        itemRow.DocReferencia = item.Cs_tag_BillingReference_DocumentTypeCode;
                        itemRow.DocReferenciaSerie = item.Cs_tag_BillingReference_ID.Split('-')[0];
                        itemRow.DocReferenciaNumero = item.Cs_tag_BillingReference_ID.Split('-')[1];
                        itemRow.DocReferenciaMotivo = item.Cs_tag_Discrepancy_ResponseCode;
                        itemRow.DocReferenciaMotivoEmision = item.Cs_tag_Discrepancy_Description;
                        if (itemRow.DocReferenciaFecha.Trim().Length > 0)
                        {
                            try
                            {
                                DateTime dt3 = DateTime.ParseExact(item.Cs_cr_DocReferenciaFecha, "yyyy-MM-dd", null);
                                itemRow.DocReferenciaFecha = dt3.ToString("dd/MM/yyyy");
                                // itemRow.DocReferenciaFecha = item.Cs_cr_DocReferenciaFecha;
                            }
                            catch { }


                        }

                        /*    if (itemRow.NumeroRelacionado.Trim().Length == 0)
                            {
                                faltantes++;
                            }
                            if (itemRow.TipoRelacionado.Trim().Length == 0)
                            {
                                faltantes++;
                            }
                            else
                            {
                                if (itemRow.TipoRelacionado.Trim().Length != 2)
                                {
                                    faltantes++;
                                }
                            }

                            if (itemRow.DocReferencia.Trim().Length == 0)
                            {
                                faltantes++;
                            }
                            if (itemRow.DocReferenciaSerie.Trim().Length == 0)
                            {
                                faltantes++;
                            }
                            else
                            {*/
                        /*if (itemRow.DocReferenciaSerie.Length == 4)
                        {
                            itemRow.DocReferenciaSerie = "00" + itemRow.DocReferenciaSerie;
                        }*/
                        //comentado a pedido de fabian
                        /*  else
                          {
                              faltantes++;
                          }*/
                        // }*/

                        /*  if (itemRow.DocReferenciaNumero.Trim().Length == 0)
                          {
                              faltantes++;
                          }
                          else
                          {*/
                        //13 digitos 
                       /* if (itemRow.DocReferenciaNumero.Length == 8)
                        {
                            itemRow.DocReferenciaNumero = "00000" + itemRow.DocReferenciaNumero;
                        }*/
                        //comentado a pedido de fabian
                        /* else
                         {
                             faltantes++;
                         }*/
                        //  }
                        /*  if (itemRow.DocReferenciaMotivo.Trim().Length == 0)
                          {
                              faltantes++;
                          }
                          if (itemRow.DocReferenciaMotivoEmision.Trim().Length == 0)
                          {
                              faltantes++;
                          }
                          if (itemRow.DocReferenciaFecha.Trim().Length == 0)
                          {
                              faltantes++;
                          }*/
                    }
                    itemRow.Moneda = item.Cs_cr_Moneda;
                    /* if (itemRow.Moneda.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/
                    itemRow.EquivalenteDolares = item.Cs_cr_EquivalenteDolares;
                    /*  if (itemRow.EquivalenteDolares.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (float.TryParse(itemRow.EquivalenteDolares.Trim(), out f))
                          {
                              // success! Use f here
                          }
                          else
                          {
                              itemRow.EquivalenteDolares = "";
                              faltantes++;
                          }
                      }*/
                    if (item.Cs_cr_FechaVencimientoDos.Trim().Length > 0)
                    {
                        try
                        {
                            DateTime dt22 = DateTime.ParseExact(item.Cs_cr_FechaVencimientoDos, "yyyy-MM-dd", null);
                            itemRow.FechaVencimientoDos = dt22.ToString("dd/MM/yyyy").Trim();
                        }
                        catch
                        {
                        }
                    }
                    /* if (itemRow.FechaVencimientoDos.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/
                    //itemRow.FechaVencimientoDos = item.Cs_cr_FechaVencimientoDos;
                    itemRow.CondicionCompra = item.Cs_cr_CondicionCompra;
                    /*  if (itemRow.CondicionCompra.Trim().Length == 0)
                      {
                          faltantes++;
                      }*/
                    itemRow.CtaContableBaseImponible = item.Cs_cr_CtaContableBaseImponible;
                    /* if (itemRow.CtaContableBaseImponible.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (itemRow.CtaContableBaseImponible.Trim().Length > 10)
                         {
                             itemRow.CtaContableBaseImponible = itemRow.CtaContableBaseImponible.Trim().Substring(0, 10);
                         }
                     }*/
                    itemRow.CtaContableOtrosTributos = item.Cs_cr_CtaContableOtrosTributos;
                    /*  if (itemRow.CtaContableOtrosTributos.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.CtaContableOtrosTributos.Trim().Length > 10)
                          {
                              itemRow.CtaContableOtrosTributos = itemRow.CtaContableOtrosTributos.Trim().Substring(0, 10);
                          }
                      }*/
                    itemRow.CtaContableTotal = item.Cs_cr_CtaContableTotal;
                    /*  if (itemRow.CtaContableTotal.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.CtaContableTotal.Trim().Length > 10)
                          {
                              itemRow.CtaContableTotal = itemRow.CtaContableTotal.Trim().Substring(0, 10);
                          }
                      }*/
                    itemRow.CentroCostosUno = item.Cs_cr_CentroCostosUno;
                    /*  if (itemRow.CentroCostosUno.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.CentroCostosUno.Trim().Length > 9)
                          {
                              itemRow.CentroCostosUno = itemRow.CentroCostosUno.Trim().Substring(0, 9);
                          }
                      }*/

                    itemRow.CentroCostosDos = item.Cs_cr_CentroCostosDos;
                    /*  if (itemRow.CentroCostosDos.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.CentroCostosDos.Trim().Length > 9)
                          {
                              itemRow.CentroCostosDos = itemRow.CentroCostosDos.Trim().Substring(0, 9);
                          }
                      }*/
                    // bool isDetraccion = false;
                    /* string valor = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2006", item.Cs_pr_Document_Id, "Cs_tag_Value");
                     if (valor.Length > 0)
                     {
                         itemRow.RegimenEspecial = "1";
                         // isDetraccion = true;
                     }
                     else
                     {
                         itemRow.RegimenEspecial = "";
                     }*/
                    itemRow.RegimenEspecial = item.Cs_cr_RegimenEspecial;
                    /* if (itemRow.RegimenEspecial.Length == 0)
                     {
                         string valor1 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2000", item.Cs_pr_Document_Id, "Cs_tag_Value");
                         if (valor1.Length > 0)
                         {
                             itemRow.RegimenEspecial = "2";
                         }
                         else
                         {
                             itemRow.RegimenEspecial = "";
                         }
                     }*/


                    /* if (itemRow.RegimenEspecial.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/

                    string valor2 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2003", item.Cs_pr_Document_Id, "Cs_tag_Percent");
                    if (valor2.Length > 0)
                    {
                        itemRow.PorcentajeRegimenEspecial = valor2;
                    }
                    else
                    {
                        itemRow.PorcentajeRegimenEspecial = "";
                    }

                    if (itemRow.PorcentajeRegimenEspecial.Length == 0)
                    {
                        string valor3 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2001", item.Cs_pr_Document_Id, "Cs_tag_Percent");
                        if (valor3.Length > 0)
                        {
                            itemRow.PorcentajeRegimenEspecial = valor3;
                        }
                        else
                        {
                            itemRow.PorcentajeRegimenEspecial = "";
                        }
                    }


                    /* if (itemRow.PorcentajeRegimenEspecial.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/

                    string valor4 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2003", item.Cs_pr_Document_Id, "Cs_tag_PayableAmount");
                    if (valor4.Length > 0)
                    {
                        itemRow.ImporteRegimenEspecial = valor4;
                    }
                    else
                    {
                        itemRow.ImporteRegimenEspecial = "";
                    }

                    if (itemRow.ImporteRegimenEspecial.Length == 0)
                    {
                        string valor5 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("2001", item.Cs_pr_Document_Id, "Cs_tag_PayableAmount");
                        if (valor5.Length > 0)
                        {
                            itemRow.ImporteRegimenEspecial = valor5;
                        }
                        else
                        {
                            itemRow.ImporteRegimenEspecial = "";
                        }
                    }

                    /*  if (itemRow.ImporteRegimenEspecial.Trim().Length == 0)
                      {
                          faltantes++;
                      }*/

                    // itemRow.ImporteRegimenEspecial = item.Cs_cr_ImporteRegimenEspecial;
                    itemRow.SerieDocumentoRegimenEspecial = item.Cs_cr_SerieDocumentoRegimenEspecial;
                    if (itemRow.RegimenEspecial.Trim().Length > 0)
                    {
                        /* if (itemRow.SerieDocumentoRegimenEspecial.Trim().Length == 0)
                         {
                             faltantes++;
                         }
                         else
                         {
                             if (itemRow.SerieDocumentoRegimenEspecial.Trim().Length > 6)
                             {
                                 itemRow.SerieDocumentoRegimenEspecial = itemRow.SerieDocumentoRegimenEspecial.Trim().Substring(0, 6);
                             }
                         }*/
                    }

                    itemRow.NumeroDocumentoRegimenEspecial = item.Cs_cr_NumeroDocumentoRegimenEspecial;
                    if (itemRow.RegimenEspecial.Trim().Length > 0)
                    {
                        /*if (itemRow.NumeroDocumentoRegimenEspecial.Trim().Length == 0)
                        {
                            faltantes++;
                        }
                        else
                        {
                            if (itemRow.NumeroDocumentoRegimenEspecial.Trim().Length > 13)
                            {
                                itemRow.NumeroDocumentoRegimenEspecial = itemRow.NumeroDocumentoRegimenEspecial.Trim().Substring(0, 13);
                            }
                        }*/
                    }
                    if (item.Cs_cr_FechaDocumentoRegimenEspecial.Trim().Length > 0)
                    {
                        try
                        {
                            DateTime dt5 = DateTime.ParseExact(item.Cs_cr_FechaDocumentoRegimenEspecial, "yyyy-MM-dd", null);
                            itemRow.FechaDocumentoRegimenEspecial = dt5.ToString("dd/MM/yyyy");
                        }
                        catch
                        {

                        }

                        //itemRow.FechaDocumentoRegimenEspecial = item.Cs_cr_FechaDocumentoRegimenEspecial;
                    }
                    if (itemRow.RegimenEspecial.Trim().Length > 0)
                    {
                        /* if (itemRow.FechaDocumentoRegimenEspecial.Trim().Length == 0)
                         {
                             faltantes++;
                         }*/
                    }


                    itemRow.CodigoPresupuesto = item.Cs_cr_CodigoPresupuesto;
                    /* if (itemRow.CodigoPresupuesto.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (itemRow.CodigoPresupuesto.Trim().Length > 10)
                         {
                             itemRow.CodigoPresupuesto = itemRow.CodigoPresupuesto.Trim().Substring(0, 10);
                         }
                     }*/
                    itemRow.PorcentajeIGV = item.Cs_cr_PorcentajeIGV;
                    /* if (itemRow.PorcentajeIGV.Trim().Length == 0)
                     {
                         faltantes++;
                     }
                     else
                     {
                         if (float.TryParse(itemRow.PorcentajeIGV.Trim(), out f))
                         {
                             // success! Use f here
                         }
                         else
                         {
                             itemRow.PorcentajeIGV = "";
                             faltantes++;
                         }
                     }*/


                    itemRow.Glosa = item.Cs_cr_Glosa;
                    /*  if (itemRow.Glosa.Trim().Length == 0)
                      {
                          faltantes++;
                      }
                      else
                      {
                          if (itemRow.Glosa.Trim().Length > 60)
                          {
                              itemRow.Glosa = itemRow.Glosa.Trim().Substring(0, 60);
                          }
                      }*/
                    itemRow.CondicionPercepcion = item.Cs_cr_CondicionPercepcion;
                    /* if (itemRow.CondicionPercepcion.Trim().Length == 0)
                     {
                         faltantes++;
                     }*/


                    lista_reporte.Add(itemRow);
                }
            }


            dgDocumentos.ItemsSource = lista_reporte;
            CantidadCompras.Content = "Existen " + lista_reporte.Count + " registros listados.";
            /*if (faltantes > 0)
            {
                btnColor.Fill = Brushes.Red;
                Faltantes.Content = "Existen " + faltantes + " campos por rellenar.";
            }
            else
            {
                Faltantes.Content = "";
                if (lista_reporte.Count > 0)
                {
                    btnColor.Fill = Brushes.Green;
                    //btnExportar.IsEnabled = true;
                }
                else
                {
                    btnColor.Fill = Brushes.Red;
                }

            }*/
        }

        /// <summary>
        /// EVENTO DE BUSQUEDA DE COMPROBANTES COMPRAS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBuscar_Click(object sender, RoutedEventArgs e)
        {
            periodoCompras = false;
            actualizarGrilla("");
        }

        /// <summary>
        /// Evento para exportar compras a excel segun formato del importador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarCompras(object sender, RoutedEventArgs e)
        {
            string rutaDirectorio = string.Empty;
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.InitialDirectory = "c:\\";
            saveFileDialog1.Filter = "xls Files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                rutaDirectorio = saveFileDialog1.FileName;
                if (rutaDirectorio.Substring(rutaDirectorio.Length - 4) != ".xls")
                {
                    rutaDirectorio = rutaDirectorio + ".xls";
                }
            }
            if (rutaDirectorio.Length > 0)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application xlApp;
                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;
                    // Microsoft.Office.Interop.Excel.Range chartRange;

                    xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    //obtener el listado de comprobantes de la grilla 
                    int numero_de_filas = lista_reporte.Count;
                    if (numero_de_filas == 0)
                    {
                        numero_de_filas = 1;
                    }
                    // numero_de_filas = 2;
                    //construir los rangos
                    string inicioFilaA = "A1";
                    string finFilaA = "A" + numero_de_filas;
                    Excel.Range formatRangeA;
                    formatRangeA = xlWorkSheet.get_Range(inicioFilaA, finFilaA);
                    formatRangeA.NumberFormat = "dd/mm/yyyy";

                    string inicioFilaB = "B1";
                    string finFilaB = "B" + numero_de_filas;
                    Excel.Range formatRangeB;
                    formatRangeB = xlWorkSheet.get_Range(inicioFilaB, finFilaB);
                    formatRangeB.NumberFormat = "dd/MM/yyyy";


                    string inicioFilaC = "C1";
                    string finFilaC = "C" + numero_de_filas;
                    Excel.Range formatRangeC;
                    formatRangeC = xlWorkSheet.get_Range(inicioFilaC, finFilaC);
                    formatRangeC.NumberFormat = "@";

                    string inicioFilaD = "D1";
                    string finFilaD = "D" + numero_de_filas;
                    Excel.Range formatRangeD;
                    formatRangeD = xlWorkSheet.get_Range(inicioFilaD, finFilaD);
                    formatRangeD.NumberFormat = "@";

                    string inicioFilaE = "E1";
                    string finFilaE = "E" + numero_de_filas;
                    Excel.Range formatRangeE;
                    formatRangeE = xlWorkSheet.get_Range(inicioFilaE, finFilaE);
                    formatRangeE.NumberFormat = "@";

                    string inicioFilaF = "F1";
                    string finFilaF = "F" + numero_de_filas;
                    Excel.Range formatRangeF;
                    formatRangeF = xlWorkSheet.get_Range(inicioFilaF, finFilaF);
                    formatRangeF.NumberFormat = "@";

                    string inicioFilaG = "G1";
                    string finFilaG = "G" + numero_de_filas;
                    Excel.Range formatRangeG;
                    formatRangeG = xlWorkSheet.get_Range(inicioFilaG, finFilaG);
                    formatRangeG.NumberFormat = "@";

                    string inicioFilaH = "H1";
                    string finFilaH = "H" + numero_de_filas;
                    Excel.Range formatRangeH;
                    formatRangeH = xlWorkSheet.get_Range(inicioFilaH, finFilaH);
                    formatRangeH.NumberFormat = "@";

                    string inicioFilaI = "I1";
                    string finFilaI = "I" + numero_de_filas;
                    Excel.Range formatRangeI;
                    formatRangeI = xlWorkSheet.get_Range(inicioFilaI, finFilaI);
                    formatRangeI.NumberFormat = "@";

                    string inicioFilaJ = "j1";
                    string finFilaJ = "j" + numero_de_filas;
                    Excel.Range formatRangeJ;
                    formatRangeJ = xlWorkSheet.get_Range(inicioFilaJ, finFilaJ);
                    formatRangeJ.NumberFormat = "#,##0.00";

                    string inicioFilaK = "k1";
                    string finFilaK = "k" + numero_de_filas;
                    Excel.Range formatRangeK;
                    formatRangeK = xlWorkSheet.get_Range(inicioFilaK, finFilaK);
                    formatRangeK.NumberFormat = "#,##0.00";

                    string inicioFilaL = "l1";
                    string finFilaL = "l" + numero_de_filas;
                    Excel.Range formatRangeL;
                    formatRangeL = xlWorkSheet.get_Range(inicioFilaL, finFilaL);
                    formatRangeL.NumberFormat = "#,##0.00";

                    string inicioFilaM = "m1";
                    string finFilaM = "m" + numero_de_filas;
                    Excel.Range formatRangeM;
                    formatRangeM = xlWorkSheet.get_Range(inicioFilaM, finFilaM);
                    formatRangeM.NumberFormat = "#,##0.00";

                    string inicioFilaN = "n1";
                    string finFilaN = "n" + numero_de_filas;
                    Excel.Range formatRangeN;
                    formatRangeN = xlWorkSheet.get_Range(inicioFilaN, finFilaN);
                    formatRangeN.NumberFormat = "#,##0.00";

                    string inicioFilaO = "o1";
                    string finFilaO = "o" + numero_de_filas;
                    Excel.Range formatRangeO;
                    formatRangeO = xlWorkSheet.get_Range(inicioFilaO, finFilaO);
                    formatRangeO.NumberFormat = "#,##0.00";

                    string inicioFilaP = "P1";
                    string finFilaP = "P" + numero_de_filas;
                    Excel.Range formatRangeP;
                    formatRangeP = xlWorkSheet.get_Range(inicioFilaP, finFilaP);
                    formatRangeP.NumberFormat = "#,##0.00";

                    string inicioFilaQ = "Q1";
                    string finFilaQ = "Q" + numero_de_filas;
                    Excel.Range formatRangeQ;
                    formatRangeQ = xlWorkSheet.get_Range(inicioFilaQ, finFilaQ);
                    formatRangeQ.NumberFormat = "#,##0.00";

                    string inicioFilaR = "R1";
                    string finFilaR = "R" + numero_de_filas;
                    Excel.Range formatRangeR;
                    formatRangeR = xlWorkSheet.get_Range(inicioFilaR, finFilaR);
                    formatRangeR.NumberFormat = "#,##0.00";

                    string inicioFilaS = "S1";
                    string finFilaS = "S" + numero_de_filas;
                    Excel.Range formatRangeS;
                    formatRangeS = xlWorkSheet.get_Range(inicioFilaS, finFilaS);
                    formatRangeS.NumberFormat = "#,##0.00";

                    string inicioFilaT = "T1";
                    string finFilaT = "T" + numero_de_filas;
                    Excel.Range formatRangeT;
                    formatRangeT = xlWorkSheet.get_Range(inicioFilaT, finFilaT);
                    formatRangeT.NumberFormat = "@";

                    string inicioFilaU = "U1";
                    string finFilaU = "U" + numero_de_filas;
                    Excel.Range formatRangeU;
                    formatRangeU = xlWorkSheet.get_Range(inicioFilaU, finFilaU);
                    formatRangeU.NumberFormat = "@";

                    string inicioFilaV = "V1";
                    string finFilaV = "V" + numero_de_filas;
                    Excel.Range formatRangeV;
                    formatRangeV = xlWorkSheet.get_Range(inicioFilaV, finFilaV);
                    formatRangeV.NumberFormat = "dd/mm/yyyy";

                    string inicioFilaW = "W1";
                    string finFilaW = "W" + numero_de_filas;
                    Excel.Range formatRangeW;
                    formatRangeW = xlWorkSheet.get_Range(inicioFilaW, finFilaW);
                    formatRangeW.NumberFormat = "#,##0.0000";

                    string inicioFilaX = "X1";
                    string finFilaX = "X" + numero_de_filas;
                    Excel.Range formatRangeX;
                    formatRangeX = xlWorkSheet.get_Range(inicioFilaX, finFilaX);
                    formatRangeX.NumberFormat = "dd/mm/yyyy";

                    string inicioFilaY = "Y1";
                    string finFilaY = "Y" + numero_de_filas;
                    Excel.Range formatRangeY;
                    formatRangeY = xlWorkSheet.get_Range(inicioFilaY, finFilaY);
                    formatRangeY.NumberFormat = "@";

                    string inicioFilaZ = "Z1";
                    string finFilaZ = "Z" + numero_de_filas;
                    Excel.Range formatRangeZ;
                    formatRangeZ = xlWorkSheet.get_Range(inicioFilaZ, finFilaZ);
                    formatRangeZ.NumberFormat = "@";

                    string inicioFilaAA = "AA1";
                    string finFilaAA = "AA" + numero_de_filas;
                    Excel.Range formatRangeAA;
                    formatRangeAA = xlWorkSheet.get_Range(inicioFilaAA, finFilaAA);
                    formatRangeAA.NumberFormat = "@";

                    string inicioFilaAB = "AB1";
                    string finFilaAB = "AB" + numero_de_filas;
                    Excel.Range formatRangeAB;
                    formatRangeAB = xlWorkSheet.get_Range(inicioFilaAB, finFilaAB);
                    formatRangeAB.NumberFormat = "@";

                    string inicioFilaAC = "AC1";
                    string finFilaAC = "AC" + numero_de_filas;
                    Excel.Range formatRangeAC;
                    formatRangeAC = xlWorkSheet.get_Range(inicioFilaAC, finFilaAC);
                    formatRangeAC.NumberFormat = "#,##0.00";

                    string inicioFilaAD = "AD1";
                    string finFilaAD = "AD" + numero_de_filas;
                    Excel.Range formatRangeAD;
                    formatRangeAD = xlWorkSheet.get_Range(inicioFilaAD, finFilaAD);
                    formatRangeAD.NumberFormat = "dd/mm/yyyy";

                    string inicioFilaAE = "AE1";
                    string finFilaAE = "AE" + numero_de_filas;
                    Excel.Range formatRangeAE;
                    formatRangeAE = xlWorkSheet.get_Range(inicioFilaAE, finFilaAE);
                    formatRangeAE.NumberFormat = "@";

                    string inicioFilaAF = "AF1";
                    string finFilaAF = "AF" + numero_de_filas;
                    Excel.Range formatRangeAF;
                    formatRangeAF = xlWorkSheet.get_Range(inicioFilaAF, finFilaAF);
                    formatRangeAF.NumberFormat = "@";

                    string inicioFilaAG = "AG1";
                    string finFilaAG = "AG" + numero_de_filas;
                    Excel.Range formatRangeAG;
                    formatRangeAG = xlWorkSheet.get_Range(inicioFilaAG, finFilaAG);
                    formatRangeAG.NumberFormat = "@";

                    string inicioFilaAH = "AH1";
                    string finFilaAH = "AH" + numero_de_filas;
                    Excel.Range formatRangeAH;
                    formatRangeAH = xlWorkSheet.get_Range(inicioFilaAH, finFilaAH);
                    formatRangeAH.NumberFormat = "@";

                    string inicioFilaAI = "AI1";
                    string finFilaAI = "AI" + numero_de_filas;
                    Excel.Range formatRangeAI;
                    formatRangeAI = xlWorkSheet.get_Range(inicioFilaAI, finFilaAI);
                    formatRangeAI.NumberFormat = "@";

                    string inicioFilaAJ = "AJ1";
                    string finFilaAJ = "AJ" + numero_de_filas;
                    Excel.Range formatRangeAJ;
                    formatRangeAJ = xlWorkSheet.get_Range(inicioFilaAJ, finFilaAJ);
                    formatRangeAJ.NumberFormat = "@";

                    string inicioFilaAK = "AK1";
                    string finFilaAK = "AK" + numero_de_filas;
                    Excel.Range formatRangeAK;
                    formatRangeAK = xlWorkSheet.get_Range(inicioFilaAK, finFilaAK);
                    formatRangeAK.NumberFormat = "0";

                    string inicioFilaAL = "AL1";
                    string finFilaAL = "AL" + numero_de_filas;
                    Excel.Range formatRangeAL;
                    formatRangeAL = xlWorkSheet.get_Range(inicioFilaAL, finFilaAL);
                    formatRangeAL.NumberFormat = "#,##0.00";

                    string inicioFilaAM = "AM1";
                    string finFilaAM = "AM" + numero_de_filas;
                    Excel.Range formatRangeAM;
                    formatRangeAM = xlWorkSheet.get_Range(inicioFilaAM, finFilaAM);
                    formatRangeAM.NumberFormat = "#,##0.00";

                    string inicioFilaAN = "AN1";
                    string finFilaAN = "AN" + numero_de_filas;
                    Excel.Range formatRangeAN;
                    formatRangeAN = xlWorkSheet.get_Range(inicioFilaAN, finFilaAN);
                    formatRangeAN.NumberFormat = "@";

                    string inicioFilaAO = "AO1";
                    string finFilaAO = "AO" + numero_de_filas;
                    Excel.Range formatRangeAO;
                    formatRangeAO = xlWorkSheet.get_Range(inicioFilaAO, finFilaAO);
                    formatRangeAO.NumberFormat = "@";

                    string inicioFilaAP = "AP1";
                    string finFilaAP = "AP" + numero_de_filas;
                    Excel.Range formatRangeAP;
                    formatRangeAP = xlWorkSheet.get_Range(inicioFilaAP, finFilaAP);
                    formatRangeAP.NumberFormat = "dd/mm/yyyy";

                    string inicioFilaAQ = "AQ1";
                    string finFilaAQ = "AQ" + numero_de_filas;
                    Excel.Range formatRangeAQ;
                    formatRangeAQ = xlWorkSheet.get_Range(inicioFilaAQ, finFilaAQ);
                    formatRangeAQ.NumberFormat = "@";

                    string inicioFilaAR = "AR1";
                    string finFilaAR = "AR" + numero_de_filas;
                    Excel.Range formatRangeAR;
                    formatRangeAR = xlWorkSheet.get_Range(inicioFilaAR, finFilaAR);
                    formatRangeAR.NumberFormat = "#,##0.00";

                    string inicioFilaAS = "AS1";
                    string finFilaAS = "AS" + numero_de_filas;
                    Excel.Range formatRangeAS;
                    formatRangeAS = xlWorkSheet.get_Range(inicioFilaAS, finFilaAS);
                    formatRangeAS.NumberFormat = "@";

                    string inicioFilaAT = "AT1";
                    string finFilaAT = "AT" + numero_de_filas;
                    Excel.Range formatRangeAT;
                    formatRangeAT = xlWorkSheet.get_Range(inicioFilaAT, finFilaAT);
                    formatRangeAT.NumberFormat = "@";

                    string inicioFilaAU = "AU1";
                    string finFilaAU = "AU" + numero_de_filas;
                    Excel.Range formatRangeAU;
                    formatRangeAU = xlWorkSheet.get_Range(inicioFilaAU, finFilaAU);
                    formatRangeAU.NumberFormat = "#,##0.00";

                    int fila = 0;
                    foreach (DocumentoCompra items in lista_reporte)
                    {
                        fila++;
                        /*  Excel.Range rg = (Excel.Range)xlWorkSheet.Cells[fila, 1];
                          rg.EntireColumn.NumberFormat = "dd/MM/yyyy";*/

                        //xlWorkSheet.Cells[fila, 1].NumberFormat = "dd/mm/yyyy";
                        DateTime myDate = DateTime.ParseExact(items.FechaEmision.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        xlWorkSheet.Cells[fila, 1] = myDate.ToOADate(); //A
                                                                        //xlWorkSheet.Cells[fila, 1].NumberFormat = "dd/MM/aaaa";
                                                                        //string inicioFilaB = "B"+ fila;
                                                                        //string finFilaB = "B" + fila;
                                                                        //Excel.Range formatRangeB;
                                                                        //formatRangeB = xlWorkSheet.get_Range(inicioFilaB, finFilaB);
                                                                        //formatRangeB.NumberFormat = "dd/mm/yyyy";
                                                                        //formatRangeB.Value = items.FechaVencimiento.Trim();
                        if (items.FechaVencimiento.Trim() != "")
                        {
                            try
                            {
                                DateTime myDate2 = DateTime.ParseExact(items.FechaVencimiento.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 2] = myDate2.ToOADate();
                            }
                            catch (Exception)
                            {
                                DateTime myDate21 = DateTime.ParseExact(items.FechaVencimiento.Trim().ToString(), "dd/MM/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime myDate2 = DateTime.ParseExact(myDate21.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 2] = myDate2.ToOADate();
                            }

                            // xlWorkSheet.Cells[fila, 2] = items.FechaVencimiento.Trim();//B
                        }



                        xlWorkSheet.Cells[fila, 3] = items.Tipo.Trim();//C
                        xlWorkSheet.Cells[fila, 4] = items.Serie.Trim();//D
                        xlWorkSheet.Cells[fila, 5] = items.AnioEmisionDUA.Trim();//E
                        xlWorkSheet.Cells[fila, 6] = items.Numero.Trim();//F
                        xlWorkSheet.Cells[fila, 7] = items.TipoDocProveedor.Trim();//G
                        xlWorkSheet.Cells[fila, 8] = items.NumeroDocProveedor.Trim();//H
                        xlWorkSheet.Cells[fila, 9] = items.RazonSocialProveedor.Trim();//I
                        xlWorkSheet.Cells[fila, 10] = items.AdqGravadasBaseImponible.Trim();//J
                        xlWorkSheet.Cells[fila, 11] = items.AdqGravadasIGV.Trim();//K
                        xlWorkSheet.Cells[fila, 12] = items.AdqGravadasBaseImponibleGravadasNoGravadas.Trim();//L
                        xlWorkSheet.Cells[fila, 13] = items.AdqGravadasIGVGravadasNoGravados.Trim();//M
                        xlWorkSheet.Cells[fila, 14] = items.AdqGravadasBaseImponibleNoGravados.Trim();//N
                        xlWorkSheet.Cells[fila, 15] = items.AdqGravadasIGVNoGravados.Trim();//O
                        xlWorkSheet.Cells[fila, 16] = items.ValorAdquisicionNoGravada.Trim();//P
                        xlWorkSheet.Cells[fila, 17] = items.Isc.Trim();//Q
                        xlWorkSheet.Cells[fila, 18] = items.OtrosTributosYCargos.Trim();//R
                        xlWorkSheet.Cells[fila, 19] = items.ImporteTotal.Trim();//S
                        xlWorkSheet.Cells[fila, 20] = items.NumeroComprobantePagoSujetoNoDomiciliado.Trim();//T
                        xlWorkSheet.Cells[fila, 21] = items.ConstDepDetNumero.Trim();//U

                        //formatRangeB.Value = items.FechaVencimiento.Trim();
                        if (items.ConstDepDetFecha.Trim() != "")
                        {
                            try
                            {

                                DateTime myDate3 = DateTime.ParseExact(items.ConstDepDetFecha.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 22] = myDate3.ToOADate();
                            }
                            catch (Exception)
                            {
                                DateTime myDate31 = DateTime.ParseExact(items.ConstDepDetFecha.Trim().ToString(), "dd/MM/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime myDate3 = DateTime.ParseExact(myDate31.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 22] = myDate3.ToOADate();
                            }

                            // xlWorkSheet.Cells[fila, 22] = items.ConstDepDetFecha.Trim();//V
                        }


                        xlWorkSheet.Cells[fila, 23] = items.TipoCambio.Trim();//W
                        if (items.DocReferenciaFecha.Trim() != "")
                        {
                            try
                            {
                                DateTime myDate4 = DateTime.ParseExact(items.DocReferenciaFecha.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 24] = myDate4.ToOADate();
                            }
                            catch (Exception)
                            {
                                DateTime myDate41 = DateTime.ParseExact(items.DocReferenciaFecha.Trim().ToString(), "dd/MM/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime myDate4 = DateTime.ParseExact(myDate41.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 24] = myDate4.ToOADate();
                            }

                            // xlWorkSheet.Cells[fila, 24] = items.DocReferenciaFecha.Trim();//X
                        }

                        xlWorkSheet.Cells[fila, 25] = items.DocReferencia.Trim();//Y
                        xlWorkSheet.Cells[fila, 26] = items.DocReferenciaSerie.Trim();//Z
                        xlWorkSheet.Cells[fila, 27] = items.DocReferenciaNumero.Trim();//AA
                        xlWorkSheet.Cells[fila, 28] = items.Moneda.Trim();//AB
                        xlWorkSheet.Cells[fila, 29] = items.EquivalenteDolares.Trim();//AC

                        if (items.FechaVencimientoDos.Trim() != "")
                        {
                            try
                            {
                                DateTime myDate5 = DateTime.ParseExact(items.FechaVencimientoDos.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 30] = myDate5.ToOADate();
                            }
                            catch (Exception)
                            {
                                DateTime myDate51 = DateTime.ParseExact(items.FechaVencimientoDos.Trim().ToString(), "dd/MM/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime myDate5 = DateTime.ParseExact(myDate51.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 30] = myDate5.ToOADate();
                            }

                            // xlWorkSheet.Cells[fila, 30] = items.FechaVencimientoDos.Trim();//AD
                        }



                        xlWorkSheet.Cells[fila, 31] = items.CondicionCompra.Trim();//AE
                        xlWorkSheet.Cells[fila, 32] = items.CtaContableBaseImponible.Trim();//AF
                        xlWorkSheet.Cells[fila, 33] = items.CtaContableOtrosTributos.Trim();//AG
                        xlWorkSheet.Cells[fila, 34] = items.CtaContableTotal.Trim();//AH
                        xlWorkSheet.Cells[fila, 35] = items.CentroCostosUno.Trim();//AI
                        xlWorkSheet.Cells[fila, 36] = items.CentroCostosDos.Trim();//AJ
                        xlWorkSheet.Cells[fila, 37] = items.RegimenEspecial.Trim();//AK
                        xlWorkSheet.Cells[fila, 38] = items.PorcentajeRegimenEspecial.Trim();//AL
                        xlWorkSheet.Cells[fila, 39] = items.ImporteRegimenEspecial.Trim();//AM
                        xlWorkSheet.Cells[fila, 40] = items.SerieDocumentoRegimenEspecial.Trim();//AN
                        xlWorkSheet.Cells[fila, 41] = items.NumeroDocumentoRegimenEspecial.Trim();//AO

                        if (items.FechaDocumentoRegimenEspecial.Trim() != "")
                        {
                            try
                            {
                                DateTime myDate6 = DateTime.ParseExact(items.FechaDocumentoRegimenEspecial.Trim().ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 42] = myDate6.ToOADate();
                            }
                            catch (Exception)
                            {
                                DateTime myDate61 = DateTime.ParseExact(items.FechaDocumentoRegimenEspecial.Trim().ToString(), "dd/MM/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime myDate6 = DateTime.ParseExact(myDate61.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                xlWorkSheet.Cells[fila, 42] = myDate6.ToOADate();
                            }

                            //  xlWorkSheet.Cells[fila, 42] = items.FechaDocumentoRegimenEspecial.Trim();//AP
                        }


                        xlWorkSheet.Cells[fila, 43] = items.CodigoPresupuesto.Trim();//AQ
                        xlWorkSheet.Cells[fila, 44] = items.PorcentajeIGV.Trim();//AR
                        xlWorkSheet.Cells[fila, 45] = items.Glosa.Trim();//AS
                        xlWorkSheet.Cells[fila, 46] = items.CondicionPercepcion.Trim();//AT
                        xlWorkSheet.Cells[fila, 47] = items.ImporteRegimenEspecial.Trim();//AU
                    }

                    string NumInicioFila = "1";
                    if (fila == 0)
                    {
                        fila = 1;
                    }
                    string NumFinFila = fila.ToString();

                    Excel.Range DinamicChartRangeA = xlWorkSheet.get_Range("A" + NumInicioFila, "A" + NumFinFila);
                    DinamicChartRangeA.Columns.AutoFit();
                    Excel.Range DinamicChartRangeB = xlWorkSheet.get_Range("B" + NumInicioFila, "B" + NumFinFila);
                    DinamicChartRangeB.Columns.AutoFit();
                    Excel.Range DinamicChartRangeC = xlWorkSheet.get_Range("C" + NumInicioFila, "C" + NumFinFila);
                    DinamicChartRangeC.Columns.AutoFit();
                    Excel.Range DinamicChartRangeD = xlWorkSheet.get_Range("D" + NumInicioFila, "D" + NumFinFila);
                    DinamicChartRangeD.Columns.AutoFit();
                    Excel.Range DinamicChartRangeE = xlWorkSheet.get_Range("E" + NumInicioFila, "E" + NumFinFila);
                    DinamicChartRangeE.Columns.AutoFit();
                    Excel.Range DinamicChartRangeF = xlWorkSheet.get_Range("F" + NumInicioFila, "F" + NumFinFila);
                    DinamicChartRangeF.Columns.AutoFit();
                    Excel.Range DinamicChartRangeG = xlWorkSheet.get_Range("G" + NumInicioFila, "G" + NumFinFila);
                    DinamicChartRangeG.Columns.AutoFit();
                    Excel.Range DinamicChartRangeH = xlWorkSheet.get_Range("H" + NumInicioFila, "H" + NumFinFila);
                    DinamicChartRangeH.Columns.AutoFit();
                    Excel.Range DinamicChartRangeI = xlWorkSheet.get_Range("I" + NumInicioFila, "I" + NumFinFila);
                    DinamicChartRangeI.Columns.AutoFit();
                    Excel.Range DinamicChartRangeJ = xlWorkSheet.get_Range("J" + NumInicioFila, "J" + NumFinFila);
                    DinamicChartRangeJ.Columns.AutoFit();
                    Excel.Range DinamicChartRangeK = xlWorkSheet.get_Range("K" + NumInicioFila, "K" + NumFinFila);
                    DinamicChartRangeK.Columns.AutoFit();
                    Excel.Range DinamicChartRangeL = xlWorkSheet.get_Range("L" + NumInicioFila, "L" + NumFinFila);
                    DinamicChartRangeL.Columns.AutoFit();
                    Excel.Range DinamicChartRangeM = xlWorkSheet.get_Range("M" + NumInicioFila, "M" + NumFinFila);
                    DinamicChartRangeM.Columns.AutoFit();
                    Excel.Range DinamicChartRangeN = xlWorkSheet.get_Range("N" + NumInicioFila, "N" + NumFinFila);
                    DinamicChartRangeN.Columns.AutoFit();
                    Excel.Range DinamicChartRangeO = xlWorkSheet.get_Range("O" + NumInicioFila, "O" + NumFinFila);
                    DinamicChartRangeO.Columns.AutoFit();
                    Excel.Range DinamicChartRangeP = xlWorkSheet.get_Range("P" + NumInicioFila, "P" + NumFinFila);
                    DinamicChartRangeP.Columns.AutoFit();
                    Excel.Range DinamicChartRangeQ = xlWorkSheet.get_Range("Q" + NumInicioFila, "Q" + NumFinFila);
                    DinamicChartRangeQ.Columns.AutoFit();
                    Excel.Range DinamicChartRangeR = xlWorkSheet.get_Range("R" + NumInicioFila, "R" + NumFinFila);
                    DinamicChartRangeR.Columns.AutoFit();
                    Excel.Range DinamicChartRangeS = xlWorkSheet.get_Range("S" + NumInicioFila, "S" + NumFinFila);
                    DinamicChartRangeS.Columns.AutoFit();
                    Excel.Range DinamicChartRangeT = xlWorkSheet.get_Range("T" + NumInicioFila, "T" + NumFinFila);
                    DinamicChartRangeT.Columns.AutoFit();
                    Excel.Range DinamicChartRangeU = xlWorkSheet.get_Range("U" + NumInicioFila, "U" + NumFinFila);
                    DinamicChartRangeU.Columns.AutoFit();
                    Excel.Range DinamicChartRangeV = xlWorkSheet.get_Range("V" + NumInicioFila, "V" + NumFinFila);
                    DinamicChartRangeV.Columns.AutoFit();
                    Excel.Range DinamicChartRangeW = xlWorkSheet.get_Range("W" + NumInicioFila, "W" + NumFinFila);
                    DinamicChartRangeW.Columns.AutoFit();
                    Excel.Range DinamicChartRangeX = xlWorkSheet.get_Range("X" + NumInicioFila, "X" + NumFinFila);
                    DinamicChartRangeX.Columns.AutoFit();
                    Excel.Range DinamicChartRangeY = xlWorkSheet.get_Range("Y" + NumInicioFila, "Y" + NumFinFila);
                    DinamicChartRangeY.Columns.AutoFit();
                    Excel.Range DinamicChartRangeZ = xlWorkSheet.get_Range("Z" + NumInicioFila, "Z" + NumFinFila);
                    DinamicChartRangeZ.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAA = xlWorkSheet.get_Range("AA" + NumInicioFila, "AA" + NumFinFila);
                    DinamicChartRangeAA.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAB = xlWorkSheet.get_Range("AB" + NumInicioFila, "AB" + NumFinFila);
                    DinamicChartRangeAB.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAC = xlWorkSheet.get_Range("AC" + NumInicioFila, "AC" + NumFinFila);
                    DinamicChartRangeAC.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAD = xlWorkSheet.get_Range("AD" + NumInicioFila, "AD" + NumFinFila);
                    DinamicChartRangeAD.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAE = xlWorkSheet.get_Range("AE" + NumInicioFila, "AE" + NumFinFila);
                    DinamicChartRangeAE.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAF = xlWorkSheet.get_Range("AF" + NumInicioFila, "AF" + NumFinFila);
                    DinamicChartRangeAF.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAG = xlWorkSheet.get_Range("AG" + NumInicioFila, "AG" + NumFinFila);
                    DinamicChartRangeAG.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAH = xlWorkSheet.get_Range("AH" + NumInicioFila, "AH" + NumFinFila);
                    DinamicChartRangeAH.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAI = xlWorkSheet.get_Range("AI" + NumInicioFila, "AI" + NumFinFila);
                    DinamicChartRangeAI.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAJ = xlWorkSheet.get_Range("AJ" + NumInicioFila, "AJ" + NumFinFila);
                    DinamicChartRangeAJ.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAK = xlWorkSheet.get_Range("AK" + NumInicioFila, "AK" + NumFinFila);
                    DinamicChartRangeAK.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAL = xlWorkSheet.get_Range("AL" + NumInicioFila, "AL" + NumFinFila);
                    DinamicChartRangeAL.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAM = xlWorkSheet.get_Range("AM" + NumInicioFila, "AM" + NumFinFila);
                    DinamicChartRangeAM.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAN = xlWorkSheet.get_Range("AN" + NumInicioFila, "AN" + NumFinFila);
                    DinamicChartRangeAN.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAO = xlWorkSheet.get_Range("AO" + NumInicioFila, "AO" + NumFinFila);
                    DinamicChartRangeAO.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAP = xlWorkSheet.get_Range("AP" + NumInicioFila, "AP" + NumFinFila);
                    DinamicChartRangeAP.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAQ = xlWorkSheet.get_Range("AQ" + NumInicioFila, "AQ" + NumFinFila);
                    DinamicChartRangeAQ.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAR = xlWorkSheet.get_Range("AR" + NumInicioFila, "AR" + NumFinFila);
                    DinamicChartRangeAR.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAS = xlWorkSheet.get_Range("AS" + NumInicioFila, "AS" + NumFinFila);
                    DinamicChartRangeAS.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAT = xlWorkSheet.get_Range("AT" + NumInicioFila, "AT" + NumFinFila);
                    DinamicChartRangeAT.Columns.AutoFit();
                    Excel.Range DinamicChartRangeAU = xlWorkSheet.get_Range("AU" + NumInicioFila, "AU" + NumFinFila);
                    DinamicChartRangeAU.Columns.AutoFit();




                    /*xlWorkSheet.Cells[1, 1] = "31/05/2014";
                    xlWorkSheet.Cells[2, 1] = "31/05/2014";
                    //add data 
                    xlWorkSheet.Cells[4, 2] = "";
                    xlWorkSheet.Cells[4, 3] = "Student1";
                    xlWorkSheet.Cells[4, 4] = "Student2";
                    xlWorkSheet.Cells[4, 5] = "Student3";

                    xlWorkSheet.Cells[5, 2] = "Term1";
                    xlWorkSheet.Cells[5, 3] = "80";
                    xlWorkSheet.Cells[5, 4] = "65";
                    xlWorkSheet.Cells[5, 5] = "45";

                    xlWorkSheet.Cells[6, 2] = "Term2";
                    xlWorkSheet.Cells[6, 3] = "78";
                    xlWorkSheet.Cells[6, 4] = "72";
                    xlWorkSheet.Cells[6, 5] = "60";

                    xlWorkSheet.Cells[7, 2] = "Term3";
                    xlWorkSheet.Cells[7, 3] = "82";
                    xlWorkSheet.Cells[7, 4] = "80";
                    xlWorkSheet.Cells[7, 5] = "65";

                    xlWorkSheet.Cells[8, 2] = "Term4";
                    xlWorkSheet.Cells[8, 3] = "75";
                    xlWorkSheet.Cells[8, 4] = "82";
                    xlWorkSheet.Cells[8, 5] = "68";

                    xlWorkSheet.Cells[9, 2] = "Total";
                    xlWorkSheet.Cells[9, 3] = "315";
                    xlWorkSheet.Cells[9, 4] = "299";
                    xlWorkSheet.Cells[9, 5] = "238";

                    xlWorkSheet.get_Range("b2", "e3").Merge(false);*/

                    /*chartRange = xlWorkSheet.get_Range("b2", "e3");
                    chartRange.FormulaR1C1 = "MARK LIST";
                    chartRange.HorizontalAlignment = 3;
                    chartRange.VerticalAlignment = 3;
                    chartRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    chartRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    chartRange.Font.Size = 20;

                    chartRange = xlWorkSheet.get_Range("b4", "e4");
                    chartRange.Font.Bold = true;
                    chartRange = xlWorkSheet.get_Range("b9", "e9");
                    chartRange.Font.Bold = true;

                    chartRange = xlWorkSheet.get_Range("b2", "e9");
                    chartRange.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Excel.XlColorIndex.xlColorIndexAutomatic);
                    */

                    xlWorkBook.SaveAs(rutaDirectorio, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    //releaseObject(xlApp);
                    //releaseObject(xlWorkBook);
                    //releaseObject(xlWorkSheet);

                    System.Windows.Forms.MessageBox.Show("Archivo creado en la ruta seleccionada.");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }


            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Seleccione una ruta valida.");
            }

        }
        private void btnAsignarRPeriodo_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxPares valueMes = (ComboBoxPares)periodoMesRAsignar.SelectedItem;
            ComboBoxPares valueAnio = (ComboBoxPares)periodoAnioRAsignar.SelectedItem;

            if (valueMes._Id == "" || valueAnio._Id == "")
            {
                System.Windows.Forms.MessageBox.Show("Seleccione mes y año para asignar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //recorrer los seleccionados
                List<DocumentoCompra> seleccionados = new List<DocumentoCompra>();
                foreach (var it in lista_reporte)
                {
                    if (it.Check == true)
                    {
                        seleccionados.Add(it);
                    }
                }

                if (seleccionados.Count > 0)
                {
                    foreach (DocumentoCompra row in seleccionados)
                    {
                        clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_pxObtenerDocumentoById(row.Id);
                        doc.Cs_cr_Periodo = valueMes._Id + valueAnio._Id;
                        doc.cs_pxActualizar(false, false);
                    }
                    System.Windows.Forms.MessageBox.Show("La asignacion se ha realizado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (periodoCompras == true)
                    {
                        ComboBoxPares valueMesR = (ComboBoxPares)periodoMesRCompras.SelectedItem;
                        ComboBoxPares valueAnioR = (ComboBoxPares)periodoAnioRCompras.SelectedItem;
                        actualizarGrilla(valueMes._Id + valueAnio._Id);
                    }
                    else
                    {
                        actualizarGrilla("");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione registros.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ButtonBuscarRegistroComprasPeriodo_Click(object sender, RoutedEventArgs e)
        {
            periodoCompras = true;
            ComboBoxPares valueMes = (ComboBoxPares)periodoMesRCompras.SelectedItem;
            ComboBoxPares valueAnio = (ComboBoxPares)periodoAnioRCompras.SelectedItem;

            if (valueMes._Id == "" || valueAnio._Id == "")
            {
                System.Windows.Forms.MessageBox.Show("Seleccione mes y año para filtrar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                actualizarGrilla(valueMes._Id + valueAnio._Id);
            }
        }

        private void chkAll_CheckedRC(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (DocumentoCompra item in lista_reporte)
                    {
                        item.Check = true;
                    }
                    dgDocumentos.ItemsSource = null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        private void chkAll_UncheckedRC(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    //checkall
                    foreach (DocumentoCompra item in lista_reporte)
                    {
                        item.Check = false;
                    }
                    dgDocumentos.ItemsSource = null;
                    dgDocumentos.Items.Clear();
                    dgDocumentos.ItemsSource = lista_reporte;
                }
            }
            catch
            {

            }
        }

        //Cristhian|03/11/2017|FEICONT2-408
        /*Nuevo evento para mostrar el visor del Documento PDF. Lo primero que se realiza es generar el Documento PDF para 
         que despues sea visualizado por el cliente. Lo que le permite al cliente seleccionar la ruta donde se guardara el 
         documento PDF*/
        /*NUEVO INICIO*/
        private void ClicEnImagen (object sender, RoutedEventArgs e)
        {
            try
            {
                if (lista_reporte.Count > 0)
                {
                    DocumentoCompra item = (DocumentoCompra)dgDocumentos.SelectedItem;
                    if (item != null)
                    {
                        try
                        {
                            clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                            string currentDirectory = Environment.CurrentDirectory;
                            string pathImage = currentDirectory + "\\" + declarante.Cs_pr_Ruc + "\\logo.png";
                            string pathDatos = currentDirectory + "\\" + declarante.Cs_pr_Ruc + "\\informacionImpreso.txt";
                            if (File.Exists(pathImage) && File.Exists(pathDatos))
                            {
                                StreamReader readDatos = new StreamReader(pathDatos);
                                string datosImpresa = readDatos.ReadToEnd();
                                readDatos.Close();
                                clsEntidadDocument cabecera = new clsEntidadDocument(localDB);
                                cabecera.cs_fxObtenerUnoPorId(item.Id);
                                if (cabecera != null)
                                {
                                    string[] partes = cabecera.Cs_tag_ID.Split('-');

                                    System.Windows.Forms.SaveFileDialog sfdDescargar = new System.Windows.Forms.SaveFileDialog();
                                    sfdDescargar.FileName = cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "_" + partes[0] + "_" + partes[1] + ".pdf";
                                    DialogResult result = sfdDescargar.ShowDialog();
                                    if (result == System.Windows.Forms.DialogResult.OK)
                                    {
                                        string fileName = sfdDescargar.FileName;
                                        if (fileName.Substring(fileName.Length - 4) != ".pdf")
                                        {
                                            fileName = fileName + ".pdf";
                                        }
                                        bool procesado = false;
                                        if (cabecera.Cs_pr_XML.Trim() != "")
                                        {
                                            procesado = RepresentacionImpresa.getRepresentacionImpresaDocumentoCargaTXT(fileName, cabecera, cabecera.Cs_pr_XML, datosImpresa, pathImage, localDB);
                                        }
                                        if (procesado)
                                        {
                                            System.Diagnostics.Process.Start(fileName);
                                        }
                                        else
                                        {
                                            System.Windows.Forms.MessageBox.Show("Ha ocurrido un error al procesar la representacion impresa.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("No se encuentra la imagen del logo y/o la información para la representacion impresa. Verifique la existencia de la imagen 'logo.png' y el archivo 'informacionImpreso.txt'  en la ruta de instalación.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            clsBaseLog.cs_pxRegistarAdd("pdf repimpresa" + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistar("Error al Cargar el Documento. Error: " + ex.ToString());
            }
            /*Se arma el archivo PDF para que sea visualizado en un Visor de PDF*/
        }
        /*NUEVO FIN*/

        private void chkDiscontinue_CheckedRC(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            DocumentoCompra comprobante = (DocumentoCompra)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked)
            {
                comprobante.Check = true;
            }
            e.Handled = true;
        }

        private void chkDiscontinue_UncheckedRC(object sender, RoutedEventArgs e)
        {
            //Obtener el elemento seleccionado.
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)e.OriginalSource;
            //Obtener la fila seleccionada y objeto asociado.
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            DocumentoCompra comprobante = (DocumentoCompra)dataGridRow.DataContext;

            if ((bool)checkBox.IsChecked)
            {
                comprobante.Check = false;
            }
            e.Handled = true;
        }

        private void btnGrabarCambiosCompras(object sender, RoutedEventArgs e)
        {
            //verificar que haya docs listados
            if (lista_reporte.Count > 0)
            {
                foreach (DocumentoCompra comprobante in lista_reporte)
                {
                    clsEntidadDocument doc = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(comprobante.Id);
                    DateTime datetime;
                    bool valid = DateTime.TryParse(comprobante.FechaVencimiento, out datetime);
                    if (valid)
                    {
                        DateTime dt11 = DateTime.Parse(comprobante.FechaVencimiento);
                        doc.Cs_cr_FechaVencimiento = dt11.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        doc.Cs_cr_FechaVencimiento = "";
                    }
                    bool valid2 = DateTime.TryParse(comprobante.DocReferenciaFecha, out datetime);
                    if (valid2)
                    {
                        DateTime dt21 = DateTime.Parse(comprobante.DocReferenciaFecha);
                        doc.Cs_cr_DocReferenciaFecha = dt21.ToString("yyyy-MM-dd");
                        // doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
                    }
                    else
                    {
                        doc.Cs_cr_DocReferenciaFecha = "";
                    }
                    bool valid21 = DateTime.TryParse(comprobante.ConstDepDetFecha, out datetime);
                    if (valid21)
                    {
                        DateTime dt211 = DateTime.Parse(comprobante.ConstDepDetFecha);
                        doc.Cs_cr_ConstDepDetFecha = dt211.ToString("yyyy-MM-dd");
                        // doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
                    }
                    else
                    {
                        doc.Cs_cr_ConstDepDetFecha = "";
                    }
                    bool valid3 = DateTime.TryParse(comprobante.FechaVencimientoDos, out datetime);
                    if (valid3)
                    {
                        DateTime dt31 = DateTime.Parse(comprobante.FechaVencimientoDos);
                        doc.Cs_cr_FechaVencimientoDos = dt31.ToString("yyyy-MM-dd");
                        //doc.Cs_cr_FechaVencimientoDos = comprobante.FechaVencimientoDos;
                    }
                    else
                    {
                        doc.Cs_cr_FechaVencimientoDos = "";
                    }

                    bool valid4 = DateTime.TryParse(comprobante.FechaDocumentoRegimenEspecial, out datetime);
                    if (valid4)
                    {
                        DateTime dt41 = DateTime.Parse(comprobante.FechaDocumentoRegimenEspecial);
                        doc.Cs_cr_FechaDocumentoRegimenEspecial = dt41.ToString("yyyy-MM-dd");
                        // doc.Cs_cr_FechaDocumentoRegimenEspecial = comprobante.FechaDocumentoRegimenEspecial;
                    }
                    else
                    {
                        doc.Cs_cr_FechaDocumentoRegimenEspecial = "";
                    }
                    if (comprobante.AnioEmisionDUA.Trim().Length == 4)
                    {
                        doc.Cs_cr_AnioEmisionDUA = comprobante.AnioEmisionDUA;
                    }
                    else
                    {
                        doc.Cs_cr_AnioEmisionDUA = "";
                    }

                    //doc.Cs_cr_AnioEmisionDUA = comprobante.AnioEmisionDUA;
                    float f;
                    if (float.TryParse(comprobante.AdqGravadasBaseImponibleGravadasNoGravadas.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = comprobante.AdqGravadasBaseImponibleGravadasNoGravadas.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = "";
                    }

                    if (float.TryParse(comprobante.AdqGravadasIGVGravadasNoGravados.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_AdqGravadasIGVGravadasNoGravados = comprobante.AdqGravadasIGVGravadasNoGravados.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_AdqGravadasIGVGravadasNoGravados = "";
                    }

                    if (float.TryParse(comprobante.AdqGravadasBaseImponibleNoGravados.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_AdqGravadasBaseImponibleNoGravados = comprobante.AdqGravadasBaseImponibleNoGravados.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_AdqGravadasBaseImponibleNoGravados = "";
                    }

                    if (float.TryParse(comprobante.AdqGravadasIGVNoGravados.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_AdqGravadasIGVNoGravados = comprobante.AdqGravadasIGVNoGravados.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_AdqGravadasIGVNoGravados = "";
                    }

                    if (comprobante.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Length > 20)
                    {
                        doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado.Trim().Substring(0, 20);

                    }
                    else
                    {
                        doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado;
                    }
                    // doc.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = comprobante.NumeroComprobantePagoSujetoNoDomiciliado;
                    if (comprobante.ConstDepDetNumero.Trim().Length > 20)
                    {
                        doc.Cs_cr_ConstDepDetNumero = comprobante.ConstDepDetNumero.Trim().Substring(0, 20);
                        faltantes++;
                    }
                    else
                    {
                        doc.Cs_cr_ConstDepDetNumero = comprobante.ConstDepDetNumero;
                    }

                    // doc.Cs_cr_ConstDepDetFecha = comprobante.ConstDepDetFecha;
                    if (float.TryParse(comprobante.TipoCambio.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_TipoCambio = comprobante.TipoCambio.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_TipoCambio = "";
                    }

                    //doc.Cs_cr_DocReferenciaFecha = comprobante.DocReferenciaFecha;
                    if (float.TryParse(comprobante.EquivalenteDolares.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_EquivalenteDolares = comprobante.EquivalenteDolares.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_EquivalenteDolares = "";
                    }

                    // doc.Cs_cr_FechaVencimientoDos = comprobante.FechaVencimientoDos;
                    doc.Cs_cr_CondicionCompra = comprobante.CondicionCompra;


                    if (comprobante.CtaContableBaseImponible.Trim().Length > 10)
                    {
                        doc.Cs_cr_CtaContableBaseImponible = comprobante.CtaContableBaseImponible.Trim().Substring(0, 10);
                    }
                    else
                    {
                        doc.Cs_cr_CtaContableBaseImponible = comprobante.CtaContableBaseImponible;
                    }

                    if (comprobante.CtaContableOtrosTributos.Trim().Length > 10)
                    {
                        doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos.Trim().Substring(0, 10);
                    }
                    else
                    {
                        doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos;
                    }

                    //doc.Cs_cr_CtaContableOtrosTributos = comprobante.CtaContableOtrosTributos;

                    if (comprobante.CtaContableTotal.Trim().Length > 10)
                    {
                        doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal.Trim().Substring(0, 10);
                    }
                    else
                    {
                        doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal;
                    }

                    // doc.Cs_cr_CtaContableTotal = comprobante.CtaContableTotal;
                    if (comprobante.CentroCostosUno.Trim().Length > 9)
                    {
                        doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno.Trim().Substring(0, 9);
                    }
                    else
                    {
                        doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno.Trim();
                    }
                    // doc.Cs_cr_CentroCostosUno = comprobante.CentroCostosUno;
                    if (comprobante.CentroCostosDos.Trim().Length > 9)
                    {
                        doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos.Trim().Substring(0, 9);
                    }
                    else
                    {
                        doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos.Trim();
                    }
                    // doc.Cs_cr_CentroCostosDos = comprobante.CentroCostosDos;

                    doc.Cs_cr_RegimenEspecial = comprobante.RegimenEspecial;
                    //doc.Cs_cr_PorcentajeRegimenEspecial = comprobante.PorcentajeRegimenEspecial;
                    //doc.Cs_cr_ImporteRegimenEspecial = comprobante.ImporteRegimenEspecial;

                    if (comprobante.SerieDocumentoRegimenEspecial.Trim().Length > 6)
                    {
                        doc.Cs_cr_SerieDocumentoRegimenEspecial = comprobante.SerieDocumentoRegimenEspecial.Trim().Substring(0, 6);
                    }
                    else
                    {
                        doc.Cs_cr_SerieDocumentoRegimenEspecial = comprobante.SerieDocumentoRegimenEspecial;
                    }

                    if (comprobante.NumeroDocumentoRegimenEspecial.Trim().Length > 13)
                    {
                        doc.Cs_cr_NumeroDocumentoRegimenEspecial = comprobante.NumeroDocumentoRegimenEspecial.Trim().Substring(0, 13);
                    }
                    else
                    {
                        doc.Cs_cr_NumeroDocumentoRegimenEspecial = comprobante.NumeroDocumentoRegimenEspecial;
                    }


                    //doc.Cs_cr_FechaDocumentoRegimenEspecial = comprobante.FechaDocumentoRegimenEspecial;

                    if (comprobante.CodigoPresupuesto.Trim().Length > 10)
                    {
                        doc.Cs_cr_CodigoPresupuesto = comprobante.CodigoPresupuesto.Trim().Substring(0, 10);
                    }
                    else
                    {
                        doc.Cs_cr_CodigoPresupuesto = comprobante.CodigoPresupuesto;
                    }

                    if (float.TryParse(comprobante.PorcentajeIGV.Trim().Replace(',', '.'), out f))
                    {
                        doc.Cs_cr_PorcentajeIGV = comprobante.PorcentajeIGV.Replace(',', '.');
                    }
                    else
                    {
                        doc.Cs_cr_PorcentajeIGV = "";
                    }
                    if (comprobante.Glosa.Trim().Length > 60)
                    {
                        doc.Cs_cr_Glosa = comprobante.Glosa.Trim().Substring(0, 60);
                    }
                    else
                    {
                        doc.Cs_cr_Glosa = comprobante.Glosa;
                    }

                    // doc.Cs_cr_Glosa = comprobante.Glosa;
                    doc.Cs_cr_CondicionPercepcion = comprobante.CondicionPercepcion;
                    //doc.Cs_cr_ImporteRegimenEspecial = comprobante.ImporteRegimenEspecial;

                    doc.cs_pxActualizar(false, false);

                    //actualizar en los items
                    /*clsEntidadDocument_Line doclinea = new clsEntidadDocument_Line(localDB).cs_pxObtenerUnoPorId(comprobante.IdLinea);
                    doclinea.Cs_cr_UnidadMedida = comprobante.ProductoUnidadMedida;
                    doclinea.Cs_cr_TransferenciaGratuitaMotivo = comprobante.TransGratuitaMotivo;
                    doclinea.Cs_cr_TransferenciaGratuitaValorReferencia = comprobante.TransGratuitaValorReferencia;
                    doclinea.cs_pxActualizar(false, true);*/
                }

                clsBaseMensaje.cs_pxMsgInformation("Mensaje", "Los registros han sido actualizados en la base de datos."); ;

            }
            else
            {
                clsBaseMensaje.cs_pxMsgAdvertencia("Atención", "Debe existir registros listados para usar esta opcion");
            }




            /* if (periodoCompras == true)
             {
                 ComboBoxPares valueMes = (ComboBoxPares)periodoMesRCompras.SelectedItem;
                 ComboBoxPares valueAnio = (ComboBoxPares)periodoAnioRCompras.SelectedItem;
                 actualizarGrilla(valueMes._Id+valueAnio._Id);
             }
             else
             {
                 actualizarGrilla("");
             }*/
        }

        private void btnDescartarRCompras_Click(object sender, RoutedEventArgs e)
        {

            if (lista_reporte.Count > 0)
            {
                List<string> ids = new List<string>();
                //checkall
                foreach (DocumentoCompra item in lista_reporte)
                {
                    if (item.Check == true)
                    {
                        ids.Add(item.Id);
                    }
                }

                if (ids.Count > 0)
                {
                    foreach (string id in ids)
                    {
                        new clsEntidadDocument(localDB).cs_pxEliminarDocumento(id);
                    }
                    System.Windows.Forms.MessageBox.Show("Los documentos seleccionados han sido eliminados.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (periodoCompras == true)
                    {
                        ComboBoxPares valueMesR = (ComboBoxPares)periodoMesRCompras.SelectedItem;
                        ComboBoxPares valueAnioR = (ComboBoxPares)periodoAnioRCompras.SelectedItem;
                        actualizarGrilla(valueMesR._Id + valueAnioR._Id);
                    }
                    else
                    {
                        actualizarGrilla("");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Seleccione al menos un documento.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
