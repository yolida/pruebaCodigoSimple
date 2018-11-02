using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseLog")]
    public class clsBaseLog
    {
        /// <summary>
        /// Metodo para registrar un error en archivo de log
        /// </summary>
        /// <param name="mensaje"></param>
        public static void cs_pxRegistar(string mensaje)
        {
            clsBaseConfiguracion entidad_configuracion = new clsBaseConfiguracion();
            if (!File.Exists(entidad_configuracion.cs_cmLog_sistema))
            {
                File.Create(entidad_configuracion.cs_cmLog_sistema).Close();
            }
            StreamWriter sw1 = new StreamWriter(entidad_configuracion.cs_cmLog_sistema);
            sw1.Write(mensaje);
            sw1.Close();
        }
        /// <summary>
        /// Metodo para registrar varios errores en un solo archivo.Para realizar seguimiento a errores segun fecha y hora.
        /// </summary>
        /// <param name="mensaje"></param>
        public static void cs_pxRegistarAdd(string mensaje)
        {
            clsBaseConfiguracion entidad_configuracion = new clsBaseConfiguracion();
            if (!File.Exists(entidad_configuracion.cs_cmLog_sistema_add))
            {
                File.Create(entidad_configuracion.cs_cmLog_sistema_add).Close();
            }
            TextWriter tw = new StreamWriter(entidad_configuracion.cs_cmLog_sistema_add, true);
            tw.WriteLine("<>" + DateTime.Now.ToString() + ":: " + mensaje);
            tw.Close();
           
        }
    }
}
