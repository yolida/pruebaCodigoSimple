using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Models.Contratos;
using Models.Modelos;
using StructureUBL.CommonAggregateComponents;
using StructureUBL.CommonBasicComponents;
using StructureUBL.CommonExtensionComponents;
using StructureUBL.CommonStaticComponents;
using StructureUBL.EstandarUbl;
using StructureUBL.SunatAggregateComponents;

namespace GenerateXML
{
    public class FacturaXml : IDocumentoXml
    {
        IEstructuraXml IDocumentoXml.Generar(IDocumentoElectronico request)
        {
            var documento           =   (DocumentoElectronico)request;
            var invoice = new Invoice
            {
                // Falta agregar EXTENSION
                UblVersionId    = "2.1",
                CustomizationId = new CustomizationID() { Value = "2.0" },
                IdInvoice       = documento.SerieCorrelativo,   // Serie y número del comprobante
                IssueDate       = documento.FechaEmision,       // Fecha de emisión yyyy-mm-dd
                IssueTime       = documento.HoraEmision,        // Hora de emisión hh-mm-ss.0z
                DueDate         = documento.FechaVencimiento,   // Fecha de vencimiento yyyy-mm-dd | Fecha de Pago
                InvoiceTypeCode = new InvoiceTypeCode() { Value = documento.TipoDocumento, ListID = documento.TipoOperacion }, // Código de tipo de documento
                // Note
                DocumentCurrencyCode    = new DocumentCurrencyCode() { Value = documento.Moneda }, // Código de tipo de moneda en la cual se emite la factura electrónica
                LineCountNumeric        = documento.CantidadItems,
                // InvoicePeriod
                OrderReference  = new OrderReference() { Id = documento.OrdenCompra ?? string.Empty },
                // DespatchDocumentReference
                // ContractDocumentReference --> Pertenece a Servicios públicos
                // AdditionalDocumentReference
                Signature = new Signature
                {
                    //Id = documento.SerieCorrelativo,
                    Id = documento.Emisor.NroDocumento,
                    SignatoryParty = new SignatoryParty
                    {
                        PartyIdentification = new PartyIdentification
                        {
                            Id = new PartyIdentificationId
                            {
                                Value = documento.Emisor.NroDocumento
                            }
                        },
                        PartyName = new PartyName
                        {
                            Name = documento.Emisor.NombreLegal
                        }
                    },
                    DigitalSignatureAttachment = new DigitalSignatureAttachment
                    {
                        ExternalReference = new ExternalReference
                        {
                            Uri = $"{documento.Emisor.NroDocumento}-{documento.SerieCorrelativo}"
                        }
                    }
                },
                AccountingSupplierParty = new AccountingContributorParty()
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
                AccountingCustomerParty = new AccountingContributorParty() // AccountingSupplierParty y AccountingCustomerParty son instancias
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
                // PayeeParty
                // Delivery, aun por implementar
                DeliveryTerms   = new DeliveryTerms
                {
                    Id      = documento.TerminosEntrega.NumeroRegistro ?? string.Empty,
                    Amount  = new Amount {
                        CurrencyID  = documento.TerminosEntrega.TipoMoneda,
                        Value       = documento.TerminosEntrega.Monto
                    },
                    DeliveryLocation = new DeliveryLocation()
                    {
                        Address = new Address()
                        {
                            StreetName              = documento.TerminosEntrega.Direccion,
                            CitySubdivisionName     = documento.TerminosEntrega.Urbanizacion,
                            CityName                = documento.TerminosEntrega.Provincia,
                            CountrySubentity        = documento.TerminosEntrega.Departamento,
                            CountrySubentityCode    = documento.TerminosEntrega.Ubigeo,
                            District                = documento.TerminosEntrega.Distrito,
                            Country     = new Country()
                            {   // Dirección del lugar en el que se entrega el bien (Código de país)
                                IdentificationCode = new IdentificationCode()
                                {   // <cbc:IdentificationCode listID="ISO3166-1" listAgencyID="6">DK</cbc:IdentificationCode>
                                    Value = documento.TerminosEntrega.Alfa2
                                }
                            }

                        }
                    }
                },
                PaymentsMeans       = new List<PaymentMeans> (),
                PaymentsTerms       = new List<PaymentTerms> (),
                PrepaidPayments     = new List<PrepaidPayment> (),
                AllowanceCharges    = new List<AllowanceCharge> (),
                TaxTotals           = new List<TaxTotal> (),
                LegalMonetaryTotal  = new LegalMonetaryTotal() {
                    LineExtensionAmount     = new PayableAmount() { Value = documento.TotalValorVenta,      CurrencyId = documento.Moneda },
                    TaxInclusiveAmount      = new PayableAmount() { Value = documento.TotalPrecioVenta,     CurrencyId = documento.Moneda },
                    AllowanceTotalAmount    = new PayableAmount() { Value = documento.TotalDescuento,       CurrencyId = documento.Moneda },
                    ChargeTotalAmount       = new PayableAmount() { Value = documento.TotalOtrosCargos,     CurrencyId = documento.Moneda },
                    PrepaidAmount           = new PayableAmount() { Value = documento.TotalAnticipos,       CurrencyId = documento.Moneda },
                    PayableAmount           = new PayableAmount() { Value = documento.ImporteTotalVenta,    CurrencyId = documento.Moneda },
                    PayableRoundingAmount   = new PayableAmount() { Value = documento.MontoRedondeo,        CurrencyId = documento.Moneda },
                }
                // InvoiceLine
            };
            // Lista de otros participantes asociados a la transacción 
            //if (documento.Receptor.OtrosParticipantes.Count > 0) // mejorar validación
            //{
            //    foreach (var otroParticipante in documento.Receptor.OtrosParticipantes)
            //    {
            //        invoice.AccountingCustomerParty.Party.PartyLegalEntity.ShareholderParties.Add(new ShareholderParty
            //        {
            //            Party = new Party()
            //            {
            //                PartyIdentification = new PartyIdentification()
            //                {
            //                    Id = new PartyIdentificationId() { SchemeId = otroParticipante.TipoDocumento, Value = otroParticipante.NroDocumento }
            //                }
            //            }
            //        });
            //    }
            //}

            // Datos en lista
            if (documento.Notas.Count > 0 && documento.Notas != null)
            {
                foreach (var nota in documento.Notas)
                {
                    if (!string.IsNullOrEmpty(nota.Descripcion.ToString()))
                    {
                        invoice.Notes.Add(new Note // Se agrega al objeto Note
                        {
                            LanguageLocaleID    =   nota.Codigo,    //  Código de leyenda
                            Value               =   nota.Descripcion    //  Descripcion
                        });
                    }
                }
            }

            #region Get out InvoicePeriod
            // Ciclo de facturación
            //if (documento.PeriodosFactura.Count > 0 && documento.PeriodosFactura != null)
            //{
            //    try
            //    {
            //        foreach (var periodoFactura in documento.PeriodosFactura)
            //        {
            //            if (!string.IsNullOrEmpty(periodoFactura.FechaInicio.ToString()) || !string.IsNullOrEmpty(periodoFactura.FechaFin.ToString()))
            //            {
            //                invoice.InvoicePeriods.Add(new InvoicePeriod
            //                {
            //                    StartDate = periodoFactura.FechaInicio,   // Fecha de inicio de ciclo de facturación
            //                    EndDate = periodoFactura.FechaFin       // Fecha de fin de ciclo de facturación
            //                });
            //            }
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}
            #endregion Get out InvoicePeriod

