using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{

    public class clsNegocioCEGuiaRemision : clsNegocioCE
    {
        private clsEntityDatabaseLocal localbd;
        public clsNegocioCEGuiaRemision(clsEntityDatabaseLocal local) : base(local)
        {
            localbd = local;
            //other stuff here
        }
        public override string cs_pxGenerarXMLAString(string Id)
        {
            string archivo_xml = string.Empty;
            try
            {
                clsEntityDespatch DespatchDocument = new clsEntityDespatch(localbd).cs_fxObtenerUnoPorId(Id);
                List<clsEntityDespatch_ShipmentStage> Despatch_ShipStage = new clsEntityDespatch_ShipmentStage(localbd).cs_fxObtenerTodoPorCabeceraId(Id);
                List<clsEntityDespatch_OrderReference> Despatch_OrderReference = new clsEntityDespatch_OrderReference(localbd).cs_fxObtenerTodoPorCabeceraId(Id);           
                List<clsEntityDespatch_PortLocation> Despatch_PortLocation = new clsEntityDespatch_PortLocation(localbd).cs_fxObtenerTodoPorCabeceraId(Id);
                List<clsEntityDespatch_Line> Despatch_Line = new clsEntityDespatch_Line(localbd).cs_fxObtenerTodoPorCabeceraId(Id);


                string fila = "";
                string ei = "    ";
                string ef = "\n";

                fila += "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"no\"?>" + ef;
                fila += "<DespatchAdvice " + ef;
                #region Cabecera
                fila += ei + "xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2\"" + ef;
                fila += ei + "xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\"" + ef;
                fila += ei + "xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\"" + ef;
                fila += ei + "xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\"" + ef;
                fila += ei + "xmlns:sac=\"urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1\"" + ef;
                fila += ei + "xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\"" + ef;
                fila += ei + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" + ef;
                fila += ei + "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + ef;
                fila += ei + "<ext:UBLExtensions>" + ef;
                fila += ei + ei + "<ext:UBLExtension><ext:ExtensionContent></ext:ExtensionContent></ext:UBLExtension>" + ef;
                fila += ei + "</ext:UBLExtensions>" + ef;
                #endregion
                fila += ei + "<cbc:UBLVersionID>2.1</cbc:UBLVersionID>" + ef;/*Se encontro que el Documento de Guia de Remision ya esta con la Version 2.1*/
                fila += ei + "<cbc:CustomizationID>1.0</cbc:CustomizationID>" + ef;
                fila += ei + "<cbc:ID>" + DespatchDocument.Cs_tag_ID + "</cbc:ID>" + ef;
                fila += ei + "<cbc:IssueDate>" + DespatchDocument.Cs_tag_IssueDate + "</cbc:IssueDate>" + ef;
                fila += ei + "<cbc:DespatchAdviceTypeCode>" + DespatchDocument.Cs_tag_AdviceTypeCode + "</cbc:DespatchAdviceTypeCode>" + ef;          
                fila += ei + "<cbc:Note>" + DespatchDocument.Cs_tag_Note + "</cbc:Note>" + ef;
                if (Despatch_OrderReference.Count > 0)
                {
                    foreach (var Despatch_OrderReference_item in Despatch_OrderReference)
                    {
                        fila += ei + "<cac:OrderReference>" + ef;
                        fila += ei + ei + "<cbc:ID>" + Despatch_OrderReference_item.Cs_tag_ID + "</cbc:ID>" + ef;
                        /*if (Despatch_OrderReference_item.Cs_tag_OrderTypeCode != "" && Despatch_OrderReference_item.Cs_tag_OrderTypeCode_Name!="")
                        {
                            fila += ei + ei + "<cbc:OrderTypeCode name=\""+Despatch_OrderReference_item.Cs_tag_OrderTypeCode_Name+"\">" + Despatch_OrderReference_item.Cs_tag_OrderTypeCode + "</cbc:OrderTypeCode>" + ef;
                        }*/
                        if (Despatch_OrderReference_item.Cs_tag_OrderTypeCode != "")
                       {
                           fila += ei + ei + "<cbc:OrderTypeCode>" + Despatch_OrderReference_item.Cs_tag_OrderTypeCode + "</cbc:OrderTypeCode>" + ef;
                       }

                        fila += ei + "</cac:OrderReference>" + ef;
                    }
                }               
                if (DespatchDocument.Cs_tag_AdditionalDocumentReference_DocumentTypeCode != "" && DespatchDocument.Cs_tag_AdditionalDocumentReference_ID!="")
                {
                    fila += ei + "<cac:AdditionalDocumentReference>" + ef;
                    fila += ei + ei + "<cbc:ID>"+DespatchDocument.Cs_tag_AdditionalDocumentReference_ID+"</cbc:ID>" + ef;              
                    fila += ei + ei + "<cbc:DocumentTypeCode>" + DespatchDocument.Cs_tag_AdditionalDocumentReference_DocumentTypeCode + "</cbc:DocumentTypeCode>" + ef;
                    fila += ei + "</cac:AdditionalDocumentReference>" + ef;
                }              
                #region Referencia de la firma digital
                fila += ei + "<cac:Signature>" + ef;
                fila += ei + ei + "<cbc:ID>SignatureSP</cbc:ID>" + ef;
                fila += ei + ei + "<cac:SignatoryParty>" + ef;
                fila += ei + ei + ei + "<cac:PartyIdentification>" + ef;
                fila += ei + ei + ei + ei + "<cbc:ID>" + declarante.Cs_pr_Ruc + "</cbc:ID>" + ef;
                fila += ei + ei + ei + "</cac:PartyIdentification>" + ef;

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
                fila += ei + "<cac:DespatchSupplierParty>" + ef;
                fila += ei + ei + "<cbc:CustomerAssignedAccountID schemeID=\"" + DespatchDocument.Cs_tag_DespatchSupParty_CustAssigAccountID_SchemeID + "\">" + DespatchDocument.Cs_tag_DespatchSupParty_CustAssigAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
                fila += ei + ei + "<cac:Party>" + ef;
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + DespatchDocument.Cs_tag_DespatchSupParty_PartyLegalEntity + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
                fila += ei + "</cac:DespatchSupplierParty>" + ef;
                fila += ei + "<cac:DeliveryCustomerParty>" + ef;
                fila += ei + ei + "<cbc:CustomerAssignedAccountID schemeID=\"" + DespatchDocument.Cs_tag_DeliveryCustParty_CustAssigAccountID_SchemeID + "\">" + DespatchDocument.Cs_tag_DeliveryCustParty_CustAssigAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
                fila += ei + ei + "<cac:Party>" + ef;
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + DespatchDocument.Cs_tag_DeliveryCustParty_PartyLegalEntity + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
                fila += ei + "</cac:DeliveryCustomerParty>" + ef;
                fila += ei + "<cac:SellerSupplierParty>" + ef;
                fila += ei + ei + "<cbc:CustomerAssignedAccountID schemeID=\"" + DespatchDocument.Cs_tag_SellerSupParty_CustAssigAccountID_SchemeID + "\">" + DespatchDocument.Cs_tag_SellerSupParty_CustAssigAccountID + "</cbc:CustomerAssignedAccountID>" + ef;
                fila += ei + ei + "<cac:Party>" + ef;
                fila += ei + ei + ei + "<cac:PartyLegalEntity>" + ef;
                fila += ei + ei + ei + ei + "<cbc:RegistrationName><![CDATA[" + DespatchDocument.Cs_tag_SellerSupParty_PartyLegalEntity + "]]></cbc:RegistrationName>" + ef;
                fila += ei + ei + ei + "</cac:PartyLegalEntity>" + ef;
                fila += ei + ei + "</cac:Party>" + ef;
                fila += ei + "</cac:SellerSupplierParty>" + ef;
                #endregion
                #region shipment
                fila += ei + "<cac:Shipment>" + ef;
                fila += ei + ei + "<cbc:ID>1</cbc:ID>" + ef;
                fila += ei + ei + "<cbc:HandlingCode>" + DespatchDocument.Cs_tag_Ship_HandlingCode + "</cbc:HandlingCode>" + ef;
               
                fila += ei + ei + "<cbc:Information>" + DespatchDocument.Cs_tag_Ship_Information + "</cbc:Information>" + ef;
                fila += ei + ei + "<cbc:GrossWeightMeasure unitCode=\"" + DespatchDocument.Cs_tag_Ship_GrossWeightMeasure_UnitCode + "\">" + DespatchDocument.Cs_tag_Ship_GrossWeightMeasure + "</cbc:GrossWeightMeasure>" + ef;
               
                if (DespatchDocument.Cs_tag_Ship_TotalTransportHandlingUnitQuantity!="")
                {
                    fila += ei + ei + "<cbc:TotalTransportHandlingUnitQuantity>" + DespatchDocument.Cs_tag_Ship_TotalTransportHandlingUnitQuantity + "</cbc:TotalTransportHandlingUnitQuantity>" + ef;
                }
                fila += ei + ei + "<cbc:SplitConsignmentIndicator>" + DespatchDocument.Cs_tag_Ship_SplitConsignmentIndicador + "</cbc:SplitConsignmentIndicator>" + ef;

                if (Despatch_ShipStage.Count > 0)
                {
                    int correlativo = 0;
                    foreach (var Despatch_ShipStage_item in Despatch_ShipStage)
                    {
                        correlativo++;
                        fila += ei + ei + "<cac:ShipmentStage>" + ef;
                       // fila += ei + ei + ei + "<cbc:ID>" + correlativo.ToString() + "</cbc:ID>" + ef;
                        fila += ei + ei + ei + "<cbc:TransportModeCode>" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_TraModeCode + "</cbc:TransportModeCode>" + ef;
                        fila += ei + ei + ei + "<cac:TransitPeriod>" + ef;
                        fila += ei + ei + ei + ei + "<cbc:StartDate>" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_TransitPeriod_StartDate + "</cbc:StartDate>" + ef;
                        fila += ei + ei + ei + "</cac:TransitPeriod>" + ef;
                        fila += ei + ei + ei + "<cac:CarrierParty>" + ef;
                        fila += ei + ei + ei + ei + "<cac:PartyIdentification>" + ef;
                        fila += ei + ei + ei + ei + ei + "<cbc:ID schemeID=\"" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_CarrierParty_PartyID_SchemeID + "\">" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_CarrierParty_PartyID_ID + "</cbc:ID>" + ef;
                        fila += ei + ei + ei + ei + "</cac:PartyIdentification>" + ef;
                        fila += ei + ei + ei + ei + "<cac:PartyName>" + ef;
                        fila += ei + ei + ei + ei + ei + "<cbc:Name><![CDATA[" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_CarrierParty_PartyName + "]]></cbc:Name>" + ef;
                        fila += ei + ei + ei + ei + "</cac:PartyName>" + ef;
                        fila += ei + ei + ei + "</cac:CarrierParty>" + ef;
                        fila += ei + ei + ei + "<cac:TransportMeans>" + ef;
                        fila += ei + ei + ei + ei + "<cac:RoadTransport>" + ef;
                        fila += ei + ei + ei + ei + ei + "<cbc:LicensePlateID>" + Despatch_ShipStage_item.Cs_tag_ship_ShipStage_TransportMeans_LicencePlateID + "</cbc:LicensePlateID>" + ef;
                        fila += ei + ei + ei + ei + "</cac:RoadTransport>" + ef;
                        fila += ei + ei + ei + "</cac:TransportMeans>" + ef;
                        

                        List<clsEntityDespatch_ShipmentStage_Driver> ShipStage_Driver = new clsEntityDespatch_ShipmentStage_Driver(localbd).cs_fxObtenerTodoPorCabeceraId(Despatch_ShipStage_item.Cs_pr_Despatch_Shipment_ID);
                       
                        if (ShipStage_Driver.Count > 0)
                        {
                            foreach (var ShipStage_Driver_Item in ShipStage_Driver)
                            {
                                fila += ei + ei + ei + "<cac:DriverPerson>" + ef;
                                fila += ei + ei + ei + ei + "<cbc:ID schemeID=\""+ShipStage_Driver_Item.Cs_tag_Driver_SchemaID+"\">" + ShipStage_Driver_Item.Cs_tag_Driver_ID + "</cbc:ID>" + ef;
                                fila += ei + ei + ei + "</cac:DriverPerson>" + ef;
                            }
                        }
                       
                        fila += ei + ei + "</cac:ShipmentStage>" + ef;
                    }
                }

                fila += ei + ei + "<cac:Delivery>" + ef;
                fila += ei + ei + ei + "<cac:DeliveryAddress>" + ef;
                fila += ei + ei + ei + ei + "<cbc:ID>" + DespatchDocument.Cs_tag_Ship_DeliveryAddress_ID + "</cbc:ID>" + ef;
                fila += ei + ei + ei + ei + "<cbc:StreetName><![CDATA[" + DespatchDocument.Cs_tag_Ship_DeliveryAddress_StreetName + "]]></cbc:StreetName>" + ef;
                fila += ei + ei + ei + "</cac:DeliveryAddress>" + ef;
                fila += ei + ei + "</cac:Delivery>" + ef;
                fila += ei + ei + "<cac:TransportHandlingUnit>" + ef;
                //fila += ei + ei + ei + "<cac:TransportEquipment>" + ef;
                fila += ei + ei + ei +  "<cbc:ID>"+ DespatchDocument.Cs_tag_Ship_TransHandUnit_Equip_ID+"</cbc:ID>" + ef;
                //fila += ei + ei + ei + "</cac:TransportEquipment>" + ef;
                fila += ei + ei + "</cac:TransportHandlingUnit>" + ef;
                fila += ei + ei + "<cac:OriginAddress>" + ef;
                fila += ei + ei + ei + "<cbc:ID>"+DespatchDocument.Cs_tag_Ship_OriginAddress_ID +"</cbc:ID>"+ ef;
                fila += ei + ei + ei + "<cbc:StreetName><![CDATA[" + DespatchDocument.Cs_tag_Ship_OriginAddress_StreetName + "]]></cbc:StreetName>" + ef;
                fila += ei + ei + "</cac:OriginAddress>" + ef;

                if (Despatch_PortLocation.Count > 0)
                {
                    foreach (var Despatch_PortLocation_Item in Despatch_PortLocation)
                    {
                        fila += ei + ei + "<cac:FirstArrivalPortLocation>" + ef;
                        fila += ei + ei + ei + "<cbc:ID>" + Despatch_PortLocation_Item.Cs_tag_ID + "</cbc:ID>" + ef;
                        fila += ei + ei + "</cac:FirstArrivalPortLocation>" + ef;
                    }
                }
                fila += ei + "</cac:Shipment>" + ef;
                #endregion
                #region Items

                if (Despatch_Line != null && Despatch_Line.Count > 0)
                {
                     try
                     {
                        int linea = 0;
                        foreach (var Line_item in Despatch_Line)
                         {
                            linea++;
                            fila += ei + "<cac:DespatchLine>" + ef;
                            fila += ei + ei + "<cbc:ID>" + linea.ToString() + "</cbc:ID>" + ef;
                            fila += ei + ei + "<cbc:DeliveredQuantity unitCode=\"" + Line_item.Cs_tag_DeliveredQuantity_UnitCode + "\">" + Line_item.Cs_tag_DeliveredQuantity + "</cbc:DeliveredQuantity>" + ef;
                            fila += ei + ei + "<cac:OrderLineReference>" + ef;
                            fila += ei + ei + ei + "<cbc:LineID>" + linea.ToString() + "</cbc:LineID>" + ef;
                            fila += ei + ei + "</cac:OrderLineReference>" + ef;
                            fila += ei + ei + "<cac:Item>" + ef;
                            fila += ei + ei + ei + "<cbc:Name><![CDATA[" + Line_item.Cs_tag_ItemName + "]]></cbc:Name>" + ef;
                            if (Line_item.Cs_tag_Item_SellersItemIdentification_ID!="")
                            {
                                fila += ei + ei + ei + "<cac:SellersItemIdentification>" + ef;
                                fila += ei + ei + ei + ei + "<cbc:ID>" + Line_item.Cs_tag_Item_SellersItemIdentification_ID + "</cbc:ID>" + ef;
                                fila += ei + ei + ei + "</cac:SellersItemIdentification>" + ef;
                            }
                            fila += ei + ei + "</cac:Item>" + ef;
                            fila += ei + "</cac:DespatchLine>" + ef;

                         }
                     }
                     catch (Exception ex)
                     {
                         clsBaseLog.cs_pxRegistarAdd("Error al crear detalle de Guia" + ex.ToString());
                     }
                 }

                 #endregion
                fila += "</DespatchAdvice>" + ef;

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
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return archivo_xml;
        }
    }
}
