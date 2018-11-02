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
    [ProgId("clsEntityDespatch_Line")]
    public class clsEntityDespatch_Line:clsBaseEntidad
    {
        public string Cs_pr_Despatch_Line_ID { get; set; }
        public string Cs_pr_Despatch_ID { get; set; }
        public string Cs_tag_OrderLineReference { get; set; }
        public string Cs_tag_DeliveredQuantity { get; set; }
        public string Cs_tag_DeliveredQuantity_UnitCode { get; set; }
        public string Cs_tag_ItemName { get; set; }
        public string Cs_tag_Item_SellersItemIdentification_ID { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_Line_ID);
            cs_cmValores.Add(Cs_pr_Despatch_ID);
            cs_cmValores.Add(Cs_tag_OrderLineReference);//1
            cs_cmValores.Add(Cs_tag_DeliveredQuantity);//2
            cs_cmValores.Add(Cs_tag_DeliveredQuantity_UnitCode);//3
            cs_cmValores.Add(Cs_tag_ItemName);//4
            cs_cmValores.Add(Cs_tag_Item_SellersItemIdentification_ID);//5

        }
        public clsEntityDespatch_Line(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch_Line";
            cs_cmCampos.Add("cs_Despatch_Line_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 5; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_Line";
            cs_cmCampos_min.Add("cs_Despatch_Line_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 5; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch_Line()
        {
           // localDB = local;
            cs_cmTabla = "cs_Despatch_Line";
            cs_cmCampos.Add("cs_Despatch_Line_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 5; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_Line";
            cs_cmCampos_min.Add("cs_Despatch_Line_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 5; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityDespatch_Line cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_Line_ID = valores[0];
                Cs_pr_Despatch_ID = valores[1];
                Cs_tag_OrderLineReference = valores[2];
                Cs_tag_DeliveredQuantity = valores[3];
                Cs_tag_DeliveredQuantity_UnitCode = valores[4];
                Cs_tag_ItemName = valores[5];
                Cs_tag_Item_SellersItemIdentification_ID = valores[6];
                return this;
            }
            else
            {
                return null;
            }
        }
           
        internal List<clsEntityDespatch_Line> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityDespatch_Line> List_DespatchLine;
            try
            {
                List_DespatchLine = new List<clsEntityDespatch_Line>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Despatch_Id=" + Id.Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntityDespatch_Line DespatchLine;
                while (datos.Read())
                {
                    DespatchLine = new clsEntityDespatch_Line(localDB);
                    DespatchLine.Cs_pr_Despatch_Line_ID = datos[0].ToString();
                    DespatchLine.Cs_pr_Despatch_ID = datos[1].ToString();
                    DespatchLine.Cs_tag_OrderLineReference = datos[2].ToString();
                    DespatchLine.Cs_tag_DeliveredQuantity = datos[3].ToString();
                    DespatchLine.Cs_tag_DeliveredQuantity_UnitCode = datos[4].ToString();
                    DespatchLine.Cs_tag_ItemName = datos[5].ToString();
                    DespatchLine.Cs_tag_Item_SellersItemIdentification_ID = datos[6].ToString();
                    List_DespatchLine.Add(DespatchLine);
                }
                cs_pxConexion_basedatos.Close();
                return List_DespatchLine;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDespatch_Line " + ex.ToString());
                return null;
            }
        }

    }
}
