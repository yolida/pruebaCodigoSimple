using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Data.Odbc;
using Extension.Negocio;
using System.Runtime.InteropServices;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation")]
    public class clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation : clsBaseEntidad
    {
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0){
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            cs_cmCampos.Add("cs_Document_Id");

            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
        }

        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            cs_cmCampos.Add("cs_Document_Id");

            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
        }
        public void setInicioDeclarante(string id) // Comentar que valor es el parámetro que se le pasa
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
        }

        public List<List<string>> cs_pxObtenerTodoPorCabeceraId(string id)
        {
            List<List<string>> tabla_contenidos = new List<List<string>>();
            try
            {          
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id=" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
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
               // return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation cs_pxObtenerTodoPorCabeceraId" + ex.ToString());
                //return null;
            }
            return tabla_contenidos;
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation )" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_iars_id : " + Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_id: " + Cs_pr_Document_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Id)) + "]" + ef;

            return contenido;
        }
        

        public List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    var item = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localDB);
                    int count = 0;
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        prop.SetValue(item, Convert.ChangeType(datos[count].ToString(), prop.PropertyType), null);
                        count++;
                    }
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
