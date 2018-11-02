using Extension.Base;
using Extension.Datos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FEICONT.Base
{
    public class clsBaseReporte
    {
        #region reporte facturas 
        public static void cs_pxReporteCSV(List<ReporteDocumento> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";
              
                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Codigo , Tipo de Comprobante , Serie-Numero , Estado SCC , Estado Sunat , Fecha Emision , Fecha Envio , Ruc Cliente , Razon Social Cliente , Comentario Sunat";
                sw0.Write(value);
                foreach (ReporteDocumento item in dgv)
                {
                   
                 
                    string textoNormalizado = item.TipoTexto.Normalize(NormalizationForm.FormD);
                    //coincide todo lo que no sean letras y números ascii o espacio
                    //y lo reemplazamos por una cadena vacía.
                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                    string textoSinAcentos = reg.Replace(textoNormalizado, "");

                    sw0.WriteLine();
                    value = item.Tipo + " , " + textoSinAcentos + " , " +item.SerieNumero+ " , " + item.EstadoSCC + "," + item.EstadoSunat + "," + item.FechaEmision + "," + item.FechaEnvio + ","+item.Ruc+"," +item.RazonSocial+","+ item.Comentario.Replace(","," ");
                    sw0.Write(value);
                    
                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }

        public static void cs_pxReportePDF(List<ReporteDocumento> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE COMPROBANTES ELECTRÓNICOS - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(10);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c0 = new PdfPCell(new Phrase("Codigo", fuente));
                    cell_c0.BackgroundColor = BaseColor.GRAY;
                    cell_c0.BorderWidth = 1;
                    tblPdf.AddCell(cell_c0);

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Tipo Comprobante", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Serie - Numero", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Fecha de envio", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Ruc", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);

                    PdfPCell cell_c8 = new PdfPCell(new Phrase("Razon Social", fuente));
                    cell_c8.BackgroundColor = BaseColor.GRAY;
                    cell_c8.BorderWidth = 1;
                    tblPdf.AddCell(cell_c8);

                    PdfPCell cell_c9 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c9.BackgroundColor = BaseColor.GRAY;
                    cell_c9.BorderWidth = 1;
                    tblPdf.AddCell(cell_c9);

                    foreach (ReporteDocumento row in dgv)
                    {
                        PdfPCell cell_0 = new PdfPCell(new Phrase(row.Tipo, fuente));
                        cell_0.BorderWidth = 1;
                        tblPdf.AddCell(cell_0);

                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.TipoTexto, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.SerieNumero, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.FechaEnvio, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Ruc, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);

                        PdfPCell cell_8 = new PdfPCell(new Phrase(row.RazonSocial, fuente));
                        cell_8.BorderWidth = 1;
                        tblPdf.AddCell(cell_8);

                        PdfPCell cell_9 = new PdfPCell(new Phrase(row.Comentario, fuente));
                        cell_9.BorderWidth = 1;
                        tblPdf.AddCell(cell_9);
                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        #endregion
        #region reporte retencion
        public static void cs_pxReporteCSV_Retencion(List<ReporteRetention> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Comprobante , Estado SCC , Estado Sunat ,Serie-Numero Relacionado ,Fecha Emision , Monto de Pago , Importe Retenido , Monto Total , Comentario Sunat";
                sw0.Write(value);
                foreach (ReporteRetention item in dgv)
                {
                    sw0.WriteLine();
                    value = item.SerieNumero + " , " + item.EstadoSCC + "," + item.EstadoSunat + ","+item.SerieNumeroRelacionado+"," + item.FechaEmision + "," + item.MontoPago + "," + item.MontoRetencion + "," + item.MontoTotal + "," + item.Comentario.Replace(",", " ");
                    sw0.Write(value);

                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }

        public static void cs_pxReportePDF_Retencion(List<ReporteRetention> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE COMPROBANTES ELECTRÓNICOS DE RETENCION - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(9);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c0 = new PdfPCell(new Phrase("Serie - Numero", fuente));
                    cell_c0.BackgroundColor = BaseColor.GRAY;
                    cell_c0.BorderWidth = 1;
                    tblPdf.AddCell(cell_c0);

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Serie-Numero Relacionado", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Monto de Pago", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Importe Retenido", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Monto Total", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);

                    PdfPCell cell_c8 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c8.BackgroundColor = BaseColor.GRAY;
                    cell_c8.BorderWidth = 1;
                    tblPdf.AddCell(cell_c8);

                    foreach (ReporteRetention row in dgv)
                    {
                        PdfPCell cell_0 = new PdfPCell(new Phrase(row.SerieNumero, fuente));
                        cell_0.BorderWidth = 1;
                        tblPdf.AddCell(cell_0);

                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.SerieNumeroRelacionado, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.MontoPago, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.MontoRetencion, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.MontoTotal, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);

                        PdfPCell cell_8 = new PdfPCell(new Phrase(row.Comentario, fuente));
                        cell_8.BorderWidth = 1;
                        tblPdf.AddCell(cell_8);

                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
#endregion
        #region reporte general
        public static void cs_pxReporteCSV_General(List<ReporteGeneral> dgvF, List<ReporteGeneral> dgvB,string archivo)
        {
            if (dgvF.Count > 0 && dgvB.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);

                sw0.WriteLine();
                value = "Tipo de Comprobante Facturas,Codigo,Aceptados,Rechazados,Sin Estado,De Baja,Emitidos";
                sw0.Write(value);
                foreach (ReporteGeneral item in dgvF)
                {
                    string textoNormalizado = item.TipoTexto.Normalize(NormalizationForm.FormD);
                    //coincide todo lo que no sean letras y números ascii o espacio
                    //y lo reemplazamos por una cadena vacía.
                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                    string textoSinAcentos = reg.Replace(textoNormalizado, "");

                    sw0.WriteLine();
                    value = textoSinAcentos + "," + item.Tipo + "," + item.Aceptado + "," + item.Rechazado + "," + item.SinEstado + "," + item.DeBaja + "," + item.Emitidos;
                    sw0.Write(value);
                }
                sw0.WriteLine();
                value = "";
                sw0.Write(value);
                sw0.WriteLine();
                value = "Tipo de Comprobante Boletas,Codigo,Aceptados,Rechazados,Sin Estado,De Baja,Emitidos";
                sw0.Write(value);
                foreach (ReporteGeneral item in dgvB)
                {
                    sw0.WriteLine();
                    value = item.TipoTexto + "," + item.Tipo + "," + item.Aceptado + "," + item.Rechazado + "," + item.SinEstado + "," + item.DeBaja + "," + item.Emitidos;
                    sw0.Write(value);
                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }
       
        public static void cs_pxReportePDF_General(List<ReporteGeneral> dgvF, List<ReporteGeneral> dgvB, string archivo)
        {
            try
            {
                if (dgvF.Count > 0 && dgvB.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("REPORTE GENERAL COMPROBANTES ELECTRÓNICOS - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(7);
                    tblPdf.WidthPercentage = 100;
                   
                    PdfPCell cell_c1 = new PdfPCell(new Phrase("COMPROBANTE", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("CODIGO", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("ACEPTADOS", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("RECHAZADOS", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("SIN ESTADO", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("DE BAJA", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("EMITIDOS", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);

                    PdfPCell cell_titulo_factura = new PdfPCell(new Phrase("Facturas electronicas", fuente));
                    cell_titulo_factura.BackgroundColor = BaseColor.DARK_GRAY;
                    cell_titulo_factura.BorderWidth = 1;
                    cell_titulo_factura.Colspan = 7;
                    tblPdf.AddCell(cell_titulo_factura);

                    foreach (ReporteGeneral row in dgvF)
                    {
                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.TipoTexto, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.Tipo, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.Aceptado, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.Rechazado, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.SinEstado, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.DeBaja, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Emitidos, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);
                    }
                    PdfPCell cell_titulo_boletas = new PdfPCell(new Phrase("Boletas de Venta ", fuente));
                    cell_titulo_boletas.BackgroundColor = BaseColor.DARK_GRAY;
                    cell_titulo_boletas.BorderWidth = 1;
                    cell_titulo_boletas.Colspan = 7;
                    tblPdf.AddCell(cell_titulo_boletas);
                    foreach (ReporteGeneral row in dgvB)
                    {
                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.TipoTexto, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.Tipo, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.Aceptado, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.Rechazado, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.SinEstado, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.DeBaja, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Emitidos, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);
                    }
                    doc.Add(tblPdf);
                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
#endregion 
        #region reporte resumen diario
        public static void cs_pxReporteCSV_resumenDetallado(clsEntityDatabaseLocal localDB, List<ReporteResumen> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Resumen Diario , Ticket , Estado SCC , Estado Sunat , Fecha Emision , Fecha Envio , Comentario Sunat, Codigo , Tipo de Comprobante , Serie-Numero , Fecha Emision ,Ruc Cliente , Razon Social Cliente";
                sw0.Write(value);
                foreach (ReporteResumen item in dgv)
                {                 
                    List<clsEntityDocument> documentos = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario_n(item.Id);
                    foreach (clsEntityDocument doc in documentos)
                    {
                        string tipoTexto= clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(doc.Cs_tag_InvoiceTypeCode);
                        string textoNormalizado = tipoTexto.Normalize(NormalizationForm.FormD);                   
                        Regex reg = new Regex("[^a-zA-Z0-9 ]");
                        string textoSinAcentos = reg.Replace(textoNormalizado, "");
                        sw0.WriteLine();
                        value = item.Archivo + "," + item.Ticket + "," + item.EstadoSCC + "," + item.EstadoSunat + "," + item.FechaEmision + "," + item.FechaEnvio + "," + item.Comentario.Replace(",", " ")+" , " + doc.Cs_tag_InvoiceTypeCode + " , " + textoSinAcentos + " , " + doc.Cs_tag_ID + " , " + doc.Cs_tag_IssueDate + ","+ doc.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + "," + doc.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
                        sw0.Write(value);
                    }

                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }
        public static void cs_pxReporteCSV_resumen(List<ReporteResumen> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Resumen Diario , Ticket , Estado SCC , Estado Sunat , Fecha Emision , Fecha Envio , Comentario Sunat";
                sw0.Write(value);
                foreach (ReporteResumen item in dgv)
                {

                    sw0.WriteLine();
                    value = item.Archivo + "," + item.Ticket + "," + item.EstadoSCC + "," + item.EstadoSunat + "," + item.FechaEmision + "," + item.FechaEnvio + "," + item.Comentario.Replace(",", " ");
                    sw0.Write(value);

                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }

        public static void cs_pxReportePDF_resumen(List<ReporteResumen> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE RESUMENES DIARIOS - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(7);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Resumen Diario", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Ticket", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Estado SUNAT", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Fecha de envio", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);
                   

                    foreach (ReporteResumen row in dgv)
                    {
                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.Archivo, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.Ticket, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.FechaEnvio, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Comentario, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);

                       
                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        public static void cs_pxReportePDF_resumenDetallado(clsEntityDatabaseLocal localDB, List<ReporteResumen> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE RESUMENES DIARIOS DETALLADO - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(13);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Resumen Diario", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Ticket", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Estado SUNAT", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Fecha de envio", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);

                    PdfPCell cell_c8 = new PdfPCell(new Phrase("Código", fuente));
                    cell_c8.BackgroundColor = BaseColor.GRAY;
                    cell_c8.BorderWidth = 1;
                    tblPdf.AddCell(cell_c8);

                    PdfPCell cell_c9 = new PdfPCell(new Phrase("Tipo de comprobante", fuente));
                    cell_c9.BackgroundColor = BaseColor.GRAY;
                    cell_c9.BorderWidth = 1;
                    tblPdf.AddCell(cell_c9);

                    PdfPCell cell_c10 = new PdfPCell(new Phrase("Serie-Numero", fuente));
                    cell_c10.BackgroundColor = BaseColor.GRAY;
                    cell_c10.BorderWidth = 1;
                    tblPdf.AddCell(cell_c10);
                    PdfPCell cell_c11 = new PdfPCell(new Phrase("Fecha Emision", fuente));
                    cell_c11.BackgroundColor = BaseColor.GRAY;
                    cell_c11.BorderWidth = 1;
                    tblPdf.AddCell(cell_c11);
                    PdfPCell cell_c12 = new PdfPCell(new Phrase("Ruc Cliente", fuente));
                    cell_c12.BackgroundColor = BaseColor.GRAY;
                    cell_c12.BorderWidth = 1;
                    tblPdf.AddCell(cell_c12);
                    PdfPCell cell_c13 = new PdfPCell(new Phrase("Razon Social Cliente", fuente));
                    cell_c13.BackgroundColor = BaseColor.GRAY;
                    cell_c13.BorderWidth = 1;
                    tblPdf.AddCell(cell_c13);


                    foreach (ReporteResumen row in dgv)
                    {                     
                        List<clsEntityDocument> documentos = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario_n(row.Id);
                        foreach (clsEntityDocument docs in documentos)
                        {
                            PdfPCell cell_1 = new PdfPCell(new Phrase(row.Archivo, fuente));
                            cell_1.BorderWidth = 1;
                            tblPdf.AddCell(cell_1);

                            PdfPCell cell_2 = new PdfPCell(new Phrase(row.Ticket, fuente));
                            cell_2.BorderWidth = 1;
                            tblPdf.AddCell(cell_2);

                            PdfPCell cell_3 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                            cell_3.BorderWidth = 1;
                            tblPdf.AddCell(cell_3);

                            PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                            cell_4.BorderWidth = 1;
                            tblPdf.AddCell(cell_4);

                            PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                            cell_5.BorderWidth = 1;
                            tblPdf.AddCell(cell_5);

                            PdfPCell cell_6 = new PdfPCell(new Phrase(row.FechaEnvio, fuente));
                            cell_6.BorderWidth = 1;
                            tblPdf.AddCell(cell_6);

                            PdfPCell cell_7 = new PdfPCell(new Phrase(row.Comentario, fuente));
                            cell_7.BorderWidth = 1;
                            tblPdf.AddCell(cell_7);

                            string tipoTexto = clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(docs.Cs_tag_InvoiceTypeCode);
                            PdfPCell cell_8 = new PdfPCell(new Phrase(docs.Cs_tag_InvoiceTypeCode, fuente));
                            cell_8.BorderWidth = 1;
                            tblPdf.AddCell(cell_8);
                                                                                 
                            PdfPCell cell_9 = new PdfPCell(new Phrase(tipoTexto, fuente));
                            cell_9.BorderWidth = 1;
                            tblPdf.AddCell(cell_9);

                            PdfPCell cell_10 = new PdfPCell(new Phrase(docs.Cs_tag_ID, fuente));
                            cell_10.BorderWidth = 1;
                            tblPdf.AddCell(cell_10);

                            PdfPCell cell_11 = new PdfPCell(new Phrase(docs.Cs_tag_IssueDate, fuente));
                            cell_11.BorderWidth = 1;
                            tblPdf.AddCell(cell_11);

                            PdfPCell cell_12 = new PdfPCell(new Phrase(docs.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID, fuente));
                            cell_12.BorderWidth = 1;
                            tblPdf.AddCell(cell_12);

                            PdfPCell cell_13 = new PdfPCell(new Phrase(docs.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName, fuente));
                            cell_13.BorderWidth = 1;
                            tblPdf.AddCell(cell_13);

                        }

                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        #endregion
        #region reporte comunicacion baja
        public static void cs_pxReporteCSV_baja(List<ReporteResumen> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Comunicacion de Baja , Ticket , Estado SCC , Estado Sunat , Fecha Emision , Fecha Envio , Comentario Sunat";
                sw0.Write(value);
                foreach (ReporteResumen item in dgv)
                {

                    sw0.WriteLine();
                    value = item.Archivo + "," + item.Ticket + "," + item.EstadoSCC + "," + item.EstadoSunat + "," + item.FechaEmision + "," + item.FechaEnvio + "," + item.Comentario.Replace(","," ");
                    sw0.Write(value);

                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }
        public static void cs_pxReporteCSV_bajaDetallado(clsEntityDatabaseLocal local, List<ReporteResumen> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Comunicacion de Baja , Ticket , Estado SCC , Estado Sunat , Fecha Emision , Fecha Envio , Comentario Sunat, Codigo , Tipo de Comprobante , Serie-Numero ,Estado Sunat, Fecha Emision ,Ruc Cliente , Razon Social Cliente";
                sw0.Write(value);
                foreach (ReporteResumen item in dgv)
                {
                    List<clsEntityDocument> documentos = new clsEntityDocument(local).cs_pxObtenerDocumentosPorComunicacionBaja_n(item.Id);
                    foreach (clsEntityDocument doc in documentos)
                    {
                        sw0.WriteLine();
                        value = item.Archivo + "," + item.Ticket + "," + item.EstadoSCC + "," + item.EstadoSunat + "," + item.FechaEmision + "," + item.FechaEnvio + "," + item.Comentario.Replace(",", " ")+ " , " + doc.Cs_tag_InvoiceTypeCode + " , " + clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(doc.Cs_tag_InvoiceTypeCode)+" , "+ doc.Cs_tag_ID+","+ clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(doc.Cs_pr_EstadoSUNAT)).ToUpper()+","+doc.Cs_tag_IssueDate+","+doc.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID+","+doc.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName;
                        sw0.Write(value);
                    }                
                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }

        public static void cs_pxReportePDF_baja(List<ReporteResumen> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE COMUNICACIONES DE BAJA - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(7);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Comunicacion de Baja", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Ticket", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Fecha de envio", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);

                    foreach (ReporteResumen row in dgv)
                    {
                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.Archivo, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.Ticket, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        PdfPCell cell_3 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                        cell_3.BorderWidth = 1;
                        tblPdf.AddCell(cell_3);

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);

                        PdfPCell cell_6 = new PdfPCell(new Phrase(row.FechaEnvio, fuente));
                        cell_6.BorderWidth = 1;
                        tblPdf.AddCell(cell_6);

                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Comentario, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);
                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        public static void cs_pxReportePDF_bajaDetallado(clsEntityDatabaseLocal local, List<ReporteResumen> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE COMUNICACIONES DE BAJA DETALLADO - FEI", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(14);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Comunicacion de Baja", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Ticket", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado SCC", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Fecha de envio", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    tblPdf.AddCell(cell_c7);
                 
                    PdfPCell cell_c8 = new PdfPCell(new Phrase("Codigo", fuente));
                    cell_c8.BackgroundColor = BaseColor.GRAY;
                    cell_c8.BorderWidth = 1;
                    tblPdf.AddCell(cell_c8);
                    PdfPCell cell_c9 = new PdfPCell(new Phrase("Tipo de comprobante", fuente));
                    cell_c9.BackgroundColor = BaseColor.GRAY;
                    cell_c9.BorderWidth = 1;
                    tblPdf.AddCell(cell_c9);
                    PdfPCell cell_c10 = new PdfPCell(new Phrase("Serie-Numero", fuente));
                    cell_c10.BackgroundColor = BaseColor.GRAY;
                    cell_c10.BorderWidth = 1;
                    tblPdf.AddCell(cell_c10);
                    PdfPCell cell_c11 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c11.BackgroundColor = BaseColor.GRAY;
                    cell_c11.BorderWidth = 1;
                    tblPdf.AddCell(cell_c11);
                    PdfPCell cell_c12 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c12.BackgroundColor = BaseColor.GRAY;
                    cell_c12.BorderWidth = 1;
                    tblPdf.AddCell(cell_c12);
                    PdfPCell cell_c13 = new PdfPCell(new Phrase("Nº Documento Cliente", fuente));
                    cell_c13.BackgroundColor = BaseColor.GRAY;
                    cell_c13.BorderWidth = 1;
                    tblPdf.AddCell(cell_c13);
                    PdfPCell cell_c14 = new PdfPCell(new Phrase("Razon Social Cliente", fuente));
                    cell_c14.BackgroundColor = BaseColor.GRAY;
                    cell_c14.BorderWidth = 1;
                    tblPdf.AddCell(cell_c14);

                    foreach (ReporteResumen row in dgv)
                    {
                        List<clsEntityDocument> documentos = new clsEntityDocument(local).cs_pxObtenerDocumentosPorComunicacionBaja_n(row.Id);
                        foreach (clsEntityDocument docs in documentos)
                        {
                            PdfPCell cell_1 = new PdfPCell(new Phrase(row.Archivo, fuente));
                            cell_1.BorderWidth = 1;
                            tblPdf.AddCell(cell_1);

                            PdfPCell cell_2 = new PdfPCell(new Phrase(row.Ticket, fuente));
                            cell_2.BorderWidth = 1;
                            tblPdf.AddCell(cell_2);

                            PdfPCell cell_3 = new PdfPCell(new Phrase(row.EstadoSCC, fuente));
                            cell_3.BorderWidth = 1;
                            tblPdf.AddCell(cell_3);

                            PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoSunat, fuente));
                            cell_4.BorderWidth = 1;
                            tblPdf.AddCell(cell_4);

                            PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                            cell_5.BorderWidth = 1;
                            tblPdf.AddCell(cell_5);

                            PdfPCell cell_6 = new PdfPCell(new Phrase(row.FechaEnvio, fuente));
                            cell_6.BorderWidth = 1;
                            tblPdf.AddCell(cell_6);

                            PdfPCell cell_7 = new PdfPCell(new Phrase(row.Comentario, fuente));
                            cell_7.BorderWidth = 1;
                            tblPdf.AddCell(cell_7);

                            PdfPCell cell_8 = new PdfPCell(new Phrase(docs.Cs_tag_InvoiceTypeCode, fuente));
                            cell_8.BorderWidth = 1;
                            tblPdf.AddCell(cell_8);

                            PdfPCell cell_9 = new PdfPCell(new Phrase(clsBaseUtil.cs_fxComprobantesElectronicos_descripcion(docs.Cs_tag_InvoiceTypeCode), fuente));
                            cell_9.BorderWidth = 1;
                            tblPdf.AddCell(cell_9);

                            PdfPCell cell_10 = new PdfPCell(new Phrase(docs.Cs_tag_ID, fuente));
                            cell_10.BorderWidth = 1;
                            tblPdf.AddCell(cell_10);

                            PdfPCell cell_11 = new PdfPCell(new Phrase(clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt16(docs.Cs_pr_EstadoSUNAT)).ToUpper(), fuente));
                            cell_11.BorderWidth = 1;
                            tblPdf.AddCell(cell_11);

                            PdfPCell cell_12 = new PdfPCell(new Phrase(docs.Cs_tag_IssueDate, fuente));
                            cell_12.BorderWidth = 1;
                            tblPdf.AddCell(cell_12);

                            PdfPCell cell_13 = new PdfPCell(new Phrase(docs.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID, fuente));
                            cell_13.BorderWidth = 1;
                            tblPdf.AddCell(cell_13);

                            PdfPCell cell_14 = new PdfPCell(new Phrase(docs.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName, fuente));
                            cell_14.BorderWidth = 1;
                            tblPdf.AddCell(cell_14);

                        }
                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        #endregion
        #region reportevalidar
        /// <summary>
        /// Metodo para generar un reporte CSV en la opcion validar
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="archivo"></param>
        public static void cs_pxReporteCSV_Validar(List<DocumentoValidar> dgv, string archivo)
        {
            if (dgv.Count > 0)
            {
                string value = "";

                StreamWriter sw0 = new StreamWriter(archivo);
                sw0.WriteLine();
                value = "Codigo , Tipo de Comprobante , Serie-Numero ,Estado Sunat , Fecha Emision  , Ruc Cliente , Razon Social Cliente , Comentario Sunat";
                sw0.Write(value);
                foreach (DocumentoValidar item in dgv)
                {


                    string textoNormalizado = item.TipoTexto.Normalize(NormalizationForm.FormD);
                    //coincide todo lo que no sean letras y números ascii o espacio
                    //y lo reemplazamos por una cadena vacía.
                    Regex reg = new Regex("[^a-zA-Z0-9 ]");
                    string textoSinAcentos = reg.Replace(textoNormalizado, "");

                    string textoNormalizadoDos = item.Comentario.Replace(" , ", " ").Normalize(NormalizationForm.FormD);
                    //coincide todo lo que no sean letras y números ascii o espacio
                    //y lo reemplazamos por una cadena vacía.
                    string textoSinAcentosDos = reg.Replace(textoNormalizadoDos, "");

                    sw0.WriteLine();
                    value = item.Tipo + " , " + textoSinAcentos + " , " + item.SerieNumero + " , " + item.EstadoValidarTexto + " , " + item.FechaEmision + " , " + item.Ruc + " , " + item.RazonSocial + " , " + textoSinAcentosDos;
                    sw0.Write(value);

                }
                sw0.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE12");
            }
        }
        /// <summary>
        /// Metodo para generar un reporte PDF en la opcion validar
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="archivo"></param>
        public static void cs_pxReportePDF_Validar(List<DocumentoValidar> dgv, string archivo)
        {
            try
            {
                if (dgv.Count > 0)
                {
                    Document doc = new Document(PageSize.A4.Rotate());

                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(archivo, FileMode.Create));
                    doc.Open();
                    doc.Add(new Paragraph("LISTADO DE COMPROBANTES - RECEPTOR VALIDAR", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.BLACK)));
                    doc.Add(Chunk.NEWLINE);

                    Font fuente = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);

                    PdfPTable tblPdf = new PdfPTable(9);
                    tblPdf.WidthPercentage = 100;

                    PdfPCell cell_c0 = new PdfPCell(new Phrase("Codigo", fuente));
                    cell_c0.BackgroundColor = BaseColor.GRAY;
                    cell_c0.BorderWidth = 1;
                    tblPdf.AddCell(cell_c0);

                    PdfPCell cell_c1 = new PdfPCell(new Phrase("Tipo Comprobante", fuente));
                    cell_c1.BackgroundColor = BaseColor.GRAY;
                    cell_c1.BorderWidth = 1;
                    tblPdf.AddCell(cell_c1);

                    PdfPCell cell_c2 = new PdfPCell(new Phrase("Serie - Numero", fuente));
                    cell_c2.BackgroundColor = BaseColor.GRAY;
                    cell_c2.BorderWidth = 1;
                    tblPdf.AddCell(cell_c2);

                    /*PdfPCell cell_c31 = new PdfPCell(new Phrase("Estado Verificar Sunat", fuente));
                    cell_c31.BackgroundColor = BaseColor.GRAY;
                    cell_c31.BorderWidth = 1;
                    tblPdf.AddCell(cell_c31);*/

                    PdfPCell cell_c3 = new PdfPCell(new Phrase("Estado Sunat", fuente));
                    cell_c3.BackgroundColor = BaseColor.GRAY;
                    cell_c3.BorderWidth = 1;
                    tblPdf.AddCell(cell_c3);

                    PdfPCell cell_c4 = new PdfPCell(new Phrase("Fecha de emision", fuente));
                    cell_c4.BackgroundColor = BaseColor.GRAY;
                    cell_c4.BorderWidth = 1;
                    tblPdf.AddCell(cell_c4);

                    PdfPCell cell_c5 = new PdfPCell(new Phrase("Ruc", fuente));
                    cell_c5.BackgroundColor = BaseColor.GRAY;
                    cell_c5.BorderWidth = 1;
                    tblPdf.AddCell(cell_c5);

                    PdfPCell cell_c6 = new PdfPCell(new Phrase("Razon Social", fuente));
                    cell_c6.BackgroundColor = BaseColor.GRAY;
                    cell_c6.BorderWidth = 1;
                    tblPdf.AddCell(cell_c6);

                    PdfPCell cell_c7 = new PdfPCell(new Phrase("Comentario", fuente));
                    cell_c7.BackgroundColor = BaseColor.GRAY;
                    cell_c7.BorderWidth = 1;
                    cell_c7.Colspan = 2;
                    tblPdf.AddCell(cell_c7);

                    foreach (DocumentoValidar row in dgv)
                    {
                        PdfPCell cell_0 = new PdfPCell(new Phrase(row.Tipo, fuente));
                        cell_0.BorderWidth = 1;
                        tblPdf.AddCell(cell_0);

                        PdfPCell cell_1 = new PdfPCell(new Phrase(row.TipoTexto, fuente));
                        cell_1.BorderWidth = 1;
                        tblPdf.AddCell(cell_1);

                        PdfPCell cell_2 = new PdfPCell(new Phrase(row.SerieNumero, fuente));
                        cell_2.BorderWidth = 1;
                        tblPdf.AddCell(cell_2);

                        /* PdfPCell cell_41 = new PdfPCell(new Phrase(row.EstadoVerificarTexto, fuente));
                         cell_41.BorderWidth = 1;
                         tblPdf.AddCell(cell_41);
                         */

                        PdfPCell cell_4 = new PdfPCell(new Phrase(row.EstadoValidarTexto, fuente));
                        cell_4.BorderWidth = 1;
                        tblPdf.AddCell(cell_4);

                        PdfPCell cell_5 = new PdfPCell(new Phrase(row.FechaEmision, fuente));
                        cell_5.BorderWidth = 1;
                        tblPdf.AddCell(cell_5);


                        PdfPCell cell_7 = new PdfPCell(new Phrase(row.Ruc, fuente));
                        cell_7.BorderWidth = 1;
                        tblPdf.AddCell(cell_7);

                        PdfPCell cell_8 = new PdfPCell(new Phrase(row.RazonSocial, fuente));
                        cell_8.BorderWidth = 1;
                        tblPdf.AddCell(cell_8);

                        PdfPCell cell_9 = new PdfPCell(new Phrase(row.Comentario, fuente));
                        cell_9.BorderWidth = 1;
                        cell_9.Colspan = 2;
                        tblPdf.AddCell(cell_9);
                    }
                    doc.Add(tblPdf);

                    doc.Close();
                    writer.Close();

                    clsBaseMensaje.cs_pxMsgOk("OKE12");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR14", ex.ToString());
            }
        }
        #endregion
    }
}
