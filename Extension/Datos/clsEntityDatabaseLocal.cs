using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Extension.Base;
using System.Runtime.InteropServices;
using System.Data.SQLite;
//using MySql.Data.MySqlClient;

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDatabaseLocal")]
    public class clsEntityDatabaseLocal : clsBaseEntidadSistema
    {
        public string Cs_pr_DatabaseLocal_Id { get; set; }
        public string Cs_pr_DBMS { get; set; }
        public string Cs_pr_DBMSDriver { get; set; }
        public string Cs_pr_DBMSServername { get; set; }
        public string Cs_pr_DBMSServerport { get; set; }
        public string Cs_pr_DBName { get; set; }
        public string Cs_pr_DBUser { get; set; }
        public string Cs_pr_DBPassword { get; set; }
        public string Cs_pr_DBUse { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }

        private clsEntityDeclarant declarant;

        public clsEntityDatabaseLocal cs_pxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_DatabaseLocal_Id = valores[0];
                Cs_pr_DBMS = valores[1];
                Cs_pr_DBMSDriver = valores[2];
                Cs_pr_DBMSServername = valores[3];
                Cs_pr_DBMSServerport = valores[4];
                Cs_pr_DBName = valores[5];
                Cs_pr_DBUser = valores[6];
                Cs_pr_DBPassword = valores[7];
                Cs_pr_DBUse = valores[8];
                Cs_pr_Declarant_Id = valores[9];
                return this;
            }
            else
            {
                return null;
            }

        }

        public clsEntityDatabaseLocal()
        {
            cs_cmTabla = "cs_DatabaseLocal";
            cs_cmCampos.Add("cs_DatabaseLocal_Id");
            for (int i = 1; i <= 9; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
        }
        public clsEntityDatabaseLocal(clsEntityDeclarant declarante)
        {
            declarant = declarante;
            cs_cmTabla = "cs_DatabaseLocal";
            cs_cmCampos.Add("cs_DatabaseLocal_Id");
            for (int i = 1; i <= 9; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
        }
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_DatabaseLocal_Id);
            cs_cmValores.Add(Cs_pr_DBMS);
            cs_cmValores.Add(Cs_pr_DBMSDriver);
            cs_cmValores.Add(Cs_pr_DBMSServername);
            cs_cmValores.Add(Cs_pr_DBMSServerport);
            cs_cmValores.Add(Cs_pr_DBName);
            cs_cmValores.Add(Cs_pr_DBUser);
            cs_cmValores.Add(Cs_pr_DBPassword);
            cs_cmValores.Add(Cs_pr_DBUse);
            cs_cmValores.Add(Cs_pr_Declarant_Id);
        }

        public string cs_pxCrearBaseDatos()
        {
            string retorno = string.Empty;
            try
            {
                //clsBaseConexion cn = new clsBaseConexion();
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
                string prConexioncadenaservidor = local.cs_prConexioncadenaservidor();
                string prConexioncadenabasedatos = local.cs_prConexioncadenabasedatos();

                //MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
                //builder.Server = local.Cs_pr_DBMSServername;
                //builder.UserID = local.Cs_pr_DBUser;
                //builder.Password = local.Cs_pr_DBPassword;
                //builder.Port = Convert.ToUInt32(local.Cs_pr_DBMSServerport);

                OdbcConnection cs_pxConexion_servidor = new OdbcConnection(prConexioncadenaservidor);
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(prConexioncadenabasedatos);
                //MySqlConnection conn = new MySqlConnection(builder.ToString());
                //clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                string RutaInstalacion = clsBaseConfiguracion.getRutaInstalacion();
                if (local.Cs_pr_DBMS == "SQLite")
                {
                    if (!Directory.Exists(RutaInstalacion))
                    {
                        Directory.CreateDirectory(RutaInstalacion);
                        File.Create(RutaInstalacion + "\\" + local.Cs_pr_DBName + ".dbc").Close();
                    }
                    else if (!File.Exists(RutaInstalacion + "\\" + local.Cs_pr_DBName + ".dbc"))
                    {
                        File.Create(RutaInstalacion + "\\" + local.Cs_pr_DBName + ".dbc").Close();
                    }
                }
                else if (local.Cs_pr_DBMS == "MySQL")
                {
                    //conn.Open();
                    //new MySqlCommand("CREATE DATABASE " + local.Cs_pr_DBName + ";", conn).ExecuteNonQuery();
                    //conn.Close();
                    //builder.Database = local.Cs_pr_DBName;
                    //conn = new MySqlConnection(builder.ToString());
                }
                else
                {
                    cs_pxConexion_servidor.Open();
                    new OdbcCommand("CREATE DATABASE " + local.Cs_pr_DBName + ";", cs_pxConexion_servidor).ExecuteNonQuery();
                    cs_pxConexion_servidor.Close();
                }

                if (local.Cs_pr_DBMS != "MySQL")
                {
                    cs_pxConexion_basedatos.Open();
                    //new OdbcCommand("CREATE TABLE " + new clsEntityAlarms().cs_cmTabla + " (" + Insercion(new clsEntityAlarms().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new OdbcCommand("CREATE TABLE " + new clsEntityDatabaseWeb().cs_cmTabla + " (" + Insercion(new clsEntityDatabaseWeb().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new OdbcCommand("CREATE TABLE " + new clsEntityDeclarant().cs_cmTabla + " (" + Insercion(new clsEntityDeclarant().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + 
                        new clsEntityDocument(local).cs_cmTabla + // NombreTributo de la tabla
                            " (" + InsercionXML(
                        new clsEntityDocument(local).cs_cmCampos, local.Cs_pr_DBMS, 
                        new clsEntityDocument(local).cs_cmTabla, 
                        new clsEntityDocument(local).cs_cmCampos[0], "none") + "); ", 
                        cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_TaxTotal(local).cs_cmCampos, new clsEntityDocument_TaxTotal(local).cs_cmCampos[0], new clsEntityDocument_TaxTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments(local).cs_cmTabla + " (" + InsercionXML(new clsEntityDocument_AdditionalComments(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntityDocument_AdditionalComments(local).cs_cmTabla, new clsEntityDocument_AdditionalComments(local).cs_cmCampos[0], new clsEntityDocument_AdditionalComments(local).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos, new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos[0], new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_DespatchDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos, new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos[0], new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line(local).cs_cmCampos, new clsEntityDocument_Line(local).cs_cmCampos[0], new clsEntityDocument_Line(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_Description(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_Description(local).cs_cmCampos, new clsEntityDocument_Line_Description(local).cs_cmCampos[0], new clsEntityDocument_Line_Description(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_PricingReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_PricingReference(local).cs_cmCampos, new clsEntityDocument_Line_PricingReference(local).cs_cmCampos[0], new clsEntityDocument_Line_PricingReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos, new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos[0], new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_AdditionalComments(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos, new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos[0], new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments(local).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntitySummaryDocuments(local).cs_cmTabla, new clsEntitySummaryDocuments(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_Notes(local).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments_Notes(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntitySummaryDocuments_Notes(local).cs_cmTabla, new clsEntitySummaryDocuments_Notes(local).cs_cmCampos[0], new clsEntitySummaryDocuments_Notes(local).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments(local).cs_cmTabla + " (" + InsercionXML(new clsEntityVoidedDocuments(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntityVoidedDocuments(local).cs_cmTabla, new clsEntityVoidedDocuments(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmTabla + " (" + Insercion(new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos, new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos[0], new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();


                    new OdbcCommand("CREATE TABLE " + new clsEntityPerception(local).cs_cmTabla + " (" + InsercionXML(new clsEntityPerception(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntityPerception(local).cs_cmTabla, new clsEntityPerception(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityPerception_PerceptionLine(local).cs_cmTabla + " (" + Insercion(new clsEntityPerception_PerceptionLine(local).cs_cmCampos, new clsEntityPerception_PerceptionLine(local).cs_cmCampos[0], new clsEntityPerception_PerceptionLine(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("CREATE TABLE " + new clsEntityRetention(local).cs_cmTabla + " (" + InsercionXML(new clsEntityRetention(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntityRetention(local).cs_cmTabla, new clsEntityRetention(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityRetention_RetentionLine(local).cs_cmTabla + " (" + Insercion(new clsEntityRetention_RetentionLine(local).cs_cmCampos, new clsEntityRetention_RetentionLine(local).cs_cmCampos[0], new clsEntityRetention_RetentionLine(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch(local).cs_cmTabla + " (" + InsercionXML(new clsEntityDespatch(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntityDespatch(local).cs_cmTabla, new clsEntityDespatch(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage(local).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_ShipmentStage(local).cs_cmCampos, new clsEntityDespatch_ShipmentStage(local).cs_cmCampos[0], new clsEntityDespatch_ShipmentStage(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos, new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos[0], new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_OrderReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_OrderReference(local).cs_cmCampos, new clsEntityDespatch_OrderReference(local).cs_cmCampos[0], new clsEntityDespatch_OrderReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_PortLocation(local).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_PortLocation(local).cs_cmCampos, new clsEntityDespatch_PortLocation(local).cs_cmCampos[0], new clsEntityDespatch_PortLocation(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_Line(local).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_Line(local).cs_cmCampos, new clsEntityDespatch_Line(local).cs_cmCampos[0], new clsEntityDespatch_Line(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                    //registro de compras
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument(local).cs_cmTabla + " (" + InsercionXML(new clsEntidadDocument(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntidadDocument(local).cs_cmTabla, new clsEntidadDocument(local).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_TaxTotal(local).cs_cmCampos, new clsEntidadDocument_TaxTotal(local).cs_cmCampos[0], new clsEntidadDocument_TaxTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalComments(local).cs_cmTabla + " (" + InsercionXML(new clsEntidadDocument_AdditionalComments(local).cs_cmCampos, local.Cs_pr_DBMS, new clsEntidadDocument_AdditionalComments(local).cs_cmTabla, new clsEntidadDocument_AdditionalComments(local).cs_cmCampos[0], new clsEntidadDocument_AdditionalComments(local).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos, new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos[0], new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_DespatchDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos, new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos[0], new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line(local).cs_cmCampos, new clsEntidadDocument_Line(local).cs_cmCampos[0], new clsEntidadDocument_Line(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_Description(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_Description(local).cs_cmCampos, new clsEntidadDocument_Line_Description(local).cs_cmCampos[0], new clsEntidadDocument_Line_Description(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_PricingReference(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos, new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos[0], new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos, new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos[0], new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos, new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[0], new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("CREATE TABLE " + 
                        new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla +
                        " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos, 
                        new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos[0],
                        new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).
                        cs_cmCampos[1], local.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                }

                if(local.Cs_pr_DBMS == "MySQL")
                {
                    //conn.Open();

                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityDocument(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityDocument(local).cs_cmTabla_min, new clsEntityDocument(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_TaxTotal(local).cs_cmCampos_min, new clsEntityDocument_TaxTotal(local).cs_cmCampos_min[0], new clsEntityDocument_TaxTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityDocument_AdditionalComments(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityDocument_AdditionalComments(local).cs_cmTabla_min, new clsEntityDocument_AdditionalComments(local).cs_cmCampos_min[0], new clsEntityDocument_AdditionalComments(local).cs_cmCampos_min[1]) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos_min, new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos_min[0], new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_DespatchDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos_min, new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos_min[0], new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_Line(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line(local).cs_cmCampos_min, new clsEntityDocument_Line(local).cs_cmCampos_min[0], new clsEntityDocument_Line(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_Line_Description(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_Description(local).cs_cmCampos_min, new clsEntityDocument_Line_Description(local).cs_cmCampos_min[0], new clsEntityDocument_Line_Description(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_Line_PricingReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_PricingReference(local).cs_cmCampos_min, new clsEntityDocument_Line_PricingReference(local).cs_cmCampos_min[0], new clsEntityDocument_Line_PricingReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_Line_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos_min, new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos_min[0], new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_Line_AdditionalComments(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos_min, new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos_min[0], new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos_min, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos_min[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntitySummaryDocuments(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntitySummaryDocuments(local).cs_cmTabla_min, new clsEntitySummaryDocuments(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments_Notes(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntitySummaryDocuments_Notes(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntitySummaryDocuments_Notes(local).cs_cmTabla_min, new clsEntitySummaryDocuments_Notes(local).cs_cmCampos_min[0], new clsEntitySummaryDocuments_Notes(local).cs_cmCampos_min[1]) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos_min, new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos_min[0], new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos_min, new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos_min[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos_min, new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos_min[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos_min, new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos_min[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    //new MySqlCommand("CREATE TABLE " + new clsEntityVoidedDocuments(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityVoidedDocuments(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityVoidedDocuments(local).cs_cmTabla_min, new clsEntityVoidedDocuments(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos_min, new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos_min[0], new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();


                    //new MySqlCommand("CREATE TABLE " + new clsEntityPerception(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityPerception(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityPerception(local).cs_cmTabla_min, new clsEntityPerception(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityPerception_PerceptionLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntityPerception_PerceptionLine(local).cs_cmCampos_min, new clsEntityPerception_PerceptionLine(local).cs_cmCampos_min[0], new clsEntityPerception_PerceptionLine(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    //new MySqlCommand("CREATE TABLE " + new clsEntityRetention(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityRetention(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityRetention(local).cs_cmTabla_min, new clsEntityRetention(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityRetention_RetentionLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntityRetention_RetentionLine(local).cs_cmCampos_min, new clsEntityRetention_RetentionLine(local).cs_cmCampos_min[0], new clsEntityRetention_RetentionLine(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityDespatch(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntityDespatch(local).cs_cmTabla_min, new clsEntityDespatch(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDespatch_ShipmentStage(local).cs_cmCampos_min, new clsEntityDespatch_ShipmentStage(local).cs_cmCampos_min[0], new clsEntityDespatch_ShipmentStage(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos_min, new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos_min[0], new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch_OrderReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDespatch_OrderReference(local).cs_cmCampos_min, new clsEntityDespatch_OrderReference(local).cs_cmCampos_min[0], new clsEntityDespatch_OrderReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch_PortLocation(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDespatch_PortLocation(local).cs_cmCampos_min, new clsEntityDespatch_PortLocation(local).cs_cmCampos_min[0], new clsEntityDespatch_PortLocation(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntityDespatch_Line(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDespatch_Line(local).cs_cmCampos_min, new clsEntityDespatch_Line(local).cs_cmCampos_min[0], new clsEntityDespatch_Line(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    ////registro de compras
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntidadDocument(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntidadDocument(local).cs_cmTabla_min, new clsEntidadDocument(local).cs_cmCampos_min[0], "none") + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_TaxTotal(local).cs_cmCampos_min, new clsEntidadDocument_TaxTotal(local).cs_cmCampos_min[0], new clsEntidadDocument_TaxTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalComments(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntidadDocument_AdditionalComments(local).cs_cmCampos_min, local.Cs_pr_DBMS, new clsEntidadDocument_AdditionalComments(local).cs_cmTabla_min, new clsEntidadDocument_AdditionalComments(local).cs_cmCampos_min[0], new clsEntidadDocument_AdditionalComments(local).cs_cmCampos_min[1]) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos_min, new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos_min[0], new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_DespatchDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos_min, new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos_min[0], new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_Line(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_Line(local).cs_cmCampos_min, new clsEntidadDocument_Line(local).cs_cmCampos_min[0], new clsEntidadDocument_Line(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_Line_Description(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_Line_Description(local).cs_cmCampos_min, new clsEntidadDocument_Line_Description(local).cs_cmCampos_min[0], new clsEntidadDocument_Line_Description(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_Line_PricingReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos_min, new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos_min[0], new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_Line_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos_min, new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos_min[0], new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min, new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min[0], new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();
                    //new MySqlCommand("CREATE TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla_min + " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min, new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min[0], new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min[1], local.Cs_pr_DBMS) + "); ", conn).ExecuteNonQuery();

                    //conn.Close();
                }

                //tablas foraneas si es sql server
                if (local.Cs_pr_DBMS != "SQLite" && local.Cs_pr_DBMS != "MySQL")
                {

                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_TaxTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_TaxTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_AdditionalComments(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_AdditionalComments(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_DespatchDocumentReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_Description(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_Description(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + new clsEntityDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_PricingReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_PricingReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + new clsEntityDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_TaxTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + new clsEntityDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_AdditionalComments(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + new clsEntityDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(local).cs_cmTabla + " (" + new clsEntityDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla + " (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla + " (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_Notes(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_Notes(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments(local).cs_cmTabla + " (" + new clsEntitySummaryDocuments(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments(local).cs_cmTabla + " (" + new clsEntitySummaryDocuments(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityVoidedDocuments(local).cs_cmTabla + " (" + new clsEntityVoidedDocuments(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntityPerception_PerceptionLine(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityPerception_PerceptionLine(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityPerception(local).cs_cmTabla + " (" + new clsEntityPerception(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntityRetention_RetentionLine(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityRetention_RetentionLine(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityRetention(local).cs_cmTabla + " (" + new clsEntityRetention(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_ShipmentStage(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_ShipmentStage(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(local).cs_cmTabla + " (" + new clsEntityDespatch(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_ShipmentStage_Driver(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch_ShipmentStage(local).cs_cmTabla + " (" + new clsEntityDespatch_ShipmentStage(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_OrderReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_OrderReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(local).cs_cmTabla + " (" + new clsEntityDespatch(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_PortLocation(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_PortLocation(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(local).cs_cmTabla + " (" + new clsEntityDespatch(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_Line(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_Line(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(local).cs_cmTabla + " (" + new clsEntityDespatch(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                    //registro de compras
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_TaxTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_TaxTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_AdditionalComments(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_AdditionalComments(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_AdditionalDocumentReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_DespatchDocumentReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_DespatchDocumentReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_Description(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_Description(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(local).cs_cmTabla + " (" + new clsEntidadDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_PricingReference(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_PricingReference(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(local).cs_cmTabla + " (" + new clsEntidadDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_TaxTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_TaxTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(local).cs_cmTabla + " (" + new clsEntidadDocument_Line(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(local).cs_cmTabla + " (" + new clsEntidadDocument(local).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();

                
                    //new OdbcCommand("CREATE TABLE " + new clsEntityAccount().cs_cmTabla + " (" + Insercion(new clsEntityAccount().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();

                    if (local.Cs_pr_DBMS != "SQLite")
                    {
                        //  //crear procedimiento almacenado
                        StringBuilder sComandoSql = new StringBuilder("");

                        sComandoSql.Remove(0, sComandoSql.Length);
                        sComandoSql.Append("CREATE PROCEDURE BackupDB (" +
                                            " @BaseLocation   varchar(1024), " +
                                            " @NameBack   varchar(1024), " +
                                            " @BackupType     varchar(32) " +
                                            " ) ");

                        sComandoSql.Append(" AS " +
                                            " BEGIN ");

                        sComandoSql.Append(" Declare @DBName     varchar(255) = db_name() " +
                                            " Declare @FileName   varchar(586) " +
                                            " Declare @Date datetime = getdate() "
                                            );
                        sComandoSql.Append(" Set @BackupType = UPPER(@BackupType) " +
                                            " Set @FileName = @NameBack + (Case When @BackupType = 'FULL' Or @BackupType = 'DIFFERENTIAL' Then '.BAK' Else '.TRN' End) " +
                                            " ");

                        sComandoSql.Append(" Declare @FullPath varchar(1580) = '' " +
                                            " If Right(@BaseLocation,1) <> '\' " +
                                            " Begin " +
                                            " Set @BaseLocation = @BaseLocation + '\' " +
                                            " End ");


                        sComandoSql.Append(" Set @FullPath = @BaseLocation + @FileName " +
                                            " ");
                        sComandoSql.Append(" If @BackupType = 'FULL' " +
                                            " Begin " +
                                            " Backup database @DBName To Disk = @FullPath " +
                                            " End " +
                                            " ");
                        sComandoSql.Append(" Else If @BackupType = 'DIFFERENTIAL' " +
                                            " Begin " +
                                            " Backup database @DBName To Disk = @FullPath WITH DIFFERENTIAL " +
                                            " End " +
                                            " ");
                        sComandoSql.Append(" Else If @BackupType = 'LOG' " +
                                            " Begin " +
                                            " Backup database @DBName To Disk = @FullPath " +
                                            " End " +
                                            "End");

                        string sqlCommand = sComandoSql.ToString();

                        cs_pxConexion_basedatos.Open();
                        new OdbcCommand(sqlCommand, cs_pxConexion_basedatos).ExecuteNonQuery();
                        cs_pxConexion_basedatos.Close();
                    
                    }
                }
                //
                // cs_pxConexion_basedatos.Open();
                //new OdbcCommand("INSERT INTO cs_DatabaseWeb (cs_DatabaseWeb_Id, cp1, cp2, cp3, cp4, cp5, cp6, cp7, cp8) VALUES ('01', '', '', '', '', '', '', '', '') ; ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //new OdbcCommand("INSERT INTO cs_Declarant (cs_Declarant_Id, ruc, clavesol, usuariosol, rutacertificadodigital, email) VALUES ('01', '10440345755', 'MODDATOS', 'moddatos', 'C:/Certificados/certificado_sunat.cer', 'jcaso@contasis.net') ; ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //new OdbcCommand("INSERT INTO cs_Users (cs_Users_Id, cp1, cp2, cp3) VALUES ('01', 'admin', 'admin123', 'ADMIN'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //new OdbcCommand("INSERT INTO cs_Alarms (cs_Alarms_Id, envioautomatico, envioautomatico_minutos, envioautomatico_minutosvalor, envioautomatico_hora, envioautomatico_horavalor, enviomanual, enviomanual_mostrarglobo, enviomanual_mostrarglobo_minutosvalor, enviomanual_nomostrarglobo, iniciarconwindows) VALUES ('01', 'T', 'T', '6', 'F', '" + DateTime.Now.ToString()+"', 'F', 'F', '10', 'T', 'F'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //new OdbcCommand("INSERT INTO cs_Account (cp1, cp2, cp3) VALUES ('01', '01', '01'); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                //cs_pxConexion_basedatos.Close();

                //clsBaseMensaje.cs_pxMsgOk("OKE4");
                retorno = "" ;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                retorno = ex.ToString();
                // clsBaseMensaje.cs_pxMsg("Base de datos - Advertencia", ex.Message);
            }
            return retorno;
        }

        public clsEntityDatabaseLocal cs_fxObtenerUnoPorDeclaranteId(string cs_pr_Declarant_ID)
        {
           /* try
            {*/
                clsEntityDeclarant declaranteDB = new clsEntityDeclarant().cs_pxObtenerUnoPorId(cs_pr_Declarant_ID);

                SQLiteDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp9 ='" + cs_pr_Declarant_ID.Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                clsEntityDatabaseLocal LOCAL = null;
                while (datos.Read())
                {
                    LOCAL = new clsEntityDatabaseLocal(declaranteDB);
                    LOCAL.Cs_pr_DatabaseLocal_Id = datos[0].ToString();
                    LOCAL.Cs_pr_DBMS = datos[1].ToString();
                    LOCAL.Cs_pr_DBMSDriver = datos[2].ToString();
                    LOCAL.Cs_pr_DBMSServername = datos[3].ToString();
                    LOCAL.Cs_pr_DBMSServerport = datos[4].ToString();
                    LOCAL.Cs_pr_DBName = datos[5].ToString();
                    LOCAL.Cs_pr_DBUser = datos[6].ToString();
                    LOCAL.Cs_pr_DBPassword = datos[7].ToString();
                    LOCAL.Cs_pr_DBUse = datos[8].ToString();
                    LOCAL.Cs_pr_Declarant_Id = datos[9].ToString();
                }
                cs_pxConexion_basedatos.Close();
                return LOCAL;
           /* }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                return null;
            }*/
         
        }
        /// <summary>
        /// Metodo para insercion de campos 
        /// </summary>
        /// <param name="campos"></param>
        /// <returns></returns>
        private string Insercion(List<string> campos, string primary, string foreign, string DBMS)
        {
            string i_campos = null;
            foreach (var item in campos)
            {
                if (item == primary)
                {
                    switch (DBMS)
                    {
                        case "Microsoft SQL Server":
                            i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                            break;
                        case "SQLite":
                            i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                            break;
                        case "MySQL":
                            i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                            break;
                        default:
                            i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                            break;
                    }

                }else if(item == foreign) {

                    switch (DBMS)
                    {
                        case "Microsoft SQL Server":
                            i_campos += item + "  INT NOT NULL,";
                            break;
                        case "SQLite":
                            i_campos += item + "  INTEGER NOT NULL,";
                            break;
                        case "MySQL":
                            i_campos += item + "  INT NOT NULL,";
                            break;
                        default:
                            i_campos += item + "  INT NOT NULL,";
                            break;
                    }
                }
                else
                {
                    i_campos += item + "  VARCHAR(500),";
                }
                
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;

        }
        private string InsercionMin(List<string> campos)
        {
            string i_campos = null;
            foreach (var item in campos)
            {             
                    i_campos += item + "  VARCHAR(500),";
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }
        /// <summary>
        /// Metodo para obtner el string de insercion con campos de texto.
        /// </summary>
        /// <param name="campos"></param>
        /// <param name="DBMS"></param>
        /// <param name="tabla"></param>
        /// <returns>String insercion </returns>
        private string InsercionXML(List<string> campos, string DBMS, string tabla, string primary, string foreign)
        {
            string tipo = string.Empty;
            string i_campos = null;

            foreach (var item in campos)
            {
                switch (tabla)
                {
                    case "cs_cDocument":
                        if (item == "Cs_pr_XML")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }

                        }
                        break;                
                    case "cs_cDocument_AdditionalComments":
                        if (item == "Cs_pr_TagValor")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  TEXT,";
                                    break;
                                case "MySQL":
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  TEXT,";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_Document":
                        if (item == "cp29" || item == "cp30" || item == "cp31")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            } else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;                                      
                                }
                            }
                            else
                            {
                                if (DBMS== "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_VoidedDocuments":
                        if (item == "cp11" || item == "cp12")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }


                        }
                        break;
                    case "cs_SummaryDocuments":
                        if (item == "cp11" || item == "cp12")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_Perception":
                        if (item == "cp35" || item == "cp36")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_Retention":
                        if (item == "cp35" || item == "cp36")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_Despatch":
                        if (item == "cp32" || item == "cp33")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
                                    break;
                                case "MySQL":
                                    //tipo = "  MEDIUMTEXT(MAX),";
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  VARCHAR(500),";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    case "cs_Document_AdditionalComments":
                        if (item == "cp3")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  TEXT,";
                                    break;
                                case "MySQL":
                                    tipo = "  TEXT,";
                                    break;
                                case "SQLite":
                                    tipo = "  TEXT,";
                                    break;
                                case "PostgreSQL":
                                    tipo = "  TEXT,";
                                    break;
                                default:
                                    tipo = "  TEXT,";
                                    break;
                            }
                            i_campos += item + tipo;
                        }
                        else
                        {
                            if (item == primary)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }

                            }
                            else if (item == foreign)
                            {
                                switch (DBMS)
                                {
                                    case "Microsoft SQL Server":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    case "SQLite":
                                        i_campos += item + "  INTEGER NOT NULL,";
                                        break;
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                    default:
                                        i_campos += item + "  INT NOT NULL,";
                                        break;
                                }
                            }
                            else
                            {
                                if (DBMS == "Microsoft SQL Server")
                                {
                                    i_campos += item + "  VARCHAR(500),";
                                }
                                else
                                {
                                    i_campos += item + "  TEXT,";//Conflicto con SQL Server
                                }
                            }
                        }
                        break;
                    default:
                        if (item == primary)
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                    break;
                                case "SQLite":
                                    i_campos += item + "  INTEGER PRIMARY KEY AUTOINCREMENT,";
                                    break;
                                case "MySQL":
                                    i_campos += item + "  INT NOT NULL PRIMARY KEY AUTO_INCREMENT,";
                                    break;
                                default:
                                    i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                    break;
                            }

                        }
                        else if (item == foreign)
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    i_campos += item + "  INT NOT NULL,";
                                    break;
                                case "SQLite":
                                    i_campos += item + "  INTEGER NOT NULL,";
                                    break;
                                case "MySQL":
                                    i_campos += item + "  INT NOT NULL,";
                                    break;
                                default:
                                    i_campos += item + "  INT NOT NULL,";
                                    break;
                            }
                        }
                        else
                        {
                            if (DBMS == "Microsoft SQL Server")
                            {
                                i_campos += item + "  VARCHAR(500),";
                            }
                            else
                            {
                                i_campos += item + "  TEXT,";//Conflicto con SQL Server
                            }
                        }
                        break;
                }
            }

            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }
        /// <summary>
        /// Metodo para actualizar la estructura de la base de datos. 
        /// </summary>
        public void cs_pxVerificarBaseDatos()
        {
            try
            {
                clsEntityDatabaseLocal localConf    = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
                string prConexioncadenaservidor     = localConf.cs_prConexioncadenaservidor();
                string prConexioncadenabasedatos    = localConf.cs_prConexioncadenabasedatos();
               
                clsBaseConfiguracion configuracion  = new clsBaseConfiguracion();
                //clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(prConexioncadenabasedatos);
                OdbcDataReader datos_store;
                int errores = 0;
                //Base de datos local tabla modulos y tabla permisos
                if (File.Exists(clsBaseConexionSistema.conexionpath))
                {
                    SQLiteDataReader datoss;
                    SQLiteConnection cs_pxConexion_basedatos_local = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite = "SELECT name FROM sqlite_master WHERE type='table';";
                    datoss = new SQLiteCommand(sqlite, cs_pxConexion_basedatos_local).ExecuteReader();
                    List<string> tablas_sqlite = new List<string>();
                    while (datoss.Read())
                    {
                        string table_name = datoss[0].ToString();
                        tablas_sqlite.Add(table_name);
                    }
                    datoss.Close();
                    cs_pxConexion_basedatos_local.Close();
                    /**** buscar usuario master y actualizarlo si no existe ****/

                    bool existe_user_master = false;
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite_user = "SELECT * FROM cs_Users WHERE cs_Users_Id='02';";
                    datoss = new SQLiteCommand(sqlite_user, cs_pxConexion_basedatos_local).ExecuteReader();                                       
                    while (datoss.Read())
                    {
                        existe_user_master = true;
                    }
                    datoss.Close();
                    cs_pxConexion_basedatos_local.Close();

                    if (existe_user_master == false)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("INSERT INTO cs_Users (cs_Users_Id, cp1, cp2, cp3) VALUES ('02', 'master', 'master@123', 'MASTER'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        cs_pxConexion_basedatos_local.Close();
                    }

                    bool existe_cuenta_master = false;
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite_cuenta = "SELECT * FROM cs_Account WHERE cp1='02';";
                    datoss = new SQLiteCommand(sqlite_cuenta, cs_pxConexion_basedatos_local).ExecuteReader();
                    while (datoss.Read())
                    {
                        existe_cuenta_master = true;
                    }
                    datoss.Close();
                    cs_pxConexion_basedatos_local.Close();

                    if (existe_cuenta_master == false)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("INSERT INTO cs_Account (cp1, cp2, cp3) VALUES ('02', '', '02'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        cs_pxConexion_basedatos_local.Close();
                    }
                    /****---------------------------------------------------***/
                    string t_declarante = new clsEntityDeclarant().cs_cmTabla;
                    bool existe_declarante = existeTabla(tablas_sqlite, t_declarante);
                    if (existe_declarante)
                    {
                        //comprobar campos
                        List<string> campos_clase = new clsEntityDeclarant().cs_cmCampos;
                        List<string> campos_bd = getCamposTabla(t_declarante,"SQLite", clsBaseConexionSistema.conexionstring);
                        if (campos_clase.Count() > 0)
                        {
                            foreach (string campo in campos_clase)
                            {

                                bool campo_existe = existeCampo(campos_bd, campo);
                                if (!campo_existe)
                                {
                                    //insertar el campo
                                    bool error = agregarCampo(campo, t_declarante, "SQLite", clsBaseConexionSistema.conexionstring);
                                    if (error)
                                    {
                                        errores++;
                                    }
                                }
                            }
                        }
                    }
                 

                    /****------------------------------------------------------***/
                    string ts_1 = new clsEntityModulo().cs_cmTabla;
                    bool existe_s1 = existeTabla(tablas_sqlite, ts_1);
                    if (existe_s1 == false)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityModulo().cs_cmTabla + " (" + InsercionMin(new clsEntityModulo().cs_cmCampos) + "); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("DELETE FROM cs_Modulo ;", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('01', 'Factura Electrónica','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('02', 'Resumen Diario','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('03', 'Boleta de Venta','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('04', 'Comunicación de Baja','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('05', 'Retención electrónica','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('06', 'Reporte General','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('07', 'Envio Sunat','Factura'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('08', 'Formas de envio y alertas','Factura'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('09', 'Generar','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('10', 'Envio a Sunat','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('11', 'Formas de Envio y Alerta','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('12', 'Generar','Comunicacion de baja'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('13', 'Envio a Sunat','Comunicacion de baja'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('14', 'Envio de comprobantes','Comprobante de retencion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('15', 'Formas de envio y alerta','Comprobante de retencion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('16', 'Almacen Local','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('17', 'Almacen Web','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('18', 'Informacion del declarante','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('19', 'Usuarios','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('20', 'Permisos de usuario','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('21', 'Generacion de Backup','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('22', 'Restauracion de Backup','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('23', 'Ruta de archivos','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('24', 'Actualizacion de estructuras','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('25', 'Activacion de Licencia','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('28', 'Generar reversion - CRE','Resumen de reversión'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('29', 'Envio Sunat y Ticket - CRE','Resumen de reversión'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('30', 'Validar XML','Receptor de Compras'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('31', 'Registros de compras','Receptor de Compras'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('32', 'Transferencia de Datos','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();

                        cs_pxConexion_basedatos_local.Close();
                    }
                    else
                    {
                        //existe la tabla verificar los registro para agregar modulos
                        //seleccionar el id 25 si no existe borrar todos y crear nuevamente los registros.
                        bool eliminarRegistros = false;                    

                        //agregar en el menu para los permisos de los modulos 30 /->optimizar busqueda de modulos y agegacion de nuevos
                        eliminarRegistros = false;
                        SQLiteDataReader datosModulo2;
                        cs_pxConexion_basedatos_local.Open();
                        string sqliteModulo2 = "SELECT cs_Modulo_Id FROM cs_Modulo WHERE cs_Modulo_Id='30';";
                        datosModulo2 = new SQLiteCommand(sqliteModulo2, cs_pxConexion_basedatos_local).ExecuteReader();

                        if (datosModulo2.HasRows == false)
                        {
                            eliminarRegistros = true;
                        }
                        datosModulo2.Close();
                        cs_pxConexion_basedatos_local.Close();
                        if (eliminarRegistros == true)
                        {
                            cs_pxConexion_basedatos_local.Open();
                            string sqliteModuloItem = "DELETE FROM cs_Modulo;";                        
                            int datosModuloItem = new SQLiteCommand(sqliteModuloItem, cs_pxConexion_basedatos_local).ExecuteNonQuery();                          
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('01', 'Factura Electrónica','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('02', 'Resumen Diario','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('03', 'Boleta de Venta','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('04', 'Comunicación de Baja','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('05', 'Retención electrónica','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('06', 'Reporte General','Reporte'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('07', 'Envio Sunat','Factura'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('08', 'Formas de envio y alertas','Factura'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('09', 'Generar','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('10', 'Envio a Sunat','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('11', 'Formas de envío y alerta','Resumen Diario'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('12', 'Generar','Comunicacion de baja'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('13', 'Envio a Sunat','Comunicacion de baja'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('14', 'Envio de comprobantes','Comprobante de retencion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('15', 'Formas de envio y alerta','Comprobante de retencion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('16', 'Almacen Local','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('17', 'Almacen Web','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('18', 'Informacion del declarante','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('19', 'Usuarios','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('20', 'Permisos de usuario','Configuracion'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('21', 'Generacion de Backup','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('22', 'Restauracion de Backup','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('23', 'Ruta de archivos','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('24', 'Actualizacion de estructuras','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('25', 'Activacion de Licencia','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('28', 'Generar reversion - CRE','Resumen de reversión'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('29', 'Envio Sunat y Ticket - CRE','Resumen de reversión'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('30', 'Validar XML','Receptor de Compras'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('31', 'Registros de compras','Receptor de Compras'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                            cs_pxConexion_basedatos_local.Close();
                        }
                    }
                    //buscar 32  si no hay insertar
                    bool insertarRegistros = false;
                    SQLiteDataReader datosModulo3;
                    cs_pxConexion_basedatos_local.Open();
                    string sqliteModulo3 = "SELECT cs_Modulo_Id FROM cs_Modulo WHERE cs_Modulo_Id='32';";
                    datosModulo3 = new SQLiteCommand(sqliteModulo3, cs_pxConexion_basedatos_local).ExecuteReader();

                    if (datosModulo3.HasRows == false)
                    {
                        insertarRegistros = true;
                    }
                    datosModulo3.Close();
                    cs_pxConexion_basedatos_local.Close();
                    if (insertarRegistros == true)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('32', 'Transferencia de datos','Utilitarios'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        cs_pxConexion_basedatos_local.Close();
                    }
                    //end buscar
                    //buscar 33  si no hay insertar
                    insertarRegistros = false;
                    SQLiteDataReader datosModulo4;
                    cs_pxConexion_basedatos_local.Open();
                    string sqliteModulo4 = "SELECT cs_Modulo_Id FROM cs_Modulo WHERE cs_Modulo_Id='33';";
                    datosModulo4 = new SQLiteCommand(sqliteModulo4, cs_pxConexion_basedatos_local).ExecuteReader();

                    if (datosModulo4.HasRows == false)
                    {
                        insertarRegistros = true;
                    }
                    datosModulo4.Close();
                    cs_pxConexion_basedatos_local.Close();
                    if (insertarRegistros == true)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("INSERT INTO cs_Modulo (cs_Modulo_Id, modulo, padre) VALUES ('33', 'Certificado Digital','Ayuda'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        cs_pxConexion_basedatos_local.Close();
                    }
                    //end buscar
                    string ts_2 = new clsEntityPermisos().cs_cmTabla;
                    bool existe_s2 = existeTabla(tablas_sqlite, ts_2);
                    if (existe_s2 == false)
                    {
                        cs_pxConexion_basedatos_local.Open();
                        new SQLiteCommand("CREATE TABLE IF NOT EXISTS " + new clsEntityPermisos().cs_cmTabla + " (" + InsercionMin(new clsEntityPermisos().cs_cmCampos) + "); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("DELETE FROM cs_Permisos ;", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('01', '01', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('02', '02', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('03', '03', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('04', '04', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('05', '05', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('06', '06', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('07', '07', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('08', '08', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('09', '09', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('10', '10', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('11', '11', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('12', '12', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('13', '13', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('14', '14', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('15', '15', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('16', '16', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('17', '17', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('18', '18', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('19', '19', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('20', '20', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('21', '21', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('22', '22', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('23', '23', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('24', '24', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('25', '25', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('26', '28', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('27', '29', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('28', '30', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        new SQLiteCommand("INSERT INTO cs_Permisos (cs_Permisos_Id, modulo, cuenta, permitido) VALUES ('29', '31', '01','1'); ", cs_pxConexion_basedatos_local).ExecuteNonQuery();
                        cs_pxConexion_basedatos_local.Close();
                    }
                }

                //buscar procedimiento almacenado
                string sqlStore = String.Empty;
                int store = 0;
                if (localConf.Cs_pr_DBMS != "SQLite")
                {
                    sqlStore = " SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[BackupDB]') AND type in (N'P', N'PC')";
                    cs_pxConexion_basedatos.Open();
                    datos_store = new OdbcCommand(sqlStore, cs_pxConexion_basedatos).ExecuteReader();
                    while (datos_store.Read())
                    {
                        store++;
                    }
                    datos_store.Close();
                    cs_pxConexion_basedatos.Close();

                    if (store == 0)
                    {
                        //crear procedimiento almacenado
                        StringBuilder sComandoSql = new StringBuilder("");

                        sComandoSql.Remove(0, sComandoSql.Length);
                        sComandoSql.Append("CREATE PROCEDURE BackupDB (" +
                                           " @BaseLocation   varchar(1024), " +
                                           " @NameBack   varchar(1024), " +
                                           " @BackupType     varchar(32) " +
                                           " ) ");

                        sComandoSql.Append(" AS " +
                                           " BEGIN ");

                        sComandoSql.Append(" Declare @DBName     varchar(255) = db_name() " +
                                           " Declare @FileName   varchar(586) " +
                                           " Declare @Date datetime = getdate() "
                                           );
                        sComandoSql.Append(" Set @BackupType = UPPER(@BackupType) " +
                                           " Set @FileName = @NameBack + (Case When @BackupType = 'FULL' Or @BackupType = 'DIFFERENTIAL' Then '.BAK' Else '.TRN' End) " +
                                           " ");

                        sComandoSql.Append(" Declare @FullPath varchar(1580) = '' " +
                                           " If Right(@BaseLocation,1) <> '\' " +
                                           " Begin " +
                                           " Set @BaseLocation = @BaseLocation + '\' " +
                                           " End ");


                        sComandoSql.Append(" Set @FullPath = @BaseLocation + @FileName " +                                           
                                           " ");
                        sComandoSql.Append(" If @BackupType = 'FULL' " +
                                           " Begin " +
                                           " Backup database @DBName To Disk = @FullPath " +
                                           " End " +
                                           " ");
                        sComandoSql.Append(" Else If @BackupType = 'DIFFERENTIAL' " +
                                         " Begin " +
                                         " Backup database @DBName To Disk = @FullPath WITH DIFFERENTIAL " +
                                         " End " +
                                         " ");
                        sComandoSql.Append(" Else If @BackupType = 'LOG' " +
                                         " Begin " +
                                         " Backup database @DBName To Disk = @FullPath " +
                                         " End " +
                                         "End");

                        string sqlCommand = sComandoSql.ToString();
                        cs_pxConexion_basedatos.Open();
                        new OdbcCommand(sqlCommand, cs_pxConexion_basedatos).ExecuteNonQuery();
                        cs_pxConexion_basedatos.Close();
                    }
                }

                //
                //buscar todas las tablas que deberian tener entidades con un metodo
                //->>>si existe 
                //->>>>>>>verificar campos 
                //->>>>>>>si no estan completos agregarlos
                //->>>no existe 
                //->>>>>>>crear la tabla
               

                /****************Obtener listado de Tablas*****************/
                OdbcDataReader datos = null;
                string sql = String.Empty;
                if (localConf.Cs_pr_DBMS == "SQLite")
                {
                    sql = "SELECT name FROM sqlite_master WHERE type='table';";
                }
                else
                {
                    sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES; ";
                }

                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> lista_tablas = new List<string>();
                if (localConf.Cs_pr_DBMS == "SQLite")
                {
                    while (datos.Read())
                    {
                        string table_name = datos[0].ToString();
                        lista_tablas.Add(table_name);
                    }
                }
                else
                {
                    while (datos.Read())
                    {
                        string table_name = datos[2].ToString();
                        lista_tablas.Add(table_name);
                    }
                }
                datos.Close();
                cs_pxConexion_basedatos.Close();
                /***************************/

                #region Verificar Tabla clsEntityDocument    
                string t_1 = new clsEntityDocument(localConf).cs_cmTabla;
                bool existe_1 = existeTabla(lista_tablas, t_1);
                if (existe_1)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_1, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {

                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_1, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityDocument(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityDocument(localConf).cs_cmTabla, new clsEntityDocument(localConf).cs_cmCampos[0], new clsEntityDocument(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_AdditionalDocumentReference    
                string t_2 = new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmTabla;
                bool existe_2 = existeTabla(lista_tablas, t_2);
                if (existe_2)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_2, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_2, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos, new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos[0], new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_DespatchDocumentReference    
                string t_3 = new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmTabla;
                bool existe_3 = existeTabla(lista_tablas, t_3);
                if (existe_3)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_3, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_3, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos, new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos[0], new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line    
                string t_4 = new clsEntityDocument_Line(localConf).cs_cmTabla;
                bool existe_4 = existeTabla(lista_tablas, t_4);
                if (existe_4)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_4, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_4, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line(localConf).cs_cmCampos, new clsEntityDocument_Line(localConf).cs_cmCampos[0], new clsEntityDocument_Line(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {                    
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                       
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_Description    
                string t_5 = new clsEntityDocument_Line_Description(localConf).cs_cmTabla;
                bool existe_5 = existeTabla(lista_tablas, t_5);
                if (existe_5)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_Description(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_5, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_5, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_Description(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_Description(localConf).cs_cmCampos, new clsEntityDocument_Line_Description(localConf).cs_cmCampos[0], new clsEntityDocument_Line_Description(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_Description(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_Description(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(localConf).cs_cmTabla + " (" + new clsEntityDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                       
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_PricingReference    
                string t_6 = new clsEntityDocument_Line_PricingReference(localConf).cs_cmTabla;
                bool existe_6 = existeTabla(lista_tablas, t_6);
                if (existe_6)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_6, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_6, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_PricingReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos, new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos[0], new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_PricingReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(localConf).cs_cmTabla + " (" + new clsEntityDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_TaxTotal    
                string t_7 = new clsEntityDocument_Line_TaxTotal(localConf).cs_cmTabla;
                bool existe_7 = existeTabla(lista_tablas, t_7);
                if (existe_7)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_7, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_7, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_TaxTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos, new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos[0], new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                      new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_TaxTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(localConf).cs_cmTabla + " (" + new clsEntityDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation    
                string t_8 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla;
                bool existe_8 = existeTabla(lista_tablas, t_8);
                if (existe_8)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_8, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_8, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {                    
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal    
                string t_9 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla;
                bool existe_9 = existeTabla(lista_tablas, t_9);
                if (existe_9)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_9, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_9, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla + " (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty    
                string t_10 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla;
                bool existe_10 = existeTabla(lista_tablas, t_10);
                if (existe_10)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_10, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_10, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos, new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[0], new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla + " (" + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments    
                string t_11 = new clsEntitySummaryDocuments(localConf).cs_cmTabla;
                bool existe_11 = existeTabla(lista_tablas, t_11);
                if (existe_11)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_11, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_11, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntitySummaryDocuments(localConf).cs_cmTabla, new clsEntitySummaryDocuments(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_Notes    
                string t_12 = new clsEntitySummaryDocuments_Notes(localConf).cs_cmTabla;
                bool existe_12 = existeTabla(lista_tablas, t_12);
                if (existe_12)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_12, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_12, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_Notes(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntitySummaryDocuments_Notes(localConf).cs_cmTabla, new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_Notes(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments(localConf).cs_cmTabla + " (" + new clsEntitySummaryDocuments(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine    
                string t_13 = new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla;
                bool existe_13 = existeTabla(lista_tablas, t_13);
                if (existe_13)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_13, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_13, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments(localConf).cs_cmTabla + " (" + new clsEntitySummaryDocuments(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge    
                string t_14 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmTabla;
                bool existe_14 = existeTabla(lista_tablas, t_14);
                if (existe_14)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_14, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_14, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment    
                string t_15 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmTabla;
                bool existe_15 = existeTabla(lista_tablas, t_15);
                if (existe_15)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_15, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_15, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal    
                string t_16 = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmTabla;
                bool existe_16 = existeTabla(lista_tablas, t_16);
                if (existe_16)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_16, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_16, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos, new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos[0], new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                       new OdbcCommand("ALTER TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla + " (" + new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_TaxTotal    
                string t_17 = new clsEntityDocument_TaxTotal(localConf).cs_cmTabla;
                bool existe_17 = existeTabla(lista_tablas, t_17);
                if (existe_17)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_TaxTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_17, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_17, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_TaxTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_TaxTotal(localConf).cs_cmCampos, new clsEntityDocument_TaxTotal(localConf).cs_cmCampos[0], new clsEntityDocument_TaxTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_TaxTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_TaxTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                       
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityVoidedDocuments    
                string t_18 = new clsEntityVoidedDocuments(localConf).cs_cmTabla;
                bool existe_18 = existeTabla(lista_tablas, t_18);
                if (existe_18)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityVoidedDocuments(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_18, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_18, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityVoidedDocuments(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityVoidedDocuments(localConf).cs_cmTabla, new clsEntityVoidedDocuments(localConf).cs_cmCampos[0], new clsEntityVoidedDocuments(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityVoidedDocuments_VoidedDocumentsLine    
                string t_19 = new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmTabla;
                bool existe_19 = existeTabla(lista_tablas, t_19);
                if (existe_19)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_19, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_19, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmTabla + " (" + Insercion(new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos, new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos[0], new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {              
                        new OdbcCommand("ALTER TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityVoidedDocuments(localConf).cs_cmTabla + " (" + new clsEntityVoidedDocuments(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                   
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_AdditionalComments    
                string t_20 = new clsEntityDocument_AdditionalComments(localConf).cs_cmTabla;
                bool existe_20 = existeTabla(lista_tablas, t_20);
                if (existe_20)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_20, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_20, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityDocument_AdditionalComments(localConf).cs_cmTabla, new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos[0], new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments().cs_cmTabla + " (" + Insercion(new clsEntityDocument_AdditionalComments().cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntityDocument_AdditionalComments(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                       
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_AdditionalComments    
                string t_21 = new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmTabla;
                bool existe_21 = existeTabla(lista_tablas, t_21);
                if (existe_21)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_21, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_21, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos, new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos[0], new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                       new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument_Line(localConf).cs_cmTabla + " (" + new clsEntityDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityPerception    
                string t_22 = new clsEntityPerception(localConf).cs_cmTabla;
                bool existe_22 = existeTabla(lista_tablas, t_22);
                if (existe_22)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityPerception(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_22, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_22, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityPerception(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityPerception(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityPerception(localConf).cs_cmTabla, new clsEntityPerception(localConf).cs_cmCampos[0], new clsEntityPerception(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityPerception_PerceptionLine    
                string t_23 = new clsEntityPerception_PerceptionLine(localConf).cs_cmTabla;
                bool existe_23 = existeTabla(lista_tablas, t_23);
                if (existe_23)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_23, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_23, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityPerception_PerceptionLine(localConf).cs_cmTabla + " (" + Insercion(new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos, new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos[0], new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityPerception_PerceptionLine(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityPerception(localConf).cs_cmTabla + " (" + new clsEntityPerception(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                    
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityRetention    
                string t_24 = new clsEntityRetention(localConf).cs_cmTabla;
                bool existe_24 = existeTabla(lista_tablas, t_24);
                if (existe_24)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityRetention(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_24, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_24, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityRetention(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityRetention(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityRetention(localConf).cs_cmTabla, new clsEntityRetention(localConf).cs_cmCampos[0], new clsEntityRetention(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityRetention_RetentionLine    
                string t_25 = new clsEntityRetention_RetentionLine(localConf).cs_cmTabla;
                bool existe_25 = existeTabla(lista_tablas, t_25);
                if (existe_25)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityRetention_RetentionLine(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_25, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_25, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityRetention_RetentionLine(localConf).cs_cmTabla + " (" + Insercion(new clsEntityRetention_RetentionLine(localConf).cs_cmCampos, new clsEntityRetention_RetentionLine(localConf).cs_cmCampos[0], new clsEntityRetention_RetentionLine(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                       new OdbcCommand("ALTER TABLE " + new clsEntityRetention_RetentionLine(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityRetention_RetentionLine(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityRetention(localConf).cs_cmTabla + " (" + new clsEntityRetention(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                    
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch    
                string t_26 = new clsEntityDespatch(localConf).cs_cmTabla;
                bool existe_26 = existeTabla(lista_tablas, t_26);
                if (existe_26)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_26, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_26, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntityDespatch(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntityDespatch(localConf).cs_cmTabla, new clsEntityDespatch(localConf).cs_cmCampos[0], new clsEntityDespatch(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_ShipmentStage    
                string t_27 = new clsEntityDespatch_ShipmentStage(localConf).cs_cmTabla;
                bool existe_27 = existeTabla(lista_tablas, t_27);
                if (existe_27)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_27, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_27, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos, new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos[0], new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_ShipmentStage(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(localConf).cs_cmTabla + " (" + new clsEntityDespatch(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_ShipmentStage_Driver    
                string t_28 = new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmTabla;
                bool existe_28 = existeTabla(lista_tablas, t_28);
                if (existe_28)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_28, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_28, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos, new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos[0], new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch_ShipmentStage(localConf).cs_cmTabla + " (" + new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_OrderReference    
                string t_29 = new clsEntityDespatch_OrderReference(localConf).cs_cmTabla;
                bool existe_29 = existeTabla(lista_tablas, t_29);
                if (existe_29)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_OrderReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_29, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_29, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_OrderReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_OrderReference(localConf).cs_cmCampos, new clsEntityDespatch_OrderReference(localConf).cs_cmCampos[0], new clsEntityDespatch_OrderReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_OrderReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_OrderReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(localConf).cs_cmTabla + " (" + new clsEntityDespatch(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_PortLocation    
                string t_30 = new clsEntityDespatch_PortLocation(localConf).cs_cmTabla;
                bool existe_30 = existeTabla(lista_tablas, t_30);
                if (existe_30)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_PortLocation(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_30, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_30, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_PortLocation(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_PortLocation(localConf).cs_cmCampos, new clsEntityDespatch_PortLocation(localConf).cs_cmCampos[0], new clsEntityDespatch_PortLocation(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_PortLocation(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_PortLocation(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(localConf).cs_cmTabla + " (" + new clsEntityDespatch(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                     }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_Line    
                string t_31 = new clsEntityDespatch_Line(localConf).cs_cmTabla;
                bool existe_31 = existeTabla(lista_tablas, t_31);
                if (existe_31)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_Line(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_31, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_31, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDespatch_Line(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDespatch_Line(localConf).cs_cmCampos, new clsEntityDespatch_Line(localConf).cs_cmCampos[0], new clsEntityDespatch_Line(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDespatch_Line(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDespatch_Line(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDespatch(localConf).cs_cmTabla + " (" + new clsEntityDespatch(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument   
                string t_32 = new clsEntidadDocument(localConf).cs_cmTabla;
                bool existe_32 = existeTabla(lista_tablas, t_32);
                if (existe_32)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_32, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_32, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    //string fd = "CREATE TABLE " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntidadDocument(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntidadDocument(localConf).cs_cmTabla, new clsEntidadDocument(localConf).cs_cmCampos[0], "none") + "); ";
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntidadDocument(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntidadDocument(localConf).cs_cmTabla, new clsEntidadDocument(localConf).cs_cmCampos[0], "none") + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_TaxTotal   
                string t_33 = new clsEntidadDocument_TaxTotal(localConf).cs_cmTabla;
                bool existe_33 = existeTabla(lista_tablas, t_33);
                if (existe_33)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_33, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_33, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_TaxTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos, new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos[0], new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_TaxTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_AdditionalComments   
                string t_34 = new clsEntidadDocument_AdditionalComments(localConf).cs_cmTabla;
                bool existe_34 = existeTabla(lista_tablas, t_34);
                if (existe_34)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_34, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_34, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalComments(localConf).cs_cmTabla + " (" + InsercionXML(new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos, localConf.Cs_pr_DBMS, new clsEntidadDocument_AdditionalComments(localConf).cs_cmTabla, new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos[0], new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos[1]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //tablas foraneas si es sql server
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_AdditionalComments(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_AdditionalDocumentReference   
                string t_35 = new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmTabla;
                bool existe_35 = existeTabla(lista_tablas, t_35);
                if (existe_35)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_35, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_35, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos, new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos[0], new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                     
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_DespatchDocumentReference   
                string t_36 = new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmTabla;
                bool existe_36 = existeTabla(lista_tablas, t_36);
                if (existe_36)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_36, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_36, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos, new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos[0], new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line   
                string t_37 = new clsEntidadDocument_Line(localConf).cs_cmTabla;
                bool existe_37 = existeTabla(lista_tablas, t_37);
                if (existe_37)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_37, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_37, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line(localConf).cs_cmCampos, new clsEntidadDocument_Line(localConf).cs_cmCampos[0], new clsEntidadDocument_Line(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_Description   
                string t_38 = new clsEntidadDocument_Line_Description(localConf).cs_cmTabla;
                bool existe_38 = existeTabla(lista_tablas, t_38);
                if (existe_38)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_Description(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_38, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_38, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_Description(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_Description(localConf).cs_cmCampos, new clsEntidadDocument_Line_Description(localConf).cs_cmCampos[0], new clsEntidadDocument_Line_Description(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_Description(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_Description(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(localConf).cs_cmTabla + " (" + new clsEntidadDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                        
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_PricingReference   
                string t_39 = new clsEntidadDocument_Line_PricingReference(localConf).cs_cmTabla;
                bool existe_39 = existeTabla(lista_tablas, t_39);
                if (existe_39)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_39, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_39, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_PricingReference(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos, new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos[0], new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_PricingReference(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(localConf).cs_cmTabla + " (" + new clsEntidadDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                    
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_TaxTotal   
                string t_40 = new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmTabla;
                bool existe_40 = existeTabla(lista_tablas, t_40);
                if (existe_40)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_40, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_40, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos, new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos[0], new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument_Line(localConf).cs_cmTabla + " (" + new clsEntidadDocument_Line(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal   
                string t_41 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla;
                bool existe_41 = existeTabla(lista_tablas, t_41);
                if (existe_41)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_41, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_41, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos, new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[0], new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                         new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();                      
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty   
                string t_42 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla;
                bool existe_42 = existeTabla(lista_tablas, t_42);
                if (existe_42)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_42, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_42, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla + " (" + Insercion(new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos, new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[0], new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                       new OdbcCommand("ALTER TABLE " + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntidadDocument(localConf).cs_cmTabla + " (" + new clsEntidadDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Advance    
                string t_43 = new clsEntityDocument_Advance(localConf).cs_cmTabla;
                bool existe_43 = existeTabla(lista_tablas, t_43);
                if (existe_43)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Advance(localConf).cs_cmCampos;
                    List<string> campos_bd = getCamposTabla(t_43, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(campos_bd, campo);
                            if (!campo_existe)
                            {
                                //insertar el campo
                                bool error = agregarCampo(campo, t_43, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                                if (error)
                                {
                                    errores++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    cs_pxConexion_basedatos.Open();
                    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Advance(localConf).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Advance(localConf).cs_cmCampos, new clsEntityDocument_Advance(localConf).cs_cmCampos[0], new clsEntityDocument_Advance(localConf).cs_cmCampos[1], localConf.Cs_pr_DBMS) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    if (localConf.Cs_pr_DBMS != "SQLite")
                    {
                        new OdbcCommand("ALTER TABLE " + new clsEntityDocument_Advance(localConf).cs_cmTabla + "  ADD FOREIGN KEY (" + new clsEntityDocument_Advance(localConf).cs_cmCampos[1] + ") REFERENCES " + new clsEntityDocument(localConf).cs_cmTabla + " (" + new clsEntityDocument(localConf).cs_cmCampos[0] + ")", cs_pxConexion_basedatos).ExecuteNonQuery();
                    }
                    cs_pxConexion_basedatos.Close();
                }
                #endregion
                if (errores > 0)
                {
                    // MessageBox.Show("Errores" + errores.ToString());
                    clsBaseLog.cs_pxRegistarAdd("Nuevos campos agregados:" + errores.ToString());
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistar(ex.ToString());
            }
           
        }
        public Boolean cs_pxSeDebeActualizarBD()
        {
            int nuevos = 0;
            bool es_necesario_verificar = false;
            try
            {
                if (File.Exists(clsBaseConexionSistema.conexionpath))
                {
                    SQLiteDataReader datoss;
                    SQLiteConnection cs_pxConexion_basedatos_local = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite = "SELECT name FROM sqlite_master WHERE type='table';";
                    datoss = new SQLiteCommand(sqlite, cs_pxConexion_basedatos_local).ExecuteReader();
                    List<string> tablas_sqlite = new List<string>();
                    while (datoss.Read())
                    {
                        string table_name = datoss[0].ToString();
                        tablas_sqlite.Add(table_name);
                    }
                    cs_pxConexion_basedatos_local.Close();

                    string ts_1 = new clsEntityModulo().cs_cmTabla;
                    bool existe_s1 = existeTabla(tablas_sqlite, ts_1);
                    if (existe_s1 == false)
                    {
                        nuevos++;
                    }

                    string ts_2 = new clsEntityPermisos().cs_cmTabla;
                    bool existe_s2 = existeTabla(tablas_sqlite, ts_2);
                    if (existe_s2 == false)
                    {
                        nuevos++;
                    }

                    /**** buscar usuario master y actualizarlo si no existe ****/

                    bool existe_user_master = false;
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite_user = "SELECT * FROM cs_Users WHERE cs_Users_Id='02';";
                    datoss = new SQLiteCommand(sqlite_user, cs_pxConexion_basedatos_local).ExecuteReader();
                    while (datoss.Read())
                    {
                        existe_user_master = true;
                    }
                    cs_pxConexion_basedatos_local.Close();

                    if (existe_user_master == false)
                    {
                        nuevos++;
                    }

                    bool existe_cuenta_master = false;
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite_cuenta = "SELECT * FROM cs_Account WHERE cp1='02';";
                    datoss = new SQLiteCommand(sqlite_cuenta, cs_pxConexion_basedatos_local).ExecuteReader();
                    while (datoss.Read())
                    {
                        existe_cuenta_master = true;
                    }
                    cs_pxConexion_basedatos_local.Close();

                    if (existe_cuenta_master == false)
                    {
                        nuevos++;
                    }

                    #region verificar modulo en tabla 

                    bool existe_modulo33 = false;
                    cs_pxConexion_basedatos_local.Open();
                    string sqlite_33 = "SELECT cs_Modulo_Id FROM cs_Modulo WHERE cs_Modulo_Id='33';";
                    datoss = new SQLiteCommand(sqlite_33, cs_pxConexion_basedatos_local).ExecuteReader();
                    while (datoss.Read())
                    {
                        existe_modulo33 = true;
                    }
                    datoss.Close();
                    cs_pxConexion_basedatos_local.Close();

                    if (existe_modulo33 == false)
                    {
                        nuevos++;
                    }
                    #endregion

                    #region Verificar Tabla clsEntityDeclarant  
                    string t_declarante     = new clsEntityDeclarant().cs_cmTabla;
                    bool existe_declarante  = existeTabla(tablas_sqlite, t_declarante);
                    if (existe_declarante)
                    {
                        //comprobar campos
                        List<string> campos_clase = new clsEntityDeclarant().cs_cmCampos;
                        //obtener campos de la tabla.
                        List<string> campos_bd = getCamposTabla(t_declarante, "SQLite", clsBaseConexionSistema.conexionstring);
                        //comparar listas y devolver cuantos nuevoshay para crear
                        int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                        nuevos = nuevos + nuevos_existe;
                       
                    }
                    else
                    {
                        nuevos++;
                    }
                    #endregion
                    /****---------------------------------------------------***/
                }
                clsEntityDatabaseLocal localConf = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
                string prConexioncadenaservidor = localConf.cs_prConexioncadenaservidor();
                string prConexioncadenabasedatos = localConf.cs_prConexioncadenabasedatos();


               // clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
               // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(prConexioncadenabasedatos);

                /****************Obtener listado de Tablas*****************/
                OdbcDataReader datos        = null;
                OdbcDataReader datos_store  = null;
                string sql = String.Empty;
                if (localConf.Cs_pr_DBMS == "SQLite")
                {
                    sql = "SELECT name FROM sqlite_master WHERE type='table';";
                }
                else
                {
                    sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES; ";
                }

                cs_pxConexion_basedatos.ConnectionTimeout = 3;
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> lista_tablas = new List<string>();

                if (localConf.Cs_pr_DBMS == "SQLite")
                {
                    while (datos.Read())
                    {
                        string table_name = datos[0].ToString();
                        lista_tablas.Add(table_name);
                    }
                }
                else
                {
                    while (datos.Read())
                    {
                        string table_name = datos[2].ToString();
                        lista_tablas.Add(table_name);
                    }
                }

                cs_pxConexion_basedatos.Close();
                /***************************/
                // obtener si existe el procedimiento almacenado
                string sqlStore = String.Empty;
                int store = 0;
                if (localConf.Cs_pr_DBMS != "SQLite")
                {
                    sqlStore = " SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[BackupDB]') AND type in (N'P', N'PC')";
                    cs_pxConexion_basedatos.Open();
                    datos_store = new OdbcCommand(sqlStore, cs_pxConexion_basedatos).ExecuteReader();
                    while (datos_store.Read())
                    {
                        store++;
                    }

                    if (store == 0)
                    {
                        nuevos++;
                    }
                }

                //////

                #region Verificar Tabla clsEntityDocument    
                string t_1 = new clsEntityDocument(localConf).cs_cmTabla;
                bool existe_1 = existeTabla(lista_tablas, t_1);
                if (existe_1)
                {
                    //comprobar campos
                    List<string> campos_clase   = new clsEntityDocument(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd      = getCamposTabla(t_1, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_1, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_AdditionalDocumentReference    
                string t_2 = new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmTabla;
                bool existe_2 = existeTabla(lista_tablas, t_2);
                if (existe_2)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_AdditionalDocumentReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_2, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_2, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_DespatchDocumentReference    
                string t_3 = new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmTabla;
                bool existe_3 = existeTabla(lista_tablas, t_3);
                if (existe_3)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_DespatchDocumentReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_3, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_3, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line    
                string t_4 = new clsEntityDocument_Line(localConf).cs_cmTabla;
                bool existe_4 = existeTabla(lista_tablas, t_4);
                if (existe_4)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_4, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_4, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_Description    
                string t_5 = new clsEntityDocument_Line_Description(localConf).cs_cmTabla;
                bool existe_5 = existeTabla(lista_tablas, t_5);
                if (existe_5)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_Description(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_5, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_5, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_PricingReference    
                string t_6 = new clsEntityDocument_Line_PricingReference(localConf).cs_cmTabla;
                bool existe_6 = existeTabla(lista_tablas, t_6);
                if (existe_6)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_PricingReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_6, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_6, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_TaxTotal    
                string t_7 = new clsEntityDocument_Line_TaxTotal(localConf).cs_cmTabla;
                bool existe_7 = existeTabla(lista_tablas, t_7);
                if (existe_7)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_TaxTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_7, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_7, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation    
                string t_8 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmTabla;
                bool existe_8 = existeTabla(lista_tablas, t_8);
                if (existe_8)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_8, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_8, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal    
                string t_9 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla;
                bool existe_9 = existeTabla(lista_tablas, t_9);
                if (existe_9)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_9, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_9, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty    
                string t_10 = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla;
                bool existe_10 = existeTabla(lista_tablas, t_10);
                if (existe_10)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_10, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_10, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments    
                string t_11 = new clsEntitySummaryDocuments(localConf).cs_cmTabla;
                bool existe_11 = existeTabla(lista_tablas, t_11);
                if (existe_11)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_11, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_11, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_Notes    
                string t_12 = new clsEntitySummaryDocuments_Notes(localConf).cs_cmTabla;
                bool existe_12 = existeTabla(lista_tablas, t_12);
                if (existe_12)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_Notes(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_12, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_12, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine    
                string t_13 = new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmTabla;
                bool existe_13 = existeTabla(lista_tablas, t_13);
                if (existe_13)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_13, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_13, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge    
                string t_14 = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmTabla;
                bool existe_14 = existeTabla(lista_tablas, t_14);
                if (existe_14)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_14, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_14, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment    
                string t_15 = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmTabla;
                bool existe_15 = existeTabla(lista_tablas, t_15);
                if (existe_15)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_15, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_15, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal    
                string t_16 = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmTabla;
                bool existe_16 = existeTabla(lista_tablas, t_16);
                if (existe_16)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_16, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_16, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_TaxTotal    
                string t_17 = new clsEntityDocument_TaxTotal(localConf).cs_cmTabla;
                bool existe_17 = existeTabla(lista_tablas, t_17);
                if (existe_17)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_TaxTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_17, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_17, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityVoidedDocuments    
                string t_18 = new clsEntityVoidedDocuments(localConf).cs_cmTabla;
                bool existe_18 = existeTabla(lista_tablas, t_18);
                if (existe_18)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityVoidedDocuments(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_18, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_18, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityVoidedDocuments_VoidedDocumentsLine    
                string t_19 = new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmTabla;
                bool existe_19 = existeTabla(lista_tablas, t_19);
                if (existe_19)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityVoidedDocuments_VoidedDocumentsLine(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_19, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_19, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_AdditionalComments    
                string t_20 = new clsEntityDocument_AdditionalComments(localConf).cs_cmTabla;
                bool existe_20 = existeTabla(lista_tablas, t_20);
                if (existe_20)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_AdditionalComments(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_20, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_20, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDocument_Line_AdditionalComments    
                string t_21 = new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmTabla;
                bool existe_21 = existeTabla(lista_tablas, t_21);
                if (existe_21)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDocument_Line_AdditionalComments(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_21, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_21, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityPerception    
                string t_22 = new clsEntityPerception(localConf).cs_cmTabla;
                bool existe_22 = existeTabla(lista_tablas, t_22);
                if (existe_22)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityPerception(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_22, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_22, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityPerception_PerceptionLine    
                string t_23 = new clsEntityPerception_PerceptionLine(localConf).cs_cmTabla;
                bool existe_23 = existeTabla(lista_tablas, t_23);
                if (existe_23)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityPerception_PerceptionLine(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_23, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_23, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityRetention    
                string t_24 = new clsEntityRetention(localConf).cs_cmTabla;
                bool existe_24 = existeTabla(lista_tablas, t_24);
                if (existe_24)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityRetention(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_24, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_24, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityRetention_RetentionLine    
                string t_25 = new clsEntityRetention_RetentionLine(localConf).cs_cmTabla;
                bool existe_25 = existeTabla(lista_tablas, t_25);
                if (existe_25)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityRetention_RetentionLine(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_25, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_25, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch    
                string t_26 = new clsEntityDespatch(localConf).cs_cmTabla;
                bool existe_26 = existeTabla(lista_tablas, t_26);
                if (existe_26)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_26, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_26, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_ShipmentStage    
                string t_27 = new clsEntityDespatch_ShipmentStage(localConf).cs_cmTabla;
                bool existe_27 = existeTabla(lista_tablas, t_27);
                if (existe_27)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_ShipmentStage(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_27, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_27, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_ShipmentStage_Driver    
                string t_28 = new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmTabla;
                bool existe_28 = existeTabla(lista_tablas, t_28);
                if (existe_28)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_ShipmentStage_Driver(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_28, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /* if (campos_clase.Count() > 0)
                     {
                         foreach (string campo in campos_clase)
                         {
                             bool campo_existe = existeCampo(t_28, campo);
                             if (!campo_existe)
                             {
                                 nuevos++;
                             }
                         }
                     }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_OrderReference    
                string t_29 = new clsEntityDespatch_OrderReference(localConf).cs_cmTabla;
                bool existe_29 = existeTabla(lista_tablas, t_29);
                if (existe_29)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_OrderReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_29, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_29, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_PortLocation    
                string t_30 = new clsEntityDespatch_PortLocation(localConf).cs_cmTabla;
                bool existe_30 = existeTabla(lista_tablas, t_30);
                if (existe_30)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_PortLocation(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_30, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                    /*
                    if (campos_clase.Count() > 0)
                    {
                        foreach (string campo in campos_clase)
                        {
                            bool campo_existe = existeCampo(t_30, campo);
                            if (!campo_existe)
                            {
                                nuevos++;
                            }
                        }
                    }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntityDespatch_Line    
                string t_31 = new clsEntityDespatch_Line(localConf).cs_cmTabla;
                bool existe_31 = existeTabla(lista_tablas, t_31);
                if (existe_31)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntityDespatch_Line(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_31, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;

                    /*  if (campos_clase.Count() > 0)
                      {
                          foreach (string campo in campos_clase)
                          {
                              bool campo_existe = existeCampo(t_31, campo);
                              if (!campo_existe)
                              {
                                  nuevos++;
                              }
                          }
                      }*/
                }
                else
                {
                    nuevos++;
                }
                #endregion

                #region Verificar Tabla clsEntidadDocument    
                string t_32     = new clsEntidadDocument(localConf).cs_cmTabla;
                bool existe_32  = existeTabla(lista_tablas, t_32);
                if (existe_32)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_32, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_TaxTotal    
                string t_33 = new clsEntidadDocument_TaxTotal(localConf).cs_cmTabla;
                bool existe_33 = existeTabla(lista_tablas, t_33);
                if (existe_33)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_TaxTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_33, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_AdditionalComments    
                string t_34 = new clsEntidadDocument_AdditionalComments(localConf).cs_cmTabla;
                bool existe_34 = existeTabla(lista_tablas, t_34);
                if (existe_34)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_AdditionalComments(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_34, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_AdditionalDocumentReference    
                string t_35 = new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmTabla;
                bool existe_35 = existeTabla(lista_tablas, t_35);
                if (existe_35)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_AdditionalDocumentReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_35, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_DespatchDocumentReference    
                string t_36 = new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmTabla;
                bool existe_36 = existeTabla(lista_tablas, t_36);
                if (existe_36)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_DespatchDocumentReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_36, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line    
                string t_37 = new clsEntidadDocument_Line(localConf).cs_cmTabla;
                bool existe_37 = existeTabla(lista_tablas, t_37);
                if (existe_37)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_37, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_Description    
                string t_38 = new clsEntidadDocument_Line_Description(localConf).cs_cmTabla;
                bool existe_38 = existeTabla(lista_tablas, t_38);
                if (existe_38)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_Description(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_38, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_PricingReference    
                string t_39 = new clsEntidadDocument_Line_PricingReference(localConf).cs_cmTabla;
                bool existe_39 = existeTabla(lista_tablas, t_39);
                if (existe_39)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_PricingReference(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_39, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_Line_TaxTotal    
                string t_40 = new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmTabla;
                bool existe_40 = existeTabla(lista_tablas, t_40);
                if (existe_40)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_Line_TaxTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_40, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal    
                string t_41 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmTabla;
                bool existe_41 = existeTabla(lista_tablas, t_41);
                if (existe_41)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_41, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                #region Verificar Tabla clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty    
                string t_42 = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmTabla;
                bool existe_42 = existeTabla(lista_tablas, t_42);
                if (existe_42)
                {
                    //comprobar campos
                    List<string> campos_clase = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localConf).cs_cmCampos;
                    //obtener campos de la tabla.
                    List<string> campos_bd = getCamposTabla(t_42, localConf.Cs_pr_DBMS, prConexioncadenabasedatos);
                    //comparar listas y devolver cuantos nuevoshay para crear
                    int nuevos_existe = getCantidadNuevosCampos(campos_clase, campos_bd);
                    nuevos = nuevos + nuevos_existe;
                }
                else
                {
                    nuevos++;
                }
                #endregion
                

                if (nuevos > 0)
                {
                    es_necesario_verificar = true;
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                es_necesario_verificar = false;
            }
            return es_necesario_verificar;
        }
        private int getCantidadNuevosCampos(List<string> campos_clase, List<string> campos_bd)
        {
            int nuevos_campos_agregados = 0;
            if (campos_clase.Count > 0)
            {
                foreach (string campo_en_clase in campos_clase)
                {
                    bool existe = false;
                    existe = existeCampo(campos_bd, campo_en_clase);
                    if (existe == false)
                    {
                        nuevos_campos_agregados++;
                    }
                }
            }

            return nuevos_campos_agregados;
        }
        public static Boolean existeCampo(List<string> lista_campos, string campo_name)
        {
            bool existe = false;
            try
            {
                if (lista_campos.Count() > 0)
                {
                    foreach (string linea in lista_campos)
                    {
                        if (string.Equals(linea, campo_name))
                        {
                            existe = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                existe = false;
            }

            return existe;
        }
        public static Boolean existeTabla(List<string> lista_tablas, string name_table)
        {
            bool existe = false;
            try
            {
                if (lista_tablas.Count() > 0)
                {
                    foreach (string linea in lista_tablas)
                    {
                        if (string.Equals(linea, name_table))
                        {
                            existe = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                existe = false;
            }

            return existe;
        }
        /*public static Boolean existeCampo(string name_table, string name_campo,string dbms,string cadenaBaseDatos)
        {
            bool existe = false;
            try
            {
                OdbcDataReader datos = null;
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                string sql = String.Empty;
                if (dbms == "SQLite")
                {
                    sql = "PRAGMA table_info(" + name_table + ")";
                }
                else
                {
                    sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + name_table + "' ";
                }
               // clsBaseConexion cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(cadenaBaseDatos);
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                List<string> lista_campos = new List<string>();
                string campo_name = String.Empty;
                if (dbms == "SQLite")
                {
                    while (datos.Read())
                    {
                        campo_name = datos[1].ToString();
                        lista_campos.Add(campo_name);
                    }
                }
                else
                {
                    while (datos.Read())
                    {
                        campo_name = datos[3].ToString();
                        lista_campos.Add(campo_name);
                    }
                }


                cs_pxConexion_basedatos.Close();

                if (lista_campos.Count() > 0)
                {
                    foreach (string linea in lista_campos)
                    {
                        if (string.Equals(linea, name_campo))
                        {
                            existe = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                existe = false;
            }

            return existe;
        }*/
        public static List<string> getCamposTabla(string name_table,string dbms,string cadenaConexionBase)
        {
            List<string> lista_campos = new List<string>();
            try
            {
                OdbcDataReader datos = null;
                SQLiteDataReader datoss = null;
                clsBaseConfiguracion configuracion = new clsBaseConfiguracion();
                string sql = String.Empty;
                if (dbms == "SQLite")
                {
                    sql = "PRAGMA table_info(" + name_table + ")";
                }
                else
                {
                    sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + name_table + "' ";
                }
                //clsBaseConexion cn = new clsBaseConexion();
               

                string campo_name = String.Empty;
                if (dbms == "SQLite")
                {
                    SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(cadenaConexionBase);
                    cs_pxConexion_basedatos.Open();
                    datoss = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                    while (datoss.Read())
                    {
                        campo_name = datoss[1].ToString();
                        lista_campos.Add(campo_name);
                    }
                    datoss.Close();
                    cs_pxConexion_basedatos.Close();
                }
                else
                {
                    OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(cadenaConexionBase);
                    cs_pxConexion_basedatos.Open();
                    datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                    while (datos.Read())
                    {
                        campo_name = datos[3].ToString();
                        lista_campos.Add(campo_name);
                    }
                    datos.Close();
                    cs_pxConexion_basedatos.Close();
                }
              
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("BUSCAR campos tabla " + name_table + " " + ex.ToString());
            }
            return lista_campos;
        }
        public static Boolean agregarCampo(string nombre_campo, string nombre_tabla, string DBMS,string cadenaBaseDatos)
        {
            bool agregado = false;

            try
            {
                //primero buscar tipo de campo segun dbms
                string tipo_campo = getTipoCampo(DBMS, nombre_campo, nombre_tabla);
                string sql = string.Empty;
                if (DBMS == "SQLite")
                {
                    sql = "ALTER TABLE " + nombre_tabla + " ADD COLUMN " + nombre_campo + " VARCHAR NULL;";
                    SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(cadenaBaseDatos);
                    cs_pxConexion_basedatos.Open();
                    SQLiteCommand afectado = new SQLiteCommand(sql, cs_pxConexion_basedatos);
                    int valor = afectado.ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                    if (valor == 0)
                    {
                        agregado = true;
                    }
                }
                else
                {
                    sql = "ALTER TABLE " + nombre_tabla + " ADD " + nombre_campo + " " + tipo_campo + " NULL;";
                    OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(cadenaBaseDatos);
                    cs_pxConexion_basedatos.Open();
                    OdbcCommand afectado = new OdbcCommand(sql, cs_pxConexion_basedatos);
                    int valor = afectado.ExecuteNonQuery();
                    cs_pxConexion_basedatos.Close();
                    if (valor == 0)
                    {
                        agregado = true;
                    }
                }
             
                //clsBaseConexion cn = new clsBaseConexion();
              

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.Message + " " + ex.ToString() + " " + nombre_campo + " " + nombre_tabla);
                agregado = false;
            }
            return agregado;
        }
        public static string getTipoCampo(string DBMS, string campo, string tabla)
        {
            string tipo = String.Empty;
            switch (tabla)
            {
                case "cs_Document":
                    if (campo == "cp29" || campo == "cp30" || campo == "cp31")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                tipo = "  VARCHAR(500) ";
                                break;
                        }
                    }
                    else
                    {
                        //tipo = " VARCHAR(500) ";
                        tipo = " TEXT ";
                    }
                    break;
                case "cs_VoidedDocuments":
                    if (campo == "cp11" || campo == "cp12")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                //tipo = "  VARCHAR(500) ";
                                tipo = " TEXT ";
                                break;
                        }
                    }
                    else
                    {
                        //tipo = " VARCHAR(500) ";
                        tipo = "  TEXT ";
                    }
                    break;

                case "cs_SummaryDocuments":
                    if (campo == "cp11" || campo == "cp12")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                //tipo = "  VARCHAR(500) ";
                                tipo = " TEXT ";
                                break;
                        }

                    }
                    else
                    {
                        //tipo = " VARCHAR(500) ";
                        tipo = "  TEXT ";
                    }
                    break;
                case "cs_Perception":
                    if (campo == "cp35" || campo == "cp36")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                //tipo = "  VARCHAR(500) ";
                                tipo = " TEXT ";
                                break;
                        }
                    }
                    else
                    {
                        //tipo = "  VARCHAR(500) ";
                        tipo = " TEXT ";
                    }
                    break;
                case "cs_Retention":
                    if (campo == "cp35" || campo == "cp36")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                //tipo = "  VARCHAR(500) ";
                                tipo = " TEXT ";
                                break;
                        }
                    }
                    else
                    {
                        tipo = "  VARCHAR(500) ";
                    }
                    break;
                case "cs_Despatch":
                    if (campo == "cp32" || campo == "cp33")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  NVARCHAR(MAX) ";
                                break;
                            case "MySQL":
                                //tipo = "  MEDIUMTEXT(MAX) ";
                                tipo = "  TEXT,";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                //tipo = "  VARCHAR(500) ";
                                tipo = " TEXT ";
                                break;
                        }
                    }
                    else
                    {
                        //tipo = "  VARCHAR(500) ";
                        tipo = " TEXT ";
                    }
                    break;
                case "cs_Document_AdditionalComments":
                    if (campo == "cp3")
                    {
                        switch (DBMS)
                        {
                            case "Microsoft SQL Server":
                                tipo = "  TEXT ";
                                break;
                            case "MySQL":
                                tipo = "  TEXT ";
                                break;
                            case "SQLite":
                                tipo = "  TEXT ";
                                break;
                            case "PostgreSQL":
                                tipo = "  TEXT ";
                                break;
                            default:
                                tipo = "  TEXT ";
                                break;
                        }
                    }
                    else
                    {
                        //tipo = "  VARCHAR(500) ";
                        tipo = " TEXT ";
                    }
                    break;
                default:
                    //tipo = "  VARCHAR(500) ";
                    tipo = " TEXT ";
                    break;
            }

            return tipo;
        }
        public string cs_prConexioncadenaservidor()
        {
            string cadena = "";
            switch (Cs_pr_DBMS)
            {
                case "MySQL":
                    cadena = "DRIVER={" + Cs_pr_DBMSDriver + "};SERVER=" + Cs_pr_DBMSServername + ";PORT=" + Cs_pr_DBMSServerport + ";USER=" + Cs_pr_DBUser + ";PASSWORD=" + Cs_pr_DBPassword + ";OPTION=3;";
                    break;
                case "Microsoft SQL Server":
                    cadena = "Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";";
                    break;
                case "SQLite":
                    cadena = "DRIVER={" + Cs_pr_DBMSDriver + "}; Database=" + clsBaseConfiguracion.getRutaInstalacion() + ".dbc\\" + Cs_pr_DBName + "; LongNames=0; Timeout=1000; NoTXN=0; SyncPragma=NORMAL; StepAPI=0;";
                    break;
            }
            return cadena;

        }
        public  string cs_prConexioncadenabasedatos()
        {
            string cadena = "";
            switch (Cs_pr_DBMS)
            {
                case "MySQL":
                    cadena = "DRIVER={" + Cs_pr_DBMSDriver + "};SERVER=" + Cs_pr_DBMSServername + ";PORT=" + Cs_pr_DBMSServerport + ";DATABASE=" + Cs_pr_DBName + ";USER=" + Cs_pr_DBUser + ";PASSWORD=" + Cs_pr_DBPassword + ";OPTION=3;";
                    break;
                case "Microsoft SQL Server":
                    cadena = "Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Database=" + Cs_pr_DBName + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";";
                    break;
                case "SQLite":
                    cadena = "DRIVER={" + Cs_pr_DBMSDriver + "}; Database=" + clsBaseConfiguracion.getRutaInstalacion() + "\\" + Cs_pr_DBName + ".dbc" + "; LongNames=0; Timeout=1000; NoTXN=0; SyncPragma=NORMAL; StepAPI=0;";
                    break;
            }
            return cadena;
        }
        /*Jorge Luis | 15/05/2018 | FEI2-322
         Método para realizar para verificar la existencia de una tabla */
        public void ValidateDatabaseStructure(string tableName) {
            clsEntityDatabaseLocal localConf    = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(declarant.Cs_pr_Declarant_Id);
            string prConexioncadenaservidor     = localConf.cs_prConexioncadenaservidor();
            string prConexioncadenabasedatos    = localConf.cs_prConexioncadenabasedatos();

            OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(prConexioncadenabasedatos);
            OdbcDataReader datos = null;
            string sql = String.Empty;
            if (localConf.Cs_pr_DBMS == "SQLite")
                sql = "select case when tbl_name is not null THEN '1' when tbl_name is null THEN '0' END tbl_name from sqlite_master WHERE type = 'table' and name = '" + tableName + "'";//cs_Document
            else
                sql = "if exists (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = '" + tableName + "') select '1'; else select '0';";
            
            cs_pxConexion_basedatos.ConnectionTimeout = 3;
            cs_pxConexion_basedatos.Open();
            datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
            List<string> lista_tablas = new List<string>();

            string result = String.Empty;
            if (localConf.Cs_pr_DBMS == "SQLite")
            {
                while (datos.Read())
                {
                    result = datos[0].ToString();
                }
            }
            else
            {
                while (datos.Read())
                {
                    result = datos[0].ToString();
                }
            }
            cs_pxConexion_basedatos.Close();
        }
    }
}
