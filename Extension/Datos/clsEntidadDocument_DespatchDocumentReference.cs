using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument_DespatchDocumentReference : clsBaseEntidad
    {
        public string Cs_pr_Document_DespatchDocumentReference_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_DespatchDocumentReference_ID { get; set; }
        public string Cs_tag_DocumentTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        /// <summary>
        /// Metodo para obtner un registro segun id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument_DespatchDocumentReference cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_DespatchDocumentReference_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                Cs_tag_DespatchDocumentReference_ID = valores[2];
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
        public clsEntidadDocument_DespatchDocumentReference(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_cDocument_DespatchDocumentReference";
            cs_cmCampos.Add("cs_Document_DespatchDocumentReference_Id");
            cs_cmCampos.Add("cs_Document_Id");
            cs_cmCampos.Add("Cs_tag_DespatchDocumentReference_ID");
            cs_cmCampos.Add("Cs_tag_DocumentTypeCode");

            cs_cmTabla_min = "cs_cDocument_DespatchDocumentReference";
            cs_cmCampos_min.Add("cs_Document_DespatchDocumentReference_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            cs_cmCampos_min.Add("Cs_tag_DespatchDocumentReference_ID");
            cs_cmCampos_min.Add("Cs_tag_DocumentTypeCode");

        }
        /// <summary>
        /// Metodo para actualizar los valores en la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_DespatchDocumentReference_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_DespatchDocumentReference_ID);
            cs_cmValores.Add(Cs_tag_DocumentTypeCode);
        }
        /// <summary>
        /// mMetodo para obtener todos los docs refrenciados segun id del documento principal
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_DespatchDocumentReference cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Obtner los documentos guias por id del documento principal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_DespatchDocumentReference> cs_pxObtenerTodoPorCabeceraId(string id)
        {
            List<clsEntidadDocument_DespatchDocumentReference> guia_remision;
            try
            {
                guia_remision = new List<clsEntidadDocument_DespatchDocumentReference>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id=" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntidadDocument_DespatchDocumentReference entidad;
                while (datos.Read())
                {
                    entidad = new clsEntidadDocument_DespatchDocumentReference(localDB);
                    entidad.Cs_pr_Document_DespatchDocumentReference_Id = cs_cmValores[0];
                    entidad.Cs_tag_DespatchDocumentReference_ID = cs_cmValores[1];
                    entidad.Cs_tag_DocumentTypeCode = cs_cmValores[2];
                    guia_remision.Add(entidad);
                }
                cs_pxConexion_basedatos.Close();
                return guia_remision;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_DespatchDocumentReference cs_pxObtenerTodoPorCabeceraId" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Obtener listado de comprobantes rferenciados segun id del comprobante principal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_DespatchDocumentReference> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntidadDocument_DespatchDocumentReference>();
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
                    var item = new clsEntidadDocument_DespatchDocumentReference(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_DespatchDocumentReference cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }

}
