using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Globalization;
using Extension.Base;

namespace FEICONT.print_cliente
{
    public class Gaitex_print : RepresentacionImpresa
    {
        /*INICIO -Representación Impresa FEI - Cliente 20256459010 - GAITEX*/
        public static bool getRepresentacionImpresa_Opcional_01(string pathToSaved, clsEntityDocument cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal();
            try
            {
                var doc_serie = "";
                var doc_correlativo = "";

                //Cristhian|14/03/2018|FEI2-GAITEX
                /*Se declara estas variables para poner en las celdas que sean opcionales
                  y se quieren mostrar en la representacion Impresa*/
                /*NUEVO INICIO*/
                /*Código ASCCI de espacio "32"*/
                int unicode = 32;
                char character = (char)unicode;
                /*Se optinene el string del codigo ASCCI*/
                string textCeldaBlanco = character.ToString();
                /*NUEVO FIN*/

                if (cabecera != null)
                {

                    string[] partes = cabecera.Cs_tag_ID.Split('-');
                    DateTime dt = DateTime.ParseExact(cabecera.Cs_tag_IssueDate, "yyyy-MM-dd", null);
                    doc_serie = partes[0];
                    doc_correlativo = partes[1];
                    string newFile = pathToSaved;

                    //Cristhian|12/01/2018|FEI2-548
                    /*Se comneta esta parte de codigo que es redundante ya que es una copia
                     del metodo de representaciion impresa*/
                    /*Nuevo Inicio*/
                    //if (File.Exists(newFile))
                    //{
                    //    File.Delete(newFile);
                    //}
                    /*Nuevo Fin*/

                    XmlDocument xmlDocument = new XmlDocument();
                    //var textXml = cabecera.Cs_pr_XML;
                    var textXml = textXML;
                    textXml = textXml.Replace("cbc:", "");
                    textXml = textXml.Replace("cac:", "");
                    textXml = textXml.Replace("sac:", "");
                    textXml = textXml.Replace("ext:", "");
                    textXml = textXml.Replace("ds:", "");
                    xmlDocument.LoadXml(textXml);

                    /*Datos de Campo 10 y 11*/
                    /*NUEVO INICIO*/
                    var ShipmentValue = xmlDocument.GetElementsByTagName("CodigoAsociado")[0].InnerText;
                    string[] ListaDatosAdicionalesImpresion = null;
                    if (ShipmentValue != null)
                    {
                        ListaDatosAdicionalesImpresion = ShipmentValue.Split('*');
                    }

                    var LegendValue = xmlDocument.GetElementsByTagName("NombreAsociado")[0].InnerText;
                    string[] ListaDatosAdicionales_Package = null;
                    if (LegendValue != null)
                    {
                        ListaDatosAdicionales_Package = LegendValue.Split('*');
                    }
                    /*NUEVO FIN*/

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

                    var info_general = getByTipo(InvoiceTypeCode);//Esta Debajo del Hash

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
                    iTextSharp.text.Font _TitleFontN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _TitleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);//
                    iTextSharp.text.Font _HeaderFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);//
                    iTextSharp.text.Font _HeaderFontMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldMin = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContent = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontBoldContentMinFooter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                    /*Fuente de la letra de la tabla nueva creada para el cliente GAITEX*/
                    iTextSharp.text.Font _clienteFontBold_Reduced = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font _clienteFontContent_Reduced = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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

                        //Cristhian|25/10/2017|FEI2-396
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
                    TituRuc.PaddingTop = 5f;
                    TituRuc.PaddingBottom = 5f;

                    PdfPCell TipoDoc = new PdfPCell(new Phrase(info_general[2], _TitleFontN));
                    TipoDoc.BorderWidthLeft = 0.75f;
                    TipoDoc.BorderWidthRight = 0.75f;
                    TipoDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    TipoDoc.PaddingTop = 5f;
                    TipoDoc.PaddingBottom = 5f;

                    /*Para el INVOICE- aparece en el formato del cliente GAITEXT*/
                    PdfPCell TagInvoice = new PdfPCell(new Phrase("COMMERCIAL INVOICE", _TitleFont));
                    TagInvoice.BorderWidthBottom = 0;
                    TagInvoice.BorderWidthRight = 0.75f;
                    TagInvoice.BorderWidthLeft = 0.75f;
                    TagInvoice.BorderWidthTop = 0.75f;
                    TagInvoice.HorizontalAlignment = Element.ALIGN_CENTER;
                    TagInvoice.PaddingTop = 5f;
                    TagInvoice.PaddingBottom = 5f;
                    /*FIN INVOICE*/

                    PdfPCell SerieDoc = new PdfPCell(new Phrase(cabecera.Cs_tag_ID, _TitleFont));
                    SerieDoc.BorderWidthBottom = 0.75f;
                    SerieDoc.BorderWidthRight = 0.75f;
                    SerieDoc.BorderWidthLeft = 0.75f;
                    SerieDoc.BorderWidthTop = 0.75f;
                    SerieDoc.HorizontalAlignment = Element.ALIGN_CENTER;
                    SerieDoc.PaddingTop = 5f;
                    SerieDoc.PaddingBottom = 5f;

                    PdfPCell blanco2 = new PdfPCell(new Paragraph(" "));
                    blanco2.Border = 0;

                    tblInforRuc.AddCell(TituRuc);
                    tblInforRuc.AddCell(TipoDoc);
                    tblInforRuc.AddCell(TagInvoice);
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

                    //doc.Add(tblBlanco);

                    #region Tabla Información Cliente
                    // Tabla Informacion cliente - INICIO
                    PdfPTable tblInfoCliente = new PdfPTable(10);
                    tblInfoCliente.WidthPercentage = 100;

                    // Llenamos la tabla con información del cliente
                    PdfPCell fecha = new PdfPCell(new Phrase("FECHA EMISIÓN / DATE OF ISSUE :", _clienteFontBoldMin));
                    fecha.BorderWidth = 1;
                    fecha.BorderWidthRight = 0;
                    fecha.BorderWidthBottom = 0;
                    fecha.Colspan = 3;

                    var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                    PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                    clFecha.BorderWidth = 1;
                    clFecha.BorderWidthLeft = 0;
                    clFecha.BorderWidthRight = 0;
                    clFecha.BorderWidthBottom = 0;
                    clFecha.Colspan = 3;

                    //NumLetra monedaLetras = new NumLetra();
                    //var monedaLetra = monedaLetras.getMoneda(DocumentCurrencyCode);
                    PdfPCell moneda = new PdfPCell(new Phrase("MONEDA / CURRENCY : ", _clienteFontBoldMin));
                    moneda.BorderWidth = 1;
                    moneda.BorderWidthLeft = 0;
                    moneda.BorderWidthRight = 0;
                    moneda.BorderWidthBottom = 0;
                    moneda.Colspan = 2;

                    PdfPCell clMoneda = new PdfPCell(new Phrase(DocumentCurrencyCode.ToUpper(), _clienteFontContentMinFooter));
                    clMoneda.BorderWidth = 1;
                    clMoneda.BorderWidthBottom = 0;
                    clMoneda.BorderWidthLeft = 0;
                    clMoneda.Colspan = 2;

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(fecha);
                    tblInfoCliente.AddCell(clFecha);
                    tblInfoCliente.AddCell(moneda);
                    tblInfoCliente.AddCell(clMoneda);

                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        PdfPCell condicionVenta = new PdfPCell(new Phrase("Documento que modifica:", _clienteFontBoldMin));
                        condicionVenta.BorderWidth = 0;
                        condicionVenta.Colspan = 2;


                        PdfPCell clCondicionVenta = new PdfPCell(new Phrase(DReferenceID, _clienteFontContentMinFooter));
                        clCondicionVenta.BorderWidth = 0;
                        clCondicionVenta.Colspan = 2;

                        tblInfoCliente.AddCell(condicionVenta);
                        tblInfoCliente.AddCell(clCondicionVenta);
                    }
                    else
                    {
                        //PdfPTable tblDatos = new PdfPTable(10);
                        //tblDatos.WidthPercentage = 100;

                        //NumLetra monedaLetras = new NumLetra();
                        //var monedaLetra = monedaLetras.getMoneda(DocumentCurrencyCode);
                        //PdfPCell moneda = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                        //moneda.BorderWidth = 0;
                        //moneda.Colspan = 2;

                        //PdfPCell clMoneda = new PdfPCell(new Phrase(monedaLetra.ToUpper(), _clienteFontContentMinFooter));
                        //clMoneda.BorderWidth = 0;
                        //clMoneda.Colspan = 2;

                        /* PdfPCell condicionVenta = new PdfPCell(new Phrase("Condicion Venta:", _clienteFontBoldMin));
                         condicionVenta.BorderWidth = 0;
                         condicionVenta.Colspan = 2;


                         PdfPCell clCondicionVenta = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                         clCondicionVenta.BorderWidth = 0;
                         clCondicionVenta.Colspan = 2;
                         */
                        //tblInfoCliente.AddCell(direccion);
                        //tblInfoCliente.AddCell(clDireccion);
                        //tblInfoCliente.AddCell(moneda);
                        //tblInfoCliente.AddCell(clMoneda);

                    }


