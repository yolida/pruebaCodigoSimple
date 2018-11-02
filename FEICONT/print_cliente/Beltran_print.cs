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
    public class Beltran_print : RepresentacionImpresa
    {
        /*INICIO -Representación Impresa FEI - Cliente 20502510470 - BELTRAN*/
        public static bool getRepresentacionImpresa_Opcional_02(string pathToSaved, clsEntityDocument cabecera, string textXML, string autorizacion_sunat, string pathImage, clsEntityDatabaseLocal localDB)
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

                    XmlDocument xmlDocument = new XmlDocument();
                    //var textXml = cabecera.Cs_pr_XML;
                    var textXml = textXML;
                    textXml = textXml.Replace("cbc:", "");
                    textXml = textXml.Replace("cac:", "");
                    textXml = textXml.Replace("sac:", "");
                    textXml = textXml.Replace("ext:", "");
                    textXml = textXml.Replace("ds:", "");
                    xmlDocument.LoadXml(textXml);

                    //Cristhian|15/02/2018|FEI2-535
                    /*Datos de Campo 10 y 11- de la parte de Datos Generales*/
                    /*NUEVO INICIO*/
                    string[] ListaDatosAdicionalesImpresion = null;
                    string[] ListaDatosAdicionales_Package = null;
                    try
                    {
                        var ShipmentValue = xmlDocument.GetElementsByTagName("CodigoAsociado")[0].InnerText;
                        /*Se obtienen los siguientes datos: Condicion de pago*Fecha de Vencimiento*Guias*/
                        if (ShipmentValue != null)
                        {
                            ListaDatosAdicionalesImpresion = ShipmentValue.Split('*');
                        }
                    }
                    catch
                    {
                        /*No hacer nada ya que algunos documentos no concideran esta parte- es opcional*/
                    }

                    try
                    {
                        var LegendValue = xmlDocument.GetElementsByTagName("NombreAsociado")[0].InnerText;
                        if (LegendValue != null)
                        {
                            ListaDatosAdicionales_Package = LegendValue.Split('*');
                        }
                    }
                    catch
                    {
                        /*No hacer nada ya que algunos documentos no concideran esta parte- es opcional*/
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
                            //ASPStreetName = stname.Item(0).InnerText;
                            ASPStreetName = "Pasaje A Nro. 45 Mercado Productores Santa Anita  Santa Anita-Lima-Lima\nTelefono: 354 3257";
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
                    PdfPCell fecha = new PdfPCell(new Phrase("Fecha de Emision:", _clienteFontBoldMin));
                    fecha.BorderWidth = 0;
                    fecha.Colspan = 2;

                    var fechaString = dt.ToString("dd") + " de " + dt.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " " + dt.ToString("yyyy");
                    PdfPCell clFecha = new PdfPCell(new Phrase(fechaString.ToUpper(), _clienteFontContentMinFooter));
                    clFecha.BorderWidth = 0;
                    clFecha.Colspan = 8;
                    PdfPCell cliente = new PdfPCell(new Phrase("Señor(es):", _clienteFontBoldMin));
                    cliente.BorderWidth = 0;
                    cliente.Colspan = 2;

                    PdfPCell clNombre = new PdfPCell(new Phrase(ACPRegistrationName, _clienteFontContentMinFooter));
                    clNombre.BorderWidth = 0;
                    clNombre.Colspan = 8;

                    // Añadimos las celdas a la tabla
                    tblInfoCliente.AddCell(fecha);
                    tblInfoCliente.AddCell(clFecha);
                    tblInfoCliente.AddCell(cliente);
                    tblInfoCliente.AddCell(clNombre);

                    PdfPCell direccion = new PdfPCell(new Phrase("Direccion:", _clienteFontBoldMin));
                    direccion.BorderWidth = 0;
                    direccion.Colspan = 2;

                    PdfPCell clDireccion = new PdfPCell(new Phrase(ACPDescription, _clienteFontContentMinFooter));
                    clDireccion.BorderWidth = 0;
                    clDireccion.Colspan = 8;

                    // Añadimos las celdas a la tabla de info cliente


                    var docName = getTipoDocIdentidad(ACPAdditionalAccountID);
                    PdfPCell ruc = new PdfPCell(new Phrase(docName + " N°:", _clienteFontBoldMin));
                    ruc.BorderWidth = 0;
                    ruc.Colspan = 2;

                    PdfPCell clRUC = new PdfPCell(new Phrase(ACPCustomerAssignedAccountID, _clienteFontContentMinFooter));
                    clRUC.BorderWidth = 0;
                    clRUC.Colspan = 8;


                    if (InvoiceTypeCode == "09")
                    {
                        clRUC.Colspan = 2;

                        string TipoDoc_GR = "";
                        string NumeDoc_GR = "";

                        PdfPCell tipodocumento_ = new PdfPCell(new Phrase("Tipo de Doc:", _clienteFontBoldMin));
                        tipodocumento_.BorderWidth = 0;
                        tipodocumento_.Colspan = 1;

                        PdfPCell clTipoDocumento_ = new PdfPCell(new Phrase(TipoDoc_GR, _clienteFontContentMinFooter));
                        clTipoDocumento_.BorderWidth = 0;
                        clTipoDocumento_.Colspan = 2;

                        PdfPCell numerodocumento_ = new PdfPCell(new Phrase("Nro Doc:", _clienteFontBoldMin));
                        numerodocumento_.BorderWidth = 0;
                        numerodocumento_.Colspan = 1;

                        PdfPCell clNumeroDocumento_ = new PdfPCell(new Phrase(NumeDoc_GR, _clienteFontContentMinFooter));
                        clNumeroDocumento_.BorderWidth = 0;
                        clNumeroDocumento_.Colspan = 2;

                        tblInfoCliente.AddCell(direccion);
                        tblInfoCliente.AddCell(clDireccion);
                        tblInfoCliente.AddCell(ruc);
                        tblInfoCliente.AddCell(clRUC);
                        tblInfoCliente.AddCell(tipodocumento_);
                        tblInfoCliente.AddCell(clTipoDocumento_);
                        tblInfoCliente.AddCell(numerodocumento_);
                        tblInfoCliente.AddCell(clNumeroDocumento_);

                    }
                    else
                    {
                        tblInfoCliente.AddCell(direccion);
                        tblInfoCliente.AddCell(clDireccion);
                        tblInfoCliente.AddCell(ruc);
                        tblInfoCliente.AddCell(clRUC);
                    }

                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        //NumLetra monedaLetras1 = new NumLetra();
                        //var monedaLetra_ = monedaLetras1.getMoneda(DocumentCurrencyCode);
                        //PdfPCell moneda_ = new PdfPCell(new Phrase("Moneda:", _clienteFontBoldMin));
                        //moneda_.BorderWidth = 0;
                        //moneda_.Colspan = 2;

                        //PdfPCell clMoneda_ = new PdfPCell(new Phrase(monedaLetra_.ToUpper(), _clienteFontContentMinFooter));
                        //clMoneda_.BorderWidth = 0;
                        //clMoneda_.Colspan = 2;
                        //tblInfoCliente.AddCell(ruc);
                        //tblInfoCliente.AddCell(clRUC);
                        //tblInfoCliente.AddCell(moneda_);
                        //tblInfoCliente.AddCell(clMoneda_);
                    }
                    else
                    {
                        ////NumLetra monedaLetras = new NumLetra();
                        ////  var monedaLetra_ = monedaLetras.getMoneda(cabecera.Cs_tag_DocumentCurrencyCode);
                        //PdfPCell moneda_ = new PdfPCell(new Phrase("Condicion de Venta", _clienteFontBoldMin));
                        //moneda_.BorderWidth = 0;
                        //moneda_.Colspan = 2;

                        //PdfPCell clMoneda_ = new PdfPCell(new Phrase(CondicionVentaXML, _clienteFontContentMinFooter));
                        //clMoneda_.BorderWidth = 0;
                        //clMoneda_.Colspan = 2;
                        //tblInfoCliente.AddCell(ruc);
                        //tblInfoCliente.AddCell(clRUC);
                        //tblInfoCliente.AddCell(moneda_);
                        //tblInfoCliente.AddCell(clMoneda_);

                    }

                    // Añadimos las celdas a la tabla inf

                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {
                        PdfPCell condicionVenta = new PdfPCell(new Phrase("Documento de Referencia:", _clienteFontBoldMin));
                        condicionVenta.BorderWidth = 0;
                        condicionVenta.Colspan = 2;


                        PdfPCell clCondicionVenta = new PdfPCell(new Phrase(DReferenceID, _clienteFontContentMinFooter));
                        clCondicionVenta.BorderWidth = 0;
                        clCondicionVenta.Colspan = 3;

                        tblInfoCliente.AddCell(condicionVenta);
                        tblInfoCliente.AddCell(clCondicionVenta);

                        clsEntityDocument doc_modificado = new clsEntityDocument(localDB);
                        string fechaModificado = doc_modificado.cs_pxBuscarFechaDocumento(DReferenceID);
                        PdfPCell docmodifica = new PdfPCell(new Phrase("Fecha Doc. Ref:", _clienteFontBoldMin));
                        docmodifica.BorderWidth = 0;
                        docmodifica.Colspan = 2;

                        try
                        {
                            if(fechaModificado == "")
                            {
                                fechaModificado = ListaDatosAdicionales_Package[3];
                            }
                        }
                        catch
                        {
                            fechaModificado = "";
                            //No realizar nada
                        }

                        PdfPCell cldocmodifica = new PdfPCell(new Phrase(fechaModificado, _clienteFontContentMinFooter));
                        cldocmodifica.BorderWidth = 0;
                        cldocmodifica.Colspan = 3;

                        tblInfoCliente.AddCell(docmodifica);
                        tblInfoCliente.AddCell(cldocmodifica);
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

                        ///* PdfPCell condicionVenta = new PdfPCell(new Phrase("Condicion Venta:", _clienteFontBoldMin));
                        //    condicionVenta.BorderWidth = 0;
                        //    condicionVenta.Colspan = 2;


                        //    PdfPCell clCondicionVenta = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                        //    clCondicionVenta.BorderWidth = 0;
                        //    clCondicionVenta.Colspan = 2;
                        //    */
                        //tblInfoCliente.AddCell(direccion);
                        //tblInfoCliente.AddCell(clDireccion);
                        //tblInfoCliente.AddCell(moneda);
                        //tblInfoCliente.AddCell(clMoneda);

                    }

                    /*En caso sea nota de credito o debito*/
                    if (InvoiceTypeCode == "07" | InvoiceTypeCode == "08")
                    {

                        PdfPCell motivomodifica = new PdfPCell(new Phrase("Concepto", _clienteFontBoldMin));
                        motivomodifica.BorderWidth = 0;
                        motivomodifica.Colspan = 2;

                        PdfPCell clmotivomodifica = new PdfPCell(new Phrase(DDescription, _clienteFontContentMinFooter));
                        clmotivomodifica.BorderWidth = 0;
                        clmotivomodifica.Colspan = 8;

                        tblInfoCliente.AddCell(motivomodifica);
                        tblInfoCliente.AddCell(clmotivomodifica);


                    }
                    else
                    {
                        //PdfPCell motivomodifica = new PdfPCell(new Phrase(" ", _clienteFontBoldMin));
                        //motivomodifica.BorderWidth = 0;
                        //motivomodifica.Colspan = 1;

                        //PdfPCell clmotivomodifica = new PdfPCell(new Phrase(" ", _clienteFontContentMinFooter));
                        //clmotivomodifica.BorderWidth = 0;
                        //clmotivomodifica.Colspan = 5;


                        //PdfPCell docmodifica = new PdfPCell(new Phrase("Vendedor:", _clienteFontBoldMin));
                        //docmodifica.BorderWidth = 0;
                        //docmodifica.Colspan = 2;

                        //PdfPCell cldocmodifica = new PdfPCell(new Phrase(VendedorXML, _clienteFontContentMinFooter));
                        //cldocmodifica.BorderWidth = 0;
                        //cldocmodifica.Colspan = 2;

                        //tblInfoCliente.AddCell(motivomodifica);
                        //tblInfoCliente.AddCell(clmotivomodifica);
                        //tblInfoCliente.AddCell(docmodifica);
                        //tblInfoCliente.AddCell(cldocmodifica);

                    }
                    /*------------------------------------*/
                    doc.Add(tblInfoCliente);
                    doc.Add(tblBlanco);

                    /*Solo en caso de Boleta y Factura, se agrega una tabla adicional*/

                    /*Datos de la tabla adicional de los comprobantes de factura y boleta*/
                    string CondicionDePago = "-";
                    string FechaVencimineto = "-";
                    string Guias = "-";
                    if (ListaDatosAdicionalesImpresion != null)
                    {
                        CondicionDePago = ListaDatosAdicionalesImpresion[0];
                    }

                    if(ListaDatosAdicionales_Package != null)
                    {
                        if (ListaDatosAdicionales_Package.Length >= 3)
                        {
                            FechaVencimineto = ListaDatosAdicionales_Package[0];
                            Guias = ListaDatosAdicionales_Package[1];
                        }
                    }
                    /*Se cargan los datos adicionales de la representación impresa*/

                    if (InvoiceTypeCode == "01")
                    {
                        PdfPTable tblFormaDePago = new PdfPTable(4);
                        tblFormaDePago.WidthPercentage = 50;
                        tblFormaDePago.HorizontalAlignment = Element.ALIGN_LEFT;

                        PdfPCell condiciondepago = new PdfPCell(new Phrase("Condicion de Pago", _clienteFontBoldMin));
                        condiciondepago.BorderWidth = 0.75f;
                        condiciondepago.Colspan = 1;
                        condiciondepago.HorizontalAlignment = Element.ALIGN_CENTER;
                        condiciondepago.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell fechavencimiento = new PdfPCell(new Phrase("Fecha de Vencimiento", _clienteFontBoldMin));
                        fechavencimiento.BorderWidth = 0.75f;
                        fechavencimiento.Colspan = 1;
                        fechavencimiento.HorizontalAlignment = Element.ALIGN_CENTER;
                        fechavencimiento.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell guias = new PdfPCell(new Phrase("Guias", _clienteFontBoldMin));
                        guias.BorderWidth = 0.75f;
                        guias.Colspan = 2;
                        guias.HorizontalAlignment = Element.ALIGN_CENTER;
                        guias.VerticalAlignment = Element.ALIGN_MIDDLE;

                        tblFormaDePago.AddCell(condiciondepago);
                        tblFormaDePago.AddCell(fechavencimiento);
                        tblFormaDePago.AddCell(guias);

                        PdfPCell clCondiciondepago = new PdfPCell(new Phrase(CondicionDePago, _clienteFontContentMinFooter));
                        clCondiciondepago.BorderWidth = 0.75f;
                        clCondiciondepago.Colspan = 1;
                        clCondiciondepago.HorizontalAlignment = Element.ALIGN_CENTER;
                        clCondiciondepago.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell clFechavencimiento = new PdfPCell(new Phrase(FechaVencimineto, _clienteFontContentMinFooter));
                        clFechavencimiento.BorderWidth = 0.75f;
                        clFechavencimiento.Colspan = 1;
                        clFechavencimiento.HorizontalAlignment = Element.ALIGN_CENTER;
                        clFechavencimiento.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell clGuias = new PdfPCell(new Phrase(Guias, _clienteFontContentMinFooter));
                        clGuias.BorderWidth = 0.75f;
                        clGuias.Colspan = 2;
                        clGuias.HorizontalAlignment = Element.ALIGN_CENTER;
                        clGuias.VerticalAlignment = Element.ALIGN_MIDDLE;

                        tblFormaDePago.AddCell(clCondiciondepago);
                        tblFormaDePago.AddCell(clFechavencimiento);
                        tblFormaDePago.AddCell(clGuias);

                        doc.Add(tblFormaDePago);
                        doc.Add(tblBlanco);
                    }
                    else if (InvoiceTypeCode == "03")
                    {
                        PdfPTable tblFormaDePago = new PdfPTable(2);
                        tblFormaDePago.WidthPercentage = 50;
                        tblFormaDePago.HorizontalAlignment = Element.ALIGN_LEFT;

                        PdfPCell condiciondepago = new PdfPCell(new Phrase("Condicion de Pago", _clienteFontBoldMin));
                        condiciondepago.BorderWidth = 0.75f;
                        condiciondepago.Colspan = 1;
                        condiciondepago.HorizontalAlignment = Element.ALIGN_CENTER;
                        condiciondepago.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell fechavencimiento = new PdfPCell(new Phrase("Fecha de Vencimiento", _clienteFontBoldMin));
                        fechavencimiento.BorderWidth = 0.75f;
                        fechavencimiento.Colspan = 1;
                        fechavencimiento.HorizontalAlignment = Element.ALIGN_CENTER;
                        fechavencimiento.VerticalAlignment = Element.ALIGN_MIDDLE;

                        tblFormaDePago.AddCell(condiciondepago);
                        tblFormaDePago.AddCell(fechavencimiento);

                        PdfPCell clCondiciondepago = new PdfPCell();
                        Phrase p1 = new Phrase();
                        try
                        {
                            p1.Add(CondicionDePago);
                            p1.Font = _clienteFontContentMinFooter;
                        }
                        catch
                        {
                            p1.Add("");
                            p1.Font = _clienteFontContentMinFooter;
                        }
                        clCondiciondepago.Phrase = p1;
                        clCondiciondepago.BorderWidth = 0.75f;
                        clCondiciondepago.Colspan = 1;
                        clCondiciondepago.HorizontalAlignment = Element.ALIGN_CENTER;
                        clCondiciondepago.VerticalAlignment = Element.ALIGN_MIDDLE;

                        PdfPCell clFechavencimiento = new PdfPCell();
                        Phrase p2 = new Phrase();
                        try
                        {
                            p2.Add(FechaVencimineto);
                            p2.Font = _clienteFontContentMinFooter;
                        }
                        catch
                        {
                            p2.Add("");
                            p2.Font = _clienteFontContentMinFooter;
                        }
                        clFechavencimiento.Phrase = p2;
                        clFechavencimiento.BorderWidth = 0.75f;
                        clFechavencimiento.Colspan = 1;
                        clFechavencimiento.HorizontalAlignment = Element.ALIGN_CENTER;
                        clFechavencimiento.VerticalAlignment = Element.ALIGN_MIDDLE;

                        tblFormaDePago.AddCell(clCondiciondepago);
                        tblFormaDePago.AddCell(clFechavencimiento);

                        doc.Add(tblFormaDePago);
                        doc.Add(tblBlanco);
                    }



                    /*Tabla de info de comprobante*/
                    PdfPTable tblInfoComprobante = new PdfPTable(12);
                    tblInfoComprobante.WidthPercentage = 100;


                    // Llenamos la tabla con información
                    //PdfPCell colCodigo = new PdfPCell(new Phrase("Item", _clienteFontBoldMin));
                    //colCodigo.BorderWidth = 0.75f;
                    //colCodigo.Colspan = 1;
                    //colCodigo.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colCantidad = new PdfPCell(new Phrase("CANTIDAD", _clienteFontBoldMin));
                    colCantidad.BorderWidth = 0.75f;
                    colCantidad.Colspan = 1;
                    colCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colUnidadMedida = new PdfPCell(new Phrase("UNIDAD DE MEDIDA", _clienteFontBoldMin));
                    colUnidadMedida.BorderWidth = 0.75f;
                    colUnidadMedida.Colspan = 1;
                    colUnidadMedida.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colDescripcion = new PdfPCell(new Phrase("DESCRIPCION", _clienteFontBoldMin));
                    colDescripcion.BorderWidth = 0.75f;
                    colDescripcion.Colspan = 8;
                    colDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell colPrecUnit = new PdfPCell(new Phrase("PRECIO UNITARIO", _clienteFontBoldMin));
                    colPrecUnit.BorderWidth = 0.75f;
                    colPrecUnit.Colspan = 1;
                    colPrecUnit.HorizontalAlignment = Element.ALIGN_CENTER;

                    //Cristhian|25/10/2017|FEI2-396
                    /*Se agrega una Nueva Columna*/
                    /*NUEVO INICIO*/
                    PdfPCell colDescuentoUnitario = new PdfPCell(new Phrase("Dscto. Unit.", _clienteFontBoldMin));
                    /*Si se encuentra un descuento unitario entonces se modifica la estructura de las columnas*/
                    if (VerificarDescuentoUnitario.Count > 0)
                    {
                        ///*Se cambia el ancho de la columna Descripción, para que la columna de descuentos quepa en el documento*/
                        //colDescripcion.Colspan = 7;

                        ///*Se asigna los valores estandar de la celda (dimensiones y tipo de alineacion del texto)*/
                        //colDescuentoUnitario.BorderWidth = 0.75f;
                        //colDescuentoUnitario.Colspan = 1;
                        //colDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    /*NUEVO FIN*/

                    PdfPCell colImporte = new PdfPCell(new Phrase("TOTAL", _clienteFontBoldMin));
                    colImporte.BorderWidth = 0.75f;
                    colImporte.Colspan = 1;
                    colImporte.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Añadimos las celdas a la tabla - Se incluye el nuevo campo de descuento
                    //tblInfoComprobante.AddCell(colCodigo);
                    tblInfoComprobante.AddCell(colCantidad);
                    tblInfoComprobante.AddCell(colUnidadMedida);
                    tblInfoComprobante.AddCell(colDescripcion);
                    tblInfoComprobante.AddCell(colPrecUnit);
                    //if (VerificarDescuentoUnitario.Count > 0)
                    //{
                    //    tblInfoComprobante.AddCell(colDescuentoUnitario);//Descuento
                    //}
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

                        //Cristhian|25/10/2017|FEI2-396
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
                        string UnidMedida = " ";
                        string[] UM;
                        foreach (var det_it in Lista_items_description)
                        {
                            text_detalle += det_it.Cs_tag_Description + " \n";
                        }
                        UM = text_detalle.Split('\n');
                        UnidMedida = UM[1];
                        text_detalle = UM[0];

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

                        itCantidad.BorderWidthLeft = 0.75f;
                        itCantidad.BorderWidthRight = 0.75f;
                        itCantidad.BorderWidthTop = 0;
                        itCantidad.HorizontalAlignment = Element.ALIGN_CENTER;

                        /*La columna Unidad de Medida parece que es condicional-dependiendo del documento se muestra*/
                        PdfPCell itUnidadMedida = new PdfPCell(new Phrase(UnidMedida, _clienteFontContentMinFooter));
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

                        /*Columna de Descripción - tambien se le da la dimension de la celda*/
                        PdfPCell itDescripcion = new PdfPCell(new Phrase(text_detalle, _clienteFontContentMinFooter));
                        itDescripcion.Colspan = 8;
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

                        //Cristhian|25/10/2017|FEI2-396
                        /*Columna de Descuentos - tambien se le da la dimension de la celda*/
                        /*Se cambia la dimension de la columan descripcion para que entre la columna de descuento unitario*/
                        /*NUEVO INICIO*/
                        PdfPCell itemDescuentoUnitario = new PdfPCell(new Phrase(item.Cs_tag_AllowanceCharge_Amount, _clienteFontContentMinFooter));
                        /*Si se tiene almenos un descuento unitario entra al IF*/
                        if (VerificarDescuentoUnitario.Count > 0)
                        {
                            /*Se dismunuye la dimension de la columna descripción*/
                            //itDescripcion.Colspan = 7;

                            ///*Se asigna el valor de uno a la dimensioin de la nueva columnas*/
                            //itemDescuentoUnitario.Colspan = 1;

                            ///*se agrega el borde superrior, a medida que se llena la grilla (en el PDF)*/
                            //if (numero_item == total_items & op_detraccion == "0.00")
                            //{
                            //    itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                            //}
                            //else
                            //{
                            //    itemDescuentoUnitario.BorderWidthBottom = 0.75f;
                            //}

                            ///*Parametros estandar de presentacion del documento PDF*/
                            //itemDescuentoUnitario.BorderWidthLeft = 0;
                            //itemDescuentoUnitario.BorderWidthRight = 0.75f;
                            //itemDescuentoUnitario.BorderWidthTop = 0;
                            //itemDescuentoUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
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
                        //tblInfoComprobante.AddCell(itCodigo);
                        tblInfoComprobante.AddCell(itCantidad);
                        tblInfoComprobante.AddCell(itUnidadMedida);
                        tblInfoComprobante.AddCell(itDescripcion);
                        tblInfoComprobante.AddCell(itPrecUnit);
                        //if (VerificarDescuentoUnitario.Count > 0)
                        //{
                        //    tblInfoComprobante.AddCell(itemDescuentoUnitario);//Campo Agregado para el Descuento 25/10/2017 FEI2-396
                        //}
                        tblInfoComprobante.AddCell(itImporte);
                    }

                    #region Parte - Operacion de Detraccion
                    /*Se agrega la operacion de Detraccion*/
                    if (op_detraccion != "0.00")
                    {
                        ////agregar mensaje

                        //PdfPCell celda_blanco = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        //celda_blanco.Colspan = 1;
                        //celda_blanco.BorderWidthBottom = 0.75f;
                        //celda_blanco.BorderWidthLeft = 0;
                        //celda_blanco.BorderWidthRight = 0.75f;
                        //celda_blanco.BorderWidthTop = 0;

                        //PdfPCell celda_blanco_dos = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        //celda_blanco_dos.Colspan = 2;
                        //celda_blanco_dos.BorderWidthBottom = 0.75f;
                        //celda_blanco_dos.BorderWidthLeft = 0;
                        //celda_blanco_dos.BorderWidthRight = 0.75f;
                        //celda_blanco_dos.BorderWidthTop = 0;

                        //PdfPCell celda_blanco_right = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        //celda_blanco_right.Colspan = 1;
                        //celda_blanco_right.BorderWidthBottom = 0.75f;
                        //celda_blanco_right.BorderWidthLeft = 0;
                        //celda_blanco_right.BorderWidthRight = 0.75f;
                        //celda_blanco_right.BorderWidthTop = 0;

                        //PdfPCell celda_blanco_left = new PdfPCell(new Phrase(" ", _clienteFontContent));
                        //celda_blanco_left.Colspan = 1;
                        //celda_blanco_left.BorderWidthBottom = 0.75f;
                        //celda_blanco_left.BorderWidthLeft = 0.75f;
                        //celda_blanco_left.BorderWidthRight = 0.75f;
                        //celda_blanco_left.BorderWidthTop = 0;

                        //var parrafo = new Paragraph();
                        //parrafo.Font = _clienteFontContentMinFooter;
                        //parrafo.Add("Operación sujeta al Sistema de Pago de Obligaciones Tributarias con el Gobierno Central \n");
                        //parrafo.Add("SPOT " + porcentaje_detraccion + "% " + cuenta_nacion + " \n");

                        //PdfPCell celda_parrafo = new PdfPCell(parrafo);
                        //celda_parrafo.Colspan = 6;
                        //celda_parrafo.BorderWidthBottom = 0.75f;
                        //celda_parrafo.BorderWidthLeft = 0;
                        //celda_parrafo.BorderWidthRight = 0.75f;
                        //celda_parrafo.BorderWidthTop = 0;
                        //celda_parrafo.PaddingTop = 10f;
                        //celda_parrafo.HorizontalAlignment = Element.ALIGN_CENTER;

                        //tblInfoComprobante.AddCell(celda_blanco_left);
                        //tblInfoComprobante.AddCell(celda_blanco);
                        //tblInfoComprobante.AddCell(celda_blanco_dos);
                        //tblInfoComprobante.AddCell(celda_parrafo);
                        //tblInfoComprobante.AddCell(celda_blanco);
                        //tblInfoComprobante.AddCell(celda_blanco_right);

                    }
                    /* ------end items------*/
                    #endregion

                    doc.Add(tblInfoComprobante);

                    /*INICIO MONTO TOTAL EN LETRAS*/
                    /*Se agrega el monto total en letras-SoloComprobante Beltran*/
                    NumLetra totalLetras = new NumLetra();
                    PdfPTable tblInfoMontoTotal = new PdfPTable(1);

                    tblInfoMontoTotal.WidthPercentage = 100;

                    PdfPCell infoTotal = new PdfPCell(new Phrase("SON: " + totalLetras.Convertir(LMTPayableAmount, true, DocumentCurrencyCode), _clienteFontContent));
                    infoTotal.BorderWidth = 0.75f;
                    infoTotal.Colspan = 1;
                    infoTotal.HorizontalAlignment = Element.ALIGN_LEFT;

                    tblInfoMontoTotal.AddCell(infoTotal);

                    doc.Add(tblInfoMontoTotal);
                    //doc.Add(tblBlanco);
                    /*FIN MONTO TOTAL EN LETRAS*/


                    doc.Add(tblBlanco);


                    //FOOTER
                    PdfPTable tblInfoFooter = new PdfPTable(10);
                    tblInfoFooter.WidthPercentage = 100;

                    //comentarios
                    PdfPTable tblInfoComentarios = new PdfPTable(1);
                    tblInfoComentarios.WidthPercentage = 100;
                    tblInfoComentarios.DefaultCell.BorderWidth = 0;

                    /*CUENTAS BANCARIAS*/
                    PdfPTable tblOrdenCuenta = new PdfPTable(10);
                    tblOrdenCuenta.WidthPercentage = 100;
                    tblOrdenCuenta.DefaultCell.BorderWidth = 0;

                    PdfPCell labelCuentas = new PdfPCell(new Phrase("Cuentas Bancarias:", _clienteFontBoldContentMinFooter));
                    labelCuentas.BorderWidth = 0;
                    labelCuentas.Colspan = 3;
                    labelCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                    var pdat = new Paragraph();
                    pdat.Font = _clienteFontContentMinFooter;
                    pdat.Add(cuentasbancarias);

                    PdfPCell valueCuentas = new PdfPCell(new Phrase("", _clienteFontContentMinFooter));
                    valueCuentas.BorderWidth = 0;
                    valueCuentas.Colspan = 7;
                    valueCuentas.HorizontalAlignment = Element.ALIGN_LEFT;

                    tblOrdenCuenta.AddCell(labelCuentas);
                    tblOrdenCuenta.AddCell(valueCuentas);

                    tblInfoComentarios.AddCell(tblOrdenCuenta);

                    PdfPCell cellBlanco = new PdfPCell(new Phrase("Banco de Credito MN   191-1185-792-0-95\nBbva Continental MN   0011-0183-0100031171-10\nCta. Cte. Detraccion   0072038088", _clienteFontContentMinFooter));
                    cellBlanco.BorderWidth = 0;

                    tblInfoComentarios.AddCell(cellBlanco);

                    //resumen
                    PdfPTable tblInfoResumen = new PdfPTable(4);
                    tblInfoResumen.WidthPercentage = 100;

                    //Cristhian|05/10/2017|FEI2-381
                    /*El sub_total debe ser el monto de op_gravada + */
                    /*MODIFICACION INICIO*/
                    sub_total += double.Parse(op_gravada, CultureInfo.InvariantCulture);
                    sub_total += double.Parse(total_descuentos, CultureInfo.InvariantCulture);
                    /*MODIFICACION FIN*/

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

                    /*Solo se agrega esta sección si es diferente de Boleta*/
                    if (InvoiceTypeCode != "03")
                    {
                        PdfPCell resItem2 = new PdfPCell(new Phrase("Op. Inafecta", _clienteFontBold));
                        resItem2.Colspan = 2;
                        resItem2.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue2 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_inafecta, _clienteFontContent));
                        resvalue2.Colspan = 2;
                        resvalue2.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem2);
                        tblInfoResumen.AddCell(resvalue2);

                        PdfPCell resItem3 = new PdfPCell(new Phrase("Op. Exonerada", _clienteFontBold));
                        resItem3.Colspan = 2;
                        resItem3.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue3 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_exonerada, _clienteFontContent));
                        resvalue3.Colspan = 2;
                        resvalue3.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem3);
                        tblInfoResumen.AddCell(resvalue3);

                        PdfPCell resItem1 = new PdfPCell(new Phrase("Op. Gravada", _clienteFontBold));
                        resItem1.Colspan = 2;
                        resItem1.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue1 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + op_gravada, _clienteFontContent));
                        resvalue1.Colspan = 2;
                        resvalue1.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem1);
                        tblInfoResumen.AddCell(resvalue1);

                        PdfPCell resItem8 = new PdfPCell(new Phrase("Descuentos", _clienteFontBold));
                        resItem8.Colspan = 2;
                        resItem8.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue8 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + total_descuentos, _clienteFontContent));
                        resvalue8.Colspan = 2;
                        resvalue8.HorizontalAlignment = Element.ALIGN_RIGHT;

                        tblInfoResumen.AddCell(resItem8);
                        tblInfoResumen.AddCell(resvalue8);

                    }

                    PdfPCell resItem6 = new PdfPCell(new Phrase("Sub Total", _clienteFontBold));
                    resItem6.Colspan = 2;
                    resItem6.HorizontalAlignment = Element.ALIGN_LEFT;
                    PdfPCell resvalue6 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + output_subtotal, _clienteFontContent));
                    resvalue6.Colspan = 2;
                    resvalue6.HorizontalAlignment = Element.ALIGN_RIGHT;

                    tblInfoResumen.AddCell(resItem6);
                    tblInfoResumen.AddCell(resvalue6);

                    if (imp_IGV != "")
                    {
                        PdfPCell resItem4_1 = new PdfPCell(new Phrase("IGV. 18%", _clienteFontBold));
                        resItem4_1.Colspan = 2;
                        resItem4_1.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell resvalue4_1 = new PdfPCell(new Phrase(monedaDatos.CurrencySymbol + " " + imp_IGV, _clienteFontContent));
                        resvalue4_1.Colspan = 2;
                        resvalue4_1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tblInfoResumen.AddCell(resItem4_1);
                        tblInfoResumen.AddCell(resvalue4_1);
                    }

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

                    //lado izquierdo
                    PdfPCell tblInfoFooterLeft = new PdfPCell(tblInfoComentarios);
                    if (InvoiceTypeCode != "03")
                    {
                        tblInfoFooterLeft.Colspan = 6;
                        tblInfoFooterLeft.PaddingRight = 10f;
                    }
                    else
                    {
                        tblInfoFooterLeft.Colspan = 6;
                        tblInfoFooterLeft.PaddingRight = 10f;
                    }

                    tblInfoFooterLeft.BorderWidth = 0;

                    tblInfoFooter.AddCell(tblInfoFooterLeft);
                    //lado derecho

                    PdfPCell tblInfoFooterRight = new PdfPCell(tblInfoResumen);
                    tblInfoFooterRight.Colspan = 4;
                    tblInfoFooterRight.BorderWidth = 0;
                    tblInfoFooter.AddCell(tblInfoFooterRight);


                    doc.Add(tblInfoFooter);
                    doc.Add(tblBlanco);

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

                    Dictionary<EncodeHintType, object> ob = new Dictionary<EncodeHintType, object>() {
                            {EncodeHintType.ERROR_CORRECTION,ErrorCorrectionLevel.Q }
                        };

                    //Cristhian|26/12/2017|FEI2-509
                    /*Se corrigio la cadena que se envia al Generador de la imagen QR*/
                    /*INICIO MODIFICACIóN*/
                    var textQR = ASPCustomerAssignedAccountID + " | " + InvoiceTypeCode + " | " + doc_serie + "|" + doc_correlativo + " | " + imp_IGV + " | " + LMTPayableAmount + " | " + IssueDate + " | " + ACPAdditionalAccountID + " | " + ACPCustomerAssignedAccountID + " |";
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
        /*FIN -Representación Impresa FEI - Cliente 20502510470 - BELTRAN*/
    }
}