            // documento relacionado o despatchdocumentreferences
            //if (documento.relacionados.count > 0 && documento.relacionados != null)
            //{
            //    foreach (var relacionado in documento.relacionados) // 0..n; relacionados = despatchdocumentreference
            //    {
            //        if (!string.isnullorempty(relacionado.tipodocumento.tostring()))
            //        {
            //            invoice.despatchdocumentreferences.add(new invoicedocumentreference
            //            {
            //                id = relacionado.nrodocumento, // número de guía de remisión relacionada con la operación que se factura
            //                documenttypecode = new documenttypecode()
            //                {   // código de tipo de guía de remisión relacionada con la operación que se factura
            //                    listname = "tipo de documento", listuri = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01", value = relacionado.tipodocumento
            //                },
            //            });
            //        }
            //    }
            //}

            #region Get out ContractDocumentReference
            // Documento contrato
            //if (documento.DocumentoContratos.Count > 0 && documento.DocumentoContratos != null)
            //{
            //    foreach (var documentoContrato in documento.DocumentoContratos)
            //    {
            //        if (!string.IsNullOrEmpty(documentoContrato.IdNumero))
            //        {
            //            invoice.ContractDocumentReferences.Add(new ContractDocumentReference
            //            {
            //                Id = documentoContrato.IdNumero, // cbc:ID
            //                DocumentTypeCode = new ContractDocumentReference_DocumentTypeCode()
            //                {   // cbc:DocumentTypeCode
            //                    Value = documentoContrato.TipoServicioPublico
            //                },
            //                LocaleCode = new LocaleCode() { Value = documentoContrato.ServicioTelecomunicaciones },
            //                DocumentStatusCode = new DocumentStatusCode() { Value = documentoContrato.TipoTarifaContratada }
            //            });
            //        }
            //    }
            //}
            #endregion Get out ContractDocumentReference

