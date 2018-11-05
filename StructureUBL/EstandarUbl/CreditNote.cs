using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common;
using Common.Constantes;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonExtensionComponents;

namespace StructureUBL.EstandarUbl
{
    [Serializable]
    public class CreditNote : IXmlSerializable, IEstructuraXml
    {
        public UBLExtensions                            UBLExtensions { get; set; }
        public string                                   Id  { get; set; }
        public Signature                                Signature { get; set; }
        public string                                   UblVersionId { get; set; }
        public CustomizationID                          CustomizationId { get; set; }
        public ProfileID                                ProfileID { get; set; }
        public string                                   IdInvoice  { get; set; } // Serie y número del comprobante
        public DateTime                                 IssueDate { get; set; }
        public string                                   IssueTime { get; set; }
        public DateTime                                 DueDate { get; set; } // Fecha de vencimiento
        public InvoiceTypeCode                          InvoiceTypeCode { get; set; }
        public List<Note>                               Notes { get; set; } // Leyenda
        public DocumentCurrencyCode                     DocumentCurrencyCode { get; set; }
        public List<InvoicePeriod>                      InvoicePeriods { get; set; } // Registrada en la pestaña de F. GRAVADA de Tania -- Volver a implementar :(
        public OrderReference                           OrderReference { get; set; }
        public List<InvoiceDocumentReference>           DespatchDocumentReferences { get; set; }
        //public List<ContractDocumentReference>          ContractDocumentReferences { get; set; } // Sale por petición de Tania
        public List<InvoiceDocumentReference>           AdditionalDocumentReferences  { get; set; }
        public AccountingContributorParty               AccountingSupplierParty { get; set; }
        public AccountingContributorParty               SellerSupplierParty { get; set; }
        public AccountingContributorParty               AccountingCustomerParty { get; set; } // Su estructura es identica a AccountingSupplierParty, por eso se reutiliza la clase
        public List<Delivery>                           Deliveries { get; set; }
        public DeliveryTerms                            DeliveryTerms { get; set; } // Error en la documentación SUNAT, este campo es de 0..1 y NO de 0..*
        public LegalMonetaryTotal                       LegalMonetaryTotal  { get; set; }
        public List<DocumentLine>                       CreditNoteLines { get; set; }
        public List<AllowanceCharge>                    AllowanceCharges { get; set; }
        public List<TaxTotal>                           TaxTotals { get; set; }
        public List<PaymentMeans>                       PaymentsMeans { get; set; }
        public List<PaymentTerms>                       PaymentsTerms { get; set; }
        public IFormatProvider                          Formato { get; set; }
        public List<DiscrepancyResponse>                DiscrepancyResponses { get; set; }
        public List<BillingReference>                   BillingReferences { get; set; }

