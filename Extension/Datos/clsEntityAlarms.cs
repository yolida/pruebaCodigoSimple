using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using Extension.Base;
using System.Data.SQLite;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityAlarms")]
    public class clsEntityAlarms : clsBaseEntidadSistema
    {
        public string Cs_pr_Alarms_Id { get; set; }
        public string Cs_pr_Envioautomatico { get; set; }
        public string Cs_pr_Envioautomatico_minutos { get; set; }
        public string Cs_pr_Envioautomatico_minutosvalor { get; set; }
        public string Cs_pr_Envioautomatico_hora { get; set; }
        public string Cs_pr_Envioautomatico_horavalor { get; set; }
        public string Cs_pr_Enviomanual { get; set; }
        public string Cs_pr_Enviomanual_mostrarglobo { get; set; }
        public string Cs_pr_Enviomanual_mostrarglobo_minutosvalor { get; set; }
        public string Cs_pr_Enviomanual_nomostrarglobo { get; set; }
        public string Cs_pr_Iniciarconwindows { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }
        public string Cs_pr_Tipo { get; set; }
        /// <summary>
        /// Obtener un objeto alarma segun Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Alarma</returns>
        public clsEntityAlarms cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Alarms_Id = valores[0];
                Cs_pr_Envioautomatico = valores[1];
                Cs_pr_Envioautomatico_minutos = valores[2];
                Cs_pr_Envioautomatico_minutosvalor = valores[3];
                Cs_pr_Envioautomatico_hora = valores[4];
                Cs_pr_Envioautomatico_horavalor = valores[5];
                Cs_pr_Enviomanual = valores[6];
                Cs_pr_Enviomanual_mostrarglobo = valores[7];
                Cs_pr_Enviomanual_mostrarglobo_minutosvalor = valores[8];
                Cs_pr_Enviomanual_nomostrarglobo = valores[9];
                Cs_pr_Iniciarconwindows = valores[10];
                Cs_pr_Declarant_Id = valores[11];
                Cs_pr_Tipo = valores[12];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityAlarms()
        {
            cs_cmTabla = "cs_Alarms";
            cs_cmCampos.Add("cs_Alarms_Id");
            cs_cmCampos.Add("envioautomatico");
            cs_cmCampos.Add("envioautomatico_minutos");
            cs_cmCampos.Add("envioautomatico_minutosvalor");
            cs_cmCampos.Add("envioautomatico_hora");
            cs_cmCampos.Add("envioautomatico_horavalor");
            cs_cmCampos.Add("enviomanual");
            cs_cmCampos.Add("enviomanual_mostrarglobo");
            cs_cmCampos.Add("enviomanual_mostrarglobo_minutosvalor");
            cs_cmCampos.Add("enviomanual_nomostrarglobo");
            cs_cmCampos.Add("iniciarconwindows");
            cs_cmCampos.Add("cs_Declarant_Id");
            cs_cmCampos.Add("tipo");
        }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Alarms_Id);
            cs_cmValores.Add(Cs_pr_Envioautomatico);
            cs_cmValores.Add(Cs_pr_Envioautomatico_minutos);
            cs_cmValores.Add(Cs_pr_Envioautomatico_minutosvalor);
            cs_cmValores.Add(Cs_pr_Envioautomatico_hora);
            cs_cmValores.Add(Cs_pr_Envioautomatico_horavalor);
            cs_cmValores.Add(Cs_pr_Enviomanual);
            cs_cmValores.Add(Cs_pr_Enviomanual_mostrarglobo);
            cs_cmValores.Add(Cs_pr_Enviomanual_mostrarglobo_minutosvalor);
            cs_cmValores.Add(Cs_pr_Enviomanual_nomostrarglobo);
            cs_cmValores.Add(Cs_pr_Iniciarconwindows);
            cs_cmValores.Add(Cs_pr_Declarant_Id);
            cs_cmValores.Add(Cs_pr_Tipo);
        }
        /// <summary>
        /// Metodo para iniciar la aplicacion cuando inicie windows.
        /// </summary>
        /// <param name="inicio"></param>
        public void cs_pxIniciarWindows(bool inicio) {
            string lbl = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + lbl;
            string sistema = "FEI";
            if (inicio == true)
            {
                try
                {
                    RegistryKey runK = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    // añadirlo al registro
                    // Si el path contiene espacios se debería incluir entre comillas dobles
                    if (sistema.StartsWith("\"") == false && sistema.IndexOf(" ") > -1)
                    {
                        sistema = "\"" + sistema + "\"";
                    }
                    runK.SetValue(path, sistema);
                }
                catch (Exception ex)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR18", ex.ToString());
                }
            }
            else
            {
                try
                {
                    RegistryKey runK = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    // quitar la clave indicada del registo
                    runK.DeleteValue(path, false);
                }
                catch (Exception ex)
                {
                    clsBaseMensaje.cs_pxMsgEr("ERR18", ex.ToString());
                }
            }
        }
        /// <summary>
        /// Metodo para obtener una alarma 
        /// </summary>
        /// <param name="cs_pr_Declarant_Id"></param>
        /// <returns></returns>
        public clsEntityAlarms cs_fxObtenerUnoPorDeclaranteId(string cs_pr_Declarant_Id,string cs_pr_Tipo)
        {
            try
            {
                SQLiteDataReader datos = null;
                string sql = String.Empty;
                if (cs_pr_Tipo != "")
                {
                    sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Declarant_Id ='" + cs_pr_Declarant_Id.Trim() + "' AND tipo='" + cs_pr_Tipo.Trim() + "';";
                }
                else
                {
                    sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Declarant_Id ='" + cs_pr_Declarant_Id.Trim() + "' AND (tipo='' OR tipo is null);";
                }

                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityAlarms ALARM = null;
                while (datos.Read())
                {
                    ALARM = new clsEntityAlarms();
                    ALARM.Cs_pr_Alarms_Id = datos[0].ToString();
                    ALARM.Cs_pr_Envioautomatico = datos[1].ToString();
                    ALARM.Cs_pr_Envioautomatico_minutos = datos[2].ToString();
                    ALARM.Cs_pr_Envioautomatico_minutosvalor = datos[3].ToString();
                    ALARM.Cs_pr_Envioautomatico_hora = datos[4].ToString(); ;
                    ALARM.Cs_pr_Envioautomatico_horavalor = datos[5].ToString(); ;
                    ALARM.Cs_pr_Enviomanual = datos[6].ToString();
                    ALARM.Cs_pr_Enviomanual_mostrarglobo = datos[7].ToString();
                    ALARM.Cs_pr_Enviomanual_mostrarglobo_minutosvalor = datos[8].ToString();
                    ALARM.Cs_pr_Enviomanual_nomostrarglobo = datos[9].ToString();
                    ALARM.Cs_pr_Iniciarconwindows = datos[10].ToString();
                    ALARM.Cs_pr_Declarant_Id = datos[11].ToString();
                    ALARM.Cs_pr_Tipo = datos[12].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return ALARM;
            }
            catch (Exception)
            {
                //jordy amaro 16/11/16 Fe-842
                /*************** Crear campo en caso salga error para configuraciones antiguas *************/

                SQLiteConnection cs_pxConexion_basedatos;
                cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                //Crear tabla de alarma en caso no exista 
                new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityAlarms().cs_cmTabla + " (" + Insercion(new clsEntityAlarms().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //Agregar campo para tipo de alarma
                string sql = "ALTER TABLE " + new clsEntityAlarms().cs_cmTabla + " ADD tipo  VARCHAR(500) NULL;";
                SQLiteCommand afectado = new SQLiteCommand(sql, cs_pxConexion_basedatos);
                int valor = afectado.ExecuteNonQuery();
                cs_pxConexion_basedatos.Close();

                /*********************************************/
                SQLiteDataReader datos = null;
                string sqls = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Declarant_Id ='" + cs_pr_Declarant_Id.Trim() + "' AND tipo='" + cs_pr_Tipo.Trim() + "' ;";
                // SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sqls, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityAlarms ALARM = null;
                //Leer los datos obtenidos para asignar al objeto alarma.
                while (datos.Read())
                {
                    ALARM = new clsEntityAlarms();
                    ALARM.Cs_pr_Alarms_Id = datos[0].ToString();
                    ALARM.Cs_pr_Envioautomatico = datos[1].ToString();
                    ALARM.Cs_pr_Envioautomatico_minutos = datos[2].ToString();
                    ALARM.Cs_pr_Envioautomatico_minutosvalor = datos[3].ToString();
                    ALARM.Cs_pr_Envioautomatico_hora = datos[4].ToString(); ;
                    ALARM.Cs_pr_Envioautomatico_horavalor = datos[5].ToString(); ;
                    ALARM.Cs_pr_Enviomanual = datos[6].ToString();
                    ALARM.Cs_pr_Enviomanual_mostrarglobo = datos[7].ToString();
                    ALARM.Cs_pr_Enviomanual_mostrarglobo_minutosvalor = datos[8].ToString();
                    ALARM.Cs_pr_Enviomanual_nomostrarglobo = datos[9].ToString();
                    ALARM.Cs_pr_Iniciarconwindows = datos[10].ToString();
                    ALARM.Cs_pr_Declarant_Id = datos[11].ToString();
                    ALARM.Cs_pr_Tipo = datos[12].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return ALARM;
            }
          
        }
        /// <summary>
        /// Metodo para la insercion de nuevos campos en tabla.
        /// </summary>
        /// <param name="campos"></param>
        /// <returns></returns>
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
    }
}
