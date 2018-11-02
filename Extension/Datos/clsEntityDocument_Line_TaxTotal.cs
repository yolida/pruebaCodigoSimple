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
    [ProgId("clsEntityDocument_Line_TaxTotal")]
    public class clsEntityDocument_Line_TaxTotal : clsBaseEntidad
    {
        public string Cs_pr_Document_Line_TaxTotal_Id { get; set; }
        public string Cs_pr_Document_Line_Id { get; set; }
        public string Cs_tag_TaxAmount_currencyID { get; set; }
        public string Cs_tag_TaxSubtotal_TaxAmount_currencyID { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TierRange { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name { get; set; }
        public string Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        public clsEntityDocument_Line_TaxTotal cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_Line_TaxTotal_Id = valores[0];
                Cs_pr_Document_Line_Id = valores[1];
                Cs_tag_TaxAmount_currencyID = valores[2];
                Cs_tag_TaxSubtotal_TaxAmount_currencyID = valores[3];
                Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = valores[4];
                Cs_tag_TaxSubtotal_TaxCategory_TierRange = valores[5];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = valores[6];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = valores[7];
                Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = valores[8];
                return this;
            }
            else
            {
                return null;
            }
           
        }

        public clsEntityDocument_Line_TaxTotal(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Document_Line_TaxTotal";
            cs_cmCampos.Add("cs_Document_Line_TaxTotal_Id");
            cs_cmCampos.Add("cs_Document_Line_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
           
            cs_cmTabla_min = "cs_Document_Line_TaxTotal";
            cs_cmCampos_min.Add("cs_Document_Line_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDocument_Line_TaxTotal()
        {
            //localDB = local;
            cs_cmTabla = "cs_Document_Line_TaxTotal";
            cs_cmCampos.Add("cs_Document_Line_TaxTotal_Id");
            cs_cmCampos.Add("cs_Document_Line_Id");
            for (int i = 1; i < 8; i++)//Número de campos
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Document_Line_TaxTotal";
            cs_cmCampos_min.Add("cs_Document_Line_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_Document_Line_Id");
            for (int i = 1; i < 8; i++)//Número de campos
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
            cs_cmValores.Add(Cs_pr_Document_Line_TaxTotal_Id);
            cs_cmValores.Add(Cs_pr_Document_Line_Id);
            cs_cmValores.Add(Cs_tag_TaxAmount_currencyID);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxAmount_currencyID);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TierRange);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line_TaxTotal cs_pxObtenerTodoPorId" + ex.ToString());
                return null;
            }
        }

        public List<clsEntityDocument_Line> cs_pxObtenerTodoPorCabeceraId(string id)
        {
            List<clsEntityDocument_Line> comprobante_detalle;
            try
            {
                comprobante_detalle = new List<clsEntityDocument_Line>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Document_Line_Id=" + id.ToString().Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                
                clsEntityDocument_Line detalle;

                while (datos.Read())
                {

                    detalle = new clsEntityDocument_Line(localDB);/*
                    detalle.ce_id = datos[0].ToString().Trim();
                    detalle.ce_id_cabecera = datos[1].ToString().Trim();
                    detalle.ce_numeroorden = datos[2].ToString().Trim();
                    detalle.ce_unidadmedida = datos[3].ToString().Trim();
                    detalle.ce_cantidad = datos[4].ToString().Trim();
                    detalle.ce_descripcion = datos[5].ToString().Trim();
                    detalle.ce_valorunitario = datos[6].ToString().Trim();
                    detalle.ce_precioventaunitario = datos[7].ToString().Trim();
                    detalle.ce_precioventaunitario_tipoprecio = datos[8].ToString().Trim();
                    detalle.ce_igv_monto = datos[9].ToString().Trim();
                    detalle.ce_igv_subtotal = datos[10].ToString().Trim();
                    detalle.ce_igv_afectacion_cat7 = datos[11].ToString().Trim();
                    detalle.ce_igv_codigotributo_cat5 = datos[12].ToString().Trim();
                    detalle.ce_igv_nombretributo_cat5 = datos[13].ToString().Trim();
                    detalle.ce_igv_codigointernacionaltributo_cat5 = datos[14].ToString().Trim();
                    detalle.ce_isc_monto = datos[15].ToString().Trim();
                    detalle.ce_isc_subtotal = datos[16].ToString().Trim();
                    detalle.ce_isc_tiposistema_cat8 = datos[17].ToString().Trim();
                    detalle.ce_isc_codigotributo_cat5 = datos[18].ToString().Trim();
                    detalle.ce_isc_nombretributo_cat5 = datos[19].ToString().Trim();
                    detalle.ce_isc_codigointernacionaltributo_cat5 = datos[20].ToString().Trim();
                    detalle.ce_valorventa = datos[21].ToString().Trim();
                    detalle.ce_codigoproducto = datos[22].ToString().Trim();
                    detalle.ce_valorreferencial_monto = datos[23].ToString().Trim();
                    detalle.ce_valorreferencial_codigo = datos[24].ToString().Trim();*/
                    comprobante_detalle.Add(detalle);
                }
                cs_pxConexion_basedatos.Close();
                return comprobante_detalle;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line_TaxTotal cs_pxObtenerTodoPorCabeceraId" + ex.ToString());
                return null;
            }
        }

        public override string cs_pxValidarReporte()
        {
            string ei = "    ";
            string ef = "\r\n";
            string contenido = "" + ef;

            contenido += ei + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto)" + ef;
            contenido += ei + "====================================================================================" + ef;
            contenido += ei + "Cedetalle_informaciondeimpuesto_id  : " + Cs_pr_Document_Line_TaxTotal_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Line_TaxTotal_Id)) + "]" + ef;
            contenido += ei + "Cedetalle_id  : " + Cs_pr_Document_Line_Id + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_ID(Cs_pr_Document_Line_Id)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxAmount_currencyID  : " + Cs_tag_TaxAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(Cs_tag_TaxAmount_currencyID)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxAmount_currencyID  : " + Cs_tag_TaxSubtotal_TaxAmount_currencyID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(Cs_tag_TaxSubtotal_TaxAmount_currencyID)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxCategory_TaxExemptionReasonCode  : " + Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxCategory_TierRange  : " + Cs_tag_TaxSubtotal_TaxCategory_TierRange + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(Cs_tag_TaxSubtotal_TaxCategory_TierRange)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_ID   : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_Name  : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an_6(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name)) + "]" + ef;
            contenido += ei + "InvoiceLine_TaxTotal_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode  : " + Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode + " [" + clsNegocioValidar_Campos.Mensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode)) + "]" + ef;

            return contenido;
        }

        public List<clsEntityDocument_Line_TaxTotal> cs_fxObtenerTodoPorCabeceraId(string id)
        {
            var resultado = new List<clsEntityDocument_Line_TaxTotal>();
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
                    var item = new clsEntityDocument_Line_TaxTotal(localDB);
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
                clsBaseLog.cs_pxRegistarAdd("clsEntityDocument_Line_TaxTotal cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
            }
            return resultado;
        }
    }
}
