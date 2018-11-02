using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Runtime.InteropServices;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDocument_AdditionalComments")]
    public class clsEntityDocument_AdditionalComments:clsBaseEntidad
    {
        public string Cs_pr_Document_AdditionalComments_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_pr_Document_AdditionalComments_Reference_Id { get; set; }
        public string Cs_pr_TagNombre { get; set; }
        public string Cs_pr_TagValor { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_AdditionalComments_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_pr_Document_AdditionalComments_Reference_Id);
            cs_cmValores.Add(Cs_pr_TagNombre);
            cs_cmValores.Add(Cs_pr_TagValor);
        }

        public clsEntityDocument_AdditionalComments(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_AdditionalComments";
            cs_cmCampos.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_AdditionalComments";
            cs_cmCampos_min.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_AdditionalComments()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_AdditionalComments";
            cs_cmCampos.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_AdditionalComments";
            cs_cmCampos_min.Add("cs_Document_AdditionalComments_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }

        public clsEntityDocument_AdditionalComments cs_fxObtenerUnoPorId(string id)
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

        public List<clsEntityDocument_AdditionalComments> cs_fxObtenerRaizXML(string id)
        {
            List<clsEntityDocument_AdditionalComments> resultado = new List<clsEntityDocument_AdditionalComments>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Id =" + id.ToString().Trim() + " AND cp1='';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDocument_AdditionalComments item;
                while (datos.Read())
                {
                    item = new clsEntityDocument_AdditionalComments(localDB);
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

        public List<clsEntityDocument_AdditionalComments> cs_fxObtenerTodoPorIdReferencia(string id)
        {
            List<clsEntityDocument_AdditionalComments> resultado = new List<clsEntityDocument_AdditionalComments>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp1 ='" + id.ToString().Trim() + "';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDocument_AdditionalComments item;
                while (datos.Read())
                {
                    item = new clsEntityDocument_AdditionalComments(localDB);
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


        public string cs_fxObtenerXML(string Id)
        {
            string resultado = string.Empty;
            try
            {
                List<clsEntityDocument_AdditionalComments> Nodos = new clsEntityDocument_AdditionalComments(localDB).cs_fxObtenerRaizXML(Id);
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

        private string cs_fxBusquedaEnProfundidad(string Id)
        {
            //fe-955
            string resultado = string.Empty;
            try{
                List<clsEntityDocument_AdditionalComments> Nodos = new clsEntityDocument_AdditionalComments(localDB).cs_fxObtenerTodoPorIdReferencia(Id);
                foreach (var Nodo in Nodos)
                {
                    resultado += "<" + Nodo.Cs_pr_TagNombre + "><![CDATA[";
                    resultado += Nodo.Cs_pr_TagValor + cs_fxBusquedaEnProfundidad(Nodo.Cs_pr_Document_AdditionalComments_Id);
                    resultado += "]]></" + Nodo.Cs_pr_TagNombre + ">";
                }
            }
            catch (Exception ex){
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_AdditionalComments cs_fxBusquedaEnProfundidad" + ex.ToString());
            }
           
            return resultado;
        }
        public List<clsEntityDocument_AdditionalComments> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_AdditionalComments>();
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
                    var item = new clsEntityDocument_AdditionalComments(localDB);
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
