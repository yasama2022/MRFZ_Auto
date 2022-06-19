using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;

namespace MRFZ_Auto.script.mrfz.battle
{
    public class BattleCharRec
    { 
          static   BattleCharRec()
        {
            LoadChar_SkinImg_MainChar(ArkChar.山, "山1");
            LoadChar_SkinImg_MainChar(ArkChar.山, "山2");
            LoadChar_SkinImg_MainChar(ArkChar.山, "山3");
            LoadChar_SkinImg_MainChar(ArkChar.煌, "煌1");
            LoadChar_SkinImg_MainChar(ArkChar.煌, "煌2");
            LoadChar_SkinImg_MainChar(ArkChar.煌, "煌3");
            LoadChar_SkinImg_MainChar(ArkChar.帕拉斯, "帕拉斯1");
            LoadChar_SkinImg_MainChar(ArkChar.帕拉斯, "帕拉斯2");
            LoadChar_SkinImg_MainChar(ArkChar.帕拉斯, "帕拉斯3");
            LoadChar_SkinImg_Heal(ArkChar.医疗预备干员, "医疗预备干员");
            LoadChar_SkinImg_Heal(ArkChar.安赛尔, "安赛尔1");
            LoadChar_SkinImg_Heal(ArkChar.安赛尔, "安赛尔2");
            LoadChar_SkinImg_Heal(ArkChar.芙蓉, "芙蓉1");
            LoadChar_SkinImg_Heal(ArkChar.芙蓉, "芙蓉2");
            LoadChar_SkinImg_Heal(ArkChar.调香师, "调香师1");
            LoadChar_SkinImg_Heal(ArkChar.调香师, "调香师2");
            LoadChar_SkinImg_Heal(ArkChar.调香师, "调香师3");
            LoadChar_SkinImg_Heal(ArkChar.调香师, "调香师4");
            LoadChar_SkinImg_Heal(ArkChar.末药, "末药1");
            LoadChar_SkinImg_Heal(ArkChar.末药, "末药2");
            LoadChar_SkinImg_Heal(ArkChar.末药, "末药3");
            LoadChar_SkinImg_Heal(ArkChar.苏苏洛, "苏苏洛1");
            LoadChar_SkinImg_Heal(ArkChar.苏苏洛, "苏苏洛2");
            LoadChar_SkinImg_Heal(ArkChar.苏苏洛, "苏苏洛3");
            LoadChar_SkinImg_Heal(ArkChar.嘉维尔, "嘉维尔1");
            LoadChar_SkinImg_Heal(ArkChar.嘉维尔, "嘉维尔2");
            LoadChar_SkinImg_Heal(ArkChar.嘉维尔, "嘉维尔3");
            LoadChar_SkinImg_Heal(ArkChar.清流, "清流1");
            LoadChar_SkinImg_Heal(ArkChar.清流, "清流2");
            LoadChar_SkinImg_Heal(ArkChar.褐果, "褐果1");
            LoadChar_SkinImg_Heal(ArkChar.褐果, "褐果2");
            //LoadChar_SkinImg_Support(ArkChar.令, "令1");
           // LoadChar_SkinImg_Support(ArkChar.令, "令2");
            /***
             * 
             * 
             * 
             *   调香师,
            末药,
            苏苏洛,
            嘉维尔,
            清流,
            褐果, 
             */
            for (int i = 0; i < 5; i++)
            {
                PointList.Add(new Rectangle(new Point(1183 - i * 119, 631),
                     new Size(61, 62)));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AscOrder">角色识别 按从小到大顺序 识别</param>
        /// <returns></returns>
        public static RecReulst CharRecNow(Boolean Heal=false,Bitmap src=null)
        { 
            RecReulst reulst = new RecReulst(); 
            if(src==null)
              src=mrfzGamePage.CatptureImg();
            ImageColor[,] srcIc = ImageColor.FromBitmap(src);
            src.Dispose();
            for(int index=0;index< PointList.Count;index++)
            {
                var P = PointList[index];
                Boolean PonitSearch = false;
                var CILIST = Heal ? HealCharImageList : SwordCharImageList;
                foreach (var kv in CILIST)
                {
                    Boolean HasSearchFlag = false;
                    var NowRecChar = kv.Key;
                    var ICList=kv.Value;
                    foreach(var IC in ICList)
                    {
                        //IC P srcIC
                        var delta = ImageColor.CalcDeltaOfTwoImg(srcIc, IC, P);
                            if(delta< mrfz_ScriptConfig.scriptConfig.dlt_battle_headimg)
                        {
                            //P
                            HasSearchFlag = true;
                            break;
                        }
                    }
                    if(HasSearchFlag)
                    {
                        reulst.PointIndex_Char.Add(index + 1, NowRecChar);
                        reulst.Char_PointIndex.Add(NowRecChar,index + 1);
                        PonitSearch = true;
                        break;
                    }
                }
                if(!PonitSearch)
                {
                    //..该位置未识别到角色 
                    reulst.PointIndex_Char.Add(index + 1, ArkChar.未知);
                }

            }
            return reulst;
        }
        static double delta_max = 20;
        public class RecReulst
        {
            public Dictionary<int, ArkChar> PointIndex_Char = new Dictionary<int, ArkChar>();
            public Dictionary<ArkChar, int> Char_PointIndex = new Dictionary<ArkChar, int>();
        }
        static List<Rectangle> PointList = new List<Rectangle>();
        static Dictionary<ArkChar, List<ImageColor[,]>> SupportCharImageList = new Dictionary<ArkChar, List<ImageColor[,]>>();
        static Dictionary<ArkChar, List<ImageColor[,]>> SwordCharImageList = new Dictionary<ArkChar, List<ImageColor[,]>>();
        static Dictionary<ArkChar, List<ImageColor[,]>> HealCharImageList = new Dictionary<ArkChar, List<ImageColor[,]>>();

        static void LoadChar_SkinImg_MainChar(ArkChar c,String skinName)
        {
            var fullName=Environment.CurrentDirectory + "\\imgs\\battle_head_imgs\\" + skinName+".png";
            var ic= ImageColor.FromFile(fullName);
           if( !SwordCharImageList.ContainsKey(c))
            {
                SwordCharImageList[c] = new List<ImageColor[,]>();
            }
            SwordCharImageList[c].Add(ic);
        }
        static void LoadChar_SkinImg_Support(ArkChar c, String skinName)
        {
            var fullName = Environment.CurrentDirectory + "\\imgs\\battle_head_imgs\\" + skinName + ".png";
            var ic = ImageColor.FromFile(fullName);
            if (!SupportCharImageList.ContainsKey(c))
            {
                SupportCharImageList[c] = new List<ImageColor[,]>();
            }
            SupportCharImageList[c].Add(ic);
        }
        static void LoadChar_SkinImg_Heal(ArkChar c, String skinName)
        {
            var fullName = Environment.CurrentDirectory + "\\imgs\\battle_head_imgs\\" + skinName + ".png";
            var ic = ImageColor.FromFile(fullName);
            if (!HealCharImageList.ContainsKey(c))
            {
                HealCharImageList[c] = new List<ImageColor[,]>();
            }
            HealCharImageList[c].Add(ic);
        }
    }
}
