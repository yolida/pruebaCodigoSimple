using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Runtime.InteropServices;
using System.Data.Odbc;

using System.Data.SQLite;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityUsers")]
    public class clsEntityUsers : clsBaseEntidadSistema
    {
        public string Cs_pr_Users_Id { get; set; }
        public string Cs_pr_User { get; set; }
        public string Cs_pr_Password { get; set; }
        public string Cs_pr_Role_Id { get; set; }

        public clsEntityUsers cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Users_Id = valores[0];
                Cs_pr_User = valores[1];
                Cs_pr_Password = valores[2];
                Cs_pr_Role_Id = valores[3];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityUsers()
        {
            cs_cmTabla = "cs_Users";
            cs_cmCampos.Add("cs_Users_Id");
            cs_cmCampos.Add("cp1");
            cs_cmCampos.Add("cp2");
            cs_cmCampos.Add("cp3");
        }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Users_Id);
            cs_cmValores.Add(Cs_pr_User);
            cs_cmValores.Add(Cs_pr_Password);
            cs_cmValores.Add(Cs_pr_Role_Id);
        }

        public string cs_pxLogin(string user, string password)
        {
            string respuesta = "";
            try
            {
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp1='" + user + "' AND cp2='" + password + "'";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    respuesta = datos[0].ToString();
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityUsers cs_pxLogin" + ex.ToString());
            }
            return respuesta;
        }

        public List<List<string>> cs_pxObtenerTodo()
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityUsers cs_pxObtenerTodo" + ex.ToString());
                return null;
            }
        }
        public List<clsEntityUsers> cs_pxObtenerLista()
        {
            List<clsEntityUsers> usuarios;
            try
            {
                usuarios = new List<clsEntityUsers>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityUsers fila;
                    while (datos.Read())
                    {
                        fila = new clsEntityUsers();
                        fila.Cs_pr_Users_Id = datos[0].ToString();
                        fila.Cs_pr_User= datos[1].ToString();
                        fila.Cs_pr_Password= datos[2].ToString();
                        fila.Cs_pr_Role_Id= datos[3].ToString();
                        usuarios.Add(fila);
                    }
                }
                cs_pxConexion_basedatos.Close();
                return usuarios;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityUsers cs_pxObtenerTodo" + ex.ToString());
                return null;
            }
        }

    }
}
