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
        //string IdDocumento  =   string.Empty;
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
                    case "01":  //  Factura
                        documento   =   generarFactura.data(data_Documento);
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
                        documento   =   generarFactura.data(data_Documento);
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
                EnviarDocumentoResponse enviarDocumentoResponse =   enviarSunat.Post_Documento(enviarDocumentoRequest, true);

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
