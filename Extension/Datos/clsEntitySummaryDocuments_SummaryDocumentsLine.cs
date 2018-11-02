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
    [ProgId("clsEntitySummaryDocuments_SummaryDocumentsLine")]
    public class clsEntitySummaryDocuments_SummaryDocumentsLine : clsBaseEntidad
    {
        public string Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id { get; set; }
        public string Cs_pr_SummaryDocuments_Id { get; set; }
        public string Cs_tag_LineID { get; set; }
        public string Cs_tag_DocumentTypeCode { get; set; }
        public string Cs_tag_DocumentSerialID { get; set; }
        public string Cs_tag_StartDocumentNumberID { get; set; }
        public string Cs_tag_EndDocumentNumberID { get; set; }
        public string Cs_tag_TotalAmount { get; set; }
        
        /*Agregado para el status FEI2-325*/
        public string Cs_tag_ConditionCode { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id);
            cs_cmValores.Add(Cs_pr_SummaryDocuments_Id);
            cs_cmValores.Add(Cs_tag_LineID);
            cs_cmValores.Add(Cs_tag_DocumentTypeCode);
            cs_cmValores.Add(Cs_tag_DocumentSerialID);
            cs_cmValores.Add(Cs_tag_StartDocumentNumberID);
            cs_cmValores.Add(Cs_tag_EndDocumentNumberID);
            cs_cmValores.Add(Cs_tag_TotalAmount);
            cs_cmValores.Add(Cs_tag_ConditionCode);
        }

        internal List<clsEntitySummaryDocuments_SummaryDocumentsLine> cs_fxObtenerTodoPorCabeceraId(string Id)
        {
            List<clsEntitySummaryDocuments_SummaryDocumentsLine> List_SummaryDocumentsLine;
            try
            {
                List_SummaryDocumentsLine = new List<clsEntitySummaryDocuments_SummaryDocumentsLine>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_Id=" + Id.Trim() + " ;";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntitySummaryDocuments_SummaryDocumentsLine SummaryDocumentsLine;
                while (datos.Read())
                {
                    SummaryDocumentsLine = new clsEntitySummaryDocuments_SummaryDocumentsLine(localDB);
                    SummaryDocumentsLine.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = datos[0].ToString();
                    SummaryDocumentsLine.Cs_pr_SummaryDocuments_Id = datos[1].ToString();
                    SummaryDocumentsLine.Cs_tag_LineID = datos[2].ToString();
                    SummaryDocumentsLine.Cs_tag_DocumentTypeCode = datos[3].ToString();
                    SummaryDocumentsLine.Cs_tag_DocumentSerialID = datos[4].ToString();
                    SummaryDocumentsLine.Cs_tag_StartDocumentNumberID = datos[5].ToString();
                    SummaryDocumentsLine.Cs_tag_EndDocumentNumberID = datos[6].ToString();
                    SummaryDocumentsLine.Cs_tag_TotalAmount = datos[7].ToString();
                    SummaryDocumentsLine.Cs_tag_ConditionCode = datos[8].ToString();
                    List_SummaryDocumentsLine.Add(SummaryDocumentsLine);
                }
                cs_pxConexion_basedatos.Close();
                return List_SummaryDocumentsLine;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_SummaryDocumentsLine cs_fxObtenerTodoPorCabeceraId " + ex.ToString());
                return null;
            }
        }

        public List<clsEntitySummaryDocuments_SummaryDocumentsLine> cs_fxObtenerTodoPorCabeceraIdYSerieYTipo(string CabeceraId, string Serie, string Tipo)
        {
            List<clsEntitySummaryDocuments_SummaryDocumentsLine> List_SummaryDocumentsLine;
            try
            {
                List_SummaryDocumentsLine = new List<clsEntitySummaryDocuments_SummaryDocumentsLine>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cs_SummaryDocuments_Id=" + CabeceraId.Trim() + "  AND cp3 = '" + Serie + "' ";// AND cp2 = '"+Tipo+"';";
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();

                clsEntitySummaryDocuments_SummaryDocumentsLine SummaryDocumentsLine;
                while (datos.Read())
                {
                    SummaryDocumentsLine = new clsEntitySummaryDocuments_SummaryDocumentsLine(localDB);
                    SummaryDocumentsLine.Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = datos[0].ToString();
                    SummaryDocumentsLine.Cs_pr_SummaryDocuments_Id = datos[1].ToString();
                    SummaryDocumentsLine.Cs_tag_LineID = datos[2].ToString();
                    SummaryDocumentsLine.Cs_tag_DocumentTypeCode = datos[3].ToString();
                    SummaryDocumentsLine.Cs_tag_DocumentSerialID = datos[4].ToString();
                    SummaryDocumentsLine.Cs_tag_StartDocumentNumberID = datos[5].ToString();
                    SummaryDocumentsLine.Cs_tag_EndDocumentNumberID = datos[6].ToString();
                    SummaryDocumentsLine.Cs_tag_TotalAmount = datos[7].ToString();
                    SummaryDocumentsLine.Cs_tag_ConditionCode = datos[8].ToString();
                    List_SummaryDocumentsLine.Add(SummaryDocumentsLine);
                }
                cs_pxConexion_basedatos.Close();
                return List_SummaryDocumentsLine;
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsgEr("ERR6", ex.ToString());
                clsBaseLog.cs_pxRegistarAdd("clsEntitySummaryDocuments_SummaryDocumentsLine cs_fxObtenerTodoPorCabeceraIdYSerieYTipo" + ex.ToString());
                return null;
            }
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }

        public clsEntitySummaryDocuments_SummaryDocumentsLine()
        {
            //localDB = local;
            cs_cmTabla = "cs_SummaryDocuments_SummaryDocumentsLine";
            cs_cmCampos.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            cs_cmCampos.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }

            cs_cmTabla_min = "cs_SummaryDocuments_SummaryDocumentsLine";
            cs_cmCampos_min.Add("cs_SummaryDocuments_SummaryDocumentsLine_Id");
            cs_cmCampos_min.Add("cs_SummaryDocuments_Id");
            for (int i = 1; i <= 7; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntitySummaryDocuments_SummaryDocumentsLine cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_SummaryDocuments_SummaryDocumentsLine_Id = valores[0];
                Cs_pr_SummaryDocuments_Id = valores[1];
                Cs_tag_LineID = valores[2];
                Cs_tag_DocumentTypeCode = valores[3];
                Cs_tag_DocumentSerialID = valores[4];
                Cs_tag_StartDocumentNumberID = valores[5];
                Cs_tag_EndDocumentNumberID = valores[6];
                Cs_tag_TotalAmount = valores[7];
                Cs_tag_ConditionCode = valores[8];
                return this;
            }
            else
            {
                return null;
            }         
        }
    }
}
