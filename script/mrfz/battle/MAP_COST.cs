using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz
{
    public class MAP_COST
    {
        public static int CurCost(int LastCost, Bitmap CurBMP,out float delta)
            
        {
            ImageColor[,] ic = ImageColor.FromBitmap(CurBMP);
            return CurCost(LastCost,ic, out delta);
        }
        public static int CurCost(int LastCost,out float delta)

        {
            var bmp=mrfzGamePage.CatptureImg();
             ImageColor[,] ic = ImageColor.FromBitmap(bmp);
            bmp.Dispose(); 
            int C= CurCost(LastCost,ic, out delta);
            return C;
        }
        public static float secondDelta = 102;
        public static float fisrt_min_Delta_max = 0;
        public static int CurCost(int Cost_Last,ImageColor[,] ic,out float delta)
        {
            int Cost = Cost_Last;
            int Cost_Cur = -1;
            float min_delta = 101; 
            Dictionary<int, float> GameFontsDelta = new Dictionary<int, float>();
            for(; Cost <=99; Cost++ )
            {
                GameFontsDelta[Cost]= GameFont.GameFonts[Cost].CurFontDelta(ic);
                if (GameFontsDelta[Cost]<1)
                {
                    delta = GameFontsDelta[Cost];
                    return Cost; 
                }
                if(min_delta>= GameFontsDelta[Cost])
                { 
                    if(secondDelta>min_delta  )
                    {
                        secondDelta = min_delta;
                    }
                    min_delta = GameFontsDelta[Cost];
                    Cost_Cur = Cost;
                    
                }
               // if(min_delta<3)
              //  {
                //    return Cost_Cur;
              //  }
            }
            //GameFontsDelta = GameFontsDelta.OrderBy(r => r.Value).ToDictionary(r => r.Key, r => r.Value);
            //GameFontsDelta.Min(kv =>kv.Value);
            delta = min_delta;
            if (fisrt_min_Delta_max < min_delta)
            {
                fisrt_min_Delta_max = min_delta;
            }
            return Cost_Cur;
        }
    }
    public class GameFont
    {
        public static Point offset;
        public static String FontPath { get; private set; }
        public static Dictionary<int, GameFont> GameFonts { get; private set; }
        static GameFont()
        {
            FontPath = Environment.CurrentDirectory + "\\imgs\\costfont";
            fontColor = Color.White;
            backColor = Color.Black;
            GameFonts = new Dictionary<int, GameFont>();
           for( int cost = 0;   cost<=99; cost++)
            {
                GameFonts.Add(cost, new GameFont(FontPath + $"\\{cost}.png",
                    true,cost,""));
            }
            fontSize = new Size(77, 37);
            offset = new Point(1197, 497);
        }
        /// <summary>
        /// 返回差异度 百分比,0-100,float 
        /// </summary>
        /// <param name="FullImageIC"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public float  CurFontDelta(ImageColor[,] FullImageIC)
        {
            float Delta = 0f;
            for (int x = 0; x < fontSize.Width; x++)
                for (int y= 0; y < fontSize.Height; y++)
                {
                    var col = FullImageIC[x + offset.X, y + offset.Y];
                    var dis_back = Math.Abs(col.R - backColor.R) + 
                        Math.Abs(col.G - backColor.G) +
                        Math.Abs(col.B - backColor.B);
                    var dis_font= Math.Abs(col.R - fontColor.R) +
                        Math.Abs(col.G - fontColor.G) +
                        Math.Abs(col.B - fontColor.B);
                    var cur=dis_back > dis_font ? fontColor : backColor;
                    if(cur.R!=ic[x,y].R|| cur.G != ic[x, y].G|| cur.B != ic[x, y].B)
                    {
                        Delta++;
                    } 
                }
           return Delta / fontSize.Width / fontSize.Height*100;
        }
        public GameFont (String imgFile,Boolean isNumber,int Number,String text)
        {
            this.ImgFile = imgFile;
            this.isNumber = isNumber;
            this.Number = Number;
            this.Text = text;
            ic = ImageColor.FromFile(imgFile);
        }
        protected ImageColor[,] ic;
        public String ImgFile;
        public bool isNumber;
        public String Text;
        public int Number;
        public static Color fontColor;
        public static Color backColor;
        public static Size fontSize { get; private set; }
    }
}
