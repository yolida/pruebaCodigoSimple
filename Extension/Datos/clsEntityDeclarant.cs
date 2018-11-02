using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using System.Data.SQLite;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDeclarant")]
    public class clsEntityDeclarant:clsBaseEntidadSistema
    {
        public string Cs_pr_Declarant_Id            { get; set; }
        public string Cs_pr_Ruc                     { get; set; }
        public string Cs_pr_Clavesol                { get; set; }
        public string Cs_pr_Usuariosol              { get; set; }
        public string Cs_pr_Rutacertificadodigital  { get; set; }
        public string Cs_pr_Email                   { get; set; }
        public string Cs_pr_Parafrasiscertificadodigital    { get; set; }
        public string Cs_pr_RazonSocial             { get; set; }
        public string Cs_pr_Alerta                  { get; set; }
        public string Cs_pr_Alerta_Dias             { get; set; }

        public clsEntityDeclarant cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Declarant_Id = valores[0];
                Cs_pr_Ruc = valores[1];
                Cs_pr_Clavesol = valores[2];
                Cs_pr_Usuariosol = valores[3];
                Cs_pr_Rutacertificadodigital = valores[4];
                Cs_pr_Email = valores[5];
                Cs_pr_Parafrasiscertificadodigital = valores[6];
                Cs_pr_RazonSocial = valores[7];
                if (valores.Count > 8) { 
                   Cs_pr_Alerta = valores[8];
                   Cs_pr_Alerta_Dias = valores[9];
                }
                return this;
            }
            else
            {
                return null;
            }  
        }

        public clsEntityDeclarant()
        {
            cs_cmTabla = "cs_Declarant";
            cs_cmCampos.Add("cs_Declarant_Id");
            cs_cmCampos.Add("ruc");
            cs_cmCampos.Add("clavesol");
            cs_cmCampos.Add("usuariosol");
            cs_cmCampos.Add("rutacertificadodigital");
            cs_cmCampos.Add("email");
            cs_cmCampos.Add("parafrasiscertificadodigital");
            cs_cmCampos.Add("razonsocial");
            cs_cmCampos.Add("alerta");
            cs_cmCampos.Add("alerta_dias");
        }

        public void cs_pxInsertarACuenta(bool obtener_mensaje_respuesta)
        {
            cs_pxActualizarEntidad();
            new clsBaseConexionSistema().cs_pxInsertar(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);

            clsEntityAccount account = new clsEntityAccount
            {
                Cs_pr_Account_Id = Guid.NewGuid().ToString(),
                Cs_pr_Declarant_Id = Cs_pr_Declarant_Id,
                Cs_pr_Users_Id = "01"
            };
            account.cs_pxInsertar(false);

            clsEntityDatabaseLocal bdlocal = new clsEntityDatabaseLocal
            {
                Cs_pr_DatabaseLocal_Id = Guid.NewGuid().ToString(),
                Cs_pr_DBMS = "Microsoft SQL Server",
                Cs_pr_DBMSDriver = "SQL Server",
                Cs_pr_DBMSServername = "SERVERNAME_REGISTERS",
                Cs_pr_DBMSServerport = "1433",
                Cs_pr_DBName = "cs_bdfei",
                Cs_pr_DBPassword = "CLAVE",
                Cs_pr_DBUse = "T",
                Cs_pr_DBUser = "USUARIO",
                Cs_pr_Declarant_Id = Cs_pr_Declarant_Id
            };
            bdlocal.cs_pxInsertar(false);

            clsEntityDatabaseWeb bdweb = new clsEntityDatabaseWeb
            {
                Cs_pr_DatabaseWeb_Id = Guid.NewGuid().ToString(),
                Cs_pr_DBMS = "Microsoft SQL Server",
                Cs_pr_DBMSDriver = "SQL Server",
                Cs_pr_DBMSServername = "SERVERNAME_WEBPUBLICATION",
                Cs_pr_DBMSServerport = "1433",
                Cs_pr_DBName = "cs_bdfei_web",
                Cs_pr_DBPassword = "CLAVE",
                Cs_pr_DBUse = "T",
                Cs_pr_DBUser = "USUARIO",
                Cs_pr_Declarant_Id = Cs_pr_Declarant_Id
            };
            bdweb.cs_pxInsertar(false);

            clsEntityAlarms alarms = new clsEntityAlarms
            {
                Cs_pr_Alarms_Id = Guid.NewGuid().ToString(),
                Cs_pr_Declarant_Id = Cs_pr_Declarant_Id,
                Cs_pr_Envioautomatico = "T",
                Cs_pr_Envioautomatico_hora = "T",
                Cs_pr_Envioautomatico_horavalor = DateTime.Now.ToString(),
                Cs_pr_Envioautomatico_minutos = "F",
                Cs_pr_Envioautomatico_minutosvalor = "6",
                Cs_pr_Enviomanual = "F",
                Cs_pr_Enviomanual_mostrarglobo = "F",
                Cs_pr_Enviomanual_mostrarglobo_minutosvalor = "10",
                Cs_pr_Enviomanual_nomostrarglobo = "T",
                Cs_pr_Iniciarconwindows = "F"
            };
            alarms.cs_pxInsertar(false);
        }

        public void cs_pxEliminarDeCuenta(bool obtener_mensaje_respuesta)
        {
            cs_pxActualizarEntidad();
            new clsBaseConexionSistema().cs_pxEliminar(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);
            new clsEntityDatabaseWeb().cs_fxObtenerUnoPorDeclaranteId(Cs_pr_Declarant_Id).cs_pxElimnar(false);
            new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(Cs_pr_Declarant_Id).cs_pxElimnar(false);
            new clsEntityAlarms().cs_fxObtenerUnoPorDeclaranteId(Cs_pr_Declarant_Id,"").cs_pxElimnar(false);
            new clsEntityAccount().cs_pxEliminarCuentasAsociadasEMPRESA(Cs_pr_Declarant_Id);
        }


        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Declarant_Id);
            cs_cmValores.Add(Cs_pr_Ruc);
            cs_cmValores.Add(Cs_pr_Clavesol);
            cs_cmValores.Add(Cs_pr_Usuariosol);
            cs_cmValores.Add(Cs_pr_Rutacertificadodigital);
            cs_cmValores.Add(Cs_pr_Email);
            cs_cmValores.Add(Cs_pr_Parafrasiscertificadodigital);
            cs_cmValores.Add(Cs_pr_RazonSocial);
            cs_cmValores.Add(Cs_pr_Alerta);
            cs_cmValores.Add(Cs_pr_Alerta_Dias);
        }

        public List<clsEntityDeclarant> cs_pxObtenerTodo()
        {
            List<clsEntityDeclarant> comprobante_detalle;
            try
            {
                comprobante_detalle = new List<clsEntityDeclarant>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityDeclarant entidad;
                    while (datos.Read())
                    {
                        entidad = new clsEntityDeclarant();
                        entidad.Cs_pr_Declarant_Id = datos[0].ToString();
                        entidad.Cs_pr_Ruc = datos[1].ToString();
                        entidad.Cs_pr_Clavesol = datos[2].ToString();
                        entidad.Cs_pr_Usuariosol = datos[3].ToString();
                        entidad.Cs_pr_Rutacertificadodigital = datos[4].ToString();
                        entidad.Cs_pr_Email = datos[5].ToString();
                        entidad.Cs_pr_Parafrasiscertificadodigital = datos[6].ToString();
                        entidad.Cs_pr_RazonSocial = datos[7].ToString();
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
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDeclarant cs_pxObtenerTodo " + ex.ToString());
                return null;
            }
        }
        public List<clsEntityDeclarant> cs_pxObtenerTodos()
        {
            List<clsEntityDeclarant> comprobante_detalle;
            try
            {
                comprobante_detalle = new List<clsEntityDeclarant>();
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + ";";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    clsEntityDeclarant entidad;
                    while (datos.Read())
                    {
                        entidad = new clsEntityDeclarant();
                        entidad.Cs_pr_Declarant_Id = datos[0].ToString();
                        entidad.Cs_pr_Ruc = datos[1].ToString();
                        entidad.Cs_pr_Clavesol = datos[2].ToString();
                        entidad.Cs_pr_Usuariosol = datos[3].ToString();
                        entidad.Cs_pr_Rutacertificadodigital = datos[4].ToString();
                        entidad.Cs_pr_Email = datos[5].ToString();
                        entidad.Cs_pr_Parafrasiscertificadodigital = datos[6].ToString();
                        entidad.Cs_pr_RazonSocial = datos[7].ToString();
                        entidad.Cs_pr_Alerta = datos[8].ToString();
                        entidad.Cs_pr_Alerta_Dias = datos[9].ToString();
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
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDeclarant cs_pxObtenerTodo " + ex.ToString());
                return null;
            }
        }
        public clsEntityDeclarant cs_pxObtenerPorRuc(string ruc)
        {         
            try
            {
                clsEntityDeclarant entidad = null;
                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " where ruc='"+ruc+"';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                if (datos.HasRows)
                {
                    while (datos.Read())
                    {
                        entidad = new clsEntityDeclarant();
                        entidad.Cs_pr_Declarant_Id = datos[0].ToString();
                        entidad.Cs_pr_Ruc = datos[1].ToString();
                        entidad.Cs_pr_Clavesol = datos[2].ToString();
                        entidad.Cs_pr_Usuariosol = datos[3].ToString();
                        entidad.Cs_pr_Rutacertificadodigital = datos[4].ToString();
                        entidad.Cs_pr_Email = datos[5].ToString();
                        entidad.Cs_pr_Parafrasiscertificadodigital = datos[6].ToString();
                        entidad.Cs_pr_RazonSocial = datos[7].ToString();
                        entidad.Cs_pr_Alerta = datos[8].ToString();
                        entidad.Cs_pr_Alerta_Dias = datos[9].ToString();
                    }
                    
                }
                cs_pxConexion_basedatos.Close();
                if (entidad == null)
                {
                    return null;
                }
                else{
                    return entidad;
                }
               
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show(ex.ToString());
               // clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDeclarant cs_pxObtenerUnoPorRuc " + ex.ToString());
                return null;
            }
        }
    }
}