            //Otros documentos relacionados
            //if (documento.OtrosDocumentosRelacionados.Count > 0 && documento.OtrosDocumentosRelacionados != null)
            //{
            //    foreach (var relacionado in documento.OtrosDocumentosRelacionados)
            //    {
            //        if (!string.IsNullOrEmpty(relacionado.TipoDocumento.ToString()))
            //        {
            //            var otroDocumentoRelacionado = new InvoiceDocumentReference()
            //            //invoice.AdditionalDocumentReferences.Add(new InvoiceDocumentReference
            //            {
            //                Id = relacionado.NroDocumento,  // Número de documento relacionado con la operación que se factura
            //                DocumentTypeCode = new DocumentTypeCode()
            //                {   // Código de tipo de documento relacionado con la operación que se factura
            //                    ListName = "Documento Relacionado", ListURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo12", Value = relacionado.TipoDocumento
            //                },
            //                DocumentStatusCode  = new DocumentStatusCode() { Value = relacionado.IdentificadorPago, ListName = "Anticipo" }, // Identificador del pago
            //                IssuerParty         = new IssuerParty() { PartyIdentifications = new List<PartyIdentification>() }
            //            };

            //            if (relacionado.Identificaciones.Count > 0)
            //            {
            //                foreach (var identificacion in relacionado.Identificaciones)
            //                {
            //                    otroDocumentoRelacionado.IssuerParty.PartyIdentifications.Add(new PartyIdentification()
            //                    {
            //                        Id = new PartyIdentificationId() { SchemeId = identificacion.TipoDocumento, Value = identificacion.NroDocumento }
            //                    });
            //                }
            //            }
            //            invoice.AdditionalDocumentReferences.Add(otroDocumentoRelacionado);
            //        }
            //    }
            //}

            // Delivery en la documentación dice que es de 0..n de cero a muchos (FALTA)
            //if (documento.Entregas.Count > 0 && documento.Entregas != null) // 
            //{
            //    foreach (var entrega in documento.Entregas)
            //    {
            //        if (!string.IsNullOrEmpty(entrega.Codigo))
            //        {
            //            var varEntrega      = new Delivery() {
            //                DeliveryId          = new DeliveryId()      { Value = entrega.Codigo        },  // Actualizado conforme al documento del 30/06/2018
            //                Quantity            = new Quantity()        { Value = entrega.Cantidad      },
            //                MaximumQuantity     = new MaximumQuantity() { Value = entrega.MaximaCantidad },
            //                DeliveryLocation    = new DeliveryLocation()
            //                {
            //                    Address = new Address()
            //                    {
            //                        AddressLine         = new AddressLine() { Line = entrega.Direccion }, // Dirección completa y detallada
            //                        CitySubdivisionName = entrega.Urbanizacion, // Urbanización
            //                        CityName            = entrega.Provincia,
            //                        Id                  = new IdUbigeo()    { Value = entrega.Ubigeo },
            //                        CountrySubentity    = entrega.Departamento,
            //                        District            = entrega.Distrito,
            //                        Country             = new Country()     { IdentificationCode = new IdentificationCode() { Value = entrega.Pais } },
            //                    }
            //                },
            //                DeliveryParty       = new DeliveryParty() { PartyLegalEntities = new List<PartyLegalEntity>() }, // Se rellena en la región DeliveryParty
            //                Shipment            = new Shipment() {  // Envío
            //                    Id                      = new ShipmentId()          { Value = entrega.Envio.CodMotivoTraslado },
            //                    GrossWeightMeasure      = new InvoicedQuantity()    { UnitCode  = entrega.Envio.UnidadMedida, Value = entrega.Envio.PesoBruto },
            //                    ShipmentStages          = new List<ShipmentStage>(), // Se rellena en la región ShipmentStage
            //                    TransportHandlingUnit   = new TransportHandlingUnit() { TransportEquipments = new List<TransportEquipment>() },
            //                    Delivery                = new Delivery()            { DeliveryAddress = new DeliveryAddress() {
            //                        Id          = new DeliveryAddressId()   { Value = entrega.Envio.PuntoLlegadaUbigeo   },
            //                        AddressLine = new AddressLine()         { Line = entrega.Envio.PuntoLlegadaDireccion }
            //                    } },
            //                    OriginAddress   = new OriginAddress() {
            //                        Id          = new OriginAddressID() { Value = entrega.Envio.PuntoLlegadaUbigeo   },
            //                        AddressLine = new AddressLine()     { Line = entrega.Envio.PuntoLlegadaDireccion }
            //                    },
            //                }
            //            };

