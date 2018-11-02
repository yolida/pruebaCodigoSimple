using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityPermisos")]
    public class clsEntityPermisos: clsBaseEntidadSistema
    {
        public string Cs_pr_Permisos_Id { get; set; }
        public string Cs_pr_Modulo { get; set; }
        public string Cs_pr_Cuenta { get; set; }
        public string Cs_pr_Permitido { get; set; }


        public clsEntityPermisos cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Permisos_Id = valores[0];
                Cs_pr_Modulo = valores[1];
                Cs_pr_Cuenta = valores[2];
                Cs_pr_Permitido = valores[3];
                return this;
            }
            else
            {
                return null;
            }

        }

        public clsEntityPermisos()
        {
            cs_cmTabla = "cs_Permisos";
            cs_cmCampos.Add("cs_Permisos_Id");
            cs_cmCampos.Add("modulo");
            cs_cmCampos.Add("cuenta");
            cs_cmCampos.Add("permitido");
        }
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Permisos_Id);
            cs_cmValores.Add(Cs_pr_Modulo);
            cs_cmValores.Add(Cs_pr_Cuenta);
            cs_cmValores.Add(Cs_pr_Permitido);
        }
        public List<clsEntityPermisos> cs_pxObtenerTodo()
        {
            List<clsEntityPermisos> comprobante_detalle;
            try
            {
                comprobante_detalle = new List<clsEntityPermisos>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityPermisos entidad;
                    while (datos.Read())
                    {
                        entidad = new clsEntityPermisos();
                        entidad.Cs_pr_Permisos_Id = datos[0].ToString();
                        entidad.Cs_pr_Modulo = datos[1].ToString();
                        entidad.Cs_pr_Cuenta = datos[2].ToString();
                        entidad.Cs_pr_Permitido = datos[3].ToString();
                        comprobante_detalle.Add(entidad);
                    }
                }
                cs_pxConexion_basedatos.Close();
                return comprobante_detalle;
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityMPermisos cs_pxObtenerTodo " + ex.ToString());
                return null;
            }
        }
        public void cs_pxEliminarPermisos(string IdCuenta)
        {
            try
            {                           
                string sql = "DELETE FROM " + cs_cmTabla + " WHERE cuenta='" + IdCuenta + "'";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteNonQuery();
                cs_pxConexion_basedatos.Close();
               
            }
            catch (Exception ex)
            {
               
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityMPermisos eliminarper " + ex.ToString());
            }
        }
        public static bool accesoPermitido(string codigo_modulo,string cuenta, string user)
        {
            bool retorno = false;
            clsEntityUsers Usuario = new clsEntityUsers().cs_pxObtenerUnoPorId(user);
            if (Usuario != null)
            {
                if (Usuario.Cs_pr_Role_Id.ToString().ToUpper() == "ADMIN" || Usuario.Cs_pr_Role_Id.ToString().ToUpper()=="MASTER")
                {
                    return true;
                }
            }
            List<clsEntityPermisos> permisos= new clsEntityPermisos().cs_pxObtenerPorCuenta(cuenta);
            if (permisos != null && permisos.Count > 0)
            {
                foreach (clsEntityPermisos item in permisos)
                {
                    if (item.Cs_pr_Modulo == codigo_modulo)
                    {
                        if (item.Cs_pr_Permitido == "1")
                        {
                            retorno = true;
                        }                      
                        break;
                    }
                }
            }
            return retorno;
        }
        public clsEntityPermisos cs_pxObtenerPorCodigoCuenta(string codigo_modulo, string IdCuenta)
        {
            clsEntityPermisos retorno = null;
            try
            {
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cuenta='" + IdCuenta + "' and modulo='"+codigo_modulo+"' ;";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                   
                    while (datos.Read())
                    {
                        retorno = new clsEntityPermisos();
                        retorno.Cs_pr_Permisos_Id = datos[0].ToString();
                        retorno.Cs_pr_Modulo = datos[1].ToString();
                        retorno.Cs_pr_Cuenta = datos[2].ToString();
                        retorno.Cs_pr_Permitido = datos[3].ToString();
                    }
                }
                cs_pxConexion_basedatos.Close();
            }
            catch
            {

            }

            return retorno;
        }
        public List<clsEntityPermisos> cs_pxObtenerPorCuenta(string IdCuenta)
        {
            List<clsEntityPermisos> comprobante_detalle=null;
            try
            {
                comprobante_detalle = new List<clsEntityPermisos>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cuenta='"+ IdCuenta + "' ;";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityPermisos entidad;
                    while (datos.Read())
                    {
                        entidad = new clsEntityPermisos();
                        entidad.Cs_pr_Permisos_Id = datos[0].ToString();
                        entidad.Cs_pr_Modulo = datos[1].ToString();
                        entidad.Cs_pr_Cuenta = datos[2].ToString();
                        entidad.Cs_pr_Permitido = datos[3].ToString();
                        comprobante_detalle.Add(entidad);
                    }
                }
                cs_pxConexion_basedatos.Close();
                return comprobante_detalle;
            }
            catch (Exception ex)
            {
               
                    // System.Windows.Forms.MessageBox.Show(ex.ToString());
                    // clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                    clsBaseLog.cs_pxRegistarAdd(" clsEntityMPermisos cs_pxObtenerTodo " + ex.ToString());
                return null;
            }
        }
       
    }
}
