using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Extension.Negocio
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsNegocioXML")]
    public class clsNegocioXML
    {
        private clsEntityDatabaseLocal localDB;
        public clsNegocioXML(clsEntityDatabaseLocal local)
        {
            localDB = local;
        }
        public clsNegocioXML()
        {      
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        public string cs_fxHash(string Id)
        {
            string XML = string.Empty;
            clsEntityDocument cabecera = new clsEntityDocument(localDB);
            cabecera.cs_fxObtenerUnoPorId(Id);
            if (cabecera.Cs_pr_XML.Trim() == "" && cabecera.Cs_pr_CDR.Trim() == "")
            {
                switch (cabecera.Cs_tag_InvoiceTypeCode)
                {
                    case "01":
                        XML = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "03":
                        XML = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "07":
                        XML = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "08":
                        XML = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                }
            }
            else
            {
                XML = cabecera.Cs_pr_XML;
            }
            XML = XML.Replace("cbc:", "");
            XML = XML.Replace("cac:", "");
            XML = XML.Replace("ext:", "");
            XML = XML.Replace("ds:", "");
            string estructura = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XML);
            return xmlDoc.GetElementsByTagName("DigestValue")[0].InnerText;
        }

        public string cs_fxSignature(string Id)
        {
            string XML = string.Empty;
            clsEntityDocument cabecera = new clsEntityDocument(localDB);
            cabecera.cs_fxObtenerUnoPorId(Id);
            if (cabecera.Cs_pr_XML.Trim() == "" && cabecera.Cs_pr_CDR.Trim() == "")
            {
                switch (cabecera.Cs_tag_InvoiceTypeCode)
                {
                    case "01":
                        XML = new clsNegocioCEFactura(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "03":
                        XML = new clsNegocioCEBoleta(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "07":
                        XML = new clsNegocioCENotaCredito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                    case "08":
                        XML = new clsNegocioCENotaDebito(localDB).cs_pxGenerarXMLAString(Id);
                        break;
                }
            }
            else
            {
                XML = cabecera.Cs_pr_XML;
            }
            XML = XML.Replace("cbc:", "");
            XML = XML.Replace("cac:", "");
            XML = XML.Replace("ext:", "");
            XML = XML.Replace("ds:", "");
            string estructura = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(XML);
            return xmlDoc.GetElementsByTagName("SignatureValue")[0].InnerText;
        }
    }
}