            //            #region DeliveryParty
            //            if (entrega.EntregaDetalle.NroDocDestinatarios.Count > 0)
            //            {
            //                foreach (var nroDocDestinatario in entrega.EntregaDetalle.NroDocDestinatarios)
            //                {
            //                    varEntrega.DeliveryParty.PartyLegalEntities.Add(new PartyLegalEntity()
            //                    {
            //                        CompanyID = new CompanyID() { SchemeID = nroDocDestinatario.TipoDocumento, Value = nroDocDestinatario.NroDocumento ?? string.Empty }
            //                    });
            //                }
            //                varEntrega.DeliveryParty.MarkAttentionIndicator = entrega.EntregaDetalle.IndSubContratacion;
            //            }
            //            #endregion DeliveryParty

            //            #region ShipmentStage
            //            if (entrega.Envio.EtapaEnvios.Count > 0) // Validación pendiente
            //            {   // ShipmentStage
            //                foreach (var etapaEnvio in entrega.Envio.EtapaEnvios)
            //                {
            //                    var varEtapaEnvio =   new ShipmentStage() { 
            //                        TransportModeCode   = new TransportModeCode()   { Value     = etapaEnvio.ModalidadTransporte }, // [0..1]
            //                        TransitPeriod       = new TransitPeriod()       { StartDate = etapaEnvio.FechaInicioTraslado }, // [0..1] 
            //                        CarrierParties      = new List<CarrierParty>(), // [0..*]
            //                        TransportMeans      = new TransportMeans()
            //                        {
            //                            RegistrationNationalityID = etapaEnvio.Vehiculo.Constancia,
            //                            RoadTransport   = new RoadTransport() { LicensePlateID = etapaEnvio.Vehiculo.Placa }
            //                        },
            //                        DriverPersons = new List<PartyIdentification>() // [0..*]
            //                    };
            //                    varEntrega.Shipment.ShipmentStages.Add(varEtapaEnvio);

            //                    #region CarrierParties
            //                    foreach (var transportista in etapaEnvio.Transportistas)
            //                    {
            //                        varEtapaEnvio.CarrierParties.Add(new CarrierParty()
            //                        {
            //                            PartyIdentification = new PartyIdentification()
            //                            {
            //                                Id = new PartyIdentificationId() { Value = transportista.NroDocumento, SchemeId = transportista.TipoDocumento },
            //                            },
            //                            PartyLegalEntity = new PartyLegalEntity()
            //                            {
            //                                RegistrationName = transportista.NombreLegal, CompanyID = new CompanyID() { Value = transportista.RegistroMTC }
            //                            }
            //                        });
            //                    }
            //                    #endregion CarrierParties

            //                    #region DriverPersons
            //                    foreach (var conductor in etapaEnvio.Conductores)
            //                    {   // // Datos de conductores - Número de documento de identidad | Tipo de documento
            //                        varEtapaEnvio.DriverPersons.Add(new PartyIdentification()
            //                        {
            //                            Id = new PartyIdentificationId() { SchemeId = conductor.TipoDocumento, Value = conductor.NroDocumento ?? string.Empty }
            //                        });
            //                    }
            //                    #endregion DriverPersons
            //                }
            //            }
            //            #endregion ShipmentStage

            //            #region TransportEquipment
            //            foreach (var vehiculo in entrega.Envio.Vehiculos)
            //            {
            //                varEntrega.Shipment.TransportHandlingUnit.TransportEquipments.Add(new TransportEquipment() { Id = vehiculo.Placa ?? string.Empty });
            //            }
            //            #endregion TransportEquipment

