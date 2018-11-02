using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEICONT
{
    public class CargarTxt
    {
        public CargarTxt()
        {

        }
        public static void Procesar()
        {
            //buscar documentos en carpeta documentos_carga
            //almacenar nombres en cada list ->facturas boletas notas de credito notas de debito

            //verificar facturas existentes
            //cortar nombre _ y sacar ruc
            //con el ruc buscar la empresa.
            //y la configuracion de la base de datos correspondiente.
            //crear cadena de conexion.
            //leer el documento txt e insertar en bd los registros encontrados (Método se le envia ruta documento)
            //si ha sido procesado correctamente
            //enviar a sunat                    

            //sino hay empresa.
            //no hacer nada escrbir en log y pasar al siguiente //continue.

            //realizar lo mismo para los demas comprobantes en caso sea boleta no enviar a sunat ni notas asociadas a boletas

            string RutaInstalacion = new clsBaseConfiguracion().cs_prRutaCargaTextoPlano;
            List<string> lista_rutas_facturas = new List<string>();
            List<string> lista_rutas_boletas = new List<string>();
            List<string> lista_rutas_notascredito = new List<string>();
            List<string> lista_rutas_notasdebito = new List<string>();

            string[] dirs = Directory.GetFiles(RutaInstalacion, "*.txt");
            if (dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    string nombreArchivo = Path.GetFileName(dir);
                    if (nombreArchivo.Contains("RUC"))
                    {
                        string RUC = nombreArchivo.Substring(3, 11);
                        string tipoDocumento = nombreArchivo.Substring(15, 2);
                        clsEntityDeclarant empresa = new clsEntityDeclarant().cs_pxObtenerPorRuc(RUC);

                        if (empresa != null)
                        {
                            switch (tipoDocumento)
                            {
                                case "01":
                                    lista_rutas_facturas.Add(dir);
                                    break;
                                case "03":
                                    lista_rutas_boletas.Add(dir);
                                    break;
                                case "07":
                                    lista_rutas_notascredito.Add(dir);
                                    break;
                                case "08":
                                    lista_rutas_notasdebito.Add(dir);
                                    break;
                            }
                            // dir
                            //existe empresa -> agregar ruta documento a lista segun tipo de comprobante;
                        }
                    }
                }

                if (lista_rutas_boletas.Count > 0)
                {
                    procesarBoletas(lista_rutas_boletas);
                }
                if (lista_rutas_facturas.Count > 0)
                {
                    procesarFacturas(lista_rutas_facturas);
                }
                if (lista_rutas_notascredito.Count > 0)
                {
                    procesarNotasCredito(lista_rutas_notascredito);
                }
                if (lista_rutas_notasdebito.Count > 0)
                {
                    procesarNotasDebito(lista_rutas_notasdebito);
                }
            }
        }

        public static void procesarBoletas(List<string> lista_boletas)
        {
            //procesar archivos
            foreach (string ruta in lista_boletas)
            {
                //obtener ruc de cada uno 
                //obtener configuracion empresa 
                //configuracion base de datos
                string nombreArchivo = Path.GetFileName(ruta);
                string RUC = nombreArchivo.Substring(3, 11);
                clsEntityDeclarant empresa = new clsEntityDeclarant().cs_pxObtenerPorRuc(RUC);
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(empresa.Cs_pr_Declarant_Id);             
                bool respuesta = leerTxtBoleta(ruta, local);
                //leer documento txt y almacenar en base de datos
                if (respuesta == true)
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch
                    {
                    }
                }
            }
        }
        private static bool leerTxtBoleta(string ruta, clsEntityDatabaseLocal local)
        {
            bool retorno = false;
            try
            {
                StreamReader sr0 = new StreamReader(ruta);
                string documento = sr0.ReadToEnd();
                sr0.Close();
                string[] partes_tablas = documento.Split('!');
                //tabla1 //cabecera

                string[] partes_tabla1 = partes_tablas[0].Split(']');//listo
                string[] datos_generales = partes_tabla1[0].Split('|');//listo
                string[] datos_emisor = partes_tabla1[1].Split('|');//listo
                string[] datos_receptor = partes_tabla1[2].Split('|');//listo

                //tabla2 //taxtotal
                string[] partes_tabla2 = partes_tablas[1].Split(']');//listo
                string[] igv_totales = partes_tabla2[0].Split('|');//listo
                string[] isc_totales = partes_tabla2[1].Split('|');//listo
                string[] otros_totales = partes_tabla2[2].Split('|');//listo
                                                                     //tabla 3 //legalMonetaryTotal
                string[] partes_tabla3 = partes_tablas[2].Split(']');//Listo
                string[] legal_monetary = partes_tabla3[0].Split('|');//Listo
                                                                      //tabla 4 additionalMonetaryTotal
                string[] partes_tabla4 = partes_tablas[3].Split(']');//Listo
                string[] op_gravadas = partes_tabla4[0].Split('|');//Listo
                string[] op_inafectas = partes_tabla4[1].Split('|');//Listo
                string[] op_exoneradas = partes_tabla4[2].Split('|');//Listo
                string[] op_gratuitas = partes_tabla4[3].Split('|');//Listo
                string[] descuentos = partes_tabla4[4].Split('|');//Listo
                string[] percepcion = partes_tabla4[5].Split('|');//Listo

                //tabla 5 additionalProperty
                List<string[]> additional_propertys = new List<string[]>();
                string[] partes_tabla5 = partes_tablas[4].Split(']');
                for (int i = 0; i <= partes_tabla5.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla5[i].Split('|');
                    additional_propertys.Add(min_partes);
                }

                //tabla 6 lineas
                List<string[]> lineas = new List<string[]>();
                string[] partes_tabla6 = partes_tablas[5].Split(']');
                for (int i = 0; i <= partes_tabla6.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla6[i].Split('|');
                    lineas.Add(min_partes);
                }

                //Insertar todos los datos en la bd correspondiente
                clsEntityDocument cabecera = new clsEntityDocument(local);
                cabecera.Cs_pr_Document_Id = Guid.NewGuid().ToString();
                cabecera.Cs_pr_EstadoSCC = "2";
                cabecera.Cs_pr_EstadoSUNAT = "2";
                cabecera.Cs_TipoCambio = datos_generales[8];
                cabecera.Cs_tag_ID = datos_generales[1];
                cabecera.Cs_tag_InvoiceTypeCode = datos_generales[0];
                cabecera.Cs_tag_IssueDate = datos_generales[2];
                cabecera.Cs_tag_DocumentCurrencyCode = datos_generales[3];
                cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos_emisor[0];
                cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos_emisor[1];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos_emisor[2];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos_emisor[3];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos_emisor[4];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos_emisor[5];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos_emisor[6];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos_emisor[7];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos_emisor[8];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos_emisor[9];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos_emisor[10];
                cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos_receptor[0];
                cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos_receptor[1];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos_receptor[2];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos_receptor[3];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = legal_monetary[0];
                cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = legal_monetary[1];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = legal_monetary[2];
                string idCabecera =  cabecera.cs_pxInsertar(false,false);
                if (datos_generales[6].Trim() != "" && datos_generales[7].Trim() != "")
                {
                    clsEntityDocument_AdditionalDocumentReference docref = new clsEntityDocument_AdditionalDocumentReference(local);
                    docref.Cs_pr_Document_AdditionalDocumentReference_Id = Guid.NewGuid().ToString();
                    docref.Cs_pr_Document_Id = idCabecera;
                    docref.Cs_tag_DocumentTypeCode = datos_generales[7];
                    docref.Cs_tag_AdditionalDocumentReference_ID = datos_generales[6];
                    docref.cs_pxInsertar(false,true);
                }
                if (datos_generales[4].Trim() != "" && datos_generales[5].Trim() != "")
                {
                    clsEntityDocument_DespatchDocumentReference despatch = new clsEntityDocument_DespatchDocumentReference(local);
                    despatch.Cs_pr_Document_DespatchDocumentReference_Id = Guid.NewGuid().ToString();
                    despatch.Cs_pr_Document_Id = idCabecera;
                    despatch.Cs_tag_DespatchDocumentReference_ID = datos_generales[4];
                    despatch.Cs_tag_DocumentTypeCode = datos_generales[5];
                    despatch.cs_pxInsertar(false,true);
                }
                if (igv_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_igv = new clsEntityDocument_TaxTotal(local);
                    taxtotal_igv.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_igv.Cs_pr_Document_Id = idCabecera;
                    taxtotal_igv.Cs_tag_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = igv_totales[1];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = igv_totales[2];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = igv_totales[3];
                    taxtotal_igv.cs_pxInsertar(false,true);
                }

                if (isc_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_isc = new clsEntityDocument_TaxTotal(local);
                    taxtotal_isc.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_isc.Cs_pr_Document_Id = idCabecera;
                    taxtotal_isc.Cs_tag_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = isc_totales[1];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = isc_totales[2];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = isc_totales[3];
                    taxtotal_isc.cs_pxInsertar(false,true);
                }
                if (otros_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_otros = new clsEntityDocument_TaxTotal(local);
                    taxtotal_otros.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_otros.Cs_pr_Document_Id = idCabecera;
                    taxtotal_otros.Cs_tag_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = otros_totales[1];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = otros_totales[2];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = otros_totales[3];
                    taxtotal_otros.cs_pxInsertar(false,true);
                }
                //additionalmoentary
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation adinf = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local);
                adinf.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idCabecera + "1";
                adinf.Cs_pr_Document_Id = idCabecera;
                string idAdditionalInformation = adinf.cs_pxInsertar(false,true);
                if (op_gravadas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gravadas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gravadas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_inafectas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_inafectas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_inafectas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_exoneradas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_exoneradas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_exoneradas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_gratuitas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gratuitas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gratuitas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (descuentos[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = descuentos[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = descuentos[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (percepcion[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = percepcion[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = percepcion[2];
                    amt.Cs_tag_Percent = percepcion[3];
                    amt.Cs_tag_ReferenceAmount = percepcion[1];
                    amt.Cs_tag_SchemeID = percepcion[4];
                    amt.Cs_tag_TotalAmount = percepcion[5];
                    amt.cs_pxInsertar(false,true);
                }               
                //additionalinformation
                foreach (string[] values in additional_propertys)
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adprop = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = Guid.NewGuid().ToString();
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    adprop.Cs_tag_ID = values[0];
                    adprop.Cs_tag_Name = "";
                    adprop.Cs_tag_Value = values[1];
                    adprop.cs_pxInsertar(false,true);
                }
                //items
                int j = 0;
                foreach (string[] values in lineas)
                {
                    j++;
                    clsEntityDocument_Line line = new clsEntityDocument_Line(local);
                    line.Cs_pr_Document_Line_Id = Guid.NewGuid().ToString();
                    line.Cs_pr_Document_Id = idCabecera;
                    line.Cs_tag_InvoiceLine_ID = j.ToString();
                    line.Cs_tag_AllowanceCharge_Amount = values[7];
                    line.Cs_tag_AllowanceCharge_ChargeIndicator = values[6];
                    line.Cs_tag_invoicedQuantity = values[2];
                    line.Cs_tag_InvoicedQuantity_unitCode = values[1];
                    line.Cs_tag_Item_SellersItemIdentification = values[18];
                    line.Cs_tag_LineExtensionAmount_currencyID = values[3];
                    line.Cs_tag_Price_PriceAmount = values[19];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID = values[4];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode = values[5];
                   string idLinea = line.cs_pxInsertar(false,true);

                    if (values[4].Trim() != "" && values[5].Trim() != "")
                    {
                        clsEntityDocument_Line_PricingReference linepriref = new clsEntityDocument_Line_PricingReference(local);
                        linepriref.Cs_pr_Document_Line_PricingReference_Id = Guid.NewGuid().ToString();
                        linepriref.Cs_pr_Document_Line_Id = idLinea;
                        linepriref.Cs_tag_PriceAmount_currencyID = values[4];
                        linepriref.Cs_tag_PriceTypeCode = values[5];
                        linepriref.cs_pxInsertar(false,true);
                    }

                    string descripcion = values[0].Replace("^", " \r ");
                    clsEntityDocument_Line_Description linedesc = new clsEntityDocument_Line_Description(local);
                    linedesc.Cs_pr_Document_Line_Description_Id = Guid.NewGuid().ToString();
                    linedesc.Cs_pr_Document_Line_Id = idLinea;
                    linedesc.Cs_tag_Description = descripcion;
                    linedesc.cs_pxInsertar(false,true);
                    if (values[8].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_igv = new clsEntityDocument_Line_TaxTotal(local);
                        tax_igv.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_igv.Cs_pr_Document_Line_Id = idLinea;
                        tax_igv.Cs_tag_TaxAmount_currencyID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = values[9];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[10];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[11];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[12];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TierRange = "";
                        tax_igv.cs_pxInsertar(false,true);
                    }
                    if (values[13].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_isc = new clsEntityDocument_Line_TaxTotal(local);
                        tax_isc.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_isc.Cs_pr_Document_Line_Id = idLinea;
                        tax_isc.Cs_tag_TaxAmount_currencyID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = "";
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[15];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[16];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[17];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TierRange = values[14];
                        tax_isc.cs_pxInsertar(false,true);
                    }
                }
                retorno = true;
            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }

        public static string procesarFacturas(List<string> lista_facturas)
        {//obtener todo el texto del archivo
         //Dividir
         //procesar archivos
            foreach (string ruta in lista_facturas)
            {
                //obtener ruc de cada uno 
                //obtener configuracion empresa 
                //configuracion base de datos
                string nombreArchivo = Path.GetFileName(ruta);
                string RUC = nombreArchivo.Substring(3, 11);
                clsEntityDeclarant empresa = new clsEntityDeclarant().cs_pxObtenerPorRuc(RUC);
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(empresa.Cs_pr_Declarant_Id);             
                bool respuesta = leerTxtFactura(ruta, local);
                if (respuesta == true)
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch
                    {

                    }
                   
                }
                //leer documento txt y almacenar en base de datos
            }
            return "";
        }

        private static bool leerTxtFactura(string ruta, clsEntityDatabaseLocal local)
        {
            bool retorno = false;
            try
            {
                StreamReader sr0 = new StreamReader(ruta);
                string documento = sr0.ReadToEnd();
                sr0.Close();
                string[] partes_tablas = documento.Split('!');
                //tabla1 //cabecera

                string[] partes_tabla1 = partes_tablas[0].Split(']');//listo
                string[] datos_generales = partes_tabla1[0].Split('|');//listo
                string[] datos_emisor = partes_tabla1[1].Split('|');//listo
                string[] datos_receptor = partes_tabla1[2].Split('|');//listo

                //tabla2 //taxtotal
                string[] partes_tabla2 = partes_tablas[1].Split(']');//listo
                string[] igv_totales = partes_tabla2[0].Split('|');//listo
                string[] isc_totales = partes_tabla2[1].Split('|');//listo
                string[] otros_totales = partes_tabla2[2].Split('|');//listo
                                                                     //tabla 3 //legalMonetaryTotal
                string[] partes_tabla3 = partes_tablas[2].Split(']');//Listo
                string[] legal_monetary = partes_tabla3[0].Split('|');//Listo
                                                                      //tabla 4 additionalMonetaryTotal
                string[] partes_tabla4 = partes_tablas[3].Split(']');//Listo
                string[] op_gravadas = partes_tabla4[0].Split('|');//Listo
                string[] op_inafectas = partes_tabla4[1].Split('|');//Listo
                string[] op_exoneradas = partes_tabla4[2].Split('|');//Listo
                string[] op_gratuitas = partes_tabla4[3].Split('|');//Listo
                string[] descuentos = partes_tabla4[4].Split('|');//Listo
                string[] percepcion = partes_tabla4[5].Split('|');//Listo
                string[] detraccion = partes_tabla4[6].Split('|');//Listo

                //tabla 5 additionalProperty
                List<string[]> additional_propertys = new List<string[]>();
                string[] partes_tabla5 = partes_tablas[4].Split(']');
                for (int i = 0; i <= partes_tabla5.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla5[i].Split('|');
                    additional_propertys.Add(min_partes);
                }

                //tabla 6 lineas
                List<string[]> lineas = new List<string[]>();
                string[] partes_tabla6 = partes_tablas[5].Split(']');
                for (int i = 0; i <= partes_tabla6.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla6[i].Split('|');
                    lineas.Add(min_partes);
                }

                //Insertar todos los datos en la bd correspondiente
                clsEntityDocument cabecera = new clsEntityDocument(local);
                cabecera.Cs_pr_Document_Id = Guid.NewGuid().ToString();
                cabecera.Cs_pr_EstadoSCC = "2";
                cabecera.Cs_pr_EstadoSUNAT = "2";
                cabecera.Cs_tag_ID = datos_generales[1];
                cabecera.Cs_tag_InvoiceTypeCode = datos_generales[0];
                cabecera.Cs_tag_IssueDate = datos_generales[2];
                cabecera.Cs_tag_DocumentCurrencyCode = datos_generales[3];
                cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos_emisor[0];
                cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos_emisor[1];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos_emisor[2];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos_emisor[3];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos_emisor[4];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos_emisor[5];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos_emisor[6];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos_emisor[7];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos_emisor[8];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos_emisor[9];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos_emisor[10];
                cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos_receptor[0];
                cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos_receptor[1];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos_receptor[2];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos_receptor[3];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = legal_monetary[0];
                cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = legal_monetary[1];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = legal_monetary[2];
                string idCabecera = cabecera.cs_pxInsertar(false,false);
                if (datos_generales[6].Trim() != "" && datos_generales[7].Trim() != "")
                {
                    clsEntityDocument_AdditionalDocumentReference docref = new clsEntityDocument_AdditionalDocumentReference(local);
                    docref.Cs_pr_Document_AdditionalDocumentReference_Id = Guid.NewGuid().ToString();
                    docref.Cs_pr_Document_Id = idCabecera;
                    docref.Cs_tag_DocumentTypeCode = datos_generales[7];
                    docref.Cs_tag_AdditionalDocumentReference_ID = datos_generales[6];
                    docref.cs_pxInsertar(false,true);
                }
                if (datos_generales[4].Trim() != "" && datos_generales[5].Trim() != "")
                {
                    clsEntityDocument_DespatchDocumentReference despatch = new clsEntityDocument_DespatchDocumentReference(local);
                    despatch.Cs_pr_Document_DespatchDocumentReference_Id = Guid.NewGuid().ToString();
                    despatch.Cs_pr_Document_Id = idCabecera;
                    despatch.Cs_tag_DespatchDocumentReference_ID = datos_generales[4];
                    despatch.Cs_tag_DocumentTypeCode = datos_generales[5];
                    despatch.cs_pxInsertar(false,true);
                }
                if (igv_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_igv = new clsEntityDocument_TaxTotal(local);
                    taxtotal_igv.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_igv.Cs_pr_Document_Id = idCabecera;
                    taxtotal_igv.Cs_tag_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = igv_totales[1];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = igv_totales[2];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = igv_totales[3];
                    taxtotal_igv.cs_pxInsertar(false,true);
                }

                if (isc_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_isc = new clsEntityDocument_TaxTotal(local);
                    taxtotal_isc.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_isc.Cs_pr_Document_Id = idCabecera;
                    taxtotal_isc.Cs_tag_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = isc_totales[1];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = isc_totales[2];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = isc_totales[3];
                    taxtotal_isc.cs_pxInsertar(false,true);
                }
                if (otros_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_otros = new clsEntityDocument_TaxTotal(local);
                    taxtotal_otros.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_otros.Cs_pr_Document_Id = idCabecera;
                    taxtotal_otros.Cs_tag_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = otros_totales[1];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = otros_totales[2];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = otros_totales[3];
                    taxtotal_otros.cs_pxInsertar(false,true);
                }
                //additionalmoentary
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation adinf = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local);
                adinf.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idCabecera + "1";
                adinf.Cs_pr_Document_Id = idCabecera;
                string idAdditionalInformation = adinf.cs_pxInsertar(false,true);
                if (op_gravadas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gravadas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gravadas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_inafectas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_inafectas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_inafectas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_exoneradas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_exoneradas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_exoneradas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_gratuitas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gratuitas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gratuitas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (descuentos[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = descuentos[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = descuentos[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (percepcion[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = percepcion[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = percepcion[2];
                    amt.Cs_tag_Percent = percepcion[3];
                    amt.Cs_tag_ReferenceAmount = percepcion[1];
                    amt.Cs_tag_SchemeID = percepcion[4];
                    amt.Cs_tag_TotalAmount = percepcion[5];
                    amt.cs_pxInsertar(false,true);
                }
                if (detraccion[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = detraccion[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = detraccion[1];
                    amt.Cs_tag_Percent = detraccion[2];
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                //additionalinformation
                foreach (string[] values in additional_propertys)
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adprop = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = Guid.NewGuid().ToString();
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    adprop.Cs_tag_ID = values[0];
                    adprop.Cs_tag_Name = "";
                    adprop.Cs_tag_Value = values[1];
                    adprop.cs_pxInsertar(false,true);
                }
                //items
                int j = 0;
                foreach (string[] values in lineas)
                {
                    j++;
                    clsEntityDocument_Line line = new clsEntityDocument_Line(local);
                    line.Cs_pr_Document_Line_Id = Guid.NewGuid().ToString();
                    line.Cs_pr_Document_Id = idCabecera;
                    line.Cs_tag_InvoiceLine_ID = j.ToString();
                    line.Cs_tag_AllowanceCharge_Amount = values[7];
                    line.Cs_tag_AllowanceCharge_ChargeIndicator = values[6];
                    line.Cs_tag_invoicedQuantity = values[2];
                    line.Cs_tag_InvoicedQuantity_unitCode = values[1];
                    line.Cs_tag_Item_SellersItemIdentification = values[18];
                    line.Cs_tag_LineExtensionAmount_currencyID = values[3];
                    line.Cs_tag_Price_PriceAmount = values[19];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID = values[4];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode = values[5];
                   string lineaId = line.cs_pxInsertar(false,true);

                    if (values[4].Trim() != "" && values[5].Trim() != "")
                    {
                        clsEntityDocument_Line_PricingReference linepriref = new clsEntityDocument_Line_PricingReference(local);
                        linepriref.Cs_pr_Document_Line_PricingReference_Id = Guid.NewGuid().ToString();
                        linepriref.Cs_pr_Document_Line_Id = lineaId;
                        linepriref.Cs_tag_PriceAmount_currencyID = values[4];
                        linepriref.Cs_tag_PriceTypeCode = values[5];
                        linepriref.cs_pxInsertar(false,true);
                    }

                    string descripcion = values[0].Replace("^", " \r ");
                    clsEntityDocument_Line_Description linedesc = new clsEntityDocument_Line_Description(local);
                    linedesc.Cs_pr_Document_Line_Description_Id = Guid.NewGuid().ToString();
                    linedesc.Cs_pr_Document_Line_Id = lineaId;
                    linedesc.Cs_tag_Description = descripcion;
                    linedesc.cs_pxInsertar(false,true);
                    if (values[8].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_igv = new clsEntityDocument_Line_TaxTotal(local);
                        tax_igv.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_igv.Cs_pr_Document_Line_Id = lineaId;
                        tax_igv.Cs_tag_TaxAmount_currencyID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = values[9];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[10];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[11];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[12];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TierRange = "";
                        tax_igv.cs_pxInsertar(false,true);
                    }
                    if (values[13].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_isc = new clsEntityDocument_Line_TaxTotal(local);
                        tax_isc.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_isc.Cs_pr_Document_Line_Id = lineaId;
                        tax_isc.Cs_tag_TaxAmount_currencyID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = "";
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[15];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[16];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[17];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TierRange = values[14];
                        tax_isc.cs_pxInsertar(false,true);
                    }
                }

                new clsBaseSunat(local).cs_pxEnviarCE(idCabecera, false);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }           
            return retorno;
        }

        public static void procesarNotasCredito(List<string> lista_notasCredito)
        {
            foreach (string ruta in lista_notasCredito)
            {
                //obtener ruc de cada uno 
                //obtener configuracion empresa 
                //configuracion base de datos
                string nombreArchivo = Path.GetFileName(ruta);
                string RUC = nombreArchivo.Substring(3, 11);
                clsEntityDeclarant empresa = new clsEntityDeclarant().cs_pxObtenerPorRuc(RUC);
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(empresa.Cs_pr_Declarant_Id);
                bool respuesta = leerTxtCredito(ruta, local);
                //leer documento txt y almacenar en base de datos
                if (respuesta == true)
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static bool leerTxtCredito(string ruta, clsEntityDatabaseLocal local)
        {
            bool retorno = false;
            try
            {
                StreamReader sr0 = new StreamReader(ruta);
                string documento = sr0.ReadToEnd();
                sr0.Close();
                string[] partes_tablas = documento.Split('!');
                //tabla1 //cabecera

                string[] partes_tabla1 = partes_tablas[0].Split(']');//listo
                string[] datos_generales = partes_tabla1[0].Split('|');//listo
                string[] datos_emisor = partes_tabla1[1].Split('|');//listo
                string[] datos_receptor = partes_tabla1[2].Split('|');//listo

                //tabla2 //taxtotal
                string[] partes_tabla2 = partes_tablas[1].Split(']');//listo
                string[] igv_totales = partes_tabla2[0].Split('|');//listo
                string[] isc_totales = partes_tabla2[1].Split('|');//listo
                string[] otros_totales = partes_tabla2[2].Split('|');//listo

                //tabla 3 //legalMonetaryTotal
                string[] partes_tabla3 = partes_tablas[2].Split(']');//Listo
                string[] legal_monetary = partes_tabla3[0].Split('|');//Listo

                //tabla 4 additionalMonetaryTotal
                string[] partes_tabla4 = partes_tablas[3].Split(']');//Listo
                string[] op_gravadas = partes_tabla4[0].Split('|');//Listo
                string[] op_inafectas = partes_tabla4[1].Split('|');//Listo
                string[] op_exoneradas = partes_tabla4[2].Split('|');//Listo
                string[] op_gratuitas = partes_tabla4[3].Split('|');//Listo
                string[] descuentos = partes_tabla4[4].Split('|');//Listo
                string[] percepcion = partes_tabla4[5].Split('|');//Listo

                //tabla 5 additionalProperty
                List<string[]> additional_propertys = new List<string[]>();
                string[] partes_tabla5 = partes_tablas[4].Split(']');
                for (int i = 0; i <= partes_tabla5.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla5[i].Split('|');
                    additional_propertys.Add(min_partes);
                }
                //tabla 6 lineas
                List<string[]> lineas = new List<string[]>();
                string[] partes_tabla6 = partes_tablas[5].Split(']');
                for (int i = 0; i <= partes_tabla6.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla6[i].Split('|');
                    lineas.Add(min_partes);
                }

                //Insertar todos los datos en la bd correspondiente
                clsEntityDocument cabecera = new clsEntityDocument(local);
                cabecera.Cs_pr_Document_Id = Guid.NewGuid().ToString();
                cabecera.Cs_pr_EstadoSCC = "2";
                cabecera.Cs_pr_EstadoSUNAT = "2";
                
                cabecera.Cs_tag_ID = datos_generales[0];
                cabecera.Cs_tag_IssueDate = datos_generales[1];
                cabecera.Cs_tag_InvoiceTypeCode = datos_generales[2];             
                cabecera.Cs_tag_DocumentCurrencyCode = datos_generales[3];
                cabecera.Cs_tag_Discrepancy_ReferenceID = datos_generales[8];
                cabecera.Cs_tag_Discrepancy_ResponseCode = datos_generales[9];
                cabecera.Cs_tag_Discrepancy_Description = datos_generales[10];
                cabecera.Cs_tag_BillingReference_ID = datos_generales[11];
                cabecera.Cs_tag_BillingReference_DocumentTypeCode = datos_generales[12];
                cabecera.Cs_TipoCambio = datos_generales[13];
                cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos_emisor[0];
                cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos_emisor[1];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos_emisor[2];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos_emisor[3];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos_emisor[4];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos_emisor[5];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos_emisor[6];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos_emisor[7];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos_emisor[8];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos_emisor[9];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos_emisor[10];
                cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos_receptor[0];
                cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos_receptor[1];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos_receptor[2];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos_receptor[3];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = legal_monetary[0];
                cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = legal_monetary[1];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = legal_monetary[2];
                string idCabecera = cabecera.cs_pxInsertar(false,false);
                if (datos_generales[6].Trim() != "" && datos_generales[7].Trim() != "")
                {
                    clsEntityDocument_AdditionalDocumentReference docref = new clsEntityDocument_AdditionalDocumentReference(local);
                    docref.Cs_pr_Document_AdditionalDocumentReference_Id = Guid.NewGuid().ToString();
                    docref.Cs_pr_Document_Id = idCabecera;
                    docref.Cs_tag_DocumentTypeCode = datos_generales[7];
                    docref.Cs_tag_AdditionalDocumentReference_ID = datos_generales[6];
                    docref.cs_pxInsertar(false,true);
                }
                if (datos_generales[4].Trim() != "" && datos_generales[5].Trim() != "")
                {
                    clsEntityDocument_DespatchDocumentReference despatch = new clsEntityDocument_DespatchDocumentReference(local);
                    despatch.Cs_pr_Document_DespatchDocumentReference_Id = Guid.NewGuid().ToString();
                    despatch.Cs_pr_Document_Id = idCabecera;
                    despatch.Cs_tag_DespatchDocumentReference_ID = datos_generales[4];
                    despatch.Cs_tag_DocumentTypeCode = datos_generales[5];
                    despatch.cs_pxInsertar(false,true);
                }
                if (igv_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_igv = new clsEntityDocument_TaxTotal(local);
                    taxtotal_igv.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_igv.Cs_pr_Document_Id = idCabecera;
                    taxtotal_igv.Cs_tag_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = igv_totales[1];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = igv_totales[2];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = igv_totales[3];
                    taxtotal_igv.cs_pxInsertar(false,true);
                }

                if (isc_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_isc = new clsEntityDocument_TaxTotal(local);
                    taxtotal_isc.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_isc.Cs_pr_Document_Id = idCabecera;
                    taxtotal_isc.Cs_tag_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = isc_totales[1];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = isc_totales[2];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = isc_totales[3];
                    taxtotal_isc.cs_pxInsertar(false,true);
                }
                if (otros_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_otros = new clsEntityDocument_TaxTotal(local);
                    taxtotal_otros.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_otros.Cs_pr_Document_Id = idCabecera;
                    taxtotal_otros.Cs_tag_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = otros_totales[1];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = otros_totales[2];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = otros_totales[3];
                    taxtotal_otros.cs_pxInsertar(false,true);
                }
                //additionalmoentary
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation adinf = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local);
                adinf.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idCabecera + "1";
                adinf.Cs_pr_Document_Id = idCabecera;
                string idAdditionalInformation = adinf.cs_pxInsertar(false,true);
                if (op_gravadas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gravadas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gravadas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false,true);
                }
                if (op_inafectas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_inafectas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_inafectas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (op_exoneradas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_exoneradas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_exoneradas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (op_gratuitas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gratuitas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gratuitas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (descuentos[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = descuentos[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = descuentos[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (percepcion[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = percepcion[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = percepcion[2];
                    amt.Cs_tag_Percent = percepcion[3];
                    amt.Cs_tag_ReferenceAmount = percepcion[1];
                    amt.Cs_tag_SchemeID = percepcion[4];
                    amt.Cs_tag_TotalAmount = percepcion[5];
                    amt.cs_pxInsertar(false, true);
                }
                //additionalinformation
                foreach (string[] values in additional_propertys)
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adprop = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = Guid.NewGuid().ToString();
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    adprop.Cs_tag_ID = values[0];
                    adprop.Cs_tag_Name = "";
                    adprop.Cs_tag_Value = values[1];
                    adprop.cs_pxInsertar(false,true);
                }
                //items
                int j = 0;
                foreach (string[] values in lineas)
                {
                    j++;
                    clsEntityDocument_Line line = new clsEntityDocument_Line(local);
                    line.Cs_pr_Document_Line_Id = Guid.NewGuid().ToString();
                    line.Cs_pr_Document_Id = idCabecera;
                    line.Cs_tag_InvoiceLine_ID = j.ToString();
                    //line.Cs_tag_AllowanceCharge_Amount = values[8];
                    //line.Cs_tag_AllowanceCharge_ChargeIndicator = values[7];
                    line.Cs_tag_invoicedQuantity = values[2];
                    line.Cs_tag_InvoicedQuantity_unitCode = values[1];
                    line.Cs_tag_Item_SellersItemIdentification = values[16];
                    line.Cs_tag_LineExtensionAmount_currencyID = values[3];
                    line.Cs_tag_Price_PriceAmount = values[17];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID = values[4];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode = values[5];
                   string idLinea = line.cs_pxInsertar(false,true);

                    if (values[4].Trim() != "" && values[5].Trim() != "")
                    {
                        clsEntityDocument_Line_PricingReference linepriref = new clsEntityDocument_Line_PricingReference(local);
                        linepriref.Cs_pr_Document_Line_PricingReference_Id = Guid.NewGuid().ToString();
                        linepriref.Cs_pr_Document_Line_Id = idLinea;
                        linepriref.Cs_tag_PriceAmount_currencyID = values[4];
                        linepriref.Cs_tag_PriceTypeCode = values[5];
                        linepriref.cs_pxInsertar(false, true);
                    }

                    string descripcion = values[0].Replace("^", " \r ");
                    clsEntityDocument_Line_Description linedesc = new clsEntityDocument_Line_Description(local);
                    linedesc.Cs_pr_Document_Line_Description_Id = Guid.NewGuid().ToString();
                    linedesc.Cs_pr_Document_Line_Id = idLinea;
                    linedesc.Cs_tag_Description = descripcion;
                    linedesc.cs_pxInsertar(false, true);
                    if (values[6].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_igv = new clsEntityDocument_Line_TaxTotal(local);
                        tax_igv.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_igv.Cs_pr_Document_Line_Id = idLinea;
                        tax_igv.Cs_tag_TaxAmount_currencyID = values[6];
                        tax_igv.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[6];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = values[7];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[9];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[10];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TierRange = "";
                        tax_igv.cs_pxInsertar(false, true);
                    }
                    if (values[11].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_isc = new clsEntityDocument_Line_TaxTotal(local);
                        tax_isc.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_isc.Cs_pr_Document_Line_Id = idLinea;
                        tax_isc.Cs_tag_TaxAmount_currencyID = values[11];
                        tax_isc.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[11];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = "";
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[14];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[15];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TierRange = values[12];
                        tax_isc.cs_pxInsertar(false, true);
                    }
                }
                if (cabecera.Cs_tag_BillingReference_DocumentTypeCode == "01")
                {
                    new clsBaseSunat(local).cs_pxEnviarCE(idCabecera, false);
                }
                retorno = true;

            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }

        public static void procesarNotasDebito(List<string> lista_notasDebito)
        {
            foreach (string ruta in lista_notasDebito)
            {
                //obtener ruc de cada uno 
                //obtener configuracion empresa 
                //configuracion base de datos
                string nombreArchivo = Path.GetFileName(ruta);
                string RUC = nombreArchivo.Substring(3, 11);
                clsEntityDeclarant empresa = new clsEntityDeclarant().cs_pxObtenerPorRuc(RUC);
                clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(empresa.Cs_pr_Declarant_Id);
                bool respuesta = leerTxtDebito(ruta, local);
                //leer documento txt y almacenar en base de datos
                if (respuesta == true)
                {
                    try
                    {
                        File.Delete(ruta);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static bool leerTxtDebito(string ruta, clsEntityDatabaseLocal local)
        {
            bool retorno = false;
            try
            {
                StreamReader sr0 = new StreamReader(ruta);
                string documento = sr0.ReadToEnd();
                sr0.Close();
                string[] partes_tablas = documento.Split('!');
                //tabla1 //cabecera

                string[] partes_tabla1 = partes_tablas[0].Split(']');//listo
                string[] datos_generales = partes_tabla1[0].Split('|');//listo
                string[] datos_emisor = partes_tabla1[1].Split('|');//listo
                string[] datos_receptor = partes_tabla1[2].Split('|');//listo

                //tabla2 //taxtotal
                string[] partes_tabla2 = partes_tablas[1].Split(']');//listo
                string[] igv_totales = partes_tabla2[0].Split('|');//listo
                string[] isc_totales = partes_tabla2[1].Split('|');//listo
                string[] otros_totales = partes_tabla2[2].Split('|');//listo

                //tabla 3 //legalMonetaryTotal
                string[] partes_tabla3 = partes_tablas[2].Split(']');//Listo
                string[] legal_monetary = partes_tabla3[0].Split('|');//Listo

                //tabla 4 additionalMonetaryTotal
                string[] partes_tabla4 = partes_tablas[3].Split(']');//Listo
                string[] op_gravadas = partes_tabla4[0].Split('|');//Listo
                string[] op_inafectas = partes_tabla4[1].Split('|');//Listo
                string[] op_exoneradas = partes_tabla4[2].Split('|');//Listo
                string[] op_gratuitas = partes_tabla4[3].Split('|');//Listo
                string[] descuentos = partes_tabla4[4].Split('|');//Listo
                string[] percepcion = partes_tabla4[5].Split('|');//Listo

                //tabla 5 additionalProperty
                List<string[]> additional_propertys = new List<string[]>();
                string[] partes_tabla5 = partes_tablas[4].Split(']');
                for (int i = 0; i <= partes_tabla5.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla5[i].Split('|');
                    additional_propertys.Add(min_partes);
                }
                //tabla 6 lineas
                List<string[]> lineas = new List<string[]>();
                string[] partes_tabla6 = partes_tablas[5].Split(']');
                for (int i = 0; i <= partes_tabla6.Length - 2; i++)
                {
                    string[] min_partes = partes_tabla6[i].Split('|');
                    lineas.Add(min_partes);
                }

                //Insertar todos los datos en la bd correspondiente
                clsEntityDocument cabecera = new clsEntityDocument(local);
                cabecera.Cs_pr_Document_Id = Guid.NewGuid().ToString();
                cabecera.Cs_pr_EstadoSCC = "2";
                cabecera.Cs_pr_EstadoSUNAT = "2";
                cabecera.Cs_tag_ID = datos_generales[0];
                cabecera.Cs_tag_IssueDate = datos_generales[1];
                cabecera.Cs_tag_InvoiceTypeCode = datos_generales[2];
                cabecera.Cs_tag_DocumentCurrencyCode = datos_generales[3];
                cabecera.Cs_tag_Discrepancy_ReferenceID = datos_generales[8];
                cabecera.Cs_tag_Discrepancy_ResponseCode = datos_generales[9];
                cabecera.Cs_tag_Discrepancy_Description = datos_generales[10];
                cabecera.Cs_tag_BillingReference_ID = datos_generales[11];
                cabecera.Cs_tag_BillingReference_DocumentTypeCode = datos_generales[12];
                cabecera.Cs_TipoCambio = datos_generales[13];
                cabecera.Cs_tag_AccountingSupplierParty_CustomerAssignedAccountID = datos_emisor[0];
                cabecera.Cs_tag_AccountingSupplierParty_AdditionalAccountID = datos_emisor[1];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyLegalEntity_RegistrationName = datos_emisor[2];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PartyName_Name = datos_emisor[3];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_ID = datos_emisor[4];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_StreetName = datos_emisor[5];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CitySubdivisionName = datos_emisor[6];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CityName = datos_emisor[7];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_CountrySubentity = datos_emisor[8];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_District = datos_emisor[9];
                cabecera.Cs_tag_AccountingSupplierParty_Party_PostalAddress_Country_IdentificationCode = datos_emisor[10];
                cabecera.Cs_tag_AccountingCustomerParty_CustomerAssignedAccountID = datos_receptor[0];
                cabecera.Cs_tag_AccountingCustomerParty_AdditionalAccountID = datos_receptor[1];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PartyLegalEntity_RegistrationName = datos_receptor[2];
                cabecera.Cs_tag_AccountingCustomerParty_Party_PhysicalLocation_Description = datos_receptor[3];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_currencyID = legal_monetary[0];
                cabecera.Cs_tag_LegalMonetaryTotal_PayableAmount_currencyID = legal_monetary[1];
                cabecera.Cs_tag_LegalMonetaryTotal_ChargeTotalAmount_AllowanceTotalAmount = legal_monetary[2];
                string idCabecera = cabecera.cs_pxInsertar(false,false);
                if (datos_generales[6].Trim() != "" && datos_generales[7].Trim() != "")
                {
                    clsEntityDocument_AdditionalDocumentReference docref = new clsEntityDocument_AdditionalDocumentReference(local);
                    docref.Cs_pr_Document_AdditionalDocumentReference_Id = Guid.NewGuid().ToString();
                    docref.Cs_pr_Document_Id = idCabecera;
                    docref.Cs_tag_DocumentTypeCode = datos_generales[7];
                    docref.Cs_tag_AdditionalDocumentReference_ID = datos_generales[6];
                    docref.cs_pxInsertar(false, true);
                }
                if (datos_generales[4].Trim() != "" && datos_generales[5].Trim() != "")
                {
                    clsEntityDocument_DespatchDocumentReference despatch = new clsEntityDocument_DespatchDocumentReference(local);
                    despatch.Cs_pr_Document_DespatchDocumentReference_Id = Guid.NewGuid().ToString();
                    despatch.Cs_pr_Document_Id = idCabecera;
                    despatch.Cs_tag_DespatchDocumentReference_ID = datos_generales[4];
                    despatch.Cs_tag_DocumentTypeCode = datos_generales[5];
                    despatch.cs_pxInsertar(false, true);
                }
                if (igv_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_igv = new clsEntityDocument_TaxTotal(local);
                    taxtotal_igv.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_igv.Cs_pr_Document_Id = idCabecera;
                    taxtotal_igv.Cs_tag_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxAmount = igv_totales[0];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = igv_totales[1];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = igv_totales[2];
                    taxtotal_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = igv_totales[3];
                    taxtotal_igv.cs_pxInsertar(false, true);
                }

                if (isc_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_isc = new clsEntityDocument_TaxTotal(local);
                    taxtotal_isc.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_isc.Cs_pr_Document_Id = idCabecera;
                    taxtotal_isc.Cs_tag_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxAmount = isc_totales[0];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = isc_totales[1];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = isc_totales[2];
                    taxtotal_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = isc_totales[3];
                    taxtotal_isc.cs_pxInsertar(false, true);
                }
                if (otros_totales[0].Trim() != "")
                {
                    clsEntityDocument_TaxTotal taxtotal_otros = new clsEntityDocument_TaxTotal(local);
                    taxtotal_otros.Cs_pr_Document_TaxTotal_Id = Guid.NewGuid().ToString();
                    taxtotal_otros.Cs_pr_Document_Id = idCabecera;
                    taxtotal_otros.Cs_tag_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxAmount = otros_totales[0];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = otros_totales[1];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = otros_totales[2];
                    taxtotal_otros.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = otros_totales[3];
                    taxtotal_otros.cs_pxInsertar(false, true);
                }
                //additionalmoentary
                clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation adinf = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation(local);
                adinf.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idCabecera + "1";
                adinf.Cs_pr_Document_Id = idCabecera;
                string idAdditionalInformation = adinf.cs_pxInsertar(false, true);
                if (op_gravadas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gravadas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gravadas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (op_inafectas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_inafectas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_inafectas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (op_exoneradas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_exoneradas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_exoneradas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (op_gratuitas[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = op_gratuitas[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = op_gratuitas[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (descuentos[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = descuentos[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = descuentos[1];
                    amt.Cs_tag_Percent = "";
                    amt.Cs_tag_ReferenceAmount = "";
                    amt.Cs_tag_SchemeID = "";
                    amt.Cs_tag_TotalAmount = "";
                    amt.cs_pxInsertar(false, true);
                }
                if (percepcion[0].Trim() != "")
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal amt = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal(local);
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalMonetaryTotal_Id = Guid.NewGuid().ToString();
                    amt.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    amt.Cs_tag_Id = percepcion[0];
                    amt.Cs_tag_Name = "";
                    amt.Cs_tag_PayableAmount = percepcion[2];
                    amt.Cs_tag_Percent = percepcion[3];
                    amt.Cs_tag_ReferenceAmount = percepcion[1];
                    amt.Cs_tag_SchemeID = percepcion[4];
                    amt.Cs_tag_TotalAmount = percepcion[5];
                    amt.cs_pxInsertar(false, true);
                }
                //additionalinformation
                foreach (string[] values in additional_propertys)
                {
                    clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty adprop = new clsEntityDocument_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty(local);
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_AdditionalProperty_Id = Guid.NewGuid().ToString();
                    adprop.Cs_pr_Document_UBLExtension_ExtensionContent_AdditionalInformation_Id = idAdditionalInformation;
                    adprop.Cs_tag_ID = values[0];
                    adprop.Cs_tag_Name = "";
                    adprop.Cs_tag_Value = values[1];
                    adprop.cs_pxInsertar(false, true);
                }
                //items
                int j = 0;
                foreach (string[] values in lineas)
                {
                    j++;
                    clsEntityDocument_Line line = new clsEntityDocument_Line(local);
                    line.Cs_pr_Document_Line_Id = Guid.NewGuid().ToString();
                    line.Cs_pr_Document_Id = idCabecera;
                    line.Cs_tag_InvoiceLine_ID = j.ToString();
                    //line.Cs_tag_AllowanceCharge_Amount = values[8];
                    //line.Cs_tag_AllowanceCharge_ChargeIndicator = values[7];
                    line.Cs_tag_invoicedQuantity = values[2];
                    line.Cs_tag_InvoicedQuantity_unitCode = values[1];
                    line.Cs_tag_Item_SellersItemIdentification = values[16];
                    line.Cs_tag_LineExtensionAmount_currencyID = values[3];
                    line.Cs_tag_Price_PriceAmount = values[17];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceAmount_currencyID = values[4];
                    // line.Cs_tag_PricingReference_AlternativeConditionPrice_PriceTypeCode = values[5];
                    string idLinea = line.cs_pxInsertar(false, true);

                    if (values[4].Trim() != "" && values[5].Trim() != "")
                    {
                        clsEntityDocument_Line_PricingReference linepriref = new clsEntityDocument_Line_PricingReference(local);
                        linepriref.Cs_pr_Document_Line_PricingReference_Id = Guid.NewGuid().ToString();
                        linepriref.Cs_pr_Document_Line_Id = idLinea;
                        linepriref.Cs_tag_PriceAmount_currencyID = values[4];
                        linepriref.Cs_tag_PriceTypeCode = values[5];
                        linepriref.cs_pxInsertar(false, true);
                    }

                    string descripcion = values[0].Replace("^", " \r ");
                    clsEntityDocument_Line_Description linedesc = new clsEntityDocument_Line_Description(local);
                    linedesc.Cs_pr_Document_Line_Description_Id = Guid.NewGuid().ToString();
                    linedesc.Cs_pr_Document_Line_Id = idLinea;
                    linedesc.Cs_tag_Description = descripcion;
                    linedesc.cs_pxInsertar(false, true);
                    if (values[6].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_igv = new clsEntityDocument_Line_TaxTotal(local);
                        tax_igv.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_igv.Cs_pr_Document_Line_Id = idLinea;
                        tax_igv.Cs_tag_TaxAmount_currencyID = values[6];
                        tax_igv.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[6];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = values[7];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[8];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[9];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[10];
                        tax_igv.Cs_tag_TaxSubtotal_TaxCategory_TierRange = "";
                        tax_igv.cs_pxInsertar(false, true);
                    }
                    if (values[11].Trim() != "")
                    {
                        clsEntityDocument_Line_TaxTotal tax_isc = new clsEntityDocument_Line_TaxTotal(local);
                        tax_isc.Cs_pr_Document_Line_TaxTotal_Id = Guid.NewGuid().ToString();
                        tax_isc.Cs_pr_Document_Line_Id = idLinea;
                        tax_isc.Cs_tag_TaxAmount_currencyID = values[11];
                        tax_isc.Cs_tag_TaxSubtotal_TaxAmount_currencyID = values[11];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxExemptionReasonCode = "";
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_ID = values[13];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_Name = values[14];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TaxScheme_TaxTypeCode = values[15];
                        tax_isc.Cs_tag_TaxSubtotal_TaxCategory_TierRange = values[12];
                        tax_isc.cs_pxInsertar(false, true);
                    }
                }
                if (cabecera.Cs_tag_BillingReference_DocumentTypeCode == "01")
                {
                    new clsBaseSunat(local).cs_pxEnviarCE(idCabecera, false);
                }
                retorno = true;
            }
            catch
            {
                retorno = false;
            }
            return retorno;
        }
    }
}
