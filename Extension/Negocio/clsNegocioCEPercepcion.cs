using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioCEPercepcion : clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCEPercepcion(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }
        public override string cs_pxGenerarXMLAString(string Id)
        {
            string archivo_xml = string.Empty;
            try
            {
                clsEntityPerception PerceptionDocument = new clsEntityPerception(localbd).cs_fxObtenerUnoPorId(Id);
                List<clsEntityPerception_PerceptionLine> PerceptionLine = new clsEntityPerception_PerceptionLine(localbd).cs_fxObtenerTodoPorCabeceraId(Id);

                string fila = "";
                string ei = "    ";
                string ef = "\n";

                fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
                fila += "<Perception " + ef + ef;
                #region Cabecera
                fila += ei + "xmlns=\"urn:sunat:names:specification:ubl:peru:schema:xsd:Perception-1\"" + ef;
                fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;
                fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
                fila += ei + "xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\"" + ef;
                fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
                fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
                fila += ei + "xmlns:qdt=\"urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2\""+ef;
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
                fila += ei + "<cbc:ID>" + PerceptionDocument.Cs_tag_Id + "</cbc:ID>" + ef;
                fila += ei + "<cbc:IssueDate>" + PerceptionDocument.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
                #region Datos del emisor del documento
                fila += ei + "<cac:AgentParty>" + ef;
                fila += ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + "<cbc:ID schemeID=\"" + PerceptionDocument.Cs_tag_PartyIdentificacion_SchemeId + "\">" + PerceptionDocument.Cs_tag_PartyIdentification_Id + "</cbc:ID>" + ef;
                fila += ei + ei + "</cac:PartyIdentification>" + ef;

                if (PerceptionDocument.Cs_tag_PartyName != "")
                {
                    fila += ei + ei + "<cac:PartyName>" + ef;
                    fila += ei + ei + ei + "<cbc:Name><![CDATA[" + PerceptionDocument.Cs_tag_PartyName + "]]></cbc:Name>" + ef;
                    fila += ei + ei + "</cac:PartyName>" + ef;
                }
               
                if(PerceptionDocument.Cs_tag_PostalAddress_Id!="" || PerceptionDocument.Cs_tag_PostalAddress_StreetName!="" || PerceptionDocument.Cs_tag_PostalAddress_CitySubdivisionName!="" || PerceptionDocument.Cs_tag_PostalAddress_CityName!="" || PerceptionDocument.Cs_tag_PostalAddress_CountrySubEntity!="" || PerceptionDocument.Cs_tag_PostalAddress_District!="" || PerceptionDocument.Cs_tag_PostalAddress_Country_IdentificationCode != "")
                {
                    fila += ei + ei + "<cac:PostalAddress>" + ef;
                    if (PerceptionDocument.Cs_tag_PostalAddress_Id != "")
                    {
                        fila += ei + ei + ei + "<cbc:ID>" + PerceptionDocument.Cs_tag_PostalAddress_Id + "</cbc:ID>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_PostalAddress_StreetName!="")
                    {
                        fila += ei + ei + ei + "<cbc:StreetName>" + PerceptionDocument.Cs_tag_PostalAddress_StreetName + "</cbc:StreetName>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_PostalAddress_CitySubdivisionName!="")
                    {
                        fila += ei + ei + ei + "<cbc:CitySubdivisionName>" + PerceptionDocument.Cs_tag_PostalAddress_CitySubdivisionName + "</cbc:CitySubdivisionName>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_PostalAddress_CityName!="")
                    {
                        fila += ei + ei + ei + "<cbc:CityName>" + PerceptionDocument.Cs_tag_PostalAddress_CityName + "</cbc:CityName>" + ef;
                    }
                    if(PerceptionDocument.Cs_tag_PostalAddress_CountrySubEntity!="")
                    {
                        fila += ei + ei + ei + "<cbc:CountrySubentity>" + PerceptionDocument.Cs_tag_PostalAddress_CountrySubEntity + "</cbc:CountrySubentity>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_PostalAddress_District != "")
                    {
                        fila += ei + ei + ei + "<cbc:District>" + PerceptionDocument.Cs_tag_PostalAddress_District + "</cbc:District>" + ef;
                    }

                    if (PerceptionDocument.Cs_tag_PostalAddress_Country_IdentificationCode != "")
                    {
                        fila += ei + ei + ei + "<cac:Country>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:IdentificationCode>" + PerceptionDocument.Cs_tag_PostalAddress_Country_IdentificationCode + "</cbc:IdentificationCode>" + ef;
                        fila += ei + ei + ei + "</cac:Country>" + ef;
                    }                                                      
                    fila += ei + ei + "</cac:PostalAddress>" + ef;
                }
                
                fila += ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + PerceptionDocument.Cs_tag_PartyLegalEntity_RegistrationName+ "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + "</cac:AgentParty>" + ef;
                #endregion

                #region Info Cliente
                fila += ei + "<cac:ReceiverParty>" + ef;
                fila += ei + ei + "<cac:PartyIdentification>" + ef; 
                fila += ei + ei + ei + "<cbc:ID schemeID=\""+PerceptionDocument.Cs_tag_ReceiveParty_PartyIdentification_SchemeId+"\">"+PerceptionDocument.Cs_tag_ReceiveParty_PartyIdentification_Id+"</cbc:ID>" + ef;
                fila += ei + ei + "</cac:PartyIdentification>" + ef;
                if (PerceptionDocument.Cs_tag_ReceiveParty_PartyName_Name != "")
                {
                    fila += ei + ei + "<cac:PartyName>" + ef;
                    fila += ei + ei + ei + "<cbc:Name><![CDATA[" + PerceptionDocument.Cs_tag_ReceiveParty_PartyName_Name + "]]></cbc:Name>" + ef;
                    fila += ei + ei + "</cac:PartyName>" + ef;
                }
               
                if(PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_Id!=""|| PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName!="" || PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName!="" || PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName!="" || PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity!="" || PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_District!="" || PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode != "")
                {
                    fila += ei + ei + "<cac:PostalAddress>" + ef;
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_Id != "")
                    {
                        fila += ei + ei + ei + "<cbc:ID>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_Id + "</cbc:ID>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName != "")
                    {
                        fila += ei + ei + ei + "<cbc:StreetName>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_StreetName + "</cbc:StreetName>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CitySubdivisionName>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName + "</cbc:CitySubdivisionName>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName != "")
                    {
                        fila += ei + ei + ei + "<cbc:CityName>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CityName + "</cbc:CityName>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity != "")
                    {
                        fila += ei + ei + ei + "<cbc:CountrySubentity>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity + "</cbc:CountrySubentity>" + ef;
                    }
                    if (PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_District != "")
                    {
                        fila += ei + ei + ei + "<cbc:District>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_District + "</cbc:District>" + ef;
                    }
                    
                    fila += ei + ei + ei + "<cac:Country>" + ef;
                    fila += ei + ei + ei + ei + "<cbc:IdentificationCode>" + PerceptionDocument.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode + "</cbc:IdentificationCode>" + ef;
                    fila += ei + ei + ei + "</cac:Country>" + ef;
                    fila += ei + ei + "</cac:PostalAddress>" + ef;
                }
               
                fila += ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + PerceptionDocument.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + "</cac:ReceiverParty>" + ef;
                #endregion

                fila += ei + "<sac:SUNATPerceptionSystemCode>" + PerceptionDocument.Cs_tag_SUNATPerceptionSystemCode+"</sac:SUNATPerceptionSystemCode>" + ef;
                fila += ei + "<sac:SUNATPerceptionPercent>" + PerceptionDocument.Cs_tag_SUNATPerceptionPercent + "</sac:SUNATPerceptionPercent>" + ef;
                if (PerceptionDocument.Cs_tag_Note != ""){
                    fila += ei + "<cbc:Note>" + PerceptionDocument.Cs_tag_Note + "</cbc:Note>" + ef;
                }
               
                fila += ei + "<cbc:TotalInvoiceAmount currencyID=\""+PerceptionDocument.Cs_tag_TotalInvoiceAmount_CurrencyId+"\">"+PerceptionDocument.Cs_tag_TotalInvoiceAmount+"</cbc:TotalInvoiceAmount>" + ef;
                fila += ei + "<sac:SUNATTotalCashed currencyID=\""+PerceptionDocument.Cs_tag_SUNATTotalCashed_CurrencyId+"\">"+PerceptionDocument.Cs_tag_SUNATTotalCashed+"</sac:SUNATTotalCashed>" + ef;

                #region Items
                if (PerceptionLine != null && PerceptionLine.Count > 0)
                {
                    try
                    {
                        foreach (var PerceptionLine_item in PerceptionLine)
                        {
                            fila += ei + "<sac:SUNATPerceptionDocumentReference>" + ef;
                            fila += ei + ei + "<cbc:ID schemeID=\""+PerceptionLine_item.Cs_tag_Id_SchemeId+"\">"+PerceptionLine_item.Cs_tag_Id+"</cbc:ID>" + ef;
                            fila += ei + ei + "<cbc:IssueDate>"+PerceptionLine_item.Cs_tag_IssueDate+"</cbc:IssueDate>" + ef;
                            fila += ei + ei + "<cbc:TotalInvoiceAmount currencyID=\""+PerceptionLine_item.Cs_tag_TotalInvoiceAmount_CurrencyId+"\">"+PerceptionLine_item.Cs_tag_TotalInvoiceAmount+"</cbc:TotalInvoiceAmount>" + ef;
                            fila += ei + ei + "<cac:Payment>" + ef;                          
                            fila += ei + ei + ei + "<cbc:ID>" + PerceptionLine_item.Cs_tag_Payment_Id + "</cbc:ID>" + ef;                           
                            fila += ei + ei + ei + "<cbc:PaidAmount currencyID=\"" + PerceptionLine_item.Cs_tag_Payment_PaidAmount_CurrencyId + "\">" + PerceptionLine_item.Cs_tag_Payment_PaidAmount + "</cbc:PaidAmount>" + ef;
                            fila += ei + ei + ei + "<cbc:PaidDate>" + PerceptionLine_item.Cs_tag_Payment_PaidDate + "</cbc:PaidDate>" + ef;
                            fila += ei + ei + "</cac:Payment>" + ef;
                            fila += ei + ei + "<sac:SUNATPerceptionInformation>" + ef;
                            fila += ei + ei + ei + "<sac:SUNATPerceptionAmount currencyID=\""+PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount_CurrencyId+"\">"+PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount+"</sac:SUNATPerceptionAmount>" + ef;
                            fila += ei + ei + ei + "<sac:SUNATPerceptionDate>" + PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionDate + "</sac:SUNATPerceptionDate>" + ef; 
                            fila += ei + ei + ei + "<sac:SUNATNetTotalCashed currencyID=\""+PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed_CurrencyId+"\">"+PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed+"</sac:SUNATNetTotalCashed>" + ef;

                            if(PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate!="" || PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date!="" || PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode!="" || PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode != "")
                            {
                                fila += ei + ei + ei + "<cac:ExchangeRate>" + ef;
                                if (PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:SourceCurrencyCode>" + PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode + "</cbc:SourceCurrencyCode>" + ef;
                                }
                                if (PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:TargetCurrencyCode>" + PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode + "</cbc:TargetCurrencyCode>" + ef;
                                }
                                if (PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:CalculationRate>" + PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate + "</cbc:CalculationRate>" + ef;
                                }
                                if (PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date != "")
                                {
                                    fila += ei + ei + ei + ei + "<cbc:Date>" + PerceptionLine_item.Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date + "</cbc:Date>" + ef;
                                }
                                
                                fila += ei + ei + ei + "</cac:ExchangeRate>" + ef;
                            }
                            
                            fila += ei + ei + "</sac:SUNATPerceptionInformation>" + ef;
                            fila += ei + "</sac:SUNATPerceptionDocumentReference>" + ef;

                        }
                    }
                    catch (Exception ex)
                    {
                        clsBaseLog.cs_pxRegistarAdd("Error al crear detalle de Percepcion"+ex.ToString());
                    }
                }

                #endregion
                fila += "</Perception>" + ef;

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
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCEPercepcion generarXML"+ex.ToString());
            }
            return archivo_xml;
        }

    }
}
