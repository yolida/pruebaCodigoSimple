using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioCEResumenDiario : clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCEResumenDiario(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }

        #region Comentado por Ser de Version XML 1.0
        //public override string cs_pxGenerarXMLAString(string Id)
        //{
        //    string archivo_xml = string.Empty;
        //    try
        //    {
        //        clsEntitySummaryDocuments SummaryDocument = new clsEntitySummaryDocuments(localbd).cs_fxObtenerUnoPorId(Id);
        //        List<clsEntitySummaryDocuments_SummaryDocumentsLine> SummaryDocumentsLine = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraId(Id);

        //        string fila = "";
        //        string ei = "    ";
        //        string ef = "\n";

        //        fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
        //        fila += "<SummaryDocuments" + ef;
        //        #region Cabecera
        //        fila += ei + "xmlns=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SummaryDocuments-1\"" + ef;
        //        fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;
        //        fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
        //        fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
        //        fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
        //        fila += ei + "xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\"" + ef;
        //        fila += ei + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + ef;
        //        fila += ei + "xsi:schemaLocation=\"urn:sunat:names:specification:ubl:peru:schema:xsd:InvoiceSummary-1\">" + ef;
        //        #endregion
        //        fila += ei + "<ext:UBLExtensions>" + ef;
        //        fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>" + ef;
        //        fila += ei + "</ext:UBLExtensions>" + ef;

        //        #region Información de cabecera
        //        fila += ei + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" + ef;
        //        fila += ei + "<cbc:CustomizationID>1.1</cbc:CustomizationID>" + ef;
        //        fila += ei + "<cbc:ID>" + SummaryDocument.Cs_tag_ID + "</cbc:ID>" + ef;
        //        fila += ei + "<cbc:ReferenceDate>" + SummaryDocument.Cs_tag_ReferenceDate + "</cbc:ReferenceDate>" + ef;
        //        if (SummaryDocument.Cs_tag_IssueDate.Trim().Length <= 0)
        //        {
        //            fila += ei + "<cbc:IssueDate>" + DateTime.Now.ToString("yyyy-MM-dd") + "</cbc:IssueDate>" + ef;
        //        }
        //        else
        //        {
        //            fila += ei + "<cbc:IssueDate>" + SummaryDocument.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
        //        }
        //        #endregion

        //        /*Creado para añadir ceros antes del número*/
        //        string StartDocument = string.Empty;
        //        string EndDocument = string.Empty;

        //        #region Referencia de la firma digital
        //        fila += ei + "<cac:Signature>" + ef;
        //        fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
        //        fila += ei + ei + "<cac:SignatoryParty>" + ef;
        //        fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
        //        fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
        //        fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;
        //        //Jordy Amaro 14/11/16 FE-865
        //        //Agregado <!CDATA.
        //        fila += ei + ei + ei + "<cac:PartyName>" + ef;
        //        fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + "RAZONSOCIALDECERTIFICADO" + "]]></cbc:Name>" + ef;
        //        fila += ei + ei + ei + "</cac:PartyName>" + ef;
        //        fila += ei + ei + "</cac:SignatoryParty>" + ef;

        //        fila += ei + ei + "<cac:DigitalSignatureAttachment>" + ef;
        //        fila += ei + ei + ei + "<cac:ExternalReference>" + ef;
        //        fila += ei + ei + ei + ei + "<cbc:URI>#SignatureSP</cbc:URI>" + ef;
        //        fila += ei + ei + ei + "</cac:ExternalReference>" + ef;
        //        fila += ei + ei + "</cac:DigitalSignatureAttachment>" + ef;
        //        fila += ei + "</cac:Signature>" + ef;
        //        #endregion

        //        #region Datos del emisor del documento
        //        fila += ei + "<cac:AccountingSupplierParty>" + ef;
        //        fila += ei + ei + "<cbc:CustomerAssignedAccountID>" + SummaryDocument.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
        //        fila += ei + ei + "<cbc:AdditionalAccountID>" + SummaryDocument.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "</cbc:AdditionalAccountID>" + ef;
        //        fila += ei + ei + "<cac:Party>" + ef;
        //        //Jordy Amaro 08/11/16 FE-851
        //        //Agregado <!CDATA.
        //        if (SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyName_Name.Trim() != "") {

        //            fila += ei + ei + ei + "<cac:PartyName>" + ef;
        //            fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "]]></cbc:Name>" + ef;
        //            fila += ei + ei + ei + "</cac:PartyName>" + ef;

        //        }
               
        //        //Jordy Amaro 08/11/16 FE-851
        //        //Agregado <!CDATA.
        //        fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
        //        fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
        //        fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
        //        fila += ei + ei + "</cac:Party>" + ef;
        //        fila += ei + "</cac:AccountingSupplierParty>" + ef;
        //        #endregion

        //        #region Items del resumen diario
        //        if (SummaryDocumentsLine != null && SummaryDocumentsLine.Count > 0)
        //        {
        //            try
        //            {
        //                int linea = 0;
        //                foreach (var SummaryDocumentsLine_item in SummaryDocumentsLine)
        //                {
        //                    linea++;
        //                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> SummaryDocumentsLine_AllowanceCharge = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
        //                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> SummaryDocumentsLine_BillingPayment = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
        //                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> SummaryDocumentsLine_TaxTotal = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);

        //                    StartDocument = SummaryDocumentsLine_item.Cs_tag_StartDocumentNumberID;
        //                    EndDocument = SummaryDocumentsLine_item.Cs_tag_EndDocumentNumberID;

        //                    while (StartDocument.Length < 8)
        //                    {
        //                        StartDocument = "0" + StartDocument;
        //                    }

        //                    while (EndDocument.Length < 8)
        //                    {
        //                        EndDocument = "0" + EndDocument;
        //                    }

        //                    fila += ei + "<sac:SummaryDocumentsLine>" + ef;
        //                    fila += ei + ei + "<cbc:LineID>" + linea.ToString() + "</cbc:LineID>" + ef;
        //                    fila += ei + ei + "<cbc:DocumentTypeCode>" + SummaryDocumentsLine_item.Cs_tag_DocumentTypeCode + "</cbc:DocumentTypeCode>" + ef;
        //                    fila += ei + ei + "<sac:DocumentSerialID>" + SummaryDocumentsLine_item.Cs_tag_DocumentSerialID + "</sac:DocumentSerialID>" + ef;
        //                    fila += ei + ei + "<sac:StartDocumentNumberID>" + StartDocument + "</sac:StartDocumentNumberID>" + ef;
        //                    fila += ei + ei + "<sac:EndDocumentNumberID>" + EndDocument + "</sac:EndDocumentNumberID>" + ef;

        //                    //Cristhian|24/10/2017|FEI-325
        //                    /*Se agrega una nueva etiqueta, para obtener el estado del Item*/
        //                    /*NUEVO INICIO*/
        //                    fila += ei + ei + "<cac:Status>" + ef;
        //                    fila += ei + ei + ei + "<cbc:ConditionCode>" + SummaryDocumentsLine_item.Cs_tag_ConditionCode + "</cbc:ConditionCode>" + ef;
        //                    fila += ei + ei + "</cac:Status>" + ef;
        //                    /*NUEVO FIN*/

        //                    //Falta agregar el currency ID (MONEDA)
        //                    fila += ei + ei + "<sac:TotalAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_item.Cs_tag_TotalAmount + "</sac:TotalAmount>" + ef;
        //                    if (SummaryDocumentsLine_BillingPayment.Count > 0)
        //                    {
        //                        foreach (var SummaryDocumentsLine_BillingPayment_item in SummaryDocumentsLine_BillingPayment)
        //                        {
        //                            //Cristhian|14/11/2017|FEI2-447
        //                            /*Se agrega una valildación para cantidades menores o iguales a 0.00 */
        //                            /*NUEVO INICIO*/
        //                            /*Si el monto es superior a 0.00, entonces se agrega a su respectiva etiqueta
        //                              caso contrario no se agrega nada y continual con el siguiente monto*/
        //                            if (Convert.ToDouble(SummaryDocumentsLine_BillingPayment_item.Cs_tag_PaidAmount)>0.00)
        //                            {
        //                                fila += ei + ei + ei + "<sac:BillingPayment>" + ef;
        //                                fila += ei + ei + ei + ei + "<cbc:PaidAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_BillingPayment_item.Cs_tag_PaidAmount + "</cbc:PaidAmount>" + ef;
        //                                fila += ei + ei + ei + ei + "<cbc:InstructionID>" + SummaryDocumentsLine_BillingPayment_item.Cs_tag_InstructionID + "</cbc:InstructionID>" + ef;
        //                                fila += ei + ei + ei + "</sac:BillingPayment>" + ef;
        //                            }
        //                            /*NUEVO FIN*/ 
        //                        }
        //                    }

        //                    if (SummaryDocumentsLine_AllowanceCharge.Count > 0)
        //                    {
        //                        foreach (var SummaryDocumentsLine_AllowanceCharge_item in SummaryDocumentsLine_AllowanceCharge)
        //                        {
        //                            //Cristhian|15/11/2017|FEI2-
        //                            /*Se agrega una valildación para cantidades menores o iguales a 0.00 */
        //                            /*NUEVO INICIO*/
        //                            /*Si el monto es superior a 0.00, entonces se agrega a su respectiva etiqueta
        //                              caso contrario no se agrega nada y continual con el siguiente monto*/
        //                            if (Convert.ToDouble(SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_Amount) > 0.00)
        //                            {
        //                                fila += ei + ei + ei + "<cac:AllowanceCharge>" + ef;
        //                                fila += ei + ei + ei + ei + "<cbc:ChargeIndicator>" + SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_ChargeIndicator + "</cbc:ChargeIndicator>" + ef;
        //                                fila += ei + ei + ei + ei + "<cbc:Amount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_Amount + "</cbc:Amount>" + ef;
        //                                fila += ei + ei + ei + "</cac:AllowanceCharge>" + ef;
        //                            }
        //                            /*NUEVO FIN*/
        //                        }
        //                    }

        //                    if (SummaryDocumentsLine_TaxTotal.Count > 0)
        //                    {
        //                        foreach (var SummaryDocumentsLine_TaxTotal_item in SummaryDocumentsLine_TaxTotal)
        //                        {
        //                            fila += ei + ei + ei + "<cac:TaxTotal>" + ef;
        //                            fila += ei + ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxAmount + "</cbc:TaxAmount>" + ef;
        //                            fila += ei + ei + ei + ei + "<cac:TaxSubtotal>" + ef;
        //                            fila += ei + ei + ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxSubtotal_TaxAmount + "</cbc:TaxAmount>" + ef;
        //                            fila += ei + ei + ei + ei + ei + "<cac:TaxCategory>" + ef;
        //                            fila += ei + ei + ei + ei + ei + ei + "<cac:TaxScheme>" + ef;
        //                            fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:ID>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_ID + "</cbc:ID>" + ef;
        //                            fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:Name>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_Name + "</cbc:Name>" + ef;
        //                            fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:TaxTypeCode>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode + "</cbc:TaxTypeCode>" + ef;
        //                            fila += ei + ei + ei + ei + ei + ei + "</cac:TaxScheme>" + ef;
        //                            fila += ei + ei + ei + ei + ei + "</cac:TaxCategory>" + ef;
        //                            fila += ei + ei + ei + ei + "</cac:TaxSubtotal>" + ef;
        //                            fila += ei + ei + ei + "</cac:TaxTotal>" + ef;
        //                        }
        //                    }
        //                    fila += ei + "</sac:SummaryDocumentsLine>" + ef;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Windows.Forms.MessageBox.Show(ex.ToString());
        //            }
        //        }
        //        #endregion

        //        fila += "</SummaryDocuments>" + ef;

        //        string pfxPath = declarante.Cs_pr_Rutacertificadodigital.Replace("\\\\", "\\");
        //        X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), declarante.Cs_pr_Parafrasiscertificadodigital);

        //        //Cristhian|25/08/2017|FEI2-352
        //        /*Se agrega un metodo de busqueda para ubicar la razon social de la empresa 
        //          ya no dependiendo de la ubicacion donde se encuentre, ahora se busca su
        //          etiqueta y se obtine el valor(la razon social)*/
        //        /*NUEVO INICIO*/
        //        string[] subject = cert.SubjectName.Name.Split(',');
        //        foreach (string item in subject)
        //        {
        //            string[] subject_o = item.ToString().Split('=');
        //            if (subject_o[0].Trim() == "O")
        //            {
        //                fila = fila.Replace("RAZONSOCIALDECERTIFICADO", subject_o[1].TrimStart());
        //            }
        //        }
        //        /*NUEVO FIN*/

        //        XmlDocument documento = new XmlDocument();
        //        documento.PreserveWhitespace = false;
        //        documento.LoadXml(fila.Replace("\\\\", "\\"));
        //        archivo_xml = FirmarXml(documento, cert);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.ToString());
        //        clsBaseLog.cs_pxRegistarAdd("clsNegocioCEResumenDiario" + ex.ToString());
        //    }

        //    return archivo_xml;
        //}
        #endregion

        public override string cs_pxGenerarXMLAString(string Id)
        {
            string archivo_xml = string.Empty;
            try
            {
                //Cristhian|04/01/2018|FEI2-325
                /*Agregado solo para acceder al metodo de busqueda del documento relacionado*/
                /*NUEVO INICIO*/
                clsEntityDocument document = new clsEntityDocument(localbd);
                /*NUEVO FIN*/

                clsEntitySummaryDocuments SummaryDocument = new clsEntitySummaryDocuments(localbd).cs_fxObtenerUnoPorId(Id);
                List<clsEntitySummaryDocuments_SummaryDocumentsLine> SummaryDocumentsLine = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraId(Id);

                string fila = "";
                string ei = "    ";
                string ef = "\n";

                fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
                fila += "<SummaryDocuments" + ef;
                #region Cabecera
                fila += ei + "xmlns=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SummaryDocuments-1\"" + ef;
                fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;
                fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
                fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
                fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
                fila += ei + "xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\"" + ef;
                fila += ei + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + ef;
                fila += ei + "xsi:schemaLocation=\"urn:sunat:names:specification:ubl:peru:schema:xsd:InvoiceSummary-1\">" + ef;
                #endregion
                fila += ei + "<ext:UBLExtensions>" + ef;
                fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>" + ef;
                fila += ei + "</ext:UBLExtensions>" + ef;

                #region Información de cabecera
                fila += ei + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" + ef;
                fila += ei + "<cbc:CustomizationID>1.1</cbc:CustomizationID>" + ef;
                fila += ei + "<cbc:ID>" + SummaryDocument.Cs_tag_ID + "</cbc:ID>" + ef;
                fila += ei + "<cbc:ReferenceDate>" + SummaryDocument.Cs_tag_ReferenceDate + "</cbc:ReferenceDate>" + ef;
                if (SummaryDocument.Cs_tag_IssueDate.Trim().Length <= 0)
                {
                    fila += ei + "<cbc:IssueDate>" + DateTime.Now.ToString("yyyy-MM-dd") + "</cbc:IssueDate>" + ef;
                }
                else
                {
                    fila += ei + "<cbc:IssueDate>" + SummaryDocument.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
                }
                #endregion

                /*Creado para añadir ceros antes del número*/
                string StartDocument = string.Empty;
                string EndDocument = string.Empty;

                #region Referencia de la firma digital
                fila += ei + "<cac:Signature>" + ef;
                //fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
                fila += ei + ei + "<cbc:ID>" + SummaryDocument.Cs_tag_ID + "</cbc:ID>" + ef;
                fila += ei + ei + "<cac:SignatoryParty>" + ef;
                fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
                fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;
                //Jordy Amaro 14/11/16 FE-865
                //Agregado <!CDATA.
                fila += ei + ei + ei + "<cac:PartyName>" + ef;
                fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + "RAZONSOCIALDECERTIFICADO" + "]]></cbc:Name>" + ef;
                fila += ei + ei + ei + "</cac:PartyName>" + ef;
                fila += ei + ei + "</cac:SignatoryParty>" + ef;

                fila += ei + ei + "<cac:DigitalSignatureAttachment>" + ef;
                fila += ei + ei + ei + "<cac:ExternalReference>" + ef;
                //fila += ei + ei + ei + ei + "<cbc:URI>#SignatureSP</cbc:URI>" + ef;
                fila += ei + ei + ei + ei + "<cbc:URI>" + SummaryDocument.Cs_tag_ID + "</cbc:URI>" + ef;
                fila += ei + ei + ei + "</cac:ExternalReference>" + ef;
                fila += ei + ei + "</cac:DigitalSignatureAttachment>" + ef;
                fila += ei + "</cac:Signature>" + ef;
                #endregion

                #region Datos del emisor del documento
                fila += ei + "<cac:AccountingSupplierParty>" + ef;
                fila += ei + ei + "<cbc:CustomerAssignedAccountID>" + SummaryDocument.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
                fila += ei + ei + "<cbc:AdditionalAccountID>" + SummaryDocument.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "</cbc:AdditionalAccountID>" + ef;
                fila += ei + ei + "<cac:Party>" + ef;
                //Jordy Amaro 08/11/16 FE-851
                //Agregado <!CDATA.
                //if (SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyName_Name.Trim() != "")
                //{
                //    fila += ei + ei + ei + "<cac:PartyName>" + ef;
                //    fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "]]></cbc:Name>" + ef;
                //    fila += ei + ei + ei + "</cac:PartyName>" + ef;
                //}

                //Jordy Amaro 08/11/16 FE-851
                //Agregado <!CDATA.
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + SummaryDocument.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
                fila += ei + "</cac:AccountingSupplierParty>" + ef;
                #endregion

                #region Items del resumen diario
                if (SummaryDocumentsLine != null && SummaryDocumentsLine.Count > 0)
                {
                    try
                    {
                        int linea = 0;
                        foreach (var SummaryDocumentsLine_item in SummaryDocumentsLine)
                        {
                            linea++;
                            List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> SummaryDocumentsLine_AllowanceCharge = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                            List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> SummaryDocumentsLine_BillingPayment = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                            List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> SummaryDocumentsLine_TaxTotal = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerTodoPorCabeceraId(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);

                            StartDocument = SummaryDocumentsLine_item.Cs_tag_StartDocumentNumberID;
                            EndDocument = SummaryDocumentsLine_item.Cs_tag_EndDocumentNumberID;

                            while (StartDocument.Length < 8)
                            {
                                StartDocument = "0" + StartDocument;
                            }

                            fila += ei + "<sac:SummaryDocumentsLine>" + ef;
                            fila += ei + ei + "<cbc:LineID>" + linea.ToString() + "</cbc:LineID>" + ef;
                            fila += ei + ei + "<cbc:DocumentTypeCode>" + SummaryDocumentsLine_item.Cs_tag_DocumentTypeCode + "</cbc:DocumentTypeCode>" + ef;
                            fila += ei + ei + "<cbc:ID>" + SummaryDocumentsLine_item.Cs_tag_DocumentSerialID+"-"+StartDocument + "</cbc:ID>" + ef;

                            /*Agregado para obtener el ID del Cliente del Cliente*/
                            List<string> Customer_Asociado = new List<string>(document.cs_Buscar_ClienteAsociado(SummaryDocumentsLine_item.Cs_tag_DocumentSerialID + "-" + StartDocument));
                            fila += ei + ei + "<cac:AccountingCustomerParty>" + ef;
                            fila += ei + ei + ei + "<cbc:CustomerAssignedAccountID>" + Customer_Asociado[0].ToString().Trim() + "</cbc:CustomerAssignedAccountID>" + ef;
                            fila += ei + ei + ei + "<cbc:AdditionalAccountID>" + Customer_Asociado[1].ToString().Trim() + "</cbc:AdditionalAccountID>" + ef;
                            fila += ei + ei + "</cac:AccountingCustomerParty>" + ef;

                            //Cristhian|04/01/2018|FEI2-325
                            /*Unico campo agregado para las Notas de Credito o Debito */
                            //Cristhian|31/01/2018|FEI2-601
                            /*En la consulta para obtener el documento (Boleta Relacionado), ahora se envia el Id el Resumen Diario*/
                            /*INICIO MODIFICACIóN*/
                            /*Nuevo solo valido para la nota de credito asociada a una boleta*/
                            List<string> ComprobanteAsociado = new List<string>(document.cs_Buscar_ComprobanteAsociado_NotaCreditoDebito(SummaryDocumentsLine_item.Cs_pr_SummaryDocuments_Id,SummaryDocumentsLine_item.Cs_tag_DocumentSerialID + "-" + StartDocument));
                            if (ComprobanteAsociado!=null && ComprobanteAsociado.Count!=0 && SummaryDocumentsLine_item.Cs_tag_DocumentTypeCode != "03")
                            {
                                fila += ei + ei + "<cac:BillingReference>" + ef;
                                fila += ei + ei + ei + "<cac:InvoiceDocumentReference>" + ef;
                                fila += ei + ei + ei + ei + "<cbc:ID>" + ComprobanteAsociado[0] + "</cbc:ID>" + ef;
                                fila += ei + ei + ei + ei + "<cbc:DocumentTypeCode>" + ComprobanteAsociado[1] + "</cbc:DocumentTypeCode>" + ef;
                                fila += ei + ei + ei + "</cac:InvoiceDocumentReference>" + ef;
                                fila += ei + ei + "</cac:BillingReference>" + ef;
                            }
                            /*FIN MODIFICACIóN*/
                            
                            //Cristhian|24/10/2017|FEI-325
                            /*Se agrega una nueva etiqueta, para obtener el estado del Item*/
                            /*NUEVO INICIO*/
                            fila += ei + ei + "<cac:Status>" + ef;
                            fila += ei + ei + ei + "<cbc:ConditionCode>" + SummaryDocumentsLine_item.Cs_tag_ConditionCode + "</cbc:ConditionCode>" + ef;
                            fila += ei + ei + "</cac:Status>" + ef;
                            /*NUEVO FIN*/

                            //Falta agregar el currency ID (MONEDA)
                            fila += ei + ei + "<sac:TotalAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_item.Cs_tag_TotalAmount + "</sac:TotalAmount>" + ef;
                            if (SummaryDocumentsLine_BillingPayment.Count > 0)
                            {
                                foreach (var SummaryDocumentsLine_BillingPayment_item in SummaryDocumentsLine_BillingPayment)
                                {
                                    //Cristhian|14/11/2017|FEI2-447
                                    /*Se agrega una valildación para cantidades menores o iguales a 0.00 */
                                    /*NUEVO INICIO*/
                                    /*Si el monto es superior a 0.00, entonces se agrega a su respectiva etiqueta
                                      caso contrario no se agrega nada y continual con el siguiente monto*/
                                    if (Convert.ToDouble(SummaryDocumentsLine_BillingPayment_item.Cs_tag_PaidAmount) > 0.00)
                                    {
                                        fila += ei + ei + ei + "<sac:BillingPayment>" + ef;
                                        fila += ei + ei + ei + ei + "<cbc:PaidAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_BillingPayment_item.Cs_tag_PaidAmount + "</cbc:PaidAmount>" + ef;
                                        fila += ei + ei + ei + ei + "<cbc:InstructionID>" + SummaryDocumentsLine_BillingPayment_item.Cs_tag_InstructionID + "</cbc:InstructionID>" + ef;
                                        fila += ei + ei + ei + "</sac:BillingPayment>" + ef;
                                    }
                                    /*NUEVO FIN*/
                                }
                            }

                            if (SummaryDocumentsLine_AllowanceCharge.Count > 0)
                            {
                                foreach (var SummaryDocumentsLine_AllowanceCharge_item in SummaryDocumentsLine_AllowanceCharge)
                                {
                                    //Cristhian|15/11/2017|FEI2-
                                    /*Se agrega una valildación para cantidades menores o iguales a 0.00 */
                                    /*NUEVO INICIO*/
                                    /*Si el monto es superior a 0.00, entonces se agrega a su respectiva etiqueta
                                      caso contrario no se agrega nada y continual con el siguiente monto*/
                                    if (Convert.ToDouble(SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_Amount) > 0.00)
                                    {
                                        fila += ei + ei + ei + "<cac:AllowanceCharge>" + ef;
                                        fila += ei + ei + ei + ei + "<cbc:ChargeIndicator>" + SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_ChargeIndicator + "</cbc:ChargeIndicator>" + ef;
                                        fila += ei + ei + ei + ei + "<cbc:Amount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_AllowanceCharge_item.Cs_tag_Amount + "</cbc:Amount>" + ef;
                                        fila += ei + ei + ei + "</cac:AllowanceCharge>" + ef;
                                    }
                                    /*NUEVO FIN*/
                                }
                            }

                            if (SummaryDocumentsLine_TaxTotal.Count > 0)
                            {
                                foreach (var SummaryDocumentsLine_TaxTotal_item in SummaryDocumentsLine_TaxTotal)
                                {
                                    fila += ei + ei + ei + "<cac:TaxTotal>" + ef;
                                    fila += ei + ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxAmount + "</cbc:TaxAmount>" + ef;
                                    fila += ei + ei + ei + ei + "<cac:TaxSubtotal>" + ef;
                                    fila += ei + ei + ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + SummaryDocument.Cs_tag_DocumentCurrencyCode + "\">" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxSubtotal_TaxAmount + "</cbc:TaxAmount>" + ef;
                                    fila += ei + ei + ei + ei + ei + "<cac:TaxCategory>" + ef;
                                    fila += ei + ei + ei + ei + ei + ei + "<cac:TaxScheme>" + ef;
                                    fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:ID>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_ID + "</cbc:ID>" + ef;
                                    fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:Name>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_Name + "</cbc:Name>" + ef;
                                    fila += ei + ei + ei + ei + ei + ei + ei + "<cbc:TaxTypeCode>" + SummaryDocumentsLine_TaxTotal_item.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode + "</cbc:TaxTypeCode>" + ef;
                                    fila += ei + ei + ei + ei + ei + ei + "</cac:TaxScheme>" + ef;
                                    fila += ei + ei + ei + ei + ei + "</cac:TaxCategory>" + ef;
                                    fila += ei + ei + ei + ei + "</cac:TaxSubtotal>" + ef;
                                    fila += ei + ei + ei + "</cac:TaxTotal>" + ef;
                                }
                            }
                            fila += ei + "</sac:SummaryDocumentsLine>" + ef;
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Windows.Forms.MessageBox.Show(ex.ToString());
                    }
                }
                #endregion

                fila += "</SummaryDocuments>" + ef;

                string pfxPath = declarante.Cs_pr_Rutacertificadodigital.Replace("\\\\", "\\");
                X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), declarante.Cs_pr_Parafrasiscertificadodigital);

                //Cristhian|25/08/2017|FEI2-352
                /*Se agrega un metodo de busqueda para ubicar la razon social de la empresa 
                  ya no dependiendo de la ubicacion donde se encuentre, ahora se busca su
                  etiqueta y se obtine el valor(la razon social)*/
                /*NUEVO INICIO*/
                string[] subject = cert.SubjectName.Name.Split(',');
                foreach (string item in subject)
                {
                    string[] subject_o = item.ToString().Split('=');
                    if (subject_o[0].Trim() == "O")
                    {
                        fila = fila.Replace("RAZONSOCIALDECERTIFICADO", subject_o[1].TrimStart());
                    }
                }
                /*NUEVO FIN*/

                XmlDocument documento = new XmlDocument();
                documento.PreserveWhitespace = false;
                documento.LoadXml(fila.Replace("\\\\", "\\"));
                archivo_xml = FirmarXml(documento, cert);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCEResumenDiario" + ex.ToString());
            }

            return archivo_xml;
        }

        public string cs_pxProcesarResumenDiario(List<string> DocumentosAResumenDiario)
        {
            string retornar = String.Empty;
            /*foreach (var documento_id in DocumentosAResumenDiario)
            {
                new clsBaseSunat().cs_pxEnviarCEToRC(documento_id);             
            }*/
            string documentos_noagregados = String.Empty;

            List<string> agregado = new List<string>();
            foreach (var documento_id in DocumentosAResumenDiario)
            {
                //string nuevo = "'" + documento_id + "'";
                string nuevo = documento_id;

                agregado.Add(nuevo);
            }

            //ordenar en orden ascendente los docs.
            List<string> lista_ordenada = new clsEntityDocument(localbd).cs_pxOrdenarAscendente(agregado);

            if (lista_ordenada.Count() > 0)
            {
                foreach (var documento_id in lista_ordenada)
                {
                    bool retorno = new clsBaseSunat(localbd).cs_pxEnviarCEToRC(documento_id, true);
                    if (retorno == false)
                    {
                        clsEntityDocument doc = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(documento_id);
                        documentos_noagregados += doc.Cs_tag_ID + " \n";
                        doc = null;
                    }
                }
            }

            if (documentos_noagregados != "")
            {
                retornar = documentos_noagregados;
                //clsBaseMensaje.cs_pxMsg("Documentos no agregados", "Los siguientes documentos no se agregaron al resumen diario: \n" + documentos_noagregados + "Verifique la fecha de emision no mayor a 7 dias. Si es sustitutorio/rectificatorio debe liberarlos para poder agregarlos al resumen.");
            }
            return retornar;
        }   
            /// <summary>
        /// Agrega un documento Boleta (NC Y ND) a Resumen diario.
        /// </summary>
        /// <param name="documento">Documento de tipo Boleta</param>
        public string cs_pxAgregarAResumenDiario(clsEntityDocument documento)
        {
            string id_resumen = string.Empty;
            bool agregado =false;
            if (documento.Cs_tag_InvoiceTypeCode == "03" || documento.Cs_tag_BillingReference_DocumentTypeCode == "03")//Solo permitir la agregación de BOLETAS, NC Y ND
            {
                string ResumenDiario_Actual = cs_pxObtenerResumenDiarioActual(documento);
                agregado=cs_pxActualizarResumenDiarioItem(ResumenDiario_Actual, documento);
                if (agregado)
                {
                    id_resumen = ResumenDiario_Actual;
                }
                
            }
            return id_resumen;
        }
        private void cs_pxAgregarSustitutorio(string resumenDiario_Actual, string resumenDiario_Enviado)
        {
            List<string> items_anterior = new List<string>();
            List<List<string>> anteriores = new clsEntityDocument(localbd).cs_pxObtenerPorResumenDiario(resumenDiario_Enviado);
            foreach (var item in anteriores)
            {
                items_anterior.Add(item[0].ToString().Trim());
            }

            if (items_anterior.Count > 0)
            {
                List<string> agregado = new List<string>();
                foreach (var documento_id in items_anterior)
                {
                    //string nuevo = "'" + documento_id + "'";
                    string nuevo = documento_id;
                    agregado.Add(nuevo);
                }

                //ordenar en orden ascendente los docs.
                List<string> lista_ordenada = new clsEntityDocument(localbd).cs_pxOrdenarAscendente(agregado);

                if (lista_ordenada.Count() > 0)
                {
                    clsEntityDocument documento;
                    foreach (var documento_id in lista_ordenada)
                    {
                        documento = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(documento_id);
                        cs_pxActualizarResumenDiarioItem(resumenDiario_Actual, documento);
                        documento.Cs_pr_Resumendiario = resumenDiario_Actual;
                        documento.Cs_pr_EstadoSCC = "0";
                        documento.Cs_pr_EstadoSUNAT = "2";
                        documento.cs_pxActualizar(false,false);
                        documento = null;
                    }
                }
            }

        }
        private string cs_pxObtenerResumenDiarioEnviado(clsEntityDocument documento)
        {
            string resumendiario_enviado = string.Empty;
            string existe_anterior = string.Empty;//0 no hay enviado 1 si hay enviado(s)
            clsEntitySummaryDocuments resumendiario = new clsEntitySummaryDocuments(localbd);
            int enviados = resumendiario.cs_fxObtenerIdResumenDiaFechaReferencia(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
            if (enviados > 0)
            {
                List<string> ultimo = resumendiario.cs_fxObtenerIdResumenDiaFechaReferencia(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate);
                resumendiario_enviado = ultimo[0].ToString().Trim();
            }

            return resumendiario_enviado;
        }

        //Cristhian|13/11/2017|FEI2-325
        /*INICIO MODIFICACIóN*/
        /*Se le agrega un limitador al numero de conmprobantes electronicos que tendra cada ressumen diario, cada paquete de Resumen Diario tendra como máximo 1000
         comprobantes electrónicos*/
        //Cristhian|12/01/2018|FEI2-539
        /*Se modifica el limite de 1000 comprobantes admitidos reducido a 500*/
        private string cs_pxObtenerResumenDiarioActual(clsEntityDocument documento)
        {
            string resumendiario_actual = string.Empty;
            clsEntitySummaryDocuments resumendiario = new clsEntitySummaryDocuments(localbd);
            string  pendiente = resumendiario.cs_fxObtenerResumenIdPorFechaReferencia_PENDIENTE(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate);

            /*Si se tiene registro de un Resumen Diario y se quiere agregar un nuevo Comprobante
             *de la misma fecha que el anterior (Boleta, Nota de Credito o Nota de Debito),
             *entonces se añade en el mismo Resumen diario, pero la cantidad que se tiene en el 
              resumendiario debe ser menor a MIL*/
            int Correlativo = documento.cs_ContarResumenDiario_Lineas(pendiente);
            if (pendiente.Trim().Length > 0 && Correlativo < 500)
            {         
                    resumendiario_actual = pendiente; 
            }
            /*En caso se trate de un Resumen Diario Nuevo, o de una boleta con fecha diferente,
              se crea el resumen diario con su respectivo codigo correlativo*/
            else
            {
                //no hay pendiente crear uno nuevo
                //obtener el correlativo
                string correlativo = resumendiario.cs_fxObtenerCorrelativo(DateTime.Now.ToString("yyyy-MM-dd"));
                resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                resumendiario.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                resumendiario.Cs_tag_ReferenceDate = documento.Cs_tag_IssueDate;
                resumendiario.Cs_pr_EstadoSCC = "2";
                resumendiario.Cs_pr_EstadoSUNAT = "2";
                resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_IssueDate.Replace("-", "") + "-" + correlativo;
                resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                resumendiario.Cs_tag_DocumentCurrencyCode = "PEN";
                resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = documento.Cs_tag_AccountingSupplierParty_Party_PartyName_Name;
                string idInsertado = resumendiario.cs_pxInsertar(false, false);
                resumendiario_actual = idInsertado;
            }  
                    
            return resumendiario_actual;
        }
        /*FIN MODIFICACIóN*/

        private string cs_pxObtenerResumenDiarioActual_Old(clsEntityDocument documento)//Olvidado por que no esta siendo utilizado
        {
            string resumendiario_actual = string.Empty;
            string existe_anterior = string.Empty;//0 no hay enviado 1 si hay enviado(s)
            clsEntitySummaryDocuments resumendiario = new clsEntitySummaryDocuments(localbd);
            //buscar si ya existe  resumen enviado para el dia de envio hoy.
            int enviados = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_ENVIADOS(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
            if (enviados <= 0)
            {
                // si no existe enviados para hoy  -> buscar si ya se creo uno pendiente de hoy con referencia a fecha de referencia del documento
                int correlativo = resumendiario.cs_fxObtenerResumenesIdPorFechaReferencia_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
                if (correlativo <= 0)
                {
                    //no existe pendiente para fecha de referencia de documento

                    //buscar si hay otros pendientes de referencia a  cualquier fecha diferente entonces agregar 1
                    int pendientes_otros = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;

                    if (pendientes_otros <= 0)
                    {
                        // no hay pendientes crear con correlativo 1;
                        resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                        resumendiario.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        resumendiario.Cs_tag_ReferenceDate = documento.Cs_tag_IssueDate;
                        resumendiario.Cs_pr_EstadoSCC = "2";
                        resumendiario.Cs_pr_EstadoSUNAT = "2";
                        resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_IssueDate.Replace("-", "") + "-1";
                        resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                        // resumendiario.Cs_tag_DocumentCurrencyCode = documento.Cs_tag_DocumentCurrencyCode;
                        resumendiario.Cs_tag_DocumentCurrencyCode = "PEN";
                        string idInsertado = resumendiario.cs_pxInsertar(false, false);
                        string serie = documento.Cs_tag_ID.Split('-')[0];
                        //resumendiario_actual = resumendiario.Cs_pr_SummaryDocuments_Id;
                        resumendiario_actual = idInsertado;
                    }
                    else
                    {
                        int pendientes_otros_1 = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;

                        //hay pendiente entonces crear correlativo +1 
                        resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                        resumendiario.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        resumendiario.Cs_tag_ReferenceDate = documento.Cs_tag_IssueDate;
                        resumendiario.Cs_pr_EstadoSCC = "2";
                        resumendiario.Cs_pr_EstadoSUNAT = "2";
                        resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_IssueDate.Replace("-", "") + "-" + Convert.ToString(pendientes_otros_1 + 1);
                        resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                        // resumendiario.Cs_tag_DocumentCurrencyCode = documento.Cs_tag_DocumentCurrencyCode;
                        resumendiario.Cs_tag_DocumentCurrencyCode = "PEN";
                        string idInsertado = resumendiario.cs_pxInsertar(false, false);
                        string serie = documento.Cs_tag_ID.Split('-')[0];
                        // resumendiario_actual = resumendiario.Cs_pr_SummaryDocuments_Id;
                        resumendiario_actual = idInsertado;
                    }


                }
                else
                {
                    //existe pendiente envio para fecha de referencia
                    foreach (var item in resumendiario.cs_fxObtenerResumenesIdPorFechaReferenciaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate))
                    {
                        resumendiario_actual = item;
                    }
                }

            }
            else
            {
                //hay enviado(s) para el dia de referencias

                //buscar si hay pendientes de envio con fecha referencia documentos y envio hoy;

                int correlativo = resumendiario.cs_fxObtenerResumenesIdPorFechaReferencia_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
                if (correlativo <= 0)
                {
                    //Si no hay pendientes crear uno 
                    //verificar si hay pendientes de envio hoy con referencia a cualquier fecha
                    int pendientes_otros = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;

                    if (pendientes_otros <= 0)
                    {
                        //no existe pendientes otros correlativo= enviados + 1
                        int new_correlativo = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_ENVIADOS(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count + 1; //Resumen diario de hoy con referencia a otra han sido enviados + 1
                        resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                        resumendiario.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        resumendiario.Cs_tag_ReferenceDate = documento.Cs_tag_IssueDate;
                        resumendiario.Cs_pr_EstadoSCC = "2";
                        resumendiario.Cs_pr_EstadoSUNAT = "2";
                        resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_IssueDate.Replace("-", "") + "-" + new_correlativo.ToString();
                        resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                        //resumendiario.Cs_tag_DocumentCurrencyCode = documento.Cs_tag_DocumentCurrencyCode;
                        resumendiario.Cs_tag_DocumentCurrencyCode = "PEN";
                        string idInsertado = resumendiario.cs_pxInsertar(false, false);
                        string serie = documento.Cs_tag_ID.Split('-')[0];
                        resumendiario_actual = idInsertado;
                    }
                    else
                    {
                        //hay pendiente otrs => correlativo=enviados+pendientes+1
                        int new_correlativo = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_ENVIADOS(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
                        int pendientes_otros_2 = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate).Count;
                        resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                        resumendiario.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        resumendiario.Cs_tag_ReferenceDate = documento.Cs_tag_IssueDate;
                        resumendiario.Cs_pr_EstadoSCC = "2";
                        resumendiario.Cs_pr_EstadoSUNAT = "2";
                        resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_IssueDate.Replace("-", "") + "-" + Convert.ToString(new_correlativo + pendientes_otros_2 + 1);
                        resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                        resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                        //resumendiario.Cs_tag_DocumentCurrencyCode = documento.Cs_tag_DocumentCurrencyCode;
                        resumendiario.Cs_tag_DocumentCurrencyCode = "PEN";
                        string idInsertado = resumendiario.cs_pxInsertar(false, false);
                        string serie = documento.Cs_tag_ID.Split('-')[0];
                        resumendiario_actual = idInsertado;
                    }


                }
                else
                {
                    //hay pendientes con fecha de referencia documento envio hoy;
                    foreach (var item in resumendiario.cs_fxObtenerResumenesIdPorFechaReferenciaEnvio_PENDIENTES(DateTime.Now.ToString("yyyy-MM-dd"), documento.Cs_tag_IssueDate))
                    {
                        resumendiario_actual = item;
                    }
                }
            }


            return resumendiario_actual;
        }
        public void cs_pxAgregarAResumenDiarioMODELO_2017(clsEntityDocument documento)
        {/*
            if (documento.Cs_tag_InvoiceTypeCode == "03")
            {
                clsEntitySummaryDocuments resumendiario = new clsEntitySummaryDocuments();
                int correlativo = resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio(DateTime.Now.ToString("yyyy-MM-dd")).Count;
                if (correlativo <= 0)
                {
                    correlativo += 1;
                    resumendiario.Cs_pr_SummaryDocuments_Id = Guid.NewGuid().ToString();
                    resumendiario.Cs_tag_ReferenceDate = DateTime.Now.ToString("yyyy-MM-dd");
                    resumendiario.Cs_tag_ID = "RC-" + resumendiario.Cs_tag_ReferenceDate.Replace("-", "") + "-" + correlativo.ToString();
                    resumendiario.Cs_tag_AccountingSupplierParty_AdditionalAccountID = documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID;
                    resumendiario.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID;
                    resumendiario.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName;
                    resumendiario.Cs_tag_DocumentCurrencyCode = documento.Cs_tag_DocumentCurrencyCode;
                    resumendiario.cs_pxInsertar(false);
                    string serie = documento.Cs_tag_ID.Split('-')[0];
                    insertaractualizarItem(resumendiario.Cs_pr_SummaryDocuments_Id, resumendiario, documento, correlativo, serie);
                }
                else
                {
                    foreach (var item in resumendiario.cs_fxObtenerResumenesIdPorFechaEnvio(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        string serie = documento.Cs_tag_ID.Split('-')[0];
                        insertaractualizarItem(item, resumendiario, documento, correlativo, serie);
                    }
                }
            }*/
        }
        private bool cs_pxActualizarResumenDiarioItem(string RD_ID, clsEntityDocument documento)
        {
            //*->SOLO SE PUEDE ACTUALIZAR SI SU ESTADO ES PENDIENTE.
            //*->SOLO SE ENVIARÁ SI SU ESTADO ES PENDIENTE
            //*->Cuando se inserte una boleta, inmediatamente se enviará a resumen diario. (Además agregar su estado de enviado o pendiente)
            //*->Cuando se actualize una boleta, inmediatamente será actualizado en resumen diario (Incluye estados de envío).

            //Buscar y actualizar resumenes diarios
            //OBTENER EL RESUMEN DIARIO DE HOY (SI NO EXISTE, CREAR)
            //OBTENER LOS RESUMENES PENDIENTES

            //!FE = OBTENER FECHA DE EMISION DE BOLETA, NC, ND
            //!FR = OBTENER FECHA DE REFEREN DE BOLETA, NC, ND

            //OBTENER EL ULTIMO CORRELATIVO SÓLO DE DOCUMENTOS ENVIADOS DE HOY (SOLO ANTES DE ENVIAR)
            //ACTUALIZAR EL CORRELATIVO ANTES DE ENVIAR.

            //SI !FR = PERIODO VÁLIDO
            //      SI !FE == !FR DONDE !FE = HOY
            //          //AGREGAR A RESUMENDIARIO DE HOY
            //          BUSCAR RESUMEN DIARIO DE HOY, SI NO EXISTE, CREAR; SI EXISTE AGREGARDOCUMENTO(DOCUMENTO, RD)
            //      FIN_SI
            //      SI !FE != !FR DONDE !FE = HOY
            //          //AGREGAR RESUMEN DIARIO SUSTITUTORIO COMPLEMENTARIO-CORRECTIVO
            //          BUSCAR RESUMEN DIARIO DE DE LA !FR; SI EXISTE (COPIAR EL DOCUMENTO DE RESUMEN COMPLETAMENTE PERO REALIZAR EL CAMBIO)  AGREGARDOCUMENTO(DOCUMENTO, RD) SI NO: NO SE PUEDE ENCONTRAR UN RESUMEN DIARIO CON LA FECHA DE REFERENCIA INDICADA ¿DESEA CREAR RD CON LA FECHA DE REFERENCIA INDICADA?
            //      FIN_SI    
            //SI NO
            //  EL PERIODO NO ES VALIDO
            //FIN_SI

            //AGREGARDOCUMENTO(DOCUMENTO, RD)
            //      EDITAR RESUMEN DIARIO CON FECHA DE EMISION = DOCUMENTO.FECHADEREFERENCIA
            //      BUSCAR EN ITEMS DE RESUMEN DIARIO LOS QUE TENGAN LA MISMA SERIE.
            //      ACTUALIZAR EL RANGO INICIO - FIN
            //      ACTUALIZAR TOTALES Y SUB TOTALES EN EL ITEM

            string FechaEmision = DateTime.Now.ToString("yyyy-MM-dd");
            string FechaReferencia = documento.Cs_tag_IssueDate;
            bool PERIODOVALIDO = false;
            bool retorno = false;
            bool DocEsDolares = false;
            double TipoCambio = 1;

            try
            {
                //Jordy Amaro 03-11-16 FE-808 
                //Inicio Modifica - 1 ::: Si ya ha sido enviado como resumen diario anteriormenete para no validar fechas 
                if (documento.Cs_ResumenUltimo_Enviado.ToString() != "")
                {
                    //ya ha sido enviado y es sustituido o rectificado pasarlo
                    PERIODOVALIDO = true;
                }

                else
                {
                    /*Se solicitó quitar la validación de 7 días para generación de resumen diario*/
                    //    //comprobar 7 dias anteriores.
                    //    if (FechaReferencia == DateTime.Now.ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") ||
                    //    FechaReferencia == DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"))
                    {
                        PERIODOVALIDO = true;
                    }
                }

                //Fin Modifica - 1

            //Periodo valido para agregar a resumen diario.
            if (PERIODOVALIDO == true)
                {
                    //Comprobar si la moneda es en dolares y hay tipo de cambio en bd.
                    if (documento.Cs_tag_DocumentCurrencyCode == "USD" && documento.Cs_TipoCambio.ToString() != "")
                    {
                        DocEsDolares = true;
                        TipoCambio = Convert.ToDouble(documento.Cs_TipoCambio.ToString(),CultureInfo.CreateSpecificCulture("en-US"));
                    }


                    //AGREGAR A RESUMENDIARIO DE HOY
                    //AGREGARDOCUMENTO(DOCUMENTO, RD)
                    //      EDITAR RESUMEN DIARIO CON FECHA DE EMISION = DOCUMENTO.FECHADEREFERENCIA
                    //      BUSCAR EN ITEMS DE RESUMEN DIARIO LOS QUE TENGAN LA MISMA SERIE.
                    //      ACTUALIZAR EL RANGO INICIO - FIN
                    //      ACTUALIZAR TOTALES Y SUB TOTALES EN EL ITEM
                    string Documento_Serie = documento.Cs_tag_ID.Split('-')[0];
                    string Documento_Numero = documento.Cs_tag_ID.Split('-')[1];
                    string tipo_doc = documento.Cs_tag_InvoiceTypeCode;

                    //Obtener Condition Code
                    string Documento_EstadoSUNAT = documento.Cs_pr_EstadoSUNAT;

                    //Obtener numero 
                    string numero_sinceros = Convert.ToString(Convert.ToInt32(Documento_Numero));
                    clsEntitySummaryDocuments RD = new clsEntitySummaryDocuments(localbd).cs_fxObtenerUnoPorId(RD_ID);
                    //RD.Cs_tag_IssueDate = documento.Cs_tag_IssueDate;            
                    //clsBaseLog.cs_pxRegistarAdd(RD_ID+"-"+ Documento_Serie + "-"+ tipo_doc );
                    List<clsEntitySummaryDocuments_SummaryDocumentsLine> collection = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraIdYSerieYTipo(RD_ID, Documento_Serie, tipo_doc);//Se verifica si previamente no se tiene un resumen diario registrado en la base de datos
                    //If no tiene items el resumen diario insertar como primer item.
                    if (collection == null || collection.Count <= 0)
                    {
                        //Insertar primer item
                        clsEntitySummaryDocuments_SummaryDocumentsLine RDLinea = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd);
                        RDLinea.Cs_pr_SummaryDocuments_Id = RD.Cs_pr_SummaryDocuments_Id;
                        RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = Guid.NewGuid().ToString();
                        RDLinea.Cs_tag_DocumentSerialID = Documento_Serie;
                        RDLinea.Cs_tag_DocumentTypeCode = documento.Cs_tag_InvoiceTypeCode;
                        RDLinea.Cs_tag_LineID = "1";
                        RDLinea.Cs_tag_StartDocumentNumberID = numero_sinceros;
                        RDLinea.Cs_tag_EndDocumentNumberID = numero_sinceros;

                        if (DocEsDolares)
                        {
                            double Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_a = Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", "."),CultureInfo.CreateSpecificCulture("en-US"));
                            double Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_n = Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_a * TipoCambio;
                            RDLinea.Cs_tag_TotalAmount = string.Format("{0:0.00}", Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_n);
                        }
                        else
                        {
                            RDLinea.Cs_tag_TotalAmount = documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", ".");
                        }

                        //Cristhian|09/11/2017|FEI2-325
                        /*Se agrega el Estado del Documento-En la etiqueta Condition Code*/
                        /*NUEVO INICIO*/
                        /*Si se encuentra el estado tres de SUNAT, entonces el documento recibe el 
                         estado 3 -> "ANULADO"*/
                        if (documento.Cs_pr_EstadoSCC == "3")
                        {
                            RDLinea.Cs_tag_ConditionCode = "3";
                        }
                        /*Si se encuentra duplicado del documento, entonces el documento recibe el 
                         estado 2 -> "MODIFICADO"*/
                        else if (documento.cs_Buscar_DocumentoDuplicado(documento.Cs_pr_Document_Id))
                        {
                            RDLinea.Cs_tag_ConditionCode = "2";
                        }
                        /*Si no cumple con ninguna de las anteriores, entonces el documento recibe el
                          1 -> "ADICIONAR"*/
                        else
                        {
                            RDLinea.Cs_tag_ConditionCode = "1";
                        }
                        /*NUEVO FIN*/

                        string RDLineaId = RDLinea.cs_pxInsertar(false,true);

                        string OP_Gravadas = "0.00";
                        string OP_Inafectas = "0.00";
                        string OP_Exoneradas = "0.00";

                        //Extraer las iars del documento.
                        List<List<string>> Iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);
                        //Por cada IARS, estraer su: Aditional monetary total.
                        foreach (var item_Iars in Iars)
                        {
                            List<List<string>> MonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(item_Iars[0].ToString());

                            foreach (var item_MonetaryTotal in MonetaryTotal)
                            {
                                if (item_MonetaryTotal[2].ToString() == "1001")//Operaciones gravadas
                                {
                                    if (item_MonetaryTotal[5].Length > 0)
                                    {
                                        OP_Gravadas = item_MonetaryTotal[5].ToString();
                                    }
                                }
                                if (item_MonetaryTotal[2].ToString() == "1002")//Operaciones inafectas
                                {
                                    if (item_MonetaryTotal[5].Length > 0)
                                    {
                                        OP_Inafectas = item_MonetaryTotal[5].ToString();
                                    }
                                }
                                if (item_MonetaryTotal[2].ToString() == "1003")//Operaciones exoneradas
                                {
                                    if (item_MonetaryTotal[5].Length > 0)
                                    {
                                        OP_Exoneradas = item_MonetaryTotal[5].ToString();
                                    }
                                }
                            }
                        }

                        //billing payment op gravadas.
                        clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                        bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                        bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        bp01.Cs_tag_InstructionID = "01";
                        if (DocEsDolares)
                        {
                            double OP_Gravadas_a = Convert.ToDouble(OP_Gravadas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double OP_Gravadas_n = OP_Gravadas_a * TipoCambio;
                            bp01.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Gravadas_n);
                        }
                        else
                        {
                            bp01.Cs_tag_PaidAmount = OP_Gravadas.Replace(",", ".");
                        }

                        bp01.cs_pxInsertar(false,true);

                        //billing payment op exoneradas.
                        clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp02 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                        bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                        bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        bp02.Cs_tag_InstructionID = "02";
                        if (DocEsDolares)
                        {
                            double OP_Exoneradas_a = Convert.ToDouble(OP_Exoneradas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double OP_Exoneradas_n = OP_Exoneradas_a * TipoCambio;
                            bp02.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Exoneradas_n);
                        }
                        else
                        {
                            bp02.Cs_tag_PaidAmount = OP_Exoneradas.Replace(",", ".");
                        }
                        // bp02.Cs_tag_PaidAmount = OP_Exoneradas.Replace(",", ".");
                        bp02.cs_pxInsertar(false,true);

                        //billing payment op inafectas.
                        clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp03 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                        bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                        bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        bp03.Cs_tag_InstructionID = "03";
                        if (DocEsDolares)
                        {
                            double OP_Inafectas_a = Convert.ToDouble(OP_Inafectas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double OP_Inafectas_n = OP_Inafectas_a * TipoCambio;
                            bp03.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Inafectas_n);
                        }
                        else
                        {
                            bp03.Cs_tag_PaidAmount = OP_Inafectas.Replace(",", ".");
                        }
                        // bp03.Cs_tag_PaidAmount = OP_Inafectas.Replace(",", ".");
                        bp03.cs_pxInsertar(false,true);

                        //Tag allowance charge 
                        clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge ac01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd);
                        ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id = Guid.NewGuid().ToString();
                        ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        if (documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Trim().Length > 0)
                        {
                            if (DocEsDolares)
                            {
                                double Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_a = Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_n = Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_a * TipoCambio;
                                ac01.Cs_tag_Amount = string.Format("{0:0.00}", Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_n);
                            }
                            else
                            {
                                ac01.Cs_tag_Amount = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                            }
                            // ac01.Cs_tag_Amount = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                        }
                        else
                        {
                            ac01.Cs_tag_Amount = "0.00";
                        }
                        ac01.Cs_tag_ChargeIndicator = "true";
                        ac01.cs_pxInsertar(false,true);


                        //obtener datos de tax total
                        List<List<string>> impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);

                        string imp_IGV = "0.00";
                        string imp_ISC = "0.00";
                        string imp_OTRO = "0.00";

                        foreach (List<string> ress in impuestos_globales)
                        {
                            Array newarray = ress.ToArray();
                            if (Convert.ToString(newarray.GetValue(4)) == "1000")
                            {//IGV
                                imp_IGV = Convert.ToString(newarray.GetValue(2));
                            }
                            else if (Convert.ToString(newarray.GetValue(4)) == "2000")
                            {//isc
                                imp_ISC = Convert.ToString(newarray.GetValue(2));
                            }
                            else if (Convert.ToString(newarray.GetValue(4)) == "9999")
                            {//otros
                                imp_OTRO = Convert.ToString(newarray.GetValue(2));
                            }
                        }

                        //Tax total ISC
                        clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_isc = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                        taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                        if (DocEsDolares)
                        {
                            double imp_ISC_a = Convert.ToDouble(imp_ISC.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double imp_ISC_n = imp_ISC_a * TipoCambio;
                            taxtotal_isc.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_ISC_n);
                            taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_ISC_n);
                        }
                        else
                        {
                            taxtotal_isc.Cs_tag_TaxAmount = imp_ISC.Replace(",", ".");
                            taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = imp_ISC.Replace(",", ".");
                        }
                        //taxtotal_isc.Cs_tag_TaxAmount = imp_ISC.Replace(",", ".");
                        taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_ID = "2000";
                        taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_Name = "ISC";
                        taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "EXC";
                        //taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = imp_ISC.Replace(",", ".");
                        taxtotal_isc.cs_pxInsertar(false,true);

                        //Tax Total IGV
                        clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_igv = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                        taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                        if (DocEsDolares)
                        {
                            double imp_IGV_a = Convert.ToDouble(imp_IGV.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double imp_IGV_n = imp_IGV_a * TipoCambio;
                            taxtotal_igv.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_IGV_n);
                            taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_IGV_n);
                        }
                        else
                        {
                            taxtotal_igv.Cs_tag_TaxAmount = imp_IGV.Replace(",", ".");
                            taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = imp_IGV.Replace(",", ".");
                        }
                        //taxtotal_igv.Cs_tag_TaxAmount = imp_IGV.Replace(",", ".");
                        taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_ID = "1000";
                        taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_Name = "IGV";
                        taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "VAT";
                        //taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = imp_IGV.Replace(",", ".");
                        taxtotal_igv.cs_pxInsertar(false,true);

                        //Tax total otros.
                        clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_otro = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                        taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLineaId;
                        taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                        if (DocEsDolares)
                        {
                            double imp_OTRO_a = Convert.ToDouble(imp_OTRO.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                            double imp_OTRO_n = imp_OTRO_a * TipoCambio;
                            taxtotal_otro.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_OTRO_n);
                            taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_OTRO_n);
                        }
                        else
                        {
                            taxtotal_otro.Cs_tag_TaxAmount = imp_OTRO.Replace(",", ".");
                            taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = imp_OTRO.Replace(",", ".");
                        }
                        // taxtotal_otro.Cs_tag_TaxAmount = imp_OTRO.Replace(",", ".");
                        taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_ID = "9999";
                        taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_Name = "OTROS";
                        taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "OTH";
                        // taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = imp_OTRO.Replace(",", ".");
                        taxtotal_otro.cs_pxInsertar(false,true);

                        
                        // RDLinea.Cs_tag_TotalAmount = documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", ".");//Buscar la fórmula del cálculo en la página del 29-30 del pdf http://contenido.app.sunat.gob.pe/insc/ComprobantesDePago+Electronicos/Guias_manualesabr2013/GUIA+XML+Resumen+de+Boletas+revisado.pdf                                      
                        retorno = true;
                    }
                    else
                    {
                        //actualizar porque ya existe al menos uno
                        //buscar por cada linea si tiene consecutivo

                        //Actualizar item el doc es consecutivo al anterior
                        bool agregado_item = false;
                        bool agregado_item_error = false;

                        #region Comentado para no buscar el consecutivo
                        //foreach (var coll in collection)
                        //{
                        //    //Cristhian|28/12/2017|FEI2-325
                        //    /*Se agrega estas condiciiones para crear un acumulado de los archivos
                        //      que estan siendo enviadas por primera vez y los que estan de baja*/
                        //    /*NUEVO INICIO*/
                        //    /*En este caso si la Etiqueta condition code es 1, es un indicativo que 
                        //      esta siendo enviado por primera vez, pero para validarlo en la continuidad
                        //      (ya que se busca comprobantes con el numero consecutivo) a la variable
                        //      condicion se le asigna el valor de 2*/
                        //    if (Convert.ToInt64(coll.Cs_tag_ConditionCode) == 1)
                        //    {
                        //        condicion = 2;
                        //    }
                        //    /*En caso el condition code sea 2, se le asigna el mismo valor de 2*/
                        //    else if (Convert.ToInt64(coll.Cs_tag_ConditionCode) == 2)
                        //    {
                        //        condicion = 2;
                        //    }
                        //    /*Y si no coincide con ninguno de los 2 anteriores entonces es un comprobante
                        //     con el estado de BAJA para este tipo de comprobantes se le asigna el valor de 3*/
                        //    else
                        //    {
                        //        condicion = 3;
                        //    }
                        //    /*NUEVO FIN*/

                        //    //buscar por cada linea si tiene consecutivo
                        //    if (Convert.ToInt64(Documento_Numero) == Convert.ToInt64(coll.Cs_tag_EndDocumentNumberID) + 1 && Convert.ToInt64(Documento_EstadoSUNAT) == condicion)
                        //    {
                        //        try
                        //        {
                        //            coll.Cs_tag_EndDocumentNumberID = numero_sinceros;//Actualizar el item con la misma serie.
                        //            coll.cs_pxActualizar(false, true);//Además actualizar los totales.

                        //            double valor_total_anterior = Convert.ToDouble(coll.Cs_tag_TotalAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //            double valor_total_documento = Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //            if (DocEsDolares)
                        //            {
                        //                valor_total_documento = valor_total_documento * TipoCambio;
                        //            }


                        //            double nuevo_total_prev = valor_total_anterior + valor_total_documento;

                        //            string nuevo_total = string.Format("{0:0.00}", nuevo_total_prev);
                        //            string OP_Gravadas = "0.00";
                        //            string OP_Inafectas = "0.00";
                        //            string OP_Exoneradas = "0.00";
                        //            /***************************************************/
                        //            //Extraer las iars del documento.
                        //            List<List<string>> Iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);
                        //            //Por cada IARS, estraer su: Aditional monetary total.
                        //            foreach (var item_Iars in Iars)
                        //            {
                        //                List<List<string>> MonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(item_Iars[0].ToString());

                        //                foreach (var item_MonetaryTotal in MonetaryTotal)
                        //                {
                        //                    if (item_MonetaryTotal[2].ToString() == "1001")//Operaciones gravadas
                        //                    {
                        //                        if (item_MonetaryTotal[5].Length > 0)
                        //                        {
                        //                            OP_Gravadas = item_MonetaryTotal[5].ToString();
                        //                        }
                        //                    }
                        //                    if (item_MonetaryTotal[2].ToString() == "1002")//Operaciones inafectas
                        //                    {
                        //                        if (item_MonetaryTotal[5].Length > 0)
                        //                        {
                        //                            OP_Inafectas = item_MonetaryTotal[5].ToString();
                        //                        }
                        //                    }
                        //                    if (item_MonetaryTotal[2].ToString() == "1003")//Operaciones exoneradas
                        //                    {
                        //                        if (item_MonetaryTotal[5].Length > 0)
                        //                        {
                        //                            OP_Exoneradas = item_MonetaryTotal[5].ToString();
                        //                        }
                        //                    }
                        //                }
                        //            }

                        //            //buscar anterior registro  , sumar mas actual y actualizar::
                        //            string bp1_id = "";
                        //            string bp2_id = "";
                        //            string bp3_id = "";
                        //            List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> bp_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerTodoPorCabeceraId(coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                        //            foreach (var itemBilling in bp_anterior)
                        //            {
                        //                if (itemBilling.Cs_tag_InstructionID == "01")
                        //                {
                        //                    bp1_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        //                }
                        //                else if (itemBilling.Cs_tag_InstructionID == "02")
                        //                {
                        //                    bp2_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        //                }
                        //                else if (itemBilling.Cs_tag_InstructionID == "03")
                        //                {
                        //                    bp3_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        //                }

                        //            }


                        //            //billing payment 01 operaciones gravadas.

                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp1_id);
                        //            if (bp01 != null)
                        //            {
                        //                double ant_operaciones_gravados = Convert.ToDouble(bp01.Cs_tag_PaidAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_operaciones_gravadas = Convert.ToDouble(OP_Gravadas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_operaciones_gravadas = new_operaciones_gravadas * TipoCambio;
                        //                }

                        //                double result_operaciones_gravadas_prev = ant_operaciones_gravados + new_operaciones_gravadas;
                        //                string result_operaciones_gravadas = string.Format("{0:0.00}", result_operaciones_gravadas_prev);

                        //                bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                        //                bp01.Cs_tag_InstructionID = "01";
                        //                bp01.Cs_tag_PaidAmount = result_operaciones_gravadas.Replace(",", ".");
                        //                bp01.cs_pxActualizar(false, true);
                        //            }
                        //            //billing payment 02 operaciones inafectas.
                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp02 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp2_id);
                        //            if (bp02 != null)
                        //            {
                        //                double ant_operaciones_inafectas = Convert.ToDouble(bp02.Cs_tag_PaidAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_operaciones_inafectas = Convert.ToDouble(OP_Inafectas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_operaciones_inafectas = new_operaciones_inafectas * TipoCambio;
                        //                }
                        //                double result_operaciones_inafectas_prev = ant_operaciones_inafectas + new_operaciones_inafectas;
                        //                string result_operaciones_inafectas = string.Format("{0:0.00}", result_operaciones_inafectas_prev);
                        //                bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                        //                bp02.Cs_tag_InstructionID = "02";
                        //                bp02.Cs_tag_PaidAmount = result_operaciones_inafectas.Replace(",", ".");
                        //                bp02.cs_pxActualizar(false, true);
                        //            }


                        //            //billing payment 01 operaciones exoneradas.
                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp03 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp3_id);
                        //            if (bp03 != null)
                        //            {
                        //                double ant_operaciones_exoneradas = Convert.ToDouble(bp03.Cs_tag_PaidAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_operaciones_exoneradas = Convert.ToDouble(OP_Exoneradas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_operaciones_exoneradas = new_operaciones_exoneradas * TipoCambio;
                        //                }
                        //                double result_operaciones_exoneradas_prev = ant_operaciones_exoneradas + new_operaciones_exoneradas;
                        //                string result_operaciones_exoneradas = string.Format("{0:0.00}", result_operaciones_exoneradas_prev);

                        //                bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                        //                bp03.Cs_tag_InstructionID = "03";
                        //                bp03.Cs_tag_PaidAmount = result_operaciones_exoneradas.Replace(",", ".");
                        //                bp03.cs_pxActualizar(false, true);
                        //            }

                        //            //allowance charge
                        //            //obtener anteriores y sumar nuevos
                        //            string ac1_id = "";
                        //            string new_ac1 = "0.00";
                        //            List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> ac_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerTodoPorCabeceraId(coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                        //            foreach (var itemAllow in ac_anterior)
                        //            {
                        //                ac1_id = itemAllow.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id;
                        //            }

                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge ac01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerUnoPorId(ac1_id);
                        //            if (ac01 != null)
                        //            {
                        //                double ant_allowed_charge = Convert.ToDouble(ac01.Cs_tag_Amount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Trim().Length > 0)
                        //                {
                        //                    new_ac1 = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                        //                }
                        //                else
                        //                {
                        //                    new_ac1 = "0.00";
                        //                }
                        //                double new_allowed_charge = Convert.ToDouble(new_ac1, CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_allowed_charge = new_allowed_charge * TipoCambio;
                        //                }
                        //                double result_allowed_charge_prev = ant_allowed_charge + new_allowed_charge;
                        //                string result_allowed_charge = string.Format("{0:0.00}", result_allowed_charge_prev);
                        //                ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                        //                ac01.Cs_tag_Amount = result_allowed_charge.Replace(",", ".");
                        //                ac01.Cs_tag_ChargeIndicator = "true";
                        //                ac01.cs_pxActualizar(false, true);
                        //            }

                        //            //obtener tax total doc
                        //            var impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);

                        //            string imp_IGV = "0.00";
                        //            string imp_ISC = "0.00";
                        //            string imp_OTRO = "0.00";

                        //            foreach (List<string> ress in impuestos_globales)
                        //            {
                        //                Array newarray = ress.ToArray();

                        //                if (Convert.ToString(newarray.GetValue(4)) == "1000")
                        //                {//IGV
                        //                    imp_IGV = Convert.ToString(newarray.GetValue(2));

                        //                }
                        //                else if (Convert.ToString(newarray.GetValue(4)) == "2000")
                        //                {//isc
                        //                    imp_ISC = Convert.ToString(newarray.GetValue(2));

                        //                }
                        //                else if (Convert.ToString(newarray.GetValue(4)) == "9999")
                        //                {//otros
                        //                    imp_OTRO = Convert.ToString(newarray.GetValue(2));

                        //                }

                        //            }
                        //            //obtener registros anteriores
                        //            string IGV_id = string.Empty;
                        //            string ISC_id = string.Empty;
                        //            string OTROS_id = string.Empty;
                        //            List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> tax_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerTodoPorCabeceraId(coll.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                        //            foreach (var itemTax in tax_anterior)
                        //            {
                        //                if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "1000")
                        //                {
                        //                    IGV_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        //                }
                        //                else if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "2000")
                        //                {
                        //                    ISC_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        //                }
                        //                else if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "9999")
                        //                {
                        //                    OTROS_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        //                }

                        //            }

                        //            //Tax Total ISC
                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_isc = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(ISC_id);
                        //            if (taxtotal_isc != null)
                        //            {
                        //                double ant_taxtotal_isc = Convert.ToDouble(taxtotal_isc.Cs_tag_TaxAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_taxtotal_isc = Convert.ToDouble(imp_ISC.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_taxtotal_isc = new_taxtotal_isc * TipoCambio;
                        //                }
                        //                double result_taxtotal_isc_prev = ant_taxtotal_isc + new_taxtotal_isc;
                        //                string result_taxtotal_isc = string.Format("{0:0.00}", result_taxtotal_isc_prev);

                        //                taxtotal_isc.Cs_tag_TaxAmount = result_taxtotal_isc.Replace(",", ".");
                        //                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_ID = "2000";
                        //                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_Name = "ISC";
                        //                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "EXC";
                        //                taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_isc.Replace(",", ".");
                        //                taxtotal_isc.cs_pxActualizar(false, true);
                        //            }


                        //            //TaxTotal IGV.
                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_igv = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(IGV_id);
                        //            if (taxtotal_igv != null)
                        //            {
                        //                double ant_taxtotal_igv = Convert.ToDouble(taxtotal_igv.Cs_tag_TaxAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_taxtotal_igv = Convert.ToDouble(imp_IGV.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_taxtotal_igv = new_taxtotal_igv * TipoCambio;
                        //                }
                        //                double result_taxtotal_igv_prev = ant_taxtotal_igv + new_taxtotal_igv;
                        //                string result_taxtotal_igv = string.Format("{0:0.00}", result_taxtotal_igv_prev);

                        //                taxtotal_igv.Cs_tag_TaxAmount = result_taxtotal_igv.Replace(",", ".");
                        //                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_ID = "1000";
                        //                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_Name = "IGV";
                        //                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "VAT";
                        //                taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_igv.Replace(",", ".");
                        //                taxtotal_igv.cs_pxActualizar(false, true);
                        //            }


                        //            //Tax Total Otro
                        //            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_otro = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(OTROS_id);
                        //            if (taxtotal_otro != null)
                        //            {
                        //                double ant_taxtotal_otro = Convert.ToDouble(taxtotal_otro.Cs_tag_TaxAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                double new_taxtotal_otro = Convert.ToDouble(imp_OTRO.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                        //                if (DocEsDolares)
                        //                {
                        //                    new_taxtotal_otro = new_taxtotal_otro * TipoCambio;
                        //                }
                        //                double result_taxtotal_otro_prev = ant_taxtotal_otro + new_taxtotal_otro;
                        //                string result_taxtotal_otro = string.Format("{0:0.00}", result_taxtotal_otro_prev);

                        //                taxtotal_otro.Cs_tag_TaxAmount = result_taxtotal_otro.Replace(",", ".");
                        //                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_ID = "9999";
                        //                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_Name = "OTROS";
                        //                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "OTH";
                        //                taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_otro.Replace(",", ".");
                        //                taxtotal_otro.cs_pxActualizar(false, true);
                        //            }

                        //            coll.Cs_tag_TotalAmount = nuevo_total.Replace(",", ".");//Buscar la fórmula del cálculo en la página del 29-30 del pdf http://contenido.app.sunat.gob.pe/insc/ComprobantesDePago+Electronicos/Guias_manualesabr2013/GUIA+XML+Resumen+de+Boletas+revisado.pdf
                        //            coll.cs_pxActualizar(false, true);
                        //            //coll.cs_pxInsertar(false);
                        //            retorno = true;
                        //            agregado_item = true;
                        //        }
                        //        catch
                        //        {
                        //            agregado_item_error = true;
                        //        }

                        //    }
                        //}/*comentar lo que esta dentro del else podria solucionar el acumulado*/
                        #endregion

                        //->sino es agregado por error tons agregado item igual a true
                        //En caso no haya algun elemento anterior en las lineas ya guardadas, insertar nueva linea al resumen diario.
                        if (agregado_item == false && agregado_item_error==false)
                        {
                            //Insertar primer item de nueva linea;
                            //Insertar primer item
                            clsEntitySummaryDocuments_SummaryDocumentsLine RDLinea = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd);
                            RDLinea.Cs_pr_SummaryDocuments_Id = RD.Cs_pr_SummaryDocuments_Id;

                            //int conteo_igual = 0;
                            //foreach (clsEntitySummaryDocuments_SummaryDocumentsLine doc_res in collection)
                            //{
                            //    if (Documento_Serie == doc_res.Cs_tag_DocumentSerialID && numero_sinceros == doc_res.Cs_tag_EndDocumentNumberID)
                            //    {
                            //        conteo_igual++;
                            //    }
                            //}

                            //if (conteo_igual > 0)
                            //{
                            //    RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                            //}
                            //else
                            //{
                                RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = Guid.NewGuid().ToString();
                            //}

                            RDLinea.Cs_tag_DocumentSerialID = Documento_Serie;
                            RDLinea.Cs_tag_DocumentTypeCode = documento.Cs_tag_InvoiceTypeCode;
                            RDLinea.Cs_tag_LineID = "1";
                            RDLinea.Cs_tag_StartDocumentNumberID = numero_sinceros;
                            RDLinea.Cs_tag_EndDocumentNumberID = numero_sinceros;

                            if (DocEsDolares)
                            {
                                double Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_a = Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_n = Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_a * TipoCambio;
                                RDLinea.Cs_tag_TotalAmount = string.Format("{0:0.00}", Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID_n);
                            }
                            else
                            {
                                RDLinea.Cs_tag_TotalAmount = documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", ".");
                            }
                            // RDLinea.Cs_tag_TotalAmount = documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", ".");//Buscar la fórmula del cálculo en la página del 29-30 del pdf http://contenido.app.sunat.gob.pe/insc/ComprobantesDePago+Electronicos/Guias_manualesabr2013/GUIA+XML+Resumen+de+Boletas+revisado.pdf

                            //Cristhian|09/11/2017|FEI2-325
                            /*Se agrega el Estado del Documento-En la etiqueta Condition Code*/
                            /*NUEVO INICIO*/
                            /*Si se encuentra el estado tres de SUNAT, entonces el documento recibe el 
                             estado 3 -> "ANULADO"*/
                            if (documento.Cs_pr_EstadoSCC == "3")
                            {
                                RDLinea.Cs_tag_ConditionCode = "3";
                            }
                            /*Si se encuentra duplicado del documento, entonces el documento recibe el 
                             estado 2 -> "MODIFICADO"*/
                            else if (documento.cs_Buscar_DocumentoDuplicado(documento.Cs_pr_Document_Id))
                            {
                                RDLinea.Cs_tag_ConditionCode = "2";
                            }
                            /*Si no cumple con ninguna de las anteriores, entonces el documento recibe el
                              1 -> "ADICIONAR"*/
                            else
                            {
                                RDLinea.Cs_tag_ConditionCode = "1";
                            }
                            /*NUEVO FIN*/

                            string IdRetorno = "";
                            //if (conteo_igual > 0)
                            //{
                            //    bool Actualizo = RDLinea.cs_pxActualizar(false, true);
                            //    IdRetorno = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                            //}
                            //else
                            //{
                                IdRetorno = RDLinea.cs_pxInsertar(false,true);
                            //}

                            string OP_Gravadas = "0.00";
                            string OP_Inafectas = "0.00";
                            string OP_Exoneradas = "0.00";

                            //Extraer las iars del documento.
                            List<List<string>> Iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);
                            //Por cada IARS, estraer su: Aditional monetary total.
                            foreach (var item_Iars in Iars)
                            {
                                List<List<string>> MonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(item_Iars[0].ToString());

                                foreach (var item_MonetaryTotal in MonetaryTotal)
                                {
                                    if (item_MonetaryTotal[2].ToString() == "1001")//Operaciones gravadas
                                    {
                                        if (item_MonetaryTotal[5].Length > 0)
                                        {
                                            OP_Gravadas = item_MonetaryTotal[5].ToString();
                                        }
                                    }
                                    if (item_MonetaryTotal[2].ToString() == "1002")//Operaciones inafectas
                                    {
                                        if (item_MonetaryTotal[5].Length > 0)
                                        {
                                            OP_Inafectas = item_MonetaryTotal[5].ToString();
                                        }
                                    }
                                    if (item_MonetaryTotal[2].ToString() == "1003")//Operaciones exoneradas
                                    {
                                        if (item_MonetaryTotal[5].Length > 0)
                                        {
                                            OP_Exoneradas = item_MonetaryTotal[5].ToString();
                                        }
                                    }
                                }
                            }

                            //billing payment op gravadas.
                            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                            bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                            bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            bp01.Cs_tag_InstructionID = "01";
                            if (DocEsDolares)
                            {
                                double OP_Gravadas_a = Convert.ToDouble(OP_Gravadas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double OP_Gravadas_n = OP_Gravadas_a * TipoCambio;
                                bp01.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Gravadas_n);
                            }
                            else
                            {
                                bp01.Cs_tag_PaidAmount = OP_Gravadas.Replace(",", ".");
                            }

                            bp01.cs_pxInsertar(false,true);

                            //billing payment op_exoneradas.
                            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp02 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                            bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                            bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            bp02.Cs_tag_InstructionID = "02";
                            if (DocEsDolares)
                            {
                                double OP_Exoneradas_a = Convert.ToDouble(OP_Exoneradas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double OP_Exoneradas_n = OP_Exoneradas_a * TipoCambio;
                                bp02.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Exoneradas_n);
                            }
                            else
                            {
                                bp02.Cs_tag_PaidAmount = OP_Exoneradas.Replace(",", ".");
                            }
                            // bp02.Cs_tag_PaidAmount = OP_Exoneradas.Replace(",", ".");
                            bp02.cs_pxInsertar(false,true);

                            //billing payment op inafectas.
                            clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp03 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                            bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                            bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            bp03.Cs_tag_InstructionID = "03";
                            if (DocEsDolares)
                            {
                                double OP_Inafectas_a = Convert.ToDouble(OP_Inafectas.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double OP_Inafectas_n = OP_Inafectas_a * TipoCambio;
                                bp03.Cs_tag_PaidAmount = string.Format("{0:0.00}", OP_Inafectas_n);
                            }
                            else
                            {
                                bp03.Cs_tag_PaidAmount = OP_Inafectas.Replace(",", ".");
                            }
                            // bp03.Cs_tag_PaidAmount = OP_Inafectas.Replace(",", ".");
                            bp03.cs_pxInsertar(false,true);

                            //allowance charge
                            clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge ac01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd);
                            ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id = Guid.NewGuid().ToString();
                            ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            if (documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Trim().Length > 0)
                            {
                                if (DocEsDolares)
                                {
                                    double Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_a = Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                    double Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_n = Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_a * TipoCambio;
                                    ac01.Cs_tag_Amount = string.Format("{0:0.00}", Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount_n);
                                }
                                else
                                {
                                    ac01.Cs_tag_Amount = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                                }
                                // ac01.Cs_tag_Amount = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                            }
                            else
                            {
                                ac01.Cs_tag_Amount = "0.00";
                            }
                            ac01.Cs_tag_ChargeIndicator = "true";
                            ac01.cs_pxInsertar(false,true);


                            //obtener tax total
                            var impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);

                            string imp_IGV = "0.00";
                            string imp_ISC = "0.00";
                            string imp_OTRO = "0.00";

                            foreach (List<string> ress in impuestos_globales)
                            {
                                Array newarray = ress.ToArray();

                                if (Convert.ToString(newarray.GetValue(4)) == "1000")
                                {//IGV
                                    imp_IGV = Convert.ToString(newarray.GetValue(2));

                                }
                                else if (Convert.ToString(newarray.GetValue(4)) == "2000")
                                {//isc
                                    imp_ISC = Convert.ToString(newarray.GetValue(2));

                                }
                                else if (Convert.ToString(newarray.GetValue(4)) == "9999")
                                {
                                    imp_OTRO = Convert.ToString(newarray.GetValue(2));

                                }

                            }

                            //Tax total ISC
                            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_isc = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                            taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                            if (DocEsDolares)
                            {
                                double imp_ISC_a = Convert.ToDouble(imp_ISC.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double imp_ISC_n = imp_ISC_a * TipoCambio;
                                taxtotal_isc.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_ISC_n);
                                taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_ISC_n);
                            }
                            else
                            {
                                taxtotal_isc.Cs_tag_TaxAmount = imp_ISC.Replace(",", ".");
                                taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = imp_ISC.Replace(",", ".");
                            }
                            //taxtotal_isc.Cs_tag_TaxAmount = imp_ISC.Replace(",", ".");
                            taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_ID = "2000";
                            taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_Name = "ISC";
                            taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "EXC";
                            //taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = imp_ISC.Replace(",", ".");
                            taxtotal_isc.cs_pxInsertar(false,true);

                            //Tax Total IGV
                            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_igv = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                            taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                            if (DocEsDolares)
                            {
                                double imp_IGV_a = Convert.ToDouble(imp_IGV.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double imp_IGV_n = imp_IGV_a * TipoCambio;
                                taxtotal_igv.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_IGV_n);
                                taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_IGV_n);
                            }
                            else
                            {
                                taxtotal_igv.Cs_tag_TaxAmount = imp_IGV.Replace(",", ".");
                                taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = imp_IGV.Replace(",", ".");
                            }
                            //taxtotal_igv.Cs_tag_TaxAmount = imp_IGV.Replace(",", ".");
                            taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_ID = "1000";
                            taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_Name = "IGV";
                            taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "VAT";
                            //taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = imp_IGV.Replace(",", ".");
                            taxtotal_igv.cs_pxInsertar(false,true);

                            //Tax Total Otro.
                            clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_otro = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                            taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = IdRetorno;
                            taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                            if (DocEsDolares)
                            {
                                double imp_OTRO_a = Convert.ToDouble(imp_OTRO.Replace(",", "."), CultureInfo.CreateSpecificCulture("en-US"));
                                double imp_OTRO_n = imp_OTRO_a * TipoCambio;
                                taxtotal_otro.Cs_tag_TaxAmount = string.Format("{0:0.00}", imp_OTRO_n);
                                taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = string.Format("{0:0.00}", imp_OTRO_n);
                            }
                            else
                            {
                                taxtotal_otro.Cs_tag_TaxAmount = imp_OTRO.Replace(",", ".");
                                taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = imp_OTRO.Replace(",", ".");
                            }
                            // taxtotal_otro.Cs_tag_TaxAmount = imp_OTRO.Replace(",", ".");
                            taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_ID = "9999";
                            taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_Name = "OTROS";
                            taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "OTH";
                            // taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = imp_OTRO.Replace(",", ".");
                            taxtotal_otro.cs_pxInsertar(false,true);

                            

                            retorno = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno = false;
                clsBaseLog.cs_pxRegistarAdd("Error agregar item to RC" + ex.ToString());
            }

            //aqui
            return retorno;
        }
        private bool cs_pxActualizarResumenDiarioItem_OLD(string RD_ID, clsEntityDocument documento)
        {
            //*->SOLO SE PUEDE ACTUALIZAR SI SU ESTADO ES PENDIENTE.
            //*->SOLO SE ENVIARÁ SI SU ESTADO ES PENDIENTE
            //*->Cuando se inserte una boleta, inmediatamente se enviará a resumen diario. (Además agregar su estado de enviado o pendiente)
            //*->Cuando se actualize una boleta, inmediatamente será actualizado en resumen diario (Incluye estados de envío).

            //Buscar y actualizar resumenes diarios
            //OBTENER EL RESUMEN DIARIO DE HOY (SI NO EXISTE, CREAR)
            //OBTENER LOS RESUMENES PENDIENTES

            //!FE = OBTENER FECHA DE EMISION DE BOLETA, NC, ND
            //!FR = OBTENER FECHA DE REFEREN DE BOLETA, NC, ND

            //OBTENER EL ULTIMO CORRELATIVO SÓLO DE DOCUMENTOS ENVIADOS DE HOY (SOLO ANTES DE ENVIAR)
            //ACTUALIZAR EL CORRELATIVO ANTES DE ENVIAR.

            //SI !FR = PERIODO VÁLIDO
            //      SI !FE == !FR DONDE !FE = HOY
            //          //AGREGAR A RESUMENDIARIO DE HOY
            //          BUSCAR RESUMEN DIARIO DE HOY, SI NO EXISTE, CREAR; SI EXISTE AGREGARDOCUMENTO(DOCUMENTO, RD)
            //      FIN_SI
            //      SI !FE != !FR DONDE !FE = HOY
            //          //AGREGAR RESUMEN DIARIO SUSTITUTORIO COMPLEMENTARIO-CORRECTIVO
            //          BUSCAR RESUMEN DIARIO DE DE LA !FR; SI EXISTE (COPIAR EL DOCUMENTO DE RESUMEN COMPLETAMENTE PERO REALIZAR EL CAMBIO)  AGREGARDOCUMENTO(DOCUMENTO, RD) SI NO: NO SE PUEDE ENCONTRAR UN RESUMEN DIARIO CON LA FECHA DE REFERENCIA INDICADA ¿DESEA CREAR RD CON LA FECHA DE REFERENCIA INDICADA?
            //      FIN_SI    
            //SI NO
            //  EL PERIODO NO ES VALIDO
            //FIN_SI

            //AGREGARDOCUMENTO(DOCUMENTO, RD)
            //      EDITAR RESUMEN DIARIO CON FECHA DE EMISION = DOCUMENTO.FECHADEREFERENCIA
            //      BUSCAR EN ITEMS DE RESUMEN DIARIO LOS QUE TENGAN LA MISMA SERIE.
            //      ACTUALIZAR EL RANGO INICIO - FIN
            //      ACTUALIZAR TOTALES Y SUB TOTALES EN EL ITEM

            string FE = DateTime.Now.ToString("yyyy-MM-dd");
            string FR = documento.Cs_tag_IssueDate;
            //bool PERIODOVALIDO = false;
            bool retorno = false;
            /* if (FR == DateTime.Now.ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") ||
                 FR == DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"))
             {
                 PERIODOVALIDO = true;
             }

             if (PERIODOVALIDO == true)
             {*/
            //if (FE == FR)
            //{
            //AGREGAR A RESUMENDIARIO DE HOY
            //AGREGARDOCUMENTO(DOCUMENTO, RD)
            //      EDITAR RESUMEN DIARIO CON FECHA DE EMISION = DOCUMENTO.FECHADEREFERENCIA
            //      BUSCAR EN ITEMS DE RESUMEN DIARIO LOS QUE TENGAN LA MISMA SERIE.
            //      ACTUALIZAR EL RANGO INICIO - FIN
            //      ACTUALIZAR TOTALES Y SUB TOTALES EN EL ITEM
            string Documento_Serie = documento.Cs_tag_ID.Split('-')[0];
            string Documento_Numero = documento.Cs_tag_ID.Split('-')[1];
            string tipo_doc = documento.Cs_tag_InvoiceTypeCode;
            //Obtener numero 
            string numero_sinceros = Convert.ToString(Convert.ToInt32(Documento_Numero));
            clsEntitySummaryDocuments RD = new clsEntitySummaryDocuments(localbd).cs_fxObtenerUnoPorId(RD_ID);
            RD.Cs_tag_IssueDate = documento.Cs_tag_IssueDate;

            List<clsEntitySummaryDocuments_SummaryDocumentsLine> collection = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraIdYSerieYTipo(RD_ID, Documento_Serie, tipo_doc);

            if (collection == null || collection.Count <= 0)
            {
                //Insertar item
                clsEntitySummaryDocuments_SummaryDocumentsLine RDLinea = new clsEntitySummaryDocuments_SummaryDocumentsLine(localbd);
                RDLinea.Cs_pr_SummaryDocuments_Id = RD.Cs_pr_SummaryDocuments_Id;
                RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = Guid.NewGuid().ToString();
                RDLinea.Cs_tag_DocumentSerialID = Documento_Serie;
                RDLinea.Cs_tag_DocumentTypeCode = documento.Cs_tag_InvoiceTypeCode;
                RDLinea.Cs_tag_LineID = "1";
                RDLinea.Cs_tag_StartDocumentNumberID = numero_sinceros;
                RDLinea.Cs_tag_EndDocumentNumberID = numero_sinceros;

                string OP_Gravadas = "0.00";
                string OP_Inafectas = "0.00";
                string OP_Exoneradas = "0.00";

                //Extraer las iars del documento.
                List<List<string>> Iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);
                //Por cada IARS, estraer su: Aditional monetary total.
                foreach (var item_Iars in Iars)
                {
                    List<List<string>> MonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(item_Iars[0].ToString());

                    foreach (var item_MonetaryTotal in MonetaryTotal)
                    {
                        if (item_MonetaryTotal[2].ToString() == "1001")//Operaciones gravadas
                        {
                            if (item_MonetaryTotal[5].Length > 0)
                            {
                                OP_Gravadas = item_MonetaryTotal[5].ToString();
                            }
                        }
                        if (item_MonetaryTotal[2].ToString() == "1002")//Operaciones inafectas
                        {
                            if (item_MonetaryTotal[5].Length > 0)
                            {
                                OP_Inafectas = item_MonetaryTotal[5].ToString();
                            }
                        }
                        if (item_MonetaryTotal[2].ToString() == "1003")//Operaciones exoneradas
                        {
                            if (item_MonetaryTotal[5].Length > 0)
                            {
                                OP_Exoneradas = item_MonetaryTotal[5].ToString();
                            }
                        }
                    }
                }

                //billing payment.
                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                bp01.Cs_tag_InstructionID = "01";
                bp01.Cs_tag_PaidAmount = OP_Gravadas.Replace(",", ".");
                bp01.cs_pxInsertar(false,true);

                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp02 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                bp02.Cs_tag_InstructionID = "02";
                bp02.Cs_tag_PaidAmount = OP_Exoneradas.Replace(",", ".");
                bp02.cs_pxInsertar(false,true);

                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp03 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd);
                bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = Guid.NewGuid().ToString();
                bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                bp03.Cs_tag_InstructionID = "03";
                bp03.Cs_tag_PaidAmount = OP_Inafectas.Replace(",", ".");
                bp03.cs_pxInsertar(false,true);

                //allowance charge
                clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge ac01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd);
                ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id = Guid.NewGuid().ToString();
                ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                if (documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Trim().Length > 0)
                {
                    ac01.Cs_tag_Amount = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                }
                else
                {
                    ac01.Cs_tag_Amount = "0.00";
                }
                ac01.Cs_tag_ChargeIndicator = "true";
                ac01.cs_pxInsertar(false,true);


                //obtener tax total
                var impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);

                string imp_IGV = "0.00";
                string imp_ISC = "0.00";
                string imp_OTRO = "0.00";

                foreach (List<string> ress in impuestos_globales)
                {
                    Array newarray = ress.ToArray();

                    if (Convert.ToString(newarray.GetValue(4)) == "1000")
                    {//IGV
                        imp_IGV = Convert.ToString(newarray.GetValue(2));

                    }
                    else if (Convert.ToString(newarray.GetValue(4)) == "2000")
                    {//isc
                        imp_ISC = Convert.ToString(newarray.GetValue(2));

                    }
                    else if (Convert.ToString(newarray.GetValue(4)) == "9999")
                    {
                        imp_OTRO = Convert.ToString(newarray.GetValue(2));

                    }

                }

                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_isc = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                taxtotal_isc.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                taxtotal_isc.Cs_tag_TaxAmount = imp_ISC.Replace(",", ".");
                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_ID = "2000";
                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_Name = "ISC";
                taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "EXC";
                taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = imp_ISC.Replace(",", ".");
                taxtotal_isc.cs_pxInsertar(false,true);

                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_igv = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                taxtotal_igv.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                taxtotal_igv.Cs_tag_TaxAmount = imp_IGV.Replace(",", ".");
                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_ID = "1000";
                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_Name = "IGV";
                taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "VAT";
                taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = imp_IGV.Replace(",", ".");
                taxtotal_igv.cs_pxInsertar(false,true);

                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_otro = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd);
                taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = RDLinea.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                taxtotal_otro.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = Guid.NewGuid().ToString();
                taxtotal_otro.Cs_tag_TaxAmount = imp_OTRO.Replace(",", ".");
                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_ID = "9999";
                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_Name = "OTROS";
                taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "OTH";
                taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = imp_OTRO.Replace(",", ".");
                taxtotal_otro.cs_pxInsertar(false,true);


                RDLinea.Cs_tag_TotalAmount = documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", ".");//Buscar la fórmula del cálculo en la página del 29-30 del pdf http://contenido.app.sunat.gob.pe/insc/ComprobantesDePago+Electronicos/Guias_manualesabr2013/GUIA+XML+Resumen+de+Boletas+revisado.pdf
                RDLinea.cs_pxInsertar(false,true);

                retorno = true;
            }
            else
            {
                //actualizar porque ya existe al menos uno
                //Actualizar item el doc es consecutivo al anterior
                if (Convert.ToInt64(Documento_Numero) == Convert.ToInt64(collection[0].Cs_tag_EndDocumentNumberID) + 1)
                {
                    collection[0].Cs_tag_EndDocumentNumberID = numero_sinceros;//Actualizar el item con la misma serie.
                    collection[0].cs_pxActualizar(false,true);//Además actualizar los totales.

                    double valor_total_anterior = Convert.ToDouble(collection[0].Cs_tag_TotalAmount.Replace(",","."));
                    double nuevo_total_prev = valor_total_anterior + Convert.ToDouble(documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID.Replace(",", "."));
                    string nuevo_total = string.Format("{0:0.00}", nuevo_total_prev);
                    string OP_Gravadas = "0.00";
                    string OP_Inafectas = "0.00";
                    string OP_Exoneradas = "0.00";
                    /***************************************************/
                    //Extraer las iars del documento.
                    List<List<string>> Iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);
                    //Por cada IARS, estraer su: Aditional monetary total.
                    foreach (var item_Iars in Iars)
                    {
                        List<List<string>> MonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(item_Iars[0].ToString());

                        foreach (var item_MonetaryTotal in MonetaryTotal)
                        {
                            if (item_MonetaryTotal[2].ToString() == "1001")//Operaciones gravadas
                            {
                                if (item_MonetaryTotal[5].Length > 0)
                                {
                                    OP_Gravadas = item_MonetaryTotal[5].ToString();
                                }
                            }
                            if (item_MonetaryTotal[2].ToString() == "1002")//Operaciones inafectas
                            {
                                if (item_MonetaryTotal[5].Length > 0)
                                {
                                    OP_Inafectas = item_MonetaryTotal[5].ToString();
                                }
                            }
                            if (item_MonetaryTotal[2].ToString() == "1003")//Operaciones exoneradas
                            {
                                if (item_MonetaryTotal[5].Length > 0)
                                {
                                    OP_Exoneradas = item_MonetaryTotal[5].ToString();
                                }
                            }
                        }
                    }

                    //buscar anterior registro  , sumar mas actual e actualizar::
                    string bp1_id = "";
                    string bp2_id = "";
                    string bp3_id = "";
                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> bp_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerTodoPorCabeceraId(collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                    foreach (var itemBilling in bp_anterior)
                    {
                        if (itemBilling.Cs_tag_InstructionID == "01")
                        {
                            bp1_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        }
                        else if (itemBilling.Cs_tag_InstructionID == "02")
                        {
                            bp2_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        }
                        else if (itemBilling.Cs_tag_InstructionID == "03")
                        {
                            bp3_id = itemBilling.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id;
                        }

                    }


                    //billing payment 01.

                    clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp1_id);
                    double ant_operaciones_gravados = Convert.ToDouble(bp01.Cs_tag_PaidAmount.Replace(",", "."));
                    double new_operaciones_gravadas = Convert.ToDouble(OP_Gravadas.Replace(",", "."));
                    double result_operaciones_gravadas_prev = ant_operaciones_gravados + new_operaciones_gravadas;
                    string result_operaciones_gravadas = string.Format("{0:0.00}", result_operaciones_gravadas_prev);

                    bp01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                    bp01.Cs_tag_InstructionID = "01";
                    bp01.Cs_tag_PaidAmount = result_operaciones_gravadas.Replace(",", ".");
                    bp01.cs_pxActualizar(false,true);

                    clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp02 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp2_id);
                    double ant_operaciones_inafectas = Convert.ToDouble(bp02.Cs_tag_PaidAmount.Replace(",", "."));
                    double new_operaciones_inafectas = Convert.ToDouble(OP_Inafectas.Replace(",", "."));
                    double result_operaciones_inafectas_prev = ant_operaciones_inafectas + new_operaciones_inafectas;
                    string result_operaciones_inafectas = string.Format("{0:0.00}", result_operaciones_inafectas_prev);
                    bp02.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                    bp02.Cs_tag_InstructionID = "02";
                    bp02.Cs_tag_PaidAmount = result_operaciones_inafectas.Replace(",", ".");
                    bp02.cs_pxActualizar(false,true);

                    clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment bp03 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localbd).cs_fxObtenerUnoPorId(bp3_id);
                    double ant_operaciones_exoneradas = Convert.ToDouble(bp03.Cs_tag_PaidAmount.Replace(",", "."));
                    double new_operaciones_exoneradas = Convert.ToDouble(OP_Exoneradas.Replace(",", "."));
                    double result_operaciones_exoneradas_prev = ant_operaciones_exoneradas + new_operaciones_exoneradas;
                    string result_operaciones_exoneradas = string.Format("{0:0.00}", result_operaciones_exoneradas_prev);

                    bp03.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                    bp03.Cs_tag_InstructionID = "03";
                    bp03.Cs_tag_PaidAmount = result_operaciones_exoneradas.Replace(",", ".");
                    bp03.cs_pxActualizar(false,true);

                    //allowance charge
                    //obtener anteriores y sumar nuevos
                    string ac1_id = "";
                    string new_ac1 = "0.00";
                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> ac_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerTodoPorCabeceraId(collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                    foreach (var itemAllow in ac_anterior)
                    {
                        ac1_id = itemAllow.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id;                 
                    }

                    clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge ac01 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localbd).cs_fxObtenerUnoPorId(ac1_id);
                    double ant_allowed_charge = Convert.ToDouble(ac01.Cs_tag_Amount.Replace(",", "."));
                    if (documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Trim().Length > 0)
                    {
                        new_ac1 = documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount.Replace(",", ".");
                    }else
                    {
                        new_ac1 = "0.00";
                    }                   
                    double new_allowed_charge = Convert.ToDouble(new_ac1);
                    double result_allowed_charge_prev = ant_allowed_charge + new_allowed_charge;
                    string result_allowed_charge = string.Format("{0:0.00}", result_allowed_charge_prev);
                    ac01.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id;
                    ac01.Cs_tag_Amount = result_allowed_charge.Replace(",", ".");                   
                    ac01.Cs_tag_ChargeIndicator = "true";
                    ac01.cs_pxActualizar(false,true);



                    //obtener tax total doc
                    var impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(documento.Cs_pr_Document_Id);

                    string imp_IGV = "0.00";
                    string imp_ISC = "0.00";
                    string imp_OTRO = "0.00";

                    foreach (List<string> ress in impuestos_globales)
                    {
                        Array newarray = ress.ToArray();

                        if (Convert.ToString(newarray.GetValue(4)) == "1000")
                        {//IGV
                            imp_IGV = Convert.ToString(newarray.GetValue(2));

                        }
                        else if (Convert.ToString(newarray.GetValue(4)) == "2000")
                        {//isc
                            imp_ISC = Convert.ToString(newarray.GetValue(2));

                        }
                        else if (Convert.ToString(newarray.GetValue(4)) == "9999")
                        {
                            imp_OTRO = Convert.ToString(newarray.GetValue(2));

                        }

                    }
                    //obtener registros anteriores
                    string IGV_id = string.Empty;
                    string ISC_id = string.Empty;
                    string OTROS_id = string.Empty;
                    List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> tax_anterior = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerTodoPorCabeceraId(collection[0].Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
                    foreach (var itemTax in tax_anterior)
                    {
                        if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "1000")
                        {
                            IGV_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        }else if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "2000")
                        {
                            ISC_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        }else if (itemTax.Cs_tag_TaxCategory_TaxScheme_ID == "9999")
                        {
                            OTROS_id = itemTax.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id;
                        }
                       
                    }

                    clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_isc = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(ISC_id);
                    double ant_taxtotal_isc = Convert.ToDouble(taxtotal_isc.Cs_tag_TaxAmount.Replace(",", "."));
                    double new_taxtotal_isc = Convert.ToDouble(imp_ISC.Replace(",", "."));
                    double result_taxtotal_isc_prev = ant_taxtotal_isc + new_taxtotal_isc;
                    string result_taxtotal_isc = string.Format("{0:0.00}", result_taxtotal_isc_prev);

                    taxtotal_isc.Cs_tag_TaxAmount = result_taxtotal_isc.Replace(",", ".");
                    taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_ID = "2000";
                    taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_Name = "ISC";
                    taxtotal_isc.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "EXC";
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_isc.Replace(",", ".");
                    taxtotal_isc.cs_pxActualizar(false,true);

                    clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_igv = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(IGV_id) ;

                    double ant_taxtotal_igv = Convert.ToDouble(taxtotal_igv.Cs_tag_TaxAmount.Replace(",", "."));
                    double new_taxtotal_igv = Convert.ToDouble(imp_IGV.Replace(",", "."));
                    double result_taxtotal_igv_prev = ant_taxtotal_igv + new_taxtotal_igv;
                    string result_taxtotal_igv = string.Format("{0:0.00}", result_taxtotal_igv_prev);

                    taxtotal_igv.Cs_tag_TaxAmount = result_taxtotal_igv.Replace(",", ".");
                    taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_ID = "1000";
                    taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_Name = "IGV";
                    taxtotal_igv.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "VAT";
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_igv.Replace(",", ".");
                    taxtotal_igv.cs_pxActualizar(false,true);

                    clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal taxtotal_otro = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localbd).cs_fxObtenerUnoPorId(OTROS_id);

                    double ant_taxtotal_otro = Convert.ToDouble(taxtotal_otro.Cs_tag_TaxAmount.Replace(",", "."));
                    double new_taxtotal_otro = Convert.ToDouble(imp_OTRO.Replace(",", "."));
                    double result_taxtotal_otro_prev = ant_taxtotal_otro + new_taxtotal_otro;
                    string result_taxtotal_otro = string.Format("{0:0.00}", result_taxtotal_otro_prev);

                    taxtotal_otro.Cs_tag_TaxAmount = result_taxtotal_otro.Replace(",", ".");
                    taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_ID = "9999";
                    taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_Name = "OTROS";
                    taxtotal_otro.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = "OTH";
                    taxtotal_otro.Cs_tag_TaxSubtotal_TaxAmount = result_taxtotal_otro.Replace(",", ".");
                    taxtotal_otro.cs_pxActualizar(false,true);

                    collection[0].Cs_tag_TotalAmount = nuevo_total.Replace(",",".");//Buscar la fórmula del cálculo en la página del 29-30 del pdf http://contenido.app.sunat.gob.pe/insc/ComprobantesDePago+Electronicos/Guias_manualesabr2013/GUIA+XML+Resumen+de+Boletas+revisado.pdf
                    collection[0].cs_pxActualizar(false,true);
                    //collection[0].cs_pxInsertar(false);
                    retorno = true;
                }
            }

            //}
            //if (FE != FR)
            //{
            //AGREGAR A RESUMENDIARIO DE REFERENCIA
            //BUSCAR RESUMEN DIARIO DE DE LA !FR Y !FE = HOY; SI EXISTE AGREGARDOCUMENTO(DOCUMENTO, RD) SI NO: NO SE PUEDE ENCONTRAR UN RESUMEN DIARIO CON LA FECHA DE REFERENCIA INDICADA ¿DESEA CREAR RD CON LA FECHA DE REFERENCIA INDICADA?
            //Insertar item a resumen diario
            //}
            // }
            //aqui
            return retorno;
        }
    }
}
