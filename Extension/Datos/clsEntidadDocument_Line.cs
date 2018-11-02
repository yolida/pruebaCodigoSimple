using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument_Line : clsBaseEntidad
    {
        public string Cs_pr_Document_Line_Id { get; set; }
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_InvoiceLine_ID { get; set; }
        public string Cs_tag_InvoicedQuantity_unitCode { get; set; }///>>asdasdrevisar 2000
        public string Cs_tag_invoicedQuantity { get; set; }
        public string Cs_tag_LineExtensionAmount_currencyID { get; set; }
        public string Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID { get; set; }
        public string Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode { get; set; }
        public string Cs_tag_Item_SellersItemIdentification { get; set; }
        public string Cs_tag_Price_PriceAmount { get; set; }
        public string Cs_tag_AllowanceCharge_ChargeIndicator { get; set; }
        public string Cs_tag_AllowanceCharge_Amount { get; set; }
        //private clsEntityDatabaseLocal localDB;
        //adicionaes
        public string Cs_cr_TransferenciaGratuitaMotivo { get; set; }
        public string Cs_cr_TransferenciaGratuitaValorReferencia { get; set; }
        public string Cs_cr_UnidadMedida { get; set; }
        /// <summary>
        /// Metodo para obtener un registro segun id de la linea
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument_Line cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_Line_Id = valores[0];
                Cs_pr_Document_Id = valores[1];
                Cs_tag_InvoiceLine_ID = valores[2];
                Cs_tag_InvoicedQuantity_unitCode = valores[3];
                Cs_tag_invoicedQuantity = valores[4];
                Cs_tag_LineExtensionAmount_currencyID = valores[5];
                Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID = valores[6];
                Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode = valores[7];
                Cs_tag_Item_SellersItemIdentification = valores[8];
                Cs_tag_Price_PriceAmount = valores[9];
                Cs_tag_AllowanceCharge_ChargeIndicator = valores[10];
                Cs_tag_AllowanceCharge_Amount = valores[11];
                Cs_cr_TransferenciaGratuitaMotivo = valores[12];
                Cs_cr_TransferenciaGratuitaValorReferencia = valores[13];
                Cs_cr_UnidadMedida = valores[14];
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
        public clsEntidadDocument_Line(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_cDocument_Line";
            cs_cmCampos.Add("cs_Document_Line_Id");
            cs_cmCampos.Add("cs_Document_Id");
            cs_cmCampos.Add("Cs_tag_InvoiceLine_ID");
            cs_cmCampos.Add("Cs_tag_InvoicedQuantity_unitCode");
            cs_cmCampos.Add("Cs_tag_invoicedQuantity");
            cs_cmCampos.Add("Cs_tag_LineExtensionAmount_currencyID");
            cs_cmCampos.Add("Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID");
            cs_cmCampos.Add("Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode");
            cs_cmCampos.Add("Cs_tag_Item_SellersItemIdentification");
            cs_cmCampos.Add("Cs_tag_Price_PriceAmount");
            cs_cmCampos.Add("Cs_tag_AllowanceCharge_ChargeIndicator");
            cs_cmCampos.Add("Cs_tag_AllowanceCharge_Amount");
            cs_cmCampos.Add("Cs_cr_TransferenciaGratuitaMotivo");
            cs_cmCampos.Add("Cs_cr_TransferenciaGratuitaValorReferencia");
            cs_cmCampos.Add("Cs_cr_UnidadMedida");

            cs_cmTabla_min = "cs_cDocument_Line";
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            cs_cmCampos_min.Add("Cs_tag_InvoiceLine_ID");
            cs_cmCampos_min.Add("Cs_tag_InvoicedQuantity_unitCode");
            cs_cmCampos_min.Add("Cs_tag_invoicedQuantity");
            cs_cmCampos_min.Add("Cs_tag_LineExtensionAmount_currencyID");
            cs_cmCampos_min.Add("Cs_tag_PricRef_AlternativeConditionPrice_PriceAmount_currencyID");//Adecuado para MySQL
            cs_cmCampos_min.Add("Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode");
            cs_cmCampos_min.Add("Cs_tag_Item_SellersItemIdentification");
            cs_cmCampos_min.Add("Cs_tag_Price_PriceAmount");
            cs_cmCampos_min.Add("Cs_tag_AllowanceCharge_ChargeIndicator");
            cs_cmCampos_min.Add("Cs_tag_AllowanceCharge_Amount");
            cs_cmCampos_min.Add("Cs_cr_TransferenciaGratuitaMotivo");
            cs_cmCampos_min.Add("Cs_cr_TransferenciaGratuitaValorReferencia");
            cs_cmCampos_min.Add("Cs_cr_UnidadMedida");
        }
        /// <summary>
        /// Metodo para actualizar los valores de la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_Line_Id);
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_InvoiceLine_ID);
            cs_cmValores.Add(Cs_tag_InvoicedQuantity_unitCode);
            cs_cmValores.Add(Cs_tag_invoicedQuantity);
            cs_cmValores.Add(Cs_tag_LineExtensionAmount_currencyID);
            cs_cmValores.Add(Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID);
            cs_cmValores.Add(Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode);
            cs_cmValores.Add(Cs_tag_Item_SellersItemIdentification);
            cs_cmValores.Add(Cs_tag_Price_PriceAmount);
            cs_cmValores.Add(Cs_tag_AllowanceCharge_ChargeIndicator);
            cs_cmValores.Add(Cs_tag_AllowanceCharge_Amount);
            cs_cmValores.Add(Cs_cr_TransferenciaGratuitaMotivo);
            cs_cmValores.Add(Cs_cr_TransferenciaGratuitaValorReferencia);
            cs_cmValores.Add(Cs_cr_UnidadMedida);
        }
        /// <summary>
        /// Metodo para obtner todas las lienas en un list de list segun id del documento 
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line cs_pxObtenerTodoPorId " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para obtener listado de objetos de lineas segun id del documento 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<clsEntidadDocument_Line> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            List<clsEntidadDocument_Line> resultado = new List<clsEntidadDocument_Line>();
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
                    int contador = 0;
                    var item = new clsEntidadDocument_Line(localDB);
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        prop.SetValue(item, Convert.ChangeType(datos[contador].ToString(), prop.PropertyType), null);
                        contador++;
                    }
                    resultado.Add(item);
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
