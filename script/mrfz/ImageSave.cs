using GamePageScript.script.mrfz;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using lib.image;
using GamePageScript.lib;

namespace MRFZ_Auto.script.mrfz
{
    //通过挂机过程中对途中的 头像,数字等需要识别的图像进行保存
    //仅在DEBUG目录下生效
    public class ImageSave
    {
        protected static bool Debug = false;
        static ImageSave()
        {
            if (Environment.CurrentDirectory.EndsWith("Debug"))
            {
                Debug = true; 
            }
        }
        public enum SaveType
        {
            SaveHeadImgInCharSelPage,
            SaveBattleHeadImg, SaveWinItem,SaveShopItem,
            SaveFriendsHeadImg,
        }
        public static void Save(SaveType st)
        {

            if (!Debug) return;
            waitSec(1);
            switch (st)
            {
                case SaveType.SaveHeadImgInCharSelPage:
                    SaveHeadImgInCharSelPage();
                    break;
                case SaveType.SaveBattleHeadImg: 
                    SaveBattleHeadImg();
                    break;
                case SaveType.SaveWinItem:
                    SaveWinItem();
                    break;
                case SaveType.SaveShopItem:
                    SaveShopItem();
                    break;
                case SaveType.SaveFriendsHeadImg:
                    SaveFrinedImgs();
                    break;

            }
        }
        //1280,809
      
        public static Bitmap ImgClone(Bitmap src,Rectangle rect)
        {
            return ImageColor.Clone(src,rect);
        }
        public static void SaveShopGold(Bitmap bmp)
        {
            if (!Debug) return;
            if(bmp==null)
              bmp = mrfzGamePage.CatptureImg();
            Rectangle rect = new Rectangle(new Point(1192 ,24), new Size(54 ,21));
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\shop_golds");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            var cz=  ImgClone(bmp, rect);
          //  var cz = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            cz.Save($"{di.FullName}\\{len++}.png");
            cz.Dispose();
            bmp.Dispose();
        }
        public static void SaveShopItem(Bitmap bmp = null)
        {
            SaveShopItemName(bmp);
            SaveShopItemExp(bmp);
        }
        public static void SaveShopItemName(Bitmap bmp = null)
        {

            if (!Debug) return;
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\shop\\itemname");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            int line = 1;
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(418+206*i,106), new Size(142, 22)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            line = 2;
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(418 + 206 * i, 317), new Size(142, 22)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }

