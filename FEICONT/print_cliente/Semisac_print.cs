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
    class Semisac_print : RepresentacionImpresa
    {
        /*INICIO - Representación Impresa FEI - Cliente 20515292781 - SEMISAC*/
        public static bool getRepresentacionImpresa_Opcional_03(string pathToSaved, clsEntityDocument cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
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

                    //if (File.Exists(newFile))
                    //{
                    //    File.Delete(newFile);
                    //}          

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
                    string CodAfiliacion = " ";
                    try
                    {
                        var ShipmentValue = xmlDocument.GetElementsByTagName("CodigoAsociado")[0].InnerText;
                        if (ShipmentValue != null)
                        {
                            CodAfiliacion = ShipmentValue.ToString();
                        }
                    }
                    catch
                    {

                    }
                    string CorpPack = " ";
                    try
                    {
                        var LegendValue = xmlDocument.GetElementsByTagName("NombreAsociado")[0].InnerText;
                        if (LegendValue != null)
                        {
                            CorpPack = LegendValue.ToString();
                        }
                    }
                    catch
                    {

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
                    //Cristhian|06/02/2018|CC-81
                    /*INICIO MODIFICACIóN*/
                    /*Se agrea un valor numerico para iniciar las variables*/
                    string LMTChargeTotalAmount = "0.00";
                    string LMTPayableAmount = "0.00";
                    /*FIN MODIFICACIóN*/
                    var info_general = getByTipo(InvoiceTypeCode);

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
                    }

                    List<clsEntityDocument_AdditionalComments> Lista_additional_coments = new List<clsEntityDocument_AdditionalComments>();
                    clsEntityDocument_AdditionalComments adittionalComents;
                    XmlNodeList datosCabecera = xmlDocument.GetElementsByTagName("DatosCabecera");
                    foreach (XmlNode dat in datosCabecera)
                    {
                        var NodosHijos = dat.ChildNodes;
                        for (int z = 0; z < NodosHijos.Count; z++)
                        {
                            adittionalComents = new clsEntityDocument_AdditionalComments();
                            adittionalComents.Cs_pr_TagNombre = NodosHijos.Item(z).LocalName;
                            adittionalComents.Cs_pr_TagValor = NodosHijos.Item(z).ChildNodes.Item(0).InnerText;
                            Lista_additional_coments.Add(adittionalComents);
                        }
                    }

                    //comentarios contenido
                    var teclaf8 = " ";//comment1
                    var teclavtrlm = " ";//commnet2
                    var cuentasbancarias = " ";//comment 3
                    string CondicionVentaXML = string.Empty;
                    string CondicionPagoXML = string.Empty;
                    string VendedorXML = string.Empty;
                    foreach (var itemm in Lista_additional_coments)
                    {
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
                    }

                    string sucursal = string.Empty;
                    string[] sucursalpartes = cuentasbancarias.Split('*');
                    if (sucursalpartes.Length > 0)
                    {
                        sucursal = sucursalpartes[0];
                    }

                    //tabla info empresa
                    PdfPTable tblInforEmpresa = new PdfPTable(1);
                    tblInforEmpresa.WidthPercentage = 100;
                    PdfPCell NameEmpresa = new PdfPCell(new Phrase(ASPRegistrationName, _HeaderFont));
                    NameEmpresa.BorderWidth = 0;
                    NameEmpresa.Border = 0;
                    tblInforEmpresa.AddCell(NameEmpresa);

                    var pa = new Paragraph();
                    pa.Font = _clienteFontBoldMin;
                    pa.Add("Dirección: AV. REPUBLICA DE PANAMA NRO. 3418 DPTO. 602 INT. P-6 URB. LIMATAMBO LIMA - LIMA - SAN ISIDRO \n");
                    //pa.Add(sucursal);

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

                    //doc.Add(tblBlanco);

                    //Informacion cliente
                    PdfPTable tblInfoCliente = new PdfPTable(10);
                    tblInfoCliente.WidthPercentage = 100;
                    tblInfoCliente.TotalWidth = 520f;
                    tblInfoCliente.LockedWidth = true;
                    float[] AnchoCelda_tblInfoCliente = new float[] { 50f, 50f, 50f, 50f, 50f, 80f, 10f, 80f, 50f, 50f };
                    tblInfoCliente.SetWidths(AnchoCelda_tblInfoCliente);

                    /* Llenamos la tabla con información del cliente*/
                    PdfPCell cliente = new PdfPCell(new Phrase("Señor(es): ", _clienteFontBoldMin));
                    cliente.BorderWidth = 0.75f;
                    cliente.BorderWidthBottom = 0;
                    cliente.BorderWidthRight = 0;
                    cliente.Colspan = 1;

                    PdfPCell clNombre = new PdfPCell(new Phrase(ACPRegistrationName, _clienteFontContentMinFooter));
                    clNombre.BorderWidth = 0.75f;
                    clNombre.BorderWidthBottom = 0;
                    clNombre.BorderWidthLeft = 0;
                    clNombre.Colspan = 5;

                    PdfPCell clblanco = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    clblanco.BorderWidth = 0;
                    clblanco.Colspan = 4;

                    var docName = getTipoDocIdentidad(ACPAdditionalAccountID);
                    PdfPCell ruc = new PdfPCell(new Phrase(docName.ToString(), _clienteFontBoldMin));
                    ruc.BorderWidth = 0;
                    ruc.BorderWidthLeft = 0.75f;
                    ruc.Colspan = 1;

                    PdfPCell clRUC = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID, _clienteFontContentMinFooter));
                    clRUC.BorderWidth = 0;
                    clRUC.BorderWidthRight = 0.75f;
                    clRUC.Colspan = 5;

                    //PdfPCell direccion = new PdfPCell(new Phrase("Direccion:", _clienteFontBoldMin));
                    //direccion.BorderWidth = 0;
                    //direccion.Colspan = 1;

                    PdfPCell clDireccion = new PdfPCell(new Phrase(ACPDescription, _clienteFontContentMinFooter));
                    clDireccion.BorderWidth = 0.75f;
                    clDireccion.BorderWidthTop = 0;
                    clDireccion.PaddingBottom = 5;
                    clDireccion.Colspan = 6;

                    PdfPCell lblanco = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    lblanco.BorderWidth = 0;
                    lblanco.Colspan = 1;

                    PdfPCell CodPromot = new PdfPCell(new Phrase("COD. PROMOT.", _clienteFontBoldMin));
                    CodPromot.BorderWidth = 0.75f;
                    CodPromot.Colspan = 1;
                    CodPromot.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfPCell Moneda = new PdfPCell(new Phrase("MONEDA", _clienteFontBoldMin));
                    Moneda.BorderWidth = 0.75f;
                    Moneda.Colspan = 1;
                    Moneda.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfPCell fecha = new PdfPCell(new Phrase("FECHA", _clienteFontBoldMin));
                    fecha.BorderWidth = 0.75f;
                    fecha.Colspan = 1;
                    fecha.VerticalAlignment = Element.ALIGN_MIDDLE;

                    PdfPCell Afiliacion = new PdfPCell(new Phrase("AFILIACIÓN: ", _clienteFontBoldMin));
                    Afiliacion.BorderWidth = 0.75f;
                    Afiliacion.PaddingBottom = 5;
                    Afiliacion.Colspan = 2;

                    PdfPCell Afiliacion_Dato1 = new PdfPCell(new Phrase(CodAfiliacion, _clienteFontContentMinFooter));
                    Afiliacion_Dato1.BorderWidth = 0.75f;
                    Afiliacion_Dato1.Colspan = 2;

                    PdfPCell Afiliacion_Dato2 = new PdfPCell(new Phrase(CorpPack, _clienteFontContentMinFooter));
                    Afiliacion_Dato2.BorderWidth = 0.75f;
                    Afiliacion_Dato2.Colspan = 2;

                    var codpromotString = "";
                    PdfPCell clCodPromot = new PdfPCell(new Phrase(codpromotString, _clienteFontContentMinFooter));
                    clCodPromot.BorderWidth = 0.75f;
                    clCodPromot.Colspan = 1;

                    NumLetra monedaLetras = new NumLetra();
                    var monedaLetra = monedaLetras.getMoneda(DocumentCurrencyCode);
                    PdfPCell clMoneda = new PdfPCell(new Phrase(monedaLetra, _clienteFontContentMinFooter));
                    clMoneda.BorderWidth = 0.75f;
                    clMoneda.Colspan = 1;

                    //var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                    var fechaString = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                    clFecha.BorderWidth = 0.75f;
                    clFecha.Colspan = 1;

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(cliente);
                    tblInfoCliente.AddCell(clNombre);
                    tblInfoCliente.AddCell(clblanco);
                    tblInfoCliente.AddCell(ruc);
                    tblInfoCliente.AddCell(clRUC);
                    tblInfoCliente.AddCell(clblanco);
                    //tblInfoCliente.AddCell(direccion);
                    tblInfoCliente.AddCell(clDireccion);
                    tblInfoCliente.AddCell(lblanco);
                    tblInfoCliente.AddCell(CodPromot);
                    tblInfoCliente.AddCell(Moneda);
                    tblInfoCliente.AddCell(fecha);
                    tblInfoCliente.AddCell(Afiliacion);
                    tblInfoCliente.AddCell(Afiliacion_Dato1);
                    tblInfoCliente.AddCell(Afiliacion_Dato2);
                    tblInfoCliente.AddCell(lblanco);
                    tblInfoCliente.AddCell(clCodPromot);
                    tblInfoCliente.AddCell(clMoneda);
                    tblInfoCliente.AddCell(clFecha);


                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        //PdfPCell condicionVenta = new PdfPCell(new Phrase("Documento que modifica:", _clienteFontBoldMin));
                        //condicionVenta.BorderWidth = 0;
                        //condicionVenta.Colspan = 2;


                        //PdfPCell clCondicionVenta = new PdfPCell(new Phrase(DReferenceID, _clienteFontContentMinFooter));
                        //clCondicionVenta.BorderWidth = 0;
                        //clCondicionVenta.Colspan = 2;

                        //tblInfoCliente.AddCell(direccion);
                        //tblInfoCliente.AddCell(clDireccion);
                        //tblInfoCliente.AddCell(condicionVenta);
                        //tblInfoCliente.AddCell(clCondicionVenta);
                    }
                    else
                    {
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

                    doc.Add(tblInfoCliente);
                    doc.Add(tblBlanco);

                    PdfPTable tblInfoComprobante = new PdfPTable(11);
                    tblInfoComprobante.WidthPercentage = 100;


                    // Llenamos la tabla con información
                    //PdfPCell colCodigo = new PdfPCell(new Phrase("Item", _clienteFontBoldMin));
                    //colCodigo.BorderWidthBottom = 0.75f;
                    //colCodigo.BorderWidthLeft = 0.75f;
                    //colCodigo.BorderWidthRight = 0.75f;
                    //colCodigo.BorderWidthTop = 0.75f;
                    //colCodigo.Colspan = 1;
                    //colCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                    /*PdfPCell colUnidadMedida= new PdfPCell(new Phrase("Und Medida", _clienteFontBoldMin));
                    colUnidadMedida.BorderWidth = 0.75f;
                    colUnidadMedida.Colspan = 1;
                    colUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;*/

                    PdfPCell colDescripcion = new PdfPCell(new Phrase("DESCRIPCIÓN", _clienteFontBoldMin));
                    colDescripcion.BorderWidthBottom = 0.75f;
                    colDescripcion.BorderWidthLeft = 0.75f;
                    colDescripcion.BorderWidthRight = 0.75f;
                    colDescripcion.BorderWidthTop = 0.75f;
                    colDescripcion.Colspan = 8;
                    colDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colCantidad = new PdfPCell(new Phrase("CANT.", _clienteFontBoldMin));
                    colCantidad.BorderWidthBottom = 0.75f;
                    colCantidad.BorderWidthLeft = 0;
                    colCantidad.BorderWidthRight = 0.75f;
                    colCantidad.BorderWidthTop = 0.75f;
                    colCantidad.Colspan = 1;
                    colCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPrecUnit = new PdfPCell(new Phrase("UNITARIO", _clienteFontBoldMin));
                    colPrecUnit.BorderWidthBottom = 0.75f;
                    colPrecUnit.BorderWidthLeft = 0;
                    colPrecUnit.BorderWidthRight = 0.75f;
                    colPrecUnit.BorderWidthTop = 0.75f;
                    colPrecUnit.Colspan = 1;
                    colPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colImporte = new PdfPCell(new Phrase("TOTAL", _clienteFontBoldMin));
                    colImporte.BorderWidthBottom = 0.75f;
                    colImporte.BorderWidthLeft = 0;
                    colImporte.BorderWidthRight = 0.75f;
                    colImporte.BorderWidthTop = 0.75f;
                    colImporte.Colspan = 1;
                    colImporte.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Añadimos las celdas a la tabla
                    //tblInfoComprobante.AddCell(colCodigo);
                    tblInfoComprobante.AddCell(colDescripcion);
                    tblInfoComprobante.AddCell(colCantidad);
                    // tblInfoComprobante.AddCell(colUnidadMedida);
                    tblInfoComprobante.AddCell(colPrecUnit);
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
                            taxTotal = new clsEntityDocument_TaxTotal();
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
                            adittionalMonetary = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal();

                            var ss = LIST1.Item(ii);
                            XmlDocument xmlDocumentinner1 = new XmlDocument();
                            xmlDocumentinner1.LoadXml(ss.OuterXml);

                            var id = xmlDocumentinner1.GetElementsByTagName("ID");
                            if (id.Count > 0)
                            {
                                adittionalMonetary.Cs_tag_Id = id.Item(0).InnerText;
                                if (id.Item(0).Attributes.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_SchemeID = id.Item(0).Attributes.GetNamedItem("schemeID").Value;
                                }
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
                            adittionalProperty = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty();

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
                    string op_percepcion = "0.00";
                    string tipo_op = "0";

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
                        else if (ress.Cs_tag_Id == "2001")
                        {
                            op_percepcion = Convert.ToString(ress.Cs_tag_PayableAmount);
                            tipo_op = Convert.ToString(ress.Cs_tag_SchemeID);
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
                        numero_item++;
                        var valor_unitario_item = "";
                        var valor_total_item = "";
                        string condition_price = "";
                        Lista_items = new List<clsEntityDocument_Line>();
                        Lista_items_description = new List<clsEntityDocument_Line_Description>();
                        Lista_items_princingreference = new List<clsEntityDocument_Line_PricingReference>();
                        Lista_items_taxtotal = new List<clsEntityDocument_Line_TaxTotal>();
                        item = new clsEntityDocument_Line();
                        XmlDocument xmlItem = new XmlDocument();
                        xmlItem.LoadXml(dat.OuterXml);

                        XmlNodeList ItemDetail = xmlItem.GetElementsByTagName("Item");
                        if (ItemDetail.Count > 0)
                        {
                            foreach (XmlNode items in ItemDetail)
                            {
                                XmlDocument xmlItemItem = new XmlDocument();
                                xmlItemItem.LoadXml(items.OuterXml);
                                XmlNodeList taxItemIdentification = xmlItemItem.GetElementsByTagName("ID");
                                if (taxItemIdentification.Count > 0)
                                {
                                    item.Cs_tag_Item_SellersItemIdentification = taxItemIdentification.Item(0).InnerText;
                                }
                                XmlNodeList taxItemDescription = xmlItemItem.GetElementsByTagName("Description");
                                int j = 0;
                                foreach (XmlNode description in taxItemDescription)
                                {
                                    j++;
                                    descripcionItem = new clsEntityDocument_Line_Description();
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
                                lines_pricing_reference = new clsEntityDocument_Line_PricingReference();
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
                                taxTotalItem = new clsEntityDocument_Line_TaxTotal();
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
                        var text_detalle_linea_final= "";
                        Phrase Descripcion = new Phrase();
                        Descripcion.SetLeading(0, 2);
                        Descripcion.Font = _clienteFontContentMinFooter;
                        //Descripcion.Add(text_detalle);

                        string[] detalle;
                        foreach (var det_it in Lista_items_description)
                        {
                            if (det_it.Cs_tag_Description.Contains('*'))
                            {
                                detalle = det_it.Cs_tag_Description.Split('*');
                                foreach (var item_Detalle in detalle)
                                {
                                    text_detalle += item_Detalle + "\n \n";
                                }
                            }
                            else
                            {
                                text_detalle_linea_final = det_it.Cs_tag_Description + " \n";
                            }
                        }
                        text_detalle += text_detalle_linea_final;
                        Descripcion.Add(text_detalle);

                        //PdfPCell itCodigo = new PdfPCell(new Phrase(numero_item.ToString(), _clienteFontContentMinFooter));
                        //itCodigo.Colspan = 1;
                        //if (numero_item == total_items & op_detraccion == "0.00")
                        //{
                        //    itCodigo.BorderWidthBottom = 0.75f;

                        //}
                        //else
                        //{
                        //    itCodigo.BorderWidthBottom = 0.75f;
                        //}
                        //itCodigo.BorderWidthLeft = 0.75f;
                        //itCodigo.BorderWidthRight = 0.75f;
                        //itCodigo.BorderWidthTop = 0;
                        //itCodigo.HorizontalAlignment = Element.ALIGN_CENTER;


                        /* PdfPCell itUnidadMedida = new PdfPCell(new Phrase(item.Cs_tag_InvoicedQuantity_unitCode, _clienteFontContentMinFooter));
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
                         itUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;*/


                        PdfPCell itDescripcion = new PdfPCell(Descripcion);
                        itDescripcion.VerticalAlignment = Element.ALIGN_MIDDLE;
                        itDescripcion.Colspan = 8;
                        if (numero_item == total_items & op_detraccion == "0.00")
                        {
                            itDescripcion.BorderWidthBottom = 0.75f;

                        }
                        else
                        {
                            itDescripcion.BorderWidthBottom = 0.75f;
                        }

                        itDescripcion.BorderWidthLeft = 0.75f;
                        itDescripcion.BorderWidthRight = 0.75f;
                        itDescripcion.BorderWidthTop = 0;
                        itDescripcion.PaddingBottom = 5f;
                        itDescripcion.PaddingTop = 5f;
                        itDescripcion.HorizontalAlignment = Element.ALIGN_LEFT;

                        PdfPCell itCantidad = new PdfPCell(new Phrase(item.Cs_tag_invoicedQuantity, _clienteFontContentMinFooter));
                        itCantidad.VerticalAlignment = Element.ALIGN_BOTTOM;
                        itCantidad.PaddingBottom = 5f;
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

                        if (op_gratuita != "0.00")
                        {
                            valor_unitario_item = condition_price;
                        }
                        else
                        {
                            valor_unitario_item = item.Cs_tag_Price_PriceAmount;
                        }

                        PdfPCell itPrecUnit = new PdfPCell(new Phrase(double.Parse(valor_unitario_item, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContentMinFooter));
                        itPrecUnit.Colspan = 1;
                        itPrecUnit.VerticalAlignment = Element.ALIGN_BOTTOM;
                        itPrecUnit.PaddingBottom = 5f;
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


                        if (op_gratuita != "0.00")
                        {
                            if (valor_unitario_item == "")
                            {
                                valor_unitario_item = "0.00";
                            }
                            double valor_total_item_1 = double.Parse(valor_unitario_item, CultureInfo.InvariantCulture) * double.Parse(item.Cs_tag_invoicedQuantity, CultureInfo.InvariantCulture);
                            valor_total_item = valor_total_item_1.ToString();
                        }
                        else
                        {
                            valor_total_item = item.Cs_tag_LineExtensionAmount_currencyID;
                        }

                        PdfPCell itImporte = new PdfPCell(new Phrase(double.Parse(valor_total_item, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContentMinFooter));
                        itImporte.VerticalAlignment = Element.ALIGN_BOTTOM;
                        itImporte.PaddingBottom = 5f;
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

                        //sub_total += Double.Parse(item.Cs_tag_LineExtensionAmount_currencyID);
                        // sub_total += double.Parse(item.Cs_tag_LineExtensionAmount_currencyID, CultureInfo.InvariantCulture);
                        // Añadimos las celdas a la tabla
                        //tblInfoComprobante.AddCell(itCodigo);
                        // tblInfoComprobante.AddCell(itUnidadMedida);
                        tblInfoComprobante.AddCell(itDescripcion);
                        tblInfoComprobante.AddCell(itCantidad);
                        tblInfoComprobante.AddCell(itPrecUnit);
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
                        celda_parrafo.Colspan = 7;
                        celda_parrafo.BorderWidthBottom = 0.75f;
                        celda_parrafo.BorderWidthLeft = 0;
                        celda_parrafo.BorderWidthRight = 0.75f;
                        celda_parrafo.BorderWidthTop = 0;
                        celda_parrafo.PaddingTop = 10f;
                        celda_parrafo.HorizontalAlignment = Element.ALIGN_CENTER;

                        tblInfoComprobante.AddCell(celda_blanco_left);
                        tblInfoComprobante.AddCell(celda_blanco);
                        //tblInfoComprobante.AddCell(celda_blanco);
                        tblInfoComprobante.AddCell(celda_parrafo);
                        tblInfoComprobante.AddCell(celda_blanco);
                        tblInfoComprobante.AddCell(celda_blanco_right);

                    }
                    /* ------end items------*/
                    doc.Add(tblInfoComprobante);
                    //doc.Add(tblBlanco);

                    if (op_gratuita != "0.00")
                    {
                        doc.Add(tblBlanco);
                    }
                    else
                    {
                        doc.Add(tblBlanco);
                    }
                    //}



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

                    /* TABLA PARA NRO ORDEN PEDIDO Y CUENTAS BANCARIAS*/
                    PdfPTable tblOrdenCuenta = new PdfPTable(11);
                    tblOrdenCuenta.WidthPercentage = 100;
                    NumLetra totalLetras = new NumLetra();
                    PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                    infoTotal.Colspan = 11;
                    infoTotal.BorderWidth = 0;
                    infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                    tblOrdenCuenta.AddCell(infoTotal);

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
                    sub_total += double.Parse(op_gravada, CultureInfo.InvariantCulture);

                    if (1 == 1)//InvoiceTypeCode != "03")
                    {
                        // moneda

                        var monedaDatos = GetCurrencySymbol(DocumentCurrencyCode);
                        string SimboloMoneda = monedaDatos.CurrencySymbol;
                        if (SimboloMoneda=="S/."|| SimboloMoneda == "s/.")
                        {
                            SimboloMoneda = SimboloMoneda.Replace(".","");
                        }

                        string output_subtotal = "";


                        if (op_gratuita == "0.00")
                        {
                            output_subtotal = sub_total.ToString("#,0.00", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            output_subtotal = "0.00";
                        }

                        PdfPCell resItem6 = new PdfPCell(new Phrase("Sub Total", _clienteFontBold));
                        resItem6.Colspan = 2;
                        resItem6.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue6 = new PdfPCell(new Phrase(SimboloMoneda + " " + output_subtotal, _clienteFontContent));
                        resvalue6.Colspan = 2;
                        resvalue6.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem6);
                        tblInfoResumen.AddCell(resvalue6);

                        if (imp_IGV != "")
                        {
                            PdfPCell resItem4_1 = new PdfPCell(new Phrase("IGV 18.00 %", _clienteFontBold));
                            resItem4_1.Colspan = 2;
                            resItem4_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue4_1 = new PdfPCell(new Phrase(SimboloMoneda + " " + double.Parse(imp_IGV, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContent));
                            resvalue4_1.Colspan = 2;
                            resvalue4_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem4_1);
                            tblInfoResumen.AddCell(resvalue4_1);
                        }

                        string importeString = "IMPORTE TOTAL:";
                        if (op_percepcion != "0.00")
                        {
                            importeString = "TOTAL:";
                        }
                        else
                        {
                            importeString = "IMPORTE TOTAL:";

                        }

                        PdfPCell resItem5 = new PdfPCell(new Phrase(importeString, _clienteFontBold));
                        resItem5.Colspan = 2;
                        resItem5.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue5 = new PdfPCell(new Phrase(SimboloMoneda + " " + double.Parse(LMTPayableAmount, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContent));
                        resvalue5.Colspan = 2;
                        resvalue5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tblInfoResumen.AddCell(resItem5);
                        tblInfoResumen.AddCell(resvalue5);

                        if (op_percepcion != "0.00")
                        {
                            PdfPCell resItem51 = new PdfPCell(new Phrase("PERCEPCION:", _clienteFontBold));
                            resItem51.Colspan = 2;
                            resItem51.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue51 = new PdfPCell(new Phrase(SimboloMoneda + " " + double.Parse(op_percepcion, CultureInfo.InvariantCulture).ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContent));
                            resvalue51.Colspan = 2;
                            resvalue51.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem51);
                            tblInfoResumen.AddCell(resvalue51);

                            double new_total = Convert.ToDouble(LMTPayableAmount, CultureInfo.CreateSpecificCulture("en-US")) + Convert.ToDouble(op_percepcion, CultureInfo.CreateSpecificCulture("en-US"));

                            PdfPCell resItem52 = new PdfPCell(new Phrase("TOTAL VENTA:", _clienteFontBold));
                            resItem52.Colspan = 2;
                            resItem52.HorizontalAlignment = Element.ALIGN_LEFT;
                            PdfPCell resvalue52 = new PdfPCell(new Phrase(SimboloMoneda + " " + new_total.ToString("#,0.00", CultureInfo.InvariantCulture), _clienteFontContent));
                            resvalue52.Colspan = 2;
                            resvalue52.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tblInfoResumen.AddCell(resItem52);
                            tblInfoResumen.AddCell(resvalue52);
                        }

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
                    //if (InvoiceTypeCode != "03")
                    //{
                    tblInfoFooterLeft.Colspan = 6;
                    tblInfoFooterLeft.PaddingRight = 10f;
                    //}
                    //else
                    //{
                    //    tblInfoFooterLeft.Colspan = 10;
                    //    tblInfoFooterLeft.PaddingRight = 0;
                    //}

                    tblInfoFooterLeft.Border = 0;

                    tblInfoFooter.AddCell(tblInfoFooterLeft);
                    //lado derecho

                    PdfPCell tblInfoFooterRight = new PdfPCell(tblInfoResumen);
                    tblInfoFooterRight.Colspan = 4;
                    tblInfoFooterRight.Border = 0;
                    tblInfoFooter.AddCell(tblInfoFooterRight);


                    doc.Add(tblInfoFooter);
                    doc.Add(tblBlanco);
                    if (InvoiceTypeCode != "03")
                    {
                        ///*----------- Monto total en letras --------------*/
                        //NumLetra totalLetras = new NumLetra();
                        //PdfPTable tblInfoMontoTotal = new PdfPTable(1);
                        //tblInfoMontoTotal.WidthPercentage = 100;
                        //PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                        //infoTotal.BorderWidth = 0.75f;
                        //infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;
                        //tblInfoMontoTotal.AddCell(infoTotal);
                        //doc.Add(tblInfoMontoTotal);
                        ///*-------------End Monto Total----------------*/
                        //doc.Add(tblBlanco);
                    }

                    PdfPTable tblFooter = new PdfPTable(10);
                    tblFooter.WidthPercentage = 100;
                    tblFooter.SpacingBefore = 5;

                    var p = new Paragraph();
                    p.Font = _clienteFontBold;
                    //if (op_percepcion != "0.00")
                    //{
                    //    string tipoOperacion = getTipoOperacion(tipo_op);
                    //    p.Add("Incorporado al regimen de agentes de Percepción de IGV - " + tipoOperacion + " (D.S 091-2013) 01/02/2014 \n\n");
                    //}
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
                    campo1.PaddingTop = 0f;
                    campo1.HorizontalAlignment = Element.ALIGN_CENTER;

                    Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                                {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                            };


                    var textQR = ASPCustomerAssignedAccountID + " | " + InvoiceTypeCode + " | " + doc_serie + " | " + doc_correlativo + " | " + imp_IGV + " | " + LMTPayableAmount + " | " + IssueDate + " | " + ACPAdditionalAccountID + " | " + ACPCustomerAssignedAccountID + " |";

                    BarcodeQRCode qrcode = new BarcodeQRCode(textQR, 400, 400, ob);

                    iTextSharp.text.Image qrcodeImage = qrcode.GetImage();

                    PdfPCell blanco12 = new PdfPCell();
                    blanco12.AddElement(new Chunk(qrcodeImage, 55f, -65f));
                    blanco12.Border = 0;
                    blanco12.PaddingTop = 15f;
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
        /*FIN - Representación Impresa FEI - Cliente 20515292781 - SEMISAC*/
    }
}
