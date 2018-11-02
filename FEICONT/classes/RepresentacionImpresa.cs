using Extension.Base;
using Extension.Datos;
using FEICONT.print_cliente;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FEICONT
{
    public class RepresentacionImpresa
    {
        #region Representacion Impresa para Otros Comprobantes
        public static bool getRepresentacionImpresa(string pathToSaved,clsEntityDocument cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal();
            try
            {
                //Cristhian||FEICONT2-510
                /*Agregado para llamar al metodo de representacion impresa opcional. Es todo lo que contiene el IF y el solo el ESLE*/
                /*NUEVO INICIO*/
                string RUC = "";
                XmlDocument xmlDocument1 = new XmlDocument();
                //var textXml = cabecera.Cs_pr_XML;
                var textXml1 = textXML;
                textXml1 = textXml1.Replace("cbc:", "");
                textXml1 = textXml1.Replace("cac:", "");
                textXml1 = textXml1.Replace("sac:", "");
                textXml1 = textXml1.Replace("ext:", "");
                textXml1 = textXml1.Replace("ds:", "");
                xmlDocument1.LoadXml(textXml1);
                XmlNodeList DatoCliente = xmlDocument1.GetElementsByTagName("AccountingSupplierParty");
                foreach (XmlNode dat in DatoCliente)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                    if (caaid.Count > 0)
                    {
                        RUC = caaid.Item(0).InnerText;
                    }
                }

                string RUC_Cliente = "";
                string CodigoAdicional_Cliente = "";

                xmlDocument1.LoadXml(textXml1);
                XmlNodeList Dato_Cliente_DelCliente = xmlDocument1.GetElementsByTagName("AccountingCustomerParty");
                foreach (XmlNode dat in Dato_Cliente_DelCliente)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var documento = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                    if (documento.Count > 0)
                    {
                        RUC_Cliente = documento.Item(0).InnerText;
                    }

                    var documento_adicional = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                    if (documento_adicional.Count > 0)
                    {
                        CodigoAdicional_Cliente = documento_adicional.Item(0).InnerText;
                    }
                }

                //Cristhian|15/02/2018|FEICONT2-487
                /*INICIO MODIFICACIóN*/
                /*Agregado para llamar al metodo de representacion impresa opcional. Es todo lo que contiene el IF y el solo el ESLE*/
                /*Si el RUC del Documento XML es igual al que esta en el codigo, entonces se crea una representación diferente*/
                /*Representación Impresa de la empresa Gaitex*/
                if (RUC == "20256459010" && RUC_Cliente == "-" && CodigoAdicional_Cliente == "0")
                {
                    bool resultado = false;
                    resultado = Gaitex_print.getRepresentacionImpresa_Opcional_01(pathToSaved, cabecera, textXML, autorizacion_sunat, pathImage, localDB);
                    return resultado;
                }
                /*Agregado para llamar a la representacion impresa Opcional 2 - Formato de la Empresa Beltran*/
                else if (RUC == "20502510470")
                {
                    bool resultado = false;
                    resultado = Beltran_print.getRepresentacionImpresa_Opcional_02(pathToSaved, cabecera, textXML, autorizacion_sunat, pathImage, localDB);
                    return resultado;
                }
                /*Agregado para llamar a la representacion impresa Opcional 3 - Formato de la Empresa SEMISAC*/
                else if (RUC == "20515292781")
                {
                    bool resultado = false;
                    resultado = Semisac_print.getRepresentacionImpresa_Opcional_03(pathToSaved, cabecera, textXML, autorizacion_sunat, pathImage, localDB);
                    return resultado;
                }
                else
                {
                /*FIN MODIFICACIóN*/
                    var doc_serie = "";
                    var doc_correlativo = "";
                    if (cabecera != null)
                    {

                        string[] partes = cabecera.Cs_tag_ID.Split('-');
                        DateTime dt = DateTime.ParseExact(cabecera.Cs_tag_IssueDate, "yyyy-MM-dd", null);
                        doc_serie = partes[0];
                        doc_correlativo = partes[1];
                        string newFile = pathToSaved;

                        if (File.Exists(newFile))
                        {
                            File.Delete(newFile);
                        }

                        XmlDocument xmlDocument = new XmlDocument();
                        //var textXml = cabecera.Cs_pr_XML;
                        var textXml = textXML;
                        textXml = textXml.Replace("cbc:", "");
                        textXml = textXml.Replace("cac:", "");
                        textXml = textXml.Replace("sac:", "");
                        textXml = textXml.Replace("ext:", "");
                        textXml = textXml.Replace("ds:", "");
                        xmlDocument.LoadXml(textXml);

                        var signatureValue = xmlDocument.GetElementsByTagName("SignatureValue")[0].InnerText;
                        var digestValue = xmlDocument.GetElementsByTagName("DigestValue")[0].InnerText;

                        string InvoiceTypeCode = String.Empty;
                        XmlNodeList InvoiceTypeCodeXml = xmlDocument.GetElementsByTagName("InvoiceTypeCode");
                        if (InvoiceTypeCodeXml.Count > 0)
                        {
                            InvoiceTypeCode = xmlDocument.GetElementsByTagName("InvoiceTypeCode")[0].InnerText;
                        }
                        else
                        {
                            InvoiceTypeCode = cabecera.Cs_tag_InvoiceTypeCode;

                        }

                        string IssueDate = xmlDocument.GetElementsByTagName("IssueDate")[0].InnerText;
                        string DocumentCurrencyCode = xmlDocument.GetElementsByTagName("DocumentCurrencyCode")[0].InnerText;
                        string ASPCustomerAssignedAccountID = "";
                        string ASPAdditionalAccountID = "";
                        string ASPStreetName = "";
                        string ASPRegistrationName = "";
                        string ACPCustomerAssignedAccountID = "";
                        string ACPAdditionalAccountID = "";
                        string ACPDescription = "";
                        string ACPRegistrationName = "";
                        string DReferenceID = "";
                        string DResponseCode = "";
                        string DDescription = "";
                        string LMTChargeTotalAmount = "";
                        string LMTPayableAmount = "";
                        string DescuentoGlobal = "";
                        var info_general = getByTipo(InvoiceTypeCode);

                        Document doc = new Document(PageSize.LETTER);
                        // Indicamos donde vamos a guardar el documento
                        PdfWriter writer = PdfWriter.GetInstance(doc,
                                                    new FileStream(newFile, FileMode.Create));

                        // Le colocamos el título y el autor
                        // Esto no será visible en el documento
                        doc.AddTitle("Documento Electronico");
                        doc.AddCreator("Contasis");

                        // Abrimos el archivo
                        doc.Open();
                        // Creamos el tipo de Font que vamos utilizar
                        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        iTextSharp.text.Font _TitleFontN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _TitleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        iTextSharp.text.Font _HeaderFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _HeaderFontMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _clienteFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _clienteFontBoldMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _clienteFontContent = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        iTextSharp.text.Font _clienteFontContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        iTextSharp.text.Font _clienteFontBoldContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        PdfPTable tblPrueba = new PdfPTable(5);
                        tblPrueba.WidthPercentage = 100;


                        //TABLA header left
                        PdfPTable tblHeaderLeft = new PdfPTable(1);
                        tblHeaderLeft.WidthPercentage = 100;

                        //string currentDirectory = Environment.CurrentDirectory;
                        //string pathImage = currentDirectory + "\\logo.png";
                        // Creamos la imagen y le ajustamos el tamaño
                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(pathImage);
                        imagen.BorderWidth = 0;
                        imagen.Alignment = Element.ALIGN_RIGHT;
                        float percentage = 0.0f;
                        percentage = 290 / imagen.Width;
                        imagen.ScalePercent(80);

                        // Insertamos la imagen en el documento

                        PdfPCell logo = new PdfPCell(imagen);
                        logo.BorderWidth = 0;
                        logo.BorderWidthBottom = 0;
                        logo.Border = 0;

                        tblHeaderLeft.AddCell(logo);


                        //get accounting supplier party
                        XmlNodeList AccountingSupplierParty = xmlDocument.GetElementsByTagName("AccountingSupplierParty");
                        foreach (XmlNode dat in AccountingSupplierParty)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);

                            var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                            if (caaid.Count > 0)
                            {
                                ASPCustomerAssignedAccountID = caaid.Item(0).InnerText;
                            }
                            var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                            if (aacid.Count > 0)
                            {
                                ASPAdditionalAccountID = aacid.Item(0).InnerText;
                            }
                            var stname = xmlDocumentinner.GetElementsByTagName("StreetName");
                            if (stname.Count > 0)
                            {
                                ASPStreetName = stname.Item(0).InnerText;
                            }
                            var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                            if (regname.Count > 0)
                            {
                                ASPRegistrationName = regname.Item(0).InnerText;
                            }
                        }
                        //get accounting supplier party
                        XmlNodeList AccountingCustomerParty = xmlDocument.GetElementsByTagName("AccountingCustomerParty");
                        foreach (XmlNode dat in AccountingCustomerParty)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);

                            var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                            if (caaid.Count > 0)
                            {
                                ACPCustomerAssignedAccountID = caaid.Item(0).InnerText;
                            }
                            var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                            if (aacid.Count > 0)
                            {
                                ACPAdditionalAccountID = aacid.Item(0).InnerText;
                            }
                            var descr = xmlDocumentinner.GetElementsByTagName("Description");
                            if (descr.Count > 0)
                            {
                                ACPDescription = descr.Item(0).InnerText;
                            }
                            var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                            if (regname.Count > 0)
                            {
                                ACPRegistrationName = regname.Item(0).InnerText;
                            }
                        }
                        XmlNodeList DiscrepancyResponse = xmlDocument.GetElementsByTagName("DiscrepancyResponse");
                        foreach (XmlNode dat in DiscrepancyResponse)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);

                            var refid = xmlDocumentinner.GetElementsByTagName("ReferenceID");
                            if (refid.Count > 0)
                            {
                                DReferenceID = refid.Item(0).InnerText;
                            }
                            var respcode = xmlDocumentinner.GetElementsByTagName("ResponseCode");
                            if (respcode.Count > 0)
                            {
                                DResponseCode = respcode.Item(0).InnerText;
                            }
                            var descr = xmlDocumentinner.GetElementsByTagName("Description");
                            if (descr.Count > 0)
                            {
                                DDescription = descr.Item(0).InnerText;
                            }

                        }

                        XmlNodeList LegalMonetaryTotal = null;

                        if (InvoiceTypeCode == "08")
                        {
                            LegalMonetaryTotal = xmlDocument.GetElementsByTagName("RequestedMonetaryTotal");
                        }
                        else
                        {
                            LegalMonetaryTotal = xmlDocument.GetElementsByTagName("LegalMonetaryTotal");
                        }

                        foreach (XmlNode dat in LegalMonetaryTotal)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);

                            var cta = xmlDocumentinner.GetElementsByTagName("ChargeTotalAmount");
                            if (cta.Count > 0)
                            {
                                LMTChargeTotalAmount = cta.Item(0).InnerText;
                            }
                            var pam = xmlDocumentinner.GetElementsByTagName("PayableAmount");
                            if (pam.Count > 0)
                            {
                                LMTPayableAmount = pam.Item(0).InnerText;
                            }

                            //Cristhian|25/10/2017|FEICONT2-396
                            /*SE obtine el valor del Descuento Global*/
                            /*INICIO MODIFICAIóN*/
                            var totalDescuento = xmlDocumentinner.GetElementsByTagName("AllowanceTotalAmount");
                            /*Si se encuentra almenos 1 datos en la etiqueta AllowanceTotalAmount
                             entonces se asigna el valor a la variable DescuentoGlobal*/
                            if (totalDescuento.Count > 0)
                            {
                                DescuentoGlobal = totalDescuento.Item(0).InnerText;
                            }
                            /*FIN MODIFICACIóN*/
                        }

                        var VerificarDescuentoUnitario = xmlDocument.GetElementsByTagName("AllowanceCharge");//Para comprobar la existencia de un descuento por Item

                        //tabla info empresa
                        PdfPTable tblInforEmpresa = new PdfPTable(1);
                        tblInforEmpresa.WidthPercentage = 100;
                        PdfPCell NameEmpresa = new PdfPCell(new Phrase(ASPRegistrationName, _HeaderFont));
                        NameEmpresa.BorderWidth = 0;
                        NameEmpresa.Border = 0;
                        tblInforEmpresa.AddCell(NameEmpresa);

                        var pa = new Paragraph();
                        pa.Font = _clienteFontBoldMin;
                        pa.Add(ASPStreetName);
                        PdfPCell EstaticoEmpresa = new PdfPCell(pa);
                        EstaticoEmpresa.BorderWidth = 0;
                        EstaticoEmpresa.Border = 0;
                        tblInforEmpresa.AddCell(EstaticoEmpresa);

                        PdfPCell celdaInfoEmpresa = new PdfPCell(tblInforEmpresa);
                        celdaInfoEmpresa.Border = 0;
                        tblHeaderLeft.AddCell(celdaInfoEmpresa);
                        // PdfPCell blanco = new PdfPCell();
                        // blanco.Border = 0;



                        List<clsEntityDocument_AdditionalComments> Lista_additional_coments = new List<clsEntityDocument_AdditionalComments>();
                        clsEntityDocument_AdditionalComments adittionalComents;
                        XmlNodeList datosCabecera = xmlDocument.GetElementsByTagName("DatosCabecera");
                        foreach (XmlNode dat in datosCabecera)
                        {
                            var NodosHijos = dat.ChildNodes;
                            for (int z = 0; z < NodosHijos.Count; z++)
                            {
                                adittionalComents = new clsEntityDocument_AdditionalComments(local);
                                adittionalComents.Cs_pr_TagNombre = NodosHijos.Item(z).LocalName;
                                adittionalComents.Cs_pr_TagValor = NodosHijos.Item(z).ChildNodes.Item(0).InnerText;
                                Lista_additional_coments.Add(adittionalComents);
                            }
                        }

                        //comentarios contenido
                        var teclaf8 = " ";//comment1
                        var teclavtrlm = " ";//commnet2
                        var cuentasbancarias = " ";//comment 3
                        var CondicionPagoXML = " ";
                        var CondicionVentaXML = " ";
                        var VendedorXML = " ";

                        //var errores = "";

                        foreach (var itemm in Lista_additional_coments)
                        {
                            if (itemm.Cs_pr_TagNombre == "DatEmpresa")
                            {
                                cuentasbancarias = itemm.Cs_pr_TagValor;
                            }
                            if (itemm.Cs_pr_TagNombre == "TeclaF8")
                            {
                                teclaf8 = itemm.Cs_pr_TagValor;
                            }
                            if (itemm.Cs_pr_TagNombre == "TeclasCtrlM")
                            {
                                teclavtrlm = itemm.Cs_pr_TagValor;
                            }
                            if (itemm.Cs_pr_TagNombre == "CondPago")
                            {
                                CondicionPagoXML = itemm.Cs_pr_TagValor;
                            }
                            if (itemm.Cs_pr_TagNombre == "Vendedor")
                            {
                                VendedorXML = itemm.Cs_pr_TagValor;
                            }
                            if (itemm.Cs_pr_TagNombre == "Condicion")
                            {
                                CondicionVentaXML = itemm.Cs_pr_TagValor;
                            }
                        }



                        //tabla para info ruc
                        PdfPTable tblInforRuc = new PdfPTable(1);
                        tblInforRuc.WidthPercentage = 100;

                        PdfPCell TituRuc = new PdfPCell(new Phrase("R.U.C. " + ASPCustomerAssignedAccountID, _TitleFontN));
                        TituRuc.BorderWidthTop = 0.75f;
                        TituRuc.BorderWidthBottom = 0.75f;
                        TituRuc.BorderWidthLeft = 0.75f;
                        TituRuc.BorderWidthRight = 0.75f;
                        TituRuc.HorizontalAlignment = Element.ALIGN_CENTER;
                        TituRuc.PaddingTop = 10f;
                        TituRuc.PaddingBottom = 10f;

                        PdfPCell TipoDoc = new PdfPCell(new Phrase(info_general[2], _TitleFontN));
                        TipoDoc.BorderWidthLeft = 0.75f;
                        TipoDoc.BorderWidthRight = 0.75f;
                        TipoDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                        TipoDoc.PaddingTop = 10f;
                        TipoDoc.PaddingBottom = 10f;

                        PdfPCell SerieDoc = new PdfPCell(new Phrase("N° " + cabecera.Cs_tag_ID, _TitleFont));
                        SerieDoc.BorderWidthBottom = 0.75f;
                        SerieDoc.BorderWidthRight = 0.75f;
                        SerieDoc.BorderWidthLeft = 0.75f;
                        SerieDoc.BorderWidthTop = 0.75f;
                        SerieDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                        SerieDoc.PaddingTop = 10f;
                        SerieDoc.PaddingBottom = 10f;

                        PdfPCell blanco2 = new PdfPCell(new Paragraph(" "));
                        blanco2.Border = 0;
                        tblInforRuc.AddCell(TituRuc);
                        //tblInforRuc.AddCell(blanco2);
                        tblInforRuc.AddCell(TipoDoc);
                        //tblInforRuc.AddCell(blanco2);
                        tblInforRuc.AddCell(SerieDoc);
                        tblInforRuc.AddCell(blanco2);

                        PdfPCell infoRuc = new PdfPCell(tblInforRuc);
                        infoRuc.Colspan = 2;
                        infoRuc.BorderWidth = 0;

                        PdfPCell celdaHeaderLeft = new PdfPCell(tblHeaderLeft);
                        celdaHeaderLeft.Border = 0;
                        celdaHeaderLeft.Colspan = 3;

                        // Añadimos las celdas a la tabla
                        tblPrueba.AddCell(celdaHeaderLeft);
                        // tblPrueba.AddCell(blanco);
                        tblPrueba.AddCell(infoRuc);

                        doc.Add(tblPrueba);

                        PdfPTable tblBlanco = new PdfPTable(1);
                        tblBlanco.WidthPercentage = 100;
                        PdfPCell blanco3 = new PdfPCell((new Paragraph(" ")));
                        blanco3.Border = 0;

                        tblBlanco.AddCell(blanco3);

                        doc.Add(tblBlanco);

                        //Informacion cliente
                        PdfPTable tblInfoCliente = new PdfPTable(10);
                        tblInfoCliente.WidthPercentage = 100;



                        // Llenamos la tabla con información del cliente
                        PdfPCell cliente = new PdfPCell(new Phrase("Cliente:", _clienteFontBoldMin));
                        cliente.BorderWidth = 0;
                        cliente.Colspan = 1;

                        PdfPCell clNombre = new PdfPCell(new Phrase(ACPRegistrationName, _clienteFontContentMinFooter));
                        clNombre.BorderWidth = 0;
                        clNombre.Colspan = 5;

                        PdfPCell fecha = new PdfPCell(new Phrase("Fecha de Emisión:", _clienteFontBoldMin));
                        fecha.BorderWidth = 0;
                        fecha.Colspan = 2;

                        var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                        PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                        clFecha.BorderWidth = 0;
                        clFecha.Colspan = 2;

                        // Añadimos las celdas a la tabla
                        tblInfoCliente.AddCell(cliente);
                        tblInfoCliente.AddCell(clNombre);
                        tblInfoCliente.AddCell(fecha);
                        tblInfoCliente.AddCell(clFecha);

                        PdfPCell direccion = new PdfPCell(new Phrase("Dirección:", _clienteFontBoldMin));
                        direccion.BorderWidth = 0;
                        direccion.Colspan = 1;

                        PdfPCell clDireccion = new PdfPCell(new Phrase(ACPDescription, _clienteFontContentMinFooter));
                        clDireccion.BorderWidth = 0;
                        clDireccion.Colspan = 5;


                        /*En caso sea nota de credito o debito*/
                        if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                        {
                            PdfPCell condicionVenta = new PdfPCell(new Phrase("Documento que modifica:", _clienteFontBoldMin));
                            condicionVenta.BorderWidth = 0;
                            condicionVenta.Colspan = 2;


                            PdfPCell clCondicionVenta = new PdfPCell(new Phrase(DReferenceID, _clienteFontContentMinFooter));
                            clCondicionVenta.BorderWidth = 0;
                            clCondicionVenta.Colspan = 2;

                            tblInfoCliente.AddCell(direccion);
                            tblInfoCliente.AddCell(clDireccion);
                            tblInfoCliente.AddCell(condicionVenta);
                            tblInfoCliente.AddCell(clCondicionVenta);
                        }
                        else
                        {
                            NumLetra monedaLetras = new NumLetra();
                            var monedaLetra = monedaLetras.getMoneda(DocumentCurrencyCode);
                            PdfPCell moneda = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                            moneda.BorderWidth = 0;
                            moneda.Colspan = 2;

                            PdfPCell clMoneda = new PdfPCell(new Phrase(monedaLetra.ToUpper(), _clienteFontContentMinFooter));
                            clMoneda.BorderWidth = 0;
                            clMoneda.Colspan = 2;

                            /* PdfPCell condicionVenta = new PdfPCell(new Phrase("Condicion Venta:", _clienteFontBoldMin));
                             condicionVenta.BorderWidth = 0;
                             condicionVenta.Colspan = 2;


                             PdfPCell clCondicionVenta = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                             clCondicionVenta.BorderWidth = 0;
                             clCondicionVenta.Colspan = 2;
                             */
                            tblInfoCliente.AddCell(direccion);
                            tblInfoCliente.AddCell(clDireccion);
                            tblInfoCliente.AddCell(moneda);
                            tblInfoCliente.AddCell(clMoneda);

                        }


                        // Añadimos las celdas a la tabla de info cliente


                        var docName = getTipoDocIdentidad(ACPAdditionalAccountID);
                        PdfPCell ruc = new PdfPCell(new Phrase(docName + " N°:", _clienteFontBoldMin));
                        ruc.BorderWidth = 0;
                        ruc.Colspan = 1;

                        PdfPCell clRUC = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID, _clienteFontContentMinFooter));
                        clRUC.BorderWidth = 0;
                        clRUC.Colspan = 5;
                        if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                        {
                            NumLetra monedaLetras1 = new NumLetra();
                            var monedaLetra_ = monedaLetras1.getMoneda(DocumentCurrencyCode);
                            PdfPCell moneda_ = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                            moneda_.BorderWidth = 0;
                            moneda_.Colspan = 2;

                            PdfPCell clMoneda_ = new PdfPCell(new Phrase(monedaLetra_.ToUpper(), _clienteFontContentMinFooter));
                            clMoneda_.BorderWidth = 0;
                            clMoneda_.Colspan = 2;
                            tblInfoCliente.AddCell(ruc);
                            tblInfoCliente.AddCell(clRUC);
                            tblInfoCliente.AddCell(moneda_);
                            tblInfoCliente.AddCell(clMoneda_);
                        }
                        else
                        {  //NumLetra monedaLetras = new NumLetra();
                           //  var monedaLetra_ = monedaLetras.getMoneda(cabecera.Cs_tag_DocumentCurrencyCode);
                            PdfPCell moneda_ = new PdfPCell(new Phrase("Condición de Venta", _clienteFontBoldMin));
                            moneda_.BorderWidth = 0;
                            moneda_.Colspan = 2;

                            PdfPCell clMoneda_ = new PdfPCell(new Phrase(CondicionVentaXML, _clienteFontContentMinFooter));
                            clMoneda_.BorderWidth = 0;
                            clMoneda_.Colspan = 2;
                            tblInfoCliente.AddCell(ruc);
                            tblInfoCliente.AddCell(clRUC);
                            tblInfoCliente.AddCell(moneda_);
                            tblInfoCliente.AddCell(clMoneda_);

                        }

                        // Añadimos las celdas a la tabla inf

                        /*En caso sea nota de credito o debito*/
                        if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                        {

                            PdfPCell motivomodifica = new PdfPCell(new Phrase("Motivo", _clienteFontBoldMin));
                            motivomodifica.BorderWidth = 0;
                            motivomodifica.Colspan = 1;

                            PdfPCell clmotivomodifica = new PdfPCell(new Phrase(DDescription, _clienteFontContentMinFooter));
                            clmotivomodifica.BorderWidth = 0;
                            clmotivomodifica.Colspan = 5;

                            clsEntityDocument doc_modificado = new clsEntityDocument(localDB);
                            string fechaModificado = doc_modificado.cs_pxBuscarFechaDocumento(DReferenceID);
                            PdfPCell docmodifica = new PdfPCell(new Phrase("Fecha Doc. Modificado:", _clienteFontBoldMin));
                            docmodifica.BorderWidth = 0;
                            docmodifica.Colspan = 2;

                            PdfPCell cldocmodifica = new PdfPCell(new Phrase(fechaModificado, _clienteFontContentMinFooter));
                            cldocmodifica.BorderWidth = 0;
                            cldocmodifica.Colspan = 2;

                            tblInfoCliente.AddCell(motivomodifica);
                            tblInfoCliente.AddCell(clmotivomodifica);
                            tblInfoCliente.AddCell(docmodifica);
                            tblInfoCliente.AddCell(cldocmodifica);

                        }
                        else
                        {
                            PdfPCell motivomodifica = new PdfPCell(new Phrase(" ", _clienteFontBoldMin));
                            motivomodifica.BorderWidth = 0;
                            motivomodifica.Colspan = 1;

                            PdfPCell clmotivomodifica = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                            clmotivomodifica.BorderWidth = 0;
                            clmotivomodifica.Colspan = 5;


                            PdfPCell docmodifica = new PdfPCell(new Phrase("Vendedor:", _clienteFontBoldMin));
                            docmodifica.BorderWidth = 0;
                            docmodifica.Colspan = 2;

                            PdfPCell cldocmodifica = new PdfPCell(new Phrase(VendedorXML, _clienteFontContentMinFooter));
                            cldocmodifica.BorderWidth = 0;
                            cldocmodifica.Colspan = 2;

                            tblInfoCliente.AddCell(motivomodifica);
                            tblInfoCliente.AddCell(clmotivomodifica);
                            tblInfoCliente.AddCell(docmodifica);
                            tblInfoCliente.AddCell(cldocmodifica);

                        }

                        /*------------------------------------*/
                        doc.Add(tblInfoCliente);
                        doc.Add(tblBlanco);

                        PdfPTable tblInfoComprobante = new PdfPTable(12);
                        tblInfoComprobante.WidthPercentage = 100;


                        // Llenamos la tabla con información
                        PdfPCell colCodigo = new PdfPCell(new Phrase("Item", _clienteFontBoldMin));
                        colCodigo.BorderWidth = 0.75f;
                        colCodigo.Colspan = 1;
                        colCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell colCantidad = new PdfPCell(new Phrase("Cantidad", _clienteFontBoldMin));
                        colCantidad.BorderWidth = 0.75f;
                        colCantidad.Colspan = 1;
                        colCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell colUnidadMedida = new PdfPCell(new Phrase("Codigo", _clienteFontBoldMin));
                        colUnidadMedida.BorderWidth = 0.75f;
                        colUnidadMedida.Colspan = 2;
                        colUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell colDescripcion = new PdfPCell(new Phrase("Descripción", _clienteFontBoldMin));
                        colDescripcion.BorderWidth = 0.75f;
                        colDescripcion.Colspan = 6;
                        colDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell colPrecUnit = new PdfPCell(new Phrase("Valor Unitario", _clienteFontBoldMin));
                        colPrecUnit.BorderWidth = 0.75f;
                        colPrecUnit.Colspan = 1;
                        colPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;

                        //Cristhian|25/10/2017|FEICONT2-396
                        /*Se agrega una Nueva Columna*/
                        /*NUEVO INICIO*/
                        PdfPCell colDescuentoUnitario = new PdfPCell(new Phrase("Dscto. Unit.", _clienteFontBoldMin));
                        /*Si se encuentra un descuento unitario entonces se modifica la estructura de las columnas*/
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            /*Se cambia el ancho de la columna Descripción, para que la columna de descuentos quepa en el documento*/
                            colDescripcion.Colspan = 5;

                            /*Se asigna los valores estandar de la celda (dimensiones y tipo de alineacion del texto)*/
                            colDescuentoUnitario.BorderWidth = 0.75f;
                            colDescuentoUnitario.Colspan = 1;
                            colDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        /*NUEVO FIN*/

                        PdfPCell colImporte = new PdfPCell(new Phrase("Valor Total", _clienteFontBoldMin));
                        colImporte.BorderWidth = 0.75f;
                        colImporte.Colspan = 1;
                        colImporte.HorizontalAlignment = Element.ALIGN_CENTER;

                        // Añadimos las celdas a la tabla - Se incluye el nuevo campo de descuento
                        tblInfoComprobante.AddCell(colCodigo);
                        tblInfoComprobante.AddCell(colCantidad);
                        tblInfoComprobante.AddCell(colUnidadMedida);
                        tblInfoComprobante.AddCell(colDescripcion);
                        tblInfoComprobante.AddCell(colPrecUnit);
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            tblInfoComprobante.AddCell(colDescuentoUnitario);//Descuento
                        }
                        tblInfoComprobante.AddCell(colImporte);

                        //impuestos globales

                        List<clsEntityDocument_TaxTotal> Lista_tax_total = new List<clsEntityDocument_TaxTotal>();
                        clsEntityDocument_TaxTotal taxTotal;
                        XmlNodeList nodestaxTotal = xmlDocument.GetElementsByTagName("TaxTotal");
                        foreach (XmlNode dat in nodestaxTotal)
                        {
                            string nodoPadre = dat.ParentNode.LocalName;
                            if (nodoPadre == "Invoice" || nodoPadre == "DebitNote" || nodoPadre == "CreditNote")
                            {
                                taxTotal = new clsEntityDocument_TaxTotal(local);
                                XmlDocument xmlDocumentTaxtotal = new XmlDocument();
                                xmlDocumentTaxtotal.LoadXml(dat.OuterXml);
                                XmlNodeList taxAmount = xmlDocumentTaxtotal.GetElementsByTagName("TaxAmount");
                                if (taxAmount.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxAmount = taxAmount.Item(0).InnerText;
                                }
                                XmlNodeList subtotal = xmlDocumentTaxtotal.GetElementsByTagName("TaxSubtotal");
                                if (subtotal.Count > 0)
                                {
                                    XmlDocument xmlDocumentTaxSubtotal = new XmlDocument();
                                    xmlDocumentTaxSubtotal.LoadXml(subtotal.Item(0).OuterXml);

                                    var subTotalAmount = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxAmount");
                                    if (subTotalAmount.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxAmount = subTotalAmount.Item(0).InnerText;
                                    }
                                    var subTotalID = xmlDocumentTaxSubtotal.GetElementsByTagName("ID");
                                    if (subTotalID.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                    }
                                    var subTotalName = xmlDocumentTaxSubtotal.GetElementsByTagName("Name");
                                    if (subTotalName.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                    }
                                    var subTotalTaxTypeCode = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                    if (subTotalTaxTypeCode.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                    }

                                }
                                Lista_tax_total.Add(taxTotal);

                            }
                        }



                        string imp_IGV = "";
                        string imp_ISC = "";
                        string imp_OTRO = "";

                        foreach (var ress in Lista_tax_total)
                        {

                            if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "1000")
                            {//IGV
                                imp_IGV = Convert.ToString(ress.Cs_tag_TaxAmount);

                            }
                            else if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "2000")
                            {//isc
                                imp_ISC = Convert.ToString(ress.Cs_tag_TaxAmount);

                            }
                            else if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "9999")
                            {
                                imp_OTRO = Convert.ToString(ress.Cs_tag_TaxAmount);

                            }

                        }

                        //Additional Monetary Total
                        List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal> Lista_additional_monetary = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal>();
                        List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty> Lista_additional_property = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty>();

                        XmlNodeList additionalInformation = xmlDocument.GetElementsByTagName("AdditionalInformation");
                        foreach (XmlNode dat in additionalInformation)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);
                            clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal adittionalMonetary;

                            XmlNodeList LIST1 = xmlDocumentinner.GetElementsByTagName("AdditionalMonetaryTotal");
                            for (int ii = 0; ii < LIST1.Count; ii++)
                            {
                                adittionalMonetary = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);

                                var ss = LIST1.Item(ii);
                                XmlDocument xmlDocumentinner1 = new XmlDocument();
                                xmlDocumentinner1.LoadXml(ss.OuterXml);

                                var id = xmlDocumentinner1.GetElementsByTagName("ID");
                                if (id.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_Id = id.Item(0).InnerText;
                                }
                                var percent = xmlDocumentinner1.GetElementsByTagName("Percent");
                                if (percent.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_Percent = percent.Item(0).InnerText;
                                }
                                var payableAmount = xmlDocumentinner1.GetElementsByTagName("PayableAmount");
                                if (payableAmount.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_PayableAmount = payableAmount.Item(0).InnerText;
                                    /*** if (payableAmount.Item(0).Attributes.Count > 0)
                                     {
                                         adittionalMonetary. = payableAmount.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                     }****/
                                }
                                Lista_additional_monetary.Add(adittionalMonetary);

                            }
                            clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adittionalProperty;
                            XmlNodeList LIST2 = xmlDocumentinner.GetElementsByTagName("AdditionalProperty");
                            for (int iii = 0; iii < LIST2.Count; iii++)
                            {
                                adittionalProperty = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);

                                var ss = LIST2.Item(iii);
                                XmlDocument xmlDocumentinner1 = new XmlDocument();
                                xmlDocumentinner1.LoadXml(ss.OuterXml);

                                var id = xmlDocumentinner1.GetElementsByTagName("ID");
                                if (id.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_ID = id.Item(0).InnerText;
                                }

                                var value = xmlDocumentinner1.GetElementsByTagName("Value");
                                if (value.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_Value = value.Item(0).InnerText;
                                }
                                var name = xmlDocumentinner1.GetElementsByTagName("Name");
                                if (name.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_Name = name.Item(0).InnerText;
                                }
                                Lista_additional_property.Add(adittionalProperty);
                            }
                        }
                        //Additional

                        var cuenta_nacion = "";
                        try
                        {
                            foreach (var it in Lista_additional_property)
                            {
                                if (it.Cs_tag_ID == "3001")
                                {
                                    cuenta_nacion = it.Cs_tag_Value;
                                }
                            }

                        }
                        catch (Exception)
                        {
                            cuenta_nacion = "";
                        }

                        string op_gravada = "0.00";
                        string op_inafecta = "0.00";
                        string op_exonerada = "0.00";
                        string op_gratuita = "0.00";
                        string op_detraccion = "0.00";
                        string porcentaje_detraccion = "";
                        string total_descuentos = "0.00";

                        foreach (var ress in Lista_additional_monetary)
                        {
                            if (ress.Cs_tag_Id == "1001")
                            {
                                op_gravada = Convert.ToString(ress.Cs_tag_PayableAmount);

                            }
                            else if (ress.Cs_tag_Id == "1002")
                            {
                                op_inafecta = Convert.ToString(ress.Cs_tag_PayableAmount);

                            }
                            else if (ress.Cs_tag_Id == "1003")
                            {
                                op_exonerada = Convert.ToString(ress.Cs_tag_PayableAmount);

                            }
                            else if (ress.Cs_tag_Id == "2005")
                            {
                                total_descuentos = Convert.ToString(ress.Cs_tag_PayableAmount);

                            }
                            else if (ress.Cs_tag_Id == "1004")
                            {
                                op_gratuita = Convert.ToString(ress.Cs_tag_PayableAmount);

                            }
                            else if (ress.Cs_tag_Id == "2003")
                            {
                                op_detraccion = Convert.ToString(ress.Cs_tag_PayableAmount);
                                porcentaje_detraccion = Convert.ToString(ress.Cs_tag_Percent);
                            }

                        }
                        /* seccion de items ------ añadir items*/
                        var numero_item = 0;
                        double sub_total = 0.00;

                        List<clsEntityDocument_Line> Lista_items;
                        List<clsEntityDocument_Line_TaxTotal> Lista_items_taxtotal;
                        clsEntityDocument_Line item;
                        XmlNodeList nodeitem;
                        if (InvoiceTypeCode == "07")
                        {
                            nodeitem = xmlDocument.GetElementsByTagName("CreditNoteLine");

                        }
                        else if (InvoiceTypeCode == "08")
                        {

                            nodeitem = xmlDocument.GetElementsByTagName("DebitNoteLine");

                        }
                        else
                        {
                            nodeitem = xmlDocument.GetElementsByTagName("InvoiceLine");
                        }
                        // XmlNodeList nodeitem = xmlDocument.GetElementsByTagName("InvoiceLine");
                        // Dictionary<string, List<clasEntityDocument_Line_Description>> dictionary = new Dictionary<string, List<clasEntityDocument_Line_Description>>();
                        List<clsEntityDocument_Line_Description> Lista_items_description;
                        List<clsEntityDocument_Line_PricingReference> Lista_items_princingreference;
                        clsEntityDocument_Line_Description descripcionItem;

                        var total_items = nodeitem.Count;

                        int i = 0;
                        foreach (XmlNode dat in nodeitem)
                        {
                            i++;
                            numero_item++; //El numero del Item Se actualliza automaticamente - ya que es progresivo
                            var valor_unitario_item = "";
                            var valor_total_item = "";
                            string condition_price = "";
                            Lista_items = new List<clsEntityDocument_Line>();
                            Lista_items_description = new List<clsEntityDocument_Line_Description>();
                            Lista_items_princingreference = new List<clsEntityDocument_Line_PricingReference>();
                            Lista_items_taxtotal = new List<clsEntityDocument_Line_TaxTotal>();
                            item = new clsEntityDocument_Line(local);
                            XmlDocument xmlItem = new XmlDocument();
                            xmlItem.LoadXml(dat.OuterXml);

                            XmlNodeList ItemDetail = xmlItem.GetElementsByTagName("Item");
                            if (ItemDetail.Count > 0)
                            {
                                foreach (XmlNode items in ItemDetail)
                                {
                                    XmlDocument xmlItemItem = new XmlDocument();
                                    xmlItemItem.LoadXml(items.OuterXml);

                                    /*Inicio Obtensión de la Unidad de Medida del producto */
                                    XmlNodeList taxItemIdentification = xmlItemItem.GetElementsByTagName("ID");
                                    if (taxItemIdentification.Count > 0)
                                    {
                                        item.Cs_tag_Item_SellersItemIdentification = taxItemIdentification.Item(0).InnerText;
                                    }
                                    /*Fin Obtensión de la Unidad de Medida del producto */

                                    XmlNodeList taxItemDescription = xmlItemItem.GetElementsByTagName("Description");
                                    int j = 0;
                                    foreach (XmlNode description in taxItemDescription)
                                    {
                                        j++;
                                        descripcionItem = new clsEntityDocument_Line_Description(local);
                                        descripcionItem.Cs_pr_Document_Line_Id = j.ToString();
                                        /* if (description.HasChildNodes)
                                         {
                                             descripcionItem.Cs_tag_Description = description.FirstChild.InnerText.Trim();
                                         }
                                         else
                                         {*/
                                        descripcionItem.Cs_tag_Description = description.InnerText.Trim();
                                        //   }

                                        Lista_items_description.Add(descripcionItem);

                                    }
                                    j = 0;
                                }
                                //dictionary[i.ToString()] = Lista_items_description;
                            }


                            XmlNodeList ID = xmlItem.GetElementsByTagName("ID");
                            if (ID.Count > 0)
                            {
                                item.Cs_tag_InvoiceLine_ID = ID.Item(0).InnerText;
                            }

                            /*Inicio Obtensión la cantidad del producto */
                            XmlNodeList InvoicedQuantity;
                            if (InvoiceTypeCode == "07")
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("CreditedQuantity");

                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }
                            }
                            else if (InvoiceTypeCode == "08")
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("DebitedQuantity");
                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }
                            }
                            else
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("InvoicedQuantity");
                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }

                            }
                            /*Fin Obtension la cantidad del producto*/

                            XmlNodeList LineExtensionAmount = xmlItem.GetElementsByTagName("LineExtensionAmount");
                            if (LineExtensionAmount.Count > 0)
                            {
                                item.Cs_tag_LineExtensionAmount_currencyID = LineExtensionAmount.Item(0).InnerText;
                            }
                            clsEntityDocument_Line_PricingReference lines_pricing_reference;
                            XmlNodeList PricingReference = xmlItem.GetElementsByTagName("PricingReference");
                            if (PricingReference.Count > 0)
                            {
                                XmlDocument xmlItemItem = new XmlDocument();
                                xmlItemItem.LoadXml(PricingReference.Item(0).OuterXml);
                                XmlNodeList AlternativeConditionPrice = xmlItemItem.GetElementsByTagName("AlternativeConditionPrice");
                                foreach (XmlNode itm in AlternativeConditionPrice)
                                {
                                    XmlDocument xmlItemPricingReference = new XmlDocument();
                                    xmlItemPricingReference.LoadXml(itm.OuterXml);
                                    lines_pricing_reference = new clsEntityDocument_Line_PricingReference(local);
                                    XmlNodeList PriceAmount = xmlItemPricingReference.GetElementsByTagName("PriceAmount");
                                    if (PriceAmount.Count > 0)
                                    {
                                        lines_pricing_reference.Cs_tag_PriceAmount_currencyID = PriceAmount.Item(0).InnerText;
                                    }
                                    XmlNodeList PriceTypeCode = xmlItemPricingReference.GetElementsByTagName("PriceTypeCode");
                                    if (PriceTypeCode.Count > 0)
                                    {
                                        lines_pricing_reference.Cs_tag_PriceTypeCode = PriceTypeCode.Item(0).InnerText;
                                    }
                                    Lista_items_princingreference.Add(lines_pricing_reference);
                                }


                            }
                            clsEntityDocument_Line_TaxTotal taxTotalItem;
                            XmlNodeList TaxTotal = xmlItem.GetElementsByTagName("TaxTotal");
                            if (TaxTotal.Count > 0)
                            {
                                foreach (XmlNode taxitem in TaxTotal)
                                {
                                    taxTotalItem = new clsEntityDocument_Line_TaxTotal(local);
                                    XmlDocument xmlItemTaxtotal = new XmlDocument();
                                    xmlItemTaxtotal.LoadXml(taxitem.OuterXml);
                                    XmlNodeList taxItemAmount = xmlItemTaxtotal.GetElementsByTagName("TaxAmount");
                                    if (taxItemAmount.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxAmount_currencyID = taxItemAmount.Item(0).InnerText;
                                    }
                                    XmlNodeList itemsubtotal = xmlItemTaxtotal.GetElementsByTagName("TaxSubtotal");
                                    if (itemsubtotal.Count > 0)
                                    {
                                        XmlDocument xmlItemTaxSubtotal = new XmlDocument();
                                        xmlItemTaxSubtotal.LoadXml(itemsubtotal.Item(0).OuterXml);

                                        var subTotalAmount = xmlItemTaxSubtotal.GetElementsByTagName("TaxAmount");
                                        if (subTotalAmount.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxAmount_currencyID = subTotalAmount.Item(0).InnerText;
                                        }
                                        var subTotalID = xmlItemTaxSubtotal.GetElementsByTagName("ID");
                                        if (subTotalID.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                        }
                                        var subTotalName = xmlItemTaxSubtotal.GetElementsByTagName("Name");
                                        if (subTotalName.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                        }
                                        var subTotalTaxTypeCode = xmlItemTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                        if (subTotalTaxTypeCode.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                        }

                                    }
                                    Lista_items_taxtotal.Add(taxTotalItem);
                                }
                            }

                            //Cristhian|14/02/2018|FEICONT2-644
                            /*INICIO MODIFICACIóN*/
                            /*Se obtiene el valor del Precio del item*/
                            XmlNodeList Price = xmlItem.GetElementsByTagName("Price");
                            /*Se verifica que el valor del precio tenga al menos 1 valor*/
                            if (Price.Count > 0)
                            {
                                /*Se descompone el elemento "Price" */
                                XmlDocument xmlItemPrice = new XmlDocument();
                                xmlItemPrice.LoadXml(Price.Item(0).OuterXml);

                                /*Se obtiene el valor del "PriceAmount" */
                                XmlNodeList PriceAmount = xmlItemPrice.GetElementsByTagName("PriceAmount");

                                /*Se verifica que se tenga al menos un valor*/
                                if (PriceAmount.Count > 0)
                                {
                                    /*Se verifica si el documento es Inafecta o Exonerada, si no lo es se aplica el IGV*/
                                    if (op_inafecta == "0.00" && op_exonerada == "0.00")
                                    {
                                        /*Al precio del Item Se aplica el IGV*/
                                        double Precio_Unitario = double.Parse(PriceAmount.Item(0).InnerText);
                                        Precio_Unitario = Precio_Unitario + (Precio_Unitario * 0.18);
                                        item.Cs_tag_Price_PriceAmount = Precio_Unitario.ToString("0.00", CultureInfo.InvariantCulture);
                                    }
                                    /*Si no corresponde se envia el valor de la etiqueta sin modificación*/
                                    else
                                    {
                                        item.Cs_tag_Price_PriceAmount = PriceAmount.Item(0).InnerText;
                                    }
                                }
                            }
                            /*FIN MODIFICACIóN*/

                            //Cristhian|25/10/2017|FEICONT2-396
                            /*Se añade el la etiqueta de donde se obtendra el valor del descuento unitario*/
                            /*NUEVO INICIO*/
                            /*Si se tiene almenos un descuento unitario registrado, se realiza lo siguiente*/
                            if (VerificarDescuentoUnitario.Count > 0)
                            {
                                /*Se declara la variable del tipo Nodo del XML*/
                                XmlNodeList DescuentoUnitario = null;
                                /*Se asigna el valor del nodo buscado por el nombre de la etiqueta*/
                                DescuentoUnitario = xmlItem.GetElementsByTagName("AllowanceCharge");

                                /*Si se encuentra almenos un descuento unitario, se le asiga el valor encontrado en la etiqueta*/
                                if (DescuentoUnitario.Count > 0)
                                {
                                    /*Se obtiene los detalles del producto*/
                                    XmlDocument xmlItemPrice = new XmlDocument();
                                    xmlItemPrice.LoadXml(DescuentoUnitario.Item(0).OuterXml);

                                    /*se obtine el valor del monto del descuento del producto*/
                                    XmlNodeList DescountAmount = xmlItemPrice.GetElementsByTagName("Amount");
                                    /*Si se tiene el valor es asignado a la variable correspondiente*/
                                    if (DescountAmount.Count > 0)
                                    {
                                        //Se tubo que crear un nuevo Campo o Item para el Descuento-En Document LINE
                                        item.Cs_tag_AllowanceCharge_Amount = DescountAmount.Item(0).InnerText;
                                    }
                                }
                                /*si no se encuentra un descuento unitario se le asigna el valor de 0.00 */
                                else
                                {
                                    item.Cs_tag_AllowanceCharge_Amount = "0.00";
                                }
                            }
                            /*NUEVO FIN*/

                            if (op_gratuita != "0.00")
                            {
                                foreach (var itm in Lista_items_princingreference)
                                {
                                    if (itm.Cs_tag_PriceTypeCode == "02")
                                    {
                                        condition_price = itm.Cs_tag_PriceAmount_currencyID;
                                    }
                                }
                            }

                            var text_detalle = "";
                            foreach (var det_it in Lista_items_description)
                            {
                                text_detalle += det_it.Cs_tag_Description + " \n";
                            }

                            PdfPCell itCodigo = new PdfPCell(new Phrase(numero_item.ToString(), _clienteFontContentMinFooter));
                            itCodigo.Colspan = 1;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itCodigo.BorderWidthBottom = 0.75f;

                            }
                            else
                            {
                                itCodigo.BorderWidthBottom = 0.75f;
                            }
                            itCodigo.BorderWidthLeft = 0.75f;
                            itCodigo.BorderWidthRight = 0.75f;
                            itCodigo.BorderWidthTop = 0;
                            itCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPCell itCantidad = new PdfPCell(new Phrase(item.Cs_tag_invoicedQuantity, _clienteFontContentMinFooter));
                            itCantidad.Colspan = 1;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itCantidad.BorderWidthBottom = 0.75f;

                            }
                            else
                            {
                                itCantidad.BorderWidthBottom = 0.75f;
                            }

                            itCantidad.BorderWidthLeft = 0;
                            itCantidad.BorderWidthRight = 0.75f;
                            itCantidad.BorderWidthTop = 0;
                            itCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                            /*La columna Unidad de Medida parece que es condicional-dependiendo del documento se muestra*/
                            PdfPCell itUnidadMedida = new PdfPCell(new Phrase(item.Cs_tag_Item_SellersItemIdentification, _clienteFontContentMinFooter));
                            itUnidadMedida.Colspan = 2;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itUnidadMedida.BorderWidthBottom = 0.75f;
                            }
                            else
                            {
                                itUnidadMedida.BorderWidthBottom = 0.75f;
                            }

                            itUnidadMedida.BorderWidthLeft = 0;
                            itUnidadMedida.BorderWidthRight = 0.75f;
                            itUnidadMedida.BorderWidthTop = 0;
                            itUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;
                            /*Fin Columna Unidad de Medida*/

                            /*Columna de Descripción - tambien se le da la dimension de la celda*/
                            PdfPCell itDescripcion = new PdfPCell(new Phrase(text_detalle, _clienteFontContentMinFooter));
                            itDescripcion.Colspan = 6;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itDescripcion.BorderWidthBottom = 0.75f;

                            }
                            else
                            {
                                itDescripcion.BorderWidthBottom = 0.75f;
                            }

                            itDescripcion.BorderWidthLeft = 0;
                            itDescripcion.BorderWidthRight = 0.75f;
                            itDescripcion.BorderWidthTop = 0;
                            itDescripcion.PaddingBottom = 5f;
                            itDescripcion.HorizontalAlignment = Element.ALIGN_LEFT;
                            /*Fin Columna Unidad de Medida*/

                            /*Columna de Precio Unitario - tambien se le da la dimension de la celda*/
                            if (op_gratuita != "0.00")
                            {
                                if (condition_price == "")
                                {
                                    valor_unitario_item = item.Cs_tag_Price_PriceAmount;
                                }
                                else
                                {
                                    valor_unitario_item = condition_price;
                                }
                            }
                            else
                            {
                                valor_unitario_item = item.Cs_tag_Price_PriceAmount;
                            }

                            PdfPCell itPrecUnit = new PdfPCell(new Phrase(valor_unitario_item, _clienteFontContentMinFooter));
                            itPrecUnit.Colspan = 1;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itPrecUnit.BorderWidthBottom = 0.75f;

                            }
                            else
                            {
                                itPrecUnit.BorderWidthBottom = 0.75f;
                            }

                            itPrecUnit.BorderWidthLeft = 0;
                            itPrecUnit.BorderWidthRight = 0.75f;
                            itPrecUnit.BorderWidthTop = 0;
                            itPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;
                            /*Fin Columna de Precio Unitario*/

                            //Cristhian|25/10/2017|FEICONT2-396
                            /*Columna de Descuentos - tambien se le da la dimension de la celda*/
                            /*Se cambia la dimension de la columan descripcion para que entre la columna de descuento unitario*/
                            /*NUEVO INICIO*/
                            PdfPCell itemDescuentoUnitario = new PdfPCell(new Phrase(item.Cs_tag_AllowanceCharge_Amount, _clienteFontContentMinFooter));
                            /*Si se tiene almenos un descuento unitario entra al IF*/
                            if (VerificarDescuentoUnitario.Count > 0)
                            {
                                /*Se dismunuye la dimension de la columna descripción*/
                                itDescripcion.Colspan = 5;

                                /*Se asigna el valor de uno a la dimensioin de la nueva columnas*/
                                itemDescuentoUnitario.Colspan = 1;

                                /*se agrega el borde superrior, a medida que se llena la grilla (en el PDF)*/
                                if (numero_item == total_items & op_detraccion == "0.00")
                                {
                                    itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                                }
                                else
                                {
                                    itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                                }

                                /*Parametros estandar de presentacion del documento PDF*/
                                itemDescuentoUnitario.BorderWidthLeft = 0;
                                itemDescuentoUnitario.BorderWidthRight = 0.75f;
                                itemDescuentoUnitario.BorderWidthTop = 0;
                                itemDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            /*Fin Columna de Descuentos*/
                            /*NUEVO FIN*/

                            /*Columna de Precio Total - tambien se le da la dimension de la celda*/
                            if (op_gratuita != "0.00")
                            {
                                if (valor_unitario_item == "")
                                {
                                    valor_unitario_item = "0.00";
                                }
                                if (condition_price != "")
                                {
                                    double valor_total_item_1 = double.Parse(item.Cs_tag_Price_PriceAmount, CultureInfo.InvariantCulture) * double.Parse(item.Cs_tag_invoicedQuantity, CultureInfo.InvariantCulture);
                                    valor_total_item = valor_total_item_1.ToString("0.00", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    double valor_total_item_1 = double.Parse(item.Cs_tag_Price_PriceAmount, CultureInfo.InvariantCulture) * double.Parse(item.Cs_tag_invoicedQuantity, CultureInfo.InvariantCulture);
                                    valor_total_item = valor_total_item_1.ToString("0.00", CultureInfo.InvariantCulture);
                                    //valor_total_item = item.Cs_tag_LineExtensionAmount_currencyID;
                                }
                            }
                            else
                            {
                                //valor_total_item = item.Cs_tag_LineExtensionAmount_currencyID;
                                double valor_total_item_1 = double.Parse(item.Cs_tag_Price_PriceAmount, CultureInfo.InvariantCulture) * double.Parse(item.Cs_tag_invoicedQuantity, CultureInfo.InvariantCulture);
                                valor_total_item = valor_total_item_1.ToString("0.00", CultureInfo.InvariantCulture);
                            }
                            PdfPCell itImporte = new PdfPCell(new Phrase(valor_total_item, _clienteFontContentMinFooter));
                            itImporte.Colspan = 1;
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itImporte.BorderWidthBottom = 0.75f;

                            }
                            else
                            {
                                itImporte.BorderWidthBottom = 0.75f;
                            }

                            itImporte.BorderWidthLeft = 0;
                            itImporte.BorderWidthRight = 0.75f;
                            itImporte.BorderWidthTop = 0;
                            itImporte.HorizontalAlignment = Element.ALIGN_CENTER;
                            /*Fin Columna de Precio Total*/

                            //sub_total += Double.Parse(item.Cs_tag_LineExtensionAmount_currencyID);
                            // sub_total += double.Parse(item.Cs_tag_LineExtensionAmount_currencyID, CultureInfo.InvariantCulture);
                            // Añadimos las celdas a la tabla
                            tblInfoComprobante.AddCell(itCodigo);
                            tblInfoComprobante.AddCell(itCantidad);
                            tblInfoComprobante.AddCell(itUnidadMedida);
                            tblInfoComprobante.AddCell(itDescripcion);
                            tblInfoComprobante.AddCell(itPrecUnit);
                            if (VerificarDescuentoUnitario.Count > 0)
                            {
                                tblInfoComprobante.AddCell(itemDescuentoUnitario);//Campo Agregado para el Descuento 25/10/2017 FEICONT2-396
                            }
                            tblInfoComprobante.AddCell(itImporte);
                        }


                        if (op_detraccion != "0.00")
                        {
                            //agregar mensaje

                            PdfPCell celda_blanco = new PdfPCell(new Phrase(" ", _clienteFontContent));
                            celda_blanco.Colspan = 1;
                            celda_blanco.BorderWidthBottom = 0.75f;
                            celda_blanco.BorderWidthLeft = 0;
                            celda_blanco.BorderWidthRight = 0.75f;
                            celda_blanco.BorderWidthTop = 0;

                            PdfPCell celda_blanco_dos = new PdfPCell(new Phrase(" ", _clienteFontContent));
                            celda_blanco_dos.Colspan = 2;
                            celda_blanco_dos.BorderWidthBottom = 0.75f;
                            celda_blanco_dos.BorderWidthLeft = 0;
                            celda_blanco_dos.BorderWidthRight = 0.75f;
                            celda_blanco_dos.BorderWidthTop = 0;

                            PdfPCell celda_blanco_right = new PdfPCell(new Phrase(" ", _clienteFontContent));
                            celda_blanco_right.Colspan = 1;
                            celda_blanco_right.BorderWidthBottom = 0.75f;
                            celda_blanco_right.BorderWidthLeft = 0;
                            celda_blanco_right.BorderWidthRight = 0.75f;
                            celda_blanco_right.BorderWidthTop = 0;

                            PdfPCell celda_blanco_left = new PdfPCell(new Phrase(" ", _clienteFontContent));
                            celda_blanco_left.Colspan = 1;
                            celda_blanco_left.BorderWidthBottom = 0.75f;
                            celda_blanco_left.BorderWidthLeft = 0.75f;
                            celda_blanco_left.BorderWidthRight = 0.75f;
                            celda_blanco_left.BorderWidthTop = 0;

                            var parrafo = new Paragraph();
                            parrafo.Font = _clienteFontContentMinFooter;
                            parrafo.Add("Operación sujeta al Sistema de Pago de Obligaciones Tributarias con el Gobierno Central \n");
                            parrafo.Add("SPOT " + porcentaje_detraccion + "% " + cuenta_nacion + " \n");

                            PdfPCell celda_parrafo = new PdfPCell(parrafo);
                            celda_parrafo.Colspan = 6;
                            celda_parrafo.BorderWidthBottom = 0.75f;
                            celda_parrafo.BorderWidthLeft = 0;
                            celda_parrafo.BorderWidthRight = 0.75f;
                            celda_parrafo.BorderWidthTop = 0;
                            celda_parrafo.PaddingTop = 10f;
                            celda_parrafo.HorizontalAlignment = Element.ALIGN_CENTER;

                            tblInfoComprobante.AddCell(celda_blanco_left);
                            tblInfoComprobante.AddCell(celda_blanco);
                            tblInfoComprobante.AddCell(celda_blanco_dos);
                            tblInfoComprobante.AddCell(celda_parrafo);
                            tblInfoComprobante.AddCell(celda_blanco);
                            tblInfoComprobante.AddCell(celda_blanco_right);

                        }
                        /* ------end items------*/
                        doc.Add(tblInfoComprobante);
                        doc.Add(tblBlanco);

                        //Cristhian|25/10/2017|FEICONT2-396
                        /*Tabla para mostrar el descuento global que debe aparecer en todos los comprobantes electronicos*/
                        /*NUEVO INICIO*/
                        if (DescuentoGlobal != "")
                        {
                            /*Se crea la tabla Descuento*/
                            PdfPTable tblInfoDescuentoGlobal = new PdfPTable(10);
                            tblInfoDescuentoGlobal.WidthPercentage = 100;

                            /*Se crea la celda donde ira el valor del descuento global, esta en blanco solo para dar formato a la tabla*/
                            PdfPCell infoDescuentoGlobal = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                            infoDescuentoGlobal.BorderWidthTop = 0.75f;
                            infoDescuentoGlobal.BorderWidthBottom = 0.75f;
                            infoDescuentoGlobal.BorderWidthLeft = 0.75f;
                            infoDescuentoGlobal.BorderWidthRight = 0;
                            infoDescuentoGlobal.Colspan = 5;
                            infoDescuentoGlobal.HorizontalAlignment = Element.ALIGN_LEFT;

                            /*Se crea una celda con el nombre del descuento*/
                            PdfPCell infoTotalDescuentoGlobal = new PdfPCell(new Phrase(" Descuento Global ", _clienteFontBoldMin));
                            infoTotalDescuentoGlobal.BorderWidthTop = 0.75f;
                            infoTotalDescuentoGlobal.BorderWidthBottom = 0.75f;
                            infoTotalDescuentoGlobal.BorderWidthLeft = 0;
                            infoTotalDescuentoGlobal.BorderWidthRight = 0;
                            infoTotalDescuentoGlobal.Colspan = 3;
                            infoTotalDescuentoGlobal.HorizontalAlignment = Element.ALIGN_RIGHT;

                            /*Se crea la celda donde ira el valor totoal del descuento*/
                            var DescuentoDato = GetCurrencySymbol(DocumentCurrencyCode);
                            string Moneda_Simbolo = DescuentoDato.CurrencySymbol;
                            if (Moneda_Simbolo == "S/."|| Moneda_Simbolo =="s/.")
                            {
                                Moneda_Simbolo = "S/";
                            }
                            PdfPCell infoTotalDescuentoGlobalVal = new PdfPCell(new Phrase(Moneda_Simbolo + " " + DescuentoGlobal, _clienteFontContent));
                            infoTotalDescuentoGlobalVal.BorderWidthTop = 0.75f;
                            infoTotalDescuentoGlobalVal.BorderWidthBottom = 0.75f;
                            infoTotalDescuentoGlobalVal.BorderWidthRight = 0.75f;
                            infoTotalDescuentoGlobalVal.BorderWidthLeft = 0;
                            infoTotalDescuentoGlobalVal.Colspan = 2;
                            infoTotalDescuentoGlobalVal.HorizontalAlignment = Element.ALIGN_RIGHT;

                            /*Se añade las celdas a la tabla DescuentoGlobal*/
                            tblInfoDescuentoGlobal.AddCell(infoDescuentoGlobal);
                            tblInfoDescuentoGlobal.AddCell(infoTotalDescuentoGlobal);
                            tblInfoDescuentoGlobal.AddCell(infoTotalDescuentoGlobalVal);
                            doc.Add(tblInfoDescuentoGlobal);

                            /*Se añade una tabla en blanco para dar espacio a la siguiente tabla*/
                            doc.Add(tblBlanco);
                        }
                        /*NUEVO FIN*/

                        if (InvoiceTypeCode == "03" | InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                        {
                            PdfPTable tblInfoOperacionesGratuitas = new PdfPTable(10);
                            tblInfoOperacionesGratuitas.WidthPercentage = 100;

                            PdfPCell infoTotalOpGratuitas = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                            infoTotalOpGratuitas.BorderWidthTop = 0.75f;
                            infoTotalOpGratuitas.BorderWidthBottom = 0.75f;
                            infoTotalOpGratuitas.BorderWidthLeft = 0.75f;
                            infoTotalOpGratuitas.BorderWidthRight = 0;
                            infoTotalOpGratuitas.Colspan = 5;
                            infoTotalOpGratuitas.HorizontalAlignment = Element.ALIGN_LEFT;

                            PdfPCell infoTotalOpGratuitasLabel = new PdfPCell(new Phrase("Valor de venta de operaciones gratuitas", _clienteFontBoldMin));
                            infoTotalOpGratuitasLabel.BorderWidthTop = 0.75f;
                            infoTotalOpGratuitasLabel.BorderWidthBottom = 0.75f;
                            infoTotalOpGratuitasLabel.BorderWidthLeft = 0;
                            infoTotalOpGratuitasLabel.BorderWidthRight = 0;
                            infoTotalOpGratuitasLabel.Colspan = 3;
                            infoTotalOpGratuitasLabel.HorizontalAlignment = Element.ALIGN_RIGHT;

                            var monedaDatos1 = GetCurrencySymbol(DocumentCurrencyCode);
                            string Moneda_Simbolo = monedaDatos1.CurrencySymbol;
                            if (Moneda_Simbolo == "S/." || Moneda_Simbolo == "s/.")
                            {
                                Moneda_Simbolo = "S/";
                            }
                            PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(Moneda_Simbolo + " " + op_gratuita, _clienteFontContent));
                            infoTotalOpGratuitasVal.BorderWidthTop = 0.75f;
                            infoTotalOpGratuitasVal.BorderWidthBottom = 0.75f;
                            infoTotalOpGratuitasVal.BorderWidthRight = 0.75f;
                            infoTotalOpGratuitasVal.BorderWidthLeft = 0;
                            infoTotalOpGratuitasVal.Colspan = 2;
                            infoTotalOpGratuitasVal.HorizontalAlignment = Element.ALIGN_RIGHT;


                            tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitas);
                            tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasLabel);
                            tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasVal);
                            doc.Add(tblInfoOperacionesGratuitas);

                            doc.Add(tblBlanco);
                            if (InvoiceTypeCode == "03")
                            {
                                /*----------- Monto total en letras --------------*/
                                NumLetra totalLetras = new NumLetra();
                                PdfPTable tblInfoMontoTotal = new PdfPTable(10);

                                tblInfoMontoTotal.WidthPercentage = 100;

                                PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                                infoTotal.BorderWidth = 0.75f;
                                infoTotal.Colspan = 7;
                                infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;

                                tblInfoMontoTotal.AddCell(infoTotal);


                                PdfPTable tbl_monto_total1 = new PdfPTable(2);
                                tbl_monto_total1.WidthPercentage = 100;


                                var monedaDatos2 = GetCurrencySymbol(DocumentCurrencyCode);
                                Moneda_Simbolo = monedaDatos2.CurrencySymbol;
                                if (Moneda_Simbolo == "S/." || Moneda_Simbolo == "s/.")
                                {
                                    Moneda_Simbolo = "S/";
                                }
                                PdfPCell labelMontoTotal1 = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                                labelMontoTotal1.HorizontalAlignment = Element.ALIGN_LEFT;
                                PdfPCell valueMontoTotal1 = new PdfPCell(new Phrase(Moneda_Simbolo + " " + LMTPayableAmount, _clienteFontContent));
                                valueMontoTotal1.HorizontalAlignment = Element.ALIGN_RIGHT;

                                tbl_monto_total1.AddCell(labelMontoTotal1);
                                tbl_monto_total1.AddCell(valueMontoTotal1);

                                PdfPCell contenedor = new PdfPCell(tbl_monto_total1);
                                contenedor.Colspan = 3;
                                contenedor.Border = 0;
                                contenedor.PaddingLeft = 10f;
                                tblInfoMontoTotal.AddCell(contenedor);
                                doc.Add(tblInfoMontoTotal);
                                /*-------------End Monto Total----------------*/
                                doc.Add(tblBlanco);
                            }


                        }
                        else
                        {

                            if (op_gratuita != "0.00")
                            {
                                /*Monto de Transferencia Gratuita*/

                                PdfPTable tblInfoOperacionesGratuitas = new PdfPTable(10);
                                tblInfoOperacionesGratuitas.WidthPercentage = 100;

                                PdfPCell infoTotalOpGratuitas = new PdfPCell(new Phrase("TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE", _clienteFontContentMinFooter));
                                infoTotalOpGratuitas.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitas.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitas.BorderWidthLeft = 0.75f;
                                infoTotalOpGratuitas.BorderWidthRight = 0;
                                infoTotalOpGratuitas.Colspan = 6;
                                infoTotalOpGratuitas.HorizontalAlignment = Element.ALIGN_LEFT;

                                PdfPCell infoTotalOpGratuitasLabel = new PdfPCell(new Phrase("Valor de venta de operaciones gratuitas", _clienteFontContentMinFooter));
                                infoTotalOpGratuitasLabel.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitasLabel.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitasLabel.BorderWidthLeft = 0;
                                infoTotalOpGratuitasLabel.BorderWidthRight = 0;
                                infoTotalOpGratuitasLabel.Colspan = 3;
                                infoTotalOpGratuitasLabel.HorizontalAlignment = Element.ALIGN_CENTER;

                                var monedaDatos1 = GetCurrencySymbol(DocumentCurrencyCode);
                                string Moneda_Simbolo = monedaDatos1.CurrencySymbol;
                                if (Moneda_Simbolo == "S/." || Moneda_Simbolo == "s/.")
                                {
                                    Moneda_Simbolo = "S/";
                                }
                                PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(Moneda_Simbolo + " " + op_gratuita, _clienteFontContent));
                                infoTotalOpGratuitasVal.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthRight = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthLeft = 0;
                                infoTotalOpGratuitasVal.Colspan = 1;
                                infoTotalOpGratuitasVal.HorizontalAlignment = Element.ALIGN_RIGHT;


                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitas);
                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasLabel);
                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasVal);
                                doc.Add(tblInfoOperacionesGratuitas);

                                doc.Add(tblBlanco);
                            }
                            else
                            {


                                PdfPTable tblInfoOperacionesGratuitas = new PdfPTable(10);
                                tblInfoOperacionesGratuitas.WidthPercentage = 100;

                                PdfPCell infoTotalOpGratuitas = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                                infoTotalOpGratuitas.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitas.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitas.BorderWidthLeft = 0.75f;
                                infoTotalOpGratuitas.BorderWidthRight = 0;
                                infoTotalOpGratuitas.Colspan = 5;
                                infoTotalOpGratuitas.HorizontalAlignment = Element.ALIGN_LEFT;

                                PdfPCell infoTotalOpGratuitasLabel = new PdfPCell(new Phrase("Valor de venta de operaciones gratuitas", _clienteFontBoldMin));
                                infoTotalOpGratuitasLabel.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitasLabel.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitasLabel.BorderWidthLeft = 0;
                                infoTotalOpGratuitasLabel.BorderWidthRight = 0;
                                infoTotalOpGratuitasLabel.Colspan = 3;
                                infoTotalOpGratuitasLabel.HorizontalAlignment = Element.ALIGN_RIGHT;

                                var monedaDatos1 = GetCurrencySymbol(DocumentCurrencyCode);
                                string Moneda_Simbolo = monedaDatos1.CurrencySymbol;
                                if (Moneda_Simbolo == "S/." || Moneda_Simbolo == "s/.")
                                {
                                    Moneda_Simbolo = "S/";
                                }
                                PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(Moneda_Simbolo + " " + op_gratuita, _clienteFontContent));
                                infoTotalOpGratuitasVal.BorderWidthTop = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthBottom = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthRight = 0.75f;
                                infoTotalOpGratuitasVal.BorderWidthLeft = 0;
                                infoTotalOpGratuitasVal.Colspan = 2;
                                infoTotalOpGratuitasVal.HorizontalAlignment = Element.ALIGN_RIGHT;


                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitas);
                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasLabel);
                                tblInfoOperacionesGratuitas.AddCell(infoTotalOpGratuitasVal);
                                doc.Add(tblInfoOperacionesGratuitas);

                                doc.Add(tblBlanco);

                            }

                        }



                        /*----------- CASO BOLETA SOLO MONTO TOTAL --------------*/
                        if (InvoiceTypeCode == "03")
                        {
                            /*  PdfPTable tblMontoTotal = new PdfPTable(10);
                              tblMontoTotal.WidthPercentage = 100;

                              PdfPCell monto_blanco = new PdfPCell(new Phrase(" ", _clienteFontContent));
                              monto_blanco.Border = 0;
                              monto_blanco.Colspan = 6;
                              tblMontoTotal.AddCell(monto_blanco);

                              PdfPTable tbl_monto_total = new PdfPTable(2);
                              tbl_monto_total.WidthPercentage = 100;
                              var monedaDatos1 = GetCurrencySymbol(cabecera.Cs_tag_DocumentCurrencyCode);
                              PdfPCell labelMontoTotal = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                              labelMontoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                              PdfPCell valueMontoTotal = new PdfPCell(new Phrase(monedaDatos1.CurrencySymbol + " " + cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID, _clienteFontContent));
                              valueMontoTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                              tbl_monto_total.AddCell(labelMontoTotal);
                              tbl_monto_total.AddCell(valueMontoTotal);

                              PdfPCell monto_total = new PdfPCell(tbl_monto_total);
                              monto_total.Border = 0;
                              monto_total.Colspan = 4;
                              tblMontoTotal.AddCell(monto_total);

                              doc.Add(tblMontoTotal);*/
                        }
                        /*-------------End Monto Total----------------*/

                        //FOOTER
                        PdfPTable tblInfoFooter = new PdfPTable(10);
                        tblInfoFooter.WidthPercentage = 100;

                        //comentarios
                        PdfPTable tblInfoComentarios = new PdfPTable(1);
                        tblInfoComentarios.WidthPercentage = 100;

                        //tblInfoComentarios.TotalWidth = 144f;
                        //tblInfoComentarios.LockedWidth = true;

                        PdfPCell tituComentarios = new PdfPCell(new Phrase("Observaciones:", _clienteFontBold));
                        tituComentarios.Border = 0;
                        tituComentarios.HorizontalAlignment = Element.ALIGN_LEFT;
                        tituComentarios.PaddingBottom = 5f;
                        if (InvoiceTypeCode == "03")
                        {
                            //cuando es boleta
                            tituComentarios.PaddingTop = -15f;
                        }
                        else
                        {
                            tituComentarios.PaddingTop = -5f;
                        }

                        tblInfoComentarios.AddCell(tituComentarios);



                        var comentarios_string = teclaf8 + " " + teclavtrlm;

                        PdfPCell contComentarios = new PdfPCell(new Phrase(teclavtrlm, _clienteFontContentMinFooter));
                        contComentarios.BorderWidth = 0.75f;
                        contComentarios.PaddingBottom = 5f;
                        contComentarios.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tblInfoComentarios.AddCell(contComentarios);

                        /* if (cabecera.Cs_tag_InvoiceTypeCode != "03")
                         {*/
                        PdfPCell tituDatos = new PdfPCell(new Phrase("DATOS:", _clienteFontBold));
                        tituDatos.Border = 0;
                        tituDatos.HorizontalAlignment = Element.ALIGN_LEFT;
                        tituDatos.PaddingBottom = 5f;
                        tblInfoComentarios.AddCell(tituDatos);


                        /* TABLA PARA NRO ORDEN PEDIDO Y CUENTAS BANCARIAS*/
                        PdfPTable tblOrdenCuenta = new PdfPTable(11);
                        tblOrdenCuenta.WidthPercentage = 100;
                        PdfPCell labelOrden = new PdfPCell(new Phrase("Nº Orden de Pedido:", _clienteFontBoldContentMinFooter));
                        labelOrden.Colspan = 2;
                        labelOrden.Border = 0;
                        labelOrden.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell valueOrden = new PdfPCell(new Phrase(teclaf8, _clienteFontContent));
                        valueOrden.Colspan = 9;
                        valueOrden.Border = 0;
                        valueOrden.HorizontalAlignment = Element.ALIGN_LEFT;
                        tblOrdenCuenta.AddCell(labelOrden);
                        tblOrdenCuenta.AddCell(valueOrden);

                        PdfPCell labelCuentas = new PdfPCell(new Phrase("Ctas Bancarias:", _clienteFontBoldContentMinFooter));
                        labelCuentas.Colspan = 2;
                        labelCuentas.Border = 0;
                        labelCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                        var pdat = new Paragraph();
                        pdat.Font = _clienteFontContentMinFooter;
                        pdat.Add(cuentasbancarias);
                        PdfPCell valueCuentas = new PdfPCell(pdat);
                        valueCuentas.Colspan = 9;
                        valueCuentas.Border = 0;
                        valueCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                        tblOrdenCuenta.AddCell(labelCuentas);
                        tblOrdenCuenta.AddCell(valueCuentas);

                        tblInfoComentarios.AddCell(tblOrdenCuenta);

                        PdfPCell cellBlanco = new PdfPCell(new Phrase("", _clienteFontBoldContentMinFooter));
                        cellBlanco.Border = 0;

                        tblInfoComentarios.AddCell(cellBlanco);
                        // }
                        /*PdfPCell contDatos = new PdfPCell(pdat);
                        contDatos.BorderWidth = 0.75f;
                        contDatos.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        tblInfoComentarios.AddCell(contDatos);
                        */

                        //resumen 
                        PdfPTable tblInfoResumen = new PdfPTable(4);
                        tblInfoResumen.WidthPercentage = 100;

                        //tblInfoResumen.TotalWidth = 144f;
                        //tblInfoResumen.LockedWidth = true;

                        //Cristhian|05/10/2017|FEICONT2-381
                        /*El sub_total debe ser el monto de op_gravada + */
                        /*MODIFICACION INICIO*/
                        sub_total += double.Parse(op_gravada, CultureInfo.InvariantCulture);
                        sub_total += double.Parse(total_descuentos, CultureInfo.InvariantCulture);
                        /*MODIFICACION FIN*/

                        if (InvoiceTypeCode != "03")
                        {
                            // moneda

                            var monedaDatos = GetCurrencySymbol(DocumentCurrencyCode);
                            string output_subtotal = "";


                            if (op_gratuita == "0.00")
                            {
                                output_subtotal = sub_total.ToString("0.00", CultureInfo.InvariantCulture);
                            }
                            else
                            {//verificar si existe op gravada y ponerlo si existe
                                if (op_gravada != "0.00")
                                {
                                    output_subtotal = sub_total.ToString("0.00", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    output_subtotal = "0.00";
                                }

                            }

                            //Cristhian|23/01/2018|FEICONT2-572
                            /*Se crea una validacion pequeña para eliminar el punto del simbolo del Nuevo Sol (Moneda Nacional)*/
                            /*NUEVO INICIO*/
                            /*Se declara la variable string*/
                            string Simbolo_Moneda = monedaDatos.CurrencySymbol;
                            /*Se verifica si contiene alguno de los siguientes simbolos*/
                            if (Simbolo_Moneda == "S/."|| Simbolo_Moneda == "s/.")
                            {
                                /*Se cambia el simbolo del nuevo sol*/
                                Simbolo_Moneda = "S/";
                            }
                            /*NUEVO FIN*/

                            PdfPCell resItem6 = new PdfPCell(new Phrase("Sub Total", _clienteFontBold));
                            resItem6.Colspan = 2;
                            resItem6.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue6 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + output_subtotal, _clienteFontContent));
                            resvalue6.Colspan = 2;
                            resvalue6.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem6);
                            tblInfoResumen.AddCell(resvalue6);

                            PdfPCell resItem7 = new PdfPCell(new Phrase("Otros Cargos", _clienteFontBold));
                            resItem7.Colspan = 2;
                            resItem7.HorizontalAlignment = Element.ALIGN_LEFT;

                            if (LMTChargeTotalAmount.Trim().Length == 0)
                            {
                                LMTChargeTotalAmount = "0.00";
                            }
                            PdfPCell resvalue7 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + LMTChargeTotalAmount, _clienteFontContent));
                            resvalue7.Colspan = 2;
                            resvalue7.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem7);
                            tblInfoResumen.AddCell(resvalue7);

                            PdfPCell resItem8 = new PdfPCell(new Phrase("Total de Descuento", _clienteFontBold));
                            resItem8.Colspan = 2;
                            resItem8.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue8 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + total_descuentos, _clienteFontContent));
                            resvalue8.Colspan = 2;
                            resvalue8.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem8);
                            tblInfoResumen.AddCell(resvalue8);

                            PdfPCell resItem1 = new PdfPCell(new Phrase("Operaciones Gravadas", _clienteFontBold));
                            resItem1.Colspan = 2;
                            resItem1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue1 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + op_gravada, _clienteFontContent));
                            resvalue1.Colspan = 2;
                            resvalue1.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem1);
                            tblInfoResumen.AddCell(resvalue1);

                            PdfPCell resItem2 = new PdfPCell(new Phrase("Operaciones Inafectas", _clienteFontBold));
                            resItem2.Colspan = 2;
                            resItem2.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue2 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + op_inafecta, _clienteFontContent));
                            resvalue2.Colspan = 2;
                            resvalue2.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem2);
                            tblInfoResumen.AddCell(resvalue2);

                            PdfPCell resItem3 = new PdfPCell(new Phrase("Operaciones Exoneradas", _clienteFontBold));
                            resItem3.Colspan = 2;
                            resItem3.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue3 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + op_exonerada, _clienteFontContent));
                            resvalue3.Colspan = 2;
                            resvalue3.HorizontalAlignment = Element.ALIGN_RIGHT;

                            tblInfoResumen.AddCell(resItem3);
                            tblInfoResumen.AddCell(resvalue3);
                            
                            //Cristhian|01/02/2018|FEICONT2-603
                            /*Se agrega el texto de IGV 18% y se agrega una validacion para que no se considere en boletas*/
                            /*INICIO MODIFICACIóN*/
                            if (imp_IGV != "" && InvoiceTypeCode != "03")
                            {
                                PdfPCell resItem4_1 = new PdfPCell(new Phrase("IGV 18%", _clienteFontBold));
                                resItem4_1.Colspan = 2;
                                resItem4_1.HorizontalAlignment = Element.ALIGN_LEFT;
                                PdfPCell resvalue4_1 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + imp_IGV, _clienteFontContent));
                                resvalue4_1.Colspan = 2;
                                resvalue4_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tblInfoResumen.AddCell(resItem4_1);
                                tblInfoResumen.AddCell(resvalue4_1);
                            }
                            /*FIN MODIFICACIóN*/

                            /*if (imp_ISC != "")
                            {
                                PdfPCell resItem4_2 = new PdfPCell(new Phrase("ISC", _clienteFontBold));
                                resItem4_2.Colspan = 2;
                                resItem4_2.HorizontalAlignment = Element.ALIGN_LEFT;
                                PdfPCell resvalue4_2 = new PdfPCell(new Phrase(imp_ISC, _clienteFontContent));
                                resvalue4_2.Colspan = 2;
                                resvalue4_2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tblInfoResumen.AddCell(resItem4_2);
                                tblInfoResumen.AddCell(resvalue4_2);
                            }
                            if (imp_OTRO != "")
                            {
                                PdfPCell resItem4_3 = new PdfPCell(new Phrase("Otros tributos", _clienteFontBold));
                                resItem4_3.Colspan = 2;
                                resItem4_3.HorizontalAlignment = Element.ALIGN_LEFT;
                                PdfPCell resvalue4_3 = new PdfPCell(new Phrase(imp_OTRO, _clienteFontContent));
                                resvalue4_3.Colspan = 2;
                                resvalue4_3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tblInfoResumen.AddCell(resItem4_3);
                                tblInfoResumen.AddCell(resvalue4_3);
                            }*/

                            PdfPCell resItem5 = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                            resItem5.Colspan = 2;
                            resItem5.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue5 = new PdfPCell(new Phrase(Simbolo_Moneda + " " + LMTPayableAmount, _clienteFontContent));
                            resvalue5.Colspan = 2;
                            resvalue5.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem5);
                            tblInfoResumen.AddCell(resvalue5);

                            PdfPCell resItem9 = new PdfPCell(new Phrase("", _clienteFontBold));
                            resItem9.Colspan = 2;
                            resItem9.Border = 0;
                            resItem9.PaddingBottom = 0f;
                            resItem9.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue9 = new PdfPCell(new Phrase("", _clienteFontContent));
                            resvalue9.Colspan = 2;
                            resvalue9.Border = 0;
                            resvalue9.PaddingBottom = 0f;
                            resvalue9.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem9);
                            tblInfoResumen.AddCell(resvalue9);


                        }
                        //lado izquierdo
                        PdfPCell tblInfoFooterLeft = new PdfPCell(tblInfoComentarios);
                        if (InvoiceTypeCode != "03")
                        {
                            tblInfoFooterLeft.Colspan = 6;
                            tblInfoFooterLeft.PaddingRight = 10f;
                        }
                        else
                        {
                            tblInfoFooterLeft.Colspan = 10;
                            tblInfoFooterLeft.PaddingRight = 0;
                        }

                        tblInfoFooterLeft.Border = 0;

                        tblInfoFooter.AddCell(tblInfoFooterLeft);
                        //lado derecho

                        PdfPCell tblInfoFooterRight = new PdfPCell(tblInfoResumen);
                        tblInfoFooterRight.Colspan = 4;
                        tblInfoFooterRight.Border = 0;
                        tblInfoFooter.AddCell(tblInfoFooterRight);


                        doc.Add(tblInfoFooter);
                        doc.Add(tblBlanco);
                        if (InvoiceTypeCode == "01")
                        {
                            /*----------- Monto total en letras --------------*/
                            NumLetra totalLetras = new NumLetra();
                            PdfPTable tblInfoMontoTotal = new PdfPTable(1);
                            tblInfoMontoTotal.WidthPercentage = 100;
                            PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                            infoTotal.BorderWidth = 0.75f;
                            infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                            tblInfoMontoTotal.AddCell(infoTotal);
                            doc.Add(tblInfoMontoTotal);
                            /*-------------End Monto Total----------------*/
                            doc.Add(tblBlanco);
                        }

                        PdfPTable tblFooter = new PdfPTable(10);
                        tblFooter.WidthPercentage = 100;

                        var p = new Paragraph();
                        p.Font = _clienteFontBold;
                        p.Add(digestValue + "\n\n");
                        p.Add(info_general[3]);
                        p.Add(autorizacion_sunat + "\n");

                        PdfPCell DataHash = new PdfPCell(new Phrase(digestValue, _clienteFontBold));
                        DataHash.Border = 0;
                        DataHash.Colspan = 6;
                        DataHash.HorizontalAlignment = Element.ALIGN_CENTER;
                        // DataHash.PaddingTop = 5f;                

                        PdfPCell campo1 = new PdfPCell(p);
                        campo1.Colspan = 6;
                        campo1.Border = 0;
                        campo1.PaddingTop = 20f;
                        campo1.HorizontalAlignment = Element.ALIGN_CENTER;

                        //codigo de barras                               
                        //var hash = new clsNegocioXML();
                        //var hash_obtenido=hash.cs_fxHash(cabecera.Cs_pr_Document_Id);

                        Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                                {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                            };

                        //Cristhian|26/12/2017|FEICONT2-509
                        /*Se corrigio la cadena que se envia al Generador de la imagen QR*/
                        /*INICIO MODIFICACIóN*/
                        var textQR = ASPCustomerAssignedAccountID + " | " + InvoiceTypeCode + " | " + doc_serie + "|" + doc_correlativo + " | " + imp_IGV + " | " + LMTPayableAmount + " | " + IssueDate + " | " + ACPAdditionalAccountID + " | " + ACPCustomerAssignedAccountID + " |";
                        /*FIN MODIFICACIóN*/

                        BarcodeQRCode qrcode = new BarcodeQRCode(textQR, 400, 400, ob);

                        iTextSharp.text.Image qrcodeImage = qrcode.GetImage();

                        /* BarcodePDF417 barcod = new BarcodePDF417();
                         barcod.SetText(cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID+" | "+ cabecera.Cs_tag_InvoiceTypeCode+" | "+ doc_serie+" | "+doc_correlativo+" | "+ impuestos_globales.Cs_tag_TaxSubtotal_TaxAmount+" | "+ cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID+" | "+ cabecera.Cs_tag_IssueDate+" | "+cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID+" | "+cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID+" | "+ digestValue + " | "+signatureValue+" |");
                         barcod.ErrorLevel = 5;
                         barcod.Options = BarcodePDF417.PDF417_FORCE_BINARY;

                         iTextSharp.text.Image imagePDF417 = barcod.GetImage();*/
                        //qrcodeImage.ScaleAbsolute(100f, 90f);
                        PdfPCell blanco12 = new PdfPCell();
                        // blanco12.Image = qrcodeImage;
                        blanco12.AddElement(new Chunk(qrcodeImage, 55f, -65f));
                        blanco12.Border = 0;
                        blanco12.PaddingTop = 20f;
                        blanco12.Colspan = 4;


                        PdfPCell blanco121 = new PdfPCell(new Paragraph(" "));
                        blanco121.Border = 0;
                        blanco121.Colspan = 4;

                        tblFooter.AddCell(campo1);
                        tblFooter.AddCell(blanco12);
                        //tblFooter.AddCell(campo1);
                        // tblFooter.AddCell(blanco121);

                        doc.Add(tblFooter);


                        doc.Close();
                        File.SetAttributes(newFile, FileAttributes.Normal);
                        writer.Close();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } 
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" generar representacion impresa " + ex.ToString());
                return false;
            }
           
        }
        #endregion

        #region Comprobantes de TXT
        public static bool getRepresentacionImpresaDocumentoCargaTXT(string pathToSaved, clsEntidadDocument cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal();
            try
            {
                var doc_serie = "";
                var doc_correlativo = "";
                if (cabecera != null)
                {

                    string[] partes = cabecera.Cs_tag_ID.Split('-');
                    DateTime dt = DateTime.ParseExact(cabecera.Cs_tag_IssueDate, "yyyy-MM-dd", null);
                    doc_serie = partes[0];
                    doc_correlativo = partes[1];
                    string newFile = pathToSaved;

                    if (File.Exists(newFile))
                    {
                        File.Delete(newFile);
                    }

                    XmlDocument xmlDocument = new XmlDocument();
                    //var textXml = cabecera.Cs_pr_XML;
                    var textXml = textXML;
                    textXml = textXml.Replace("cbc:", "");
                    textXml = textXml.Replace("cac:", "");
                    textXml = textXml.Replace("sac:", "");
                    textXml = textXml.Replace("ext:", "");
                    textXml = textXml.Replace("ds:", "");
                    xmlDocument.LoadXml(textXml);

                    var signatureValue = xmlDocument.GetElementsByTagName("SignatureValue")[0].InnerText;
                    var digestValue = xmlDocument.GetElementsByTagName("DigestValue")[0].InnerText;

                    string InvoiceTypeCode = String.Empty;
                    XmlNodeList InvoiceTypeCodeXml = xmlDocument.GetElementsByTagName("InvoiceTypeCode");
                    if (InvoiceTypeCodeXml.Count > 0)
                    {
                        InvoiceTypeCode = xmlDocument.GetElementsByTagName("InvoiceTypeCode")[0].InnerText;
                    }
                    else
                    {
                        InvoiceTypeCode = cabecera.Cs_tag_InvoiceTypeCode;

                    }

                    string IssueDate = xmlDocument.GetElementsByTagName("IssueDate")[0].InnerText;
                    string DocumentCurrencyCode = xmlDocument.GetElementsByTagName("DocumentCurrencyCode")[0].InnerText;
                    string ASPCustomerAssignedAccountID = "";
                    string ASPAdditionalAccountID = "";
                    string ASPStreetName = "";
                    string ASPRegistrationName = "";
                    string ACPCustomerAssignedAccountID = "";
                    string ACPAdditionalAccountID = "";
                    string ACPDescription = "";
                    string ACPRegistrationName = "";
                    string DReferenceID = "";
                    string DResponseCode = "";
                    string DDescription = "";
                    string LMTChargeTotalAmount = "";
                    string LMTPayableAmount = "";
                    string DescuentoGlobal = "";
                    var info_general = getByTipo(InvoiceTypeCode);

                    Document doc = new Document(PageSize.LETTER);
                    // Indicamos donde vamos a guardar el documento
                    PdfWriter writer = PdfWriter.GetInstance(doc,
                                                new FileStream(newFile, FileMode.Create));

                    // Le colocamos el título y el autor
                    // Esto no será visible en el documento
                    doc.AddTitle("Documento Electronico");
                    doc.AddCreator("Contasis");

                    // Abrimos el archivo
                    doc.Open();
                    // Creamos el tipo de Font que vamos utilizar
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _TitleFontN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _TitleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _HeaderFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _HeaderFontMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContent = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    PdfPTable tblPrueba = new PdfPTable(5);
                    tblPrueba.WidthPercentage = 100;


                    //TABLA header left
                    PdfPTable tblHeaderLeft = new PdfPTable(1);
                    tblHeaderLeft.WidthPercentage = 100;

                    //string currentDirectory = Environment.CurrentDirectory;
                    //string pathImage = currentDirectory + "\\logo.png";
                    // Creamos la imagen y le ajustamos el tamaño
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(pathImage);
                    imagen.BorderWidth = 0;
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 290 / imagen.Width;
                    imagen.ScalePercent(80);

                    // Insertamos la imagen en el documento

                    PdfPCell logo = new PdfPCell(imagen);
                    logo.BorderWidth = 0;
                    logo.BorderWidthBottom = 0;
                    logo.Border = 0;

                    tblHeaderLeft.AddCell(logo);


                    //get accounting supplier party
                    XmlNodeList AccountingSupplierParty = xmlDocument.GetElementsByTagName("AccountingSupplierParty");
                    foreach (XmlNode dat in AccountingSupplierParty)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                        if (caaid.Count > 0)
                        {
                            ASPCustomerAssignedAccountID = caaid.Item(0).InnerText;
                        }
                        var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                        if (aacid.Count > 0)
                        {
                            ASPAdditionalAccountID = aacid.Item(0).InnerText;
                        }
                        var stname = xmlDocumentinner.GetElementsByTagName("StreetName");
                        if (stname.Count > 0)
                        {
                            ASPStreetName = stname.Item(0).InnerText;
                        }
                        var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                        if (regname.Count > 0)
                        {
                            ASPRegistrationName = regname.Item(0).InnerText;
                        }
                    }
                    //get accounting supplier party
                    XmlNodeList AccountingCustomerParty = xmlDocument.GetElementsByTagName("AccountingCustomerParty");
                    foreach (XmlNode dat in AccountingCustomerParty)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                        if (caaid.Count > 0)
                        {
                            ACPCustomerAssignedAccountID = caaid.Item(0).InnerText;
                        }
                        var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                        if (aacid.Count > 0)
                        {
                            ACPAdditionalAccountID = aacid.Item(0).InnerText;
                        }
                        var descr = xmlDocumentinner.GetElementsByTagName("Description");
                        if (descr.Count > 0)
                        {
                            ACPDescription = descr.Item(0).InnerText;
                        }
                        var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                        if (regname.Count > 0)
                        {
                            ACPRegistrationName = regname.Item(0).InnerText;
                        }
                    }
                    XmlNodeList DiscrepancyResponse = xmlDocument.GetElementsByTagName("DiscrepancyResponse");
                    foreach (XmlNode dat in DiscrepancyResponse)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var refid = xmlDocumentinner.GetElementsByTagName("ReferenceID");
                        if (refid.Count > 0)
                        {
                            DReferenceID = refid.Item(0).InnerText;
                        }
                        var respcode = xmlDocumentinner.GetElementsByTagName("ResponseCode");
                        if (respcode.Count > 0)
                        {
                            DResponseCode = respcode.Item(0).InnerText;
                        }
                        var descr = xmlDocumentinner.GetElementsByTagName("Description");
                        if (descr.Count > 0)
                        {
                            DDescription = descr.Item(0).InnerText;
                        }

                    }

                    XmlNodeList LegalMonetaryTotal = null;

                    if (InvoiceTypeCode == "08")
                    {
                        LegalMonetaryTotal = xmlDocument.GetElementsByTagName("RequestedMonetaryTotal");
                    }
                    else
                    {
                        LegalMonetaryTotal = xmlDocument.GetElementsByTagName("LegalMonetaryTotal");
                    }

                    foreach (XmlNode dat in LegalMonetaryTotal)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var cta = xmlDocumentinner.GetElementsByTagName("ChargeTotalAmount");
                        if (cta.Count > 0)
                        {
                            LMTChargeTotalAmount = cta.Item(0).InnerText;
                        }
                        var pam = xmlDocumentinner.GetElementsByTagName("PayableAmount");
                        if (pam.Count > 0)
                        {
                            LMTPayableAmount = pam.Item(0).InnerText;
                        }

                        //Cristhian|25/10/2017|FEICONT2-396
                        /*Se obtine el valor del Descuento Global*/
                        /*INICIO MODIFICAIóN*/
                        var totalDescuento = xmlDocumentinner.GetElementsByTagName("AllowanceTotalAmount");
                        if (totalDescuento.Count > 0)
                        {
                            DescuentoGlobal = totalDescuento.Item(0).InnerText;
                        }
                        /*FIN MODIFICACIóN*/
                    }

                    var VerificarDescuentoUnitario = xmlDocument.GetElementsByTagName("AllowanceCharge");//Para comprobar la existencia de un descuento por Item

                    //tabla info empresa
                    PdfPTable tblInforEmpresa = new PdfPTable(1);
                    tblInforEmpresa.WidthPercentage = 100;
                    PdfPCell NameEmpresa = new PdfPCell(new Phrase(ASPRegistrationName, _HeaderFont));
                    NameEmpresa.BorderWidth = 0;
                    NameEmpresa.Border = 0;
                    tblInforEmpresa.AddCell(NameEmpresa);

                    var pa = new Paragraph();
                    pa.Font = _clienteFontBoldMin;
                    pa.Add(ASPStreetName);
                    PdfPCell EstaticoEmpresa = new PdfPCell(pa);
                    EstaticoEmpresa.BorderWidth = 0;
                    EstaticoEmpresa.Border = 0;
                    tblInforEmpresa.AddCell(EstaticoEmpresa);

                    PdfPCell celdaInfoEmpresa = new PdfPCell(tblInforEmpresa);
                    celdaInfoEmpresa.Border = 0;
                    tblHeaderLeft.AddCell(celdaInfoEmpresa);
                    // PdfPCell blanco = new PdfPCell();
                    // blanco.Border = 0;



                    List<clsEntityDocument_AdditionalComments> Lista_additional_coments = new List<clsEntityDocument_AdditionalComments>();
                    clsEntityDocument_AdditionalComments adittionalComents;
                    XmlNodeList datosCabecera = xmlDocument.GetElementsByTagName("DatosCabecera");
                    foreach (XmlNode dat in datosCabecera)
                    {
                        var NodosHijos = dat.ChildNodes;
                        for (int z = 0; z < NodosHijos.Count; z++)
                        {
                            adittionalComents = new clsEntityDocument_AdditionalComments(local);
                            adittionalComents.Cs_pr_TagNombre = NodosHijos.Item(z).LocalName;
                            adittionalComents.Cs_pr_TagValor = NodosHijos.Item(z).ChildNodes.Item(0).InnerText;
                            Lista_additional_coments.Add(adittionalComents);
                        }
                    }

                    //comentarios contenido
                    var teclaf8 = " ";//comment1
                    var teclavtrlm = " ";//commnet2
                    var cuentasbancarias = " ";//comment 3
                    var CondicionPagoXML = " ";
                    var CondicionVentaXML = " ";
                    var VendedorXML = " ";

                    //var errores = "";

                    foreach (var itemm in Lista_additional_coments)
                    {
                        if (itemm.Cs_pr_TagNombre == "DatEmpresa")
                        {
                            cuentasbancarias = itemm.Cs_pr_TagValor;
                        }
                        if (itemm.Cs_pr_TagNombre == "TeclaF8")
                        {
                            teclaf8 = itemm.Cs_pr_TagValor;
                        }
                        if (itemm.Cs_pr_TagNombre == "TeclasCtrlM")
                        {
                            teclavtrlm = itemm.Cs_pr_TagValor;
                        }
                        if (itemm.Cs_pr_TagNombre == "CondPago")
                        {
                            CondicionPagoXML = itemm.Cs_pr_TagValor;
                        }
                        if (itemm.Cs_pr_TagNombre == "Vendedor")
                        {
                            VendedorXML = itemm.Cs_pr_TagValor;
                        }
                        if (itemm.Cs_pr_TagNombre == "Condicion")
                        {
                            CondicionVentaXML = itemm.Cs_pr_TagValor;
                        }
                    }



                    //tabla para info ruc
                    PdfPTable tblInforRuc = new PdfPTable(1);
                    tblInforRuc.WidthPercentage = 100;

                    PdfPCell TituRuc = new PdfPCell(new Phrase("R.U.C. " + ASPCustomerAssignedAccountID, _TitleFontN));
                    TituRuc.BorderWidthTop = 0.75f;
                    TituRuc.BorderWidthBottom = 0.75f;
                    TituRuc.BorderWidthLeft = 0.75f;
                    TituRuc.BorderWidthRight = 0.75f;
                    TituRuc.HorizontalAlignment = Element.ALIGN_CENTER;
                    TituRuc.PaddingTop = 10f;
                    TituRuc.PaddingBottom = 10f;

                    PdfPCell TipoDoc = new PdfPCell(new Phrase(info_general[2], _TitleFontN));
                    TipoDoc.BorderWidthLeft = 0.75f;
                    TipoDoc.BorderWidthRight = 0.75f;
                    TipoDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    TipoDoc.PaddingTop = 10f;
                    TipoDoc.PaddingBottom = 10f;

                    PdfPCell SerieDoc = new PdfPCell(new Phrase("N° " + cabecera.Cs_tag_ID, _TitleFont));
                    SerieDoc.BorderWidthBottom = 0.75f;
                    SerieDoc.BorderWidthRight = 0.75f;
                    SerieDoc.BorderWidthLeft = 0.75f;
                    SerieDoc.BorderWidthTop = 0.75f;
                    SerieDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    SerieDoc.PaddingTop = 10f;
                    SerieDoc.PaddingBottom = 10f;

                    PdfPCell blanco2 = new PdfPCell(new Paragraph(" "));
                    blanco2.Border = 0;
                    tblInforRuc.AddCell(TituRuc);
                    //tblInforRuc.AddCell(blanco2);
                    tblInforRuc.AddCell(TipoDoc);
                    //tblInforRuc.AddCell(blanco2);
                    tblInforRuc.AddCell(SerieDoc);
                    tblInforRuc.AddCell(blanco2);

                    PdfPCell infoRuc = new PdfPCell(tblInforRuc);
                    infoRuc.Colspan = 2;
                    infoRuc.BorderWidth = 0;

                    PdfPCell celdaHeaderLeft = new PdfPCell(tblHeaderLeft);
                    celdaHeaderLeft.Border = 0;
                    celdaHeaderLeft.Colspan = 3;

                    // Añadimos las celdas a la tabla
                    tblPrueba.AddCell(celdaHeaderLeft);
                    // tblPrueba.AddCell(blanco);
                    tblPrueba.AddCell(infoRuc);

                    doc.Add(tblPrueba);

                    PdfPTable tblBlanco = new PdfPTable(1);
                    tblBlanco.WidthPercentage = 100;
                    PdfPCell blanco3 = new PdfPCell((new Paragraph(" ")));
                    blanco3.Border = 0;

                    tblBlanco.AddCell(blanco3);

                    doc.Add(tblBlanco);

                    //Informacion cliente
                    PdfPTable tblInfoCliente = new PdfPTable(10);
                    tblInfoCliente.WidthPercentage = 100;



                    // Llenamos la tabla con información del cliente
                    PdfPCell cliente = new PdfPCell(new Phrase("Cliente:", _clienteFontBoldMin));
                    cliente.BorderWidth = 0;
                    cliente.Colspan = 1;

                    PdfPCell clNombre = new PdfPCell(new Phrase(ACPRegistrationName, _clienteFontContentMinFooter));
                    clNombre.BorderWidth = 0;
                    clNombre.Colspan = 5;

                    PdfPCell fecha = new PdfPCell(new Phrase("Fecha de Emision:", _clienteFontBoldMin));
                    fecha.BorderWidth = 0;
                    fecha.Colspan = 2;

                    var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                    PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                    clFecha.BorderWidth = 0;
                    clFecha.Colspan = 2;

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(cliente);
                    tblInfoCliente.AddCell(clNombre);
                    tblInfoCliente.AddCell(fecha);
                    tblInfoCliente.AddCell(clFecha);

                    PdfPCell direccion = new PdfPCell(new Phrase("Direccion:", _clienteFontBoldMin));
                    direccion.BorderWidth = 0;
                    direccion.Colspan = 1;

                    PdfPCell clDireccion = new PdfPCell(new Phrase(ACPDescription, _clienteFontContentMinFooter));
                    clDireccion.BorderWidth = 0;
                    clDireccion.Colspan = 5;


                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        PdfPCell condicionVenta = new PdfPCell(new Phrase("Documento que modifica:", _clienteFontBoldMin));
                        condicionVenta.BorderWidth = 0;
                        condicionVenta.Colspan = 2;


                        PdfPCell clCondicionVenta = new PdfPCell(new Phrase(DReferenceID, _clienteFontContentMinFooter));
                        clCondicionVenta.BorderWidth = 0;
                        clCondicionVenta.Colspan = 2;

                        tblInfoCliente.AddCell(direccion);
                        tblInfoCliente.AddCell(clDireccion);
                        tblInfoCliente.AddCell(condicionVenta);
                        tblInfoCliente.AddCell(clCondicionVenta);
                    }
                    else
                    {
                        NumLetra monedaLetras = new NumLetra();
                        var monedaLetra = monedaLetras.getMoneda(DocumentCurrencyCode);
                        PdfPCell moneda = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                        moneda.BorderWidth = 0;
                        moneda.Colspan = 2;

                        PdfPCell clMoneda = new PdfPCell(new Phrase(monedaLetra.ToUpper(), _clienteFontContentMinFooter));
                        clMoneda.BorderWidth = 0;
                        clMoneda.Colspan = 2;

                        /* PdfPCell condicionVenta = new PdfPCell(new Phrase("Condicion Venta:", _clienteFontBoldMin));
                         condicionVenta.BorderWidth = 0;
                         condicionVenta.Colspan = 2;


                         PdfPCell clCondicionVenta = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                         clCondicionVenta.BorderWidth = 0;
                         clCondicionVenta.Colspan = 2;
                         */
                        tblInfoCliente.AddCell(direccion);
                        tblInfoCliente.AddCell(clDireccion);
                        tblInfoCliente.AddCell(moneda);
                        tblInfoCliente.AddCell(clMoneda);

                    }


                    // Añadimos las celdas a la tabla de info cliente


                    var docName = getTipoDocIdentidad(ACPAdditionalAccountID);
                    PdfPCell ruc = new PdfPCell(new Phrase(docName + " N°:", _clienteFontBoldMin));
                    ruc.BorderWidth = 0;
                    ruc.Colspan = 1;

                    PdfPCell clRUC = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID, _clienteFontContentMinFooter));
                    clRUC.BorderWidth = 0;
                    clRUC.Colspan = 5;
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        NumLetra monedaLetras1 = new NumLetra();
                        var monedaLetra_ = monedaLetras1.getMoneda(DocumentCurrencyCode);
                        PdfPCell moneda_ = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                        moneda_.BorderWidth = 0;
                        moneda_.Colspan = 2;

                        PdfPCell clMoneda_ = new PdfPCell(new Phrase(monedaLetra_.ToUpper(), _clienteFontContentMinFooter));
                        clMoneda_.BorderWidth = 0;
                        clMoneda_.Colspan = 2;
                        tblInfoCliente.AddCell(ruc);
                        tblInfoCliente.AddCell(clRUC);
                        tblInfoCliente.AddCell(moneda_);
                        tblInfoCliente.AddCell(clMoneda_);
                    }
                    else
                    {  //NumLetra monedaLetras = new NumLetra();
                       //  var monedaLetra_ = monedaLetras.getMoneda(cabecera.Cs_tag_DocumentCurrencyCode);
                        PdfPCell moneda_ = new PdfPCell(new Phrase("Condicion de Venta", _clienteFontBoldMin));
                        moneda_.BorderWidth = 0;
                        moneda_.Colspan = 2;

                        PdfPCell clMoneda_ = new PdfPCell(new Phrase(CondicionVentaXML, _clienteFontContentMinFooter));
                        clMoneda_.BorderWidth = 0;
                        clMoneda_.Colspan = 2;
                        tblInfoCliente.AddCell(ruc);
                        tblInfoCliente.AddCell(clRUC);
                        tblInfoCliente.AddCell(moneda_);
                        tblInfoCliente.AddCell(clMoneda_);

                    }

                    // Añadimos las celdas a la tabla inf

                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {

                        PdfPCell motivomodifica = new PdfPCell(new Phrase("Motivo", _clienteFontBoldMin));
                        motivomodifica.BorderWidth = 0;
                        motivomodifica.Colspan = 1;

                        PdfPCell clmotivomodifica = new PdfPCell(new Phrase(DDescription, _clienteFontContentMinFooter));
                        clmotivomodifica.BorderWidth = 0;
                        clmotivomodifica.Colspan = 5;

                        clsEntityDocument doc_modificado = new clsEntityDocument(localDB);
                        string fechaModificado = doc_modificado.cs_pxBuscarFechaDocumento(DReferenceID);
                        PdfPCell docmodifica = new PdfPCell(new Phrase("Fecha Doc. Modificado:", _clienteFontBoldMin));
                        docmodifica.BorderWidth = 0;
                        docmodifica.Colspan = 2;

                        PdfPCell cldocmodifica = new PdfPCell(new Phrase(fechaModificado, _clienteFontContentMinFooter));
                        cldocmodifica.BorderWidth = 0;
                        cldocmodifica.Colspan = 2;

                        tblInfoCliente.AddCell(motivomodifica);
                        tblInfoCliente.AddCell(clmotivomodifica);
                        tblInfoCliente.AddCell(docmodifica);
                        tblInfoCliente.AddCell(cldocmodifica);

                    }
                    else
                    {
                        PdfPCell motivomodifica = new PdfPCell(new Phrase(" ", _clienteFontBoldMin));
                        motivomodifica.BorderWidth = 0;
                        motivomodifica.Colspan = 1;

                        PdfPCell clmotivomodifica = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                        clmotivomodifica.BorderWidth = 0;
                        clmotivomodifica.Colspan = 5;


                        PdfPCell docmodifica = new PdfPCell(new Phrase("Vendedor:", _clienteFontBoldMin));
                        docmodifica.BorderWidth = 0;
                        docmodifica.Colspan = 2;

                        PdfPCell cldocmodifica = new PdfPCell(new Phrase(VendedorXML, _clienteFontContentMinFooter));
                        cldocmodifica.BorderWidth = 0;
                        cldocmodifica.Colspan = 2;

                        tblInfoCliente.AddCell(motivomodifica);
                        tblInfoCliente.AddCell(clmotivomodifica);
                        tblInfoCliente.AddCell(docmodifica);
                        tblInfoCliente.AddCell(cldocmodifica);

                    }

                    /*------------------------------------*/
                    doc.Add(tblInfoCliente);
                    doc.Add(tblBlanco);

                    PdfPTable tblInfoComprobante = new PdfPTable(12);
                    tblInfoComprobante.WidthPercentage = 100;


                    // Llenamos la tabla con información
                    PdfPCell colCodigo = new PdfPCell(new Phrase("Item", _clienteFontBoldMin));
                    colCodigo.BorderWidth = 0.75f;
                    colCodigo.Colspan = 1;
                    colCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colCantidad = new PdfPCell(new Phrase("Cantidad", _clienteFontBoldMin));
                    colCantidad.BorderWidth = 0.75f;
                    colCantidad.Colspan = 1;
                    colCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colUnidadMedida = new PdfPCell(new Phrase("Codigo", _clienteFontBoldMin));
                    colUnidadMedida.BorderWidth = 0.75f;
                    colUnidadMedida.Colspan = 2;
                    colUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colDescripcion = new PdfPCell(new Phrase("Descripcion", _clienteFontBoldMin));
                    colDescripcion.BorderWidth = 0.75f;
                    colDescripcion.Colspan = 6;
                    colDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPrecUnit = new PdfPCell(new Phrase("Valor Unitario (Sin IGV)", _clienteFontBoldMin));
                    colPrecUnit.BorderWidth = 0.75f;
                    colPrecUnit.Colspan = 1;
                    colPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Cristhian|25/10/2017|FEICONT2-396
                    /*Se agrega una Nueva Columna*/
                    /*NUEVO INICIO*/
                    PdfPCell colDescuentoUnitario = new PdfPCell(new Phrase("Dscto. Unit.", _clienteFontBoldMin));
                    /*Si se tiene almenos un descuento unitario se cumple las condiciones del IF*/
                    if (VerificarDescuentoUnitario.Count > 0)
                    {
                        /*Se cambia el ancho de la columna Descripción, para que la columna de descuentos quepa en el documento*/
                        colDescripcion.Colspan = 5;

                        /*Configuracion estandar de la celda*/
                        colDescuentoUnitario.BorderWidth = 0.75f;
                        colDescuentoUnitario.Colspan = 1;
                        colDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    /*NUEVO FIN*/

                    PdfPCell colImporte = new PdfPCell(new Phrase("Valor Total (Sin IGV)", _clienteFontBoldMin));
                    colImporte.BorderWidth = 0.75f;
                    colImporte.Colspan = 1;
                    colImporte.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Añadimos las celdas a la tabla - Se incluye el nuevo campo de descuento
                    tblInfoComprobante.AddCell(colCodigo);
                    tblInfoComprobante.AddCell(colCantidad);
                    tblInfoComprobante.AddCell(colUnidadMedida);
                    tblInfoComprobante.AddCell(colDescripcion);
                    tblInfoComprobante.AddCell(colPrecUnit);
                    if (VerificarDescuentoUnitario.Count > 0)
                    {
                        tblInfoComprobante.AddCell(colDescuentoUnitario);//Descuento
                    }
                    tblInfoComprobante.AddCell(colImporte);

                    //impuestos globales

                    List<clsEntityDocument_TaxTotal> Lista_tax_total = new List<clsEntityDocument_TaxTotal>();
                    clsEntityDocument_TaxTotal taxTotal;
                    XmlNodeList nodestaxTotal = xmlDocument.GetElementsByTagName("TaxTotal");
                    foreach (XmlNode dat in nodestaxTotal)
                    {
                        string nodoPadre = dat.ParentNode.LocalName;
                        if (nodoPadre == "Invoice" || nodoPadre == "DebitNote" || nodoPadre == "CreditNote")
                        {
                            taxTotal = new clsEntityDocument_TaxTotal(local);
                            XmlDocument xmlDocumentTaxtotal = new XmlDocument();
                            xmlDocumentTaxtotal.LoadXml(dat.OuterXml);
                            XmlNodeList taxAmount = xmlDocumentTaxtotal.GetElementsByTagName("TaxAmount");
                            if (taxAmount.Count > 0)
                            {
                                taxTotal.Cs_tag_TaxAmount = taxAmount.Item(0).InnerText;
                            }
                            XmlNodeList subtotal = xmlDocumentTaxtotal.GetElementsByTagName("TaxSubtotal");
                            if (subtotal.Count > 0)
                            {
                                XmlDocument xmlDocumentTaxSubtotal = new XmlDocument();
                                xmlDocumentTaxSubtotal.LoadXml(subtotal.Item(0).OuterXml);

                                var subTotalAmount = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxAmount");
                                if (subTotalAmount.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxSubtotal_TaxAmount = subTotalAmount.Item(0).InnerText;
                                }
                                var subTotalID = xmlDocumentTaxSubtotal.GetElementsByTagName("ID");
                                if (subTotalID.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                }
                                var subTotalName = xmlDocumentTaxSubtotal.GetElementsByTagName("Name");
                                if (subTotalName.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                }
                                var subTotalTaxTypeCode = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                if (subTotalTaxTypeCode.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                }

                            }
                            Lista_tax_total.Add(taxTotal);

                        }
                    }



                    string imp_IGV = "";
                    string imp_ISC = "";
                    string imp_OTRO = "";

                    foreach (var ress in Lista_tax_total)
                    {

                        if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "1000")
                        {//IGV
                            imp_IGV = Convert.ToString(ress.Cs_tag_TaxAmount);

                        }
                        else if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "2000")
                        {//isc
                            imp_ISC = Convert.ToString(ress.Cs_tag_TaxAmount);

                        }
                        else if (ress.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID == "9999")
                        {
                            imp_OTRO = Convert.ToString(ress.Cs_tag_TaxAmount);

                        }

                    }

                    //Additional Monetary Total
                    List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal> Lista_additional_monetary = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal>();
                    List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty> Lista_additional_property = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty>();

                    XmlNodeList additionalInformation = xmlDocument.GetElementsByTagName("AdditionalInformation");
                    foreach (XmlNode dat in additionalInformation)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);
                        clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal adittionalMonetary;

                        XmlNodeList LIST1 = xmlDocumentinner.GetElementsByTagName("AdditionalMonetaryTotal");
                        for (int ii = 0; ii < LIST1.Count; ii++)
                        {
                            adittionalMonetary = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);

                            var ss = LIST1.Item(ii);
                            XmlDocument xmlDocumentinner1 = new XmlDocument();
                            xmlDocumentinner1.LoadXml(ss.OuterXml);

                            var id = xmlDocumentinner1.GetElementsByTagName("ID");
                            if (id.Count > 0)
                            {
                                adittionalMonetary.Cs_tag_Id = id.Item(0).InnerText;
                            }
                            var percent = xmlDocumentinner1.GetElementsByTagName("Percent");
                            if (percent.Count > 0)
                            {
                                adittionalMonetary.Cs_tag_Percent = percent.Item(0).InnerText;
                            }
                            var payableAmount = xmlDocumentinner1.GetElementsByTagName("PayableAmount");
                            if (payableAmount.Count > 0)
                            {
                                adittionalMonetary.Cs_tag_PayableAmount = payableAmount.Item(0).InnerText;
                                /*** if (payableAmount.Item(0).Attributes.Count > 0)
                                 {
                                     adittionalMonetary. = payableAmount.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                 }****/
                            }
                            Lista_additional_monetary.Add(adittionalMonetary);

                        }
                        clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adittionalProperty;
                        XmlNodeList LIST2 = xmlDocumentinner.GetElementsByTagName("AdditionalProperty");
                        for (int iii = 0; iii < LIST2.Count; iii++)
                        {
                            adittionalProperty = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);

                            var ss = LIST2.Item(iii);
                            XmlDocument xmlDocumentinner1 = new XmlDocument();
                            xmlDocumentinner1.LoadXml(ss.OuterXml);

                            var id = xmlDocumentinner1.GetElementsByTagName("ID");
                            if (id.Count > 0)
                            {
                                adittionalProperty.Cs_tag_ID = id.Item(0).InnerText;
                            }

                            var value = xmlDocumentinner1.GetElementsByTagName("Value");
                            if (value.Count > 0)
                            {
                                adittionalProperty.Cs_tag_Value = value.Item(0).InnerText;
                            }
                            var name = xmlDocumentinner1.GetElementsByTagName("Name");
                            if (name.Count > 0)
                            {
                                adittionalProperty.Cs_tag_Name = name.Item(0).InnerText;
                            }
                            Lista_additional_property.Add(adittionalProperty);
                        }
                    }
                    //Additional

                    var cuenta_nacion = "";
                    try
                    {
                        foreach (var it in Lista_additional_property)
                        {
                            if (it.Cs_tag_ID == "3001")
                            {
                                cuenta_nacion = it.Cs_tag_Value;
                            }
                        }

                    }
                    catch (Exception)
                    {
                        cuenta_nacion = "";
                    }

                    string op_gravada = "0.00";
                    string op_inafecta = "0.00";
                    string op_exonerada = "0.00";
                    string op_gratuita = "0.00";
                    string op_detraccion = "0.00";
                    string porcentaje_detraccion = "";
                    string total_descuentos = "0.00";

                    foreach (var ress in Lista_additional_monetary)
                    {
                        if (ress.Cs_tag_Id == "1001")
                        {
                            op_gravada = Convert.ToString(ress.Cs_tag_PayableAmount);

                        }
                        else if (ress.Cs_tag_Id == "1002")
                        {
                            op_inafecta = Convert.ToString(ress.Cs_tag_PayableAmount);

                        }
                        else if (ress.Cs_tag_Id == "1003")
                        {
                            op_exonerada = Convert.ToString(ress.Cs_tag_PayableAmount);

                        }
                        else if (ress.Cs_tag_Id == "2005")
                        {
                            total_descuentos = Convert.ToString(ress.Cs_tag_PayableAmount);

                        }
                        else if (ress.Cs_tag_Id == "1004")
                        {
                            op_gratuita = Convert.ToString(ress.Cs_tag_PayableAmount);

                        }
                        else if (ress.Cs_tag_Id == "2003")
                        {
                            op_detraccion = Convert.ToString(ress.Cs_tag_PayableAmount);
                            porcentaje_detraccion = Convert.ToString(ress.Cs_tag_Percent);
                        }

                    }
                    /* seccion de items ------ añadir items*/
                    var numero_item = 0;
                    double sub_total = 0.00;

                    List<clsEntityDocument_Line> Lista_items;
                    List<clsEntityDocument_Line_TaxTotal> Lista_items_taxtotal;
                    clsEntityDocument_Line item;
                    XmlNodeList nodeitem;
                    if (InvoiceTypeCode == "07")
                    {
                        nodeitem = xmlDocument.GetElementsByTagName("CreditNoteLine");

                    }
                    else if (InvoiceTypeCode == "08")
                    {

                        nodeitem = xmlDocument.GetElementsByTagName("DebitNoteLine");

                    }
                    else
                    {
                        nodeitem = xmlDocument.GetElementsByTagName("InvoiceLine");
                    }
                    // XmlNodeList nodeitem = xmlDocument.GetElementsByTagName("InvoiceLine");
                    // Dictionary<string, List<clasEntityDocument_Line_Description>> dictionary = new Dictionary<string, List<clasEntityDocument_Line_Description>>();
                    List<clsEntityDocument_Line_Description> Lista_items_description;
                    List<clsEntityDocument_Line_PricingReference> Lista_items_princingreference;
                    clsEntityDocument_Line_Description descripcionItem;

                    var total_items = nodeitem.Count;

                    int i = 0;
                    foreach (XmlNode dat in nodeitem)
                    {
                        i++;
                        numero_item++; //El numero del Item Se actualliza automaticamente - ya que es progresivo
                        var valor_unitario_item = "";
                        var valor_total_item = "";
                        string condition_price = "";
                        Lista_items = new List<clsEntityDocument_Line>();
                        Lista_items_description = new List<clsEntityDocument_Line_Description>();
                        Lista_items_princingreference = new List<clsEntityDocument_Line_PricingReference>();
                        Lista_items_taxtotal = new List<clsEntityDocument_Line_TaxTotal>();
                        item = new clsEntityDocument_Line(local);
                        XmlDocument xmlItem = new XmlDocument();
                        xmlItem.LoadXml(dat.OuterXml);

                        XmlNodeList ItemDetail = xmlItem.GetElementsByTagName("Item");
                        if (ItemDetail.Count > 0)
                        {
                            foreach (XmlNode items in ItemDetail)
                            {
                                XmlDocument xmlItemItem = new XmlDocument();
                                xmlItemItem.LoadXml(items.OuterXml);

                                /*Inicio Obtensión de la Unidad de Medida del producto */
                                XmlNodeList taxItemIdentification = xmlItemItem.GetElementsByTagName("ID");
                                if (taxItemIdentification.Count > 0)
                                {
                                    item.Cs_tag_Item_SellersItemIdentification = taxItemIdentification.Item(0).InnerText;
                                }
                                /*Fin Obtensión de la Unidad de Medida del producto */

                                XmlNodeList taxItemDescription = xmlItemItem.GetElementsByTagName("Description");
                                int j = 0;
                                foreach (XmlNode description in taxItemDescription)
                                {
                                    j++;
                                    descripcionItem = new clsEntityDocument_Line_Description(local);
                                    descripcionItem.Cs_pr_Document_Line_Id = j.ToString();
                                    /* if (description.HasChildNodes)
                                     {
                                         descripcionItem.Cs_tag_Description = description.FirstChild.InnerText.Trim();
                                     }
                                     else
                                     {*/
                                    descripcionItem.Cs_tag_Description = description.InnerText.Trim();
                                    //   }

                                    Lista_items_description.Add(descripcionItem);

                                }
                                j = 0;
                            }
                            //dictionary[i.ToString()] = Lista_items_description;
                        }


                        XmlNodeList ID = xmlItem.GetElementsByTagName("ID");
                        if (ID.Count > 0)
                        {
                            item.Cs_tag_InvoiceLine_ID = ID.Item(0).InnerText;
                        }

                        /*Inicio Obtensión la cantidad del producto */
                        XmlNodeList InvoicedQuantity;
                        if (InvoiceTypeCode == "07")
                        {
                            InvoicedQuantity = xmlItem.GetElementsByTagName("CreditedQuantity");

                            if (InvoicedQuantity.Count > 0)
                            {
                                item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                {
                                    item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                }
                            }
                        }
                        else if (InvoiceTypeCode == "08")
                        {
                            InvoicedQuantity = xmlItem.GetElementsByTagName("DebitedQuantity");
                            if (InvoicedQuantity.Count > 0)
                            {
                                item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                {
                                    item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                }
                            }
                        }
                        else
                        {
                            InvoicedQuantity = xmlItem.GetElementsByTagName("InvoicedQuantity");
                            if (InvoicedQuantity.Count > 0)
                            {
                                item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                {
                                    item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                }
                            }

                        }
                        /*Fin Obtension la cantidad del producto*/

                        XmlNodeList LineExtensionAmount = xmlItem.GetElementsByTagName("LineExtensionAmount");
                        if (LineExtensionAmount.Count > 0)
                        {
                            item.Cs_tag_LineExtensionAmount_currencyID = LineExtensionAmount.Item(0).InnerText;
                        }
                        clsEntityDocument_Line_PricingReference lines_pricing_reference;
                        XmlNodeList PricingReference = xmlItem.GetElementsByTagName("PricingReference");
                        if (PricingReference.Count > 0)
                        {
                            XmlDocument xmlItemItem = new XmlDocument();
                            xmlItemItem.LoadXml(PricingReference.Item(0).OuterXml);
                            XmlNodeList AlternativeConditionPrice = xmlItemItem.GetElementsByTagName("AlternativeConditionPrice");
                            foreach (XmlNode itm in AlternativeConditionPrice)
                            {
                                XmlDocument xmlItemPricingReference = new XmlDocument();
                                xmlItemPricingReference.LoadXml(itm.OuterXml);
                                lines_pricing_reference = new clsEntityDocument_Line_PricingReference(local);
                                XmlNodeList PriceAmount = xmlItemPricingReference.GetElementsByTagName("PriceAmount");
                                if (PriceAmount.Count > 0)
                                {
                                    lines_pricing_reference.Cs_tag_PriceAmount_currencyID = PriceAmount.Item(0).InnerText;
                                }
                                XmlNodeList PriceTypeCode = xmlItemPricingReference.GetElementsByTagName("PriceTypeCode");
                                if (PriceTypeCode.Count > 0)
                                {
                                    lines_pricing_reference.Cs_tag_PriceTypeCode = PriceTypeCode.Item(0).InnerText;
                                }
                                Lista_items_princingreference.Add(lines_pricing_reference);
                            }


                        }
                        clsEntityDocument_Line_TaxTotal taxTotalItem;
                        XmlNodeList TaxTotal = xmlItem.GetElementsByTagName("TaxTotal");
                        if (TaxTotal.Count > 0)
                        {
                            foreach (XmlNode taxitem in TaxTotal)
                            {
                                taxTotalItem = new clsEntityDocument_Line_TaxTotal(local);
                                XmlDocument xmlItemTaxtotal = new XmlDocument();
                                xmlItemTaxtotal.LoadXml(taxitem.OuterXml);
                                XmlNodeList taxItemAmount = xmlItemTaxtotal.GetElementsByTagName("TaxAmount");
                                if (taxItemAmount.Count > 0)
                                {
                                    taxTotalItem.Cs_tag_TaxAmount_currencyID = taxItemAmount.Item(0).InnerText;
                                }
                                XmlNodeList itemsubtotal = xmlItemTaxtotal.GetElementsByTagName("TaxSubtotal");
                                if (itemsubtotal.Count > 0)
                                {
                                    XmlDocument xmlItemTaxSubtotal = new XmlDocument();
                                    xmlItemTaxSubtotal.LoadXml(itemsubtotal.Item(0).OuterXml);

                                    var subTotalAmount = xmlItemTaxSubtotal.GetElementsByTagName("TaxAmount");
                                    if (subTotalAmount.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxSubtotal_TaxAmount_currencyID = subTotalAmount.Item(0).InnerText;
                                    }
                                    var subTotalID = xmlItemTaxSubtotal.GetElementsByTagName("ID");
                                    if (subTotalID.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                    }
                                    var subTotalName = xmlItemTaxSubtotal.GetElementsByTagName("Name");
                                    if (subTotalName.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                    }
                                    var subTotalTaxTypeCode = xmlItemTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                    if (subTotalTaxTypeCode.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                    }

                                }
                                Lista_items_taxtotal.Add(taxTotalItem);
                            }
                        }

                        XmlNodeList Price = xmlItem.GetElementsByTagName("Price");
                        if (Price.Count > 0)
                        {
                            XmlDocument xmlItemPrice = new XmlDocument();
                            xmlItemPrice.LoadXml(Price.Item(0).OuterXml);
                            XmlNodeList PriceAmount = xmlItemPrice.GetElementsByTagName("PriceAmount");
                            if (PriceAmount.Count > 0)
                            {
                                item.Cs_tag_Price_PriceAmount = PriceAmount.Item(0).InnerText;
                            }
                        }

                        //Cristhian|25/10/2017|FEICONT2-396
                        /*Se añade el la etiqueta de donde se obtendra el valor del descuento unitario*/
                        /*NUEVO INICIO*/
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            /*De aqui se obtine el valor del descuento unitario, por producto*/
                            XmlNodeList DescuentoUnitario = null;
                            DescuentoUnitario = xmlItem.GetElementsByTagName("AllowanceCharge");

                            /*Si se tiene el descuento unitario, el valor de este es asignado a una variable*/
                            if (DescuentoUnitario.Count > 0)
                            {
                                /*Se obtiene los detalles del producto*/
                                XmlDocument xmlItemPrice = new XmlDocument();
                                xmlItemPrice.LoadXml(DescuentoUnitario.Item(0).OuterXml);
                                /*Se obtiene el valor del Descuento*/
                                XmlNodeList DescountAmount = xmlItemPrice.GetElementsByTagName("Amount");
                                if (DescountAmount.Count > 0)
                                {
                                    /*se asigna el valor del descuento ala variable*/
                                    item.Cs_tag_AllowanceCharge_Amount = DescountAmount.Item(0).InnerText;//Se tubo que crear un nuevo Campo o Item para el Descuento-En Document LINE
                                }
                            }
                            /*Caso contario solo se asigan el valor de 0*/
                            else
                            {
                                item.Cs_tag_AllowanceCharge_Amount = "0.00";
                            }
                        }
                        /*NUEVO FIN*/

                        if (op_gratuita != "0.00")
                        {
                            foreach (var itm in Lista_items_princingreference)
                            {
                                if (itm.Cs_tag_PriceTypeCode == "02")
                                {
                                    condition_price = itm.Cs_tag_PriceAmount_currencyID;
                                }
                            }
                        }

                        var text_detalle = "";
                        foreach (var det_it in Lista_items_description)
                        {
                            text_detalle += det_it.Cs_tag_Description + " \n";
                        }

                        PdfPCell itCodigo = new PdfPCell(new Phrase(numero_item.ToString(), _clienteFontContentMinFooter));
                        itCodigo.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itCodigo.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itCodigo.BorderWidthBottom = 0.75f;
                        }
                        itCodigo.BorderWidthLeft = 0.75f;
                        itCodigo.BorderWidthRight = 0.75f;
                        itCodigo.BorderWidthTop = 0;
                        itCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itCantidad = new PdfPCell(new Phrase(item.Cs_tag_invoicedQuantity, _clienteFontContentMinFooter));
                        itCantidad.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itCantidad.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itCantidad.BorderWidthBottom = 0.75f;
                        }

                        itCantidad.BorderWidthLeft = 0;
                        itCantidad.BorderWidthRight = 0.75f;
                        itCantidad.BorderWidthTop = 0;
                        itCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                        /*La columna Unidad de Medida parece que es condicional-dependiendo del documento se muestra*/
                        PdfPCell itUnidadMedida = new PdfPCell(new Phrase(item.Cs_tag_Item_SellersItemIdentification, _clienteFontContentMinFooter));
                        itUnidadMedida.Colspan = 2;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itUnidadMedida.BorderWidthBottom = 0.75f;
                        }
                        else
                        {
                            itUnidadMedida.BorderWidthBottom = 0.75f;
                        }

                        itUnidadMedida.BorderWidthLeft = 0;
                        itUnidadMedida.BorderWidthRight = 0.75f;
                        itUnidadMedida.BorderWidthTop = 0;
                        itUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;
                        /*Fin Columna Unidad de Medida*/

                        /*Columna de Descripción - tambien se le da la dimension de la celda*/
                        PdfPCell itDescripcion = new PdfPCell(new Phrase(text_detalle, _clienteFontContentMinFooter));
                        itDescripcion.Colspan = 6;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itDescripcion.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itDescripcion.BorderWidthBottom = 0.75f;
                        }

                        itDescripcion.BorderWidthLeft = 0;
                        itDescripcion.BorderWidthRight = 0.75f;
                        itDescripcion.BorderWidthTop = 0;
                        itDescripcion.PaddingBottom = 5f;
                        itDescripcion.HorizontalAlignment = Element.ALIGN_LEFT;
                        /*Fin Columna Unidad de Medida*/

                        /*Columna de Precio Unitario - tambien se le da la dimension de la celda*/
                        if (op_gratuita != "0.00")
                        {
                            if (condition_price == "")
                            {
                                valor_unitario_item = item.Cs_tag_Price_PriceAmount;
                            }
                            else
                            {
                                valor_unitario_item = condition_price;
                            }
                        }
                        else
                        {
                            valor_unitario_item = item.Cs_tag_Price_PriceAmount;
                        }

                        PdfPCell itPrecUnit = new PdfPCell(new Phrase(valor_unitario_item, _clienteFontContentMinFooter));
                        itPrecUnit.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itPrecUnit.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itPrecUnit.BorderWidthBottom = 0.75f;
                        }

                        itPrecUnit.BorderWidthLeft = 0;
                        itPrecUnit.BorderWidthRight = 0.75f;
                        itPrecUnit.BorderWidthTop = 0;
                        itPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;
                        /*Fin Columna de Precio Unitario*/

                        //Cristhian|25/10/2017|FEICONT2-396
                        /*Columna de Descuentos - tambien se le da la dimension de la celda*/
                        /*Se cambia la dimension de la columan descripcion para que entre la columna de descuento unitario*/
                        /*NUEVO INICIO*/
                        PdfPCell itemDescuentoUnitario = new PdfPCell(new Phrase(item.Cs_tag_AllowanceCharge_Amount, _clienteFontContentMinFooter));
                        /*Si se tiene almenos un descuento unitario se procede con la definicion del IF*/
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            /*Se desminuye el espacio asignado a la columna Descripción*/
                            itDescripcion.Colspan = 5;

                            /*se le da el espacio de 1 a la columna Descuento Unitario*/
                            itemDescuentoUnitario.Colspan = 1;
                            /*Se le agrega los bordes superiores*/
                            if (numero_item == total_items & op_detraccion == "0.00")
                            {
                                itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                            }
                            else
                            {
                                itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                            }

                            /*Configuracion general*/
                            itemDescuentoUnitario.BorderWidthLeft = 0;
                            itemDescuentoUnitario.BorderWidthRight = 0.75f;
                            itemDescuentoUnitario.BorderWidthTop = 0;
                            itemDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        /*Fin Columna de Descuentos*/
                        /*NUEVO FIN*/

                        /*Columna de Precio Total - tambien se le da la dimension de la celda*/
                        if (op_gratuita != "0.00")
                        {
                            if (valor_unitario_item == "")
                            {
                                valor_unitario_item = "0.00";
                            }
                            if (condition_price != "")
                            {
                                double valor_total_item_1 = double.Parse(valor_unitario_item, CultureInfo.InvariantCulture) * double.Parse(item.Cs_tag_invoicedQuantity, CultureInfo.InvariantCulture);
                                valor_total_item = valor_total_item_1.ToString("0.00", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                valor_total_item = item.Cs_tag_LineExtensionAmount_currencyID;
                            }
                        }
                        else
                        {
                            valor_total_item = item.Cs_tag_LineExtensionAmount_currencyID;
                        }
                        PdfPCell itImporte = new PdfPCell(new Phrase(valor_total_item, _clienteFontContentMinFooter));
                        itImporte.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itImporte.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itImporte.BorderWidthBottom = 0.75f;
                        }

                        itImporte.BorderWidthLeft = 0;
                        itImporte.BorderWidthRight = 0.75f;
                        itImporte.BorderWidthTop = 0;
                        itImporte.HorizontalAlignment = Element.ALIGN_CENTER;
                        /*Fin Columna de Precio Total*/

                        //sub_total += Double.Parse(item.Cs_tag_LineExtensionAmount_currencyID);
                        // sub_total += double.Parse(item.Cs_tag_LineExtensionAmount_currencyID, CultureInfo.InvariantCulture);
                        // Añadimos las celdas a la tabla
                        tblInfoComprobante.AddCell(itCodigo);
                        tblInfoComprobante.AddCell(itCantidad);
                        tblInfoComprobante.AddCell(itUnidadMedida);
                        tblInfoComprobante.AddCell(itDescripcion);
                        tblInfoComprobante.AddCell(itPrecUnit);
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            tblInfoComprobante.AddCell(itemDescuentoUnitario);//Campo Agregado para el Descuento 25/10/2017 FEICONT2-396
                        }
                        tblInfoComprobante.AddCell(itImporte);
                    }


                    if (op_detraccion != "0.00")
                    {
                        //agregar mensaje

                        PdfPCell celda_blanco = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        celda_blanco.Colspan = 1;
                        celda_blanco.BorderWidthBottom = 0.75f;
                        celda_blanco.BorderWidthLeft = 0;
                        celda_blanco.BorderWidthRight = 0.75f;
                        celda_blanco.BorderWidthTop = 0;

                        PdfPCell celda_blanco_dos = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        celda_blanco_dos.Colspan = 2;
                        celda_blanco_dos.BorderWidthBottom = 0.75f;
                        celda_blanco_dos.BorderWidthLeft = 0;
                        celda_blanco_dos.BorderWidthRight = 0.75f;
                        celda_blanco_dos.BorderWidthTop = 0;

                        PdfPCell celda_blanco_right = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        celda_blanco_right.Colspan = 1;
                        celda_blanco_right.BorderWidthBottom = 0.75f;
                        celda_blanco_right.BorderWidthLeft = 0;
                        celda_blanco_right.BorderWidthRight = 0.75f;
                        celda_blanco_right.BorderWidthTop = 0;

                        PdfPCell celda_blanco_left = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        celda_blanco_left.Colspan = 1;
                        celda_blanco_left.BorderWidthBottom = 0.75f;
                        celda_blanco_left.BorderWidthLeft = 0.75f;
                        celda_blanco_left.BorderWidthRight = 0.75f;
                        celda_blanco_left.BorderWidthTop = 0;

                        var parrafo = new Paragraph();
                        parrafo.Font = _clienteFontContentMinFooter;
                        parrafo.Add("Operación sujeta al Sistema de Pago de Obligaciones Tributarias con el Gobierno Central \n");
                        parrafo.Add("SPOT " + porcentaje_detraccion + "% " + cuenta_nacion + " \n");

                        PdfPCell celda_parrafo = new PdfPCell(parrafo);
                        celda_parrafo.Colspan = 6;
                        celda_parrafo.BorderWidthBottom = 0.75f;
                        celda_parrafo.BorderWidthLeft = 0;
                        celda_parrafo.BorderWidthRight = 0.75f;
                        celda_parrafo.BorderWidthTop = 0;
                        celda_parrafo.PaddingTop = 10f;
                        celda_parrafo.HorizontalAlignment = Element.ALIGN_CENTER;

                        tblInfoComprobante.AddCell(celda_blanco_left);
                        tblInfoComprobante.AddCell(celda_blanco);
                        tblInfoComprobante.AddCell(celda_blanco_dos);
                        tblInfoComprobante.AddCell(celda_parrafo);
                        tblInfoComprobante.AddCell(celda_blanco);
                        tblInfoComprobante.AddCell(celda_blanco_right);

                    }
                    /* ------end items------*/
                    doc.Add(tblInfoComprobante);
                    doc.Add(tblBlanco);

                    //Cristhian|25/10/2017|FEICONT2-396
                    /*Tabla para mostrar el descuento global que debe aparecer en todos los comprobantes electronicos*/
                    /*NUEVO INICIO*/
                    if (DescuentoGlobal != "")
                    {
                        /*Se crea la tabla Descuento Global*/
                        PdfPTable tblInfoDescuentoGlobal = new PdfPTable(10);
                        tblInfoDescuentoGlobal.WidthPercentage = 100;

                        /*Se crea la celda de Descuento Global, esta en blanco, eso para acomodar las celdas a la derecha*/
                        PdfPCell infoDescuentoGlobal = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                        infoDescuentoGlobal.BorderWidthTop = 0.75f;
                        infoDescuentoGlobal.BorderWidthBottom = 0.75f;
                        infoDescuentoGlobal.BorderWidthLeft = 0.75f;
                        infoDescuentoGlobal.BorderWidthRight = 0;
                        infoDescuentoGlobal.Colspan = 5;
                        infoDescuentoGlobal.HorizontalAlignment = Element.ALIGN_LEFT;

                        /*Se crea la celda donde esta r el combre de la Celda*/
                        PdfPCell infoTotalDescuentoGlobal = new PdfPCell(new Phrase(" Descuento Global ", _clienteFontBoldMin));
                        infoTotalDescuentoGlobal.BorderWidthTop = 0.75f;
                        infoTotalDescuentoGlobal.BorderWidthBottom = 0.75f;
                        infoTotalDescuentoGlobal.BorderWidthLeft = 0;
                        infoTotalDescuentoGlobal.BorderWidthRight = 0;
                        infoTotalDescuentoGlobal.Colspan = 3;
                        infoTotalDescuentoGlobal.HorizontalAlignment = Element.ALIGN_RIGHT;

                        /*Se crea la celda donde estara el valor del Descuento global*/
                        var DescuentoDato = GetCurrencySymbol(DocumentCurrencyCode);
                        PdfPCell infoTotalDescuentoGlobalVal = new PdfPCell(new Phrase(DescuentoDato.CurrencySymbol + " " + DescuentoGlobal, _clienteFontContent));
                        infoTotalDescuentoGlobalVal.BorderWidthTop = 0.75f;
                        infoTotalDescuentoGlobalVal.BorderWidthBottom = 0.75f;
                        infoTotalDescuentoGlobalVal.BorderWidthRight = 0.75f;
                        infoTotalDescuentoGlobalVal.BorderWidthLeft = 0;
                        infoTotalDescuentoGlobalVal.Colspan = 2;
                        infoTotalDescuentoGlobalVal.HorizontalAlignment = Element.ALIGN_RIGHT;

                        /*se añaden las 3 tablas a la tabla Descuento Global*/
                        tblInfoDescuentoGlobal.AddCell(infoDescuentoGlobal);
                        tblInfoDescuentoGlobal.AddCell(infoTotalDescuentoGlobal);
                        tblInfoDescuentoGlobal.AddCell(infoTotalDescuentoGlobalVal);
                        doc.Add(tblInfoDescuentoGlobal);

                        doc.Add(tblBlanco);
                    }
                    /*NUEVO FIN*/

                    /*----------- CASO BOLETA SOLO MONTO TOTAL --------------*/
                    if (InvoiceTypeCode == "03")
                    {
                        /*  PdfPTable tblMontoTotal = new PdfPTable(10);
                          tblMontoTotal.WidthPercentage = 100;

                          PdfPCell monto_blanco = new PdfPCell(new Phrase(" ", _clienteFontContent));
                          monto_blanco.Border = 0;
                          monto_blanco.Colspan = 6;
                          tblMontoTotal.AddCell(monto_blanco);

                          PdfPTable tbl_monto_total = new PdfPTable(2);
                          tbl_monto_total.WidthPercentage = 100;
                          var monedaDatos1 = GetCurrencySymbol(cabecera.Cs_tag_DocumentCurrencyCode);
                          PdfPCell labelMontoTotal = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                          labelMontoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                          PdfPCell valueMontoTotal = new PdfPCell(new Phrase(monedaDatos1.CurrencySymbol + " " + cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID, _clienteFontContent));
                          valueMontoTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                          tbl_monto_total.AddCell(labelMontoTotal);
                          tbl_monto_total.AddCell(valueMontoTotal);

                          PdfPCell monto_total = new PdfPCell(tbl_monto_total);
                          monto_total.Border = 0;
                          monto_total.Colspan = 4;
                          tblMontoTotal.AddCell(monto_total);

                          doc.Add(tblMontoTotal);*/
                    }
                    /*-------------End Monto Total----------------*/

                    //FOOTER
                    PdfPTable tblInfoFooter = new PdfPTable(10);
                    tblInfoFooter.WidthPercentage = 100;

                    //comentarios
                    PdfPTable tblInfoComentarios = new PdfPTable(1);
                    tblInfoComentarios.WidthPercentage = 100;

                    //tblInfoComentarios.TotalWidth = 144f;
                    //tblInfoComentarios.LockedWidth = true;

                    PdfPCell tituComentarios = new PdfPCell(new Phrase("Observaciones:", _clienteFontBold));
                    tituComentarios.Border = 0;
                    tituComentarios.HorizontalAlignment = Element.ALIGN_LEFT;
                    tituComentarios.PaddingBottom = 5f;
                    if (InvoiceTypeCode == "03")
                    {
                        //cuando es boleta
                        tituComentarios.PaddingTop = -15f;
                    }
                    else
                    {
                        tituComentarios.PaddingTop = -5f;
                    }

                    tblInfoComentarios.AddCell(tituComentarios);



                    var comentarios_string = teclaf8 + " " + teclavtrlm;

                    PdfPCell contComentarios = new PdfPCell(new Phrase(teclavtrlm, _clienteFontContentMinFooter));
                    contComentarios.BorderWidth = 0.75f;
                    contComentarios.PaddingBottom = 5f;
                    contComentarios.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    tblInfoComentarios.AddCell(contComentarios);

                    /* if (cabecera.Cs_tag_InvoiceTypeCode != "03")
                     {*/
                    PdfPCell tituDatos = new PdfPCell(new Phrase("DATOS:", _clienteFontBold));
                    tituDatos.Border = 0;
                    tituDatos.HorizontalAlignment = Element.ALIGN_LEFT;
                    tituDatos.PaddingBottom = 5f;
                    tblInfoComentarios.AddCell(tituDatos);


                    /* TABLA PARA NRO ORDEN PEDIDO Y CUENTAS BANCARIAS*/
                    //PdfPTable tblOrdenCuenta = new PdfPTable(11);
                    //tblOrdenCuenta.WidthPercentage = 100;
                    //PdfPCell labelOrden = new PdfPCell(new Phrase("Nº Orden de Pedido:", _clienteFontBoldContentMinFooter));
                    //labelOrden.Colspan = 2;
                    //labelOrden.Border = 0;
                    //labelOrden.HorizontalAlignment = Element.ALIGN_LEFT;
                    //PdfPCell valueOrden = new PdfPCell(new Phrase(teclaf8, _clienteFontContent));
                    //valueOrden.Colspan = 9;
                    //valueOrden.Border = 0;
                    //valueOrden.HorizontalAlignment = Element.ALIGN_LEFT;
                    //tblOrdenCuenta.AddCell(labelOrden);
                    //tblOrdenCuenta.AddCell(valueOrden);

                    //PdfPCell labelCuentas = new PdfPCell(new Phrase("Ctas Bancarias:", _clienteFontBoldContentMinFooter));
                    //labelCuentas.Colspan = 2;
                    //labelCuentas.Border = 0;
                    //labelCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                    //var pdat = new Paragraph();
                    //pdat.Font = _clienteFontContentMinFooter;
                    //pdat.Add(cuentasbancarias);
                    //PdfPCell valueCuentas = new PdfPCell(pdat);
                    //valueCuentas.Colspan = 9;
                    //valueCuentas.Border = 0;
                    //valueCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                    //tblOrdenCuenta.AddCell(labelCuentas);
                    //tblOrdenCuenta.AddCell(valueCuentas);

                    //tblInfoComentarios.AddCell(tblOrdenCuenta);

                    //PdfPCell cellBlanco = new PdfPCell(new Phrase("", _clienteFontBoldContentMinFooter));
                    //cellBlanco.Border = 0;

                    //tblInfoComentarios.AddCell(cellBlanco);
                    // }
                    /*PdfPCell contDatos = new PdfPCell(pdat);
                    contDatos.BorderWidth = 0.75f;
                    contDatos.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    tblInfoComentarios.AddCell(contDatos);
                    */

                    //resumen 
                    PdfPTable tblInfoResumen = new PdfPTable(4);
                    tblInfoResumen.WidthPercentage = 100;

                    //tblInfoResumen.TotalWidth = 144f;
                    //tblInfoResumen.LockedWidth = true;

                    //Cristhian|05/10/2017|FEICONT2-381
                    /*El sub_total debe ser el monto de op_gravada + */
                    /*MODIFICACION INICIO*/
                    sub_total += double.Parse(op_gravada, CultureInfo.InvariantCulture);
                    sub_total += double.Parse(total_descuentos, CultureInfo.InvariantCulture);
                    /*MODIFICACION FIN*/

                    if (InvoiceTypeCode != "03")
                    {
                        // moneda

                        var monedaDatos = GetCurrencySymbol(DocumentCurrencyCode);
                        string output_subtotal = "";


                        if (op_gratuita == "0.00")
                        {
                            output_subtotal = sub_total.ToString("0.00", CultureInfo.InvariantCulture);
                        }
                        else
                        {//verificar si existe op gravada y ponerlo si existe
                            if (op_gravada != "0.00")
                            {
                                output_subtotal = sub_total.ToString("0.00", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                output_subtotal = "0.00";
                            }

                        }

                        PdfPCell resItem6 = new PdfPCell(new Phrase("Sub Total", _clienteFontBold));
                        resItem6.Colspan = 2;
                        resItem6.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue6 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + output_subtotal, _clienteFontContent));
                        resvalue6.Colspan = 2;
                        resvalue6.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem6);
                        tblInfoResumen.AddCell(resvalue6);

                        PdfPCell resItem7 = new PdfPCell(new Phrase("Otros Cargos", _clienteFontBold));
                        resItem7.Colspan = 2;
                        resItem7.HorizontalAlignment = Element.ALIGN_LEFT;

                        if (LMTChargeTotalAmount.Trim().Length == 0)
                        {
                            LMTChargeTotalAmount = "0.00";
                        }
                        PdfPCell resvalue7 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + LMTChargeTotalAmount, _clienteFontContent));
                        resvalue7.Colspan = 2;
                        resvalue7.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem7);
                        tblInfoResumen.AddCell(resvalue7);

                        PdfPCell resItem8 = new PdfPCell(new Phrase("Total de Descuento", _clienteFontBold));
                        resItem8.Colspan = 2;
                        resItem8.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue8 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + total_descuentos, _clienteFontContent));
                        resvalue8.Colspan = 2;
                        resvalue8.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem8);
                        tblInfoResumen.AddCell(resvalue8);

                        PdfPCell resItem1 = new PdfPCell(new Phrase("Operaciones Gravadas", _clienteFontBold));
                        resItem1.Colspan = 2;
                        resItem1.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue1 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_gravada, _clienteFontContent));
                        resvalue1.Colspan = 2;
                        resvalue1.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem1);
                        tblInfoResumen.AddCell(resvalue1);

                        PdfPCell resItem2 = new PdfPCell(new Phrase("Operaciones Inafectas", _clienteFontBold));
                        resItem2.Colspan = 2;
                        resItem2.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue2 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_inafecta, _clienteFontContent));
                        resvalue2.Colspan = 2;
                        resvalue2.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem2);
                        tblInfoResumen.AddCell(resvalue2);

                        PdfPCell resItem3 = new PdfPCell(new Phrase("Operaciones Exoneradas", _clienteFontBold));
                        resItem3.Colspan = 2;
                        resItem3.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue3 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_exonerada, _clienteFontContent));
                        resvalue3.Colspan = 2;
                        resvalue3.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem3);
                        tblInfoResumen.AddCell(resvalue3);

                        if (imp_IGV != "")
                        {
                            PdfPCell resItem4_1 = new PdfPCell(new Phrase("IGV", _clienteFontBold));
                            resItem4_1.Colspan = 2;
                            resItem4_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue4_1 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + imp_IGV, _clienteFontContent));
                            resvalue4_1.Colspan = 2;
                            resvalue4_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem4_1);
                            tblInfoResumen.AddCell(resvalue4_1);
                        }
                        /*if (imp_ISC != "")
                        {
                            PdfPCell resItem4_2 = new PdfPCell(new Phrase("ISC", _clienteFontBold));
                            resItem4_2.Colspan = 2;
                            resItem4_2.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue4_2 = new PdfPCell(new Phrase(imp_ISC, _clienteFontContent));
                            resvalue4_2.Colspan = 2;
                            resvalue4_2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem4_2);
                            tblInfoResumen.AddCell(resvalue4_2);
                        }
                        if (imp_OTRO != "")
                        {
                            PdfPCell resItem4_3 = new PdfPCell(new Phrase("Otros tributos", _clienteFontBold));
                            resItem4_3.Colspan = 2;
                            resItem4_3.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue4_3 = new PdfPCell(new Phrase(imp_OTRO, _clienteFontContent));
                            resvalue4_3.Colspan = 2;
                            resvalue4_3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem4_3);
                            tblInfoResumen.AddCell(resvalue4_3);
                        }*/

                        PdfPCell resItem5 = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                        resItem5.Colspan = 2;
                        resItem5.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue5 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + LMTPayableAmount, _clienteFontContent));
                        resvalue5.Colspan = 2;
                        resvalue5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tblInfoResumen.AddCell(resItem5);
                        tblInfoResumen.AddCell(resvalue5);

                        PdfPCell resItem9 = new PdfPCell(new Phrase("", _clienteFontBold));
                        resItem9.Colspan = 2;
                        resItem9.Border = 0;
                        resItem9.PaddingBottom = 0f;
                        resItem9.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue9 = new PdfPCell(new Phrase("", _clienteFontContent));
                        resvalue9.Colspan = 2;
                        resvalue9.Border = 0;
                        resvalue9.PaddingBottom = 0f;
                        resvalue9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tblInfoResumen.AddCell(resItem9);
                        tblInfoResumen.AddCell(resvalue9);


                    }
                    //lado izquierdo
                    PdfPCell tblInfoFooterLeft = new PdfPCell(tblInfoComentarios);
                    if (InvoiceTypeCode != "03")
                    {
                        tblInfoFooterLeft.Colspan = 6;
                        tblInfoFooterLeft.PaddingRight = 10f;
                    }
                    else
                    {
                        tblInfoFooterLeft.Colspan = 10;
                        tblInfoFooterLeft.PaddingRight = 0;
                    }

                    tblInfoFooterLeft.Border = 0;

                    tblInfoFooter.AddCell(tblInfoFooterLeft);
                    //lado derecho

                    PdfPCell tblInfoFooterRight = new PdfPCell(tblInfoResumen);
                    tblInfoFooterRight.Colspan = 4;
                    tblInfoFooterRight.Border = 0;
                    tblInfoFooter.AddCell(tblInfoFooterRight);


                    doc.Add(tblInfoFooter);
                    doc.Add(tblBlanco);
                    if (InvoiceTypeCode == "01")
                    {
                        /*----------- Monto total en letras --------------*/
                        NumLetra totalLetras = new NumLetra();
                        PdfPTable tblInfoMontoTotal = new PdfPTable(1);
                        tblInfoMontoTotal.WidthPercentage = 100;
                        PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                        infoTotal.BorderWidth = 0.75f;
                        infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                        tblInfoMontoTotal.AddCell(infoTotal);
                        doc.Add(tblInfoMontoTotal);
                        /*-------------End Monto Total----------------*/
                        doc.Add(tblBlanco);
                    }

                    PdfPTable tblFooter = new PdfPTable(10);
                    tblFooter.WidthPercentage = 100;

                    var p = new Paragraph();
                    p.Font = _clienteFontBold;
                    p.Add(digestValue + "\n\n");
                    p.Add(info_general[3]);
                    p.Add(autorizacion_sunat + "\n");

                    PdfPCell DataHash = new PdfPCell(new Phrase(digestValue, _clienteFontBold));
                    DataHash.Border = 0;
                    DataHash.Colspan = 6;
                    DataHash.HorizontalAlignment = Element.ALIGN_CENTER;
                    // DataHash.PaddingTop = 5f;                

                    PdfPCell campo1 = new PdfPCell(p);
                    campo1.Colspan = 6;
                    campo1.Border = 0;
                    campo1.PaddingTop = 20f;
                    campo1.HorizontalAlignment = Element.ALIGN_CENTER;

                    //codigo de barras                               
                    //var hash = new clsNegocioXML();
                    //var hash_obtenido=hash.cs_fxHash(cabecera.Cs_pr_Document_Id);

                    Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                                {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                            };

                    //Cristhian|26/12/2017|FEICONT2-509
                    /*Se corrigio la cadena que se envia al Generador de la imagen QR*/
                    /*INICIO MODIFICACIóN*/
                    var textQR = ASPCustomerAssignedAccountID + " | " + InvoiceTypeCode + " | " + doc_serie + "|" + doc_correlativo + " | " + imp_IGV + " | " + LMTPayableAmount + " | " + IssueDate + " | " + ACPAdditionalAccountID + " | " + ACPCustomerAssignedAccountID + " |";
                    /*INICIO MODIFICACIóN*/

                    BarcodeQRCode qrcode = new BarcodeQRCode(textQR, 400, 400, ob);

                    iTextSharp.text.Image qrcodeImage = qrcode.GetImage();

                    /* BarcodePDF417 barcod = new BarcodePDF417();
                     barcod.SetText(cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID+" | "+ cabecera.Cs_tag_InvoiceTypeCode+" | "+ doc_serie+" | "+doc_correlativo+" | "+ impuestos_globales.Cs_tag_TaxSubtotal_TaxAmount+" | "+ cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID+" | "+ cabecera.Cs_tag_IssueDate+" | "+cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID+" | "+cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID+" | "+ digestValue + " | "+signatureValue+" |");
                     barcod.ErrorLevel = 5;
                     barcod.Options = BarcodePDF417.PDF417_FORCE_BINARY;

                     iTextSharp.text.Image imagePDF417 = barcod.GetImage();*/
                    //qrcodeImage.ScaleAbsolute(100f, 90f);
                    PdfPCell blanco12 = new PdfPCell();
                    // blanco12.Image = qrcodeImage;
                    blanco12.AddElement(new Chunk(qrcodeImage, 55f, -65f));
                    blanco12.Border = 0;
                    blanco12.PaddingTop = 20f;
                    blanco12.Colspan = 4;


                    PdfPCell blanco121 = new PdfPCell(new Paragraph(" "));
                    blanco121.Border = 0;
                    blanco121.Colspan = 4;

                    tblFooter.AddCell(campo1);
                    tblFooter.AddCell(blanco12);
                    //tblFooter.AddCell(campo1);
                    // tblFooter.AddCell(blanco121);

                    doc.Add(tblFooter);


                    doc.Close();
                    File.SetAttributes(newFile, FileAttributes.Normal);
                    writer.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" generar representacion impresa " + ex.ToString());
                return false;
            }

        }
        #endregion

        #region Comprobante de Retencion
        public static bool getRepresentacionImpresaRetencion(string pathToSaved, clsEntityRetention cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal();
            try
            {
                var doc_serie = "";
                var doc_correlativo = "";
                if (cabecera != null)
                {

                    string[] partes = cabecera.Cs_tag_Id.Split('-');
                    DateTime dt = DateTime.ParseExact(cabecera.Cs_tag_IssueDate, "yyyy-MM-dd", null);
                    doc_serie = partes[0];
                    doc_correlativo = partes[1];
                    string newFile = pathToSaved;

                    if (File.Exists(newFile))
                    {
                        File.Delete(newFile);
                    }

                    XmlDocument xmlDocument = new XmlDocument();
                    //var textXml = cabecera.Cs_pr_XML;
                    var textXml = textXML;
                    textXml = textXml.Replace("cbc:", "");
                    textXml = textXml.Replace("cac:", "");
                    textXml = textXml.Replace("sac:", "");
                    textXml = textXml.Replace("ext:", "");
                    textXml = textXml.Replace("ds:", "");
                    xmlDocument.LoadXml(textXml);

                    var signatureValue = xmlDocument.GetElementsByTagName("SignatureValue")[0].InnerText;
                    var digestValue = xmlDocument.GetElementsByTagName("DigestValue")[0].InnerText;                   

                    string IssueDate = xmlDocument.GetElementsByTagName("IssueDate")[0].InnerText;
                    string APPartyIdentificationID = "";
                    string APPartyIdentificationSchemeID = "";
                    string APPartyName = "";
                    string APPostalAdressID = "";
                    string APPostalAdressStreetName = "";
                    string APPostalAdressCitySubDivision = "";
                    string APPostalAdressCityName = "";
                    string APPostalAdressCountrySubEntity = "";
                    string APPostalAdressDistrict = "";
                    string APPostalAdressCountryIdentificationCode = "";
                    string APPartyLegalEntityRegistrationName = "";
                    string ARIdentificationID = "";
                    string ARIdentificationID_SchemeID = "";
                    string ARPartyName_Name = "";
                    string ARPostalAddressId = "";
                    string ARPostalAddressStreetName = "";
                    string ARPostalAddressCitySubDivisionName = "";
                    string ARPostalAddressCityName = "";
                    string ARPostalAddressCountrySubentity = "";
                    string ARPostalAddressDisctrict = "";
                    string ARPostalAddressCountryIdentificationCode = "";
                    string ARPartyLegalEntityRegistrationName = "";
                    string RetentionSystemCode = "";
                    string RetentionPercent = "";
                    string TotalInvoiceAmount = "";
                    string TotalInvoiceAmountCurrencyID = "";
                    string SunatTotalPaid = "";
                    string SunatTotalPaidCurrencyID = "";
                    string ExchangeRateCalculationRate = "";

                    Document doc = new Document(PageSize.A4);
                    // Indicamos donde vamos a guardar el documento
                    PdfWriter writer = PdfWriter.GetInstance(doc,
                                                new FileStream(newFile, FileMode.Create));

                    // Le colocamos el título y el autor
                    // Esto no será visible en el documento
                    doc.AddTitle("Documento Electronico");
                    doc.AddCreator("Contasis");

                    // Abrimos el archivo
                    doc.Open();
                    // Creamos el tipo de Font que vamos utilizar
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _TitleFontN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _TitleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _HeaderFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _HeaderFontMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContent = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    PdfPTable tblPrueba = new PdfPTable(5);
                    tblPrueba.WidthPercentage = 100;

                    //TABLA header left
                    PdfPTable tblHeaderLeft = new PdfPTable(1);
                    tblHeaderLeft.WidthPercentage = 100;

                    //string currentDirectory = Environment.CurrentDirectory;
                    //string pathImage = currentDirectory + "\\logo.png";
                    //Creamos la imagen y le ajustamos el tamaño
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(pathImage);
                    imagen.BorderWidth = 0;
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 290 / imagen.Width;
                    imagen.ScalePercent(80);

                    // Insertamos la imagen en el documento

                    PdfPCell logo = new PdfPCell(imagen);
                    logo.BorderWidth = 0;
                    logo.BorderWidthBottom = 0;
                    logo.Border = 0;

                    tblHeaderLeft.AddCell(logo);


                    //get accounting supplier party
                    XmlNodeList AgentParty = xmlDocument.GetElementsByTagName("AgentParty");
                    foreach (XmlNode dat in AgentParty)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var caaid = xmlDocumentinner.GetElementsByTagName("PartyIdentification");
                        if (caaid.Count > 0)
                        {
                            APPartyIdentificationID = caaid.Item(0).InnerText;
                        }
                       
                        if (caaid.Count > 0)
                        {                    
                            if (caaid.Item(0).ChildNodes[0].Attributes.Count > 0)
                            {
                                APPartyIdentificationSchemeID = caaid.Item(0).ChildNodes[0].Attributes.GetNamedItem("schemeID").Value;
                            }
                        }                     
                        var aacid = xmlDocumentinner.GetElementsByTagName("PartyName");
                        if (aacid.Count > 0)
                        {
                            APPartyName = aacid.Item(0).InnerText;
                        }
                        var stname = xmlDocumentinner.GetElementsByTagName("ID");
                        if (stname.Count > 0)
                        {
                            APPostalAdressID = stname.Item(0).InnerText;
                        }                        
                        var regname = xmlDocumentinner.GetElementsByTagName("StreetName");
                        if (regname.Count > 0)
                        {
                            APPostalAdressStreetName = regname.Item(0).InnerText;
                        }
                        var regnamesn = xmlDocumentinner.GetElementsByTagName("CitySubdivisionName");
                        if (regnamesn.Count > 0)
                        {
                            APPostalAdressCitySubDivision = regnamesn.Item(0).InnerText;
                        }
                        var regnamesncn = xmlDocumentinner.GetElementsByTagName("CityName");
                        if (regnamesncn.Count > 0)
                        {
                            APPostalAdressCityName = regnamesncn.Item(0).InnerText;
                        }
                        var regcs = xmlDocumentinner.GetElementsByTagName("CountrySubentity");
                        if (regcs.Count > 0)
                        {
                            APPostalAdressCountrySubEntity = regcs.Item(0).InnerText;
                        }
                        var regcsd = xmlDocumentinner.GetElementsByTagName("District");
                        if (regcsd.Count > 0)
                        {
                            APPostalAdressDistrict = regcsd.Item(0).InnerText;
                        }
                        var regcsdic = xmlDocumentinner.GetElementsByTagName("IdentificationCode");
                        if (regcsdic.Count > 0)
                        {
                            APPostalAdressCountryIdentificationCode = regcsdic.Item(0).InnerText;
                        }
                        var regcsdicrn = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                        if (regcsdicrn.Count > 0)
                        {
                            APPartyLegalEntityRegistrationName = regcsdicrn.Item(0).InnerText;
                        }              
                    }
                    //get receiver party
                    XmlNodeList ReceiverParty = xmlDocument.GetElementsByTagName("ReceiverParty");
                    foreach (XmlNode dat in ReceiverParty)
                    {
                        XmlDocument xmlDocumentinner = new XmlDocument();
                        xmlDocumentinner.LoadXml(dat.OuterXml);

                        var caaid = xmlDocumentinner.GetElementsByTagName("PartyIdentification");
                        if (caaid.Count > 0)
                        {
                            ARIdentificationID = caaid.Item(0).InnerText;
                        }

                        if (caaid.Count > 0)
                        {
                            if (caaid.Item(0).ChildNodes[0].Attributes.Count > 0)
                            {
                                ARIdentificationID_SchemeID = caaid.Item(0).ChildNodes[0].Attributes.GetNamedItem("schemeID").Value;
                            }
                        }
                        var aacid = xmlDocumentinner.GetElementsByTagName("PartyName");
                        if (aacid.Count > 0)
                        {
                            ARPartyName_Name = aacid.Item(0).InnerText;
                        }
                        var stname = xmlDocumentinner.GetElementsByTagName("ID");
                        if (stname.Count > 0)
                        {
                            ARPostalAddressId = stname.Item(0).InnerText;
                        }
                        var regname = xmlDocumentinner.GetElementsByTagName("StreetName");
                        if (regname.Count > 0)
                        {
                            ARPostalAddressStreetName = regname.Item(0).InnerText;
                        }
                        var regnamesn = xmlDocumentinner.GetElementsByTagName("CitySubdivisionName");
                        if (regnamesn.Count > 0)
                        {
                            ARPostalAddressCitySubDivisionName = regnamesn.Item(0).InnerText;
                        }
                        var regnamesncn = xmlDocumentinner.GetElementsByTagName("CityName");
                        if (regnamesncn.Count > 0)
                        {
                            ARPostalAddressCityName = regnamesncn.Item(0).InnerText;
                        }
                        var regcs = xmlDocumentinner.GetElementsByTagName("CountrySubentity");
                        if (regcs.Count > 0)
                        {
                            ARPostalAddressCountrySubentity = regcs.Item(0).InnerText;
                        }
                        var regcsd = xmlDocumentinner.GetElementsByTagName("District");
                        if (regcsd.Count > 0)
                        {
                            ARPostalAddressDisctrict = regcsd.Item(0).InnerText;
                        }
                        var regcsdic = xmlDocumentinner.GetElementsByTagName("IdentificationCode");
                        if (regcsdic.Count > 0)
                        {
                            ARPostalAddressCountryIdentificationCode = regcsdic.Item(0).InnerText;
                        }
                        var regcsdicrn = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                        if (regcsdicrn.Count > 0)
                        {
                            ARPartyLegalEntityRegistrationName = regcsdicrn.Item(0).InnerText;
                        }
                    }

                    //Cristhian|25/09/2017|FEICONT2-364
                    /*Se copia el codigo del "exchangeRate" y se modifica por "exchangeRates", esto se realiza para obtener el dato de
                      "tipo de cambio" este dato, si se revisa el XML, se encuentra tantas veces como montos se tiene en el documento,
                      se decide obtener el primero ya que en el resto el mismo dato se repite*/
                    /*NUEVO INICIO*/
                    XmlNodeList exchangeRates = xmlDocument.GetElementsByTagName("ExchangeRate");
                    if (exchangeRates.Count > 0)
                    {
                        XmlDocument xmlItemExchangeRate = new XmlDocument();
                        xmlItemExchangeRate.LoadXml(exchangeRates.Item(0).OuterXml);

                        var sourceCurrency = xmlItemExchangeRate.GetElementsByTagName("SourceCurrencyCode");
                        if (sourceCurrency.Count > 0)
                        {
                            //Sin Variable
                        }
                        var targetCurrency = xmlItemExchangeRate.GetElementsByTagName("TargetCurrencyCode");
                        if (targetCurrency.Count > 0)
                        {
                            //Sin Variable
                        }
                        var calRate = xmlItemExchangeRate.GetElementsByTagName("CalculationRate");
                        if (calRate.Count > 0)
                        {
                            ExchangeRateCalculationRate = calRate.Item(0).InnerText;//Para el Tipo de Cambio - FEICONT-632
                        }
                        var datee = xmlItemExchangeRate.GetElementsByTagName("Date");
                        if (datee.Count > 0)
                        {
                            //Sin Variable
                        }
                    }
                    /*NUEVO FIN*/

                    RetentionSystemCode = xmlDocument.GetElementsByTagName("SUNATRetentionSystemCode")[0].InnerText;
                    RetentionPercent = xmlDocument.GetElementsByTagName("SUNATRetentionPercent")[0].InnerText;
                    TotalInvoiceAmount = xmlDocument.GetElementsByTagName("TotalInvoiceAmount")[0].InnerText;
                    var TotalInvoiceA= xmlDocument.GetElementsByTagName("TotalInvoiceAmount");
                    if (TotalInvoiceA.Item(0).Attributes.Count > 0)
                    {
                        TotalInvoiceAmountCurrencyID = TotalInvoiceA.Item(0).Attributes.GetNamedItem("currencyID").Value;
                    }

                    SunatTotalPaid= xmlDocument.GetElementsByTagName("SUNATTotalPaid")[0].InnerText;

                    var SunatTotalPaidC = xmlDocument.GetElementsByTagName("SUNATTotalPaid");
                    if (SunatTotalPaidC.Item(0).Attributes.Count > 0)
                    {
                        SunatTotalPaidCurrencyID = SunatTotalPaidC.Item(0).Attributes.GetNamedItem("currencyID").Value;
                    }
 
                    //tabla info empresa
                    PdfPTable tblInforEmpresa = new PdfPTable(1);
                    tblInforEmpresa.WidthPercentage = 100;
                    PdfPCell NameEmpresa = new PdfPCell(new Phrase(APPartyLegalEntityRegistrationName, _HeaderFont));
                    NameEmpresa.BorderWidth = 0;
                    NameEmpresa.Border = 0;
                    tblInforEmpresa.AddCell(NameEmpresa);

                    var pa = new Paragraph();
                    pa.Font = _clienteFontBoldMin;
                    pa.Add(APPostalAdressStreetName);
                    PdfPCell EstaticoEmpresa = new PdfPCell(pa);
                    EstaticoEmpresa.BorderWidth = 0;
                    EstaticoEmpresa.Border = 0;
                    tblInforEmpresa.AddCell(EstaticoEmpresa);

                    PdfPCell celdaInfoEmpresa = new PdfPCell(tblInforEmpresa);
                    celdaInfoEmpresa.Border = 0;
                    tblHeaderLeft.AddCell(celdaInfoEmpresa);
                    // PdfPCell blanco = new PdfPCell();
                    // blanco.Border = 0;
                    //tabla para info ruc
                    PdfPTable tblInforRuc = new PdfPTable(1);
                    tblInforRuc.WidthPercentage = 100;

                    PdfPCell TituRuc = new PdfPCell(new Phrase("R.U.C. " + APPartyIdentificationID, _TitleFontN));
                    TituRuc.BorderWidthTop = 0.75f;
                    TituRuc.BorderWidthBottom = 0.75f;
                    TituRuc.BorderWidthLeft = 0.75f;
                    TituRuc.BorderWidthRight = 0.75f;
                    TituRuc.HorizontalAlignment = Element.ALIGN_CENTER;
                    TituRuc.PaddingTop = 10f;
                    TituRuc.PaddingBottom = 10f;

                    PdfPCell TipoDoc = new PdfPCell(new Phrase("Comprobante de Retención Electrónico", _TitleFontN));
                    TipoDoc.BorderWidthLeft = 0.75f;
                    TipoDoc.BorderWidthRight = 0.75f;
                    TipoDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    TipoDoc.PaddingTop = 10f;
                    TipoDoc.PaddingBottom = 10f;

                    PdfPCell SerieDoc = new PdfPCell(new Phrase("N° " + cabecera.Cs_tag_Id, _TitleFont));
                    SerieDoc.BorderWidthBottom = 0.75f;
                    SerieDoc.BorderWidthRight = 0.75f;
                    SerieDoc.BorderWidthLeft = 0.75f;
                    SerieDoc.BorderWidthTop = 0.75f;
                    SerieDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    SerieDoc.PaddingTop = 10f;
                    SerieDoc.PaddingBottom = 10f;

                    PdfPCell blanco2 = new PdfPCell(new Paragraph(" "));
                    blanco2.Border = 0;
                    tblInforRuc.AddCell(TituRuc);
                    //tblInforRuc.AddCell(blanco2);
                    tblInforRuc.AddCell(TipoDoc);
                    //tblInforRuc.AddCell(blanco2);
                    tblInforRuc.AddCell(SerieDoc);
                    tblInforRuc.AddCell(blanco2);

                    PdfPCell infoRuc = new PdfPCell(tblInforRuc);
                    infoRuc.Colspan = 2;
                    infoRuc.BorderWidth = 0;

                    PdfPCell celdaHeaderLeft = new PdfPCell(tblHeaderLeft);
                    celdaHeaderLeft.Border = 0;
                    celdaHeaderLeft.Colspan = 3;

                    // Añadimos las celdas a la tabla
                    tblPrueba.AddCell(celdaHeaderLeft);
                    // tblPrueba.AddCell(blanco);
                    tblPrueba.AddCell(infoRuc);

                    doc.Add(tblPrueba);

                    PdfPTable tblBlanco = new PdfPTable(1);
                    tblBlanco.WidthPercentage = 100;
                    PdfPCell blanco3 = new PdfPCell((new Paragraph(" ")));
                    blanco3.Border = 0;

                    tblBlanco.AddCell(blanco3);

                    doc.Add(tblBlanco);

                    //Informacion cliente
                    PdfPTable tblInfoCliente = new PdfPTable(10);
                    tblInfoCliente.WidthPercentage = 100;

                    // Llenamos la tabla con información del cliente
                    PdfPCell cliente = new PdfPCell(new Phrase("Cliente:", _clienteFontBoldMin));
                    cliente.BorderWidth = 0;
                    cliente.Colspan = 1;

                    PdfPCell clNombre = new PdfPCell(new Phrase(ARPartyLegalEntityRegistrationName, _clienteFontContentMinFooter));
                    clNombre.BorderWidth = 0;
                    clNombre.Colspan = 5;

                    PdfPCell fecha = new PdfPCell(new Phrase("Fecha de Emision:", _clienteFontBoldMin));
                    fecha.BorderWidth = 0;
                    fecha.Colspan = 2;

                    var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                    PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                    clFecha.BorderWidth = 0;
                    clFecha.Colspan = 2;

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(cliente);
                    tblInfoCliente.AddCell(clNombre);
                    tblInfoCliente.AddCell(fecha);
                    tblInfoCliente.AddCell(clFecha);

                    PdfPCell Rucc = new PdfPCell(new Phrase("RUC:", _clienteFontBoldMin));
                    Rucc.BorderWidth = 0;
                    Rucc.Colspan = 1;

                    PdfPCell clRuc = new PdfPCell(new Phrase(ARIdentificationID, _clienteFontContentMinFooter));
                    clRuc.BorderWidth = 0;
                    clRuc.Colspan = 5;

                    //PdfPCell bl1 = new PdfPCell(new Phrase("", _clienteFontBoldMin));
                    //bl1.BorderWidth = 0;
                    //bl1.Colspan = 4;
                    
                    //Cristhian|25/09/2017|FEICONT2-364
                    /*NUEVO INICIO*/
                    /*Agregado para la representación impresa donde se mostrará el tipo de cambio*/
                    PdfPCell tipodecambio = new PdfPCell(new Phrase("Tipo de Cambio:", _clienteFontBoldMin));
                    tipodecambio.BorderWidth = 0;
                    tipodecambio.Colspan = 2;

                    PdfPCell clTipodecambio = new PdfPCell(new Phrase(ExchangeRateCalculationRate, _clienteFontContentMinFooter));
                    clTipodecambio.BorderWidth = 0;
                    clTipodecambio.Colspan = 2;
                    /*NUEVO FIN*/

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(Rucc);
                    tblInfoCliente.AddCell(clRuc);
                    //tblInfoCliente.AddCell(bl1);

                    //Cristhian|25/09/2017|FEICONT2-364
                    /*Agregado para la representación impresa donde se mostrará el tipo de cambio,
                      aqui se añade la celda a la tabla*/
                    /*NUEVO INICIO*/
                    tblInfoCliente.AddCell(tipodecambio);
                    tblInfoCliente.AddCell(clTipodecambio);
                    /*NUEVO FIN*/

                    PdfPCell direccion = new PdfPCell(new Phrase("Direccion:", _clienteFontBoldMin));
                    direccion.BorderWidth = 0;
                    direccion.Colspan = 1;

                    PdfPCell clDireccion = new PdfPCell(new Phrase(ARPostalAddressStreetName, _clienteFontContentMinFooter));
                    clDireccion.BorderWidth = 0;
                    clDireccion.Colspan = 9;
                    
                    tblInfoCliente.AddCell(direccion);
                    tblInfoCliente.AddCell(clDireccion);

                    doc.Add(tblInfoCliente);
                    doc.Add(tblBlanco);

                    PdfPTable tblInfoComprobante = new PdfPTable(16);
                    tblInfoComprobante.WidthPercentage = 100;

                    PdfPCell colSuperior = new PdfPCell(new Phrase("COMPROBANTES DE PAGO QUE DAN ORIGEN A LA RETENCION", _clienteFontBoldMin));
                    colSuperior.BorderWidthTop = 0.75f;
                    colSuperior.BorderWidthRight = 0.75f;
                    colSuperior.BorderWidthLeft = 0.75f;
                    colSuperior.BorderWidthBottom = 0.75f;
                    colSuperior.PaddingTop = 5;
                    colSuperior.PaddingBottom = 5;
                    colSuperior.Colspan = 12;
                    colSuperior.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colSuperiorDerechaUno = new PdfPCell(new Phrase("", _clienteFontBoldMin));
                    colSuperiorDerechaUno.BorderWidthTop = 0.75f;
                    colSuperiorDerechaUno.BorderWidthRight = 0.75f;
                    colSuperiorDerechaUno.BorderWidthLeft = 0;
                    colSuperiorDerechaUno.BorderWidthBottom = 0;
                    colSuperiorDerechaUno.Colspan = 2;
                    colSuperiorDerechaUno.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colSuperiorDerechaDos = new PdfPCell(new Phrase("", _clienteFontBoldMin));
                    colSuperiorDerechaDos.BorderWidthTop = 0.75f;
                    colSuperiorDerechaDos.BorderWidthRight = 0.75f;
                    colSuperiorDerechaDos.BorderWidthLeft = 0;
                    colSuperiorDerechaDos.BorderWidthBottom = 0;
                    colSuperiorDerechaDos.Colspan = 2;
                    colSuperiorDerechaDos.HorizontalAlignment = Element.ALIGN_CENTER;

                    tblInfoComprobante.AddCell(colSuperior);
                    tblInfoComprobante.AddCell(colSuperiorDerechaUno);
                    tblInfoComprobante.AddCell(colSuperiorDerechaDos);
                    // Llenamos la tabla con información
                    PdfPCell colTipo = new PdfPCell(new Phrase("TIPO", _clienteFontBoldMin));
                    colTipo.BorderWidthLeft = 0.75f;
                    colTipo.BorderWidthRight = 0.75f;
                    colTipo.BorderWidthBottom = 0.75f;
                    colTipo.BorderWidthTop = 0;
                    colTipo.Colspan = 2;
                    colTipo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colSerie = new PdfPCell(new Phrase("SERIE", _clienteFontBoldMin));
                    colSerie.BorderWidthLeft = 0;
                    colSerie.BorderWidthRight = 0.75f;
                    colSerie.BorderWidthBottom = 0.75f;
                    colSerie.BorderWidthTop = 0;
                    colSerie.Colspan = 1;
                    colSerie.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colCorrelativo = new PdfPCell(new Phrase("CORRELATIVO", _clienteFontBoldMin));
                    colCorrelativo.BorderWidthLeft = 0;
                    colCorrelativo.BorderWidthRight = 0.75f;
                    colCorrelativo.BorderWidthBottom = 0.75f;
                    colCorrelativo.BorderWidthTop = 0;
                    colCorrelativo.Colspan = 2;
                    colCorrelativo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colFecha = new PdfPCell(new Phrase("FECHA EMISION", _clienteFontBoldMin));
                    colFecha.BorderWidthLeft = 0;
                    colFecha.BorderWidthRight = 0.75f;
                    colFecha.BorderWidthBottom = 0.75f;
                    colFecha.BorderWidthTop = 0;
                    colFecha.Colspan = 2;
                    colFecha.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colFechaPago = new PdfPCell(new Phrase("FECHA PAGO", _clienteFontBoldMin));
                    colFechaPago.BorderWidthLeft = 0;
                    colFechaPago.BorderWidthRight = 0.75f;
                    colFechaPago.BorderWidthBottom = 0.75f;
                    colFechaPago.BorderWidthTop = 0;
                    colFechaPago.Colspan = 2;
                    colFechaPago.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colNumeroPago = new PdfPCell(new Phrase("Nº PAGO", _clienteFontBoldMin));
                    colNumeroPago.BorderWidthLeft = 0;
                    colNumeroPago.BorderWidthRight = 0.75f;
                    colNumeroPago.BorderWidthBottom = 0.75f;
                    colNumeroPago.BorderWidthTop = 0;
                    colNumeroPago.Colspan = 1;
                    colNumeroPago.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colMontoDocumento = new PdfPCell(new Phrase("MONTO DOC", _clienteFontBoldMin));
                    colMontoDocumento.BorderWidthLeft = 0;
                    colMontoDocumento.BorderWidthRight = 0.75f;
                    colMontoDocumento.BorderWidthBottom = 0.75f;
                    colMontoDocumento.BorderWidthTop = 0;
                    colMontoDocumento.Colspan = 2;
                    colMontoDocumento.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colMontoPago = new PdfPCell(new Phrase("MONTO PAGO", _clienteFontBoldMin));
                    colMontoPago.BorderWidthLeft = 0;
                    colMontoPago.BorderWidthRight = 0.75f;
                    colMontoPago.BorderWidthBottom = 0.75f;
                    colMontoPago.BorderWidthTop = 0;
                    colMontoPago.Colspan = 2;
                    colMontoPago.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colRetenido = new PdfPCell(new Phrase("RETENIDO", _clienteFontBoldMin));
                    colRetenido.BorderWidthLeft = 0;
                    colRetenido.BorderWidthRight = 0.75f;
                    colRetenido.BorderWidthBottom = 0.75f;
                    colRetenido.BorderWidthTop = 0;
                    colRetenido.Colspan = 2;
                    colRetenido.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Añadimos las celdas a la tabla
                    tblInfoComprobante.AddCell(colTipo);
                    tblInfoComprobante.AddCell(colSerie);
                    tblInfoComprobante.AddCell(colCorrelativo);
                    tblInfoComprobante.AddCell(colFecha);
                    tblInfoComprobante.AddCell(colFechaPago);
                    tblInfoComprobante.AddCell(colNumeroPago);
                    tblInfoComprobante.AddCell(colMontoDocumento);
                    tblInfoComprobante.AddCell(colMontoPago);
                    tblInfoComprobante.AddCell(colRetenido);

                    /* seccion de items ------ añadir items*/
                    var numero_item = 0;

                    List<clsEntityRetention_RetentionLine> Lista_items;
                    clsEntityRetention_RetentionLine item;
                    XmlNodeList nodeitem;
                                     
                    nodeitem = xmlDocument.GetElementsByTagName("SUNATRetentionDocumentReference");
                                  
                    var total_items = nodeitem.Count;

                    int i = 0;
                    foreach (XmlNode dat in nodeitem)
                    {
                        i++;
                        numero_item++;
                        Lista_items = new List<clsEntityRetention_RetentionLine>();                    
                        item = new clsEntityRetention_RetentionLine(local);
                        XmlDocument xmlItem = new XmlDocument();
                        xmlItem.LoadXml(dat.OuterXml);
                    
                        XmlNodeList DocumentReferenceID = xmlItem.GetElementsByTagName("ID");
                        if (DocumentReferenceID.Count > 0)
                        {
                             item.Cs_tag_Id = DocumentReferenceID.Item(0).InnerText;
                             if (DocumentReferenceID.Item(0).Attributes.Count > 0)
                             {
                                    item.Cs_tag_Id_SchemeId = DocumentReferenceID.Item(0).Attributes.GetNamedItem("schemeID").Value;
                             }
                        }
                       
                        XmlNodeList IssueDates = xmlItem.GetElementsByTagName("IssueDate");
                        if (IssueDates.Count > 0)
                        {
                            item.Cs_tag_IssueDate = IssueDates.Item(0).InnerText;
                        }

                        XmlNodeList TotalInvoiceAm = xmlItem.GetElementsByTagName("TotalInvoiceAmount");
                        if (TotalInvoiceAm.Count > 0)
                        {
                            item.Cs_tag_TotalInvoiceAmount = TotalInvoiceAm.Item(0).InnerText;
                            if (TotalInvoiceAm.Item(0).Attributes.Count > 0)
                            {
                                item.Cs_tag_TotalInvoiceAmount_CurrencyId = TotalInvoiceAm.Item(0).Attributes.GetNamedItem("currencyID").Value;
                            }
                        }

                       XmlNodeList Payment = xmlItem.GetElementsByTagName("Payment");
                       if (Payment.Count > 0)
                       {
                           foreach (XmlNode items in Payment)
                           {
                               XmlDocument xmlItemItem = new XmlDocument();
                               xmlItemItem.LoadXml(items.OuterXml);

                               XmlNodeList idd = xmlItemItem.GetElementsByTagName("ID");
                               if (idd.Count > 0)
                               {
                                   item.Cs_tag_Payment_Id = idd.Item(0).InnerText;
                               }

                                XmlNodeList paidamount = xmlItemItem.GetElementsByTagName("PaidAmount");
                                if (paidamount.Count > 0)
                                {
                                    item.Cs_tag_Payment_PaidAmount = paidamount.Item(0).InnerText;
                                    if (paidamount.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_Payment_PaidAmount_CurrencyId = paidamount.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                    }
                                }
                                XmlNodeList paiddatee = xmlItemItem.GetElementsByTagName("PaidDate");
                                if (paiddatee.Count > 0)
                                {
                                    item.Cs_tag_Payment_PaidDate = paiddatee.Item(0).InnerText;
                                }
                            }                                                    
                       }

                        XmlNodeList SunatRetentionInformation = xmlItem.GetElementsByTagName("SUNATRetentionInformation");
                        if (SunatRetentionInformation.Count > 0)
                        {
                            foreach (XmlNode taxitem in SunatRetentionInformation)
                            {
                                                       
                                XmlDocument xmlSUNATRetention = new XmlDocument();
                                xmlSUNATRetention.LoadXml(taxitem.OuterXml);
                                XmlNodeList taxItemAmount = xmlSUNATRetention.GetElementsByTagName("SUNATRetentionAmount");
                                if (taxItemAmount.Count > 0)
                                {
                                    item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = taxItemAmount.Item(0).InnerText;

                                    if (taxItemAmount.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId = taxItemAmount.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                    }
                                }

                                XmlNodeList retentionDate = xmlSUNATRetention.GetElementsByTagName("SUNATRetentionDate");
                                if (retentionDate.Count > 0)
                                {
                                    item.Cs_tag_SUNATRetentionInformation_SUNATRetentionDate = retentionDate.Item(0).InnerText;
                                }

                                XmlNodeList netTotalPaid = xmlSUNATRetention.GetElementsByTagName("SUNATNetTotalPaid");
                                if (netTotalPaid.Count > 0)
                                {
                                    item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = netTotalPaid.Item(0).InnerText;

                                    if (netTotalPaid.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId = netTotalPaid.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                    }
                                }

                                XmlNodeList exchangeRate = xmlSUNATRetention.GetElementsByTagName("ExchangeRate");
                                if (exchangeRate.Count > 0)
                                {
                                    XmlDocument xmlItemExchangeRate = new XmlDocument();
                                    xmlItemExchangeRate.LoadXml(exchangeRate.Item(0).OuterXml);

                                    var sourceCurrency = xmlItemExchangeRate.GetElementsByTagName("SourceCurrencyCode");
                                    if (sourceCurrency.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode = sourceCurrency.Item(0).InnerText;
                                    }
                                    var targetCurrency = xmlItemExchangeRate.GetElementsByTagName("TargetCurrencyCode");
                                    if (targetCurrency.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode = targetCurrency.Item(0).InnerText;
                                    }
                                    var calRate = xmlItemExchangeRate.GetElementsByTagName("CalculationRate");
                                    if (calRate.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate = calRate.Item(0).InnerText;
                                        ExchangeRateCalculationRate = calRate.Item(0).InnerText;//Para el Tipo de Cambio - FEICONT-632
                                    }
                                    var datee = xmlItemExchangeRate.GetElementsByTagName("Date");
                                    if (datee.Count > 0)
                                    {
                                        item.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date = datee.Item(0).InnerText;
                                    }
                                }                               
                            }
                        }
                        

                        string tipoComp = RepresentacionImpresa.getTipoComprobante(item.Cs_tag_Id_SchemeId);
                        PdfPCell itTipo = new PdfPCell(new Phrase(tipoComp, _clienteFontContentMinFooter));
                        itTipo.Colspan = 2;
                        itTipo.BorderWidthBottom = 0.75f;
                        itTipo.BorderWidthLeft = 0.75f;
                        itTipo.BorderWidthRight = 0.75f;
                        itTipo.BorderWidthTop = 0;
                        itTipo.HorizontalAlignment = Element.ALIGN_CENTER;

                        string[] partess = item.Cs_tag_Id.Split('-');
                        PdfPCell itSerie = new PdfPCell(new Phrase(partess[0], _clienteFontContentMinFooter));
                        itSerie.Colspan = 1;
                        itSerie.BorderWidthBottom = 0.75f;
                        itSerie.BorderWidthLeft = 0;
                        itSerie.BorderWidthRight = 0.75f;
                        itSerie.BorderWidthTop = 0;
                        itSerie.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itCorrelativo = new PdfPCell(new Phrase(partess[1], _clienteFontContentMinFooter));
                        itCorrelativo.Colspan = 2;
                        itCorrelativo.BorderWidthBottom = 0.75f;
                        itCorrelativo.BorderWidthLeft = 0;
                        itCorrelativo.BorderWidthRight = 0.75f;
                        itCorrelativo.BorderWidthTop = 0;
                        itCorrelativo.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itFecha = new PdfPCell(new Phrase(item.Cs_tag_IssueDate, _clienteFontContentMinFooter));
                        itFecha.Colspan = 2;
                        itFecha.BorderWidthBottom = 0.75f;
                        itFecha.BorderWidthLeft = 0;
                        itFecha.BorderWidthRight = 0.75f;
                        itFecha.BorderWidthTop = 0;
                        itFecha.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itFechaPago = new PdfPCell(new Phrase(item.Cs_tag_Payment_PaidDate, _clienteFontContentMinFooter));
                        itFechaPago.Colspan = 2;
                        itFechaPago.BorderWidthBottom = 0.75f;
                        itFechaPago.BorderWidthLeft = 0;
                        itFechaPago.BorderWidthRight = 0.75f;
                        itFechaPago.BorderWidthTop = 0;
                        itFechaPago.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itNumeroPago = new PdfPCell(new Phrase(item.Cs_tag_Payment_Id, _clienteFontContentMinFooter));
                        itNumeroPago.Colspan = 1;
                        itNumeroPago.BorderWidthBottom = 0.75f;
                        itNumeroPago.BorderWidthLeft = 0;
                        itNumeroPago.BorderWidthRight = 0.75f;
                        itNumeroPago.BorderWidthTop = 0;
                        itNumeroPago.HorizontalAlignment = Element.ALIGN_CENTER;

                        string montoDoc = double.Parse(item.Cs_tag_TotalInvoiceAmount, CultureInfo.InvariantCulture).ToString("#,0.00");
                        //montoDoc = CambiarPuntoPorComaDescimales(montoDoc);//FEICONT2-364
                        //Cristhian|14/12/2017|FEICONT2-490
                        /*Se agrega un condicional para verificar el tipo de moneda que se esta recibiendo y en base a ello seleccionar el simbolo
                         adecuado para la denonimación del dinero*/
                        /*NUEVO INICIO*/
                        if (item.Cs_tag_TotalInvoiceAmount_CurrencyId == "PEN")
                        {
                            montoDoc = "S/." + montoDoc;
                        }
                        else if (item.Cs_tag_TotalInvoiceAmount_CurrencyId == "USD")
                        {
                            montoDoc = "$." + montoDoc;
                        }
                        /*NUEVO FIN*/
                        PdfPCell itMontoDocumento = new PdfPCell(new Phrase(montoDoc, _clienteFontContentMinFooter));
                        itMontoDocumento.Colspan = 2;
                        itMontoDocumento.BorderWidthBottom = 0.75f;
                        itMontoDocumento.BorderWidthLeft = 0;
                        itMontoDocumento.BorderWidthRight = 0.75f;
                        itMontoDocumento.BorderWidthTop = 0;
                        itMontoDocumento.HorizontalAlignment = Element.ALIGN_CENTER;

                        string montoPag = double.Parse(item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid, CultureInfo.InvariantCulture).ToString("#,0.00");
                        //montoPag=CambiarPuntoPorComaDescimales(montoPag);//FEICONT2-364
                        //Cristhian|14/12/2017|FEICONT2-490
                        /*Se agrega un condicional para verificar el tipo de moneda que se esta recibiendo y en base a ello seleccionar el simbolo
                         adecuado para la denonimación del dinero*/
                        /*NUEVO INICIO*/
                        if (item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId == "PEN")
                        {
                            montoPag = "S/." + montoPag;
                        }
                        else if (item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId == "USD")
                        {
                            montoPag = "$." + montoPag;
                        }
                        /*NUEVO FIN*/
                        PdfPCell itMontoPago = new PdfPCell(new Phrase(montoPag, _clienteFontContentMinFooter));
                        itMontoPago.Colspan = 2;
                        itMontoPago.BorderWidthBottom = 0.75f;
                        itMontoPago.BorderWidthLeft = 0;
                        itMontoPago.BorderWidthRight = 0.75f;
                        itMontoPago.BorderWidthTop = 0;
                        itMontoPago.HorizontalAlignment = Element.ALIGN_CENTER;

                        string montoReten = double.Parse(item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount, CultureInfo.InvariantCulture).ToString("#,0.00");
                        //montoReten = CambiarPuntoPorComaDescimales(montoReten);//FEICONT2-364
                        //Cristhian|14/12/2017|FEICONT2-490
                        /*Se agrega un condicional para verificar el tipo de moneda que se esta recibiendo y en base a ello seleccionar el simbolo
                         adecuado para la denonimación del dinero*/
                        /*NUEVO INICIO*/
                        if (item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId == "PEN")
                        {
                            montoReten = "S/." + montoReten;
                        }
                        else if (item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId == "USD")
                        {
                            montoReten = "$." + montoReten;
                        }
                        /*NUEVO FIN*/
                        PdfPCell itRetencion = new PdfPCell(new Phrase(montoReten, _clienteFontContentMinFooter));
                        itRetencion.Colspan = 2;
                        itRetencion.BorderWidthBottom = 0.75f;
                        itRetencion.BorderWidthLeft = 0;
                        itRetencion.BorderWidthRight = 0.75f;
                        itRetencion.BorderWidthTop = 0;
                        itRetencion.HorizontalAlignment = Element.ALIGN_CENTER;

                        tblInfoComprobante.AddCell(itTipo);
                        tblInfoComprobante.AddCell(itSerie);
                        tblInfoComprobante.AddCell(itCorrelativo);
                        tblInfoComprobante.AddCell(itFecha);
                        tblInfoComprobante.AddCell(itFechaPago);
                        tblInfoComprobante.AddCell(itNumeroPago);
                        tblInfoComprobante.AddCell(itMontoDocumento);
                        tblInfoComprobante.AddCell(itMontoPago);
                        tblInfoComprobante.AddCell(itRetencion);

                    }

                    /* ---------- Añadir totales ------------*/
                    PdfPCell itBlancoDos = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    itBlancoDos.Colspan = 2;
                    itBlancoDos.BorderWidthBottom = 0;
                    itBlancoDos.BorderWidthLeft = 0;
                    itBlancoDos.BorderWidthRight = 0;
                    itBlancoDos.BorderWidthTop = 0;
                    itBlancoDos.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell itBlancoUno = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    itBlancoUno.Colspan = 1;
                    itBlancoUno.BorderWidthBottom = 0;
                    itBlancoUno.BorderWidthLeft = 0;
                    itBlancoUno.BorderWidthRight = 0;
                    itBlancoUno.BorderWidthTop = 0;
                    itBlancoUno.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell itBlancoUnoDerecho = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    itBlancoUnoDerecho.Colspan = 1;
                    itBlancoUnoDerecho.BorderWidthBottom = 0;
                    itBlancoUnoDerecho.BorderWidthLeft = 0;
                    itBlancoUnoDerecho.BorderWidthRight = 0.75f;
                    itBlancoUnoDerecho.BorderWidthTop = 0;
                    itBlancoUnoDerecho.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell itLabelTotales = new PdfPCell(new Phrase("TOTALES", _clienteFontContentMinFooter));
                    itLabelTotales.Colspan = 2;
                    itLabelTotales.BorderWidthBottom = 0.75f;
                    itLabelTotales.BorderWidthLeft = 0;
                    itLabelTotales.BorderWidthRight = 0.75f;
                    itLabelTotales.BorderWidthTop = 0;
                    itLabelTotales.HorizontalAlignment = Element.ALIGN_CENTER;


                    string montoTotalPago = double.Parse(SunatTotalPaid, CultureInfo.InvariantCulture).ToString("#,0.00");
                    //montoTotalPago = CambiarPuntoPorComaDescimales(montoTotalPago);//FEICONT2-364
                    PdfPCell itLabelTotalesMontoPago = new PdfPCell(new Phrase(montoTotalPago, _clienteFontContentMinFooter));
                    itLabelTotalesMontoPago.Colspan = 2;
                    itLabelTotalesMontoPago.BorderWidthBottom = 0.75f;
                    itLabelTotalesMontoPago.BorderWidthLeft = 0;
                    itLabelTotalesMontoPago.BorderWidthRight = 0.75f;
                    itLabelTotalesMontoPago.BorderWidthTop = 0;
                    itLabelTotalesMontoPago.HorizontalAlignment = Element.ALIGN_CENTER;

                    string montoTotalRetencion = double.Parse(TotalInvoiceAmount, CultureInfo.InvariantCulture).ToString("#,0.00");
                    //montoTotalRetencion = CambiarPuntoPorComaDescimales(montoTotalRetencion);//FEICONT2-364
                    PdfPCell itLabelTotalesMontoRetenido = new PdfPCell(new Phrase(montoTotalRetencion, _clienteFontContentMinFooter));
                    itLabelTotalesMontoRetenido.Colspan = 2;
                    itLabelTotalesMontoRetenido.BorderWidthBottom = 0.75f;
                    itLabelTotalesMontoRetenido.BorderWidthLeft = 0;
                    itLabelTotalesMontoRetenido.BorderWidthRight = 0.75f;
                    itLabelTotalesMontoRetenido.BorderWidthTop = 0;
                    itLabelTotalesMontoRetenido.HorizontalAlignment = Element.ALIGN_CENTER;

                    tblInfoComprobante.AddCell(itBlancoDos);
                    tblInfoComprobante.AddCell(itBlancoUno);
                    tblInfoComprobante.AddCell(itBlancoDos);
                    tblInfoComprobante.AddCell(itBlancoDos);
                    tblInfoComprobante.AddCell(itBlancoDos);
                    tblInfoComprobante.AddCell(itBlancoUnoDerecho);
                    tblInfoComprobante.AddCell(itLabelTotales);
                    tblInfoComprobante.AddCell(itLabelTotalesMontoPago);
                    tblInfoComprobante.AddCell(itLabelTotalesMontoRetenido);
                    /* ------end items------*/
                    doc.Add(tblInfoComprobante);
                    doc.Add(tblBlanco);
                

                    //FOOTER
                    PdfPTable tblInfoFooter = new PdfPTable(10);
                    tblInfoFooter.WidthPercentage = 100;                 

                    PdfPCell tituComentarios = new PdfPCell(new Phrase("Regimen de Retención: TASA "+ RetentionPercent + "%", _clienteFontBold));
                    tituComentarios.Colspan = 10;
                    tituComentarios.HorizontalAlignment = Element.ALIGN_LEFT;
                    tituComentarios.Border = 0;               

                    tblInfoFooter.AddCell(tituComentarios);

                    NumLetra totalLetras = new NumLetra();
                    string montoTotalRetencionAletras = double.Parse(TotalInvoiceAmount, CultureInfo.InvariantCulture).ToString("0.00");
                    //montoTotalRetencionAletras = CambiarPuntoPorComaDescimales(montoTotalRetencionAletras);//FEICONT2-364
                    PdfPCell tituMontoLetras = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(montoTotalRetencionAletras, true, TotalInvoiceAmountCurrencyID), _clienteFontBold));
                    tituMontoLetras.Colspan = 10;
                    tituMontoLetras.HorizontalAlignment = Element.ALIGN_LEFT;
                    tituMontoLetras.Border = 0;

                    tblInfoFooter.AddCell(tituMontoLetras);          

                    doc.Add(tblInfoFooter);
                    doc.Add(tblBlanco);
                                                   
                    PdfPTable tblFooter = new PdfPTable(10);
                    tblFooter.WidthPercentage = 100;

                    var p = new Paragraph();
                    p.Font = _clienteFontBold;
                    p.Add(digestValue + "\n\n");
                    p.Add("Representación Impresa del Comprobante de Retención Electrónico\n");
                    p.Add(autorizacion_sunat + "\n");

                    PdfPCell DataHash = new PdfPCell(new Phrase(digestValue, _clienteFontBold));
                    DataHash.Border = 0;
                    DataHash.Colspan = 6;
                    DataHash.HorizontalAlignment = Element.ALIGN_CENTER;
                    // DataHash.PaddingTop = 5f;                

                    PdfPCell campo1 = new PdfPCell(p);
                    campo1.Colspan = 6;
                    campo1.Border = 0;
                    campo1.PaddingTop = 20f;
                    campo1.HorizontalAlignment = Element.ALIGN_CENTER;

                    Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                                {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                            };
                    
                    //Cristhian|26/12/2017|FEICONT2-509
                    /*Se corrigio la cadena que se envia al Generador de la imagen QR*/
                    /*INICIO MODIFICACIóN*/
                    var textQR =  " | 20 | " + doc_serie + "|" + doc_correlativo + " |  |  |  |  |  |";
                    /*FIN MODIFICACIóN*/

                    BarcodeQRCode qrcode = new BarcodeQRCode(textQR, 400, 400, ob);
                    iTextSharp.text.Image qrcodeImage = qrcode.GetImage();
                    PdfPCell blanco12 = new PdfPCell();
                    // blanco12.Image = qrcodeImage;
                    blanco12.AddElement(new Chunk(qrcodeImage, 55f, -65f));
                    blanco12.Border = 0;
                    blanco12.PaddingTop = 20f;
                    blanco12.Colspan = 4;

                    PdfPCell blanco121 = new PdfPCell(new Paragraph(" "));
                    blanco121.Border = 0;
                    blanco121.Colspan = 4;

                    tblFooter.AddCell(campo1);
                    tblFooter.AddCell(blanco121);
                    doc.Add(tblFooter);

                    doc.Close();
                    File.SetAttributes(newFile, FileAttributes.Normal);
                    writer.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" generar representacion impresa retencion " + ex.ToString());
                return false;
            }

        }
        #endregion

        //Cristhian|25/09/2017|FEICONT2-364
        /*Se cambia el formato de los decimales de los montos, ejemplo: cambiar de 12,005.53 a 12.005,53
          solo se especifico que afectara a la representacion impresa de Retención*/
        /*NUEVO INICIO*/
        public static string CambiarPuntoPorComaDescimales (string MontoNumerico)
        {
            /*Se separa el texto por punto y luego por coma*/
            string montoPago = MontoNumerico;
            string[] monto = montoPago.Split('.');
            string[] monto1 = monto[0].ToString().Split(',');
            int ContadorDeMontos = 0;
            montoPago = "";

            /*Se rearma la nueva estructura*/
            foreach (var montoIndividual in monto1)
            {
                if (ContadorDeMontos < monto1.Count() - 1)
                {
                    montoPago = montoPago + montoIndividual + ".";
                }
                else
                {
                    montoPago = montoPago + montoIndividual;
                }
                ContadorDeMontos++;
            }

            /*Se junta las cadenas*/
            montoPago = montoPago + "," + monto[1];

            return montoPago;
        } 
        /*NUEVO FIN*/

        public static string[] getByTipo(string type)
        {
            string[] valores= new string[4];
            var tipo = "";
            var imagen = "";
            var nombre = "";
            var texto = "";

            //Cristhian|23/01/2018|FEICONT2-572
            /*Se grega tildes a las palabras que lo nesecitan*/
            /*INICIO MODIFICACIóN*/
            if (type == "01")
            {
                //factura
                tipo = "01";
                imagen = "~/images/logo.png";
                nombre = "FACTURA ELECTRÓNICA";
                texto = "Representación impresa de la Factura Electrónica \n";
            }
            else if (type == "03")
            {   //Boleta
                tipo = "03";
                imagen = "~/images/logo.png";
                nombre = "Boleta de Venta Electrónica";
                texto = "Representación impresa de la Boleta de Venta Electrónica \n";
            }
            else if (type == "07")
            {   //Boleta
                tipo = "07";
                imagen = "~/images/logo.png";
                nombre = "Nota de Crédito Electrónica";
                texto = "Representación impresa de la Nota de Credito Electrónica \n";
            }
            else if (type == "08")
            {   //Boleta
                tipo = "08";
                imagen = "~/images/logo.png";
                nombre = "Nota de Débito Electrónica";
                texto = "Representación impresa de la Nota de Debito Electrónica \n";
            }
            /*FIN MODIFICACIóN*/
            valores[0] = tipo;
            valores[1] = imagen;
            valores[2] = nombre;
            valores[3] = texto;       
            return valores;
        }

        public static System.Globalization.RegionInfo GetCurrencySymbol(string code)
        {
            System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                          where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                          let region = new System.Globalization.RegionInfo(culture.LCID)
                                                          where String.Equals(region.ISOCurrencySymbol, code, StringComparison.InvariantCultureIgnoreCase)
                                                          select region).First();

            return regionInfo;
        }
        public static string getTipoDocIdentidad(string codigo)
        {
            string documento = "";
            switch (codigo)
            {
                case "0":
                    documento = "DOC TRIB NO DOM SIN RUC";
                    break;
                case "1":
                    documento = "DNI";
                    break;
                case "4":
                    documento = "Carnet de Extranjeria";
                    break;
                case "6":
                    documento = "RUC";
                    break;
                case "7":
                    documento = "Pasaporte";
                    break;
                default:
                    documento = "No definido";
                    break;
            }
            return documento;
        }
        public static string getTipoComprobante(string codigo)
        {
            string documento = "";
            switch (codigo)
            {
                case "01":
                    documento = "Factura";
                    break;
                case "03":
                    documento = "Boleta";
                    break;
                case "07":
                    documento = "Nota Credito";
                    break;
                case "08":
                    documento = "Nota Debito";
                    break;             
                default:
                    documento = " ";
                    break;
            }
            return documento;
        }

        public static string getTipoOperacion(string codigo)
        {
            string documento = "";
            switch (codigo)
            {
                case "01":
                    documento = "Venta Interna";
                    break;
                case "02":
                    documento = "Exportación";
                    break;
                case "03":
                    documento = "No Domiciliados";
                    break;
                case "04":
                    documento = "Venta Interna - Anticipos";
                    break;
                case "05":
                    documento = "Venta Itinerante";
                    break;
                case "06":
                    documento = "";
                    break;
            }
            return documento;
        }

        //Cristhian|06/02/2018|FEICONT2-596
        /*Metodo creado para añadir el numero de pagina en la representación impresa,
         el documento pdf tiene que estar previamente creado*/
        /*NUEVO INICIO*/
        public static bool Agregar_Numero_Pagina(string NombreArchivo_Falso, string NombreArchivo_Original)
        {
            bool resultado = false;
            try
            {
                /*Se lee el archivo ya creado con anterioridad, esto para agregar la estampa del número de pagina*/
                byte[] bytes = File.ReadAllBytes(NombreArchivo_Falso);
                /*se define la funete de letra del numero de pagina*/
                Font blackFont = FontFactory.GetFont("HELVETICA", 8, Font.BOLD, BaseColor.BLACK);

                using (MemoryStream stream = new MemoryStream())
                {
                    /*Se lee los bytes recolectados del documento PDF*/
                    PdfReader reader = new PdfReader(bytes);

                    /*Se usa el método stamp del iTexSharp*/
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        /*Se obtiene la cantidad de paginas del documento*/
                        int pages = reader.NumberOfPages;

                        /*Por cada pagina se agrega el nummero correspondiente*/
                        for (int i = 1; i <= pages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("Página "+i.ToString()+" de "+pages.ToString(), blackFont), 590f, 15f, 0);
                        }
                    }
                    bytes = stream.ToArray();
                }
                /*Se vuelve a crear un nuevo documento_con el nombre original que el usuario le da*/
                File.WriteAllBytes(NombreArchivo_Original, bytes);

                /*El Archivo creado previanmente con el nombre falso es borrado*/
                File.Delete(NombreArchivo_Falso);

                /*Si todo concluye sin problemas se envia true*/
                resultado = true;
                return resultado;
            }
            catch (Exception es)
            {
                /*Si el proceso se detiene, entonces se registra el error y se envia false*/
                clsBaseLog.cs_pxRegistarAdd("Error al asignar el número de página (Representación Impresa) " + es.ToString());
                resultado = false;
                return resultado;
            }
        }
        /*NUEVO FIN*/
    }
}
