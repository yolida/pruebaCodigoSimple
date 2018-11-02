using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Extension.Negocio
{
    public class clsNegocioValidar_Campos
    {
        private const string er_solo_letras_espacios = @"[a-zA-Z\s]";
        private const string er_solo_numeros_letras = @"[a-zA-Z0-9]";
        private const string er_solo_numeros_letras_espacios = @"[a-zA-Z0-9\s]";

        private const string er_solo_letras_espacios_letrasvocalestildadas = @"[A-Za-záéíóúñÁÉÍÓÚÑ\s]";
        private const string er_solo_numeros_letras_espacios_letrasvocalestildadas = @"[A-Za-z0-9áéíóúñÁÉÍÓÚÑ\s]";

        private const string er_solo_texto_tradicional = @"[A-Za-z0-9áéíóúñÁÉÍÓÚÑ\s\;\:\.\,\*\-\(\)_]";

        private const string er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion = @"[A-Za-z0-9áéíóúñÁÉÍÓÚÑ\s\-\.\,\;]";

        private const string er_solo_numeros = "[0-9]";

        //Cristhian|03/01/2018|FEI2-409
        /*Se crea el array constante que contiene los caracteres especicales que son consideradas como error*/
        /*NUEVO INICIO*/
        public static char[] CaracteresEspeciales = { '{', '[', '(', '¿', '¡', '#', '$', '%', '&', '/', '~', '^', ':', '.', '-', '*', '+', '|', '¬', '°', '`', '=', '\"', '!', '?', ')', ']', '}' };
        /*NUEVO FIN*/

        public static bool cs_prSER_ID(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9\-]{1,36}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_10_F_YYYY_MM_DD(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_3000(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                return false;
            }
            else
            {
                Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{1,3000}$");
                return expresion.IsMatch(cadena);
            }
        }

        public static bool cs_prSER_M_an_100(string cadena)/*Se esta usando para validar la Razon Social del Negocio*/
        {
            //Cristhian|03/01/2018|FEI2-491
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Por cada caracter especial que encuentre sera remplazado por el espacio en blanco
             (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Length == 0)
            {
                return false;
            }
            else
            {
                Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{1,100}$");
                return expresion.IsMatch(cadena);
            }
        }

        public static bool cs_prSER_C_an_15(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "1";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{0,15}$");
            return expresion.IsMatch(cadena);
        }

        //Cristhian|02/11/2017|FEI2-401
        /*Se añade una nueva validación para permitir Numeros, textos, Guion simple y SubGuion(Guion Bajo)*/
        //Cristhian|11/01/2018|FEI2-541
        /*Se extendio el limite de 50 a 150 caracteres como máximo*/
        /*INICIO MODIFICACIóN*/
        public static bool cs_prSER_C_an_50(string cadena)
        {
            //Cristhian|12/01/2018|FEI2-545
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Cada caracter especial que encuentre sera remplazado por el espacio en blanco
             (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Length == 0)
            {
                cadena = "1";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{0,150}$");
            return expresion.IsMatch(cadena);
        }
        /*FIN MODIFICACIóN*/

        public static bool cs_prSER_C_an_100(string cadena)
        {
            //Cristhian|03/01/2018|FEI2-491
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Cada caracter especial que encuentre sera remplazado por el espacio en blanco
             (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Length == 0)
            {
                cadena = "1";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{0,100}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an_6(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras + "{0,6}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an_25(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas + "{0,25}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an6(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{0,6}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an_30(string cadena)
        {
            //Cristhian|16/01/2018|FEI2-510
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Cada caracter especial que encuentre sera remplazado por el espacio en blanco
              (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas + "{0,150}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an2(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras + "{0,2}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_n11(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros + "{1,11}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_n_3(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros + "{1,3}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_n1(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros + "{1}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an2(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^" + er_solo_numeros_letras + "{2}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_13_FHHH_NNNNNNNN(string cadena)//¿F-seguido de 3 numeros, seguido de 8 Ns?
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9\-]{1,13}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_13(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{13}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_15(string cadena)
        {
            //Cristhian|10/01/2018|FEI2-538
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Cada caracter especial que encuentre sera remplazado por el espacio en blanco
              (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Trim().Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{1,15}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an1(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{1}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_3(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{1,3}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_16_F_n12c3(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[0-9]{1,12}(\.[0-9]{1,3})?$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_250(string cadena)
        {
            //Cristhian|03/01/2018|FEI2-491
            /*Los caracteres especiales son remplazados por un espacio en blanco en la cadena que se recibe de la validación*/
            /*NUEVO INICIO*/

            /*Cada caracter especial que encuentre sera remplazado por el espacio en blanco
              (Los caracteres especiales estan declaradas al inicio del codigo)*/
            foreach (char CEspecial in CaracteresEspeciales)
            {
                cadena = cadena.Replace(CEspecial, ' ');
            }
            /*NUEVO FIN*/

            if (cadena == null || cadena.Length == 0)
            {
                return false;
            }
            else
            {
                Regex expresion = new Regex(@"^" + er_solo_numeros_letras_espacios_letrasvocalestildadas_signospuntuacion + "{1,250}$");
                return expresion.IsMatch(cadena);
            }
        }

        public static bool cs_prSER_M_an_15_F_n12c2(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[0-9]{1,12}(\.[0-9]{1,2})?$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an4(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{4}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an_6(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{1,6}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_M_an3(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "<";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{3}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an_15_F_n12c2(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "123";
            }
            Regex expresion = new Regex(@"^[0-9]{1,12}(\.[0-9]{1,2})?$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an_18_F_n15c2(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "123";
            }
            Regex expresion = new Regex(@"^[0-9]{1,15}(\.[0-9]{1,2})?$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_Porcentaje(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "123";
            }
            Regex expresion = new Regex(@"^[0-9\.\,\%]{0,5}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an4(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{0,4}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_C_an3(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{0,3}$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prSER_an1(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{0,1}$");
            return expresion.IsMatch(cadena);
        }

        /** Validación de campos en general */
        public static bool cs_prEmail(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return expresion.IsMatch(cadena);
        }

        public static bool cs_prFEI_ID(string cadena)
        {
            if (cadena == null || cadena.Length == 0)
            {
                cadena = "A";
            }
            Regex expresion = new Regex(@"^[a-zA-Z0-9]{1,64}$");
            return expresion.IsMatch(cadena);
        }

        public static string Mensaje(bool valor)
        {
            string mensaje = null;
            if (valor == true)
            {
                mensaje = "Correcto";
            }
            else
            {
                mensaje = "Error";
            }
            return mensaje;
        }

        public static bool cs_prVacio(string cadena)
        {
            if (cadena.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        // Nuevos desarrollos
        public static string regexHoraFormato24h = @"^\d{2,2}(:)\d{2,2}(:)\d{2,2}$"; // Falta validar que no este en fuera de hora como 26:61:99
        /* Jorge Luis| 09/05/2018 | FEI2-322
          Método para crear un regex que valide dígitos, en un rango de cantidad de caracteres*/
        public string CreateRegex_ValidateDigitsInRanges(string firstRange, string secondRange)
        {
            string resultado = @"^\d{" + firstRange + "," + secondRange + "}$";
            return resultado;
        }
        /* Jorge Luis| 09/05/2018 | FEI2-322
          Método para validar un string según un regex,( valorAComprobar = el valor que la variable; valorRegex = el código regex para validar */
        public static bool ComprobarRegex(string valorAComprobar, string valorRegex)
        {
            bool resultado = false;
            if (valorAComprobar == null || valorAComprobar.Length == 0 || valorRegex == null || valorRegex.Length == 0)
                resultado = false;
            else
            {
                Regex expresion = new Regex(valorRegex);
                resultado = expresion.IsMatch(valorAComprobar);
            }
            return resultado;
        }
    }
}
