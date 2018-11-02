using Extension.Base;
using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Negocio
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsNegocioCEEstadosFEI")]
    public class clsNegocioCEEstadosFEI
    {
        private clsEntityDatabaseLocal localDB;
        public clsNegocioCEEstadosFEI(clsEntityDatabaseLocal local){
            localDB = local;
        }
        public clsNegocioCEEstadosFEI()
        {
           
        }
        public void setInicioDeclarante(string id)
        {
            clsEntityDatabaseLocal local = new clsEntityDatabaseLocal().cs_fxObtenerUnoPorDeclaranteId(id);
            this.localDB = local;
        }
        /// <summary>
        /// Obtiene el estado del conprobante en SUNAT(ACEPTADO, RECHAZADO, SIN ESTADO, ANULADO)
        /// </summary>
        /// <param name="Id">Id del comprobante</param>
        /// <returns></returns>
        public string cs_fxEstadoSUNAT(string Id)
        {
            clsEntityDocument cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(Id);
            return clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt32(cabecera.Cs_pr_EstadoSUNAT.Trim()));
        }

        /// <summary>
        /// Obtiene el estado del conprobante en SUNAT para los comprobantes de retencion(ACEPTADO, RECHAZADO, SIN ESTADO, ANULADO)
        /// </summary>
        /// <param name="Id">Id del comprobante</param>
        /// <returns></returns>
        public string cs_fxEstadoSUNATRetention(string Id)
        {
            clsEntityRetention cabecera = new clsEntityRetention(localDB).cs_fxObtenerUnoPorId(Id);
            return clsBaseUtil.cs_fxComprobantesEstadosSUNAT_descripcion(Convert.ToInt32(cabecera.Cs_pr_EstadoSUNAT.Trim()));
        }


        /// <summary>
        /// Obtiene el estado del comprobante en el SISTEMA CONTABLE COMERCIAL (ENVIADO, PENDIENTE (CORRECTO), PENDIENTE (CON ERRORES))
        /// </summary>
        /// <param name="Id">Id del comprobante</param>
        /// <returns></returns>
        public string cs_fxEstadoSCC(string Id)
        {
            clsEntityDocument cabecera = new clsEntityDocument(localDB).cs_fxObtenerUnoPorId(Id);
            return clsBaseUtil.cs_fxComprobantesEstadosSCC_descripcion(Convert.ToInt32(cabecera.Cs_pr_EstadoSCC.Trim()));
        }
    }
}
