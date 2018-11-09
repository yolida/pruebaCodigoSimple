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

namespace BusinessLayer
{
    public class Consultar
    {
        private readonly IServicioSunatConsultas    _servicioSunatConsultas;
        private readonly IServicioSunatDocumentos   _servicioSunatDocumentos;
        private readonly ISerializador  _serializador;
        public Consultar()
        {
            ServicioSunatConsultas  servicioSunatConsultas      =   new ServicioSunatConsultas();
            _servicioSunatConsultas     =   (IServicioSunatConsultas)servicioSunatConsultas;

            ServicioSunatDocumentos  servicioSunatDocumentos    =   new ServicioSunatDocumentos();
            _servicioSunatDocumentos    =   (IServicioSunatDocumentos)servicioSunatDocumentos;

            Serializador serializador   =   new Serializador();
            _serializador               =   (ISerializador)serializador;
        }

        public EnviarDocumentoResponse Post_Ticket(ConsultaTicketRequest request)
        {
            var response = new EnviarDocumentoResponse();

            try
            {
                _servicioSunatDocumentos.Inicializar(new ParametrosConexion
                {
                    Ruc         =   request.Ruc,
                    UserName    =   request.UsuarioSol,
                    Password    =   request.ClaveSol,
                    EndPointUrl =   request.EndPointUrl
                });

                var resultado   =   _servicioSunatDocumentos.ConsultarTicket(request.NroTicket);

                if (!resultado.Exito)
                {
                    response.Exito          =   false;
                    response.MensajeError   =   resultado.MensajeError;
                }
                else
                    response    =   _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion, true);
            }
            catch (Exception ex)
            {
                response.MensajeError   = ex.Source == "DotNetZip"  ? "El Ticket no existe" : ex.Message;
                response.Pila           = ex.StackTrace;
                response.Exito          = false;
            }

            return response;
        }

        public EnviarDocumentoResponse Post_Constancia(ConsultaConstanciaRequest request)
        {
            var response    =   new EnviarDocumentoResponse();

            try
            {
                _servicioSunatConsultas.Inicializar(new ParametrosConexion
                {
                    Ruc         =   request.Ruc,
                    UserName    =   request.UsuarioSol,
                    Password    =   request.ClaveSol,
                    EndPointUrl =   request.EndPointUrl
                });

                var resultado   =   _servicioSunatConsultas.ConsultarConstanciaDeRecepcion(new DatosDocumento
                {
                    RucEmisor       =   request.Ruc,
                    TipoComprobante =   request.TipoDocumento,
                    Serie           =   request.Serie,
                    Numero          =   request.Numero
                });

                if (!resultado.Exito)
                {
                    response.Exito              =   false;
                    response.MensajeRespuesta   =   resultado.MensajeError;
                }
                else
                    response    =   _serializador.GenerarDocumentoRespuesta(resultado.ConstanciaDeRecepcion, true);
            }
            catch (Exception ex)
            {
                response.MensajeError   =   ex.Message;
                response.Pila           =   ex.StackTrace;
                response.Exito          =   false;
            }

            return response;
        }
    }
}