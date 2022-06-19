using GamePageScript.lib;
using lib.image;
using MRFZ_Auto.script;
using MRFZ_Auto.script.mrfz;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script.mrfz.map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;
using static MRFZ_Auto.script.CharPoint;
using static MRFZ_Auto.script.mrfz.battle.BattleCharRec;

namespace GamePageScript.script.mrfz
{
    public partial class mrfz_ScriptUnit
    {
        public class rs_put_pos
        {
            public String L;
            public String H;
            public Dir dir_L;
            public Dir dir_H;
        }
        Dictionary<mrfz_ScriptConfig.ArkChar, int> char_cost = new Dictionary<mrfz_ScriptConfig.ArkChar, int>()
        {

            [mrfz_ScriptConfig.ArkChar.山] = 11,
            [mrfz_ScriptConfig.ArkChar.煌] = 24,
            [mrfz_ScriptConfig.ArkChar.棘刺] = 20,
            [mrfz_ScriptConfig.ArkChar.帕拉斯] = 18,
            [mrfz_ScriptConfig.ArkChar.令] = 12,
        };

        int COST_HEAL = 17;
        int MainCharCost;
        int Cost = 0;
        RecReulst bcr = null;
        /// <summary>
        /// 放置角色通用
        /// </summary>
        protected void Common_battle_to_putchar()
        {
            MainCharCost = char_cost[mrfz_ScriptConfig.scriptConfig.mainChar];
            Cost = 0;
            while (true)
            {
                float delta = -999;
                if ((Cost = MAP_COST.CurCost(Cost, out delta)) >=
                    MainCharCost)
                {
                    break;
                }
                else
                {
                    if (Debug)
                    {
                        onMsg?.Invoke($"COST={Cost},DTA= {delta}");
                    }
                    wait(500);
                }
            }
            var dstTime_REC_IMG = DateTime.Now.AddMilliseconds(4000);
            //ImageSave.Save(ImageSave.SaveType.SaveBattleHeadImg);
            while (true)
            {
                if (DateTime.Now > dstTime_REC_IMG)
                {

                    StopScript($"不能识别{mrfz_ScriptConfig.scriptConfig.mainChar}头像, 识别超时");
                    break;
                }
                bcr = BattleCharRec.CharRecNow();
                if (!bcr.Char_PointIndex.ContainsKey(mrfz_ScriptConfig.scriptConfig.mainChar))
                {
                    onMsg?.Invoke($"不能识别{mrfz_ScriptConfig.scriptConfig.mainChar}头像,重新尝试识别");
                    System.Threading.Thread.Sleep(200);
                }
                else
                {
                    break;
                }
            }
            var P_Click = mrfz_rogue_battlemap.GetCharPOS_Start(bcr.Char_PointIndex[mrfz_ScriptConfig.scriptConfig.mainChar] - 1);
            adb.Tap(P_Click);
            onMsg?.Invoke("P1/2-> Battle_PutChar");

            var flag = BattleMap.Battle_PutChar.CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript("放置角色 判断超时----CODE =21");
                return;
            }
        }
        /// <summary>
        /// 计算L1,L2区域哪个更符合当前战斗地图
        /// </summary>
        /// <param name="MapName">战斗地图名</param>
        /// <param name="L1"></param>
        /// <param name="L2"></param>
        /// <returns>L1或者L2</returns>
        protected String CalcMindltOfTwoLowRegion(String MapName, String L1, String L2)
        {
            Bitmap bmp = mrfzGamePage.CatptureImg();
            var src = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            var dst_L1 = PutCharMap.Maps[MapName].LowLand[L1].srcIC;
            var dst_L2 = PutCharMap.Maps[MapName].LowLand[L2].srcIC;
            var d1 = ImageColor.CalcDeltaOfTwoImg(src, dst_L1, PutCharMap.Maps[MapName].LowLand[L1].rect);
            var d2 = ImageColor.CalcDeltaOfTwoImg(src, dst_L2, PutCharMap.Maps[MapName].LowLand[L2].rect);
            if (d1 < d2)
            {
                return L1;
            }
            else
            {
                return L2;
            }
        }

