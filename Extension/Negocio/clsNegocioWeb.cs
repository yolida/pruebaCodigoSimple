using Extension.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Negocio
{
    public class clsNegocioWeb
    {
        private clsEntityDatabaseLocal localDB;
        public clsNegocioWeb(clsEntityDatabaseLocal local){
            localDB = local;
        }
        public clsEntityDocument cs_pxBuscarDocumento(string tipodocumento, string serienumero, string fecha, string monto)
        {
            clsEntityDocument entidad =  new clsEntityDocument(localDB).cs_pxBuscarDocumento(tipodocumento, serienumero, fecha, monto);
            return entidad;
        }

        public List<clsEntityDocument> cs_pxBuscarDocumentos(string ruc, string tipodocumento, string fecha_inicio, string fecha_fin)
        {
            List<clsEntityDocument> entidades = new clsEntityDocument(localDB).cs_pxBuscarDocumentos(ruc, tipodocumento, fecha_inicio, fecha_fin);
            return entidades;
        }
    }
}
