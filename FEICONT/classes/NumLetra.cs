using System;
using System.Text.RegularExpressions;

/// <summary>
/// Descripción breve de NumLetra
/// </summary>
namespace FEICONT
{
    public class NumLetra
    {
        private String[] UNIDADES = { "", "un ", "dos ", "tres ", "cuatro ", "cinco ", "seis ", "siete ", "ocho ", "nueve " };
        private String[] DECENAS = {"diez ", "once ", "doce ", "trece ", "catorce ", "quince ", "dieciseis ",
        "diecisiete ", "dieciocho ", "diecinueve", "veinte ", "treinta ", "cuarenta ",
        "cincuenta ", "sesenta ", "setenta ", "ochenta ", "noventa "};
        private String[] ESPECIAL = {"veintiuno ", "veintidos ", "veintitres ", "veinticuatro ", "veinticinco ", "veintiseis ", "veintisiete ",
        "veintiocho ", "veintinueve"};
        private String[] CENTENAS = {"", "ciento ", "doscientos ", "trecientos ", "cuatrocientos ", "quinientos ", "seiscientos ",
        "setecientos ", "ochocientos ", "novecientos "};

        private Regex r;

        /*Numeros en Ingles*/
        private String[] UNIT_USA = { "", "one ", "two ", "three ", "four ", "five ", "six ", "seven ", "eight ", "nine " };
        private String[] TEN_USA = {"ten ", "eleven ", "twelve ", "thirteen ", "fourteen ", "fifteen ", "sixteen ",
        "seventeen ", "eighteen ", "nineteen", "twenty ", "thirty ", "forty ", "fifty ", "sixty ", "seventy ", "eighty ", "ninety "};
        private String[] ESPECIAL_USA = {"hundred ","thousand ","million "};

        public NumLetra()
        {
        }

        public String Convertir(String numero, bool mayusculas, string moneda)
        {

            String literal = "";
            String parte_decimal;
            String literal_moneda = "";
            //si el numero utiliza (.) en lugar de (,) -> se reemplaza
            numero = numero.Replace(".", ",");

            //si el numero no tiene parte decimal, se le agrega ,00
            if (numero.IndexOf(",") == -1)
            {
                numero = numero + ",00";
            }
            //se valida formato de entrada -> 0,00 y 999 999 999,00
            r = new Regex(@"\d{1,9},\d{1,2}");
            MatchCollection mc = r.Matches(numero);
            if (mc.Count > 0)
            {
                //se divide el numero 0000000,00 -> entero y decimal
                String[] Num = numero.Split(',');

                literal_moneda = getMoneda(moneda);
                //de da formato al numero decimal
                parte_decimal = " con " + Num[1] + "/100 " + literal_moneda;
                //se convierte el numero a literal
                if (int.Parse(Num[0]) == 0)
                {//si el valor es cero                
                    literal = " cero ";
                }
                else if (int.Parse(Num[0]) > 999999)
                {//si es millon
                    literal = getMillones(Num[0]);
                }
                else if (int.Parse(Num[0]) > 999)
                {//si es miles
                    literal = getMiles(Num[0]);
                }
                else if (int.Parse(Num[0]) > 99)
                {//si es centena
                    literal = getCentenas(Num[0]);
                }
                else if (int.Parse(Num[0]) > 9)
                {//si es decena
                    literal = getDecenas(Num[0]);
                }
                else
                {//sino unidades -> 9
                    literal = getUnidades(Num[0]);
                }
                //devuelve el resultado en mayusculas o minusculas
                if (mayusculas)
                {
                    return (literal + parte_decimal).ToUpper();
                }
                else
                {
                    return (literal + parte_decimal);
                }
            }
            else
            {//error, no se puede convertir
                return literal = null;
            }
        }
        /* funciones para convertir los numeros a literales */

        private String getUnidades(String numero)
        {   // 1 - 9            
            //si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);
            return UNIDADES[int.Parse(num)];
        }

        private String getDecenas(String num)
        {// 99                        
            int n = int.Parse(num);
            if (n < 10)
            {//para casos como -> 01 - 09
                return getUnidades(num);
            }
            else if (n > 19)
            {//para 20...99
                if (n > 20 & n < 30)
                {
                    String u = getUnidades(num);

                    return ESPECIAL[int.Parse(num.Substring(1, 1)) - 1];

                }
                else
                {
                    String u = getUnidades(num);
                    if (u.Equals(""))
                    { //para 20,30,40,50,60,70,80,90
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8];
                    }
                    else
                    {
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + " y " + u;
                    }
                }

            }
            else
            {//numeros entre 11 y 19
                return DECENAS[n - 10];
            }
        }

