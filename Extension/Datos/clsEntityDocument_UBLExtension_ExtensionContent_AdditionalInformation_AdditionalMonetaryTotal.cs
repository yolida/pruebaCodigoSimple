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
    [ProgId("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal")]
    public class clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal : clsBaseEntidad
    {
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id { get; set; }
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id { get; set; }
        public string Cs_tag_Id { get; set; }
        public string Cs_tag_Name { get; set; }
        public string Cs_tag_ReferenceAmount { get; set; }
        public string Cs_tag_PayableAmount { get; set; }
        public string Cs_tag_Percent { get; set; }
        public string Cs_tag_TotalAmount { get; set; }
        //jordy amaro 02/11/16 Fe-832
        //Agregado propiedad para permitir tag de percepcion
        public string Cs_tag_SchemeID { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = valores[0];
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = valores[1];
                Cs_tag_Id = valores[2];
                Cs_tag_Name = valores[3];
                Cs_tag_ReferenceAmount = valores[4];
                Cs_tag_PayableAmount = valores[5];
                Cs_tag_Percent = valores[6];
                Cs_tag_TotalAmount = valores[7];
                Cs_tag_SchemeID = valores[8];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal";         
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id");
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
          
            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation_AdditMonetaryTotal";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_AdditMonetaryTotal_Id");
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id");
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Doc_UBLExt_ExtContent_AdditInformation_AdditMonetaryTotal";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_AdditMonetaryTotal_Id");
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AdditInformation_Id");
            for (int i = 1; i < 8; i++)//Número de campos
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
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id);
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id);
            cs_cmValores.Add(Cs_tag_Id);
            cs_cmValores.Add(Cs_tag_Name);
            cs_cmValores.Add(Cs_tag_ReferenceAmount);
            cs_cmValores.Add(Cs_tag_PayableAmount);
            cs_cmValores.Add(Cs_tag_Percent);
            cs_cmValores.Add(Cs_tag_TotalAmount);
            cs_cmValores.Add(Cs_tag_SchemeID);
        }

        public List<List<string>> cs_pxObtenerTodoPorCabeceraId(string id)
        {
            List<List<string>> tabla_contenidos = new List<List<string>>();
            try
            {
                //tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id =" + id.ToString().Trim() + " ;";
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
               
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal cs_pxObtenerTodoPorCabeceraId " + ex.ToString());
               // return null;
            }
            return tabla_contenidos;
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _tipomonetario)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_id : " + Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_id : " + Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_ID : " + Cs_tag_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(Cs_tag_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_Name : " + Cs_tag_Name + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(Cs_tag_Name)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_ReferenceAmount : " + Cs_tag_ReferenceAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_ReferenceAmount)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_PayableAmount : " + Cs_tag_PayableAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_18_F_n15c2(Cs_tag_PayableAmount)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_Percent : " + Cs_tag_Percent + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_Porcentaje(Cs_tag_Percent)) + "]" + ef;
            contenido += ei + "Cecabecera_iars_tipomonetario_TotalAmount : " + Cs_tag_TotalAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_TotalAmount)) + "]" + ef;

            return contenido;
        }

        public List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal>();
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
                    var item = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
