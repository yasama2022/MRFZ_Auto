using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace lib.image
{
    public class NumberModel
    {
        int w, h;
        public byte[,] bits { get; private set; } 
        protected static Size fontSize = new Size(20, 31);
        protected static int SplitNumber = 127 + 127 + 127;
        protected static Point[] PointNumber = new Point[6];
        protected static ImageColor ic_backColor = new ImageColor(12, 35, 70);
        protected static ImageColor ic_font = new ImageColor(255, 255, 255);
        protected static byte BackColor = 1;
        protected static byte FontColor = 2;
        protected static byte unkowned = 0;

        public static NumberModel[] model { get; protected set; } = new NumberModel[10];
        protected static String DI_Numbers;
        static NumberModel()
        {
            DI_Numbers = Environment.CurrentDirectory + @"\imgs\RecNumberFont";
            DirectoryInfo di = new DirectoryInfo(DI_Numbers);
            var fss = di.GetFiles("*.png");
            foreach (var fs in fss)
            {
                Bitmap newBmp = new Bitmap(fs.FullName);
                model[int.Parse(fs.Name.Split('.')[0])] = new NumberModel(newBmp);
                newBmp.Dispose();
            }
            int W = 20; int H = 31;
            int SPIT = 10;
            int startY = 413; 
            int startX = 957;
            PointNumber[0] = new Point(startX - 2 * SPIT - 5 * W, startY);
            PointNumber[1] = new Point(startX - 2 * SPIT - 4 * W, startY);
            PointNumber[2] = new Point(startX - 1 * SPIT - 3 * W, startY);
            PointNumber[3] = new Point(startX - 1 * SPIT - 2 * W, startY);
            PointNumber[4] = new Point(startX - 0 * SPIT - 1 * W, startY);
            PointNumber[5] = new Point(startX - 0 * SPIT - 0 * W, startY);  
        }
        /// <summary>
        /// @todo POS3 最后一位 2和7会混淆,在该位置重新录制数字
        /// POS4 用另外的号开启
        /// POS5-6 重新录制数字
        /// </summary>
        
       
        protected NumberModel(Bitmap bmp)
        {
            var ic = ImageColor.FromBitmap(bmp);
            w = bmp.Width; h = bmp.Height;
            bits = new byte[w, h];  
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var p = ic[x, y];
                    var BackColor_Dis = Math.Abs(p.R - ic_backColor.R) + Math.Abs(p.G - ic_backColor.G) + Math.Abs(p.B - ic_backColor.B);
                    var FontColor_Dis = Math.Abs(p.R - ic_font.R) + Math.Abs(p.G - ic_font.G) + Math.Abs(p.B - ic_font.B); 
                    bits[x, y] = FontColor_Dis > BackColor_Dis ? BackColor: FontColor;
                }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="POS">1-6</param>
        public static TimeSpan RecTime(ImageColor[,] full_Image_Colors,Size fullImageSize)
        {
            
            int[] T = new int[6] { 0,7,5,9,5,9};
            var max_number_index = 5;
            for (int i = 0; i <= max_number_index; i++)
            {
                var point = PointNumber[ i];
                var size = fontSize;
                var bits = new byte[fontSize.Width, fontSize.Height];
                for (int x = 0; x <  size.Width && x +point.X< fullImageSize.Width; x++)
                    for (int y = 0; y <  size.Height&&y+point.Y<fullImageSize.Height; y++)
                    {
                        var p = full_Image_Colors[x+point.X, y+point.Y];
                        var BackColor_Dis = Math.Abs(p.R - ic_backColor.R) + Math.Abs(p.G - ic_backColor.G) + Math.Abs(p.B - ic_backColor.B);
                        var FontColor_Dis = Math.Abs(p.R - ic_font.R) + Math.Abs(p.G - ic_font.G) + Math.Abs(p.B - ic_font.B);
                        bits[x, y] = FontColor_Dis > BackColor_Dis ? BackColor : FontColor; 
                    } 
                int maxIndex = 0;
                int max = 0;
                for (int n_index=0; n_index<10; n_index++)
                {
                   int curSimiliar= model[n_index].GetSimiliar(bits);
                    if (n_index == 0) max = 0;
                    if (curSimiliar > max)
                    {
                        max = curSimiliar;
                        maxIndex = n_index;
                    }
                }
                T[i] = maxIndex;
            }
            var h = T[0] * 10 + T[1];
            var m = T[2] * 10 + T[3];
            var s = T[4] * 10 + T[5];
            return new TimeSpan(h,m,s);
        }
        protected int GetSimiliar(byte[,] bits)
        {
            int total = 0;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    if (this.bits[x, y] == bits[x, y]) total++;
                }
            return total;
        }


        public static TimeSpan RecTime(Bitmap bmp)
        {
            var ic=ImageColor.FromBitmap(bmp);
            return RecTime(ic, new Size(bmp.Width, bmp.Height));
        }
        public Bitmap ToModelBitMap()
        {
            Bitmap bmp = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {

                    bmp.SetPixel(x, y, bits[x, y] ==BackColor ? Color.Transparent : Color.Black);
                }
            return bmp;
        }
    }
}