                    // Añadimos las celdas a la tabla de info cliente


                    //var docName = getTipoDocIdentidad(ACPAdditionalAccountID);
                    //PdfPCell ruc = new PdfPCell(new Phrase(docName + " N°:", _clienteFontBoldMin));
                    //ruc.BorderWidth = 0;
                    //ruc.Colspan = 1;

                    //PdfPCell clRUC = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID, _clienteFontContentMinFooter));
                    //clRUC.BorderWidth = 0;
                    //clRUC.Colspan = 5;
                    //if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    //{
                    //    NumLetra monedaLetras1 = new NumLetra();
                    //    var monedaLetra_ = monedaLetras1.getMoneda(DocumentCurrencyCode);
                    //    PdfPCell moneda_ = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                    //    moneda_.BorderWidth = 0;
                    //    moneda_.Colspan = 2;

                    //    PdfPCell clMoneda_ = new PdfPCell(new Phrase(monedaLetra_.ToUpper(), _clienteFontContentMinFooter));
                    //    clMoneda_.BorderWidth = 0;
                    //    clMoneda_.Colspan = 2;
                    //    tblInfoCliente.AddCell(ruc);
                    //    tblInfoCliente.AddCell(clRUC);
                    //    tblInfoCliente.AddCell(moneda_);
                    //    tblInfoCliente.AddCell(clMoneda_);
                    //}
                    //else
                    //{  //NumLetra monedaLetras = new NumLetra();
                    //   //  var monedaLetra_ = monedaLetras.getMoneda(cabecera.Cs_tag_DocumentCurrencyCode);
                    //    PdfPCell moneda_ = new PdfPCell(new Phrase("Condicion de Venta", _clienteFontBoldMin));
                    //    moneda_.BorderWidth = 0;
                    //    moneda_.Colspan = 2;

