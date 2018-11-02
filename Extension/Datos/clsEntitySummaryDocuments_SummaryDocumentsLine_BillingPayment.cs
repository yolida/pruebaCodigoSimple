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
    [ProgId("clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment")]
    public class clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id { get; set; }
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id { get; set; }
        public string Cs_tag_PaidAmount { get; set; }
        public string Cs_tag_InstructionID { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id);
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
            cs_cmValores.Add(Cs_tag_PaidAmount);
            cs_cmValores.Add(Cs_tag_InstructionID);
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment()
        {
           // localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        internal List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment> List_SummaryDocumentsLine_BillingPayment = new List<clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment>();
            try
            {
                
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_SummaryDocumentsLine_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment SummaryDocumentsLine_BillingPayment;
                while (datos.Read())
                {
                    SummaryDocumentsLine_BillingPayment = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localDB);
                    SummaryDocumentsLine_BillingPayment.Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = datos[0].ToString();
                    SummaryDocumentsLine_BillingPayment.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = datos[1].ToString();
                    SummaryDocumentsLine_BillingPayment.Cs_tag_PaidAmount = datos[2].ToString();
                    SummaryDocumentsLine_BillingPayment.Cs_tag_InstructionID = datos[3].ToString();
                    List_SummaryDocumentsLine_BillingPayment.Add(SummaryDocumentsLine_BillingPayment);
                }
                cs_pxConexion_basedatos.Close();
                
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment" + ex.ToString());
                //return null;
            }
            return List_SummaryDocumentsLine_BillingPayment;
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_BillingPayment_Id = valores[0];
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = valores[1];
                Cs_tag_PaidAmount = valores[2];
                Cs_tag_InstructionID = valores[3];
                return this;
            }
            else
            {
                return null;
            }
           
        }
    }
}
