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
    [ProgId("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty")]
    public class clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty : clsBaseEntidad
    {
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id { get; set; }
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_Name { get; set; }
        public string Cs_tag_Value { get; set; }

        //private clsEntityDatabaseLocal localDB;

        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = valores[0];
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = valores[1];
                Cs_tag_ID = valores[2];
                Cs_tag_Name = valores[3];
                Cs_tag_Value = valores[4];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id");
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            for (int i = 1; i < 4; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation_AdditProperty";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AddiInformation_AdditProperty_Id");
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            for (int i = 1; i < 4; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty()
        {
           // localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id");
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            for (int i = 1; i < 4; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation_AdditProperty";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AddiInformation_AdditProperty_Id");
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            for (int i = 1; i < 4; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id);
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id);
            cs_cmValores.Add(Cs_tag_ID);
            cs_cmValores.Add(Cs_tag_Name);
            cs_cmValores.Add(Cs_tag_Value);
        }

        public List<List<string>> cs_pxObtenerTodoPorId(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id=" + id.ToString().Trim() + " ;";
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
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty  cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _cualquiertipo)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_iars_cualquiertipo_id  : " + Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_id  : " + Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_cualquiertipo_ID  : " + Cs_tag_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(Cs_tag_ID)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_cualquiertipo_Name  : " + Cs_tag_Name + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(Cs_tag_Name)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_cualquiertipo_Value  : " + Cs_tag_Value + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15(Cs_tag_Value)) + "]" + ef;

            return contenido;
        }
        
        public List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id =" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    var item = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
