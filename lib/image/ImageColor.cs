using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace lib.image
{
    /// <summary>
    /// 图像RGB结构[不使用系统的是因为速度问题]
    /// 表示像素点RGB或者RGB向量
    /// </summary>
    public struct ImageColor
    {
        public int R, G, B;
        public ImageColor(System.Drawing.Color c)
        {
            this.R = c.R;
            this.G = c.G;
            this.B = c.B;
        }
        public ImageColor(int r, int g, int b)
        {
            this.R = r; this.G = g; this.B = b;
        }
        /// <summary>
        /// 返回图像的颜色点阵(加速)
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static ImageColor[,] FromBitmap(Bitmap bmp, String src = null)
        {
            return new PointBitmap(bmp, src).toImageColor();
        }
        /// <summary>
        /// 返回图像的颜色点阵(加速)
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static ImageColor[,] FromFile(String fileName)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(fileName);
                return new PointBitmap(bmp).toImageColor();
            }catch
            {
                return null;
            }finally
            {
                bmp?.Dispose();
            }
        }
        public static Bitmap FromIC(ImageColor[,] ic)
        {
           int w=   ic.GetLength(0);
            int h = ic.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            for (int x=0; x<w;x++)
                for(int y=0; y<h;y++)
                {
                    bmp.SetPixel(x,y, FromIC(ic[x,y]));
                }
            return bmp;
        }
        public static Bitmap FromIC(ImageColor[,] ic,Rectangle rect)
        {
            int w = rect.Width;// ic.GetLength(0);
            int h = rect.Height;//.GetLength(1);
            var src_w = ic.GetLength(0);
            var src_h = ic.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            for (int x =0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    bmp.SetPixel(x, y, FromIC(ic[x+rect.X, y+rect.Y]));
                }
            return bmp;
        }
        public static Bitmap Clone(Bitmap src,Rectangle rect)
        {
            int w = rect.Width;// ic.GetLength(0);
            int h = rect.Height;//.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    bmp.SetPixel(x, y, src.GetPixel( x + rect.X, y + rect.Y));
                }
            return bmp;
        }
        public static Color FromIC(ImageColor c)
        {
            return Color.FromArgb(c.R,c.G,c.B);
        }

        /// <summary>
        /// 灰度图，仅有文字和背景组成
        /// 需要提供背景色
        /// 非背景色被认为是字体色
        /// 计算两者字体色的差异百分比
        /// </summary>
        /// <param name="srcIC">截图</param>
        /// <param name="dstIC">目标字体图</param>
        /// <param name="backColor">背景</param>
        /// <param name="offsetRect">偏移</param>
        /// <returns></returns>
        public static double CalcDeltaOfTwoImgInFont(ImageColor[,] srcIC, ImageColor[,] dstIC,Color backColor, Rectangle offsetRect
            ,int backColor_dis=6)
        {
            var sum_delt = 0d;
            int fontPixCount = 0;
            for (int x = 0; x < offsetRect.Width; x++)
                for (int y = 0; y < offsetRect.Height; y++)
                {
                    var ori = srcIC[x + offsetRect.X, y + offsetRect.Y];
                    var dst = dstIC[x, y];

                  //  var dr = dst.R - ori.R;
                   // var dg = dst.G - ori.G;
                   // var db = dst.B - ori.B;
                    //该点为背景色
                    if (Math.Abs(ori.R - backColor.R) < backColor_dis && 
                        Math.Abs(ori.G - backColor.G) < backColor_dis && 
                        Math.Abs(ori.B - backColor.B) < backColor_dis)
                    {
                        //截图时也是背景色,无差异
                        if (Math.Abs(dst.R - backColor.R) < backColor_dis
                            && Math.Abs(dst.G - backColor.G) < backColor_dis
                            && Math.Abs(dst.B - backColor.B) < backColor_dis)
                        {
                            //sum_delt++;
                        }
                        else
                        //截图为非背景色,因此有差异
                        {
                            sum_delt++;
                        }
                    } else//该点 应为 字体色
                    {
                        fontPixCount++;
                        // //截图时是背景色，因此有差异
                        if (Math.Abs(dst.R - backColor.R) < backColor_dis
                             && Math.Abs(dst.G - backColor.G) < backColor_dis
                             && Math.Abs(dst.B - backColor.B) < backColor_dis)
                        {

                            sum_delt++;
                        }
                        else//无差异
                        {

                           // sum_delt++;
                        }
                    }
                    //var dr = dst.R - ori.R;
                    //var dg = dst.G - ori.G;
                    //var db = dst.B - ori.B;
                   // var delt = Math.Sqrt(3 * dr * dr + 4 * dg * dg + 2 * db * db) / 3d / 255d * 100;
                   // sum_delt += delt;
                }
            var avg = sum_delt / (fontPixCount)*100;
            return avg;
        }
       
        public static double CalcDeltaOfTwoImg(ImageColor[,] srcIC,ImageColor[,] dstIC,Rectangle offsetRect)
        {
            var sum_delt = 0d;
            for (int x = 0; x < offsetRect.Width; x++)
                for (int y = 0; y < offsetRect.Height; y++)
                {
                    var ori = srcIC[x + offsetRect.X, y + offsetRect.Y];
                    var dst = dstIC[x, y];
                    var dr = dst.R - ori.R;
                    var dg = dst.G - ori.G;
                    var db = dst.B - ori.B;
                    var delt = Math.Sqrt(3*dr * dr +4* dg * dg + 2*db * db) / 3d / 255d * 100;
                    sum_delt += delt;
                }
            var avg = sum_delt   / (offsetRect.Width * offsetRect.Height);
            return avg;
        } 
        
    }
    public class PointBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;
        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        String src;

        public PointBitmap(Bitmap source, String src = null)
        {
            this.source = source;
            Width = source.Width;
            Height = source.Height;
            this.src = src;
        }

        public void LockBits()
        {

            // get total locked pixels count
            int PixelCount = Width * Height;

            // Create rectangle to lock
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // get source bitmap pixel format size
            Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

            // Check if bpp (Bits Per Pixel) is 8, 24, or 32
            if (Depth != 8 && Depth != 24 && Depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }

            // Lock bitmap and return bitmap data
            bitmapData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                         source.PixelFormat);

            //得到首地址
            unsafe
            {
                Iptr = bitmapData.Scan0;
                //二维图像循环
            }


        }

        public void UnlockBits()
        {
            source.UnlockBits(bitmapData);
        }

        /// <summary>
        /// 返回图像的抽样点序列
        /// 抽样方法如下:
        /// 
        /// 
        /// # # #
        /// # # #
        /// # # #
        /// 划分9个大区域
        /// 每个大区域取 米 型的所有像素
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ImageColor> toImageColor_Of_Sample()
        {
            LockBits();
            List<ImageColor> imgList = new List<ImageColor>();
            var block_w = this.Width / 4;
            var block_h = Height / 4;

            unsafe
            {
                byte* ptr = (byte*)Iptr;
                for (int x = block_w; x < Width - 1; x += block_w)
                {
                    for (int y = 0; y < Height; y += 1)
                    {
                        var nowptr = ptr + bitmapData.Stride * y + Depth * x / 8;
                        int r = nowptr[2];
                        int g = nowptr[1];
                        int b = nowptr[0];

                        var nowptr2 = ptr + bitmapData.Stride * y + Depth * (x + 1) / 8;
                        int r2 = nowptr[2];
                        int g2 = nowptr[1];
                        int b2 = nowptr[0];
                        imgList.Add(new ImageColor(r, g, b));
                        imgList.Add(new ImageColor(r2, g2, b2));

                    }
                }
                for (int y = block_h; y < Height - 1; y += block_h)
                {
                    for (int x = 0; x < Width; x += 1)
                    {
                        var nowptr = ptr + bitmapData.Stride * y + Depth * x / 8;
                        int r = nowptr[2];
                        int g = nowptr[1];
                        int b = nowptr[0];

                        var nowptr2 = ptr + bitmapData.Stride * (y + 1) + Depth * (x) / 8;
                        int r2 = nowptr[2];
                        int g2 = nowptr[1];
                        int b2 = nowptr[0];

                        imgList.Add(new ImageColor(r, g, b));
                        imgList.Add(new ImageColor(r2, g2, b2));

                    }
                }
            }


            UnlockBits();
            return imgList;
        }

        public ImageColor[,] toImageColor()
        {
           // if (new String[] {"http://g-search1.alicdn.com/img/bao/uploaded/i4/i3/3157741451/TB2P__ppwJlpuFjSspjXXcT.pXa_!!3157741451.jpg",
             //    "http://g-search2.alicdn.com/img/bao/uploaded/i4/i3/359348918/TB2RJBlXTtYBeNjy1XdXXXXyVXa_!!359348918.jpg",
              //   "http://g-search3.alicdn.com/img/bao/uploaded/i4/i3/3157741451/TB2P__ppwJlpuFjSspjXXcT.pXa_!!3157741451.jpg",

           // }.Contains(src))
          //      return null;
            LockBits();
            ImageColor[,] color = new ImageColor[this.Width, this.Height];
            unsafe
            {
                try
                {
                    byte* ptr = (byte*)Iptr;
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            var nowptr = ptr + bitmapData.Stride * y + Depth * x / 8;
                            int r = nowptr[2];
                            int g = nowptr[1];
                            int b = nowptr[0];
                            color[x, y] = new ImageColor(nowptr[2], nowptr[1], nowptr[0]);
                        }
                    }
                }
                catch (Exception exx)
                {
                    return null;
                }
                finally
                {

                    UnlockBits();
                }
            }
            return color;
        }

    }


    public class ImageColorLibary
    {

        //灰度统计
        public static Int32[] Total(Bitmap bmp)
        {
            var ic=ImageColor.FromBitmap(bmp);
            return Total(ic);
        }
        //灰度统计
        public static Int32[] Total(ImageColor[,] ic)
        {
            Int32[] GrayTotal = new Int32[256];
            for (int i = 0; i < 256; i++)
                GrayTotal[i] = 0;
            var w = ic.GetLength(0);
            var h = ic.GetLength(1);
            for (int x=0;x<w ;x++)
            {
                for(int y=0; y<h;y++)
                {
                    var Level=(ic[x,y].R * 19595 + ic[x, y].G * 38469 + ic[x, y].B * 7472) >> 16;
                    GrayTotal[Level]++;
                }
            }
            return GrayTotal;
        }
    }

    public class RGBImgTotal
    {

        public static readonly int graybins = 2;
        /// <summary>
        /// H通道分区间的 数量相对值
        /// </summary>
        public int[] gary_num_total;
        /// <summary>
        /// H通道分区间的 数量相对值
        /// </summary>
        public Double[] gray_num_total_d;
        public class RS
        {
            public double gray;
            public double sqrt_gray;
            public double avg_gray;
        }
        public RGBImgTotal(Bitmap bmp)
        {
           
            gary_num_total = new int[256 / graybins];
            gray_num_total_d = new Double[256 / graybins];
            var max = bmp.Width * bmp.Height;
            var imgs = ImageColor.FromBitmap(bmp);
            //CALC
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var p = imgs[x, y];
                    var gray = (p.R * 19595 + p.G * 38469 + p.B * 7472) >> 16;
                    gary_num_total[(int)(gray / graybins)]++;
                    gray_num_total_d[(int)(gray / graybins)]++;
                }
            for (var i = 0; i < gary_num_total.Length; i++)
            {
                gary_num_total[i] = gary_num_total[i] * 100 / max;
                gray_num_total_d[i] = gray_num_total_d[i] / max;
            }
            //bmp.Dispose();
        }
        public RS CalcCYD(RGBImgTotal rgvt)
        {
            double[] gray_a = new Double[gray_num_total_d.Length];
            double gray = 0d;
            double sqrt_gray = 0;
            var graylen = 0;
            for (var i = 0; i < gray_num_total_d.Length; i++)
            {
                gray_a[i] = Math.Abs(gray_num_total_d[i] - rgvt.gray_num_total_d[i]);
                gray += gray_a[i];
                if (rgvt.gray_num_total_d[i] != 0 || gray_num_total_d[i] != 0)
                {
                    graylen++;
                }
            }
            if (graylen == 0)
                graylen = 1;
            var avg_gray = gray / graylen;
            for (var i = 0; i < gray_num_total_d.Length; i++)
            {
                var dd = (gray_a[i] - avg_gray);
                sqrt_gray += dd * dd;
            }

            sqrt_gray = Math.Sqrt(sqrt_gray / graylen);
            //  h /= 2;
            // s /= 2;
            // v/= 2;
            return new RS() { avg_gray = avg_gray, gray = gray, sqrt_gray = sqrt_gray };
        }
    }
}
