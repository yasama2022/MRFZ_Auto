using GamePageScript.script.mrfz;
using lib.image;
using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz
{
    public class Add_Role_Sel
    {
        static RectColor rectColor_hasHope;
        static RectColor rectColor_NoHope;
        static ImageColor[,]  hasHopeIC;
        static ImageColor[,] NoHopeIC;
        static int Y =137;
        static int X = 1003- 613;
        public static int FindHasHopeIndex(Bitmap src=null)
        {
            if (src == null)
                src = mrfzGamePage.CatptureImg();
           var srcIC= ImageColor.FromBitmap(src);
            src.Dispose();
            Dictionary<int, double> nohope_index_hope_dic = new Dictionary<int, double>();
            var loc =  rectColor_NoHope.rect.Location;
            int index = 1;
            for (int x=0; x<2;x++)
                for(int y=0;y<4 ;y++)
                {
                    Rectangle rect = new Rectangle(new Point( loc.X+x*X,loc.Y+y*Y),
                        rectColor_NoHope.rect.Size);

                    var dlt = ImageColor.CalcDeltaOfTwoImg(srcIC, NoHopeIC, rect);
                    if(dlt<mrfz_ScriptConfig.scriptConfig.dlt_region)
                    {
                        nohope_index_hope_dic.Add(index, dlt);
                    }  
                    index++;
                }
            for(index=1; index<=8;index++)
            {
                if (nohope_index_hope_dic.ContainsKey(index))
                    continue;
                else
                    return index;
            }
            return 1;
           // var min = nohope_index_hope_dic.Aggregate((l, r) => l.Value < r.Value ? l : r); 
          //  return min.Key;
        }
        static Add_Role_Sel()
        {
            rectColor_hasHope = mrfzGamePage.GamePageDic["hashope"].regions[0];
            rectColor_NoHope = mrfzGamePage.GamePageDic["nohope"].regions[0];
            hasHopeIC = rectColor_hasHope.GetRegionIC();
            NoHopeIC = rectColor_NoHope.GetRegionIC();
            ///111->248 Y=135
            ///613->1003 X= 390 
        }
    }
}
