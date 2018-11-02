using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityAccount")]
    public class clsEntityAccount : clsBaseEntidadSistema
    {
        public string Cs_pr_Account_Id { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }
        public string Cs_pr_Users_Id { get; set; }
        /// <summary>
        /// Metodo para obtener una cuenta segun Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto clsEntityAccount</returns>
        public clsEntityAccount cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Account_Id = valores[0];
                Cs_pr_Declarant_Id = valores[1];
                Cs_pr_Users_Id = valores[2];
                return this;
            }
            else
            {
                return null;
            }           
        }

        public clsEntityAccount()
        {
            cs_cmTabla = "cs_Account";
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
        }
        /// <summary>
        /// Actualizacion de entidad cuenta.
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Account_Id);
            cs_cmValores.Add(Cs_pr_Declarant_Id);
            cs_cmValores.Add(Cs_pr_Users_Id);
        }
        /// <summary>
        /// Obtener Lista de cuentas asociadas a un usuario.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Lista de cuentas</returns>
        public List<clsEntityAccount> dgvEmpresasUsuario(string Id)
        {
            List<clsEntityAccount> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<clsEntityAccount>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM cs_Account WHERE cp3 ='" + Id.ToString().Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityAccount fila;
                while (datos.Read())
                {
                    fila = new clsEntityAccount();
                    fila.Cs_pr_Account_Id = datos[0].ToString();
                    fila.Cs_pr_Declarant_Id = datos[1].ToString();
                    fila.Cs_pr_Users_Id = datos[2].ToString();
                    tabla_contenidos.Add(fila);
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityAccount dgvEmpresasUsuario " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Obtener Lista de cuentas asociadas a un usuario.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Lista de cuentas</returns>
        public List<clsEntityAccount> cs_pxListaUsuariosPorEmpresa(string IdEmpresa)
        {
            List<clsEntityAccount> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<clsEntityAccount>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM cs_Account WHERE cp2 ='" + IdEmpresa.ToString().Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityAccount fila;
                while (datos.Read())
                {
                    fila = new clsEntityAccount();
                    fila.Cs_pr_Account_Id = datos[0].ToString();
                    fila.Cs_pr_Declarant_Id = datos[1].ToString();
                    fila.Cs_pr_Users_Id = datos[2].ToString();
                    tabla_contenidos.Add(fila);
                }
                cs_pxConexion_basedatos.Close();
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                //clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityAccount listusers " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Obtener si ya existe una cuenta con la misma empresa asociada.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="empresa"></param>
        /// <returns>True or false para existe o no existe cuenta respectivamente</returns>
        public bool dgvCuentaDuplicada(string usuario, string empresa)
        {
            try
            {
                int count = 0;
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM cs_Account WHERE cp3 ='" + usuario.ToString().Trim() + "' AND cp2='" + empresa + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    count++;
                }
                cs_pxConexion_basedatos.Close();
                if (count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityCuenta dgvCuentaDuplicada " + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Obtiene el identificador de la asociacion de usuario y declarante.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="declarantid"></param>
        /// <returns>Id del identificador</returns>
        public string dgvVerificarCuenta(string userid, string declarantid)
        {
            string account_id = "";
            try
            {
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM cs_Account WHERE cp3 ='" + userid.ToString().Trim() + "' AND  cp2 ='" + declarantid.ToString().Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                
                while (datos.Read())
                {
                    account_id = datos[0].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return account_id;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityCuenta dgvVerificarCuenta " + ex.ToString());
                return account_id;
            }
        }
        /// <summary>
        /// Realiza la eliminacion de una asociacion de usuario a empresa.
        /// </summary>
        /// <param name="cs_pr_Declarant_Id"></param>
        public void cs_pxEliminarCuentasAsociadasEMPRESA(string cs_pr_Declarant_Id)
        {
            try
            {
                SQLiteDataReader datos = null;
                string sql = "DELETE FROM cs_Account WHERE cp2 ='" + cs_pr_Declarant_Id.ToString().Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityCuenta cs_pxEliminarCuentasAsociadasEMPRESA " + ex.ToString());
            }
        }
        /// <summary>
        /// Realiza la eliminacion de una asociacion de empresa a usuario.
        /// </summary>
        /// <param name="cs_pr_UsersId"></param>
        public void cs_pxEliminarCuentasAsociadasUSUARIO(string cs_pr_UsersId)
        {
            try
            {
                SQLiteDataReader datos = null;
                string sql = "DELETE FROM cs_Account WHERE cp3 ='" + cs_pr_UsersId.ToString().Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityCuenta cs_pxEliminarCuentasAsociadasUSUARIO " + ex.ToString());
            }
        }

    }
}
