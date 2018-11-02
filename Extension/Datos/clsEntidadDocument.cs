using Extension.Base;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Datos
{
    public class clsEntidadDocument : clsBaseEntidad
    {
        public string Cs_pr_Document_Id { get; set; }
        public string Cs_tag_ID { get; set; }
        public string Cs_tag_IssueDate { get; set; }
        public string Cs_tag_InvoiceTypeCode { get; set; }//No incluir en NOTAS DE DEBITO O CREDITO
        public string Cs_tag_DocumentCurrencyCode { get; set; }
        public string Cs_tag_Discrepancy_ReferenceID { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_Discrepancy_ResponseCode { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_Discrepancy_Description { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_BillingReference_ID { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_BillingReference_DocumentTypeCode { get; set; }//SOLO NOTAS DE DEBITO O CREDITO
        public string Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_AdditionalAccountID { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyName_Name { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID { get; set; } // Pruebas aquí
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_District { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode { get; set; }
        public string Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID { get; set; }
        public string Cs_tag_AccountingCustomerParty_AdditionalAccountID { get; set; }
        public string Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName { get; set; }
        public string Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description { get; set; } //SOLO BOLETA
        public string Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID { get; set; }
        public string Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID { get; set; }
        public string Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount { get; set; }//Elemento que no estra en el pdf de estructura SUNAT
        public string Cs_pr_EstadoValidar { get; set; }
        public string Cs_pr_Procesado { get; set; }
        public string Cs_pr_XML { get; set; }
        public string Cs_pr_ComentarioSUNAT { get; set; }
        //DATOS ADICIONALES DEL CLIENTE COMPRAS
        public string Cs_cr_Movimiento { get; set; }
        public string Cs_cr_FechaVencimiento { get; set; }
        public string Cs_cr_Pago { get; set; }
        public string Cs_cr_Moneda { get; set; }
        public string Cs_cr_CondicionPago { get; set; }
        public string Cs_cr_CondicionCompra { get; set; }
        public string Cs_cr_TipoCambio { get; set; }
        public string Cs_cr_Almacen { get; set; }
        public string Cs_cr_CentroCostosUno { get; set; }
        public string Cs_cr_CentroCostosDos { get; set; }
        public string Cs_cr_DocReferenciaFecha { get; set; }
        //DATOS ADICIONALES DEL CLIENTE COMPRAS CONTABLE
        public string Cs_cr_AnioEmisionDUA { get; set; }
        public string Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado { get; set; }
        public string Cs_cr_ConstDepDetNumero { get; set; }
        public string Cs_cr_ConstDepDetFecha { get; set; }
        public string Cs_cr_EquivalenteDolares { get; set; }
        public string Cs_cr_CtaContableBaseImponible { get; set; }
        public string Cs_cr_CtaContableOtrosTributos { get; set; }
        public string Cs_cr_CtaContableTotal { get; set; }
        public string Cs_cr_RegimenEspecial { get; set; }
        public string Cs_cr_PorcentajeRegimenEspecial { get; set; }
        public string Cs_cr_ImporteRegimenEspecial { get; set; }
        public string Cs_cr_SerieDocumentoRegimenEspecial { get; set; }
        public string Cs_cr_NumeroDocumentoRegimenEspecial { get; set; }
        public string Cs_cr_FechaDocumentoRegimenEspecial { get; set; }
        public string Cs_cr_CodigoPresupuesto { get; set; }
        public string Cs_cr_PorcentajeIGV { get; set; }
        public string Cs_cr_Glosa { get; set; }
        public string Cs_cr_CondicionPercepcion { get; set; }
        public string Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas { get; set; }
        public string Cs_cr_AdqGravadasIGVGravadasNoGravados { get; set; }
        public string Cs_cr_AdqGravadasBaseImponibleNoGravados { get; set; }
        public string Cs_cr_AdqGravadasIGVNoGravados { get; set; }
        public string Cs_cr_FechaVencimientoDos { get; set; }
        public string Cs_cr_Periodo { get; set; }

        /* UBL 2.1
        A partir de esta línea se agregará las nuevas etiquetas para la versión 2.1 de UBL */
        public string Cs_tag_ProfileID { get; set; } // Etiqueta agregada para la versión 2.1 de UBL
        public string Cs_tag_IssueTime { get; set; } // Etiqueta agregada para la versión 2.1 de UBL

        /// <summary>
        /// Metodo para obtener un registro por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public clsEntidadDocument cs_fxObtenerUnoPorId(string id)
        {
            // clsEntityDatabaseLocal conf = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId();
            List<string> valores = new clsBaseConexion(localDB).cs_fxObtenerUnoPorId(cs_cmTabla, cs_cmCampos, id, false);
            if (valores.Count > 0)
            {
                Cs_pr_Document_Id = valores[0];
                Cs_tag_ID = valores[1];
                Cs_tag_IssueDate = valores[2];
                Cs_tag_InvoiceTypeCode = valores[3];//No incluir en NOTAS DE DEBITO O CREDITO
                Cs_tag_DocumentCurrencyCode = valores[4];
                Cs_tag_Discrepancy_ReferenceID = valores[5];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_Discrepancy_ResponseCode = valores[6];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_Discrepancy_Description = valores[7];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_BillingReference_ID = valores[8];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_BillingReference_DocumentTypeCode = valores[9];//SOLO NOTAS DE DEBITO O CREDITO
                Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = valores[10];
                Cs_tag_AccountingSupplierParty_AdditionalAccountID = valores[11];
                Cs_tag_AccountingSupplierParty_Party_PartyName_Name = valores[12];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = valores[13];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = valores[14];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = valores[15];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = valores[16];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = valores[17];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = valores[18];
                Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = valores[19];
                Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = valores[20];
                Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = valores[21];
                Cs_tag_AccountingCustomerParty_AdditionalAccountID = valores[22];
                Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = valores[23];
                Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = valores[24]; //SOLO BOLETA
                Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = valores[25];
                Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = valores[26];
                Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = valores[27];
                Cs_pr_EstadoValidar = valores[28];
                Cs_pr_Procesado = valores[29];
                Cs_pr_XML = valores[30];
                Cs_pr_ComentarioSUNAT = valores[31];
                Cs_cr_Movimiento = valores[32];
                Cs_cr_FechaVencimiento = valores[33];
                Cs_cr_Pago = valores[34];
                Cs_cr_Moneda = valores[35];
                Cs_cr_CondicionPago = valores[36];
                Cs_cr_CondicionCompra = valores[37];
                Cs_cr_TipoCambio = valores[38];
                Cs_cr_Almacen = valores[39];
                Cs_cr_CentroCostosUno = valores[40];
                Cs_cr_CentroCostosDos = valores[41];
                Cs_cr_DocReferenciaFecha = valores[42];
                Cs_cr_AnioEmisionDUA = valores[43];
                Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = valores[44];
                Cs_cr_ConstDepDetNumero = valores[45];
                Cs_cr_ConstDepDetFecha = valores[46];
                Cs_cr_EquivalenteDolares = valores[47];
                Cs_cr_CtaContableBaseImponible = valores[48];
                Cs_cr_CtaContableOtrosTributos = valores[49];
                Cs_cr_CtaContableTotal = valores[50];
                Cs_cr_RegimenEspecial = valores[51];
                Cs_cr_PorcentajeRegimenEspecial = valores[52];
                Cs_cr_ImporteRegimenEspecial = valores[53];
                Cs_cr_SerieDocumentoRegimenEspecial = valores[54];
                Cs_cr_NumeroDocumentoRegimenEspecial = valores[55];
                Cs_cr_FechaDocumentoRegimenEspecial = valores[56];
                Cs_cr_CodigoPresupuesto = valores[57];
                Cs_cr_PorcentajeIGV = valores[58];
                Cs_cr_Glosa = valores[59];
                Cs_cr_CondicionPercepcion = valores[60];
                Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = valores[61];
                Cs_cr_AdqGravadasIGVGravadasNoGravados = valores[62];
                Cs_cr_AdqGravadasBaseImponibleNoGravados = valores[63];
                Cs_cr_AdqGravadasIGVNoGravados = valores[64];
                Cs_cr_FechaVencimientoDos = valores[65];
                Cs_cr_Periodo = valores[66];

                // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                Cs_tag_ProfileID = valores[67];
                Cs_tag_IssueTime = valores[68];
                return this;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="local"></param>
        public clsEntidadDocument(clsEntityDatabaseLocal local)
        {
            this.localDB = local;
            cs_cmTabla = "cs_cDocument";
            cs_cmCampos.Add("cs_Document_Id");     //0     
            cs_cmCampos.Add("Cs_tag_ID");//1
            cs_cmCampos.Add("Cs_tag_IssueDate");//2
            cs_cmCampos.Add("Cs_tag_InvoiceTypeCode");//3
            cs_cmCampos.Add("Cs_tag_DocumentCurrencyCode");//4
            cs_cmCampos.Add("Cs_tag_Discrepancy_ReferenceID");//5
            cs_cmCampos.Add("Cs_tag_Discrepancy_ResponseCode");//6
            cs_cmCampos.Add("Cs_tag_Discrepancy_Description");//7
            cs_cmCampos.Add("Cs_tag_BillingReference_ID");//8
            cs_cmCampos.Add("Cs_tag_BillingReference_DocumentTypeCode");//9
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_CustomerAssignedAccountID");//10
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_AdditionalAccountID");//11
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PartyName_Name");//12
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_ID");//13
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_StreetName");//14
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CitySubdivisionName");//15
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CityName");//16
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CountrySubentity");//17
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_District");//18
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_Country_IdentificationCode");//19
            cs_cmCampos.Add("Cs_tag_AccSupplierParty_Party_PartyLegalEntity_RegistrationName");//20
            cs_cmCampos.Add("Cs_tag_AccCustomerParty_CustomerAssignedAccountID");//21
            cs_cmCampos.Add("Cs_tag_AccCustomerParty_AdditionalAccountID");//22
            cs_cmCampos.Add("Cs_tag_AccCustomerParty_Party_PartyLegalEntity_RegistrationName");//23
            cs_cmCampos.Add("Cs_tag_AccCustomerParty_Party_PhysicalLocation_Description");//24
            cs_cmCampos.Add("Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID");//25
            cs_cmCampos.Add("Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID");//26
            cs_cmCampos.Add("Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount");//27
            cs_cmCampos.Add("Cs_pr_EstadoValidar");//28
            cs_cmCampos.Add("Cs_pr_Procesado");//29
            cs_cmCampos.Add("Cs_pr_XML");//30
            cs_cmCampos.Add("Cs_pr_ComentarioSUNAT");//31
            cs_cmCampos.Add("Cs_cr_Movimiento");//32
            cs_cmCampos.Add("Cs_cr_FechaVencimiento");//33
            cs_cmCampos.Add("Cs_cr_Pago");//34
            cs_cmCampos.Add("Cs_cr_Moneda");//35
            cs_cmCampos.Add("Cs_cr_CondicionPago");//36
            cs_cmCampos.Add("Cs_cr_CondicionCompra");//37
            cs_cmCampos.Add("Cs_cr_TipoCambio");//38
            cs_cmCampos.Add("Cs_cr_Almacen");//39
            cs_cmCampos.Add("Cs_cr_CentroCostosUno");//40
            cs_cmCampos.Add("Cs_cr_CentroCostosDos");//41
            cs_cmCampos.Add("Cs_cr_DocReferenciaFecha");//42
            cs_cmCampos.Add("Cs_cr_AnioEmisionDUA");//43
            cs_cmCampos.Add("Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado");//44
            cs_cmCampos.Add("Cs_cr_ConstDepDetNumero");//45
            cs_cmCampos.Add("Cs_cr_ConstDepDetFecha");//46
            cs_cmCampos.Add("Cs_cr_EquivalenteDolares");//47
            cs_cmCampos.Add("Cs_cr_CtaContableBaseImponible");//48
            cs_cmCampos.Add("Cs_cr_CtaContableOtrosTributos");//49
            cs_cmCampos.Add("Cs_cr_CtaContableTotal");//50
            cs_cmCampos.Add("Cs_cr_RegimenEspecial");//51
            cs_cmCampos.Add("Cs_cr_PorcentajeRegimenEspecial");//52
            cs_cmCampos.Add("Cs_cr_ImporteRegimenEspecial");//53
            cs_cmCampos.Add("Cs_cr_SerieDocumentoRegimenEspecial");//54
            cs_cmCampos.Add("Cs_cr_NumeroDocumentoRegimenEspecial");//55
            cs_cmCampos.Add("Cs_cr_FechaDocumentoRegimenEspecial");//56
            cs_cmCampos.Add("Cs_cr_CodigoPresupuesto");//57
            cs_cmCampos.Add("Cs_cr_PorcentajeIGV");//58
            cs_cmCampos.Add("Cs_cr_Glosa");//59
            cs_cmCampos.Add("Cs_cr_CondicionPercepcion");//60
            cs_cmCampos.Add("Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas");//61
            cs_cmCampos.Add("Cs_cr_AdqGravadasIGVGravadasNoGravados");//62
            cs_cmCampos.Add("Cs_cr_AdqGravadasBaseImponibleNoGravados");//63
            cs_cmCampos.Add("Cs_cr_AdqGravadasIGVNoGravados");//64
            cs_cmCampos.Add("Cs_cr_FechaVencimientoDos");//65
            cs_cmCampos.Add("Cs_cr_Periodo");//66

            // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
            cs_cmCampos.Add("Cs_tag_ProfileID");
            cs_cmCampos.Add("Cs_tag_IssueTime");

            cs_cmTabla_min = "cs_cDocument";
            cs_cmCampos_min.Add("cs_Document_Id");     //0     
            cs_cmCampos_min.Add("Cs_tag_ID");//1
            cs_cmCampos_min.Add("Cs_tag_IssueDate");//2
            cs_cmCampos_min.Add("Cs_tag_InvoiceTypeCode");//3
            cs_cmCampos_min.Add("Cs_tag_DocumentCurrencyCode");//4
            cs_cmCampos_min.Add("Cs_tag_Discrepancy_ReferenceID");//5
            cs_cmCampos_min.Add("Cs_tag_Discrepancy_ResponseCode");//6
            cs_cmCampos_min.Add("Cs_tag_Discrepancy_Description");//7
            cs_cmCampos_min.Add("Cs_tag_BillingReference_ID");//8
            cs_cmCampos_min.Add("Cs_tag_BillingReference_DocumentTypeCode");//9
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_CustomerAssignedAccountID");//10
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_AdditionalAccountID");//11
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PartyName_Name");//12
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_ID");//13
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_StreetName");//14
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CitySubdivisionName");//15
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CityName");//16
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_CountrySubentity");//17
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_District");//18
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PostalAddress_Country_IdCode");//19
            cs_cmCampos_min.Add("Cs_tag_AccSupplierParty_Party_PartyLegalEntity_RegistrationName");//20
            cs_cmCampos_min.Add("Cs_tag_AccCustomerParty_CustomerAssignedAccountID");//21
            cs_cmCampos_min.Add("Cs_tag_AccCustomerParty_AdditionalAccountID");//22
            cs_cmCampos_min.Add("Cs_tag_AccCustomerParty_Party_PartyLegalEntity_RegistrationName");//23
            cs_cmCampos_min.Add("Cs_tag_AccCustomerParty_Party_PhysicalLocation_Description");//24
            cs_cmCampos_min.Add("Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID");//25
            cs_cmCampos_min.Add("Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID");//26
            cs_cmCampos_min.Add("Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount");//27
            cs_cmCampos_min.Add("Cs_pr_EstadoValidar");//28
            cs_cmCampos_min.Add("Cs_pr_Procesado");//29
            cs_cmCampos_min.Add("Cs_pr_XML");//30
            cs_cmCampos_min.Add("Cs_pr_ComentarioSUNAT");//31
            cs_cmCampos_min.Add("Cs_cr_Movimiento");//32
            cs_cmCampos_min.Add("Cs_cr_FechaVencimiento");//33
            cs_cmCampos_min.Add("Cs_cr_Pago");//34
            cs_cmCampos_min.Add("Cs_cr_Moneda");//35
            cs_cmCampos_min.Add("Cs_cr_CondicionPago");//36
            cs_cmCampos_min.Add("Cs_cr_CondicionCompra");//37
            cs_cmCampos_min.Add("Cs_cr_TipoCambio");//38
            cs_cmCampos_min.Add("Cs_cr_Almacen");//39
            cs_cmCampos_min.Add("Cs_cr_CentroCostosUno");//40
            cs_cmCampos_min.Add("Cs_cr_CentroCostosDos");//41
            cs_cmCampos_min.Add("Cs_cr_DocReferenciaFecha");//42
            cs_cmCampos_min.Add("Cs_cr_AnioEmisionDUA");//43
            cs_cmCampos_min.Add("Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado");//44
            cs_cmCampos_min.Add("Cs_cr_ConstDepDetNumero");//45
            cs_cmCampos_min.Add("Cs_cr_ConstDepDetFecha");//46
            cs_cmCampos_min.Add("Cs_cr_EquivalenteDolares");//47
            cs_cmCampos_min.Add("Cs_cr_CtaContableBaseImponible");//48
            cs_cmCampos_min.Add("Cs_cr_CtaContableOtrosTributos");//49
            cs_cmCampos_min.Add("Cs_cr_CtaContableTotal");//50
            cs_cmCampos_min.Add("Cs_cr_RegimenEspecial");//51
            cs_cmCampos_min.Add("Cs_cr_PorcentajeRegimenEspecial");//52
            cs_cmCampos_min.Add("Cs_cr_ImporteRegimenEspecial");//53
            cs_cmCampos_min.Add("Cs_cr_SerieDocumentoRegimenEspecial");//54
            cs_cmCampos_min.Add("Cs_cr_NumeroDocumentoRegimenEspecial");//55
            cs_cmCampos_min.Add("Cs_cr_FechaDocumentoRegimenEspecial");//56
            cs_cmCampos_min.Add("Cs_cr_CodigoPresupuesto");//57
            cs_cmCampos_min.Add("Cs_cr_PorcentajeIGV");//58
            cs_cmCampos_min.Add("Cs_cr_Glosa");//59
            cs_cmCampos_min.Add("Cs_cr_CondicionPercepcion");//60
            cs_cmCampos_min.Add("Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas");//61
            cs_cmCampos_min.Add("Cs_cr_AdqGravadasIGVGravadasNoGravados");//62
            cs_cmCampos_min.Add("Cs_cr_AdqGravadasBaseImponibleNoGravados");//63
            cs_cmCampos_min.Add("Cs_cr_AdqGravadasIGVNoGravados");//64
            cs_cmCampos_min.Add("Cs_cr_FechaVencimientoDos");//65
            cs_cmCampos_min.Add("Cs_cr_Periodo");//66

            // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
            cs_cmCampos_min.Add("Cs_tag_ProfileID");
            cs_cmCampos_min.Add("Cs_tag_IssueTime");

        }
        /// <summary>
        /// Metodo para actualizar los valores en la entidad
        /// </summary>
        protected override void cs_pxActualizarEntidad()
        {
            cs_cmValores = new List<string>();
            cs_cmValores.Add(Cs_pr_Document_Id);
            cs_cmValores.Add(Cs_tag_ID);
            cs_cmValores.Add(Cs_tag_IssueDate);
            cs_cmValores.Add(Cs_tag_InvoiceTypeCode);//No incluir en NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_DocumentCurrencyCode);
            cs_cmValores.Add(Cs_tag_Discrepancy_ReferenceID);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_Discrepancy_ResponseCode);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_Discrepancy_Description);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_BillingReference_ID);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_BillingReference_DocumentTypeCode);//SOLO NOTAS DE DEBITO O CREDITO
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_AdditionalAccountID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyName_Name);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_District);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode);
            cs_cmValores.Add(Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_AdditionalAccountID);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName);
            cs_cmValores.Add(Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description); //SOLO BOLETA
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID);
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID);
            cs_cmValores.Add(Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount);
            cs_cmValores.Add(Cs_pr_EstadoValidar);
            cs_cmValores.Add(Cs_pr_Procesado);
            cs_cmValores.Add(Cs_pr_XML);
            cs_cmValores.Add(Cs_pr_ComentarioSUNAT);
            cs_cmValores.Add(Cs_cr_Movimiento);//34
            cs_cmValores.Add(Cs_cr_FechaVencimiento);//37
            cs_cmValores.Add(Cs_cr_Pago);//38
            cs_cmValores.Add(Cs_cr_Moneda);//39
            cs_cmValores.Add(Cs_cr_CondicionPago);//40
            cs_cmValores.Add(Cs_cr_CondicionCompra);
            cs_cmValores.Add(Cs_cr_TipoCambio);//41
            cs_cmValores.Add(Cs_cr_Almacen);//42
            cs_cmValores.Add(Cs_cr_CentroCostosUno);//43
            cs_cmValores.Add(Cs_cr_CentroCostosDos);//44
            cs_cmValores.Add(Cs_cr_DocReferenciaFecha);//45
            cs_cmValores.Add(Cs_cr_AnioEmisionDUA);//44
            cs_cmValores.Add(Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado);//45
            cs_cmValores.Add(Cs_cr_ConstDepDetNumero);//46
            cs_cmValores.Add(Cs_cr_ConstDepDetFecha);//47
            cs_cmValores.Add(Cs_cr_EquivalenteDolares);//48
            cs_cmValores.Add(Cs_cr_CtaContableBaseImponible);//49
            cs_cmValores.Add(Cs_cr_CtaContableOtrosTributos);//50
            cs_cmValores.Add(Cs_cr_CtaContableTotal);//51
            cs_cmValores.Add(Cs_cr_RegimenEspecial);//52
            cs_cmValores.Add(Cs_cr_PorcentajeRegimenEspecial);//53
            cs_cmValores.Add(Cs_cr_ImporteRegimenEspecial);//54
            cs_cmValores.Add(Cs_cr_SerieDocumentoRegimenEspecial);//55
            cs_cmValores.Add(Cs_cr_NumeroDocumentoRegimenEspecial);//56
            cs_cmValores.Add(Cs_cr_FechaDocumentoRegimenEspecial);//57
            cs_cmValores.Add(Cs_cr_CodigoPresupuesto);//58
            cs_cmValores.Add(Cs_cr_PorcentajeIGV);//59
            cs_cmValores.Add(Cs_cr_Glosa);//60
            cs_cmValores.Add(Cs_cr_CondicionPercepcion);//61
            cs_cmValores.Add(Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas);//61
            cs_cmValores.Add(Cs_cr_AdqGravadasIGVGravadasNoGravados);//61
            cs_cmValores.Add(Cs_cr_AdqGravadasBaseImponibleNoGravados);//61
            cs_cmValores.Add(Cs_cr_AdqGravadasIGVNoGravados);//61
            cs_cmValores.Add(Cs_cr_FechaVencimientoDos);//61
            cs_cmValores.Add(Cs_cr_Periodo);

            // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
            cs_cmValores.Add(Cs_tag_ProfileID);
            cs_cmValores.Add(Cs_tag_IssueTime);

        }
        /// <summary>
        /// Metodo para eliminar un documento segun id cabecera
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool cs_pxEliminarDocumento(string Id)
        {
            bool resultado = false;
            try
            {
                //Eliminar la cabecera y todos sus registros.
                clsEntidadDocument Document = new clsEntidadDocument(localDB).cs_fxObtenerUnoPorId(Id);
                clsEntidadDocument_AdditionalComments Document_AdditionalComments = new clsEntidadDocument_AdditionalComments(localDB);
                clsEntidadDocument_AdditionalDocumentReference AdditionalDocumentReference = new clsEntidadDocument_AdditionalDocumentReference(localDB);
                clsEntidadDocument_DespatchDocumentReference DespatchDocumentReference = new clsEntidadDocument_DespatchDocumentReference(localDB);
                clsEntidadDocument_Line Document_Line = new clsEntidadDocument_Line(localDB);
                clsEntidadDocument_Line_Description Line_Description = new clsEntidadDocument_Line_Description(localDB);
                clsEntidadDocument_Line_PricingReference Line_PricingReference = new clsEntidadDocument_Line_PricingReference(localDB);
                clsEntidadDocument_Line_TaxTotal Document_Line_TaxTotal = new clsEntidadDocument_Line_TaxTotal(localDB);
                clsEntidadDocument_TaxTotal Document_TaxTotal = new clsEntidadDocument_TaxTotal(localDB);
                clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB);
                clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB);

                foreach (var item in Document_AdditionalComments.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in AdditionalDocumentReference.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in DespatchDocumentReference.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }

                foreach (var item in Document_Line.cs_fxObtenerTodoPorCabeceraId(Id))
                {

                    foreach (var item2 in Line_Description.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item2.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Line_PricingReference.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    foreach (var item3 in Document_Line_TaxTotal.cs_fxObtenerTodoPorCabeceraId(item.Cs_pr_Document_Line_Id))
                    {
                        item3.cs_pxElimnar(false);
                    }

                    item.cs_pxElimnar(false);
                }

                foreach (var item in Document_TaxTotal.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item.cs_pxElimnar(false);
                }


                foreach (var item1 in Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item1.cs_pxElimnar(false);
                }

                foreach (var item2 in Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty.cs_fxObtenerTodoPorCabeceraId(Id))
                {
                    item2.cs_pxElimnar(false);
                }

                Document.cs_pxElimnar(false);
                resultado = true;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(" clsEntityDocument cs_pxEliminarDocumento " + ex.ToString());
            }
            return resultado;

        }
        /// <summary>
        /// Metodo para obtener fecha de documento segun la serie-numero
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public string cs_pxObtenerValorExportacionById(string id)
        {
            string result = "";
            try
            {
                List<clsEntidadDocument_Line> listaLineas = new clsEntidadDocument_Line(localDB).cs_fxObtenerTodoPorCabeceraId(id);
                foreach (clsEntidadDocument_Line it in listaLineas)
                {
                    //buscar tax por item
                    List<clsEntidadDocument_Line_TaxTotal> taxes = new clsEntidadDocument_Line_TaxTotal(localDB).cs_fxObtenerTodoPorCabeceraId(it.Cs_pr_Document_Line_Id);
                    foreach (clsEntidadDocument_Line_TaxTotal tax in taxes)
                    {
                        if (tax.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode == "40")
                        {
                            result = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerValorPorTagIdAndDocumentoId("1002", it.Cs_pr_Document_Id, "Cs_tag_PayableAmount");
                            break;
                        }
                    }
                    break;
                }


            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());

            }
            return result;
        }

        /// <summary>
        /// Metodo para obtener fecha de documento segun la serie-numero
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public string cs_pxBuscarFechaDocumento(string serienumero, string rucEmisor)
        {
            string result = "";
            string date = "";
            try
            {
                string text = "SELECT Cs_tag_IssueDate FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_tag_ID  = '" + serienumero + "' AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID='" + rucEmisor + "'  ";
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                clsEntidadDocument clsEntityDocument = new clsEntidadDocument(localDB);
                while (odbcDataReader.Read())
                {
                    date = odbcDataReader[0].ToString();

                }
                odbcConnection.Close();
                result = date;
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Metodo para obtener fecha de documento segun la serie-numero
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public List<string> cs_pxObtenerDocumentoByTipoSerieNumero(string tipo, string serienumero, string fechaInicio, string fechaFin, string ruc, string tipoRegistro)
        {
            List<string> result = new List<string>();
            string date = "";
            try
            {
                string text = "SELECT cs_Document_Id,Cs_tag_InvoiceTypeCode,Cs_tag_IssueDate,Cs_tag_ID,Cs_pr_EstadoValidar,Cs_tag_AccCustomerParty_CustomerAssignedAccountID,Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID,Cs_tag_DocumentCurrencyCode,Cs_tag_BillingReference_ID,Cs_tag_BillingReference_DocumentTypeCode FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_tag_ID  = '" + serienumero + "' AND  Cs_tag_InvoiceTypeCode='" + tipo + "' AND Cs_pr_CargoValidar = '1' ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    text += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "' ";
                }
                if (tipoRegistro == "1")
                {
                    //compras
                    text += " AND Cs_tag_AccCustomerParty_CustomerAssignedAccountID='" + ruc + "' ";
                }
                else if (tipoRegistro == "2")
                {
                    //ventas
                    text += " AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID='" + ruc + "' ";
                }
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                clsEntidadDocument clsEntityDocument = new clsEntidadDocument(localDB);

                while (odbcDataReader.Read())
                {
                    result = new List<string>();
                    for (int i = 0; i < odbcDataReader.FieldCount; i++)
                    {
                        result.Add(odbcDataReader[i].ToString());
                    }
                    date = odbcDataReader[0].ToString();

                }
                odbcConnection.Close();

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                result = null;
            }
            return result;
        }


        /// <summary>
        /// Metodo para obtener todo el registro del documento segun serie numero
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public clsEntidadDocument cs_pxObtenerDocumento(string serienumero, string rucEmisor)
        {
            clsEntidadDocument item = null;
            try
            {
                string text = "SELECT * FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_tag_ID  = '" + serienumero + "' AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID='" + rucEmisor + "' ";

                OdbcDataReader datos = null;
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();

                datos = new OdbcCommand(text, odbcConnection).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34

                    item.Cs_cr_AnioEmisionDUA = datos[43].ToString();
                    item.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = datos[44].ToString();
                    item.Cs_cr_ConstDepDetNumero = datos[45].ToString();
                    item.Cs_cr_ConstDepDetFecha = datos[46].ToString();
                    item.Cs_cr_EquivalenteDolares = datos[47].ToString();
                    item.Cs_cr_CtaContableBaseImponible = datos[48].ToString();
                    item.Cs_cr_CtaContableOtrosTributos = datos[49].ToString();
                    item.Cs_cr_CtaContableTotal = datos[50].ToString();
                    item.Cs_cr_RegimenEspecial = datos[51].ToString();
                    item.Cs_cr_PorcentajeRegimenEspecial = datos[52].ToString();
                    item.Cs_cr_ImporteRegimenEspecial = datos[53].ToString();
                    item.Cs_cr_SerieDocumentoRegimenEspecial = datos[54].ToString();
                    item.Cs_cr_NumeroDocumentoRegimenEspecial = datos[55].ToString();
                    item.Cs_cr_FechaDocumentoRegimenEspecial = datos[56].ToString();
                    item.Cs_cr_CodigoPresupuesto = datos[57].ToString();
                    item.Cs_cr_PorcentajeIGV = datos[58].ToString();
                    item.Cs_cr_Glosa = datos[59].ToString();
                    item.Cs_cr_CondicionPercepcion = datos[60].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = datos[61].ToString();
                    item.Cs_cr_AdqGravadasIGVGravadasNoGravados = datos[62].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleNoGravados = datos[63].ToString();
                    item.Cs_cr_AdqGravadasIGVNoGravados = datos[64].ToString();
                    item.Cs_cr_FechaVencimientoDos = datos[65].ToString();
                    item.Cs_cr_Periodo = datos[66].ToString();

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[67].ToString();
                    item.Cs_tag_IssueTime = datos[68].ToString();
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }
            return item;
        }
        /// <summary>
        /// Metodo para obtener todo el registro del documento segun serie numero
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public clsEntidadDocument cs_pxObtenerDocumentoById(string id)
        {
            clsEntidadDocument item = null;
            try
            {
                string text = "SELECT * FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_Document_Id  = " + id + " ";

                OdbcDataReader datos = null;
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();

                datos = new OdbcCommand(text, odbcConnection).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();            
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34

                    item.Cs_cr_AnioEmisionDUA = datos[43].ToString();
                    item.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = datos[44].ToString();
                    item.Cs_cr_ConstDepDetNumero = datos[45].ToString();
                    item.Cs_cr_ConstDepDetFecha = datos[46].ToString();
                    item.Cs_cr_EquivalenteDolares = datos[47].ToString();
                    item.Cs_cr_CtaContableBaseImponible = datos[48].ToString();
                    item.Cs_cr_CtaContableOtrosTributos = datos[49].ToString();
                    item.Cs_cr_CtaContableTotal = datos[50].ToString();
                    item.Cs_cr_RegimenEspecial = datos[51].ToString();
                    item.Cs_cr_PorcentajeRegimenEspecial = datos[52].ToString();
                    item.Cs_cr_ImporteRegimenEspecial = datos[53].ToString();
                    item.Cs_cr_SerieDocumentoRegimenEspecial = datos[54].ToString();
                    item.Cs_cr_NumeroDocumentoRegimenEspecial = datos[55].ToString();
                    item.Cs_cr_FechaDocumentoRegimenEspecial = datos[56].ToString();
                    item.Cs_cr_CodigoPresupuesto = datos[57].ToString();
                    item.Cs_cr_PorcentajeIGV = datos[58].ToString();
                    item.Cs_cr_Glosa = datos[59].ToString();
                    item.Cs_cr_CondicionPercepcion = datos[60].ToString();

                    item.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = datos[61].ToString();
                    item.Cs_cr_AdqGravadasIGVGravadasNoGravados = datos[62].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleNoGravados = datos[63].ToString();
                    item.Cs_cr_AdqGravadasIGVNoGravados = datos[64].ToString();
                    item.Cs_cr_FechaVencimientoDos = datos[65].ToString();
                    item.Cs_cr_Periodo = datos[66].ToString();

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[67].ToString();
                    item.Cs_tag_IssueTime = datos[68].ToString();
                }
                odbcConnection.Close();
            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }
            return item;
        }
        /// <summary>
        /// Metodo para validar si existe un documento
        /// </summary>
        /// <param name="serienumero"></param>
        /// <returns></returns>
        public bool cs_pxExisteDocumento(string serienumero, string rucEmisor)
        {
            bool existe = false;
            string date = "";
            try
            {
                string text = "SELECT Cs_tag_ID FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_tag_ID  = '" + serienumero + "' AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID ='" + rucEmisor + "' ";
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                clsEntidadDocument clsEntityDocument = new clsEntidadDocument(localDB);
                while (odbcDataReader.Read())
                {
                    date = odbcDataReader[0].ToString();
                    existe = true;
                }
                odbcConnection.Close();

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
                existe = false;
            }
            return existe;
        }
        public string cs_pxGetIdDocumento(string serienumero)
        {
            string date = "";
            try
            {
                string text = "SELECT Cs_pr_Document_Id FROM " + this.cs_cmTabla + " WHERE 1=1 ";
                text = text + " AND Cs_tag_ID  = '" + serienumero + "' ";
                //clsBaseConexion clsBaseConexion = new clsBaseConexion();
                OdbcConnection odbcConnection = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                odbcConnection.Open();
                OdbcDataReader odbcDataReader = new OdbcCommand(text, odbcConnection).ExecuteReader();
                clsEntidadDocument clsEntityDocument = new clsEntidadDocument(localDB);
                while (odbcDataReader.Read())
                {
                    date = odbcDataReader[0].ToString();
                }
                odbcConnection.Close();

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd(ex.ToString());
            }
            return date;
        }
        /// <summary>
        /// Metodo para listar los comprobantes validados y aceptados segun rango de fecha
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerAceptadosValidado(string fechaInicio, string fechaFin, string Ruc,string tipo, string periodo)
        {
            List<clsEntidadDocument> lista_documentos;
            clsEntidadDocument item;
            try
            {
                //buscar facturas y notas asociadas
                lista_documentos = new List<clsEntidadDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1 ";
                sql += " AND (Cs_tag_InvoiceTypeCode='01' OR Cs_tag_BillingReference_DocumentTypeCode='01') ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    sql += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "' ";
                }
                if (tipo != "")
                {
                    if (tipo == "1")
                    {
                        //compras
                        sql += " AND Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + Ruc + "' ";
                    }
                    else
                    {
                        //ventas
                        sql += " AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + Ruc + "' ";
                    }

                }
                else
                {
                    sql += " AND (Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + Ruc + "' OR Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + Ruc + "') ";
                }

                sql += " AND Cs_cr_Periodo = '" + periodo + "' ";
                sql += " AND Cs_pr_Procesado ='2'  "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34

                    item.Cs_cr_AnioEmisionDUA = datos[43].ToString();
                    item.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = datos[44].ToString();
                    item.Cs_cr_ConstDepDetNumero = datos[45].ToString();
                    item.Cs_cr_ConstDepDetFecha = datos[46].ToString();
                    item.Cs_cr_EquivalenteDolares = datos[47].ToString();
                    item.Cs_cr_CtaContableBaseImponible = datos[48].ToString();
                    item.Cs_cr_CtaContableOtrosTributos = datos[49].ToString();
                    item.Cs_cr_CtaContableTotal = datos[50].ToString();
                    item.Cs_cr_RegimenEspecial = datos[51].ToString();
                    item.Cs_cr_PorcentajeRegimenEspecial = datos[52].ToString();
                    item.Cs_cr_ImporteRegimenEspecial = datos[53].ToString();
                    item.Cs_cr_SerieDocumentoRegimenEspecial = datos[54].ToString();
                    item.Cs_cr_NumeroDocumentoRegimenEspecial = datos[55].ToString();
                    item.Cs_cr_FechaDocumentoRegimenEspecial = datos[56].ToString();
                    item.Cs_cr_CodigoPresupuesto = datos[57].ToString();
                    item.Cs_cr_PorcentajeIGV = datos[58].ToString();
                    item.Cs_cr_Glosa = datos[59].ToString();
                    item.Cs_cr_CondicionPercepcion = datos[60].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = datos[61].ToString();
                    item.Cs_cr_AdqGravadasIGVGravadasNoGravados = datos[62].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleNoGravados = datos[63].ToString();
                    item.Cs_cr_AdqGravadasIGVNoGravados = datos[64].ToString();
                    item.Cs_cr_FechaVencimientoDos = datos[65].ToString();
                    item.Cs_cr_Periodo = datos[66].ToString();

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datos[67].ToString();
                    item.Cs_tag_IssueTime = datos[68].ToString();

                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();

                //unir con las boletas y notas asociadas 
                OdbcDataReader datosBoleta = null;
                string sqlBoleta = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1 ";
                sqlBoleta += " AND (Cs_tag_InvoiceTypeCode='03' OR Cs_tag_BillingReference_DocumentTypeCode='03') ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    sqlBoleta += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "' ";
                }
                if (tipo != "")
                {
                    if (tipo == "1")
                    {
                        //compras
                    }
                    else
                    {
                        //ventas
                        sqlBoleta += " AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + Ruc + "' ";
                    }

                }
                else
                {
                    sqlBoleta += " AND (Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + Ruc + "' OR Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + Ruc + "') ";
                }

                sqlBoleta += " AND Cs_cr_Periodo = '" + periodo + "' ";
                sqlBoleta += " AND Cs_pr_Procesado ='2'  "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                cs_pxConexion_basedatos.Open();
                datosBoleta = new OdbcCommand(sqlBoleta, cs_pxConexion_basedatos).ExecuteReader();
                while (datosBoleta.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datosBoleta[0].ToString();
                    item.Cs_tag_ID = datosBoleta[1].ToString();
                    item.Cs_tag_IssueDate = datosBoleta[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datosBoleta[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datosBoleta[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datosBoleta[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datosBoleta[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datosBoleta[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datosBoleta[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datosBoleta[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datosBoleta[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datosBoleta[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datosBoleta[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datosBoleta[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datosBoleta[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datosBoleta[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datosBoleta[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datosBoleta[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datosBoleta[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datosBoleta[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datosBoleta[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datosBoleta[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datosBoleta[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datosBoleta[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datosBoleta[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datosBoleta[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datosBoleta[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datosBoleta[27].ToString();
                    item.Cs_pr_EstadoValidar = datosBoleta[28].ToString();
                    item.Cs_pr_Procesado = datosBoleta[29].ToString();
                    item.Cs_pr_XML = datosBoleta[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datosBoleta[31].ToString();
                    item.Cs_cr_Movimiento = datosBoleta[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datosBoleta[33].ToString(); //34
                    item.Cs_cr_Pago = datosBoleta[34].ToString(); //34
                    item.Cs_cr_Moneda = datosBoleta[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datosBoleta[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datosBoleta[37].ToString();
                    item.Cs_cr_TipoCambio = datosBoleta[38].ToString(); //34
                    item.Cs_cr_Almacen = datosBoleta[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datosBoleta[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datosBoleta[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datosBoleta[42].ToString(); //34

                    item.Cs_cr_AnioEmisionDUA = datosBoleta[43].ToString();
                    item.Cs_cr_NumeroComprobantePagoSujetoNoDomiciliado = datosBoleta[44].ToString();
                    item.Cs_cr_ConstDepDetNumero = datosBoleta[45].ToString();
                    item.Cs_cr_ConstDepDetFecha = datosBoleta[46].ToString();
                    item.Cs_cr_EquivalenteDolares = datosBoleta[47].ToString();
                    item.Cs_cr_CtaContableBaseImponible = datosBoleta[48].ToString();
                    item.Cs_cr_CtaContableOtrosTributos = datosBoleta[49].ToString();
                    item.Cs_cr_CtaContableTotal = datosBoleta[50].ToString();
                    item.Cs_cr_RegimenEspecial = datosBoleta[51].ToString();
                    item.Cs_cr_PorcentajeRegimenEspecial = datosBoleta[52].ToString();
                    item.Cs_cr_ImporteRegimenEspecial = datosBoleta[53].ToString();
                    item.Cs_cr_SerieDocumentoRegimenEspecial = datosBoleta[54].ToString();
                    item.Cs_cr_NumeroDocumentoRegimenEspecial = datosBoleta[55].ToString();
                    item.Cs_cr_FechaDocumentoRegimenEspecial = datosBoleta[56].ToString();
                    item.Cs_cr_CodigoPresupuesto = datosBoleta[57].ToString();
                    item.Cs_cr_PorcentajeIGV = datosBoleta[58].ToString();
                    item.Cs_cr_Glosa = datosBoleta[59].ToString();
                    item.Cs_cr_CondicionPercepcion = datosBoleta[60].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleGravadasNoGravadas = datosBoleta[61].ToString();
                    item.Cs_cr_AdqGravadasIGVGravadasNoGravados = datosBoleta[62].ToString();
                    item.Cs_cr_AdqGravadasBaseImponibleNoGravados = datosBoleta[63].ToString();
                    item.Cs_cr_AdqGravadasIGVNoGravados = datosBoleta[64].ToString();
                    item.Cs_cr_FechaVencimientoDos = datosBoleta[65].ToString();
                    item.Cs_cr_Periodo = datosBoleta[66].ToString();

                    // A partir de esta celda se agregará las nuevas etiquetas para la versión 2.1 de UBL
                    item.Cs_tag_ProfileID = datosBoleta[67].ToString();
                    item.Cs_tag_IssueTime = datosBoleta[68].ToString();

                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                //
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para obtener todos los documentos 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerTodosEstados(string fechaInicio, string fechaFin, string RUC, string tipo, string periodo)
        {
            List<clsEntidadDocument> lista_documentos = new List<clsEntidadDocument>();
            clsEntidadDocument item;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1  ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    sql += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "'  ";
                }
                if (tipo != "")
                {
                    if (tipo == "1")
                    {
                        //compras
                        sql += " AND Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + RUC + "' ";
                    }
                    else
                    {
                        //ventas
                        sql += " AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + RUC + "' ";
                    }

                }
                else
                {
                    sql += " AND (Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + RUC + "' OR Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + RUC + "') ";
                }
                if (periodo != "")
                {
                    sql += " AND Cs_cr_Periodo = '" + periodo + "' ";
                }
                // sql += " AND Cs_pr_Procesado ='2'  "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();


            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
            }
            return lista_documentos;
        }
        /// <summary>
        /// Metodo para obtener todos los documentos 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerTodosEstadosCompras(string fechaInicio, string fechaFin, string RUC, string tipo, string periodo)
        {
            List<clsEntidadDocument> lista_documentos = new List<clsEntidadDocument>();
            clsEntidadDocument item;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1  ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    sql += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "'  ";
                }
                if (tipo != "")
                {
                    if (tipo == "1")
                    {
                        //compras
                        sql += " AND Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + RUC + "' ";
                    }
                    else
                    {
                        //ventas
                        sql += " AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + RUC + "' ";
                    }

                }
                else
                {
                    sql += " AND (Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + RUC + "' OR Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + RUC + "') ";
                }

                sql += " AND Cs_cr_Periodo = '" + periodo + "' ";

                // sql += " AND Cs_pr_Procesado ='2'  "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();


            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
            }
            return lista_documentos;
        }
        /// <summary>
        /// Metodo para obtener todos los documentos 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerTodosEstadosVisor(string fechaInicio, string fechaFin, string RUC)
        {
            List<clsEntidadDocument> lista_documentos = new List<clsEntidadDocument>();
            clsEntidadDocument item;
            try
            {
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1  ";

                if (fechaInicio != "" && fechaFin != "")
                {
                    sql += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "'  ";
                }

                sql += " AND (Cs_tag_AccSupplierParty_CustomerAssignedAccountID = '" + RUC + "' OR Cs_tag_AccCustomerParty_CustomerAssignedAccountID = '" + RUC + "') ";

                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();


            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
            }
            return lista_documentos;
        }
        /// <summary>
        /// Metodo lara listar los comprobantes validados y aceotados para ventas
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerAceptadosValidadoVentas(string fechaInicio, string fechaFin, string RUC)
        {
            List<clsEntidadDocument> lista_documentos;
            clsEntidadDocument item;
            try
            {
                lista_documentos = new List<clsEntidadDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1 AND Cs_tag_AccSupplierParty_CustomerAssignedAccountID='" + RUC + "' ";
                if (fechaInicio != "" && fechaFin != "")
                {
                    sql += " AND Cs_tag_IssueDate >='" + fechaInicio + "' AND Cs_tag_IssueDate <='" + fechaFin + "' ";
                }
                sql += " AND (Cs_pr_EstadoValidar ='0') ";
                sql += " AND Cs_pr_Procesado ='2'  "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    item.Cs_cr_Movimiento = datos[32].ToString(); //34
                    item.Cs_cr_FechaVencimiento = datos[33].ToString(); //34
                    item.Cs_cr_Pago = datos[34].ToString(); //34
                    item.Cs_cr_Moneda = datos[35].ToString(); //34
                    item.Cs_cr_CondicionPago = datos[36].ToString(); //34
                    item.Cs_cr_CondicionCompra = datos[37].ToString();
                    item.Cs_cr_TipoCambio = datos[38].ToString(); //34
                    item.Cs_cr_Almacen = datos[39].ToString(); //34
                    item.Cs_cr_CentroCostosUno = datos[40].ToString(); //34
                    item.Cs_cr_CentroCostosDos = datos[41].ToString(); //34
                    item.Cs_cr_DocReferenciaFecha = datos[42].ToString(); //34
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// Metodo para listar los comprobantes activos validados
        /// </summary>
        /// <returns></returns>
        public List<clsEntidadDocument> cs_pxObtenerActivosValidado(string RUC)
        {
            List<clsEntidadDocument> lista_documentos;
            clsEntidadDocument item;
            try
            {
                lista_documentos = new List<clsEntidadDocument>();
                OdbcDataReader datos = null;
                string sql = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1"; 
                sql += " AND (Cs_tag_InvoiceTypeCode='01' OR Cs_tag_BillingReference_DocumentTypeCode='01') ";
                sql += " AND Cs_pr_Procesado ='1' AND  Cs_tag_AccCustomerParty_CustomerAssignedAccountID='" + RUC + "' "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                //cn = new clsBaseConexion();
                OdbcConnection cs_pxConexion_basedatos = new OdbcConnection(localDB.cs_prConexioncadenabasedatos());
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sql, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                //buscar boletas y notas asociadas que se emitieron a -
                string sqlBoletas = "SELECT * FROM " + cs_cmTabla + " WHERE 1=1";
                sqlBoletas += " AND (Cs_tag_InvoiceTypeCode='03' OR Cs_tag_BillingReference_DocumentTypeCode='03') ";
                sqlBoletas += " AND Cs_pr_Procesado ='1' "; //Ingresa directo al paso2   0 -> paso-1  1 -> paso2  2->paso3 listo para registro de compras
                cs_pxConexion_basedatos.Open();
                datos = new OdbcCommand(sqlBoletas, cs_pxConexion_basedatos).ExecuteReader();
                while (datos.Read())
                {
                    item = new clsEntidadDocument(localDB);
                    item.Cs_pr_Document_Id = datos[0].ToString();
                    item.Cs_tag_ID = datos[1].ToString();
                    item.Cs_tag_IssueDate = datos[2].ToString();
                    item.Cs_tag_InvoiceTypeCode = datos[3].ToString();//No incluir en NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_DocumentCurrencyCode = datos[4].ToString();
                    item.Cs_tag_Discrepancy_ReferenceID = datos[5].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_ResponseCode = datos[6].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_Discrepancy_Description = datos[7].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_ID = datos[8].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_BillingReference_DocumentTypeCode = datos[9].ToString();//SOLO NOTAS DE DEBITO O CREDITO
                    item.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos[10].ToString();
                    item.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos[11].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos[12].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos[13].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos[14].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos[15].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos[16].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos[17].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos[18].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos[19].ToString();
                    item.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos[20].ToString();
                    item.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos[21].ToString();
                    item.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos[22].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos[23].ToString();
                    item.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos[24].ToString(); //SOLO BOLETA
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = datos[25].ToString();
                    item.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = datos[26].ToString();
                    item.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = datos[27].ToString();
                    item.Cs_pr_EstadoValidar = datos[28].ToString();
                    item.Cs_pr_Procesado = datos[29].ToString();
                    item.Cs_pr_XML = datos[30].ToString();
                    item.Cs_pr_ComentarioSUNAT = datos[31].ToString();
                    lista_documentos.Add(item);
                }
                cs_pxConexion_basedatos.Close();
                return lista_documentos;

            }
            catch (Exception ex)
            {
                clsBaseLog.cs_pxRegistarAdd("Cargar reporte " + ex.ToString());
                return null;
            }
        }

    }
   
}
