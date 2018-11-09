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
    public class VoidedDocuments : IXmlSerializable, IEstructuraXml
    {
        public UBLExtensions    UBLExtensions   { get; set; }
        public string           UblVersionId    { get; set; }
        public string           CustomizationId { get; set; }
        public string           IdInvoice       { get; set; }

        /// <summary>
        /// Fecha de generación de la comunicación
        /// </summary>
        public DateTime         IssueDate       { get; set; }

        /// <summary>
        /// Fecha de generación del documento dado de baja
        /// </summary>
        public DateTime         ReferenceDate   { get; set; }
        public Signature        Signature       { get; set; }
        public AccountingContributorParty   AccountingSupplierParty { get; set; }
        public List<VoidedDocumentsLine>    VoidedDocumentsLines { get; set; }
        public IFormatProvider  Formato     { get; set; }
        public string Id    { get; set; }

        public VoidedDocuments()
        {
            UBLExtensions           =   new UBLExtensions();
            Signature               =   new Signature();
            AccountingSupplierParty =   new AccountingContributorParty();
            VoidedDocumentsLines    =   new List<VoidedDocumentsLine>();
            UblVersionId            =   "2.0";
            CustomizationId         =   "1.0";
            Formato =   new System.Globalization.CultureInfo(Formatos.Cultura);
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
            writer.WriteAttributeString("xmlns",        EspacioNombres.xmlnsVoidedDocuments);
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
            writer.WriteElementString("cbc:ID",                 Id);
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
            
            #region VoidedDocumentsLines
            foreach (var item in VoidedDocumentsLines)
            {
                writer.WriteStartElement("sac:VoidedDocumentsLine");
                {
                    writer.WriteElementString("cbc:LineID",                 item.LineId.ToString());
                    writer.WriteElementString("cbc:DocumentTypeCode",       item.DocumentTypeCode);
                    writer.WriteElementString("sac:DocumentSerialID",       item.DocumentSerialID);
                    writer.WriteElementString("sac:DocumentNumberID",       item.DocumentNumberID.ToString());

                    writer.WriteStartElement("sac:VoidReasonDescription");
                    writer.WriteCData(item.VoidReasonDescription);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            #endregion VoidedDocumentsLines
        }
    }
}
