using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Common.Constantes;
using ServicesSunat.Documentos;

namespace ServicesSunat.SOAP
{
    public class ServicioSunatDocumentos : IServicioSunatDocumentos
    {
        private Documentos.billServiceClient _proxyDocumentos;

        Binding CreateBinding()
        {
            var binding     =   new BasicHttpBinding(BasicHttpSecurityMode.TransportWithMessageCredential);
            var elements    =   binding.CreateBindingElements();
            elements.Find<SecurityBindingElement>().IncludeTimestamp    =   false;
            return new CustomBinding(elements);
        }

        void IServicioSunat.Inicializar(ParametrosConexion parametros)
        {
            ServicePointManager.UseNagleAlgorithm                   =   true;
            ServicePointManager.Expect100Continue                   =   false;
            ServicePointManager.CheckCertificateRevocationList      =   true;
            ServicePointManager.ServerCertificateValidationCallback =   new RemoteCertificateValidationCallback(ValidateServerCertificate);

            _proxyDocumentos = new billServiceClient(CreateBinding(), new EndpointAddress(parametros.EndPointUrl))
            {
                ClientCredentials   =
                {
                    UserName    =
                    {
                        UserName    =   parametros.Ruc  +   parametros.UserName,
                        Password    =   parametros.Password
                    }
                }
            };
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        RespuestaSincrono IServicioSunatDocumentos.EnviarDocumento(DocumentoSunat request)
        {
            var dataOrigen  =   Convert.FromBase64String(request.TramaXml);
            var response    =   new RespuestaSincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado   =   _proxyDocumentos.sendBill(request.NombreArchivo,    dataOrigen, "");

                _proxyDocumentos.Close();

                response.ConstanciaDeRecepcion  =   Convert.ToBase64String(resultado);
                response.Exito                  =   true;
            }
            catch (FaultException ex)
            {
                response.MensajeError   =   string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg     =   string.Concat(ex.InnerException?.Message,   ex.Message);
                if (msg.Contains(Formatos.FaultCode))
                {
                    var posicion    =   msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                    var codigoError =   msg.Substring(posicion + Formatos.FaultCode.Length, 4);
                    msg             =   $"El Código de Error es {codigoError}";
                }
                response.MensajeError = msg;
            }

            return response;
        }

        RespuestaAsincrono IServicioSunatDocumentos.EnviarResumen(DocumentoSunat request)
        {
            var dataOrigen  =   Convert.FromBase64String(request.TramaXml);
            var response    =   new RespuestaAsincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado   =   _proxyDocumentos.sendSummary(request.NombreArchivo, dataOrigen, "0");

                _proxyDocumentos.Close();

                response.NumeroTicket   =   resultado;
                response.Exito          =   true;
            }
            catch (FaultException ex)
            {
                response.MensajeError   =   string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg =   ex.InnerException   !=  null    ?   string.Concat(ex.InnerException.Message,    ex.Message) :   ex.Message;
                if (msg.Contains(Formatos.FaultCode))
                {
                    var posicion        =   msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                    var codigoError     =   msg.Substring(posicion  +   Formatos.FaultCode.Length, 4);
                    msg                 =   $"El Código de Error es {codigoError}";
                }
                response.MensajeError   =   msg;
            }

            return response;
        }

        RespuestaSincrono IServicioSunatDocumentos.ConsultarTicket(string numeroTicket)
        {
            var response    =   new RespuestaSincrono();

            try
            {
                _proxyDocumentos.Open();
                var resultado   =   _proxyDocumentos.getStatus(numeroTicket);

                _proxyDocumentos.Close();

                var estado      =   (resultado.statusCode != "98");

                response.ConstanciaDeRecepcion  =   estado  ?   Convert.ToBase64String(resultado.content)   :   "Aun en proceso";
                response.Exito                  =   true;
            }
            catch (FaultException ex)
            {
                response.MensajeError   =   string.Concat(ex.Code.Name, ex.Message);
            }
            catch (Exception ex)
            {
                var msg =   ex.InnerException   !=  null    ?   string.Concat(ex.InnerException.Message,    ex.Message) :   ex.Message;
                if (msg.Contains(Formatos.FaultCode))
                {
                    var posicion        =   msg.IndexOf(Formatos.FaultCode, StringComparison.Ordinal);
                    var codigoError     =   msg.Substring(posicion  +   Formatos.FaultCode.Length,  4);
                    msg                 =   $"El Código de Error es {codigoError}";
                }
                response.MensajeError   =   msg;
            }

            return response;
        }
    }
}