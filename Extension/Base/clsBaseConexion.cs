using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using Extension.Base;
using Extension.Datos;
//using FEI.Usuario;


namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseConexion")]
    public class clsBaseConexion
    {
        private OdbcConnection cs_cmConexion=null;
        private string prConexioncadenabasedatos="";
        private string prConexioncadenaservidor="";
        private string dbms="";

        public clsBaseConexion()
        {

        }
        public clsBaseConexion(clsEntityDatabaseLocal bdlocal)
        {
            //cs_cmConfiguracion = new clsBaseConfiguracion();
            //cs_cmConexion = new OdbcConnection(cs_prConexioncadenabasedatos);
            if (bdlocal != null)
            {
                prConexioncadenabasedatos = bdlocal.cs_prConexioncadenabasedatos();
                prConexioncadenaservidor  = bdlocal.cs_prConexioncadenaservidor();
                dbms = bdlocal.Cs_pr_DBMS;
                cs_cmConexion = new OdbcConnection(prConexioncadenabasedatos);
            } 
        }
        /// <summary>
        /// Metodo que retorna la cadena de conexion del servidor para ODBC.
        /// </summary>
       /* public string cs_prConexioncadenaservidor
        {
            get
            {
                string cadena = "";
                switch (cs_cmConfiguracion.cs_prDbms)
                {
                    case "MySQL":
                        cadena = "DRIVER={" + cs_cmConfiguracion.cs_prDbmsdriver + "};SERVER=" + cs_cmConfiguracion.cs_prDbmsservidor + ";PORT=" + cs_cmConfiguracion.cs_prDbmsservidorpuerto + ";USER=" + cs_cmConfiguracion.cs_prDbusuario + ";PASSWORD=" + cs_cmConfiguracion.cs_prDbclave + ";OPTION=3;";
                        break;
                    case "Microsoft SQL Server":
                        cadena = "Driver={" + cs_cmConfiguracion.cs_prDbmsdriver + "};Server=" + cs_cmConfiguracion.cs_prDbmsservidor + "," + cs_cmConfiguracion.cs_prDbmsservidorpuerto + ";Uid=" + cs_cmConfiguracion.cs_prDbusuario + ";Pwd=" + cs_cmConfiguracion.cs_prDbclave + ";";
                        break;
                    case "SQLite":
                        cadena = "DRIVER={" + cs_cmConfiguracion.cs_prDbmsdriver + "}; Database=" + cs_cmConfiguracion.cs_prRutainstalacion + ".dbc\\" + cs_cmConfiguracion.cs_prDbnombre + "; LongNames=0; Timeout=1000; NoTXN=0; SyncPragma=NORMAL; StepAPI=0;";
                        break;
                }
                return cadena;
            }
        }
        /// <summary>
        /// Metodo que retorna la cadena de conexion a base de datos en ODBC.
        /// </summary>
        public string cs_prConexioncadenabasedatos
        {
            get
            {
                string cadena = "";
                switch (cs_cmConfiguracion.cs_prDbms)
                {
                    case "MySQL":
                        cadena = "DRIVER={" + cs_cmConfiguracion.cs_prDbmsdriver + "};SERVER=" + cs_cmConfiguracion.cs_prDbmsservidor + ";PORT=" + cs_cmConfiguracion.cs_prDbmsservidorpuerto + ";DATABASE=" + cs_cmConfiguracion.cs_prDbnombre + ";USER=" + cs_cmConfiguracion.cs_prDbusuario + ";PASSWORD=" + cs_cmConfiguracion.cs_prDbclave + ";OPTION=3;";
                        break;
                    case "Microsoft SQL Server":
                        cadena = "Driver={" + cs_cmConfiguracion.cs_prDbmsdriver + "};Server=" + cs_cmConfiguracion.cs_prDbmsservidor + "," + cs_cmConfiguracion.cs_prDbmsservidorpuerto + ";Database=" + cs_cmConfiguracion.cs_prDbnombre + ";Uid=" + cs_cmConfiguracion.cs_prDbusuario + ";Pwd=" + cs_cmConfiguracion.cs_prDbclave + ";";
                        break;
                    case "SQLite":
                        cadena = "DRIVER={" + cs_cmConfiguracion.cs_prDbmsdriver + "}; Database=" + cs_cmConfiguracion.cs_prRutainstalacion + "\\" + cs_cmConfiguracion.cs_prDbnombre + ".dbc" + "; LongNames=0; Timeout=1000; NoTXN=0; SyncPragma=NORMAL; StepAPI=0;";
                        break;
                }
                return cadena;
            }
        }*/
        /// <summary>
        /// Metodo para obtener un registro de base de datos.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="id"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        /// <returns>Listado de valores del registro</returns>
        public List<string> cs_fxObtenerUnoPorId(string tabla, List<string> campos, string id, bool obtener_mensaje_respuesta)
        {
            List<string> valores = new List<string>();
            try
            {
                string sql = "SELECT * FROM " + tabla + " WHERE " + campos[0] + " =" + id + " ";
                cs_cmConexion.Open();
                OdbcDataReader odr = new OdbcCommand(sql, cs_cmConexion).ExecuteReader();
                while (odr.Read())
                {
                    for (int i = 0; i < campos.Count; i++)
                    {
                        valores.Add(odr[i].ToString().Trim());
                    }   
                }
                cs_cmConexion.Close();
            }
            catch (Exception ex)
            {
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                }            
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_fxObtenerUnoPorId " + tabla + " " + ex.ToString());
            }
            return valores;
        }
        /* Jorge Luis | 14/06/2018 | FEI2-752
         * Método para obtener los valores de una fila*/
        public List<string> GetFieldValuesById(string tabla, string idFieldName, string id)
        {
            List<string> values = new List<string>();
            try
            {
                string query = "SELECT TOP 1 * FROM " + tabla + " WHERE " + idFieldName + " = '" + id + "' ";
                cs_cmConexion.Open();
                OdbcDataReader reader = new OdbcCommand(query, cs_cmConexion).ExecuteReader();
                while (reader.Read())
                {
                    values.Add(reader.ToString().Trim());
                }
                cs_cmConexion.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_fxObtenerUnoPorId " + tabla + " " + ex.ToString());
            }
            return values;
        }
        /// <summary>
        /// Metodo para actualizar un registro de la base de datos, opcional muestra mensaje de confirmacion.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public bool cs_pxActualizar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta, bool tiene_foreignkey)
        {
            bool retorno = false;
            try
            {
                string sql = "";
                for (int i = 1; i < campos.Count; i++)
                {
                    if (i == 1)
                    {
                        if (tiene_foreignkey == true)
                        {
                            sql += " " + campos[i] + "=" + valores[i].Replace("'", "''") + ",";
                        }
                        else
                        {
                            sql += " " + campos[i] + "='" + valores[i].Replace("'", "''") + "',";
                        }
                       
                    }
                    else
                    {
                        sql += " " + campos[i] + "='" + valores[i].Replace("'", "''") + "',";
                    }
                   

                }
                sql = "UPDATE " + tabla + " SET " + sql.Substring(1, sql.Length - 2) + " WHERE " + campos[0] + "=" + valores[0] + " ;";
                cs_cmConexion.Open();
                int afectado = new OdbcCommand(sql, cs_cmConexion).ExecuteNonQuery();
                cs_cmConexion.Close();

                if (afectado == 1)
                {
                    retorno = true;
                }
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE5");
                }
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR5", ex.ToString());
                }
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_pxActualizar " + tabla + " " + ex.ToString());
            }
            return retorno;
        }
        /// <summary>
        /// Metodo para eliminar un registro de base de datos.Opcional mostrar mensaje de exito de operacion.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public bool cs_pxEliminar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            bool retorno = false;
            try
            {
                string sql = "DELETE FROM " + tabla + " WHERE " + campos[0] + "=" + valores[0] + " ;";
                cs_cmConexion.Open();
                int afectado = new OdbcCommand(sql, cs_cmConexion).ExecuteNonQuery();
                cs_cmConexion.Close();
                if (afectado == 1)
                {
                    retorno = true;
                }
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE7");
                }

            }
            catch (Exception ex)
            {
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR7", ex.ToString());
                }              
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_pxEliminar " + tabla + " " + ex.ToString());
            }
            return retorno;
        }
        /// <summary>
        /// Metodo para insertar un registro en base de datos.Opcional mensaje de confirmacion de resultado.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public string cs_pxInsertar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta,bool tiene_foreignkey)
        {
            string idRetorno = "";
            bool grabar = true;
            try
            {
                string sql = "", sql1 = "", sql_campos = "", sql_valores = "";
                /*if(tabla== "cs_Document")
                {
                    //verificar existencia o no del documento
                    clsBaseLog.cs_pxRegistarAdd(valores[1]+" f ");
                    bool existe = new clsEntityDocument().cs_pxBuscarDocumentoPorSerieNumero(valores[1], prConexioncadenabasedatos);
                    if (existe == false)
                    {
                        grabar = true;
                        clsBaseLog.cs_pxRegistarAdd("grabar true ");
                    }
                    else
                    {
                        grabar = false;
                        clsBaseLog.cs_pxRegistarAdd("grabar false ");
                    }
                }*/
                if (grabar == true)
                {
                    for (int i = 1; i < campos.Count; i++)
                    {
                        sql_campos += " " + campos[i] + ",";

                        if (i == 1)
                        {
                            if (tiene_foreignkey == true)
                            {
                                sql_valores += " " + valores[i] + ",";
                            }
                            else
                            {
                                sql_valores += " '" + valores[i] + "',";
                            }
                        }
                        else
                        {
                            sql_valores += " '" + valores[i] + "',";
                        }

                    }
                    /*old*/
                    /* if (dbms == "SQLite")
                     {
                         sql = "INSERT INTO " + tabla + " (" + sql_campos.Substring(1, sql_campos.Length - 2) + ") VALUES (" + sql_valores.Substring(1, sql_valores.Length - 2) + "); SELECT last_insert_rowid()";
                     }*/
                    /*enold*/
                    if (dbms == "SQLite")
                    {
                        sql = "INSERT INTO " + tabla + " (" + sql_campos.Substring(1, sql_campos.Length - 2) + ") VALUES (" + sql_valores.Substring(1, sql_valores.Length - 2) + ");";
                        sql1 = "SELECT last_insert_rowid();";
                    }
                    else
                    {
                        sql = "INSERT INTO " + tabla + " (" + sql_campos.Substring(1, sql_campos.Length - 2) + ") VALUES (" + sql_valores.Substring(1, sql_valores.Length - 2) + "); SELECT SCOPE_IDENTITY()";
                    }

                    //clsBaseLog.cs_pxRegistarAdd(sql);
                    cs_cmConexion.Open();
                    /*old*/
                    //var afectado = new OdbcCommand(sql, cs_cmConexion).ExecuteScalar();  
                    /**/
                    object y = null;
                    var afectado = y;
                    if (dbms == "SQLite")
                    {
                        new OdbcCommand(sql, cs_cmConexion).ExecuteNonQuery();
                        afectado = new OdbcCommand(sql1, cs_cmConexion).ExecuteScalar();
                    }
                    else
                    {
                        afectado = new OdbcCommand(sql, cs_cmConexion).ExecuteScalar();
                    }
                    cs_cmConexion.Close();
                    if (afectado != null)
                    {
                        idRetorno = afectado.ToString();
                    }

                    if (obtener_mensaje_respuesta)
                    {
                        clsBaseMensaje.cs_pxMsgOk("OKE5");
                    }
                }
                else
                {
                    clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_pxInsertar :" + tabla + " " + campos[1]+" El doc ya se encuentra registrado.");   
                    //MessageBox.Show("El documento ya se encuentra grabado.", "Excepcion", MessageBoxButtons.OK, MessageBoxIcon.Warning);                    
                }               
            }
            catch (Exception ex)
            {

                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR5", ex.ToString());
                }
                
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_pxInsertar :" + tabla + " " + ex.ToString());
            }

            return idRetorno;
        }
        /// <summary>
        /// Metodo para obtener todos los registros de una tabla en base de datos.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        /// <returns>Lista de listas de registros.</returns>
        public List<List<string>> cs_fxSeleccionartodo(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + tabla;
                cs_cmConexion.Open();
                datos = new OdbcCommand(sql, cs_cmConexion).ExecuteReader();
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
                cs_cmConexion.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                }                
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion cs_fxSeleccionartodo :" + tabla + " " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para obtener el estado de conexion con el servidor.
        /// </summary>
        /// <returns>TRUE / FALSE segun sea el caso para activo/inactivo</returns>
        public bool cs_fxConexionEstadoServidor()
        {
            OdbcConnection cnserver = null;

            if (dbms == "SQLite")
            {
                cnserver = new OdbcConnection(prConexioncadenabasedatos);
            }
            else if(dbms == "MySQL")
            {
                cnserver = new OdbcConnection(prConexioncadenabasedatos);
            }
            else
            {
                cnserver = new OdbcConnection(prConexioncadenaservidor);
            }
            bool respuesta = false;
            try
            {
                cnserver.ConnectionTimeout = 1;
                cnserver.Open();
                respuesta = true;
            }
            catch (Exception es)
            {
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionServidor: conexionEstado->" + es.ToString());
                respuesta = false;
            }
            finally
            {
                cnserver.Close();
                cnserver.Dispose();
            }

            return respuesta;
        }
        /// <summary>
        /// Metodo para obtener el estado de conexion a base de datos.
        /// </summary>
        /// <returns>True/False para activo e inactivo respectivamente</returns>
        public bool cs_fxConexionEstado()
        {
            OdbcConnection conection = new OdbcConnection(prConexioncadenabasedatos);
            
            bool respuesta = false;
            try
            {
                conection.ConnectionTimeout = 3;
                conection.Open();
                respuesta = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexion: conexionEstado->"+ex.ToString());
                respuesta = false;
            }
            finally
            {
                conection.Close();
                conection.Dispose();
            }
            return respuesta;
        }
    }
}
