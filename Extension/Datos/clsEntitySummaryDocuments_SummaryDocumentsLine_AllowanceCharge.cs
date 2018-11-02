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
    [ProgId("clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge")]
    public class clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id { get; set; }
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id { get; set; }
        public string Cs_tag_ChargeIndicator { get; set; }
        public string Cs_tag_Amount { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id);
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
            cs_cmValores.Add(Cs_tag_ChargeIndicator);
            cs_cmValores.Add(Cs_tag_Amount);
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge()
        {
          //  localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 3; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id");
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

        internal List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge> List_SummaryDocumentsLine_AllowanceCharge = new List<clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge>();
            try
            {
                
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_SummaryDocumentsLine_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge SummaryDocumentsLine_AllowanceCharge;
                while (datos.Read())
                {
                    SummaryDocumentsLine_AllowanceCharge = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localDB);
                    SummaryDocumentsLine_AllowanceCharge.Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id = datos[0].ToString();
                    SummaryDocumentsLine_AllowanceCharge.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = datos[1].ToString();
                    SummaryDocumentsLine_AllowanceCharge.Cs_tag_ChargeIndicator = datos[2].ToString();
                    SummaryDocumentsLine_AllowanceCharge.Cs_tag_Amount = datos[3].ToString();
                    List_SummaryDocumentsLine_AllowanceCharge.Add(SummaryDocumentsLine_AllowanceCharge);
                }
                cs_pxConexion_basedatos.Close();
               
            }
            catch (Exception ex)
            {
               // System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
                //return null;
            }
            return List_SummaryDocumentsLine_AllowanceCharge;
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_AllowanceCharge_Id = valores[0];
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = valores[1];
                Cs_tag_ChargeIndicator = valores[2];
                Cs_tag_Amount = valores[3];
                return this;
            }
            else
            {
                return null;
            }
            
        }
    }
}