                    //    PdfPCell clMoneda_ = new PdfPCell(new Phrase(CondicionVentaXML, _clienteFontContentMinFooter));
                    //    clMoneda_.BorderWidth = 0;
                    //    clMoneda_.Colspan = 2;
                    //    tblInfoCliente.AddCell(ruc);
                    //    tblInfoCliente.AddCell(clRUC);
                    //    tblInfoCliente.AddCell(moneda_);
                    //    tblInfoCliente.AddCell(clMoneda_);

                    //}

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

                        //tblInfoCliente.AddCell(motivomodifica);
                        //tblInfoCliente.AddCell(clmotivomodifica);
                        //tblInfoCliente.AddCell(docmodifica);
                        //tblInfoCliente.AddCell(cldocmodifica);

                    }

                    /*------------------------------------*/
                    doc.Add(tblInfoCliente);
                    //doc.Add(tblBlanco);
                    // Tabla Informacion cliente - FIN
                    #endregion

                    PdfPTable tblDatosGenerales = new PdfPTable(3);
                    tblDatosGenerales.WidthPercentage = 100;

                    PdfPCell clCliente = new PdfPCell(new Phrase("VENDIDO A / SOLD TO : \n", _clienteFontBoldMin));
                    clCliente.BorderWidth = 1;
                    clCliente.BorderWidthBottom = 0;
                    clCliente.Colspan = 1;

                    PdfPCell clTerminospago = new PdfPCell(new Phrase("TERMINOS DE PAGO / TERMS OF PAYMENT : ", _clienteFontBoldMin));
                    clTerminospago.BorderWidth = 1;
                    clTerminospago.BorderWidthBottom = 0;
                    clTerminospago.BorderWidthLeft = 0;
                    clTerminospago.Colspan = 2;

                    tblDatosGenerales.AddCell(clCliente);
                    tblDatosGenerales.AddCell(clTerminospago);

                    PdfPCell clBlanco = new PdfPCell(new Phrase(" ", _clienteFontBoldMin));
                    clBlanco.BorderWidth = 1;
                    clBlanco.BorderWidthBottom = 0;
                    clBlanco.BorderWidthTop = 0;
                    //clBlanco.Colspan = 5;
                    tblDatosGenerales.AddCell(clBlanco);

