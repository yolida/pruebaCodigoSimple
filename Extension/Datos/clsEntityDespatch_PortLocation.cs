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
    [ProgId("clsEntityDespatch_PortLocation")]
    public class clsEntityDespatch_PortLocation:clsBaseEntidad
    {
        public string Cs_pr_Despatch_PortLocation_ID { get; set; }
        public string Cs_pr_Despatch_ID { get; set; }
        public string Cs_tag_ID { get; set; }//1   
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_PortLocation_ID);
            cs_cmValores.Add(Cs_pr_Despatch_ID);
            cs_cmValores.Add(Cs_tag_ID);//1   
        }
        public clsEntityDespatch_PortLocation(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch_PortLocation";
            cs_cmCampos.Add("cs_Despatch_PortLocation_ID");
            cs_cmCampos.Add("cs_Despatch_ID");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_PortLocation";
            cs_cmCampos_min.Add("cs_Despatch_PortLocation_ID");
            cs_cmCampos_min.Add("cs_Despatch_ID");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch_PortLocation()
        {
            //localDB = local;
            cs_cmTabla = "cs_Despatch_PortLocation";
            cs_cmCampos.Add("cs_Despatch_PortLocation_ID");
            cs_cmCampos.Add("cs_Despatch_ID");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Despatch_PortLocation";
            cs_cmCampos_min.Add("cs_Despatch_PortLocation_ID");
            cs_cmCampos_min.Add("cs_Despatch_ID");
            for (int i = 1; i <= 1; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityDespatch_PortLocation cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_PortLocation_ID = valores[0];
                Cs_pr_Despatch_ID = valores[1];
                Cs_tag_ID = valores[2];
                return this;
            }
            else
            {
                return null;
            }
          
        }
        internal List<clsEntityDespatch_PortLocation> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityDespatch_PortLocation> List_ShipmentStagePort;
            try
            {
                List_ShipmentStagePort = new List<clsEntityDespatch_PortLocation>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Despatch_ID=" + Id.Trim() + ";";
               // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDespatch_PortLocation ShipmentStagePort_Line;
                while (datos.Read())
                {
                    ShipmentStagePort_Line = new clsEntityDespatch_PortLocation(localDB);
                    ShipmentStagePort_Line.Cs_pr_Despatch_PortLocation_ID = datos[0].ToString();
                    ShipmentStagePort_Line.Cs_pr_Despatch_ID = datos[1].ToString();
                    ShipmentStagePort_Line.Cs_tag_ID = datos[2].ToString();
                    List_ShipmentStagePort.Add(ShipmentStagePort_Line);
                }
                cs_pxConexion_basedatos.Close();
                return List_ShipmentStagePort;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityDespatch_PortLocation cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
                return null;
            }
        }
    }
}
