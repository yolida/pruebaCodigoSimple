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
    [ProgId("clsEntityPerception")]
    public class clsEntityPerception : clsBaseEntidad
    {
        public string Cs_pr_Perception_id { get; set; }
        public string Cs_tag_Id { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_PartyIdentification_Id { get; set; }
        public string Cs_tag_PartyIdentificacion_SchemeId { get; set; }
        public string Cs_tag_PartyName { get; set; }
        public string Cs_tag_PostalAddress_Id { get; set; }
        public string Cs_tag_PostalAddress_StreetName { get; set; }
        public string Cs_tag_PostalAddress_CitySubdivisionName { get; set; }
        public string Cs_tag_PostalAddress_CityName { get; set; }
        public string Cs_tag_PostalAddress_CountrySubEntity { get; set; }
        public string Cs_tag_PostalAddress_District { get; set; }
        public string Cs_tag_PostalAddress_Country_IdentificationCode { get; set; }
        public string Cs_tag_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_ReceiveParty_PartyIdentification_Id { get; set; }
        public string Cs_tag_ReceiveParty_PartyIdentification_SchemeId { get; set; }
        public string Cs_tag_ReceiveParty_PartyName_Name { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_Id { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_StreetName { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_CityName{ get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_CountrySubentity { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_District { get; set; }
        public string Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode { get; set; }
        public string Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_SUNATPerceptionSystemCode { get; set; }
        public string Cs_tag_SUNATPerceptionPercent { get; set; }
        public string Cs_tag_Note { get; set; }
        public string Cs_tag_TotalInvoiceAmount { get; set; }
        public string Cs_tag_TotalInvoiceAmount_CurrencyId { get; set; }
        public string Cs_tag_SUNATTotalCashed { get; set; }
        public string Cs_tag_SUNATTotalCashed_CurrencyId { get; set; }
        public string Cs_pr_EstadoSCC { get; set; }
        public string Cs_pr_EstadoSUNAT { get; set; }
        public string Cs_pr_ComentarioSUNAT { get; set; }
        public string Cs_pr_XML { get; set; }
        public string Cs_pr_CDR { get; set; }
        //private clsEntityDatabaseLocal localDB;
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Perception_id);
            cs_cmValores.Add(Cs_tag_Id);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_PartyIdentification_Id);
            cs_cmValores.Add(Cs_tag_PartyIdentificacion_SchemeId);
            cs_cmValores.Add(Cs_tag_PartyName);
            cs_cmValores.Add(Cs_tag_PostalAddress_Id);
            cs_cmValores.Add(Cs_tag_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_PostalAddress_CountrySubEntity);
            cs_cmValores.Add(Cs_tag_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyIdentification_Id);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyIdentification_SchemeId);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyName_Name);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_Id);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_CountrySubentity);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionSystemCode);
            cs_cmValores.Add(Cs_tag_SUNATPerceptionPercent);
            cs_cmValores.Add(Cs_tag_Note);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount);
            cs_cmValores.Add(Cs_tag_TotalInvoiceAmount_CurrencyId);
            cs_cmValores.Add(Cs_tag_SUNATTotalCashed);
            cs_cmValores.Add(Cs_tag_SUNATTotalCashed_CurrencyId);
            cs_cmValores.Add(Cs_pr_EstadoSCC);
            cs_cmValores.Add(Cs_pr_EstadoSUNAT);
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);
            cs_cmValores.Add(Cs_pr_XML);
            cs_cmValores.Add(Cs_pr_CDR);
        }
        public clsEntityPerception(clsEntityDatabaseLocal local)
        {
            localDB = local;
            cs_cmTabla = "cs_Perception";
            cs_cmCampos.Add("cs_Perception_Id");
            for (int i = 1; i <= 36; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Perception";
            cs_cmCampos_min.Add("cs_Perception_Id");
            for (int i = 1; i <= 36; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public clsEntityPerception()
        {
           // localDB = local;
            cs_cmTabla = "cs_Perception";
            cs_cmCampos.Add("cs_Perception_Id");
            for (int i = 1; i <= 36; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
            cs_cmTabla_min = "cs_Perception";
            cs_cmCampos_min.Add("cs_Perception_Id");
            for (int i = 1; i <= 36; i++)
            {
                cs_cmCampos_min.Add("cp" + i.ToString());
            }
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public clsEntityPerception cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Perception_id = valores[0];
                Cs_tag_Id = valores[1];
                Cs_tag_IssueDate = valores[2];
                Cs_tag_PartyIdentification_Id = valores[3];
                Cs_tag_PartyIdentificacion_SchemeId = valores[4];
                Cs_tag_PartyName = valores[5];
                Cs_tag_PostalAddress_Id = valores[6];
                Cs_tag_PostalAddress_StreetName = valores[7];
                Cs_tag_PostalAddress_CitySubdivisionName = valores[8];
                Cs_tag_PostalAddress_CityName = valores[9];
                Cs_tag_PostalAddress_CountrySubEntity = valores[10];
                Cs_tag_PostalAddress_District = valores[11];
                Cs_tag_PostalAddress_Country_IdentificationCode = valores[12];
                Cs_tag_PartyLegalEntity_RegistrationName = valores[13];
                Cs_tag_ReceiveParty_PartyIdentification_Id = valores[14];
                Cs_tag_ReceiveParty_PartyIdentification_SchemeId = valores[15];
                Cs_tag_ReceiveParty_PartyName_Name = valores[16];
                Cs_tag_ReceiveParty_PostalAddress_Id = valores[17];
                Cs_tag_ReceiveParty_PostalAddress_StreetName = valores[18];
                Cs_tag_ReceiveParty_PostalAddress_CitySubdivisionName = valores[19];
                Cs_tag_ReceiveParty_PostalAddress_CityName = valores[20];
                Cs_tag_ReceiveParty_PostalAddress_CountrySubentity = valores[21];
                Cs_tag_ReceiveParty_PostalAddress_District = valores[22];
                Cs_tag_ReceiveParty_PostalAddress_Country_IdentificationCode = valores[23];
                Cs_tag_ReceiveParty_PartyLegalEntity_RegistrationName = valores[24];
                Cs_tag_SUNATPerceptionSystemCode = valores[25];
                Cs_tag_SUNATPerceptionPercent = valores[26];
                Cs_tag_Note = valores[27];
                Cs_tag_TotalInvoiceAmount = valores[28];
                Cs_tag_TotalInvoiceAmount_CurrencyId = valores[29];
                Cs_tag_SUNATTotalCashed = valores[30];
                Cs_tag_SUNATTotalCashed_CurrencyId = valores[31];
                Cs_pr_EstadoSCC = valores[32];
                Cs_pr_EstadoSUNAT = valores[33];
                Cs_pr_ComentarioSUNAT = valores[34];
                Cs_pr_XML = valores[35];
                Cs_pr_CDR = valores[36];
                return this;
            }
            else
            {
                return null;
            }
            
           
        }
    }
}