                    PdfPCell terminospago = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[2].Trim(), _clienteFontContent));/*Terminos de Pago*/
                    terminospago.BorderWidth = 1;
                    terminospago.BorderWidthTop = 0;
                    terminospago.BorderWidthLeft = 0;
                    terminospago.Colspan = 2;
                    tblDatosGenerales.AddCell(terminospago);

                    PdfPCell cliente = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID + "\n " + ACPRegistrationName + "\n " + ACPDescription, _clienteFontContent));/*Sold To/Vendido a*/
                    cliente.BorderWidth = 1;
                    cliente.BorderWidthTop = 0;
                    cliente.Rowspan = 2;
                    tblDatosGenerales.AddCell(cliente);

                    PdfPCell clEmbarque = new PdfPCell(new Phrase("LUGAR DE EMBARQUE / PORT OF SHIPMENT : ", _clienteFontBoldMin));
                    clEmbarque.BorderWidth = 0;
                    //clEmbarque.Colspan = 2;
                    //clEmbarque.Rowspan = 2;
                    tblDatosGenerales.AddCell(clEmbarque);

                    PdfPCell clOrigen = new PdfPCell(new Phrase("ORIGEN / ORIGIN : ", _clienteFontBoldMin));
                    clOrigen.BorderWidth = 0;
                    clOrigen.BorderWidthRight = 1;
                    //clOrigen.Colspan = 2;
                    //clOrigen.Colspan = 2;
                    tblDatosGenerales.AddCell(clOrigen);

                    PdfPCell embarque = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[3].Trim(), _clienteFontContentMinFooter));/*Lugar de Embarque*/
                    embarque.BorderWidth = 0;
                    embarque.BorderWidthBottom = 1;
                    //embarque.Rowspan = 2;
                    //embarque.Colspan = 3;
                    tblDatosGenerales.AddCell(embarque); ;

                    PdfPCell origen = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[4].Trim(), _clienteFontContentMinFooter));/*Lugar de Origen*/
                    origen.BorderWidth = 0;
                    origen.BorderWidthBottom = 1;
                    origen.BorderWidthRight = 1;
                    //origen.Rowspan = 2;
                    //origen.Colspan = 2;
                    tblDatosGenerales.AddCell(origen);


                    PdfPCell clEmbarcadoA = new PdfPCell(new Phrase("EMBARCADO A / SHIP TO : \n", _clienteFontBoldMin));
                    clEmbarcadoA.BorderWidth = 1;
                    clEmbarcadoA.BorderWidthBottom = 0;
                    clEmbarcadoA.BorderWidthTop = 0;
                    clEmbarcadoA.Colspan = 1;
                    tblDatosGenerales.AddCell(clEmbarcadoA);

                    PdfPCell descripcion = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[5].Trim() + " ", _clienteFontContent));
                    descripcion.BorderWidth = 1;
                    descripcion.BorderWidthTop = 0;
                    descripcion.BorderWidthLeft = 0;
                    descripcion.BorderWidthBottom = 0;
                    descripcion.Rowspan = 2;
                    descripcion.Colspan = 1;
                    //descripcion.BorderColor = BaseColor.ORANGE;
                    tblDatosGenerales.AddCell(descripcion);

                    PdfPCell descripcion_2 = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[6].Trim() + " ", _clienteFontContent));
                    descripcion_2.BorderWidth = 1;
                    descripcion_2.BorderWidthTop = 0;
                    descripcion_2.BorderWidthLeft = 0;
                    descripcion_2.BorderWidthBottom = 0;
                    descripcion_2.Rowspan = 2;
                    descripcion_2.Colspan = 1;
                    //descripcion.BorderColor = BaseColor.ORANGE;
                    tblDatosGenerales.AddCell(descripcion_2);

                    PdfPCell embarcadoA = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[0].Trim(), _clienteFontContent));/*Embarcado a / ship to*/
                    embarcadoA.VerticalAlignment = Element.ALIGN_MIDDLE;
                    embarcadoA.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    embarcadoA.BorderWidth = 1;
                    embarcadoA.BorderWidthTop = 0;
                    embarcadoA.Rowspan = 3;
                    embarcadoA.Colspan = 1;
                    //embarcadoA.BorderColor = BaseColor.CYAN;
                    tblDatosGenerales.AddCell(embarcadoA);

                    PdfPCell clAgenteAduana = new PdfPCell(new Phrase("AGENTE ADUANA :", _clienteFontBoldMin));
                    clAgenteAduana.BorderWidth = 1;
                    clAgenteAduana.BorderWidthLeft = 0;
                    clAgenteAduana.BorderWidthBottom = 0;
                    tblDatosGenerales.AddCell(clAgenteAduana);

                    PdfPCell clAgenteCarga = new PdfPCell(new Phrase("AGENTE CARGA :", _clienteFontBoldMin));
                    clAgenteCarga.BorderWidth = 1;
                    clAgenteCarga.BorderWidthBottom = 0;
                    clAgenteCarga.BorderWidthLeft = 0;
                    clAgenteCarga.BorderWidthRight = 1;
                    tblDatosGenerales.AddCell(clAgenteCarga);

                    PdfPCell agenteAduana = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[7].Trim(), _clienteFontContent));/*Agente Aduana*/
                    agenteAduana.BorderWidth = 1;
                    agenteAduana.BorderWidthTop = 0;
                    agenteAduana.BorderWidthLeft = 0;
                    //clEmbarque.Colspan = 2;
                    //clEmbarque.Rowspan = 2;
                    tblDatosGenerales.AddCell(agenteAduana);

                    PdfPCell agenteCarga = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[8].Trim(), _clienteFontContent));/*Agente carga*/
                    agenteCarga.BorderWidth = 1;
                    agenteCarga.BorderWidthTop = 0;
                    agenteCarga.BorderWidthLeft = 0;
                    //clOrigen.Colspan = 2;
                    //clOrigen.Colspan = 2;
                    tblDatosGenerales.AddCell(agenteCarga);

                    PdfPCell clGuiaRemision = new PdfPCell(new Phrase("GUIA DE REMISION :", _clienteFontBoldMin));
                    clGuiaRemision.VerticalAlignment = Element.ALIGN_MIDDLE;
                    clGuiaRemision.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    clGuiaRemision.BorderWidth = 1;
                    clGuiaRemision.BorderWidthTop = 0;
                    clGuiaRemision.BorderWidthBottom = 0;
                    clGuiaRemision.Colspan = 1;
                    //clGuiaRemision.BorderColor = BaseColor.CYAN;
                    tblDatosGenerales.AddCell(clGuiaRemision);

                    PdfPCell descripcion3 = new PdfPCell(new Phrase("INCOTERM: ", _clienteFontBoldMin));
                    descripcion3.BorderWidth = 1;
                    descripcion3.BorderWidthTop = 0;
                    descripcion3.BorderWidthLeft = 0;
                    descripcion3.Colspan = 1;
                    descripcion3.Rowspan = 2;
                    tblDatosGenerales.AddCell(descripcion3);

                    PdfPCell descripcion4 = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[9].Trim(), _clienteFontContent));
                    descripcion4.BorderWidth = 1;
                    descripcion4.BorderWidthTop = 0;
                    descripcion4.BorderWidthLeft = 0;
                    descripcion4.Colspan = 1;
                    descripcion4.Rowspan = 2;
                    tblDatosGenerales.AddCell(descripcion4);

                    PdfPCell guiaRemision = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[1].Trim(), _clienteFontContent));/*Guia de Remisión*/
                    guiaRemision.VerticalAlignment = Element.ALIGN_MIDDLE;
                    guiaRemision.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    guiaRemision.BorderWidth = 1;
                    guiaRemision.BorderWidthTop = 0;
                    guiaRemision.Colspan = 1;
                    //guiaRemision.BorderColor = BaseColor.CYAN;
                    tblDatosGenerales.AddCell(guiaRemision);

                    doc.Add(tblDatosGenerales);
                    doc.Add(tblBlanco);

                    PdfPTable tblDatoExtraCliente = new PdfPTable(2);
                    tblDatoExtraCliente.WidthPercentage = 100;
                    tblDatoExtraCliente.TotalWidth = 540f;
                    tblDatoExtraCliente.LockedWidth = true;
                    float[] widths_DatoExtraCliente = new float[] { 130f, 410f };
                    tblDatoExtraCliente.SetWidths(widths_DatoExtraCliente);

                    PdfPCell cDatoExtra = new PdfPCell(new Phrase("PROGRAMA / DESCRIPCION: ", _clienteFontBoldMin));
                    cDatoExtra.Colspan = 1;

                    PdfPCell datoExtra = new PdfPCell(new Phrase(ListaDatosAdicionalesImpresion[10].Trim(), _clienteFontContent));
                    datoExtra.Colspan = 1;

                    tblDatoExtraCliente.AddCell(cDatoExtra);
                    tblDatoExtraCliente.AddCell(datoExtra);
                    doc.Add(tblDatoExtraCliente);
                    doc.Add(tblBlanco);

                    PdfPTable tblInfoComprobante = new PdfPTable(12);
                    tblInfoComprobante.WidthPercentage = 100;
                    tblInfoComprobante.TotalWidth = 600f;
                    tblInfoComprobante.LockedWidth = true;
                    float[] widths_InfoComprobante = new float[] { 25f, 25f, 50f, 50f, 50f, 150f, 50f, 25f, 25f, 50f, 50f, 50f };
                    tblInfoComprobante.SetWidths(widths_InfoComprobante);


                    // Llenamos la tabla con información
                    PdfPCell colCodigo = new PdfPCell(new Phrase("ITEM", _clienteFontBold_Reduced));
                    colCodigo.BorderWidth = 0.75f;
                    colCodigo.Colspan = 1;
                    colCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPV = new PdfPCell(new Phrase("PROV", _clienteFontBold_Reduced));
                    colPV.BorderWidth = 0.75f;
                    colPV.Colspan = 1;
                    colPV.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPO = new PdfPCell(new Phrase("REFERENCE", _clienteFontBold_Reduced));
                    colPO.BorderWidth = 0.75f;
                    colPO.Colspan = 1;
                    colPO.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colEstilo = new PdfPCell(new Phrase("ESTILO/\nSTYLE", _clienteFontBold_Reduced));
                    colEstilo.BorderWidth = 0.75f;
                    colEstilo.Colspan = 1;
                    colEstilo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colTemporada = new PdfPCell(new Phrase("TEMPORADA/\nSEASON", _clienteFontBold_Reduced));
                    colTemporada.BorderWidth = 0.75f;
                    colTemporada.Colspan = 1;
                    colTemporada.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colDescripcion = new PdfPCell(new Phrase("DESCRIPCION/\nDESCRIPTION", _clienteFontBold_Reduced));
                    colDescripcion.BorderWidth = 0.75f;
                    colDescripcion.Colspan = 1;
                    colDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colColor = new PdfPCell(new Phrase("COLOR", _clienteFontBold_Reduced));
                    colColor.BorderWidth = 0.75f;
                    colColor.Colspan = 1;
                    colColor.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colTalla = new PdfPCell(new Phrase("TALLA/\nSIZE", _clienteFontBold_Reduced));
                    colTalla.BorderWidth = 0.75f;
                    colTalla.Colspan = 1;
                    colTalla.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colUnidad = new PdfPCell(new Phrase("UND", _clienteFontBold_Reduced));
                    colUnidad.BorderWidth = 0.75f;
                    colUnidad.Colspan = 1;
                    colUnidad.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colCantidad = new PdfPCell(new Phrase("CANTIDAD/\nQTY", _clienteFontBold_Reduced));
                    colCantidad.BorderWidth = 0.75f;
                    colCantidad.Colspan = 1;
                    colCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPUnitario = new PdfPCell(new Phrase("P.UNITARIO/\nU.PRICE", _clienteFontBold_Reduced));
                    colPUnitario.BorderWidth = 0.75f;
                    colPUnitario.Colspan = 1;
                    colPUnitario.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colTAmount = new PdfPCell(new Phrase("TOTAL/\nAMOUNT", _clienteFontBold_Reduced));
                    colTAmount.BorderWidth = 0.75f;
                    colTAmount.Colspan = 1;
                    colTAmount.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Añadimos las celdas a la tabla - Se incluye el nuevo campo de descuento
                    tblInfoComprobante.AddCell(colCodigo);
                    tblInfoComprobante.AddCell(colPV);
                    tblInfoComprobante.AddCell(colPO);
                    tblInfoComprobante.AddCell(colEstilo);
                    tblInfoComprobante.AddCell(colTemporada);
                    tblInfoComprobante.AddCell(colDescripcion);
                    tblInfoComprobante.AddCell(colColor);
                    tblInfoComprobante.AddCell(colTalla);
                    tblInfoComprobante.AddCell(colUnidad);
                    tblInfoComprobante.AddCell(colCantidad);
                    tblInfoComprobante.AddCell(colPUnitario);
                    tblInfoComprobante.AddCell(colTAmount);


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

                        //Cristhian|25/10/2017|FEI2-396
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
                        int ContadorColumna = 0;
                        string AdicionalesDelItem = "";

                        foreach (var det_it in Lista_items_description)
                        {
                            if (ContadorColumna == 1)
                            {
                                AdicionalesDelItem = det_it.Cs_tag_Description;
                                ContadorColumna++;
                            }
                            else
                            {
                                text_detalle += det_it.Cs_tag_Description + " \n";
                                ContadorColumna++;
                            }

                        }

                        //Cristhian|14/03/2018|FEI2-GAITEX
                        /*NUEVO INICIO*/
                        /*Se declara una lista para obtener los datos de las columnas nuevas del cliente*/
                        List<string> DatosAdicionalesDelItem = new List<string>();
                        /*Cada item obtenido es adicionado a la lista*/
                        foreach (string datoAdicional in AdicionalesDelItem.Split('*'))
                        {
                            /*Agregando los datos adicionales de la representación impresa*/
                            DatosAdicionalesDelItem.Add(datoAdicional);
                        }

                        /*Por cada dato faltante se añade el valor del ASCCI convertido en texto*/
                        while (DatosAdicionalesDelItem.Count() < 5)
                        {
                            /*Se agrega el dato vacio a la lista*/
                            DatosAdicionalesDelItem.Add(textCeldaBlanco);
                        }
                        /*NUEVO FIN*/

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

                        PdfPCell itPV = new PdfPCell(new Phrase(DatosAdicionalesDelItem[0].ToString(), _clienteFontContentMinFooter));
                        itPV.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itPV.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itPV.BorderWidthBottom = 0.75f;
                        }
                        itPV.BorderWidthLeft = 0;
                        itPV.BorderWidthRight = 0.75f;
                        itPV.BorderWidthTop = 0;
                        itPV.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itReferences = new PdfPCell(new Phrase(DatosAdicionalesDelItem[1], _clienteFontContentMinFooter));
                        itReferences.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itReferences.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itReferences.BorderWidthBottom = 0.75f;
                        }
                        itReferences.BorderWidthLeft = 0;
                        itReferences.BorderWidthRight = 0.75f;
                        itReferences.BorderWidthTop = 0;
                        itReferences.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itStyle = new PdfPCell(new Phrase(item.Cs_tag_Item_SellersItemIdentification, _clienteFontContentMinFooter));
                        itStyle.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itStyle.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itStyle.BorderWidthBottom = 0.75f;
                        }
                        itStyle.BorderWidthLeft = 0;
                        itStyle.BorderWidthRight = 0.75f;
                        itStyle.BorderWidthTop = 0;
                        itStyle.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itSeason = new PdfPCell(new Phrase(DatosAdicionalesDelItem[2], _clienteFontContentMinFooter));
                        itSeason.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itSeason.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itSeason.BorderWidthBottom = 0.75f;
                        }
                        itSeason.BorderWidthLeft = 0;
                        itSeason.BorderWidthRight = 0.75f;
                        itSeason.BorderWidthTop = 0;
                        itSeason.HorizontalAlignment = Element.ALIGN_CENTER;

                        /*Columna de Descripción - tambien se le da la dimension de la celda*/
                        PdfPCell itDescripcion = new PdfPCell(new Phrase(text_detalle, _clienteFontContentMinFooter));
                        itDescripcion.Colspan = 1;
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
                        /*Fin Columna Descripción*/

                        PdfPCell itColor = new PdfPCell(new Phrase(DatosAdicionalesDelItem[3], _clienteFontContentMinFooter));
                        itColor.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itColor.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itColor.BorderWidthBottom = 0.75f;
                        }
                        itColor.BorderWidthLeft = 0;
                        itColor.BorderWidthRight = 0.75f;
                        itColor.BorderWidthTop = 0;
                        itColor.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell itTalla = new PdfPCell(new Phrase(DatosAdicionalesDelItem[4], _clienteFontContentMinFooter));
                        itTalla.Colspan = 1;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itTalla.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itTalla.BorderWidthBottom = 0.75f;
                        }
                        itTalla.BorderWidthLeft = 0;
                        itTalla.BorderWidthRight = 0.75f;
                        itTalla.BorderWidthTop = 0;
                        itTalla.HorizontalAlignment = Element.ALIGN_CENTER;

                        /*La columna Unidad de Medida parece que es condicional-dependiendo del documento se muestra*/
                        PdfPCell itUnidadMedida = new PdfPCell(new Phrase(item.Cs_tag_InvoicedQuantity_unitCode, _clienteFontContentMinFooter));
                        itUnidadMedida.Colspan = 1;
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

                        //Cristhian|25/10/2017|FEI2-396
                        /*Columna de Descuentos - tambien se le da la dimension de la celda*/
                        /*Se cambia la dimension de la columan descripcion para que entre la columna de descuento unitario*/
                        /*NUEVO INICIO*/
                        PdfPCell itemDescuentoUnitario = new PdfPCell(new Phrase(item.Cs_tag_AllowanceCharge_Amount, _clienteFontContentMinFooter));
                        /*Si se tiene almenos un descuento unitario se procede con la definicion del IF*/
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            /*Se desminuye el espacio asignado a la columna Descripción*/
                            itDescripcion.Colspan = 1;

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
                        tblInfoComprobante.AddCell(itPV);//1
                        tblInfoComprobante.AddCell(itReferences);//2
                        tblInfoComprobante.AddCell(itStyle);
                        tblInfoComprobante.AddCell(itSeason);//3
                        tblInfoComprobante.AddCell(itDescripcion);
                        tblInfoComprobante.AddCell(itColor);//4
                        tblInfoComprobante.AddCell(itTalla);//5
                        tblInfoComprobante.AddCell(itUnidadMedida);
                        tblInfoComprobante.AddCell(itCantidad);
                        tblInfoComprobante.AddCell(itPrecUnit);
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            tblInfoComprobante.AddCell(itemDescuentoUnitario);//Campo Agregado para el Descuento 25/10/2017 FEI2-396
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

                    PdfPTable tblPesoProducto = new PdfPTable(8);
                    tblPesoProducto.WidthPercentage = 100;

                    /*Se crea la celda donde esta r el combre de la Celda*/
                    PdfPCell infoTotalPackages = new PdfPCell(new Phrase(" Total / Packages : ", _clienteFontBoldMin));
                    infoTotalPackages.BorderWidthTop = 0.75f;
                    infoTotalPackages.BorderWidthBottom = 0.75f;
                    infoTotalPackages.BorderWidthLeft = 0.75f;
                    infoTotalPackages.BorderWidthRight = 0;
                    infoTotalPackages.Colspan = 1;
                    infoTotalPackages.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell infoTotalPackagesVal = new PdfPCell(new Phrase(ListaDatosAdicionales_Package[0], _clienteFontContent));
                    infoTotalPackagesVal.BorderWidthTop = 0.75f;
                    infoTotalPackagesVal.BorderWidthBottom = 0.75f;
                    infoTotalPackagesVal.BorderWidthRight = 0;
                    infoTotalPackagesVal.BorderWidthLeft = 0;
                    infoTotalPackagesVal.Colspan = 1;
                    infoTotalPackagesVal.HorizontalAlignment = Element.ALIGN_LEFT;

                    /*Se crea la celda donde esta r el combre de la Celda*/
                    PdfPCell infoTotalPesoBruto = new PdfPCell(new Phrase(" Caja de Medidas / Box Measures : ", _clienteFontBoldMin));
                    infoTotalPesoBruto.BorderWidthTop = 0.75f;
                    infoTotalPesoBruto.BorderWidthBottom = 0.75f;
                    infoTotalPesoBruto.BorderWidthLeft = 0;
                    infoTotalPesoBruto.BorderWidthRight = 0;
                    infoTotalPesoBruto.Colspan = 1;
                    infoTotalPesoBruto.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell infoTotalPesoBrutoVal = new PdfPCell(new Phrase(ListaDatosAdicionales_Package[3], _clienteFontContent));
                    infoTotalPesoBrutoVal.BorderWidthTop = 0.75f;
                    infoTotalPesoBrutoVal.BorderWidthBottom = 0.75f;
                    infoTotalPesoBrutoVal.BorderWidthRight = 0;
                    infoTotalPesoBrutoVal.BorderWidthLeft = 0;
                    infoTotalPesoBrutoVal.Colspan = 1;
                    infoTotalPesoBrutoVal.HorizontalAlignment = Element.ALIGN_LEFT;

                    /*Se crea la celda donde esta r el combre de la Celda*/
                    PdfPCell infoTotalPesoNeto = new PdfPCell(new Phrase(" Peso Bruto/ Gross Weight: ", _clienteFontBoldMin));
                    infoTotalPesoNeto.BorderWidthTop = 0.75f;
                    infoTotalPesoNeto.BorderWidthBottom = 0.75f;
                    infoTotalPesoNeto.BorderWidthLeft = 0;
                    infoTotalPesoNeto.BorderWidthRight = 0;
                    infoTotalPesoNeto.Colspan = 1;
                    infoTotalPesoNeto.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell infoTotalPesoNetoVal = new PdfPCell(new Phrase(ListaDatosAdicionales_Package[2], _clienteFontContent));
                    infoTotalPesoNetoVal.BorderWidthTop = 0.75f;
                    infoTotalPesoNetoVal.BorderWidthBottom = 0.75f;
                    infoTotalPesoNetoVal.BorderWidthRight = 0;
                    infoTotalPesoNetoVal.BorderWidthLeft = 0;
                    infoTotalPesoNetoVal.Colspan = 1;
                    infoTotalPesoNetoVal.HorizontalAlignment = Element.ALIGN_LEFT;

                    /*Se crea la celda donde esta r el combre de la Celda*/
                    PdfPCell infoTotalCantidad = new PdfPCell(new Phrase(" Cantidad / Quantity : ", _clienteFontBoldMin));
                    infoTotalCantidad.BorderWidthTop = 0.75f;
                    infoTotalCantidad.BorderWidthBottom = 0.75f;
                    infoTotalCantidad.BorderWidthLeft = 0;
                    infoTotalCantidad.BorderWidthRight = 0;
                    infoTotalCantidad.Colspan = 1;
                    infoTotalCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell infoTotalCantidadVal = new PdfPCell(new Phrase(ListaDatosAdicionales_Package[1], _clienteFontContent));
                    infoTotalCantidadVal.BorderWidthTop = 0.75f;
                    infoTotalCantidadVal.BorderWidthBottom = 0.75f;
                    infoTotalCantidadVal.BorderWidthRight = 0.75f;
                    infoTotalCantidadVal.BorderWidthLeft = 0;
                    infoTotalCantidadVal.Colspan = 1;
                    infoTotalCantidadVal.HorizontalAlignment = Element.ALIGN_LEFT;

                    tblPesoProducto.AddCell(infoTotalPackages);
                    tblPesoProducto.AddCell(infoTotalPackagesVal);
                    tblPesoProducto.AddCell(infoTotalPesoBruto);
                    tblPesoProducto.AddCell(infoTotalPesoBrutoVal);
                    tblPesoProducto.AddCell(infoTotalPesoNeto);
                    tblPesoProducto.AddCell(infoTotalPesoNetoVal);
                    tblPesoProducto.AddCell(infoTotalCantidad);
                    tblPesoProducto.AddCell(infoTotalCantidadVal);

                    doc.Add(tblPesoProducto);
                    doc.Add(tblBlanco);

                    //Cristhian|25/10/2017|FEI2-396
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
                        PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(monedaDatos1.CurrencySymbol + " " + op_gratuita, _clienteFontContent));
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

                            PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir_USA(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                            infoTotal.BorderWidth = 0.75f;
                            infoTotal.Colspan = 7;
                            infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;

                            tblInfoMontoTotal.AddCell(infoTotal);


                            PdfPTable tbl_monto_total1 = new PdfPTable(2);
                            tbl_monto_total1.WidthPercentage = 100;


                            var monedaDatos2 = GetCurrencySymbol(DocumentCurrencyCode);
                            PdfPCell labelMontoTotal1 = new PdfPCell(new Phrase("IMPORTE TOTAL:", _clienteFontBold));
                            labelMontoTotal1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell valueMontoTotal1 = new PdfPCell(new Phrase(monedaDatos2.CurrencySymbol + " " + LMTPayableAmount, _clienteFontContent));
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
                            PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(monedaDatos1.CurrencySymbol + " " + op_gratuita, _clienteFontContent));
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
                            PdfPCell infoTotalOpGratuitasVal = new PdfPCell(new Phrase(monedaDatos1.CurrencySymbol + " " + op_gratuita, _clienteFontContent));
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

                    //Cristhian|05/10/2017|FEI2-381
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
                        PdfPCell infoTotal_USA = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir_USA(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                        infoTotal_USA.BorderWidth = 0.75f;
                        infoTotal_USA.HorizontalAlignment = Element.ALIGN_LEFT;
                        tblInfoMontoTotal.AddCell(infoTotal_USA);
                        doc.Add(tblInfoMontoTotal);
                        /*-------------End Monto Total----------------*/
                        doc.Add(tblBlanco);
                        doc.Add(tblBlanco);
                        doc.Add(tblBlanco);
                    }

                    PdfPTable tblFooter = new PdfPTable(10);
                    tblFooter.WidthPercentage = 100;

                    var p = new Paragraph();
                    p.Font = _clienteFontBold;
                    p.Add(ListaDatosAdicionales_Package[4] + "\n\n"); //Dato Adicional
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

                    Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                                {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                            };

                    //Cristhian|26/12/2017|FEI2-509
                    /*Se corrigio la cadena que se envia al Generador de la imagen QR*/
                    /*INICIO MODIFICACIóN*/
                    var textQR = ASPCustomerAssignedAccountID + " | " + InvoiceTypeCode + " | " + doc_serie + "|" + doc_correlativo + " | " + imp_IGV + " | " + LMTPayableAmount + " | " + IssueDate + " | " + ACPAdditionalAccountID + " | " + ACPCustomerAssignedAccountID + " |";
                    /*INICIO MODIFICACIóN*/
                    BarcodeQRCode qrcode = new BarcodeQRCode(textQR, 400, 400, ob);
                    iTextSharp.text.Image qrcodeImage = qrcode.GetImage();

                    PdfPCell blanco12 = new PdfPCell();
                    blanco12.AddElement(new Chunk(qrcodeImage, 55f, -65f));
                    blanco12.Border = 0;
                    blanco12.PaddingTop = 20f;
                    blanco12.Colspan = 4;


                    PdfPCell blanco121 = new PdfPCell(new Paragraph(" "));
                    blanco121.Border = 0;
                    blanco121.Colspan = 4;

                    tblFooter.AddCell(campo1);
                    tblFooter.AddCell(blanco12);

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
        /*FIN -Representación Impresa FEI - Cliente 20256459010 - GAITEX*/
    }
}
