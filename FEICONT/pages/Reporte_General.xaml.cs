using FEICONT.ayuda;
using FEICONT.Base;
using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Jordy Amaro 11-01-17 FEICONT2-4
/// Cambio de interfaz - Reportes de comprobantes en general.
/// </summary>
namespace FEICONT.pages
{
    /// <summary>
    /// Lógica de interacción para Reporte_General.xaml
    /// </summary>
    public partial class Reporte_General : Page
    {
        List<clsEntityDocument> registros;
        List<ReporteGeneral> lista_reporte_facturas;
        List<ReporteGeneral> lista_reporte_boletas;
        List<ComboBoxPares> tipos_reporte = new List<ComboBoxPares>();
        ReporteGeneral itemRow;
        string fecha_inicio_formato;
        string fecha_fin_formato;
        DateTime fecha_inicio;
        DateTime fecha_fin;
        int facturas_emitidos;
        int facturas_aceptadas;
        int facturas_sinestado;
        int facturas_rechazado;
        int facturas_debaja;

        int fcredito_emitidos;
        int fcredito_aceptadas;
        int fcredito_sinestado;
        int fcredito_rechazado;
        int fcredito_debaja;

        int fdebito_emitidos;
        int fdebito_aceptadas;
        int fdebito_sinestado;
        int fdebito_rechazado;
        int fdebito_debaja;

        int boletas_emitidos;
        int boletas_aceptadas;
        int boletas_sinestado;
        int boletas_rechazado;
        int boletas_debaja;

        int bcredito_emitidos;
        int bcredito_aceptadas;
        int bcredito_sinestado;
        int bcredito_rechazado;
        int bcredito_debaja;

        int bdebito_emitidos;
        int bdebito_aceptadas;
        int bdebito_sinestado;
        int bdebito_rechazado;
        int bdebito_debaja;
        private clsEntityDatabaseLocal localDB;
        //Metodo constructor.
        public Reporte_General(clsEntityDatabaseLocal local)
        {
            InitializeComponent();
            localDB = local;
        }
        //Evento de carga de  ventana.
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Agregar valores al combobox de tipos de reporte.
            tipos_reporte.Add(new ComboBoxPares("0", "PDF"));
            tipos_reporte.Add(new ComboBoxPares("1", "CSV"));
            cboDownload.DisplayMemberPath = "_Value";
            cboDownload.SelectedValuePath = "_key";
            cboDownload.SelectedIndex = 0;
            cboDownload.ItemsSource = tipos_reporte;
            datePick_inicio.Text = DateTime.Now.Date.ToString();
            datePick_fin.Text = DateTime.Now.Date.ToString();
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }

