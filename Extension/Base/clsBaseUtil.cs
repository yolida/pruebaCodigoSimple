using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Base
{
    public class clsBaseUtil
    {
        public static string cs_fxDBMS(int bdms){
            string motor = "";
            switch (bdms)
            {
                case 1:
                    motor = "Microsoft SQL Server";
                    break;
                case 2:
                    motor = "MySQL";
                    break;
                case 3:
                    motor = "SQLite";
                    break;
                case 4:
                    motor = "PostgreSQL";
                    break;
                default:
                    motor = "PostgreSQL";
                    break;

            }
            return motor;
        }

        public static string cs_fxDBMS_Driver(int bdms)
        {
            string motor = "";
            switch (bdms)
            {
                case 1:
                    motor = "SQL Server";
                    break;
                case 2:
                    motor = "MySQL ODBC 5.3 ANSI Driver";
                    break;
                case 3:
                    motor = "SQLite3 ODBC Driver";
                    break;
                case 4:
                    motor = "PostgreSQL";
                    break;
                default:
                    motor = "PostgreSQL";
                    break;
            }
            return motor;
        }



        public static string cs_fxCatalogo(int cataloo)
        {
            string codigo = "";
            switch (cataloo)
            {
                case 0:
                    codigo = "01";//TIPO DE DOCUMENTO
                    break;
                case 1:
                    codigo = "02";//TIPO DE MONEDA
                    break;
                case 2:
                    codigo = "03";//TIPO DE EXISTENCIA
                    break;
                case 3:
                    codigo = "04";//CÓDIGO DE UNIDAD DE MEDIDA
                    break;
            }
            return codigo;
        }

        public static bool cs_fxStringToBoolean(string valor) 
        {
            if (valor.Trim() == "T")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string cs_fxBooleanToString(bool valor)
        {
            if (valor == true)
	        {
                return "T";
	        }
            else
	        {
                return "F";
	        }
        }
        public static string cs_GetMoneda(string rep)
        {
            string valor = "";
            switch (rep)
            {
                case "PEN":
                    valor = "S"; //Factura
                    break;
                case "USD":
                    valor = "D"; //Factura
                    break;
            }
            return valor;
        }
        public static string cs_GetMonedaCode(string rep)
        {
            string valor = "";
            switch (rep)
            {
                case "S":
                    valor = "PEN"; //Factura
                    break;
                case "D":
                    valor = "USD"; //Factura
                    break;
            }
            return valor;
        }
        public static string cs_fxComprobantesEstadosValidar_Descripcion(int index)
        {
            string[] comprobantes = clsBaseUtil.cs_fxComprobantesEstadosValidar();
            return comprobantes[index];
        }
        public static string[] cs_fxComprobantesEstadosValidar()
        {
            string[] comprobantes = new string[]
            {
                "Aceptado",
                "Rechazado",
                "De Baja",
                "No existe",
                "XML no validado",
                "Excepcion"
            };
            return comprobantes;
        }

        public static string cs_fxComprobantesElectronicos_codigo(int indice)
        {
            string codigo= "";

            switch (indice)
            {
                case 0:
                    codigo = "01"; //Factura
                    break;
                case 1:
                    codigo = "03";//Boleta de venta
                    break;
                case 2:
                    codigo = "07";//Nota de crédito
                    break;
                case 3:
                    codigo = "08";//Nota de débito
                    break;
                case 4:
                    codigo = "09"; //Guía de remisión remitente
                    break;
            }
            //"Comprobante de retención electrónica",
            //"Comprobante de percepción electrónica"
            return codigo;
        }

        public static string cs_fxComprobantesElectronicos_descripcion(string codigo)
        {
            string descripcion = "";
            switch (codigo)
            {
                case "01":
                    descripcion = "Factura electrónica"; //Factura
                    break;
                case "03":
                    descripcion = "Boleta de venta electrónica";//Boleta de venta
                    break;
                case "07":
                    descripcion = "Nota de crédito electrónica";//Nota de crédito
                    break;
                case "08":
                    descripcion = "Nota de débito electrónica";//Nota de débito
                    break;
                case "09":
                    descripcion = "Guía de remisión electrónica"; //Guía de remisión remitente
                    break;
            }
            //"Comprobante de retención electrónica",
            //"Comprobante de percepción electrónica"
            return descripcion;
        }
        
        public static string[] cs_fxComprobantesElectronicos()
        {
            string[] comprobantes = new string[]{
                "Factura electrónica",
                "Boleta de venta electrónica",
                "Nota de crédito electrónica",
                "Nota de débito electrónica",
                //"Guía de remisión electrónica"
                //"Comprobante de retención electrónica",
                //"Comprobante de percepción electrónica"
            };
            return comprobantes;
        }

        public static string cs_fxComprobantesElectronicos_descripcion(int index)
        {
            string[] comprobantes = clsBaseUtil.cs_fxComprobantesElectronicos();
            return comprobantes[index];
        }

        public static string[] cs_fxComprobantesEstadosSUNAT()
        {
            string[] comprobantes = new string[]{
                //"Enviado",
                "Aceptado",
                //"Aceptado con observaciones",
                "Rechazado",
                "Sin estado",//"Sin respuesta",
                "De Baja",
                "En proceso",
                "Ticket a consultar"
            };
            return comprobantes;
        }
        public static string[] cs_fxComprobantesEstadosSUNATInterfaz()
        {
            string[] comprobantes = new string[]{
                //"Enviado",
                "Aceptado",
                //"Aceptado con observaciones",
                "Rechazado",
                "Sin estado",//"Sin respuesta",
                "De Baja",
            };
            return comprobantes;
        }

        public static string cs_fxComprobantesEstadosSUNAT_descripcion(int index)
        {
            string[] comprobantes = clsBaseUtil.cs_fxComprobantesEstadosSUNAT();
            return comprobantes[index];
        }
        
        public static string[] cs_fxComprobantesEstadosSCC()
        {
            string[] comprobantes = new string[]{
                "Enviado",
                "Pendiente (errores)",
                "Pendiente (correcto)",
                "Pendiente (de baja)",
            };
            return comprobantes;
        }

        public static string cs_fxComprobantesEstadosSCC_descripcion(int index)
        {
            string[] comprobantes = clsBaseUtil.cs_fxComprobantesEstadosSCC();
            return comprobantes[index];
        }

        public static string[] cs_fxReportesFormatos()
        {
            string[] comprobantes = new string[]{
                "CSV",
                "PDF",
            };
            return comprobantes;
        }
    }
}