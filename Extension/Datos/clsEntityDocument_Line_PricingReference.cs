using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDocument_Line_PricingReference")]
    public class clsEntityDocument_Line_PricingReference : clsBaseEntidad
    {
        public string Cs_pr_Document_Line_PricingReference_Id { get; set; }
        public string Cs_pr_Document_Line_Id { get; set; }
        public string Cs_tag_PriceAmount_currencyID { get; set; }
        public string Cs_tag_PriceTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_Line_PricingReference cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_Line_PricingReference_Id = valores[0];
                Cs_pr_Document_Line_Id = valores[1];
                Cs_tag_PriceAmount_currencyID = valores[2];
                Cs_tag_PriceTypeCode = valores[3];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_Line_PricingReference(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_Line_PricingReference";          
            cs_cmCampos.Add("cs_Document_Line_PricingReference_ID");
            cs_cmCampos.Add("cs_Document_Line_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Line_PricingReference";
            cs_cmCampos_min.Add("cs_Document_Line_PricingReference_ID");
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        public clsEntityDocument_Line_PricingReference()
        {
           // localDB = local;
            cs_cmTabla = "cs_Document_Line_PricingReference";
            cs_cmCampos.Add("cs_Document_Line_PricingReference_ID");
            cs_cmCampos.Add("cs_Document_Line_Id");
            for (int i = 1; i < 3; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Line_PricingReference";
            cs_cmCampos_min.Add("cs_Document_Line_PricingReference_ID");
            cs_cmCampos_min.Add("cs_Document_Line_Id");
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
        public List<List<string>> cs_pxObtenerTodoPorId(string id)
        {
            List<List<string>> tabla_contenidos;
            try
            {
                tabla_contenidos = new List<List<string>>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Line_Id=" + id.ToString().Trim() + " ;";
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line_PricingReference cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_Line_PricingReference_Id);
            cs_cmValores.Add(Cs_pr_Document_Line_Id);
            cs_cmValores.Add(Cs_tag_PriceAmount_currencyID);
            cs_cmValores.Add(Cs_tag_PriceTypeCode);
        }
        
        public List<clsEntityDocument_Line_PricingReference> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_Line_PricingReference>();
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Line_Id =" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    var item = new clsEntityDocument_Line_PricingReference(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line_PricingReference cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
