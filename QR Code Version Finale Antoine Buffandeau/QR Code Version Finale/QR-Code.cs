using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Collections;
//using ReedSolomon;
using System.Drawing;

namespace QR_Code_Version_Finale
{
    class QR_Code //for version 1 and 2 in alphanumeric mode
    {
        #region Fields (2) :
        int version; // 1 or 2
        int eccCount; //L = 7% //error correction
        string mode; //alphanumeric mode = "0010" //9 bits
        string lengthTexts;
        string message;
        Pixel[,] matQrCode;
        byte[] bytesMessage;
        byte[,] code;
        Encoding utf8 = Encoding.UTF8;

        byte[] chainbyte;
        #endregion

        #region Constructors (.) :
        /// <summary>
        /// Constructor which code and create a QrCode
        /// </summary>
        /// <param name="message"></param>
        public QR_Code(string message) //create a QRCODE
        {
            this.message = message.ToUpper();
            switch(choiceVersion())
            {
                case 1:
                    this.version = 1;
                    this.matQrCode = new Pixel[21, 21];
                    this.code = new byte[21, 21];
                    this.eccCount = 7;
                    break;
                case 2:
                    this.version = 2;
                    this.matQrCode = new Pixel[25, 25];
                    this.code = new byte[25, 25];
                    this.eccCount = 10;
                    break;
                default:
                    break;
            }
            this.mode = "0010";
            /*
            this.bytesMessage = utf8.GetBytes(message);
            byte[] errorCorrector = ReedSolomonAlgorithm.Encode(bytesMessage, eccCount, ErrorCorrectionCodeType.QRCode);
            string correctionBits = "";
            foreach(byte a in errorCorrector)
            {
                correctionBits += intToBits((int)a, 8);
            }
            for (int a = 0; a < this.matQrCode.GetLength(0); a++)
            {
                for (int b = 0; b < this.matQrCode.GetLength(1); b++)
                {
                    this.matQrCode[a, b] = new Pixel(0, 0, 0);
                }
            }
            FinderPattern();
            Separators();
            if (this.version > 1) { AlignementPattern(); }
            TimingPattern(this.version);
            DarkModule(this.version);
            */
            //mask + matrix

            //this.lengthTexts = intToBits(message.Length, 9);
            //byte [] message1 = encoding(message);


            //byte[] textsEncoding = encoding(texts);
            /*
            Encoding u8 = Encoding.UTF8;
            string a = "HELLO WORD";
            int iBC = u8.GetByteCount(a);
            byte[] bytesa = u8.GetBytes(a);
            string b = "HELLO WORF";
            byte[] bytesb = u8.GetBytes(b);
            //byte[] result = ReedSolomonAlgorithm.Encode(bytesa, 7);
            //Privilégiez l'écriture suivante car par défaut le type choisi est DataMatrix 
            byte[] result = ReedSolomonAlgorithm.Encode(bytesa, 7, ErrorCorrectionCodeType.QRCode);
            byte[] result1 = ReedSolomonAlgorithm.Decode(bytesb, result);
            foreach (byte val in a) Console.Write(val + " ");
            Console.WriteLine();
            */
        }
        public QR_Code(Bitmap qrcode) //Decode a QRCODE
        {

        }
        #endregion

        #region Properties (2) :
        public int Version => version;
        public double EccCount => eccCount;
        public Pixel[,] MatQrCode => matQrCode;
        #endregion

