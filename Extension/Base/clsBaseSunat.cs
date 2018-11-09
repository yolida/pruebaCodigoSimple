using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Extension.Datos;
using Extension.Negocio;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Linq;
using Extension.www.sunat.gob.pe;
using Extension.ServiceReference_Perception;
using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseSunat")]
    public class clsBaseSunat
    {

        //private string wsProduccion = "https://www.sunat.gob.pe/ol-ti-itcpfegem/billService";//producion old
        private static string wsProduccion = "https://e-factura.sunat.gob.pe/ol-ti-itcpfegem/billService";//producion new
        //private string wsProduccion = "https://www.sunat.gob.pe/ol-ti-itcpgem-sqa/billService";//homologacion
        private static string wsBeta = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";
        //private string wsBeta = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService?wsdl";

        //private string wsPerception = "https://www.sunat.gob.pe/ol-ti-itemision-otroscpe-gem/billService"; //Para percepcion y retencion produccion;
        private static string wsPerception_Beta = "https://e-beta.sunat.gob.pe/ol-ti-itemision-otroscpe-gem-beta/billService"; //Para percepcion y retencion beta;
        //private string wsPerception = "https://www.sunat.gob.pe/ol-ti-itemision-otroscpe-gem/billService"; //NUEVO 2018 - Para percepcion y retencion produccion;
        private static string wsPerception = "https://e-factura.sunat.gob.pe/ol-ti-itemision-otroscpe-gem/billService"; //NUEVO 2018 - Mayo - Para percepcion y retencion produccion;

        private static string wsGuiaRemision = "https://e-guiaremision.sunat.gob.pe/ol-ti-itemision-guia-gem/billService"; //Para Guia de Remisión;
        private static string wsGuiaRemision_Beta = "https://e-beta.sunat.gob.pe/ol-ti-itemision-guia-gem-beta/billService"; //Para guia beta;

        private string ws = string.Empty;
        private clsEntityDatabaseLocal local;
        private clsBaseConfiguracion configuracion;

        /*Se invoca al Modulo para obtener la Version del Compilado*/
        static ModVers VersionP = new ModVers();
        private string versionProducto = VersionP.Vers_Compilado();
        public clsBaseSunat(clsEntityDatabaseLocal localDatabase)
        {
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            local = localDatabase;
            configuracion = new clsBaseConfiguracion();
        }
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal localDB = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.local = localDB;
            configuracion = new clsBaseConfiguracion();
        }
        /// <summary>
        /// Genera un comprobante electrónico (BOLETA, FACTURA, NC, ND)
        /// </summary>
        /// <param name="Id"></param>
        internal void cs_pxDocumentoGenerarCE(string Id)
        {         
            try
            {
                clsEntityDocument cabecera = new clsEntityDocument(local).cs_fxObtenerUnoPorId(Id);
                switch (cabecera.Cs_tag_InvoiceTypeCode)
                {
                    case "01":
                        new clsNegocioCEFactura(local).cs_pxGenerarCE(Id);
                        break;
                    case "03":
                        new clsNegocioCEBoleta(local).cs_pxGenerarCE(Id);
                        break;
                    case "07":
                        new clsNegocioCENotaCredito(local).cs_pxGenerarCE(Id);
                        break;
                    case "08":
                        new clsNegocioCENotaDebito(local).cs_pxGenerarCE(Id);
                        break;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" cs_pxDocumentoGenerarCE " + ex.ToString());
            }
        }

        /// <summary>
        /// Genera el RESUMEN DIARIO DE BOLETAS.
        /// </summary>
        /// <param name="Id">Identificador del comprobante.</param>
        internal void cs_pxDocumentoGenerarRC(string Id)
        {
            try
            {
                new clsNegocioCEResumenDiario(local).cs_pxGenerarResumenRC(Id);
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" cs_pxDocumentoGenerarRC " + ex.ToString());
            }
        }

        /// <summary>
        /// Genera el documento de COMUNICACIÓN DE BAJA.
        /// </summary>
        /// <param name="Id">Identificador del comprobante.</param>
        internal void cs_pxDocumentoGenerarRA(string Id)
        {
            try
            {
                new clsNegocioCEComunicacionBaja(local).cs_pxGenerarResumenRA(Id);
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" cs_pxDocumentoGenerarRA " + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para enviar comprobante de retencion a Sunat.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True caso exitoso envio False Fallo en envio</returns>
        public bool cs_pxEnviarCERetention(string Id,bool mensaje)
        {
            bool estado = false;
            try
            {
                clsEntityRetention ce = new clsEntityRetention(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(ce.Cs_tag_IssueDate, versionProducto);

                if (licencia)
                {
                    new clsNegocioCE(local).cs_pxGenerarRetention(Id);

                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-20-" + ce.Cs_tag_Id.ToString() + ".zip";
                        XmlDocument documentoXML = new XmlDocument();

                        ServiceReference_Perception.billServiceClient bsc = new ServiceReference_Perception.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "2")));
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        byte[] allbytes = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                        string x = "" + configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico;

                        string documentoenvio = string.Empty;
                        StreamReader sr = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                        documentoenvio = sr.ReadToEnd();
                        sr.Close();
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        bsc.Open();
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        byte[] comprobante_electronico_bytes = bsc.sendBill(comprobante_electronico, allbytes, "");
                        bsc.Close();

                        FileStream fs = new FileStream(CDR, FileMode.Create);
                        fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
                        fs.Close();

                        //Verificar el contenido del archivo
                        ZipFile.ExtractToDirectory(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
                        documentoXML.Load(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-20-" + ce.Cs_tag_Id + ".xml");
                        ce.Cs_pr_EstadoSCC = "0";
                        ce.Cs_pr_FechaEnvio = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        if (documentoXML.OuterXml.Contains("ha sido aceptad"))
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "0";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                            ce.Cs_pr_FechaRecepcion = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        }
                        else
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "1";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                            ce.Cs_pr_FechaRecepcion = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        }

                        string cadena_xml = @documentoXML.OuterXml;
                        cadena_xml = cadena_xml.Replace("ext:", "");
                        cadena_xml = cadena_xml.Replace("cbc:", "");
                        cadena_xml = cadena_xml.Replace("cac:", "");
                        cadena_xml = cadena_xml.Replace("ds:", "");
                        cadena_xml = cadena_xml.Replace("ar:", "");
                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
                        if (cadena_xml.Trim().Length > 0)
                        {
                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns:ar=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">", "<ApplicationResponse>");
                            XmlDocument d = new XmlDocument();
                            d.LoadXml(cadena_xml);
                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                        }
                        ce.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        estado = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + ". \nPara mayor información revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCERetention " + ex.ToString());
                    }

                }
                else
                {
                    estado = false;
                    //No tiene licencia
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }

                }
            }
            catch (Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor información revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCERetention " + ex.ToString());
            }           
            return estado;
        }
        /// <summary>
        /// Metodo para enviar comprobante de Percepcion
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True envio correcto y false Presencia de error al enviar</returns>
        public bool cs_pxEnviarCEPerception(string Id,bool mensaje)
        {
            bool estado = false;
            try
            {
                clsEntityPerception ce = new clsEntityPerception(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(ce.Cs_tag_IssueDate, versionProducto);

                if (licencia)
                {
                    new clsNegocioCE(local).cs_pxGenerarPerception(Id);



                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-40-" + ce.Cs_tag_Id.ToString() + ".zip";
                        XmlDocument documentoXML = new XmlDocument();

                        ServiceReference_Perception.billServiceClient bsc = new ServiceReference_Perception.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "2")));
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        byte[] allbytes = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                        string x = "" + configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico;

                        string documentoenvio = string.Empty;
                        StreamReader sr = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                        documentoenvio = sr.ReadToEnd();
                        sr.Close();
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        bsc.Open();
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        byte[] comprobante_electronico_bytes = bsc.sendBill(comprobante_electronico, allbytes, "");
                        bsc.Close();

                        FileStream fs = new FileStream(CDR, FileMode.Create);
                        fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
                        fs.Close();

                        //Verificar el contenido del archivo
                        ZipFile.ExtractToDirectory(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
                        documentoXML.Load(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-40-" + ce.Cs_tag_Id + ".xml");
                        ce.Cs_pr_EstadoSCC = "0";

                        if (documentoXML.OuterXml.Contains("ha sido aceptad"))
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "0";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                        }
                        else
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "1";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                        }

                        string cadena_xml = @documentoXML.OuterXml;
                        cadena_xml = cadena_xml.Replace("ext:", "");
                        cadena_xml = cadena_xml.Replace("cbc:", "");
                        cadena_xml = cadena_xml.Replace("cac:", "");
                        cadena_xml = cadena_xml.Replace("ds:", "");
                        cadena_xml = cadena_xml.Replace("ar:", "");
                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
                        if (cadena_xml.Trim().Length > 0)
                        {
                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns:ar=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">", "<ApplicationResponse>");
                            XmlDocument d = new XmlDocument();
                            d.LoadXml(cadena_xml);
                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                        }
                        ce.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        estado = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError+ "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + "\nPara mayor información revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEPerception " + ex.ToString());
                    }
                }
                else
                {
                    estado = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                }
            }
            catch (Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor información revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEPerception " + ex.ToString());
            }
           
            return estado;
        }
        /// <summary>
        /// Metodo para enviar Guia de remision a Sunat
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>True Envio Correcto y False algun error</returns>
        public bool cs_pxEnviarCEGuiaRemision(string Id, bool mensaje)
        {
            bool estado = false;
            try
            {
                clsEntityDespatch ce = new clsEntityDespatch(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(ce.Cs_tag_IssueDate, versionProducto);

                if (licencia)
                {
                    new clsNegocioCE(local).cs_pxGenerarGuiaRemision(Id);


                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-09-" + ce.Cs_tag_ID.ToString() + ".zip";
                        XmlDocument documentoXML = new XmlDocument();

                        ServiceReference_Guia.billServiceClient bsc = new ServiceReference_Guia.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "3")));
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        byte[] allbytes = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                        string x = "" + configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico;

                        string documentoenvio = string.Empty;
                        StreamReader sr = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                        documentoenvio = sr.ReadToEnd();
                        sr.Close();
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        bsc.Open();
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        byte[] comprobante_electronico_bytes = bsc.sendBill(comprobante_electronico, allbytes, "");
                        bsc.Close();

                        FileStream fs = new FileStream(CDR, FileMode.Create);
                        fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
                        fs.Close();

                        //Verificar el contenido del archivo
                        ZipFile.ExtractToDirectory(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
                        documentoXML.Load(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-09-" + ce.Cs_tag_ID + ".xml");
                        ce.Cs_pr_EstadoSCC = "0";

                        if (documentoXML.OuterXml.Contains("ha sido aceptad"))
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "0";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                        }
                        else
                        {
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "1";
                            ce.Cs_pr_CDR = documentoXML.OuterXml;
                        }

                        string cadena_xml = @documentoXML.OuterXml;
                        cadena_xml = cadena_xml.Replace("ext:", "");
                        cadena_xml = cadena_xml.Replace("cbc:", "");
                        cadena_xml = cadena_xml.Replace("cac:", "");
                        cadena_xml = cadena_xml.Replace("ds:", "");
                        cadena_xml = cadena_xml.Replace("ar:", "");
                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
                        if (cadena_xml.Trim().Length > 0)
                        {
                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns:ar=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">", "<ApplicationResponse>");
                            XmlDocument d = new XmlDocument();
                            d.LoadXml(cadena_xml);
                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                        }
                        ce.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        estado = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + ". \nPara mayor información revise el archivo de errores. ");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEGuiaRemision " + ex.ToString());
                    }

                }
                else
                {
                    estado = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor..");
                    }
                }
            }
            catch(Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor información revise el archivo de errores. ");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEGuiaRemision " + ex.ToString());
            }
            
            return estado;
        }
        /// <summary>
        /// Envía COMPROBANTE ELECTRÓNICO FACTURA Y OTROS.
        /// </summary>
        /// <param name="Id">Identificador de COMPROBANTE(Tabla: cs_Documents, Campo: cs_Documents_Id)</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxEnviarCE(string Id, bool mensaje) // Análisis de código
        {
            bool estado = false;
            try
            {
                clsEntityDocument ce = new clsEntityDocument(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(ce.Cs_tag_IssueDate, versionProducto);
                if (licencia)
                {
                    cs_pxDocumentoGenerarCE(Id);                
                    //Si es nota de crédito o débito; verificar la existencia del documento principal. (IGNORAR A PERDIDO DE TANIA)
                    //Si existe la referencia, continuar, de lo contrario, mostrar mensaje que no existe el documento principal. (IGNORAR A PERDIDO DE TANIA)
                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante   = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding  = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
                        binding.IncludeTimestamp        = false;
                        string comprobante_electronico  = declarante.Cs_pr_Ruc + "-" + ce.Cs_tag_InvoiceTypeCode + "-" + ce.Cs_tag_ID + ".zip";
                        XmlDocument documentoXML        = new XmlDocument();
                        /** Tipos de comprobantes
                            01 - FACTURA
                            03 - BOLETA
                            07 - NOTA DE CRÉDITO
                            08 - NOTA DE DÉBITO */

                        //Comentar para Homologacion
                        //if (ce.Cs_tag_InvoiceTypeCode == "03" || ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                        if (ce.Cs_tag_InvoiceTypeCode == "03" && ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                        {
                            string documentoenvio = string.Empty;
                            StreamReader srx = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                            documentoenvio = srx.ReadToEnd();
                            srx.Close();
                            ce.Cs_pr_EstadoSCC = "0";
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "2";
                            //Comentar para Homologacion
                            string id_resumen = new clsNegocioCEResumenDiario(local).cs_pxAgregarAResumenDiario(ce);
                            ce.Cs_pr_Resumendiario = id_resumen;
                            ////Además agregar las NC y ND de Resumenes diarios.
                        }
                        else
                        {
                            //Fin Homologacion Comentado

                            /*Quitar comentario si se pide enviar las bolesa al resumen diario*/
                            //if (ce.Cs_tag_InvoiceTypeCode == "03" || ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                            //{
                            //string id_resumen = new clsNegocioCEResumenDiario(local).cs_pxAgregarAResumenDiario(ce);
                            //ce.Cs_pr_Resumendiario = id_resumen;
                            //}
                            /*Fin quitar comentario*/

                            www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(
                                new CustomBinding(binding, 
                                    new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), 
                                    new HttpsTransportBindingElement()),
                                    new EndpointAddress(cs_fxObtenerWS(declarante, "1"))); // Aquí se le pasa como parámetro la URL del Web Services 

                            string FechaHora    = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                            byte[] allbytes     = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                            string x            = "" + configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico;

                            string documentoenvio = string.Empty;
                            StreamReader sr = new StreamReader(
                                configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + 
                                comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                            documentoenvio = sr.ReadToEnd();
                            sr.Close();
                            bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                            bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                            bsc.Open();
                            string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                            byte[] comprobante_electronico_bytes = bsc.sendBill(comprobante_electronico, allbytes, "");
                            bsc.Close();

                            FileStream fs = new FileStream(CDR, FileMode.Create);
                            fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
                            fs.Close();
                            
                            //Verificar el contenido del archivo
                            ZipFile.ExtractToDirectory(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
                            documentoXML.Load(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-" + ce.Cs_tag_InvoiceTypeCode + "-" + ce.Cs_tag_ID + ".xml");
                            ce.Cs_pr_EstadoSCC = "0";

                            if (documentoXML.OuterXml.Contains("ha sido aceptad"))
                            {
                                ce.Cs_pr_XML = documentoenvio;
                                ce.Cs_pr_EstadoSUNAT = "0";
                                ce.Cs_pr_CDR = documentoXML.OuterXml;
                            }
                            else
                            {
                                ce.Cs_pr_XML = documentoenvio;
                                ce.Cs_pr_EstadoSUNAT = "1";
                                ce.Cs_pr_CDR = documentoXML.OuterXml;
                            }
                        }

                        string cadena_xml = @documentoXML.OuterXml;
                        cadena_xml = cadena_xml.Replace("ext:", "");
                        cadena_xml = cadena_xml.Replace("cbc:", "");
                        cadena_xml = cadena_xml.Replace("cac:", "");
                        cadena_xml = cadena_xml.Replace("ds:", "");
                        cadena_xml = cadena_xml.Replace("ar:", "");
                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
                        if (cadena_xml.Trim().Length > 0)
                        {
                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">", "<ApplicationResponse>");
                            XmlDocument d = new XmlDocument();
                            d.LoadXml(cadena_xml);
                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                        }

                        ce.comprobante_fechaenviodocumento = DateTime.Now.ToString();
                        ce.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        estado = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100)+"...";
                                }else
                                {
                                    if(mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + mensajeError  + "\nPara mayor información revise el archivo de errores.");
                            }
                        }                      
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCE " + ex.ToString());
                    }                 
                }
                else
                {
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                    estado = false;
                }
           
            }
            catch (Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor información revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCE " + ex.ToString());               

            }
            return estado;
        }
        /// <summary>
        /// Envía COMPROBANTE ELECTRÓNICO BOLETA  Y NOTAS DE CREDITO Y DEBITO ASOCIADAS A BOLETAS al resumen diario.
        /// </summary>
        /// <param name="Id">Identificador de COMPROBANTE(Tabla: cs_Documents, Campo: cs_Documents_Id)</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxEnviarCEToRC(string Id, bool mensaje)
        {
           
            bool estado = false;
            try
            {
                cs_pxDocumentoGenerarCE(Id);
                clsEntityDocument ce = new clsEntityDocument(local).cs_fxObtenerUnoPorId(Id);
                try
                {
                    //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                    clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                    string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + ce.Cs_tag_InvoiceTypeCode + "-" + ce.Cs_tag_ID + ".zip";
                    XmlDocument documentoXML = new XmlDocument();

                    if (ce.Cs_tag_InvoiceTypeCode == "03" || ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                    {
                        string documentoenvio = string.Empty;
                        StreamReader srx = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                        documentoenvio = srx.ReadToEnd();
                        srx.Close();

                        string id_resumen = new clsNegocioCEResumenDiario(local).cs_pxAgregarAResumenDiario(ce);
                        if (id_resumen != "")
                        {
                            ce.Cs_pr_Resumendiario = id_resumen;
                            //ce.Cs_pr_EstadoSCC = "0";
                            ce.Cs_pr_XML = documentoenvio;
                            //ce.Cs_pr_EstadoSUNAT = "2";
                            ce.comprobante_fechaenviodocumento = DateTime.Now.ToString();
                            ce.cs_pxActualizar(false, false);
                            estado = true;
                        }

                    }
                    else
                    {
                        if (mensaje == true)
                        {
                            clsBaseMensaje.cs_pxMsg("Excepcion Agregar a resumen ", "El documento no puede ser agregado a resumen diario verifique que sea una boleta, NC O ND asociada a boletas.");
                        }
                        estado = false;
                    }
                }
                catch (Exception ex)
                {
                    estado = false;
                    // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos a resumen", " " + ex.Message.Replace("  ", "").Replace("\n", ""));
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Error - Envío de documentos a resumen", "Se ha producido un error al agregar los comprobantes a resumen diario.");
                    }
                    clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEToRC " + ex.ToString());
                }
            }
            catch(Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos a resumen", "Se ha producido un error al agregar los comprobantes a resumen diario.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCEToRC " + ex.ToString());
            }
          
            return estado;
        }
        /// <summary>
        /// Envía RESUMEN DIARIO DE BOLETAS.
        /// </summary>
        /// <param name="Id">Identificador de COMPROBANTE(Tabla: cs_SummaryDocuments, Campo: cs_SummaryDocuments_Id)</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxEnviarRC(string Id,bool mensaje)
        {
            bool retorno = false;
            try
            {
                clsEntitySummaryDocuments SummaryDocuments = new clsEntitySummaryDocuments(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(SummaryDocuments.Cs_tag_IssueDate, versionProducto);

                if (licencia)
                {
                    cs_pxDocumentoGenerarRC(Id);
                    try
                    {
                        if (SummaryDocuments.Cs_pr_Ticket.Trim() == "")
                        {
                            //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                            clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                            SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;

                            www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(
                                new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), 
                                new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "1")));

                            string comprobante_electronico = declarante.Cs_pr_Ruc + "-RC-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + SummaryDocuments.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".zip";
                            bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                            bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                            bsc.Open();
                            byte[] documento = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                            SummaryDocuments.Cs_pr_Ticket = bsc.sendSummary(comprobante_electronico, documento, "");
                            bsc.Close();

                            //SummaryDocuments.Cs_pr_Ticket = "";
                            string documentoenvio = string.Empty;
                            StreamReader sr = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                            documentoenvio = sr.ReadToEnd();
                            if (SummaryDocuments.Cs_pr_Ticket.Trim() != "")
                            {
                                SummaryDocuments.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                            }
                            SummaryDocuments.Cs_pr_XML = documentoenvio;
                            SummaryDocuments.Cs_pr_CDR = "";
                            SummaryDocuments.Cs_pr_EstadoSCC = "0";
                            SummaryDocuments.Cs_pr_EstadoSUNAT = "5";
                            //Agregar estado de ticket
                            SummaryDocuments.cs_pxActualizar(false, false);
                            retorno = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        retorno = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de resumen diario", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                    //clsBaseMensaje.cs_pxMsg("SUNAT rechazo el Resumen diario debido a: ", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de resumen diario", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(excepcionSunat));
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes electrónicos a SUNAT; Por favor revise su conexión a internet.\n");
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de resumen diario", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + "\nPara mayor informacion revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRC " + ex.ToString());
                    }
                }
                else
                {
                    retorno = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                }
            }
            catch (Exception ex)
            {
                retorno = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de resumen diario", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor informacion revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRC " + ex.ToString());
            }
          
            return retorno;
        }


        /// <summary>
        /// Envía documento de COMUNICACIÓN DE BAJA.
        /// </summary>
        /// <param name="Id">Identificador de COMPROBANTE(Tabla: cs_VoidedDocuments, Campo: cs_VoidedDocuments_Id)</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxEnviarRA(string Id, bool mensaje)
        {
            bool estado = false;
            try
            {
                clsEntityVoidedDocuments entidad    =   new clsEntityVoidedDocuments(local).cs_fxObtenerUnoPorId(Id);
                bool licencia                       =   new clsBaseLicencia().licenceActive(entidad.Cs_tag_IssueDate, versionProducto);
                ///cambiar es otro para retencion otro metodo 
                /// solo este y el ticket 
                if (licencia)
                {
                    cs_pxDocumentoGenerarRA(Id);

                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;

                        www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(
                            new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), 
                            new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "1")));

                        string documento_nombre = declarante.Cs_pr_Ruc + "-" + entidad.Cs_tag_ID.Split('-')[0].Trim().ToString() + "-" + 
                            entidad.Cs_tag_IssueDate.Replace("-", "") + "-" + entidad.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".zip";

                        string xx = documento_nombre;
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        bsc.Open();
                        byte[] documento = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + documento_nombre);
                        entidad.Cs_pr_Ticket = bsc.sendSummary(documento_nombre, documento, "");
                        bsc.Close();
                        //entidad.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        entidad.Cs_pr_EstadoSCC = "0";
                        entidad.Cs_pr_XML = new clsNegocioCEComunicacionBaja(local).cs_pxGenerarXMLAString(Id);
                        if (entidad.Cs_pr_Ticket.Trim().Length > 0)
                        {
                            entidad.Cs_pr_EstadoSUNAT = "5";
                        }
                        else
                        {
                            entidad.Cs_pr_EstadoSUNAT = "1";
                        }
                        entidad.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de comunicacion de baja", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de comunicacion de baja", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de comunicacion de baja", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + "\nPara mayor informacion revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRA " + ex.ToString());
                        estado = false;
                    }

                }
                else
                {
                    estado = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                }
            }catch(Exception ex)
            {
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de comunicacion de baja", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor informacion revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRA " + ex.ToString());
                estado = false;
            }
          
            return estado;
        }

        /// <summary>
        /// Consulta el ticket RA, true: si ya tiene respuesta, false si aun no tiene respuesta
        /// </summary>
        /// <param name="ticket">ticket para verificar</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxConsultarTicket(string ticket,bool mensaje)
        {
            bool retornoValor = false;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(local).cs_fxObtenerUnoPorTicket(ticket);
                bool licencia = new clsBaseLicencia().licenceActive(documento.Cs_tag_IssueDate, versionProducto);
                if (licencia)
                {

                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);

                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "1")));
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        www.sunat.gob.pe.statusResponse sr = new www.sunat.gob.pe.statusResponse();
                        sr = bsc.getStatus(ticket);
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + documento.Cs_tag_ID + ".zip";
                        //Enviar y Recibir documento
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        string comentario_desde_sunat = "";
                        string estado_sunat = "";
                        clsBaseLog.cs_pxRegistarAdd("Consulta Ticket - RA => Codigo devuelto " + sr.statusCode.ToString());
                        switch (sr.statusCode)
                        {
                            case "0"://Proceso correcto
                                byte[] CDR_X = sr.content;
                                FileStream fs_x = new FileStream(CDR, FileMode.Create);
                                fs_x.Write(CDR_X, 0, CDR_X.Length);
                                fs_x.Close();
                                estado_sunat = "0";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                            case "98"://En proceso
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "99"://Proceso con errores
                                byte[] CDR_X2 = sr.content;
                                FileStream fs_x2 = new FileStream(CDR, FileMode.Create);
                                fs_x2.Write(CDR_X2, 0, CDR_X2.Length);
                                fs_x2.Close();
                                estado_sunat = "1";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                            case "0098":
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "0099"://Proceso con errores
                                byte[] CDR_X21 = sr.content;
                                FileStream fs_x21 = new FileStream(CDR, FileMode.Create);
                                fs_x21.Write(CDR_X21, 0, CDR_X21.Length);
                                fs_x21.Close();
                                estado_sunat = "1";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                        }
                        documento.Cs_pr_ComentarioSUNAT = comentario_desde_sunat;
                        documento.Cs_pr_EstadoSUNAT = estado_sunat;
                        if (estado_sunat != "4")
                        {
                            documento.Cs_pr_CDR = XML_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                        }
                        documento.cs_pxActualizar(false, false);
                        //actualizar los estados de documentos relacionados en la comunicacion de baja si ha sido aceptada:
                        if (estado_sunat == "0")
                        {
                            List<List<string>> registros = new clsEntityDocument(local).cs_pxObtenerDocumentosPorComunicacionBaja(documento.Cs_pr_VoidedDocuments_Id);
                            clsEntityDocument registro;
                            if (registros.Count() > 0)
                            {
                                foreach (var item in registros)
                                {
                                    var id_doc = item[0].ToString().Trim();//ID doc
                                    registro = new clsEntityDocument(local).cs_fxObtenerUnoPorId(id_doc);

                                    if (registro.Cs_tag_InvoiceTypeCode == "03" || registro.Cs_tag_BillingReference_DocumentTypeCode == "03")
                                    {
                                        //si es boleta o nota asociada buscar si esta en resumen
                                        if (registro.Cs_pr_Resumendiario.Trim() != "")
                                        {
                                           
                                            //esta en resumen liberar lso items del resumen
                                            List<List<string>> docs = new clsEntityDocument(local).cs_pxObtenerPorResumenDiario(registro.Cs_pr_Resumendiario);
                                            if (docs.Count() > 0)
                                            {
                                                foreach (var it in docs)
                                                {
                                                    var doc_id = it[0].ToString().Trim();//ID doc
                                                    clsEntityDocument reg = new clsEntityDocument(local).cs_fxObtenerUnoPorId(doc_id);
                                                    reg.Cs_ResumenUltimo_Enviado = reg.Cs_pr_Resumendiario;
                                                    reg.Cs_pr_Resumendiario = "";
                                                    reg.Cs_pr_EstadoSCC = "2";
                                                    reg.Cs_pr_EstadoSUNAT = "2";
                                                    reg.cs_pxActualizar(false, false);
                                                    reg = null;
                                                }
                                            }
                                            //buscar el resumen y si no esta enviado eliminar el resumen de lo contrario solo realizar la liberacion de los documentos
                                            clsEntitySummaryDocuments resumen = new clsEntitySummaryDocuments(local).cs_fxObtenerUnoPorId(registro.Cs_pr_Resumendiario);
                                            if (resumen.Cs_pr_EstadoSUNAT == "2")
                                            {
                                                new clsEntitySummaryDocuments(local).cs_pxEliminarDocumento(resumen.Cs_pr_SummaryDocuments_Id);
                                            }
                                        }
                                    }
                                    registro = new clsEntityDocument(local).cs_fxObtenerUnoPorId(id_doc);
                                    registro.Cs_pr_EstadoSCC = "0";
                                    registro.Cs_pr_EstadoSUNAT = "3";
                                    registro.cs_pxActualizar(false, false);                                  
                                }
                                retornoValor = true;
                            }
                        }
                        //esta rechazado entonces a pedido de tania liberar los documentos relacionados para poder generar uno nuevo
                        if (estado_sunat == "1")
                        {
                            List<List<string>> registros = new clsEntityDocument(local).cs_pxObtenerDocumentosPorComunicacionBaja(documento.Cs_pr_VoidedDocuments_Id);
                            clsEntityDocument registro;
                            if (registros.Count() > 0)
                            {
                                foreach (var item in registros)
                                {
                                    var id_doc = item[0].ToString().Trim();//ID doc
                                    registro = new clsEntityDocument(local).cs_fxObtenerUnoPorId(id_doc);
                                    registro.Cs_pr_ComunicacionBaja = "";
                                    registro.cs_pxActualizar(false, false);
                                }
                                retornoValor = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        retornoValor = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n "+ mensajeError + "\nRevise el archivo de errores para mayor información.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd("Consulta ticket RA: " + ex.ToString());
                    }

                }
                else
                {
                    retornoValor = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }

                }
            }
            catch (Exception ex)
            {
                retornoValor = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n Revise el archivo de errores para mayor información.");
                }
                clsBaseLog.cs_pxRegistarAdd("Consulta ticket RA: " + ex.ToString());
            }
            return retornoValor;
        }
        /// <summary>
        /// Enviar resumen de reversiones RR
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="mensaje"></param>
        /// <param name="tipoContiene"></param>
        /// <returns></returns>
        public bool cs_pxEnviarRR(string Id, bool mensaje)
        {
            bool estado = false;
            try
            {
                clsEntityVoidedDocuments entidad = new clsEntityVoidedDocuments(local).cs_fxObtenerUnoPorId(Id);
                bool licencia = new clsBaseLicencia().licenceActive(entidad.Cs_tag_IssueDate, versionProducto);
                ///cambiar es otro para retencion otro metodo 
                /// solo este y el ticket 
                if (licencia)
                {
                    cs_pxDocumentoGenerarRA(Id);

                    try
                    {

                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        ServiceReference_Perception.billServiceClient bsc = new ServiceReference_Perception.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "2")));
                        string documento_nombre = declarante.Cs_pr_Ruc + "-" + entidad.Cs_tag_ID.Split('-')[0].Trim().ToString() + "-" + entidad.Cs_tag_IssueDate.Replace("-", "") + "-" + entidad.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".zip";
                        string xx = documento_nombre;
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        bsc.Open();
                        byte[] documento = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + documento_nombre);
                        entidad.Cs_pr_Ticket = bsc.sendSummary(documento_nombre, documento, "");
                        bsc.Close();
                        //entidad.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                        entidad.Cs_pr_EstadoSCC = "0";
                        entidad.Cs_pr_XML = new clsNegocioCEComunicacionBaja(local).cs_pxGenerarXMLAString(Id);
                        if (entidad.Cs_pr_Ticket.Trim().Length > 0)
                        {
                            entidad.Cs_pr_EstadoSUNAT = "5";
                        }
                        else
                        {
                            entidad.Cs_pr_EstadoSUNAT = "1";
                        }
                        entidad.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de resumen de reversión", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de resumen de reversión", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de resumen de reversión", "Se ha producido un error al enviar los comprobantes a SUNAT. \n "+ mensajeError + " \nPara mayor informacion revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRA " + ex.ToString());
                        estado = false;
                    }

                }
                else
                {
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                    estado = false;
                }

            }
            catch (Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de resumen de reversión", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor informacion revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarRA " + ex.ToString());           
            }
          
            return estado;
        }

        /// <summary>
        /// Consulta el ticket, true: si ya tiene respuesta, false si aun no tiene respuesta
        /// </summary>
        /// <param name="ticket">ticket para verificar</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxConsultarTicketRR(string ticket, bool mensaje)
        {
            bool retornoValor = false;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(local).cs_fxObtenerUnoPorTicket(ticket);
                bool licencia = new clsBaseLicencia().licenceActive(documento.Cs_tag_IssueDate, versionProducto);
                if (licencia)
                {
                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);

                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        ServiceReference_Perception.billServiceClient bsc = new ServiceReference_Perception.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "2")));
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        ServiceReference_Perception.statusResponse sr = new ServiceReference_Perception.statusResponse();
                        sr = bsc.getStatus(ticket);
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + documento.Cs_tag_ID + ".zip";
                        //Enviar y Recibir documento
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        string comentario_desde_sunat = "";
                        string estado_sunat = "";
                        clsBaseLog.cs_pxRegistarAdd("Consulta Ticket - RR CRE => Codigo devuelto " + sr.statusCode.ToString());
                        switch (sr.statusCode)
                        {
                            case "0"://Proceso correcto
                                byte[] CDR_X = sr.content;
                                FileStream fs_x = new FileStream(CDR, FileMode.Create);
                                fs_x.Write(CDR_X, 0, CDR_X.Length);
                                fs_x.Close();
                                estado_sunat = "0";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                            case "98"://En proceso
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "99"://Proceso con errores
                                byte[] CDR_X2 = sr.content;
                                FileStream fs_x2 = new FileStream(CDR, FileMode.Create);
                                fs_x2.Write(CDR_X2, 0, CDR_X2.Length);
                                fs_x2.Close();
                                estado_sunat = "1";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                            case "0098":
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "0099"://Proceso con errores
                                byte[] CDR_X21 = sr.content;
                                FileStream fs_x21 = new FileStream(CDR, FileMode.Create);
                                fs_x21.Write(CDR_X21, 0, CDR_X21.Length);
                                fs_x21.Close();
                                estado_sunat = "1";
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                break;
                        }
                        documento.Cs_pr_ComentarioSUNAT = comentario_desde_sunat;
                        documento.Cs_pr_EstadoSUNAT = estado_sunat;
                        if (estado_sunat != "4")
                        {
                            documento.Cs_pr_CDR = XML_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                        }
                        documento.cs_pxActualizar(false, false);
                        //actualizar los estados de documentos relacionados en el resumen de reversion si ha sido aceptada:
                        if (estado_sunat == "0")
                        {
                            List<string> registros = new clsEntityRetention(local).cs_pxObtenerDocumentosPorResumenReversion(documento.Cs_pr_VoidedDocuments_Id);
                            clsEntityRetention registro;
                            if (registros.Count() > 0)
                            {
                                foreach (var item in registros)
                                {
                                    var id_doc = item.Trim();//ID doc
                                    registro = new clsEntityRetention(local).cs_fxObtenerUnoPorId(id_doc);
                                    registro.Cs_pr_EstadoSCC = "0";
                                    registro.Cs_pr_EstadoSUNAT = "3";
                                    registro.cs_pxActualizar(false, false);
                                    retornoValor = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        retornoValor = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n "+ mensajeError + "\nRevise el archivo de errores para mayor información.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd("Consulta ticket RA: " + ex.ToString());
                    }
                    return retornoValor;
                }
                else
                {
                    retornoValor = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                    return retornoValor;
                }
            }catch(Exception ex)
            {
                retornoValor = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n Revise el archivo de errores para mayor información.");
                }
                clsBaseLog.cs_pxRegistarAdd("Consulta ticket RA: " + ex.ToString());
                return retornoValor;
            }          
        }
        /// <summary>
        /// Consulta el ticket rc, true: si ya tiene respuesta, false si aun no tiene respuesta
        /// </summary>
        /// <param name="ticket">ticket para verificar</param>
        /// <returns>False: Si no se pudo enviar, True si se envía correctamente.</returns>
        public bool cs_pxConsultarTicketRC(string ticket,bool mensaje)
        {
            bool retornar = false;
            try
            {
                clsEntitySummaryDocuments documento = new clsEntitySummaryDocuments(local).cs_fxObtenerUnoPorTicket(ticket);
                bool licencia = new clsBaseLicencia().licenceActive(documento.Cs_tag_IssueDate, versionProducto);

                if (licencia)
                {
                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);
                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement(); binding.IncludeTimestamp = false;
                        www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(new CustomBinding(binding, new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8), new HttpsTransportBindingElement()), new EndpointAddress(cs_fxObtenerWS(declarante, "1")));
                        bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                        bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                        www.sunat.gob.pe.statusResponse sr = new www.sunat.gob.pe.statusResponse();
                        sr = bsc.getStatus(ticket);
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + documento.Cs_tag_ID + ".zip";
                        //Enviar y Recibir documento
                        string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                        string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                        string comentario_desde_sunat = "";
                        string estado_sunat = "5";
                        clsBaseLog.cs_pxRegistarAdd("Consulta Ticket - RC => Codigo devuelto " + sr.statusCode.ToString());
                        switch (sr.statusCode)
                        {
                            case "0"://Proceso correcto
                                byte[] CDR_X = sr.content;
                                FileStream fs_x = new FileStream(CDR, FileMode.Create);
                                fs_x.Write(CDR_X, 0, CDR_X.Length);
                                fs_x.Close();
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                estado_sunat = "0";
                                break;
                            case "98"://En proceso
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "99"://Proceso con errores
                                byte[] CDR_X2 = sr.content;
                                FileStream fs_x2 = new FileStream(CDR, FileMode.Create);
                                fs_x2.Write(CDR_X2, 0, CDR_X2.Length);
                                fs_x2.Close();
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                estado_sunat = "1";
                                break;
                            case "0098":
                                comentario_desde_sunat = "En proceso";
                                estado_sunat = "4";
                                break;
                            case "0099"://Proceso con errores
                                byte[] CDR_X21 = sr.content;
                                FileStream fs_x21 = new FileStream(CDR, FileMode.Create);
                                fs_x21.Write(CDR_X21, 0, CDR_X21.Length);
                                fs_x21.Close();
                                comentario_desde_sunat = Mensaje_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                                estado_sunat = "1";
                                break;
                        }
                        documento.Cs_pr_ComentarioSUNAT = comentario_desde_sunat;
                        documento.Cs_pr_EstadoSUNAT = estado_sunat;
                        if (estado_sunat != "4")
                        {
                            documento.Cs_pr_CDR = XML_desde_SUNAT(CDR, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID, documento.Cs_tag_ID);
                        }
                        documento.cs_pxActualizar(false, false);
                        //actualizar los estados de documentos relacionados en el resumen si ha sido aceptada el resumen:
                        if (estado_sunat == "0")
                        {
                            List<List<string>> registros = new clsEntityDocument(local).cs_pxObtenerPorResumenDiario(documento.Cs_pr_SummaryDocuments_Id);
                            clsEntityDocument registro;
                            if (registros.Count() > 0)
                            {
                                foreach (var item in registros)
                                {
                                    var id_doc = item[0].ToString().Trim();//ID doc
                                    registro = new clsEntityDocument(local).cs_fxObtenerUnoPorId(id_doc);
                                    //Cristhian|27/02/2018|FEI2-585
                                    /*Se pone el estado correspondiente en el campo de estados de la SUNAT*/
                                    /*NUEVO INICIO*/
                                    /*Si el estado del comprobante esta de baja en el EstadoSCC*/
                                    if (registro.Cs_pr_EstadoSCC.ToString() =="3")
                                    {
                                        /*Se pone el estado 3 en la columna EstadoSUNAT, ya que fue dado de baja*/
                                        registro.Cs_pr_EstadoSCC = "0";
                                        registro.Cs_pr_EstadoSUNAT = "3";
                                        DateTime Date = DateTime.Now;
                                        registro.Cs_tag_FechaDeBaja = Date.ToString("yyyy-MM-dd");
                                        registro.cs_pxActualizar(false, false);
                                    }
                                    /*En caso contrario*/
                                    else
                                    {
                                        /*Se pone el estado 0 en la columna EstadoSCC, extrañamente no se actualizaba*/
                                        registro.Cs_pr_EstadoSCC = "0";
                                        registro.Cs_pr_EstadoSUNAT = estado_sunat;
                                        registro.cs_pxActualizar(false, false);
                                    }
                                    /*NUEVO FIN*/
                                }
                            }
                        }
                        retornar = true;
                    }
                    catch (Exception ex)
                    {
                        retornar = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Consulta de Ticket", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                           
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n "+mensajeError+" \nRevise el archivo de errores para mayor información.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd("Consulta ticket RC:" + ex.ToString());
                    }
                }
                else
                {
                    retornar = false;
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                }
            }
            catch (Exception ex)
            {
                retornar = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Consulta de Ticket", "Se ha producido un error al consultar el ticket con SUNAT.\n Revise el archivo de errores para mayor información.");
                }
                clsBaseLog.cs_pxRegistarAdd("Consulta ticket RC:" + ex.ToString());
            }
           
            return retornar;
        }
        /// <summary>
        /// Obtiene el mensaje del xml de recepcion de Sunat
        /// </summary>
        /// <param name="ruta_documento"></param>
        /// <param name="RUC"></param>
        /// <param name="ID"></param>
        /// <returns>Cadena con el mensaje de Sunat</returns>

        private string Mensaje_desde_SUNAT(string ruta_documento, string RUC, string ID)
        {
            string valor = string.Empty;
            try
            {
                //Verificar el contenido del archivo
                ZipFile.ExtractToDirectory(ruta_documento, ruta_documento + "-dc");
                XmlDocument documentoXML = new XmlDocument();
                //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                documentoXML.Load(ruta_documento + "-dc" + "\\" + "R-" + RUC + "-" + ID + ".xml");

                string cadena_xml = @documentoXML.OuterXml;
                cadena_xml = cadena_xml.Replace("ext:", "");
                cadena_xml = cadena_xml.Replace("cbc:", "");
                cadena_xml = cadena_xml.Replace("cac:", "");
                cadena_xml = cadena_xml.Replace("ds:", "");
                cadena_xml = cadena_xml.Replace("ar:", "");
                cadena_xml = cadena_xml.Replace("\\\"", "\"");
                if (cadena_xml.Trim().Length > 0)
                {
                    cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">", "<ApplicationResponse>");
                    XmlDocument d = new XmlDocument();
                    d.LoadXml(cadena_xml);
                    valor = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" Mensaje_desde_SUNAT " + ex.ToString());
            }
            
            return valor;
        }
        /// <summary>
        /// Metodo para extraer el XML de respuesta de Sunat
        /// </summary>
        /// <param name="ruta_documento"></param>
        /// <param name="RUC"></param>
        /// <param name="ID"></param>
        /// <returns>XML de recepcion</returns>
        private string XML_desde_SUNAT(string ruta_documento, string RUC, string ID)
        {
            string valor = string.Empty;
            try
            {
                //Verificar el contenido del archivo
                //ZipFile.ExtractToDirectory(ruta_documento, ruta_documento + "-dc");
                XmlDocument documentoXML = new XmlDocument();
                //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                documentoXML.Load(ruta_documento + "-dc" + "\\" + "R-" + RUC + "-" + ID + ".xml");
                valor = documentoXML.OuterXml;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" XML_desde_SUNAT " + ex.ToString());
            }         
            return valor;
        }

        public bool isServidorBeta(clsEntityDeclarant declarante)
        {
            bool isbeta = false;
            if ((declarante.Cs_pr_Usuariosol == "moddatos" || declarante.Cs_pr_Usuariosol == "MODDATOS") && (declarante.Cs_pr_Clavesol == "moddatos" || declarante.Cs_pr_Clavesol == "MODDATOS"))
            {
                isbeta = true;
            }
            return isbeta;
        }
        /// <summary>
        /// Metodo para obtener el webservice a trabajar , segun sea el caso.
        /// </summary>
        /// <param name="declarante"></param>
        /// <param name="destino"></param>
        /// <returns>Ruta del webservice</returns>
        public static string cs_fxObtenerWS(clsEntityDeclarant declarante, string destino)
        {
            string ws = string.Empty;
            switch (destino)
            {
                case "1": //webservice de factura, boleta , nota de crédito y nota de débito
                    if ((declarante.Cs_pr_Usuariosol == "moddatos" || declarante.Cs_pr_Usuariosol == "MODDATOS") && (declarante.Cs_pr_Clavesol == "moddatos" || declarante.Cs_pr_Clavesol == "MODDATOS"))
                        ws = wsBeta;
                    else
                        ws = wsProduccion;
                    break;
                case "2": //webservice de percepción y retención
                    if ((declarante.Cs_pr_Usuariosol == "moddatos" || declarante.Cs_pr_Usuariosol == "MODDATOS") && (declarante.Cs_pr_Clavesol == "moddatos" || declarante.Cs_pr_Clavesol == "MODDATOS"))
                        ws = wsPerception_Beta;
                    else
                        ws = wsPerception;
                    break;
                case "3": //webservice de guía de remisión
                    if ((declarante.Cs_pr_Usuariosol == "moddatos" || declarante.Cs_pr_Usuariosol == "MODDATOS") && (declarante.Cs_pr_Clavesol == "moddatos" || declarante.Cs_pr_Clavesol == "MODDATOS"))
                        ws = wsGuiaRemision_Beta;
                    else
                        ws = wsGuiaRemision;
                    break;
                default:
                    break;
            }
            return ws;
        }
        internal XmlDocument stripDocumentNamespace(XmlDocument oldDom)
        {
            // Eliminar xmlns:*
            XmlDocument newDom = new XmlDocument();
            newDom.LoadXml(System.Text.RegularExpressions.Regex.Replace(
            oldDom.OuterXml, @"(xmlns:?[^=]*=[""][^""]*[""])", "",
            RegexOptions.IgnoreCase | RegexOptions.Multiline)
            );
            return newDom;
        }

        // Nuevas clases modificadas para hacer la actualización de UBL y demas temas
        public bool SendToSunatProcessing(string Id, bool mensaje) // Análisis de código
        {
            bool estado = false;
            try
            {
                clsEntityDocument ce = new clsEntityDocument(local).cs_fxObtenerUnoPorId(Id);

                // Generar el XML en caso de que no lo hubiera

                bool licencia = new clsBaseLicencia().licenceActive(ce.Cs_tag_IssueDate, versionProducto);
                if (licencia)
                {
                    cs_pxDocumentoGenerarCE(Id);
                    //Si es nota de crédito o débito; verificar la existencia del documento principal. (IGNORAR A PERDIDO DE TANIA)
                    //Si existe la referencia, continuar, de lo contrario, mostrar mensaje que no existe el documento principal. (IGNORAR A PERDIDO DE TANIA)
                    try
                    {
                        //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                        clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(local.Cs_pr_Declarant_Id);

                        SecurityBindingElement binding = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
                        binding.IncludeTimestamp = false;
                        string comprobante_electronico = declarante.Cs_pr_Ruc + "-" + ce.Cs_tag_InvoiceTypeCode + "-" + ce.Cs_tag_ID + ".zip";
                        XmlDocument documentoXML = new XmlDocument();
                        /** Tipos de comprobantes
                            01 - FACTURA
                            03 - BOLETA
                            07 - NOTA DE CRÉDITO
                            08 - NOTA DE DÉBITO */

                        //Comentar para Homologacion
                        //if (ce.Cs_tag_InvoiceTypeCode == "03" || ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                        if (ce.Cs_tag_InvoiceTypeCode == "03" && ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                        {
                            string documentoenvio = string.Empty;
                            StreamReader srx = new StreamReader(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" + comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                            documentoenvio = srx.ReadToEnd();
                            srx.Close();
                            ce.Cs_pr_EstadoSCC = "0";
                            ce.Cs_pr_XML = documentoenvio;
                            ce.Cs_pr_EstadoSUNAT = "2";
                            //Comentar para Homologacion
                            string id_resumen = new clsNegocioCEResumenDiario(local).cs_pxAgregarAResumenDiario(ce);
                            ce.Cs_pr_Resumendiario = id_resumen;
                            ////Además agregar las NC y ND de Resumenes diarios.
                        }
                        else
                        {
                            //Fin Homologacion Comentado

                            /*Quitar comentario si se pide enviar las bolesa al resumen diario*/
                            //if (ce.Cs_tag_InvoiceTypeCode == "03" || ce.Cs_tag_BillingReference_DocumentTypeCode == "03")
                            //{
                            //string id_resumen = new clsNegocioCEResumenDiario(local).cs_pxAgregarAResumenDiario(ce);
                            //ce.Cs_pr_Resumendiario = id_resumen;
                            //}
                            /*Fin quitar comentario*/

                            www.sunat.gob.pe.billServiceClient bsc = new www.sunat.gob.pe.billServiceClient(
                                new CustomBinding(binding,
                                    new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8),
                                    new HttpsTransportBindingElement()),
                                    new EndpointAddress(cs_fxObtenerWS(declarante, "1"))); // Aquí se le pasa como parámetro la URL del Web Services 

                            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
                            byte[] allbytes = File.ReadAllBytes(configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico);
                            string x = "" + configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico;

                            string documentoenvio = string.Empty;
                            StreamReader sr = new StreamReader(
                                configuracion.cs_prRutadocumentosenvio + "\\" + comprobante_electronico.Replace(".zip", "") + "\\" +
                                comprobante_electronico.Replace(".zip", "") + ".xml", Encoding.GetEncoding("ISO-8859-1"));
                            documentoenvio = sr.ReadToEnd();
                            sr.Close();
                            bsc.ClientCredentials.UserName.UserName = declarante.Cs_pr_Ruc + declarante.Cs_pr_Usuariosol;
                            bsc.ClientCredentials.UserName.Password = declarante.Cs_pr_Clavesol;
                            bsc.Open();
                            string CDR = configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico;
                            byte[] comprobante_electronico_bytes = bsc.sendBill(comprobante_electronico, allbytes, "");
                            bsc.Close();

                            FileStream fs = new FileStream(CDR, FileMode.Create);
                            fs.Write(comprobante_electronico_bytes, 0, comprobante_electronico_bytes.Length);
                            fs.Close();

                            //Verificar el contenido del archivo
                            ZipFile.ExtractToDirectory(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico, configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc");
                            documentoXML.Load(configuracion.cs_prRutadocumentosrecepcion + "\\" + FechaHora + " " + comprobante_electronico + "-dc\\" + "R-" + declarante.Cs_pr_Ruc + "-" + ce.Cs_tag_InvoiceTypeCode + "-" + ce.Cs_tag_ID + ".xml");
                            ce.Cs_pr_EstadoSCC = "0";

                            if (documentoXML.OuterXml.Contains("ha sido aceptad"))
                            {
                                ce.Cs_pr_XML = documentoenvio;
                                ce.Cs_pr_EstadoSUNAT = "0";
                                ce.Cs_pr_CDR = documentoXML.OuterXml;
                            }
                            else
                            {
                                ce.Cs_pr_XML = documentoenvio;
                                ce.Cs_pr_EstadoSUNAT = "1";
                                ce.Cs_pr_CDR = documentoXML.OuterXml;
                            }
                        }

                        string cadena_xml = @documentoXML.OuterXml;
                        cadena_xml = cadena_xml.Replace("ext:", "");
                        cadena_xml = cadena_xml.Replace("cbc:", "");
                        cadena_xml = cadena_xml.Replace("cac:", "");
                        cadena_xml = cadena_xml.Replace("ds:", "");
                        cadena_xml = cadena_xml.Replace("ar:", "");
                        cadena_xml = cadena_xml.Replace("\\\"", "\"");
                        if (cadena_xml.Trim().Length > 0)
                        {
                            cadena_xml = cadena_xml.Replace("<ApplicationResponse xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">", "<ApplicationResponse>");
                            XmlDocument d = new XmlDocument();
                            d.LoadXml(cadena_xml);
                            ce.Cs_pr_ComentarioSUNAT = d.SelectSingleNode("/ApplicationResponse/DocumentResponse/Response/Description").InnerText;
                        }

                        ce.comprobante_fechaenviodocumento = DateTime.Now.ToString();
                        ce.cs_pxActualizar(false, false);
                        estado = true;
                    }
                    catch (Exception ex)
                    {
                        estado = false;
                        if (ex.Message.Contains("ticket:"))
                        {
                            if (mensaje == true)
                            {
                                string excepcionSunat = ex.Message.Replace("\n", " ").Replace("  ", "");
                                string[] stringSeparators = new string[] { "error:" };
                                string[] mensajeMotivoSunat = excepcionSunat.Split(stringSeparators, StringSplitOptions.None);
                                if (mensajeMotivoSunat.Length > 1)
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mensajeMotivoSunat[1].Trim()));
                                }
                                else
                                {
                                    clsBaseMensaje.cs_pxMsg("Excepcion - Envío de documentos electrónicos", excepcionSunat);
                                }
                            }
                        }
                        else
                        {
                            // clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + ex.Message.Replace("  ", "").Replace("\n", ""));
                            if (mensaje == true)
                            {
                                string mensajeError = ex.Message.Replace("\n", " ").Replace("  ", "");
                                if (mensajeError.Length >= 100)
                                {
                                    mensajeError = mensajeError.Substring(0, 100) + "...";
                                }
                                else
                                {
                                    if (mensajeError.Length >= 75)
                                    {
                                        mensajeError = mensajeError.Substring(0, 75) + "...";
                                    }
                                    else
                                    {
                                        if (mensajeError.Length >= 50)
                                        {
                                            mensajeError = mensajeError.Substring(0, 50) + "...";
                                        }
                                        else
                                        {
                                            if (mensajeError.Length >= 25)
                                            {
                                                mensajeError = mensajeError.Substring(0, 25) + "...";
                                            }
                                            else
                                            {
                                                mensajeError = mensajeError + "...";
                                            }
                                        }
                                    }
                                }
                                clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT. \n" + mensajeError + "\nPara mayor información revise el archivo de errores.");
                            }
                        }
                        clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCE " + ex.ToString());
                    }
                }
                else
                {
                    if (mensaje == true)
                    {
                        clsBaseMensaje.cs_pxMsg("Licencia invalida", "No se ha podido enviar el comprobante. Verifique la vigencia de su licencia. Por favor contactese con su proveedor.");
                    }
                    estado = false;
                }

            }
            catch (Exception ex)
            {
                estado = false;
                if (mensaje == true)
                {
                    clsBaseMensaje.cs_pxMsg("Error - Envío de documentos electrónicos", "Se ha producido un error al enviar los comprobantes a SUNAT.\n Para mayor información revise el archivo de errores.");
                }
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarCE " + ex.ToString());

            }
            return estado;
        }
    }
}