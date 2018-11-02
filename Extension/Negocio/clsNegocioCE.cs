using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioCE
    {
        protected clsEntityDeclarant declarante;
        //= new clsEntityDeclarant().cs_pxObtenerUnoPorId(new clsBaseConfiguracion().Cs_pr_Declarant_Id);
        protected clsEntityDatabaseLocal localDB;
        public virtual string cs_pxGenerarXMLAString(string id)
        {
            return string.Empty;
        }

        public clsNegocioCE(clsEntityDatabaseLocal local){
            localDB = local;
            declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
        }

        private void cs_prComprimirZIP(DirectoryInfo directorySelected)
        {
            if (File.Exists(directorySelected.ToString() + ".zip"))
            {
                File.Delete(directorySelected.ToString() + ".zip");
                ZipFile.CreateFromDirectory(directorySelected.ToString(), directorySelected.ToString() + ".zip");
            }
            else
            {
                ZipFile.CreateFromDirectory(directorySelected.ToString(), directorySelected.ToString() + ".zip");
            }
        }

        /// <summary>
        /// Genera un Comprobate de pago: factura, boletas y sus NC y ND asociadas.
        /// </summary>
        /// <param name="Id">Id del comprobante.</param>
        /// <returns>NombreTributo del archivo generado.</returns>
        public string cs_pxGenerarCE(string Id)
        {
            string fila = string.Empty;
            string archivo_nombre_XML = string.Empty, archivo_nombre_ZIP = string.Empty, archivo_nombre_directorio = string.Empty;
            try
            {
                clsEntityDocument cabecera = new clsEntityDocument(localDB);
                cabecera.cs_fxObtenerUnoPorId(Id);
                //fila = cs_pxGenerarXMLaString(Id,localDB);
                switch (cabecera.Cs_tag_InvoiceTypeCode)
                {
                    case "01":
                        fila = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(Id );
                        break;
                    case "03":
                        fila = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "07":
                        fila = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "08":
                        fila = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                }
                #region Genera los nombres de archivo
                archivo_nombre_XML          = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_InvoiceTypeCode + "-" + cabecera.Cs_tag_ID + ".xml";
                archivo_nombre_ZIP          = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_InvoiceTypeCode + "-" + cabecera.Cs_tag_ID + ".zip";
                archivo_nombre_directorio   = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_InvoiceTypeCode + "-" + cabecera.Cs_tag_ID;
                string comprobante_ruta     = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/";
                string comprobante_ruta_nombre  = comprobante_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;

                if (File.Exists(comprobante_ruta_nombre)) { // Sí existe el archivo comprobante_ruta_nombre
                    File.Delete(comprobante_ruta_nombre);   // se elimina
                }

                if (Directory.Exists(comprobante_ruta + archivo_nombre_directorio)) { // Sí existe el directorio
                    var dir = new DirectoryInfo(comprobante_ruta + archivo_nombre_directorio); // Instancia de la clase DirectoryInfo 
                    dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; // Asignación de su atributo
                    dir.Delete(true); // Permitir eliminar de forma recursiva
                }

                if (!Directory.Exists(comprobante_ruta + archivo_nombre_directorio)) { // Sí no existe el directorio
                    Directory.CreateDirectory(comprobante_ruta + archivo_nombre_directorio); // Lo creamos ;)
                }

                if (!File.Exists(comprobante_ruta_nombre)) { // Sí el archivo no existe
                    File.Create(comprobante_ruta_nombre).Close(); // También lo creamos ;) ;)
                }

                StreamWriter sw1 = new StreamWriter(
                    File.Open(comprobante_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); // Códifica el archivo generado
                sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                DirectoryInfo x = new DirectoryInfo(comprobante_ruta + archivo_nombre_directorio); 
                cs_prComprimirZIP(x);
                #endregion
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE cs_pxGenerarCE" + ex.ToString());
            }
            return archivo_nombre_ZIP;
        }

        /// <summary>
        /// Genera un Documento de baja.
        /// </summary>
        /// <param name="Id">Id del documento.</param>
        /// <returns>NombreTributo del archivo generado.</returns>
        public string cs_pxGenerarResumenRA(string Id)
        {
            string archivo_nombre_XML = string.Empty;
            string archivo_nombre_ZIP = string.Empty;
            string archivo_nombre_directorio = string.Empty;
            string fila = string.Empty;
            try
            {
                clsEntityVoidedDocuments cabecera = new clsEntityVoidedDocuments(localDB);
                cabecera.cs_fxObtenerUnoPorId(Id);
                fila = new clsNegocioCEComunicacionBaja(localDB).cs_pxGenerarXMLAString(Id);

                #region Genera los nombres de archivo

                archivo_nombre_XML = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_ID.Split('-')[0].Trim().ToString() + "-" + cabecera.Cs_tag_IssueDate.Replace("-", "") + "-" + cabecera.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".xml";
                archivo_nombre_ZIP = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_ID.Split('-')[0].Trim().ToString() + "-" + cabecera.Cs_tag_IssueDate.Replace("-", "") + "-" + cabecera.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".zip";
                archivo_nombre_directorio = declarante.Cs_pr_Ruc + "-" + cabecera.Cs_tag_ID.Split('-')[0].Trim().ToString() + "-" + cabecera.Cs_tag_IssueDate.Replace("-", "") + "-" + cabecera.Cs_tag_ID.Split('-')[2].Trim().ToString();
                string comprobante_ruta = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/";
                string comprobante_ruta_nombre = comprobante_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;
                if (File.Exists(comprobante_ruta_nombre)) { File.Delete(comprobante_ruta_nombre); }
                if (Directory.Exists(comprobante_ruta + archivo_nombre_directorio)) { var dir = new DirectoryInfo(comprobante_ruta + archivo_nombre_directorio); dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; dir.Delete(true); }
                if (!Directory.Exists(comprobante_ruta + archivo_nombre_directorio)) { Directory.CreateDirectory(comprobante_ruta + archivo_nombre_directorio); }
                if (!File.Exists(comprobante_ruta_nombre)) { File.Create(comprobante_ruta_nombre).Close(); }
                // StreamWriter sw1 = new StreamWriter(comprobante_ruta_nombre); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                StreamWriter sw1 = new StreamWriter(File.Open(comprobante_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                DirectoryInfo x = new DirectoryInfo(comprobante_ruta + archivo_nombre_directorio);
                cs_prComprimirZIP(x);
                #endregion
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE cs_pxGenerarResumenRA" + ex.ToString());
            }
            
            return archivo_nombre_ZIP;
        }

        /// <summary>
        /// Genera un Resumen diario de boletas y sus NC y ND asociadas.
        /// </summary>
        /// <param name="Id">Id del resumen diario.</param>
        /// <returns>NombreTributo del archivo generado.</returns>
        public string cs_pxGenerarResumenRC(string Id)
        {
            string archivo_nombre_XML = string.Empty, archivo_nombre_ZIP = string.Empty, archivo_nombre_directorio = string.Empty;

            try
            {
                clsEntitySummaryDocuments SummaryDocuments = new clsEntitySummaryDocuments(localDB).cs_fxObtenerUnoPorId(Id);
                if (SummaryDocuments.Cs_pr_EstadoSUNAT != "1")
                {
                    string fila = string.Empty; fila = new clsNegocioCEResumenDiario(localDB).cs_pxGenerarXMLAString(Id );
                    #region Genera el nombre de archivo
                    archivo_nombre_XML = declarante.Cs_pr_Ruc + "-" + "RC" + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + SummaryDocuments.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".xml";
                    archivo_nombre_ZIP = declarante.Cs_pr_Ruc + "-" + "RC" + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + SummaryDocuments.Cs_tag_ID.Split('-')[2].Trim().ToString() + ".zip";
                    archivo_nombre_directorio = declarante.Cs_pr_Ruc + "-" + "RC" + "-" + DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "") + "-" + SummaryDocuments.Cs_tag_ID.Split('-')[2].Trim().ToString();
                    string documento_ruta = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/", documento_ruta_nombre = documento_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;
                    if (File.Exists(documento_ruta_nombre)) { File.Delete(documento_ruta_nombre); }
                    if (Directory.Exists(documento_ruta + archivo_nombre_directorio)) { var dir = new DirectoryInfo(documento_ruta + archivo_nombre_directorio); dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; dir.Delete(true); }
                    if (!Directory.Exists(documento_ruta + archivo_nombre_directorio)) { Directory.CreateDirectory(documento_ruta + archivo_nombre_directorio); }
                    if (!File.Exists(documento_ruta_nombre)) { File.Create(documento_ruta_nombre).Close(); }
                    //StreamWriter sw1 = new StreamWriter(documento_ruta_nombre); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    StreamWriter sw1 = new StreamWriter(File.Open(documento_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    DirectoryInfo x = new DirectoryInfo(documento_ruta + archivo_nombre_directorio);
                    cs_prComprimirZIP(x);
                    #endregion
                }
                return archivo_nombre_ZIP;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE cs_pxGenerarResumenRC" + ex.ToString());
                return null;
            }
        }

        public string cs_pxGenerarPerception(string Id)
        {
            string archivo_nombre_XML = string.Empty, archivo_nombre_ZIP = string.Empty, archivo_nombre_directorio = string.Empty;

            try
            {
                clsEntityPerception PerceptionDocument = new clsEntityPerception(localDB).cs_fxObtenerUnoPorId(Id);
                if (PerceptionDocument.Cs_pr_EstadoSUNAT != "1")
                {
                    string fila = string.Empty; fila = new clsNegocioCEPercepcion(localDB).cs_pxGenerarXMLAString(Id);
                    #region Genera el nombre de archivo
                    archivo_nombre_XML = declarante.Cs_pr_Ruc + "-" + "40" +  "-" + PerceptionDocument.Cs_tag_Id.ToString() + ".xml";
                    archivo_nombre_ZIP = declarante.Cs_pr_Ruc + "-" + "40" +  "-" + PerceptionDocument.Cs_tag_Id.ToString() + ".zip";
                    archivo_nombre_directorio = declarante.Cs_pr_Ruc + "-" + "40" + "-" + PerceptionDocument.Cs_tag_Id.ToString();
                    string documento_ruta = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/", documento_ruta_nombre = documento_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;
                    if (File.Exists(documento_ruta_nombre)) { File.Delete(documento_ruta_nombre); }
                    if (Directory.Exists(documento_ruta + archivo_nombre_directorio)) { var dir = new DirectoryInfo(documento_ruta + archivo_nombre_directorio); dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; dir.Delete(true); }
                    if (!Directory.Exists(documento_ruta + archivo_nombre_directorio)) { Directory.CreateDirectory(documento_ruta + archivo_nombre_directorio); }
                    if (!File.Exists(documento_ruta_nombre)) { File.Create(documento_ruta_nombre).Close(); }
                    //StreamWriter sw1 = new StreamWriter(documento_ruta_nombre); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    StreamWriter sw1 = new StreamWriter(File.Open(documento_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    DirectoryInfo x = new DirectoryInfo(documento_ruta + archivo_nombre_directorio);
                    cs_prComprimirZIP(x);
                    #endregion
                }
                return archivo_nombre_ZIP;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE cs_pxGenerarPerception " + ex.ToString());
                return null;
            }
        }
        public string cs_pxGenerarRetention(string Id)
        {
            string archivo_nombre_XML = string.Empty, archivo_nombre_ZIP = string.Empty, archivo_nombre_directorio = string.Empty;

            try
            {
                clsEntityRetention RetentionDocument = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(Id);
                if (RetentionDocument.Cs_pr_EstadoSUNAT != "1")
                {
                    string fila = string.Empty; fila = new clsNegocioCERetention(localDB).cs_pxGenerarXMLAString(Id);
                    #region Genera el nombre de archivo
                    archivo_nombre_XML = declarante.Cs_pr_Ruc + "-" + "20" + "-" + RetentionDocument.Cs_tag_Id.ToString() + ".xml";
                    archivo_nombre_ZIP = declarante.Cs_pr_Ruc + "-" + "20" + "-" + RetentionDocument.Cs_tag_Id.ToString() + ".zip";
                    archivo_nombre_directorio = declarante.Cs_pr_Ruc + "-" + "20" + "-" + RetentionDocument.Cs_tag_Id.ToString();
                    string documento_ruta = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/", documento_ruta_nombre = documento_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;
                    if (File.Exists(documento_ruta_nombre)) { File.Delete(documento_ruta_nombre); }
                    if (Directory.Exists(documento_ruta + archivo_nombre_directorio)) { var dir = new DirectoryInfo(documento_ruta + archivo_nombre_directorio); dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; dir.Delete(true); }
                    if (!Directory.Exists(documento_ruta + archivo_nombre_directorio)) { Directory.CreateDirectory(documento_ruta + archivo_nombre_directorio); }
                    if (!File.Exists(documento_ruta_nombre)) { File.Create(documento_ruta_nombre).Close(); }
                    // StreamWriter sw1 = new StreamWriter(documento_ruta_nombre); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    StreamWriter sw1 = new StreamWriter(File.Open(documento_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    DirectoryInfo x = new DirectoryInfo(documento_ruta + archivo_nombre_directorio);
                    cs_prComprimirZIP(x);
                    #endregion
                }
                return archivo_nombre_ZIP;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE cs_pxGenerarRetention " + ex.ToString());
                return null;
            }
        }
        public string cs_pxGenerarGuiaRemision(string Id)
        {
            string archivo_nombre_XML = string.Empty, archivo_nombre_ZIP = string.Empty, archivo_nombre_directorio = string.Empty;

            try
            {
                clsEntityDespatch DespatchDocument = new clsEntityDespatch(localDB).cs_fxObtenerUnoPorId(Id);
                if (DespatchDocument.Cs_pr_EstadoSUNAT != "1")
                {
                    string fila = string.Empty; fila = new clsNegocioCEGuiaRemision(localDB).cs_pxGenerarXMLAString(Id);
                    #region Genera el nombre de archivo
                    archivo_nombre_XML = declarante.Cs_pr_Ruc + "-" + "09" + "-" + DespatchDocument.Cs_tag_ID.ToString() + ".xml";
                    archivo_nombre_ZIP = declarante.Cs_pr_Ruc + "-" + "09" + "-" + DespatchDocument.Cs_tag_ID.ToString() + ".zip";
                    archivo_nombre_directorio = declarante.Cs_pr_Ruc + "-" + "09" + "-" + DespatchDocument.Cs_tag_ID.ToString();
                    string documento_ruta = new clsBaseConfiguracion().cs_prRutadocumentosenvio + "/", documento_ruta_nombre = documento_ruta + archivo_nombre_directorio + "\\" + archivo_nombre_XML;
                    if (File.Exists(documento_ruta_nombre)) { File.Delete(documento_ruta_nombre); }
                    if (Directory.Exists(documento_ruta + archivo_nombre_directorio)) { var dir = new DirectoryInfo(documento_ruta + archivo_nombre_directorio); dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly; dir.Delete(true); }
                    if (!Directory.Exists(documento_ruta + archivo_nombre_directorio)) { Directory.CreateDirectory(documento_ruta + archivo_nombre_directorio); }
                    if (!File.Exists(documento_ruta_nombre)) { File.Create(documento_ruta_nombre).Close(); }
                   // StreamWriter sw1 = new StreamWriter(documento_ruta_nombre); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;
                    StreamWriter sw1 = new StreamWriter(File.Open(documento_ruta_nombre, FileMode.OpenOrCreate), Encoding.GetEncoding("ISO-8859-1")); sw1.WriteLine(fila); sw1.Close(); fila = string.Empty;

                    DirectoryInfo x = new DirectoryInfo(documento_ruta + archivo_nombre_directorio);
                    cs_prComprimirZIP(x);
                    #endregion
                }
                return archivo_nombre_ZIP;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" clsNegocioCE cs_pxGenerarGuiaRemision " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Firma un documento XML.
        /// </summary>
        /// <param name="documentoXML">Documento XML</param>
        /// <param name="certificado">Certificado</param>
        /// <returns>Documento Firmado</returns>
        protected string FirmarXml(XmlDocument documentoXML, X509Certificate2 certificado)
        {
            try
            {
                documentoXML.PreserveWhitespace = false;
                SignedXml signedXml = new SignedXml(documentoXML);
                signedXml.SigningKey = certificado.PrivateKey;

                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data keyData = new KeyInfoX509Data(certificado);
                keyData.AddSubjectName(certificado.SubjectName.Name);
                keyInfo.AddClause(keyData);

                signedXml.KeyInfo = keyInfo;
                signedXml.Signature.Id = "SignatureSP";
                Reference reference = new Reference("#SignatureSP");
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.Uri = "";

                signedXml.AddReference(reference);
                signedXml.ComputeSignature();
                signedXml.CheckSignature(certificado, true);

                string xml_string = documentoXML.InnerXml.ToString();
                xml_string = xml_string.Replace("<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>", "<ext:UBLExtension><ext:ExtensionContent>" + signedXml.GetXml().OuterXml+ "</ext:ExtensionContent></ext:UBLExtension>");
                return xml_string;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCE FirmarXml " + ex.ToString());
                return null;
            }

        }
    }
}