using DataLayer.CRUD;
using Models.Intercambio;
using Security;
using ServicesSunat;
using ServicesSunat.SOAP;
using System;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EnviarSunat
    {
        private readonly ISerializador _serializador;
        private readonly IServicioSunatDocumentos _servicioSunatDocumentos;
        public EnviarSunat()
        {
            Serializador serializador   =   new Serializador();
            _serializador               =   (ISerializador)serializador;

            ServicioSunatDocumentos servicioSunatDocumentos     =   new ServicioSunatDocumentos();
            _servicioSunatDocumentos    =   (IServicioSunatDocumentos)servicioSunatDocumentos;
        }

        public EnviarDocumentoRequest Data(string tramaXmlFirmado, Data_Documentos data_Documento, string endPointUrl)
        {
            Data_AccesosSunat data_AccesosSunat     =   new Data_AccesosSunat(data_Documento.IdEmisor);
            data_AccesosSunat.Read_AccesosSunat();

            Data_Contribuyente  data_Contribuyente  =   new Data_Contribuyente(data_Documento.IdEmisor);
            data_Contribuyente.Read_Contribuyente();

            var enviarDocumentoRequest = new EnviarDocumentoRequest()
            {
                Ruc             =   data_Contribuyente.NroDocumento,
                UsuarioSol      =   data_AccesosSunat.UsuarioSol,
                ClaveSol        =   data_AccesosSunat.ClaveSol,
                EndPointUrl     =   endPointUrl,
                IdDocumento     =   data_Documento.SerieCorrelativo,
                TipoDocumento   =   data_Documento.TipoDocumento,
                TramaXmlFirmado =   tramaXmlFirmado
            };

            return enviarDocumentoRequest;
        }

        public async Task<EnviarDocumentoResponse> Post_Documento(EnviarDocumentoRequest request)
        {
            var response        =   new EnviarDocumentoResponse();
            var nombreArchivo   =   $"{request.Ruc}-{request.TipoDocumento}-{request.IdDocumento}";
            var tramaZip        =   await _serializador.GenerarZip(request.TramaXmlFirmado, nombreArchivo);

            _servicioSunatDocumentos.Inicializar(new ParametrosConexion
            {
                Ruc             =   request.Ruc,
                UserName        =   request.UsuarioSol,
                Password        =   request.ClaveSol,
                EndPointUrl     =   request.EndPointUrl
            });

            var resultado = _servicioSunatDocumentos.EnviarDocumento(new DocumentoSunat
            {
                TramaXml        =   tramaZip,
                NombreArchivo   =   $"{nombreArchivo}.zip"
            });

            if (!resultado.Exito)
            {
                response.Exito          =   false;
                response.MensajeError   =   resultado.MensajeError;
            }
            else
            {
                response                =   await _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion);
                // Quitamos la R y la extensión devueltas por el Servicio.
                response.NombreArchivo  =   nombreArchivo;
            }

            return response;
        }

        public EnviarDocumentoResponse Post_Documento(EnviarDocumentoRequest request, bool sincrono)
        {
            var response        =   new EnviarDocumentoResponse();
            var nombreArchivo   =   $"{request.Ruc}-{request.TipoDocumento}-{request.IdDocumento}";
            var tramaZip        =   _serializador.GenerarZip(request.TramaXmlFirmado, nombreArchivo, sincrono);

            _servicioSunatDocumentos.Inicializar(new ParametrosConexion
            {
                Ruc             =   request.Ruc,
                UserName        =   request.UsuarioSol,
                Password        =   request.ClaveSol,
                EndPointUrl     =   request.EndPointUrl
            });

            var resultado = _servicioSunatDocumentos.EnviarDocumento(new DocumentoSunat
            {
                TramaXml        =   tramaZip,
                NombreArchivo   =   $"{nombreArchivo}.zip"
            });

            if (!resultado.Exito)
            {
                response.Exito          =   false;
                response.MensajeError   =   resultado.MensajeError;
            }
            else
            {
                response                =   _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion, sincrono);
                // Quitamos la R y la extensión devueltas por el Servicio.
                response.NombreArchivo  =   nombreArchivo;
            }

            return response;
        }

        public async Task<EnviarDocumentoResponse> Post_Resumen(EnviarDocumentoRequest request)
        {
            var response        =   new EnviarDocumentoResponse();
            

            return response;
        }
    }
}
