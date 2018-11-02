using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument_AdditionalComments : clsBaseEntidad
    {
        public string Cs_pr_Document_AdditionalComments_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_pr_Document_AdditionalComments_Reference_Id { get; set; }
        public string Cs_pr_TagNombre { get; set; }
        public string Cs_pr_TagValor { get; set; }

        /// <summary>
        /// Metodo para actualizar valores en la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_AdditionalComments_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_pr_Document_AdditionalComments_Reference_Id);
            cs_cmValores.Add(Cs_pr_TagNombre);
            cs_cmValores.Add(Cs_pr_TagValor);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="local"></param>
        public clsEntidadDocument_AdditionalComments(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_cDocument_AdditionalComments";
            cs_cmCampos.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos.Add("cs_Document_Id");
            cs_cmCampos.Add("Cs_pr_Document_AdditionalComments_Reference_Id");
            cs_cmCampos.Add("Cs_pr_TagNombre");
            cs_cmCampos.Add("Cs_pr_TagValor");

            cs_cmTabla_min = "cs_cDocument_AdditionalComments";
            cs_cmCampos_min.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            cs_cmCampos_min.Add("Cs_pr_Document_AdditionalComments_Reference_Id");
            cs_cmCampos_min.Add("Cs_pr_TagNombre");
            cs_cmCampos_min.Add("Cs_pr_TagValor");

        }
        /// <summary>
        /// Obtener un registro segun id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument_AdditionalComments cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_AdditionalComments_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                Cs_pr_Document_AdditionalComments_Reference_Id = valores[2];
                Cs_pr_TagNombre = valores[3];
                Cs_pr_TagValor = valores[4];
                return this;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// Metodo para verificar la existencia de comentarios adicionales segun id de documento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool cs_fxVerificarExistencia(string id)
        {
            try
            {
                int cantidad = 0;
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    cantidad++;
                }
                cs_pxConexion_basedatos.Close();

                if (cantidad > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxVerificarExistencia" + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Metodo para obtener los comentarios asociados al principañ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_AdditionalComments> cs_fxObtenerRaizXML(string id)
        {
            List<clsEntidadDocument_AdditionalComments> resultado = new List<clsEntidadDocument_AdditionalComments>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + " AND Cs_pr_Document_AdditionalComments_Reference_Id='';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntidadDocument_AdditionalComments item;
                while (datos.Read())
                {
                    item = new clsEntidadDocument_AdditionalComments(localDB);
                    item.Cs_pr_Document_AdditionalComments_Id = datos[0].ToString();
                    item.Cs_pr_Document_Id = datos[1].ToString();
                    item.Cs_pr_Document_AdditionalComments_Reference_Id = datos[2].ToString();
                    item.Cs_pr_TagNombre = datos[3].ToString();
                    item.Cs_pr_TagValor = datos[4].ToString();
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxObtenerRaizXML" + ex.ToString());
            }
            return resultado;
        }
        /// <summary>
        /// Metodo par obener todos los comentarios adicionales segun el id de referencia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_AdditionalComments> cs_fxObtenerTodoPorIdReferencia(string id)
        {
            List<clsEntidadDocument_AdditionalComments> resultado = new List<clsEntidadDocument_AdditionalComments>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE Cs_pr_Document_AdditionalComments_Reference_Id ='" + id.ToString().Trim() + "';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntidadDocument_AdditionalComments item;
                while (datos.Read())
                {
                    item = new clsEntidadDocument_AdditionalComments(localDB);
                    item.Cs_pr_Document_AdditionalComments_Id = datos[0].ToString();
                    item.Cs_pr_Document_Id = datos[1].ToString();
                    item.Cs_pr_Document_AdditionalComments_Reference_Id = datos[2].ToString();
                    item.Cs_pr_TagNombre = datos[3].ToString();
                    item.Cs_pr_TagValor = datos[4].ToString();
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxObtenerTodoPorIdReferencia " + ex.ToString());
            }
            return resultado;
        }

        /// <summary>
        /// Metodo para obtener la cdena de xml segun id del comentario principal
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string cs_fxObtenerXML(string Id)
        {
            string resultado = string.Empty;
            try
            {
                List<clsEntidadDocument_AdditionalComments> Nodos = new clsEntidadDocument_AdditionalComments(localDB).cs_fxObtenerRaizXML(Id);
                foreach (var Nodo in Nodos)
                {
                    resultado += "<" + Nodo.Cs_pr_TagNombre + ">" + cs_fxBusquedaEnProfundidad(Nodo.Cs_pr_Document_AdditionalComments_Id) + "</" + Nodo.Cs_pr_TagNombre + ">";
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxObtenerXML" + ex.ToString());
            }

            return resultado;
        }
        /// <summary>
        /// Metodo para buscar dentro de los comentarios adicionales si hay dentro otros
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private string cs_fxBusquedaEnProfundidad(string Id)
        {
            //fe-955
            string resultado = string.Empty;
            try
            {
                List<clsEntidadDocument_AdditionalComments> Nodos = new clsEntidadDocument_AdditionalComments(localDB).cs_fxObtenerTodoPorIdReferencia(Id);
                foreach (var Nodo in Nodos)
                {
                    resultado += "<" + Nodo.Cs_pr_TagNombre + "><![CDATA[";
                    resultado += Nodo.Cs_pr_TagValor + cs_fxBusquedaEnProfundidad(Nodo.Cs_pr_Document_AdditionalComments_Id);
                    resultado += "]]></" + Nodo.Cs_pr_TagNombre + ">";
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxBusquedaEnProfundidad" + ex.ToString());
            }

            return resultado;
        }
        /// <summary>
        /// Metodo para obtener todos los comentarios aadicionales segun las cabeceras del documento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_AdditionalComments> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntidadDocument_AdditionalComments>();
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
                    var item = new clsEntidadDocument_AdditionalComments(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
            }
            return resultado;
        }

    }

}
