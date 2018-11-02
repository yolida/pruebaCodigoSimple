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
    [ProgId("clsEntityDocument_AdditionalDocumentReference")]
    public class clsEntityDocument_AdditionalDocumentReference : clsBaseEntidad
    {
        public string Cs_pr_Document_AdditionalDocumentReference_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_AdditionalDocumentReference_ID { get; set; }
        public string Cs_tag_DocumentTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_AdditionalDocumentReference cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_AdditionalDocumentReference_Id = valores[1];
                Cs_pr_Document_Id = valores[1];
                Cs_tag_AdditionalDocumentReference_ID = valores[2];
                Cs_tag_DocumentTypeCode = valores[3];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_AdditionalDocumentReference(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_AdditionalDocumentReference";        
            cs_cmCampos.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "c";
            cs_cmCampos_min.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_AdditionalDocumentReference()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_AdditionalDocumentReference";
            cs_cmCampos.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_AdditionalDocumentReference";
            cs_cmCampos_min.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 3; i++)//Número de campos
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
            cs_cmValores.Add(Cs_pr_Document_AdditionalDocumentReference_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_AdditionalDocumentReference_ID);
            cs_cmValores.Add(Cs_tag_DocumentTypeCode);
        }

        public List<List<string>> cs_pxObtenerTodoPorId(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + " ;";
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalDocumentReference cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntityDocument_otrodocumentorelacionado)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cecabecera_otrodocumentorelacionado_id  : " + Cs_pr_Document_AdditionalDocumentReference_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_AdditionalDocumentReference_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_id: " + Cs_pr_Document_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Id)) + "]" + ef;
            contenido += ei + "AdditionalDocumentReference_ID  : " + Cs_tag_AdditionalDocumentReference_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(Cs_tag_AdditionalDocumentReference_ID)) + "]" + ef;
            contenido += ei + "AdditionalDocumentReference_DocumentTypeCode  : " + Cs_tag_DocumentTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(Cs_tag_DocumentTypeCode)) + "]" + ef;

            return contenido;
        }
        
        public List<clsEntityDocument_AdditionalDocumentReference> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_AdditionalDocumentReference>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    var item = new clsEntityDocument_AdditionalDocumentReference(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalDocumentReference cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
            }
            return resultado;
        }
    }
}
