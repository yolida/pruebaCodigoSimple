using DataLayer.CRUD;
using GenerateXML;
using Models.Intercambio;
using Models.Modelos;
using Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GenerarFactura
    {
        private readonly IDocumentoXml _documentoXml;
        private readonly ISerializador _serializador;

        public GenerarFactura()
        {
            FacturaXml documentoElectronico = new FacturaXml();
            _documentoXml = (IDocumentoXml)documentoElectronico;

            Serializador serializador = new Serializador();
            _serializador = (ISerializador)serializador;
        }
        public string       IdDocumento     { get; set; }
        
        /// <summary>
        /// IMPORTANTE
        /// Este método obtiene todos los datos  del documento electrónico, por el momento esta como sincrono para testing pero en producción se pondrá como asíncrono
        /// </summary>
        /// <param name="data_Documento"></param>
        /// <returns></returns>
        public DocumentoElectronico Data(Data_Documentos data_Documento)
        {
            DocumentoElectronico documento;
            try
            {
                Data_CabeceraDocumento cabeceraDocumento    =   new Data_CabeceraDocumento(data_Documento.IdCabeceraDocumento);
                cabeceraDocumento.Read_CabeceraDocumento();

                documento    =   new DocumentoElectronico()  //  Documento principal
                {
                    SerieCorrelativo    =   data_Documento.SerieCorrelativo,
                    TipoDocumento       =   data_Documento.TipoDocumento ?? string.Empty,
                    FechaEmision        =   cabeceraDocumento.FechaEmision,
                    HoraEmision         =   cabeceraDocumento.HoraEmision,
                    FechaVencimiento    =   cabeceraDocumento.FechaVencimiento,
                    OrdenCompra         =   cabeceraDocumento.OrdenCompra,
                    Moneda              =   cabeceraDocumento.Moneda,
                    TipoOperacion       =   cabeceraDocumento.TipoOperacion,
                    CantidadItems       =   cabeceraDocumento.CantidadItems,
                    TotalValorVenta     =   cabeceraDocumento.TotalValorVenta,
                    TotalPrecioVenta    =   cabeceraDocumento.TotalPrecioVenta,
                    TotalDescuento      =   cabeceraDocumento.TotalDescuento,
                    TotalOtrosCargos    =   cabeceraDocumento.TotalOtrosCargos,
                    TotalAnticipos      =   cabeceraDocumento.TotalAnticipos,
                    ImporteTotalVenta   =   cabeceraDocumento.ImporteTotalVenta
                };

                #region Emisor
                Data_Contribuyente Emisor   =   new Data_Contribuyente(data_Documento.IdEmisor);
                Emisor.Read_Contribuyente();
                documento.Emisor            =   Emisor;
                #endregion Emisor

                #region Receptor
                Data_Contribuyente Receptor =   new Data_Contribuyente(cabeceraDocumento.IdReceptor);
                Receptor.Read_Contribuyente();
                documento.Receptor          =   Receptor;
                documento.Receptor.OtrosParticipantes = new List<Contribuyente>();  //  Pendiente crear entidad OtrosParticipantes
                #endregion Receptor

                #region documentoDetalle
                Data_DocumentoDetalle data_DocumentoDetalle =   new Data_DocumentoDetalle(data_Documento.IdCabeceraDocumento);
                List<DetalleDocumento> detalleDocumentos    =   data_DocumentoDetalle.Read_DocumentoDetalle();

                if (detalleDocumentos.Count > 0)    //  Validar en caso de que no haya detalles del documento
                {
                    foreach (var detalleDocumento in detalleDocumentos)
                    {
                        #region lineaTotalImpuesto
                        Data_TotalImpuesto lineaTotalImpuesto   =   new Data_TotalImpuesto(detalleDocumento.IdDocumentoDetalle);
                        List<TotalImpuesto> lineaTotalImpuestos =   lineaTotalImpuesto.Read_TotalImpuestos(2);  //  El parámetro -> 2 <- es indicativo de que es por cada línea

                        foreach (var st_totalImpuesto in lineaTotalImpuestos)
                        {
                            Data_SubTotalImpuesto data_SubTotalImpuesto =   new Data_SubTotalImpuesto(st_totalImpuesto.IdTotalImpuestos);
                            List<SubTotalImpuestos> subTotalImpuestos   =   data_SubTotalImpuesto.Read_SubTotalImpuesto();
                            st_totalImpuesto.SubTotalesImpuestos        =   subTotalImpuestos;
                        }
                        detalleDocumento.TotalImpuestos =   lineaTotalImpuestos;
                        #endregion lineaTotalImpuesto

                        #region Notas
                        Data_Nota lineaData_Nota    =   new Data_Nota(detalleDocumento.IdDocumentoDetalle);  // Parámetro es el id de cabecera de documento
                        List<Nota> lineaNotas       =   lineaData_Nota.Read(2);
                        detalleDocumento.Notas      =   lineaNotas;
                        #endregion Notas

                        #region Descripciones
                        List<Descripcion> descripciones =   data_DocumentoDetalle.Read_Descripcion(detalleDocumento.IdDocumentoDetalle);
                        detalleDocumento.Descripciones  =   descripciones;
                        #endregion Descripciones

                        #region PrecioAlternativo
                        Data_PrecioAlternativo data_PrecioAlternativo   =   new Data_PrecioAlternativo(detalleDocumento.IdDocumentoDetalle);
                        List<PrecioAlternativo> precioAlternativos      =   data_PrecioAlternativo.Read_PrecioAlternativo();
                        detalleDocumento.PreciosAlternativos            =   precioAlternativos;                    
                        #endregion PrecioAlternativo

                        #region emergency
                        List<PropiedadAdicional> propiedadAdicionales = new List<PropiedadAdicional>();
                        detalleDocumento.PropiedadesAdicionales = propiedadAdicionales;

                        List<Entrega> entregas1 = new List<Entrega>();

                        Entrega data_entrega;
                        data_entrega = new Entrega()
                        {
                            Cantidad = 0,
                            MaximaCantidad = 0,
                            Envio = new Envio(),
                        };
                        entregas1.Add(data_entrega);
                                        
                        detalleDocumento.Entregas = entregas1;
                        #endregion emergency
                    }
                    documento.DetalleDocumentos     =   detalleDocumentos;
                }
                #endregion documentoDetalle

                #region TotalImpuestos
                Data_TotalImpuesto data_TotalImpuesto   =   new Data_TotalImpuesto(data_Documento.IdCabeceraDocumento);
                List<TotalImpuesto> totalImpuestos      =   data_TotalImpuesto.Read_TotalImpuestos(1);   //  El parámetro -> 1 <- es indicativo de que es por cada línea

                foreach (var st_totalImpuesto in totalImpuestos)
                {
                    Data_SubTotalImpuesto data_SubTotalImpuesto =   new Data_SubTotalImpuesto(st_totalImpuesto.IdTotalImpuestos);
                    List<SubTotalImpuestos> subTotalImpuestos   =   data_SubTotalImpuesto.Read_SubTotalImpuesto();
                    st_totalImpuesto.SubTotalesImpuestos        =   subTotalImpuestos;
                }
                documento.TotalImpuestos = totalImpuestos;
                #endregion TotalImpuestos

                #region Notas
                Data_Nota data_Nota =   new Data_Nota(data_Documento.IdCabeceraDocumento);  // Parámetro es el id de cabecera de documento
                List<Nota> notas    =   data_Nota.Read(1);
                documento.Notas     =   notas;
                #endregion Notas

                #region TerminosEntregas
                Data_TerminosEntrega data_TerminosEntrega   =   new Data_TerminosEntrega(data_Documento.IdCabeceraDocumento);
                data_TerminosEntrega.Read_TerminosEntrega();   //  El parámetro -> 1 <- es indicativo de que es por cada línea
                documento.TerminosEntrega                   =   data_TerminosEntrega;
                #endregion TerminosEntregas

                #region Anticipos
                Data_Anticipo anticipo  =   new Data_Anticipo(data_Documento.IdCabeceraDocumento);
                List<Anticipo> anticipos=   anticipo.Read_Anticipo();
                documento.Anticipos     =   anticipos;
                #endregion Anticipos

                #region Relacionados
                Data_DocumentoRelacionado data_DocumentoRelacionado =   new Data_DocumentoRelacionado(data_Documento.IdCabeceraDocumento);
                List<DocumentoRelacionado> documentoRelacionados    =   data_DocumentoRelacionado.Read_DocumentoRelacionado();
                documento.Relacionados                              =   documentoRelacionados;
                #endregion Relacionados

                #region Descuentos
                Data_Descuento data_Descuento   =   new Data_Descuento(data_Documento.IdCabeceraDocumento);
                List<Descuento> descuentos      =   data_Descuento.Read_Descuento_Total();
                documento.Descuentos            =   descuentos;
                #endregion Descuentos

                List<PeriodoFactura> periodoFacturas = null;
                List<DocumentoRelacionado> otrosDocumentosRelacionados = null;
                List<Entrega> entregas = null;
                List<MedioPago> medioPagos = null;

                documento.OtrosDocumentosRelacionados   = otrosDocumentosRelacionados;
                documento.Entregas                      = entregas;
                documento.MedioPagos                    = medioPagos;
                documento.PeriodosFactura               = periodoFacturas;

            }
            catch (Exception ex)
            {
                documento   =   new DocumentoElectronico();
            }

            return documento;
        }
        
        public async Task<DocumentoResponse> Post(DocumentoElectronico documento)
        {
            var response    =   new DocumentoResponse();
            try
            {
                var invoice                 =   _documentoXml.Generar(documento);
                response.TramaXmlSinFirma   =   await _serializador.GenerarXml(invoice);
                response.Exito              =   true;
            }
            catch (Exception ex)
            {
                response.MensajeError       =   ex.Message;
                response.Pila               =   ex.StackTrace;
                response.Exito              =   false;
            }

            return response;
        }

        public DocumentoResponse Post(DocumentoElectronico documento, bool sincrono)
        {
            var response    =   new DocumentoResponse();
            try
            {
                var invoice                 =   _documentoXml.Generar(documento);
                response.TramaXmlSinFirma   =   _serializador.GenerarXml(invoice, sincrono);
                response.Exito              =   true;
            }
            catch (Exception ex)
            {
                response.MensajeError       =   ex.Message;
                response.Pila               =   ex.StackTrace;
                response.Exito              =   false;
            }

            return response;
        }
    }
}
