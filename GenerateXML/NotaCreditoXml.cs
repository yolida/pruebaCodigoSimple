using System.Collections.Generic;
using Common;
using Models.Contratos;
using Models.Modelos;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonStaticComponents;
using StructureUBL.EstandarUbl;

namespace GenerateXML
{
    public class NotaCreditoXml : IDocumentoXml
    {
        IEstructuraXml IDocumentoXml.Generar(IDocumentoElectronico request)
        {
            var documento = (DocumentoElectronico)request;
            var creditNote = new CreditNote
            {
                UblVersionId            =   "2.1",
                CustomizationId         =   new CustomizationID() { Value = "2.0" },
                IdInvoice               =   documento.SerieCorrelativo,   // Serie y número del comprobante
                IssueDate               =   documento.FechaEmision,       // Fecha de emisión yyyy-mm-dd
                IssueTime               =   documento.HoraEmision,        // Hora de emisión hh-mm-ss.0z
                DiscrepancyResponses    =   new List<DiscrepancyResponse>(),
                InvoiceTypeCode         =   new InvoiceTypeCode() { Value = documento.TipoDocumento, ListID = documento.TipoOperacion }, // Código de tipo de documento
                DocumentCurrencyCode    =   new DocumentCurrencyCode() { Value = documento.Moneda }, // Código de tipo de moneda en la cual se emite la factura electrónica
                OrderReference          =   new OrderReference() { Id = documento.OrdenCompra ?? string.Empty },
                Signature               =   new Signature
                {
                    //Id = documento.SerieCorrelativo,
                    Id = documento.Emisor.NroDocumento,
                    SignatoryParty = new SignatoryParty
                    {
                        PartyIdentification = new PartyIdentification
                        {
                            Id = new PartyIdentificationId { Value = documento.Emisor.NroDocumento } 
                        },
                        PartyName = new PartyName { Name = documento.Emisor.NombreLegal }
                    },
                    DigitalSignatureAttachment = new DigitalSignatureAttachment
                    {
                        ExternalReference = new ExternalReference
                        {
                            Uri = $"{documento.Emisor.NroDocumento}-{documento.SerieCorrelativo}"
                        }
                    }
                },
                AccountingSupplierParty =   new AccountingContributorParty()
                { // Emisor y Receptor son instancias de la clase Contribuyente, por lo que comparten los mismos atributos
                    Party = new Party()
                    {
                        PartyName = new PartyName() { Name = documento.Emisor.NombreComercial }, // NombreTributo Comercial del emisor
                        PartyIdentification = new PartyIdentification()
                        {   // RUC
                            Id = new PartyIdentificationId() { Value = documento.Emisor.NroDocumento, SchemeId = documento.Emisor.TipoDocumento }
                        },
                        PartyLegalEntity = new PartyLegalEntity() {
                            RegistrationName    = documento.Emisor.NombreLegal, // Apellidos y nombres, denominación o razón social
                            RegistrationAddress = new RegistrationAddress() {
                                AddressLine         = new AddressLine() {   Line = documento.Emisor.Direccion },
                                CitySubdivisionName = documento.Emisor.Urbanizacion,
                                CityName            = documento.Emisor.Provincia,
                                IdUbigeo            = new IdUbigeo() {  Value = documento.Emisor.Ubigeo },
                                CountrySubentity    = documento.Emisor.Departamento,
                                District            = documento.Emisor.Distrito,
                                Country         = new Country() {   IdentificationCode = new IdentificationCode() { Value = documento.Emisor.Pais } },
                                AddressTypeCode = new AddressTypeCode() { Value = "0000" }  // Por defecto 0000, no se asigna otro valor debido a que no se tiene suficientes especificaciones
                            },
                            ShareholderParties = new List<ShareholderParty>() // Se implementa líneas mas abajo
                        },

                        #region PartyTaxScheme
                        PartyTaxScheme = new PartyTaxScheme()
                        {
                            RegistrationName = documento.Emisor.NombreLegal, // NombreTributo o razón social del emisor
                            CompanyID = new CompanyID() { SchemeID = documento.Emisor.TipoDocumento, Value = documento.Emisor.NroDocumento }, // Número de RUC del emisor
                            RegistrationAddress = new RegistrationAddress() { AddressTypeCode = new AddressTypeCode() {
                                Value = "0000" /*documento.Emisor.Ubigeo.ToString()*/ // Cambiar esto !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                }
                            }
                        }
                        #endregion PartyTaxScheme
                    },
                },
                AccountingCustomerParty =   new AccountingContributorParty() // AccountingSupplierParty y AccountingCustomerParty son instancias
                {   // de la clase AccountingContributorParty, por lo que comparten los mismos atributos
                    Party = new Party()
                    {
                        PartyName = new PartyName() { Name = documento.Receptor.NombreComercial ?? string.Empty }, // NombreTributo Comercial del Receptor
                        PartyIdentification = new PartyIdentification()
                        {   // Número de documento
                            Id = new PartyIdentificationId() { Value = documento.Receptor.NroDocumento, SchemeId = documento.Receptor.TipoDocumento }
                        },
                        PartyLegalEntity = new PartyLegalEntity()
                        {
                            RegistrationName = documento.Receptor.NombreComercial,
                            RegistrationAddress = new RegistrationAddress()
                            {
                                AddressLine         = new AddressLine() { Line = documento.Receptor.Direccion },
                                CitySubdivisionName = documento.Receptor.Urbanizacion,
                                CityName            = documento.Receptor.Provincia,
                                IdUbigeo            = new IdUbigeo() { Value = documento.Receptor.Ubigeo },
                                CountrySubentity    = documento.Receptor.Departamento,
                                District            = documento.Receptor.Distrito,
                                Country             = new Country() { IdentificationCode = new IdentificationCode() { Value = documento.Receptor.Pais } },
                                AddressTypeCode     = new AddressTypeCode() {  }    // Investigar mas sobre este valor, por ahora no irá   
                            }
                        },

                        #region PartyTaxScheme
                        PartyTaxScheme = new PartyTaxScheme() // Tambien se excluyó, decreto de SUNAT 30/06/2018
                        {   // Número de RUC del Receptor
                            CompanyID = new CompanyID() { SchemeID = documento.Receptor.TipoDocumento, Value = documento.Receptor.NroDocumento }
                        },
                        #endregion PartyTaxScheme
                    },

                },
                BillingReferences       =   new List<BillingReference>(),
                TaxTotals               =   new List<TaxTotal>(),
                LegalMonetaryTotal      =   new LegalMonetaryTotal() {
                    LineExtensionAmount     = new PayableAmount() { Value   =   documento.TotalValorVenta,      CurrencyId = documento.Moneda },
                    TaxInclusiveAmount      = new PayableAmount() { Value   =   documento.TotalPrecioVenta,     CurrencyId = documento.Moneda },
                    AllowanceTotalAmount    = new PayableAmount() { Value   =   documento.TotalDescuento,       CurrencyId = documento.Moneda },
                    ChargeTotalAmount       = new PayableAmount() { Value   =   documento.TotalOtrosCargos,     CurrencyId = documento.Moneda },
                    PrepaidAmount           = new PayableAmount() { Value   =   documento.TotalAnticipos,       CurrencyId = documento.Moneda },
                    PayableAmount           = new PayableAmount() { Value   =   documento.ImporteTotalVenta,    CurrencyId = documento.Moneda },
                    PayableRoundingAmount   = new PayableAmount() { Value   =   documento.MontoRedondeo,        CurrencyId = documento.Moneda },
                }
            };

            foreach (var discrepancia in documento.Discrepancias)
            {
                creditNote.DiscrepancyResponses.Add(new DiscrepancyResponse
                {
                    ResponseCode    =   new ResponseCode() { Value  = discrepancia.Tipo, ListName   =   "Tipo de nota de credito" },
                    Description     =   discrepancia.Descripcion
                });
            }

            foreach (var relacionado in documento.Relacionados)
            {
                creditNote.BillingReferences.Add(new BillingReference
                {
                    InvoiceDocumentReference = new InvoiceDocumentReference
                    {
                        Id                  =   relacionado.NroDocumento,
                        DocumentTypeCode    =   new DocumentTypeCode() { Value = relacionado.TipoDocumento ?? string.Empty, ListName = "Tipo de Documento" }
                    }
                });
            }

            //foreach (var relacionado in documento.OtrosDocumentosRelacionados)
            //{
            //    creditNote.AdditionalDocumentReferences.Add(new InvoiceDocumentReference
            //    {
            //        Id                  =   relacionado.NroDocumento,
            //        DocumentTypeCode    =   new DocumentTypeCode() { Value = relacionado.TipoDocumento ?? string.Empty, ListName = "Documento Relacionado" }
            //    });
            //}

            // TaxTotal o TotalImpuestos
            foreach (var totalImpuesto in documento.TotalImpuestos)
                {
                    var lineaTotalImpuestos = new TaxTotal
                    {   // Monto total de impuestos
                        TaxAmount = new PayableAmount() { CurrencyId = totalImpuesto.TipoMonedaTotal, Value = totalImpuesto.MontoTotal }
                    };

                    foreach (var subTotalImpuestos in totalImpuesto.SubTotalesImpuestos)
                    {
                        lineaTotalImpuestos.TaxSubtotals.Add(new TaxSubtotal {
                            TaxableAmount   = new PayableAmount {   // Monto base
                                CurrencyId  = subTotalImpuestos.TipoMonedaBase, // Código de tipo de moneda del monto de las operaciones gravadas/exoneradas/inafectas del impuesto
                                Value       = subTotalImpuestos.BaseImponible   // Monto las operaciones gravadas/exoneradas/inafectas del impuesto
                            },
                            TaxAmount = new PayableAmount {
                                CurrencyId  = subTotalImpuestos.TipoMonedaTotal,    // Código de tipo de moneda del monto total del impuesto
                                Value       = subTotalImpuestos.MontoTotal          // Monto total del impuesto
                            },
                            TaxCategory = new TaxCategory {
                                TaxCategoryId   = new TaxCategoryId { Value = subTotalImpuestos.CategoriaImpuestos }, // Categoría de impuestos
                                TaxScheme   = new TaxScheme {
                                    TaxSchemeId = new TaxSchemeId { Value   = subTotalImpuestos.CodigoTributo },  // Código de tributo
                                    Name        = subTotalImpuestos.NombreTributo,    // NombreTributo de tributo
                                    TaxTypeCode = subTotalImpuestos.CodigoInternacional     // Código internacional tributo
                                }
                            }
                        });
                    }
                    creditNote.TaxTotals.Add(lineaTotalImpuestos);
                }

            foreach (var detalleDocumento in documento.DetalleDocumentos)
            {
                var linea = new DocumentLine
                {
                    IdDocumentLine      =   detalleDocumento.NumeroOrden,
                    CreditedQuantity    =   new InvoicedQuantity() { UnitCode = detalleDocumento.UnidadMedida, Value = detalleDocumento.Cantidad },
                    LineExtensionAmount =   new PayableAmount() { CurrencyId = detalleDocumento.Moneda, Value = detalleDocumento.TotalVenta },  // Valor de venta por ítem
                    PricingReference    =   new PricingReference() { AlternativeConditionPrices = new List<AlternativeConditionPrice>() },
                    Item = new Item() {
                        Descriptions                =   new List<Description>(), // Se implementa líneas abajo
                        SellersItemIdentification   =   new SellersItemIdentification() { Id = detalleDocumento.CodigoItem }, // Código de producto
                        CommodityClassification     =   new CommodityClassification() // No hay una tabla con este listado, debido a inestabilidad de Sunat y por ser demasiado extenso
                        {   // Codigo producto de SUNAT | (Catálogo No. 25) | Es excluyente cuando el tipo de operación esta entre 0200 a 0208
                            ItemClassificationCode  =   new ItemClassificationCode() { Value = detalleDocumento.CodigoProductoSunat ?? string.Empty }
                        },  // Si tipo de operación es 0112, no debe haber productos con código de Sunat igual a 84121901 o 80131501
                        // StandardItemIdentification  = new StandardItemIdentification() { } Codigo de barras, no se agregará a pedido de Tania
                        AdditionalItemProperties    =   new List<AdditionalItemProperty>()   // Implementado en la región AdditionalItemProperty
                    },
                    Price = new Price() { PriceAmount = new PayableAmount() { CurrencyId = detalleDocumento.Moneda, Value = detalleDocumento.ValorVenta } }, // Valor unitario por ítem
                };

                #region AlternativeConditionPrice
                if (detalleDocumento.PreciosAlternativos.Count > 0)
                {
                    foreach (var precioAlternativo in detalleDocumento.PreciosAlternativos)
                    {
                        linea.PricingReference.AlternativeConditionPrices.Add(new AlternativeConditionPrice()
                        {
                            PriceAmount     = new PayableAmount() { CurrencyId = precioAlternativo.TipoMoneda, Value = precioAlternativo.Monto },
                            PriceTypeCode   = new PriceTypeCode() { Value = precioAlternativo.TipoPrecio }  // Código de precio | Catálogo No. 16
                        });
                    }
                }
                #endregion AlternativeConditionPrice

                #region TaxTotal
                if (detalleDocumento.TotalImpuestos.Count > 0)
                {   // Monto total de impuestos del ítem
                    foreach (var totalImpuesto in detalleDocumento.TotalImpuestos)
                    {   // Total de impuestos para cada ítem
                        var lineaTotalImpuestos = new TaxTotal
                        {   // Monto total de impuestos por línea
                            TaxAmount = new PayableAmount() { CurrencyId = totalImpuesto.TipoMonedaTotal, Value = totalImpuesto.MontoTotal }
                        };

                        foreach (var subTotalImpuestos in totalImpuesto.SubTotalesImpuestos)
                        {   // Afectación al IGV por la línea | Afectación IVAP por la línea
                            lineaTotalImpuestos.TaxSubtotals.Add(new TaxSubtotal
                            {
                                TaxableAmount = new PayableAmount
                                {   // Monto base
                                    CurrencyId  = subTotalImpuestos.TipoMonedaBase, // Código de tipo de moneda del monto de las operaciones gravadas/exoneradas/inafectas del impuesto
                                    Value       = subTotalImpuestos.BaseImponible   // Monto las operaciones gravadas/exoneradas/inafectas del impuesto
                                },
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

                #region Description
                foreach (var descripcion in detalleDocumento.Descripciones)
                {   // Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
                    linea.Item.Descriptions.Add(new Description() { Detail = descripcion.Detalle ?? string.Empty });
                }
                #endregion Description

                #region AdditionalItemProperty
                if (detalleDocumento.PropiedadesAdicionales.Count > 0) // validar
                {
                    foreach (var propiedadAdicional in detalleDocumento.PropiedadesAdicionales)
                    {
                        linea.Item.AdditionalItemProperties.Add(new AdditionalItemProperty()
                        {
                            Name            = propiedadAdicional.Nombre, // Sí existe el Tag no debe estar vacío | Catálogo No. 55
                            NameCode        = new NameCode()        { Value   = propiedadAdicional.Codigo },
                            Value           = propiedadAdicional.ValorPropiedad, // Se le puede poner distintos valores o hubo confusión de Sunat..
                            // En la documentación vista el 30/06/2018 se observó que el campo ValueQualifier fue removido o no fue considerado en la documentación, 
                            ValueQualifier  = new ValueQualifier()  { Detail = propiedadAdicional.Concepto ?? string.Empty },
                            UsabilityPeriod = new UsabilityPeriod()
                            {
                                StartDate       = propiedadAdicional.FechaInicio, EndDate = propiedadAdicional.FechaFin,
                                DurationMeasure = new DurationMeasure() { Value = propiedadAdicional.Duracion }
                            },
                            ValueQuantity   = new ValueQuantity()   { Value = propiedadAdicional.CantidadEspecies },

                        });
                    }
                }
                #endregion AdditionalItemProperty

                #region AlternativeConditionPrice
                foreach (var precioAlternativo in detalleDocumento.PreciosAlternativos)
                {
                    linea.PricingReference.AlternativeConditionPrices.Add(new AlternativeConditionPrice()
                    {
                        PriceAmount     = new PayableAmount() { CurrencyId = precioAlternativo.TipoMoneda, Value = precioAlternativo.Monto },
                        PriceTypeCode   = new PriceTypeCode() { Value = precioAlternativo.TipoPrecio }  // Código de precio | Catálogo No. 16
                    });
                }
                #endregion AlternativeConditionPrice

                creditNote.CreditNoteLines.Add(linea);
            }

            return creditNote;
        }
    }
}
