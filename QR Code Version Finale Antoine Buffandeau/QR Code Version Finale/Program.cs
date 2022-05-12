using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QR_Code_Version_Finale
{
    class Program
    {
        #region Methods for menu :
        /// <summary>
        /// Menu 
        /// </summary>
        static void Menu()
        {
            string oldFilename = null; //no oldFilename in memory during first process
            string filename = null;
            /*
            ConsoleKeyInfo cki;
            do {
                filename = FilenameChoice(oldFilename);
                operationChoice(filename);
                Console.WriteLine("\nPress SPACE in order to quit !");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);
            */
            bool execution = true;
            while(execution)
            {
                filename = FilenameChoice(oldFilename);
                execution = OperationList(filename);
            }
        } 
        /// <summary>
        /// Method which enables the customer to select the filename he wants to utilize
        /// </summary>
        /// <param name="oldFilename"></param>
        /// <returns></returns>
        static string FilenameChoice(string oldFilename)
        {
            Console.Clear();
            Console.WriteLine("Project made by Antoine Buffandeau during 01/01/22 to 05/03/22 :");
            Console.WriteLine("The Images :\n"
                + "\t1 : Coco image\n"
                + "\t2 : Lena image\n"
                + "\t3 : Lac image\n"
                + "\t4 : Test image\n"
                + "\t5 : Last image\n"
                + "\t6 : Quit\n"
                + "Please enter the number corresponding to the image you want to manipulate :\n");
            int number = TryInt();
            string filename = oldFilename;
            switch (number) //selection of the filename (5 or other caracter == default)
            {
                case 1:
                    filename = "coco.bmp";
                    break;
                case 2:
                    filename = "lena.bmp";
                    break;
                case 3:
                    filename = "lac.bmp";
                    break;
                case 4:
                    filename = "Test.bmp";
                    break;
                case 5:
                    if (filename != null)
                    {
                        filename = OldFilename(oldFilename);
                    }
                    else filename = "coco.bmp";
                    break;
                case 6:
                    break;
                default :
                    break;
            }
            Console.Clear();
            return filename;
        }
        /// <summary>
        /// Methods which take into account the last image and use it again
        /// </summary>
        /// <param name="oldFilename"></param>
        /// <returns></returns>
        static string OldFilename(string oldFilename)
        {
            string filename ="";
            switch(oldFilename)
            {
                case "coco.bmp":
                    filename = "coco.bmp";
                    break;
                case "lena.bmp":
                    filename = "lena.bmp";
                    break;
                case "lac.bmp":
                    filename = "lac.bmp";
                    break;
                case "Test.bmp":
                    filename = "Test.bmp";
                    break;
                default:
                    break;
            }
            return filename;
        }
        /// <summary>
        /// Method which show the operation list possible throught this project
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool OperationList(string filename) //NEW VERSION OF MENU
        {
            Console.Clear();
            Console.WriteLine("\nManipulation of the image " + filename + " :\n"
                + "\t01 : Image Processing\n"
                + "\t02 : Apply a filter\n"
                + "\t03 : Generate an image\n"
                + "\t04 : Create a Quick Response Code (QR Code)\n"
                + "\t05 : Quit \n"
                + "Please choice the number of the operation you want to achieve :\n");
            int secondchoice = TryInt();
            bool execution = true;
            while (execution)
            {
                switch (secondchoice)
                {
                    case 1:
                        execution = ImageProcessing(filename);
                        break;
                    case 2:
                        execution = ApplyAFilter(filename);
                        break;
                    case 3:
                        execution = GenerateAnImage(filename);
                        break;
                    case 4:
                        execution = QRCode(filename);
                        break;
                    case 5:
                        execution = false;
                        break;
                    default:
                        execution = false;
                        break;
                }
            }
            return execution;
        } 
        /// <summary>
        /// Method which executes some image processing
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool ImageProcessing(string filename)
        {
            Console.Clear();
            Console.WriteLine("\nManipulation of the image " + filename + " :\n"
                + "\t01 : Grey image\n"
                + "\t02 : Dark&White image\n"
                + "\t03 : Resize image\n"
                + "\t04 : Rotate image at 90° - 180° - 270°\n"
                + "\t05 : Rotate the whole image at any angle\n"
                + "\t06 : Rotate image's color at any angle\n"
                + "\t07 : Mirror Reverse image\n"
                + "\t08 : Mirror Effect image\n"
                + "\t09 : Quit \n"
                + "Please choice the number of the operation you want to achieve :\n");
            int thirdchoice = TryInt();
            double vald;
            int vali;
            bool execution = true;
            bool ex = true;
            Bitmap bitmap = new Bitmap(filename);
            Bitmap modified;
            while (execution)
            {
                switch (thirdchoice)
                {
                    case 1:
                        modified = bitmap.toGreyScale();
                        modified.save("./GreyScale_" + filename);
                        break;
                    case 2:
                        modified = bitmap.toBlackAndWhite();
                        modified.save("./BlackAndWhite_" + filename);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Choice a double number between 0 and infinite");
                        vald = TryDouble();
                        modified = bitmap.Resize(vald);
                        modified.save("./Resize_" + filename);
                        break;
                    case 4:
                        Console.Clear();
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Choice a integer number between 90 - 180 - 270");
                            vali = TryInt();
                        } while (vali != 90 && vali != 180 && vali != 270); // "||" ?????????????????????????????????????????????????
                        modified = bitmap.anyAngleRotation(vali);
                        modified.save("./RotateWholeAtPredefinedAngle_" + filename);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Choice a double number between 0 and infinite");
                        vald = TryDouble();
                        modified = bitmap.RotationXY(vald);
                        modified.save("./RotateWholeAtAnyAngle_" + filename);
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("Choice a double number between 0 and infinite");
                        vald = TryDouble();
                        modified = bitmap.RotationColorXY(vald);
                        modified.save("./RotateColorAtAnyAngle_" + filename);
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("Choice one of these directions : \n"
                            + "\t01 : horizontal image\n"
                            + "\t02 : vertical image\n");
                        vali = TryInt();
                        switch (vali)
                        {
                            case 01:
                                modified = bitmap.Mirror('h');
                                modified.save("./Mirror_" + filename);
                                break;
                            case 02:
                                modified = bitmap.Mirror('v');
                                modified.save("./Mirror_" + filename);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 8:
                        Console.Clear();
                        Console.WriteLine("Choice one of these directions : \n"
                            + "\t01 : horizontal effect image\n"
                            + "\t02 : vertical effect image\n"
                            + "\t03 : the two effect image\n");
                        vali = TryInt();
                        switch (vali)
                        {
                            case 01:
                                modified = bitmap.mirrorEffect('h');
                                modified.save("./MirrorEffect_" + filename);
                                break;
                            case 02:
                                modified = bitmap.mirrorEffect('v');
                                modified.save("./MirrorEffect_" + filename);
                                break;
                            case 03:
                                modified = bitmap.mirrorEffect('&');
                                modified.save("./MirrorEffect_" + filename);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 9:
                        ex = false;
                        break;
                    default:
                        ex = false;
                        break;
                }
                execution = false;
            }
            return ex;
        }
        /// <summary>
        /// Method which execute some filters
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool ApplyAFilter(string filename)
        {
            Console.Clear();
            Console.WriteLine("\nApply a filter on " + filename + " :\n"
                + "\t01 : Edge Detection image\n"
                + "\t02 : Edge Reinforcement image\n"
                + "\t03 : Blury image\n"
                + "\t04 : Push Back image\n"
                + "\t05 : Quit \n"
                + "Please choice the number of the operation you want to achieve :\n");
            int thirdchoice = TryInt();
            bool execution = true;
            bool ex = true;
            Bitmap bitmap = new Bitmap(filename);
            Bitmap modified;
            while (execution)
            {
                switch (thirdchoice)
                {
                    case 1:
                        modified = bitmap.toBlackAndWhite();
                        modified = bitmap.edgeDetection();
                        modified.save("./EdgeDetection_" + filename);
                        break;
                    case 2:
                        modified = bitmap.toBlackAndWhite();
                        modified = bitmap.edgeDetection();
                        modified = bitmap.edgeReinforcement();
                        modified.save("./EdgeReinforcement_" + filename);
                        break;
                    case 3:
                        modified = bitmap.toBlurry();
                        modified.save("./Blurry_" + filename);
                        break;
                    case 4:
                        modified = bitmap.pushBack();
                        modified.save("./PushBack_" + filename);
                        break;
                    case 5:
                        ex = false;
                        break;
                    default:
                        ex = false;
                        break;
                }
                execution = false;
            }
            return ex;
        }
        /// <summary>
        /// Method which create some image like fractal, histogram, etc
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool GenerateAnImage(string filename)
        {
            Console.Clear();
            Console.WriteLine("\nGenerate an Image :\n"
                + "\t01 : Mandelbort's Fractal \n"
                + "\t02 : Histogram\n"
                + "\t03 : Encoding Image\n"
                + "\t04 : Desencoding Image\n"
                + "Please choice the number of the operation you want to achieve :\n");
            int thirdchoice = TryInt();
            bool execution = true;
            bool ex = true;
            Bitmap bitmap = new Bitmap(filename);
            Bitmap modified;
            while (execution)
            {
                switch (thirdchoice)
                {
                    case 1:
                        modified = bitmap.fractal("mandelbrot", 50, 1000);
                        modified.save("./Fractal_" + "mandelbrot.bmp");
                        break;
                    case 2:
                        modified = bitmap.histogram();
                        modified.save("./Histogram_" + filename);
                        break;
                    case 3:
                        /*
                        Console.WriteLine("\tEncoding Images :\n"
                            + "\t1 : Coco image\n"
                            + "\t2 : Lena image\n"
                            + "\t3 : Lac image\n"
                            + "\t4 : Test image\n"
                            + "\t5 : Last image\n"
                            + "\t6 : Quit\n"
                            + "Please enter the number corresponding to the image you want to encode :\n");
                        */
                        modified = bitmap.codeImage(new Bitmap("./lac.bmp"));
                        modified.save("./Encoding_" + filename);
                        break;
                    case 4:
                        modified = bitmap.decodeImage(new Bitmap("./Encoding_coco.bmp"));
                        modified.save("./Desencoding_" + filename);
                        break;
                    case 5:
                        ex = false;
                        break;
                    default:
                        ex = false;
                        break;
                }
                execution = false;
            }
            return ex;
        }
        /// <summary>
        /// Method which allow the customer to choice between making a QR-Code or read a Qr-Code
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool QRCode(string filename)
        {
            Console.Clear();
            Console.WriteLine("\nGenerate a Quick Response Code:\n"
                + "\t01 : Generate a QR-Code from a message \n"
                + "\t02 : Decode a QR-Code\n"
                + "\t03 : Quit\n"
                + "Please choice the number of the operation you want to achieve :\n");
            bool execution = true;
            bool ex = true;
            int vali = TryInt();
            Bitmap bitmap = new Bitmap(filename);
            Bitmap modified;
            while (execution)
            {
                switch (vali)
                {
                    case 1:
                        modified = bitmap.madeQR_CODE("HELLO WORLD");
                        modified.save("./QrCode_" + filename);
                        break;
                    case 2:
                        break;
                    case 3:
                        ex = false;
                        break;
                    default:
                        ex = false;
                        break;
                }
                execution = false;
            }
            return ex;
        }
        /// <summary>
        /// Method which does the try/catch for an integer
        /// </summary>
        /// <returns></returns>
        static int TryInt()
        {
            int val;
            while(!int.TryParse(Console.ReadLine(), out val)) { }
            return val;
        }
        /// <summary>
        /// Method which does the try/catch for a double number
        /// </summary>
        /// <returns></returns>
        static double TryDouble()
        {
            double val;
            while (!double.TryParse(Console.ReadLine(), out val)) { }
            return val;
        }
        /// <summary>
        /// Old Menu operation selection
        /// </summary>
        /// <param name="filename"></param>
        static void operationChoice(string filename) //OLD VERSION OF MENU
        {
            Console.Clear();
            Console.WriteLine("\nManipulation of the image " + filename + " :\n"
                + "\t01 : Grey image\n"
                + "\t02 : Dark&White image\n"
                + "\t03 : Resize image\n"
                + "\t04 : Predefined Angle Rotation image\n"
                + "\t05 : Mirror image\n"
                + "\t06 : Mirror Effect image\n"
                + "\t07 : Edge Detection image\n"
                + "\t08 : Edge Reinforcement image\n"
                + "\t09 : Blury image\n"
                + "\t10 : Push Back image\n"
                + "\t11 : Fractal image\n"
                + "\t12 : Histogram image\n"
                + "\t13 : Encoding image\n"
                + "\t14 : Desencoding image\n"
                + "\t15 : AnyAngle Rotation image\n"
                + "\t16 : QrCode image\n"
                + "\t17 : AnyAngle Rotation Color image\n"
                + "Please choice the number of the operation you want to achieve :\n");
            int firstChoice = Convert.ToInt32(Console.ReadLine());
            Bitmap bitmap = new Bitmap(filename);
            Bitmap modified;
            double numberDouble = -1;
            int numberInt = -1;
            int secondChoice = -1;
            char choiceChar = ' ';
            string choiceString = " ";
            switch (firstChoice)
            {
                case 01:  //WORK
                    modified = bitmap.toGreyScale(); 
                    modified.save("./GreyScale_" + filename);
                    break;
                case 02:  //WORK
                    modified = bitmap.toBlackAndWhite(); 
                    modified.save("./BlackAndWhite_" + filename);
                    break;
                case 03:  //WORK but problem with string readline()
                    Console.Clear(); 
                    Console.WriteLine("Choice a double number between 0 and infinite");
                    numberDouble = Convert.ToDouble(Console.ReadLine());
                    modified = bitmap.Resize(numberDouble);
                    modified.save("./Resize_" + filename);
                    break;
                case 04:  //WORK but problem with string readline()
                    Console.Clear(); 
                    Console.WriteLine("Choice a integer number between 0 and 360");
                    /*numberInt = Convert.ToInt32(Console.ReadLine());
                    modified = bitmap.predefinedAngleRotation(numberInt);
                    modified.save("./Rotate_" + filename);
                    */
                    break;
                case 05:  //WORK but problem with string readline()
                    Console.Clear(); 
                    Console.WriteLine("Choice one of these directions : \n"
                        + "\t01 : horizontal image\n"
                        + "\t02 : vertical image\n");
                    secondChoice = Convert.ToInt32(Console.ReadLine());
                    switch (secondChoice)
                    {
                        case 01:
                            choiceChar = 'h';
                            break;
                        case 02:
                            choiceChar = 'v';
                            break;
                        default :
                            break;
                    }
                    modified = bitmap.Mirror(choiceChar);
                    modified.save("./Mirror_" + filename);
                    break;
                case 06:  //WORK
                    Console.Clear(); 
                    Console.WriteLine("Choice one of these directions : \n"
                        + "\t01 : horizontal effect image\n"
                        + "\t02 : vertical effect image\n"
                        + "\t03 : the two effect image\n");
                    secondChoice = Convert.ToInt32(Console.ReadLine());
                    switch (secondChoice)
                    {
                        case 01:
                            choiceChar = 'h';
                            break;
                        case 02:
                            choiceChar = 'v';
                            break;
                        case 03:
                            choiceChar = '&';
                            break;
                        default:
                            break;
                    }
                    modified = bitmap.mirrorEffect(choiceChar);
                    modified.save("./MirrorEffect_" + filename);
                    break;
                case 07:  //WORK
                    modified = bitmap.toBlackAndWhite();
                    modified = bitmap.edgeDetection();
                    modified.save("./EdgeDetection_" + filename);
                    break;
                case 08:  //WORK
                    modified = bitmap.toBlackAndWhite();
                    modified = bitmap.edgeDetection();
                    modified = bitmap.edgeReinforcement();
                    modified.save("./EdgeReinforcement_" + filename);
                    break;
                case 09:  //WORK
                    modified = bitmap.toBlurry();
                    modified.save("./Blurry_" + filename);
                    break;
                case 10:  //DOESN'T WORK
                    modified = bitmap.pushBack();
                    modified.save("./PushBack_" + filename);
                    break;
                case 11:  //WORK
                    modified = bitmap.fractal("mandelbrot",50,1000);
                    modified.save("./Fractal_" + "mandelbrot.bmp");
                    break;
                case 12:  //WORK
                    modified = bitmap.histogram();
                    modified.save("./Histogram_" + filename);
                    break;
                case 13:  //WORK
                    modified = bitmap.codeImage(new Bitmap("./lac.bmp"));
                    modified.save("./Encoding_" + filename);
                    break;
                case 14:  //TO FINISH
                    modified = bitmap.decodeImage(new Bitmap("coco.bmp"));
                    modified.save("./Desencoding_" + filename);
                    break;
                case 15:  //TO FINISH
                    Console.Clear();
                    Console.WriteLine("Choice a double number between 0 and infinite");
                    numberDouble = Convert.ToDouble(Console.ReadLine());
                    modified = bitmap.RotationXY(numberDouble);
                    modified.save("./AnyAngleRotate_" + filename);
                    break;
                case 16:  //TO FINISH
                    modified = bitmap.madeQR_CODE("message");
                    modified.save("./QrCode_" + filename);
                    break;
                case 17:  //TO FINISH
                    Console.Clear();
                    Console.WriteLine("Choice a double number between 0 and infinite");
                    numberDouble = Convert.ToDouble(Console.ReadLine());
                    modified = bitmap.RotationColorXY(numberDouble);
                    modified.save("./AnyAngleRotateColor_" + filename);
                    break;
                default :
                    break;
            }
            Console.Clear();
            //return ???
        }
        #endregion

        #region TD 1 : WORK !
        static void EssaisVerificationTD1()
        {
            //Les numéros des TDs ne sont pas les mêmes que pour le projet administratif ({td n°(n) pour moi} == {td n°(n+1) pour le projet PSI})
            Bitmap Test = new Bitmap("./coco.bmp");
            Test.save("./InfosFilecoco.txt");
            Test.save("./copiecoco.bmp");
        }
        #endregion

        #region Menu :
        /// <summary>
        /// Method : allow main menu to work
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //EssaisVerificationTD1();
            Menu();
        }
        #endregion
    }
}
