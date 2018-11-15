using DataLayer.CRUD;
using Models.Intercambio;
using Models.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ProcesarEnvio
    {
        Data_Usuario data_Usuario;
        Data_Log data_Log;

        public ProcesarEnvio(Data_Usuario idUser_Empresa)
        {
            data_Usuario    =   idUser_Empresa;
        }

        public void Post(Data_Documentos IE_IdDocumento)    // Ejecución
        {
            try
            {
                string IdDocumento = IE_IdDocumento.IdDocumento;
                Data_Documentos data_Documento  =   new Data_Documentos(IdDocumento);   //  IdDocumento variable global
                data_Documento.Read_Documento();

                GenerarFactura generarFactura           =   new GenerarFactura();
                GenerarNotaCredito generarNotaCredito   =   new GenerarNotaCredito();
                GenerarNotaDebito generarNotaDebito     =   new GenerarNotaDebito();

                DocumentoElectronico documento;
                DocumentoResponse response;

                switch (data_Documento.TipoDocumento)
                {
                    case "01":  //  Factura y boletas
                        documento   =   generarFactura.Data(data_Documento);
                        response    =   generarFactura.Post(documento, true);
                        break;
                    case "07":  //  Nota de crédito
                        documento   =   generarNotaCredito.data(data_Documento);
                        response    =   generarNotaCredito.Post(documento, true);
                        break;
                    case "08":  //  Nota de Débito
                        documento   =   generarNotaDebito.data(data_Documento);
                        response    =   generarNotaDebito.Post(documento, true);
                        break;
                    default:
                        documento   =   generarFactura.Data(data_Documento);
                        response    =   generarFactura.Post(documento, true);
                        break;
                }

                if (!response.Exito)
                {
                    Data_Log data_Log   =   new Data_Log()
                    {
                        DetalleError    =   response.MensajeError,
                        Comentario      =   $"El XML con serie correlativo: {documento.SerieCorrelativo} no se pudo generar",
                        IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                    };
                    data_Log.Create_Log();
                }

                string rutaArchivo = Path.Combine(data_Documento.Ruta, $"{documento.SerieCorrelativo}.xml");
                
                Firmar firmar   =   new Firmar();

                FirmadoRequest firmadoRequest   =   firmar.Data(data_Usuario.IdAccesosSunat, response.TramaXmlSinFirma);
                //FirmadoResponse firmadoResponse =   await firmar.Post(firmadoRequest);      //  Ya se obtuvo el documento firmado
                FirmadoResponse firmadoResponse =   firmar.Post(firmadoRequest, true);      //  Ya se obtuvo el documento firmado

                if (firmadoResponse.Exito)  //  Comprobamos que se haya firmado de forma correcta
                {
                    Data_Documentos actualizacionXML = new Data_Documentos(IdDocumento) { XmlFirmado = firmadoResponse.TramaXmlFirmado };
                    if (!actualizacionXML.Update_Documento_OneColumn("[dbo].[Update_Documento_SignedXML]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al actualizar el xmlFirmado del documento",
                            Comentario      =   "Error con la base de datos",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }
                }
                else
                {
                    data_Log = new Data_Log() { DetalleError = response.MensajeError, Comentario = "Error al firmar el documento", IdUser_Empresa = data_Usuario.IdUser_Empresa };
                    data_Log.Create_Log();
                }

                EnviarSunat enviarSunat =   new EnviarSunat();

                EnviarDocumentoRequest enviarDocumentoRequest   =   enviarSunat.Data(firmadoResponse.TramaXmlFirmado, data_Documento, 
                                                                    GetURL(data_Documento.TipoDocumento));  // Obtenemos los datos para EnviarDocumentoRequest

                //EnviarDocumentoResponse enviarDocumentoResponse =   await enviarSunat.Post_Documento(enviarDocumentoRequest);
                EnviarDocumentoResponse enviarDocumentoResponse =   enviarSunat.Post_Documento(enviarDocumentoRequest,  true);

                if (enviarDocumentoResponse.Exito && !string.IsNullOrEmpty(enviarDocumentoResponse.TramaZipCdr))    // Comprobar envío a sunat
                {
                    Data_Documentos actualizacionCDR    =   new Data_Documentos() { IdDocumento = IdDocumento,   CdrSunat    = enviarDocumentoResponse.TramaZipCdr };
                    if (!actualizacionCDR.Update_Documento_OneColumn("[dbo].[Update_Documento_CDR]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al actualizar el CDR del documento",
                            Comentario      =   "Error con la base de datos: [dbo].[Update_Documento_CDR]",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }

                    Data_Documentos actualizacionComentario = new Data_Documentos() { IdDocumento = IdDocumento, ComentarioDocumento = enviarDocumentoResponse.MensajeRespuesta };
                    if (!actualizacionComentario.Update_Documento_OneColumn("[dbo].[Update_Documento_Comentario]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al guardar el comentario notificando la recepción del CDR del documento",
                            Comentario      =   "No se pudo guardar el CDR",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }

                    Data_Documentos actualizacionEstadoSunat =  new Data_Documentos() { IdDocumento = IdDocumento, EstadoSunat = "Aceptado" };
                    if (!actualizacionEstadoSunat.Update_Documento_OneColumn("[dbo].[Update_Documento_EstadoSunat]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al guardar el comentario notificando el estado del documento",
                            Comentario      =   "No se pudo guardar el estado del documento",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }
                }
                else
                {
                    data_Log = new Data_Log() { DetalleError = enviarDocumentoResponse.MensajeError, Comentario = "Error al enviar el documento", IdUser_Empresa = data_Usuario.IdUser_Empresa };
                    data_Log.Create_Log();

                    Data_Documentos actualizacionComentario = new Data_Documentos() { IdDocumento = IdDocumento, ComentarioDocumento = enviarDocumentoResponse.MensajeError };
                    if (!actualizacionComentario.Update_Documento_OneColumn("[dbo].[Update_Documento_Comentario]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al guardar el comentario notificando el error al no poder recibir  el CDR del documento",
                            Comentario      =   "No se recibió el CDR",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }

                    Data_Documentos actualizacionEstadoSunat =  new Data_Documentos() { IdDocumento = IdDocumento, EstadoSunat = "Rechazado" };
                    if (!actualizacionEstadoSunat.Update_Documento_OneColumn("[dbo].[Update_Documento_EstadoSunat]"))
                    {
                        data_Log = new Data_Log()
                        {
                            DetalleError    =   "Error al guardar el comentario notificando el estado del documento",
                            Comentario      =   "No se pudo guardar el estado del documento",
                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                        };
                        data_Log.Create_Log();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg =   string.Concat(ex.InnerException?.Message,   ex.Message);
                data_Log = new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al procesar el envío del documento",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
        }

        public string Post(List<Data_Documentos> data_Documentos)
        {
            string mensajeRespuesta =   string.Empty;
            try
            {
                GenerarComunicacionBaja generarComunicacionBaja =   new GenerarComunicacionBaja();
                ComunicacionBaja documento  =   generarComunicacionBaja.data(data_Documentos);
                DocumentoResponse response  =   generarComunicacionBaja.Post(documento);
                
                if (!response.Exito)
                {
                    Data_Log data_Log   =   new Data_Log()
                    {
                        DetalleError    =   response.MensajeError,
                        Comentario      =   $"El XML con serie correlativo: {documento.IdDocumento} no se pudo generar",
                        IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                    };
                    data_Log.Create_Log();
                }
                else
                {
                    string rutaArchivo = Path.Combine(data_Documentos[0].Ruta, $"{documento.IdDocumento}.xml");
                
                    Firmar firmar   =   new Firmar();

                    FirmadoRequest firmadoRequest   =   firmar.Data(data_Usuario.IdAccesosSunat, response.TramaXmlSinFirma);
                    FirmadoResponse firmadoResponse =   firmar.Post(firmadoRequest, true);      //  Ya se obtuvo el documento firmado

                    if (firmadoResponse.Exito)  //  Comprobamos que se haya firmado de forma correcta
                    {
                        EnviarSunat enviarSunat                         =   new EnviarSunat();

                        EnviarDocumentoRequest enviarDocumentoRequest   =   enviarSunat.Data(firmadoResponse.TramaXmlFirmado, data_Documentos[0], GetURL("03"));
                        string nombreZip    =   $"{documento.Emisor.NroDocumento}-{documento.IdDocumento}";
                        EnviarResumenResponse enviarResumenResponse     =   enviarSunat.Post_Figurativo(enviarDocumentoRequest, nombreZip);
                
                        if (enviarResumenResponse.Exito)    // Comprobar envío a sunat
                        {
                            ConsultaConstanciaRequest consultaConstanciaRequest =   new ConsultaConstanciaRequest() {
                                Ruc         =   enviarDocumentoRequest.Ruc,
                                UsuarioSol  =   enviarDocumentoRequest.UsuarioSol,
                                ClaveSol    =   enviarDocumentoRequest.ClaveSol,
                                EndPointUrl =   " https://e-factura.sunat.gob.pe/ol-it-wsconscpegem/billConsultService"
                            };
                            Consultar consultar =   new Consultar();
                            EnviarDocumentoResponse enviarDocumentoResponse =   consultar.Post_Constancia(consultaConstanciaRequest);
                            if (enviarDocumentoResponse.Exito)
                                mensajeRespuesta    =   $"La comunicación de baja se ha realizado con éxito, detalle: {enviarDocumentoResponse.MensajeRespuesta}";
                            else
                            {
                                mensajeRespuesta    =   $"La comunicación de baja se ha realizado con éxito, pero hemos tenido" +
                                    $"inconvenietes al obtener el CDR del documento: {documento.IdDocumento}, probablemente se este empleado el usuario MODDATOS," +
                                    $"para obtener el CDR debes descargarlo de forma manual en la opción 'Consulta de CDR'";
                                data_Log = new Data_Log()
                                {
                                    DetalleError    =   mensajeRespuesta,
                                    Comentario      =   $"Ha ocurrido un error al generar el CDR de la comunicación de baja: {documento.IdDocumento}, probablemente se este empleado MODDATOS",
                                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                };
                                data_Log.Create_Log();
                            }

                            Data_DocumentoFigurativo data_DocumentoFigurativo   =   new Data_DocumentoFigurativo()
                            {
                                XMLFirmado          =   firmadoResponse.TramaXmlFirmado,
                                Tipo                =   "Comunicación de baja",
                                ComentarioDocumento =   mensajeRespuesta,
                                Identificador       =   documento.IdDocumento,
                                NumeroTicket        =   enviarResumenResponse.NroTicket,
                                CdrSunat            =   enviarDocumentoResponse.TramaZipCdr ?? string.Empty
                            };
                            
                            if (!data_DocumentoFigurativo.Create_DocumentoFigurativo())
                            {
                                data_Log = new Data_Log()
                                {
                                    DetalleError    =   "Error inesperado en la base de datos",
                                    Comentario      =   "Ha ocurrido un error al guardar el registro de la comunicación de baja",
                                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                };
                                data_Log.Create_Log();
                            }
                            else
                            {
                                Data_DocumentoFigurativo dataUnion_DocumentoFigurativo;
                                Data_Documentos updateDocumento;
                                foreach (var data_Documento in data_Documentos)
                                {
                                    dataUnion_DocumentoFigurativo   =   new Data_DocumentoFigurativo() {
                                        IdDocumentoFigurativo   =   data_DocumentoFigurativo.SCOPE_IDENTITY_VALUE,
                                        IdDocumento             =   data_Documento.IdDocumento
                                    };
                                    if (!dataUnion_DocumentoFigurativo.Create_Figurativo_Documentos())
                                    {
                                        data_Log = new Data_Log()
                                        {
                                            DetalleError    =   $"Detalle de tablas: Documentos {data_Documento.IdDocumento},{data_Documento.SerieCorrelativo} con la " +
                                            $"tabla Figurativo_Documentos {data_DocumentoFigurativo.SCOPE_IDENTITY_VALUE}, {data_DocumentoFigurativo.Identificador} ",
                                            Comentario      =   "Ha ocurrido un error al guardar el registro de la comunicación de baja, en la tabla de unión",
                                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                        };
                                        data_Log.Create_Log();
                                    }

                                    updateDocumento         =   new Data_Documentos() {
                                        IdDocumento         =   data_Documento.IdDocumento,
                                        EnviadoSunat        =   true,
                                        EstadoSunat         =   "De baja",
                                        ComentarioDocumento =   $"El documento figura dentro de la comunicación de baja con código de identificación: {data_DocumentoFigurativo.Identificador}",
                                        ComunicacionBaja    =   true,
                                    };
                                    if (!updateDocumento.Update_Documento())
                                    {
                                        data_Log = new Data_Log()
                                        {
                                            DetalleError    =   $"No se ha podido actualizar el documento{data_Documento.IdDocumento}, {data_Documento.SerieCorrelativo} para indicar que se ha dado de baja",
                                            Comentario      =   "Error en la base de datos",
                                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                        };
                                        data_Log.Create_Log();
                                    }
                                }
                            }
                        }
                        else
                        {
                            mensajeRespuesta    = enviarResumenResponse.MensajeError;
                            data_Log = new Data_Log()
                            {
                                DetalleError    =   enviarResumenResponse.MensajeError,
                                Comentario      =   "Error al crear la comunicación de baja",
                                IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                            };
                            data_Log.Create_Log();
                        }
                    }
                    else
                    {
                        data_Log = new Data_Log() { DetalleError = response.MensajeError, Comentario = "Error al firmar el documento", IdUser_Empresa = data_Usuario.IdUser_Empresa };
                        data_Log.Create_Log();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg =   string.Concat(ex.InnerException?.Message,   ex.Message);
                data_Log = new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al procesar el envío del documento",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
            }
            return mensajeRespuesta;
        }

        public string PostResumen(List<Data_Documentos> data_Documentos)
        {
            string mensajeRespuesta =   string.Empty;
            try
            {
                GenerarResumenDiario generarResumenDiario   =   new GenerarResumenDiario();
                ResumenDiario resumenDiario =   generarResumenDiario.Data(data_Documentos);
                DocumentoResponse response  =   generarResumenDiario.Post(resumenDiario);
                
                if (!response.Exito)
                {
                    Data_Log data_Log   =   new Data_Log()
                    {
                        DetalleError    =   response.MensajeError,
                        Comentario      =   $"El XML con identificador: {resumenDiario.IdDocumento} no se pudo generar",
                        IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                    };
                    data_Log.Create_Log();
                }
                else
                {
                    string rutaArchivo  =   Path.Combine(data_Documentos[0].Ruta, $"{resumenDiario.IdDocumento}.xml");
                
                    Firmar firmar   =   new Firmar();
                    FirmadoRequest firmadoRequest   =   firmar.Data(data_Usuario.IdAccesosSunat, response.TramaXmlSinFirma);
                    FirmadoResponse firmadoResponse =   firmar.Post(firmadoRequest, true);      //  Ya se obtuvo el documento firmado
                    
                    if (firmadoResponse.Exito)  //  Comprobamos que se haya firmado de forma correcta
                    {
                        EnviarSunat enviarSunat                         =   new EnviarSunat();
                        EnviarDocumentoRequest enviarDocumentoRequest   =   enviarSunat.Data(firmadoResponse.TramaXmlFirmado, data_Documentos[0], GetURL("03"));
                        string nombreZip                                =   $"{resumenDiario.Emisor.NroDocumento}-{resumenDiario.IdDocumento}";
                        EnviarResumenResponse enviarResumenResponse     =   enviarSunat.Post_Figurativo(enviarDocumentoRequest, nombreZip);
                
                        if (enviarResumenResponse.Exito)    // Comprobar envío a sunat
                        {
                            ConsultaConstanciaRequest consultaConstanciaRequest =   new ConsultaConstanciaRequest() {
                                Ruc         =   enviarDocumentoRequest.Ruc,
                                UsuarioSol  =   enviarDocumentoRequest.UsuarioSol,
                                ClaveSol    =   enviarDocumentoRequest.ClaveSol,
                                EndPointUrl =   " https://e-factura.sunat.gob.pe/ol-it-wsconscpegem/billConsultService"
                            };
                            Consultar consultar =   new Consultar();
                            EnviarDocumentoResponse enviarDocumentoResponse =   consultar.Post_Constancia(consultaConstanciaRequest);
                            if (enviarDocumentoResponse.Exito)
                                mensajeRespuesta    =   $"El resumen diario ha realizado con éxito, detalle: {enviarDocumentoResponse.MensajeRespuesta}";
                            else
                            {
                                mensajeRespuesta    =   "El resumen diario ha realizado con éxito, pero hemos tenido inconvenietes al obtener" +
                                    " el CDR del documento: " + resumenDiario.IdDocumento + ", probablemente se este empleado el usuario MODDATOS," +
                                    "para obtener el CDR debes descargarlo de forma manual en la opción 'Consulta de CDR'";
                                data_Log = new Data_Log()
                                {
                                    DetalleError    =   mensajeRespuesta,
                                    Comentario      =   $"Ha ocurrido un error al generar el CDR del resumen diario: {resumenDiario.IdDocumento}, probablemente se este empleado MODDATOS",
                                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                };
                                data_Log.Create_Log();
                            }

                            Data_DocumentoFigurativo data_DocumentoFigurativo   =   new Data_DocumentoFigurativo()
                            {
                                XMLFirmado          =   firmadoResponse.TramaXmlFirmado,
                                Tipo                =   "Resumen diario",
                                ComentarioDocumento =   mensajeRespuesta,
                                Identificador       =   resumenDiario.IdDocumento,
                                NumeroTicket        =   enviarResumenResponse.NroTicket,
                                CdrSunat            =   enviarDocumentoResponse.TramaZipCdr ?? string.Empty
                            };
                            
                            if (!data_DocumentoFigurativo.Create_DocumentoFigurativo())
                            {
                                data_Log = new Data_Log()
                                {
                                    DetalleError    =   "Error inesperado en la base de datos",
                                    Comentario      =   "Ha ocurrido un error al guardar el registro del resumen diario",
                                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                };
                                data_Log.Create_Log();
                            }
                            else
                            {
                                Data_DocumentoFigurativo dataUnion_DocumentoFigurativo;
                                Data_Documentos updateDocumento;
                                foreach (var data_Documento in data_Documentos)
                                {
                                    dataUnion_DocumentoFigurativo   =   new Data_DocumentoFigurativo() {
                                        IdDocumentoFigurativo   =   data_DocumentoFigurativo.SCOPE_IDENTITY_VALUE,
                                        IdDocumento             =   data_Documento.IdDocumento
                                    };
                                    if (!dataUnion_DocumentoFigurativo.Create_Figurativo_Documentos())
                                    {
                                        data_Log = new Data_Log()
                                        {
                                            DetalleError    =   $"Detalle de tablas: Documentos {data_Documento.IdDocumento},{data_Documento.SerieCorrelativo} con la " +
                                            $"tabla Figurativo_Documentos {data_DocumentoFigurativo.SCOPE_IDENTITY_VALUE}, {data_DocumentoFigurativo.Identificador} ",
                                            Comentario      =   "Ha ocurrido un error al guardar el registro del resumen diario, en la tabla de unión",
                                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                        };
                                        data_Log.Create_Log();
                                    }

                                    updateDocumento         =   new Data_Documentos() {
                                        IdDocumento         =   data_Documento.IdDocumento,
                                        EnviadoSunat        =   true,
                                        EstadoSunat         =   "Aceptado",
                                        ComentarioDocumento =   $"El documento figura dentro del resumen diario con el código de identificación: {data_DocumentoFigurativo.Identificador}",
                                        ComunicacionBaja    =   false,
                                    };
                                    if (!updateDocumento.Update_Documento())
                                    {
                                        data_Log = new Data_Log()
                                        {
                                            DetalleError    =   $"No se ha podido actualizar el documento{data_Documento.IdDocumento}, {data_Documento.SerieCorrelativo} para indicar que resumen diario",
                                            Comentario      =   "Error en la base de datos",
                                            IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                                        };
                                        data_Log.Create_Log();
                                    }
                                }
                            }
                        }
                        else
                        {
                            mensajeRespuesta    = enviarResumenResponse.MensajeError;
                            data_Log = new Data_Log()
                            {
                                DetalleError    =   enviarResumenResponse.MensajeError,
                                Comentario      =   "Error al crear resumen diario",
                                IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                            };
                            data_Log.Create_Log();
                        }
                    }
                    else
                    {
                        data_Log = new Data_Log() { DetalleError = response.MensajeError, Comentario = "Error al firmar el documento", IdUser_Empresa = data_Usuario.IdUser_Empresa };
                        data_Log.Create_Log();
                    }
                }
            }
            catch (Exception ex)
            {
                var msg =   string.Concat(ex.InnerException?.Message,   ex.Message);
                data_Log = new Data_Log()
                {
                    DetalleError    =   $"Detalle del error: {msg}",
                    Comentario      =   "Error al procesar el envío del documento",
                    IdUser_Empresa  =   data_Usuario.IdUser_Empresa
                };
                data_Log.Create_Log();
                mensajeRespuesta    =    $"Detalle del error: {msg}";
            }
            return mensajeRespuesta;
        }

        public string GetURL(string tipoDocumento)
        {
            string url  =   string.Empty;
            Data_AccesosSunat data_AccesosSunat =   new Data_AccesosSunat(data_Usuario.IdAccesosSunat);
            data_AccesosSunat.Read_AccesosSunat();

            if (tipoDocumento.Equals("09")) // Aquí se debe identificar si es guía de remisión
            {
                if (data_AccesosSunat.UsuarioSol.Equals("MODDATOS"))
                    url =   "https://e-beta.sunat.gob.pe/ol-ti-itemision-guia-gem-beta/billService";
                else
                    url =   "https://e-guiaremision.sunat.gob.pe/ol-ti-itemision-guia-gem/billService";
            }
            else
            {
                if (data_AccesosSunat.UsuarioSol.Equals("MODDATOS"))
                    url =   "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";
                else
                    url =   "https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService";
            }
            //https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService
            return url;
        }
    }
}
