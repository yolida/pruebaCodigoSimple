using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common;
using Common.Constantes;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonExtensionComponents;
using StructureUBL.SunatAggregateComponents;

namespace StructureUBL.EstandarUbl
{
    [Serializable]
    public class SummaryDocuments : IXmlSerializable, IEstructuraXml
    {
        public UBLExtensions    UBLExtensions   { get; set; }
        public string           Id              { get; set; }
        public string           UblVersionId    { get; set; }
        public string           CustomizationId { get; set; }
        public string           IdInvoice       { get; set; }
        public DateTime         IssueDate       { get; set; }
        public DateTime         ReferenceDate   { get; set; }
        public Signature        Signature       { get; set; }
        public AccountingContributorParty   AccountingSupplierParty { get; set; }
        public List<VoidedDocumentsLine>    SummaryDocumentsLines   { get; set; }
        public IFormatProvider  Formato { get; set; }
        
        public SummaryDocuments()
        {
            UBLExtensions           =   new UBLExtensions();
            Signature               =   new Signature();
            AccountingSupplierParty =   new AccountingContributorParty();
            SummaryDocumentsLines   =   new List<VoidedDocumentsLine>();
            UblVersionId            =   "2.0";
            CustomizationId         =   "1.0";
            Formato = new System.Globalization.CultureInfo(Formatos.Cultura);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("xmlns", EspacioNombres.xmlnsSummaryDocuments);
            writer.WriteAttributeString("xmlns:cac",    EspacioNombres.cac);
            writer.WriteAttributeString("xmlns:cbc",    EspacioNombres.cbc);
            writer.WriteAttributeString("xmlns:ds",     EspacioNombres.ds);
            writer.WriteAttributeString("xmlns:ext",    EspacioNombres.ext);
            writer.WriteAttributeString("xmlns:sac",    EspacioNombres.sac);
            writer.WriteAttributeString("xmlns:xsi",    EspacioNombres.xsi);

            #region UBLExtensions
            writer.WriteStartElement("ext:UBLExtensions");
            {
                writer.WriteStartElement("ext:UBLExtension");
                {
                    writer.WriteStartElement("ext:ExtensionContent");
                    {
                        #region Signature

                        writer.WriteStartElement("cac:Signature");
                        {
                            writer.WriteElementString("cbc:ID", Signature.Id);

                            #region SignatoryParty

                            writer.WriteStartElement("cac:SignatoryParty");

                            writer.WriteStartElement("cac:PartyIdentification");
                            writer.WriteElementString("cbc:ID", Signature.SignatoryParty.PartyIdentification.Id.Value);
                            writer.WriteEndElement();

                            #region PartyName

                            writer.WriteStartElement("cac:PartyName");
                            writer.WriteElementString("cbc:Name", Signature.SignatoryParty.PartyName.Name);
                            writer.WriteEndElement();

                            #endregion PartyName

                            writer.WriteEndElement();

                            #endregion SignatoryParty

                            #region DigitalSignatureAttachment

                            writer.WriteStartElement("cac:DigitalSignatureAttachment");

                            writer.WriteStartElement("cac:ExternalReference");
                            writer.WriteElementString("cbc:URI", Signature.DigitalSignatureAttachment.ExternalReference.Uri.Trim());
                            writer.WriteEndElement();

                            writer.WriteEndElement();

                            #endregion DigitalSignatureAttachment
                        }
                        writer.WriteEndElement();

                        #endregion Signature
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            #endregion UBLExtensions

            writer.WriteElementString("cbc:UBLVersionID",       UblVersionId);
            writer.WriteElementString("cbc:CustomizationID",    CustomizationId);
            writer.WriteElementString("cbc:ID",                 Id);    // Identificador único ex: RC-20181026-2
            writer.WriteElementString("cbc:ReferenceDate",      ReferenceDate.ToString("yyyy-MM-dd"));
            writer.WriteElementString("cbc:IssueDate",          IssueDate.ToString("yyyy-MM-dd"));
            
            #region Signature
            writer.WriteStartElement("cac:Signature");
            {
                writer.WriteElementString("cbc:ID", Signature.Id);

                #region SignatoryParty
                writer.WriteStartElement("cac:SignatoryParty");
                {
                    writer.WriteStartElement("cac:PartyIdentification");
                    writer.WriteElementString("cbc:ID", Signature.SignatoryParty.PartyIdentification.Id.Value);
                    writer.WriteEndElement();

                    #region PartyName
                    writer.WriteStartElement("cac:PartyName");
                    {
                        writer.WriteStartElement("cbc:Name");
                        writer.WriteCData(Signature.SignatoryParty.PartyName.Name);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    #endregion PartyName
                }
                writer.WriteEndElement();
                #endregion SignatoryParty

                #region DigitalSignatureAttachment
                writer.WriteStartElement("cac:DigitalSignatureAttachment");
                {
                    writer.WriteStartElement("cac:ExternalReference");
                    writer.WriteElementString("cbc:URI", Signature.DigitalSignatureAttachment.ExternalReference.Uri.Trim());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion DigitalSignatureAttachment
            }
            writer.WriteEndElement();
            #endregion Signature

            #region AccountingSupplierParty
            writer.WriteStartElement("cac:AccountingSupplierParty");
            {
                writer.WriteElementString("cbc:CustomerAssignedAccountID",  AccountingSupplierParty.CustomerAssignedAccountId);
                writer.WriteElementString("cbc:AdditionalAccountID",        AccountingSupplierParty.AdditionalAccountId);

                #region Party
                writer.WriteStartElement("cac:Party");
                {
                    #region PartyLegalEntity
                    writer.WriteStartElement("cac:PartyLegalEntity");
                    {
                        writer.WriteStartElement("cbc:RegistrationName");
                        writer.WriteCData(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationName);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    #endregion PartyLegalEntity
                }
                writer.WriteEndElement();
                #endregion Party
            }
            writer.WriteEndElement();
            #endregion AccountingSupplierParty

            #region SummaryDocumentsLines

            foreach (var summaryDocumentsLine in SummaryDocumentsLines)
            {
                writer.WriteStartElement("sac:SummaryDocumentsLine");
                {
                    writer.WriteElementString("cbc:LineID",             summaryDocumentsLine.LineId.ToString());
                    writer.WriteElementString("cbc:DocumentTypeCode",   summaryDocumentsLine.DocumentTypeCode);
                    writer.WriteElementString("cbc:ID", summaryDocumentsLine.Id); // Serie correlativo
                    if (!string.IsNullOrEmpty(summaryDocumentsLine.AccountingCustomerParty.AdditionalAccountId))
                    {
                        writer.WriteStartElement("cac:AccountingCustomerParty");
                        {
                            writer.WriteElementString("cbc:CustomerAssignedAccountID", summaryDocumentsLine.AccountingCustomerParty.CustomerAssignedAccountId);
                            writer.WriteElementString("cbc:AdditionalAccountID", summaryDocumentsLine.AccountingCustomerParty.AdditionalAccountId);
                        }
                        writer.WriteEndElement();
                    }
                    if (!string.IsNullOrEmpty(summaryDocumentsLine.BillingReference.InvoiceDocumentReference.Id))
                    {
                        writer.WriteStartElement("cac:BillingReference");   //  Referencia del comprobante modificado
                        {
                            writer.WriteStartElement("cac:InvoiceDocumentReference");
                            {
                                writer.WriteElementString("cbc:ID",                 summaryDocumentsLine.BillingReference.InvoiceDocumentReference.Id);
                                writer.WriteElementString("cbc:DocumentTypeCode",   summaryDocumentsLine.BillingReference.InvoiceDocumentReference.DocumentTypeCode.Value);
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    if (summaryDocumentsLine.ConditionCode.HasValue)
                    {
                        writer.WriteStartElement("cac:Status");
                        {
                            writer.WriteElementString("cbc:ConditionCode", summaryDocumentsLine.ConditionCode.Value.ToString());
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("sac:TotalAmount");
                    {
                        writer.WriteAttributeString("currencyID", summaryDocumentsLine.TotalAmount.CurrencyId);
                        writer.WriteValue(summaryDocumentsLine.TotalAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                    }
                    writer.WriteEndElement();

                    foreach (var billing in summaryDocumentsLine.BillingPayments)
                    {
                        writer.WriteStartElement("sac:BillingPayment");
                        {
                            writer.WriteStartElement("cbc:PaidAmount");
                            {
                                writer.WriteAttributeString("currencyID", summaryDocumentsLine.TotalAmount.CurrencyId);
                                writer.WriteValue(billing.PaidAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                            }
                            writer.WriteEndElement();
                            writer.WriteElementString("cbc:InstructionID", billing.InstructionId);
                        }
                        writer.WriteEndElement();
                    }

                    if (summaryDocumentsLine.AllowanceCharge.Amount.Value > 0)
                    {
                        writer.WriteStartElement("cac:AllowanceCharge");
                        {
                            writer.WriteElementString("cbc:ChargeIndicator", summaryDocumentsLine.AllowanceCharge.ChargeIndicator ? "true" : "false");

                            writer.WriteStartElement("cbc:Amount");
                            {
                                writer.WriteAttributeString("currencyID", summaryDocumentsLine.AllowanceCharge.Amount.CurrencyId);
                                writer.WriteValue(summaryDocumentsLine.AllowanceCharge.Amount.Value.ToString(Formatos.FormatoNumerico, Formato));
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    
                    foreach (var taxTotal in summaryDocumentsLine.TaxTotals)
                    {
                        writer.WriteStartElement("cac:TaxTotal");
                        {
                            writer.WriteStartElement("cbc:TaxAmount");
                            {
                                writer.WriteAttributeString("currencyID", taxTotal.TaxAmount.CurrencyId);
                                writer.WriteString(taxTotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                            }
                            writer.WriteEndElement();

                            #region TaxSubtotal
                            foreach (var taxSubtotal in taxTotal.TaxSubtotals)
                            {
                                writer.WriteStartElement("cac:TaxSubtotal");
                                { 
                                    writer.WriteStartElement("cbc:TaxAmount");
                                    {
                                        writer.WriteAttributeString("currencyID", taxSubtotal.TaxAmount.CurrencyId);
                                        writer.WriteString(taxSubtotal.TaxAmount.Value.ToString(Formatos.FormatoNumerico, Formato));
                                    }
                                    writer.WriteEndElement();

                                    #region TaxCategory
                                    writer.WriteStartElement("cac:TaxCategory");
                                    {
                                        #region TaxScheme
                                        {
                                            writer.WriteStartElement("cac:TaxScheme");

                                            writer.WriteElementString("cbc:ID",             taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.Value);
                                            writer.WriteElementString("cbc:Name",           taxSubtotal.TaxCategory.TaxScheme.Name);
                                            writer.WriteElementString("cbc:TaxTypeCode",    taxSubtotal.TaxCategory.TaxScheme.TaxTypeCode);

                                            writer.WriteEndElement();
                                        }
                                        #endregion TaxScheme
                                    }
                                    writer.WriteEndElement();
                                    #endregion TaxCategory
                                }
                                writer.WriteEndElement();
                            }
                            #endregion TaxSubtotal
                        }
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }

            #endregion SummaryDocumentsLines
        }
    }
}
