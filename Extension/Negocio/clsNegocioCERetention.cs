using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Extension.Datos;
using Extension.Base;
using System.IO;

namespace Extension.Negocio
{
    public class clsNegocioCERetention:clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCERetention(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }

        public override string cs_pxGenerarXMLAString(string Id)
        {
            string archivo_xml = string.Empty;
            try
            {
                clsEntityRetention RetentionDocument = new clsEntityRetention(localbd).cs_fxObtenerUnoPorId(Id);
                List<clsEntityRetention_RetentionLine> RetentionLine = new clsEntityRetention_RetentionLine(localbd).cs_fxObtenerTodoPorCabeceraId(Id);

                string fila = "";
                string ei = "    ";
                string ef = "\n";

                fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
                fila += "<Retention " + ef + ef;
                #region Cabecera
                fila += ei + "xmlns=\"urn:sunat:names:specification:ubl:peru:schema:xsd:Retention-1\"" + ef;
                fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;
                fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
                fila += ei + "xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\"" + ef;
                fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
                fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
                fila += ei + "xmlns:qdt=\"urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2\"" + ef;
                fila += ei + "xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\"" + ef;
                fila += ei + "xmlns:udt=\"urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2\"" + ef;
                fila += ei + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + ef;
                fila += ei + "<ext:UBLExtensions>" + ef;
                fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>" + ef;
                fila += ei + "</ext:UBLExtensions>" + ef;
                #endregion
                fila += ei + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" + ef;
                fila += ei + "<cbc:CustomizationID>1.0</cbc:CustomizationID>" + ef;

                #region Referencia de la firma digital
                fila += ei + "<cac:Signature>" + ef;
                fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
                fila += ei + ei + "<cac:SignatoryParty>" + ef;
                fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
                fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;

                fila += ei + ei + ei + "<cac:PartyName>" + ef;
                fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + "RAZONSOCIALDECERTIFICADO" + "]]></cbc:Name>" + ef;
                fila += ei + ei + ei + "</cac:PartyName>" + ef;
                fila += ei + ei + "</cac:SignatoryParty>" + ef;

                fila += ei + ei + "<cac:DigitalSignatureAttachment>" + ef;
                fila += ei + ei + ei + "<cac:ExternalReference>" + ef;
                fila += ei + ei + ei + ei + "<cbc:URI>#SignatureSP</cbc:URI>" + ef;
                fila += ei + ei + ei + "</cac:ExternalReference>" + ef;
                fila += ei + ei + "</cac:DigitalSignatureAttachment>" + ef;
                fila += ei + "</cac:Signature>" + ef;
                #endregion
                fila += ei + "<cbc:ID>" + RetentionDocument.Cs_tag_Id + "</cbc:ID>" + ef;
                fila += ei + "<cbc:IssueDate>" + RetentionDocument.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
                #region Datos del emisor del documento
                fila += ei + "<cac:AgentParty>" + ef;
                fila += ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + "<cbc:ID schemeID=\"" + RetentionDocument.Cs_tag_PartyIdentificacion_SchemeId + "\">" + RetentionDocument.Cs_tag_PartyIdentification_Id + "</cbc:ID>" + ef;
                fila += ei + ei + "</cac:PartyIdentification>" + ef;

                if (RetentionDocument.Cs_tag_PartyName != "")
                {
                    fila += ei + ei + "<cac:PartyName>" + ef;
                    fila += ei + ei + ei + "<cbc:Name><![CDATA[" + RetentionDocument.Cs_tag_PartyName + "]]></cbc:Name>" + ef;
                    fila += ei + ei + "</cac:PartyName>" + ef;
                }

                if (RetentionDocument.Cs_tag_PostalAddress_Id != "" || RetentionDocument.Cs_tag_PostalAddress_StreetName != "" || RetentionDocument.Cs_tag_PostalAddress_CitySubdivisionName != "" || RetentionDocument.Cs_tag_PostalAddress_CityName != "" || RetentionDocument.Cs_tag_PostalAddress_CountrySubEntity != "" || RetentionDocument.Cs_tag_PostalAddress_District != "" || RetentionDocument.Cs_tag_PostalAddress_Country_IdentificationCode != "")
                {
                    fila += ei + ei + "<cac:PostalAddress>" + ef;
                    if (RetentionDocument.Cs_tag_PostalAddress_Id != "")
                    {
                        fila += ei + ei + ei + "<cbc:ID>" + RetentionDocument.Cs_tag_PostalAddress_Id + "</cbc:ID>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_PostalAddress_StreetName != "")
                    {
                        fila += ei + ei + ei + "<cbc:StreetName>" + RetentionDocument.Cs_tag_PostalAddress_StreetName + "</cbc:StreetName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_PostalAddress_CitySubdivisionName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CitySubdivisionName>" + RetentionDocument.Cs_tag_PostalAddress_CitySubdivisionName + "</cbc:CitySubdivisionName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_PostalAddress_CityName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CityName>" + RetentionDocument.Cs_tag_PostalAddress_CityName + "</cbc:CityName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_PostalAddress_CountrySubEntity != "")
                    {
                        fila += ei + ei + ei + "<cbc:CountrySubentity>" + RetentionDocument.Cs_tag_PostalAddress_CountrySubEntity + "</cbc:CountrySubentity>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_PostalAddress_District != "")
                    {
                        fila += ei + ei + ei + "<cbc:District>" + RetentionDocument.Cs_tag_PostalAddress_District + "</cbc:District>" + ef;
                    }

                    if (RetentionDocument.Cs_tag_PostalAddress_Country_IdentificationCode != "")
                    {
                        fila += ei + ei + ei + "<cac:Country>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:IdentificationCode>" + RetentionDocument.Cs_tag_PostalAddress_Country_IdentificationCode + "</cbc:IdentificationCode>" + ef;
                        fila += ei + ei + ei + "</cac:Country>" + ef;
                    }
                    fila += ei + ei + "</cac:PostalAddress>" + ef;
                }

                fila += ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + RetentionDocument.Cs_tag_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + "</cac:AgentParty>" + ef;
                #endregion

                #region Info  Proveedor
                fila += ei + "<cac:ReceiverParty>" + ef;
                fila += ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + "<cbc:ID schemeID=\"" + RetentionDocument.Cs_tag_ReceiveParty_PartyIdentification_SchemeId + "\">" + RetentionDocument.Cs_tag_ReceiveParty_PartyIdentification_Id + "</cbc:ID>" + ef;
                fila += ei + ei + "</cac:PartyIdentification>" + ef;
                if (RetentionDocument.Cs_tag_ReceiveParty_PartyName_Name != "")
                {
                    fila += ei + ei + "<cac:PartyName>" + ef;
                    fila += ei + ei + ei + "<cbc:Name><![CDATA[" + RetentionDocument.Cs_tag_ReceiveParty_PartyName_Name + "]]></cbc:Name>" + ef;
                    fila += ei + ei + "</cac:PartyName>" + ef;
                }

                if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Id != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_District != "" || RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode != "")
                {
                    fila += ei + ei + "<cac:PostalAddress>" + ef;
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Id != "")
                    {
                        fila += ei + ei + ei + "<cbc:ID>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Id + "</cbc:ID>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName != "")
                    {
                        fila += ei + ei + ei + "<cbc:StreetName>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName + "</cbc:StreetName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CitySubdivisionName>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName + "</cbc:CitySubdivisionName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CityName>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName + "</cbc:CityName>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity != "")
                    {
                        fila += ei + ei + ei + "<cbc:CountrySubentity>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity + "</cbc:CountrySubentity>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_District != "")
                    {
                        fila += ei + ei + ei + "<cbc:District>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_District + "</cbc:District>" + ef;
                    }
                    if (RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode != "")
                    {
                        fila += ei + ei + ei + "<cac:Country>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:IdentificationCode>" + RetentionDocument.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode + "</cbc:IdentificationCode>" + ef;
                        fila += ei + ei + ei + "</cac:Country>" + ef;
                    }
                    fila += ei + ei + "</cac:PostalAddress>" + ef;
                }

                fila += ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + RetentionDocument.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + "</cac:ReceiverParty>" + ef;
                #endregion

                fila += ei + "<sac:SUNATRetentionSystemCode>" + RetentionDocument.Cs_tag_SUNATRetentionSystemCode + "</sac:SUNATRetentionSystemCode>" + ef;
                fila += ei + "<sac:SUNATRetentionPercent>" + RetentionDocument.Cs_tag_SUNATRetentionPercent + "</sac:SUNATRetentionPercent>" + ef;

                //Cristhian|17/10/2017|FEI2-362
                /*Preparar un compilado Aparte, donde este cerrado la etiqueta cbc:Note eso con la finalidad de validar si SUNAT ya
                  permite en BETA realizar este tipo de pruebas. Se decidio que, para este tipo de documento, esta etiqueta ya no se
                  usa (es obsoleto)*/
                /*INICIO MODIFICAIóN*/
                //if (RetentionDocument.Cs_tag_Note != "")
                //{
                //    fila += ei + "<cbc:Note>" + RetentionDocument.Cs_tag_Note + "</cbc:Note>" + ef;
                //fila += ei + "<cbc:Note>"  + "</cbc:Note>" + ef;
                //}
                //else
                //{
                //    fila += ei + "<cbc:Note/>" + ef; 
                //}
                /*FIN MODIFICACIóN*/

                fila += ei + "<cbc:TotalInvoiceAmount currencyID=\"" + RetentionDocument.Cs_tag_TotalInvoiceAmount_CurrencyId + "\">" + RetentionDocument.Cs_tag_TotalInvoiceAmount + "</cbc:TotalInvoiceAmount>" + ef;
                fila += ei + "<sac:SUNATTotalPaid currencyID=\"" + RetentionDocument.Cs_tag_TotalPaid_CurrencyId + "\">" + RetentionDocument.Cs_tag_TotalPaid + "</sac:SUNATTotalPaid>" + ef;

                #region Items
                if (RetentionLine != null && RetentionLine.Count > 0)
                {
                    try
                    {
                        foreach (var RetentionLine_item in RetentionLine)
                        {
                            fila += ei + "<sac:SUNATRetentionDocumentReference>" + ef;
                            fila += ei + ei + "<cbc:ID schemeID=\"" + RetentionLine_item.Cs_tag_Id_SchemeId + "\">" + RetentionLine_item.Cs_tag_Id + "</cbc:ID>" + ef;
                            fila += ei + ei + "<cbc:IssueDate>" + RetentionLine_item.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
                            fila += ei + ei + "<cbc:TotalInvoiceAmount currencyID=\"" + RetentionLine_item.Cs_tag_TotalInvoiceAmount_CurrencyId + "\">" + RetentionLine_item.Cs_tag_TotalInvoiceAmount + "</cbc:TotalInvoiceAmount>" + ef;
                            fila += ei + ei + "<cac:Payment>" + ef;
                            //Cristhian|02/03/2018|CPD-853
                            /*Si es nota de credito no se debe enviar esta parte del codigo en el documento XML*/
                            /*NUEVO INICIO*/
                            /*Para detectar si es nota de credito en percepcion, el comercial enviara el caracter X*/
                            if (RetentionLine_item.Cs_tag_Payment_Id != "X" )
                            {
                                /*Se añadira esta parte de codigo si esque el Cs_tag_Payment_Id es diferente de X */
                                fila += ei + ei + ei + "<cbc:ID>" + RetentionLine_item.Cs_tag_Payment_Id + "</cbc:ID>" + ef;
                                fila += ei + ei + ei + "<cbc:PaidAmount currencyID=\"" + RetentionLine_item.Cs_tag_Payment_PaidAmount_CurrencyId + "\">" + RetentionLine_item.Cs_tag_Payment_PaidAmount + "</cbc:PaidAmount>" + ef;
                                fila += ei + ei + ei + "<cbc:PaidDate>" + RetentionLine_item.Cs_tag_Payment_PaidDate + "</cbc:PaidDate>" + ef;
                            } 
                            /*NUEVO FIN*/                        
                            fila += ei + ei + "</cac:Payment>" + ef;
                            fila += ei + ei + "<sac:SUNATRetentionInformation>" + ef;
                            fila += ei + ei + ei + "<sac:SUNATRetentionAmount currencyID=\"" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId + "\">" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount + "</sac:SUNATRetentionAmount>" + ef;
                            fila += ei + ei + ei + "<sac:SUNATRetentionDate>" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_SUNATRetentionDate + "</sac:SUNATRetentionDate>" + ef;
                            fila += ei + ei + ei + "<sac:SUNATNetTotalPaid currencyID=\"" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId + "\">" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid + "</sac:SUNATNetTotalPaid>" + ef;

                            //Tipo de Cambio -  Moneda
                            if (RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate != "" || RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date != "" || RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode != "" || RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode != "")
                            {
                                fila += ei + ei + ei + "<cac:ExchangeRate>" + ef;
                                if (RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:SourceCurrencyCode>" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode + "</cbc:SourceCurrencyCode>" + ef;
                                }
                                if (RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:TargetCurrencyCode>" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode + "</cbc:TargetCurrencyCode>" + ef;
                                }
                                if (RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:CalculationRate>" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate + "</cbc:CalculationRate>" + ef;
                                }
                                if (RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:Date>" + RetentionLine_item.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date + "</cbc:Date>" + ef;
                                }

                                fila += ei + ei + ei + "</cac:ExchangeRate>" + ef;
                            }
                            //////////
                            fila += ei + ei + "</sac:SUNATRetentionInformation>" + ef;
                            fila += ei + "</sac:SUNATRetentionDocumentReference>" + ef;

                        }
                    }
                    catch (Exception ex)
                    {
                        clsBaseLog.cs_pxRegistarAdd("Error al crear detalle de Retencion" + ex.ToString());
                    }
                }

                #endregion
                fila += "</Retention>" + ef;

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
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCERetention generarxml" + ex.ToString());
            }
            return archivo_xml;
        }
    }
}
