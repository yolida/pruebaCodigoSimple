using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Base
{
    public class clsBaseXML
    {
        /// <summary>
        /// Metodo para procesar la cadena del xml brindado.
        /// </summary>
        /// <param name="cadenaXML"></param>
        /// <param name="local"></param>
        /// <param name="facturas"></param>
        /// <returns></returns>
        /// Aquí se le esta pasando el XML en la cadena cadenaXML
        public string procesarXML(string cadenaXML, clsEntityDatabaseLocal local, string RucReceptor)
        {
            string resultado = string.Empty;

            try
            {
                clsEntidadDocument documento = new clsEntidadDocument(local);
                XmlDocument xmlDocument = new XmlDocument();
                //var textXml = cabecera.Cs_pr_XML;
                var textXml = cadenaXML;
                textXml     = textXml.Replace("cbc:", "");
                textXml     = textXml.Replace("cac:", "");
                textXml     = textXml.Replace("sac:", "");
                textXml     = textXml.Replace("ext:", "");
                textXml     = textXml.Replace("ds:", "");
                xmlDocument.LoadXml(textXml);

                var signatureValue = xmlDocument.GetElementsByTagName("SignatureValue")[0].InnerText;
                var digestValue = xmlDocument.GetElementsByTagName("DigestValue")[0].InnerText;

                XmlNodeList IDS = xmlDocument.GetElementsByTagName("ID");
                foreach (XmlNode node in IDS)
                {
                    string padre = node.ParentNode.LocalName;
                    if (padre == "Invoice" || padre == "CreditNote" || padre == "DebitNote")
                    {
                        documento.Cs_tag_ID = node.InnerText;
                        string[] partes = node.InnerText.Split('-');
                        string serie = partes[0];
                        string numero = partes[1].PadLeft(8, '0'); // Completa con ceros las parte del número de documento
                        documento.Cs_tag_ID = serie + "-" + numero;
                        break;
                    }

                }

                XmlNodeList InvoiceTypeCodeXml = xmlDocument.GetElementsByTagName("InvoiceTypeCode");
                if (InvoiceTypeCodeXml.Count > 0)
                {
                    documento.Cs_tag_InvoiceTypeCode = xmlDocument.GetElementsByTagName("InvoiceTypeCode")[0].InnerText;
                }
                else
                {
                    //buscar si es nota de credito o debito
                    XmlNodeList nodeitemCreditNote = xmlDocument.GetElementsByTagName("CreditNoteLine");
                    if (nodeitemCreditNote.Count > 0)
                    {
                        documento.Cs_tag_InvoiceTypeCode = "07";
                    }

                    XmlNodeList nodeitemDebitNote = xmlDocument.GetElementsByTagName("DebitNoteLine");
                    if (nodeitemDebitNote.Count > 0)
                    {
                        documento.Cs_tag_InvoiceTypeCode = "08";
                    }
                }

                documento.Cs_tag_IssueDate = xmlDocument.GetElementsByTagName("IssueDate")[0].InnerText;
                documento.Cs_tag_DocumentCurrencyCode = xmlDocument.GetElementsByTagName("DocumentCurrencyCode")[0].InnerText;

                documento.Cs_cr_Moneda = clsBaseUtil.cs_GetMoneda(documento.Cs_tag_DocumentCurrencyCode);
                //get accounting supplier party
                XmlNodeList AccountingSupplierParty = xmlDocument.GetElementsByTagName("AccountingSupplierParty");
                foreach (XmlNode dat in AccountingSupplierParty)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var identicode = xmlDocumentinner.GetElementsByTagName("IdentificationCode");
                    if (identicode.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = identicode.Item(0).InnerText;
                    }

                    var streetcountry = xmlDocumentinner.GetElementsByTagName("CountrySubentity");
                    if (streetcountry.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = streetcountry.Item(0).InnerText;
                    }
                    var streetdistrict = xmlDocumentinner.GetElementsByTagName("District");
                    if (streetdistrict.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = streetdistrict.Item(0).InnerText;
                    }

                    var cityname = xmlDocumentinner.GetElementsByTagName("CityName");
                    if (cityname.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = cityname.Item(0).InnerText;
                    }

                    var citysub = xmlDocumentinner.GetElementsByTagName("CitySubdivisionName");
                    if (citysub.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = citysub.Item(0).InnerText;
                    }

                    var asppai = xmlDocumentinner.GetElementsByTagName("ID");
                    if (asppai.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = asppai.Item(0).InnerText;
                    }

                    var asppn = xmlDocumentinner.GetElementsByTagName("PartyName");
                    if (asppn.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = asppn.Item(0).InnerText;
                    }
                    var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                    if (caaid.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = caaid.Item(0).InnerText;
                    }
                    var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                    if (aacid.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_AdditionalAccountID = aacid.Item(0).InnerText;
                    }
                    var stname = xmlDocumentinner.GetElementsByTagName("StreetName");
                    if (stname.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = stname.Item(0).InnerText;
                    }
                    var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                    if (regname.Count > 0)
                    {
                        documento.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = regname.Item(0).InnerText;
                    }
                }
                //get accounting supplier party
                XmlNodeList AccountingCustomerParty = xmlDocument.GetElementsByTagName("AccountingCustomerParty");
                foreach (XmlNode dat in AccountingCustomerParty)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var caaid = xmlDocumentinner.GetElementsByTagName("CustomerAssignedAccountID");
                    if (caaid.Count > 0)
                    {
                        documento.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = caaid.Item(0).InnerText;
                    }
                    var aacid = xmlDocumentinner.GetElementsByTagName("AdditionalAccountID");
                    if (aacid.Count > 0)
                    {
                        documento.Cs_tag_AccountingCustomerParty_AdditionalAccountID = aacid.Item(0).InnerText;
                    }
                    var descr = xmlDocumentinner.GetElementsByTagName("Description");
                    if (descr.Count > 0)
                    {
                        documento.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = descr.Item(0).InnerText;
                    }
                    var regname = xmlDocumentinner.GetElementsByTagName("RegistrationName");
                    if (regname.Count > 0)
                    {
                        documento.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = regname.Item(0).InnerText;
                    }
                }
                XmlNodeList DiscrepancyResponse = xmlDocument.GetElementsByTagName("DiscrepancyResponse");
                foreach (XmlNode dat in DiscrepancyResponse)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var refid = xmlDocumentinner.GetElementsByTagName("ReferenceID");
                    if (refid.Count > 0)
                    {
                        string[] partes = refid.Item(0).InnerText.Split('-');
                        string serie = partes[0];
                        string numero = partes[1].PadLeft(8, '0');

                        documento.Cs_tag_Discrepancy_ReferenceID = serie + "-" + numero;
                    }
                    var respcode = xmlDocumentinner.GetElementsByTagName("ResponseCode");
                    if (respcode.Count > 0)
                    {
                        documento.Cs_tag_Discrepancy_ResponseCode = respcode.Item(0).InnerText;
                    }
                    var descr = xmlDocumentinner.GetElementsByTagName("Description");
                    if (descr.Count > 0)
                    {
                        documento.Cs_tag_Discrepancy_Description = descr.Item(0).InnerText;
                    }

                }
                XmlNodeList BillingResponse = xmlDocument.GetElementsByTagName("BillingReference");
                foreach (XmlNode dat in BillingResponse)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var brefid = xmlDocumentinner.GetElementsByTagName("ID");
                    if (brefid.Count > 0)
                    {
                        string[] partes = brefid.Item(0).InnerText.Split('-');
                        string serie = partes[0];
                        string numero = partes[1].PadLeft(8, '0');

                        documento.Cs_tag_BillingReference_ID = serie + "-" + numero;
                    }
                    var respcode = xmlDocumentinner.GetElementsByTagName("DocumentTypeCode");
                    if (respcode.Count > 0)
                    {
                        documento.Cs_tag_BillingReference_DocumentTypeCode = respcode.Item(0).InnerText;
                    }

                }

                XmlNodeList LegalMonetaryTotal = null;

                if (documento.Cs_tag_InvoiceTypeCode == "08")
                {
                    LegalMonetaryTotal = xmlDocument.GetElementsByTagName("RequestedMonetaryTotal");
                }
                else
                {
                    LegalMonetaryTotal = xmlDocument.GetElementsByTagName("LegalMonetaryTotal");
                }

                foreach (XmlNode dat in LegalMonetaryTotal)
                {
                    XmlDocument xmlDocumentinner = new XmlDocument();
                    xmlDocumentinner.LoadXml(dat.OuterXml);

                    var cta = xmlDocumentinner.GetElementsByTagName("ChargeTotalAmount");
                    if (cta.Count > 0)
                    {
                        documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = cta.Item(0).InnerText;
                    }
                    var pam = xmlDocumentinner.GetElementsByTagName("PayableAmount");
                    if (pam.Count > 0)
                    {
                        documento.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = pam.Item(0).InnerText;
                    }
                    var ata = xmlDocumentinner.GetElementsByTagName("AllowanceTotalAmount");
                    if (ata.Count > 0)
                    {
                        documento.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = ata.Item(0).InnerText;
                    }

                }

                documento.Cs_pr_EstadoValidar = "4";
                documento.Cs_pr_Procesado = "1";
                documento.Cs_pr_XML = cadenaXML.Replace("'", "\"");

                bool procesar = false;
                
                if (RucReceptor.Trim() == documento.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID)
                {
                    procesar = true;
                }

                if (documento.Cs_tag_InvoiceTypeCode == "03" || documento.Cs_tag_BillingReference_DocumentTypeCode == "03")
                {
                    procesar = true;
                }

                if (procesar == true)
                {
                    //buscar si existe el doc en la base de datos
                    bool existe = new clsEntidadDocument(local).cs_pxExisteDocumento(documento.Cs_tag_ID, documento.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID);
                    if (existe) // No le encuentro sentido a su existencia
                    {
                        return "";
                    }

                    string idDocumento = documento.cs_pxInsertar(false, false); // Insertar

                    if (idDocumento != "")
                    {
                        XmlNodeList nodestaxTotal = xmlDocument.GetElementsByTagName("TaxTotal");
                        foreach (XmlNode dat in nodestaxTotal)
                        {
                            string nodoPadre = dat.ParentNode.LocalName;
                            if (nodoPadre == "Invoice" || nodoPadre == "DebitNote" || nodoPadre == "CreditNote")
                            {
                                clsEntidadDocument_TaxTotal taxTotal = new clsEntidadDocument_TaxTotal(local);
                                taxTotal.Cs_pr_Document_Id = idDocumento;

                                XmlDocument xmlDocumentTaxtotal = new XmlDocument();
                                xmlDocumentTaxtotal.LoadXml(dat.OuterXml);
                                XmlNodeList taxAmount = xmlDocumentTaxtotal.GetElementsByTagName("TaxAmount");
                                if (taxAmount.Count > 0)
                                {
                                    taxTotal.Cs_tag_TaxAmount = taxAmount.Item(0).InnerText;
                                }
                                XmlNodeList subtotal = xmlDocumentTaxtotal.GetElementsByTagName("TaxSubtotal");
                                if (subtotal.Count > 0)
                                {
                                    XmlDocument xmlDocumentTaxSubtotal = new XmlDocument();
                                    xmlDocumentTaxSubtotal.LoadXml(subtotal.Item(0).OuterXml);

                                    var subTotalAmount = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxAmount");
                                    if (subTotalAmount.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxAmount = subTotalAmount.Item(0).InnerText;
                                    }
                                    var subTotalID = xmlDocumentTaxSubtotal.GetElementsByTagName("ID");
                                    if (subTotalID.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                    }
                                    var subTotalName = xmlDocumentTaxSubtotal.GetElementsByTagName("Name");
                                    if (subTotalName.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                    }
                                    var subTotalTaxTypeCode = xmlDocumentTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                    if (subTotalTaxTypeCode.Count > 0)
                                    {
                                        taxTotal.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                    }

                                }
                                taxTotal.cs_pxInsertar(false, true);
                            }
                        }

                        XmlNodeList additionalInformation = xmlDocument.GetElementsByTagName("AdditionalInformation");
                        foreach (XmlNode dat in additionalInformation)
                        {
                            XmlDocument xmlDocumentinner = new XmlDocument();
                            xmlDocumentinner.LoadXml(dat.OuterXml);
                            clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal adittionalMonetary;

                            XmlNodeList LIST1 = xmlDocumentinner.GetElementsByTagName("AdditionalMonetaryTotal");
                            for (int ii = 0; ii < LIST1.Count; ii++)
                            {
                                adittionalMonetary = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                                adittionalMonetary.Cs_pr_Document_Id = idDocumento;

                                var ss = LIST1.Item(ii);
                                XmlDocument xmlDocumentinner1 = new XmlDocument();
                                xmlDocumentinner1.LoadXml(ss.OuterXml);

                                var id = xmlDocumentinner1.GetElementsByTagName("ID");
                                if (id.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_Id = id.Item(0).InnerText;

                                    if (id.Item(0).Attributes.Count > 0)
                                    {
                                        adittionalMonetary.Cs_tag_SchemeID = id.Item(0).Attributes.GetNamedItem("schemeID").Value;
                                    }
                                }
                                var Name = xmlDocumentinner1.GetElementsByTagName("Name");
                                if (Name.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_Name = Name.Item(0).InnerText;
                                }
                                var TotalAmount = xmlDocumentinner1.GetElementsByTagName("TotalAmount");
                                if (TotalAmount.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_TotalAmount = TotalAmount.Item(0).InnerText;
                                }
                                var percent = xmlDocumentinner1.GetElementsByTagName("Percent");
                                if (percent.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_Percent = percent.Item(0).InnerText;
                                }
                                var ReferenceAmount = xmlDocumentinner1.GetElementsByTagName("ReferenceAmount");
                                if (ReferenceAmount.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_ReferenceAmount = ReferenceAmount.Item(0).InnerText;
                                }
                                var payableAmount = xmlDocumentinner1.GetElementsByTagName("PayableAmount");
                                if (payableAmount.Count > 0)
                                {
                                    adittionalMonetary.Cs_tag_PayableAmount = payableAmount.Item(0).InnerText;
                                    /*** if (payableAmount.Item(0).Attributes.Count > 0)
                                     {
                                         adittionalMonetary. = payableAmount.Item(0).Attributes.GetNamedItem("currencyID").Value;
                                     }****/
                                }
                                adittionalMonetary.cs_pxInsertar(false, true);

                            }


                            clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adittionalProperty;
                            XmlNodeList LIST2 = xmlDocumentinner.GetElementsByTagName("AdditionalProperty");
                            for (int iii = 0; iii < LIST2.Count; iii++)
                            {
                                adittionalProperty = new clsEntidadDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);
                                adittionalProperty.Cs_pr_Document_Id = idDocumento;
                                var ss = LIST2.Item(iii);
                                XmlDocument xmlDocumentinner1 = new XmlDocument();
                                xmlDocumentinner1.LoadXml(ss.OuterXml);

                                var id = xmlDocumentinner1.GetElementsByTagName("ID");
                                if (id.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_ID = id.Item(0).InnerText;
                                }

                                var value = xmlDocumentinner1.GetElementsByTagName("Value");
                                if (value.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_Value = value.Item(0).InnerText;
                                }
                                var name = xmlDocumentinner1.GetElementsByTagName("Name");
                                if (name.Count > 0)
                                {
                                    adittionalProperty.Cs_tag_Name = name.Item(0).InnerText;
                                }
                                adittionalProperty.cs_pxInsertar(false, true);
                            }
                        }
                        //Additional


                        /* seccion de items ------ añadir items*/
                        var numero_item = 0;
                        // double sub_total = 0.00;
                        List<clsEntidadDocument_Line> Lista_items;
                        List<clsEntidadDocument_Line_TaxTotal> Lista_items_taxtotal;
                        clsEntidadDocument_Line item;
                        XmlNodeList nodeitem;
                        if (documento.Cs_tag_InvoiceTypeCode == "07")
                        {
                            nodeitem = xmlDocument.GetElementsByTagName("CreditNoteLine");

                        }
                        else if (documento.Cs_tag_InvoiceTypeCode == "08")
                        {

                            nodeitem = xmlDocument.GetElementsByTagName("DebitNoteLine");

                        }
                        else
                        {
                            nodeitem = xmlDocument.GetElementsByTagName("InvoiceLine");
                        }

                        //List<clsEntidadDocument_Line_Description> Lista_items_description;
                        List<clsEntidadDocument_Line_PricingReference> Lista_items_princingreference;
                        clsEntidadDocument_Line_Description descripcionItem;

                        var total_items = nodeitem.Count;

                        int i = 0;
                        foreach (XmlNode dat in nodeitem)
                        {
                            i++;
                            numero_item++;
                            //var valor_unitario_item = "";
                            //var valor_total_item = "";
                            //string condition_price = "";
                            Lista_items = new List<clsEntidadDocument_Line>();
                            Lista_items_princingreference = new List<clsEntidadDocument_Line_PricingReference>();
                            Lista_items_taxtotal = new List<clsEntidadDocument_Line_TaxTotal>();


                            item = new clsEntidadDocument_Line(local);
                            item.Cs_pr_Document_Id = idDocumento;
                            XmlDocument xmlItem = new XmlDocument();
                            xmlItem.LoadXml(dat.OuterXml);

                            XmlNodeList ItemDetail = xmlItem.GetElementsByTagName("Item");
                            if (ItemDetail.Count > 0)
                            {
                                foreach (XmlNode items in ItemDetail)
                                {
                                    XmlDocument xmlItemItem = new XmlDocument();
                                    xmlItemItem.LoadXml(items.OuterXml);
                                    XmlNodeList taxItemIdentification = xmlItemItem.GetElementsByTagName("ID");
                                    if (taxItemIdentification.Count > 0)
                                    {
                                        item.Cs_tag_Item_SellersItemIdentification = taxItemIdentification.Item(0).InnerText;
                                    }
                                    /* XmlNodeList taxItemDescription = xmlItemItem.GetElementsByTagName("Description");
                                     int j = 0;
                                     foreach (XmlNode description in taxItemDescription)
                                     {
                                         j++;
                                         descripcionItem = new clsEntidadDocument_Line_Description(local);
                                         descripcionItem.Cs_pr_Document_Line_Id = j.ToString();
                                         descripcionItem.Cs_tag_Description = description.InnerText.Trim();
                                         descripcionItem.cs_pxInsertar(false,true);
                                     }
                                     j = 0;*/
                                }

                            }


                            XmlNodeList ID = xmlItem.GetElementsByTagName("ID");
                            if (ID.Count > 0)
                            {
                                item.Cs_tag_InvoiceLine_ID = ID.Item(0).InnerText;
                            }

                            XmlNodeList InvoicedQuantity;
                            if (documento.Cs_tag_InvoiceTypeCode == "07")
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("CreditedQuantity");

                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }
                            }
                            else if (documento.Cs_tag_InvoiceTypeCode == "08")
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("DebitedQuantity");
                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }
                            }
                            else
                            {
                                InvoicedQuantity = xmlItem.GetElementsByTagName("InvoicedQuantity");
                                if (InvoicedQuantity.Count > 0)
                                {
                                    item.Cs_tag_invoicedQuantity = InvoicedQuantity.Item(0).InnerText;
                                    if (InvoicedQuantity.Item(0).Attributes.Count > 0)
                                    {
                                        item.Cs_tag_InvoicedQuantity_unitCode = InvoicedQuantity.Item(0).Attributes.GetNamedItem("unitCode").Value;
                                    }
                                }
                            }

                            XmlNodeList LineExtensionAmount = xmlItem.GetElementsByTagName("LineExtensionAmount");
                            if (LineExtensionAmount.Count > 0)
                            {
                                item.Cs_tag_LineExtensionAmount_currencyID = LineExtensionAmount.Item(0).InnerText;
                            }

                            XmlNodeList Price = xmlItem.GetElementsByTagName("Price");
                            if (Price.Count > 0)
                            {
                                XmlDocument xmlItemPrice = new XmlDocument();
                                xmlItemPrice.LoadXml(Price.Item(0).OuterXml);
                                XmlNodeList PriceAmount = xmlItemPrice.GetElementsByTagName("PriceAmount");
                                if (PriceAmount.Count > 0)
                                {
                                    item.Cs_tag_Price_PriceAmount = PriceAmount.Item(0).InnerText;
                                }
                            }
                            XmlNodeList AllowanceCharge = xmlItem.GetElementsByTagName("AllowanceCharge");
                            if (AllowanceCharge.Count > 0)
                            {
                                XmlDocument xmlItemAllowanceCharge = new XmlDocument();
                                xmlItemAllowanceCharge.LoadXml(AllowanceCharge.Item(0).OuterXml);
                                XmlNodeList ChargeIndicator = xmlItemAllowanceCharge.GetElementsByTagName("ChargeIndicator");
                                if (ChargeIndicator.Count > 0)
                                {
                                    item.Cs_tag_AllowanceCharge_ChargeIndicator = ChargeIndicator.Item(0).InnerText;
                                }
                                XmlNodeList Amount = xmlItemAllowanceCharge.GetElementsByTagName("Amount");
                                if (Amount.Count > 0)
                                {
                                    item.Cs_tag_AllowanceCharge_Amount = Amount.Item(0).InnerText;
                                }
                            }

                            string idItem = item.cs_pxInsertar(false, true);

                            XmlNodeList ItemDetailes = xmlItem.GetElementsByTagName("Item");
                            if (ItemDetailes.Count > 0)
                            {
                                foreach (XmlNode items in ItemDetailes)
                                {
                                    XmlDocument xmlItemItem = new XmlDocument();
                                    xmlItemItem.LoadXml(items.OuterXml);
                                    XmlNodeList taxItemDescription = xmlItemItem.GetElementsByTagName("Description");
                                    foreach (XmlNode description in taxItemDescription)
                                    {
                                        descripcionItem = new clsEntidadDocument_Line_Description(local);
                                        descripcionItem.Cs_pr_Document_Line_Id = idItem;
                                        descripcionItem.Cs_tag_Description = description.InnerText.Trim().Replace("'", "\"");
                                        descripcionItem.cs_pxInsertar(false, true);
                                    }
                                }

                            }


                            clsEntidadDocument_Line_PricingReference lines_pricing_reference;
                            XmlNodeList PricingReference = xmlItem.GetElementsByTagName("PricingReference");
                            if (PricingReference.Count > 0)
                            {
                                XmlDocument xmlItemItem = new XmlDocument();
                                xmlItemItem.LoadXml(PricingReference.Item(0).OuterXml);
                                XmlNodeList AlternativeConditionPrice = xmlItemItem.GetElementsByTagName("AlternativeConditionPrice");
                                foreach (XmlNode itm in AlternativeConditionPrice)
                                {
                                    XmlDocument xmlItemPricingReference = new XmlDocument();
                                    xmlItemPricingReference.LoadXml(itm.OuterXml);
                                    lines_pricing_reference = new clsEntidadDocument_Line_PricingReference(local);
                                    lines_pricing_reference.Cs_pr_Document_Line_Id = idItem;
                                    XmlNodeList PriceAmount = xmlItemPricingReference.GetElementsByTagName("PriceAmount");
                                    if (PriceAmount.Count > 0)
                                    {
                                        lines_pricing_reference.Cs_tag_PriceAmount_currencyID = PriceAmount.Item(0).InnerText;
                                    }
                                    XmlNodeList PriceTypeCode = xmlItemPricingReference.GetElementsByTagName("PriceTypeCode");
                                    if (PriceTypeCode.Count > 0)
                                    {
                                        lines_pricing_reference.Cs_tag_PriceTypeCode = PriceTypeCode.Item(0).InnerText;
                                    }
                                    lines_pricing_reference.cs_pxInsertar(false, true);
                                }

                            }

                            clsEntidadDocument_Line_TaxTotal taxTotalItem;
                            XmlNodeList TaxTotal = xmlItem.GetElementsByTagName("TaxTotal");
                            if (TaxTotal.Count > 0)
                            {
                                foreach (XmlNode taxitem in TaxTotal)
                                {
                                    taxTotalItem = new clsEntidadDocument_Line_TaxTotal(local);
                                    taxTotalItem.Cs_pr_Document_Line_Id = idItem;
                                    XmlDocument xmlItemTaxtotal = new XmlDocument();
                                    xmlItemTaxtotal.LoadXml(taxitem.OuterXml);
                                    XmlNodeList taxItemAmount = xmlItemTaxtotal.GetElementsByTagName("TaxAmount");
                                    if (taxItemAmount.Count > 0)
                                    {
                                        taxTotalItem.Cs_tag_TaxAmount_currencyID = taxItemAmount.Item(0).InnerText;
                                    }
                                    XmlNodeList itemsubtotal = xmlItemTaxtotal.GetElementsByTagName("TaxSubtotal");
                                    if (itemsubtotal.Count > 0)
                                    {
                                        XmlDocument xmlItemTaxSubtotal = new XmlDocument();
                                        xmlItemTaxSubtotal.LoadXml(itemsubtotal.Item(0).OuterXml);

                                        var subTotalAmount = xmlItemTaxSubtotal.GetElementsByTagName("TaxAmount");
                                        if (subTotalAmount.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxAmount_currencyID = subTotalAmount.Item(0).InnerText;
                                        }
                                        var subTotalID = xmlItemTaxSubtotal.GetElementsByTagName("ID");
                                        if (subTotalID.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = subTotalID.Item(0).InnerText;
                                        }
                                        var subTotalName = xmlItemTaxSubtotal.GetElementsByTagName("Name");
                                        if (subTotalName.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = subTotalName.Item(0).InnerText;
                                        }
                                        var subTotalTaxTypeCode = xmlItemTaxSubtotal.GetElementsByTagName("TaxTypeCode");
                                        if (subTotalTaxTypeCode.Count > 0)
                                        {
                                            taxTotalItem.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = subTotalTaxTypeCode.Item(0).InnerText;
                                        }
                                    }
                                    taxTotalItem.cs_pxInsertar(false, true);

                                }
                            }
                        }
                    }
                    resultado = idDocumento;
                }

            }
            catch (Exception ex)
            {
                string Errror = ex.ToString();
                resultado = "";
            }
            return resultado;
        }
    }

}