        protected void SwipeToPutChar(String MapName, Boolean isLowLand, int CharIndex, String LandName, Boolean isForceDir,
            Dir dir = Dir.右, int ms = 600)
        {

            int delta = 300;
            var P_Click = mrfz_rogue_battlemap.GetCharPOS_Start(CharIndex - 1);
            if (isLowLand)
            {
                adb.Swipe(P_Click, PutCharMap.Maps[MapName].LowLandPoints[LandName].Point, ms);
                Dir dstDir = PutCharMap.Maps[MapName].LowLandPoints[LandName].DIR_PUT;
                if (isForceDir)
                {
                    dstDir = dir;
                }
                Point dstPoint = PutCharMap.Maps[MapName].LowLandPoints[LandName].Point;
                switch (dir)
                {
                    case Dir.上:
                        dstPoint.Y -= delta;
                        break;
                    case Dir.下:
                        dstPoint.Y += delta;
                        break;
                    case Dir.右:
                        dstPoint.X += delta;
                        break;
                    case Dir.左:
                        dstPoint.X -= delta;
                        break;
                }
                if (dstPoint.X < 0) dstPoint.X = 0;
                if (dstPoint.X >= ImgW) dstPoint.X = ImgW - 1;
                if (dstPoint.Y < 0) dstPoint.Y = 0;
                if (dstPoint.Y >= ImgH) dstPoint.Y = ImgH - 1;
                adb.Swipe(PutCharMap.Maps[MapName].LowLandPoints[LandName].Point, dstPoint, ms);
            }
            else
            {
                adb.Swipe(P_Click, PutCharMap.Maps[MapName].HighLandPoints[LandName].Point, ms);
                Dir dstDir = PutCharMap.Maps[MapName].HighLandPoints[LandName].DIR_PUT;
                if (isForceDir)
                {
                    dstDir = dir;
                }
                Point dstPoint = PutCharMap.Maps[MapName].HighLandPoints[LandName].Point;
                switch (dir)
                {
                    case Dir.上:
                        dstPoint.Y -= delta;
                        break;
                    case Dir.下:
                        dstPoint.Y += delta;
                        break;
                    case Dir.右:
                        dstPoint.X += delta;
                        break;
                    case Dir.左:
                        dstPoint.X -= delta;
                        break;
                }
                if (dstPoint.X < 0) dstPoint.X = 0;
                if (dstPoint.X >= ImgW) dstPoint.X = ImgW - 1;
                if (dstPoint.Y < 0) dstPoint.Y = 0;
                if (dstPoint.Y >= ImgH) dstPoint.Y = ImgH - 1;
                adb.Swipe(PutCharMap.Maps[MapName].HighLandPoints[LandName].Point, dstPoint, ms);
            }
        }

