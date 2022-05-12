using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace QR_Code_Version_Finale
{
    class Fractale
    {
        #region Fields (6) :
        int nbMaxIteration;
        int zoom;
        double x1, x2, y1, y2;
        #endregion

        #region Constructors (1) :
        /// <summary>
        /// Constructor which helps create a fractal
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="nbMaxIteration"></param>
        /// <param name="zoom"></param>
        public Fractale(string choice, int nbMaxIteration, int zoom)
        {
            if (choice == "mandelbrot")
            {
                MandelbrotVariables();
            }
            else if (choice == "julia")
            {
                JuliaVariables();
            }
            else
            {
                //empty
            }

            this.nbMaxIteration = nbMaxIteration ;
            this.zoom = zoom;
        }
        /* Maybe another for after
        public Fractale(int x1, int x2, int y1, int y2)
        {
            int height = (int)((x2 - x1) * zoom); //length of the matrix for the image
            int width = (int)((y2 - y1) * zoom);
        }
        */
        #endregion

        #region Methods (2) :
        public int Zoom => zoom;
        public int NbMaxIteration => nbMaxIteration;
        #endregion

        #region Properties (3) :
        /// <summary>
        /// Method which creates a Mandelbrot or Julia Fractal
        /// </summary>
        /// <returns></returns>
        public Pixel[,] MandelbrotOrJuliaFractal() //WORK
        {
            /*
            double x1 = -2.1; //area where the fractal begins
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;
            */
            int height = (int)((x2 - x1) * zoom); //length of the matrix for the image
            int width = (int)((y2 - y1) * zoom);
            Pixel[,] fractal = new Pixel[height,width];

            for(int x=0;x<height;x++)
            {
                for(int y=0; y<width;y++)
                {
                    double cr = (double)x / zoom + x1; //r =reel
                    double ci = (double)y / zoom + y1; //i =image
                    double zr = 0;
                    double zi = 0;
                    int i = 0;

                    do
                    {
                        double tmp = zr;
                        zr = zr * zr - zi * zi + cr;
                        zi = 2 * zi * tmp + ci;
                        i += 1;
                    } while ((zr*zr + zi*zi) <4 && i<nbMaxIteration);

                    if(i==nbMaxIteration) fractal[x, y] = new Pixel(0, 0, 0);
                    else if(i>nbMaxIteration*3/4) fractal[x, y] = new Pixel(192, 192, 192);
                    else if (i > nbMaxIteration/2) fractal[x, y] = new Pixel(128, 128, 128);
                    else if (i > nbMaxIteration * 1 / 4) fractal[x, y] = new Pixel(64, 64, 64);
                    //else if (i > nbMaxIteration * 1 / 8) fractal[x, y] = new Pixel(32, 32, 32); //useless
                    //else if (i > nbMaxIteration * 1 / 16) fractal[x, y] = new Pixel(16, 16, 16); //useless
                    //else if (i > nbMaxIteration * 1 / 32) fractal[x, y] = new Pixel(8, 8, 8); //useless
                    else fractal[x, y] = new Pixel(255, 255, 255);
                }
            }
            return fractal;
        }
        /// <summary>
        /// Coordinates for Mandelbrot's fractal
        /// </summary>
        public void MandelbrotVariables() //WORK
        {
            this.x1 = -2.1; //area where the fractal begins
            this.x2 = 0.6;
            this.y1 = -1.2;
            this.y2 = 1.2;
        }
        /// <summary>
        /// Coordinates for Julia's fractal
        /// </summary>
        public void JuliaVariables() //NOT USE YET
        {
            this.x1 = -1;
            this.x2 = 1;
            this.y1 = -1.2;
            this.y2 = 1.2;
        }
        #endregion
    }
}
