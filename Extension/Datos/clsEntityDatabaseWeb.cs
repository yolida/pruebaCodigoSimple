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

namespace Extension.Datos
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsEntityDatabaseWeb")]
    public class clsEntityDatabaseWeb : clsBaseEntidadSistema
    {
        public string Cs_pr_DatabaseWeb_Id { get; set; }
        public string Cs_pr_DBMS { get; set; }
        public string Cs_pr_DBMSDriver { get; set; }
        public string Cs_pr_DBMSServername { get; set; }
        public string Cs_pr_DBMSServerport { get; set; }
        public string Cs_pr_DBName { get; set; }
        public string Cs_pr_DBUser { get; set; }
        public string Cs_pr_DBPassword { get; set; }
        public string Cs_pr_DBUse { get; set; }
        public string Cs_pr_Declarant_Id { get; set; }

        public clsEntityDatabaseWeb()
        {
            cs_cmTabla = "cs_DatabaseWeb";
            cs_cmCampos.Add("cs_DatabaseWeb_Id");
            for (int i = 1; i <= 9; i++)
            {
                cs_cmCampos.Add("cp" + i.ToString());
            }
        }

        

        public clsEntityDatabaseWeb cs_fxObtenerUnoPorId(string id)
        {
            List<string> valores = new clsBaseConexionSistema().cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_DatabaseWeb_Id = valores[0];
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

        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_DatabaseWeb_Id);
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

        public void cs_pxCrearBaseDatos()
        {
            try
            {
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal();

                string cadena_servidor = "";
                string cadena_base_datos = "";
                switch (Cs_pr_DBMS)
                {
                    case "MySQL":
                        cadena_servidor = "DRIVER={" + Cs_pr_DBMSDriver + "};SERVER=" + Cs_pr_DBMSServername + ";PORT=" + Cs_pr_DBMSServerport + ";USER=" + Cs_pr_DBUser + ";PASSWORD=" + Cs_pr_DBPassword + ";OPTION=3;";
                        cadena_base_datos= "DRIVER={" + Cs_pr_DBMSDriver + "};SERVER=" + Cs_pr_DBMSServername + ";PORT=" + Cs_pr_DBMSServerport + ";Database="+ Cs_pr_DBName + ";USER=" + Cs_pr_DBUser + ";PASSWORD=" + Cs_pr_DBPassword + ";OPTION=3;";
                        break;
                    case "Microsoft SQL Server":
                        cadena_servidor = "Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";";
                        cadena_base_datos = "Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Database=" + Cs_pr_DBName + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";";
                        break;
                    
                }
               

                //OdbcConnection cs_pxConexion_servidor = new OdbcConnection("Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";");
                //OdbcConnection cs_pxConexion_basedatos = new OdbcConnection("Driver={" + Cs_pr_DBMSDriver + "};Server=" + Cs_pr_DBMSServername + "," + Cs_pr_DBMSServerport + ";Database=" + Cs_pr_DBName + ";Uid=" + Cs_pr_DBUser + ";Pwd=" + Cs_pr_DBPassword + ";");
                OdbcConnection cs_pxConexion_servidor = new OdbcConnection(cadena_servidor);
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(cadena_base_datos);

                cs_pxConexion_servidor.Open();
                new OdbcCommand("CREATE DATABASE " + Cs_pr_DBName + ";", cs_pxConexion_servidor).ExecuteNonQuery();
                cs_pxConexion_servidor.Close();

                cs_pxConexion_basedatos.Open();

                switch (Cs_pr_DBMS)
                {
                    case "MySQL":
                       // string asd = "CREATE TABLE " + new clsEntityDocument().cs_cmTabla + " (" + InsercionXML(new clsEntityDocument().cs_cmCampos, Cs_pr_DBMS, new clsEntityDocument().cs_cmTabla) + "); ";
                        new OdbcCommand("CREATE TABLE " + new clsEntityDocument(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntityDocument(local).cs_cmCampos_min, Cs_pr_DBMS, new clsEntityDocument(local).cs_cmTabla_min, new clsEntityDocument(local).cs_cmCampos_min[0]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                        //new OdbcCommand("CREATE TABLE " + new clsEntityDocument().cs_cmTabla + " (" + InsercionXML(new clsEntityDocument().cs_cmCampos, configuracion.cs_prDbms, new clsEntityDocument().cs_cmTabla) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_DespatchDocumentReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_Description(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_Description(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_PricingReference(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_PricingReference(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntitySummaryDocuments(local).cs_cmCampos_min, Cs_pr_DBMS, new clsEntitySummaryDocuments(local).cs_cmTabla_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_Notes(local).cs_cmTabla_min + " (" + InsercionXML(new clsEntitySummaryDocuments_Notes(local).cs_cmCampos_min, Cs_pr_DBMS, new clsEntitySummaryDocuments_Notes(local).cs_cmTabla_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityDocument_TaxTotal(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_TaxTotal(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments(local).cs_cmTabla_min + " (" + Insercion(new clsEntityVoidedDocuments(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmTabla_min + " (" + Insercion(new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_AdditionalComments(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
//new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_AdditionalComments(local).cs_cmTabla_min + " (" + Insercion(new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos_min) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                        break;
                    case "Microsoft SQL Server":
                       // string asd = "CREATE TABLE " + new clsEntityDocument().cs_cmTabla + " (" + InsercionXML(new clsEntityDocument().cs_cmCampos, Cs_pr_DBMS, new clsEntityDocument().cs_cmTabla) + "); ";
                        new OdbcCommand("CREATE TABLE " + new clsEntityDocument(local).cs_cmTabla + " (" + InsercionXML(new clsEntityDocument(local).cs_cmCampos, Cs_pr_DBMS, new clsEntityDocument(local).cs_cmTabla, new clsEntityDocument(local).cs_cmCampos[0]) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_AdditionalDocumentReference(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                        //new OdbcCommand("CREATE TABLE " + new clsEntityDocument().cs_cmTabla + " (" + InsercionXML(new clsEntityDocument().cs_cmCampos, configuracion.cs_prDbms, new clsEntityDocument().cs_cmTabla) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();

                       // new OdbcCommand("CREATE TABLE " + new clsEntityDocument_DespatchDocumentReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_DespatchDocumentReference(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                      //  new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_Description(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_Description(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_PricingReference(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_PricingReference(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_TaxTotal(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments(local).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments(local).cs_cmCampos, Cs_pr_DBMS, new clsEntitySummaryDocuments(local).cs_cmTabla) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_Notes(local).cs_cmTabla + " (" + InsercionXML(new clsEntitySummaryDocuments_Notes(local).cs_cmCampos, Cs_pr_DBMS, new clsEntitySummaryDocuments_Notes(local).cs_cmTabla) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_AllowanceCharge(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_BillingPayment(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntitySummaryDocuments_SummaryDocumentsLine_TaxTotal(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_TaxTotal(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_TaxTotal(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments(local).cs_cmTabla + " (" + Insercion(new clsEntityVoidedDocuments(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmTabla + " (" + Insercion(new clsEntityVoidedDocuments_VoidedDocumentsLine(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                     //   new OdbcCommand("CREATE TABLE " + new clsEntityDocument_AdditionalComments(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_AdditionalComments(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                    //    new OdbcCommand("CREATE TABLE " + new clsEntityDocument_Line_AdditionalComments(local).cs_cmTabla + " (" + Insercion(new clsEntityDocument_Line_AdditionalComments(local).cs_cmCampos) + "); ", cs_pxConexion_basedatos).ExecuteNonQuery();
                        break;

                }

                
                cs_pxConexion_basedatos.Close();
                clsBaseMensaje.cs_pxMsgOk("OKE4");
            }
            catch (Exception ex)
            {
                clsBaseMensaje.cs_pxMsg("Base de datos - Advertencia", ex.Message);
                clsBaseLog.cs_pxRegistarAdd("cs_pxCrearBaseDatos " + ex.ToString());
            }
        }

        public clsEntityDatabaseWeb cs_fxObtenerUnoPorDeclaranteId(string cs_pr_Declarant_Id)
        {
            SQLiteDataReader datos = null;
            clsEntityDatabaseWeb WEB = null;
            try
            {
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE cp9 ='" + cs_pr_Declarant_Id.Trim() + "';";
                SQLiteConnection cs_pxConexion_basedatos = new SQLiteConnection(clsBaseConexionSistema.conexionstring);
                cs_pxConexion_basedatos.Open();
                datos = new SQLiteCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    WEB = new clsEntityDatabaseWeb();
                    WEB.Cs_pr_DatabaseWeb_Id = datos[0].ToString();
                    WEB.Cs_pr_DBMS = datos[1].ToString();
                    WEB.Cs_pr_DBMSDriver = datos[2].ToString();
                    WEB.Cs_pr_DBMSServername = datos[3].ToString();
                    WEB.Cs_pr_DBMSServerport = datos[4].ToString(); ;
                    WEB.Cs_pr_DBName = datos[5].ToString(); ;
                    WEB.Cs_pr_DBUser = datos[6].ToString();
                    WEB.Cs_pr_DBPassword = datos[7].ToString();
                    WEB.Cs_pr_DBUse = datos[8].ToString();
                    WEB.Cs_pr_Declarant_Id = datos[9].ToString();
                }
                cs_pxConexion_basedatos.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" cs_fxObtenerUnoPorDeclaranteId " + ex.ToString());
            }
           
            return WEB;
        }

        private string Insercion(List<string> campos)
        {
            string i_campos = null;
            foreach (var item in campos)
            {
                i_campos += item + "  VARCHAR(500),";
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }

        //Inserción de datos para de cabecera y campos XML
        private string InsercionXML(List<string> campos, string DBMS, string tabla, string primary)
        {
            string tipo = string.Empty;
            string i_campos = null;

            foreach (var item in campos)
            {
                switch (tabla)
                {
                    case "cs_Document":
                        if (item == "cp29" || item == "cp30" || item == "cp31")
                        {
                            switch (DBMS)
                            {
                                case "Microsoft SQL Server":
                                    tipo = "  NVARCHAR(MAX),";
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
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL AUTO_INCREMENT PRIMARY KEY,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }                               
                            }                           
                            else
                            {
                                i_campos += item + "  VARCHAR(500),";
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
                                    case "MySQL":
                                        i_campos += item + "  INT NOT NULL AUTO_INCREMENT PRIMARY KEY,";
                                        break;
                                    default:
                                        i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                                        break;
                                }
                            }
                            else
                            {
                                i_campos += item + "  VARCHAR(500),";
                            }
                        }
                        break;
                    default:
                        if (item == primary)
                        {
                            i_campos += item + "  INT IDENTITY(1,1) PRIMARY KEY,";
                        }
                        else
                        {
                            i_campos += item + "  VARCHAR(500),";
                        }
                        break;
                }
            }
            i_campos = i_campos.Substring(0, i_campos.Length - 1);
            return i_campos;
        }

        public void cs_pxEnviarAWeb(clsEntityDeclarant Empresa)
        {
            try
            {
                clsEntityDatabaseLocal localDB = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(Empresa.Cs_pr_Declarant_Id);
                //Seleccionar la configuración web de la empresa.
                clsEntityDatabaseWeb bd = new clsEntityDatabaseWeb().cs_fxObtenerUnoPorDeclaranteId(Empresa.Cs_pr_Declarant_Id);
                
                List<string> documentos_web;
                //1. Seleccionar todas las cabeceras en la web.               
                string cadena_base_datos = "";
                switch (bd.Cs_pr_DBMS)
                {
                    case "MySQL":
                       
                        cadena_base_datos = "DRIVER={" + bd.Cs_pr_DBMSDriver + "};SERVER=" + bd.Cs_pr_DBMSServername + ";PORT=" + bd.Cs_pr_DBMSServerport + ";Database=" + bd.Cs_pr_DBName + ";USER=" + bd.Cs_pr_DBUser + ";PASSWORD=" + bd.Cs_pr_DBPassword + ";OPTION=3;";
                        break;
                    case "Microsoft SQL Server":
                       
                        cadena_base_datos = "Driver={" + bd.Cs_pr_DBMSDriver + "};Server=" + bd.Cs_pr_DBMSServername + "," + bd.Cs_pr_DBMSServerport + ";Database=" + bd.Cs_pr_DBName + ";Uid=" + bd.Cs_pr_DBUser + ";Pwd=" + bd.Cs_pr_DBPassword + ";";
                        break;

                }

                OdbcConnection cn_web = new OdbcConnection(cadena_base_datos);
                cn_web.Open();
                documentos_web = new List<string>();
                OdbcDataReader datos = null;
                DateTime FechaLimite = DateTime.Today.AddDays(-7);
                string sql = "SELECT cp1 FROM cs_Document WHERE (cp3='01' OR cp3='03' OR cp3='07' OR cp3='08') AND cp2>='" + FechaLimite.ToString("yyyy-MM-dd") + "' AND cp27='0';";

                datos = new OdbcCommand(sql, cn_web).ExecuteReader();
                while (datos.Read())
                {
                    documentos_web.Add(datos[0].ToString().Trim());
                }
                cn_web.Close();

                //2. Seleccionar todas las cabeceras en local.
                List<List<string>> documentos_local = new clsEntityDocument(localDB).cs_pxObtenerActualizacionAWeb();
                if (documentos_local != null && documentos_local.Count > 0)
                {
                    //3. Buscar si local existe en la web. Si no existe, insertar.
                    foreach (var item_local in documentos_local)
                    {
                        bool insertar = true;
                        string Id = item_local[0].ToString();
                        foreach (var item_web in documentos_web)
                        {
                            if (item_local[1].ToString().Equals(item_web.ToString()))
                            {
                                insertar = false;
                            }
                        }
                        if (insertar == true)
                        {
                            cs_pxInsertarRegistrosEnWeb(Id, bd.Cs_pr_DatabaseWeb_Id, localDB);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" cs_pxEnviarAWeb " + ex.Message);               
            }
        }

        private void cs_pxInsertarRegistrosEnWeb(string Id, string Id_InformacionBD,clsEntityDatabaseLocal localBD)
        {
            int errores = 0;
            bool inserto_exito = false;
            try
            {
                clsEntityDatabaseWeb bd = new clsEntityDatabaseWeb().cs_fxObtenerUnoPorId(Id_InformacionBD);
               
                clsEntityDocument anterior_clsEntityDocument = new clsEntityDocument(localBD).cs_fxObtenerUnoPorId(Id);        

                //Insertar: clsEntityDocument
                List<string> valores1 = new List<string>();
                foreach (var prop in anterior_clsEntityDocument.GetType().GetProperties())
                {
                    valores1.Add(prop.GetValue(anterior_clsEntityDocument, null).ToString().Replace("'"," "));
                }
                if(bd.Cs_pr_DBMS== "MySQL")
                {
                    inserto_exito=cs_pxInsertarEnWeb(anterior_clsEntityDocument.cs_cmTabla_min, anterior_clsEntityDocument.cs_cmCampos_min, valores1, false, Id_InformacionBD);
                    if (inserto_exito == false)
                    {
                        errores++;
                    }
                }
                else
                {
                    inserto_exito= cs_pxInsertarEnWeb(anterior_clsEntityDocument.cs_cmTabla, anterior_clsEntityDocument.cs_cmCampos, valores1, false, Id_InformacionBD);
                    if (inserto_exito == false)
                    {
                        errores++;
                    }
                }               
              
                if (errores > 0)
                {
                    //clsBaseMensaje.cs_pxMsg("Error envio de documentos a web","Se ha producido un error al intentar enviar documentos a la web.");
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" cs_pxInsertarRegistrosEnWeb " + ex.ToString());
            }

        }
        

        private bool cs_pxInsertarEnWeb(string tabla, List<string> campos, List<string> valores, bool obtener_mensaje_respuesta, string Id_InformacionBD)
        {
            bool retorno = false;
            try
            {
                clsEntityDatabaseWeb bd = new clsEntityDatabaseWeb().cs_fxObtenerUnoPorId(Id_InformacionBD);
                string cadena_base_datos = "";
                switch (bd.Cs_pr_DBMS)
                {
                    case "MySQL":

                        cadena_base_datos = "DRIVER={" + bd.Cs_pr_DBMSDriver + "};SERVER=" + bd.Cs_pr_DBMSServername + ";PORT=" + bd.Cs_pr_DBMSServerport + ";Database=" + bd.Cs_pr_DBName + ";USER=" + bd.Cs_pr_DBUser + ";PASSWORD=" + bd.Cs_pr_DBPassword + ";OPTION=3;";
                        break;
                    case "Microsoft SQL Server":

                        cadena_base_datos = "Driver={" + bd.Cs_pr_DBMSDriver + "};Server=" + bd.Cs_pr_DBMSServername + "," + bd.Cs_pr_DBMSServerport + ";Database=" + bd.Cs_pr_DBName + ";Uid=" + bd.Cs_pr_DBUser + ";Pwd=" + bd.Cs_pr_DBPassword + ";";
                        break;

                }

               // OdbcConnection cn_web = new OdbcConnection(cadena_base_datos);

                OdbcConnection cs_cmConexion = new OdbcConnection(cadena_base_datos);
                string sql = "", sql_campos = "", sql_valores = "";
                for (int i = 1; i < campos.Count; i++)
                {
                    sql_campos += " " + campos[i] + ",";
                    sql_valores += " '" + valores[i] + "',";
                }
                sql = "INSERT INTO " + tabla + " (" + sql_campos.Substring(1, sql_campos.Length - 2) + ") VALUES (" + sql_valores.Substring(1, sql_valores.Length - 2) + ");";
                
                cs_cmConexion.Open();
                new OdbcCommand(sql, cs_cmConexion).ExecuteReader();
                cs_cmConexion.Close();
                if (obtener_mensaje_respuesta)
                {
                    clsBaseMensaje.cs_pxMsgOk("OKE5");
                }
                retorno = true;
            }
            catch (Exception ex)
            {
                retorno = false;
                //clsBaseLog.cs_pxRegistar(ex.ToString());
                clsBaseLog.cs_pxRegistarAdd(" cs_pxInsertarEnWeb " + tabla + " "+ ex.ToString());
                //clsBaseMensaje.cs_pxMsgEr("ERR5", ex.ToString());
            }
            return retorno;
        }
    }
}