        public static void SaveTeamLogo(Bitmap bmp = null)
        {
            if (!Debug) return; 
          //  Thread.Sleep(1000);
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\teamlogo");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            //XSTART=90
            //YSTART=511  SIZE=20,100
            //OFFSETX=284
           var srcIC= ImageColor.FromBitmap(bmp);
            for(int x=1;x<1280;x++)
            {
                var col = srcIC[x-1, 487];
           //     var l2 = srcIC[x-1, 488];
                if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                  Math.Abs(col.B - col.G) <= 5)
                {
                    continue;
                      
                }else
                {

                    col = srcIC[x, 487];
                    if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                 Math.Abs(col.B - col.G) <= 5)
                    {
                        col = srcIC[x, 488];
                        if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                        Math.Abs(col.B - col.G) <= 5)
                        {
                            int LEN = 10;
                            Boolean checkFlag = true;
                            for (int i = 0; i < LEN; i++)
                            {
                                col = srcIC[x + i, 487];
                                if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                       Math.Abs(col.B - col.G) <= 5)
                                {
                                    col = srcIC[x + i, 488];
                                    if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                      Math.Abs(col.B - col.G) <= 5)
                                    {

                                    }
                                    else
                                    {
                                        checkFlag = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    checkFlag = false;
                                    break;
                                }
                            }
                            if (checkFlag)
                            {
                                //
                                //108 270
                                //240 395
                                var x_start = x + 108 - 62;
                                var y_start = 270;
                                var h = 125;
                                var w = 132;
                                bmplist.Add(bmp.Clone(new Rectangle(new Point(x_start, y_start), new Size(w, h)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                                x += 222;
                                //62->121
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {

                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                     

            }
            for (int i = 0; i < 5; i++)
            {
                break;
                adb.Swipe(new Point(1172, 440), new Point(600, 440), 1000);
                Thread.Sleep(500);
                break;
                bmp = mrfzGamePage.CatptureImg();
                bmplist.Add(bmp.Clone(new Rectangle(new Point(90 + 4 * 284, 511), new Size(20, 43)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));

            }
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        public static void SaveShopItemExp(Bitmap bmp=null)
        {

            if (!Debug) return;
            if(bmp==null)
              bmp = mrfzGamePage.CatptureImg();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\shop\\itemexp");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            int line = 1;
            for(int i=0; i<4;i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(1162- 206*(3-i), 258), new Size(50, 18)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            line = 2;
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(1162 - 206 * (3 - i), 258+211), new Size(50, 18)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        public static void SaveMapDebuff(Bitmap bmp = null)
        {

            if (!Debug) return;
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
           var OneBuff_Start = new Point(636, 23);
            var TwoBuff_Left_Start = new Point(697, 23);
         var   TwoBuff_Right_Start = new Point(676, 23);
            var  size = new Size(48, 18);
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\mapdebuff");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            bmplist.Add(bmp.Clone(new Rectangle(OneBuff_Start, size), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            bmplist.Add(bmp.Clone(new Rectangle(TwoBuff_Left_Start, size), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            bmplist.Add(bmp.Clone(new Rectangle(TwoBuff_Right_Start, size), System.Drawing.Imaging.PixelFormat.Format32bppArgb));

            //  bmplist.Add(bmp.Clone(new Rectangle(new Point(667,
            //      22), new Size(653 - 588 + 1, 42 - 22 + 1)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));

            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        public static void SaveShopItemName_Gray()
        {
            //F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\shop\item_name\招募卷
            if (!Debug) return;

            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\shop\\item_name\\支援");
            if (!di.Exists) di.Create();
            var fss = di.GetFiles("*.png");
            Color fontCol = Color.White;
            Color backCol = Color.Black;
            foreach (var fs in fss)
            {
                Bitmap bmp = new Bitmap(42, 22);
                //Bitmap bmp = new Bitmap(fs.FullName);
                var ic = ImageColor.FromFile(fs.FullName);
                for (int x = 0; x < bmp.Width; x++)
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        var col = ic[x, y];
                        if (col.R > 100 && col.G > 100 & col.B > 100
                            && Math.Abs(col.R - col.G) < 30 && Math.Abs(col.R - col.G) < 30 & Math.Abs(col.G - col.B) < 30)
                        {
                            bmp.SetPixel(x, y, fontCol);
                        }
                        else
                        {

                            bmp.SetPixel(x, y, backCol);
                        }
                    }
                bmp.Save(Environment.CurrentDirectory + "\\imgs\\shop\\item_name\\支援font\\" + fs.Name);
                bmp.Dispose();
            }
        }
        public static void SaveShopItemExp_GRAY( )
        {

            if (!Debug) return;
           
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\shop\\item_exp");
            if (!di.Exists) di.Create();
            var fss=di.GetFiles("*.png");
            Color fontCol = Color.White;
            Color backCol = Color.Black;
             foreach(var fs in fss)
            {
                Bitmap bmp = new Bitmap(50,18);
                //Bitmap bmp = new Bitmap(fs.FullName);
                var ic=ImageColor.FromFile(fs.FullName);
                for(int x=0; x<bmp.Width;x++)
                    for(int y=0; y<bmp.Height;y++)
                    {
                        var col = ic[x, y];
                        if(col.R>100&&col.G>100&col.B>100
                            && Math.Abs(col.R - col.G) < 30 && Math.Abs(col.R - col.G) < 30 & Math.Abs(col.G - col.B) < 30)
                        {
                            bmp.SetPixel(x,y,fontCol);
                        }
                        else
                        {

                            bmp.SetPixel(x, y, backCol);
                        }
                    }
                bmp.Save(Environment.CurrentDirectory + "\\imgs\\shop\\exp_fonts\\"+fs.Name);
                bmp.Dispose();
            }
        }

        public static void SaveShopGoldFont()
        {
            if (!Debug) return;
            int Y = 0;int H = 21;
            int X_START = 38;
            int X_END = 53;
            Rectangle rect = new Rectangle(new Point(X_START, Y), new Size(X_END- X_START+1, H));
            Bitmap bmp = new Bitmap(Environment.CurrentDirectory+ @"\imgs\shop_golds\27.png");
            var dst=bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            dst.Save(Environment.CurrentDirectory + @"\imgs\shop_golds\font\9.png");
            dst.Dispose();
            bmp.Dispose();
        }
        protected static void SaveHeadImgInCharSelPage()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            Rectangle rect = new Rectangle(new Point(513,117),new Size(65, 65));
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory+"\\imgs\\selchar_headimgs");
            if (!di.Exists) di.Create();
            int len=  di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            for(int i=0;i<4;i++)
            {
                bmplist.Add( bmp.Clone(new Rectangle(new Point(513, 117+i*137), new Size(65, 65)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add( bmp.Clone(new Rectangle(new Point(513+ 390, 117 + i * 137), new Size(65, 65)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            foreach(var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        public static void SaveFriendHeadImgInFriendsSelPage()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            {
                
                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\freinds\\btn_ok");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for(int i=0; i<8;i++)
                {
                    var X = 128 + i * 139;
                    var Y = 636- (660 - 625);
                    var W = 30;
                    var H = 20;
                    Rectangle rect = new Rectangle(new Point(X,Y), new Size(W,H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\freinds\\name");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 8; i++)
                { 
                    var X = 143 + i * 139;
                    var W = 99;
                    var Y = 570 - (660 - 625);
                    var H = 19;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
        }
        public static void SaveBAOQUANFriendHeadImgInFriendsSelPage()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\friends\\btn_ok");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 8; i++)
                {
                    var X = 128 + i * 139;
                    var Y = 635;
                    var W = 30;
                    var H = 20;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\friends\\name");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 8; i++)
                {
                    var X = 143 + i * 139;
                    var W = 99;
                    var Y = 581  ;
                    var H = 19;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
        }
        public static void SaveBAOQUANPAIZHU_ZB()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\equip");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 4; i++)
                { 
                    var X = 143 + i * 315;
                    var Y = 214;
                    var W = 15;
                    var H = 16;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                for (int i = 0; i < 4; i++)
                {
                    var X = 143 + i * 315;
                    var Y = 214+ 229;
                    var W = 15;
                    var H = 16;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
            
        }
        public static void SaveTeamClass()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\team\\class");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 7; i++)
                {
                    var X = 59 + i * 133;
                    var Y = 115;
                    var W = 18;
                    var H = 18;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                for (int i = 0; i < 7; i++)
                {
                    var X = 59 + i * 133;
                    var Y = 115+250;
                    var W = 18;
                    var H = 18;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }

        }
        public static void SaveTeamCharName()
        {
            if (!Debug) return;
            Bitmap bmp = mrfzGamePage.CatptureImg();
            {

                DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\team\\name");
                if (!di.Exists) di.Create();
                int len = di.GetFiles("*.png").Length;
                List<Bitmap> bmplist = new List<Bitmap>();
                for (int i = 0; i < 7; i++)
                {
                    var X = 116 + i * 133;
                    var Y = 323;
                    var W = 40;
                    var H = 19;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                for (int i = 0; i < 7; i++)
                {
                    var X = 116 + i * 133;
                    var Y = 323 + 250;
                    var W = 40;
                    var H = 19;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    bmplist.Add(bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                foreach (var b in bmplist)
                {
                    b.Save($"{di.FullName}\\{len++}.png");
                }
            }
        }
        protected static void SaveWinItem()
        {
            if (!Debug) return;

            waitSec(1);
            Bitmap bmp = mrfzGamePage.CatptureImg();
            Rectangle rect = new Rectangle(new Point(1183, 631), new Size(61, 62));
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\winitems");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(75 + i * 280, 537),
                     new Size(45, 28)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            for (int i = 0; i < 4; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(107 + i * 280, 373),
                     new Size(114, 23)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        public static void SaveBattleHeadImg()
        {
            if (!Debug) return;
           // if(bmp==null)
            Bitmap bmp = mrfzGamePage.CatptureImg();
            Rectangle rect = new Rectangle(new Point(1183,631), new Size(61, 62));
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\battle_head_imgs");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            for (int i = 0; i < 3; i++)
            {
                bmplist.Add(bmp.Clone(new Rectangle(new Point(1183-i*119, 631 ),
                     new Size(61, 62)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            } 
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }
        }
        /***
         * 
         *  P[1] = new RectColor(new Rectangle(P[4].rect.X - 452, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height),
                   P[4].FileName,
                     "P1");
                P[7] = new RectColor(new Rectangle(P[4].rect.X + 451, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height),
                     P[4].FileName,
                     "P7");
                P[5] = new RectColor(new Rectangle(P[4].rect.X, P[4].rect.Y + 199, P[4].rect.Width, P[4].rect.Height),
                    P[4].FileName,
                    "P5");
                P[6] = new RectColor(new Rectangle(P[4].rect.X, P[4].rect.Y + 199 + 198, P[4].rect.Width, P[4].rect.Height),
                   P[4].FileName,
                   "P6");
                P[2] = new RectColor(new Rectangle(P[1].rect.X, P[1].rect.Y + 199, P[1].rect.Width, P[1].rect.Height),
                   P[4].FileName,
                   "P2");
                P[3] = new RectColor(new Rectangle(P[1].rect.X, P[1].rect.Y + 199 + 198, P[1].rect.Width, P[1].rect.Height),
                  P[4].FileName,
                  "P3");
                P[8] = new RectColor(new Rectangle(P[7].rect.X, P[7].rect.Y + 199, P[7].rect.Width, P[7].rect.Height),
                 P[4].FileName,
                 "P8");
                P[9] = new RectColor(new Rectangle(P[7].rect.X, P[7].rect.Y + 199 + 198, P[7].rect.Width, P[7].rect.Height),
                P[4].FileName,
                "P9");
         * */
        public static void SaveFrinedImgs()
        {
            if (!Debug) return;
            Rectangle[] P = new Rectangle[10];
            P[4] = mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].regions[0].rect;
            P[1] = new Rectangle(P[4].X - 452, P[4].Y,
                P[4].Width, P[4].Height);
            P[7] = new Rectangle(P[4].X +451, P[4].Y,
                P[4].Width, P[4].Height);
            P[5] = new Rectangle(P[4].X , P[4].Y+ 199,
                P[4].Width, P[4].Height); 
            P[6] = new Rectangle(P[4].X , P[4].Y + 199+ 198,
                P[4].Width, P[4].Height); 
            P[2] = new Rectangle(P[1].X , P[4].Y + 199,
                P[4].Width, P[4].Height);
            P[3] = new Rectangle(P[1].X , P[4].Y + 199 + 198,
                P[4].Width, P[4].Height);
            P[8] = new Rectangle(P[7].X , P[4].Y + 199,
                P[4].Width, P[4].Height);
            P[9] = new Rectangle(P[7].X , P[4].Y + 199 + 198,
                P[4].Width, P[4].Height);
            Bitmap bmp = mrfzGamePage.CatptureImg();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\freindhead");
            if (!di.Exists) di.Create();
            int len = di.GetFiles("*.png").Length;
            List<Bitmap> bmplist = new List<Bitmap>();
            for (int i = 1; i < 10; i++)
            {
                bmplist.Add(bmp.Clone(P[i], System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            }
            foreach (var b in bmplist)
            {
                b.Save($"{di.FullName}\\{len++}.png");
            }

        }
        protected static void waitSec(int sec)
        {
            var end = DateTime.Now.AddSeconds(sec);
            while (true)
            {
                if (DateTime.Now >= end)
                {
                    return;
                }
                Thread.Sleep(100);
            }
        }
        protected static void wait(int ms)
        {
            if (ms > 1000)
            {
                int t = ms / 1000;
                for (int i = 0; i < t; i++)
                {

                    Thread.Sleep(ms);

                }
                if (ms - t * 1000 > 0)
                    Thread.Sleep(ms - t * 1000);
            }
            else
                Thread.Sleep(ms);
        }


    }
}
