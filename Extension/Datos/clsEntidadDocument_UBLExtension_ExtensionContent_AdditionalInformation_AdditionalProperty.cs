using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty : clsBaseEntidad
    {
        public string Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_Name { get; set; }
        public string Cs_tag_Value { get; set; }

        //private clsEntityDatabaseLocal localDB;
        /// <summary>
        /// Metodo para obtener un registro segun el id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
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
        /// <summary>
        /// Metodo constructor
        /// </summary>
        /// <param name="local"></param>
        public clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_cDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty";
            cs_cmCampos.Add("cs_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id");
            cs_cmCampos.Add("cs_Document_Id");
            cs_cmCampos.Add("Cs_tag_ID");
            cs_cmCampos.Add("Cs_tag_Name");
            cs_cmCampos.Add("Cs_tag_Value");


            cs_cmTabla_min = "cs_cDoc_UBLExt_ExtContent_AdditInformation_AdditProperty";
            cs_cmCampos_min.Add("cs_Doc_UBLExt_ExtContent_AddiInformation_AdditProperty_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            cs_cmCampos_min.Add("Cs_tag_ID");
            cs_cmCampos_min.Add("Cs_tag_Name");
            cs_cmCampos_min.Add("Cs_tag_Value");
        }
        /// <summary>
        /// Metodo para actualizar los valores de la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_ID);
            cs_cmValores.Add(Cs_tag_Name);
            cs_cmValores.Add(Cs_tag_Value);
        }
        /// <summary>
        /// Metodo paraobtener el valor de un campo segun el id del documento
        /// </summary>
        /// <param name="TagId"></param>
        /// <param name="DocumentoId"></param>
        /// <param name="valorObtener"></param>
        /// <returns></returns>
        public string cs_pxObtenerValorPorTagIdAndDocumentoId(string TagId, string DocumentoId, string valorObtener)
        {
            string resultado = string.Empty;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT " + valorObtener + " FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + DocumentoId.ToString().Trim() + " AND Cs_tag_ID='" + TagId + "' ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    resultado = datos[0].ToString().Trim();
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }
            return resultado;
        }
        /// <summary>
        /// Metodo para obtener un listado de los aditional property segun id del documento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<List<string>> cs_pxObtenerTodoPorId(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
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
                return tabla_contenidos;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty  cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para obtener un listado de objetos segun el id del documento 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty>();
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
                    var item = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB);
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
