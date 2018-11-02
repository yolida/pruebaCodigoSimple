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
    [ProgId("clsEntityDespatch_ShipmentStage_Driver")]
    public class clsEntityDespatch_ShipmentStage_Driver:clsBaseEntidad
    {
        public string Cs_pr_Despatch_ShipStage_Driver_ID { get; set; }
        public string Cs_pr_Despatch_ShipStage_ID { get; set; }
        public string Cs_tag_Driver_ID { get; set; }//1
        public string Cs_tag_Driver_SchemaID { get; set; }//2    
        //private clsEntityDatabaseLocal localDB;  
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_ShipStage_Driver_ID);
            cs_cmValores.Add(Cs_pr_Despatch_ShipStage_ID);
            cs_cmValores.Add(Cs_tag_Driver_ID);//1
            cs_cmValores.Add(Cs_tag_Driver_SchemaID);//2      
        }
        public clsEntityDespatch_ShipmentStage_Driver(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch_ShipmentStage_Driver";
            cs_cmCampos.Add("cs_Despatch_ShipStage_Driver_ID");
            cs_cmCampos.Add("cs_Despatch_ShipStage_ID");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_ShipmentStage_Driver";
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_Driver_ID");
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_ID");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch_ShipmentStage_Driver()
        {
           // localDB = local;
            cs_cmTabla = "cs_Despatch_ShipmentStage_Driver";
            cs_cmCampos.Add("cs_Despatch_ShipStage_Driver_ID");
            cs_cmCampos.Add("cs_Despatch_ShipStage_ID");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_ShipmentStage_Driver";
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_Driver_ID");
            cs_cmCampos_min.Add("cs_Despatch_ShipStage_ID");
            for (int i = 1; i <= 2; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityDespatch_ShipmentStage_Driver cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_ShipStage_Driver_ID = valores[0];
                Cs_pr_Despatch_ShipStage_ID = valores[1];
                Cs_tag_Driver_ID = valores[2];
                Cs_tag_Driver_SchemaID = valores[3];
                return this;
            }
            else
            {
                return null;
            }
           
        }
        internal List<clsEntityDespatch_ShipmentStage_Driver> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityDespatch_ShipmentStage_Driver> List_ShipmentStageDrive;
            try
            {
                List_ShipmentStageDrive = new List<clsEntityDespatch_ShipmentStage_Driver>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Despatch_ShipStage_ID=" + Id.Trim() + ";";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDespatch_ShipmentStage_Driver ShipmentStageDriver_Line;
                while (datos.Read())
                {
                    ShipmentStageDriver_Line = new clsEntityDespatch_ShipmentStage_Driver(localDB);
                    ShipmentStageDriver_Line.Cs_pr_Despatch_ShipStage_Driver_ID = datos[0].ToString();
                    ShipmentStageDriver_Line.Cs_pr_Despatch_ShipStage_ID = datos[1].ToString();
                    ShipmentStageDriver_Line.Cs_tag_Driver_ID = datos[2].ToString();
                    ShipmentStageDriver_Line.Cs_tag_Driver_SchemaID = datos[3].ToString();
                    List_ShipmentStageDrive.Add(ShipmentStageDriver_Line);
                }
                cs_pxConexion_basedatos.Close();
                return List_ShipmentStageDrive;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDespatch_ShipmentStage_Driver cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
                return null;
            }
        }
    }
}