            //            invoice.Deliveries.Add(varEntrega);
            //        }
                    
            //    }
            //}

            // Medio de pago
            //if (documento.MedioPagos.Count > 0 && documento.MedioPagos != null)
            //{
            //    foreach (var medioPago in documento.MedioPagos)
            //    {
            //        if (!string.IsNullOrEmpty(medioPago.NroCuenta.ToString()))
            //        {
            //            invoice.PaymentsMeans.Add(new PaymentMeans()
            //            {   // Cuenta del banco de la nacion (detraccion)
            //                PaymentID               = medioPago.Autorizacion,
            //                PayeeFinancialAccount   = new PayeeFinancialAccount()   { Id = medioPago.NroCuenta          ?? string.Empty },  // Número de cuenta
            //                PaymentMeansCode        = new PaymentMeansCode()        { Value = medioPago.CodigoCatalogo  ?? string.Empty }   // Código de medio de pago
            //            });
            //        }
            //    }
            //}

            // Terminos de pago
            //if (documento.TerminosPagos.Count > 0 && documento.TerminosPagos != null)
            //{
            //    foreach (var terminosPago in documento.TerminosPagos)
            //    {
            //        if (!string.IsNullOrEmpty(terminosPago.Codigo))
            //        {   // Monto y Porcentaje de la detracción
            //            invoice.PaymentsTerms.Add(new PaymentTerms()
            //            {
            //                PaymentTermsId  = new PaymentTermsId() { Value = terminosPago.Codigo }, // Catálogo No. 54
            //                PaymentPercent  = terminosPago.Porcentaje,  // Tasa o porcentaje de detracción
            //                Amount          = terminosPago.Monto        // Monto de detraccion
            //            });
            //        }
            //    }
            //}

            // PrepaidPayment
            if (documento.Anticipos.Count > 0 && documento.Anticipos != null)
            {
                foreach (var anticipo in documento.Anticipos)
                {   // Información prepagado o anticipado (Deducción de anticipos)
                    invoice.PrepaidPayments.Add(new PrepaidPayment
                    {
                        PrepaidPaymentId = new PrepaidPaymentId // cbc:ID | Identificador del pago
                        {   // Serie y número de comprobante del anticipo (para el caso de reorganización de empresas, incluye el RUC)
                            SchemeID    = anticipo.TipoDocumento, Value = anticipo.ComprobanteAnticipo
                        },
                        PaidAmount      = new PaidAmount { CurrencyID = anticipo.Moneda, Value = anticipo.Monto }, // Monto anticipado
                        PaidTime        = anticipo.FechaPago
                    });
                }
            }

            //// AllowanceCharge o Descuentos
            //if (documento.Descuentos.Count > 0 && documento.Descuentos != null)
            //{
            //    foreach (var descuentos in documento.Descuentos)
            //    {
            //        if (!string.IsNullOrEmpty(descuentos.Indicador.ToString()))
            //        {
            //            invoice.AllowanceCharges.Add(new AllowanceCharge
            //            {
            //                ChargeIndicator             = descuentos.Indicador,
            //                AllowanceChargeReasonCode   = new AllowanceChargeReasonCode() { Value = descuentos.CodigoMotivo },
            //                MultiplierFactorNumeric     = descuentos.Factor,
            //                Amount      = new PayableAmount { CurrencyId = descuentos.Moneda,       Value = descuentos.Monto },
            //                BaseAmount  = new PayableAmount { CurrencyId = descuentos.MonedaBase,   Value = descuentos.MontoBase }
            //            });
            //        }
            //    }
            //}

            // TaxTotal o TotalImpuestos
            if (documento.TotalImpuestos.Count > 0 && documento.TotalImpuestos != null)
            {   // Total de impuestos para el documento
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
                    invoice.TaxTotals.Add(lineaTotalImpuestos); // Realizar debug
                }
            }

