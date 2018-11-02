using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityModulo")]
    public class clsEntityModulo : clsBaseEntidadSistema
    {
        public string Cs_pr_Modulo_Id { get; set; }
        public string Cs_pr_Modulo { get; set; }
        public string Cs_pr_Modulo_Padre { get; set; }

        public clsEntityModulo cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Modulo_Id = valores[0];
                Cs_pr_Modulo = valores[1];
                Cs_pr_Modulo_Padre = valores[2];
              
                return this;
            }
            else
            {
                return null;
            }

        }
        public void cs_pxEliminarModulos()
        {
            try
            {
                string sql = "DELETE  FROM " + cs_cmTabla + "; ";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteNonQuery();
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityModulo eliminarmod " + ex.ToString());
            }
        }

        public clsEntityModulo()
        {
            cs_cmTabla = "cs_Modulo";
            cs_cmCampos.Add("cs_Modulo_Id");
            cs_cmCampos.Add("modulo");
            cs_cmCampos.Add("padre");
        }
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Modulo_Id);
            cs_cmValores.Add(Cs_pr_Modulo);
            cs_cmValores.Add(Cs_pr_Modulo_Padre);
        }
        public List<clsEntityModulo> cs_pxObtenerTodo()
        {
            List<clsEntityModulo> comprobante_detalle;
            try
            {
                comprobante_detalle = new List<clsEntityModulo>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityModulo entidad;
                    while (datos.Read())
                    {
                        entidad = new clsEntityModulo();
                        entidad.Cs_pr_Modulo_Id = datos[0].ToString();
                        entidad.Cs_pr_Modulo = datos[1].ToString();
                        entidad.Cs_pr_Modulo_Padre = datos[2].ToString();                    
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
                clsBaseLog.cs_pxRegistarAdd(" clsEntityModulo cs_pxObtenerTodo " + ex.ToString());
                return null;
            }
        }
    }
}
