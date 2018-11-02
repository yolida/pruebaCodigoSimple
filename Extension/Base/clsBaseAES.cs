using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Base
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("clsBaseAES")]
    public class clsBaseAES
    {
        private double[] AesCajaS = new double[256];
        private int[] AesCajaIS = new int[256];
        private int[] AesTablaInv = new int[4];
        private int AesTbInvTemporal = 0;
        private string[,] AesCbTemporal = new string[4, 4];
        private string[,] AesBloqueCifrado = new string[4, 4];
        private string[,] AesExpKey = new string[4, 4];
        private string KeyTipoAES = String.Empty;
        private int NumeroRounds = 0;
        private int AesI = 0;
        private string ModoAESCipher = String.Empty;
        private string llave = "4asd2¨as°AÑLKJ/ü180É3b69?bñ[3{D'#$~%57f^";
        private int NumeroRaundsAux = 0;
        private string TextPlainTipo = "";
        public clsBaseAES()
        {
            sBoxes();
            InvSBoxes();
        }
        public string InvSubByte(string tnNumero)
        {
            //tnNumero 01101101" -> binario
            int numero = (int)BinDec(tnNumero);
            int caja = this.AesCajaIS[numero];
            string decimal_string = caja.ToString();
            string binario = DecBin(decimal_string);
            return binario;
        }
        public string SubByte(string tnByte)
        {
            //tnByte 0000000 -> binario string
            int numero = (int)BinDec(tnByte);
            double caja = AesCajaS[(int)BinDec(tnByte)];
            string decimal_string= caja.ToString();
            string binario = DecBin(decimal_string);
            return binario;
        }
        public double BinDec(string tcBinario)
        {
            //recibe binario devuelve decimal
            double lnDecimal;
            int lnExpo, lnContador;
            lnDecimal = 0;
            lnExpo = 0;
            for (lnContador = tcBinario.ToString().Length; lnContador >= 1; lnContador--)
            {
                lnDecimal = lnDecimal + (Convert.ToDouble(tcBinario.ToString().Substring(lnContador-1, 1)) * Math.Pow(2, lnExpo));
                lnExpo = lnExpo + 1;
            }
            return lnDecimal;
        }
        public string DecBin(string tnDecimal)
        {
            //recibe decimal devuelve binario
            decimal tnDecimal_1 = Convert.ToDecimal(tnDecimal);
            string lcCeros = "000000";
            string lcTeb = "";

            if (tnDecimal_1 >= 2)
            {
                do
                {
                    lcTeb = ((tnDecimal_1 % 2).ToString()).Trim() + lcTeb;

                    tnDecimal_1 = (int)(tnDecimal_1 / 2);
                }
                while
                (tnDecimal_1 >= 2);
                lcTeb = "1" + lcTeb;

                if (lcTeb.Length < 8)
                {
                    lcTeb = lcCeros.Substring(0, 8 - lcTeb.Length) + lcTeb;
                }
            }
            else
            {
                if (tnDecimal_1 == 0)
                {
                    lcTeb = "00000000";
                }
                else
                {
                    lcTeb = "00000001";
                }
            }
            return lcTeb;
        }
        public string decodificar(string tcMensaje)
        {
            string tcKey = this.llave;
            int lnAnchoMensaje = tcMensaje.Length;
            string lcRetornar = "";
            string lckey = "";
            int lnBloques;
            this.ModoAESCipher = "D";
            if (lnAnchoMensaje % 32 > 0)
            {
                return lcRetornar;
            }
            this.InvSBoxes();
            lckey = keyexpansion(tcKey);
            if (lckey != "/Error/")
            {
                lnBloques = (int)(lnAnchoMensaje / 32);

                for (int AES_D_C = 0; AES_D_C < lnBloques; AES_D_C++)
                {

                    lckey = this.Hacercipherblock(tcMensaje.Substring(32 * AES_D_C, 32));

                    if (lckey == "/Error/")
                    {
                        return lcRetornar;
                    }
                    this.invfinalroundaes();

                    for (int AES_D_R = 1; AES_D_R <= this.NumeroRounds; AES_D_R++)
                    {
                        this.InvRoundAes();
                    }

                    this.roundkeyaddition();
                    this.NumeroRaundsAux = this.AesI;
                    if (this.ModoAESCipher == "DH")
                    {
                        lcRetornar = lcRetornar + this.Hacersalidahex();
                    }
                    else
                    {
                        lcRetornar = lcRetornar + this.Hacersalida();
                    }
                }
            }
            return lcRetornar;
        }
        public string Hacersalida()
        {
            string lcRetornar;
            int lnContador, lnContadorAux;
            lcRetornar = "";
            for (lnContador = 0; lnContador < 4; lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    if(this.AesBloqueCifrado[lnContadorAux, lnContador] == "00000000")
                    {
                        return lcRetornar;
                    }
                    else
                    {
                        double resultado = BinDec(AesBloqueCifrado[lnContadorAux, lnContador]);
                        int unicode = (int)resultado;
                        char character = (char)unicode;
                        string text = character.ToString();

                       // char cd = Char.ToString(resultado);
                        lcRetornar = lcRetornar + text;
                    }
                }
            }
            return lcRetornar;
        }
        public string Hacersalidahex()
        {
            string lcRetornar;
            int lnContador, lnContadorAux;
            lcRetornar = "";
            for (lnContador = 0; lnContador < 4; lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    lcRetornar = lcRetornar + this.StrHex(this.BinDec(this.AesBloqueCifrado[lnContadorAux, lnContador]));
                }
           }

            return lcRetornar;
        }
        public string StrHex(double tcCadena)
        {
            string cadena = "0123456789ABCDEF";
            string retorno = cadena.Substring((int)(tcCadena / 16), 1) + cadena.Substring((int)(tcCadena % 16), 1);
            return retorno;
        }
        public void invmixcolumn()
        {
            int lnContador, lnContadorAux, lnContadorAux2;

            for (lnContador=0;lnContador<4;lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    this.AesCbTemporal[lnContador, lnContadorAux] = this.AesBloqueCifrado[lnContador, lnContadorAux];
                }     
            }
            for (lnContador=0;lnContador<4;lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    this.AesBloqueCifrado[lnContadorAux, lnContador] = this.bytexor(this.bytexor(this.bytexor(this.multporx(this.AesCbTemporal[0, lnContador], this.AesTablaInv[0]), this.multporx(this.AesCbTemporal[1, lnContador], this.AesTablaInv[1])), this.multporx(this.AesCbTemporal[2, lnContador], this.AesTablaInv[2])), this.multporx(this.AesCbTemporal[3, lnContador], this.AesTablaInv[3]));
                    for (lnContadorAux2 = 3; lnContadorAux2 >= 1; lnContadorAux2--) {
                        this.AesTablaInv[lnContadorAux2] = this.AesTablaInv[lnContadorAux2 - 1];
                    }
                    this.AesTablaInv[0] = this.AesTbInvTemporal;
                    this.AesTbInvTemporal = this.AesTablaInv[3];
                }
            }
        }
        public string xtime(string tcBinario)
        {
            string nuevo = tcBinario + "0";

            if(tcBinario.Substring(0, 1) == "1")
            {
                return nuevo.Substring(1, 8);
            }
            else
            {
                return nuevo.Substring(1, 8);

            }         
        }
        public string multporx(string tcByteBinario,int tnX)
        {        
           
            string lvM2, lvM4, lvM8;
            if (tnX > 1 && tnX <= 3)
            {
                lvM2 = this.xtime(tcByteBinario);
                if (tnX == 2) {
                    tcByteBinario = lvM2;
                }
                else
                {
                    tcByteBinario = this.bytexor(lvM2, tcByteBinario);
                }
      
            }
            else
            {
                lvM2 = this.xtime(tcByteBinario);
                lvM4 = this.xtime(lvM2);
                lvM8 = this.xtime(lvM4);
                switch (tnX)
                {
                    case 9:
                        tcByteBinario = this.bytexor(lvM8, tcByteBinario);
                        break;
                    case 11:
                        tcByteBinario = this.bytexor(this.bytexor(lvM2, lvM8), tcByteBinario);
                        break;
                    case 13:
                        tcByteBinario = this.bytexor(this.bytexor(lvM4, lvM8), tcByteBinario);
                        break;
                    case 14:
                        tcByteBinario = this.bytexor(this.bytexor(lvM4, lvM8), lvM2);
                        break;
                }
       
            }
           return tcByteBinario;
        }
        public void InvRoundAes()
        {
            string lcShiftRow;
            int lnContador, lnContadorAux;
            lcShiftRow = "";
            this.roundkeyaddition();
            this.invmixcolumn();

            lcShiftRow = this.AesBloqueCifrado[1, 3] + this.AesBloqueCifrado[1, 0] + this.AesBloqueCifrado[1, 1] + this.AesBloqueCifrado[1, 2];
            for (lnContador=0;lnContador<4;lnContador++)
            {
                this.AesBloqueCifrado[1, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }

            lcShiftRow = this.AesBloqueCifrado[2, 2] + this.AesBloqueCifrado[2, 3] + this.AesBloqueCifrado[2, 0] + this.AesBloqueCifrado[2, 1];
            for (lnContador = 0; lnContador < 4; lnContador++) {
                this.AesBloqueCifrado[2, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }

            lcShiftRow = this.AesBloqueCifrado[3, 1] + this.AesBloqueCifrado[3, 2] + this.AesBloqueCifrado[3, 3] + this.AesBloqueCifrado[3, 0];
            for (lnContador = 0; lnContador < 4; lnContador++) {
                this.AesBloqueCifrado[3, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }
            for (lnContador = 0; lnContador < 4; lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    this.AesBloqueCifrado[lnContador, lnContadorAux] = this.InvSubByte(this.AesBloqueCifrado[lnContador, lnContadorAux]);
                }
            }
        }
        public void roundkeyaddition()
        {
            int lnContador, lnContadorAux;
            if (this.ModoAESCipher == "E")
            {
                for(lnContador = 0; lnContador < 4; lnContador++)
                {
                    for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                    {
                        this.AesBloqueCifrado[lnContadorAux, lnContador] = this.bytexor(this.AesBloqueCifrado[lnContadorAux, lnContador], this.AesExpKey[this.NumeroRaundsAux, lnContadorAux]);
                    }
                    this.NumeroRaundsAux = this.NumeroRaundsAux + 1;
                }         
            }
            else
            {
                for (lnContador = 3; lnContador >= 0; lnContador--)
                {
                    for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                    {
                        this.AesBloqueCifrado[lnContadorAux, lnContador] = this.bytexor(this.AesBloqueCifrado[lnContadorAux, lnContador], this.AesExpKey[this.NumeroRaundsAux-1, lnContadorAux]);
                    }
                    this.NumeroRaundsAux = this.NumeroRaundsAux - 1;
                }
      
            }
        }
        public void invfinalroundaes()
        {
            string lcShiftRow;
            int lnContador, lnContadorAux;

            lcShiftRow = "";
            this.roundkeyaddition();
            lcShiftRow = this.AesBloqueCifrado[1, 3] + this.AesBloqueCifrado[1, 0] + this.AesBloqueCifrado[1, 1] + this.AesBloqueCifrado[1, 2];

            for(lnContador = 0; lnContador < 4; lnContador++)
            {
                this.AesBloqueCifrado[1, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }
            lcShiftRow = this.AesBloqueCifrado[2, 2] + this.AesBloqueCifrado[2, 3] + this.AesBloqueCifrado[2, 0] + this.AesBloqueCifrado[2, 1];
            for( lnContador = 0; lnContador < 4; lnContador++)
            {
                this.AesBloqueCifrado[2, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }
            lcShiftRow = this.AesBloqueCifrado[3, 1] + this.AesBloqueCifrado[3, 2] + this.AesBloqueCifrado[3, 3] + this.AesBloqueCifrado[3, 0];

            for(lnContador = 0; lnContador < 4; lnContador++)
            {
                this.AesBloqueCifrado[3, lnContador] = lcShiftRow.Substring(8 * lnContador, 8);
            }
            for (lnContador = 0; lnContador < 4; lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    this.AesBloqueCifrado[lnContador, lnContadorAux] = this.InvSubByte(this.AesBloqueCifrado[lnContador, lnContadorAux]);
                }
            }
        }
        public string Hacercipherblock(string tcCadena)
        {
            int lnAnchoCadena, lnContador, lnContadorAux;
            string[] laBloque = new string[16];
            lnAnchoCadena = tcCadena.Length;
            if (lnAnchoCadena == 0)
            {
                for (lnContador = 0; lnContador < 4; lnContador++)
                {
                    for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                    {
                        this.AesBloqueCifrado[lnContador, lnContadorAux] = "00000000";
                    }

                }
            }
            else
            {
                if (this.ModoAESCipher == "D" || this.TextPlainTipo == "/H")
                {
                    if (lnAnchoCadena % 2 == 1)
                    {
                        tcCadena = tcCadena + "0";
                        lnAnchoCadena = lnAnchoCadena + 1;
                    }

                    if (lnAnchoCadena < 32)
                    {
                        for (lnContador = 1; lnContador <= (32-lnAnchoCadena); lnContador++)
                        {
                            tcCadena = tcCadena + "0";
                        }
                    }
                    for (lnContador = 0; lnContador < 4; lnContador++)
                    {
                        for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                        {
                            this.AesBloqueCifrado[lnContadorAux, lnContador] = this.hexDec(tcCadena.Substring((8 * (lnContador)) + 2 * lnContadorAux, 2));
                            if (this.AesBloqueCifrado[lnContadorAux, lnContador] == "-1")
                            {
                                return "/Error/";
                            }
                            else
                            {
                                string result = Convert.ToInt32(this.AesBloqueCifrado[lnContadorAux, lnContador], 16).ToString();
                                this.AesBloqueCifrado[lnContadorAux, lnContador] = DecBin(result);
                            }

                        }
                    }

                }
                else
                {
                    for (lnContador = 0; lnContador < lnAnchoCadena; lnContador++)
                    {
                        string d = tcCadena.Substring(lnContador, 1);
                        decimal f = Convert.ToChar(d);
                        laBloque[lnContador] = this.DecBin(f.ToString());
                    }
                    if (lnAnchoCadena < 16)
                    {
                        for (lnContador = lnAnchoCadena; lnContador < 16; lnContador++)
                        {
                            laBloque[lnContador] = "00000000";
                        }
                    }
                    for (lnContador = 0; lnContador < 4; lnContador++)
                    {
                        for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                        {
                            this.AesBloqueCifrado[lnContadorAux, lnContador] = laBloque[4 * (lnContador) + lnContadorAux];
                        }
                    }

                }

            }
            return "Bien";
        }
        void ResizeArray<T>(ref T[,] original, int newCoNum, int newRoNum)
        {
            var newArray = new T[newCoNum, newRoNum];
            int columnCount = original.GetLength(1);
            int columnCount2 = newRoNum;
            int columns = original.GetUpperBound(0);
            for (int co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
            original = newArray;
        }
        public string bytexor(string tcBinarioA, string tcBinarioB)
        {
            double dA1 = BinDec(tcBinarioA);
            double dA2 = BinDec(tcBinarioB);
            byte[] a = BitConverter.GetBytes(BinDec(tcBinarioA));
            string result1 = System.Text.Encoding.UTF8.GetString(a);
            byte[] b = BitConverter.GetBytes(BinDec(tcBinarioB));
            string result2 = System.Text.Encoding.UTF8.GetString(b);

            string c = exclusiveOR(tcBinarioA, tcBinarioB);
            string result = Convert.ToInt32(c,2).ToString();
           // return this.DecBin(System.Text.Encoding.UTF8.GetString(c));
            return this.DecBin(result);

        }
        public static string exclusiveOR(string arr1,string arr2)
        {
            string resultado = "";
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException("arr1 and arr2 are not the same length");
            }

            byte[] result = new byte[arr1.Length];

            for (int i = 0; i < arr1.Length; ++i)
            {
                string valor1 = arr1.Substring(i, 1);
                string valor2 = arr2.Substring(i, 1);

                if (valor1 == valor2)
                {
                    resultado += "0";
                }
                else
                {
                    resultado += "1";
                }
            }
              

            return resultado;
        }
        public static byte[] exclusiveOsR(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
            {
                throw new ArgumentException("arr1 and arr2 are not the same length");
            }

            byte[] result = new byte[arr1.Length];

            for (int i = 0; i < arr1.Length; ++i)
                result[i] = (byte)(arr1[i] ^ arr2[i]);

            return result;
        }

        public string keyexpansion(string tcKey)
        {
            string lcByteTemp;
            int lnAnchoKey, lnAnchoKeyAux, lnNk, lnContador, lnContadorAux;
            lnAnchoKey = tcKey.Length;
            string[] laKeTemporal = new string[4];
            string[] laKeContador = new string[10];
            laKeContador[0] = "00000001";
            laKeContador[1] = "00000010";
            laKeContador[2] = "00000100";
            laKeContador[3] = "00001000";
            laKeContador[4] = "00010000";
            laKeContador[5] = "00100000";
            laKeContador[6] = "01000000";
            laKeContador[7] = "10000000";
            laKeContador[8] = "00011011";
            laKeContador[9] = "00110110";

            if (this.KeyTipoAES != "/H")
            {
                if (lnAnchoKey > 32)
                {
                    lnAnchoKeyAux = 44;
                    lnNk = 11;
                    this.NumeroRounds = 16;
                }
                else
                {
                    if (lnAnchoKey > 28)
                    {
                        lnAnchoKeyAux = 32;
                        lnNk = 8;
                        this.NumeroRounds = 13;
                    }
                    else
                    {
                        lnAnchoKeyAux = 28;
                        lnNk = 7;
                        this.NumeroRounds = 12;
                    }
                }
            }
            else
            {
                if (lnAnchoKey > 64)
                {
                    lnAnchoKeyAux = 44;
                    lnNk = 11;
                    this.NumeroRounds = 16;
                }
                else
                {
                    if (lnAnchoKey > 56)
                    {
                        lnAnchoKeyAux = 32;
                        lnNk = 8;
                        this.NumeroRounds = 13;
                    }
                    else
                    {
                        lnAnchoKeyAux = 28;
                        lnNk = 7;
                        this.NumeroRounds = 12;
                    }
                }
            }

            this.AesI = 4 * (this.NumeroRounds + 2);
            if (this.ModoAESCipher == "E")
            {
                this.NumeroRaundsAux = 1;
            }
            else
            {
                this.NumeroRaundsAux = this.AesI;
            }
            ResizeArray(ref AesExpKey, this.AesI,4);
            //DIMENSION this.AesExpKey[this.AesI, 4]
            string[] laKeyB = new string[lnAnchoKeyAux];

            if (lnAnchoKey == 0)
            {
                for (lnContador = 0; lnContador < 28; lnContador++)
                {
                    laKeyB[lnContador] = "00000000";
                }
            }
            if (this.KeyTipoAES != "/H")
            {

                for (lnContador = 0; lnContador < lnAnchoKey; lnContador++)
                {
                    string d = tcKey.Substring(lnContador, 1);
                    decimal f = Convert.ToChar(d);
                    byte[] s = Encoding.Unicode.GetBytes(d);
                    laKeyB[lnContador] = this.DecBin(f.ToString());
                }

                if (lnAnchoKey < lnAnchoKeyAux)
                {
                    for (lnContador = lnAnchoKey; lnContador < lnAnchoKeyAux; lnContador++)
                    {
                        laKeyB[lnContador] = "00000000";
                    }
                }
            }
            else
            {
                if (lnAnchoKey % 2 == 1)
                {
                    tcKey = "0" + tcKey;
                    lnAnchoKey = lnAnchoKey + 1;
                }
                for (lnContador = 0; lnContador < (int)(lnAnchoKey / 2); lnContador++)
                {
                    laKeyB[lnContador] = this.hexDec(tcKey.Substring(2 * lnContador, 2));
                    if (laKeyB[lnContador] == "-1")
                    {
                        return "/Error/";
                    }
                    else
                    {
                        laKeyB[lnContador] = this.DecBin(laKeyB[lnContador]);
                    }
                }
                if ((int)(lnAnchoKey / 2) < lnAnchoKeyAux)
                {
                    for (lnContador = (lnAnchoKey / 2) ; lnContador < lnAnchoKeyAux; lnContador++)
                    {
                        laKeyB[lnContador] = "00000000";
                    }

                }
            }

            for (lnContador = 0; lnContador < lnNk; lnContador++)
            {
                this.AesExpKey[lnContador, 0] = laKeyB[(4 * lnContador)];
                this.AesExpKey[lnContador, 1] = laKeyB[4 * lnContador + 1];
                this.AesExpKey[lnContador, 2] = laKeyB[4 * lnContador + 2];
                this.AesExpKey[lnContador, 3] = laKeyB[4 * lnContador + 3];
            }
            for (lnContador = lnNk ; lnContador < this.AesI; lnContador++)
            {
                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    laKeTemporal[lnContadorAux] = this.AesExpKey[lnContador-1, lnContadorAux];
                }
                if (lnContador % lnNk == 0)
                {
                    lcByteTemp = this.SubByte(laKeTemporal[0]);
                    for (lnContadorAux = 0; lnContadorAux < 3; lnContadorAux++)
                    {
                        laKeTemporal[lnContadorAux] = this.SubByte(laKeTemporal[lnContadorAux+1]);
                    }
                    laKeTemporal[3] = lcByteTemp;
                    laKeTemporal[0] = this.bytexor(laKeTemporal[0], laKeContador[Convert.ToInt32(lnContador / lnNk)-1]);
                }
                else
                {
                    if (lnContador % 4 == 0)
                    {
                        for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                        {
                            laKeTemporal[lnContadorAux] = this.SubByte(laKeTemporal[lnContadorAux]);
                        }
                    }
                }

                for (lnContadorAux = 0; lnContadorAux < 4; lnContadorAux++)
                {
                    this.AesExpKey[lnContador, lnContadorAux] = this.bytexor(this.AesExpKey[lnContador - lnNk, lnContadorAux], laKeTemporal[lnContadorAux]);
                }
            }

            return "Bien";
        }

        public string hexDec(string tcNumeroHex)
        {
            tcNumeroHex = "0x" + tcNumeroHex.PadLeft(8, '0');

            return tcNumeroHex;
        }
        public void sBoxes()
        {
            this.AesCajaS[0] = 99;
            this.AesCajaS[1] = 124;
            this.AesCajaS[2] = 119;
            this.AesCajaS[3] = 123;
            this.AesCajaS[4] = 242;
            this.AesCajaS[5] = 107;
            this.AesCajaS[6] = 111;
            this.AesCajaS[7] = 197;
            this.AesCajaS[8] = 48;
            this.AesCajaS[9] = 1;
            this.AesCajaS[10] = 103;
            this.AesCajaS[11] = 43;
            this.AesCajaS[12] = 254;
            this.AesCajaS[13] = 215;
            this.AesCajaS[14] = 171;
            this.AesCajaS[15] = 118;
            this.AesCajaS[16] = 202;
            this.AesCajaS[17] = 130;
            this.AesCajaS[18] = 201;
            this.AesCajaS[19] = 125;
            this.AesCajaS[20] = 25;
            this.AesCajaS[21] = 89;
            this.AesCajaS[22] = 71;
            this.AesCajaS[23] = 240;
            this.AesCajaS[24] = 173;
            this.AesCajaS[25] = 212;
            this.AesCajaS[26] = 162;
            this.AesCajaS[27] = 175;
            this.AesCajaS[28] = 156;
            this.AesCajaS[29] = 164;
            this.AesCajaS[30] = 114;
            this.AesCajaS[31] = 192;
            this.AesCajaS[32] = 183;
            this.AesCajaS[33] = 253;
            this.AesCajaS[34] = 147;
            this.AesCajaS[35] = 38;
            this.AesCajaS[36] = 54;
            this.AesCajaS[37] = 63;
            this.AesCajaS[38] = 247;
            this.AesCajaS[39] = 204;
            this.AesCajaS[40] = 52;
            this.AesCajaS[41] = 165;
            this.AesCajaS[42] = 229;
            this.AesCajaS[43] = 241;
            this.AesCajaS[44] = 113;
            this.AesCajaS[45] = 216;
            this.AesCajaS[46] = 49;
            this.AesCajaS[47] = 21;
            this.AesCajaS[45] = 4;
            this.AesCajaS[49] = 199;
            this.AesCajaS[50] = 35;
            this.AesCajaS[51] = 195;
            this.AesCajaS[52] = 24;
            this.AesCajaS[53] = 150;
            this.AesCajaS[54] = 5;
            this.AesCajaS[55] = 154;
            this.AesCajaS[56] = 7;
            this.AesCajaS[57] = 18;
            this.AesCajaS[58] = 128;
            this.AesCajaS[59] = 226;
            this.AesCajaS[60] = 235;
            this.AesCajaS[61] = 39;
            this.AesCajaS[62] = 178;
            this.AesCajaS[63] = 117;
            this.AesCajaS[64] = 9;
            this.AesCajaS[65] = 131;
            this.AesCajaS[66] = 44;
            this.AesCajaS[67] = 26;
            this.AesCajaS[68] = 27;
            this.AesCajaS[69] = 110;
            this.AesCajaS[70] = 90;
            this.AesCajaS[71] = 160;
            this.AesCajaS[72] = 82;
            this.AesCajaS[73] = 59;
            this.AesCajaS[74] = 214;
            this.AesCajaS[75] = 179;
            this.AesCajaS[76] = 41;
            this.AesCajaS[77] = 227;
            this.AesCajaS[78] = 47;
            this.AesCajaS[79] = 132;
            this.AesCajaS[80] = 83;
            this.AesCajaS[81] = 209;
            this.AesCajaS[82] = 0;
            this.AesCajaS[83] = 237;
            this.AesCajaS[84] = 32;
            this.AesCajaS[85] = 252;
            this.AesCajaS[86] = 177;
            this.AesCajaS[87] = 91;
            this.AesCajaS[88] = 106;
            this.AesCajaS[89] = 203;
            this.AesCajaS[90] = 190;
            this.AesCajaS[91] = 57;
            this.AesCajaS[92] = 74;
            this.AesCajaS[93] = 76;
            this.AesCajaS[94] = 88;
            this.AesCajaS[95] = 207;
            this.AesCajaS[96] = 208;
            this.AesCajaS[97] = 239;
            this.AesCajaS[98] = 170;
            this.AesCajaS[99] = 251;
            this.AesCajaS[100] = 67;
            this.AesCajaS[101] = 77;
            this.AesCajaS[102] = 51;
            this.AesCajaS[103] = 133;
            this.AesCajaS[104] = 69;
            this.AesCajaS[105] = 249;
            this.AesCajaS[106] = 2;
            this.AesCajaS[107] = 127;
            this.AesCajaS[108] = 80;
            this.AesCajaS[109] = 60;
            this.AesCajaS[110] = 159;
            this.AesCajaS[111] = 168;
            this.AesCajaS[112] = 81;
            this.AesCajaS[113] = 163;
            this.AesCajaS[114] = 64;
            this.AesCajaS[115] = 143;
            this.AesCajaS[116] = 146;
            this.AesCajaS[117] = 157;
            this.AesCajaS[118] = 56;
            this.AesCajaS[119] = 245;
            this.AesCajaS[120] = 188;
            this.AesCajaS[121] = 182;
            this.AesCajaS[122] = 218;
            this.AesCajaS[123] = 33;
            this.AesCajaS[124] = 16;
            this.AesCajaS[125] = 255;
            this.AesCajaS[126] = 243;
            this.AesCajaS[127] = 210;
            this.AesCajaS[128] = 205;
            this.AesCajaS[129] = 12;
            this.AesCajaS[130] = 19;
            this.AesCajaS[131] = 236;
            this.AesCajaS[132] = 95;
            this.AesCajaS[133] = 151;
            this.AesCajaS[134] = 68;
            this.AesCajaS[135] = 23;
            this.AesCajaS[136] = 196;
            this.AesCajaS[137] = 167;
            this.AesCajaS[138] = 126;
            this.AesCajaS[139] = 61;
            this.AesCajaS[140] = 100;
            this.AesCajaS[141] = 93;
            this.AesCajaS[142] = 25;
            this.AesCajaS[143] = 115;
            this.AesCajaS[144] = 96;
            this.AesCajaS[145] = 129;
            this.AesCajaS[146] = 79;
            this.AesCajaS[147] = 220;
            this.AesCajaS[148] = 34;
            this.AesCajaS[149] = 42;
            this.AesCajaS[150] = 144;
            this.AesCajaS[151] = 136;
            this.AesCajaS[152] = 70;
            this.AesCajaS[153] = 238;
            this.AesCajaS[154] = 184;
            this.AesCajaS[155] = 20;
            this.AesCajaS[156] = 222;
            this.AesCajaS[157] = 94;
            this.AesCajaS[158] = 11;
            this.AesCajaS[159] = 219;
            this.AesCajaS[160] = 224;
            this.AesCajaS[161] = 50;
            this.AesCajaS[162] = 58;
            this.AesCajaS[163] = 10;
            this.AesCajaS[164] = 73;
            this.AesCajaS[165] = 6;
            this.AesCajaS[166] = 36;
            this.AesCajaS[167] = 92;
            this.AesCajaS[168] = 194;
            this.AesCajaS[169] = 211;
            this.AesCajaS[170] = 172;
            this.AesCajaS[171] = 98;
            this.AesCajaS[172] = 145;
            this.AesCajaS[173] = 149;
            this.AesCajaS[174] = 228;
            this.AesCajaS[175] = 121;
            this.AesCajaS[176] = 231;
            this.AesCajaS[177] = 200;
            this.AesCajaS[178] = 55;
            this.AesCajaS[179] = 109;
            this.AesCajaS[180] = 141;
            this.AesCajaS[181] = 213;
            this.AesCajaS[182] = 78;
            this.AesCajaS[183] = 169;
            this.AesCajaS[184] = 108;
            this.AesCajaS[185] = 86;
            this.AesCajaS[186] = 244;
            this.AesCajaS[187] = 234;
            this.AesCajaS[188] = 101;
            this.AesCajaS[189] = 122;
            this.AesCajaS[190] = 174;
            this.AesCajaS[191] = 8;
            this.AesCajaS[192] = 186;
            this.AesCajaS[193] = 120;
            this.AesCajaS[194] = 37;
            this.AesCajaS[195] = 46;
            this.AesCajaS[196] = 28;
            this.AesCajaS[197] = 166;
            this.AesCajaS[198] = 180;
            this.AesCajaS[199] = 198;
            this.AesCajaS[200] = 232;
            this.AesCajaS[201] = 221;
            this.AesCajaS[202] = 116;
            this.AesCajaS[203] = 31;
            this.AesCajaS[204] = 75;
            this.AesCajaS[205] = 189;
            this.AesCajaS[206] = 139;
            this.AesCajaS[207] = 138;
            this.AesCajaS[208] = 112;
            this.AesCajaS[209] = 62;
            this.AesCajaS[210] = 181;
            this.AesCajaS[211] = 102;
            this.AesCajaS[212] = 72;
            this.AesCajaS[213] = 3;
            this.AesCajaS[214] = 246;
            this.AesCajaS[215] = 14;
            this.AesCajaS[216] = 97;
            this.AesCajaS[217] = 53;
            this.AesCajaS[218] = 87;
            this.AesCajaS[219] = 185;
            this.AesCajaS[220] = 134;
            this.AesCajaS[221] = 193;
            this.AesCajaS[222] = 29;
            this.AesCajaS[223] = 158;
            this.AesCajaS[224] = 225;
            this.AesCajaS[225] = 248;
            this.AesCajaS[226] = 152;
            this.AesCajaS[227] = 17;
            this.AesCajaS[228] = 105;
            this.AesCajaS[229] = 217;
            this.AesCajaS[230] = 142;
            this.AesCajaS[231] = 148;
            this.AesCajaS[232] = 155;
            this.AesCajaS[233] = 30;
            this.AesCajaS[234] = 135;
            this.AesCajaS[235] = 233;
            this.AesCajaS[236] = 206;
            this.AesCajaS[237] = 85;
            this.AesCajaS[238] = 40;
            this.AesCajaS[239] = 223;
            this.AesCajaS[240] = 140;
            this.AesCajaS[241] = 161;
            this.AesCajaS[242] = 137;
            this.AesCajaS[243] = 13;
            this.AesCajaS[244] = 191;
            this.AesCajaS[245] = 230;
            this.AesCajaS[246] = 66;
            this.AesCajaS[247] = 104;
            this.AesCajaS[248] = 65;
            this.AesCajaS[249] = 153;
            this.AesCajaS[250] = 45;
            this.AesCajaS[251] = 15;
            this.AesCajaS[252] = 176;
            this.AesCajaS[253] = 84;
            this.AesCajaS[254] = 187;
            this.AesCajaS[255] = 22;
        }
        public void InvSBoxes()
        {
            this.AesTablaInv[0] = 14;
            this.AesTablaInv[1] = 11;
            this.AesTablaInv[2] = 13;
            this.AesTablaInv[3] = 9;
            this.AesTbInvTemporal = this.AesTablaInv[3];
            this.AesCajaIS[0] = 82;
            this.AesCajaIS[1] = 9;
            this.AesCajaIS[2] = 106;
            this.AesCajaIS[3] = 213;
            this.AesCajaIS[4] = 48;
            this.AesCajaIS[5] = 54;
            this.AesCajaIS[6] = 165;
            this.AesCajaIS[7] = 56;
            this.AesCajaIS[8] = 191;
            this.AesCajaIS[9] = 64;
            this.AesCajaIS[10] = 163;
            this.AesCajaIS[11] = 158;
            this.AesCajaIS[12] = 129;
            this.AesCajaIS[13] = 243;
            this.AesCajaIS[14] = 215;
            this.AesCajaIS[15] = 251;
            this.AesCajaIS[16] = 124;
            this.AesCajaIS[17] = 227;
            this.AesCajaIS[18] = 57;
            this.AesCajaIS[19] = 130;
            this.AesCajaIS[20] = 155;
            this.AesCajaIS[21] = 47;
            this.AesCajaIS[22] = 255;
            this.AesCajaIS[23] = 135;
            this.AesCajaIS[24] = 52;
            this.AesCajaIS[25] = 142;
            this.AesCajaIS[26] = 67;
            this.AesCajaIS[27] = 68;
            this.AesCajaIS[28] = 196;
            this.AesCajaIS[29] = 222;
            this.AesCajaIS[30] = 233;
            this.AesCajaIS[31] = 203;
            this.AesCajaIS[32] = 84;
            this.AesCajaIS[33] = 123;
            this.AesCajaIS[34] = 148;
            this.AesCajaIS[35] = 50;
            this.AesCajaIS[36] = 166;
            this.AesCajaIS[37] = 194;
            this.AesCajaIS[38] = 35;
            this.AesCajaIS[39] = 61;
            this.AesCajaIS[40] = 238;
            this.AesCajaIS[41] = 76;
            this.AesCajaIS[42] = 149;
            this.AesCajaIS[43] = 11;
            this.AesCajaIS[44] = 66;
            this.AesCajaIS[45] = 250;
            this.AesCajaIS[46] = 195;
            this.AesCajaIS[47] = 78;
            this.AesCajaIS[48] = 8;
            this.AesCajaIS[49] = 46;
            this.AesCajaIS[50] = 161;
            this.AesCajaIS[51] = 102;
            this.AesCajaIS[52] = 40;
            this.AesCajaIS[53] = 217;
            this.AesCajaIS[54] = 36;
            this.AesCajaIS[55] = 178;
            this.AesCajaIS[56] = 118;
            this.AesCajaIS[57] = 91;
            this.AesCajaIS[58] = 162;
            this.AesCajaIS[59] = 73;
            this.AesCajaIS[60] = 109;
            this.AesCajaIS[61] = 139;
            this.AesCajaIS[62] = 209;
            this.AesCajaIS[63] = 37;
            this.AesCajaIS[64] = 114;
            this.AesCajaIS[65] = 248;
            this.AesCajaIS[66] = 246;
            this.AesCajaIS[67] = 100;
            this.AesCajaIS[68] = 134;
            this.AesCajaIS[69] = 104;
            this.AesCajaIS[70] = 152;
            this.AesCajaIS[71] = 22;
            this.AesCajaIS[72] = 212;
            this.AesCajaIS[73] = 164;
            this.AesCajaIS[74] = 92;
            this.AesCajaIS[75] = 204;
            this.AesCajaIS[76] = 93;
            this.AesCajaIS[77] = 101;
            this.AesCajaIS[78] = 182;
            this.AesCajaIS[79] = 146;
            this.AesCajaIS[80] = 108;
            this.AesCajaIS[81] = 112;
            this.AesCajaIS[82] = 72;
            this.AesCajaIS[83] = 80;
            this.AesCajaIS[84] = 253;
            this.AesCajaIS[85] = 237;
            this.AesCajaIS[86] = 185;
            this.AesCajaIS[87] = 218;
            this.AesCajaIS[88] = 94;
            this.AesCajaIS[89] = 21;
            this.AesCajaIS[90] = 70;
            this.AesCajaIS[91] = 87;
            this.AesCajaIS[92] = 167;
            this.AesCajaIS[93] = 141;
            this.AesCajaIS[94] = 157;
            this.AesCajaIS[95] = 132;
            this.AesCajaIS[96] = 144;
            this.AesCajaIS[97] = 216;
            this.AesCajaIS[98] = 171;
            this.AesCajaIS[99] = 0;
            this.AesCajaIS[100] = 140;
            this.AesCajaIS[101] = 188;
            this.AesCajaIS[102] = 211;
            this.AesCajaIS[103] = 10;
            this.AesCajaIS[104] = 247;
            this.AesCajaIS[105] = 228;
            this.AesCajaIS[106] = 88;
            this.AesCajaIS[107] = 5;
            this.AesCajaIS[108] = 184;
            this.AesCajaIS[109] = 179;
            this.AesCajaIS[110] = 69;
            this.AesCajaIS[111] = 6;
            this.AesCajaIS[112] = 208;
            this.AesCajaIS[113] = 44;
            this.AesCajaIS[114] = 30;
            this.AesCajaIS[115] = 143;
            this.AesCajaIS[116] = 202;
            this.AesCajaIS[117] = 63;
            this.AesCajaIS[118] = 15;
            this.AesCajaIS[119] = 2;
            this.AesCajaIS[120] = 193;
            this.AesCajaIS[121] = 175;
            this.AesCajaIS[122] = 189;
            this.AesCajaIS[123] = 3;
            this.AesCajaIS[124] = 1;
            this.AesCajaIS[125] = 19;
            this.AesCajaIS[126] = 138;
            this.AesCajaIS[127] = 107;
            this.AesCajaIS[128] = 58;
            this.AesCajaIS[129] = 145;
            this.AesCajaIS[130] = 17;
            this.AesCajaIS[131] = 65;
            this.AesCajaIS[132] = 79;
            this.AesCajaIS[133] = 103;
            this.AesCajaIS[134] = 220;
            this.AesCajaIS[135] = 234;
            this.AesCajaIS[136] = 151;
            this.AesCajaIS[137] = 242;
            this.AesCajaIS[138] = 207;
            this.AesCajaIS[139] = 206;
            this.AesCajaIS[140] = 240;
            this.AesCajaIS[141] = 180;
            this.AesCajaIS[142] = 230;
            this.AesCajaIS[143] = 115;
            this.AesCajaIS[144] = 150;
            this.AesCajaIS[145] = 172;
            this.AesCajaIS[146] = 116;
            this.AesCajaIS[147] = 34;
            this.AesCajaIS[148] = 231;
            this.AesCajaIS[149] = 173;
            this.AesCajaIS[150] = 53;
            this.AesCajaIS[151] = 133;
            this.AesCajaIS[152] = 226;
            this.AesCajaIS[153] = 249;
            this.AesCajaIS[154] = 55;
            this.AesCajaIS[155] = 232;
            this.AesCajaIS[156] = 28;
            this.AesCajaIS[157] = 117;
            this.AesCajaIS[158] = 223;
            this.AesCajaIS[159] = 110;
            this.AesCajaIS[160] = 71;
            this.AesCajaIS[161] = 241;
            this.AesCajaIS[162] = 26;
            this.AesCajaIS[163] = 113;
            this.AesCajaIS[164] = 29;
            this.AesCajaIS[165] = 41;
            this.AesCajaIS[166] = 197;
            this.AesCajaIS[167] = 137;
            this.AesCajaIS[168] = 111;
            this.AesCajaIS[169] = 183;
            this.AesCajaIS[170] = 98;
            this.AesCajaIS[171] = 14;
            this.AesCajaIS[172] = 170;
            this.AesCajaIS[173] = 24;
            this.AesCajaIS[174] = 190;
            this.AesCajaIS[175] = 27;
            this.AesCajaIS[176] = 252;
            this.AesCajaIS[177] = 86;
            this.AesCajaIS[178] = 62;
            this.AesCajaIS[179] = 75;
            this.AesCajaIS[180] = 198;
            this.AesCajaIS[181] = 210;
            this.AesCajaIS[182] = 121;
            this.AesCajaIS[183] = 32;
            this.AesCajaIS[184] = 154;
            this.AesCajaIS[185] = 219;
            this.AesCajaIS[186] = 192;
            this.AesCajaIS[187] = 254;
            this.AesCajaIS[188] = 120;
            this.AesCajaIS[189] = 205;
            this.AesCajaIS[190] = 90;
            this.AesCajaIS[191] = 244;
            this.AesCajaIS[192] = 31;
            this.AesCajaIS[193] = 221;
            this.AesCajaIS[194] = 168;
            this.AesCajaIS[195] = 51;
            this.AesCajaIS[196] = 136;
            this.AesCajaIS[197] = 7;
            this.AesCajaIS[198] = 199;
            this.AesCajaIS[199] = 49;
            this.AesCajaIS[200] = 177;
            this.AesCajaIS[201] = 18;
            this.AesCajaIS[202] = 16;
            this.AesCajaIS[203] = 89;
            this.AesCajaIS[204] = 39;
            this.AesCajaIS[205] = 128;
            this.AesCajaIS[206] = 236;
            this.AesCajaIS[207] = 95;
            this.AesCajaIS[208] = 96;
            this.AesCajaIS[209] = 81;
            this.AesCajaIS[210] = 127;
            this.AesCajaIS[211] = 169;
            this.AesCajaIS[212] = 25;
            this.AesCajaIS[213] = 181;
            this.AesCajaIS[214] = 74;
            this.AesCajaIS[215] = 13;
            this.AesCajaIS[216] = 45;
            this.AesCajaIS[217] = 229;
            this.AesCajaIS[218] = 122;
            this.AesCajaIS[219] = 159;
            this.AesCajaIS[220] = 147;
            this.AesCajaIS[221] = 201;
            this.AesCajaIS[222] = 156;
            this.AesCajaIS[223] = 239;
            this.AesCajaIS[224] = 160;
            this.AesCajaIS[225] = 224;
            this.AesCajaIS[226] = 59;
            this.AesCajaIS[227] = 77;
            this.AesCajaIS[228] = 174;
            this.AesCajaIS[229] = 42;
            this.AesCajaIS[230] = 245;
            this.AesCajaIS[231] = 176;
            this.AesCajaIS[232] = 200;
            this.AesCajaIS[233] = 235;
            this.AesCajaIS[234] = 187;
            this.AesCajaIS[235] = 60;
            this.AesCajaIS[236] = 131;
            this.AesCajaIS[237] = 83;
            this.AesCajaIS[238] = 153;
            this.AesCajaIS[239] = 97;
            this.AesCajaIS[240] = 23;
            this.AesCajaIS[241] = 43;
            this.AesCajaIS[242] = 4;
            this.AesCajaIS[243] = 126;
            this.AesCajaIS[244] = 186;
            this.AesCajaIS[245] = 119;
            this.AesCajaIS[246] = 214;
            this.AesCajaIS[247] = 38;
            this.AesCajaIS[248] = 225;
            this.AesCajaIS[249] = 105;
            this.AesCajaIS[250] = 20;
            this.AesCajaIS[251] = 99;
            this.AesCajaIS[252] = 85;
            this.AesCajaIS[253] = 33;
            this.AesCajaIS[254] = 12;
            this.AesCajaIS[255] = 125;
        }
    }
}
