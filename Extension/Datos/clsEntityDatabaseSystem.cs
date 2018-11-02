using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Extension.Base;
using System.Runtime.InteropServices;
using System.Data.SQLite;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDatabasePrincipal")]
    public class clsEntityDatabaseSystem : clsBaseEntidad
    {
        public string Cs_pr_DatabaseLocal_Id { get; set; }
        public string Cs_pr_DBMS { get; set; }
        public string Cs_pr_DBMSDriver { get; set; }
        public string Cs_pr_DBMSServername { get; set; }
        public string Cs_pr_DBMSServerport { get; set; }
        public string Cs_pr_DBName { get; set; }
        public string Cs_pr_DBUser { get; set; }
        public string Cs_pr_DBPassword { get; set; }
        public string Cs_pr_DBUse { get; set; }

        public clsEntityDatabaseSystem()
        {
            cs_cmTabla = "cs_DatabasePrincipal";
            cs_cmCampos.Add("cs_DatabasePrincipal_Id");
            for (int i = 1; i <= 8; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
        }

        /*public override void cs_pxActualizar(bool obtener_mensaje_respuesta)
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_DatabaseLocal_Id);
            cs_cmValores.Add(Cs_pr_DBMS);
            cs_cmValores.Add(Cs_pr_DBMSDriver);
            cs_cmValores.Add(Cs_pr_DBMSServername);
            cs_cmValores.Add(Cs_pr_DBMSServerport);
            cs_cmValores.Add(Cs_pr_DBName);
            cs_cmValores.Add(Cs_pr_DBUser);
            cs_cmValores.Add(Cs_pr_DBPassword);
            cs_cmValores.Add(Cs_pr_DBUse);
        }*/

        public void cs_pxCrearBaseDatos()
        {
            try
            {
                if (!File.Exists(clsBaseConexionSistema.conexionpath))
                {
                    SQLiteConnection.CreateFile(clsBaseConexionSistema.conexionpath);
                    SQLiteConnection cs_pxConexion_basedatos;
                    cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                    cs_pxConexion_basedatos.Open();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityAlarms().cs_cmTabla + " (" + Insercion(new clsEntityAlarms().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityDatabaseWeb().cs_cmTabla + " (" + Insercion(new clsEntityDatabaseWeb().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityDatabaseLocal().cs_cmTabla + " (" + Insercion(new clsEntityDatabaseLocal().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityAccount().cs_cmTabla + " (" + Insercion(new clsEntityAccount().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityUsers().cs_cmTabla + " (" + Insercion(new clsEntityUsers().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityDeclarant().cs_cmTabla + " (" + Insercion(new clsEntityDeclarant().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityModulo().cs_cmTabla + " (" + Insercion(new clsEntityModulo().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityPermisos().cs_cmTabla + " (" + Insercion(new clsEntityPermisos().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    //Admin
                    new SQLiteCommand("INSERT INTO cs_Users (cs_Users_Id, cp1, cp2, cp3) VALUES ('01', 'admin', 'admin123', 'ADMIN'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Account (cp1, cp2, cp3) VALUES ('01', '', '01'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //Master
                    new SQLiteCommand("INSERT INTO cs_Users (cs_Users_Id, cp1, cp2, cp3) VALUES ('02', 'master', 'master@123', 'MASTER'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Account (cp1, cp2, cp3) VALUES ('02', '', '02'); ", cs_pxConexion_basedatos).ExecuteNonQuery();


                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('01', 'Factura Electrónica','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('02', 'Resumen Diario','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('03', 'Boleta de Venta','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('04', 'Comunicación de Baja','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('05', 'Retención electrónica','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('06', 'Reporte General','Reporte'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('07', 'Envio Sunat','Factura'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('08', 'Formas de envio y alertas','Factura'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('09', 'Generar','Resumen Diario'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('10', 'Envio a Sunat','Resumen Diario'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('11', 'Formas de alerta y envío','Resumen Diario'); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('12', 'Generar','Comunicacion de baja'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('13', 'Envio a Sunat','Comunicacion de baja'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('14', 'Envio de comprobantes','Comprobante de retencion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('15', 'Formas de envio y alerta','Comprobante de retencion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('16', 'Almacen Local','Configuracion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('17', 'Almacen Web','Configuracion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('18', 'Informacion del declarante','Configuracion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('19', 'Usuarios','Configuracion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('20', 'Permisos de usuario','Configuracion'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('21', 'Generacion de Backup','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('22', 'Restauracion de Backup','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('23', 'Ruta de archivos','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('24', 'Actualizacion de estructuras','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('25', 'Activacion de Licencia','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('28', 'Generar reversion - CRE','Resumen de reversión'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('29', 'Envio Sunat y Ticket - CRE ','Resumen de reversión'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('30', 'Validar XML','Receptor de Compras'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('31', 'Registros de compras','Receptor de Compras'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('32', 'Transferencia de Datos','Utilitarios'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('33', 'Certificado Digital','Ayuda'); ", cs_pxConexion_basedatos).ExecuteNonQuery();


                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('01', '01', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('02', '02', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('03', '03', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('04', '04', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('05', '05', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('06', '06', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('07', '07', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('08', '08', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('09', '09', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('10', '10', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('11', '11', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('12', '12', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('13', '13', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('14', '14', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('15', '15', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('16', '16', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('17', '17', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('18', '18', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('19', '19', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('20', '20', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('21', '21', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('22', '22', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('23', '23', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('24', '24', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('25', '25', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('26', '26', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('27', '27', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('28', '28', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('29', '29', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('30', '30', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('31', '31', '01','1'); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    //new SQLiteCommand("INSERT INTO cs_Declarant (cs_Declarant_Id, ruc, clavesol, usuariosol, rutacertificadodigital, email) VALUES ('01', '10440345755', 'MODDATOS', 'moddatos', 'C:/Certificados/certificado_sunat.cer', 'jcaso@contasis.net') ; ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new SQLiteCommand("INSERT INTO cs_Alarms (cs_Alarms_Id, envioautomatico, envioautomatico_minutos, envioautomatico_minutosvalor, envioautomatico_hora, envioautomatico_horavalor, enviomanual, enviomanual_mostrarglobo, enviomanual_mostrarglobo_minutosvalor, enviomanual_nomostrarglobo, iniciarconwindows) VALUES ('01', 'T', 'T', '6', 'F', '" + DateTime.Now.ToString() + "', 'F', 'F', '10', 'T', 'F'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new SQLiteCommand("INSERT INTO cs_DatabaseWeb (cs_DatabaseWeb_Id, cp1, cp2, cp3, cp4, cp5, cp6, cp7, cp8) VALUES ('01', '', '', '', '', '', '', '', '') ; ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new SQLiteCommand("INSERT INTO cs_DatabaseLocal (cs_DatabaseLocal_Id, cp1, cp2, cp3, cp4, cp5, cp6, cp7, cp8) VALUES ('01', '', '', '', '', '', '', '', '') ; ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsg("Base de datos - Advertencia", ex.Message);
            }
        }

        private string Insercion(List<string> campos)
        {
            string i_campos = null;
            foreach (var item in campos)
            {
                i_campos += item + "  VARCHAR(500),";
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }

        private string InsercionXML(List<string> campos, string DBMS, string tabla)
        {
            string tipo = string.Empty;
            string i_campos = null;
            
            foreach (var item in campos)
            {
                switch (tabla)
                {
                    case "cs_Document":
                        if (item == "cp29" || item == "cp30" || item == "cp31")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    tipo = "  MEDIUMTEXT(MAX),";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            i_campos += item + "  VARCHAR(500),";
                        }
                        break;
                    case "cs_SummaryDocuments":
                        if (item == "cp11" || item == "cp12")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    tipo = "  MEDIUMTEXT(MAX),";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            i_campos += item + "  VARCHAR(500),";
                        }
                        break;
                    default:
                        i_campos += item + "  VARCHAR(500),";
                        break;
                }
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }
    }
}
