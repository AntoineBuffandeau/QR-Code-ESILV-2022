using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace QR_Code_Version_Finale
{
    class Bitmap
    {
        #region Fields : (8 - 9) //all private by default
        byte[] header;
        string typeImage;
        int fileSize;
        int offsetSize;
        int width; //largeur
        int height; //hauteur
        int colorBits;
        int compression; //en bonus (la compression)
        Pixel[,] matPixel;
        #endregion

        #region Constructors : (4)
        /// <summary>
        /// Constructor that can read filename and create an Image Bitmap 24bits
        /// </summary>
        /// <param name="filename"></param>
        public Bitmap(string filename) //FINI 
        {
            byte[] file = File.ReadAllBytes(filename);
            if (file[0] == 66 && file[1] == 77) typeImage = ".bmp";
            else typeImage = "otherformat";
            for (int a = 0; a < 4; a++) //for element' size = 4 bytes
            {
                fileSize += file[a + 2] * (int)(Math.Pow(256, a));  //Math.Pow = operateur puissance
                offsetSize += file[a + 10] * (int)(Math.Pow(256, a));
                width += file[a + 18] * (int)(Math.Pow(256, a));
                height += file[a + 22] * (int)(Math.Pow(256, a));
                compression += file[a + 30] * (int)(Math.Pow(256, a));
            }
            colorBits = file[28] + file[29] * 256; //size = 2 bytes

            header = new byte[54];
            for (int b = 0; b < 54; b++)
            { header[b] = file[b]; }

            matPixel = new Pixel[height, width];
            int cst = 54; //start at the 54th byte
            for (int c = 0; c < matPixel.GetLength(0); c++)
            {
                for (int d = 0; d < matPixel.GetLength(1); d++)
                {
                    matPixel[c, d] = new Pixel(file[cst], file[cst + 1], file[cst + 2]);
                    cst += 3; //goes 3 by 3 (R,G,B)
                }
            }
        }
        /// <summary>
        /// Constructor that can create a copy of the image in instance with width and height
        /// </summary>
        /// <param name="newwidth"></param>
        /// <param name="newheight"></param>
        public Bitmap(int newwidth, int newheight) //CASI FINI
        {
            header = Header;
            //fileSize = offsetSize + newwidth * newheight;
            //byte[] byteFileSize = convertirIntToLittleEndian(fileSize);
            byte[] byteWidth = convertIntToLittleEndian(newwidth);
            byte[] byteHeight = convertIntToLittleEndian(newheight);
            for(int a=0; a<4;a++)
            {
                //this.header[a+2] = byteFileSize[a];
                this.header[a + 18] = byteWidth[a];
                this.header[a + 22] = byteHeight[a];
            }
            matPixel = new Pixel[newheight, newwidth]; 
        }
        public Bitmap(int compression) //A FINIR  to do with or without loss of data
        {
            byte[] bytesCompression = convertIntToLittleEndian(compression);

            for (int a = 0; a < 2; a++)
            {
                header[a + 30] = bytesCompression[a+2];
            }
        }
        /// <summary>
        /// Constructor that can create a copy of the image in instance
        /// </summary>
        /// <param name="header"></param>
        /// <param name="matPixel"></param>
        public Bitmap(byte[] header, Pixel[,] matPixel) //FINI MAIS A EFFACER
        {
            this.header = header;
            this.matPixel = matPixel;
            height = matPixel.GetLength(0);
            width = matPixel.GetLength(1);
            byte[] byteWidth = convertIntToLittleEndian(width);
            byte[] byteHeight = convertIntToLittleEndian(height);
            for (int a = 0; a < 4; a++)
            {
                //this.header[a+2] = byteFileSize[a];
                this.header[a + 18] = byteWidth[a];
                this.header[a + 22] = byteHeight[a];
            }
        }
        #endregion

        #region Properties : (9)
        public string TypeImage => typeImage;
        public int FileSize => fileSize;
        public int OffsetSize => offsetSize;
        public int Width => width;
        public int Height => height;
        public int ColorBits => colorBits;
        public int Compression => compression;
        public Pixel[,] MatPixel => matPixel;
        public byte[] Header => header;
        #endregion

        #region Methods :
        #region TD 1 : (3)
        /// <summary>
        /// Method that can convert LittleEndian into an integer
        /// </summary>
        /// <param name="bts">tab of bytes</param>
        /// <returns></returns>
        public int convertLittleEndianToInt(byte[] bts)
        {
            int val = 0;
            for(int a=0;a<bts.Length;a++)
            {
                val += bts[a] * (int)(Math.Pow(256, a));
            }
            return val;
        }
        /// <summary>
        /// Method that can convert interger into LittleEndian
        /// </summary>
        /// <param name="val">integer value</param>
        /// <returns></returns>
        public byte[] convertIntToLittleEndian(int val)
        {
            byte[] bts = new byte[4];

            int octetsDePoidsFort = (int)Math.DivRem(val, (int)(Math.Pow(2, 24)), out int r1); //quotient (dividend, divisor, out remainder)
            int octetsDePoidsMoinsFort1 = (int)Math.DivRem(r1, (int)(Math.Pow(2, 16)), out int r2);
            int octetsDePoidsMoinsFort2 = (int)Math.DivRem(r2, (int)(Math.Pow(2, 8)), out int octetsDePoidsFaible);

            bts[0] = (byte)octetsDePoidsFaible;
            bts[1] = (byte)octetsDePoidsMoinsFort2;
            bts[2] = (byte)octetsDePoidsMoinsFort1;
            bts[3] = (byte)octetsDePoidsFort;

            return bts;
        }
        /// <summary>
        /// Method which saves the class image into a filename created during processus 
        /// </summary>
        /// <param name="filename">name of the filename</param>
        public void save(string filename)
        {
            FileStream fs = File.OpenWrite(filename);
            for(int a=0; a<54;a++)
            {
                fs.WriteByte(header[a]);
            }
            for (int b = 0; b < matPixel.GetLength(0); b++)
            {
                for (int c = 0; c < matPixel.GetLength(1); c++)
                {
                    fs.WriteByte(matPixel[b, c].Red);
                    fs.WriteByte(matPixel[b, c].Green);
                    fs.WriteByte(matPixel[b, c].Blue);
                }
                if((int)(matPixel.GetLength(1)*colorBits/8)%4!=0)
                {
                    for (int d = 0; d < (matPixel.GetLength(1) * colorBits / 8)% 4;d++)
                    {
                        fs.WriteByte(0);
                    }
                }
            }
            fs.Close();
        }
        #endregion

        #region TD 2 : (5)
        /// <summary>
        /// Method which create a grey scale image
        /// </summary>
        /// <returns></returns>
        public Bitmap toGreyScale()
        {
            //Bitmap Grey = new Bitmap(height, width);
            Bitmap Grey = new Bitmap(header, matPixel);
            for (int a = 0; a < height; a++)
            {
                for (int b = 0; b < width; b++)
                {
                    byte g1 =(byte)(0.299 * matPixel[a, b].Red);
                    byte g2 = (byte)(0.587 * matPixel[a, b].Green);
                    byte g3 = (byte)(0.114 * matPixel[a, b].Blue);
                    byte[] bts = { g1, g2, g3 };
                    //int n = convertLittleEndianToInt(bts);  //to use if you don't use the factors (0.299 , 0.587 , 0.114)
                    //bts = convertIntToLittleEndian((n / 3));
                    matPixel[a, b] = new Pixel(bts[0], bts[0], bts[0]);
                }
            }
            return Grey;
        }
        /// <summary>
        /// Method which transform an image into black and white image
        /// </summary>
        /// <returns></returns>
        public Bitmap toBlackAndWhite()
        {
            //Bitmap Grey = new Bitmap(height, width);
            Bitmap BlackWhite = new Bitmap(header, matPixel);
            for (int a = 0; a < matPixel.GetLength(0); a++)
            {
                for (int b = 0; b < matPixel.GetLength(1); b++)
                {
                    byte g1 = (byte)(0.33 * matPixel[a, b].Red);
                    byte g2 = (byte)(0.33 * matPixel[a, b].Green);
                    byte g3 = (byte)(0.33 * matPixel[a, b].Blue);
                    byte[] bts = { g1, g2, g3 };
                    byte m = (byte)((int)(convertLittleEndianToInt(bts) /*/ 3*/));
                    /*if (m < 0) m = 255;
                    if (m > 255) m = 0;*/
                    if (m < 35) { m = 0; }
                    else { m = 255; }
                    matPixel[a, b] = new Pixel((byte)m, (byte)m, (byte)m);
                }
            }
            return BlackWhite;
        }
        /// <summary>
        /// Method which resize the image on a specific size entered by customer
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public Bitmap Resize(double val)
        {
            Bitmap Resize = null;
            if (val > 0 && val != 1) //val == 1 is useless
            {
                int newWidth = (int)(width * val);
                int newHeight = (int)(height * val);
                Pixel[,] newmatPixel = new Pixel[newHeight, newWidth];
                for (int a = 0; a < newmatPixel.GetLength(0); a++)
                {
                    for (int b = 0; b < newmatPixel.GetLength(1); b++)
                    {
                        newmatPixel[a, b] = matPixel[(int)(a / val), (int)(b / val)];
                    }
                }
                Resize = new Bitmap(header, newmatPixel);
            }
            return Resize;
        }
        /// <summary>
        /// Method that rotate the image at 90° or 180° or 270°
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Bitmap anyAngleRotation(int degree) //NOT FINISHED - FIRST ATTEMPT NOT WORKING
        {
            /*
            //https://images.math.cnrs.fr/Rotations-discretes.html
            //https://fr.wikipedia.org/wiki/Matrice_de_rotation
            //https://www.f-legrand.fr/scidoc/docmml/image/niveaux/interpolation/interpolation.html

            double hypotenus = (double)Math.Sqrt((height / 2) ^ 2 + (width / 2) ^ 2);
            double radian = (double)(degree / 180) * Math.PI; //Math.PI = 3.1415926535897931
            Pixel[,] rotatematpixel = new Pixel[2 * (height / 2 * (int)Math.Cos(degree)), 2 * (width / 2 * (int)Math.Sin(degree))];
            for(int a=0; a<rotatematpixel.GetLength(0); a++)
            {
                for(int b=0; b<rotatematpixel.GetLength(1);b++)
                {
                    rotatematpixel[a, b] = new Pixel(0, 0, 0);
                }
            }
            for(int a=0; a<height;a++)
            {
                for(int b=0; b<width;b++)
                {
                    hypotenus= (double)Math.Sqrt((a) ^ 2 + (b) ^ 2);
                    rotatematpixel[a * (int)Math.Cos(degree) - b * (int)Math.Sin(degree), a * (int)Math.Sin(degree) + b * (int)Math.Cos(degree)] = matPixel[a, b];
                }
            }
            */
            Rotation rotate = new Rotation(degree, MatPixel);
            Pixel[,] rotatematpixel = rotate.predefinedAngleRotation(degree, MatPixel);
            return new Bitmap(header, rotatematpixel);
        }
        /// <summary>
        /// Method which rotate the image at any angle defined by customer (but unfortunately with some loss of data)
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Bitmap RotationXY(double degree) //WORK
        {
            Rotation rotate = new Rotation(degree, MatPixel);
            Pixel[,] newrotate = rotate.RotateXY(degree, MatPixel);
            return new Bitmap(Header, newrotate);
        }
        /// <summary>
        /// Method which rotate the color at respective angle defined by customer
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Bitmap RotationColorXY(double degree) //WORK
        {
            Rotation rotate = new Rotation(degree, MatPixel);
            Pixel[,] newrotate = rotate.RotateColorXY(degree, MatPixel);
            return new Bitmap(Header, newrotate);
        }
        public Bitmap Derotate() //NOT FINISHED
        {
            Bitmap Derotate = new Bitmap(header, matPixel);
            return Derotate;
        }
        /// <summary>
        /// Method that create 3 types of mirror effects
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public Bitmap mirrorEffect(char choice) //choice == {'h'}U{'v'}U{'&'}
        {
            Bitmap mirror = null;
            if (choice == 'h' || choice == 'v' || choice == '&') //choix vertical or horizontal or the two at the same time
            {
                Pixel[,] newmatPixel = null;
                if (choice == 'h')
                {
                    newmatPixel = new Pixel[2*height, width];
                    for(int a=0; a<matPixel.GetLength(0);a++)
                    {
                        for(int b=0;b<matPixel.GetLength(1);b++)
                        {
                            newmatPixel[a, b] = matPixel[matPixel.GetLength(0) - a - 1, b]; 
                            newmatPixel[a + matPixel.GetLength(0) , b] = matPixel[a, b];
                        }
                    }
                }
                else if (choice == 'v')
                {
                    newmatPixel = new Pixel[height, 2*width];
                    for (int a = 0; a < matPixel.GetLength(0); a++)
                    {
                        for (int b = 0; b < matPixel.GetLength(1); b++)
                        {
                            newmatPixel[a, b] = matPixel[a, b];
                            newmatPixel[a , b + matPixel.GetLength(1)] = matPixel[a, matPixel.GetLength(1) - 1 - b];
                        }
                    }
                }
                else //pas optimal //pour char '&' == {'h'} U {'v'}
                {
                    newmatPixel = new Pixel[2*height, 2*width];
                    Pixel[,] newpassingmatPixel = new Pixel[2*height, width];
                    for (int a = 0; a < matPixel.GetLength(0); a++)
                    {
                        for (int b = 0; b < matPixel.GetLength(1); b++)
                        {
                            newpassingmatPixel[a, b] = matPixel[matPixel.GetLength(0) - a - 1, b];
                            newpassingmatPixel[a + matPixel.GetLength(0), b] = matPixel[a, b];
                        }
                    }
                    for (int a = 0; a < matPixel.GetLength(0); a++)
                    {
                        for (int b = 0; b < matPixel.GetLength(1); b++)
                        {
                            newmatPixel[a, b] = newpassingmatPixel[a, b];
                            newmatPixel[a + matPixel.GetLength(0), b] = newpassingmatPixel[a + matPixel.GetLength(0), b];
                            newmatPixel[a, b + matPixel.GetLength(1)] = newpassingmatPixel[a, matPixel.GetLength(1) - 1 - b];
                            newmatPixel[a + matPixel.GetLength(0), b + matPixel.GetLength(1)] = newpassingmatPixel[matPixel.GetLength(0) - 1 - a, matPixel.GetLength(1) - 1 - b];
                        }
                    }
                }
                mirror = new Bitmap(header, newmatPixel);
            }
            return mirror;
        }
        /// <summary>
        /// Method which enables the clients to inverse or reverse image 
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public Bitmap Mirror(char choice) //choice == {'h'}U{'v'}
        {
            Bitmap mirror = null;
            if (choice == 'h' || choice == 'v') //choice vertical or horizontal
            {
                Pixel[,] newmatPixel = null;
                if (choice == 'h')
                {
                    newmatPixel = new Pixel[height, width];
                    for (int a = 0; a < matPixel.GetLength(0); a++)
                    {
                        for (int b = 0; b < matPixel.GetLength(1); b++)
                        {
                            newmatPixel[a, b] = matPixel[matPixel.GetLength(0) - 1 - a, b];
                        }
                    }
                }
                else //(choice == 'v') //not working
                {
                    newmatPixel = new Pixel[width, height];
                    for (int a = 0; a < matPixel.GetLength(0); a++)
                    {
                        for (int b = 0; b < matPixel.GetLength(1); b++)
                        {
                            newmatPixel[b, a] = matPixel[a, matPixel.GetLength(1) - 1 - b];
                        }
                    }
                }
                mirror = new Bitmap(header, newmatPixel);
            }
            return mirror;
        }
        #endregion

        #region TD 3 : (5)
        /// <summary>
        /// General method that allows the filter to work
        /// </summary>
        /// <param name="matPixel"></param>
        /// <param name="matFilter"></param>
        /// <returns></returns>
        public Pixel[,] MatriceConvolution(Pixel[,] matPixel, int[,] matFilter) // A VERIFIER
        {
            Pixel[,] newmatPixel = new Pixel[matPixel.GetLength(0), matPixel.GetLength(1)];
            int number=0;
            for(int i=0; i<matFilter.GetLength(0);i++)
            {
                for(int j=0; j<matFilter.GetLength(1);j++)
                {
                    number += matFilter[i, j];
                }
            }
            if (number == 0) number = 1;

            int e = 0, f = 0;
            for (int a = 0; a < matPixel.GetLength(0); a++)
            {
                for (int b = 0; b < matPixel.GetLength(1); b++)
                {
                    //newmatPixel[a, b] = new Pixel(); //on remet la somme à 0 à l'aide du 2nd constructeur pixel
                    int red = 0, green = 0, blue = 0;
                    for (int c = 0; c < matFilter.GetLength(0); c++)
                    {
                        for (int d = 0; d < matFilter.GetLength(1); d++)
                        {
                            e = a + c - 1;
                            f = b + d - 1;
                            if (e >= 0 && e <= matPixel.GetLength(0)-1 && f >= 0 && f <= matPixel.GetLength(1)-1)  //afin d'éviter les déplacements en dehors de l'image (addition de 0 sinon useless)
                            { //f (1) ???????
                                red += (byte)(matPixel[e, f].Red * matFilter[c, d]);
                                green += (byte)(matPixel[e, f].Green * matFilter[c, d]);
                                blue += (byte)(matPixel[e, f].Blue * matFilter[c, d]);
                            }
                        }
                    }
                    red /= number;
                    if (red < 0) red = 255;
                    if (red > 255) red = 0;
                    green /= number;
                    if (green < 0) green = 255;
                    if (green > 255) green = 0;
                    blue /= number;
                    if (blue < 0) blue = 255;
                    if (blue > 255) blue = 0;
                    newmatPixel[a, b] = new Pixel((byte)red, (byte)green, (byte)blue);
                }
            }
            return newmatPixel;
        }
        /// <summary>
        /// Method which detects the edge of the image (dark and white)
        /// </summary>
        /// <returns></returns>
        public Bitmap edgeDetection()
        {
            //int[,] matDetectionFilter = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } }; //less powered and optimized
            int[,] matDetectionFilter = { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } }; //more accurate
            Pixel[,] newmatPixel = MatriceConvolution(matPixel, matDetectionFilter);
            return new Bitmap(header, newmatPixel);
        }
        /// <summary>
        /// Method which reinforce the edge of the image
        /// </summary>
        /// <returns></returns>
        public Bitmap edgeReinforcement()
        {
            int[,] matReinforcementFilter = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            //int[,] matReinforcementFilter = { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            Pixel[,] newmatPixel = MatriceConvolution(matPixel, matReinforcementFilter);
            return new Bitmap(header, newmatPixel);
        }
        /// <summary>
        /// Method which create a blurring image of the image in instance
        /// </summary>
        /// <returns></returns>
        public Bitmap toBlurry()
        {
            int[,] matBlurringFilter = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            //int[,] matBlurringFilter = { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } }; //flou gaussien 
            Pixel[,] newmatPixel = MatriceConvolution(matPixel, matBlurringFilter);
            return new Bitmap(header, newmatPixel);
        }
        /// <summary>
        /// Method which create a Push Back Filter
        /// </summary>
        /// <returns></returns>
        public Bitmap pushBack()
        {
            int[,] matPushBackFilter = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Pixel[,] newmatPixel = MatriceConvolution(matPixel, matPushBackFilter);
            return new Bitmap(header, newmatPixel);
        }
        #endregion

        #region TD 4 :
        /// <summary>
        /// Method which create a fractal (mandelbrot or julia)
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="nbMaxIteration"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Bitmap fractal(string choice, int nbMaxIteration, int zoom)
        {
            Fractale fractale = new Fractale(choice, nbMaxIteration, zoom);
            Pixel[,] mat = fractale.MandelbrotOrJuliaFractal();
            Bitmap Fractal = new Bitmap(header, mat);
            return Fractal; 
        }
        /// <summary>
        /// Method which creates an histogram of the colors of the image in instance
        /// </summary>
        /// <returns></returns>
        public Bitmap histogram()
        {
            int[] tabHistogram = new int[768]; //768 = 256*3(B,G,R)
            int maximumValue = 0;
            for(int a=0;a<matPixel.GetLength(0); a++)
            {
                for(int b=0; b<matPixel.GetLength(1);b++)
                {
                    for(int c=0;c<3;c++)
                    {
                        if (c == 0) tabHistogram[matPixel[a, b].Blue]++; // "..." += 1
                        else if (c == 1) tabHistogram[matPixel[a, b].Green + 256]++;
                        else tabHistogram[matPixel[a, b].Red + 256*2]++; //if c==2
                    }
                }
            }
            for(int d=0; d<tabHistogram.Length;d++) //maximum value of histogram in order to create the graph
            {
                if (maximumValue < tabHistogram[d]) maximumValue = tabHistogram[d];
            }
            Pixel[,] matHistogram = new Pixel[maximumValue, 768];
            for (int f = 0; f < maximumValue; f++) 
            {
                for (int e = 0; e < 768; e++)
                {
                    if (f < tabHistogram[e])
                    {
                        if(e<256) matHistogram[f, e] = new Pixel(255, 0, 0); 
                        else if (e<256*2) matHistogram[f, e] = new Pixel(0, 255, 0);
                        else matHistogram[f, e] = new Pixel(0, 0, 255);
                    }
                    else matHistogram[f, e] = new Pixel(255, 255, 255);
                }
            }
            Bitmap histogram = new Bitmap(header, matHistogram);
            return histogram;
        } //Maybe problem of rotation
        /// <summary>
        /// Method which allows the client to hide an image under another one more bigger
        /// </summary>
        /// <param name="Image1"></param>
        /// <returns></returns>
        public Bitmap codeImage(Bitmap Image1 /*, Bitmap Image2*/) //to do again because there are three cases possible with the heigth and width (which image has the buggest one) in order to lose minimum information
        {
            Pixel[,] matPixelImage1 = Image1.MatPixel, newmatPixelImage = new Pixel[matPixel.GetLength(0), matPixel.GetLength(1)];
            //Pixel[,] matPixelImage2 = Image2.MatPixel;
            int Red1 = 0, Red2 = 0, Green1=0, Green2=0, Blue1=0, Blue2=0;
            for(int a=0; a<matPixel.GetLength(0);a++)
            {
                for(int b=0; b<matPixel.GetLength(1);b++)
                {
                    Red1 = (int)(matPixel[a, b].Red);
                    Green1 = (int)(matPixel[a, b].Green);
                    Blue1 = (int)(matPixel[a, b].Blue);

                    if (a < matPixelImage1.GetLength(0) && b < matPixelImage1.GetLength(1))
                    {
                        Red2 = (int)(matPixelImage1[a, b].Red / 16);
                        Green2 = (int)(matPixelImage1[a, b].Green / 16);
                        Blue2 = (int)(matPixelImage1[a, b].Blue / 16);

                        newmatPixelImage[a, b] = new Pixel((byte)((Red1 + Red2)/2), (byte)((Green1 + Green2)/2), (byte)((Blue1 + Blue2)/2));
                    }
                    else newmatPixelImage[a, b] = new Pixel((byte)Red1, (byte)Green1, (byte)Blue1);
                }
            }
            Bitmap encoding = new Bitmap(header, newmatPixelImage);
            return encoding;
        }
        /// <summary>
        /// Method which tries to find the hidden image made from the processus of method codeImage
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public Bitmap decodeImage(Bitmap image)
        {
            Pixel[,] matHidden = new Pixel[image.MatPixel.GetLength(0), image.MatPixel.GetLength(1)];
            for(int a=0; a<matPixel.GetLength(0); a++)
            {
                for(int b=0; b<matPixel.GetLength(1);b++)
                {
                    int Red1 = Math.DivRem(image.MatPixel[a, b].Red, 16, out int Red2); //euclidean division
                    int Green1 = Math.DivRem(image.MatPixel[a, b].Red, 16, out int Green2);
                    int Blue1 = Math.DivRem(image.MatPixel[a, b].Red, 16, out int Blue2);
                    matHidden[a, b] = new Pixel((byte)(Red2*16), (byte)(Green2*16), (byte)(Blue2*16));
                    //matHidden[a, b] = new Pixel((byte)Red1, (byte)Green1, (byte)Blue1);
                }
            }
            Bitmap desencoding = new Bitmap(image.Header, matHidden);
            return desencoding;
        } //to finish 
        #endregion

        #region QR_CODE :
        /// <summary>
        /// Method which create a QR-Code
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public Bitmap madeQR_CODE(string chain)
        {
            QR_Code qrcode = new QR_Code(chain);
            this.matPixel = qrcode.QrCode(2);
            //Bitmap qrcodebis = new Bitmap(header, matPixel);
            //Bitmap qrcodebis = Resize(4.0);
            this.matPixel = anyAngleRotation(90).MatPixel;
            return new Bitmap(header, matPixel );
        }
        #endregion 
        #endregion
    }
}