            // InvoiceLine o DetalleFactura
            foreach (var detalleDocumento in documento.DetalleDocumentos)
            {
                var linea = new DocumentLine {
                    IdDocumentLine = detalleDocumento.NumeroOrden, // Número de orden del Ítem | No debe repetirse
                    InvoicedQuantity = new InvoicedQuantity() {
                        Value = detalleDocumento.Cantidad,    // Cantidad de unidades por ítem
                        UnitCode = detalleDocumento.UnidadMedida // Unidad de medida por ítem | (Catálogo No. 03)
                    },
                    LineExtensionAmount = new PayableAmount() { CurrencyId = detalleDocumento.Moneda, Value = detalleDocumento.TotalVenta },    // Valor de venta por ítem
                    //TaxPointDate = "", // Se omitió esta etiqueta en la documentacion EXCEL de SUNAT
                    PricingReference = new PricingReference() { AlternativeConditionPrices = new List<AlternativeConditionPrice>() }, // La implementación esta dentro de la region AlternativeConditionPrice
                    // AllowanceCharge
                    //TaxTotals = new List<TaxTotal>(),   // Implementado en la región TaxTotal
                    Item = new Item() {
                        Descriptions = new List<Description>(), // Se implementa líneas abajo
                        SellersItemIdentification = new SellersItemIdentification() { Id = detalleDocumento.CodigoItem }, // Código de producto
                        CommodityClassification = new CommodityClassification() // No hay una tabla con este listado, debido a inestabilidad de Sunat y por ser demasiado extenso
                        {   // Codigo producto de SUNAT | (Catálogo No. 25) | Es excluyente cuando el tipo de operación esta entre 0200 a 0208
                            ItemClassificationCode = new ItemClassificationCode() { Value = detalleDocumento.CodigoProductoSunat ?? string.Empty }
                        },  // Si tipo de operación es 0112, no debe haber productos con código de Sunat igual a 84121901 o 80131501
                        // StandardItemIdentification  = new StandardItemIdentification() { } Codigo de barras, no se agregará a pedido de Tania
                        AdditionalItemProperties = new List<AdditionalItemProperty>()   // Implementado en la región AdditionalItemProperty
                    },
                    Price = new Price() { PriceAmount = new PayableAmount() { CurrencyId = detalleDocumento.Moneda, Value = detalleDocumento.ValorVenta } }, // Valor unitario por ítem
                    Deliveries = new List<Delivery>()
                };

                //if (true) Aun no se completa debido a que en la documentación especificada por Tania no se contempla estas etiquetas
                //{
                //    foreach (var entrega in detalleDocumento.Entregas)
                //    {
                //        linea.Deliveries.Add(new Delivery()
                //        {
                //            //DeliveryParty = new DeliveryParty() {
                //            //    PartyIdentification = new PartyIdentification() { Id = new PartyIdentificationId() { Value = AUN POR COMPLETAR!  } }
                //            //},
                //            Despatch = new Despatch()
                //            {
                //                DespatchAddress = new DespatchAddress() { ID = new DespatchAddressID() { Value = "" } }
                //            }
                //        });
                //    }
                //}

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

                #region AllowanceCharge
                if (detalleDocumento.Descuentos.Count > 0) // Validar
                {
                    foreach (var descuento in detalleDocumento.Descuentos)
                    {   // De estar vacio o nulo, este boleano indicara si se debe llenar el nodo o no ya que es nodo requerido para su nodo padre
                        if (!string.IsNullOrEmpty(descuento.Indicador.ToString()))
                        {   
                            linea.AllowanceCharges.Add(new AllowanceCharge()
                            {
                                ChargeIndicator             = descuento.Indicador,  // Indicador de cargo/descuento
                                AllowanceChargeReasonCode   = new AllowanceChargeReasonCode() { Value = descuento.CodigoMotivo }, // Código de cargo/descuento
                                MultiplierFactorNumeric     = descuento.Factor,     // Factor de cargo/descuento
                                Amount                      = new PayableAmount { CurrencyId = descuento.Moneda, Value = descuento.Monto }, // Monto de cargo/descuento
                                BaseAmount                  = new PayableAmount { CurrencyId = descuento.MonedaBase, Value = descuento.MontoBase } // Monto base del cargo/descuento
                            });
                        }
                    }
                }
                #endregion AllowanceCharge
                    
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
                if (detalleDocumento.Descripciones.Count > 0) // Validar
                {
                    foreach (var descripcion in detalleDocumento.Descripciones)
                    {   // Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características.
                        linea.Item.Descriptions.Add(new Description() { Detail = descripcion.Detalle ?? string.Empty});
                    }
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
                
                invoice.InvoiceLines.Add(linea);
            }

            return invoice;
        }
        
    }
}