using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extension.Base;
using System.Data.Odbc;
using System.Runtime.InteropServices;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseEntidadSistema")]
    public class clsBaseEntidadSistema
    {
        public List<string> cs_cmCampos = new List<string>();
        protected List<string> cs_cmValores = new List<string>();
        public string cs_cmTabla = "";
        /// <summary>
        /// Metodo para actualizar un registro de base de datos sistema.
        /// </summary>
        /// <param name="obtener_mensaje_respuesta"></param>
        public virtual void cs_pxActualizar(bool obtener_mensaje_respuesta)
        {
            cs_pxActualizarEntidad();
            new clsBaseConexionSistema().cs_pxActualizar(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);
        }
        /// <summary>
        /// Metodo para insertar un registro de base de datos sistema.
        /// </summary>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxInsertar(bool obtener_mensaje_respuesta)
        {
            cs_pxActualizarEntidad();
            new clsBaseConexionSistema().cs_pxInsertar(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);
        }
        /// <summary>
        /// Metodo para eliminar un registro de base de datos sistema.
        /// </summary>
        /// <param name="obtener_mensaje_respuesta"></param>
        public void cs_pxElimnar(bool obtener_mensaje_respuesta)
        {
            cs_pxActualizarEntidad();
            new clsBaseConexionSistema().cs_pxEliminar(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);
        }
        /// <summary>
        /// Metodo para obtener todos los registros de tabla en base de datos sistema.
        /// </summary>
        /// <param name="obtener_mensaje_respuesta"></param>
        /// <returns></returns>
        public List<List<string>> cs_pxEntidadSeleccionartodo(bool obtener_mensaje_respuesta)
        {
            return new clsBaseConexionSistema().cs_fxSeleccionartodo(cs_cmTabla, cs_cmCampos, cs_cmValores, obtener_mensaje_respuesta);
        }

        protected virtual void cs_pxActualizarEntidad()
        {

        }
        public bool cs_pxValidar()
        {
            return !cs_pxValidarReporte().Contains("Error");
        }

        public virtual string cs_pxValidarReporte()
        {
            return "";
        }
    }
}