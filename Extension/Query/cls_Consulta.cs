using Extension.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Query
{
    public class cls_Consulta
    {
        public string Cadena_SQL ="";

        //Cristhian|02/03/2018|FEI2-585
        /*Consulta SQL creado para buscar el comprobante electronico por Serie,Numero,Codigo de Comprobante y RUC o DNI del cliente (cliente del cliente)
          creado para realizar la busqueda desde el comercial, ya que no tiene el mismo ID de registro (llave primaria) del comprobante*/
        /*NUEVO INICIO*/
        public string Seleccionar_Comprobante_DBFComercial(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente)
        {
            Cadena_SQL =  " SELECT TOP(1) cs_Document_Id, cp8, cp9, cp27, cp28 " +
                              " FROM cs_Document "+
                              " WHERE cp1 = '"+SerieNumero+"' AND cp3 = '"+Codigo_Comprobante+"' AND cp21 = '"+Identificacion_Cliente+ "' " +
                              " ORDER BY cs_Document_Id DESC";

            return Cadena_SQL;
        }
        /*NUEVO FIN*/

        //Cristhian|06/03/2018|FEI2-585
        /*Consulta SQL creado para buscar el comprobante electronico por Serie,Numero,Codigo de Comprobante y RUC o DNI del cliente (cliente del cliente)
          creado para realizar la busqueda desde el comercial, ya que no tiene el mismo ID de registro (llave primaria) del comprobante*/
        /*NUEVO INICIO*/
        public string Anular_Comprobante_DBFComercial(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente, string estado)
        {
            Cadena_SQL = " UPDATE cs_Document " +
                         " SET cp28='" + estado + "' " +
                         " WHERE cp1 = '" + SerieNumero + "' AND cp3 = '" + Codigo_Comprobante + "' AND cp21 = '" + Identificacion_Cliente + "' ";//AND cp27 IN (0,3)

            return Cadena_SQL;
        }
        /*NUEVO FIN*/

        //Cristhian|06/03/2018|FEI2-585
        /*Consulta SQL creado para buscar el comprobante electronico por Serie,Numero,Codigo de Comprobante y RUC o DNI del cliente (cliente del cliente)
          creado para realizar la busqueda desde el comercial, ya que no tiene el mismo ID de registro (llave primaria) del comprobante*/
        /*NUEVO INICIO*/
        public string Anular_Comprobante_DBFComercial(string Id_Comprobante,string estado)
        {
            DateTime Date = DateTime.Now;
            /*Los dos campos a modificarse son los el CP28=se maneja estado propio de la empresa y el cp33 para liberarlo del resumendiario*/
            Cadena_SQL = " UPDATE cs_Document " +
                         " SET cp28='" + estado + "', cp33='', cp46='"+ Date.ToString("yyyy-MM-dd") + "' " +
                         " WHERE cs_Document_Id='"+ Id_Comprobante + "' ";

            return Cadena_SQL;
        }
        /*NUEVO FIN*/

        //Cristhian|06/03/2018|FEI2-585
        /*Se debe evitar la modificación del comprobante una vez que el comprobante electrónico a sido aceptado por SUNAT*/
        /*NUEVO INICIO*/
        public string Estado_SUNAT_para_DBFComercial(string SerieNumero, string Codigo_Comprobante, string Identificacion_Cliente, string Monto)
        {
            Cadena_SQL = " SELECT cp27 " +
                         " FROM cs_Document " +
                         " WHERE cp1 = '" + SerieNumero + "' AND cp3 = '" + Codigo_Comprobante + "' AND cp21 = '" + Identificacion_Cliente + "' AND cp26='"+Monto+"' ";

            return Cadena_SQL;
        }
        /*NUEVO FIN*/
        // Nuevos desarrollos
        //Jorge Luis|03/05/2018|Pendiente
        /*Método para consultar el estado de un documento mediante su ID*/
        public string ConsultaEstadoDocumento(string serieNumero, string codigoComprobante, string identificacionCliente)
        {
            //setInicioDeclarante("");

            ClassNegocioExtractData classNegocioExtractData = new ClassNegocioExtractData();
            string query = string.Empty;
            try
            {
                query = classNegocioExtractData.ExtractUniqueValue("select top 1 cp27 from cs_Document " +
                "where cp1 = '" + serieNumero + "' and cp3 = '" + codigoComprobante + "' and cp21 = '" + identificacionCliente + "' order by cs_Document_Id desc");
            }
            catch (Exception ex)
            {
                query = "Sin resultado" + ex;
            }
            return query;
        }
    }
}
