using Extension.Base;
using Extension.Datos;
using Extension.Query;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioCEComunicacionBaja : clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCEComunicacionBaja(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }
        /// <summary>
        /// Metodo para generar la cadena de xml.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override string cs_pxGenerarXMLAString(string Id)
        {
            string archivo_xml = string.Empty;
            clsEntityVoidedDocuments cabecera = new clsEntityVoidedDocuments(localbd).cs_fxObtenerUnoPorId(Id);
            List<clsEntityVoidedDocuments_VoidedDocumentsLine> detalle = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraId(cabecera.Cs_pr_VoidedDocuments_Id);
            string fila = "";
            string ei = "    ";
            string ef = "\n";

            fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
            fila += "<VoidedDocuments xmlns=\"urn:sunat:names:specification:ubl:peru:schema:xsd:VoidedDocuments-1\" xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\" xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + ef;
            fila += ei + "<ext:UBLExtensions>" + ef;
            fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>" + ef;
            fila += ei + "</ext:UBLExtensions>" + ef;

            #region Información de cabecera
            fila += ei + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" + ef;
            fila += ei + "<cbc:CustomizationID>1.0</cbc:CustomizationID>" + ef;
            fila += ei + "<cbc:ID>" + cabecera.Cs_tag_ID + "</cbc:ID>" + ef;
            fila += ei + "<cbc:ReferenceDate>" + cabecera.Cs_tag_ReferenceDate + "</cbc:ReferenceDate>" + ef;
            fila += ei + "<cbc:IssueDate>" + cabecera.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
            #endregion

            #region Referencia de la firma digital
            fila += ei + "<cac:Signature>" + ef;
            fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
            fila += ei + ei + "<cac:SignatoryParty>" + ef;
            fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
            fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
            fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;
            //Jordy Amaro 14/11/16 FE-865
            //Agregado <!CDATA.
            fila += ei + ei + ei + "<cac:PartyName>" + ef;
            fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + "RAZONSOCIALDECERTIFICADO" + "]]></cbc:Name>" + ef;
            fila += ei + ei + ei + "</cac:PartyName>" + ef;
            fila += ei + ei + "</cac:SignatoryParty>" + ef;

            fila += ei + ei + "<cac:DigitalSignatureAttachment>" + ef;
            fila += ei + ei + ei + "<cac:ExternalReference>" + ef;
            fila += ei + ei + ei + ei + "<cbc:URI>#SignatureSP</cbc:URI>" + ef;
            fila += ei + ei + ei + "</cac:ExternalReference>" + ef;
            fila += ei + ei + "</cac:DigitalSignatureAttachment>" + ef;
            fila += ei + "</cac:Signature>" + ef;
            #endregion

            #region Datos del emisor del documento
            fila += ei + "<cac:AccountingSupplierParty>" + ef;
            fila += ei + ei + "<cbc:CustomerAssignedAccountID>" + cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
            fila += ei + ei + "<cbc:AdditionalAccountID>" + cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "</cbc:AdditionalAccountID>" + ef;
            if (cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName.Trim().Length > 0)
            {
                //Jordy Amaro 08/11/16 FE-851
                //Agregado <!CDATA.
                fila += ei + ei + "<cac:Party>" + ef;
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
            }
            fila += ei + "</cac:AccountingSupplierParty>" + ef;
            #endregion
            #region Lineas de detalle del documento
            Int32 linea_indice = 0;
            foreach (var linea in detalle)
            {
                linea_indice++;
                fila += ei + "<sac:VoidedDocumentsLine>" + ef;
                fila += ei + "<cbc:LineID>"+ linea_indice + "</cbc:LineID>" + ef;
                fila += ei + "<cbc:DocumentTypeCode>" + linea.Cs_tag_DocumentTypeCode + "</cbc:DocumentTypeCode>" + ef;
                fila += ei + "<sac:DocumentSerialID>" + linea.Cs_tag_DocumentSerialID + "</sac:DocumentSerialID>" + ef;
                fila += ei + "<sac:DocumentNumberID>" + linea.Cs_tag_DocumentNumberID + "</sac:DocumentNumberID>" + ef;
                fila += ei + "<sac:VoidReasonDescription>" + linea.Cs_tag_VoidReasonDescription + "</sac:VoidReasonDescription>" + ef;
                fila += ei + "</sac:VoidedDocumentsLine>" + ef;
            }
            fila += "</VoidedDocuments>" + ef;

            try
            {
                string pfxPath = declarante.Cs_pr_Rutacertificadodigital.Replace("\\\\", "\\");
                X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), declarante.Cs_pr_Parafrasiscertificadodigital);

                //Cristhian|25/08/2017|FEI2-352
                /*Se agrega un metodo de busqueda para ubicar la razon social de la empresa 
                  ya no dependiendo de la ubicacion donde se encuentre, ahora se busca su
                  etiqueta y se obtine el valor(la razon social)*/
                /*NUEVO INICIO*/
                string[] subject = cert.SubjectName.Name.Split(',');
                foreach (string item in subject)
                {
                    string[] subject_o = item.ToString().Split('=');
                    if (subject_o[0].Trim() == "O")
                    {
                        fila = fila.Replace("RAZONSOCIALDECERTIFICADO", subject_o[1].TrimStart());
                    }
                }
                /*NUEVO FIN*/

                XmlDocument documento = new XmlDocument();
                documento.PreserveWhitespace = false;
                documento.LoadXml(fila.Replace("\\\\", "\\"));
                archivo_xml = FirmarXml(documento, cert);
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCEComunicacionBaja generarXML" + ex.ToString());
            }
            return archivo_xml;
            #endregion
        }
        /// <summary>
        /// Obtener el listado de comunicaciones de baja entre fechas.
        /// </summary>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <returns></returns>
        public List<List<string>> cs_pxObtenerPorFiltroPrincipal(string fechainicio, string fechafin)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_VoidedDocuments WHERE 1=1";
                sql += " AND cp2 >='" + fechainicio + "' ";
                sql += " AND cp2 <='" + fechafin + "' ";
                sql += " AND cp3 >='" + fechainicio + "' ";
                sql += " AND cp3 <='" + fechafin + "' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localbd.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila;
                while (datos.Read())
                {
                    //Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                    fila = new List<string>();
                    for (int i = 0; i < datos.FieldCount; i++)
                    {
                        fila.Add(datos[i].ToString().Trim());
                    }
                    tabla_contenidos.Add(fila);
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsNegocioCEComunicacionBaja cs_pxObtenerPorFiltroPrincipal " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// METODO PARA PROCESAR DOCUMENTOS A DAR DE BAJA FEI2-222 29-05-2017
        /// </summary>
        /// <param name="DocumentosADarDeBaja"></param>
        /// <param name="tipoContenido"></param>
        /// <returns></returns>
        public string cs_pxProcesarComunicacionBaja(List<string> DocumentosADarDeBaja,string tipoContenido)
        {
            string mensaje = string.Empty;
            try
            {
                //Buscar grupos de fechas seleccionadas y por cada grupo debe crear un archivo de comunicación de baja.
                #region "AGRUPAR POR COMUNICACIÓN DE BAJA"
                List<string> grupo = new List<string>();//Crear grupos de fecha por IssueDate - FECHA DE EMISION
                List<string> docs_no_agregados = new List<string>();//Lista de doccumentos no agregados
                //Recorrer los documentos a dar de baja para agregar asu respectiva comunicacion de baja.
                foreach (var documento_id in DocumentosADarDeBaja)
                {
                    //Si los comprobantes son facturas notas y/o boletas.
                    if (tipoContenido == "0")
                    {
                        clsEntityDocument document = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(documento_id);
                        if (document.Cs_pr_ComunicacionBaja.Trim().Length <= 0)
                        {
                            //Verificar los 7 dias anteriores al dia de hoy para permitir agregar a resumen diario.
                            string fechaEmision = document.Cs_tag_IssueDate;
                            if (fechaEmision == DateTime.Now.ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"))
                            {
                                int count = 0;
                                foreach (var item_grupo in grupo)
                                {
                                    if (fechaEmision == item_grupo)
                                    {
                                        count++;
                                    }
                                }
                                if (count <= 0)//Si no existe el elemento en la lista, que se agregue.
                                {
                                    grupo.Add(fechaEmision);
                                }
                            }
                            else {
                                docs_no_agregados.Add(document.Cs_tag_ID);
                            }                           
                        }
                    }
                    else if (tipoContenido=="1")
                    {
                        //Cuando se agregan tipo de documentos retencion
                        clsEntityRetention document = new clsEntityRetention(localbd).cs_fxObtenerUnoPorId(documento_id);
                        if (document.Cs_pr_Reversion.Trim().Length <= 0)
                        {
                            //Asignar valor a fecha de emision.
                            string fechaEmision = document.Cs_tag_IssueDate;

                            int count = 0;
                            foreach (var item_grupo in grupo)
                            {
                                if (fechaEmision == item_grupo)
                                {
                                    count++;
                                }
                            }
                            if (count <= 0)//Si no existe el elemento en la lista, que se agregue.
                            {
                                grupo.Add(fechaEmision);
                            }
                        }
                    }
                   
                }
                #endregion
                //Crear grupos de fecha por IssueDate - FechaEmision
                List<string> doc_agregados = new List<string>();
                clsEntityVoidedDocuments comunicacion_baja;
                if (DocumentosADarDeBaja.Count > 0)
                {
                    //Si existen documentos a dar de baja por grupos.
                    foreach (var fecha_comunicacion in grupo)
                    {
                        //Buscar comunicacion de baja existente.
                        string documento_baja_existente = new clsEntityVoidedDocuments(localbd).cs_pxObtenerDocumentoComuninicacionBajaExisente(fecha_comunicacion,tipoContenido);
                        if (documento_baja_existente != "")
                        {
                            //Si existe comunicacion de baja Agregar los comprobantes al que ya se encuentra creado.
                            bool agregado = new clsEntityVoidedDocuments(localbd).cs_pxComunicacionBajaActualizar(DocumentosADarDeBaja, documento_baja_existente,tipoContenido);
                            if (agregado == true)
                            {
                                //Si se han agregado los comprobantes actualizar el nombre de la comunicacion de baja en caso se este agregando a otro que se haya generado anteriormente.
                                comunicacion_baja = new clsEntityVoidedDocuments(localbd).cs_fxObtenerUnoPorId(documento_baja_existente);
                                comunicacion_baja.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                                string correlativo = comunicacion_baja.Cs_tag_ID.Split('-')[2];
                                if (tipoContenido == "0")
                                {//Facturas Boletas y Notas
                                    comunicacion_baja.Cs_tag_ID = "RA-" + comunicacion_baja.Cs_tag_IssueDate.Replace("-", "") + "-" + correlativo;
                                }
                                else if (tipoContenido == "1")
                                {//Retencion
                                    comunicacion_baja.Cs_tag_ID = "RR-" + comunicacion_baja.Cs_tag_IssueDate.Replace("-", "") + "-" + correlativo;

                                }
                                comunicacion_baja.cs_pxActualizar(false,false);
                                doc_agregados.Add(comunicacion_baja.Cs_tag_ID);
                            }
                        }
                        else
                        {
                            //Crear documento de comunicación de baja ya que no existe
                            string documento_nuevo_id = Guid.NewGuid().ToString();
                            clsEntityVoidedDocuments documento_nuevo = new clsEntityVoidedDocuments(localbd);
                            documento_nuevo.Cs_pr_VoidedDocuments_Id = documento_nuevo_id;
                            documento_nuevo.Cs_tag_ReferenceDate = fecha_comunicacion;//DateTime.Now.ToString("yyyy-MM-dd");
                            documento_nuevo.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                            if (tipoContenido == "0")
                            {
                                documento_nuevo.Cs_tag_ID = "RA-" + documento_nuevo.Cs_tag_IssueDate.Replace("-", "") + "-" + new clsEntityVoidedDocuments(localbd).cs_fxObtenerCorrelativo("0");
                            }
                            else if (tipoContenido=="1")
                            {
                                documento_nuevo.Cs_tag_ID = "RR-" + documento_nuevo.Cs_tag_IssueDate.Replace("-", "") + "-" + new clsEntityVoidedDocuments(localbd).cs_fxObtenerCorrelativo("1");

                            }
                            documento_nuevo.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = declarante.Cs_pr_Ruc;
                            documento_nuevo.Cs_tag_AccountingSupplierParty_AdditionalAccountID = "6";
                            documento_nuevo.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = declarante.Cs_pr_RazonSocial;
                            documento_nuevo.Cs_pr_Ticket = "";
                            documento_nuevo.Cs_pr_EstadoSCC = "2";
                            documento_nuevo.Cs_pr_EstadoSUNAT = "2";
                            documento_nuevo.Cs_pr_ComentarioSUNAT = "";
                            documento_nuevo.Cs_pr_XML = "";
                            documento_nuevo.Cs_pr_CDR = "";
                            documento_nuevo.Cs_pr_DocumentoRelacionado = "";
                            documento_nuevo.Cs_pr_TipoContenido = tipoContenido;
                            string idRetorno = documento_nuevo.cs_pxInsertar(false,false);
                            clsEntityVoidedDocuments_VoidedDocumentsLine linea;
                            //Amaro Quispe Jordy | 2017-07-05 | FEI2-220
                            //Se comento el codigo que agregaba los documentos enviados anteriormente.Por pedido de Tania.
                            //INI-MODIFICA-01

                            //Buscar el ultimo documento de comunicación de baja enviado con la fecha de referencia = fecha del grupo
                            //Si existe, adjuntar los items existentes a este nuevo documento
                            /*string id_comunicacion_de_baja_existente_para_adjuntar_registros_a_sustitutorio = new clsEntityVoidedDocuments(localbd).cs_pxObtenerDocumentoComuninicacionBajaExisente(fecha_comunicacion, true,tipoContenido);
                            if (id_comunicacion_de_baja_existente_para_adjuntar_registros_a_sustitutorio != null && id_comunicacion_de_baja_existente_para_adjuntar_registros_a_sustitutorio.Trim().Length > 0)
                            {
                                List<clsEntityVoidedDocuments_VoidedDocumentsLine> DocumentosDadosDeBajaConAnterioridad = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraId(id_comunicacion_de_baja_existente_para_adjuntar_registros_a_sustitutorio);
                                foreach (var itemx in DocumentosDadosDeBajaConAnterioridad)
                                {
                                    if (tipoContenido == "0")
                                    {
                                        //facturas boletas y notas
                                        clsEntityDocument documento = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(itemx.Cs_pr_IDDocumentoRelacionado);
                                        //Agregar estos items y actualizar el id de comunicación de baja en el documento principal.
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        //linea.Cs_pr_VoidedDocuments_Id = itemx.Cs_pr_VoidedDocuments_Id;
                                        //Jordy Amaro 09-12-16 FE-912
                                        //CAMBIO DE ASOCIACION IDS
                                        //Ini-Modifica
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        //Fin-Modifica
                                        linea.Cs_tag_LineID = itemx.Cs_tag_LineID;
                                        linea.Cs_tag_DocumentTypeCode = itemx.Cs_tag_DocumentTypeCode;
                                        linea.Cs_tag_DocumentSerialID = itemx.Cs_tag_DocumentSerialID;
                                        linea.Cs_tag_DocumentNumberID = itemx.Cs_tag_DocumentNumberID;
                                        linea.Cs_tag_VoidReasonDescription = itemx.Cs_tag_VoidReasonDescription;
                                        linea.Cs_pr_IDDocumentoRelacionado = itemx.Cs_pr_IDDocumentoRelacionado;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_ComunicacionBaja = idRetorno;//Relación de comunicación de baja.
                                        documento.cs_pxActualizar(false, false);

                                    }
                                    else if (tipoContenido == "1")
                                    {
                                        //retencion
                                        clsEntityRetention documento = new clsEntityRetention(localbd).cs_fxObtenerUnoPorId(itemx.Cs_pr_IDDocumentoRelacionado);
                                        //Agregar estos items y actualizar el id de comunicación de baja en el documento principal.
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        //linea.Cs_pr_VoidedDocuments_Id = itemx.Cs_pr_VoidedDocuments_Id;
                                        //Jordy Amaro 09-12-16 FE-912
                                        //CAMBIO DE ASOCIACION IDS
                                        //Ini-Modifica
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        //Fin-Modifica
                                        linea.Cs_tag_LineID = itemx.Cs_tag_LineID;
                                        linea.Cs_tag_DocumentTypeCode = itemx.Cs_tag_DocumentTypeCode;
                                        linea.Cs_tag_DocumentSerialID = itemx.Cs_tag_DocumentSerialID;
                                        linea.Cs_tag_DocumentNumberID = itemx.Cs_tag_DocumentNumberID;
                                        linea.Cs_tag_VoidReasonDescription = itemx.Cs_tag_VoidReasonDescription;
                                        linea.Cs_pr_IDDocumentoRelacionado = itemx.Cs_pr_IDDocumentoRelacionado;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_Reversion = idRetorno;//Relación de comunicación de baja reversion.
                                        documento.cs_pxActualizar(false, false);

                                    }
                                   
                                }
                            }*/

                            //FIN-MODIFICA-01

                            //Recorrer los documentos a dar de Baja y  agregar las lineas.
                            foreach (var Item in DocumentosADarDeBaja)
                            {
                                if (tipoContenido == "0")
                                {//facturas boletas y notas
                                    //Si la fecha de referencia del item es igual a la fecha de referencia de su grupo, agregar
                                    clsEntityDocument documento = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(Item);
                                    if (documento.Cs_tag_IssueDate == fecha_comunicacion)
                                    {
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        linea.Cs_tag_LineID = "";
                                        linea.Cs_tag_DocumentTypeCode = documento.Cs_tag_InvoiceTypeCode;
                                        linea.Cs_tag_DocumentSerialID = documento.Cs_tag_ID.Split('-')[0].ToString();
                                        linea.Cs_tag_DocumentNumberID = documento.Cs_tag_ID.Split('-')[1].ToString();
                                        linea.Cs_tag_VoidReasonDescription = "";
                                        linea.Cs_pr_IDDocumentoRelacionado = Item;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_ComunicacionBaja = idRetorno;//Relación de comunicación de baja.
                                        documento.cs_pxActualizar(false, false);
                                    }
                                }else if (tipoContenido=="1")
                                {//retencion
                                 //Si la fecha de referencia del item es igual a la fecha de referencia de su grupo, agregar
                                    clsEntityRetention documento = new clsEntityRetention(localbd).cs_fxObtenerUnoPorId(Item);
                                    if (documento.Cs_tag_IssueDate == fecha_comunicacion)
                                    {
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        linea.Cs_tag_LineID = "";
                                        linea.Cs_tag_DocumentTypeCode = "20";
                                        linea.Cs_tag_DocumentSerialID = documento.Cs_tag_Id.Split('-')[0].ToString();
                                        linea.Cs_tag_DocumentNumberID = documento.Cs_tag_Id.Split('-')[1].ToString();
                                        linea.Cs_tag_VoidReasonDescription = "";
                                        linea.Cs_pr_IDDocumentoRelacionado = Item;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_Reversion = idRetorno;//Relación de comunicación de baja.
                                        documento.cs_pxActualizar(false, false);
                                    }
                                }
                            }
                            doc_agregados.Add(documento_nuevo.Cs_tag_ID);
                        }
                    }
                    //1. Buscar un resumen diario con la fecha de referencia que sea la misma del documento a dar de baja.
                    //2. Si ya existe, actualizar.
                    //3. Si no existe, agregar.


                    /*Buscar algún documento que tenga estado "sin estado"
                    SI ENCUENTRA
                        Actualizar los items no repetidos.
                    NO
                        Agregar documento de comunicación de baja
                        Agregar los items. */
                    if (docs_no_agregados.Count > 0)
                    {
                        foreach (string doc in docs_no_agregados)
                        {
                            mensaje += doc + "\n";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("procesarcomunicacion baja"+ex.ToString());
            }
            
            return mensaje;
        }
        /// <summary>
        /// Metodo para agregar listado - obsoleto
        /// </summary>
        /// <param name="tag_id"></param>
        /// <returns></returns>
        public bool agregarListado(string tag_id)
        {
            bool resultado = false;

            return resultado;
        }
        /// <summary>
        /// Metodo para descartar algun documento de comunicacion de baja.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="tipoContiene"></param>
        public void cs_pxDescartarDocumento(string Id,string tipoContiene)
        {
            //Solo se puede descartar aquellos documentos que no hayan tenido alguna respuesta de SUNAT
            clsEntityVoidedDocuments comunicacionbaja = new clsEntityVoidedDocuments(localbd).cs_fxObtenerUnoPorId(Id);
            if (comunicacionbaja.Cs_pr_EstadoSUNAT == "2")
            {
                List<clsEntityVoidedDocuments_VoidedDocumentsLine> comunicacionbaja_items = new clsEntityVoidedDocuments_VoidedDocumentsLine(localbd).cs_fxObtenerTodoPorCabeceraId(comunicacionbaja.Cs_pr_VoidedDocuments_Id);
                if (tipoContiene == "0")
                {
                    clsEntityDocument documento_principal;
                    foreach (var item in comunicacionbaja_items)
                    {
                        documento_principal = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(item.Cs_pr_IDDocumentoRelacionado);
                        documento_principal.Cs_pr_ComunicacionBaja = "";
                        documento_principal.cs_pxActualizar(false, false);
                    }
                }else if (tipoContiene == "1")
                {
                    clsEntityRetention documento_principal;
                    foreach (var item in comunicacionbaja_items)
                    {
                        documento_principal = new clsEntityRetention(localbd).cs_fxObtenerUnoPorId(item.Cs_pr_IDDocumentoRelacionado);
                        documento_principal.Cs_pr_Reversion = "";
                        documento_principal.cs_pxActualizar(false, false);
                    }
                }
               
                comunicacionbaja.cs_pxComunicacionBajaEliminar(Id);
            }
        }

        /*INICIO - Pequeña parte de Programacion orientada a Eventos*/

        //Cristhian|01/03/2018|FEI2-586
        /*Metodo creado para dar de baja los comprobantes electronicos que son enviados desde
          el sistema DBF comercial, en este caso solo facturas.*/
        /*NUEVO INICIO*/
        /// <summary>
        /// Método para dar de baja al comprobante electronico
        /// </summary>
        /// <param name="Id_Documento"></param>
        /// <param name="tipoContenido"></param>
        /// <returns>mensaje</returns>
        public string cs_pxProcesarComunicacionBaja(string Id_Documento, string tipoContenido)
        {
            string mensaje = string.Empty;
            try
            {
                //Buscar grupos de fechas seleccionadas y por cada grupo debe crear un archivo de comunicación de baja.
                #region "AGRUPAR POR COMUNICACIÓN DE BAJA"
                List<string> grupo = new List<string>();//Crear grupos de fecha por IssueDate - FECHA DE EMISION
                List<string> docs_no_agregados = new List<string>();//Lista de doccumentos no agregados

                    //Si los comprobantes son facturas, notas de creddito o debito, el tipo de contenido sera 0.
                    if (tipoContenido == "0")
                    {
                        clsEntityDocument document = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(Id_Documento);
                        if (document.Cs_pr_ComunicacionBaja.Trim().Length <= 0)
                        {
                            //Verificar los 7 dias anteriores al dia de hoy para permitir agregar a resumen diario.
                            string fechaEmision = document.Cs_tag_IssueDate;
                            if (fechaEmision == DateTime.Now.ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd") ||
                                fechaEmision == DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"))
                            {
                                int count = 0;
                                foreach (var item_grupo in grupo)
                                {
                                    if (fechaEmision == item_grupo)
                                    {
                                        count++;
                                    }
                                }
                                if (count <= 0)//Si no existe el elemento en la lista, que se agregue.
                                {
                                    grupo.Add(fechaEmision);
                                }
                            }
                            else
                            {
                                docs_no_agregados.Add(document.Cs_tag_ID);
                            }
                        }
                    }
                    /*En caso sean de tipo contenido 1, es para los documentos de retención*/
                    else if (tipoContenido == "1")
                    {
                        //Cuando se agregan tipo de documentos retencion
                        clsEntityRetention document = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(Id_Documento);
                        if (document.Cs_pr_Reversion.Trim().Length <= 0)
                        {
                            //Asignar valor a fecha de emision.
                            string fechaEmision = document.Cs_tag_IssueDate;

                            int count = 0;
                            foreach (var item_grupo in grupo)
                            {
                                if (fechaEmision == item_grupo)
                                {
                                    count++;
                                }
                            }
                            if (count <= 0)//Si no existe el elemento en la lista, que se agregue.
                            {
                                grupo.Add(fechaEmision);
                            }
                        }
                    }
                #endregion

                //Crear grupos de fecha por IssueDate - FechaEmision
                List<string> doc_agregados = new List<string>();
                clsEntityVoidedDocuments comunicacion_baja;

                    //Si existen documentos a dar de baja por grupos.
                    foreach (var fecha_comunicacion in grupo)
                    {
                        //Buscar comunicacion de baja existente.
                        string documento_baja_existente = new clsEntityVoidedDocuments(localDB).cs_pxObtenerDocumentoComuninicacionBajaExisente(fecha_comunicacion, tipoContenido);
                        if (documento_baja_existente != "")
                        {
                            //Si existe comunicacion de baja Agregar los comprobantes al que ya se encuentra creado.
                            bool agregado = new clsEntityVoidedDocuments(localDB).cs_pxComunicacionBajaActualizar(Id_Documento, documento_baja_existente, tipoContenido);
                            if (agregado == true)
                            {
                                //Si se han agregado los comprobantes actualizar el nombre de la comunicacion de baja en caso se este agregando a otro que se haya generado anteriormente.
                                comunicacion_baja = new clsEntityVoidedDocuments(localDB).cs_fxObtenerUnoPorId(documento_baja_existente);
                                comunicacion_baja.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                                string correlativo = comunicacion_baja.Cs_tag_ID.Split('-')[2];
                                if (tipoContenido == "0")
                                {//Facturas y Notas
                                    comunicacion_baja.Cs_tag_ID = "RA-" + comunicacion_baja.Cs_tag_IssueDate.Replace("-", "") + "-" + correlativo;
                                }
                                else if (tipoContenido == "1")
                                {//Retencion
                                    comunicacion_baja.Cs_tag_ID = "RR-" + comunicacion_baja.Cs_tag_IssueDate.Replace("-", "") + "-" + correlativo;

                                }
                                comunicacion_baja.cs_pxActualizar(false, false);
                                doc_agregados.Add(comunicacion_baja.Cs_tag_ID);
                            }
                        }
                        else
                        {
                            //Crear documento de comunicación de baja ya que no existe
                            string documento_nuevo_id = Guid.NewGuid().ToString();
                            clsEntityVoidedDocuments documento_nuevo = new clsEntityVoidedDocuments(localDB);
                            documento_nuevo.Cs_pr_VoidedDocuments_Id = documento_nuevo_id;
                            documento_nuevo.Cs_tag_ReferenceDate = fecha_comunicacion;//DateTime.Now.ToString("yyyy-MM-dd");
                            documento_nuevo.Cs_tag_IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
                            if (tipoContenido == "0")
                            {
                                documento_nuevo.Cs_tag_ID = "RA-" + documento_nuevo.Cs_tag_IssueDate.Replace("-", "") + "-" + new clsEntityVoidedDocuments(localbd).cs_fxObtenerCorrelativo("0");
                            }
                            else if (tipoContenido == "1")
                            {
                                documento_nuevo.Cs_tag_ID = "RR-" + documento_nuevo.Cs_tag_IssueDate.Replace("-", "") + "-" + new clsEntityVoidedDocuments(localbd).cs_fxObtenerCorrelativo("1");

                            }
                            documento_nuevo.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = declarante.Cs_pr_Ruc;
                            documento_nuevo.Cs_tag_AccountingSupplierParty_AdditionalAccountID = "6";
                            documento_nuevo.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = declarante.Cs_pr_RazonSocial;
                            documento_nuevo.Cs_pr_Ticket = "";
                            documento_nuevo.Cs_pr_EstadoSCC = "2";
                            documento_nuevo.Cs_pr_EstadoSUNAT = "2";
                            documento_nuevo.Cs_pr_ComentarioSUNAT = "";
                            documento_nuevo.Cs_pr_XML = "";
                            documento_nuevo.Cs_pr_CDR = "";
                            documento_nuevo.Cs_pr_DocumentoRelacionado = "";
                            documento_nuevo.Cs_pr_TipoContenido = tipoContenido;
                            string idRetorno = documento_nuevo.cs_pxInsertar(false, false);
                            clsEntityVoidedDocuments_VoidedDocumentsLine linea;

                                if (tipoContenido == "0")
                                {//facturas boletas y notas
                                    //Si la fecha de referencia del item es igual a la fecha de referencia de su grupo, agregar
                                    clsEntityDocument documento = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(Id_Documento);
                                    if (documento.Cs_tag_IssueDate == fecha_comunicacion)
                                    {
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        linea.Cs_tag_LineID = "";
                                        linea.Cs_tag_DocumentTypeCode = documento.Cs_tag_InvoiceTypeCode;
                                        linea.Cs_tag_DocumentSerialID = documento.Cs_tag_ID.Split('-')[0].ToString();
                                        linea.Cs_tag_DocumentNumberID = documento.Cs_tag_ID.Split('-')[1].ToString();
                                        linea.Cs_tag_VoidReasonDescription = "";
                                        linea.Cs_pr_IDDocumentoRelacionado = Id_Documento;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_ComunicacionBaja = idRetorno;//Relación de comunicación de baja.
                                        documento.cs_pxActualizar(false, false);
                                    }
                                }
                                else if (tipoContenido == "1")
                                {//retencion
                                 //Si la fecha de referencia del item es igual a la fecha de referencia de su grupo, agregar
                                    clsEntityRetention documento = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(Id_Documento);
                                    if (documento.Cs_tag_IssueDate == fecha_comunicacion)
                                    {
                                        linea = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB);
                                        linea.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                        linea.Cs_pr_VoidedDocuments_Id = idRetorno;
                                        linea.Cs_tag_LineID = "";
                                        linea.Cs_tag_DocumentTypeCode = "20";
                                        linea.Cs_tag_DocumentSerialID = documento.Cs_tag_Id.Split('-')[0].ToString();
                                        linea.Cs_tag_DocumentNumberID = documento.Cs_tag_Id.Split('-')[1].ToString();
                                        linea.Cs_tag_VoidReasonDescription = "";
                                        linea.Cs_pr_IDDocumentoRelacionado = Id_Documento;
                                        linea.cs_pxInsertar(false, true);
                                        documento.Cs_pr_Reversion = idRetorno;//Relación de comunicación de baja.
                                        documento.cs_pxActualizar(false, false);
                                    }
                                }
                            doc_agregados.Add(documento_nuevo.Cs_tag_ID);
                        }
                    //1. Buscar un resumen diario con la fecha de referencia que sea la misma del documento a dar de baja.
                    //2. Si ya existe, actualizar.
                    //3. Si no existe, agregar.


                    /*Buscar algún documento que tenga estado "sin estado"
                    SI ENCUENTRA
                        Actualizar los items no repetidos.
                    NO
                        Agregar documento de comunicación de baja
                        Agregar los items. */
                    if (docs_no_agregados.Count > 0)
                    {
                        foreach (string doc in docs_no_agregados)
                        {
                            mensaje += doc + "\n";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Comunicación de Baja Factura DBF: " + ex.ToString());
            }

            return mensaje;
        }
        /*NUEVO FIN*/

        //Cristhian|01/03/2018|FEI2-586
        /*Se crea este metodo, para procesar la comunicacion de baja documento por documento
          existe un metodo igual pero que solicita una lista de comprobantes, pero se usa
          desde el mismo FEI, para el caso de la comunicación con el DBF se usara este ya que
          el sistema Comercial puede dar de baja una factura a las vez*/
        /*NUEVO INICIO*/
        /// <summary>
        /// Metodo para procesar la comunicación de baja que proviene del sistema comercial.
        /// </summary>
        /// <param name="Id_Documento"></param>
        /// <returns>resultadoNoAgregados</returns>
        public string ProcesarComunicacionBaja(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente)
        {
            /*Se declara la variable para obtener el resutado */
            string resultadoNoAgregados = string.Empty;
            string Id_Comprobante = "";
            cls_Consulta QuerySQL = new cls_Consulta();
            
            /*Se instancia a cadena de conexion*/
            OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
            try
            {
                /*Se abre la conexion con el servidor de datos*/
                odbcConnection.Open();

                /*Se obtinene el Id del Comprobante*/
                OdbcCommand ExecuteQuery1 = new OdbcCommand(QuerySQL.Seleccionar_Comprobante_DBFComercial(SerieNumero, Codigo_Comprobante, Identificacion_Cliente), odbcConnection);
                Id_Comprobante = ExecuteQuery1.ExecuteScalar().ToString();
                odbcConnection.Close();

                if (Id_Comprobante != "")
                {
                    //Enviar el id del comprobante electronico para procesar en la comunicación de baja
                    resultadoNoAgregados = new clsNegocioCEComunicacionBaja(localDB).cs_pxProcesarComunicacionBaja(Id_Comprobante, "0");
                }
                
            }
            catch (Exception ex)
            {
                /*Su surge un error se registra en el archivo LOG*/
                clsBaseLog.cs_pxRegistarAdd("Factura de Baja: " + ex.ToString());
                resultadoNoAgregados = string.Empty;
            }
            
            return resultadoNoAgregados;
        }
        /*NUEVO FIN*/

        //Cristhian|07/03/2018|FEI2-586
        /*Conexion con la base de datos SQL Server para el DBF Comercial*/
        /*NUEVO INICIO*/
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        /*NUEVO FIN*/

        /*FIN - Pequeña parte de Programacion orientada a Eventos*/
    }
}
