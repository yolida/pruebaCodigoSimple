using Extension.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDespatch")]
    public class clsEntityDespatch : clsBaseEntidad
    {   
        public string Cs_pr_Despatch_ID { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_IssueTime { get; set; }
        public string Cs_tag_AdviceTypeCode { get; set; }
        public string Cs_tag_Note { get; set; }
        public string Cs_tag_AdditionalDocumentReference_ID { get; set; }
        public string Cs_tag_AdditionalDocumentReference_DocumentType { get; set; }
        public string Cs_tag_AdditionalDocumentReference_DocumentTypeCode { get; set; }
        public string Cs_tag_DespatchSupParty_CustAssigAccountID { get; set; }
        public string Cs_tag_DespatchSupParty_CustAssigAccountID_SchemeID { get; set; }
        public string Cs_tag_DespatchSupParty_PartyLegalEntity { get; set; }
        public string Cs_tag_DeliveryCustParty_PartyLegalEntity { get; set; }
        public string Cs_tag_DeliveryCustParty_CustAssigAccountID { get; set; }
        public string Cs_tag_DeliveryCustParty_CustAssigAccountID_SchemeID { get; set; }    
        public string Cs_tag_SellerSupParty_CustAssigAccountID { get; set; }
        public string Cs_tag_SellerSupParty_CustAssigAccountID_SchemeID { get; set; }
        public string Cs_tag_SellerSupParty_PartyLegalEntity { get; set; }
        public string Cs_tag_Ship_HandlingCode { get; set; }
        public string Cs_tag_Ship_Information { get; set; }
        public string Cs_tag_Ship_SplitConsignmentIndicador { get; set; }
        public string Cs_tag_Ship_GrossWeightMeasure { get; set; }
        public string Cs_tag_Ship_GrossWeightMeasure_UnitCode { get; set; }
        public string Cs_tag_Ship_TotalTransportHandlingUnitQuantity { get; set; }
        public string Cs_tag_Ship_DeliveryAddress_ID { get; set; }
        public string Cs_tag_Ship_DeliveryAddress_StreetName { get; set; }
        public string Cs_tag_Ship_TransHandUnit_ID { get; set; }
        public string Cs_tag_Ship_TransHandUnit_Equip_ID { get; set; }
        public string Cs_tag_Ship_OriginAddress_ID { get; set; }
        public string Cs_tag_Ship_OriginAddress_StreetName { get; set; }
        public string Cs_pr_EstadoSUNAT { get; set; }
        public string Cs_pr_EstadoSCC { get; set; }
        public string Cs_pr_XML { get; set; }
        public string Cs_pr_CDR { get; set; }
        public string Cs_pr_ComentarioSUNAT { get; set; }
       // private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Despatch_ID);
            cs_cmValores.Add(Cs_tag_ID);//1
            cs_cmValores.Add(Cs_tag_IssueDate);//2
            cs_cmValores.Add(Cs_tag_IssueTime);//3
            cs_cmValores.Add(Cs_tag_AdviceTypeCode);//4
            cs_cmValores.Add(Cs_tag_Note);//5
            cs_cmValores.Add(Cs_tag_AdditionalDocumentReference_ID);//6
            cs_cmValores.Add(Cs_tag_AdditionalDocumentReference_DocumentType);//7
            cs_cmValores.Add(Cs_tag_AdditionalDocumentReference_DocumentTypeCode);//8
            cs_cmValores.Add(Cs_tag_DespatchSupParty_CustAssigAccountID);//9
            cs_cmValores.Add(Cs_tag_DespatchSupParty_CustAssigAccountID_SchemeID);//10
            cs_cmValores.Add(Cs_tag_DespatchSupParty_PartyLegalEntity);//11
            cs_cmValores.Add(Cs_tag_DeliveryCustParty_CustAssigAccountID);//12
            cs_cmValores.Add(Cs_tag_DeliveryCustParty_CustAssigAccountID_SchemeID);//13
            cs_cmValores.Add(Cs_tag_DeliveryCustParty_PartyLegalEntity);//14
            cs_cmValores.Add(Cs_tag_SellerSupParty_CustAssigAccountID);//15
            cs_cmValores.Add(Cs_tag_SellerSupParty_CustAssigAccountID_SchemeID);//16
            cs_cmValores.Add(Cs_tag_SellerSupParty_PartyLegalEntity);//17
            cs_cmValores.Add(Cs_tag_Ship_HandlingCode);//18
            cs_cmValores.Add(Cs_tag_Ship_Information);//19
            cs_cmValores.Add(Cs_tag_Ship_SplitConsignmentIndicador);//20
            cs_cmValores.Add(Cs_tag_Ship_GrossWeightMeasure);//21
            cs_cmValores.Add(Cs_tag_Ship_GrossWeightMeasure_UnitCode);//22
            cs_cmValores.Add(Cs_tag_Ship_TotalTransportHandlingUnitQuantity);//23
            cs_cmValores.Add(Cs_tag_Ship_DeliveryAddress_ID);//24
            cs_cmValores.Add(Cs_tag_Ship_DeliveryAddress_StreetName);//25
            cs_cmValores.Add(Cs_tag_Ship_TransHandUnit_ID);//26
            cs_cmValores.Add(Cs_tag_Ship_TransHandUnit_Equip_ID);//27
            cs_cmValores.Add(Cs_tag_Ship_OriginAddress_ID);//28
            cs_cmValores.Add(Cs_tag_Ship_OriginAddress_StreetName);//29
            cs_cmValores.Add(Cs_pr_EstadoSUNAT);//30
            cs_cmValores.Add(Cs_pr_EstadoSCC);//31
            cs_cmValores.Add(Cs_pr_XML);//32
            cs_cmValores.Add(Cs_pr_CDR);//33
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);//34           
        }
        public clsEntityDespatch(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Despatch";
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 34; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Despatch";
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 34; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityDespatch()
        {
           // localDB = local;
            cs_cmTabla = "cs_Despatch";
            cs_cmCampos.Add("cs_Despatch_Id");
            for (int i = 1; i <= 34; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Despatch";
            cs_cmCampos_min.Add("cs_Despatch_Id");
            for (int i = 1; i <= 34; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityDespatch cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Despatch_ID = valores[0];
                Cs_tag_ID = valores[1];
                Cs_tag_IssueDate = valores[2];
                Cs_tag_IssueTime = valores[3];
                Cs_tag_AdviceTypeCode = valores[4];
                Cs_tag_Note = valores[5];
                Cs_tag_AdditionalDocumentReference_ID = valores[6];
                Cs_tag_AdditionalDocumentReference_DocumentType = valores[7];
                Cs_tag_AdditionalDocumentReference_DocumentTypeCode = valores[8];
                Cs_tag_DespatchSupParty_CustAssigAccountID = valores[9];
                Cs_tag_DespatchSupParty_CustAssigAccountID_SchemeID = valores[10];
                Cs_tag_DespatchSupParty_PartyLegalEntity = valores[11];
                Cs_tag_DeliveryCustParty_CustAssigAccountID = valores[12];
                Cs_tag_DeliveryCustParty_CustAssigAccountID_SchemeID = valores[13];
                Cs_tag_DeliveryCustParty_PartyLegalEntity = valores[14];
                Cs_tag_SellerSupParty_CustAssigAccountID = valores[15];
                Cs_tag_SellerSupParty_CustAssigAccountID_SchemeID = valores[16];
                Cs_tag_SellerSupParty_PartyLegalEntity = valores[17];
                Cs_tag_Ship_HandlingCode = valores[18];
                Cs_tag_Ship_Information = valores[19];
                Cs_tag_Ship_SplitConsignmentIndicador = valores[20];
                Cs_tag_Ship_GrossWeightMeasure = valores[21];
                Cs_tag_Ship_GrossWeightMeasure_UnitCode = valores[22];
                Cs_tag_Ship_TotalTransportHandlingUnitQuantity = valores[23];
                Cs_tag_Ship_DeliveryAddress_ID = valores[24];
                Cs_tag_Ship_DeliveryAddress_StreetName = valores[25];
                Cs_tag_Ship_TransHandUnit_ID = valores[26];
                Cs_tag_Ship_TransHandUnit_Equip_ID = valores[27];
                Cs_tag_Ship_OriginAddress_ID = valores[28];
                Cs_tag_Ship_OriginAddress_StreetName = valores[29];
                Cs_pr_EstadoSUNAT = valores[30];
                Cs_pr_EstadoSCC = valores[31];
                Cs_pr_XML = valores[32];
                Cs_pr_CDR = valores[33];
                Cs_pr_ComentarioSUNAT = valores[34];
                return this;
            }
            else
            {
                return null;
            }

        }

    }
}
