using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{
    public class clsNegocioCENotaCredito : clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCENotaCredito(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }
        public override string cs_pxGenerarXMLAString(string id)
        {
            string archivo_xml = string.Empty;
            clsEntityDocument cabecera = new clsEntityDocument(localbd).cs_fxObtenerUnoPorId(id);
            List<clsEntityDocument_Line> detalle = new clsEntityDocument_Line(localbd).cs_fxObtenerTodoPorCabeceraId(cabecera.Cs_pr_Document_Id);

            string fila = "";
            string ei = "    ";
            string ef = "\n";

            fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
            fila += "<CreditNote" + ef;
            #region Cabecera
            fila += ei + "xmlns =\"urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2\"" + ef;
            fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
            fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;//Antes aparecia en la fila 2. Modificado por un mensaje de error que indica que la fila 2 debe ser CommonBasicComponents.
            fila += ei + "xmlns:ccts=\"urn:un:unece:uncefact:documentation:2\"" + ef;
            fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
            fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
            fila += ei + "xmlns:qdt=\"urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2\"" + ef;
            fila += ei + "xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\"" + ef;
            fila += ei + "xmlns:udt=\"urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2\"" + ef;
            fila += ei + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" + ef;
            #endregion
            fila += ei + "<ext:UBLExtensions>" + ef;
            #region Extension
            fila += ei + ei + "<ext:UBLExtension>" + ef;
            //Extension de información.
            clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation  objIars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation (localbd);
            List<List<string>> iars = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation (localbd).cs_pxObtenerTodoPorCabeceraId(cabecera.Cs_pr_Document_Id);
            List<List<string>> guias_remision = new clsEntityDocument_DespatchDocumentReference(localbd).cs_pxObtenerTodoPorId(cabecera.Cs_pr_Document_Id);
            List<List<string>> otro_documento_relacionado = new clsEntityDocument_AdditionalDocumentReference(localbd).cs_pxObtenerTodoPorId(cabecera.Cs_pr_Document_Id);//
            List<List<string>> impuestos_globales = new clsEntityDocument_TaxTotal(localbd).cs_pxObtenerTodoPorCabeceraId(cabecera.Cs_pr_Document_Id);
            fila += ei + ei + ei + "<ext:ExtensionContent>" + ef;
            if (iars.Count > 0)
            {
                List<List<string>> iars_tipomonetario = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(localbd).cs_pxObtenerTodoPorCabeceraId(iars[0][0]);
                List<List<string>> iars_cualquiertipo = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(localbd).cs_pxObtenerTodoPorId(iars[0][0]);

                if (iars_tipomonetario != null || iars_cualquiertipo != null)
                {
                    if (iars_tipomonetario.Count > 0 || iars_cualquiertipo.Count > 0)
                    {
                        foreach (var item_iars in iars)
                        {
                            fila += ei + ei + ei + ei + "<sac:AdditionalInformation>" + ef;
                            foreach (var item_iars_tipomonetario in iars_tipomonetario)
                            {
                                fila += ei + ei + ei + ei + ei + "<sac:AdditionalMonetaryTotal>" + ef;
                                fila += ei + ei + ei + ei + ei + ei + "<cbc:ID>" + item_iars_tipomonetario[2] + "</cbc:ID>" + ef;
                                if (item_iars_tipomonetario[3].Trim().Length > 0)
                                {
                                    fila += ei + ei + ei + ei + ei + ei + "<cbc:Name>" + item_iars_tipomonetario[3] + "</cbc:Name>" + ef;
                                }
                                if (item_iars_tipomonetario[4].Trim().Length > 0)
                                {
                                    fila += ei + ei + ei + ei + ei + ei + "<sac:ReferenceAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_iars_tipomonetario[4] + "</sac:ReferenceAmount>" + ef;
                                }
                                fila += ei + ei + ei + ei + ei + ei + "<cbc:PayableAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_iars_tipomonetario[5] + "</cbc:PayableAmount>" + ef;
                                if (item_iars_tipomonetario[6].Trim().Length > 0)
                                {
                                    fila += ei + ei + ei + ei + ei + ei + "<sac:Percent>" + item_iars_tipomonetario[6] + "</sac:Percent>" + ef;
                                }
                                if (item_iars_tipomonetario[7].Trim().Length > 0)
                                {
                                    fila += ei + ei + ei + ei + ei + ei + "<sac:TotalAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_iars_tipomonetario[7] + "</sac:TotalAmount>" + ef;
                                }
                                fila += ei + ei + ei + ei + ei + "</sac:AdditionalMonetaryTotal>" + ef;
                            }
                            foreach (var item_iars_cualquiertipo in iars_cualquiertipo)
                            {
                                fila += ei + ei + ei + ei + ei + "<sac:AdditionalProperty>" + ef;
                                fila += ei + ei + ei + ei + ei + ei + "<cbc:ID>" + item_iars_cualquiertipo[2] + "</cbc:ID>" + ef;
                                if (item_iars_cualquiertipo[3].Trim().Length > 0)
                                {
                                    fila += ei + ei + ei + ei + ei + ei + "<cbc:Name>" + item_iars_cualquiertipo[3] + "</cbc:Name>" + ef;
                                }
                                fila += ei + ei + ei + ei + ei + ei + "<cbc:Value>" + item_iars_cualquiertipo[4] + "</cbc:Value>" + ef;
                                fila += ei + ei + ei + ei + ei + "</sac:AdditionalProperty>" + ef;
                            }
                            fila += ei + ei + ei + ei + "</sac:AdditionalInformation>" + ef;
                        }
                    }
                }
            }
            fila += ei + ei + ei + "</ext:ExtensionContent>" + ef;
            //                     Componente de extensión para especificar información adicional
            fila += ei + ei + "</ext:UBLExtension>" + ef;
            #endregion

            #region Extension de comentarios adicionales
            if (new clsEntityDocument_AdditionalComments(localbd).cs_fxVerificarExistencia(cabecera.Cs_pr_Document_Id) == true)
            {
                fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent>" + new clsEntityDocument_AdditionalComments(localbd).cs_fxObtenerXML(cabecera.Cs_pr_Document_Id) + "</ext:ExtensionContent></ext:UBLExtension>" + ef;
            }
            #endregion

            fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>".Trim() + ef;
            fila += ei + "</ext:UBLExtensions>" + ef;

            

            #region Información de cabecera
            fila += ei + "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" + ef;
            fila += ei + "<cbc:CustomizationID>1.0</cbc:CustomizationID>" + ef;
            fila += ei + "<cbc:ID>" + cabecera.Cs_tag_ID + "</cbc:ID>" + ef;
            fila += ei + "<cbc:IssueDate>" + cabecera.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
            fila += ei + "<cbc:DocumentCurrencyCode>" + cabecera.Cs_tag_DocumentCurrencyCode + "</cbc:DocumentCurrencyCode>" + ef;
            fila += ei + "<cac:DiscrepancyResponse>" + ef;
            fila += ei + "<cbc:ReferenceID>"+ cabecera.Cs_tag_Discrepancy_ReferenceID + "</cbc:ReferenceID>" + ef;
            fila += ei + "<cbc:ResponseCode>" + cabecera.Cs_tag_Discrepancy_ResponseCode + "</cbc:ResponseCode>" + ef;
            fila += ei + "<cbc:Description>" + cabecera.Cs_tag_Discrepancy_Description + "</cbc:Description>" + ef;
            fila += ei + "</cac:DiscrepancyResponse>" + ef;
            fila += ei + "<cac:BillingReference>" + ef;
            fila += ei + "<cac:InvoiceDocumentReference>" + ef;
            fila += ei + "<cbc:ID>" + cabecera.Cs_tag_BillingReference_ID + "</cbc:ID>" + ef;
            fila += ei + "<cbc:DocumentTypeCode>" + cabecera.Cs_tag_BillingReference_DocumentTypeCode + "</cbc:DocumentTypeCode>" + ef;
            fila += ei + "</cac:InvoiceDocumentReference>" + ef;
            fila += ei + "</cac:BillingReference>" + ef;
            #endregion

            #region Guías de remisión
            if (guias_remision.Count > 0)
            {
                foreach (var item_guias_remision in guias_remision)
                {
                    fila += ei + "<cac:DespatchDocumentReference>" + ef;
                    fila += ei + ei + "<cbc:ID>" + item_guias_remision[2] + "</cbc:ID>" + ef;
                    fila += ei + ei + "<cbc:DocumentTypeCode>" + item_guias_remision[3] + "</cbc:DocumentTypeCode>" + ef;
                    fila += ei + "</cac:DespatchDocumentReference>" + ef;
                }
            }
            #endregion

            #region Cualquier otro documeto relacionado a la operación
            if (otro_documento_relacionado.Count > 0)
            {
                foreach (var item_otro_documento_relacionado in otro_documento_relacionado)
                {
                    fila += ei + "<cac:AdditionalDocumentReference>" + ef;
                    fila += ei + ei + "<cbc:ID>" + item_otro_documento_relacionado[2] + "</cbc:ID>" + ef;
                    fila += ei + ei + "<cbc:DocumentTypeCode>" + item_otro_documento_relacionado[3] + "</cbc:DocumentTypeCode>" + ef;
                    fila += ei + "</cac:AdditionalDocumentReference>" + ef;
                }
            }
            #endregion

            #region Referencia de la firma digital
            fila += ei + "<cac:Signature>" + ef;
            fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
            fila += ei + ei + "<cac:SignatoryParty>" + ef;
            fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
            fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
            fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;
            //Jordy Amaro 14/11/16 FE-865
            //Agregado <!CDATA.
            fila += ei + ei + ei + "<cac:PartyName>" + ef;
            fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + "RAZONSOCIALDECERTIFICADO" + "]]></cbc:Name>" + ef;
            fila += ei + ei + ei + "</cac:PartyName>" + ef;
            fila += ei + ei + "</cac:SignatoryParty>" + ef;

            fila += ei + ei + "<cac:DigitalSignatureAttachment>" + ef;
            fila += ei + ei + ei + "<cac:ExternalReference>" + ef;
            fila += ei + ei + ei + ei + "<cbc:URI>#SignatureSP</cbc:URI>" + ef;
            fila += ei + ei + ei + "</cac:ExternalReference>" + ef;
            fila += ei + ei + "</cac:DigitalSignatureAttachment>" + ef;
            fila += ei + "</cac:Signature>" + ef;
            #endregion

            #region Datos del emisor del documento
            fila += ei + "<cac:AccountingSupplierParty>" + ef;
            fila += ei + ei + "<cbc:CustomerAssignedAccountID>" + cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
            fila += ei + ei + "<cbc:AdditionalAccountID>" + cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID + "</cbc:AdditionalAccountID>" + ef;

            if (cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName.Trim().Length > 0)
            {
                fila += ei + ei + "<cac:Party>" + ef;

                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name.Trim().Length > 0)
                {
                    //Jordy Amaro 08/11/16 FE-851
                    //Agregado <!CDATA.
                    fila += ei + ei + ei + "<cac:PartyName>" + ef;
                    fila += ei + ei + ei + ei + "<cbc:Name><![CDATA[" + cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name + "]]></cbc:Name>" + ef;
                    fila += ei + ei + ei + "</cac:PartyName>" + ef;
                }

                fila += ei + ei + ei + "<cac:PostalAddress>" + ef;

                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:ID>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID + "</cbc:ID>" + ef;
                }
                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:StreetName><![CDATA[" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName + "]]></cbc:StreetName>" + ef;
                }
                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:CitySubdivisionName>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName + "</cbc:CitySubdivisionName>" + ef;
                }
                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:CityName>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName + "</cbc:CityName>" + ef;
                }
                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:CountrySubentity>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity + "</cbc:CountrySubentity>" + ef;
                }
                if (cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District.Trim().Length > 0)
                {
                    fila += ei + ei + ei + ei + "<cbc:District>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District + "</cbc:District>" + ef;
                }
                fila += ei + ei + ei + ei + "<cac:Country>" + ef;
                fila += ei + ei + ei + ei + ei + "<cbc:IdentificationCode>" + cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode + "</cbc:IdentificationCode>" + ef;
                fila += ei + ei + ei + ei + "</cac:Country>" + ef;
                fila += ei + ei + ei + "</cac:PostalAddress>" + ef;
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                //Jordy Amaro 08/11/16 FE-851
                //Agregado <!CDATA.
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
            }
            fila += ei + "</cac:AccountingSupplierParty>" + ef;
            #endregion

            #region Datos del adquiriente o usuario
            fila += ei + "<cac:AccountingCustomerParty>" + ef;
            fila += ei + ei + "<cbc:CustomerAssignedAccountID>" + cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
            fila += ei + ei + "<cbc:AdditionalAccountID>" + cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID + "</cbc:AdditionalAccountID>" + ef;
            fila += ei + ei + "<cac:Party>" + ef;
            if (cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description.Length > 0)
            {
                fila += ei + ei + ei + "<cac:PhysicalLocation >" + ef;
                fila += ei + ei + ei + ei + "<cbc:Description><![CDATA[" + cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description + "]]></cbc:Description>" + ef;
                fila += ei + ei + ei + "</cac:PhysicalLocation>" + ef;
            }
            fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
            //Jordy Amaro 08/11/16 FE-851
            //Agregado <!CDATA.
            fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName + "]]></cbc:RegistrationName>" + ef;
            fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
           
            fila += ei + ei + "</cac:Party>" + ef;
            fila += ei + "</cac:AccountingCustomerParty>" + ef;
            #endregion

            #region Impuestos Globales
            if (impuestos_globales.Count > 0)
            {
                foreach (var item_impuestos_globales in impuestos_globales)
                {
                    fila += ei + "<cac:TaxTotal>" + ef;
                    fila += ei + ei + "<cbc:TaxAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_impuestos_globales[2] + "</cbc:TaxAmount>" + ef;
                    fila += ei + ei + "<cac:TaxSubtotal>" + ef;
                    fila += ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_impuestos_globales[3] + "</cbc:TaxAmount>" + ef;
                    fila += ei + ei + ei + "<cac:TaxCategory>" + ef;
                    fila += ei + ei + ei + ei + "<cac:TaxScheme>" + ef;
                    fila += ei + ei + ei + ei + ei + "<cbc:ID>" + item_impuestos_globales[4] + "</cbc:ID>" + ef;
                    fila += ei + ei + ei + ei + ei + "<cbc:Name>" + item_impuestos_globales[5] + "</cbc:Name>" + ef;
                    fila += ei + ei + ei + ei + ei + "<cbc:TaxTypeCode>" + item_impuestos_globales[6] + "</cbc:TaxTypeCode>" + ef;
                    fila += ei + ei + ei + ei + "</cac:TaxScheme>" + ef;
                    fila += ei + ei + ei + "</cac:TaxCategory>" + ef;
                    fila += ei + ei + "</cac:TaxSubtotal>" + ef;
                    fila += ei + "</cac:TaxTotal>" + ef;
                }
            }
            #endregion

            #region Totales a pagar de la factura y cargos
            fila += ei + "<cac:LegalMonetaryTotal>" + ef;
            if (cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID.Trim().Length > 0)
            {
                fila += ei + ei + "<cbc:ChargeTotalAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID + "</cbc:ChargeTotalAmount>" + ef;
            }
            fila += ei + ei + "<cbc:PayableAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID + "</cbc:PayableAmount>" + ef;
            fila += ei + "</cac:LegalMonetaryTotal>" + ef;
            #endregion

            #region Items de factura
            foreach (var item_detalle in detalle)
            {
                fila += ei + "<cac:CreditNoteLine>" + ef;
                fila += ei + ei + "<cbc:ID>" + item_detalle.Cs_tag_InvoiceLine_ID + "</cbc:ID>" + ef;
                fila += ei + ei + "<cbc:CreditedQuantity unitCode=\"" + item_detalle.Cs_tag_InvoicedQuantity_unitCode + "\">" + item_detalle.Cs_tag_invoicedQuantity + "</cbc:CreditedQuantity>" + ef;
                fila += ei + ei + "<cbc:LineExtensionAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle.Cs_tag_LineExtensionAmount_currencyID + "</cbc:LineExtensionAmount>" + ef;
                List<List<string>> detalle_valoresunitarios = new clsEntityDocument_Line_PricingReference(localbd).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);

                if (detalle_valoresunitarios != null && detalle_valoresunitarios.Count > 0)
                {
                    fila += ei + ei + "<cac:PricingReference>" + ef;
                    foreach (var item_detalle_valoresunitarios in detalle_valoresunitarios)
                    {
                        fila += ei + ei + ei + "<cac:AlternativeConditionPrice>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:PriceAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle_valoresunitarios[2].ToString() + "</cbc:PriceAmount>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:PriceTypeCode>" + item_detalle_valoresunitarios[3].ToString() + "</cbc:PriceTypeCode>" + ef;
                        fila += ei + ei + ei + "</cac:AlternativeConditionPrice>" + ef;
                    }
                    fila += ei + ei + "</cac:PricingReference>" + ef;
                }

                if (item_detalle.Cs_tag_AllowanceCharge_ChargeIndicator != null && item_detalle.Cs_tag_AllowanceCharge_Amount != null)
                {
                    if (item_detalle.Cs_tag_AllowanceCharge_ChargeIndicator.Trim().Length > 0 && item_detalle.Cs_tag_AllowanceCharge_Amount.Trim().Length > 0)
                    {
                        fila += ei + ei + ei + "<cac:AllowanceCharge>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:Amount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle.Cs_tag_AllowanceCharge_Amount + "</cbc:Amount>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:ChargeIndicator>" + item_detalle.Cs_tag_AllowanceCharge_ChargeIndicator + "</cbc:ChargeIndicator>" + ef;
                        fila += ei + ei + ei + "</cac:AllowanceCharge>" + ef;
                    }
                }


                List<List<string>> detalle_informaciongeneral = new clsEntityDocument_Line_TaxTotal(localbd).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);
                List<List<string>> detalle_descripcion = new clsEntityDocument_Line_Description(localbd).cs_pxObtenerTodoPorId(item_detalle.Cs_pr_Document_Line_Id);

                foreach (var item_detalle_informaciongeneral in detalle_informaciongeneral)
                {


                    fila += ei + ei + "<cac:TaxTotal>" + ef;
                    fila += ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle_informaciongeneral[2] + "</cbc:TaxAmount>" + ef;
                    fila += ei + ei + ei + "<cac:TaxSubtotal>" + ef;
                    fila += ei + ei + ei + ei + "<cbc:TaxAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle_informaciongeneral[3] + "</cbc:TaxAmount>" + ef;
                    fila += ei + ei + ei + ei + "<cac:TaxCategory>" + ef;
                    fila += ei + ei + ei + ei + ei + "<cbc:TaxExemptionReasonCode>" + item_detalle_informaciongeneral[4] + "</cbc:TaxExemptionReasonCode>" + ef;
                    if (item_detalle_informaciongeneral[5].ToString().Trim().Length > 0)
                    {
                        fila += ei + ei + ei + ei + ei + "<cbc:TierRange>" + item_detalle_informaciongeneral[5] + "</cbc:TierRange>" + ef;
                    }
                    fila += ei + ei + ei + ei + ei + "<cac:TaxScheme>" + ef;
                    fila += ei + ei + ei + ei + ei + ei + "<cbc:ID>" + item_detalle_informaciongeneral[6] + "</cbc:ID>" + ef;
                    fila += ei + ei + ei + ei + ei + ei + "<cbc:Name>" + item_detalle_informaciongeneral[7] + "</cbc:Name>" + ef;
                    fila += ei + ei + ei + ei + ei + ei + "<cbc:TaxTypeCode>" + item_detalle_informaciongeneral[8] + "</cbc:TaxTypeCode>" + ef;
                    fila += ei + ei + ei + ei + ei + "</cac:TaxScheme>" + ef;
                    fila += ei + ei + ei + ei + "</cac:TaxCategory>" + ef;
                    fila += ei + ei + ei + "</cac:TaxSubtotal>" + ef;
                    fila += ei + ei + "</cac:TaxTotal>" + ef;
                }
                fila += ei + ei + "<cac:Item>" + ef;
                foreach (var item_detalle_descripcion in detalle_descripcion)
                {
                    fila += ei + ei + ei + "<cbc:Description><![CDATA[" + item_detalle_descripcion[2] + "]]></cbc:Description>" + ef;
                }
                fila += ei + ei + ei + "<cac:SellersItemIdentification>" + ef;
                fila += ei + ei + ei + ei + "<cbc:ID>" + item_detalle.Cs_tag_Item_SellersItemIdentification + "</cbc:ID>" + ef;
                fila += ei + ei + ei + "</cac:SellersItemIdentification>" + ef;
                fila += ei + ei + "</cac:Item>" + ef;
                fila += ei + ei + "<cac:Price>" + ef;
                fila += ei + ei + ei + "<cbc:PriceAmount currencyID=\"" + cabecera.Cs_tag_DocumentCurrencyCode + "\">" + item_detalle.Cs_tag_Price_PriceAmount + "</cbc:PriceAmount>" + ef;
                fila += ei + ei + "</cac:Price>" + ef;
                fila += ei + "</cac:CreditNoteLine>" + ef;
            }
            #endregion

            fila += "</CreditNote>" + ef;
            //fila = UnicodeToUTF8(fila);

            string pfxPath = declarante.Cs_pr_Rutacertificadodigital.Replace("\\\\", "\\");
            X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(pfxPath), declarante.Cs_pr_Parafrasiscertificadodigital);

            //Cristhian|25/08/2017|FEI2-352
            /*Se agrega un metodo de busqueda para ubicar la razon social de la empresa 
              ya no dependiendo de la ubicacion donde se encuentre, ahora se busca su
              etiqueta y se obtine el valor(la razon social)*/
            /*NUEVO INICIO*/
            string[] subject = cert.SubjectName.Name.Split(',');
            foreach (string item in subject)
            {
                string[] subject_o = item.ToString().Split('=');
                if (subject_o[0].Trim() == "O")
                {
                    fila = fila.Replace("RAZONSOCIALDECERTIFICADO", subject_o[1].TrimStart());
                }
            }
            /*NUEVO FIN*/

            XmlDocument documento = new XmlDocument();
            documento.PreserveWhitespace = false;
            documento.LoadXml(fila.Replace("\\\\", "\\"));
            archivo_xml = FirmarXml(documento, cert);
            return archivo_xml;
        }

        private string UnicodeToUTF8(string strFrom)
        {
            byte[] bytSrc;
            byte[] bytDestination;
            string strTo = String.Empty;

            bytSrc = Encoding.Unicode.GetBytes(strFrom);
            bytDestination = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, bytSrc);
            strTo = Encoding.ASCII.GetString(bytDestination);

            return strTo;
        }
    }
}
