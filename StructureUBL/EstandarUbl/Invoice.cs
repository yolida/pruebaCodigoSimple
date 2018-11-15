using Common;
using Common.Constantes;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonExtensionComponents;
using StructureUBL.SunatAggregateComponents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StructureUBL.EstandarUbl
{
    [Serializable]
    public class Invoice : IXmlSerializable, IEstructuraXml
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

        /// <summary>
        ///  Catálogo 51: Código de tipo de operación
        /// </summary>
        public InvoiceTypeCode                          InvoiceTypeCode { get; set; }
        public List<Note>                               Notes { get; set; } // Leyenda
        public DocumentCurrencyCode                     DocumentCurrencyCode { get; set; }
        public int                                      LineCountNumeric { get; set; }
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
        public List<DocumentLine>                        InvoiceLines { get; set; }
        public List<AllowanceCharge>                    AllowanceCharges { get; set; }
        public List<TaxTotal>                           TaxTotals { get; set; }
        public List<PaymentMeans>                       PaymentsMeans { get; set; }
        public List<PaymentTerms>                       PaymentsTerms { get; set; }
        public List<PrepaidPayment>                     PrepaidPayments { get; set; }
        public IFormatProvider                          Formato { get; set; }
        
        public Invoice()
        {
            UblVersionId                    = "2.1";
            CustomizationId                 = new CustomizationID();                             
            //ProfileID                       = new ProfileID(); // No  esta en el excel del 30/06/2018
            InvoiceTypeCode                 = new InvoiceTypeCode();
            Notes                           = new List<Note>();
            DocumentCurrencyCode            = new DocumentCurrencyCode();
            //InvoicePeriods                  = new List<InvoicePeriod>(); // No  esta en el excel del 30/06/2018 | Tania confirma que sale
            OrderReference                  = new OrderReference(); // Puesto en la parte de extension
            AccountingSupplierParty         = new AccountingContributorParty(); // Modificado
            AccountingCustomerParty         = new AccountingContributorParty(); // Modificado
            //PayeeParty                      = new PayeeParty(); // Creado | Amigos vi mal la documentació, esto no va
            Deliveries                      = new List<Delivery>(); // Puesto en invoiceline y extension
            DeliveryTerms                   = new DeliveryTerms(); // De 0..1 según OASIS, 0..* según SUNAT | No  esta en el excel del 30/06/2018, se tomará como 0..1
            DespatchDocumentReferences      = new List<InvoiceDocumentReference>();
            AdditionalDocumentReferences    = new List<InvoiceDocumentReference>();
            UBLExtensions                   = new UBLExtensions(); // recontra modificado
            Signature                       = new Signature(); // No  esta en el excel del 30/06/2018
            InvoiceLines                    = new List<DocumentLine>();
            TaxTotals                       = new List<TaxTotal>();
            LegalMonetaryTotal              = new LegalMonetaryTotal();
            Formato                         = new CultureInfo(Formatos.Cultura);
            PaymentsMeans                   = new List<PaymentMeans>(); // Modificado
            PaymentsTerms                   = new List<PaymentTerms>();  // No  esta en el excel del 30/06/2018
            PrepaidPayments                 = new List<PrepaidPayment>();
            AllowanceCharges                = new List<AllowanceCharge>();
            //ContractDocumentReferences      = new List<ContractDocumentReference>(); // No  esta en el excel del 30/06/2018 | Tania confirma que sale
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
            writer.WriteAttributeString("xmlns",        EspacioNombres.xmlnsInvoice);
            writer.WriteAttributeString("xmlns:cbc",    EspacioNombres.cbc);
            writer.WriteAttributeString("xmlns:cac",    EspacioNombres.cac);
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

            #region Dates
            writer.WriteElementString("cbc:ID", IdInvoice); // 1 | Serie Correlativo
            writer.WriteElementString("cbc:IssueDate", IssueDate.ToString(Formatos.FormatoFecha)); // 1

            if (!string.IsNullOrEmpty(IssueTime.ToString()))
                writer.WriteElementString("cbc:IssueTime", IssueTime.ToString()); // 0..1

            if (DueDate.ToString() != "01/01/1900 0:00:00")
                writer.WriteElementString("cbc:DueDate", DueDate.ToString(Formatos.FormatoFecha)); // 0..1
            #endregion Dates
            
            #region InvoiceTypeCode
            if (!string.IsNullOrEmpty(InvoiceTypeCode.Value))
            {
                writer.WriteStartElement("cbc:InvoiceTypeCode"); // 0..1
                {
                    writer.WriteAttributeString("listID",           InvoiceTypeCode.ListID);
                    writer.WriteAttributeString("listAgencyName",   InvoiceTypeCode.ListAgencyName);
                    writer.WriteAttributeString("listName",         InvoiceTypeCode.ListName);
                    writer.WriteAttributeString("listURI",          InvoiceTypeCode.ListURI);
                    //writer.WriteAttributeString("name",             InvoiceTypeCode.Name);
                    //writer.WriteAttributeString("listSchemeURI",    InvoiceTypeCode.ListSchemeURI);
                    writer.WriteValue(InvoiceTypeCode.Value);
                }
                writer.WriteEndElement();
            }
            #endregion InvoiceTypeCode
            
            #region Note
            if (Notes.Count > 0)
            {
                foreach (var note in Notes)
                {
                    writer.WriteStartElement("cbc:Note"); // 0..n
                    {
                        writer.WriteAttributeString("languageLocaleID", note.LanguageLocaleID);
                        writer.WriteCData(note.Value);
                    }
                    writer.WriteEndElement();
                }
            }
            #endregion Note

            #region DocumentCurrencyCode
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
            #endregion DocumentCurrencyCode

            writer.WriteElementString("cbc:LineCountNumeric", LineCountNumeric.ToString()); // Cantidad de ítems de la factura

            #region Signature

            writer.WriteStartElement("cac:Signature");
            writer.WriteElementString("cbc:ID", Signature.Id);

            #region SignatoryParty

            writer.WriteStartElement("cac:SignatoryParty");

            writer.WriteStartElement("cac:PartyIdentification");
            writer.WriteElementString("cbc:ID", Signature.SignatoryParty.PartyIdentification.Id.Value);
            writer.WriteEndElement();

            #region PartyName

            writer.WriteStartElement("cac:PartyName");

            writer.WriteStartElement("cbc:Name");
            writer.WriteCData(Signature.SignatoryParty.PartyName.Name);
            writer.WriteEndElement();

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

            #endregion Signature

            //#region InvoicePeriod
            ////if (InvoicePeriods.Count > 0) // Sale por la documentación de Sunat del 30/06/2018
            ////{
            ////    foreach (var invoicePeriod in InvoicePeriods)
            ////    {
            ////        writer.WriteStartElement("cac:InvoicePeriod"); // 0..n
            ////        {
            ////            if (!string.IsNullOrEmpty(invoicePeriod.StartDate.ToString().ToString()))
            ////                writer.WriteElementString("cbc:StartDate", invoicePeriod.StartDate.ToString(Formatos.FormatoFecha)); // 0..1

            ////            if (!string.IsNullOrEmpty(invoicePeriod.EndDate.ToString().ToString()))
            ////                writer.WriteElementString("cbc:EndDate", invoicePeriod.EndDate.ToString(Formatos.FormatoFecha)); // 0..1
            ////        }
            ////        writer.WriteEndElement();
            ////    }
            ////}
            //#endregion InvoicePeriod

            //#region OrderReference
            //if (!string.IsNullOrEmpty(OrderReference.Id))
            //{
            //    writer.WriteStartElement("cac:OrderReference"); // 0..1
            //    {
            //        writer.WriteElementString("cbc:ID", OrderReference.Id);
            //    }
            //    writer.WriteEndElement();
            //}
            //#endregion OrderReference

            //#region DespatchDocumentReference
            //if (DespatchDocumentReferences.Count > 0)
            //{
            //    foreach (var despatchDocumentReference in DespatchDocumentReferences) // 0..n
            //    {   // Tipo y número de la guía de remisión relacionada
            //        writer.WriteStartElement("cac:DespatchDocumentReference");
            //        {
            //            writer.WriteElementString("cbc:ID", despatchDocumentReference.Id);  // Número de documento
            //            if (!string.IsNullOrEmpty(despatchDocumentReference.DocumentTypeCode.Value.ToString()))
            //            {   // Tipo de guía relacionado | Catálogo No. 01
            //                writer.WriteStartElement("cbc:DocumentTypeCode");
            //                {
            //                    writer.WriteAttributeString("listAgencyName",   despatchDocumentReference.DocumentTypeCode.ListAgencyName);
            //                    writer.WriteAttributeString("listName",         despatchDocumentReference.DocumentTypeCode.ListName);   // Tipo de Documento
            //                    writer.WriteAttributeString("listURI",          despatchDocumentReference.DocumentTypeCode.ListURI);
            //                    writer.WriteValue(despatchDocumentReference.DocumentTypeCode.Value);
            //                }
            //                writer.WriteEndElement();
            //            }
            //        }
            //        writer.WriteEndElement();
            //    }
            //}
            //#endregion DespatchDocumentReference

            //#region ContractDocumentReference
            ////if (ContractDocumentReferences.Count > 0) // Get out ContractDocumentReference | Tania confirma que sale
            ////{
            ////    foreach (var contractDocumentReference in ContractDocumentReferences)
            ////    {
            ////        writer.WriteStartElement("cac:ContractDocumentReference"); // [0..*] 
            ////        {
            ////            writer.WriteElementString("cbc:ID", contractDocumentReference.Id); // Número de suministro/Número de teléfono
            ////            if(!string.IsNullOrEmpty(contractDocumentReference.DocumentTypeCode.Value.ToString()))
            ////            {
            ////                writer.WriteStartElement("cbc:DocumentTypeCode"); // Tipo de Servicio Público
            ////                {
            ////                    writer.WriteAttributeString("listAgencyName", contractDocumentReference.DocumentTypeCode.ListAgencyName);
            ////                    writer.WriteAttributeString("listName", contractDocumentReference.DocumentTypeCode.ListName);
            ////                    writer.WriteAttributeString("listURI", contractDocumentReference.DocumentTypeCode.ListURI);
            ////                    writer.WriteValue(contractDocumentReference.DocumentTypeCode.Value);
            ////                }
            ////                writer.WriteEndElement();
            ////            }
            ////            if (!string.IsNullOrEmpty(contractDocumentReference.LocaleCode.Value.ToString()))
            ////            {
            ////                writer.WriteStartElement("cbc:LocaleCode"); // Código de Servicios de Telecomunicaciones (De corresponder)
            ////                {
            ////                    writer.WriteAttributeString("listAgencyName", contractDocumentReference.LocaleCode.ListAgencyName);
            ////                    writer.WriteAttributeString("listName", contractDocumentReference.LocaleCode.ListName);
            ////                    writer.WriteAttributeString("listURI", contractDocumentReference.LocaleCode.ListURI);
            ////                    writer.WriteValue(contractDocumentReference.LocaleCode.Value);
            ////                }
            ////                writer.WriteEndElement();
            ////            }
            ////            if (!string.IsNullOrEmpty(contractDocumentReference.DocumentStatusCode.Value.ToString()))
            ////            {
            ////                writer.WriteStartElement("cbc:DocumentStatusCode"); // Código de Tipo de Tarifa contratada
            ////                {
            ////                    writer.WriteAttributeString("listAgencyName", contractDocumentReference.DocumentStatusCode.ListAgencyName);
            ////                    writer.WriteAttributeString("listName", contractDocumentReference.DocumentStatusCode.ListName);
            ////                    writer.WriteAttributeString("listURI", contractDocumentReference.DocumentStatusCode.ListURI);
            ////                    writer.WriteValue(contractDocumentReference.DocumentStatusCode.Value);
            ////                }
            ////                writer.WriteEndElement();
            ////            }
            ////        }
            ////        writer.WriteEndElement();
            ////    }
            ////}
            //#endregion ContractDocumentReference

            //#region AdditionalDocumentReference
            //if (AdditionalDocumentReferences.Count > 0)
            //{
            //    foreach (var additionalDocumentReference in AdditionalDocumentReferences)
            //    {   // Tipo y número de otro documento relacionado
            //        writer.WriteStartElement("cac:AdditionalDocumentReference"); // 0..n
            //        {   // Número de documento relacionado con la operación que se factura. (Serie y Número de comprobante que se realizó el anticipo).
            //            writer.WriteElementString("cbc:ID", additionalDocumentReference.Id); // 1

            //            if (!string.IsNullOrEmpty(additionalDocumentReference.DocumentTypeCode.Value.ToString()))
            //            {   // Código de tipo de documento relacionado con la operación que se factura. Catalogo 12. | Factura - Emitida por Anticipo = "02" | Boleta - Emitida por Anticipo = "03"
            //                writer.WriteStartElement("cbc:DocumentTypeCode");
            //                {   // Catálogo No. 12
            //                    writer.WriteAttributeString("listAgencyName",   additionalDocumentReference.DocumentTypeCode.ListAgencyName);
            //                    writer.WriteAttributeString("listName",         additionalDocumentReference.DocumentTypeCode.ListName);
            //                    writer.WriteAttributeString("listURI",          additionalDocumentReference.DocumentTypeCode.ListURI);
            //                    writer.WriteValue(additionalDocumentReference.DocumentTypeCode.Value);
            //                }
            //                writer.WriteEndElement();
            //            }

            //            if (additionalDocumentReference.IssuerParty.PartyIdentifications.Count > 0) // New
            //            {
            //                writer.WriteStartElement("cac:IssuerParty");    // [0..1]
            //                {
            //                    foreach (var partyIdentification in additionalDocumentReference.IssuerParty.PartyIdentifications)
            //                    {
            //                        if (string.IsNullOrEmpty(partyIdentification.Id.Value))
            //                        {
            //                            writer.WriteStartElement("cac:PartyIdentification");    // [0..*]
            //                            {
            //                                writer.WriteStartElement("cbc:ID");    // [1..1] 
            //                                {
            //                                    writer.WriteAttributeString("schemeId", partyIdentification.Id.SchemeId);   // Tipo de documento del emisor del anticipo | Catálogo No. 06
            //                                    writer.WriteAttributeString("schemeName",       partyIdentification.Id.SchemeName);
            //                                    writer.WriteAttributeString("schemeAgencyName", partyIdentification.Id.SchemeAgencyName);
            //                                    writer.WriteAttributeString("schemeURI",        partyIdentification.Id.SchemeURI);
            //                                    writer.WriteValue(partyIdentification.Id.Value);    // Número de documento del emisor del anticipo
            //                                }
            //                                writer.WriteEndElement();
            //                            }
            //                            writer.WriteEndElement();
            //                        }
            //                    }
            //                }
            //                writer.WriteEndElement();
            //            }
            //        }
            //        writer.WriteEndElement();
            //    }
            //}
            //#endregion AdditionalDocumentReference

            #region AccountingSupplierParty
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

                    #region PartyTaxScheme
                    //A partir del documento de SUNAT emitido el 2018 / 06 / 30, la etiqueta PartyTaxScheme desaparece del nodo de AccountingSupplierParty
                    //writer.WriteStartElement("cac:PartyTaxScheme"); // [0..*] Se esta obviando el de 0 a muchos y se asume un [0..1]
                    //{
                    //    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyTaxScheme.RegistrationName))
                    //        writer.WriteElementString("cbc:RegistrationName", AccountingSupplierParty.Party.PartyTaxScheme.RegistrationName); //  [0..1]

                    //    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.Value))
                    //    {
                    //        writer.WriteStartElement("cbc:CompanyID"); // 0..1
                    //        {
                    //            writer.WriteAttributeString("schemeID", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeID);
                    //            writer.WriteAttributeString("schemeName", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeName);
                    //            writer.WriteAttributeString("schemeAgencyName", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeAgencyName);
                    //            writer.WriteAttributeString("schemeURI", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeURI);
                    //            writer.WriteValue(AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.Value);
                    //        }
                    //        writer.WriteEndElement();
                    //    }

                    //    if (!string.IsNullOrEmpty(AccountingSupplierParty.Party.PartyTaxScheme.RegistrationAddress.AddressTypeCode.ToString()))
                    //    {
                    //        writer.WriteStartElement("cac:RegistrationAddress"); // [0..1]
                    //        {   // Código del domicilio fiscal o de local anexo del emisor n4
                    //            writer.WriteElementString("cbc:AddressTypeCode",
                    //                AccountingSupplierParty.Party.PartyTaxScheme.RegistrationAddress.AddressTypeCode.Value); // [0..1] 
                    //        }
                    //        writer.WriteEndElement();
                    //    }

                    //    #region esto facil debe salir esta para pruebas
                    //    writer.WriteStartElement("cac:TaxScheme"); // TaxScheme [1..1]
                    //    {
                    //        writer.WriteStartElement("cbc:ID"); // TaxScheme [0..1] | Código de tributo
                    //        {
                    //            writer.WriteAttributeString("schemeID", "6");
                    //            writer.WriteAttributeString("schemeName", "SUNAT:Identificador de Documento de Identidad");
                    //            writer.WriteAttributeString("schemeAgencyName", "PE:SUNAT");
                    //            writer.WriteAttributeString("schemeURI", "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"); // Nuevo
                    //            writer.WriteValue("20000000001");
                    //        }
                    //        writer.WriteEndElement();
                    //    }
                    //    writer.WriteEndElement(); // end TaxScheme [1..1]
                    //    #endregion esto facil debe salir esta para pruebas
                    //}
                    //writer.WriteEndElement();
                    #endregion PartyTaxScheme

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
            #endregion AccountingSupplierParty

            #region AccountingCustomerParty
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

                    #region PartyTaxScheme
                    //writer.WriteStartElement("cac:PartyTaxScheme"); // [0..*] 
                    //{
                    //    writer.WriteElementString("cbc:RegistrationName", AccountingSupplierParty.Party.PartyTaxScheme.RegistrationName); //  [0..1]
                    //    writer.WriteStartElement("cbc:CompanyID");
                    //    {
                    //        writer.WriteAttributeString("schemeID", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeID);
                    //        writer.WriteAttributeString("schemeName", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeName);
                    //        writer.WriteAttributeString("schemeAgencyName", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeAgencyName);
                    //        writer.WriteAttributeString("schemeURI", AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.SchemeURI);
                    //        writer.WriteValue(AccountingSupplierParty.Party.PartyTaxScheme.CompanyID.Value);
                    //    }
                    //    writer.WriteEndElement();
                    //}
                    //writer.WriteEndElement();
                    #endregion PartyTaxScheme

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
            #endregion AccountingCustomerParty

            //#region Delivery
            //if (Deliveries.Count > 0)
            //{
            //    foreach (var delivery in Deliveries)
            //    {
            //        writer.WriteStartElement("cac:Delivery"); // Entrega
            //        {
            //            #region ID
            //            if (!string.IsNullOrEmpty(delivery.DeliveryId.Value.ToString()))
            //            {
            //                writer.WriteStartElement("cbc:ID"); // 0..1
            //                {
            //                    writer.WriteAttributeString("schemeID",         delivery.DeliveryId.SchemeID.ToString());
            //                    writer.WriteAttributeString("schemeName",       delivery.DeliveryId.SchemeName.ToString());
            //                    writer.WriteAttributeString("schemeAgencyName", delivery.DeliveryId.SchemeAgencyName.ToString());
            //                    writer.WriteAttributeString("schemeURI",        delivery.DeliveryId.SchemeURI.ToString());
            //                    writer.WriteValue(delivery.DeliveryId.Value.ToString());
            //                }
            //                writer.WriteEndElement();
            //            }
            //            #endregion ID

            //            #region Quantity
            //            if (!string.IsNullOrEmpty(delivery.Quantity.Value.ToString()))
            //            {
            //                writer.WriteStartElement("cbc:Quantity"); // 0..1
            //                {
            //                    writer.WriteAttributeString("unitCode", delivery.Quantity.UnitCode.ToString());
            //                    writer.WriteAttributeString("unitCodeListID", delivery.Quantity.UnitCodeListID.ToString());
            //                    writer.WriteAttributeString("unitCodeListAgencyName", delivery.Quantity.UnitCodeListAgencyName.ToString());
            //                    writer.WriteValue(delivery.Quantity.Value.ToString());
            //                }
            //                writer.WriteEndElement();
            //            }
            //            #endregion Quantity

            //            #region MaximumQuantity
            //            if (!string.IsNullOrEmpty(delivery.MaximumQuantity.Value.ToString()))
            //            {
            //                writer.WriteStartElement("cbc:MaximumQuantity"); // 0..1
            //                {
            //                    writer.WriteAttributeString("unitCode", delivery.MaximumQuantity.UnitCode.ToString());
            //                    writer.WriteAttributeString("unitCodeListID", delivery.MaximumQuantity.UnitCodeListID.ToString());
            //                    writer.WriteAttributeString("unitCodeListAgencyName", delivery.MaximumQuantity.UnitCodeListAgencyName.ToString());
            //                    writer.WriteValue(delivery.MaximumQuantity.Value.ToString());
            //                }
            //                writer.WriteEndElement();
            //            }
            //            #endregion MaximumQuantity

            //            #region DeliveryLocation
            //            writer.WriteStartElement("cac:DeliveryLocation"); // [0..1]
            //            {
            //                writer.WriteStartElement("cac:Address"); // [0..1]  
            //                {
            //                    if (!string.IsNullOrEmpty(delivery.DeliveryLocation.Address.AddressLine.Line))
            //                    {
            //                        writer.WriteStartElement("cac:AddressLine"); // Según OASIS [0..*], pero se tomará como un [0..1]
            //                        {   // Dirección completa y detallada
            //                            writer.WriteElementString("cbc:Line", delivery.DeliveryLocation.Address.AddressLine.Line); // [1..1]
            //                        }
            //                        writer.WriteEndElement();
            //                    }

            //                    writer.WriteElementString("cbc:CitySubdivisionName", delivery.DeliveryLocation.Address.CitySubdivisionName.ToString()); // [0..1] | Urbanización
            //                    writer.WriteElementString("cbc:CityName ",  delivery.DeliveryLocation.Address.CityName.ToString()); // [0..1] | Provincia

            //                    #region Id
            //                    if (!string.IsNullOrEmpty(delivery.DeliveryLocation.Address.Id.Value.ToString()))
            //                    {   // Código de ubigeo
            //                        writer.WriteStartElement("cbc:ID"); // [0..1]
            //                        {   // Catálogo No. 13
            //                            writer.WriteAttributeString("schemeAgencyName", delivery.DeliveryLocation.Address.Id.SchemeAgencyName.ToString());
            //                            writer.WriteAttributeString("schemeName",   delivery.DeliveryLocation.Address.Id.SchemeName.ToString()); // Ubigeos
            //                            writer.WriteValue(delivery.DeliveryLocation.Address.Id.Value.ToString());
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    #endregion Id
                                    
            //                    writer.WriteElementString("cbc:CountrySubentity", delivery.DeliveryLocation.Address.CitySubdivisionName.ToString()); // [0..1]
            //                    writer.WriteElementString("cbc:District",   delivery.DeliveryLocation.Address.CitySubdivisionName.ToString()); // [0..1]

            //                    #region Country
            //                    if (!string.IsNullOrEmpty(delivery.DeliveryLocation.Address.Country.IdentificationCode.Value.ToString()))
            //                    {   // Catálogo No. 04
            //                        writer.WriteStartElement("cac:Country"); // [0..1] 
            //                        {   // Código de país
            //                            writer.WriteStartElement("cbc:IdentificationCode"); // [0..1] 
            //                            {
            //                                writer.WriteAttributeString("listID",           delivery.DeliveryLocation.Address.Country.IdentificationCode.ListID.ToString());
            //                                writer.WriteAttributeString("listAgencyName",   delivery.DeliveryLocation.Address.Country.IdentificationCode.ListAgencyName.ToString());
            //                                writer.WriteAttributeString("listName",         delivery.DeliveryLocation.Address.Country.IdentificationCode.ListName.ToString());
            //                                writer.WriteValue(delivery.DeliveryLocation.Address.Country.IdentificationCode.Value.ToString());
            //                            }
            //                            writer.WriteEndElement();
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    #endregion Country
            //                }
            //                writer.WriteEndElement();
            //            }
            //            writer.WriteEndElement();
            //            #endregion DeliveryLocation

            //            #region DeliveryParty
            //            writer.WriteStartElement("cac:DeliveryParty"); // [0..1] 
            //            {
            //                foreach (var partyLegalEntity in delivery.DeliveryParty.PartyLegalEntities)
            //                {
            //                    writer.WriteStartElement("cac:PartyLegalEntity"); // [0..*]
            //                    {   // Número de documento de identidad del destinatario
            //                        writer.WriteStartElement("cbc:CompanyID"); // [0..1]
            //                        {
            //                            writer.WriteAttributeString("schemeID",         partyLegalEntity.CompanyID.SchemeID); // Código de tipo de documento de identidad del destinatario
            //                            writer.WriteAttributeString("schemeName",       partyLegalEntity.CompanyID.SchemeName);
            //                            writer.WriteAttributeString("schemeAgencyName", partyLegalEntity.CompanyID.SchemeAgencyName);
            //                            writer.WriteAttributeString("schemeURI",        partyLegalEntity.CompanyID.SchemeURI);
            //                            writer.WriteValue(partyLegalEntity.CompanyID.Value); // Número de documento de identidad del destinatario
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //            }
            //            writer.WriteEndElement();
            //            #endregion DeliveryParty

            //            #region Shipment
            //            writer.WriteStartElement("cac:Shipment"); // [0..1] | Envío
            //            {
            //                #region ID
            //                writer.WriteStartElement("cbc:ID"); // Código de motivo de traslado
            //                {   // Catálogo No. 20
            //                    writer.WriteAttributeString("schemeName",       delivery.Shipment.Id.SchemeName);
            //                    writer.WriteAttributeString("schemeAgencyName", delivery.Shipment.Id.SchemeAgencyName);
            //                    writer.WriteAttributeString("schemeURI",        delivery.Shipment.Id.SchemeURI);
            //                    writer.WriteValue(delivery.Shipment.Id.Value);
            //                }
            //                writer.WriteEndElement();
            //                #endregion ID

            //                #region GrossWeightMeasure
            //                writer.WriteStartElement("cbc:GrossWeightMeasure"); // manual | [0..1]
            //                {   // Peso bruto total de la Factura
            //                    writer.WriteAttributeString("unitCode",                 delivery.Shipment.GrossWeightMeasure.UnitCode); // Catálogo No. 03
            //                    writer.WriteAttributeString("unitCodeListVersionID",    delivery.Shipment.GrossWeightMeasure.UnitCodeListVersionID);
            //                }
            //                writer.WriteEndElement();
            //                #endregion GrossWeightMeasure

            //                #region ShipmentStage
            //                if (delivery.Shipment.ShipmentStages.Count > 0)
            //                {
            //                    foreach (var shipmentStage in delivery.Shipment.ShipmentStages)
            //                    {
            //                        writer.WriteStartElement("cac:ShipmentStage"); // [0..*] | Etapa de envío
            //                        {
            //                            #region TransportModeCode
            //                            writer.WriteStartElement("cbc:TransportModeCode"); // [0..1] 
            //                            {   // Modalidad de Transporte. Dato exclusivo para la Factura Guía Remitente (FG Remitente)
            //                                writer.WriteAttributeString("listName",         shipmentStage.TransportModeCode.ListName);
            //                                writer.WriteAttributeString("listAgencyName",   shipmentStage.TransportModeCode.ListAgencyName);
            //                                writer.WriteAttributeString("listURI",          shipmentStage.TransportModeCode.ListURI);
            //                                writer.WriteValue(shipmentStage.TransportModeCode.Value);
            //                            }
            //                            writer.WriteEndElement();
            //                            #endregion TransportModeCode

            //                            #region TransitPeriod
            //                            writer.WriteStartElement("cac:TransitPeriod"); // [0..1] 
            //                            {   // Fecha de inicio del traslado o fecha de entrega de bienes al transportista
            //                                writer.WriteElementString("cbc:StartDate", shipmentStage.TransitPeriod.StartDate.ToString(Formatos.FormatoFecha)); // [0..1] 
            //                            }
            //                            writer.WriteEndElement();
            //                            #endregion TransitPeriod

            //                            #region CarrierParty
            //                            if (shipmentStage.CarrierParties.Count > 0)
            //                            {
            //                                foreach (var carrierParty in shipmentStage.CarrierParties)
            //                                {
            //                                    writer.WriteStartElement("cac:CarrierParty"); // [0..*]
            //                                    {
            //                                        if (string.IsNullOrEmpty(carrierParty.PartyIdentification.Id.Value))
            //                                        {
            //                                            writer.WriteStartElement("cac:PartyIdentification"); // [0..*] según OASIS, pero se tomará como [0..1], ya que, CarrierParty ya es [0..*]
            //                                            {
            //                                                writer.WriteStartElement("cbc:ID");
            //                                                {
            //                                                    writer.WriteAttributeString("SchemeID",         carrierParty.PartyIdentification.Id.SchemeId); // Tipo de documento de identidad
            //                                                    writer.WriteAttributeString("SchemeName",       carrierParty.PartyIdentification.Id.SchemeName);
            //                                                    writer.WriteAttributeString("SchemeAgencyName", carrierParty.PartyIdentification.Id.SchemeAgencyName);
            //                                                    writer.WriteAttributeString("SchemeId",         carrierParty.PartyIdentification.Id.SchemeURI);
            //                                                    writer.WriteValue(carrierParty.PartyIdentification.Id.Value);    // Número de documento de identidad
            //                                                }
            //                                                writer.WriteEndElement();
            //                                            }
            //                                            writer.WriteEndElement();
            //                                        }

            //                                        if (string.IsNullOrEmpty(carrierParty.PartyLegalEntity.RegistrationName))
            //                                        {
            //                                            writer.WriteStartElement("cac:PartyLegalEntity");   // [0..*] según OASIS, pero se tomará como [0..1], ya que, CarrierParty ya es [0..*]
            //                                            {   // Datos del Transportista (FG Remitente) o Transportista contratante (FG Transportista) - Apellidos y nombres o razón social
            //                                                writer.WriteElementString("cbc:RegistrationName",   carrierParty.PartyLegalEntity.RegistrationName);   // [0..1]
            //                                            }
            //                                            writer.WriteEndElement();
            //                                        }
            //                                    }
            //                                    writer.WriteEndElement();
            //                                }
            //                            }
            //                            #endregion CarrierParty

            //                            #region TransportMeans
            //                            if (string.IsNullOrEmpty(shipmentStage.TransportMeans.RoadTransport.LicensePlateID))
            //                            {
            //                                writer.WriteStartElement("cac:TransportMeans"); // [0..1]
            //                                {
            //                                    writer.WriteElementString("cbc:RegistrationNationalityID", shipmentStage.TransportMeans.RegistrationNationalityID); // [0..1] 
            //                                    writer.WriteElementString("cac:RoadTransport", shipmentStage.TransportMeans.RoadTransport.LicensePlateID);  // [1..1]
            //                                }
            //                                writer.WriteEndElement();
            //                            }
            //                            #endregion TransportMeans

            //                            #region DriverPerson
            //                            if (shipmentStage.DriverPersons.Count > 0)
            //                            {
            //                                foreach (var driverPerson in shipmentStage.DriverPersons)
            //                                {
            //                                    if (!string.IsNullOrEmpty(driverPerson.Id.Value))
            //                                    {
            //                                        writer.WriteStartElement("cac:DriverPerson");   // [0..*] 
            //                                        {
            //                                            writer.WriteStartElement("cbc:ID"); // [0..1] 
            //                                            {
            //                                                writer.WriteAttributeString("schemeId",         driverPerson.Id.SchemeId);
            //                                                writer.WriteAttributeString("schemeName",       driverPerson.Id.SchemeName);
            //                                                writer.WriteAttributeString("schemeAgencyName", driverPerson.Id.SchemeAgencyName);
            //                                                writer.WriteAttributeString("schemeURI",        driverPerson.Id.SchemeURI);
            //                                                writer.WriteValue(driverPerson.Id.Value);   // Datos de conductores - Número de documento de identidad
            //                                            }
            //                                            writer.WriteEndElement();
            //                                        }
            //                                        writer.WriteEndElement();
            //                                    }
            //                                }
            //                            }
            //                            #endregion DriverPerson
            //                        }
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //                #endregion ShipmentStage

            //                #region TransportHandlingUnit
            //                if (delivery.Shipment.TransportHandlingUnit.TransportEquipments.Count > 0)
            //                {
            //                    writer.WriteStartElement("cac:TransportHandlingUnit");  // [0..*]  
            //                    {
            //                        foreach (var transportEquipment in delivery.Shipment.TransportHandlingUnit.TransportEquipments)
            //                        {
            //                            if (!string.IsNullOrEmpty(transportEquipment.Id))
            //                            {
            //                                writer.WriteStartElement("cac:TransportEquipment"); // [0..*] 
            //                                {   // Información de vehículos secundarios (Número de placa)
            //                                    writer.WriteElementString("cbc:ID", transportEquipment.Id); // [0..1] 
            //                                }
            //                                writer.WriteEndElement();
            //                            }
            //                        }
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //                #endregion TransportHandlingUnit

            //                #region Delivery
            //                writer.WriteStartElement("cac:Delivery");   // [0..1]
            //                {
            //                    #region DeliveryAddress
            //                    if (!string.IsNullOrEmpty(delivery.Shipment.Delivery.DeliveryAddress.AddressLine.Line))
            //                    {   // Dirección punto de llegada - Código de ubigeo
            //                        writer.WriteStartElement("cac:DeliveryAddress");    // [0..1] 
            //                        { 
            //                            writer.WriteStartElement("cac:AddressLine");    // [0..*] según Oasis pero se tomará como [0..1]
            //                            {
            //                                writer.WriteElementString("cbc:Line", delivery.Shipment.Delivery.DeliveryAddress.AddressLine.Line);
            //                            }
            //                            writer.WriteEndElement();
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    #endregion DeliveryAddress

            //                    #region DeliveryParty
            //                    if (!string.IsNullOrEmpty(delivery.Shipment.Delivery.DeliveryParty.MarkAttentionIndicator.ToString()))
            //                    {
            //                        writer.WriteStartElement("cac:DeliveryParty");  // [0..1]
            //                        {
            //                            writer.WriteElementString("cbc:MarkAttentionIndicator", delivery.Shipment.Delivery.DeliveryParty.MarkAttentionIndicator.ToString()); // [0..1]
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    #endregion DeliveryParty
            //                }
            //                writer.WriteEndElement();
            //                #endregion Delivery

            //                #region OriginAddress
            //                if (!string.IsNullOrEmpty(delivery.Shipment.OriginAddress.Id.Value))
            //                {   // Dirección punto de partida - Código de ubigeo
            //                    writer.WriteStartElement("cac:OriginAddress");  //  [0..1]
            //                    {
            //                        writer.WriteStartElement("cbc:ID");
            //                        {
            //                            writer.WriteAttributeString("schemeAgencyName", delivery.Shipment.OriginAddress.Id.SchemeAgencyName);
            //                            writer.WriteAttributeString("schemeName",       delivery.Shipment.OriginAddress.Id.SchemeName);
            //                            writer.WriteValue(delivery.Shipment.OriginAddress.Id.Value);
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //                #endregion OriginAddress
            //            }
            //            writer.WriteEndElement();
            //            #endregion Shipment
            //        }
            //        writer.WriteEndElement();
            //    }
            //}
            //#endregion Delivery

            //#region DeliveryTerms
            //if (DeliveryTerms != null)
            //{
            //    if (!string.IsNullOrEmpty(DeliveryTerms.Id.ToString()))
            //    {
            //        writer.WriteStartElement("cac:DeliveryTerms"); // [0..1]
            //        {
            //            writer.WriteElementString("cbc:ID", DeliveryTerms.Id); // [0..1] Número de registro MTC
            //            if (!string.IsNullOrEmpty(DeliveryTerms.Amount.Value.ToString()))
            //            {
            //                writer.WriteStartElement("cbc:Amount"); // [0..1]
            //                {
            //                    writer.WriteAttributeString("currencyID", DeliveryTerms.Amount.CurrencyID); // Moneda del valor referencial PEN
            //                    writer.WriteValue(DeliveryTerms.Amount.Value); // Monto Referencial
            //                }
            //                writer.WriteEndElement();
            //            }

            //            if (!string.IsNullOrEmpty(DeliveryTerms.DeliveryLocation.Address.Country.IdentificationCode.Value))
            //            {
            //                writer.WriteStartElement("cac:DeliveryLocation"); //  [0..1] 
            //                {
            //                    writer.WriteStartElement("cac:Address");
            //                    {
            //                        writer.WriteElementString("cbc:StreetName",             DeliveryTerms.DeliveryLocation.Address.StreetName);
            //                        writer.WriteElementString("cbc:CitySubdivisionName",    DeliveryTerms.DeliveryLocation.Address.CitySubdivisionName);
            //                        writer.WriteElementString("cbc:CityName",               DeliveryTerms.DeliveryLocation.Address.CityName);
            //                        writer.WriteElementString("cbc:CountrySubentity",       DeliveryTerms.DeliveryLocation.Address.CountrySubentity);
            //                        writer.WriteElementString("cbc:CountrySubentityCode",   DeliveryTerms.DeliveryLocation.Address.CountrySubentityCode);
            //                        writer.WriteElementString("cbc:District",               DeliveryTerms.DeliveryLocation.Address.District);

            //                        writer.WriteStartElement("cac:Country"); // [0..1] 
            //                        {
            //                            writer.WriteStartElement("cbc:IdentificationCode"); // [0..1] Dirección del lugar en el que se entrega el bien (Código de país)
            //                            {
            //                                writer.WriteAttributeString("listID",           DeliveryTerms.DeliveryLocation.Address.Country.IdentificationCode.ListID);
            //                                writer.WriteAttributeString("listAgencyName",   DeliveryTerms.DeliveryLocation.Address.Country.IdentificationCode.ListAgencyName);
            //                                writer.WriteAttributeString("listName",         DeliveryTerms.DeliveryLocation.Address.Country.IdentificationCode.ListName);
            //                                writer.WriteValue(DeliveryTerms.DeliveryLocation.Address.Country.IdentificationCode.Value);
            //                            }
            //                            writer.WriteEndElement();
            //                        }
            //                        writer.WriteEndElement();
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //                writer.WriteEndElement();
            //            }
            //        }
            //        writer.WriteEndElement();
            //    }
            //}
            //#endregion DeliveryTerms

            //#region PaymentMeans
            //if (PaymentsMeans.Count > 0)
            //{
            //    foreach (var paymentMeans in PaymentsMeans)
            //    {
            //        if (!string.IsNullOrEmpty(paymentMeans.PayeeFinancialAccount.Id))
            //        {
            //            writer.WriteStartElement("cac:PaymentMeans"); // [0..*] 
            //            {
            //                if (!string.IsNullOrEmpty(paymentMeans.PayeeFinancialAccount.Id))
            //                {
            //                    writer.WriteStartElement("cac:PayeeFinancialAccount"); // [0..1] 
            //                    {   // [0..1]  Cuenta del banco de la nacion (detraccion)
            //                        writer.WriteElementString("cbc:ID", paymentMeans.PayeeFinancialAccount.Id);
            //                    }
            //                    writer.WriteEndElement();
            //                }

            //                if (!string.IsNullOrEmpty(paymentMeans.PaymentMeansCode.Value))
            //                {
            //                    writer.WriteStartElement("cbc:PaymentMeansCode");
            //                    {
            //                        writer.WriteAttributeString("listName",         paymentMeans.PaymentMeansCode.ListName);
            //                        writer.WriteAttributeString("listAgencyName",   paymentMeans.PaymentMeansCode.ListAgencyName);
            //                        writer.WriteAttributeString("listURI",          paymentMeans.PaymentMeansCode.ListURI);
            //                        writer.WriteValue(paymentMeans.PaymentMeansCode.Value);
            //                    }
            //                    writer.WriteEndElement();
            //                }
            //            }
            //            writer.WriteEndElement();
            //        }
            //    }
            //}
            //#endregion PaymentMeans

            //#region PaymentTerms
            //if (PaymentsTerms.Count > 0)
            //{
            //    foreach (var paymentTerms in PaymentsTerms) 
            //    {
            //        if (!string.IsNullOrEmpty(paymentTerms.PaymentTermsId.Value.ToString()))
            //        {
            //            writer.WriteStartElement("cac:PaymentTerms"); // [0..*] 
            //            {
            //                writer.WriteStartElement("cbc:ID"); // [0..1] Codigo del bien o producto sujeto a detracción
            //                {
            //                    writer.WriteAttributeString("schemeName",       paymentTerms.PaymentTermsId.SchemeName);
            //                    writer.WriteAttributeString("schemeAgencyName", paymentTerms.PaymentTermsId.SchemeAgencyName);
            //                    writer.WriteAttributeString("schemeURI",        paymentTerms.PaymentTermsId.SchemeURI); // En la guía de SUNAT esta sin la s, "chemeURI"
            //                    writer.WriteValue(paymentTerms.PaymentTermsId.Value.ToString());
            //                }
            //                writer.WriteEndElement();

            //                writer.WriteElementString("cbc:PaymentPercent", paymentTerms.PaymentPercent.ToString()); // Porcentaje de la detracción
            //                writer.WriteElementString("cbc:Amount",         paymentTerms.Amount.ToString()); // Monto de la detracción
            //            }
            //            writer.WriteEndElement();
            //        }
            //    }
            //}
            //#endregion PaymentTerms

            #region PrepaidPayment
            if (PrepaidPayments.Count > 0)
            {
                foreach (var prepaidPayments in PrepaidPayments)
                {
                    writer.WriteStartElement("cac:PrepaidPayment"); // [0..*] 
                    {
                        if (!string.IsNullOrEmpty(prepaidPayments.PrepaidPaymentId.Value.ToString()))
                        {
                            writer.WriteStartElement("cbc:ID"); // [0..1] Serie y número de comprobante del anticipo (para el caso de reorganización de empresas, incluye el RUC)
                            {
                                writer.WriteAttributeString("schemeID", prepaidPayments.PrepaidPaymentId.SchemeID);
                                writer.WriteAttributeString("schemeName", prepaidPayments.PrepaidPaymentId.SchemeName);
                                writer.WriteAttributeString("schemeAgencyName", prepaidPayments.PrepaidPaymentId.SchemeAgencyName);
                                writer.WriteValue(prepaidPayments.PrepaidPaymentId.Value.ToString());
                            }
                            writer.WriteEndElement();
                        }

                        if (!string.IsNullOrEmpty(prepaidPayments.PaidAmount.Value.ToString()))
                        {
                            writer.WriteStartElement("cbc:PaidAmount"); // [0..1]  Monto prepagado o anticipado
                            {
                                writer.WriteAttributeString("currencyID", prepaidPayments.PaidAmount.CurrencyID); // Código de tipo de moneda del monto prepagado o anticipado
                                writer.WriteValue(prepaidPayments.PaidAmount.Value.ToString()); // Monto prepagado o anticipado
                            }
                            writer.WriteEndElement();
                        }

                        if (!string.IsNullOrEmpty(prepaidPayments.PaidTime.ToString()))
                            writer.WriteElementString("cbc:PaidTime", prepaidPayments.PaidTime.ToString(Formatos.FormatoFecha));  // [0..1]
                    }
                    writer.WriteEndElement();
                }
            }
            #endregion PrepaidPayment

            #region AllowanceCharge
            if (AllowanceCharges.Count > 0)
            {
                foreach (var allowanceCharge in AllowanceCharges)
                {
                    if (!string.IsNullOrEmpty(allowanceCharge.ChargeIndicator.ToString()))
                    {
                        writer.WriteStartElement("cac:AllowanceCharge"); // [0..*] 
                        {
                            writer.WriteElementString("cbc:ChargeIndicator",    allowanceCharge.ChargeIndicator.ToString()); //  [1..1] | Indicador de cargo

                            if (!string.IsNullOrEmpty(allowanceCharge.AllowanceChargeReasonCode.Value))
                            {
                                writer.WriteStartElement("cbc:AllowanceChargeReasonCode");
                                {   // [0..1]
                                    writer.WriteAttributeString("listAgencyName",   allowanceCharge.AllowanceChargeReasonCode.ListAgencyName);
                                    writer.WriteAttributeString("listName",         allowanceCharge.AllowanceChargeReasonCode.ListName);
                                    writer.WriteAttributeString("listURI",          allowanceCharge.AllowanceChargeReasonCode.ListURI);
                                    writer.WriteValue(allowanceCharge.AllowanceChargeReasonCode.Value);
                                }
                                writer.WriteEndElement();
                            }

                            if (allowanceCharge.MultiplierFactorNumeric >= 0)
                                writer.WriteElementString("cbc:MultiplierFactorNumeric", allowanceCharge.MultiplierFactorNumeric.ToString()); // [0..1] 
                            if (allowanceCharge.Amount.Value > 0)
                            {
                                writer.WriteStartElement("cbc:Amount"); // [1..1] Monto del cargo/descuento global
                                {   // Código de tipo de moneda del monto del cargo/descuento global
                                    writer.WriteAttributeString("currencyID",       allowanceCharge.Amount.CurrencyId.ToString());
                                    writer.WriteValue(allowanceCharge.Amount.Value.ToString()); // Monto del cargo/descuento global
                                }
                                writer.WriteEndElement();
                            }

                            if (allowanceCharge.BaseAmount.Value > 0)
                            {
                                writer.WriteStartElement("cbc:BaseAmount"); // [0..1] Monto de base de cargo/descuento global
                                {   // Código de tipo de moneda del monto de base del cargo/descuento global
                                    writer.WriteAttributeString("currencyID",   allowanceCharge.BaseAmount.CurrencyId.ToString());
                                    writer.WriteValue(allowanceCharge.BaseAmount.Value.ToString()); // Monto de base de cargo/descuento global
                                }
                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();
                    }
                }
            }
            #endregion AllowanceCharge

            #region TaxTotal
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

            #region LegalMonetaryTotal
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
                    writer.WriteStartElement("cbc:AllowanceTotalAmount"); // [0..1]  Monto total de descuentos del comprobante
                    {   // Código de tipo de moneda del monto total de descuentos globales del comprobante
                        writer.WriteAttributeString("currencyID", LegalMonetaryTotal.AllowanceTotalAmount.CurrencyId.ToString());
                        writer.WriteValue(LegalMonetaryTotal.AllowanceTotalAmount.Value.ToString()); // Monto total de descuentos del comprobante
                    }
                    writer.WriteEndElement();
                }

                if (LegalMonetaryTotal.ChargeTotalAmount.Value != 0)
                {
                    writer.WriteStartElement("cbc:ChargeTotalAmount"); // [0..1]  Monto total de otros cargos del comprobante
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
            #endregion LegalMonetaryTotal

            #region InvoiceLine
            foreach (var invoiceLine in InvoiceLines) // Sin validación porque siempre debe haber al menos uno
            {
                writer.WriteStartElement("cac:InvoiceLine"); // [1..*] InvoiceLine
                {
                    #region Atributos propios de InvoiceLine 
                    writer.WriteElementString("cbc:ID", invoiceLine.IdDocumentLine.ToString()); // Número de orden del Ítem

                    #region InvoicedQuantity
                    if (!string.IsNullOrEmpty(invoiceLine.InvoicedQuantity.Value.ToString()))
                    {
                        writer.WriteStartElement("cbc:InvoicedQuantity"); // [0..1] Cantidad de unidades del ítem
                        {
                            writer.WriteAttributeString("unitCode",                 invoiceLine.InvoicedQuantity.UnitCode.ToString()); // Código de unidad de medida del ítem
                            writer.WriteAttributeString("unitCodeListID",           invoiceLine.InvoicedQuantity.UnitCodeListID.ToString());
                            writer.WriteAttributeString("unitCodeListAgencyName",   invoiceLine.InvoicedQuantity.UnitCodeListAgencyName.ToString());
                            writer.WriteValue(invoiceLine.InvoicedQuantity.Value.ToString());
                        }
                        writer.WriteEndElement();
                    }
                    #endregion InvoicedQuantity

                    #region LineExtensionAmount
                    writer.WriteStartElement("cbc:LineExtensionAmount"); // [1..1] Valor de venta del ítem
                    {   // Código de tipo de moneda del valor de venta del ítem
                        writer.WriteAttributeString("currencyID", invoiceLine.LineExtensionAmount.CurrencyId.ToString());
                        writer.WriteValue(invoiceLine.LineExtensionAmount.Value.ToString());
                    }
                    writer.WriteEndElement();
                    #endregion LineExtensionAmount

                    //if (!string.IsNullOrEmpty(invoiceLine.TaxPointDate.ToString())) // Se omitió esta etiqueta en la documentacion EXCEL de SUNAT
                    //    writer.WriteElementString("cbc:TaxPointDate", invoiceLine.TaxPointDate.ToString()); // [0..1]
                    #endregion Atributos propios de InvoiceLine 

                    // Completar NOTE

                    #region InvoiceLine > PricingReference
                    if (invoiceLine.PricingReference.AlternativeConditionPrices.Count > 0)
                    {
                        writer.WriteStartElement("cac:PricingReference"); //  [0..1] PricingReference
                        {
                            #region AlternativeConditionPrice
                            if (invoiceLine.PricingReference.AlternativeConditionPrices.Count > 0)
                            {
                                foreach (var alternativeConditionPrice in invoiceLine.PricingReference.AlternativeConditionPrices)
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
                    #endregion InvoiceLine > PricingReference

                    //#region InvoiceLine > Delivery
                    //if (invoiceLine.Deliveries.Count > 0) // start Delivery
                    //{
                    //    foreach (var delivery in invoiceLine.Deliveries)
                    //    {
                    //        if (!string.IsNullOrEmpty(delivery.DeliveryParty.PartyIdentification.Id.Value))
                    //        {
                    //            writer.WriteStartElement("cac:Delivery"); // [0..*] Delivery
                    //            {
                    //                #region InvoiceLine > Delivery > DeliveryParty
                    //                writer.WriteStartElement("cac:DeliveryParty"); //  [0..1] DeliveryParty
                    //                {
                    //                    #region InvoiceLine > Delivery > DeliveryParty > PartyIdentification
                    //                    if (!string.IsNullOrEmpty(delivery.DeliveryParty.PartyIdentification.Id.Value.ToString()))
                    //                    {
                    //                        writer.WriteStartElement("cac:PartyIdentification"); // [0..*]  PartyIdentification
                    //                        {
                    //                            writer.WriteStartElement("cbc:ID"); // [1..1]
                    //                            {
                    //                                writer.WriteAttributeString("schemeID",         delivery.DeliveryParty.PartyIdentification.Id.SchemeId); // Código de tipo de documento de identidad del huesped
                    //                                writer.WriteAttributeString("schemeName",       delivery.DeliveryParty.PartyIdentification.Id.SchemeName);
                    //                                writer.WriteAttributeString("schemeAgencyName", delivery.DeliveryParty.PartyIdentification.Id.SchemeAgencyName);
                    //                                writer.WriteAttributeString("schemeAgencyID",   delivery.DeliveryParty.PartyIdentification.Id.schemeAgencyID); // Código del país de emisión del pasaporte
                    //                                writer.WriteValue(delivery.DeliveryParty.PartyIdentification.Id.Value);
                    //                            }
                    //                            writer.WriteEndElement();
                    //                        }
                    //                        writer.WriteEndElement(); // PartyIdentification
                    //                    }
                    //                    #endregion InvoiceLine > Delivery > DeliveryParty > PartyIdentification

                    //                    #region InvoiceLine > Delivery > DeliveryParty > PartyName
                    //                    if (!string.IsNullOrEmpty(delivery.DeliveryParty.PartyName.Name.ToString()))
                    //                    {
                    //                        writer.WriteStartElement("cac:PartyName"); // [0..*] PartyName
                    //                        {   // Apellidos y Nombres o denominacion o razon social del huesped
                    //                            writer.WriteElementString("cbc:Name", delivery.DeliveryParty.PartyName.Name.ToString()); //  [1..1]
                    //                        }
                    //                        writer.WriteEndElement(); // end PartyName
                    //                    }
                    //                    #endregion InvoiceLine > Delivery > DeliveryParty > PartyName

                    //                    #region InvoiceLine > Delivery > DeliveryParty > PostalAddress
                    //                    if (!string.IsNullOrEmpty(delivery.DeliveryParty.PostalAddress.Country.IdentificationCode.Value.ToString()))
                    //                    {
                    //                        writer.WriteStartElement("cac:PostalAddress"); // [0..1]   PostalAddress
                    //                        {
                    //                            writer.WriteStartElement("cac:Country"); // [0..1]  
                    //                            {
                    //                                writer.WriteStartElement("cbc:IdentificationCode"); // [0..1]  
                    //                                {
                    //                                    writer.WriteAttributeString("listID", delivery.DeliveryParty.PostalAddress.Country.IdentificationCode.ListID.ToString());
                    //                                    writer.WriteAttributeString("listAgencyName", delivery.DeliveryParty.PostalAddress.Country.IdentificationCode.ListAgencyName.ToString());
                    //                                    writer.WriteAttributeString("listName", delivery.DeliveryParty.PostalAddress.Country.IdentificationCode.ListName.ToString());
                    //                                    writer.WriteValue(delivery.DeliveryParty.PostalAddress.Country.IdentificationCode.Value.ToString());
                    //                                }
                    //                                writer.WriteEndElement();
                    //                            }
                    //                            writer.WriteEndElement();
                    //                        }
                    //                        writer.WriteEndElement(); // end PostalAddress
                    //                    }
                    //                    #endregion InvoiceLine > Delivery > DeliveryParty > PostalAddress

                    //                    #region InvoiceLine > Delivery > DeliveryParty > Person
                    //                    if (!string.IsNullOrEmpty(delivery.DeliveryParty.Person.PersonId.Value.ToString()))
                    //                    {   // SUNAT lo considera de 0..1 pero OASIS es de 0..*, se tomará como valido la indicación de SUNAT.
                    //                        writer.WriteStartElement("cac:Person"); // [0..1] 
                    //                        {
                    //                            writer.WriteStartElement("cbc:ID"); // [0..1] 
                    //                            {
                    //                                writer.WriteAttributeString("schemeID", delivery.DeliveryParty.Person.PersonId.SchemeID.ToString()); // Paquete turístico – Código de tipo de Documento identidad del huésped
                    //                                writer.WriteAttributeString("schemeName", delivery.DeliveryParty.Person.PersonId.SchemeName.ToString());
                    //                                writer.WriteAttributeString("schemeAgencyName", delivery.DeliveryParty.Person.PersonId.SchemeAgencyName.ToString());
                    //                                writer.WriteAttributeString("schemeURI", delivery.DeliveryParty.Person.PersonId.SchemeURI.ToString());
                    //                                writer.WriteValue(delivery.DeliveryParty.Person.PersonId.Value.ToString());
                    //                            }
                    //                            writer.WriteEndElement();
                    //                            // FirstName > Paquete turístico – Número de documento identidad de huésped
                    //                            writer.WriteElementString("cbc:FirstName", delivery.DeliveryParty.Person.FirstName.ToString());
                    //                        }
                    //                        writer.WriteEndElement();
                    //                    }
                    //                    #endregion InvoiceLine > Delivery > DeliveryParty > Person

                    //                }
                    //                writer.WriteEndElement(); // end DeliveryParty
                    //                #endregion InvoiceLine > Delivery > DeliveryParty

                    //                #region InvoiceLine > Delivery > Shipment
                    //                if (!string.IsNullOrEmpty(delivery.Shipment.Id.Value))
                    //                {
                    //                    writer.WriteStartElement("cac:Shipment"); // [0..1] 
                    //                    {
                    //                        writer.WriteElementString("cbc:ID", delivery.Shipment.Id.Value); // Código de motivo de traslado

                    //                        writer.WriteStartElement("cbc:GrossWeightMeasure"); // manual
                    //                        {
                    //                            writer.WriteAttributeString("unitCode", delivery.Shipment.GrossWeightMeasure.UnitCode);
                    //                            writer.WriteAttributeString("unitCodeListVersionID", delivery.Shipment.GrossWeightMeasure.UnitCodeListVersionID);
                    //                        }
                    //                        writer.WriteEndElement();

                    //                        #region ShipmentStage
                    //                        if (delivery.Shipment.ShipmentStages.Count > 0)
                    //                        {
                    //                            writer.WriteStartElement("ShipmentStage");
                    //                            {
                    //                                foreach (var shipmentStage in delivery.Shipment.ShipmentStages)
                    //                                {
                    //                                    writer.WriteStartElement("cbc:ID"); // [0..*]  Número de Asiento (Transporte de Pasajeros)
                    //                                    {   // Información de Manifiesto de pasajero
                    //                                        writer.WriteAttributeString("schemeID", shipmentStage.ShipmentStageId.SchemeID.ToString());
                    //                                        writer.WriteValue(shipmentStage.ShipmentStageId.Value.ToString()); // Número de Asiento (Transporte de Pasajeros)
                    //                                    }
                    //                                    writer.WriteEndElement();
                    //                                }
                    //                            }
                    //                            writer.WriteEndElement();
                    //                        }
                    //                        #endregion ShipmentStage
                    //                        // Continua pero se esta obviando

                    //                    }
                    //                    writer.WriteEndElement();
                    //                }
                    //                #endregion InvoiceLine > Delivery > Shipment

                    //                #region Despatch
                    //                writer.WriteStartElement("cac:Despatch");   // [0..1]
                    //                {
                    //                    writer.WriteStartElement("cac:DespatchAddress"); // [0..1] 
                    //                    {
                    //                        writer.WriteStartElement("cbc:ID"); // [0..1]  
                    //                        {
                    //                            writer.WriteAttributeString("schemeAgencyName", delivery.Despatch.DespatchAddress.ID.SchemeAgencyName);
                    //                            writer.WriteAttributeString("schemeName",       delivery.Despatch.DespatchAddress.ID.SchemeName);
                    //                            writer.WriteValue(delivery.Despatch.DespatchAddress.ID.Value);
                    //                        }
                    //                        writer.WriteEndElement();
                    //                    }
                    //                    writer.WriteEndElement();
                    //                }
                    //                writer.WriteEndElement();
                    //                #endregion Despatch
                    //            }
                    //            writer.WriteEndElement(); // end node Delivery
                    //        }
                    //    }
                    //} // end Delivery
                    //#endregion InvoiceLine > Delivery

                    //#region InvoiceLine > AllowanceCharge
                    //if (invoiceLine.AllowanceCharges.Count > 0)
                    //{
                    //    foreach (var allowanceCharge in invoiceLine.AllowanceCharges)
                    //    {
                    //        writer.WriteStartElement("cac:AllowanceCharge"); // [0..*] 
                    //        {
                    //            // Indicador del cargo / descuento del ítem | Boolean
                    //            writer.WriteElementString("cbc:ChargeIndicator", allowanceCharge.ChargeIndicator.ToString()); // [1..1]

                    //            writer.WriteStartElement("cbc:AllowanceChargeReasonCode");
                    //            {   // [0..1]
                    //                writer.WriteAttributeString("listAgencyName",   allowanceCharge.AllowanceChargeReasonCode.ListAgencyName);
                    //                writer.WriteAttributeString("listName",         allowanceCharge.AllowanceChargeReasonCode.ListName);
                    //                writer.WriteAttributeString("listURI",          allowanceCharge.AllowanceChargeReasonCode.ListURI);
                    //                writer.WriteValue(allowanceCharge.AllowanceChargeReasonCode.Value);
                    //            }
                    //            writer.WriteEndElement();

                    //            writer.WriteElementString("cbc:MultiplierFactorNumeric", allowanceCharge.MultiplierFactorNumeric.ToString()); // [0..1]

                    //            #region Amount
                    //            if (!string.IsNullOrEmpty(allowanceCharge.Amount.Value.ToString()))
                    //            {   // Monto del cargo/descuento del ítem
                    //                writer.WriteStartElement("cbc:Amount"); // [1..1]
                    //                {
                    //                    writer.WriteAttributeString("currencyID", allowanceCharge.Amount.CurrencyId.ToString());
                    //                    writer.WriteValue(allowanceCharge.Amount.Value.ToString());
                    //                }
                    //                writer.WriteEndElement();
                    //            }
                    //            #endregion Amount

                    //            #region BaseAmount
                    //            if (!string.IsNullOrEmpty(allowanceCharge.BaseAmount.Value.ToString()))
                    //            {   // Monto de base del cargo/descuento del ítem
                    //                writer.WriteStartElement("cbc:BaseAmount"); // [0..1]
                    //                {
                    //                    writer.WriteAttributeString("currencyID", allowanceCharge.BaseAmount.CurrencyId.ToString());
                    //                    writer.WriteValue(allowanceCharge.BaseAmount.Value.ToString());
                    //                }
                    //                writer.WriteEndElement();
                    //            }
                    //            #endregion BaseAmount
                    //        }
                    //        writer.WriteEndElement();
                    //    }
                    //}
                    //#endregion InvoiceLine > AllowanceCharge

                    #region InvoiceLine > TaxTotal
                    if (invoiceLine.TaxTotals.Count > 0)
                    {
                        foreach (var taxTotal in invoiceLine.TaxTotals)
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
                                                #region Id
                                                //if (!string.IsNullOrEmpty(taxSubtotal.TaxCategory.TaxCategoryId.Value.ToString()))
                                                //{
                                                //    writer.WriteStartElement("cbc:ID"); // [0..1] 
                                                //    {
                                                //        writer.WriteAttributeString("schemeID", taxSubtotal.TaxCategory.TaxCategoryId.SchemeID.ToString());
                                                //        writer.WriteAttributeString("schemeAgencyID", taxSubtotal.TaxCategory.TaxCategoryId.SchemeAgencyID.ToString());
                                                //        writer.WriteValue(taxSubtotal.TaxCategory.TaxCategoryId.Value.ToString());
                                                //    }
                                                //    writer.WriteEndElement();
                                                //}
                                                #endregion

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
                                                            writer.WriteAttributeString("schemeID", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeID);
                                                            writer.WriteAttributeString("schemeName", taxSubtotal.TaxCategory.TaxScheme.TaxSchemeId.SchemeName);
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
                    #endregion InvoiceLine > TaxTotal

                    #region InvoiceLine > Item
                    writer.WriteStartElement("cac:Item");   //  [1..1] 
                    {
                        #region Item > Description
                        if (invoiceLine.Item.Descriptions.Count > 0)
                        {   // Descripción(es) detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
                            foreach (var description in invoiceLine.Item.Descriptions)
                                writer.WriteElementString("cbc:Description", description.Detail.ToString()); // [0..*] 
                        }
                        #endregion Item > Description

                        #region Item > SellersItemIdentification
                        if (!string.IsNullOrEmpty(invoiceLine.Item.SellersItemIdentification.Id.ToString()))
                        {
                            writer.WriteStartElement("cac:SellersItemIdentification"); // [0..1] 
                            {
                                writer.WriteElementString("cbc:ID", invoiceLine.Item.SellersItemIdentification.Id.ToString()); // [1..1] | Código de producto del ítem
                            }
                            writer.WriteEndElement();
                        }
                        #endregion Item > SellersItemIdentification

                        #region Item > CommodityClassification
                        if (!string.IsNullOrEmpty(invoiceLine.Item.CommodityClassification.ItemClassificationCode.Value.ToString()))
                        {   // No es obligatorio
                            writer.WriteStartElement("cac:CommodityClassification"); // [0..1] Según SUNAT, [0..*] según OASIS
                            {   // Código de producto (SUNAT)
                                writer.WriteStartElement("cbc:ItemClassificationCode"); // [1..1] Según SUNAT, [0..1] Según OASIS
                                {
                                    writer.WriteAttributeString("listID", invoiceLine.Item.CommodityClassification.ItemClassificationCode.ListID.ToString());
                                    writer.WriteAttributeString("listAgencyName", invoiceLine.Item.CommodityClassification.ItemClassificationCode.ListAgencyName.ToString());
                                    writer.WriteAttributeString("listName", invoiceLine.Item.CommodityClassification.ItemClassificationCode.ListName.ToString());
                                    writer.WriteValue(invoiceLine.Item.CommodityClassification.ItemClassificationCode.Value.ToString());
                                }
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        #endregion Item > CommodityClassification

                        #region Item > AdditionalItemProperty
                        if (invoiceLine.Item.AdditionalItemProperties.Count > 0)
                        {
                            foreach (var additionalItemProperty in invoiceLine.Item.AdditionalItemProperties)
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
                        #endregion InvoiceLine > Item
                    }
                    writer.WriteEndElement();

                    #region InvoiceLine > Price
                    if (!string.IsNullOrEmpty(invoiceLine.Price.PriceAmount.Value.ToString()))
                    {   // Valor unitario del ítem
                        writer.WriteStartElement("cac:Price"); // [0..1] 
                        {    // Código de tipo de moneda del valor unitario del ítem
                            writer.WriteStartElement("cbc:PriceAmount");
                            {
                                writer.WriteAttributeString("currencyID", invoiceLine.Price.PriceAmount.CurrencyId);
                                writer.WriteValue(invoiceLine.Price.PriceAmount.Value.ToString());
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    #endregion InvoiceLine > Price
                }
                writer.WriteEndElement(); // end InvoiceLine
            }
            #endregion InvoiceLine
        }
    }
}
