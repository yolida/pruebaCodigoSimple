using System;
using System.Collections.Generic;
using Extension.Base;
using System.Data.Odbc;
using Extension.Negocio;
using System.Runtime.InteropServices;
using Extension.Query;
using System.Data;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDocument")]
    public class clsEntityDocument : clsBaseEntidad
    {
        #region Atributos
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_InvoiceTypeCode { get; set; }//No incluir en NOTAS DE DEBITO O CREDITO
        public string Cs_tag_DocumentCurrencyCode { get; set; }
        public string Cs_tag_Discrepancy_ReferenceID { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_Discrepancy_ResponseCode { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_Discrepancy_Description { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_BillingReference_ID { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_BillingReference_DocumentTypeCode { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_AdditionalAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyName_Name { get; set; } // Valor de prueba
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_District { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID { get; set; }
        public string Cs_tag_AccountingCustomerParty_AdditionalAccountID { get; set; }
        public string Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description { get; set; } //SOLO BOLETA
        public string Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID { get; set; }
        public string Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID { get; set; }
        public string Cs_pr_EstadoSUNAT { get; set; }
        public string Cs_pr_EstadoSCC { get; set; }
        public string Cs_pr_XML { get; set; }
        public string Cs_pr_CDR { get; set; }
        public string comprobante_xml_ticket { get; set; }
        public string Cs_pr_ComentarioSUNAT { get; set; }
        public string Cs_pr_Resumendiario { get; set; }
        public string comprobante_estadodocumentomodificacion { get; set; }
        public string comprobante_fechaenviodocumento { get; set; }
        public string Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount { get; set; }//Elemento que no estra en el pdf de estructura SUNAT
        public string Cs_pr_ComunicacionBaja { get; set; }//37
        public string Cs_pr_Empresa { get; set; }//38
        public string Cs_pr_Periodo { get; set; }//39
        public string Cs_Estado_EnvioCorreo { get; set; }//40
        public string Cs_Email_Cliente { get; set; }//41
        public string Cs_ResumenUltimo_Enviado { get; set; }//42
        public string Cs_TipoCambio { get; set; }//43
        public string Cs_tag_PerceptionSystemCode { get; set; }//44
        public string Cs_tag_PerceptionPercent { get; set; }//45
        /*Comunicación de Baja Boleta o Facturas*/
        public string Cs_tag_FechaDeBaja { get; set; }//46
        /*Anticipo Factura*/
        public string Cs_tag_Transaction { get; set; }//47

        /* UBL 2.1
        A partir de esta línea se agregará las nuevas etiquetas para la versión 2.1 de UBL */

        //public string { get; set; }
        public string Cs_tag_ProfileID { get; set; } // Etiqueta agregada para la versión 2.1 de UBL
        public string Cs_tag_IssueTime { get; set; } // Etiqueta agregada para la versión 2.1 de UBL


        #endregion
        public int cantidadElementos = 50; //Este valor se cambiará de acuerdo la cantidad de atributos del documento.

        public clsEntityDocument cs_fxObtenerUnoPorId(string id)
        {
            // clsEntityDatabaseLocal conf = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId();
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                #region Instancias fijas de atributos del documento
                Cs_pr_Document_Id = valores[0];
                Cs_tag_ID = valores[1];
                Cs_tag_IssueDate = valores[2];
                Cs_tag_InvoiceTypeCode = valores[3];//No incluir en NOTAS DE DEBITO O CREDITO
                Cs_tag_DocumentCurrencyCode = valores[4];
                Cs_tag_Discrepancy_ReferenceID = valores[5];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_Discrepancy_ResponseCode = valores[6];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_Discrepancy_Description = valores[7];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_BillingReference_ID = valores[8];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_BillingReference_DocumentTypeCode = valores[9];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = valores[10];
                Cs_tag_AccountingSupplierParty_AdditionalAccountID = valores[11];
                Cs_tag_AccountingSupplierParty_Party_PartyName_Name = valores[12];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = valores[13];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = valores[14];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = valores[15];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = valores[16];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = valores[17];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = valores[18];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = valores[19];
                Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = valores[20];
                Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = valores[21];
                Cs_tag_AccountingCustomerParty_AdditionalAccountID = valores[22];
                Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = valores[23];
                Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = valores[24]; //SOLO BOLETA
                Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = valores[25];
                Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = valores[26];
                Cs_pr_EstadoSUNAT = valores[27];
                Cs_pr_EstadoSCC = valores[28];
                Cs_pr_XML = valores[29];
                Cs_pr_CDR = valores[30];
                comprobante_xml_ticket = valores[31];
                Cs_pr_ComentarioSUNAT = valores[32];
                Cs_pr_Resumendiario = valores[33];
                comprobante_estadodocumentomodificacion = valores[34];
                comprobante_fechaenviodocumento = valores[35];
                Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = valores[36];
                Cs_pr_ComunicacionBaja = valores[37];
                Cs_pr_Empresa = valores[38];
                Cs_pr_Periodo = valores[39];
                Cs_Estado_EnvioCorreo = valores[40];
                Cs_Email_Cliente = valores[41];//agregado para email.
                Cs_ResumenUltimo_Enviado = valores[42];
                Cs_TipoCambio = valores[43];
                Cs_tag_PerceptionSystemCode = valores[44];
                Cs_tag_PerceptionPercent = valores[45];
                Cs_tag_FechaDeBaja = valores[46];
                Cs_tag_Transaction = valores[47];
                #endregion
                // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL


                Cs_tag_ProfileID = valores[49];


                Cs_tag_IssueTime = valores[50];

                return this;
            }
            else
            {
                return null;
            }
        }

        public clsEntityDocument FillData(string id)
        {
            // clsEntityDatabaseLocal conf = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId();
            List<string> tableValueList = new clsBaseConexion(localDB).GetFieldValuesById(cs_cmTabla, "cs_Document_Id", id);
            string[] clsEntityDocumentValues = new string[tableValueList.Count];
            if (tableValueList.Count > 0)
            {
                
                    //clsEntityDocumentValues[i] = tableValueList[i];

            }
            else
            {
            }
            return this; // Retornamos la misma clase
        }

        /*Clase Anidada clsEntityDocument Creado para el DBF*/
        /*INICIO CLASE DBF*/
        public clsEntityDocument()
        {
            cs_cmTabla = "cs_Document";
            cs_cmTabla_min = "cs_Document";
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= cantidadElementos; i++)//Número de campos, este valor se tiene que cambiar debido a las nuevas etiquetas
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= cantidadElementos; i++)//Número de campos, este valor se tiene que cambiar debido a las nuevas etiquetas
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        /*FIN CLASE DBF*/

        public clsEntityDocument(clsEntityDatabaseLocal local)
        {
            this.localDB = local;
            cs_cmTabla = "cs_Document";
            cs_cmTabla_min = "cs_Document";
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= cantidadElementos; i++)//Número de campos, este valor se tiene que cambiar debido a las nuevas etiquetas
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= cantidadElementos; i++)//Número de campos, este valor se tiene que cambiar debido a las nuevas etiquetas
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        /*Conexión con la base de datos SQL Server - Metodo creado para el uso del DBF COMERCIAL y CORP*/
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        /*Encontrado Fin*/

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_ID);//cp1
            cs_cmValores.Add(Cs_tag_IssueDate);//cp2
            cs_cmValores.Add(Cs_tag_InvoiceTypeCode);//No incluir en NOTAS DE DEBITO O CREDITO // cp3
            cs_cmValores.Add(Cs_tag_DocumentCurrencyCode);
            cs_cmValores.Add(Cs_tag_Discrepancy_ReferenceID);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_Discrepancy_ResponseCode);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_Discrepancy_Description);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_BillingReference_ID);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_BillingReference_DocumentTypeCode);//SOLO NOTAS DE DEBITO O CREDITO //cp9
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_AdditionalAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyName_Name);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID); //cp21
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_AdditionalAccountID);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName);//cp23
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description); //SOLO BOLETA
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID);
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID);
            cs_cmValores.Add(Cs_pr_EstadoSUNAT);//cp27
            cs_cmValores.Add(Cs_pr_EstadoSCC);//cp28
            cs_cmValores.Add(Cs_pr_XML);
            cs_cmValores.Add(Cs_pr_CDR);
            cs_cmValores.Add(comprobante_xml_ticket);
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);//cp32
            cs_cmValores.Add(Cs_pr_Resumendiario);//cp33
            cs_cmValores.Add(comprobante_estadodocumentomodificacion);
            cs_cmValores.Add(comprobante_fechaenviodocumento);//cp35
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount);
            cs_cmValores.Add(Cs_pr_ComunicacionBaja);//37
            cs_cmValores.Add(Cs_pr_Empresa);//38
            cs_cmValores.Add(Cs_pr_Periodo);//39
            cs_cmValores.Add(Cs_Estado_EnvioCorreo);//40
            cs_cmValores.Add(Cs_Email_Cliente);//41 agregado para el correo del
            cs_cmValores.Add(Cs_ResumenUltimo_Enviado);//42
            cs_cmValores.Add(Cs_TipoCambio);//43
            cs_cmValores.Add(Cs_tag_PerceptionSystemCode);//44
            cs_cmValores.Add(Cs_tag_PerceptionPercent);//45
            cs_cmValores.Add(Cs_tag_FechaDeBaja);//46 Creado para registrar la fecha de comunicación de baja
            cs_cmValores.Add(Cs_tag_Transaction);

            /* A partir de esta línea se agregará las nuevas etiquetas para la versión 2.1 de UBL */
            cs_cmValores.Add(Cs_tag_ProfileID); // Etiqueta agregada para la versión 2.1 de UBL
            cs_cmValores.Add(Cs_tag_IssueTime); // Etiqueta agregada para la versión 2.1 de UBL
        }
        public List<clsEntityDocument> cs_pxObtenerFiltroPrincipal(string tipo, string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin, string tipos_general, bool boletas)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                // sql += " AND (cp3='01' OR cp9='01') ";

                if (tipos_general != "")
                {
                    sql += " AND  cp3 IN (" + tipos_general + ") ";
                }
                if (tipo != "")
                {
                    sql += " AND cp3 ='" + tipo + "' ";
                }
                if (estadocomprobantescc != "")
                {
                    sql += " AND cp28 ='" + estadocomprobantescc + "' ";
                }
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                if (boletas)
                {
                    sql += " AND (cp9 ='03' or cp9='') ";
                }
                else
                {
                    sql += " AND cp9 !='03' ";
                    //sql += " AND cp1 LIKE 'F%' AND (cp8 LIKE '-' OR cp8 LIKE 'F%') ";//FEI2-MALO
                }
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[27].ToString();
                    item.Cs_pr_EstadoSCC = datos[28].ToString();
                    item.Cs_pr_XML = datos[29].ToString();
                    item.Cs_pr_CDR = datos[30].ToString();
                    item.comprobante_xml_ticket = datos[31].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[32].ToString();
                    item.Cs_pr_Resumendiario = datos[33].ToString();
                    item.comprobante_estadodocumentomodificacion = datos[34].ToString();
                    item.comprobante_fechaenviodocumento = datos[35].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[36].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[37].ToString();
                    item.Cs_pr_Empresa = datos[38].ToString();
                    item.Cs_pr_Periodo = datos[39].ToString();
                    item.Cs_Estado_EnvioCorreo = datos[40].ToString();
                    item.Cs_Email_Cliente = datos[41].ToString();//agregado para email.
                    item.Cs_ResumenUltimo_Enviado = datos[42].ToString();
                    item.Cs_TipoCambio = datos[43].ToString();
                    item.Cs_tag_PerceptionSystemCode = datos[44].ToString();
                    item.Cs_tag_PerceptionPercent = datos[45].ToString();
                    item.Cs_tag_FechaDeBaja = datos[46].ToString();//cp46 creado para registrar la fecha de baja
                    item.Cs_tag_Transaction = datos[47].ToString();//cp47 creado para declarar como anticipo
                    lista_documentos.Add(item);

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[48].ToString();
                    item.Cs_tag_IssueTime = datos[49].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDocument> cs_pxObtenerFiltroReporteGeneral(string tipo, string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin, string tipos_general, bool boletas)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT cp3,cp27 FROM " + cs_cmTabla + " WHERE 1=1";
                // sql += " AND (cp3='01' OR cp9='01') ";

                if (tipos_general != "")
                {

                    sql += " AND  cp3 IN (" + tipos_general + ") ";
                }
                if (tipo != "")
                {
                    sql += " AND cp3 ='" + tipo + "' ";
                }
                if (estadocomprobantescc != "")
                {
                    sql += " AND cp28 ='" + estadocomprobantescc + "' ";
                }
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                if (boletas)
                {
                    sql += " AND(cp9 ='03' or cp9='') ";
                }
                else
                {
                    sql += " AND cp9 !='03' ";
                }
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_tag_InvoiceTypeCode = datos[0].ToString();//No incluir en NOTAS DE DEBITO O CREDITO   
                    item.Cs_pr_EstadoSUNAT = datos[1].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDocument> cs_pxObtenerFiltroEnvioSunatFacturas(string tipo, string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin, string tipos_general, bool boletas)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                // sql += " AND (cp3='01' OR cp9='01') ";

                if (tipos_general != "")
                {

                    sql += " AND  cp3 IN (" + tipos_general + ") ";
                }
                if (tipo != "")
                {
                    sql += " AND cp3 ='" + tipo + "' ";
                }

                sql += " AND (cp28 ='0' or cp28='2') ";

                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                if (boletas)
                {
                    sql += " AND(cp9 ='03' or cp9='') ";
                }
                else
                {
                    sql += " AND cp9 !='03' ";
                    //sql += " AND cp1 LIKE 'F%' AND (cp8 LIKE '-' OR cp8 LIKE 'F%') ";//FEI2-MALO
                }
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[27].ToString();
                    item.Cs_pr_EstadoSCC = datos[28].ToString();
                    item.Cs_pr_XML = datos[29].ToString();
                    item.Cs_pr_CDR = datos[30].ToString();
                    item.comprobante_xml_ticket = datos[31].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[32].ToString();
                    item.Cs_pr_Resumendiario = datos[33].ToString();
                    item.comprobante_estadodocumentomodificacion = datos[34].ToString();
                    item.comprobante_fechaenviodocumento = datos[35].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[36].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[37].ToString();
                    item.Cs_pr_Empresa = datos[38].ToString();
                    item.Cs_pr_Periodo = datos[39].ToString();
                    item.Cs_Estado_EnvioCorreo = datos[40].ToString();
                    item.Cs_Email_Cliente = datos[41].ToString();//agregado para email.
                    item.Cs_ResumenUltimo_Enviado = datos[42].ToString();
                    item.Cs_TipoCambio = datos[43].ToString();
                    item.Cs_tag_PerceptionSystemCode = datos[44].ToString();
                    item.Cs_tag_PerceptionPercent = datos[45].ToString();
                    item.Cs_tag_FechaDeBaja = datos[46].ToString();//cp46 Agregado para registrar la fecha de baja
                    item.Cs_tag_Transaction = datos[47].ToString();//cp47 ¡Agregado para Anticipos

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[49].ToString();
                    item.Cs_tag_IssueTime = datos[50].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }

        //FEI2-325
        public List<clsEntityDocument> cs_pxObtenerFiltroAgregarResumen(string tipo, string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin, string tipos_general, bool boletas)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT cs_Document_Id,cp1,cp2,cp3,cp9,cp21,cp23,cp27,cp28,cp32,cp33,cp35,cp37 FROM " + cs_cmTabla + " WHERE 1=1";
                // sql += " AND (cp3='01' OR cp9='01') ";

                if (tipos_general != "")
                {

                    sql += " AND  cp3 IN (" + tipos_general + ") ";
                }
                if (tipo != "")
                {
                    sql += " AND cp3 ='" + tipo + "' ";
                }

                sql += " AND (cp28 = '2' or cp28 = '3')";// or cp28 = '3')";

                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                if (boletas)
                {
                    sql += " AND(cp9 ='03' or cp9='') ";
                }
                else
                {
                    sql += " AND cp9 !='03' ";
                }
                sql += "AND (cp33 = '' or cp33 is null) AND cp27 != '3'";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[4].ToString();//SOLO NOTAS DE DEBITO O CREDITO                 
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[5].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[6].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[7].ToString();
                    item.Cs_pr_EstadoSCC = datos[8].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[9].ToString();
                    item.Cs_pr_Resumendiario = datos[10].ToString();
                    item.comprobante_fechaenviodocumento = datos[11].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[12].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte 3" + ex.ToString());
                return null;
            }
        }
        public List<List<string>> cs_pxObtenerPorFiltroPrincipal(bool filtrardescripcion, bool filtrarrango, int comprobantetipo, string estadocomprobantesunat, string estadocomprobantescc, string serienumero, string ruc, string razonsocial, string fechainicio, string fechafin)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                if (filtrardescripcion == true)
                {
                    sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                    sql += " AND cp28 ='" + estadocomprobantescc + "' ";
                    sql += " AND cp1 LIKE '%" + serienumero + "%'";
                    sql += " AND cp10 LIKE '%" + ruc + "%'";
                    sql += " AND cp23 LIKE '%" + razonsocial + "%'";
                }
                if (filtrarrango == true)
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());

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
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerPorFiltroPrincipal " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDocument> cs_pxObtenerFiltroComunicacionBaja(string tipo, string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT cs_Document_Id,cp1,cp2,cp3,cp9,cp21,cp23,cp27,cp28,cp32,cp33,cp35,cp37 FROM " + cs_cmTabla + " WHERE 1=1 ";
                if (tipo != "")
                {
                    sql += " AND cp3 ='" + tipo + "' ";
                }
                if (estadocomprobantescc != "")
                {
                    sql += " AND cp28 ='" + estadocomprobantescc + "' ";
                }
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp27 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                sql += "AND (cp37 = '' or cp37 is null)";

                //Cristhian|27/02/2018|FEI2-585
                /*Script para evitar Boletas, Notas de Credito y Debito de Boletas*/
                /*NUEVO INICIO*/
                /*Como se sabe que el documento de boleta (Nota de cradito y Debito)
                 *empieza con la letra 'B' se omitira todo documento que empiece con B */
                sql += "AND cp1 NOT LIKE 'B%' ";
                /*NUEVO FIN*/

                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO                 
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[4].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[5].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[6].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[7].ToString();
                    item.Cs_pr_EstadoSCC = datos[8].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[9].ToString();
                    item.Cs_pr_Resumendiario = datos[10].ToString();
                    item.comprobante_fechaenviodocumento = datos[11].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[12].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDocument> cs_pxObtenerPorResumenDiario_n(string id)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            OdbcDataReader datos = null;
            try
            {
                lista_documentos = new List<clsEntityDocument>();
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cp33 ='" + id + "' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                //Lectura de registros consulta
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[27].ToString();
                    item.Cs_pr_EstadoSCC = datos[28].ToString();
                    item.Cs_pr_XML = datos[29].ToString();
                    item.Cs_pr_CDR = datos[30].ToString();
                    item.comprobante_xml_ticket = datos[31].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[32].ToString();
                    item.Cs_pr_Resumendiario = datos[33].ToString();
                    item.comprobante_estadodocumentomodificacion = datos[34].ToString();
                    item.comprobante_fechaenviodocumento = datos[35].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[36].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[37].ToString();
                    item.Cs_pr_Empresa = datos[38].ToString();
                    item.Cs_pr_Periodo = datos[39].ToString();
                    item.Cs_Estado_EnvioCorreo = datos[40].ToString();
                    item.Cs_Email_Cliente = datos[41].ToString();//agregado para email.
                    item.Cs_ResumenUltimo_Enviado = datos[42].ToString();
                    item.Cs_TipoCambio = datos[43].ToString();
                    item.Cs_tag_PerceptionSystemCode = datos[44].ToString();
                    item.Cs_tag_PerceptionPercent = datos[45].ToString();
                    item.Cs_tag_FechaDeBaja = datos[46].ToString();//46 Creado para registrar la fecha de comunicación de baja
                    item.Cs_tag_Transaction = datos[47].ToString();//cp47 Creado para anticipos

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[49].ToString();
                    item.Cs_tag_IssueTime = datos[50].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                return null;
            }
        }
        public List<List<string>> cs_pxObtenerPorResumenDiario(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cp33 ='" + id + "' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument cs_pxObtenerPorResumenDiario " + ex.ToString());
                return null;
            }
        }
        public List<string> cs_pxOrdenarAscendente(List<string> ids)
        {
            List<string> ordenado;
            try
            {
                ordenado = new List<string>();
                string comas = string.Join(",", ids.ToArray());
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cs_Document_Id IN (" + comas + ")";
                sql += " ORDER BY  cp1 ASC";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    ordenado.Add(datos[0].ToString().Trim());
                }
                cs_pxConexion_basedatos.Close();
                return ordenado;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxOrdenarAscendente " + ex.ToString());
                return null;
            }
        }
        public List<List<string>> cs_pxObtenerPorComunicacionBaja(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cp37 ='" + id + "' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila;
                while (datos.Read())//Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                {
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
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerPorComunicacionBaja" + ex.ToString());
                return null;
            }
        }

        public List<List<string>> cs_pxObtenerDocumentosPorComunicacionBaja(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                //Buscar todos los documentos en esta comunicación de baja
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE 1=1";
                //string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cs_VoidedDocuments_Id =" + id + " ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila2 = new List<string>();
                while (datos.Read())//Obtener los Id de los documentos relacionados
                {
                    fila2.Add(datos[7].ToString());
                }
                if (fila2.Count > 0)//Entonces buscar los documentos y agregarlos a la lista
                {
                    tabla_contenidos = new List<List<string>>();
                    foreach (var item in fila2)
                    {
                        OdbcDataReader datos1 = null;
                        //clsBaseConexion cn1 = new clsBaseConexion();
                        OdbcConnection cs_pxConexion_basedatos1 = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());

                        string sql1 = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                        sql1 += " AND cs_Document_Id =" + item + " ";
                        cs_pxConexion_basedatos1.Open();
                        datos1 = new OdbcCommand(sql1, cs_pxConexion_basedatos1).ExecuteReader();
                        List<string> fila;
                        while (datos1.Read())//Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                        {
                            fila = new List<string>();
                            for (int i = 0; i < datos1.FieldCount; i++)
                            {
                                fila.Add(datos1[i].ToString().Trim());
                            }
                            tabla_contenidos.Add(fila);
                        }
                        cs_pxConexion_basedatos.Close();
                    }
                }
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerDocumentosPorComunicacionBaja " + ex.ToString());
                return null;
            }
        }

        public List<string> cs_pxObtenerPendientesEnvio()
        {
            List<string> tabla_contenidos = new List<string>();
            try
            {
                //tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT cs_Document_Id FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND (cp3='01' OR cp9='01') ";
                sql += " AND cp27 ='2' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                // List<string> fila;
                while (datos.Read())
                {
                    //Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                    /* fila = new List<string>();
                     for (int i = 0; i < datos.FieldCount; i++)
                     {
                         fila.Add(datos[i].ToString().Trim());
                     }*/
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                //clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerPendientesEnvio" + ex.ToString());
                return tabla_contenidos;
            }
        }

        public List<string> cs_pxObtenerPendientesDeAgregarResumenDiario()
        {
            List<string> tabla_contenidos = new List<string>();
            try
            {
                //tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                DateTime FechaLimite = DateTime.Now.AddDays(-7);
                string sql = "SELECT cs_Document_Id FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND (cp3='03' OR cp9='03') ";
                sql += " AND cp2 >='" + FechaLimite.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp27='2' AND cp28='2' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                // List<string> fila;
                while (datos.Read())
                {
                    //Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                    /* fila = new List<string>();
                     for (int i = 0; i < datos.FieldCount; i++)
                     {
                         fila.Add(datos[i].ToString().Trim());
                     }*/
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                //clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerPendientesEnvio" + ex.ToString());
                return tabla_contenidos;
            }
        }

        public List<List<string>> cs_pxObtenerActualizacionAWeb()
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                DateTime FechaLimite = DateTime.Now.AddDays(-7);
                string sql = "SELECT cs_Document_Id,cp1 FROM cs_Document WHERE (cp3='01' OR cp3='03' OR cp3='07' OR cp3='08')  AND cp2>='" + FechaLimite.ToString("yyyy-MM-dd") + "' AND cp27='0';";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
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
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument cs_pxObtenerActualizacionAWeb " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDocument> cs_pxObtenerDocumentosPorComunicacionBaja_n(string id)
        {
            List<clsEntityDocument> lista_documentos;
            clsEntityDocument item;
            OdbcDataReader datos = null;
            try
            {
                //Buscar todos los documentos en esta comunicación de baja
                string sql = "SELECT cp6 FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE 1=1";
                //string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cs_VoidedDocuments_Id =" + id + " ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila2 = new List<string>();
                lista_documentos = new List<clsEntityDocument>();
                while (datos.Read())//Obtener los Id de los documentos relacionados
                {
                    fila2.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                if (fila2.Count > 0)//Entonces buscar los documentos y agregarlos a la lista
                {

                    foreach (var idDoc in fila2)
                    {
                        item = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(idDoc);
                        lista_documentos.Add(item);
                    }
                }
                return lista_documentos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                return null;
            }
        }
        public string cs_pxObtenerCantidadPendientesEnvio()
        {
            //string estadocomprobantescc = "0";
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp28 <> '" + estadocomprobantescc + "' ";
                sql += " AND (cp3='01' OR cp9='01') ";
                sql += " AND cp27 ='2' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                Int32 registros = 0;
                while (datos.Read())
                {
                    registros++;
                }
                cs_pxConexion_basedatos.Close();
                return registros.ToString();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument cs_pxObtenerCantidadPendientesEnvio" + ex.ToString());
                return null;
            }
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_id: " + Cs_pr_Document_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Id)) + "]" + ef;
            contenido += ei + "Invoice_ID: " + Cs_tag_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13_FHHH_NNNNNNNN(Cs_tag_ID)) + "]" + ef;
            contenido += ei + "Invoice_IssueDate: " + Cs_tag_IssueDate + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_10_F_YYYY_MM_DD(Cs_tag_IssueDate)) + "]" + ef; 
            contenido += ei + "Invoice_IssueTime: " + Cs_tag_IssueTime + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.ComprobarRegex(Cs_tag_IssueTime, clsNegocioValidar_Campos.regexHoraFormato24h)) + "]" + ef; // Agregado para la versión 2.1 de UBL
            if (Cs_tag_InvoiceTypeCode != "07" && Cs_tag_InvoiceTypeCode != "08")
            {
                contenido += ei + "Invoice_InvoiceTypeCode: " + Cs_tag_InvoiceTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(Cs_tag_InvoiceTypeCode)) + "]" + ef;//No incluir en NOTAS DE DEBITO O CREDITO
            }
            contenido += ei + "Invoice_DocumentCurrencyCode: " + Cs_tag_DocumentCurrencyCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(Cs_tag_DocumentCurrencyCode)) + "]" + ef;
            if (Cs_tag_InvoiceTypeCode == "07" && Cs_tag_InvoiceTypeCode == "08")
            {
                contenido += ei + "Invoice_Discrepancy_ReferenceID: " + Cs_tag_Discrepancy_ReferenceID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(Cs_tag_Discrepancy_ReferenceID)) + "]" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                contenido += ei + "Invoice_Discrepancy_ResponseCode: " + Cs_tag_Discrepancy_ResponseCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(Cs_tag_Discrepancy_ResponseCode)) + "]" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                contenido += ei + "Invoice_Discrepancy_Description: " + Cs_tag_Discrepancy_Description + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(Cs_tag_Discrepancy_Description)) + "]" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                contenido += ei + "Invoice_BillingReference_ID: " + Cs_tag_BillingReference_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(Cs_tag_BillingReference_ID)) + "]" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                contenido += ei + "Invoice_BillingReference_DocumentTypeCode: " + Cs_tag_BillingReference_DocumentTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(Cs_tag_BillingReference_DocumentTypeCode)) + "]" + ef;//SOLO NOTAS DE DEBITO O CREDITO
            }
            contenido += ei + "AccountingSupplierParty_CustomerAssignedAccountID: " + Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n11(Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_AdditionalAccountID: " + Cs_tag_AccountingSupplierParty_AdditionalAccountID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(Cs_tag_AccountingSupplierParty_AdditionalAccountID)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PartyName_Name: " + Cs_tag_AccountingSupplierParty_Party_PartyName_Name + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(Cs_tag_AccountingSupplierParty_Party_PartyName_Name)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_ID: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an6(Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_StreetName: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_25(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_CityName: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_CountrySubentity: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_District: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_District + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(Cs_tag_AccountingSupplierParty_Party_PostalAddress_District)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode: " + Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode)) + "]" + ef;
            contenido += ei + "AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName: " + Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName)) + "]" + ef;
            contenido += ei + "AccountingCustomerParty_CustomerAssignedAccountID: " + Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15(Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID)) + "]" + ef;
            contenido += ei + "AccountingCustomerParty_AdditionalAccountID: " + Cs_tag_AccountingCustomerParty_AdditionalAccountID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(Cs_tag_AccountingCustomerParty_AdditionalAccountID)) + "]" + ef;
            contenido += ei + "AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName: " + Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName)) + "]" + ef;
            if (Cs_tag_InvoiceTypeCode == "03")
            {
                contenido += ei + "AccountingCustomerParty_Party_PhysicalLocation_Description: " + Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description)) + "]" + ef; //SOLO BOLETA
            }
            contenido += ei + "LegalMonetaryTotal_ChargeTotalAmount_currencyID: " + Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID)) + "]" + ef; //No se encuentra el tipo que será validado (No está en la documentación de facturación electrónica)
            contenido += ei + "LegalMonetaryTotal_AllowanceTotalAmount: " + Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID)) + "]" + ef; //No se encuentra el tipo que será validado (No está en la documentación de facturación electrónica)
            contenido += ei + "LegalMonetaryTotal_PayableAmount_currencyID: " + Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID)) + "]" + ef;
            contenido += ei + "comprobanteestado_sunat: " + Cs_pr_EstadoSUNAT + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(Cs_pr_EstadoSUNAT)) + "]" + ef;
            contenido += ei + "comprobanteestado_scc: " + Cs_pr_EstadoSCC + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(Cs_pr_EstadoSCC)) + "]" + ef;
            return contenido;
        }

        internal List<clsEntityDocument> cs_pxBuscarDocumentos(string ruc, string tipodocumento, string fecha_inicio, string fecha_fin)
        {
            List<clsEntityDocument> documentos;
            try
            {
                documentos = new List<clsEntityDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp10 = '" + ruc + "' ";
                sql += " AND cp3  = '" + tipodocumento + "' ";
                sql += " AND cp2 >= '" + fecha_inicio + "' ";
                sql += " AND cp2 <= '" + fecha_fin + "' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntityDocument item;
                while (datos.Read())
                {
                    item = new clsEntityDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[27].ToString();
                    item.Cs_pr_EstadoSCC = datos[28].ToString();
                    item.Cs_pr_XML = datos[29].ToString();
                    item.Cs_pr_CDR = datos[30].ToString();
                    item.comprobante_xml_ticket = datos[31].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[32].ToString();
                    item.Cs_pr_Resumendiario = datos[33].ToString();
                    item.comprobante_estadodocumentomodificacion = datos[34].ToString();
                    item.comprobante_fechaenviodocumento = datos[35].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[36].ToString();
                    item.Cs_pr_ComunicacionBaja = datos[37].ToString();
                    item.Cs_pr_Empresa = datos[38].ToString();
                    item.Cs_pr_Periodo = datos[39].ToString();
                    item.Cs_Estado_EnvioCorreo = datos[40].ToString();
                    item.Cs_Email_Cliente = datos[41].ToString();
                    item.Cs_ResumenUltimo_Enviado = datos[42].ToString();
                    item.Cs_TipoCambio = datos[43].ToString();
                    item.Cs_tag_PerceptionSystemCode = datos[44].ToString();
                    item.Cs_tag_PerceptionPercent = datos[45].ToString();
                    item.Cs_tag_FechaDeBaja = datos[46].ToString();//46 Creado para registrar la fecha de comunicación de baja
                    documentos.Add(item);

                    // A partir de esta línea se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[49].ToString();
                    item.Cs_tag_IssueTime = datos[50].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return documentos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument cs_pxBuscarDocumentos" + ex.ToString());
                return null;
            }
        }

        internal clsEntityDocument cs_pxBuscarDocumento(string tipodocumento, string serienumero, string fecha, string monto)
        {
            clsEntityDocument documento;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3  = '" + tipodocumento + "' ";
                sql += " AND cp1  = '" + serienumero + "' ";
                sql += " AND cp2  = '" + fecha + "' ";
                sql += " AND cp26 = '" + monto + "' ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                documento = new clsEntityDocument(localDB);
                while (datos.Read())
                {
                    documento.Cs_pr_Document_Id = datos[0].ToString();
                    documento.Cs_tag_ID = datos[1].ToString();
                    documento.Cs_tag_IssueDate = datos[2].ToString();
                    documento.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    documento.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    documento.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    documento.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    documento.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    documento.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    documento.Cs_pr_EstadoSUNAT = datos[27].ToString();
                    documento.Cs_pr_EstadoSCC = datos[28].ToString();
                    documento.Cs_pr_XML = datos[29].ToString();
                    documento.Cs_pr_CDR = datos[30].ToString();
                    documento.comprobante_xml_ticket = datos[31].ToString();
                    documento.Cs_pr_ComentarioSUNAT = datos[32].ToString();
                    documento.Cs_pr_Resumendiario = datos[33].ToString();
                    documento.comprobante_estadodocumentomodificacion = datos[34].ToString();
                    documento.comprobante_fechaenviodocumento = datos[35].ToString();
                    documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[36].ToString();
                    documento.Cs_pr_ComunicacionBaja = datos[37].ToString();
                    documento.Cs_pr_Empresa = datos[38].ToString();
                    documento.Cs_pr_Periodo = datos[39].ToString();
                    documento.Cs_Estado_EnvioCorreo = datos[40].ToString();
                    documento.Cs_Email_Cliente = datos[41].ToString();
                    documento.Cs_ResumenUltimo_Enviado = datos[42].ToString();
                    documento.Cs_TipoCambio = datos[43].ToString();
                    documento.Cs_tag_PerceptionSystemCode = datos[44].ToString();
                    documento.Cs_tag_PerceptionPercent = datos[45].ToString();
                    documento.Cs_tag_FechaDeBaja = datos[46].ToString();//46 Creado para registrar la fecha de comunicación de baja
                    documento.Cs_tag_Transaction = datos[47].ToString();//47 Creado para facturas con anticipo

                    // A partir de esta línea se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    documento.Cs_tag_ProfileID = datos[49].ToString();
                    documento.Cs_tag_IssueTime = datos[50].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return documento;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument cs_pxBuscarDocumento " + ex.ToString());
                return null;
            }
        }
        public bool cs_pxEliminarDocumento(string Id)
        {
            bool resultado = false;
            try
            {
                //Eliminar la cabecera y todos sus registros.
                clsEntityDocument Document = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(Id);
                clsEntityDocument_AdditionalComments Document_AdditionalComments = new clsEntityDocument_AdditionalComments(localDB);
                clsEntityDocument_AdditionalDocumentReference AdditionalDocumentReference = new clsEntityDocument_AdditionalDocumentReference(localDB);
                clsEntityDocument_DespatchDocumentReference DespatchDocumentReference = new clsEntityDocument_DespatchDocumentReference(localDB);
                clsEntityDocument_Line Document_Line = new clsEntityDocument_Line(localDB);
                clsEntityDocument_Line_AdditionalComments Line_AdditionalComments = new clsEntityDocument_Line_AdditionalComments(localDB);
                clsEntityDocument_Line_Description Line_Description = new clsEntityDocument_Line_Description(localDB);
                clsEntityDocument_Line_PricingReference Line_PricingReference = new clsEntityDocument_Line_PricingReference(localDB);
                clsEntityDocument_Line_TaxTotal Document_Line_TaxTotal = new clsEntityDocument_Line_TaxTotal(localDB);
                clsEntityDocument_TaxTotal Document_TaxTotal = new clsEntityDocument_TaxTotal(localDB);
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation Document_UBLExtension_ExtensionContent_AdditionalInformation = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localDB);
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB);
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB);

                foreach (var item in Document_AdditionalComments.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in AdditionalDocumentReference.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in DespatchDocumentReference.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in Document_Line.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    foreach (var item1 in Line_AdditionalComments.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item1.cs_pxElimnar(false);
                    }

                    foreach (var item2 in Line_Description.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item2.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Line_PricingReference.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Document_Line_TaxTotal.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    item.cs_pxElimnar(false);
                }

                foreach (var item in Document_TaxTotal.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in Document_UBLExtension_ExtensionContent_AdditionalInformation.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    foreach (var item1 in Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id))
                    {
                        item1.cs_pxElimnar(false);
                    }

                    foreach (var item2 in Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id))
                    {
                        item2.cs_pxElimnar(false);
                    }

                    item.cs_pxElimnar(false);
                }

                Document.cs_pxElimnar(false);
                resultado = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxEliminarDocumento " + ex.ToString());
            }
            return resultado;

        }
        public string cs_pxBuscarFechaDocumento(string serienumero)
        {
            string result = "";
            string date = "";
            try
            {
                string text = "SELECT * FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND cp1  = '" + serienumero + "' ";
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                clsEntityDocument clsEntityDocument = new clsEntityDocument(localDB);
                while (odbcDataReader.Read())
                {
                    date = odbcDataReader[2].ToString();

                }
                odbcConnection.Close();
                result = date;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                //result = null;
            }
            return result;
        }
        public bool cs_pxBuscarDocumentoPorSerieNumero(string serienumero, string cadenaBaseDatos)
        {
            bool result = false;
            string date = "";
            try
            {
                string text = "SELECT cp1 FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND cp1  = '" + serienumero + "' ";
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(cadenaBaseDatos);
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                while (odbcDataReader.Read())
                {
                    date = odbcDataReader[0].ToString();
                    result = true;
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString() + serienumero + cadenaBaseDatos);
            }
            return result;
        }
        public bool cs_Buscar_DocumentoDuplicado(string SerieDocumento)
        {
            bool result = false;
            string SerieDoc = "";
            try
            {
                /*Cadena de Texto SQL para encontrar un documento Duplicado, si se encunetra, quiere decir que paso una modificación*/
                string text = "DECLARE @retVal int " +
                              "SELECT @retVal = COUNT(cp1) FROM cs_Document WHERE cp1 LIKE '" + SerieDocumento + "' " +
                              "IF(@retVal > 1) " +
                              "SELECT cp1 FROM cs_Document " +
                              "WHERE cp1 LIKE '" + SerieDocumento + "' AND cs_Document_Id = (SELECT MAX(cs_Document_Id) FROM cs_Document) ";

                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();

                /*Si encuentra datos para leer, entonces se tiene un documento modificado*/
                while (odbcDataReader.Read())
                {
                    /*Se devuelve true si se tiene un Docuemnto Duplicado (Modificado)*/
                    SerieDoc = odbcDataReader[0].ToString();
                    result = true;
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Buscar Duplicado Documento (clsEntityDocument): " + ex.ToString() + SerieDocumento);
            }
            /*Si se devuelve false, quiere decir que hay un solo registro por lo tanto no se ha modificado*/
            return result;
        }

        //Cristhian|13/11/2017|FEI2-325
        /*Metodo Creado para contrar los comprobantes asociados o contenidos en un Resumendiario, creado para limitar el numero de
         comprobantes a 500*/
        /*NUEVO INICIO*/
        public int cs_ContarResumenDiario_Lineas(string Summary_IdDocumento)
        {
            int result = 0;
            try
            {
                /*Cadena de Texto SQL para contar los comprobantes asociados al resumen diario*/
                string text = "SELECT COUNT(*) " +
                              "FROM cs_Document " +
                              "WHERE cp33 LIKE '" + Summary_IdDocumento + "' ";

                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();

                /*Leera la unica celda del, el cual es la cantidad de registtros de un determinado Resumen Diario*/
                while (odbcDataReader.Read())
                {
                    /*Se obtiene la Cantidad*/
                    result = Convert.ToInt32(odbcDataReader[0].ToString());
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Contar Documentos Asociados a Resumendiario (clsEntityDocument): " + ex.ToString());
            }
            /*Devuelve la cantidad de Comprobantes asociados*/
            return result;
        }
        /*NUEVO FIN*/

        //Cristhian|04/01/2018|FEI2-325
        /*Metodo Creado encontrar el documento asociado cuando se genera el resumen con notas de credito o debito*/
        //Cristhian|31/01/2018|FEI2-601
        /*Se agrega una variable mas al metodo, el metodo recibira el ID del Sumary Document*/
        /*INICIO MODIFICACIóN*/
        public List<string> cs_Buscar_ComprobanteAsociado_NotaCreditoDebito(string SummaryDocument_ID, string SummaryLine_DocumentoRelacionado)
        {
            List<string> result = new List<string>();

            try
            {
                /*Cadena de Texto SQL para Jalar los datos del Comprobante asociado a la nota de credito*/
                string text = "SELECT cp8, cp9 " +
                              "FROM cs_Document " +
                              "WHERE cp1 LIKE '" + SummaryLine_DocumentoRelacionado + "' AND (cp3 LIKE '07' OR cp3 LIKE '08') AND cp33 LIKE '" + SummaryDocument_ID + "' ";/*Formato del Dato a Pasar B001-00000008 */

                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();

                /*Leera las 2 celdas que contienen la serie y el numero correlativo del documento asociado a la nota de credito*/
                while (odbcDataReader.Read())
                {
                    /*Se obtiene los datos del comprobante asociado*/
                    result.Add(odbcDataReader[0].ToString());
                    result.Add(odbcDataReader[1].ToString());
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Documentos Asociados a Nota de Credito (clsEntityDocument): " + ex.ToString());
            }
            /*Devuelve los datos del comprobante asociado*/
            return result;
        }
        /*MODIFICACIóN FIN*/

        //Cristhian|04/01/2018|FEI2-325
        /*Metodo Creado encontrar el ID del cliente y su id adicional*/
        /*NUEVO INICIO*/
        public List<string> cs_Buscar_ClienteAsociado(string SummaryLine_DocumentoRelacionado)
        {
            List<string> result = new List<string>();

            try
            {
                /*Cadena de Texto SQL para Jalar los datos del Cliente(vendria a ser el cliente del cliente) asociado al comprobante*/
                string text = "SELECT cp21, cp22 " +
                              "FROM cs_Document " +
                              "WHERE cp1 LIKE '" + SummaryLine_DocumentoRelacionado + "' ";/*Formato del Dato a Pasar B001-00000008 */

                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();

                /*Leera las 2 celdas que contienen su DNI o RUC y el codigo adicional asignado por la empresa*/
                while (odbcDataReader.Read())
                {
                    /*Se obtiene los datos del Cliente asociado*/
                    result.Add(odbcDataReader[0].ToString());
                    result.Add(odbcDataReader[1].ToString());
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Documentos Asociados a Nota de Credito (clsEntityDocument): " + ex.ToString());
            }
            /*Devuelve los datos del cliente*/
            return result;
        }
        /*NUEVO FIN*/

        //Cristhian|14/02/2018|FEI2-487
        /*NUEVO INICIO*/
        /// <summary>
        /// Devuelve un valor booleano (true or false). Devuelve true si el documento tiene duplicado y a sido aceptado por SUNAT
        /// Devuelve false si el documento no tiene duplicados o el documento consultado no a sido aceptado por SUNAT
        /// </summary>
        /// <param name="Id_Comprobante"></param>
        /// <param name="SerieDocumento"></param>
        /// <param name="Fecha"></param>
        /// <param name="Codigo_tipo_documento"></param>
        /// <returns>Result(True or False)</returns>
        public bool cs_Buscar_DocumentoDuplicado(string Id_Comprobante, string SerieDocumento, string Fecha, string Codigo_tipo_documento)
        {
            bool result = false;
            int cantidad_documentos = 0;
            try
            {
                /*Cadena de Texto SQL para contar cuantos comprobates duplicados hay*/
                string text = "SELECT count(*) " +
                              "FROM cs_Document " +
                              "WHERE cp1 LIKE '" + SerieDocumento + "' AND cp2 LIKE '" + Fecha + "' AND cp3 LIKE '" + Codigo_tipo_documento + "' ";

                /*Se abre la cadena de conexion con el servidor - Una sola vez*/
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();

                /*Se ejecuta el comando SQL, con el Scalar, ya que la sentencia SQL nos devuelbe un solo valor*/
                OdbcCommand ObtenerDuplicado = new OdbcCommand(text, odbcConnection);

                /*El valor devvuelto se almacena en la variable cantidad_documentos*/
                cantidad_documentos = int.Parse(ObtenerDuplicado.ExecuteScalar().ToString());

                /*Si es más de 1 entonces, entonces se procede a verificar si el archivo consultado ha sido aceptado por SUNAT*/
                if (cantidad_documentos > 1)
                {
                    //result = Verificar_Estado_Diferente_Pendiente(Id_Comprobante);
                    result = false;
                }
                /*Si es uno quiere decir que no tiene duplicados por lo tanto se envia false*/
                else
                {
                    //result = Verificar_Estado_Diferente_Pendiente(Id_Comprobante);
                    result = true;
                }
                /*Se cierra la cadena de conexión*/
                odbcConnection.Close();
            }
            /*Si existe algun error se registra en el archivo Log del FEI*/
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Buscar Duplicado Documento (clsEntityDocument): " + ex.ToString() + " Stack Trace: " + ex.StackTrace);
            }

            return result;
        }
        /*NUEVO FIN*/

        //Cristhian|14/02/2018|FEI2-487
        /*No se utilza a peticion de TANIA*/
        /*NUEVO INICIO*/
        public bool Verificar_Estado_Diferente_Pendiente(string Id_Comprobante)
        {
            bool result = false;
            int cantidad_documentos = 0;
            try
            {
                /*Cadena de texto para averiguar si el comprobante consultado ha sido aceptado por SUNAT*/
                string text = "SELECT count(*) " +
                       "FROM cs_Document " +
                       "WHERE cs_Document_Id LIKE '" + Id_Comprobante + "' " +
                       "AND cp28 NOT LIKE '2' AND cp27 NOT LIKE '2' OR cp27 NOT LIKE '4'  ";

                /*Se abre la cadena de conexion con el servidor - Una sola vez*/
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();

                /*Se ejecuta el comando SQL, con el Scalar, ya que la sentencia SQL nos devuelbe un solo valor*/
                OdbcCommand Verificar_Comprobante_Aceptado = new OdbcCommand(text, odbcConnection);

                /*El valor devuelto se almacena en la variable cantidad_documentos*/
                cantidad_documentos = int.Parse(Verificar_Comprobante_Aceptado.ExecuteScalar().ToString());

                /*Si el documento es aceptado por SUNAT se envia true*/
                if (cantidad_documentos == 1)
                {
                    result = true;
                }
                /*Caso contrario se envia false, ya que no ha sido aceptado por SUNAT y tiene Duplicados registrados en SUNAT*/
                else
                {
                    result = false;
                }
            }
            /*Si existe algun error se registra en el archivo Log del Sistema FEI*/
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Buscar Duplicado Documento (clsEntityDocument): " + ex.ToString() + " Stack Trace: " + ex.StackTrace);
            }
            return result;
        }
        /*NUEVO FIN*/

        //Cristhian|01/03/2018|FEI2-586
        /*Metodo creado para ser utilizado para dar de Baja los comprobantes electronicos desde el DBF Comercial*/
        /*NUEVO INICIO*/
        public bool Comprobante_Anular(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente, string Estado)
        {
            /*Se declaran las variables que se utilizaran en el metodo*/
            bool Resultado = false;
            string Id_Comprobante = "";
            string NumeroSerie_Comprobante_Asociado = "00";
            string CodigoComprobante_Asosiado = "00";
            string Estado_SUNAT = "";
            string Estado_CCS = "";
            string Mensaje_Error = "";

            /*Se instancia la clase donde se almacena las cadenas SQL*/
            cls_Consulta QuerySQL = new cls_Consulta();

            /*Se instancia a cadena de conexion*/
            OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
            OdbcCommand ExecuteQuery1;
            ExecuteQuery1 = new OdbcCommand(QuerySQL.Seleccionar_Comprobante_DBFComercial(SerieNumero, Codigo_Comprobante, Identificacion_Cliente), odbcConnection);
            try
            {
                /*Se abre la conexion con el servidor de datos*/
                odbcConnection.Open();

                /*Se obtinene el Id del Comprobante*/
                OdbcDataReader Datos_Comprobante = ExecuteQuery1.ExecuteReader();
                while (Datos_Comprobante.Read())
                {
                    Id_Comprobante = Datos_Comprobante.GetString(0);
                    if (Datos_Comprobante.GetString(1).Trim().Length != 0 && Datos_Comprobante.GetString(2).Trim().Length != 0)
                    {
                        NumeroSerie_Comprobante_Asociado = Datos_Comprobante.GetString(1);
                        CodigoComprobante_Asosiado = Datos_Comprobante.GetString(2);
                    }
                    Estado_SUNAT = Datos_Comprobante.GetString(3);
                    Estado_CCS = Datos_Comprobante.GetString(4);
                }

                if (Id_Comprobante != "")
                {
                    if (Codigo_Comprobante == "03" || (NumeroSerie_Comprobante_Asociado.Substring(0, 1) == "B" && CodigoComprobante_Asosiado == "03"))
                    {
                        if (Estado_SUNAT == "0" && Estado_CCS == "0")
                        {
                            /*Se efectua el cambio*/
                            ExecuteQuery1 = new OdbcCommand(QuerySQL.Anular_Comprobante_DBFComercial(Id_Comprobante, Estado), odbcConnection);
                            ExecuteQuery1.ExecuteNonQuery();
                            Resultado = true;
                        }
                        else
                        {
                            /*Como en el FEI no se a enviado el Documento a SUNAT, no se debe dar de baja el documento*/
                            Mensaje_Error = "La Boleta o Nota electrónica de Boleta no a sido comunicado a SUNAT o esta en Proceso";
                            Mensaje_Error += "Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03";
                            Resultado = false;
                        }

                    }
                    else if (Codigo_Comprobante == "01" || (NumeroSerie_Comprobante_Asociado.Substring(0, 1) == "F" && CodigoComprobante_Asosiado == "01"))
                    {
                        if (Estado_SUNAT == "0" && Estado_CCS == "0")
                        {
                            clsNegocioCEComunicacionBaja Factura = new clsNegocioCEComunicacionBaja(localDB);
                            Factura.ProcesarComunicacionBaja(SerieNumero, Codigo_Comprobante, Identificacion_Cliente);
                            Resultado = true;
                        }
                        else
                        {
                            /*Como en el FEI no se a enviado el Documento a SUNAT, no se debe dar de baja el documento*/
                            Mensaje_Error = "La Factura o Nota electrónica de Factura no a sido comunicado a SUNAT o esta en Proceso";
                            Mensaje_Error += "Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03";
                            Resultado = false;
                        }
                    }
                    else
                    {
                        /*Se devuelve false por que no cumple con ninguno de los criterios anteriores*/
                        Mensaje_Error = "Revisar el código del comprobante y sus comprobantes asociados por si es una nota de credito o debito";
                        Mensaje_Error += "Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03";
                        Resultado = false;
                    }
                }
                else
                {
                    /*Se devuelve false por que no se encuentra el id_documento, por los siguientes motivos: */
                    Mensaje_Error = "No se encontró el documento: \n>El comprobante no esta registrado en el FEI o\n>El comprobante ya fue enviado a SUNAT";
                    Mensaje_Error += "Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03";
                    Resultado = false;
                }

                if (Mensaje_Error != "")
                {
                    clsBaseLog.cs_pxRegistar("Anular comprobante: " + Mensaje_Error);
                    clsBaseLog.cs_pxRegistar("Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03");
                }
            }
            catch (Exception ex)
            {
                /*Si se presenta un error es registrado en el archivo LOG*/
                clsBaseLog.cs_pxRegistar("Anular comprobante: " + ex.ToString() + " datos de consulta " + Id_Comprobante + "; " + NumeroSerie_Comprobante_Asociado + "; " + CodigoComprobante_Asosiado);
                clsBaseLog.cs_pxRegistar("Comprobante Asociado" + NumeroSerie_Comprobante_Asociado.Substring(0, 1) + "= B y Cpdigo Comprobante" + CodigoComprobante_Asosiado + "= 03");
                /*Y se devuelve false*/
                Resultado = false;
            }
            /*Se cierra la conexion con el servidor de datos*/
            odbcConnection.Close();

            /*y se devuelve true o false*/
            return Resultado;
        }
        /*NUEVO FIN*/

        //Cristhian|01/03/2018|FEI2-586
        /*Metodo creado para verificar si el comprobante electronico a sido enviado a SUNAT, como se asocia con el sistema DBF Comercial
          este debe devolver true or false para que le sistema DBF comercial no permita la modificacion del comprobante una vez que ya se
          halla enviado a SUNAT*/
        /*NUEVO INICIO*/
        public string Verificar_Estado_Comprobante(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente, string Monto)
        {
            /*Se declaran las variables que se utilizaran en el metodo*/
            string Resultado = "2";
            string EstadoComprobante = "";

            /*Se instancia la clase donde se almacena las cadenas SQL*/
            cls_Consulta QuerySQL = new cls_Consulta();

            /*Se instancia a cadena de conexion*/
            OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
            try
            {
                /*Se abre la conexion con el servidor de datos*/
                odbcConnection.Open();

                /*Se busca el estado del comprobante - SUNAT cp27*/
                OdbcCommand ExecuteQuery1 = new OdbcCommand(QuerySQL.Estado_SUNAT_para_DBFComercial(SerieNumero, Codigo_Comprobante, Identificacion_Cliente, Monto), odbcConnection);
                EstadoComprobante = ExecuteQuery1.ExecuteScalar().ToString();

                /*Se verifica que se tenga algo diferente a lo iniciado*/
                if (EstadoComprobante != "")
                {
                    switch (EstadoComprobante)
                    {
                        case "0":
                            Resultado = "Aprobado";
                            break;
                        case "1":
                            Resultado = "Rechazado";
                            break;
                        case "2":
                            Resultado = "Pendiente";
                            break;
                        case "3":
                            Resultado = "Anulado";
                            break;
                        default:
                            Resultado = "En Proceso";
                            break;
                    }
                }
                else
                {
                    /*Si se tiene lo mismo que la variable iniciada - sin dudar se devuelve false*/
                    Resultado = "No Estado";
                }

            }
            catch (Exception ex)
            {
                /*Si se presenta un error es registrado en el archivo LOG*/
                clsBaseLog.cs_pxRegistar("Buscar estado documento: " + ex.ToString());

                /*Y se devuelve false*/
                Resultado = "Error Consulta Estado";
            }
            /*Se cierra la conexion con el servidor de datos*/
            odbcConnection.Close();

            /*y se devuelve true o false*/
            return Resultado;
        }
        /*NUEVO FIN*/

        /*Nuevos desarrollos*/
        /*Jorge Luis|03/05/2018|Pendiente
        Método para consultar el estado del documento*/
        public string ConsultaEstadoDocumento(string serieNumero, string codigoComprobante, string identificacionCliente)
        {
            OdbcConnection odbcConnection;
            string query = string.Empty;
            string resultado = string.Empty;
            OdbcCommand cmd;
            try
            {
                odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                query = " select top 1 cp27 from cs_Document " + " where cp1 = '" + serieNumero +
                    "' and cp3 = '" + codigoComprobante + "' and cp21 = '" + identificacionCliente + "' order by cs_Document_Id desc";
                cmd = new OdbcCommand(query, odbcConnection);
                //cmd.CommandType = CommandType.Text;
                resultado = (string)cmd.ExecuteScalar();
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistar("resultado: " + resultado);
                query = "Sin resultado";
                clsBaseLog.cs_pxRegistar(ex.ToString());
                clsBaseLog.cs_pxRegistar(ex.ToString() + "-  Cadena de conexión" + " - odbcConnection: "  /* +odbcConnection.ToString()*/);
            }
            return resultado;
        }
    }
}
