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
    [ProgId("clsEntityDocument_Line")]
    public class clsEntityDocument_Line : clsBaseEntidad
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
        public string Cs_tag_Price_PriceAmount{ get; set; }
        public string Cs_tag_AllowanceCharge_ChargeIndicator { get; set; }
        public string Cs_tag_AllowanceCharge_Amount { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_Line cs_pxObtenerUnoPorId(string id)
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
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_Line(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_Line";        
            cs_cmCampos.Add("cs_Document_Line_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 11; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Line";
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 11; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_Line()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_Line";
            cs_cmCampos.Add("cs_Document_Line_Id");
            cs_cmCampos.Add("cs_Document_Id");
            for (int i = 1; i < 11; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Line";
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            cs_cmCampos_min.Add("cs_Document_Id");
            for (int i = 1; i < 11; i++)//Número de campos
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line cs_pxObtenerTodoPorId " + ex.ToString());
                return null;
            }
        }
        public string cs_pxObtenerIdRelacionadoDescripcionUltimo(string idDiferente,string codigoProducto)
        {
            string tabla_contenidos= string.Empty;
            try
            {         
                OdbcDataReader datos = null;
                string sql = "SELECT TOP 1 cs_Document_Line_Id FROM " + cs_cmTabla + " WHERE cs_Document_Id !=" + idDiferente.ToString().Trim() + " AND cp7='"+codigoProducto+ "' ORDER BY cs_Document_Line_Id DESC ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
               
                while (datos.Read())
                {
                    tabla_contenidos= datos[0].ToString().Trim();
                }
                cs_pxConexion_basedatos.Close();
               
            }
            catch (Exception)
            {
                tabla_contenidos = string.Empty;
            }
            return tabla_contenidos;
        }

        public List<clsEntityDocument_Line> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_Line>();
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
                    var item = new clsEntityDocument_Line(localDB);
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

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;
            contenido += ei + "(Tabla: clsEntityDocument )" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cedetalle_id : " + Cs_pr_Document_Line_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Line_Id)) + "]" + ef;
            contenido += ei + "Cecabecera_id: " + Cs_pr_Document_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Id)) + "]" + ef;
            contenido += ei + "InvoiceLine_ID : " + Cs_tag_InvoiceLine_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_n_3(Cs_tag_InvoiceLine_ID)) + "]" + ef;
            contenido += ei + "InvoiceLine_InvoicedQuantity_unitCode : " + Cs_tag_InvoicedQuantity_unitCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_3(Cs_tag_InvoicedQuantity_unitCode)) + "]" + ef;
            contenido += ei + "InvoiceLine_InvoicedQuantity : " + Cs_tag_invoicedQuantity + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_16_F_n12c3(Cs_tag_invoicedQuantity)) + "]" + ef;
            contenido += ei + "InvoiceLine_LineExtensionAmount_currencyID  : " + Cs_tag_LineExtensionAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(Cs_tag_LineExtensionAmount_currencyID)) + "]" + ef;
            contenido += ei + "InvoiceLine_AllowanceCharge_ChargeIndicator  : " + Cs_tag_AllowanceCharge_ChargeIndicator + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15(Cs_tag_AllowanceCharge_ChargeIndicator)) + "]" + ef;
            contenido += ei + "InvoiceLine_AllowanceCharge_Amount  : " + Cs_tag_AllowanceCharge_Amount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(Cs_tag_AllowanceCharge_Amount)) + "]" + ef;
            //contenido += ei + "InvoiceLine_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID : " + InvoiceLine_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(InvoiceLine_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID)) + "]" + ef;
            //contenido += ei + "InvoiceLine_PricingReference_AlternativeConditionPrice_PriceTypeCode : " + InvoiceLine_PricingReference_AlternativeConditionPrice_PriceTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(InvoiceLine_PricingReference_AlternativeConditionPrice_PriceTypeCode)) + "]" + ef;
            contenido += ei + "InvoiceLine_Item_SellersItemIdentification : " + Cs_tag_Item_SellersItemIdentification + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(Cs_tag_Item_SellersItemIdentification)) + "]" + ef;
            contenido += ei + "InvoiceLine_Price_PriceAmount : " + Cs_tag_Price_PriceAmount + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(Cs_tag_Price_PriceAmount)) + "]" + ef;
            return contenido;
        }
    }
}