            cs_pxCargarDgvComprobanteselectronicos(fecha_inicio_formato, fecha_fin_formato);
        }
        //Metodo para cargar los datos en la grilla.
        private void cs_pxCargarDgvComprobanteselectronicos(string fechainicio, string fechafin)
        {
            dgComprobantesFacturas.ItemsSource = null;
            dgComprobantesFacturas.Items.Clear();
            lista_reporte_facturas = new List<ReporteGeneral>();
            //Obtener comporbantes segun fecha selecionada para facturas y notas.
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroReporteGeneral("","","", fechainicio, fechafin, "'01','07','08'", false);
            facturas_emitidos = 0;
            facturas_aceptadas=0;
            facturas_sinestado=0;
            facturas_rechazado=0;
            facturas_debaja=0;

            fcredito_emitidos=0;
            fcredito_aceptadas=0;
            fcredito_sinestado=0;
            fcredito_rechazado=0;
            fcredito_debaja=0;

            fdebito_emitidos=0;
            fdebito_aceptadas=0;
            fdebito_sinestado=0;
            fdebito_rechazado=0;
            fdebito_debaja=0;
            //si existen registros
            if (registros != null)
            {
                //Recorrer los registros y realizar los calculos para mostrar en el resumen general.
                foreach (var item in registros)
                {
                    if (item.Cs_tag_InvoiceTypeCode == "01")
                    {
                        facturas_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                facturas_aceptadas++;
                                break;
                            case "1":
                                facturas_rechazado++;
                                break;
                            case "2":
                                facturas_sinestado++;
                                break;
                            case "3":
                                facturas_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                    if (item.Cs_tag_InvoiceTypeCode == "07")
                    {
                        fcredito_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                fcredito_aceptadas++;
                                break;
                            case "1":
                                fcredito_rechazado++;
                                break;
                            case "2":
                                fcredito_sinestado++;
                                break;
                            case "3":
                                fcredito_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                    if (item.Cs_tag_InvoiceTypeCode == "08")
                    {
                        fdebito_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                fdebito_aceptadas++;
                                break;
                            case "1":
                                fdebito_rechazado++;
                                break;
                            case "2":
                                fdebito_sinestado++;
                                break;
                            case "3":
                                fdebito_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
           
            //Adicionar informacion de factura a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "FACTURA ELECTRONICA";
            itemRow.Tipo = "01";
            itemRow.Rechazado = facturas_rechazado.ToString();
            itemRow.SinEstado = facturas_sinestado.ToString();
            itemRow.DeBaja = facturas_debaja.ToString();
            itemRow.Aceptado = facturas_aceptadas.ToString();
            itemRow.Emitidos = facturas_emitidos.ToString();
            lista_reporte_facturas.Add(itemRow);
            //Adicionar informacion de nota de credito a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "NOTA DE CREDITO ELECTRONICA";
            itemRow.Tipo = "07";
            itemRow.Rechazado = fcredito_rechazado.ToString();
            itemRow.SinEstado = fcredito_sinestado.ToString();
            itemRow.DeBaja = fcredito_debaja.ToString();
            itemRow.Aceptado = fcredito_aceptadas.ToString();
            itemRow.Emitidos = fcredito_emitidos.ToString();
            lista_reporte_facturas.Add(itemRow);
            //Adicionar informacion de factura a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "NOTA DE DEBITO ELECTRONICA";
            itemRow.Tipo = "08";
            itemRow.Rechazado = fdebito_rechazado.ToString();
            itemRow.SinEstado = fdebito_sinestado.ToString();
            itemRow.DeBaja = fdebito_debaja.ToString();
            itemRow.Aceptado = fdebito_aceptadas.ToString();
            itemRow.Emitidos = fdebito_emitidos.ToString();
            lista_reporte_facturas.Add(itemRow);

            dgComprobantesFacturas.ItemsSource = lista_reporte_facturas;


            dgComprobantesBoletas.ItemsSource = null;
            dgComprobantesBoletas.Items.Clear();
            lista_reporte_boletas = new List<ReporteGeneral>();
            //Obtener comporbantes segun fecha selecionada para boletas y notas.
            registros = new clsEntityDocument(localDB).cs_pxObtenerFiltroReporteGeneral("", "", "", fechainicio, fechafin, "'03','07','08'", true);
            boletas_emitidos = 0;
            boletas_aceptadas = 0;
            boletas_sinestado = 0;
            boletas_rechazado = 0;
            boletas_debaja = 0;

            bcredito_emitidos = 0;
            bcredito_aceptadas = 0;
            bcredito_sinestado = 0;
            bcredito_rechazado = 0;
            bcredito_debaja = 0;

            bdebito_emitidos = 0;
            bdebito_aceptadas = 0;
            bdebito_sinestado = 0;
            bdebito_rechazado = 0;
            bdebito_debaja = 0;
            //Si existen registros
            if (registros != null) {
                //Recorrer los registros para realizar los calculos y mostrar en el resumen.
                foreach (var item in registros)
                {
                    if (item.Cs_tag_InvoiceTypeCode == "03")
                    {
                        boletas_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                boletas_aceptadas++;
                                break;
                            case "1":
                                boletas_rechazado++;
                                break;
                            case "2":
                                boletas_sinestado++;
                                break;
                            case "3":
                                boletas_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                    if (item.Cs_tag_InvoiceTypeCode == "07")
                    {
                        bcredito_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                bcredito_aceptadas++;
                                break;
                            case "1":
                                bcredito_rechazado++;
                                break;
                            case "2":
                                bcredito_sinestado++;
                                break;
                            case "3":
                                bcredito_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                    if (item.Cs_tag_InvoiceTypeCode == "08")
                    {
                        bdebito_emitidos++;
                        switch (item.Cs_pr_EstadoSUNAT)
                        {
                            case "0":
                                bdebito_aceptadas++;
                                break;
                            case "1":
                                bdebito_rechazado++;
                                break;
                            case "2":
                                bdebito_sinestado++;
                                break;
                            case "3":
                                bdebito_debaja++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
         
            //Adicionar informacion de boleta a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "BOLETA DE VENTA ELECTRONICA";
            itemRow.Tipo = "03";
            itemRow.Rechazado = boletas_rechazado.ToString();
            itemRow.SinEstado = boletas_sinestado.ToString();
            itemRow.DeBaja = boletas_debaja.ToString();
            itemRow.Aceptado = boletas_aceptadas.ToString();
            itemRow.Emitidos = boletas_emitidos.ToString();
            lista_reporte_boletas.Add(itemRow);
            //Adicionar informacion de nota de credito a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "NOTA DE CREDITO ELECTRONICA";
            itemRow.Tipo = "07";
            itemRow.Rechazado = bcredito_rechazado.ToString();
            itemRow.SinEstado = bcredito_sinestado.ToString();
            itemRow.DeBaja = bcredito_debaja.ToString();
            itemRow.Aceptado = bcredito_aceptadas.ToString();
            itemRow.Emitidos = bcredito_emitidos.ToString();
            lista_reporte_boletas.Add(itemRow);
            //Adicionar informacion de factura a grid.
            itemRow = new ReporteGeneral();
            itemRow.TipoTexto = "NOTA DE DEBITO ELECTRONICA";
            itemRow.Tipo = "08";
            itemRow.Rechazado = bdebito_rechazado.ToString();
            itemRow.SinEstado = bdebito_sinestado.ToString();
            itemRow.DeBaja = bdebito_debaja.ToString();
            itemRow.Aceptado = bdebito_aceptadas.ToString();
            itemRow.Emitidos = bdebito_emitidos.ToString();
            lista_reporte_boletas.Add(itemRow);

            dgComprobantesBoletas.ItemsSource = lista_reporte_boletas;
        }
        //Evento para consultar el filtro de documentos.
        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            if (datePick_inicio.SelectedDate != null)
            {
                fecha_inicio = (DateTime)datePick_inicio.SelectedDate;
                fecha_inicio_formato = fecha_inicio.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_inicio_formato = string.Empty;
            }
            if (datePick_fin.SelectedDate != null)
            {
                fecha_fin = (DateTime)datePick_fin.SelectedDate;
                fecha_fin_formato = fecha_fin.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha_fin_formato = string.Empty;
            }

            cs_pxCargarDgvComprobanteselectronicos(fecha_inicio_formato, fecha_fin_formato);
        }
        //Evento para descargar reporte en csv y pdf.
        private void btnReporte_Click(object sender, RoutedEventArgs e)
        {
            if (lista_reporte_facturas.Count > 0 || lista_reporte_boletas.Count>0)
            {
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                switch (cboDownload.SelectedIndex)
                {
                    case 0:
                        clsBaseReporte.cs_pxReportePDF_General(lista_reporte_facturas,lista_reporte_boletas, configuracion.cs_prRutareportesPDF + "\\GENERAL-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".pdf");
                        break;
                    case 1:
                        clsBaseReporte.cs_pxReporteCSV_General(lista_reporte_facturas,lista_reporte_boletas, configuracion.cs_prRutareportesCSV + "\\GENERAL-" + DateTime.Today.Date.ToShortDateString().Replace('/', '-') + "--" + DateTime.Now.ToShortTimeString().Replace(':', '-') + ".csv");
                        break;

                }
            }
            else
            {
                clsBaseMensaje.cs_pxMsg("Error al generar reporte", "Debe existir datos listados para generar el reporte.");
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int compare = e.Key.CompareTo(System.Windows.Input.Key.F12);
            if (compare == 0)
            {
                AyudaPrincipal ayuda = new AyudaPrincipal("9");
                ayuda.ShowDialog();
            }
        }
    }
}
