using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static MRFZ_Auto.script.保全派驻.通用.TeamCharName;

namespace MRFZ_Auto.script.保全派驻.通用
{
    public class TeamClassInTeamEdit
    {
       
        public static Dictionary<CharClass, ImageColor[,]> classIC = new Dictionary<CharClass, ImageColor[,]>();
        static TeamClassInTeamEdit()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\team\\class"); 
            var fss = di.GetFiles("*.png");
            foreach (var fs in fss)
            {
                CharClass rc = (CharClass)Enum.Parse(typeof(CharClass), fs.Name.Split('.')[0]);
                classIC.Add(rc, ImageColor.FromFile(fs.FullName));
            }
            return;
        }
        public static List<Tuple<CharClass, double>> GetCharClassInTeamEdit()
        {
            var bmp = mrfzGamePage.CatptureImg();
            var srcIC = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            List<Tuple<CharClass, double>> list = new List<Tuple<CharClass, double>>();
            for (int i = 0; i < 14; i++)
            {
                double dlt = 0d;
                list.Add(new Tuple<CharClass, double>(GetTeamClass(srcIC, i, out dlt), dlt));
            }
            return list;
        }
        public static CharClass GetTeamClass(ImageColor[,] srcIC, int Index, out double dlt)
        {
            Dictionary<CharClass, double> recDic = new Dictionary<CharClass, double>();
            var X = 116 + Index * 133;
            var Y = 323;
            var W = 40;
            var H = 19;
            Rectangle rect;
            if (Index < 7)
            {
                  X = 59 + Index * 133;
                  Y = 115;
                  W = 18;
                  H = 18; 
                  rect = new Rectangle(new Point(X, Y), new Size(W, H));

            }
            else
            { 
                  X = 59 +( Index-7) * 133;
                  Y = 115 + 250;
                  W = 18;
                  H = 18; 
                  rect = new Rectangle(new Point(X, Y), new Size(W, H));
            }

            foreach (var kv in classIC)
            {
                dlt = ImageColor.CalcDeltaOfTwoImg(srcIC, kv.Value, rect);

                // var dlt = ImageColor.CalcDeltaOfTwoImgInFont(srcIC, kv.Value, Color.FromArgb(34, 34, 34), rect,10);
                recDic.Add(kv.Key, dlt);
            }
            var min = recDic.Aggregate((l, r) => l.Value < r.Value ? l : r);

            dlt = min.Value;
            if (min.Value < 8)
            {
                return min.Key;
            }
            else
            {
                return CharClass.未知;
            }
        }
    
   }
}