        #region Methods (.) :
        /// <summary>
        /// Method that choice the best version of QrCode to utilize according to the length of the message
        /// </summary>
        /// <returns></returns>
        public int choiceVersion()
        {
            int choice = 0;
            if (this.message.Length <= 25) choice = 1;
            else if (this.message.Length <= 47) choice = 2;
            return choice;
        }
        /// <summary>
        /// Method which convert a string chain into integer
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public int bitsToInt(string chain)
        {
            int number = 0;
            for(int a=0; a<chain.Length;a++)
            {
                if (chain[a] == '1') number += (int)Math.Pow(2, (double)chain.Length - 1 - a);
            }
            return number;
        }
        /// <summary>
        /// Method which converts integer into a string chain
        /// </summary>
        /// <param name="var"></param>
        /// <param name="wishSize"></param>
        /// <returns></returns>
        public string intToBits(int var, int wishSize)
        {
            string chain = "";
            while(var!=0 && wishSize >=0)
            {
                if((int)Math.Pow(2,wishSize)>var)
                {
                    chain += '0';
                }
                else
                {
                    chain += '1';
                    var -= (int)Math.Pow(2, (double)wishSize);
                }
            }
            for(int a=wishSize;a>=0;a--)
            {
                chain += '0';
            }
            return chain;
        }
        public string bytesToBits(byte[] tabBytes)
        {
            string chain = "";
            foreach (byte a in tabBytes)
            {
                chain += intToBits((int)a, 8);
            }
            return chain;
        }
        public byte[] bitsToBytes(string chain)
        {
            byte[] tabBytes = null;
            if(chain.Length == 8)
            {
                tabBytes = new byte[(int)chain.Length / 8];
                for(int a=0; a<chain.Length;a++)
                {
                    tabBytes[a] = 0;
                    for(int b=0; b<8; b++)
                    {
                        if(chain[a*8+b]=='1')
                        {
                            tabBytes[a] += (byte)Math.Pow(2, (double)7 - b);
                        }
                    }
                }
            }
            return tabBytes;
        }
        /// <summary>
        /// Method which use the alphanumeric mode and convert integer into char 
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public char intToChar(int var) //exception to manage !!!
        {
            char c = ' ';
            if(var>=0 && var<=44)
            {
                if (var <= 9) c = (char)('0' + var);
                else if (var <= 35) c = (char)(var + 'A' - 10);
                else
                {
                    switch(var)
                    {
                        case 36:
                            c = ' '; break;
                        case 37:
                            c = '$'; break;
                        case 38:
                            c = '%'; break;
                        case 39:
                            c = '*'; break;
                        case 40:
                            c = '+'; break;
                        case 41:
                            c = '-'; break;
                        case 42:
                            c = '.'; break;
                        case 43:
                            c = '/'; break;
                        case 44:
                            c = ':'; break;
                        default:
                            break;
                    }
                }
            }
            return c;
        }
        /// <summary>
        /// Method which use the alphanumeric mode and convert char into integer
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int charToInt(char c) //exception to manage !!!
        {
            int var = -1;
            if (c >= '0' && c <= '1') var = (int)(c - '0');
            else if (c >= 'A' && c <= 'Z') var = (int)(c - 'A' + 10);
            else
            {
                switch(c)
                {
                    case ' ':
                        var = 36; break;
                    case '$':
                        var = 37; break;
                    case '%':
                        var = 38; break;
                    case '*':
                        var = 39; break;
                    case '+':
                        var = 40; break;
                    case '-':
                        var = 41; break;
                    case '.':
                        var = 42; break;
                    case '/':
                        var = 43; break;
                    case ':':
                        var = 44; break;
                    default:
                        break;
                }
            }
            return var;
        }
        public string encoding(string chain)
        {
            int length;
            if (chain.Length % 2 == 0)
            {
                length = chain.Length;
                chainbyte = new byte[chain.Length / 2];
            }
            else
            {
                length = chain.Length - 1;
                chainbyte = new byte[chain.Length / 2 + 1];
            }
            int number = 0;
            for(int a=0; a<chainbyte.Length; a++)
            {
                chainbyte[a] = (byte)(charToInt(chain[number]) * 45 + charToInt(chain[number + 1]));
                number += 2;
            }
            if (chain.Length % 2 != 0) chainbyte[chainbyte.Length - 1] = (byte)(charToInt(chain[chain.Length - 1]));
            string encodingmessage = bytesToBits(chainbyte);
            return encodingmessage;
        }
        /// <summary>
        /// Method which place the finder pattern on the matrix
        /// </summary>
        public void FinderPattern()
        {
            for(int a=0; a<=7;a++)
            {
                for(int b=0; b<=7; b++)
                {
                    if((a== 0) || (b== 0) || (a== 7) || (b== 7))
                    {
                        code[a, b] = 1;
                        code[a + matQrCode.GetLength(0) - 8, b] = 1;
                        code[a, b + matQrCode.GetLength(1) - 8] = 1;
                    }
                    else if((a == 1) || (b == 1) || (a == 5) || (b == 5))
                    {
                        code[a, b] = 0;
                        code[a + matQrCode.GetLength(0) - 8, b] = 0;
                        code[a, b + matQrCode.GetLength(1) - 8] = 0;
                    }
                    else
                    {
                        code[a, b] = 1;
                        code[a + matQrCode.GetLength(0) - 8, b] = 1;
                        code[a, b + matQrCode.GetLength(1) - 8] = 1;
                    }
                }
            }
        }
        /// <summary>
        /// Method which places the separator on the matrix
        /// </summary>
        public void Separators()
        {
            for(int a=0; a<=7;a++)
            {
                for(int b=0; b<=7;b++)
                {
                    if(a==7 || b == 7)
                    {
                        matQrCode[a, b] = new Pixel(255, 255, 255);
                        matQrCode[a + matQrCode.GetLength(0) - 8, b] = new Pixel(255, 255, 255);
                        matQrCode[a, b + matQrCode.GetLength(1) - 8] = new Pixel(255, 255, 255);
                    }
                }
            }
        }
        /// <summary>
        /// Method used only for version 2 of QrCode and places alignement pattern
        /// </summary>
        public void AlignementPattern()
        {
            for(int a=0; a<=4;a++)
            {
                for(int b=0;b<=4;b++)
                {
                    if(a==0 || b==0)
                    {
                        matQrCode[a + matQrCode.GetLength(0) - 10, b + matQrCode.GetLength(1) - 10] = new Pixel(0, 0, 0);
                    }
                    else if(a==1 || b==1 || a==3 || b==3)
                    {
                        matQrCode[a + matQrCode.GetLength(0) - 10, b + matQrCode.GetLength(1) - 10] = new Pixel(255, 255, 255);
                    }
                    else
                    {
                        matQrCode[a + matQrCode.GetLength(0) - 10, b + matQrCode.GetLength(1) - 10] = new Pixel(0, 0, 0);
                    }
                }
            }
        }
        /// <summary>
        /// Methods which places Timing Pattern
        /// </summary>
        /// <param name="version"></param>
        public void TimingPattern(int version)
        {
            bool color = true;
            for(int a=0; a<((4*version)+1);a++)
            {
                if(color == true)
                {
                    matQrCode[6, a+8] = new Pixel(0, 0, 0);
                    matQrCode[a+8, 6] = new Pixel(0, 0, 0);
                    color = false;
                }
                else
                {
                    matQrCode[6, a+8] = new Pixel(255, 255, 255);
                    matQrCode[a+8, 6] = new Pixel(255, 255, 255);
                    color = true;
                }
            }
        }
        /// <summary>
        /// Method which places DarkModule
        /// </summary>
        /// <param name="version"></param>
        public void DarkModule(int version)
        {
            matQrCode[((4 * version) + 9), 8] = new Pixel(0, 0, 0);
        }
        public char ApplyMask()
        {
            return 'A';
        }

        public Pixel[,] MatrixQRCode(byte[,] code)
        {
            Pixel[,] matqr = new Pixel[code.GetLength(0), code.GetLength(1)];
            for(int a=0; a<code.GetLength(0);a++)
            {
                for(int b=0; b<code.GetLength(1);b++)
                {
                    if(code[a,b]==0)
                    {
                        matqr[a, b] = new Pixel(255, 255, 255);
                    }
                    else if(code[a,b]==1)
                    {
                        matqr[a, b] = new Pixel(0, 0, 0);
                    }
                    else matqr[a, b] = new Pixel(255, 255, 255);
                }
            }
            return matqr;
        }

        public Pixel[,] QrCode(int version)
        {
            for (int a = 0; a < code.GetLength(0); a++)
            {
                for (int b = 0; b < code.GetLength(1); b++)
                {
                    code[a, b] = 0;
                }
            }
            FinderPattern();
            
            Separators();
            if (version > 1) { AlignementPattern(); }
            TimingPattern(version);
            DarkModule(version);
            return MatrixQRCode(code);
        }
        #endregion
    }
}
