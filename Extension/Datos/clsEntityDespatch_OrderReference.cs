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
    [ProgId("clsEntityDespatch_OrderReference")]
    public class clsEntityDespatch_OrderReference:clsBaseEntidad
    {
        public string Cs_pr_Despatch_OrderReference_ID { get; set; }
        public string Cs_pr_Despatch_ID { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_OrderTypeCode { get; set; }
        public string Cs_tag_OrderTypeCode_Name { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_OrderReference_ID);
            cs_cmValores.Add(Cs_pr_Despatch_ID);
            cs_cmValores.Add(Cs_tag_ID);//1
            cs_cmValores.Add(Cs_tag_OrderTypeCode);//2
            cs_cmValores.Add(Cs_tag_OrderTypeCode_Name);//3     
        }
        public clsEntityDespatch_OrderReference(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch_OrderReference";
            cs_cmCampos.Add("cs_Despatch_OrderReference_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_OrderReference";
            cs_cmCampos_min.Add("cs_Despatch_OrderReference_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch_OrderReference()
        {
            //localDB = local;
            cs_cmTabla = "cs_Despatch_OrderReference";
            cs_cmCampos.Add("cs_Despatch_OrderReference_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_OrderReference";
            cs_cmCampos_min.Add("cs_Despatch_OrderReference_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
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
        public clsEntityDespatch_OrderReference cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_OrderReference_ID = valores[0];
                Cs_pr_Despatch_ID = valores[1];
                Cs_tag_ID = valores[2];
                Cs_tag_OrderTypeCode = valores[3];
                Cs_tag_OrderTypeCode_Name = valores[4];
                return this;
            }
            else
            {
                return null;
            }
            
        }
        internal List<clsEntityDespatch_OrderReference> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityDespatch_OrderReference> List_ShipmentOrder;
            try
            {
                List_ShipmentOrder = new List<clsEntityDespatch_OrderReference>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Despatch_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDespatch_OrderReference ShipmentOrder_Line;
                while (datos.Read())
                {
                    ShipmentOrder_Line = new clsEntityDespatch_OrderReference(localDB);
                    ShipmentOrder_Line.Cs_pr_Despatch_OrderReference_ID = datos[0].ToString();
                    ShipmentOrder_Line.Cs_pr_Despatch_ID = datos[1].ToString();
                    ShipmentOrder_Line.Cs_tag_ID = datos[2].ToString();
                    ShipmentOrder_Line.Cs_tag_OrderTypeCode = datos[3].ToString();
                    ShipmentOrder_Line.Cs_tag_OrderTypeCode_Name = datos[4].ToString();
                    List_ShipmentOrder.Add(ShipmentOrder_Line);
                }
                cs_pxConexion_basedatos.Close();
                return List_ShipmentOrder;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDespatch_OrderReference cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
                return null;
            }
        }

    }
}
