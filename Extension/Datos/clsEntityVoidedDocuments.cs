using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Data.Odbc;
using Extension.Negocio;
using System.Runtime.InteropServices;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityVoidedDocuments")]
    public class clsEntityVoidedDocuments : clsBaseEntidad
    {
        public string Cs_pr_VoidedDocuments_Id { get; set; }
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
        public string Cs_pr_DocumentoRelacionado { get; set; }
        public string Cs_pr_TipoContenido { get; set; }

        //private clsEntityDatabaseLocal localDB;
        public clsEntityVoidedDocuments(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_VoidedDocuments";
            cs_cmCampos.Add("cs_VoidedDocuments_Id");
            for (int i = 1; i <= 14; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_VoidedDocuments";
            cs_cmCampos_min.Add("cs_VoidedDocuments_Id");
            for (int i = 1; i <= 14; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityVoidedDocuments()
        {
            //localDB = local;
            cs_cmTabla = "cs_VoidedDocuments";
            cs_cmCampos.Add("cs_VoidedDocuments_Id");
            for (int i = 1; i <= 14; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_VoidedDocuments";
            cs_cmCampos_min.Add("cs_VoidedDocuments_Id");
            for (int i = 1; i <= 14; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }

        public clsEntityVoidedDocuments cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_VoidedDocuments_Id = valores[0];
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
                Cs_pr_DocumentoRelacionado = valores[13];
                Cs_pr_TipoContenido = valores[14];
                return this;
            }
            else
            {
                return null;
            }
            
        }

        public int cs_fxCantidadElementos(string id)
        {
            int elementos = 0;
            List<clsEntityVoidedDocuments_VoidedDocumentsLine> VoidedDocuments_VoidedDocumentsLine = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB).cs_fxObtenerTodoPorCabeceraId(id);
            elementos = VoidedDocuments_VoidedDocumentsLine.Count();
            return elementos;
        }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_VoidedDocuments_Id);
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
            cs_cmValores.Add(Cs_pr_DocumentoRelacionado);
            cs_cmValores.Add(Cs_pr_TipoContenido);
        }
       
        public List<clsEntityVoidedDocuments> cs_pxObtenerFiltroPrincipal(string estadocomprobantesunat, string fechainicio, string fechafin,string tipoContiene)
        {
            List<clsEntityVoidedDocuments> lista_documentos;
            clsEntityVoidedDocuments item;
            try
            {
                lista_documentos = new List<clsEntityVoidedDocuments>();
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
                if (tipoContiene != "")
                {
                    sql += "AND cp14 ='"+tipoContiene+"'";
                }
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityVoidedDocuments(localDB);
                    item.Cs_pr_VoidedDocuments_Id = datos[0].ToString();
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
                    item.Cs_pr_DocumentoRelacionado = datos[13].ToString();
                    item.Cs_pr_TipoContenido = datos[14].ToString();
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
        public List<clsEntityVoidedDocuments> cs_pxObtenerFiltroSecundario(string fechainicio, string fechafin, string TipoContiene)
        {
            List<clsEntityVoidedDocuments> lista_documentos;
            clsEntityVoidedDocuments item;
            try
            {
                lista_documentos = new List<clsEntityVoidedDocuments>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";

               // sql += " AND cp9 IN ('0','2','5') ";

                if (fechainicio != "" && fechafin != "")
                {
                    sql += " AND cp3 >= '" + fechainicio + "' AND cp3 <= '" + fechafin + "'";
                }
                if (TipoContiene != "")
                {
                    sql += " AND cp14 ='"+TipoContiene+"'";
                }
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntityVoidedDocuments(localDB);
                    item.Cs_pr_VoidedDocuments_Id = datos[0].ToString();
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
                    item.Cs_pr_DocumentoRelacionado = datos[13].ToString();
                    item.Cs_pr_TipoContenido = datos[14].ToString();
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
        public string cs_pxObtenerDocumentoComuninicacionBajaExisente(string fecha_comunicacion,string tipoContiene)//Fecha a la que pertenece el conjunto de documentos.
        {
            string documento_id = string.Empty;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp8='2' AND cp2='"+ fecha_comunicacion + "' AND cp14='"+tipoContiene+"';";//Estado SCC
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    if (datos[8].ToString()=="2")////Estado SCC == SIN ESTADO
                    {
                        documento_id = datos[0].ToString();
                        break;
                    }
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxObtenerDocumentoComuninicacionBajaExisente" + ex.ToString());
            }
            return documento_id;
        }

        public string cs_pxObtenerDocumentoComuninicacionBajaExisente(string fecha_comunicacion, bool registro_mas_reciente,string tipoContiene)//Fecha a la que pertenece el conjunto de documentos.
        {
            string documento_id = string.Empty;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp8!='2' AND cp2='" + fecha_comunicacion + "' AND cp14='"+tipoContiene+"' ORDER BY cp3 DESC;";//Estado SCC
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                int count = 0;
                while (datos.Read())
                {
                    count++;
                    if (count==1)
                    {
                        documento_id = datos[0].ToString();
                        break;
                    }
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxObtenerDocumentoComuninicacionBajaExisente 2 param " + ex.ToString());
            }
            return documento_id;
        }

        public string cs_fxObtenerCorrelativo(string TipoContiene)
        {
            Int32 correlativo = 1;
            try
            {
                List<string> ras = new List<string>();
                List<int> lista_correlativos = new List<int>();
                //buscar todos los correlativos ordenar ascendente
                OdbcDataReader datos = null;
                //string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp8='2' AND cp3='"+DateTime.Now.ToString("yyyy-MM-dd")+"';";//Estado SCC->coorelativo para bajas
                string sql = "SELECT cp1 FROM " + cs_cmTabla + " WHERE cp3='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND cp14='"+TipoContiene+"' ORDER BY cp1 asc ;";//Estado SCC->coorelativo para bajas
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
                    string parte ="";
                    bool completo = true;
                    foreach (string archivo in ras)
                    {
                        parte = archivo.Split('-')[2];
                        lista_correlativos.Add(Convert.ToInt32(parte));
                    }

                    int max = lista_correlativos.Max();
                    //verificar hasta el numero maximo 
                    for (int i=1; i<max; i++) {

                        bool contiene = lista_correlativos.Contains(i);
                        if (contiene == false) {
                            completo = false;
                            correlativo = i;
                            break;                          
                        }
                    }

                    if (completo == true) {
                        correlativo = max + 1;
                    }
                   

                }
                else {

                    correlativo = 1;
                }


               /* clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                //string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp8='2' AND cp3='"+DateTime.Now.ToString("yyyy-MM-dd")+"';";//Estado SCC->coorelativo para bajas
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp3='"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND cp14='"+TipoContiene+"' ;";//Estado SCC->coorelativo para bajas
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    correlativo++;
                }
                cs_pxConexion_basedatos.Close();*/
                return correlativo.ToString();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_fxObtenerCorrelativo " + ex.ToString());
                return "0";
            }
            
        }

        public clsEntityVoidedDocuments cs_fxObtenerUnoPorTicket(string ticket)
        {
            clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
            Int32 correlativo = 0;
            correlativo++;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp7='"+ticket+"';";//Estado SCC
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                
                while (datos.Read())
                {
                    documento.Cs_pr_VoidedDocuments_Id = datos[0].ToString();
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
                    documento.Cs_pr_DocumentoRelacionado = datos[13].ToString();
                    documento.Cs_pr_TipoContenido = datos[14].ToString();
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_fxObtenerUnoPorTicket" + ex.ToString());
            }
            return documento;
        }

        public bool cs_pxComunicacionBajaNuevo(List<string> items) {
            return true;
        }

        public bool cs_pxComunicacionBajaEliminar(string Id)
        {
            string documento_id = string.Empty;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                string sql1 = "DELETE FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE cs_VoidedDocuments_Id=" + Id + " ;";//Estado SCC
                string sql2 = "DELETE FROM cs_VoidedDocuments WHERE cs_VoidedDocuments_Id= " + Id + " ;";//Estado SCC
              //  clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                new OdbcCommand(sql1, cs_pxConexion_basedatos).ExecuteReader();
                new OdbcCommand(sql2, cs_pxConexion_basedatos).ExecuteReader();
                cs_pxConexion_basedatos.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxComunicacionBajaEliminar" + ex.ToString());
                return false;
            }
        }

        public bool cs_pxValidarMotivosDeBajaEnItems(string Id)
        {
            bool Valido = true;
            try
            {
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE cs_VoidedDocuments_Id=" + Id + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    if (datos[6].ToString() == "")
                    {
                        Valido = false;
                        break;
                    }
                }
                return Valido;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxValidarMotivosDeBajaEnItems" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Actualizar items de comunicacion de baja a documento existente 
        /// Jordy Amaro 09-12-16 FE-906 Actualizacion
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool cs_pxComunicacionBajaActualizar(List<string> Items, string Id,string TipoContiene)
        {
            /**
             * 1.Verificar si ya existe.
             *  Si ya existe, no agregar.
             *  No no agregar.
             */
            string documento_id = string.Empty;
            try
            {
                //Buscar elementos existentes en la comunicación de baja actual.
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE cs_VoidedDocuments_Id="+ Id + " ;";//Estado SCC
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                //Mantener solo los que no sean duplicados.
                List<string> actualizar_estos_items = new List<string>();
                bool agregar;
                Int32 items_total = 0;

                foreach (var Item in Items)
                {
                    agregar = true;
                    while (datos.Read())
                    {
                        items_total++;
                        string sada = datos[7].ToString() + "-" + Item;
                        if (datos[7].ToString() == Item)
                        {
                            agregar = false;
                        }
                    }
                    if (agregar == true)
                    {
                        actualizar_estos_items.Add(Item);
                    }
                }

                if (actualizar_estos_items.Count>0)
                {
                    foreach (var item in actualizar_estos_items)
                    {
                        items_total++;
                        clsEntityVoidedDocuments_VoidedDocumentsLine detalle = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB);

                        if (TipoContiene == "0")
                        {
                            clsEntityDocument documento_cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item);
                            //Jordy Amaro 09-12-16 FE-906
                            //cambio > por <=  para que actualize los documentos a enviarse en comunicacion de baja.
                            //Ini-Modifica
                            if (documento_cabecera.Cs_pr_ComunicacionBaja.Length <= 0)
                            {//Fin-Modifica
                                detalle.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                detalle.Cs_pr_VoidedDocuments_Id = Id;
                                detalle.Cs_tag_LineID = items_total.ToString();
                                detalle.Cs_tag_DocumentTypeCode = documento_cabecera.Cs_tag_InvoiceTypeCode;
                                detalle.Cs_tag_DocumentSerialID = documento_cabecera.Cs_tag_ID.Split('-')[0].ToString();
                                detalle.Cs_tag_DocumentNumberID = documento_cabecera.Cs_tag_ID.Split('-')[1].ToString();
                                detalle.Cs_tag_VoidReasonDescription = "";
                                detalle.Cs_pr_IDDocumentoRelacionado = item;
                                detalle.cs_pxInsertar(false, true);
                                documento_cabecera.Cs_pr_ComunicacionBaja = Id;
                                documento_cabecera.cs_pxActualizar(false, false);
                            }
                        }else if (TipoContiene=="1")
                        {
                            clsEntityRetention documento_cabecera = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(item);
                            //Jordy Amaro 09-12-16 FE-906
                            //cambio > por <=  para que actualize los documentos a enviarse en comunicacion de baja.
                            //Ini-Modifica
                            if (documento_cabecera.Cs_pr_Reversion.Length <= 0)
                            {//Fin-Modifica
                                detalle.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                detalle.Cs_pr_VoidedDocuments_Id = Id;
                                detalle.Cs_tag_LineID = items_total.ToString();
                                detalle.Cs_tag_DocumentTypeCode = "20";
                                detalle.Cs_tag_DocumentSerialID = documento_cabecera.Cs_tag_Id.Split('-')[0].ToString();
                                detalle.Cs_tag_DocumentNumberID = documento_cabecera.Cs_tag_Id.Split('-')[1].ToString();
                                detalle.Cs_tag_VoidReasonDescription = "";
                                detalle.Cs_pr_IDDocumentoRelacionado = item;
                                detalle.cs_pxInsertar(false, true);
                                documento_cabecera.Cs_pr_Reversion = Id;
                                documento_cabecera.cs_pxActualizar(false, false);
                            }
                        }
                       
                    }
                }
                cs_pxConexion_basedatos.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxComunicacionBajaActualizar" + ex.ToString());
                return false;
            }
        }

        //Cristhian|01/03/2018|FEI2-586
        /*Metodo para actuazliar el docuemnto de comunicacion de baja*/
        /*NUEVO INICIO*/
        /// <summary>
        /// Actualizar el documento de comunicacción de baja a documento existente
        /// </summary>
        /// <param name="Id_Documento"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool cs_pxComunicacionBajaActualizar(string Id_Documento, string Id, string TipoContiene)
        {
            /**
             * 1.Verificar si ya existe.
             *  Si ya existe, no agregar.
             *  No no agregar.
             */
            string documento_id = string.Empty;
            try
            {
                //Buscar elementos existentes en la comunicación de baja actual.
                clsEntityVoidedDocuments documento = new clsEntityVoidedDocuments(localDB);
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM cs_VoidedDocuments_VoidedDocumentsLine WHERE cs_VoidedDocuments_Id=" + Id + " ;";//Estado SCC
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                //Mantener solo los que no sean duplicados.
                List<string> actualizar_estos_items = new List<string>();
                bool agregar;
                Int32 items_total = 0;

                agregar = true;
                while (datos.Read())
                {
                    items_total++;
                    string sada = datos[7].ToString() + "-" + Id_Documento;
                    if (datos[7].ToString() == Id_Documento)
                    {
                        agregar = false;
                    }
                }
                if (agregar == true)
                {
                    actualizar_estos_items.Add(Id_Documento);
                }

                if (actualizar_estos_items.Count > 0)
                {
                    foreach (var item in actualizar_estos_items)
                    {
                        items_total++;
                        clsEntityVoidedDocuments_VoidedDocumentsLine detalle = new clsEntityVoidedDocuments_VoidedDocumentsLine(localDB);

                        if (TipoContiene == "0")
                        {
                            clsEntityDocument documento_cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(item);
                            //Jordy Amaro 09-12-16 FE-906
                            //cambio > por <=  para que actualize los documentos a enviarse en comunicacion de baja.
                            //Ini-Modifica
                            if (documento_cabecera.Cs_pr_ComunicacionBaja.Length <= 0)
                            {//Fin-Modifica
                                detalle.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                detalle.Cs_pr_VoidedDocuments_Id = Id;
                                detalle.Cs_tag_LineID = items_total.ToString();
                                detalle.Cs_tag_DocumentTypeCode = documento_cabecera.Cs_tag_InvoiceTypeCode;
                                detalle.Cs_tag_DocumentSerialID = documento_cabecera.Cs_tag_ID.Split('-')[0].ToString();
                                detalle.Cs_tag_DocumentNumberID = documento_cabecera.Cs_tag_ID.Split('-')[1].ToString();
                                detalle.Cs_tag_VoidReasonDescription = "";
                                detalle.Cs_pr_IDDocumentoRelacionado = item;
                                detalle.cs_pxInsertar(false, true);
                                documento_cabecera.Cs_pr_ComunicacionBaja = Id;
                                documento_cabecera.cs_pxActualizar(false, false);
                            }
                        }
                        else if (TipoContiene == "1")
                        {
                            clsEntityRetention documento_cabecera = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(item);
                            //Jordy Amaro 09-12-16 FE-906
                            //cambio > por <=  para que actualize los documentos a enviarse en comunicacion de baja.
                            //Ini-Modifica
                            if (documento_cabecera.Cs_pr_Reversion.Length <= 0)
                            {//Fin-Modifica
                                detalle.Cs_pr_VoidedDocuments_VoidedDocumentsLine_Id = Guid.NewGuid().ToString();
                                detalle.Cs_pr_VoidedDocuments_Id = Id;
                                detalle.Cs_tag_LineID = items_total.ToString();
                                detalle.Cs_tag_DocumentTypeCode = "20";
                                detalle.Cs_tag_DocumentSerialID = documento_cabecera.Cs_tag_Id.Split('-')[0].ToString();
                                detalle.Cs_tag_DocumentNumberID = documento_cabecera.Cs_tag_Id.Split('-')[1].ToString();
                                detalle.Cs_tag_VoidReasonDescription = "";
                                detalle.Cs_pr_IDDocumentoRelacionado = item;
                                detalle.cs_pxInsertar(false, true);
                                documento_cabecera.Cs_pr_Reversion = Id;
                                documento_cabecera.cs_pxActualizar(false, false);
                            }
                        }

                    }
                }
                cs_pxConexion_basedatos.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityVoidedDocuments cs_pxComunicacionBajaActualizar" + ex.ToString());
                return false;
            }
        }
        /*NUEVO FIN*/
    }
}
