using System;
using System.Collections.Generic;
using Common;
using Models.Contratos;
using Models.Modelos;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;
using StructureUBL.EstandarUbl;
using StructureUBL.SunatAggregateComponents;

namespace GenerateXML
{
    public class ResumenDiarioNuevoXml : IDocumentoXml
    {
        IEstructuraXml IDocumentoXml.Generar(IDocumentoElectronico request)
        {
            var documento   =   (ResumenDiario)request;
            var summary     =   new SummaryDocuments
            {
                Id              =   documento.IdDocumento,
                IssueDate       =   Convert.ToDateTime(documento.FechaEmision),
                ReferenceDate   =   Convert.ToDateTime(documento.FechaReferencia),
                CustomizationId =   "1.1",
                UblVersionId    =   "2.0",
                Signature   =   new Signature
                {
                    Id              =   documento.IdDocumento,
                    SignatoryParty  =   new SignatoryParty
                    {
                        PartyIdentification = new PartyIdentification
                        {
                            Id      =   new PartyIdentificationId { Value   =   documento.Emisor.NroDocumento }
                        },
                        PartyName   =   new PartyName { Name = documento.Emisor.NombreLegal }
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

            foreach (var resumen in documento.Resumenes)
            {
                var linea = new VoidedDocumentsLine
                {
                    LineId              =   resumen.Id,
                    DocumentTypeCode    =   resumen.TipoDocumento,
                    Id                  =   resumen.Serie,    // Serie correlativo
                    AccountingCustomerParty = new AccountingContributorParty
                    {
                        AdditionalAccountId         =   resumen.Receptor.TipoDocumento,
                        CustomerAssignedAccountId   =   resumen.Receptor.NroDocumento
                    },
                    BillingReference    =   new BillingReference
                    {
                        InvoiceDocumentReference    =   new InvoiceDocumentReference
                        {
                            Id                  =   resumen.DocumentoRelacionado,
                            DocumentTypeCode    =   new DocumentTypeCode() { Value  = resumen.TipoDocumentoRelacionado }
                        }
                    },
                    ConditionCode   =   resumen.CodigoEstadoItem,
                    TotalAmount     =   new PayableAmount
                    {
                        CurrencyId  =   resumen.Moneda,
                        Value       =   resumen.TotalVenta
                    },
                    BillingPayments =   new List<BillingPayment>()
                    {
                      new BillingPayment
                      {
                          PaidAmount =  new PayableAmount
                          {
                              CurrencyId    =   resumen.Moneda,
                              Value         =   resumen.Gravadas
                          },
                          InstructionId     =   "01"
                      }
                    },
                    AllowanceCharge =   new AllowanceCharge
                    {
                        ChargeIndicator =   true,
                        Amount          =   new PayableAmount
                        {
                            CurrencyId  =   resumen.Moneda,
                            Value       =   resumen.TotalDescuentos
                        }
                    },

                };

                #region TaxTotal
                if (resumen.TotalImpuestos.Count > 0)
                {   // Monto total de impuestos del ítem
                    foreach (var totalImpuesto in resumen.TotalImpuestos)
                    {   // Total de impuestos para cada ítem
                        var lineaTotalImpuestos = new TaxTotal
                        {   // Monto total de impuestos por línea
                            TaxAmount = new PayableAmount() { CurrencyId = totalImpuesto.TipoMonedaTotal, Value = totalImpuesto.MontoTotal }
                        };

                        foreach (var subTotalImpuestos in totalImpuesto.SubTotalesImpuestos)
                        {   // Afectación al IGV por la línea | Afectación IVAP por la línea
                            lineaTotalImpuestos.TaxSubtotals.Add(new TaxSubtotal
                            {
                                TaxAmount = new PayableAmount
                                {   // Monto de IGV/IVAP de la línea | Monto total de impuestos por linea
                                    CurrencyId  = subTotalImpuestos.TipoMonedaTotal,// Código de tipo de moneda del monto total del impuesto
                                    Value       = subTotalImpuestos.MontoTotal      // Monto total del impuesto
                                },
                                TaxCategory = new TaxCategory
                                {   // Tasa del IGV o  Tasa del IVAP
                                    TaxCategoryId           = new TaxCategoryId { Value = subTotalImpuestos.CategoriaImpuestos }, // Categoría de impuestos
                                    Percent                 = subTotalImpuestos.PorcentajeImp,  // Tasa del IGV o  Tasa del IVAP | Tasa del tributo
                                    TaxExemptionReasonCode  = new TaxExemptionReasonCode() { Value = subTotalImpuestos.TipoAfectacion }, // Afectación al IGV o IVAP cuando corresponda
                                    TierRange               = subTotalImpuestos.TipoSistemaISC,
                                    TaxScheme = new TaxScheme
                                    {
                                        TaxSchemeId = new TaxSchemeId { Value = subTotalImpuestos.CodigoTributo },    // Código de tributo por línea | Catálogo No. 05
                                        Name        = subTotalImpuestos.NombreTributo,    // NombreTributo de tributo
                                        TaxTypeCode = subTotalImpuestos.CodigoInternacional     // Código internacional tributo
                                    }
                                }
                            });
                        }
                        linea.TaxTotals.Add(lineaTotalImpuestos); // Realizar debug
                    }
                }
                #endregion TaxTotal

                if (resumen.Exoneradas > 0)
                {
                    linea.BillingPayments.Add(new BillingPayment
                    {
                        PaidAmount  =   new PayableAmount
                        {
                            CurrencyId  =   resumen.Moneda,
                            Value       =   resumen.Exoneradas
                        },
                        InstructionId = "02"
                    });
                }

                if (resumen.Inafectas > 0)
                {
                    linea.BillingPayments.Add(new BillingPayment
                    {
                        PaidAmount = new PayableAmount
                        {
                            CurrencyId  =   resumen.Moneda,
                            Value       =   resumen.Inafectas
                        },
                        InstructionId   =   "03"
                    });
                }
                if (resumen.Exportaciones > 0)
                {
                    linea.BillingPayments.Add(new BillingPayment
                    {
                        PaidAmount = new PayableAmount
                        {
                            CurrencyId  =   resumen.Moneda,
                            Value       =   resumen.Exportaciones
                        },
                        InstructionId   =   "04"
                    });
                }
                if (resumen.Gratuitas > 0)
                {
                    linea.BillingPayments.Add(new BillingPayment
                    {
                        PaidAmount  =   new PayableAmount
                        {
                            CurrencyId  =   resumen.Moneda,
                            Value       =   resumen.Gratuitas
                        },
                        InstructionId   =   "05"
                    });
                }
                
                summary.SummaryDocumentsLines.Add(linea);
            }

            return summary;
        }
    }
}
