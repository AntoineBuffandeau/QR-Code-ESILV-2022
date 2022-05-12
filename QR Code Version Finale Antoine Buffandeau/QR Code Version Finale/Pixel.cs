using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace QR_Code_Version_Finale
{
    class Pixel
    {
        #region Fields : (3)
        byte R;
        byte G;
        byte B;
        #endregion

        #region Constructors : (1)
        /// <summary>
        /// To initialize R-G-B :
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public Pixel(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        #endregion

        #region Properties : (6)
        public byte Red => R;
        public byte Green => G;
        public byte Blue => B;
        public byte RedSet 
        {
            get => R;
            set 
            {
                if(value>=0 && value<=255) this.R = value; 
            } 
        }
        public byte GreenSet
        {
            get => G;
            set
            {
                if (value >= 0 && value <= 255) this.G = value;
            }
        }
        public byte BlueSet
        {
            get => B;
            set
            {
                if (value >= 0 && value <= 255) this.B = value;
            }
        }
        #endregion

        #region Methods : (0)
        #endregion
    }
}