        protected void BattleRun_近卫()
        {
            onMsg?.Invoke("BattleRun_近卫 start");
            //PRE---在战斗前的判断 
            switch (BattleMap.MapName)
            {
                case "驯兽小屋":
                    onMsg?.Invoke("Pre_近卫_驯兽小屋");
                    this.Pre_近卫_驯兽小屋();
                    break;
            }
            //RUN
            //放置角色共通
            Common_battle_to_putchar();
            rs_put_pos rs_pos = null;
            //获取放置位置-L,H
            switch (BattleMap.MapName)
            {
                case "驯兽小屋":
                    rs_pos = GetLH_近卫_驯兽小屋();
                    break;
                case "意外":
                    rs_pos = GetLH_近卫_意外();
                    break;
                case "礼炮小队":
                    rs_pos = GetLH_近卫_礼炮小队();
                    break;
                case "与虫为伴":
                    rs_pos = GetLH_近卫_与虫为伴();
                    break;
                case "死斗":
                    rs_pos = GetLH_近卫_死斗();
                    break;
                default:
                    throw new Exception("地图名错误");
            }
            //放置
            SwipeToPutChar(BattleMap.MapName, true,
               bcr.Char_PointIndex[mrfz_ScriptConfig.scriptConfig.mainChar], rs_pos. L, true, rs_pos.dir_L, 
               mrfz_ScriptConfig.scriptConfig.DragRoleToBattleTime_ms
                );
            Boolean putCharFlag = false;
            wait(300);
            bcr = BattleCharRec.CharRecNow(false);
            if(bcr.Char_PointIndex.ContainsKey(mrfz_ScriptConfig.scriptConfig.mainChar))
            {
                onMsg?.Invoke($"放置{mrfz_ScriptConfig.scriptConfig.mainChar}失败,尝试重新放置");
                for(int k=0; k<3;k++)
                {
                    SwipeToPutChar(BattleMap.MapName, true,
                      bcr.Char_PointIndex[mrfz_ScriptConfig.scriptConfig.mainChar], rs_pos.L, true, rs_pos.dir_L,
                          1800);
                    wait(300);
                    bcr = BattleCharRec.CharRecNow();
                    if (bcr.Char_PointIndex.ContainsKey(mrfz_ScriptConfig.scriptConfig.mainChar))
                    {
                        onMsg?.Invoke($"放置{mrfz_ScriptConfig.scriptConfig.mainChar}失败,尝试重新放置");
                    }
                    else
                    {

                        putCharFlag = true;
                        break;
                    }
                }
            }else
            {
                putCharFlag = true;
            }
            if(!putCharFlag)
            {
                
                StopScript("放置角色失败，请重新运行脚本，或联系作者");
            }
            putCharFlag = false;
            //SKILL---ONLY 山
            if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.山)
            {
                // SkillReadyMap.Maps[BattleMap.MapName].LowLand[L];
                //通过L的名字 获取 点击位置。。。待完成
                var SKILL_L_P=NormalMap.Maps[BattleMap.MapName].LowLandPoints[rs_pos.L];
                waitSec(5);
                //角色技能 
                adb.Tap(SKILL_L_P);
                //PageClick(BattleMap.Battle_First.Name, $"PW{final_PW_Index}", false);
                WaitToCurPage(BattleMap.Battle_Skill.Name, TimeOut_NORMAL);
                var USESKILL_L_P=UseSkillMap.Maps[BattleMap.MapName].LowLandPoints[rs_pos.L].Point;
                waitSec(1);
                adb.Tap(USESKILL_L_P);
              //  PageClick(BattleMap.Battle_Skill.Name, $"PW{final_PW_Index}", false);

                onMsg?.Invoke($"SKILL ");
                WaitToCurPage(BattleMap.Battle_First.Name, TimeOut_NORMAL);
                waitSec(1);
                COST_HEAL = 17;
            }else
            {
                waitSec(2);
            }
            COST_HEAL = 17;
            //HEAL CHAR
            {
                Cost = 0;
                while (true)
                {
                    float delta = -999;
                    if ((Cost = MAP_COST.CurCost(Cost, out delta)) >= COST_HEAL)
                    {
                        //PUT HEAL CHAR
                        onMsg?.Invoke($"PUT CHAR HEAL ");

                        ArkChar[] HealChar = new ArkChar[] { ArkChar.安赛尔,
                         ArkChar.芙蓉, ArkChar.医疗预备干员,
                         ArkChar.调香师,
                         ArkChar.末药, ArkChar.苏苏洛,
                         ArkChar.嘉维尔, ArkChar.清流, ArkChar.褐果};
                        //ImageSave.Save(ImageSave.SaveType.SaveBattleHeadImg);
                        bcr = null;
                        //  var  dst = DateTime.Now.AddMilliseconds(4000);
                        //ImageSave.Save(ImageSave.SaveType.SaveBattleHeadImg);
                        var dstTime_REC_IMG = DateTime.Now.AddMilliseconds(5000);
                        int Heal_P_Index = -1;
                        ArkChar CurHealChar = ArkChar.安赛尔;
                        while (true)
                        {
                            if (DateTime.Now > dstTime_REC_IMG)
                            {

                                StopScript($"不能识别医疗干员头像,请联系作者.  ");
                                return;
                            }
                            bcr = BattleCharRec.CharRecNow(true);
                            Heal_P_Index = -1;
                            foreach (var hc in HealChar)
                            {
                                if (bcr.Char_PointIndex.ContainsKey(hc))
                                {
                                    CurHealChar = hc;
                                    Heal_P_Index = bcr.Char_PointIndex[hc];
                                    break;
                                }
                            }
                            if (Heal_P_Index == -1)
                            {
                                onMsg?.Invoke($"不能识别医疗干员头像头像,重新尝试识别");
                                System.Threading.Thread.Sleep(200);
                                continue;
                            }
                            else
                            {

                                break;
                            }
                        }
                        //放置HEAL 待完成
                        SwipeToPutChar(BattleMap.MapName, false,
                Heal_P_Index, rs_pos.H, true, rs_pos.dir_H, mrfz_ScriptConfig.scriptConfig.DragRoleToBattleTime_ms
              );
                        wait(300);
                        bcr = BattleCharRec.CharRecNow(true);
                        if (bcr.Char_PointIndex.ContainsKey(CurHealChar))
                        {
                            onMsg?.Invoke($"放置{CurHealChar}失败,尝试重新放置");
                            for (int k = 0; k < 3; k++)
                            {
                                SwipeToPutChar(BattleMap.MapName, false, Heal_P_Index, rs_pos.H, true, rs_pos.dir_H,  1800);
                                wait(300);
                                bcr = BattleCharRec.CharRecNow(true);
                                if (bcr.Char_PointIndex.ContainsKey(CurHealChar))
                                {
                                    onMsg?.Invoke($"放置{CurHealChar}失败,尝试重新放置");
                                }
                                else
                                {
                                    putCharFlag = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            putCharFlag = true;
                        }
                        if (!putCharFlag)
                        {
                            onMsg?.Invoke($"放置{CurHealChar}失败。");
                           // StopScript("放置角色失败，请重新运行脚本，或联系作者");
                        }
                        putCharFlag = false;
                        WaitToCurPage(BattleMap.Battle_First.Name, TimeOut_NORMAL);
                        break;
                    }
                    else
                    {
                        if (Debug)
                        {
                            onMsg?.Invoke($"COST={Cost},DTA= {delta}");
                        }
                        wait(500);
                    }
                }
            }
            //AFTER 

        }
    }
}
