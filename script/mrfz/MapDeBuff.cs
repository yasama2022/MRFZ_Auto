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
    public class MapDeBuff
    {
        public static Dictionary<DeBuff, MapDeBuff> deBuffs = new Dictionary<DeBuff, MapDeBuff>();
        public String ImgFile;
        public ImageColor[,] detIC;
        public DeBuff debuff;
        public static Point TwoBuff_Left_Start;
        public static Point TwoBuff_Right_Start;
        public static Point OneBuff_Start;
        public static Size size;
        public static DeBuff[] CurMapDebuff(Bitmap bmp=null)
        {
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
            Rectangle REC_Buff_1 = new Rectangle(TwoBuff_Left_Start, size);
            Rectangle REC_Buff_2 = new Rectangle(TwoBuff_Right_Start, size);
            Rectangle REC_Buff_0 = new Rectangle(OneBuff_Start, size);
            var src=ImageColor.FromBitmap(bmp);
            var list = new List<ImageColor>() { src[658, 22], src[659, 22], src[661, 22], src[658, 42], src[659, 42],
                src[661, 42], };
            Color BackColor = Color.FromArgb(72,5,5);
            bmp.Dispose();
            Boolean isOneBuff = true;
            foreach(var li in list)
            {
                var R = Math.Abs(li.R - BackColor.R);
                var G = Math.Abs(li.G - BackColor.G);
                var B = Math.Abs(li.B - BackColor.B);
                if (R<10&&G<10&&B<10)
                {

                }else
                {
                    isOneBuff = false;
                    break;
                }
            }
            Boolean isTwoBuff = !isOneBuff;
            if(!isOneBuff)
            {
                list = new List<ImageColor>() { src[592, 22], src[593, 22], src[670, 22], src[671, 42], src[672, 42],
                src[593, 42], };
                foreach (var li in list)
                {
                    var R = Math.Abs(li.R - BackColor.R);
                    var G = Math.Abs(li.G - BackColor.G);
                    var B = Math.Abs(li.B - BackColor.B);
                    if (R < 10 && G < 10 && B < 10)
                    {

                    }
                    else
                    {
                        isTwoBuff = false;
                        return null; 
                    }
                }
            }
            Dictionary<DeBuff, double> dltsdic_left = new Dictionary<DeBuff, double>();
            Dictionary<DeBuff, double> dltsdic_right = new Dictionary<DeBuff, double>();
            Dictionary<DeBuff, double> dltsdic= new Dictionary<DeBuff, double>();
            foreach (var v in deBuffs)
            {
                if (v.Key == DeBuff.NONE) break;
                var dst=v.Value.detIC;
                if(isTwoBuff)
                {
                   // var dlt = ImageColor.CalcDeltaOfTwoImgUseBackColor(src, dst, BackColor, REC_Buff_1);
                    var dlt = ImageColor.CalcDeltaOfTwoImg(src, dst, REC_Buff_1);
                    dltsdic_left.Add(v.Key,dlt);
                    //  if (dlt <= mrfz_ScriptConfig.scriptConfig.dlt_region) return v.Key;
                    dlt = ImageColor.CalcDeltaOfTwoImg(src, dst, REC_Buff_2);
                 //   dlt = ImageColor.CalcDeltaOfTwoImgUseBackColor(src, dst, BackColor, REC_Buff_2);
                    dltsdic_right.Add(v.Key, dlt);

                    //  if (dlt <= mrfz_ScriptConfig.scriptConfig.dlt_region) return v.Key;

                }
                else
                {
                    var dlt = ImageColor.CalcDeltaOfTwoImg(src, dst, REC_Buff_0);
                    //var dlt = ImageColor.CalcDeltaOfTwoImgUseBackColor(src, dst, BackColor, REC_Buff_0);
                    dltsdic.Add(v.Key,dlt);
               //     if (dlt <= mrfz_ScriptConfig.scriptConfig.dlt_region) return v.Key;
                }
                 
            }
            if(isOneBuff)
            {
                var Min_dlt = dltsdic.Aggregate((l, r) => l.Value < r.Value ? l : r);
                return new DeBuff[] { Min_dlt.Key };
            }else
            {
                var Min_dlt = dltsdic_left.Aggregate((l, r) => l.Value < r.Value ? l : r);
                var Min_dlt2 = dltsdic_right.Aggregate((l, r) => l.Value < r.Value ? l : r);

                return new DeBuff[] { Min_dlt.Key, Min_dlt2.Key };
            } 
        }
        static MapDeBuff()
        {
            size = new Size(653 - 588 + 1, 42 - 22 + 1);
            TwoBuff_Left_Start = new Point(697, 23);
            TwoBuff_Right_Start = new Point(676, 23);
            OneBuff_Start = new Point(636,23);
            size = new Size(48, 18);
            deBuffs.Add(DeBuff.生存, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\生存.png",
                debuff = DeBuff.生存,

            });
            deBuffs.Add(DeBuff.敏感, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\敏感.png",
                debuff = DeBuff.敏感,

            }); deBuffs.Add(DeBuff.孤独, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\孤独.png",
                debuff = DeBuff.孤独,

            });
            deBuffs.Add(DeBuff.谨慎, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\谨慎.png",
                debuff = DeBuff.谨慎,

            });
            deBuffs.Add(DeBuff.盲目, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\盲目.png",
                debuff = DeBuff.盲目,

            }); 
            deBuffs.Add(DeBuff.迷茫, new MapDeBuff()
            {
                ImgFile = Environment.CurrentDirectory + "\\imgs\\mapdebuff\\迷茫.png",
                debuff = DeBuff.迷茫,

            });
            deBuffs.Add(DeBuff.NONE, new MapDeBuff() { debuff = DeBuff.NONE });
            foreach (var b in deBuffs)
            {
                b.Value.detIC = ImageColor.FromFile(b.Value.ImgFile);
            }
        }
        public enum DeBuff
        {
            生存,
            敏感,
            孤独,
            谨慎,
            盲目,
            迷茫, 
            NONE,
        }
    }
}
