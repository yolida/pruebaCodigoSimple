using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument_AdditionalDocumentReference : clsBaseEntidad
    {
        public string Cs_pr_Document_AdditionalDocumentReference_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_AdditionalDocumentReference_ID { get; set; }
        public string Cs_tag_DocumentTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        /// <summary>
        /// Metodo para obtner un registro segun id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument_AdditionalDocumentReference cs_pxObtenerUnoPorId(string id)
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
        /// <summary>
        /// Metodo constructor
        /// </summary>
        /// <param name="local"></param>
        public clsEntidadDocument_AdditionalDocumentReference(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_cDocument_AdditionalDocumentReference";
            cs_cmCampos.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos.Add("cs_Document_Id");
            cs_cmCampos.Add("Cs_tag_AdditionalDocumentReference_ID");
            cs_cmCampos.Add("Cs_tag_DocumentTypeCode");

            cs_cmTabla_min = "cs_cDocument_AdditionalDocumentReference";
            cs_cmCampos_min.Add("cs_Document_AdditionalDocumentReference_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            cs_cmCampos_min.Add("Cs_tag_AdditionalDocumentReference_ID");
            cs_cmCampos_min.Add("Cs_tag_DocumentTypeCode");

        }
        /// <summary>
        /// Actualizacion de valores a la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_AdditionalDocumentReference_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_AdditionalDocumentReference_ID);
            cs_cmValores.Add(Cs_tag_DocumentTypeCode);
        }
        /// <summary>
        /// Obtner todos los documentos referenciados segun id del documento principal
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
        /// <summary>
        /// Obtner todos los comprobantes relacionados segun id del documento principal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_AdditionalDocumentReference> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntidadDocument_AdditionalDocumentReference>();
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
                    var item = new clsEntidadDocument_AdditionalDocumentReference(localDB);
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
