using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    /*Modulo creado para ayuda a empaquetamiento al momento de generar el compilado
      oficial del sistema FEI*/
    public class ModVers
    {
        public string error = "";

        /*Metodo para asignar la version del Compilado*/
        public string Vers_Compilado()
        {
            string result = "";
            try
            {
                result = "02.01.01";
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return result;
        }

        /*Metodo para asignar la Fecha del Compilado*/
        public string Build_Compilado()
        {
            string result = "";
            try
            {
                result = "27042018";
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return result;
        }

        /*Metodo para Cambiar el titulo del formulario Principal*/
        public List<string> Cabecera_Ventana()
        {
            List<string> result  = new List<string>();
            try
            {
                result.Add("Facturación Electrónica Integrada BETA - Servidor Beta Sunat");
                result.Add("Facturación Electrónica Integrada BETA");
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return result;
        }

        /*Metodo para activar la validación de la Licencia*/
        public bool Active_LIQUY()
        {
            bool result=false;
            try
            {
                result = false;//Cambiar por true para activar la seccion de LIQUY
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
            return result;
        }
    }
}
