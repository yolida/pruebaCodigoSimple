using Extension.Datos;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseConfiguracion")]
    public class clsBaseConfiguracion
    {
        private const string cs_cnLlave = "EuIZ7jvl€7Tsb~BVk#lfKrVH8lNTjimVb9kuWVh(2?yIFXIZ(MlQu]GeE27slae0~UR8}Bat$nlw¿O8dcrW4LXrM[s!6f¡{}xd1r1Gvbkfs~P%?9&~fBffBY$gVFfk@¡]LYdils#9#gLRrKFt€he2MIgmMfvvE]ww!dP&~kRxoo$%gv~@YRe(ISLn¡0YozbYdE@8pA)Ab¡Y(zU=gg€eFH¡gux)&{ed005coyv07Yc7pjld¿CmZ?Fh~Qs8oo{hdnGwmk~lFInjKm[h23~20rhDJ1$wDd=3%WV(0QLgQu[8NixAYZikWl5qxY%u#n9YCn%S}!6tcfYEBAj€PGW08nyE)rGZa?QqPY6&4P1p@Qf96osVEzjO=I@fiMN@NljS~p&&WMNMl79RuZOlaair¡tqrFd~w8rcujV¡}¿Ow#}Cc9jz&€m3AM]9un)iSnuWgJqQ%xn€BACY%e$HA$abyccrt8imZOhnoRxEVcDSCEd82mxEEadX$TcUd";

        private const char cs_cnArchivoconfiguracion_separador = '|';

        private string cs_cmArchivoconfiguracion;

        public string cs_cmLog_sistema, cs_cmLog_dll , cs_cmLog_sistema_add;
        public string cs_prRutainstalacion { get; set; }
        public string cs_prRutareportesPDF { get; set; }
        public string cs_prRutareportesCSV { get; set; }
        public string cs_prRutadocumentosenvio { get; set; }
        public string cs_prRutadocumentosrecepcion { get; set; }
        public string cs_prRutaCargaTextoPlano { get; set; }
        public string cs_prDbms { get; set; }
        public string cs_prDbmsdriver { get; set; }
        public string cs_prDbmsservidor { get; set; }
        public string cs_prDbmsservidorpuerto { get; set; }
        public string cs_prDbnombre { get; set; }
        public string cs_prDbusuario { get; set; }
        public string cs_prDbclave { get; set; }
        public string cs_prActivacioncodigo { get; set; }
        public string cs_prLoginUsuario { get; set; }
        public string cs_prLoginPassword { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }

        public clsBaseConfiguracion()
        {
            string ruta = new clsRegistry().Read("RUTA");          
            if (ruta == null)
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            cs_prRutainstalacion            = ruta + @"\FEI\Sistema";
            cs_prRutareportesPDF            = ruta + @"\FEI\Reportes\PDF";
            cs_prRutareportesCSV            = ruta + @"\FEI\Reportes\CSV";
            cs_prRutadocumentosenvio        = ruta + @"\FEI\Documentos_Envio";
            cs_prRutadocumentosrecepcion    = ruta + @"\FEI\Documentos_Recepcion";
            cs_prRutaCargaTextoPlano        = ruta + @"\FEI\Documentos_Carga";
            string cs_prRutaBackUp          = ruta + @"\FEI\Sistema\BackUp";
            try
            {
                cs_cmArchivoconfiguracion   = cs_prRutainstalacion + "\\" + "fei.config";
                cs_cmLog_sistema            = cs_prRutainstalacion + "\\" + "fei_sistema.log";
                cs_cmLog_sistema_add        = cs_prRutainstalacion + "\\" + "fei_sistema_add.log";
                cs_cmLog_dll                = cs_prRutainstalacion + "\\" + "fei_dll.log";
                if (!Directory.Exists(cs_prRutainstalacion))
                {
                    Directory.CreateDirectory(cs_prRutadocumentosenvio);
                    Directory.CreateDirectory(cs_prRutadocumentosrecepcion);
                    Directory.CreateDirectory(cs_prRutainstalacion);
                    Directory.CreateDirectory(cs_prRutaBackUp).Attributes = FileAttributes.Hidden;
                    GrantAccess(cs_prRutainstalacion);
                    Directory.CreateDirectory(cs_prRutareportesPDF);
                    Directory.CreateDirectory(cs_prRutareportesCSV);
                    Directory.CreateDirectory(cs_prRutaCargaTextoPlano);
                    File.Create(cs_cmArchivoconfiguracion).Close();

                    //ejecutar el archivo exe para el codigo de peticion
                    string fileExecutable = Environment.CurrentDirectory + "\\Srlc.exe";
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = fileExecutable;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.Close();
                }
                else if (!File.Exists(cs_cmArchivoconfiguracion))
                {
                    File.Create(cs_cmArchivoconfiguracion).Close();
                    //ejecutar el archivo exe para el codigo de peticion
                    string  fileExecutable = Environment.CurrentDirectory + "\\Srlc.exe";
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = fileExecutable;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.Close();
                }
               /* StreamReader sr0 = new StreamReader(cs_cmArchivoconfiguracion);
                string configuracionbasedatos = sr0.ReadLine() + "";
                if (configuracionbasedatos.Trim().Length != 0)
                {
                    configuracionbasedatos = clsBaseCifrado.cs_fxDescifrar(configuracionbasedatos, cs_cnLlave);
                    string[] configuracion = configuracionbasedatos.Split(cs_cnArchivoconfiguracion_separador);
                    if (configuracion.Length == 15)
                    {
                        //cs_prRutainstalacion = configuracion[0];
                        //cs_prRutadocumentosenvio = configuracion[1];
                        //cs_prRutadocumentosrecepcion = configuracion[2];
                        cs_prDbms = configuracion[3];
                        cs_prDbmsdriver = configuracion[4];
                        cs_prDbmsservidor = configuracion[5];
                        cs_prDbmsservidorpuerto = configuracion[6];
                        cs_prDbnombre = configuracion[7];
                        cs_prDbusuario = configuracion[8];
                        cs_prDbclave = configuracion[9];
                        cs_prActivacioncodigo = configuracion[10];
                        cs_prLoginUsuario = configuracion[11];
                        cs_prLoginPassword = configuracion[12];
                        Cs_pr_Declarant_Id = configuracion[13];
                    }
                    else
                    {
                        cs_prDbms = "Microsoft SQL Server";
                        cs_prDbmsdriver = "SQL Server";
                        cs_prDbmsservidor = "DESKTOP-G3PC1E0";
                        cs_prDbmsservidorpuerto = "1433";
                        cs_prDbnombre = "cs_bdfei";
                        cs_prDbusuario = "usuario";
                        cs_prDbclave = "password";
                        cs_prActivacioncodigo = "CODIGODEACTIVACION";
                        cs_prLoginUsuario = "admin";
                        cs_prLoginPassword = "admin123";
                        Cs_pr_Declarant_Id = "0";
                    }
                }
                else
                {
                    cs_prDbms = "Microsoft SQL Server";
                    cs_prDbmsdriver = "SQL Server";
                    cs_prDbmsservidor = "DESKTOP-G3PC1E0";
                    cs_prDbmsservidorpuerto = "1433";
                    cs_prDbnombre = "cs_bdfei";
                    cs_prDbusuario = "usuario";
                    cs_prDbclave = "password";
                    cs_prActivacioncodigo = "CODIGODEACTIVACION";
                    cs_prLoginUsuario = "admin";
                    cs_prLoginPassword = "admin123";
                    Cs_pr_Declarant_Id = "0";
                }
                sr0.Close();*/
                new clsEntityDatabaseSystem().cs_pxCrearBaseDatos();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsBaseConfiguracion " + ex.ToString());
                clsBaseMensaje.cs_pxMsgEr("ERR1", ex.ToString());
               
            }
        }
        private static void GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
        public static string getRutaInstalacion()
        {
            string ruta = new clsRegistry().Read("RUTA");
            if (ruta == null)
            {
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            return ruta + @"\FEI\Sistema";
        }
      
        /// <summary>
        /// Metodo para actualizar el archivo de configuracion. Opcional mensaje de confirmacion.
        /// </summary>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxActualizar(bool obtener_mensaje_respuesta)
        {
            try
            {
                if (!Directory.Exists(cs_prRutainstalacion))
                {
                    Directory.CreateDirectory(cs_prRutainstalacion);
                    File.Create(cs_cmArchivoconfiguracion).Close();
                }
                else if (!File.Exists(cs_cmArchivoconfiguracion))
                {
                    File.Create(cs_cmArchivoconfiguracion).Close();
                }

               /* StreamWriter sw1 = new StreamWriter(cs_cmArchivoconfiguracion);
                sw1.Write(clsBaseCifrado.cs_fxCifrar(
                    cs_prRutainstalacion + cs_cnArchivoconfiguracion_separador +
                    cs_prRutadocumentosenvio + cs_cnArchivoconfiguracion_separador +
                    cs_prRutadocumentosrecepcion + cs_cnArchivoconfiguracion_separador +
                    cs_prDbms + cs_cnArchivoconfiguracion_separador +
                    cs_prDbmsdriver + cs_cnArchivoconfiguracion_separador +
                    cs_prDbmsservidor + cs_cnArchivoconfiguracion_separador +
                    cs_prDbmsservidorpuerto + cs_cnArchivoconfiguracion_separador +
                    cs_prDbnombre + cs_cnArchivoconfiguracion_separador +
                    cs_prDbusuario + cs_cnArchivoconfiguracion_separador +
                    cs_prDbclave + cs_cnArchivoconfiguracion_separador +
                    cs_prActivacioncodigo + cs_cnArchivoconfiguracion_separador +
                    cs_prLoginUsuario + cs_cnArchivoconfiguracion_separador +
                    cs_prLoginPassword + cs_cnArchivoconfiguracion_separador +
                    Cs_pr_Declarant_Id + cs_cnArchivoconfiguracion_separador,
                    cs_cnLlave));
                sw1.Close();
                */
                if (obtener_mensaje_respuesta == true)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE3");
                }
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR3", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsBaseConfiguracion cs_pxActualizar" + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo para actualizar la ruta de instalacion.
        /// </summary>
        public void actualizarRutaInstalacion()
        {
            string ruta = new clsRegistry().Read("RUTA");
            // string defecto = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (ruta == null)
            {
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); ;
            }
            cs_prRutainstalacion = ruta + @"\FEI\Sistema";
            cs_prRutareportesPDF = ruta + @"\FEI\Reportes\PDF";
            cs_prRutareportesCSV = ruta + @"\FEI\Reportes\CSV";
            cs_prRutadocumentosenvio = ruta + @"\FEI\Documentos_Envio";
            cs_prRutadocumentosrecepcion = ruta + @"\FEI\Documentos_Recepcion";
            cs_prRutaCargaTextoPlano = ruta + @"\FEI\Documentos_Carga";
            try
            {
                cs_cmArchivoconfiguracion = cs_prRutainstalacion + "\\" + "fei.config";
                cs_cmLog_sistema = cs_prRutainstalacion + "\\" + "fei_sistema.log";
                cs_cmLog_sistema_add = cs_prRutainstalacion + "\\" + "fei_sistema_add.log";
                cs_cmLog_dll = cs_prRutainstalacion + "\\" + "fei_dll.log";
              /*  if (File.Exists(cs_cmArchivoconfiguracion))
                {
                    string acs_prRutainstalacion="";
                    string acs_prRutadocumentosenvio="";
                    string acs_prRutadocumentosrecepcion="";
                    string acs_prDbms="";
                    string acs_prDbmsdriver="";
                    string acs_prDbmsservidor="";
                    string acs_prDbmsservidorpuerto="";
                    string acs_prDbnombre="";
                    string acs_prDbusuario="";
                    string acs_prDbclave="";
                    string acs_prActivacioncodigo="";
                    string acs_prLoginUsuario="";
                    string acs_prLoginPassword="";
                    string aCs_pr_Declarant_Id="";

                    StreamReader sr0 = new StreamReader(cs_cmArchivoconfiguracion);
                    string configuracionbasedatos = sr0.ReadLine() + "";
                    if (configuracionbasedatos.Trim().Length != 0)
                    {
                        configuracionbasedatos = clsBaseCifrado.cs_fxDescifrar(configuracionbasedatos, cs_cnLlave);
                        string[] configuracion = configuracionbasedatos.Split(cs_cnArchivoconfiguracion_separador);
                        if (configuracion.Length == 15)
                        {
                            acs_prRutainstalacion = configuracion[0];
                            acs_prRutadocumentosenvio = configuracion[1];
                            acs_prRutadocumentosrecepcion = configuracion[2];
                            acs_prDbms = configuracion[3];
                            acs_prDbmsdriver = configuracion[4];
                            acs_prDbmsservidor = configuracion[5];
                            acs_prDbmsservidorpuerto = configuracion[6];
                            acs_prDbnombre = configuracion[7];
                            acs_prDbusuario = configuracion[8];
                            acs_prDbclave = configuracion[9];
                            acs_prActivacioncodigo = configuracion[10];
                            acs_prLoginUsuario = configuracion[11];
                            acs_prLoginPassword = configuracion[12];
                            aCs_pr_Declarant_Id = configuracion[13];
                        }

                    }
                    sr0.Close();
                    */
                    //actualizar rutas:
                   /* StreamWriter sw2 = new StreamWriter(cs_cmArchivoconfiguracion);
                    sw2.Write(clsBaseCifrado.cs_fxCifrar(
                        cs_prRutainstalacion + cs_cnArchivoconfiguracion_separador +
                        cs_prRutadocumentosenvio + cs_cnArchivoconfiguracion_separador +
                        cs_prRutadocumentosrecepcion + cs_cnArchivoconfiguracion_separador +
                        acs_prDbms + cs_cnArchivoconfiguracion_separador +
                        acs_prDbmsdriver + cs_cnArchivoconfiguracion_separador +
                        acs_prDbmsservidor + cs_cnArchivoconfiguracion_separador +
                        acs_prDbmsservidorpuerto + cs_cnArchivoconfiguracion_separador +
                        acs_prDbnombre + cs_cnArchivoconfiguracion_separador +
                        acs_prDbusuario + cs_cnArchivoconfiguracion_separador +
                        acs_prDbclave + cs_cnArchivoconfiguracion_separador +
                        acs_prActivacioncodigo + cs_cnArchivoconfiguracion_separador +
                        acs_prLoginUsuario + cs_cnArchivoconfiguracion_separador +
                        acs_prLoginPassword + cs_cnArchivoconfiguracion_separador +
                        aCs_pr_Declarant_Id + cs_cnArchivoconfiguracion_separador,
                        cs_cnLlave));
                    sw2.Close();
                }    */         
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsg("Error actualizacion ruta","Error al actualizar las rutas de configuracion");
                clsBaseLog.cs_pxRegistarAdd("No se pudo actualizar las rutas"+ex.ToString());
            }

        }
    }
}
