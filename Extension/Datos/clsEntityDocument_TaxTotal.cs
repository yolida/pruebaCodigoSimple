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
    [ProgId("clsEntityDocument_TaxTotal")]
    public class clsEntityDocument_TaxTotal : clsBaseEntidad
    {
        public string Cs_pr_Document_TaxTotal_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_TaxAmount { get; set; }
        public string Cs_tag_TaxSubtotal_TaxAmount { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_TaxTotal cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_TaxTotal_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                Cs_tag_TaxAmount = valores[2];
                Cs_tag_TaxSubtotal_TaxAmount = valores[3];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = valores[4];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = valores[5];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = valores[6];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_TaxTotal(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_TaxTotal";
            cs_cmCampos.Add("cs_Document_TaxTotal_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 6; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_TaxTotal";
            cs_cmCampos_min.Add("cs_Document_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 6; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_TaxTotal()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_TaxTotal";
            cs_cmCampos.Add("cs_Document_TaxTotal_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 6; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_TaxTotal";
            cs_cmCampos_min.Add("cs_Document_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 6; i++)//Número de campos
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
            cs_cmValores.Add(Cs_pr_Document_TaxTotal_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_TaxAmount);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxAmount);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode);
        }

        public List<List<string>> cs_pxObtenerTodoPorCabeceraId(string id)
        {
            List<List<string>> tabla_contenidos = new List<List<string>>();
            try
            {
                
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + ";";
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_TaxTotal cs_pxObtenerTodoPorCabeceraId " + ex.ToString());
                //return null;
            }
            return tabla_contenidos;
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument_impuestosglobales)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_impuestosglobales_id : " + Cs_pr_Document_TaxTotal_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_TaxTotal_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_id: " + Cs_pr_Document_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Id)) + "]" + ef;
            contenido += ei + "TaxTotal_TaxAmount : " + Cs_tag_TaxAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_TaxAmount)) + "]" + ef;
            contenido += ei + "TaxTotal_TaxSubtotal_TaxAmount : " + Cs_tag_TaxSubtotal_TaxAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_TaxSubtotal_TaxAmount)) + "]" + ef;
            contenido += ei + "TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_ID : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID)) + "]" + ef;
            contenido += ei + "TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_Name : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_6(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name)) + "]" + ef;
            contenido += ei + "TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an3(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode)) + "]" + ef;

            return contenido;
        }
        
        public List<clsEntityDocument_TaxTotal> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_TaxTotal>();
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
                    var item = new clsEntityDocument_TaxTotal(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_TaxTotal cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
            }
            return resultado;
        }
    }
}
