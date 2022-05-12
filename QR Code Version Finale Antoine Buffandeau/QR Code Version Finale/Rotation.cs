using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace QR_Code_Version_Finale
{
    class Rotation
    {
        //Rotate in any anlge => loss of data 
        //Solution : linear interpolation image
        #region Fields (4) :
        double degree;
        int height;
        int width;
        Pixel[,] matPixel;
        #endregion

        #region Constructors (1) :
        /// <summary>
        /// Constructor which create new Rotation
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="matPixel"></param>
        public Rotation(double degree, Pixel[,] matPixel)
        {
            this.degree = degree;
            this.matPixel = matPixel;
            this.height = matPixel.GetLength(0);
            this.width = matPixel.GetLength(1);
        }
        #endregion

        #region Properties (0) :
        //None for now
        #endregion

        #region Methods (4) :
        /// <summary>
        /// Method which rotates image at 90° or 180° or 270°
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="matPixel"></param>
        /// <returns></returns>
        public Pixel[,] predefinedAngleRotation(int degree, Pixel[,] matPixel) //ANGLE DEFINED
        {
            //créer une image plus grande contenant la diagonale de la vraie image en longueur
            //inverser par rapport à la diagonale
            //anlge pas quelconque ici car je ne sais pas comment faire
            Pixel[,] rotate = null;
            if (degree == 90)
            {
                rotate = new Pixel[width, height];
                for (int a = 0; a < matPixel.GetLength(1); a++)
                {
                    for (int b = 0; b < matPixel.GetLength(0); b++)
                    {
                            rotate[a, b] = matPixel[b, rotate.GetLength(0) - 1 - a];
                    }
                }
            }
            else if (degree == 270)
            {
                rotate = new Pixel[width, height];
                for (int a = 0; a < matPixel.GetLength(1); a++)
                {
                    for (int b = 0; b < matPixel.GetLength(0); b++)
                    {
                        rotate[a, b] = matPixel[rotate.GetLength(1) - 1 - b, a];
                    }
                }
            }
            else //180
            {
                rotate = new Pixel[height, width];
                for (int a = 0; a < matPixel.GetLength(0); a++)
                {
                    for (int b = 0; b < matPixel.GetLength(1); b++)
                    {
                        rotate[a, b] = matPixel[matPixel.GetLength(0) - 1 - a, matPixel.GetLength(1) - 1 - b];
                    }
                }
            }
            return rotate;
        }
        /// <summary>
        /// Method which rotates the image at any angle 
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="matPixel"></param>
        /// <returns></returns>
        public Pixel[,] RotateXY(double degree, Pixel[,] matPixel) //WORK
        {
            double radian = (double)(degree / 180) * Math.PI; //Math.PI = 3.1415926535897931
            /*int Rxmax = 0;
            if (R_x(0, 0, width, height, radian) > R_x(0, matPixel.GetLength(1), width, height, radian))
            {
                Rxmax = R_x(0, 0, width, height, radian);
            }
            else Rxmax = R_x(0, matPixel.GetLength(1), width, height, radian);
            int Rymax = 0;
            if (R_y(0, matPixel.GetLength(1), width, height, radian) > R_y(matPixel.GetLength(0), matPixel.GetLength(1), width, height, radian))
            {
                Rymax = R_y(0, matPixel.GetLength(1), width, height, radian);
            }
            else Rymax = R_y(matPixel.GetLength(0), matPixel.GetLength(1), width, height, radian);
            Pixel[,] matRotate = new Pixel[Rxmax, Rymax];
            */
            Pixel[,] matRotate = new Pixel[2*height, 2*width];
            for (int a=0; a<matRotate.GetLength(0);a++)
            {
                for(int b=0; b<matRotate.GetLength(1);b++)
                {
                    matRotate[a, b] = new Pixel(0, 0, 0);
                }
            }
            int newx = -1;
            int newy = -1;
            for (int c = 0; c < matPixel.GetLength(0);c++)
            {
                for(int d =0; d<matPixel.GetLength(1);d++)
                {
                    newx = matRotate.GetLength(0) / 4 + R_x(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radian);
                    newy = matRotate.GetLength(1) / 4 + R_y(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radian);
                    if (newx >= 0 && newx < matRotate.GetLength(0) && newy >= 0 && newy < matRotate.GetLength(1))
                    {
                        matRotate[newx, newy] = matPixel[c, d];
                    }
                }
            }
            return matRotate;
        }
        /// <summary>
        /// Method which helps find the new x coordinate for allowing rotation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public int R_x(int x, int y, int width, int height, double radian) //WORK
        {
            return (int)((x - width / 2) * Math.Cos(radian) - (y - height / 2) * Math.Sin(radian) + (width) / 2);
        }
        /// <summary>
        /// Method which helps find the new y coordinate for allowing rotation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="radian"></param>
        /// <returns></returns>
        public int R_y(int x, int y, int width, int height, double radian) //WORK
        {
            return (int)((x - width / 2) * Math.Sin(radian) + (y - height / 2) * Math.Cos(radian) + (height) / 2);
        }
        /// <summary>
        /// Method which rotates the colors of the ilage at any angle 
        /// </summary>
        /// <param name="degree"></param>
        /// <param name="matPixel"></param>
        /// <returns></returns>
        public Pixel[,] RotateColorXY(double degree, Pixel[,] matPixel) //WORK
        {
            double radianR = (double)(degree / 180) * Math.PI; //Math.PI = 3.1415926535897931
            double radianG = (double)(2*degree / 180) * Math.PI;
            double radianB = (double)(3*degree / 180) * Math.PI;
            Pixel[,] matRotate = new Pixel[2 * height, 2 * width];
            for (int a = 0; a < matRotate.GetLength(0); a++)
            {
                for (int b = 0; b < matRotate.GetLength(1); b++)
                {
                    matRotate[a, b] = new Pixel(0, 0, 0);
                }
            }
            int newxR = -1;
            int newyR = -1;
            int newxG = -1;
            int newyG = -1;
            int newxB = -1;
            int newyB = -1;
            for (int c = 0; c < matPixel.GetLength(0); c++)
            {
                for (int d = 0; d < matPixel.GetLength(1); d++)
                {
                    newxR = matRotate.GetLength(0) / 4 + R_x(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianR);
                    newyR = matRotate.GetLength(1) / 4 + R_y(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianR);
                    newxG = matRotate.GetLength(0) / 4 + R_x(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianG);
                    newyG = matRotate.GetLength(1) / 4 + R_y(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianG);
                    newxB = matRotate.GetLength(0) / 4 + R_x(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianB);
                    newyB = matRotate.GetLength(1) / 4 + R_y(c, d, matPixel.GetLength(0), matPixel.GetLength(1), radianB);
                    if (newxR >= 0 && newxR < matRotate.GetLength(0) && newyR >= 0 && newyR < matRotate.GetLength(1))
                    {
                        matRotate[newxR, newyR].RedSet = matPixel[c, d].Red;
                    }
                    if (newxG >= 0 && newxG < matRotate.GetLength(0) && newyG >= 0 && newyG < matRotate.GetLength(1))
                    {
                        matRotate[newxG, newyG].GreenSet = matPixel[c, d].Green;
                    }
                    if (newxB >= 0 && newxB < matRotate.GetLength(0) && newyB >= 0 && newyB < matRotate.GetLength(1))
                    {
                        matRotate[newxB, newyB].BlueSet = matPixel[c, d].Blue;
                    }
                }
            }

            return matRotate;
        }
        /*
        public Pixel[,] DeRotateXY()
        {

        }
        */
        #endregion
    }
}