        public CreditNote()
        {
            UblVersionId                    =   "2.1";
            CustomizationId                 =   new CustomizationID();                             
            InvoiceTypeCode                 =   new InvoiceTypeCode();
            Notes                           =   new List<Note>();
            DocumentCurrencyCode            =   new DocumentCurrencyCode();
            //InvoicePeriods                  = new List<InvoicePeriod>(); // No  esta en el excel del 30/06/2018 | Tania confirma que sale
            OrderReference                  =   new OrderReference(); // Puesto en la parte de extension
            AccountingSupplierParty         =   new AccountingContributorParty(); // Modificado
            AccountingCustomerParty         =   new AccountingContributorParty(); // Modificado
            //PayeeParty                      = new PayeeParty(); // Creado | Amigos vi mal la documentació, esto no va
            Deliveries                      =   new List<Delivery>(); // Puesto en invoiceline y extension
            DeliveryTerms                   =   new DeliveryTerms(); // De 0..1 según OASIS, 0..* según SUNAT | No  esta en el excel del 30/06/2018, se tomará como 0..1
            DespatchDocumentReferences      =   new List<InvoiceDocumentReference>();
            AdditionalDocumentReferences    =   new List<InvoiceDocumentReference>();
            UBLExtensions                   =   new UBLExtensions(); // recontra modificado
            Signature                       =   new Signature(); // No  esta en el excel del 30/06/2018
            CreditNoteLines                 =   new List<DocumentLine>();
            TaxTotals                       =   new List<TaxTotal>();
            LegalMonetaryTotal              =   new LegalMonetaryTotal();
            Formato                         =   new CultureInfo(Formatos.Cultura);
            PaymentsMeans                   =   new List<PaymentMeans>(); // Modificado
            PaymentsTerms                   =   new List<PaymentTerms>();  // No  esta en el excel del 30/06/2018
            AllowanceCharges                =   new List<AllowanceCharge>();
            BillingReferences               =   new List<BillingReference>();
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
            #region xmlns
            writer.WriteAttributeString("xmlns",        EspacioNombres.xmlnsCreditNote);
            writer.WriteAttributeString("xmlns:cac",    EspacioNombres.cac);
            writer.WriteAttributeString("xmlns:cbc",    EspacioNombres.cbc);
            writer.WriteAttributeString("xmlns:ccts",   EspacioNombres.ccts);
            writer.WriteAttributeString("xmlns:ds",     EspacioNombres.ds);
            writer.WriteAttributeString("xmlns:ext",    EspacioNombres.ext);
            writer.WriteAttributeString("xmlns:qdt",    EspacioNombres.qdt);
            writer.WriteAttributeString("xmlns:sac",    EspacioNombres.sac);
            writer.WriteAttributeString("xmlns:udt",    EspacioNombres.udt);
            writer.WriteAttributeString("xmlns:xsi",    EspacioNombres.xsi);
            #endregion xmlns  

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

                            //writer.WriteStartElement("cbc:Name");
                            //writer.WriteCData(Signature.SignatoryParty.PartyName.Name);
                            //writer.WriteEndElement();
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

            #region Version
            writer.WriteElementString("cbc:UBLVersionID", UblVersionId);

            writer.WriteStartElement("cbc:CustomizationID");
            {
                //writer.WriteAttributeString("schemeAgencyName", CustomizationId.schemeAgencyName.ToString());
                writer.WriteValue(CustomizationId.Value.ToString());
            }
            writer.WriteEndElement();
            #endregion Version

            #region Dates Nro Oasis 9, 10
            writer.WriteElementString("cbc:ID", IdInvoice); // 1 | Serie Correlativo
            writer.WriteElementString("cbc:IssueDate", IssueDate.ToString(Formatos.FormatoFecha)); // 1

            if (!string.IsNullOrEmpty(IssueTime.ToString()))
                writer.WriteElementString("cbc:IssueTime", IssueTime.ToString()); // 0..1
            #endregion Dates Nro Oasis 9, 10

            #region DocumentCurrencyCode Nro Oasis 14
            if (!string.IsNullOrEmpty(DocumentCurrencyCode.Value))
            {   // Tipo de moneda
                writer.WriteStartElement("cbc:DocumentCurrencyCode"); // 0..1
                {
                    writer.WriteAttributeString("listID",           DocumentCurrencyCode.ListID);
                    writer.WriteAttributeString("listName",         DocumentCurrencyCode.ListName);
                    writer.WriteAttributeString("listAgencyName",   DocumentCurrencyCode.ListAgencyName);
                    writer.WriteValue(DocumentCurrencyCode.Value);
                }
                writer.WriteEndElement();
            }
            #endregion DocumentCurrencyCode Nro Oasis 14

            #region DiscrepancyResponse Nro Oasis 24
            foreach (var discrepancy in DiscrepancyResponses)
            {
                writer.WriteStartElement("cac:DiscrepancyResponse");
                {
                    writer.WriteElementString("cbc:ResponseCode",   discrepancy.ResponseCode.Value);
                    writer.WriteStartElement("cbc:Description");
                    writer.WriteCData(discrepancy.Description);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            #endregion DiscrepancyResponse

            #region BillingReference Nro Oasis 26
            foreach (var billingReference in BillingReferences)
            {
                writer.WriteStartElement("cac:BillingReference");   //  [0..*]   
                {
                    writer.WriteStartElement("cac:InvoiceDocumentReference");   // [0..1]
                    {
                        writer.WriteElementString("cbc:ID", billingReference.InvoiceDocumentReference.Id);
                        writer.WriteElementString("cbc:DocumentTypeCode", billingReference.InvoiceDocumentReference.DocumentTypeCode.Value);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            #endregion BillingReference Nro Oasis 26

            #region DespatchDocumentReference Nro Oasis 27
            //foreach (var item in DespatchDocumentReferences)    // Puesto en espera
            //{
            //    writer.WriteStartElement("cac:DespatchDocumentReference");
            //    {
            //        writer.WriteElementString("cbc:ID", item.Id);
            //        writer.WriteElementString("cbc:DocumentTypeCode", item.DocumentTypeCode);
            //    }
            //    writer.WriteEndElement();
            //}
            #endregion DespatchDocumentReference Nro Oasis 27

            #region AdditionalDocumentReference Nro Oasis 30
            //foreach (var additionalDocumentReference in AdditionalDocumentReferences)
            //{
            //    writer.WriteStartElement("cac:AdditionalDocumentReference");
            //    {
            //        writer.WriteElementString("cbc:ID",                 additionalDocumentReference.Id);
            //        writer.WriteElementString("cbc:DocumentTypeCode",   additionalDocumentReference.DocumentTypeCode.Value);
            //    }
            //    writer.WriteEndElement();
            //}
            #endregion AdditionalDocumentReference Nro Oasis 30

            #region Signature Nro Oasis 33

            writer.WriteStartElement("cac:Signature");
            writer.WriteElementString("cbc:ID", Signature.Id);

            #region SignatoryParty

            writer.WriteStartElement("cac:SignatoryParty");

            writer.WriteStartElement("cac:PartyIdentification");
            writer.WriteElementString("cbc:ID", Signature.SignatoryParty.PartyIdentification.Id.Value);
            writer.WriteEndElement();

            #region PartyName

            writer.WriteStartElement("cac:PartyName");

            //writer.WriteStartElement("cbc:Name");
            //writer.WriteCData(Signature.SignatoryParty.PartyName.Name);
            //writer.WriteEndElement();
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

            writer.WriteEndElement();

            #endregion Signature Nro Oasis 33

            #region AccountingSupplierParty Nro Oasis 34
            writer.WriteStartElement("cac:AccountingSupplierParty"); // [1..1]  
            {
                #region Party
                writer.WriteStartElement("cac:Party"); // [0..1] 
                {
                    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyIdentification.Id.Value))
                    {   // [0..*] según OASIS, por el momento se tomará como [0..1] ya que esto es consecuente con los sistemas comerciales
                        writer.WriteStartElement("cac:PartyIdentification");
                        {   // Número de RUC
                            writer.WriteStartElement("cbc:ID");
                            {
                                writer.WriteAttributeString("schemeID", AccountingSupplierParty.Party.PartyIdentification.Id.SchemeId);
                                writer.WriteAttributeString("schemeName", AccountingSupplierParty.Party.PartyIdentification.Id.SchemeName);
                                writer.WriteAttributeString("schemeAgencyName	", AccountingSupplierParty.Party.PartyIdentification.Id.SchemeAgencyName);
                                writer.WriteAttributeString("schemeURI	", AccountingSupplierParty.Party.PartyIdentification.Id.SchemeURI);
                                writer.WriteValue(AccountingSupplierParty.Party.PartyIdentification.Id.Value); // Número de RUC
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyName.Name))
                    {
                        writer.WriteStartElement("cac:PartyName"); // [0..*] 
                        {   // Nombre Comercial
                            writer.WriteStartElement("cbc:Name");   //  [1..1]
                            writer.WriteCData(AccountingSupplierParty.Party.PartyName.Name);
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }

                    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationName))
                    {   // Domicilio Fiscal
                        writer.WriteStartElement("cac:PartyLegalEntity"); // OASIS [0..*], SUNAT sin información al respecto | PartyLegalEntity
                        {   // Apellidos y nombres, denominación o razón social
                            writer.WriteStartElement("cbc:RegistrationName");   //  [0..1]
                            writer.WriteValue(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationName);
                            writer.WriteEndElement();

                            #region RegistrationAddress
                            writer.WriteStartElement("cac:RegistrationAddress");
                            {
                                writer.WriteStartElement("cbc:ID"); // Código de ubigeo | (Catálogo No. 13)
                                {
                                    writer.WriteAttributeString("schemeAgencyName", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.SchemeAgencyName); // schemeName
                                    writer.WriteAttributeString("schemeName",       AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.SchemeName);
                                    writer.WriteValue(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.Value); // UBIGEO
                                }

                                writer.WriteEndElement();

                                #region AddressTypeCode
                                writer.WriteStartElement("cbc:AddressTypeCode"); // [0..1]
                                {
                                    //writer.WriteAttributeString("schemeAgencyName", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.schemeAgencyName);
                                    //writer.WriteAttributeString("schemeName",       AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.schemeAgencyName); // esto no debería estar
                                    writer.WriteAttributeString("listAgencyName",   AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.ListAgencyName);
                                    writer.WriteAttributeString("listName",         AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.ListName);
                                    //writer.WriteValue(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.schemeAgencyName);
                                    writer.WriteValue("0000");
                                }
                                writer.WriteEndElement();
                                #endregion AddressTypeCode

                                writer.WriteStartElement("cbc:CitySubdivisionName");    //  Urbanización
                                writer.WriteCData(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.CitySubdivisionName);
                                writer.WriteEndElement();

                                writer.WriteStartElement("cbc:CityName");   //  Provincia
                                writer.WriteCData(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.CityName);
                                writer.WriteEndElement();

                                writer.WriteElementString("cbc:CountrySubentity", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.CountrySubentity); // Departamento
                                writer.WriteElementString("cbc:District", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.District); // Distrito

                                #region AddressLine
                                writer.WriteStartElement("cac:AddressLine");    //  [0..*] 
                                {
                                    writer.WriteStartElement("cbc:Line");   //  (Dirección completa y detallada)
                                    writer.WriteCData(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.AddressLine.Line);
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                #endregion AddressLine

                                #region Country
                                writer.WriteStartElement("cac:Country"); // [0..1] 
                                {   // Catálogo No. 04
                                    writer.WriteStartElement("cbc:IdentificationCode"); // [0..1]
                                    {   // Código de país
                                        writer.WriteAttributeString("listID", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListID);
                                        writer.WriteAttributeString("listAgencyName", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListAgencyName);
                                        writer.WriteAttributeString("listName", AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListName);
                                        writer.WriteValue(AccountingSupplierParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.Value);
                                    }
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                                #endregion Country

                            }
                            writer.WriteEndElement();
                            #endregion RegistrationAddress
                        }
                        writer.WriteEndElement(); // end PartyLegalEntity
                    }
                }
                writer.WriteEndElement();
                #endregion Party
            }
            writer.WriteEndElement();
            #endregion AccountingSupplierParty Nro Oasis 34

            #region AccountingCustomerParty Nro Oasis 35
            writer.WriteStartElement("cac:AccountingCustomerParty"); // [1..1]  
            {
                writer.WriteStartElement("cac:Party"); // [0..1] 
                {   // Número de RUC
                    writer.WriteStartElement("cac:PartyIdentification"); // [0..*] según OASIS, por el momento se tomará como [0..1] ya que esto es consecuente con los sistemas comerciales
                    {
                        writer.WriteStartElement("cbc:ID");
                        {
                            writer.WriteAttributeString("schemeID",         AccountingCustomerParty.Party.PartyIdentification.Id.SchemeId); // Tipo de documento de identidad
                            writer.WriteAttributeString("schemeName",       AccountingCustomerParty.Party.PartyIdentification.Id.SchemeName);
                            writer.WriteAttributeString("schemeAgencyName", AccountingCustomerParty.Party.PartyIdentification.Id.SchemeAgencyName);
                            writer.WriteAttributeString("schemeURI",        AccountingCustomerParty.Party.PartyIdentification.Id.SchemeURI);
                            writer.WriteValue(AccountingCustomerParty.Party.PartyIdentification.Id.Value);  // Número de RUC
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    if (AccountingCustomerParty.Party.PartyLegalEntity != null)
                    {   // Domicilio Fiscal
                        writer.WriteStartElement("cac:PartyLegalEntity"); // OASIS [0..*], SUNAT sin información al respecto | PartyLegalEntity
                        {
                            if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationName))
                            {
                                writer.WriteStartElement("cbc:RegistrationName");   //  [0..1]
                                writer.WriteCData(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationName);
                                writer.WriteEndElement();
                            }

                            #region RegistrationAddress
                            writer.WriteStartElement("cac:RegistrationAddress");
                            {
                                if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.Value))
                                {
                                    writer.WriteStartElement("cbc:ID"); // Código de ubigeo | (Catálogo No. 13)
                                    {
                                        writer.WriteAttributeString("schemeAgencyName", AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.SchemeAgencyName); // schemeName
                                        writer.WriteAttributeString("schemeName",       AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.SchemeName);
                                        writer.WriteValue(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.IdUbigeo.Value); // UBIGEO
                                    }
                                    writer.WriteEndElement();
                                }

                                #region AddressTypeCode
                                //if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.Value))    // Verificar
                                //{
                                //    writer.WriteStartElement("cbc:AddressTypeCode"); // [0..1]
                                //    {
                                //        writer.WriteAttributeString("schemeAgencyName", AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.schemeAgencyName);
                                //        writer.WriteAttributeString("listAgencyName",   AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.ListAgencyName);
                                //        writer.WriteAttributeString("listName",         AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.ListName);
                                //        writer.WriteValue(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressTypeCode.Value);
                                //    }
                                //    writer.WriteEndElement();
                                //}
                                #endregion AddressTypeCode

                                if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CitySubdivisionName))
                                {
                                    writer.WriteStartElement("cbc:CitySubdivisionName"); // Urbanización
                                    writer.WriteCData(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CitySubdivisionName);
                                    writer.WriteEndElement();
                                }

                                if(!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CityName))  // Provincia
                                    writer.WriteElementString("cbc:CityName",   AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CityName);

                                if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CountrySubentity)) // Departamento
                                    writer.WriteElementString("cbc:CountrySubentity", AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.CountrySubentity);

                                if(!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.District))  // Distrito
                                    writer.WriteElementString("cbc:District",   AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.District);

                                #region AddressLine
                                if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressLine.Line))
                                {
                                    writer.WriteStartElement("cac:AddressLine");    //  [0..*] 
                                    {   // (Dirección completa y detallada)
                                        writer.WriteStartElement("cbc:Line");
                                        writer.WriteCData(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.AddressLine.Line);
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                }
                                #endregion AddressLine

                                #region Country
                                if (!string.IsNullOrEmpty(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.Value))
                                {
                                    writer.WriteStartElement("cac:Country");    // [0..1] 
                                    {   // Catálogo No. 04
                                        writer.WriteStartElement("cbc:IdentificationCode"); // [0..1]
                                        {   // Código de país
                                            writer.WriteAttributeString("listID",           AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListID);
                                            writer.WriteAttributeString("listAgencyName",   AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListAgencyName);
                                            writer.WriteAttributeString("listName",         AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.ListName);
                                            writer.WriteValue(AccountingCustomerParty.Party.PartyLegalEntity.RegistrationAddress.Country.IdentificationCode.Value);
                                        }
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                }
                                #endregion Country
                            }
                            writer.WriteEndElement();
                            #endregion RegistrationAddress

                            foreach (var shareholderParty in AccountingCustomerParty.Party.PartyLegalEntity.ShareholderParties)
                            {
                                writer.WriteStartElement("cac:ShareholderParty");
                                {   // Tipo y Número de documento de identidad de otros participantes asociados a la transacción 
                                    writer.WriteStartElement("cac:Party");  // [0..1]  
                                    {   // Apellidos y nombres, denominación o razón social de otros participantes asociados a la transacción
                                        writer.WriteStartElement("cac:PartyIdentification");    // [0..*]
                                        {
                                            writer.WriteStartElement("cbc:ID");
                                            {   // Catálogo No. 06
                                                writer.WriteAttributeString("schemeID",         shareholderParty.Party.PartyIdentification.Id.SchemeId); // Tipo de documento de identidad
                                                writer.WriteAttributeString("schemeName",       shareholderParty.Party.PartyIdentification.Id.SchemeName);
                                                writer.WriteAttributeString("schemeAgencyName", shareholderParty.Party.PartyIdentification.Id.SchemeAgencyName);
                                                writer.WriteAttributeString("schemeURI",        shareholderParty.Party.PartyIdentification.Id.SchemeURI);
                                                writer.WriteValue(shareholderParty.Party.PartyIdentification.Id.Value);  // Número de documento
                                            }
                                            writer.WriteEndElement();
                                        }
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement();
                                }
                                writer.WriteEndElement();
                            }
                            
                            //writer.WriteElementString("cbc:RegistrationName", AccountingCustomerParty.Party.PartyLegalEntity.RegistrationName); // [0..1] | Nombre
                        }
                        writer.WriteEndElement(); // end PartyLegalEntity
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            #endregion AccountingCustomerParty Nro Oasis 35

            #region TaxTotal Nro Oasis 49
            if (TaxTotals.Count > 0)
            {
                foreach (var taxTotal in TaxTotals)
                {
                    if (!string.IsNullOrEmpty(taxTotal.TaxAmount.Value.ToString()))
                    {
                        writer.WriteStartElement("cac:TaxTotal"); // [0..*] TaxTotal
                        {
                            writer.WriteStartElement("cbc:TaxAmount"); // [1..1] Monto total del impuestos
                            {   // Código de tipo de moneda del monto total del tributo
                                writer.WriteAttributeString("currencyID", taxTotal.TaxAmount.CurrencyId);
                                writer.WriteValue(taxTotal.TaxAmount.Value.ToString()); // Monto total del impuestos
                            }
                            writer.WriteEndElement();

                            if (taxTotal.TaxSubtotals.Count > 0) // TaxSubtotal
                            {
                                foreach (var taxSubtotal in taxTotal.TaxSubtotals)
                                {
                                    writer.WriteStartElement("cac:TaxSubtotal"); // [0..*] 
                                    {
                                        if (!string.IsNullOrEmpty(taxSubtotal.TaxAmount.Value.ToString()))
                                        {   // Monto las operaciones gravadas/exoneradas/inafectas del impuesto
                                            writer.WriteStartElement("cbc:TaxableAmount"); // [0..1] TaxableAmount | Importe del tributo
                                            {   // currencyID => Código de tipo de moneda del monto de las operaciones gravadas/exoneradas/inafectas del impuesto
                                                writer.WriteAttributeString("currencyID", taxSubtotal.TaxableAmount.CurrencyId);
                                                writer.WriteValue(taxSubtotal.TaxableAmount.Value.ToString()); // "0.00"
                                            }
                                            writer.WriteEndElement(); // end TaxableAmount
                                        }

                                        writer.WriteStartElement("cbc:TaxAmount"); // [1..1]  Monto total del impuesto
                                        {   // Código de tipo de moneda del monto total del impuesto
                                            writer.WriteAttributeString("currencyID", taxSubtotal.TaxAmount.CurrencyId);
                                            writer.WriteValue(taxSubtotal.TaxAmount.Value.ToString());
                                        }
                                        writer.WriteEndElement();
                                    
                                        writer.WriteStartElement("cac:TaxCategory"); // [1..1]  TaxCategory
                                        {
                                            //writer.WriteStartElement("cbc:ID"); // [0..1]  Categoría de impuestos
                                            //{
                                            //    writer.WriteAttributeString("schemeID",         taxSubtotal.TaxCategory.TaxCategoryId.SchemeID);
                                            //    writer.WriteAttributeString("schemeName",       taxSubtotal.TaxCategory.TaxCategoryId.SchemeName);
                                            //    writer.WriteAttributeString("schemeAgencyName", taxSubtotal.TaxCategory.TaxCategoryId.SchemeAgencyName);
                                            //    writer.WriteValue(taxSubtotal.TaxCategory.TaxCategoryId.Value.ToString());
                                            //}
                                            //writer.WriteEndElement();

                                            writer.WriteStartElement("cac:TaxScheme"); // TaxScheme [1..1]
                                            {
                                                writer.WriteStartElement("cbc:ID"); // TaxScheme [0..1] | Código de tributo
                                                {
                                                    writer.WriteAttributeString("schemeID", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeID);
                                                    writer.WriteAttributeString("schemeName", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeName);
                                                    writer.WriteAttributeString("schemeURI", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeURI); // Nuevo
                                                    writer.WriteAttributeString("schemeAgencyName", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeAgencyName);
                                                    writer.WriteValue(taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.Value.ToString());
                                                }
                                                writer.WriteEndElement();

                                                writer.WriteElementString("cbc:Name", taxSubtotal.TaxCategory.TaxScheme.Name.ToString()); // Nombre de tributo [0..1]
                                                writer.WriteElementString("cbc:TaxTypeCode", taxSubtotal.TaxCategory.TaxScheme.TaxTypeCode.ToString()); // Código internacional tributo [0..1]
                                            }
                                            writer.WriteEndElement(); // end TaxScheme [1..1]
                                        }
                                        writer.WriteEndElement(); // end TaxCategory
                                    }
                                    writer.WriteEndElement();
                                }
                            }   // end TaxSubtotal
                        }   // TaxTotal
                        writer.WriteEndElement();
                    }
                }
            }
            #endregion TaxTotal

            #region LegalMonetaryTotal Nro Oasis 49
            writer.WriteStartElement("cac:LegalMonetaryTotal"); // [1..1] 
            {
                writer.WriteStartElement("cbc:LineExtensionAmount"); // [0..1]  Total valor de venta
                {   // Código de tipo de moneda del total valor de venta
                    writer.WriteAttributeString("currencyID", LegalMonetaryTotal.LineExtensionAmount.CurrencyId.ToString());
                    writer.WriteValue(LegalMonetaryTotal.LineExtensionAmount.Value.ToString()); // Total valor de venta
                }
                writer.WriteEndElement();

                if (LegalMonetaryTotal.TaxInclusiveAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:TaxInclusiveAmount"); // [0..1]  Total precio de venta (incluye impuestos)
                    {   // Código de tipo de moneda del total precio de venta (incluye impuestos)
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.TaxInclusiveAmount.CurrencyId.ToString());
                        writer.WriteValue(LegalMonetaryTotal.TaxInclusiveAmount.Value.ToString()); // Total precio de venta (incluye impuestos)
                    }
                    writer.WriteEndElement();
                }

                if (LegalMonetaryTotal.AllowanceTotalAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:AllowanceTotalAmount");   //  [0..1]  Monto total de descuentos del comprobante
                    {   // Código de tipo de moneda del monto total de descuentos globales del comprobante
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.AllowanceTotalAmount.CurrencyId.ToString());
                        writer.WriteValue(LegalMonetaryTotal.AllowanceTotalAmount.Value.ToString()); // Monto total de descuentos del comprobante
                    }
                    writer.WriteEndElement();
                }

                if (LegalMonetaryTotal.ChargeTotalAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:ChargeTotalAmount");      //  [0..1]  Monto total de otros cargos del comprobante
                    {   // Código de tipo de moneda del monto total de otros cargos del comprobante
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.ChargeTotalAmount.CurrencyId.ToString());
                        writer.WriteValue(LegalMonetaryTotal.ChargeTotalAmount.Value.ToString()); // Monto total de otros cargos del comprobante
                    }
                    writer.WriteEndElement();
                }

