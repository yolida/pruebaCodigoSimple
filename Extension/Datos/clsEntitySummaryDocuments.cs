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
    [ProgId("clsEntitySummaryDocuments")]
    public class clsEntitySummaryDocuments : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_Id { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_ReferenceDate { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_AdditionalAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_pr_Ticket { get; set; }
        public string Cs_pr_EstadoSCC { get; set; }
        public string Cs_pr_EstadoSUNAT { get; set; }
        public string Cs_pr_ComentarioSUNAT { get; set; }
        public string Cs_pr_XML { get; set; }
        public string Cs_pr_CDR { get; set; }
        public string Cs_pr_ExceptionSUNAT { get; set; }
        public string Cs_tag_DocumentCurrencyCode { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyName_Name { get; set; } //FE-1085 PartyName NombreTributo Comercial
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_Id);
            cs_cmValores.Add(Cs_tag_ID);
            cs_cmValores.Add(Cs_tag_ReferenceDate);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_AdditionalAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_pr_Ticket);
            cs_cmValores.Add(Cs_pr_EstadoSCC);
            cs_cmValores.Add(Cs_pr_EstadoSUNAT);
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);
            cs_cmValores.Add(Cs_pr_XML);
            cs_cmValores.Add(Cs_pr_CDR);
            cs_cmValores.Add(Cs_pr_ExceptionSUNAT);
            cs_cmValores.Add(Cs_tag_DocumentCurrencyCode);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyName_Name);
        }

        public clsEntitySummaryDocuments(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments";
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 15; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_SummaryDocuments";
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 15; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntitySummaryDocuments()
        {
            //localDB = local;
            cs_cmTabla = "cs_SummaryDocuments";
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 15; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_SummaryDocuments";
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 15; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntitySummaryDocuments cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_Id = valores[0];
                Cs_tag_ID = valores[1];
                Cs_tag_ReferenceDate = valores[2];
                Cs_tag_IssueDate = valores[3];
                Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = valores[4];
                Cs_tag_AccountingSupplierParty_AdditionalAccountID = valores[5];
                Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = valores[6];
                Cs_pr_Ticket = valores[7];
                Cs_pr_EstadoSCC = valores[8];
                Cs_pr_EstadoSUNAT = valores[9];
                Cs_pr_ComentarioSUNAT = valores[10];
                Cs_pr_XML = valores[11];
                Cs_pr_CDR = valores[12];
                Cs_pr_ExceptionSUNAT = valores[13];
                Cs_tag_DocumentCurrencyCode = valores[14];
                Cs_tag_AccountingSupplierParty_Party_PartyName_Name = valores[15];
                return this;
            }
            else
            {
                return null;
            }
           
        }
        public clsEntitySummaryDocuments cs_fxObtenerUnoPorTicket(string ticket)
        {
            clsEntitySummaryDocuments documento = new clsEntitySummaryDocuments(localDB);
            Int32 correlativo = 0;
            correlativo++;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp7='" + ticket + "';";//Estado 
               // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                while (datos.Read())
                {
                    documento.Cs_pr_SummaryDocuments_Id = datos[0].ToString();
                    documento.Cs_tag_ID = datos[1].ToString();
                    documento.Cs_tag_ReferenceDate = datos[2].ToString();
                    documento.Cs_tag_IssueDate = datos[3].ToString();
                    documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[4].ToString();
                    documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[5].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[6].ToString();
                    documento.Cs_pr_Ticket = datos[7].ToString();
                    documento.Cs_pr_EstadoSCC = datos[8].ToString();
                    documento.Cs_pr_EstadoSUNAT = datos[9].ToString();
                    documento.Cs_pr_ComentarioSUNAT = datos[10].ToString();
                    documento.Cs_pr_XML = datos[11].ToString();
                    documento.Cs_pr_CDR = datos[12].ToString();
                    documento.Cs_pr_ExceptionSUNAT = datos[13].ToString();
                    documento.Cs_tag_DocumentCurrencyCode = datos[14].ToString();
                    documento.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[15].ToString();
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerUnoPorTicket" + ex.ToString());
            }
            return documento;
        }
        public string cs_fxObtenerResumenIdPorFechaReferencia_PENDIENTE(string fecha, string fecha_reference)
        {
            string tabla_contenidos=string.Empty;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT cs_SummaryDocuments_Id FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + fecha + "' ";
                sql += " AND cp2 = '" + fecha_reference + "' ";
                sql += " AND cp8 = '2' ";
                sql += " AND cp9 = '2' ";
                // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos=datos[0].ToString();
                }
                cs_pxConexion_basedatos.Close();
               
            }
            catch (Exception ex)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaReferencia_PENDIENTES " + ex.ToString());
                tabla_contenidos = string.Empty;
            }
            return tabla_contenidos;
        }
        /// <summary>
        /// Obtener los ID de resúmenes diarios de boletas de un determinado día (PENDIENTES DE ENVÍO).
        /// </summary>
        /// <param name="fecha">Fecha en formato: YYYY-MM-DD (Reference Date)</param>
        /// <returns>Lista de id de resumenes diarios pendientes de envío.</returns>
        public List<string> cs_fxObtenerResumenesIdPorFechaReferencia_PENDIENTES(string fecha, string fecha_reference)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp2 = '" + fecha_reference + "' ";
                sql += " AND cp8 = '2' ";
                sql += " AND cp9 = '2' ";
               // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaReferencia_PENDIENTES " + ex.ToString());
                return null;
            }
        }
        public List<string> cs_fxObtenerResumenesIdPorFechaReferenciaEnvio_PENDIENTES(string fecha, string fecha_reference)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp2 = '" + fecha_reference + "' ";
                sql += " AND cp8 = '2' ";
                sql += " AND cp9 = '2' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaReferenciaEnvio_PENDIENTES" + ex.ToString());
                return null;
            }
        }
        public List<string> cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES(string fecha, string fecha_reference)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                //sql += " AND cp2 = '" + fecha_reference + "' ";
                sql += " AND cp8 = '2' ";
                sql += " AND cp9 = '2' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaEnvio_PENDIENTES" + ex.ToString());
                return null;
            }
        }
        //obtener id resumen del da para fecha de referencia
        public List<string> cs_fxObtenerIdResumenDiaFechaReferencia(string fecha, string fecha_referencia)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp2 = '" + fecha_referencia + "' ";
                sql += " AND cp8 = '0' ";
                sql += " AND cp9 = '0' ";
                sql += " ORDER BY cp3 DESC , cp1 DESC ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerIdResumenDiaFechaReferencia" + ex.ToString());
                return null;
            }
        }
        //validar si existe resumen del dia para la fecha de referencia
        public bool cs_fxValidarResumenDiaFechaReferencia(string fecha, string fecha_referencia)
        {
            bool resultado = false;
            try
            {

                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp2 = '" + fecha_referencia + "' ";
                sql += " AND cp8 = '0' ";
                sql += " AND cp9 = '0' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    resultado = true;
                }
                cs_pxConexion_basedatos.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxValidarResumenDiaFechaReferencia" + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Obtener los ID de resúmenes diarios de boletas de un determinado día. (ENVIADOS)
        /// </summary>
        /// <param name="fecha">Fecha en formato: YYYY-MM-DD (Reference Date)</param>
        /// <returns>Lista de id de resumenes diarios pendientes de envío.</returns>
        public List<string> cs_fxObtenerResumenesIdPorFechaReferencia_ENVIADOS(string fecha, string fecha_referencia)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                sql += " AND cp2 = '" + fecha_referencia + "' ";
                sql += " AND cp8 = '0' ";
                sql += " AND cp9 = '0' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaReferencia_ENVIADOS" + ex.ToString());
                return null;
            }
        }
        public List<string> cs_fxObtenerResumenesIdPorFechaEnvio_ENVIADOS(string fecha, string fecha_referencia)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                //sql += " AND cp2 = '" + fecha_referencia + "' ";
                sql += " AND cp8 = '0' ";
                sql += " AND cp9 != '2' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaReferencia_ENVIADOS" + ex.ToString());
                return null;
            }
        }

        public List<string> cs_fxObtenerResumenesIdPorFechaEnvio_TOTAL(string fecha)
        {
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
                // sql += " AND cp3 = '" + fecha_referencia + "' ";
                // sql += " AND cp8 = '0' ";
                // sql += " AND cp9 = '0' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    tabla_contenidos.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesIdPorFechaEnvio_TOTAL" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Obtener los resúmenes diarios de boletas en un rango de fechas.
        /// </summary>
        /// <param name="fecha_inicio">Fecha en formato: YYYY-MM-DD (Reference Date)</param>
        /// <param name="fecha_fin">Fecha en formato: YYYY-MM-DD (Reference Date)</param>
        /// <returns>Lista de de resumenes diarios</returns>
        public List<List<string>> cs_fxObtenerResumenesPorRangoFecha(string fecha_inicio, string fecha_fin)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp2 >= '" + fecha_inicio + "' ";
                sql += " AND cp2 <= '" + fecha_fin + "' ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila;
                while (datos.Read())
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
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenesPorRangoFecha" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Obtener la cantidad de items de resumen.
        /// </summary>
        /// <param name="Id">Id del Resumen</param>
        /// <returns>Número de items</returns>
        public Int64 cs_fxObtenerResumenNumeroItems(string Id)
        {
            Int64 filas = 0;
            List<string> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<string>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_SummaryDocuments_SummaryDocumentsLine WHERE 1=1";
                sql += " AND cs_SummaryDocuments_Id = " + Id + " ";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    filas++;
                }
                cs_pxConexion_basedatos.Close();

                return filas;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_fxObtenerResumenNumeroItems" + ex.ToString());
                return 0;
            }
        }
        /// <summary>
        /// Metodo para liberar resumenes diarios.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool cs_pxLiberarSustitutorioDocumento(string Id)
        {
            bool retorno = false;
            try
            {
                //buscar resumenes con el id y poner en liberados; cambiar estado de sunat y scc -> agregar el ultimo valor de resumen a campo 42 para tener quienes eran del anterior resumen.
                List<List<string>> docs_anteriores = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario(Id);
                clsEntityDocument doc;
                foreach (var item in docs_anteriores)
                {
                    //Cristhian|28/12/2017|FEI2-325
                    /*Se modifica esta parte del codigo ya que cuando libera los ducmentos asosciados a un resumen
                      diario, los comprobantes que tenian el estado de baja cambian a pendiuente correcto*/
                    /*INICIO MODIFICACIóN*/
                    /*Entonces si el comprobante no esta de baja en este caso un codigo diferente de 3*/
                    if (item[28]!="3")
                    {
                        /*se aplican los siguinetes cambios a sus valores, esto afecta a la tabla cs_Document*/
                        doc = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item[0].ToString().Trim());
                        doc.Cs_pr_EstadoSCC = "2";
                        doc.Cs_pr_EstadoSUNAT = "2";
                        doc.Cs_ResumenUltimo_Enviado = doc.Cs_pr_Resumendiario;
                        doc.Cs_pr_Resumendiario = "";
                        doc.cs_pxActualizar(false, false);
                        retorno = true;
                    }
                    /*Si el comprobante esta de baja en este caso es el codigo 3*/
                    else if (item[28] == "3")
                    {
                        /*No se modifica los estados del docuento y se dajan tal cual estan registrados en la 
                          base de datos*/
                        doc = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item[0].ToString().Trim());
                        doc.Cs_ResumenUltimo_Enviado = doc.Cs_pr_Resumendiario;
                        doc.Cs_pr_Resumendiario = "";
                        doc.Cs_pr_EstadoSCC = "3";
                        doc.Cs_pr_EstadoSUNAT = "0";
                        doc.cs_pxActualizar(false, false);
                        retorno = true;
                    }
                    /*FIN MODIFICACIóN*/
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_pxLiberarSustitutorioDocumento" + ex.ToString());
            }

            return retorno;
        }
        public void cs_pxEliminarDocumento(string Id)
        {
            try
            {
                //actualizar relacion y estado en tabla document.
                List<List<string>> registros = new clsEntityDocument(localDB).cs_pxObtenerPorResumenDiario(Id);
                clsEntityDocument doc;
                foreach (var item in registros)
                {
                    doc = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item[0].ToString().Trim());
                    //doc.Cs_pr_EstadoSCC = "2";
                    //doc.Cs_pr_EstadoSUNAT = "2";
                    doc.Cs_pr_Resumendiario = "";
                    doc.cs_pxActualizar(false,false);
                }

                //eliminar de summary documents y los relacionados:
                clsEntitySummaryDocuments Summary = new clsEntitySummaryDocuments(localDB).cs_fxObtenerUnoPorId(Id);
                clsEntitySummaryDocuments_Notes Summary_Notes = new clsEntitySummaryDocuments_Notes(localDB);//eliminado
                clsEntitySummaryDocuments_SummaryDocumentsLine Summary_Line = new clsEntitySummaryDocuments_SummaryDocumentsLine(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge Summary_Line_Allowance_Charge = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment Summary_Line_Billing_Payment = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal Summay_Line_Tax_Total = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localDB);

                foreach (var item in Summary_Notes.cs_fxObtenerTodoPorSummaryId(Id))
                {
                    item.cs_pxElimnar(false);
                }
                foreach (var item in Summary_Line.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    foreach (var item1 in Summary_Line_Billing_Payment.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item1.cs_pxElimnar(false);
                    }

                    foreach (var item2 in Summary_Line_Allowance_Charge.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item2.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Summay_Line_Tax_Total.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    item.cs_pxElimnar(false);
                }

                Summary.cs_pxElimnar(false);
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_pxEliminarDocumento " + ex.ToString());
            }
        }

        //Cristhian|16/04/2018|FEI2-585
        /*Creado para limpiar los items del resumen diario cuando se activa liberar documentos, ya que se detecto que
          los items del resumen diario se estaban duplicando generando error en el envio del resumen diario*/
        /*INICIO MODIFICACIóN*/
        public void cs_pxEliminarDocumento(string Id, bool LiberarDocumentos)
        {
            try
            {
                clsEntitySummaryDocuments_Notes Summary_Notes = new clsEntitySummaryDocuments_Notes(localDB);//eliminado
                clsEntitySummaryDocuments_SummaryDocumentsLine Summary_Line = new clsEntitySummaryDocuments_SummaryDocumentsLine(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge Summary_Line_Allowance_Charge = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment Summary_Line_Billing_Payment = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localDB);
                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal Summay_Line_Tax_Total = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localDB);

                foreach (var item in Summary_Notes.cs_fxObtenerTodoPorSummaryId(Id))
                {
                    item.cs_pxElimnar(false);
                }
                foreach (var item in Summary_Line.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    foreach (var item1 in Summary_Line_Billing_Payment.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item1.cs_pxElimnar(false);
                    }

                    foreach (var item2 in Summary_Line_Allowance_Charge.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item2.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Summay_Line_Tax_Total.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    item.cs_pxElimnar(false);
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments cs_pxEliminarDocumento " + ex.ToString());
            }
        }
        /*FIN MODIFICACIóN*/

        public List<clsEntitySummaryDocuments> cs_pxObtenerFiltroPrincipal(string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            List<clsEntitySummaryDocuments> lista_documentos;
            clsEntitySummaryDocuments item;
            try
            {
                lista_documentos = new List<clsEntitySummaryDocuments>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";

                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp9 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp3 >= '" + fechainicio + "' AND cp3 <= '" + fechafin + "'";
                }
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntitySummaryDocuments(localDB);
                    item.Cs_pr_SummaryDocuments_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_ReferenceDate = datos[2].ToString();
                    item.Cs_tag_IssueDate = datos[3].ToString();
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[4].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[5].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[6].ToString();
                    item.Cs_pr_Ticket = datos[7].ToString();
                    item.Cs_pr_EstadoSCC = datos[8].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[9].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[10].ToString();
                    item.Cs_pr_XML = datos[11].ToString();
                    item.Cs_pr_CDR = datos[12].ToString();
                    item.Cs_pr_ExceptionSUNAT = datos[13].ToString();
                    item.Cs_tag_DocumentCurrencyCode = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[15].ToString();
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
        //
        public List<clsEntitySummaryDocuments> cs_pxObtenerFiltroSecundario(string estadocomprobantesunat, string fechainicio, string fechafin)
        {
            List<clsEntitySummaryDocuments> lista_documentos;
            clsEntitySummaryDocuments item;
            try
            {
                lista_documentos = new List<clsEntitySummaryDocuments>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                //sql += " AND cp9 IN ('2','4','5') ";
                if (estadocomprobantesunat != "")
                {
                    sql += " AND cp9 ='" + estadocomprobantesunat + "' ";
                }
                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp3 >= '" + fechainicio + "' AND cp3 <= '" + fechafin + "'";
                }
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntitySummaryDocuments(localDB);
                    item.Cs_pr_SummaryDocuments_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_ReferenceDate = datos[2].ToString();
                    item.Cs_tag_IssueDate = datos[3].ToString();
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[4].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[5].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[6].ToString();
                    item.Cs_pr_Ticket = datos[7].ToString();
                    item.Cs_pr_EstadoSCC = datos[8].ToString();
                    item.Cs_pr_EstadoSUNAT = datos[9].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[10].ToString();
                    item.Cs_pr_XML = datos[11].ToString();
                    item.Cs_pr_CDR = datos[12].ToString();
                    item.Cs_pr_ExceptionSUNAT = datos[13].ToString();
                    item.Cs_tag_DocumentCurrencyCode = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[15].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("cs_pxObtenerFiltroSecundario " + ex.ToString());
                return null;
            }
        }
        public List<List<string>> cs_pxObtenerPendientesEnvioRD()
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sql += " AND cp8 ='2' AND cp9 ='2' AND cp3 = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> fila;
                while (datos.Read())
                {
                    List<clsEntitySummaryDocuments_SummaryDocumentsLine> items = new clsEntitySummaryDocuments_SummaryDocumentsLine(localDB).cs_fxObtenerTodoPorCabeceraId(datos[0].ToString());
                    //Devuelve los datos en el mismo orden que aparecen en la tabla. Considerar al llenar el datagridview.
                    if (items.Count > 0)
                    {
                        fila = new List<string>();
                        for (int i = 0; i < datos.FieldCount; i++)
                        {
                            fila.Add(datos[i].ToString().Trim());
                        }
                        tabla_contenidos.Add(fila);
                    }

                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception)
            {
                // clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                return null;
            }
        }
        public string cs_fxObtenerCorrelativo(string diaIssueDate)
        {
            Int32 correlativo = 1;
            try
            {
                List<string> ras = new List<string>();
                List<int> lista_correlativos = new List<int>();
                //buscar todos los correlativos ordenar ascendente
                OdbcDataReader datos = null;
                string sql = "SELECT cp1 FROM " + cs_cmTabla + " WHERE cp3='" + diaIssueDate + "' ORDER BY cp1 asc ;";//Estado SCC->coorelativo para bajas
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    ras.Add(datos[0].ToString());
                }
                cs_pxConexion_basedatos.Close();

                if (ras.Count > 0)
                {
                    string parte = "";
                    bool completo = true;
                    foreach (string archivo in ras)
                    {
                        parte = archivo.Split('-')[2];
                        lista_correlativos.Add(Convert.ToInt32(parte));
                    }

                    int max = lista_correlativos.Max();
                    //verificar hasta el numero maximo 
                    for (int i = 1; i < max; i++)
                    {
                        bool contiene = lista_correlativos.Contains(i);
                        if (contiene == false)
                        {
                            completo = false;
                            correlativo = i;
                            break;
                        }
                    }

                    if (completo == true)
                    {
                        correlativo = max + 1;
                    }
                }
                else
                {
                    correlativo = 1;
                }
                return correlativo.ToString();
            }
            catch (Exception ex)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsSummaryDocuments cs_fxObtenerCorrelativo " + ex.ToString());
                return "0";
            }
        }
    }
}
