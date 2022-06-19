using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using lib.image;
using System.Drawing;
using GamePageScript.script.mrfz;

namespace MRFZ_Auto.script.mrfz.shop
{
    public class shop_gold
    {
        //F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\shop\goldfonts
        public static Dictionary<int, List<font>> Width_FontList = new Dictionary<int, List<font>>();
        public static Dictionary<int, font> FontList = new Dictionary<int,font>();
        public static Dictionary<int, int> font_width = new Dictionary<int, int>();
        public class font
        {
            //0-9
            public int Num=0;
            public String ImgFile;
            public int Width = 10;
            public ImageColor[,] font_ic;
        }
        public static int RecCurGolds(Bitmap bmp)
        {
            if(bmp==null)
              bmp = mrfzGamePage.CatptureImg();
            ImageColor[,] srcic = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            int X_OFFSET_END = Right;
            int Gold = 0;
            int Number = CurLevel_Number(X_OFFSET_END, srcic);
            if(Number>=0&&Number<=9)
            {
                Gold += Number;
                X_OFFSET_END -= font_width[Number];
                Number = CurLevel_Number(X_OFFSET_END, srcic);
                if (Number >= 0 && Number <= 9)
                {
                    Gold += Number*10;
                    X_OFFSET_END -= font_width[Number];
                    Number = CurLevel_Number(X_OFFSET_END, srcic);
                    if (Number > 0 && Number <= 9)
                    {
                        Gold += Number * 100; 
                    }
                }
                else
                {

                }
            }else
            {

            }
            bmp.Dispose();
            return Gold;
        }
        protected static Bitmap SaveToW(Bitmap rectBmp)
        {
            var ic = ImageColor.FromBitmap(rectBmp);// f.font_ic;
            int w = rectBmp.Width;
            int h = Height;
            Color backColor = Color.FromArgb(49, 49, 49);
            Color fontColor = Color.FromArgb(255, 255, 255);
            Bitmap newFont = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var dis_back = 3 * Math.Pow(ic[x, y].R - backColor.R, 2) +
                        4 * Math.Pow(ic[x, y].G - backColor.G, 2) +
                        2 * Math.Pow(ic[x, y].B - backColor.B, 2);
                    var dis_font = 3 * Math.Pow(ic[x, y].R - fontColor.R, 2) +
                        4 * Math.Pow(ic[x, y].G - fontColor.G, 2) +
                        2 * Math.Pow(ic[x, y].B - fontColor.B, 2);
                    if (dis_back > dis_font)
                    {

                        newFont.SetPixel(x, y, fontColor);
                    }
                    else
                    {

                        newFont.SetPixel(x, y, backColor);
                    }
                }
            }
            return newFont;
        }
        protected static double Dis_GrayFrom(ImageColor ic1,ImageColor ic2)
        {
            return -1;
        }
        protected static double DisGray(ImageColor[,] ic1,ImageColor[,] ic2,int w,int h)
        {
            double dlt = 0d;
            for (int x=0;x<w ;x++)
            {
                for(int y=0; y<h;y++)
                {
                    var g1 = (ic1[x, y].R + ic1[x, y].G + ic1[x, y].B) / 3;
                    var g2 = (ic2[x, y].R + ic2[x, y].G + ic2[x, y].B) / 3;
                    dlt+=Math.Abs(g1 - g2);
                }
            }
            dlt /= 255d;
            dlt *= 100; 
            return dlt;
        }
        public static double Test()
        {
            var dlt=DisGray(FontList[1].font_ic, ImageColor.FromFile(@"D:\1235_w_11.png"), FontList[1].Width, Height);
            return dlt;
        }
    
        protected static int CurLevel_Number(int X_OFFSET_END, ImageColor[,] srcic)
        {
            Dictionary<int, double> font_dlts = new Dictionary<int, double>(); 
            foreach (var kv in Width_FontList)
            {
                var W = kv.Key;
                var list = kv.Value;
                var startX = X_OFFSET_END - W;
            //    Rectangle rect = new Rectangle(new Point(startX, Y_Start), new Size(W, Height));
               // var img = ImageColor.FromIC(srcic, rect);
             //   img.Save($"D:\\{startX}_w_{W}.png");
                foreach (var f in list)
                {
                   
                    int sum = f.Width * Height;
                    double dlt = 0;
                    for(int x=0; x<f.Width;x++)
                    {
                        for(int y=0; y<Height;y++)
                        {
                           var Col= srcic[x + startX, y + Y_Start];
                           var fontCol = f.font_ic[x, y];
                            dlt += Math.Abs((Col.R + Col.G + Col.B) / 3- (fontCol.R + fontCol.G + fontCol.B) / 3);
                         //   var dis_p = Math.Sqrt(  ( Math.Pow(Col.R - fontCol.R, 2) +
                          //     1 * Math.Pow(Col.G - fontCol.G, 2) +
                         //      1 * Math.Pow(Col.B - fontCol.B, 2))/3 
                         //      );
                         //   dlt += dis_p/255;//0-255
                         //   var dis_font = 3 * Math.Pow(Col.R - fontCol.R, 2) +
                         //     4 * Math.Pow(Col.G - fontCol.G, 2) +
                         //     2 * Math.Pow(Col.B - v.B, 2);
                            /* if(dis_back>dis_font)
                             {
                                 var Checked = (fontCol.R == fontColor.R &&
                                      fontCol.G == fontColor.G &&
                                      fontCol.B == fontColor.B);
                                 if (Checked)
                                     dlt++;
                             }
                             else
                             {
                                 var Checked = (fontCol.R == backColor.R &&
                                      fontCol.G == backColor.G &&
                                      fontCol.B == backColor.B);
                                 if (Checked)
                                     dlt++;
                             }
                             */
                        }
                    }
                    font_dlts.Add(f.Num,dlt/sum/255d*100);
                   // var clonebmp = srcbmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //.

                    //  Save($"D:\\{f.Num}.png");
                   // SaveToW(clonebmp).Save($"D:\\{f.Num}.png");

                  //  return 10;
                }
            }
            double min = 9999;
            int recNum = 0;
            foreach(var kv in font_dlts)
            {
                if(kv.Value<= min)
                {
                   
                    min = kv.Value;
                    recNum = kv.Key;
                }
            } 
          //  if(max<74)
           // {
           //     recNum = -1;
           // }
           
            return recNum;
        }
        public static void GrayN()
        {
            Color backColor = Color.FromArgb(49,49,49);
            Color fontColor = Color.FromArgb(255,255,255);
            foreach(var kv in Width_FontList)
            {
                int w = kv.Key;
                int h = 19;
                foreach(var f in kv.Value)
                {
                    var ic= f.font_ic;
                    Bitmap newFont = new Bitmap(w, h);
                    for(int x=0; x<w;x++)
                    {
                        for(int y=0; y<h;y++)
                        {
                           /* var dis_back = 3 * Math.Pow(ic[x, y].R - backColor.R, 2) +
                                4 * Math.Pow(ic[x, y].G - backColor.G, 2) +
                                2 * Math.Pow(ic[x, y].B - backColor.B, 2);
                            var dis_font = 3 * Math.Pow(ic[x, y].R - fontColor.R, 2) +
                                4 * Math.Pow(ic[x, y].G - fontColor.G, 2) +
                                2 * Math.Pow(ic[x, y].B - fontColor.B, 2);
                            if(dis_back>dis_font)
                            {

                                newFont.SetPixel(x, y, fontColor);
                            }
                            */
                           if(Math.Abs( backColor.R- ic[x, y].R)<30
                                &&Math.Abs( ic[x, y].G-backColor.G)<30&&
                                Math.Abs( ic[x, y].B-backColor.B)<30)
                            {

                                newFont.SetPixel(x, y, backColor);
                            } else
                            {

                                newFont.SetPixel(x, y, fontColor);
                            }
                        }
                    }
                    newFont.Save(Environment.CurrentDirectory+ @"\imgs\shop\Gold_Font\"+f.Num+".png");
                    newFont.Dispose();
                }
            }
        }
        static int Right = 1192 + 54;// 1196+45+4+0;
          static int Y_Start = 24;
        static int Height= 21;
        static shop_gold()
        { 
            String fontDI = Environment.CurrentDirectory+ @"\imgs\shop\goldfonts";
            DirectoryInfo di = new DirectoryInfo(fontDI);
            if(di.Exists)
            {
                var fss=  di.GetFiles("*.png");
                foreach(var fs in fss)
                {
                   int num=int.Parse( fs.Name.Split('.')[0]);
                    Bitmap bmp = new Bitmap(fs.FullName);
                    int w=bmp.Width;
                    bmp.Dispose();
                    font f = new font() { Num=num, Width=w, ImgFile=fs.FullName, font_ic
                    =ImageColor.FromFile(fs.FullName)};
                    font_width.Add(num, w);
                    FontList.Add(num, f);
                    if (Width_FontList.ContainsKey(w))
                    {
                        Width_FontList[w].Add(f);
                    }
                    else
                    {
                        Width_FontList.Add(w, new List<font>() { f });
                    }
                }
                Width_FontList= Width_FontList.OrderByDescending((kv)=>
                {
                    return kv.Key;
                }).ToDictionary(x => x.Key, x => x.Value); ;
            }

        }
    }
}
