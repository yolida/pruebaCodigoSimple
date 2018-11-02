using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityRetention")]
    public class clsEntityRetention:clsBaseEntidad
    {
        public string Cs_pr_Retention_id { get; set; }
        public string Cs_tag_Id { get; set; } //1
        public string Cs_tag_IssueDate { get; set; } //2
        public string Cs_tag_PartyIdentification_Id { get; set; } //3
        public string Cs_tag_PartyIdentificacion_SchemeId { get; set; } //4
        public string Cs_tag_PartyName { get; set; } //5
        public string Cs_tag_PostalAddress_Id { get; set; } //6
        public string Cs_tag_PostalAddress_StreetName { get; set; } //7
        public string Cs_tag_PostalAddress_CitySubdivisionName { get; set; } //8
        public string Cs_tag_PostalAddress_CityName { get; set; } //9
        public string Cs_tag_PostalAddress_CountrySubEntity { get; set; } //10
        public string Cs_tag_PostalAddress_District { get; set; } //11
        public string Cs_tag_PostalAddress_Country_IdentificationCode { get; set; } //12
        public string Cs_tag_PartyLegalEntity_RegistrationName { get; set; } //13
        public string Cs_tag_ReceiveParty_PartyIdentification_Id { get; set; } //14
        public string Cs_tag_ReceiveParty_PartyIdentification_SchemeId { get; set; } //15
        public string Cs_tag_ReceiveParty_PartyName_Name { get; set; } //16
        public string Cs_tag_ReceiveParty_PostalAddress_Id { get; set; } //17
        public string Cs_tag_ReceiveParty_PostalAddress_StreetName { get; set; } //18
        public string Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName { get; set; } //19
        public string Cs_tag_ReceiveParty_PostalAddress_CityName { get; set; } //20
        public string Cs_tag_ReceiveParty_PostalAddress_CountrySubentity { get; set; } //21
        public string Cs_tag_ReceiveParty_PostalAddress_District { get; set; } //22
        public string Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode { get; set; } //23
        public string Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName { get; set; } //24
        public string Cs_tag_SUNATRetentionSystemCode { get; set; } //25
        public string Cs_tag_SUNATRetentionPercent { get; set; } //26
        public string Cs_tag_Note { get; set; } //27
        public string Cs_tag_TotalInvoiceAmount { get; set; } //28
        public string Cs_tag_TotalInvoiceAmount_CurrencyId { get; set; } //29
        public string Cs_tag_TotalPaid { get; set; } //30
        public string Cs_tag_TotalPaid_CurrencyId { get; set; }  //31
        public string Cs_pr_EstadoSCC { get; set; } //32
        public string Cs_pr_EstadoSUNAT { get; set; } //33
        public string Cs_pr_ComentarioSUNAT { get; set; } //34
        public string Cs_pr_XML { get; set; } //35
        public string Cs_pr_CDR { get; set; } //36
        public string Cs_pr_Reversion { get; set; } //37
        public string Cs_pr_Reversion_Anterior { get; set; } //38
        public string Cs_pr_FechaEnvio { get; set; } //39
        public string Cs_pr_FechaRecepcion { get; set; } //40
        //private clsBaseConexion cn;
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Retention_id);
            cs_cmValores.Add(Cs_tag_Id);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_PartyIdentification_Id);
            cs_cmValores.Add(Cs_tag_PartyIdentificacion_SchemeId);
            cs_cmValores.Add(Cs_tag_PartyName);
            cs_cmValores.Add(Cs_tag_PostalAddress_Id);
            cs_cmValores.Add(Cs_tag_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CountrySubEntity);
            cs_cmValores.Add(Cs_tag_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyIdentification_Id);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyIdentification_SchemeId);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyName_Name);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_Id);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CountrySubentity);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_SUNATRetentionSystemCode);
            cs_cmValores.Add(Cs_tag_SUNATRetentionPercent);
            cs_cmValores.Add(Cs_tag_Note);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_TotalPaid);
            cs_cmValores.Add(Cs_tag_TotalPaid_CurrencyId);
            cs_cmValores.Add(Cs_pr_EstadoSCC);
            cs_cmValores.Add(Cs_pr_EstadoSUNAT);
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);
            cs_cmValores.Add(Cs_pr_XML);
            cs_cmValores.Add(Cs_pr_CDR);
            cs_cmValores.Add(Cs_pr_Reversion);
            cs_cmValores.Add(Cs_pr_Reversion_Anterior);
            cs_cmValores.Add(Cs_pr_FechaEnvio);
            cs_cmValores.Add(Cs_pr_FechaRecepcion);
        }
       
        public clsEntityRetention(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Retention";
            cs_cmCampos.Add("cs_Retention_Id");
            for (int i = 1; i <= 40; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Retention";
            cs_cmCampos_min.Add("cs_Retention_Id");
            for (int i = 1; i <= 40; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityRetention()
        {
           // localDB = local;
            cs_cmTabla = "cs_Retention";
            cs_cmCampos.Add("cs_Retention_Id");
            for (int i = 1; i <= 40; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Retention";
            cs_cmCampos_min.Add("cs_Retention_Id");
            for (int i = 1; i <= 40; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityRetention cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Retention_id = valores[0];
                Cs_tag_Id = valores[1];
                Cs_tag_IssueDate = valores[2];
                Cs_tag_PartyIdentification_Id = valores[3];
                Cs_tag_PartyIdentificacion_SchemeId = valores[4];
                Cs_tag_PartyName = valores[5];
                Cs_tag_PostalAddress_Id = valores[6];
                Cs_tag_PostalAddress_StreetName = valores[7];
                Cs_tag_PostalAddress_CitySubdivisionName = valores[8];
                Cs_tag_PostalAddress_CityName = valores[9];
                Cs_tag_PostalAddress_CountrySubEntity = valores[10];
                Cs_tag_PostalAddress_District = valores[11];
                Cs_tag_PostalAddress_Country_IdentificationCode = valores[12];
                Cs_tag_PartyLegalEntity_RegistrationName = valores[13];
                Cs_tag_ReceiveParty_PartyIdentification_Id = valores[14];
                Cs_tag_ReceiveParty_PartyIdentification_SchemeId = valores[15];
                Cs_tag_ReceiveParty_PartyName_Name = valores[16];
                Cs_tag_ReceiveParty_PostalAddress_Id = valores[17];
                Cs_tag_ReceiveParty_PostalAddress_StreetName = valores[18];
                Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName = valores[19];
                Cs_tag_ReceiveParty_PostalAddress_CityName = valores[20];
                Cs_tag_ReceiveParty_PostalAddress_CountrySubentity = valores[21];
                Cs_tag_ReceiveParty_PostalAddress_District = valores[22];
                Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode = valores[23];
                Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName = valores[24];
                Cs_tag_SUNATRetentionSystemCode = valores[25];
                Cs_tag_SUNATRetentionPercent = valores[26];
                Cs_tag_Note = valores[27];
                Cs_tag_TotalInvoiceAmount = valores[28];
                Cs_tag_TotalInvoiceAmount_CurrencyId = valores[29];
                Cs_tag_TotalPaid = valores[30];
                Cs_tag_TotalPaid_CurrencyId = valores[31];
                Cs_pr_EstadoSCC = valores[32];
                Cs_pr_EstadoSUNAT = valores[33];
                Cs_pr_ComentarioSUNAT = valores[34];
                Cs_pr_XML = valores[35];
                Cs_pr_CDR = valores[36];
                Cs_pr_Reversion = valores[37];
                Cs_pr_Reversion_Anterior = valores[38];
                Cs_pr_FechaEnvio = valores[39];
                Cs_pr_FechaRecepcion = valores[40];
                return this;
            }
            else
            {
                return null;
            }        

        }
        public List<clsEntityRetention> cs_pxObtenerFiltroPrincipal(string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin)
        {
            List<clsEntityRetention> lista_documentos;
            clsEntityRetention item;
            try
            {
                lista_documentos = new List<clsEntityRetention>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
               
                if (estadocomprobantescc != "")
                {
                    sql += " AND cp32 ='" + estadocomprobantescc + "' ";
                }
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp33 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                
                //cn = new clsBaseConexion()-;
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityRetention(localDB);
                    item.Cs_pr_Retention_id = datos[0].ToString();
                    item.Cs_tag_Id = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_PartyIdentification_Id = datos[3].ToString();
                    item.Cs_tag_PartyIdentificacion_SchemeId = datos[4].ToString();
                    item.Cs_tag_PartyName = datos[5].ToString();
                    item.Cs_tag_PostalAddress_Id = datos[6].ToString();
                    item.Cs_tag_PostalAddress_StreetName = datos[7].ToString();
                    item.Cs_tag_PostalAddress_CitySubdivisionName = datos[8].ToString();
                    item.Cs_tag_PostalAddress_CityName = datos[9].ToString();
                    item.Cs_tag_PostalAddress_CountrySubEntity = datos[10].ToString();
                    item.Cs_tag_PostalAddress_District = datos[11].ToString();
                    item.Cs_tag_PostalAddress_Country_IdentificationCode = datos[12].ToString();
                    item.Cs_tag_PartyLegalEntity_RegistrationName = datos[13].ToString();
                    item.Cs_tag_ReceiveParty_PartyIdentification_Id = datos[14].ToString();
                    item.Cs_tag_ReceiveParty_PartyIdentification_SchemeId = datos[15].ToString();
                    item.Cs_tag_ReceiveParty_PartyName_Name = datos[16].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_Id = datos[17].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_StreetName = datos[18].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName = datos[19].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CityName = datos[20].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity = datos[21].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_District = datos[22].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode = datos[23].ToString();
                    item.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName = datos[24].ToString();
                    item.Cs_tag_SUNATRetentionSystemCode = datos[25].ToString();
                    item.Cs_tag_SUNATRetentionPercent = datos[26].ToString();
                    item.Cs_tag_Note = datos[27].ToString();
                    item.Cs_tag_TotalInvoiceAmount = datos[28].ToString();
                    item.Cs_tag_TotalInvoiceAmount_CurrencyId = datos[29].ToString();
                    item.Cs_tag_TotalPaid = datos[30].ToString();
                    item.Cs_tag_TotalPaid_CurrencyId = datos[31].ToString();
                    item.Cs_pr_EstadoSCC = datos[32].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[33].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[34].ToString();
                    item.Cs_pr_XML = datos[35].ToString();
                    item.Cs_pr_CDR = datos[36].ToString();
                    item.Cs_pr_Reversion = datos[37].ToString();
                    item.Cs_pr_Reversion_Anterior = datos[38].ToString();
                    item.Cs_pr_FechaEnvio = datos[39].ToString();
                    item.Cs_pr_FechaRecepcion = datos[40].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Listar retention " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityRetention> cs_pxObtenerFiltroPrincipalReversion(string estadocomprobantesunat, string estadocomprobantescc, string fechainicio, string fechafin)
        {
            List<clsEntityRetention> lista_documentos;
            clsEntityRetention item;
            try
            {
                lista_documentos = new List<clsEntityRetention>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";

                if (estadocomprobantescc != "")
                {
                    sql += " AND cp32 ='" + estadocomprobantescc + "' ";
                }
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp33 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp2 >= '" + fechainicio + "' AND cp2 <= '" + fechafin + "'";
                }
                sql += "AND (cp37 = '' or cp37 is null)";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityRetention(localDB);
                    item.Cs_pr_Retention_id = datos[0].ToString();
                    item.Cs_tag_Id = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_PartyIdentification_Id = datos[3].ToString();
                    item.Cs_tag_PartyIdentificacion_SchemeId = datos[4].ToString();
                    item.Cs_tag_PartyName = datos[5].ToString();
                    item.Cs_tag_PostalAddress_Id = datos[6].ToString();
                    item.Cs_tag_PostalAddress_StreetName = datos[7].ToString();
                    item.Cs_tag_PostalAddress_CitySubdivisionName = datos[8].ToString();
                    item.Cs_tag_PostalAddress_CityName = datos[9].ToString();
                    item.Cs_tag_PostalAddress_CountrySubEntity = datos[10].ToString();
                    item.Cs_tag_PostalAddress_District = datos[11].ToString();
                    item.Cs_tag_PostalAddress_Country_IdentificationCode = datos[12].ToString();
                    item.Cs_tag_PartyLegalEntity_RegistrationName = datos[13].ToString();
                    item.Cs_tag_ReceiveParty_PartyIdentification_Id = datos[14].ToString();
                    item.Cs_tag_ReceiveParty_PartyIdentification_SchemeId = datos[15].ToString();
                    item.Cs_tag_ReceiveParty_PartyName_Name = datos[16].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_Id = datos[17].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_StreetName = datos[18].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName = datos[19].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CityName = datos[20].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_CountrySubentity = datos[21].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_District = datos[22].ToString();
                    item.Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode = datos[23].ToString();
                    item.Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName = datos[24].ToString();
                    item.Cs_tag_SUNATRetentionSystemCode = datos[25].ToString();
                    item.Cs_tag_SUNATRetentionPercent = datos[26].ToString();
                    item.Cs_tag_Note = datos[27].ToString();
                    item.Cs_tag_TotalInvoiceAmount = datos[28].ToString();
                    item.Cs_tag_TotalInvoiceAmount_CurrencyId = datos[29].ToString();
                    item.Cs_tag_TotalPaid = datos[30].ToString();
                    item.Cs_tag_TotalPaid_CurrencyId = datos[31].ToString();
                    item.Cs_pr_EstadoSCC = datos[32].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[33].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[34].ToString();
                    item.Cs_pr_XML = datos[35].ToString();
                    item.Cs_pr_CDR = datos[36].ToString();
                    item.Cs_pr_Reversion = datos[37].ToString();
                    item.Cs_pr_Reversion_Anterior = datos[38].ToString();
                    item.Cs_pr_FechaEnvio = datos[39].ToString();
                    item.Cs_pr_FechaRecepcion = datos[40].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Listar retention " + ex.ToString());
                return null;
            }
        }
        public bool cs_pxEliminarDocumento(string Id)
        {
            bool resultado = false;
            try
            {
                //Eliminar la cabecera y todos sus registros.
                clsEntityRetention Document = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(Id);
                clsEntityRetention_RetentionLine Document_Line = new clsEntityRetention_RetentionLine(localDB);
             
                foreach (var item in Document_Line.cs_fxObtenerTodoPorCabeceraId(Id))
                {          
                    item.cs_pxElimnar(false);
                }
            
                Document.cs_pxElimnar(false);
                resultado = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" clsEntityRetention cs_pxEliminarDocumento " + ex.ToString());
            }
            return resultado;

        }
        public List<List<string>> cs_pxObtenerPendientesEnvio()
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp33 ='2' ";
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
            catch (Exception)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                //clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerPendientesEnvio" + ex.ToString());
                return null;
            }
        }
        public List<clsEntityRetention> cs_pxObtenerDocumentosPorComunicacionBaja_n(string id)
        {
            List<clsEntityRetention> lista_documentos;
            clsEntityRetention item;
            OdbcDataReader datos = null;
            try
            {
                //Buscar todos los documentos en esta comunicación de baja
                string sql = "SELECT * FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE 1=1";
                //string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 ='" + clsBaseUtil.cs_fxComprobantesElectronicos_codigo(comprobantetipo) + "' ";
                sql += " AND cs_VoidedDocuments_Id =" + id + " ";
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila2 = new List<string>();
                lista_documentos = new List<clsEntityRetention>();
                while (datos.Read())//Obtener los Id de los documentos relacionados
                {
                    fila2.Add(datos[7].ToString());
                }
                cs_pxConexion_basedatos.Close();
                if (fila2.Count > 0)//Entonces buscar los documentos y agregarlos a la lista
                {

                    foreach (var idDoc in fila2)
                    {
                        item = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(idDoc);
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
        public List<string> cs_pxObtenerDocumentosPorResumenReversion(string id)
        {
            List<List<string>> tabla_contenidos;
            List<string> retenciones = new List<string>(); 
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
                cs_pxConexion_basedatos.Close();
                if (fila2.Count > 0)//Entonces buscar los documentos y agregarlos a la lista
                {
                   
                    foreach (var item in fila2)
                    {
                        OdbcDataReader datos1 = null;
                        //clsBaseConexion cn1 = new clsBaseConexion();
                        OdbcConnection cs_pxConexion_basedatos1 = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());

                        string sql1 = "SELECT cs_Retention_Id FROM " + cs_cmTabla + " WHERE 1=1";
                        sql1 += " AND cs_Retention_Id =" + item + " ";
                        cs_pxConexion_basedatos1.Open();
                        datos1 = new OdbcCommand(sql1, cs_pxConexion_basedatos1).ExecuteReader();
                        while (datos1.Read())//Devuelve el id de los datos.
                        {
                            retenciones.Add(datos1[0].ToString());
                        }
                        cs_pxConexion_basedatos1.Close();
                    }
                }
               
                return retenciones;
                           
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxObtenerDocumentosPorResumenReversion " + ex.ToString());
                return null;
            }
        }
    }
}
