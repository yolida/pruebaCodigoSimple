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
    [ProgId("clsEntityPerception_PerceptionLine")]
    public class clsEntityPerception_PerceptionLine : clsBaseEntidad
    {
        public string Cs_pr_Perception_PerceptionLine_Id { get; set; }
        public string Cs_pr_Perception_Id { get; set; }
        public string Cs_tag_Id { get; set; }
        public string Cs_tag_Id_SchemeId { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_TotalInvoiceAmount { get; set; }
        public string Cs_tag_TotalInvoiceAmount_CurrencyId { get; set; }
        public string Cs_tag_Payment_PaidDate { get; set; }
        public string Cs_tag_Payment_Id { get; set; }
        public string Cs_tag_Payment_PaidAmount { get; set; }
        public string Cs_tag_Payment_PaidAmount_CurrencyId { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount_CurrencyId { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_SUNATPerceptionDate { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed{ get; set; }
        public string Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed_CurrencyId { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate { get; set; }
        public string Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date { get; set; }

        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Perception_PerceptionLine_Id);
            cs_cmValores.Add(Cs_pr_Perception_Id);
            cs_cmValores.Add(Cs_tag_Id);
            cs_cmValores.Add(Cs_tag_Id_SchemeId);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_Payment_PaidDate);
            cs_cmValores.Add(Cs_tag_Payment_Id);
            cs_cmValores.Add(Cs_tag_Payment_PaidAmount);
            cs_cmValores.Add(Cs_tag_Payment_PaidAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_SUNATPerceptionDate);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date);
        }
        public clsEntityPerception_PerceptionLine(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Perception_PerceptionLine";
            cs_cmCampos.Add("cs_Perception_PerceptionLine_Id");
            cs_cmCampos.Add("cs_Perception_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Perception_PerceptionLine";
            cs_cmCampos_min.Add("cs_Perception_PerceptionLine_Id");
            cs_cmCampos_min.Add("cs_Perception_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityPerception_PerceptionLine()
        {
            //localDB = local;
            cs_cmTabla = "cs_Perception_PerceptionLine";
            cs_cmCampos.Add("cs_Perception_PerceptionLine_Id");
            cs_cmCampos.Add("cs_Perception_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Perception_PerceptionLine";
            cs_cmCampos_min.Add("cs_Perception_PerceptionLine_Id");
            cs_cmCampos_min.Add("cs_Perception_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityPerception_PerceptionLine cs_fxObtenerUnoPorId(string id)
        {

            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Perception_PerceptionLine_Id = valores[0];
                Cs_pr_Perception_Id = valores[1];
                Cs_tag_Id = valores[2];
                Cs_tag_Id_SchemeId = valores[3];
                Cs_tag_IssueDate = valores[4];
                Cs_tag_TotalInvoiceAmount = valores[5];
                Cs_tag_TotalInvoiceAmount_CurrencyId = valores[6];
                Cs_tag_Payment_PaidDate = valores[7];
                Cs_tag_Payment_Id = valores[8];
                Cs_tag_Payment_PaidAmount = valores[9];
                Cs_tag_Payment_PaidAmount_CurrencyId = valores[10];
                Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount = valores[11];
                Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount_CurrencyId = valores[12];
                Cs_tag_SUNATPerceptionInformation_SUNATPerceptionDate = valores[13];
                Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed = valores[14];
                Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed_CurrencyId = valores[15];
                Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode = valores[16];
                Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode = valores[17];
                Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate = valores[18];
                Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date = valores[19];
                return this;
            }
            else
            {
                return null;
            }
           
        }
        internal List<clsEntityPerception_PerceptionLine> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityPerception_PerceptionLine> List_PerceptionLine;
            try
            {
                List_PerceptionLine = new List<clsEntityPerception_PerceptionLine>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Perception_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntityPerception_PerceptionLine PerceptionLine;
                while (datos.Read())
                {
                    PerceptionLine = new clsEntityPerception_PerceptionLine(localDB);
                    PerceptionLine.Cs_pr_Perception_PerceptionLine_Id = datos[0].ToString();
                    PerceptionLine.Cs_pr_Perception_Id = datos[1].ToString();
                    PerceptionLine.Cs_tag_Id = datos[2].ToString();
                    PerceptionLine.Cs_tag_Id_SchemeId = datos[3].ToString();
                    PerceptionLine.Cs_tag_IssueDate = datos[4].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount = datos[5].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount_CurrencyId = datos[6].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidDate = datos[7].ToString();
                    PerceptionLine.Cs_tag_Payment_Id = datos[8].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount = datos[9].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount_CurrencyId = datos[10].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount = datos[11].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionAmount_CurrencyId = datos[12].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_SUNATPerceptionDate = datos[13].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed = datos[14].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_SUNATNetTotalCashed_CurrencyId = datos[15].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_ExchangeRate_SourceCurrencyCode = datos[16].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_ExchangeRate_TargetCurrencyCode = datos[17].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_ExchangeRate_CalculationRate = datos[18].ToString();
                    PerceptionLine.Cs_tag_SUNATPerceptionInformation_ExchangeRate_Date = datos[19].ToString();                
                    List_PerceptionLine.Add(PerceptionLine);
                }
                cs_pxConexion_basedatos.Close();
                return List_PerceptionLine;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityPerception_PerceptionLine cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
                return null;
            }
        }

    }
}