        private String getCentenas(String num)
        {// 999 o 099
            if (int.Parse(num) > 99)
            {//es centena
                if (int.Parse(num) == 100)
                {//caso especial
                    return " cien ";
                }
                else
                {
                    return CENTENAS[int.Parse(num.Substring(0, 1))] + getDecenas(num.Substring(1));
                }
            }
            else
            {//por Ej. 099 
             //se quita el 0 antes de convertir a decenas
                return getDecenas(int.Parse(num) + "");
            }
        }

        private String getMiles(String numero)
        {// 999 999
         //obtiene las centenas
            String c = numero.Substring(numero.Length - 3);
            //obtiene los miles
            String m = numero.Substring(0, numero.Length - 3);
            String n = "";
            //se comprueba que miles tenga valor entero
            if (int.Parse(m) > 0)
            {
                n = getCentenas(m);
                return n + " mil " + getCentenas(c);
            }
            else
            {
                return "" + getCentenas(c);
            }

        }

        private String getMillones(String numero)
        { //000 000 000        
          //se obtiene los miles
            String miles = numero.Substring(numero.Length - 6);
            //se obtiene los millones
            String millon = numero.Substring(0, numero.Length - 6);
            String n = "";
            if (millon.Length > 1)
            {
                n = getCentenas(millon) + "millones ";
            }
            else
            {
                n = getUnidades(millon) + "millon ";
            }
            return n + getMiles(miles);
        }

        public string getMoneda(string codigo)
        {
            string moneda = "";
            if (codigo == "PEN")
            {
                moneda = "soles";
            }
            else if (codigo == "USD")
            {
                moneda = "dolares americanos";
            }
            else
            {
                moneda = "soles";
            }
            return moneda;
        }

        /*Numero en letras - USA*/
        public String Convertir_USA(String numero, bool mayusculas, string moneda)
        {

            String literal = "";
            String parte_decimal;
            String literal_moneda = "";
            string decena = "";
            string centena = "";
            string millar = "";
            string millon = "";

            //si el numero utiliza (.) en lugar de (,) -> se reemplaza
            numero = numero.Replace(".", ",");

            //si el numero no tiene parte decimal, se le agrega ,00
            if (numero.IndexOf(",") == -1)
            {
                numero = numero + ",00";
            }
            //se valida formato de entrada -> 0,00 y 999 999 999,00
            r = new Regex(@"\d{1,9},\d{1,2}");
            MatchCollection mc = r.Matches(numero);
            if (mc.Count > 0)
            {
                //se divide el numero 0000000,00 -> entero y decimal
                String[] Num = numero.Split(',');

                literal_moneda = getMoneda_USA(moneda);
                //de da formato al numero decimal
                parte_decimal = " " + Num[1] + "/100 " + literal_moneda;
                //se convierte el numero a literal
                if (int.Parse(Num[0]) == 0)
                {//si el valor es cero                
                    literal = " cero ";
                }

                else if (int.Parse(Num[0]) > 999999 && int.Parse(Num[0]) < 1000000000)
                {//si es millon
                    millon = getMillones_USA(Num[0]);
                    literal = millon;
                }

                else if (int.Parse(Num[0]) > 999)
                {//si es miles
                    millar = getMiles_USA(Num[0]);
                    literal = millar;
                }

                else if (int.Parse(Num[0]) > 99)
                {//si es centena
                    centena = getCentenas_USA(Num[0]);
                    literal = centena;
                }

                else if (int.Parse(Num[0]) > 9)
                {//si es decena
                    decena = getDecenas_USA(Num[0]);
                    decena = decena + ", " + getUnidades_USA(Num[0].Substring(Num[0].Length - 1, 1));
                    literal = decena;
                }
                else
                {//sino unidades -> 9
                    literal = getUnidades_USA(Num[0]);
                }
                //devuelve el resultado en mayusculas o minusculas
                if (mayusculas)
                {
                    return (literal + parte_decimal).ToUpper();
                }
                else
                {
                    return (literal + parte_decimal);
                }
            }
            else
            {//error, no se puede convertir
                return literal = null;
            }
        }

        /*Para numeros de USA*/
        private String getUnidades_USA(String numero)
        {   // 1 - 9            
            //si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);
            return UNIT_USA[int.Parse(num)];
        }

