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
    [ProgId("clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal")]
    public class clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id { get; set; }
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id { get; set; }
        public string Cs_tag_TaxAmount { get; set; }
        public string Cs_tag_TaxSubtotal_TaxAmount { get; set; }
        public string Cs_tag_TaxCategory_TaxScheme_ID { get; set; }
        public string Cs_tag_TaxCategory_TaxScheme_Name { get; set; }
        public string Cs_tag_TaxCategory_TaxScheme_TaxTypeCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id);
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
            cs_cmValores.Add(Cs_tag_TaxAmount);
            cs_cmValores.Add(Cs_tag_TaxSubtotal_TaxAmount);
            cs_cmValores.Add(Cs_tag_TaxCategory_TaxScheme_ID);
            cs_cmValores.Add(Cs_tag_TaxCategory_TaxScheme_Name);
            cs_cmValores.Add(Cs_tag_TaxCategory_TaxScheme_TaxTypeCode);
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 6; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 6; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal()
        {
           // localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 6; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            for (int i = 1; i < 6; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        internal List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal> List_SummaryDocumentsLine_TaxTotal = new List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal>();
            try
            {
               // List_SummaryDocumentsLine_TaxTotal = new List<clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_SummaryDocumentsLine_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal SummaryDocumentsLine_TaxTotal;
                while (datos.Read())
                {
                    SummaryDocumentsLine_TaxTotal = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localDB);
                    SummaryDocumentsLine_TaxTotal.Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = datos[0].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = datos[1].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_tag_TaxAmount = datos[2].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_tag_TaxSubtotal_TaxAmount = datos[3].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_tag_TaxCategory_TaxScheme_ID = datos[4].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_tag_TaxCategory_TaxScheme_Name = datos[5].ToString();
                    SummaryDocumentsLine_TaxTotal.Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = datos[6].ToString();
                    List_SummaryDocumentsLine_TaxTotal.Add(SummaryDocumentsLine_TaxTotal);
                }
                cs_pxConexion_basedatos.Close();
                
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
               // return null;
            }
            return List_SummaryDocumentsLine_TaxTotal;
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_TaxTotal_Id = valores[0];
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = valores[1];
                Cs_tag_TaxAmount = valores[2];
                Cs_tag_TaxSubtotal_TaxAmount = valores[3];
                Cs_tag_TaxCategory_TaxScheme_ID = valores[4];
                Cs_tag_TaxCategory_TaxScheme_Name = valores[5];
                Cs_tag_TaxCategory_TaxScheme_TaxTypeCode = valores[6];
                return this;
            }
            else
            {
                return null;
            }
            
        }
    }
}
