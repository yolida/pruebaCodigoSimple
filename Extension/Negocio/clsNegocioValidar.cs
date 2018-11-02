using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioValidar
    {
        private clsEntityDatabaseLocal localDB;

        public clsNegocioValidar(clsEntityDatabaseLocal local)
        {
            localDB = local;
        }
        public clsNegocioValidar()
        {
          
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        /// <summary>
        /// Valida un comprobante
        /// </summary>
        /// <param name="id">Representa el ID de la cabecera del comprobante.</param>
        /// <returns>True: Si no tiene errores, False: Si tiene errores.</returns>
        public bool cs_pxVerificarComprobante(string id)
        {
            bool respuesta = true;
            string xmlValidado = cs_pxGenerarReporteACadena(id);
            if (xmlValidado.Contains("Error") || xmlValidado.Contains("ERROR") || xmlValidado.Contains("error"))
            {
                respuesta = false;
            }
            return respuesta;
        }

        /// <summary>
        /// Generar reporte a archivo
        /// </summary>
        /// <param name="id">Representa el ID de la cabecera del comprobante.</param>
        /// <param name="ruta">La ruta donde se genera el archivo con detalle de errores.</param>
        public void cs_pxGenerarReporteAArchivo(string id, string ruta)
        {
            string mensaje = cs_pxGenerarReporteACadena(id);
            if (!File.Exists(ruta))
            {
                File.Create(ruta).Close();
            }
            StreamWriter sw1 = new StreamWriter(mensaje);
            sw1.Write(mensaje);
            sw1.Close();
        }

        //Cristhian|29/11/2017|
        /// <summary>
        /// DevolverReporte
        /// </summary>
        /// <param name="id">Representa el ID de la cabecera del comprobante.</param>
        /// <param name="ruta">La ruta donde se genera el archivo con detalle de errores.</param>
        public string cs_pxGenerarReporteParaGuardarArchivo(string id, clsEntityDatabaseLocal localDB)
        {
            string mensaje = cs_pxGenerarReporteACadena(id,localDB);
            return mensaje;
        }

        //Cristhian|29/11/2017|
        /*Creado para obtener los errores que puede encontrar el validador*/
        public string cs_pxGenerarReporteACadena(string id, clsEntityDatabaseLocal localDB)
        {
            clsEntityDocument entidad = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(id);
            string ei = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            string ef = "\r\n";
            string contenido = "" + ef;
            try
            {
                contenido += ei + "Fecha y hora de reporte: " + DateTime.Now.ToString() + "<br/>" + ef;
                contenido += ei + "<br/>" + ef;


                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                if (declarante.Cs_pr_Ruc.Trim() != entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID.Trim())
                {
                    contenido += ei + "El RUC de emisor del documento de ser sigual al RUC del declarante FEI." + "[" + "<span class=\"error\">Error</span>" + "]" + ef;
                }

                #region cs_Documents
                contenido += ei + "(Tabla: clsEntityDocument)<br/>" + ef;
                contenido += ei + "====================================================================================<br>" + ef;
                contenido += ei + "/Invoice/cbc:ID -> " + entidad.Cs_tag_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13_FHHH_NNNNNNNN(entidad.Cs_tag_ID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cbc:IssueDate -> " + entidad.Cs_tag_IssueDate + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_10_F_YYYY_MM_DD(entidad.Cs_tag_IssueDate)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode != "07" && entidad.Cs_tag_InvoiceTypeCode != "08")
                {
                    contenido += ei + "Invoice/cbc:InvoiceTypeCode -> " + entidad.Cs_tag_InvoiceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_InvoiceTypeCode)).ToString() + "]<br/>" + ef;//No incluir en NOTAS DE DEBITO O CREDITO
                }
                contenido += ei + "Invoice/cbc:DocumentCurrencyCode: " + entidad.Cs_tag_DocumentCurrencyCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(entidad.Cs_tag_DocumentCurrencyCode)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode == "07" && entidad.Cs_tag_InvoiceTypeCode == "08")
                {
                    contenido += ei + "Invoice_Discrepancy_ReferenceID: " + entidad.Cs_tag_Discrepancy_ReferenceID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_Discrepancy_ReferenceID)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_Discrepancy_ResponseCode: " + entidad.Cs_tag_Discrepancy_ResponseCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_Discrepancy_ResponseCode)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_Discrepancy_Description: " + entidad.Cs_tag_Discrepancy_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(entidad.Cs_tag_Discrepancy_Description)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_BillingReference_ID: " + entidad.Cs_tag_BillingReference_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_BillingReference_ID)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_BillingReference_DocumentTypeCode: " + entidad.Cs_tag_BillingReference_DocumentTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_BillingReference_DocumentTypeCode)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                }
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID (Número de RUC) -> " + entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n11(entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No. 06) -> " + entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyName/cbc:Name -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:ID (Código de ubigeo - Catálogo No. 13) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an6(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:StreetName (Dirección completa y detallada) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CitySubdivisionName (Urbanización) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_25(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CityName (Provincia) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CountrySubentity (Departamento) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:District (Distrito) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode (Código de país - Catálogo No. 04) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID (Número de documento) -> " + entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15(entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No 6) -> " + entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode == "03")
                {
                    contenido += ei + "AccountingCustomerParty_Party_PhysicalLocation_Description: " + entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description)).ToString() + "]<br/>" + ef; //SOLO BOLETA
                }
                contenido += ei + "/Invoice/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount/@currencyID -> " + entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID)).ToString() + "]<br/>" + ef; //No se encuentra el tipo que será validado (No está en la documentación de facturación electrónica)
                contenido += ei + "/Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount/@currencyID " + entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID)).ToString() + "]<br/>" + ef;
                contenido += ei + "comprobanteestado_sunat: " + entidad.Cs_pr_EstadoSUNAT + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSUNAT)).ToString() + "]<br/>" + ef;
                contenido += ei + "comprobanteestado_scc: " + entidad.Cs_pr_EstadoSCC + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSCC)).ToString() + "]<br/>" + ef;
                #endregion

                List<List<string>> guias_remision = new clsEntityDocument_DespatchDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                #region cs_Document_UBLExtension_ExtensionContent_AdditionalInformation
                if (guias_remision.Count > 0)
                {
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument_guiasremision)<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cecabecera_guiasremision_id : " + guias_remision[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[0].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + guias_remision[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[1].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:DespatchDocumentReference/cbc:ID (Número de guía) -> " + guias_remision[2].ToString() + "[" + clsNegocioValidar_Campos.cs_prSER_C_an_30(guias_remision[2].ToString()).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:DespatchDocumentReference/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.01) -> " + guias_remision[3].ToString() + "  [" + clsNegocioValidar_Campos.cs_prSER_C_an2(guias_remision[3].ToString()).ToString() + "]<br/>" + ef;
                }
                #endregion

                List<List<string>> iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                #region cs_tbcecabecera_iars
                int contador = -1;
                if (iars.Count > 0)
                {
                    contador++;
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation )<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cecabecera_iars_id : " + iars[contador][0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][0].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + iars[contador][1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][1].ToString())).ToString() + "]<br/>" + ef;

                    foreach (var item_iars in iars)
                    {
                        //----------
                        List<List<string>> iars_tipomonetario = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerTodoPorCabeceraId(item_iars[0]);
                        foreach (var item_iars_tipomonetario in iars_tipomonetario)
                        {
                            contenido += "<br/>";
                            #region cs_tbcecabecera_iars_tipomonetario
                            contenido += ei + ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _tipomonetario)<br/>" + ef;
                            contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_tipomonetario_id : " + item_iars_tipomonetario[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[0].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_id : " + item_iars_tipomonetario[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[1].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:ID (Código del tipo de elemento - Catálogo No. 14) -> " + item_iars_tipomonetario[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_tipomonetario[2].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:Name -> " + item_iars_tipomonetario[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_tipomonetario[3].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:ReferenceAmount -> " + item_iars_tipomonetario[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[4].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:PayableAmount -> " + item_iars_tipomonetario[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_18_F_n15c2(item_iars_tipomonetario[5].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:Percent -> " + item_iars_tipomonetario[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_Porcentaje(item_iars_tipomonetario[6].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:TotalAmount -> " + item_iars_tipomonetario[7].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[7].ToString())).ToString() + "]<br/>" + ef;
                            #endregion
                        }
                        //----------
                        List<List<string>> iars_cualquiertipo = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerTodoPorId(item_iars[0]);
                        foreach (var item_iars_cualquiertipo in iars_cualquiertipo)
                        {
                            contenido += "<br/>";
                            #region cs_tbcecabecera_iars_cualquiertipo
                            contenido += ei + ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _cualquiertipo)<br/>" + ef;
                            contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_cualquiertipo_id  : " + item_iars_cualquiertipo[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[0].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_id  : " + item_iars_cualquiertipo[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[1].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:ID (Código del concepto - Catálogo No. 15) -> " + item_iars_cualquiertipo[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_cualquiertipo[2].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Name -> " + item_iars_cualquiertipo[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_cualquiertipo[3].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Value(Valor del concepto) -> " + item_iars_cualquiertipo[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_50(item_iars_cualquiertipo[4].ToString())).ToString() + "]<br/>" + ef;
                            #endregion
                        }
                        //----------
                    }
                }
                #endregion
                #region cs_tbcecabecera_impuestosglobales
                List<List<string>> impuestos_globales = new clsEntityDocument_TaxTotal(localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                if (impuestos_globales.Count > 0)
                {
                    foreach (var item_impuestos_globales in impuestos_globales)
                    {
                        contenido += "<br/>";
                        contenido += ei + ei + "(Tabla: clsEntityDocument_impuestosglobales)<br/>" + ef;
                        contenido += ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + "Cecabecera_impuestosglobales_id : " + item_impuestos_globales[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "Cecabecera_id: " + item_impuestos_globales[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[3].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Catálogo No. 05) -> " + item_impuestos_globales[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_impuestos_globales[4].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (Catálogo No. 05) -> " + item_impuestos_globales[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_6(item_impuestos_globales[5].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Catálogo No. 05) -> " + item_impuestos_globales[6].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an3(item_impuestos_globales[6].ToString())).ToString() + "]<br/>" + ef;
                    }
                }
                #endregion
                #region cs_tbcecabecera_otrodocumentorelacionado
                List<List<string>> otro_documento_relacionado = new clsEntityDocument_AdditionalDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                if (otro_documento_relacionado.Count > 0)
                {
                    foreach (var item_otro_documento_relacionado in otro_documento_relacionado)
                    {
                        contenido += "<br/>";
                        contenido += ei + ei + "(Tabla: clsEntityDocument_otrodocumentorelacionado)<br/>" + ef;
                        contenido += ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + "Cecabecera_otrodocumentorelacionado_id  : " + item_otro_documento_relacionado[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "Cecabecera_id: " + item_otro_documento_relacionado[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:AdditionalDocumentReference/cbc:ID (Número de documento relacionado) -> " + item_otro_documento_relacionado[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_otro_documento_relacionado[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:AdditionalDocumentReference/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.12) -> " + item_otro_documento_relacionado[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_otro_documento_relacionado[3].ToString())).ToString() + "]<br/>" + ef;
                    }
                }
                #endregion
                #region cs_tbcedetalle
                List<clsEntityDocument_Line> detalle = new clsEntityDocument_Line(localDB).cs_fxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                foreach (var item_detalle in detalle)
                {
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument )<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cedetalle_id : " + item_detalle.Cs_pr_Document_Line_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Line_Id)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + item_detalle.Cs_pr_Document_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Id)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:ID -> " + item_detalle.Cs_tag_InvoiceLine_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n_3(item_detalle.Cs_tag_InvoiceLine_ID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity/@unitCode (Unidad de medida - Catálogo No. 03) -> " + item_detalle.Cs_tag_InvoicedQuantity_unitCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_3(item_detalle.Cs_tag_InvoicedQuantity_unitCode)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity -> " + item_detalle.Cs_tag_invoicedQuantity + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_16_F_n12c3(item_detalle.Cs_tag_invoicedQuantity)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:LineExtensionAmount/@currencyID -> " + item_detalle.Cs_tag_LineExtensionAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_LineExtensionAmount_currencyID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceAmount/@currencyID -> " + item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceTypeCode (Código de tipo de precio -Catálogo No. 16) " + item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:Item/cac:SellersItemIdentification/cbc:ID -> " + item_detalle.Cs_tag_Item_SellersItemIdentification + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_detalle.Cs_tag_Item_SellersItemIdentification)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:Price/cbc:PriceAmount/@currencyID -> " + item_detalle.Cs_tag_Price_PriceAmount + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_Price_PriceAmount)).ToString() + "]<br/>" + ef;

                    List<List<string>> detalle_informaciongeneral = new clsEntityDocument_Line_TaxTotal(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);
                    List<List<string>> detalle_descripcion = new clsEntityDocument_Line_Description(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);

                    foreach (var item_detalle_informaciongeneral in detalle_informaciongeneral)
                    {
                        contenido += "<br/>";
                        #region cs_tbcedetalle_informaciondeimpuesto
                        contenido += ei + ei + ei + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto)<br/>" + ef;
                        contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_informaciondeimpuesto_id  : " + item_detalle_informaciongeneral[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_id  : " + item_detalle_informaciongeneral[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[2].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[3].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TaxExemptionReasonCode (Catálogo No. 07) -> " + item_detalle_informaciongeneral[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[4].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TierRange (Tipo de sistema de ISC - Catálogo No. 08) -> " + item_detalle_informaciongeneral[5].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[5].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Código de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_detalle_informaciongeneral[6].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (NombreTributo de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[7].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_6(item_detalle_informaciongeneral[7].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Código internacional tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[8].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(item_detalle_informaciongeneral[8].ToString())).ToString() + "]<br/>" + ef;
                        #endregion
                    }
                    foreach (var item_detalle_descripcion in detalle_descripcion)
                    {
                        contenido += "<br/>";
                        #region cs_tbcedetalle_descripcionitem
                        contenido += ei + ei + ei + "(Tabla: clsEntidadCedetalle_descripcionitem)<br/>" + ef;
                        contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_descripcionitem_id : " + item_detalle_descripcion[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_id : " + item_detalle_descripcion[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:Item/cbc:Description ->" + item_detalle_descripcion[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(item_detalle_descripcion[2].ToString())).ToString() + "]<br/>" + ef;
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception)
            {

            }
            return contenido;
        }

        /// <summary>
        /// Generar reporte a cadena
        /// </summary>
        /// <param name="id">Representa el ID de la cabecera del comprobante.</param>
        /// <returns>Devuelve en un texto con las validaciones sobre un comprobante electrónico.</returns>
        public string cs_pxGenerarReporteACadena(string id)
        {
            clsEntityDocument entidad = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(id);
            string ei = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            string ef = "\r\n";
            string contenido = "" + ef;
            try
            {
                contenido += ei + "Fecha y hora de reporte: " + DateTime.Now.ToString() +"<br/>"+ ef;
                contenido += ei + "<br/>" + ef;


                clsEntityDeclarant declarante =  new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                if (declarante.Cs_pr_Ruc.Trim() != entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID.Trim())
                {
                    contenido += ei + "El RUC de emisor del documento de ser sigual al RUC del declarante FEI." +"["+ "<span class=\"error\">Error</span>" + "]"+ ef;
                }

                #region cs_Documents
                contenido += ei + "(Tabla: clsEntityDocument)<br/>" + ef;
                contenido += ei + "====================================================================================<br>" + ef;
                contenido += ei + "/Invoice/cbc:ID -> " + entidad.Cs_tag_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13_FHHH_NNNNNNNN(entidad.Cs_tag_ID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cbc:IssueDate -> " + entidad.Cs_tag_IssueDate + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_10_F_YYYY_MM_DD(entidad.Cs_tag_IssueDate)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode != "07" && entidad.Cs_tag_InvoiceTypeCode != "08")
                {
                    contenido += ei + "Invoice/cbc:InvoiceTypeCode -> " + entidad.Cs_tag_InvoiceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_InvoiceTypeCode)).ToString() + "]<br/>" + ef;//No incluir en NOTAS DE DEBITO O CREDITO
                }
                contenido += ei + "Invoice/cbc:DocumentCurrencyCode: " + entidad.Cs_tag_DocumentCurrencyCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(entidad.Cs_tag_DocumentCurrencyCode)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode == "07" && entidad.Cs_tag_InvoiceTypeCode == "08")
                {
                    contenido += ei + "Invoice_Discrepancy_ReferenceID: " + entidad.Cs_tag_Discrepancy_ReferenceID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_Discrepancy_ReferenceID)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_Discrepancy_ResponseCode: " + entidad.Cs_tag_Discrepancy_ResponseCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_Discrepancy_ResponseCode)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_Discrepancy_Description: " + entidad.Cs_tag_Discrepancy_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(entidad.Cs_tag_Discrepancy_Description)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_BillingReference_ID: " + entidad.Cs_tag_BillingReference_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_BillingReference_ID)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei + "Invoice_BillingReference_DocumentTypeCode: " + entidad.Cs_tag_BillingReference_DocumentTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_BillingReference_DocumentTypeCode)).ToString() + "]<br/>" + ef;//SOLO NOTAS DE DEBITO O CREDITO
                }
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID (Número de RUC) -> " + entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n11(entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No. 06) -> " + entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyName/cbc:Name -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:ID (Código de ubigeo - Catálogo No. 13) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an6(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:StreetName (Dirección completa y detallada) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CitySubdivisionName (Urbanización) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_25(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CityName (Provincia) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CountrySubentity (Departamento) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:District (Distrito) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode (Código de país - Catálogo No. 04) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID (Número de documento) -> " + entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15(entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No 6) -> " + entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID)).ToString() + "]<br/>" + ef;
                contenido += ei + "/Invoice/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef;
                if (entidad.Cs_tag_InvoiceTypeCode == "03")
                {
                    contenido += ei + "AccountingCustomerParty_Party_PhysicalLocation_Description: " + entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description)).ToString() + "]<br/>" + ef; //SOLO BOLETA
                }
                contenido += ei + "/Invoice/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount/@currencyID -> " + entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID)).ToString() + "]<br/>" + ef; //No se encuentra el tipo que será validado (No está en la documentación de facturación electrónica)
                contenido += ei + "/Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount/@currencyID " + entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID)).ToString() + "]<br/>" + ef;
                contenido += ei + "comprobanteestado_sunat: " + entidad.Cs_pr_EstadoSUNAT + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSUNAT)).ToString() + "]<br/>" + ef;
                contenido += ei + "comprobanteestado_scc: " + entidad.Cs_pr_EstadoSCC + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSCC)).ToString() + "]<br/>" + ef;
                #endregion

                List<List<string>> guias_remision = new clsEntityDocument_DespatchDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                #region cs_Document_UBLExtension_ExtensionContent_AdditionalInformation
                if (guias_remision.Count > 0)
                {
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument_guiasremision)<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cecabecera_guiasremision_id : " + guias_remision[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[0].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + guias_remision[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[1].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:DespatchDocumentReference/cbc:ID (Número de guía) -> " + guias_remision[2].ToString() + "[" + clsNegocioValidar_Campos.cs_prSER_C_an_30(guias_remision[2].ToString()).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:DespatchDocumentReference/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.01) -> " + guias_remision[3].ToString() + "  [" + clsNegocioValidar_Campos.cs_prSER_C_an2(guias_remision[3].ToString()).ToString() + "]<br/>" + ef;
                }
                #endregion

                List<List<string>> iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                #region cs_tbcecabecera_iars
                int contador = -1;
                if (iars.Count > 0)
                {
                    contador++;
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation )<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cecabecera_iars_id : " + iars[contador][0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][0].ToString())).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + iars[contador][1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][1].ToString())).ToString() + "]<br/>" + ef;

                    foreach (var item_iars in iars)
                    {
                        //----------
                        List<List<string>> iars_tipomonetario = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerTodoPorCabeceraId(item_iars[0]);
                        foreach (var item_iars_tipomonetario in iars_tipomonetario)
                        {
                            contenido += "<br/>";
                            #region cs_tbcecabecera_iars_tipomonetario
                            contenido += ei + ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _tipomonetario)<br/>" + ef;
                            contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_tipomonetario_id : " + item_iars_tipomonetario[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[0].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_id : " + item_iars_tipomonetario[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[1].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:ID (Código del tipo de elemento - Catálogo No. 14) -> " + item_iars_tipomonetario[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_tipomonetario[2].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:Name -> " + item_iars_tipomonetario[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_tipomonetario[3].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:ReferenceAmount -> " + item_iars_tipomonetario[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[4].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:PayableAmount -> " + item_iars_tipomonetario[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_18_F_n15c2(item_iars_tipomonetario[5].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:Percent -> " + item_iars_tipomonetario[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_Porcentaje(item_iars_tipomonetario[6].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:TotalAmount -> " + item_iars_tipomonetario[7].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[7].ToString())).ToString() + "]<br/>" + ef;
                            #endregion
                        }
                        //----------
                        List<List<string>> iars_cualquiertipo = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerTodoPorId(item_iars[0]);
                        foreach (var item_iars_cualquiertipo in iars_cualquiertipo)
                        {
                            contenido += "<br/>";
                            #region cs_tbcecabecera_iars_cualquiertipo
                            contenido += ei + ei + ei + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _cualquiertipo)<br/>" + ef;
                            contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_cualquiertipo_id  : " + item_iars_cualquiertipo[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[0].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "Cecabecera_iars_id  : " + item_iars_cualquiertipo[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[1].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:ID (Código del concepto - Catálogo No. 15) -> " + item_iars_cualquiertipo[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_cualquiertipo[2].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Name -> " + item_iars_cualquiertipo[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_cualquiertipo[3].ToString())).ToString() + "]<br/>" + ef;
                            contenido += ei + ei + ei + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Value(Valor del concepto) -> " + item_iars_cualquiertipo[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_50(item_iars_cualquiertipo[4].ToString())).ToString() + "]<br/>" + ef;
                            #endregion
                        }
                        //----------
                    }
                }
                #endregion
                #region cs_tbcecabecera_impuestosglobales
                List<List<string>> impuestos_globales = new clsEntityDocument_TaxTotal(localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                if (impuestos_globales.Count > 0)
                {
                    foreach (var item_impuestos_globales in impuestos_globales)
                    {
                        contenido += "<br/>";
                        contenido += ei + ei + "(Tabla: clsEntityDocument_impuestosglobales)<br/>" + ef;
                        contenido += ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + "Cecabecera_impuestosglobales_id : " + item_impuestos_globales[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "Cecabecera_id: " + item_impuestos_globales[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[3].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Catálogo No. 05) -> " + item_impuestos_globales[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_impuestos_globales[4].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (Catálogo No. 05) -> " + item_impuestos_globales[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_6(item_impuestos_globales[5].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Catálogo No. 05) -> " + item_impuestos_globales[6].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an3(item_impuestos_globales[6].ToString())).ToString() + "]<br/>" + ef;
                    }
                }
                #endregion
                #region cs_tbcecabecera_otrodocumentorelacionado
                List<List<string>> otro_documento_relacionado = new clsEntityDocument_AdditionalDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                if (otro_documento_relacionado.Count > 0)
                {
                    foreach (var item_otro_documento_relacionado in otro_documento_relacionado)
                    {
                        contenido += "<br/>";
                        contenido += ei + ei + "(Tabla: clsEntityDocument_otrodocumentorelacionado)<br/>" + ef;
                        contenido += ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + "Cecabecera_otrodocumentorelacionado_id  : " + item_otro_documento_relacionado[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "Cecabecera_id: " + item_otro_documento_relacionado[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:AdditionalDocumentReference/cbc:ID (Número de documento relacionado) -> " + item_otro_documento_relacionado[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_otro_documento_relacionado[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + "/Invoice/cac:AdditionalDocumentReference/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.12) -> " + item_otro_documento_relacionado[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_otro_documento_relacionado[3].ToString())).ToString() + "]<br/>" + ef;
                    }
                }
                #endregion
                #region cs_tbcedetalle
                List<clsEntityDocument_Line> detalle = new clsEntityDocument_Line(localDB).cs_fxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                foreach (var item_detalle in detalle)
                {
                    contenido += "<br/>";
                    contenido += ei + ei + "(Tabla: clsEntityDocument )<br/>" + ef;
                    contenido += ei + ei + "====================================================================================<br>" + ef;
                    contenido += ei + ei + "Cedetalle_id : " + item_detalle.Cs_pr_Document_Line_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Line_Id)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "Cecabecera_id: " + item_detalle.Cs_pr_Document_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Id)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:ID -> " + item_detalle.Cs_tag_InvoiceLine_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n_3(item_detalle.Cs_tag_InvoiceLine_ID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity/@unitCode (Unidad de medida - Catálogo No. 03) -> " + item_detalle.Cs_tag_InvoicedQuantity_unitCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_3(item_detalle.Cs_tag_InvoicedQuantity_unitCode)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity -> " + item_detalle.Cs_tag_invoicedQuantity + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_16_F_n12c3(item_detalle.Cs_tag_invoicedQuantity)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cbc:LineExtensionAmount/@currencyID -> " + item_detalle.Cs_tag_LineExtensionAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_LineExtensionAmount_currencyID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceAmount/@currencyID -> " + item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:PricingReference/cac:AlternativeConditionPrice/cbc:PriceTypeCode (Código de tipo de precio -Catálogo No. 16) " + item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:Item/cac:SellersItemIdentification/cbc:ID -> " + item_detalle.Cs_tag_Item_SellersItemIdentification + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_detalle.Cs_tag_Item_SellersItemIdentification)).ToString() + "]<br/>" + ef;
                    contenido += ei + ei + "/Invoice/cac:InvoiceLine/cac:Price/cbc:PriceAmount/@currencyID -> " + item_detalle.Cs_tag_Price_PriceAmount + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_Price_PriceAmount)).ToString() + "]<br/>" + ef;
                    
                    List<List<string>> detalle_informaciongeneral = new clsEntityDocument_Line_TaxTotal(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);
                    List<List<string>> detalle_descripcion = new clsEntityDocument_Line_Description(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);

                    foreach (var item_detalle_informaciongeneral in detalle_informaciongeneral)
                    {
                        contenido += "<br/>";
                        #region cs_tbcedetalle_informaciondeimpuesto
                        contenido += ei + ei + ei + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto)<br/>" + ef;
                        contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_informaciondeimpuesto_id  : " + item_detalle_informaciongeneral[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_id  : " + item_detalle_informaciongeneral[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[2].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[2].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[3].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TaxExemptionReasonCode (Catálogo No. 07) -> " + item_detalle_informaciongeneral[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[4].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TierRange (Tipo de sistema de ISC - Catálogo No. 08) -> " + item_detalle_informaciongeneral[5].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[5].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Código de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_detalle_informaciongeneral[6].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (NombreTributo de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[7].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_6(item_detalle_informaciongeneral[7].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Código internacional tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[8].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(item_detalle_informaciongeneral[8].ToString())).ToString() + "]<br/>" + ef;
                        #endregion
                    }
                    foreach (var item_detalle_descripcion in detalle_descripcion)
                    {
                        contenido += "<br/>";
                        #region cs_tbcedetalle_descripcionitem
                        contenido += ei + ei + ei + "(Tabla: clsEntidadCedetalle_descripcionitem)<br/>" + ef;
                        contenido += ei + ei + ei + "====================================================================================<br>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_descripcionitem_id : " + item_detalle_descripcion[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[0].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "Cedetalle_id : " + item_detalle_descripcion[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[1].ToString())).ToString() + "]<br/>" + ef;
                        contenido += ei + ei + ei + "/Invoice/cac:InvoiceLine/cac:Item/cbc:Description ->" + item_detalle_descripcion[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(item_detalle_descripcion[2].ToString())).ToString() + "]<br/>" + ef;
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                
            }
            return contenido;
        }

        /// <summary>
        /// Genera reporte a HTML para el visor
        /// </summary>
        /// <param name="id">Representa el ID de la cabecera del comprobante.</param>
        /// <returns></returns>
        public string cs_pxGenerarReporteAHTML(string id)
        {
            clsEntityDocument entidad = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(id);
            string ef = "\r\n";
            string ei_table = "<tr><td>";
            string ei_table_titulo = "<tr><td class=\"titulo\">";
            string ef_table = "</td></tr>";
            string contenido = "";
            try
            {
                contenido += "<h1>DOCUMENTO XML (INCLUYE VALIDACIONES)</h1>";
                contenido += "<h1>Fecha y hora: " + DateTime.Now.ToString() + "</h1>";

                clsEntityDeclarant declarante = new clsEntityDeclarant().cs_pxObtenerUnoPorId(localDB.Cs_pr_Declarant_Id);
                if (declarante.Cs_pr_Ruc.Trim() != entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID.Trim())
                {
                    contenido += "<p>"+ "[" + "<span class=\"error\">Error</span>" + "] -> "+"El RUC de emisor del documento de ser sigual al RUC del declarante FEI.</p>" + ef;
                }

                contenido += "<table cellspacing=\"0\">";
                #region cs_Documents
                contenido += ei_table_titulo + "(Tabla: clsEntityDocument)<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cbc:ID -> " + entidad.Cs_tag_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13_FHHH_NNNNNNNN(entidad.Cs_tag_ID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cbc:IssueDate -> " + entidad.Cs_tag_IssueDate + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_10_F_YYYY_MM_DD(entidad.Cs_tag_IssueDate)).ToString() + "]<br/>" + ef_table;
                if (entidad.Cs_tag_InvoiceTypeCode != "07" && entidad.Cs_tag_InvoiceTypeCode != "08")
                {
                    contenido += ei_table + "/Invoice/cbc:InvoiceTypeCode -> " + entidad.Cs_tag_InvoiceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_InvoiceTypeCode)).ToString() + "]<br/>" + ef_table;//No incluir en NOTAS DE DEBITO O CREDITO
                }
                contenido += ei_table + "/Invoice/cbc:DocumentCurrencyCode: " + entidad.Cs_tag_DocumentCurrencyCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(entidad.Cs_tag_DocumentCurrencyCode)).ToString() + "]<br/>" + ef_table;
                if (entidad.Cs_tag_InvoiceTypeCode == "07" && entidad.Cs_tag_InvoiceTypeCode == "08")
                {
                    contenido += ei_table + "Invoice_Discrepancy_Ref_tableerenceID: " + entidad.Cs_tag_Discrepancy_ReferenceID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_Discrepancy_ReferenceID)).ToString() + "]<br/>" + ef_table;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei_table + "Invoice_Discrepancy_ResponseCode: " + entidad.Cs_tag_Discrepancy_ResponseCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_Discrepancy_ResponseCode)).ToString() + "]<br/>" + ef_table;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei_table + "Invoice_Discrepancy_Description: " + entidad.Cs_tag_Discrepancy_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(entidad.Cs_tag_Discrepancy_Description)).ToString() + "]<br/>" + ef_table;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei_table + "Invoice_BillingRef_tableerence_ID: " + entidad.Cs_tag_BillingReference_ID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_13(entidad.Cs_tag_BillingReference_ID)).ToString() + "]<br/>" + ef_table;//SOLO NOTAS DE DEBITO O CREDITO
                    contenido += ei_table + "Invoice_BillingRef_tableerence_DocumentTypeCode: " + entidad.Cs_tag_BillingReference_DocumentTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an2(entidad.Cs_tag_BillingReference_DocumentTypeCode)).ToString() + "]<br/>" + ef_table;//SOLO NOTAS DE DEBITO O CREDITO
                }
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cbc:CustomerAssignedAccountID (Número de RUC) -> " + entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n11(entidad.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No. 06) -> " + entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingSupplierParty_AdditionalAccountID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyName/cbc:Name -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyName_Name)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:ID (Código de ubigeo - Catálogo No. 13) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an6(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:StreetName (Dirección completa y detallada) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CitySubdivisionName (Urbanización) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_25(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CityName (Provincia) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:CountrySubentity (Departamento) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cbc:District (Distrito) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress/cac:Country/cbc:IdentificationCode (Código de país - Catálogo No. 04) -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(entidad.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingSupplierParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingCustomerParty/cbc:CustomerAssignedAccountID (Número de documento) -> " + entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15(entidad.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingCustomerParty/cbc:AdditionalAccountID (Tipo de documento - Catálogo No 6) -> " + entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_tag_AccountingCustomerParty_AdditionalAccountID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "/Invoice/cac:AccountingCustomerParty/cac:Party/cac:PartyLegalEntity/cbc:RegistrationName -> " + entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName)).ToString() + "]<br/>" + ef_table;
                if (entidad.Cs_tag_InvoiceTypeCode == "03")
                {
                    contenido += ei_table + "AccountingCustomerParty_Party_PhysicalLocation_Description: " + entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(entidad.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description)).ToString() + "]<br/>" + ef_table; //SOLO BOLETA
                }
                contenido += ei_table + "/Invoice/cac:LegalMonetaryTotal/cbc:ChargeTotalAmount/@currencyID -> " + entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID)).ToString() + "]<br/>" + ef_table; //No se encuentra el tipo que será validado (No está en la documentación de facturación electrónica)
                contenido += ei_table + "/Invoice/cac:LegalMonetaryTotal/cbc:PayableAmount/@currencyID " + entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(entidad.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "comprobanteestado_sunat: " + entidad.Cs_pr_EstadoSUNAT + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSUNAT)).ToString() + "]<br/>" + ef_table;
                contenido += ei_table + "comprobanteestado_scc: " + entidad.Cs_pr_EstadoSCC + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n1(entidad.Cs_pr_EstadoSCC)).ToString() + "]<br/>" + ef_table;
                #endregion

                List<List<string>> guias_remision = new clsEntityDocument_DespatchDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                #region cs_Document_UBLExtension_ExtensionContent_AdditionalInformation
                if (guias_remision.Count > 0)
                {
                    contenido += ei_table_titulo + "(Tabla: clsEntityDocument_guiasremision)<br/>" + ef_table;
                    contenido += ei_table + "Cecabecera_guiasremision_id : " + guias_remision[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[0].ToString())).ToString() + "]<br/>" + ef_table;
                    contenido += ei_table + "Cecabecera_id: " + guias_remision[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(guias_remision[1].ToString())).ToString() + "]<br/>" + ef_table;
                    contenido += ei_table + "/Invoice/cac:DespatchDocumentRef_tableerence/cbc:ID (Número de guía) -> " + guias_remision[2].ToString() + "[" + clsNegocioValidar_Campos.cs_prSER_C_an_30(guias_remision[2].ToString()).ToString() + "]<br/>" + ef_table;
                    contenido += ei_table + "/Invoice/cac:DespatchDocumentRef_tableerence/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.01) -> " + guias_remision[3].ToString() + "  [" + clsNegocioValidar_Campos.cs_prSER_C_an2(guias_remision[3].ToString()).ToString() + "]<br/>" + ef_table;
                }
                #endregion

                List<List<string>> iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation (localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                #region cs_tbcecabecera_iars
                int contador = -1;
                if (iars.Count > 0)
                {
                    contador++;
                    contenido += ei_table_titulo + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation )<br/>" + ef_table;
                    contenido += ei_table + "Cecabecera_iars_id : " + iars[contador][0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][0].ToString())).ToString() + "]<br/>" + ef_table;
                    contenido += ei_table + "Cecabecera_id: " + iars[contador][1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(iars[contador][1].ToString())).ToString() + "]<br/>" + ef_table;

                    foreach (var item_iars in iars)
                    {
                        //----------
                        List<List<string>> iars_tipomonetario = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localDB).cs_pxObtenerTodoPorCabeceraId(item_iars[0]);
                        foreach (var item_iars_tipomonetario in iars_tipomonetario)
                        {
                            #region cs_tbcecabecera_iars_tipomonetario
                            contenido += ei_table_titulo + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _tipomonetario)<br/>" + ef_table;
                            contenido += ei_table + "Cecabecera_iars_tipomonetario_id : " + item_iars_tipomonetario[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[0].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "Cecabecera_iars_id : " + item_iars_tipomonetario[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_tipomonetario[1].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:ID (Código del tipo de elemento - Catálogo No. 14) -> " + item_iars_tipomonetario[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_tipomonetario[2].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:Name -> " + item_iars_tipomonetario[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_tipomonetario[3].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:Ref_tableerenceAmount -> " + item_iars_tipomonetario[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[4].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/cbc:PayableAmount -> " + item_iars_tipomonetario[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_18_F_n15c2(item_iars_tipomonetario[5].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:Percent -> " + item_iars_tipomonetario[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_Porcentaje(item_iars_tipomonetario[6].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalMonetaryTotal/sac:TotalAmount -> " + item_iars_tipomonetario[7].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_iars_tipomonetario[7].ToString())).ToString() + "]<br/>" + ef_table;
                            #endregion
                        }
                        //----------
                        List<List<string>> iars_cualquiertipo = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localDB).cs_pxObtenerTodoPorId(item_iars[0]);
                        foreach (var item_iars_cualquiertipo in iars_cualquiertipo)
                        {
                            #region cs_tbcecabecera_iars_cualquiertipo
                            contenido += ei_table_titulo + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation _cualquiertipo)<br/>" + ef_table;
                            contenido += ei_table + "Cecabecera_iars_cualquiertipo_id  : " + item_iars_cualquiertipo[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[0].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "Cecabecera_iars_id  : " + item_iars_cualquiertipo[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_iars_cualquiertipo[1].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:ID (Código del concepto - Catálogo No. 15) -> " + item_iars_cualquiertipo[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_iars_cualquiertipo[2].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Name -> " + item_iars_cualquiertipo[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_100(item_iars_cualquiertipo[3].ToString())).ToString() + "]<br/>" + ef_table;
                            contenido += ei_table + "/Invoice/ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent/sac:AdditionalInformation/sac:AdditionalProperty/cbc:Value(Valor del concepto) -> " + item_iars_cualquiertipo[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15(item_iars_cualquiertipo[4].ToString())).ToString() + "]<br/>" + ef_table;
                            #endregion
                        }
                        //----------
                    }
                }
                else
                {
                    contenido += ei_table_titulo + "(Tabla: clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation  - Debe registrar por lo menos un elemento en esta tabla) [" + "<span class=\"error\">Error</span>" + "]" + ef_table;
                }
                #endregion
                #region cs_tbcecabecera_impuestosglobales
                List<List<string>> impuestos_globales = new clsEntityDocument_TaxTotal(localDB).cs_pxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);
                if (impuestos_globales.Count > 0)
                {
                    foreach (var item_impuestos_globales in impuestos_globales)
                    {
                        contenido += ei_table_titulo + "(Tabla: clsEntityDocument_impuestosglobales)<br/>" + ef_table;
                        contenido += ei_table + "Cecabecera_impuestosglobales_id : " + item_impuestos_globales[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[0].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "Cecabecera_id: " + item_impuestos_globales[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_impuestos_globales[1].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[2].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_impuestos_globales[3].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_impuestos_globales[3].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Catálogo No. 05) -> " + item_impuestos_globales[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_impuestos_globales[4].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (Catálogo No. 05) -> " + item_impuestos_globales[5].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_6(item_impuestos_globales[5].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Catálogo No. 05) -> " + item_impuestos_globales[6].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an3(item_impuestos_globales[6].ToString())).ToString() + "]<br/>" + ef_table;
                    }
                }
                else
                {
                    contenido += ei_table_titulo + "(Tabla: clsEntityDocument_impuestosglobales - Debe registrar por lo menos un elemento en esta tabla) [" + "<span class=\"error\">Error</span>" + "]" + ef_table;
                }
                #endregion
                #region cs_tbcecabecera_otrodocumentorelacionado
                List<List<string>> otro_documento_relacionado = new clsEntityDocument_AdditionalDocumentReference(localDB).cs_pxObtenerTodoPorId(entidad.Cs_pr_Document_Id);
                if (otro_documento_relacionado.Count > 0)
                {
                    foreach (var item_otro_documento_relacionado in otro_documento_relacionado)
                    {
                        contenido += ei_table_titulo + "(Tabla: clsEntityDocument_otrodocumentorelacionado)<br/>" + ef_table;
                        contenido += ei_table + "Cecabecera_otrodocumentorelacionado_id  : " + item_otro_documento_relacionado[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[0].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "Cecabecera_id: " + item_otro_documento_relacionado[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_otro_documento_relacionado[1].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:AdditionalDocumentReference/cbc:ID (Número de documento relacionado) -> " + item_otro_documento_relacionado[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_otro_documento_relacionado[2].ToString())).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:AdditionalDocumentReference/cbc:DocumentTypeCode (Tipo de documento - Catálogo No.12) -> " + item_otro_documento_relacionado[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_otro_documento_relacionado[3].ToString())).ToString() + "]<br/>" + ef_table;
                    }
                }
                #endregion
                #region cs_tbcedetalle
                List<clsEntityDocument_Line> detalle = new clsEntityDocument_Line(localDB).cs_fxObtenerTodoPorCabeceraId(entidad.Cs_pr_Document_Id);

                if (detalle.Count>0)
                {
                    foreach (var item_detalle in detalle)
                    {
                        contenido += ei_table_titulo + "(Tabla: clsEntityDocument )<br/>" + ef_table;
                        contenido += ei_table + "Cedetalle_id : " + item_detalle.Cs_pr_Document_Line_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Line_Id)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "Cecabecera_id: " + item_detalle.Cs_pr_Document_Id + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle.Cs_pr_Document_Id)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cbc:ID -> " + item_detalle.Cs_tag_InvoiceLine_ID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_n_3(item_detalle.Cs_tag_InvoiceLine_ID)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity/@unitCode (Unidad de medida - Catálogo No. 03) -> " + item_detalle.Cs_tag_InvoicedQuantity_unitCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_3(item_detalle.Cs_tag_InvoicedQuantity_unitCode)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cbc:InvoicedQuantity -> " + item_detalle.Cs_tag_invoicedQuantity + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_16_F_n12c3(item_detalle.Cs_tag_invoicedQuantity)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cbc:LineExtensionAmount/@currencyID -> " + item_detalle.Cs_tag_LineExtensionAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_LineExtensionAmount_currencyID)).ToString() + "]<br/>" + ef_table;
                        //contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:PricingRef_tableerence/cac:AlternativeConditionPrice/cbc:PriceAmount/@currencyID -> " + item_detalle.InvoiceLine_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_15_F_n12c2(item_detalle.InvoiceLine_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID)).ToString() + "]<br/>" + ef_table;
                        //contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:PricingRef_tableerence/cac:AlternativeConditionPrice/cbc:PriceTypeCode (Código de tipo de precio -Catálogo No. 16) " + item_detalle.InvoiceLine_PricingReference_AlternativeConditionPrice_PriceTypeCode + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle.InvoiceLine_PricingReference_AlternativeConditionPrice_PriceTypeCode)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:Item/cac:SellersItemIdentification/cbc:ID -> " + item_detalle.Cs_tag_Item_SellersItemIdentification + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an_30(item_detalle.Cs_tag_Item_SellersItemIdentification)).ToString() + "]<br/>" + ef_table;
                        contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:Price/cbc:PriceAmount/@currencyID -> " + item_detalle.Cs_tag_Price_PriceAmount + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle.Cs_tag_Price_PriceAmount)).ToString() + "]<br/>" + ef_table;

                        List<List<string>> detalle_informaciongeneral = new clsEntityDocument_Line_TaxTotal(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);
                        List<List<string>> detalle_descripcion = new clsEntityDocument_Line_Description(localDB).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);

                        if (detalle_informaciongeneral.Count > 0)
                        {
                            foreach (var item_detalle_informaciongeneral in detalle_informaciongeneral)
                            {
                                #region cs_tbcedetalle_informaciondeimpuesto
                                contenido += ei_table_titulo + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto)<br/>" + ef_table;
                                contenido += ei_table + "Cedetalle_informaciondeimpuesto_id  : " + item_detalle_informaciongeneral[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[0].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "Cedetalle_id  : " + item_detalle_informaciongeneral[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_informaciongeneral[1].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[2].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[2].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cbc:TaxAmount/@currencyID -> " + item_detalle_informaciongeneral[3].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_15_F_n12c2(item_detalle_informaciongeneral[3].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TaxExemptionReasonCode (Catálogo No. 07) -> " + item_detalle_informaciongeneral[4].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[4].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cbc:TierRange (Tipo de sistema de ISC - Catálogo No. 08) -> " + item_detalle_informaciongeneral[5].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an2(item_detalle_informaciongeneral[5].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:ID (Código de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[6].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_C_an4(item_detalle_informaciongeneral[6].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:Name (NombreTributo de tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[7].ToString() + "[" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_6(item_detalle_informaciongeneral[7].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:TaxTotal/cac:TaxSubtotal/cac:TaxCategory/cac:TaxScheme/cbc:TaxTypeCode (Código internacional tributo - Catálogo No. 05) -> " + item_detalle_informaciongeneral[8].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an3(item_detalle_informaciongeneral[8].ToString())).ToString() + "]<br/>" + ef_table;
                                #endregion
                            }
                        }
                        else
                        {
                            contenido += ei_table_titulo + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto - Debe registrar por lo menos un elemento en esta tabla) [" + "<span class=\"error\">Error</span>" + "]" + ef_table;
                        }


                        if (detalle_descripcion.Count>0)
                        {
                            foreach (var item_detalle_descripcion in detalle_descripcion)
                            {
                                #region cs_tbcedetalle_descripcionitem
                                contenido += ei_table_titulo + "(Tabla: clsEntidadCedetalle_descripcionitem)<br/>" + ef_table;
                                contenido += ei_table + "Cedetalle_descripcionitem_id : " + item_detalle_descripcion[0].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[0].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "Cedetalle_id : " + item_detalle_descripcion[1].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_ID(item_detalle_descripcion[1].ToString())).ToString() + "]<br/>" + ef_table;
                                contenido += ei_table + "/Invoice/cac:InvoiceLine/cac:Item/cbc:Description ->" + item_detalle_descripcion[2].ToString() + "  [" + cs_pxMensaje(clsNegocioValidar_Campos.cs_prSER_M_an_250(item_detalle_descripcion[2].ToString())).ToString() + "]<br/>" + ef_table;
                                #endregion
                            }
                        }
                        else
                        {
                            contenido += ei_table_titulo + "(Tabla: clsEntidadCedetalle_informaciondeimpuesto - Debe registrar por lo menos un elemento en esta tabla) [" + "<span class=\"error\">Error</span>" + "]" + ef_table;
                        }
                        
                    }
                }
                else
                {
                    contenido += ei_table_titulo + "(Tabla: clsEntityDocument  - Debe registrar por lo menos un elemento en esta tabla) [" + "<span class=\"error\">Error</span>" + "]" + ef_table;
                }

                
                contenido += "</table>";
                #endregion
            }
            catch (Exception)
            {

            }
            return contenido;
        }

        private string cs_pxMensaje(bool contenido)
        {
            string mensaje = null;
            if (contenido == true)
            {
                mensaje = "<span class=\"correcto\">Correcto</span>";
            }
            if (contenido == false)
            {
                mensaje = "<span class=\"error\">Error</span>";
            }
            return mensaje;
        }

    }
}