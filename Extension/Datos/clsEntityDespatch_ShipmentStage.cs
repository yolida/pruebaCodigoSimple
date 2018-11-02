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
    [ProgId("clsEntityDespatch_ShipmentStage")]
    public class clsEntityDespatch_ShipmentStage : clsBaseEntidad
    {
        public string Cs_pr_Despatch_Shipment_ID { get; set; }
        public string Cs_pr_Despatch_ID { get; set; }
        public string Cs_tag_ship_ShipStage_TraModeCode { get; set; }//1
        public string Cs_tag_ship_ShipStage_TransitPeriod_StartDate { get; set; }//2
        public string Cs_tag_ship_ShipStage_CarrierParty_PartyID_ID { get; set; }//3
        public string Cs_tag_ship_ShipStage_CarrierParty_PartyID_SchemeID { get; set; }//4
        public string Cs_tag_ship_ShipStage_CarrierParty_PartyName { get; set; }//5
        public string Cs_tag_ship_ShipStage_TransportMeans_LicencePlateID { get; set; }//6
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_Shipment_ID);
            cs_cmValores.Add(Cs_pr_Despatch_ID);
            cs_cmValores.Add(Cs_tag_ship_ShipStage_TraModeCode);//1
            cs_cmValores.Add(Cs_tag_ship_ShipStage_TransitPeriod_StartDate);//2
            cs_cmValores.Add(Cs_tag_ship_ShipStage_CarrierParty_PartyID_ID);//3
            cs_cmValores.Add(Cs_tag_ship_ShipStage_CarrierParty_PartyID_SchemeID);//4
            cs_cmValores.Add(Cs_tag_ship_ShipStage_CarrierParty_PartyName);//5
            cs_cmValores.Add(Cs_tag_ship_ShipStage_TransportMeans_LicencePlateID);//6           
        }
        public clsEntityDespatch_ShipmentStage(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch_ShipmentStage";
            cs_cmCampos.Add("cs_Despatch_ShipStage_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 6; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_ShipmentStage";
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 6; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch_ShipmentStage()
        {
            //localDB = local;
            cs_cmTabla = "cs_Despatch_ShipmentStage";
            cs_cmCampos.Add("cs_Despatch_ShipStage_Id");
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 6; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_ShipmentStage";
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_Id");
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 6; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityDespatch_ShipmentStage cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_Shipment_ID = valores[0];
                Cs_pr_Despatch_ID = valores[1];
                Cs_tag_ship_ShipStage_TraModeCode = valores[2];
                Cs_tag_ship_ShipStage_TransitPeriod_StartDate = valores[3];
                Cs_tag_ship_ShipStage_CarrierParty_PartyID_ID = valores[4];
                Cs_tag_ship_ShipStage_CarrierParty_PartyID_SchemeID = valores[5];
                Cs_tag_ship_ShipStage_CarrierParty_PartyName = valores[6];
                Cs_tag_ship_ShipStage_TransportMeans_LicencePlateID = valores[7];
                return this;
            }
            else
            {
                return null;
            }
           
        }
        internal List<clsEntityDespatch_ShipmentStage> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityDespatch_ShipmentStage> List_ShipmentStage;
            try
            {
                List_ShipmentStage = new List<clsEntityDespatch_ShipmentStage>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Despatch_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDespatch_ShipmentStage ShipmentStage_Line;
                while (datos.Read())
                {
                    ShipmentStage_Line = new clsEntityDespatch_ShipmentStage(localDB);
                    ShipmentStage_Line.Cs_pr_Despatch_Shipment_ID = datos[0].ToString();
                    ShipmentStage_Line.Cs_pr_Despatch_ID = datos[1].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_TraModeCode = datos[2].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_TransitPeriod_StartDate = datos[3].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_CarrierParty_PartyID_ID = datos[4].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_CarrierParty_PartyID_SchemeID = datos[5].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_CarrierParty_PartyName = datos[6].ToString();
                    ShipmentStage_Line.Cs_tag_ship_ShipStage_TransportMeans_LicencePlateID = datos[7].ToString();
                    List_ShipmentStage.Add(ShipmentStage_Line);
                }
                cs_pxConexion_basedatos.Close();
                return List_ShipmentStage;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDespatch_ShipmentStage cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
                return null;
            }
        }
    }
}
