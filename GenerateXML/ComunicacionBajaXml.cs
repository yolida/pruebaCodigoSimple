using System;
using Common;
using Models.Contratos;
using Models.Modelos;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.EstandarUbl;
using StructureUBL.SunatAggregateComponents;

namespace GenerateXML
{
    public class ComunicacionBajaXml : IDocumentoXml
    {
        IEstructuraXml IDocumentoXml.Generar(IDocumentoElectronico request)
        {
            var documento = (ComunicacionBaja)request;
            var voidedDocument = new VoidedDocuments
            {
                Id              =   documento.IdDocumento,
                IdInvoice       =   documento.IdDocumento,
                IssueDate       =   Convert.ToDateTime(documento.FechaEmision),
                ReferenceDate   =   Convert.ToDateTime(documento.FechaReferencia),
                CustomizationId =   "1.0",
                UblVersionId    =   "2.0",
                Signature   =   new Signature
                {
                    Id  =   documento.IdDocumento,
                    SignatoryParty  =   new SignatoryParty
                    {
                        PartyIdentification = new PartyIdentification
                        {
                            Id      =   new PartyIdentificationId { Value   =   documento.Emisor.NroDocumento }
                        },
                        PartyName   =   new PartyName { Name    =   documento.Emisor.NombreLegal }
                    },
                    DigitalSignatureAttachment  =   new DigitalSignatureAttachment
                    {
                        ExternalReference   =   new ExternalReference
                        {
                            Uri = $"{documento.Emisor.NroDocumento}-{documento.IdDocumento}"
                        }
                    }
                },
                AccountingSupplierParty =   new AccountingContributorParty
                {
                    CustomerAssignedAccountId   =   documento.Emisor.NroDocumento,
                    AdditionalAccountId         =   documento.Emisor.TipoDocumento,
                    Party   =   new Party
                    {
                        PartyLegalEntity    =   new PartyLegalEntity { RegistrationName = documento.Emisor.NombreLegal }
                    }
                }
            };

            foreach (var baja in documento.Bajas)
            {
                voidedDocument.VoidedDocumentsLines.Add(new VoidedDocumentsLine
                {
                    LineId                  =   baja.Id,
                    DocumentTypeCode        =   baja.TipoDocumento,
                    DocumentSerialID        =   baja.Serie,
                    DocumentNumberID        =   baja.Correlativo,
                    VoidReasonDescription   =   baja.MotivoBaja
                });
            }

            return voidedDocument;
        }
    }
}
