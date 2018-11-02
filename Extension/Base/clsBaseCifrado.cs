using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Extension.Base
{
   
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseCifrado")]
    public class clsBaseCifrado
    { 
        /// <summary>
        /// Metodo de cifrado a string segun llave dada.
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="llave"></param>
        /// <returns>Cadena cifrada</returns>
        public static string cs_fxCifrar(string cadena, string llave)
        {
      
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] llave_cifrado = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llave));
            md5.Clear();
            byte[] arreglo_cadena = UTF8Encoding.UTF8.GetBytes(cadena);
            TripleDESCryptoServiceProvider a_3des = new TripleDESCryptoServiceProvider();
            a_3des.Key = llave_cifrado;
            a_3des.Mode = CipherMode.ECB;
            a_3des.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = a_3des.CreateEncryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo_cadena, 0, arreglo_cadena.Length);
            a_3des.Clear();
            return Convert.ToBase64String(resultado, 0, resultado.Length);
        }
        /// <summary>
        /// Metodo para descifrar una cadena.
        /// </summary>
        /// <param name="cadena"></param>
        /// <param name="llave"></param>
        /// <returns>Cadena sin cifrar</returns>
        public static string cs_fxDescifrar(string cadena, string llave)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] llave_cifrado = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(llave));
            md5.Clear();
            byte[] arreglo_cadena = Convert.FromBase64String(cadena); 
            TripleDESCryptoServiceProvider a_3des = new TripleDESCryptoServiceProvider();
            a_3des.Key = llave_cifrado;
            a_3des.Mode = CipherMode.ECB;
            a_3des.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = a_3des.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo_cadena, 0, arreglo_cadena.Length);
            a_3des.Clear();
            return UTF8Encoding.UTF8.GetString(resultado);
        }
        /// <summary>
        /// Metodo para generar una llave aleatoria.
        /// </summary>
        /// <param name="longitud"></param>
        /// <returns>Cadena de llave aleatoria</returns>
        public static string cd_pxLlaveAleatoria(int longitud)
        {
            char[] cadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#~€¡!{}[]¿?$%&()=".ToCharArray();
            byte[] caracter = new byte[1];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(caracter);
            caracter = new byte[longitud];
            rng.GetNonZeroBytes(caracter);
            StringBuilder llave = new StringBuilder(longitud);
            foreach (byte b in caracter)
            {
                llave.Append(cadena[b % (cadena.Length)]);
            }
            return llave.ToString();
        }
    }
}
