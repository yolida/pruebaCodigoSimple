using DataLayer;
using DataLayer.CRUD;
using GenerateXML;
using Models.Intercambio;
using Models.Modelos;
using Security;
using ServicesSunat;
using ServicesSunat.SOAP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class GenerarResumenDiario
    {
        private readonly IDocumentoXml _documentoXml;
        private readonly ISerializador _serializador;
        private readonly IServicioSunatDocumentos _servicioSunatDocumentos;
        public GenerarResumenDiario()
        {
            ResumenDiarioNuevoXml documentoElectronico = new ResumenDiarioNuevoXml();
            _documentoXml   =   (IDocumentoXml)documentoElectronico;

            Serializador serializador = new Serializador();
            _serializador   =   (ISerializador)serializador;

            ServicioSunatDocumentos servicioSunatDocumentos =   new ServicioSunatDocumentos();
            _servicioSunatDocumentos    =   (IServicioSunatDocumentos)servicioSunatDocumentos;
        }

        public ResumenDiario Data(List<Data_Documentos> data_Documentos)
        {
            DateTime dateTime   =   DateTime.Now;
            ResumenDiario resumenDiario;
            ReadGeneralData readGeneralData = new ReadGeneralData();
            try
            {
                Data_CabeceraDocumento cabeceraDocumento = new Data_CabeceraDocumento(data_Documentos[0].IdCabeceraDocumento);
                cabeceraDocumento.Read_CabeceraDocumento();

                Data_Contribuyente data_Emisor  = new Data_Contribuyente(data_Documentos[0].IdEmisor);
                data_Emisor.Read_Contribuyente();

                int numeracion  =   0;
                string mes  =   dateTime.Month  < 10 ? $"0{dateTime.Month}" : dateTime.Month.ToString();
                string dia  =   dateTime.Day    < 10 ? $"0{dateTime.Day}"   : dateTime.Day.ToString();
                try
                {
                    numeracion  =   readGeneralData.GetScalarValueINT("[dbo].[Query_Scalar_GetValue_CantidadDocsDia]", "@IdDatosFox", data_Documentos[0].IdDatosFox);
                }
                catch (Exception)
                {
                    numeracion++;
                }

                resumenDiario   =   new ResumenDiario()  //  Documento principal
                {
                    IdDocumento         =   $"RC-{dateTime.Year}{mes}{dia}-{numeracion}",
                    FechaEmision        =   dateTime.ToString(),
                    FechaReferencia     =   data_Documentos[0].FechaEmision,
                    Emisor              =   data_Emisor,
                    Resumenes           =   new List<GrupoResumenNuevo>()
                };

                int contador    =   1;
                foreach (var data_Documento in data_Documentos)
                {
                    Data_CabeceraDocumento data_CabeceraDocumento   =   new Data_CabeceraDocumento(data_Documento.IdCabeceraDocumento);
                    data_CabeceraDocumento.Read_CabeceraDocumento();

                    Data_Contribuyente data_Receptor  = new Data_Contribuyente(data_CabeceraDocumento.IdReceptor);
                    data_Receptor.Read_Contribuyente();

                    var resumen  = new GrupoResumenNuevo() {
                        Id                      =   contador,
                        TipoDocumento           =   data_Documento.TipoDocumento,
                        Serie                   =   data_Documento.SerieCorrelativo,
                        Receptor                =   data_Receptor,
                        TotalVenta              =   data_CabeceraDocumento.ImporteTotalVenta,
                        Moneda                  =   data_CabeceraDocumento.Moneda,
                        Gravadas                =   data_CabeceraDocumento.Gravadas,
                        Exoneradas              =   data_CabeceraDocumento.Exoneradas,
                        Inafectas               =   data_CabeceraDocumento.Inafectas,
                        Exportaciones           =   data_CabeceraDocumento.Exportaciones,
                        Gratuitas               =   data_CabeceraDocumento.Gratuitas,
                        TotalDescuentos         =   data_CabeceraDocumento.TotalDescuento,
                    };
                        
                    Data_DocumentoRelacionado data_DocumentoRelacionado =   new Data_DocumentoRelacionado(data_Documento.IdCabeceraDocumento);
                    List<DocumentoRelacionado> documentoRelacionados    =   data_DocumentoRelacionado.Read_DocumentoRelacionado();
                    if (documentoRelacionados.Count > 0)
                        resumen.DocumentoRelacionado    =   documentoRelacionados[0].NroDocumento ?? string.Empty; // Sólo habrá un doc relacionado para cada boleta
                    
                    Data_Discrepancia data_Discrepancia =   new Data_Discrepancia(data_Documento.IdCabeceraDocumento);
                    List<Discrepancia> discrepancias    =   data_Discrepancia.Read_DiscrepanciaDocumento();
                    if (discrepancias.Count > 0)
                        resumen.CodigoEstadoItem    =   Convert.ToInt32(discrepancias[0].Tipo ?? string.Empty); // // en la documentación dice que es el valor del tipo de discrepancia catalogo 9
                    else
                        resumen.CodigoEstadoItem    =   1;  // Poca información respecto a lo que va aquí

                        #region TotalImpuestos
                    Data_TotalImpuesto data_TotalImpuesto   =   new Data_TotalImpuesto(data_Documento.IdCabeceraDocumento);
                    List<TotalImpuesto> totalImpuestos      =   data_TotalImpuesto.Read_TotalImpuestos(1);   //  El parámetro -> 1 <- es indicativo de que es por cada línea

                    foreach (var st_totalImpuesto in totalImpuestos)
                    {
                        Data_SubTotalImpuesto data_SubTotalImpuesto =   new Data_SubTotalImpuesto(st_totalImpuesto.IdTotalImpuestos);
                        List<SubTotalImpuestos> subTotalImpuestos   =   data_SubTotalImpuesto.Read_SubTotalImpuesto();
                        st_totalImpuesto.SubTotalesImpuestos        =   subTotalImpuestos;
                    }
                    resumen.TotalImpuestos = totalImpuestos;
                    #endregion TotalImpuestos

                    resumenDiario.Resumenes.Add(resumen);
                    contador++;
                }
            }
            catch (Exception ex)
            {
                resumenDiario   =   new ResumenDiario();
            }
            
            return resumenDiario;
        }

        public DocumentoResponse Post(ResumenDiario resumen)
        {
            var response = new DocumentoResponse();
            try
            {
                var summary                 =   _documentoXml.Generar(resumen);
                response.TramaXmlSinFirma   =   _serializador.GenerarXml(summary, true);
                response.Exito              =   true;
            }
            catch (Exception ex)
            {
                response.Exito          =   false;
                response.MensajeError   =   ex.Message;
                response.Pila           =   ex.StackTrace;
            }

            return response;
        }
    }
}
