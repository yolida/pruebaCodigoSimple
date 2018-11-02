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
    [ProgId("clsEntityRetention_RetentionLine")]
    public class clsEntityRetention_RetentionLine:clsBaseEntidad
    {
        public string Cs_pr_Retention_RetentionLine_Id { get; set; }//
        public string Cs_pr_Retention_Id { get; set; }//1
        public string Cs_tag_Id { get; set; }//1
        public string Cs_tag_Id_SchemeId { get; set; }//2
        public string Cs_tag_IssueDate { get; set; }//3
        public string Cs_tag_TotalInvoiceAmount { get; set; }//4
        public string Cs_tag_TotalInvoiceAmount_CurrencyId { get; set; }//5
        public string Cs_tag_Payment_PaidDate { get; set; }//6
        public string Cs_tag_Payment_Id { get; set; }//7
        public string Cs_tag_Payment_PaidAmount { get; set; }//8
        public string Cs_tag_Payment_PaidAmount_CurrencyId { get; set; }//9
        public string Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount { get; set; }//10
        public string Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId { get; set; }//11
        public string Cs_tag_SUNATRetentionInformation_SUNATRetentionDate { get; set; }//12
        public string Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid { get; set; }//13
        public string Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId { get; set; }//14
        public string Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode { get; set; }//15
        public string Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode { get; set; }//16
        public string Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate { get; set; }//17
        public string Cs_tag_SUNATRetentionInformation_ExchangeRate_Date { get; set; }//18
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Retention_RetentionLine_Id);
            cs_cmValores.Add(Cs_pr_Retention_Id);
            cs_cmValores.Add(Cs_tag_Id);
            cs_cmValores.Add(Cs_tag_Id_SchemeId);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_Payment_PaidDate);
            cs_cmValores.Add(Cs_tag_Payment_Id);
            cs_cmValores.Add(Cs_tag_Payment_PaidAmount);
            cs_cmValores.Add(Cs_tag_Payment_PaidAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_SUNATRetentionDate);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate);
            cs_cmValores.Add(Cs_tag_SUNATRetentionInformation_ExchangeRate_Date);
        }
        public clsEntityRetention_RetentionLine(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Retention_RetentionLine";
            cs_cmCampos.Add("cs_Retention_RetentionLine_Id");
            cs_cmCampos.Add("cs_Retention_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Retention_RetentionLine";
            cs_cmCampos_min.Add("cs_Retention_RetentionLine_Id");
            cs_cmCampos_min.Add("cs_Retention_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityRetention_RetentionLine()
        {
            //localDB = local;
            cs_cmTabla = "cs_Retention_RetentionLine";
            cs_cmCampos.Add("cs_Retention_RetentionLine_Id");
            cs_cmCampos.Add("cs_Retention_Id");
            for (int i = 1; i <= 18; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_Retention_RetentionLine";
            cs_cmCampos_min.Add("cs_Retention_RetentionLine_Id");
            cs_cmCampos_min.Add("cs_Retention_Id");
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

        public clsEntityRetention_RetentionLine cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Retention_RetentionLine_Id = valores[0];
                Cs_pr_Retention_Id = valores[1];
                Cs_tag_Id = valores[2];
                Cs_tag_Id_SchemeId = valores[3];
                Cs_tag_IssueDate = valores[4];
                Cs_tag_TotalInvoiceAmount = valores[5];
                Cs_tag_TotalInvoiceAmount_CurrencyId = valores[6];
                Cs_tag_Payment_PaidDate = valores[7];
                Cs_tag_Payment_Id = valores[8];
                Cs_tag_Payment_PaidAmount = valores[9];
                Cs_tag_Payment_PaidAmount_CurrencyId = valores[10];
                Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = valores[11];
                Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId = valores[12];
                Cs_tag_SUNATRetentionInformation_SUNATRetentionDate = valores[13];
                Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = valores[14];
                Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId = valores[15];
                Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode = valores[16];
                Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode = valores[17];
                Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate = valores[18];
                Cs_tag_SUNATRetentionInformation_ExchangeRate_Date = valores[19];

                return this;
            }
            else
            {
                return null;
            }
           
        }
        public List<clsEntityRetention_RetentionLine> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntityRetention_RetentionLine> List_PerceptionLine;
            try
            {
                List_PerceptionLine = new List<clsEntityRetention_RetentionLine>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Retention_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntityRetention_RetentionLine PerceptionLine;
                while (datos.Read())
                {
                    PerceptionLine = new clsEntityRetention_RetentionLine(localDB);
                    PerceptionLine.Cs_pr_Retention_RetentionLine_Id = datos[0].ToString();
                    PerceptionLine.Cs_pr_Retention_Id = datos[1].ToString();
                    PerceptionLine.Cs_tag_Id = datos[2].ToString();
                    PerceptionLine.Cs_tag_Id_SchemeId = datos[3].ToString();
                    PerceptionLine.Cs_tag_IssueDate = datos[4].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount = datos[5].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount_CurrencyId = datos[6].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidDate = datos[7].ToString();
                    PerceptionLine.Cs_tag_Payment_Id = datos[8].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount = datos[9].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount_CurrencyId = datos[10].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = datos[11].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId = datos[12].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionDate = datos[13].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = datos[14].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId = datos[15].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode = datos[16].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode = datos[17].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate = datos[18].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date = datos[19].ToString();
                    List_PerceptionLine.Add(PerceptionLine);
                }
                cs_pxConexion_basedatos.Close();
                return List_PerceptionLine;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityRetention_RetentionLine cs_fxObtenerTodoPorCabeceraId" + ex.ToString());
                return null;
            }
        }
        public clsEntityRetention_RetentionLine cs_fxObtenerUnoPorCabeceraId(string Id)
        {
            clsEntityRetention_RetentionLine PerceptionLine=null;
            try
            {               
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_Retention_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                
                while (datos.Read())
                {
                    PerceptionLine = new clsEntityRetention_RetentionLine(localDB);
                    PerceptionLine.Cs_pr_Retention_RetentionLine_Id = datos[0].ToString();
                    PerceptionLine.Cs_pr_Retention_Id = datos[1].ToString();
                    PerceptionLine.Cs_tag_Id = datos[2].ToString();
                    PerceptionLine.Cs_tag_Id_SchemeId = datos[3].ToString();
                    PerceptionLine.Cs_tag_IssueDate = datos[4].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount = datos[5].ToString();
                    PerceptionLine.Cs_tag_TotalInvoiceAmount_CurrencyId = datos[6].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidDate = datos[7].ToString();
                    PerceptionLine.Cs_tag_Payment_Id = datos[8].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount = datos[9].ToString();
                    PerceptionLine.Cs_tag_Payment_PaidAmount_CurrencyId = datos[10].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount = datos[11].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionAmount_CurrencyId = datos[12].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATRetentionDate = datos[13].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid = datos[14].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_SUNATNetTotalPaid_CurrencyId = datos[15].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_SourceCurrencyCode = datos[16].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_TargetCurrencyCode = datos[17].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_CalculationRate = datos[18].ToString();
                    PerceptionLine.Cs_tag_SUNATRetentionInformation_ExchangeRate_Date = datos[19].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return PerceptionLine;
            }
            catch (Exception ex)
            {
               // clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntityRetention_RetentionLine cs_fxObtenerUnoPorCabeceraId" + ex.ToString());
                return null;
            }
        }
    }
}
