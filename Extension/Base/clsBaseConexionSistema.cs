using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using Extension.Base;
using System.Data.SQLite;
using System.IO;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseConexionSistema")]
    public class clsBaseConexionSistema
    {
        private clsBaseConfiguracion cs_cmConfiguracion;
        private SQLiteConnection cs_cmConexion;
        //public static string conexionstring = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FEI\Sistema" + "\\FEI.data.dll" + "; Version=3;";
        //public static string conexionpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FEI\Sistema" + "\\FEI.data.dll";
        public static string conexionstring = "Data Source=" + get() + @"\FEI\Sistema" + "\\FEI.data.dll" + "; Version=3;";
        public static string conexionpath = get() + @"\FEI\Sistema" + "\\FEI.data.dll";
        public clsBaseConexionSistema()
        {
            cs_cmConfiguracion = new clsBaseConfiguracion();
            if (!File.Exists(conexionpath)) {
                SQLiteConnection.CreateFile(conexionpath);
            }
            cs_cmConexion = new SQLiteConnection(conexionstring);
        }

        public static string get()
        {
            string ruta = new clsRegistry().Read("RUTA");         
            if (ruta == null)
            {
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            return ruta;
        }

        /// <summary>
        /// Metodo para obtener un registro por clave primaria de tabla en base de datos.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="id"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        /// <returns>Lista con los valores del registro consultado</returns>
        public List<string> cs_fxObtenerUnoPorId(string tabla, List<string> campos, string id, bool obtener_mensaje_respuesta)

        {
            List<string> valores = new List<string>();
            try
            {
                string sql = "SELECT * FROM " + tabla + " WHERE " + campos[0] + " = '" + id + "'";
                cs_cmConexion.Open();

                SQLiteDataReader odr = new SQLiteCommand(sql, cs_cmConexion).ExecuteReader();
                while (odr.Read())
                {                
                    for (int i = 0; i < odr.FieldCount; i++)
                    {
                        valores.Add(odr[i].ToString().Trim());
                    }
                }
                cs_cmConexion.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_fxObtenerUnoPorId " + ex.ToString());
            }
            return valores;
        }
        /// <summary>
        /// Metodo para actualizar un registro de base de datos segun Id.Opcional mensaje de confirmacion.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxActualizar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            try
            {
                string sql = "";
                for (int i = 1; i < campos.Count; i++)
                {
                    sql += " " + campos[i] + "='" + valores[i].Replace("'", "''") + "',";

                }
                sql = "UPDATE " + tabla + " SET " + sql.Substring(1, sql.Length - 2) + " WHERE " + campos[0] + "='" + valores[0] + "';";
                cs_cmConexion.Open();
                new SQLiteCommand(sql, cs_cmConexion).ExecuteReader();
                cs_cmConexion.Close();
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE5");
                }
            }
            catch (Exception ex)
            {
               // System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseMensaje.cs_pxMsgEr("ERR5", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_pxActualizar " + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para eliminar un registro de base de datos.Opcional mensaje de confirmacion.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxEliminar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            try
            {
                string sql = "DELETE FROM " + tabla + " WHERE " + campos[0] + "='" + valores[0] + "';";
                cs_cmConexion.Open();
                new SQLiteCommand(sql, cs_cmConexion).ExecuteReader();
                cs_cmConexion.Close();
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE7");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR7", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_pxEliminar " + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para insertar un registro en base de datos. Opcional mensaje de confirmacion.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxInsertar(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            try
            {
                string sql = "", sql_campos = "", sql_valores = "";

                for (int i = 0; i < campos.Count; i++)
                {
                    sql_campos += " " + campos[i] + ",";
                    sql_valores += " '" + valores[i] + "',";
                }

                sql = "INSERT INTO " + tabla + " (" + sql_campos.Substring(1, sql_campos.Length - 2) + ") VALUES (" + sql_valores.Substring(1, sql_valores.Length - 2) + ");";
                cs_cmConexion.Open();
                new SQLiteCommand(sql, cs_cmConexion).ExecuteReader();
                cs_cmConexion.Close();
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE5");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR5", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_pxInsertar " + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para obtener todos los registros de una tabla.
        /// </summary>
        /// <param name="tabla"></param>
        /// <param name="campos"></param>
        /// <param name="valores"></param>
        /// <param name="obtener_mensaje_respuesta"></param>
        /// <returns>Lista de listas con los registros obtenidos</returns>
        public List<List<string>> cs_fxSeleccionartodo(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + tabla;
                cs_cmConexion.Open();
                datos = new SQLiteCommand(sql, cs_cmConexion).ExecuteReader();
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
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_fxSeleccionartodo " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para obtener el estado de conexion SQLite.
        /// </summary>
        /// <returns>True / False </returns>
        public bool cs_fxConexionEstado()
        {
            SQLiteConnection cn = new SQLiteConnection(cs_cmConexion);
            bool respuesta = false;
            try
            {
                cn.Open();
                respuesta = true;
            }
            catch (Exception ex)
            {
                //clsBaseLog.cs_pxRegistar(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConexionSistema cs_fxConexionEstado " + ex.ToString());
                respuesta = false;
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
            return respuesta;
        }
    }
}
