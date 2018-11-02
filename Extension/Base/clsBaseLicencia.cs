//using ContasisALF;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseLicencia")]
    public class clsBaseLicencia
    {
        private const string cs_cnLlave = "EuIZ7jvl€7Tsb~BVk#lfKrVH8lNTjimVb9kuWVh(2?yIFXIZ(MlQu]GeE27slae0~UR8}Bat$nlw¿O8dcrW4LXrM[s!6f¡{}xd1r1Gvbkfs~P%?9&~fBffBY$gVFfk@¡]LYdils#9#gLRrKFt€he2MIgmMfvvE]ww!dP&~kRxoo$%gv~@YRe(ISLn¡0YozbYdE@8pA)Ab¡Y(zU=gg€eFH¡gux)&{ed005coyv07Yc7pjld¿CmZ?Fh~Qs8oo{hdnGwmk~lFInjKm[h23~20rhDJ1$wDd=3%WV(0QLgQu[8NixAYZikWl5qxY%u#n9YCn%S}!6tcfYEBAj€PGW08nyE)rGZa?QqPY6&4P1p@Qf96osVEzjO=I@fiMN@NljS~p&&WMNMl79RuZOlaair¡tqrFd~w8rcujV¡}¿Ow#}Cc9jz&€m3AM]9un)iSnuWgJqQ%xn€BACY%e$HA$abyccrt8imZOhnoRxEVcDSCEd82mxEEadX$TcUd";
        public ModVers Licencia;

        public clsBaseLicencia()
        {
        }
        public string[] getDatosLicencia()
        {
            try
            {
                string directory = Environment.CurrentDirectory;
                string directory2 = Directory.GetCurrentDirectory();
                string install = new clsBaseLicencia().getRutaInstalacionLicencia();
                StreamReader sr = new StreamReader(install + "lcl.txt", System.Text.Encoding.Default);
                string textoPeticion = sr.ReadToEnd();
                sr.Close();

                string[] retorno = new string[9];
                string licencia = new clsBaseLicencia().loadLicence();
                string lic_base = new clsBaseLicencia().fx_DecodificarBase64(licencia);
                char[] split = new char[] { '[', ']' };
                string licencia_aes = new clsBaseAES().decodificar(lic_base);
                string[] decodificado = licencia_aes.Split(split, StringSplitOptions.RemoveEmptyEntries);

                string cTiempo = string.Empty;
                string cCodActivacion = new clsBaseLicencia().fx_DecodificarBase64(decodificado[0].Trim());
                string cCodProducto = new clsBaseLicencia().fx_DecodificarBase64(decodificado[1].Trim());
                string cNroLicencias = new clsBaseLicencia().fx_DecodificarBase64(decodificado[2].Trim());
                string cVersion = new clsBaseLicencia().fx_DecodificarBase64(decodificado[3].Trim());
                string cPeticion = new clsBaseLicencia().fx_DecodificarBase64(decodificado[4].Trim());
                if (decodificado.Length > 5)
                {
                    cTiempo = new clsBaseLicencia().fx_DecodificarBase64(decodificado[5].Trim());
                }
                string app_tipolicencia = cCodProducto.Substring(9, 2);
                string app_codproyecto = cCodProducto.Substring(0, 3);
                string cCodModulo = cCodProducto.Substring(4, 4);
                retorno[0] = cCodActivacion;
                retorno[1] = cCodProducto;
                retorno[2] = cNroLicencias;
                retorno[3] = cVersion;
                retorno[4] = cPeticion;
                retorno[5] = cTiempo;
                retorno[6] = app_tipolicencia;
                retorno[7] = app_codproyecto;
                retorno[8] = cCodModulo;
                if (textoPeticion != cPeticion)
                {
                    clsBaseLog.cs_pxRegistarAdd("gdatl -> Peticion de la licencia diff Peticion Maquina.");
                    retorno = null;
                }             
                return retorno;                           
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("gdatl -> " + ex.ToString());
                return null;
            }
        }
        public  bool licenceExists()
        {
            bool retorno = false;
            string licencia = new clsBaseLicencia().loadLicence();
            bool flag = licencia.Equals(string.Empty);
            if (flag)
            {
                retorno = false;
            }
            else
            {
                retorno = true;
            }
            return retorno;
        }

        public bool licenceActive(string fecha,string version)
        {
            /*Se Hace uso del Modulo para activar cierta Seccion del Codigo, Valido para la Licencia*/
            Licencia = new ModVers();
            if (Licencia.Active_LIQUY() == true)
            {
                //clsBaseLog.cs_pxRegistarAdd("licenciaactive");
                bool active = false;
                string text2 = new clsBaseLicencia().loadLicence();
                /*obtener si esta vacio o no*/
                bool flag3 = text2.Equals(string.Empty);
                if (!flag3)
                {
                    /*existe info, obtener fecha y comparar si esta activa o no*/
                    try
                    {
                        clsBaseLog.cs_pxRegistarAdd("getdatoslicencia");
                        string[] datosLicencia = getDatosLicencia();
                        if (datosLicencia != null)
                        {
                            clsBaseLog.cs_pxRegistarAdd("licencia active " + version.Substring(0, 5) + " " + datosLicencia[3].ToString().Substring(0, 5));
                            /*comprobar version de la licencia.*/
                            if (datosLicencia[3].ToString().Substring(0,5) == version.Substring(0,5))
                            {
                                /*es la misma version comprobar la fecha*/

                                DateTime fechaComprobante = DateTime.ParseExact(fecha, "yyyy-MM-dd", null);
                                clsBaseLog.cs_pxRegistarAdd(fecha + " " + datosLicencia[5]);
                                if (datosLicencia[5] != "")
                                {
                                    string cadenafecha = datosLicencia[5];
                                    int mesLicencia = Convert.ToInt16(cadenafecha.Substring(0, 2));
                                    int anioLicencia = Convert.ToInt16(cadenafecha.Substring(2, 4));
                                    int mesHoy = Convert.ToInt16(fechaComprobante.ToString("MM"));
                                    int anioHoy = Convert.ToInt16(fechaComprobante.ToString("yyyy"));
                                    if (anioHoy <= anioLicencia)
                                    {
                                        if (anioHoy == anioLicencia)
                                        {/*si es el mismo año comparar los meses*/
                                            if (mesHoy <= mesLicencia)
                                            {
                                                active = true;
                                            }
                                            else
                                            {
                                                active = false;
                                            }
                                        }
                                        else
                                        {/*el año actual es menor que el año de la licencia*/
                                            active = true;
                                        }
                                    }
                                    else
                                    {
                                        active = false;
                                    }

                                }
                                else
                                {
                                    /*permanente*/
                                    active = true;
                                }
                            }
                            else
                            {
                                active = false;
                            }
                        }
                        else
                        {
                            active = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsBaseLog.cs_pxRegistarAdd("liact->" + ex.ToString());
                    }
                }
                return active;
            }
            else
            {
                return true;
            } 
        }
        public bool saveLicence(string cadena)
        {
            bool retorno = false;
            try
            {
                
                string currentDirectory = Environment.CurrentDirectory;
                string location = currentDirectory + "//Entity.dll";

                clsBaseLog.cs_pxRegistarAdd("Globo: "+location);

                if (!File.Exists(location))
                {
                    File.Create(location).Close();
                }
                StreamWriter sw1 = new StreamWriter(location);
                sw1.Write(cadena);
                sw1.Close();
                GrantAccess(location);
                retorno = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("RegistroL" + ex.ToString());
            }
            return retorno;
        }
        public bool activarLicencia(string version, string modulo, string codeMaquina, string codigoActivacion, string licencia)
        {
            bool retorno = false;
            try
            {
                string lic_base = fx_DecodificarBase64(licencia);
                char[] split = new char[] { '[', ']' };
                string licencia_aes = new clsBaseAES().decodificar(lic_base);
                string[] decodificado = licencia_aes.Split(split, StringSplitOptions.RemoveEmptyEntries);

                string cTiempo          = string.Empty;
                string cCodActivacion   = fx_DecodificarBase64(decodificado[0].Trim());
                string cCodProducto     = fx_DecodificarBase64(decodificado[1].Trim());
                string cNroLicencias    = fx_DecodificarBase64(decodificado[2].Trim());
                string cVersion         = fx_DecodificarBase64(decodificado[3].Trim());
                string cPeticion        = fx_DecodificarBase64(decodificado[4].Trim());
                if (decodificado.Length > 5)
                {
                    cTiempo = fx_DecodificarBase64(decodificado[5].Trim());
                }
                string app_tipolicencia = cCodProducto.Substring(9, 2);
                string app_codproyecto  = cCodProducto.Substring(0, 3);
                string cCodModulo       = cCodProducto.Substring(4, 4);

                if (cCodActivacion.Equals(string.Empty))
                {
                    return false;
                }
                if (cCodProducto.Equals(string.Empty))
                {
                    return false;
                }
                if (cVersion.Equals(string.Empty))
                {
                    return false;
                }
                if (cPeticion.Equals(string.Empty))
                {
                    return false;
                }
                string xPeticion = codeMaquina;
                string xVersion = version;
                string xCodModulo = modulo;

                clsBaseLog.cs_pxRegistarAdd("Globo: " +codigoActivacion+" == "+cCodActivacion+" && "+xCodModulo +"=="+ cCodModulo +"&&"+ xPeticion+ "==" + cPeticion + "&&" + xVersion +"=="+ cVersion);

                if (codigoActivacion == cCodActivacion && xCodModulo == cCodModulo && xPeticion == cPeticion && xVersion.Substring(0,5) == cVersion.Substring(0,5))
                {
                    if (cTiempo.Equals(string.Empty)) {

                        retorno = true;
                    }
                    else
                    {
                        retorno = true;
                    }

                }

                return retorno;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("actreglic " + ex.ToString());
            }
            return retorno;
        }
        public bool removeLicence()
        {
            bool retorno = false;
            try
            {
                string currentDirectory = Environment.CurrentDirectory;
                string location = currentDirectory + @"\Entity.dll";
                File.Delete(location);
                retorno = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("dltlic " + ex.ToString());
            }
            return retorno;
        }
        public string getRutaInstalacionLicencia()
        {
            string install = string.Empty;
            if (Environment.Is64BitOperatingSystem)
            {

                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Contasis S.A.C");
                if (key != null)
                {
                    Object o = key.GetValue("InstallDirFEI");
                    install = o.ToString();

                }
            }
            else
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Contasis S.A.C");
                if (key != null)
                {
                    Object o = key.GetValue("InstallDirFEI");
                    install = o.ToString();
                }
            }
            return install;
        }
        public string loadLicence()
        {
            string retorno = String.Empty;
            string install = string.Empty;
           
            try
            {
                install = new clsBaseLicencia().getRutaInstalacionLicencia();
                string currentDirectory = Environment.CurrentDirectory;
                string otro = Directory.GetCurrentDirectory();
                string location = install + "Entity.dll";
                //clsBaseLog.cs_pxRegistarAdd(location);
                if (File.Exists(location))
                {
                    StreamReader sr0 = new StreamReader(location);
                    retorno = sr0.ReadToEnd().Trim();
                    sr0.Close();
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("LoadRegistroL" + ex.ToString());
            }
            return retorno;
        }
        public string getFechaVencimiento()
        {
            string fechaVencimiento = string.Empty;
            try
            {
                string text2 = new clsBaseLicencia().loadLicence();
                if (text2.Equals(string.Empty))
                {
                    fechaVencimiento = "La licencia no esta activada";
                }
                else
                {
                    string[] datosLicencia = getDatosLicencia();
                    if (datosLicencia != null)
                    {
                        //descencriptar y obtener fechaa
                        if (datosLicencia[5] != "")
                        {
                            string cadenafecha = datosLicencia[5];
                            int mesLicencia = Convert.ToInt16(cadenafecha.Substring(0, 2));
                            int anioLicencia = Convert.ToInt16(cadenafecha.Substring(2, 4));

                            DateTime fechaHoy = DateTime.Now.Date;
                            int mesHoy = Convert.ToInt16(fechaHoy.ToString("MM"));
                            int anioHoy = Convert.ToInt16(fechaHoy.ToString("yyyy"));
                            var firstDayOfMonth = new DateTime(anioLicencia, mesLicencia, 1);
                            DateTime fechaLicencia = firstDayOfMonth.AddMonths(1).AddDays(-1);

                            if (anioHoy <= anioLicencia)
                            {
                                if (anioHoy == anioLicencia)
                                {//si es el mismo año comparar los meses
                                    if (mesHoy <= mesLicencia)
                                    {
                                        if (mesHoy == mesLicencia)
                                        {
                                            TimeSpan ts = fechaLicencia - fechaHoy;

                                            int dias = ts.Days + 1;

                                            if (dias <= 0)
                                            {
                                                fechaVencimiento = "Su licencia ha vencido el dia " + fechaLicencia.ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                if (dias == 1)
                                                {
                                                    fechaVencimiento = "Su licencia vence hoy";
                                                }
                                                else if (dias == 2)
                                                {
                                                    fechaVencimiento = "Su licencia vence mañana";
                                                }
                                                else
                                                {
                                                    dias = dias - 1;
                                                    fechaVencimiento = "Su licencia vence en " + dias.ToString() + " dias";
                                                }
                                            }

                                        }
                                        else
                                        {
                                            fechaVencimiento = "Su licencia vence en " + firstDayOfMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " de " + anioLicencia + "";
                                        }

                                    }
                                    else
                                    {
                                        fechaVencimiento = "Licencia vencida en " + firstDayOfMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " de " + anioLicencia + "";
                                    }
                                }
                                else
                                {
                                    //el año actual es menor que el año licencia
                                    fechaVencimiento = "Su licencia vence en " + firstDayOfMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " de " + anioLicencia + "";
                                }
                            }
                            else
                            {
                                fechaVencimiento = "Licencia vencida en " + firstDayOfMonth.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-ES")) + " de " + anioLicencia + "";
                            }
                            // DateTime fechaLicencia = DateTime.ParseExact(fechaVencimiento, "yyyy-MM-dd", null);

                        }
                        else
                        {
                            fechaVencimiento = "Licencia permanente";
                        }
                    }
                    else
                    {
                        fechaVencimiento = "La licencia no se pudo cargar correctamente";
                    }

                }
            }
            catch
            {
                fechaVencimiento = "Licencia caducada.Contacte a su proveedor.";
            }
            return fechaVencimiento;
        }
        public bool getIsTerminalServer() {

           // return true;
            
            bool retorno = false;
            bool existe = licenceExists();
            if (existe)
            {
                string[] datos = getDatosLicencia();
                if (datos != null)
                {
                    if (datos[6] == "RD")
                    {
                        retorno = true;
                    }
                }
                else
                {
                    retorno = false;
                }
            }
            else
            {
                retorno = false;
            }
            return retorno;
        }
        public int getConexionesPermitidas(){
            //  return 1;
            int conexiones=0;
            try
            {
                string[] datos = getDatosLicencia();
                conexiones = Convert.ToInt16(datos[2]);
            }
            catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("gcp "+ex.ToString());
            }
            
            return conexiones;
        }
        
        public  int getUsuariosActivos()
        {
            int numero = 0;
            try
            {
                string currentDirectory =  Environment.CurrentDirectory;
                string commonDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string location = currentDirectory + "//System.Xdf.dll";
                string location_alternative = commonDirectory + "//Facturacion//System.Fdx.dll";
                if (File.Exists(location))
                {
                    StreamReader sr0 = new StreamReader(location);
                    string retorno = sr0.ReadLine();
                    sr0.Close();
                    string descifrado = cs_fxDescifrar(retorno,cs_cnLlave);
                    string[] partes = descifrado.Split('_');
                    numero = Convert.ToInt16(partes[0]);
                }
                else
                {
                    if (Directory.Exists(commonDirectory + "//Facturacion"))
                    {
                        if (File.Exists(location_alternative))
                        {
                            StreamReader sr0 = new StreamReader(location_alternative);
                            string retorno = sr0.ReadLine();
                            sr0.Close();
                            string descifrado = cs_fxDescifrar(retorno, cs_cnLlave);
                            string[] partes = descifrado.Split('_');
                            numero = Convert.ToInt16(partes[0]);
                            saveUsuariosActivos(numero.ToString());
                        }
                    }                      
                }
            }
            catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("get usuarios activos" + ex.ToString());
                numero = 0;
            }
            return numero;
        }
        public bool saveUsuariosActivos(string licencias)
        {         
            try
            {
                string cifrado = cs_fxCifrar(licencias+"_TerminalServerLicence_Licencia_Decrypt", cs_cnLlave);
                string currentDirectory = Environment.CurrentDirectory;
                string commonDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                string location = currentDirectory + "//System.Xdf.dll";
                string location_alternative = commonDirectory + "//Facturacion//System.Fdx.dll";
               
                if (!File.Exists(location))
                {
                    File.Create(location).Close();
                }
                StreamWriter sw1 = new StreamWriter(location);
                sw1.Write(cifrado);
                sw1.Close();
                GrantAccess(location);

                if (!Directory.Exists(commonDirectory + "//Facturacion"))
                {
                    Directory.CreateDirectory(commonDirectory + "//Facturacion");
                    if (!File.Exists(location_alternative))
                    {
                        File.Create(location_alternative).Close();
                    }
                }
                          
                StreamWriter sw2 = new StreamWriter(location_alternative);
                sw2.Write(cifrado);
                sw2.Close();
                GrantAccess(location_alternative);
                return true;
            }
            catch(Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("svaelicac"+ex.ToString());
                return false;
            }
                     
        }
        private void GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);
        }
        public string getTipoLicencia(string codigo)
        {
            string tipoLicencia = string.Empty;
            try
            {
                switch (codigo)
                {
                    case "ES":
                        tipoLicencia = "Licencia Estandar";
                        break;
                    case "RD":
                        tipoLicencia = "Licencia Terminal Server";
                        break;
                    case "RL":
                        tipoLicencia = "Licencia Red Local";
                        break;
                    default:
                        tipoLicencia = "-";
                        break;
                }               
            }
            catch
            {
                tipoLicencia = string.Empty;
            }
            return tipoLicencia;
        }
        public string cs_fxCifrar(string cadena, string llave)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] llave_cifrado = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llave));
            md5.Clear();
            byte[] arreglo_cadena = UTF8Encoding.UTF8.GetBytes(cadena);
            TripleDESCryptoServiceProvider a_3des = new TripleDESCryptoServiceProvider();
            a_3des.Key = llave_cifrado;
            a_3des.Mode = CipherMode.ECB;
            a_3des.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = a_3des.CreateEncryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo_cadena, 0, arreglo_cadena.Length);
            a_3des.Clear();
            return Convert.ToBase64String(resultado, 0, resultado.Length);
        }
        /// <summary>
        /// Metodo para descifrar una cadena.
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="llave"></param>
        /// <returns>Cadena sin cifrar</returns>
        public string cs_fxDescifrar(string cadena, string llave)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] llave_cifrado = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llave));
            md5.Clear();
            byte[] arreglo_cadena = Convert.FromBase64String(cadena);
            TripleDESCryptoServiceProvider a_3des = new TripleDESCryptoServiceProvider();
            a_3des.Key = llave_cifrado;
            a_3des.Mode = CipherMode.ECB;
            a_3des.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = a_3des.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo_cadena, 0, arreglo_cadena.Length);
            a_3des.Clear();
            return UTF8Encoding.UTF8.GetString(resultado);
        }
        /// <summary>
        /// Metodo para generar una llave aleatoria.
        /// </summary>
        /// <param name="longitud"></param>
        /// <returns>Cadena de llave aleatoria</returns>
        public string cd_pxLlaveAleatoria(int longitud)
        {
            char[] cadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#~€¡!{}[]¿?$%&()=".ToCharArray();
            byte[] caracter = new byte[1];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(caracter);
            caracter = new byte[longitud];
            rng.GetNonZeroBytes(caracter);
            StringBuilder llave = new StringBuilder(longitud);
            foreach (byte b in caracter)
            {
                llave.Append(cadena[b % (cadena.Length)]);
            }
            return llave.ToString();
        }
        public  string fx_CodificarBase64(string cadena)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(cadena);
            return Convert.ToBase64String(bytes);
        }

        public string fx_DecodificarBase64(string cadena_codificadaB64)
        {
            byte[] bytes = Convert.FromBase64String(cadena_codificadaB64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