                if (LegalMonetaryTotal.PrepaidAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:PrepaidAmount"); // [0..1]  Monto total de anticipos del comprobante
                    {   // Código de tipo de moneda del monto total de anticipos del comprobante
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.PrepaidAmount.CurrencyId.ToString());
                        writer.WriteValue(LegalMonetaryTotal.PrepaidAmount.Value.ToString()); // Monto total de anticipos del comprobante
                    }
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("cbc:PayableAmount"); // [1..1]  Importe total de la venta, cesión en uso o del servicio prestado
                {   // Código tipo de moneda del importe total de la venta, cesión en uso o del servicio prestado
                    writer.WriteAttributeString("currencyID", LegalMonetaryTotal.PayableAmount.CurrencyId.ToString());
                    writer.WriteValue(LegalMonetaryTotal.PayableAmount.Value.ToString()); // Importe total de la venta, cesión en uso o del servicio prestado
                }
                writer.WriteEndElement();

                if (!string.IsNullOrEmpty(LegalMonetaryTotal.PayableRoundingAmount.Value.ToString()) && LegalMonetaryTotal.PayableRoundingAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:PayableRoundingAmount");  // New
                    {
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.PayableRoundingAmount.CurrencyId);
                        writer.WriteValue(LegalMonetaryTotal.PayableRoundingAmount.Value);
                    }
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            #endregion LegalMonetaryTotal Nro Oasis 49