        private String getDecenas_USA(String num)
        {// 99                        
            int n = int.Parse(num);
            if (n < 10)
            {//para casos como -> 01 - 09
                return getUnidades_USA(num);
            }
            else if (10==n)
            {
                return TEN_USA[0];
            }
            else if (n > 19)
            {//para 20...99
                String u = getUnidades_USA(num);
                if (u.Equals(""))
                { //para 20,30,40,50,60,70,80,90
                    return TEN_USA[int.Parse(num.Substring(0, 1)) + 8];
                }
                else
                {
                    return TEN_USA[int.Parse(num.Substring(0, 1)) + 8] + u;
                }
            }
            else
            {//numeros entre 11 y 19
                return TEN_USA[n - 10];
            }
        }

        private String getCentenas_USA(String num)
        {// 999 o 099
            string centena = "";
            int numero = Convert.ToInt32(num);
            if (numero > 99 && numero < 1000)
            {
                centena = UNIT_USA[Convert.ToInt32(numero.ToString().Substring(0,1))] + ESPECIAL_USA[0];
                centena = centena + getDecenas_USA(numero.ToString().Substring(1,2));
                return centena;
            }
            else if (numero > 9 && numero < 99)
            {
                centena = centena + getDecenas_USA(numero.ToString());
                return centena;
            }
            else if (numero < 10 && numero > -1)
            {
                centena = centena + getUnidades_USA(numero.ToString());
                return centena;
            }
            else
            {
                return "";
            }
        }

        private String getMiles_USA(String num)
        {// 999 999
            string millar = "";
            int numero = Convert.ToInt32(num);
            if(numero < 1000000 && numero>99999)
            {
                millar = getCentenas_USA(numero.ToString().Substring(0, 1)) + ESPECIAL_USA[0];
                millar = millar + getDecenas_USA(numero.ToString().Substring(1, 2)) + ESPECIAL_USA[1];
                millar = millar + getCentenas_USA(numero.ToString().Substring(3, 3));
                return millar;
            }
            else if (numero < 100000 && numero > 9999)
            {
                millar = millar + getDecenas_USA(numero.ToString().Substring(0, 2)) + ESPECIAL_USA[1];
                millar = millar + getCentenas_USA(numero.ToString().Substring(2, 3));
                return millar;
            }
            else if (numero < 10000 && numero > 999)
            {
                millar= UNIT_USA[Convert.ToInt32(numero.ToString().Substring(0,1))] + ESPECIAL_USA[1];
                millar = millar + getCentenas_USA(numero.ToString().Substring(1,3));
                return millar;
            }
            else
            {
                return "";
            }
        }

        private String getMillones_USA(String num)
        { //000 000 000        
            string millon = "";
            int numero = Convert.ToInt32(num);
            if (numero < 1000000000 && numero > 99999999)
            {
                millon = getCentenas_USA(numero.ToString().Substring(0, 1)) + ESPECIAL_USA[0];
                millon = millon + getDecenas_USA(numero.ToString().Substring(1, 2)) + ESPECIAL_USA[2] + ",";
                millon = millon + getMiles_USA(numero.ToString().Substring(3, 6));
                //millon = millon + getCentenas_USA(numero.ToString().Substring(3, 3));
                return millon;
            }
            else if (numero < 100000000 && numero > 9999999)
            {
                millon = millon + getDecenas_USA(numero.ToString().Substring(0, 2)) + ESPECIAL_USA[2] + ",";
                millon = millon + getMiles_USA(numero.ToString().Substring(2, 6));
                //millon = millon + getCentenas_USA(numero.ToString().Substring(2, 3));
                return millon;
            }
            else if (numero < 10000000 && numero > 999999)
            {
                millon = UNIT_USA[Convert.ToInt32(numero.ToString().Substring(0, 1))] + ESPECIAL_USA[2] + ",";
                millon = millon + getMiles_USA(numero.ToString().Substring(1, 6));
                //millon = millon + getCentenas_USA(numero.ToString().Substring(1, 3));
                return millon;
            }
            else
            {
                return "";
            }
        }

        public string getMoneda_USA(string codigo)
        {
            string moneda = "";
            if (codigo == "PEN")
            {
                moneda = "pen soles";
            }
            else if (codigo == "USD")
            {
                moneda = "us dolars";
            }
            else
            {
                moneda = "pen soles";
            }
            return moneda;
        }
    }
}