            #region CreditNoteLine Nro Oasis 51
            foreach (var creditNoteLine in CreditNoteLines) // Sin validación porque siempre debe haber al menos uno
            {
                writer.WriteStartElement("cac:CreditNoteLine"); // [1..*] InvoiceLine
                {
                    #region Atributos propios de creditNoteLine 
                    writer.WriteElementString("cbc:ID", creditNoteLine.IdDocumentLine.ToString()); // Número de orden del Ítem

                    #region CreditedQuantity
                    if (!string.IsNullOrEmpty(creditNoteLine.CreditedQuantity.Value.ToString()))
                    {
                        writer.WriteStartElement("cbc:CreditedQuantity"); // [0..1] Cantidad de unidades del ítem
                        {
                            writer.WriteAttributeString("unitCode", creditNoteLine.CreditedQuantity.UnitCode); // Código de unidad de medida del ítem
                            writer.WriteAttributeString("unitCodeListID", creditNoteLine.CreditedQuantity.UnitCodeListID);
                            writer.WriteAttributeString("unitCodeListAgencyName", creditNoteLine.CreditedQuantity.UnitCodeListAgencyName);
                            writer.WriteValue(creditNoteLine.CreditedQuantity.Value.ToString());
                        }
                        writer.WriteEndElement();
                    }
                    #endregion CreditedQuantity

                    #region LineExtensionAmount
                    writer.WriteStartElement("cbc:LineExtensionAmount"); // [1..1] Valor de venta del ítem
                    {   // Código de tipo de moneda del valor de venta del ítem
                        writer.WriteAttributeString("currencyID", creditNoteLine.LineExtensionAmount.CurrencyId.ToString());
                        writer.WriteValue(creditNoteLine.LineExtensionAmount.Value.ToString());
                    }
                    writer.WriteEndElement();
                    #endregion LineExtensionAmount

                    //if (!string.IsNullOrEmpty(invoiceLine.TaxPointDate.ToString())) // Se omitió esta etiqueta en la documentacion EXCEL de SUNAT
                    //    writer.WriteElementString("cbc:TaxPointDate", invoiceLine.TaxPointDate.ToString()); // [0..1]
                    #endregion Atributos propios de creditNoteLine

                    #region CreditNoteLine > PricingReference
                    if (creditNoteLine.PricingReference.AlternativeConditionPrices.Count > 0)
                    {
                        writer.WriteStartElement("cac:PricingReference"); //  [0..1] PricingReference
                        {
                            #region AlternativeConditionPrice
                            if (creditNoteLine.PricingReference.AlternativeConditionPrices.Count > 0)
                            {
                                foreach (var alternativeConditionPrice in creditNoteLine.PricingReference.AlternativeConditionPrices)
                                {
                                    writer.WriteStartElement("cac:AlternativeConditionPrice"); // [0..*] AlternativeConditionPrice
                                    {
                                        writer.WriteStartElement("cbc:PriceAmount"); // [1..1] Precio de venta unitario/ Valor referencial unitario en operaciones no onerosas
                                        {   // Código de tipo de moneda del precio de venta unitario o valor referencial unitario
                                            writer.WriteAttributeString("currencyID", alternativeConditionPrice.PriceAmount.CurrencyId.ToString());
                                            writer.WriteValue(alternativeConditionPrice.PriceAmount.Value.ToString());
                                        }
                                        writer.WriteEndElement();

                                        writer.WriteStartElement("cbc:PriceTypeCode"); // [0..1] Código de tipo de precio
                                        {
                                            writer.WriteAttributeString("listName", alternativeConditionPrice.PriceTypeCode.ListName.ToString());
                                            writer.WriteAttributeString("listAgencyName", alternativeConditionPrice.PriceTypeCode.ListAgencyName.ToString());
                                            writer.WriteAttributeString("listURI", alternativeConditionPrice.PriceTypeCode.ListURI.ToString());
                                            writer.WriteValue(alternativeConditionPrice.PriceTypeCode.Value.ToString());
                                        }
                                        writer.WriteEndElement();
                                    }
                                    writer.WriteEndElement(); // end AlternativeConditionPrice
                                }
                            }
                            #endregion AlternativeConditionPrice
                        }
                        writer.WriteEndElement(); // end PricingReference
                    }
                    #endregion CreditNoteLine > PricingReference
                    #region CreditNoteLine > TaxTotal
                    if (creditNoteLine.TaxTotals.Count > 0)
                    {
                        foreach (var taxTotal in creditNoteLine.TaxTotals)
                        {
                            writer.WriteStartElement("cac:TaxTotal"); // [0..*] 
                            {   // Monto de tributo del ítem
                                writer.WriteStartElement("cbc:TaxAmount"); // [1..1]  | Monto total de impuestos
                                {   // Código de tipo de moneda del monto de tributo del ítem
                                    writer.WriteAttributeString("currencyID", taxTotal.TaxAmount.CurrencyId.ToString());
                                    writer.WriteValue(taxTotal.TaxAmount.Value.ToString()); // Monto de tributo del ítem
                                }
                                writer.WriteEndElement();

                                #region TaxTotal > TaxSubtotal
                                if (taxTotal.TaxSubtotals.Count > 0)   // mejora de Validación pendiente
                                {
                                    foreach (var taxSubtotal in taxTotal.TaxSubtotals)
                                    {
                                        writer.WriteStartElement("cac:TaxSubtotal");    //  [0..*] 
                                        {
                                            if (!string.IsNullOrEmpty(taxSubtotal.TaxableAmount.Value.ToString()))
                                            {
                                                writer.WriteStartElement("cbc:TaxableAmount");  //  [0..1]
                                                {
                                                    writer.WriteAttributeString("currencyID", taxSubtotal.TaxableAmount.CurrencyId);
                                                    writer.WriteValue(taxSubtotal.TaxableAmount.Value);
                                                }
                                                writer.WriteEndElement();
                                            }

                                            if (!string.IsNullOrEmpty(taxSubtotal.TaxAmount.Value.ToString()))
                                            {
                                                writer.WriteStartElement("cbc:TaxAmount");  //  [0..1]
                                                {
                                                    writer.WriteAttributeString("currencyID", taxSubtotal.TaxAmount.CurrencyId);
                                                    writer.WriteValue(taxSubtotal.TaxAmount.Value);
                                                }
                                                writer.WriteEndElement();
                                            }

                                            #region TaxCategory
                                            writer.WriteStartElement("cac:TaxCategory"); // [1..1] 
                                            {
                                                if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.Percent.ToString()))
                                                    writer.WriteElementString("cbc:Percent", Math.Round(taxSubtotal.TaxCategory.Percent, 0).ToString()); // [0..1] 

                                                #region TaxExemptionReasonCode
                                                if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TaxExemptionReasonCode.Value.ToString()))
                                                {
                                                    writer.WriteStartElement("cbc:TaxExemptionReasonCode"); // [0..1] 
                                                    {
                                                        writer.WriteAttributeString("listName", taxSubtotal.TaxCategory.TaxExemptionReasonCode.ListName.ToString());
                                                        writer.WriteAttributeString("listAgencyName", taxSubtotal.TaxCategory.TaxExemptionReasonCode.ListAgencyName.ToString());
                                                        writer.WriteAttributeString("listURI", taxSubtotal.TaxCategory.TaxExemptionReasonCode.ListURI.ToString());
                                                        writer.WriteValue(taxSubtotal.TaxCategory.TaxExemptionReasonCode.Value.ToString());
                                                    }
                                                    writer.WriteEndElement();
                                                }
                                                #endregion TaxExemptionReasonCode

                                                if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TierRange))
                                                    writer.WriteElementString("cbc:TierRange", taxSubtotal.TaxCategory.TierRange); // [0..1]

                                                #region TaxScheme
                                                writer.WriteStartElement("cac:TaxScheme"); // [1..1] 
                                                {
                                                    if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.Value.ToString()))
                                                    {   // Código internacional tributo
                                                        writer.WriteStartElement("cbc:ID"); // [0..1] 
                                                        {
                                                            writer.WriteAttributeString("schemeID",         taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeID);
                                                            writer.WriteAttributeString("schemeName",       taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeName);
                                                            writer.WriteAttributeString("schemeAgencyName", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeAgencyName);
                                                            writer.WriteValue(taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.Value.ToString());
                                                        }
                                                        writer.WriteEndElement();
                                                    }

                                                    if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TaxScheme.Name.ToString()))
                                                        writer.WriteElementString("cbc:Name", taxSubtotal.TaxCategory.TaxScheme.Name.ToString()); // [0..1] | Nombre de tributo

                                                    if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TaxScheme.TaxTypeCode.ToString()))
                                                        writer.WriteElementString("cbc:TaxTypeCode", taxSubtotal.TaxCategory.TaxScheme.TaxTypeCode.ToString()); // [0..1] | Código del tributo

                                                }
                                                writer.WriteEndElement();
                                                #endregion TaxScheme
                                            }
                                            writer.WriteEndElement();
                                            #endregion TaxCategory
                                        }
                                        writer.WriteEndElement();
                                    }
                                }
                                #endregion TaxTotal > TaxSubtotal
                            }
                            writer.WriteEndElement();
                        }
                    }
                    #endregion CreditNoteLine > TaxTotal

                    #region CreditNoteLine > Item
                    writer.WriteStartElement("cac:Item");   //  [1..1] 
                    {
                        #region Item > Description
                        if (creditNoteLine.Item.Descriptions.Count > 0)
                        {   // Descripción(es) detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
                            foreach (var description in creditNoteLine.Item.Descriptions)
                                writer.WriteElementString("cbc:Description", description.Detail.ToString()); // [0..*] 
                        }
                        #endregion Item > Description

                        #region Item > SellersItemIdentification
                        if (!string.IsNullOrEmpty(creditNoteLine.Item.SellersItemIdentification.Id.ToString()))
                        {
                            writer.WriteStartElement("cac:SellersItemIdentification"); // [0..1] 
                            {
                                writer.WriteElementString("cbc:ID", creditNoteLine.Item.SellersItemIdentification.Id.ToString()); // [1..1] | Código de producto del ítem
                            }
                            writer.WriteEndElement();
                        }
                        #endregion Item > SellersItemIdentification

                        #region Item > CommodityClassification
                        if (!string.IsNullOrEmpty(creditNoteLine.Item.CommodityClassification.ItemClassificationCode.Value.ToString()))
                        {   // No es obligatorio
                            writer.WriteStartElement("cac:CommodityClassification"); // [0..1] Según SUNAT, [0..*] según OASIS
                            {   // Código de producto (SUNAT)
                                writer.WriteStartElement("cbc:ItemClassificationCode"); // [1..1] Según SUNAT, [0..1] Según OASIS
                                {
                                    writer.WriteAttributeString("listID", creditNoteLine.Item.CommodityClassification.ItemClassificationCode.ListID.ToString());
                                    writer.WriteAttributeString("listAgencyName", creditNoteLine.Item.CommodityClassification.ItemClassificationCode.ListAgencyName.ToString());
                                    writer.WriteAttributeString("listName", creditNoteLine.Item.CommodityClassification.ItemClassificationCode.ListName.ToString());
                                    writer.WriteValue(creditNoteLine.Item.CommodityClassification.ItemClassificationCode.Value.ToString());
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        #endregion Item > CommodityClassification

                        #region Item > AdditionalItemProperty
                        if (creditNoteLine.Item.AdditionalItemProperties.Count > 0)
                        {
                            foreach (var additionalItemProperty in creditNoteLine.Item.AdditionalItemProperties)
                            {
                                if (!string.IsNullOrEmpty(additionalItemProperty.Name))
                                {
                                    writer.WriteStartElement("cac:AdditionalItemProperty"); // [0..*]
                                    {
                                        writer.WriteElementString("cbc:Name", additionalItemProperty.Name); // [1..1] | Nombre del concepto tributario

                                        #region NameCode
                                        writer.WriteStartElement("cbc:NameCode"); // [0..1] | Código del concepto tributario (del ítem)
                                        {
                                            writer.WriteAttributeString("listName", additionalItemProperty.NameCode.ListName);
                                            writer.WriteAttributeString("listAgencyName", additionalItemProperty.NameCode.ListAgencyName);
                                            writer.WriteAttributeString("listURI", additionalItemProperty.NameCode.ListURI);
                                            writer.WriteValue(additionalItemProperty.NameCode.Value);
                                        }
                                        writer.WriteEndElement();
                                        #endregion NameCode

                                        writer.WriteElementString("cbc:Value", additionalItemProperty.Value); // [0..1] | Valor de la propiedad del ítem

                                        #region ValueQualifier
                                        if (!string.IsNullOrEmpty(additionalItemProperty.ValueQualifier.Detail.ToString())) // [0..*] | Código del concepto del ítem | Cambiar si así lo designan en Lima
                                            writer.WriteElementString("cbc:ValueQualifier", additionalItemProperty.ValueQualifier.Detail.ToString());
                                        #endregion ValueQualifier

                                        #region UsabilityPeriod
                                        writer.WriteStartElement("cac:UsabilityPeriod"); // [0..1] 
                                        {
                                            if (!string.IsNullOrEmpty(additionalItemProperty.UsabilityPeriod.StartDate.ToString())) // Fecha de inicio de la propiedad del ítem
                                                writer.WriteElementString("cbc:StartDate", additionalItemProperty.UsabilityPeriod.StartDate.ToString(Formatos.FormatoFecha));

                                            if (!string.IsNullOrEmpty(additionalItemProperty.UsabilityPeriod.EndDate.ToString())) // Fecha de fin de la propiedad del ítem
                                                writer.WriteElementString("cbc:EndDate", additionalItemProperty.UsabilityPeriod.EndDate.ToString(Formatos.FormatoFecha));

                                            if (!string.IsNullOrEmpty(additionalItemProperty.UsabilityPeriod.DurationMeasure.Value)) // Duración (días) de la propiedad del ítem
                                                writer.WriteElementString("cbc:DurationMeasure", additionalItemProperty.UsabilityPeriod.DurationMeasure.Value);
                                        }
                                        writer.WriteEndElement();
                                        #endregion UsabilityPeriod

                                        #region ValueQuantity
                                        writer.WriteStartElement("cbc:ValueQuantity");  // [0..1]
                                        {
                                            writer.WriteAttributeString("unitCode", additionalItemProperty.ValueQuantity.UnitCode);
                                            writer.WriteValue(additionalItemProperty.ValueQuantity.Value.ToString());
                                        }
                                        writer.WriteEndElement();
                                        #endregion ValueQuantity
                                    }
                                    writer.WriteEndElement();
                                }
                            }
                        }
                        #endregion Item > AdditionalItemProperty
                        #endregion CreditNoteLine > Item
                    }
                    writer.WriteEndElement();

                    #region CreditNoteLine > Price
                    if (!string.IsNullOrEmpty(creditNoteLine.Price.PriceAmount.Value.ToString()))
                    {   // Valor unitario del ítem
                        writer.WriteStartElement("cac:Price"); // [0..1] 
                        {    // Código de tipo de moneda del valor unitario del ítem
                            writer.WriteStartElement("cbc:PriceAmount");
                            {
                                writer.WriteAttributeString("currencyID", creditNoteLine.Price.PriceAmount.CurrencyId);
                                writer.WriteValue(creditNoteLine.Price.PriceAmount.Value.ToString());
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    #endregion CreditNoteLine > Price
                }
                writer.WriteEndElement(); // end InvoiceLine
            }
            #endregion CreditNoteLine Nro Oasis 51
        }
    }
}
