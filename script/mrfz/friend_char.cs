using GamePageScript.lib;
using GamePageScript.script.mrfz;
using lib.image;
using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;

namespace MRFZ_Auto.script.mrfz
{
    /// <summary>
    /// 助战干员
    /// </summary>
    public class friend_char
    {
        /// <summary>
        /// 助战干员的图像 IC=Bitmap
        /// </summary>
        protected Dictionary<String, ImageColor[,]> IC_dic = new Dictionary<string, ImageColor[,]>();
        /// <summary>
        /// 助战干员的图像 所在区域
        /// </summary>
        protected Dictionary<String, RectColor[]> P_dic = new Dictionary<string, RectColor[]>();
        /// <summary>
        /// 好友的干员(按钮识别)的矩形区域
        /// </summary>
        protected Dictionary<int, RectColor> Btn_OK_Region_Dic = new Dictionary<int, RectColor>();
        /// <summary>
        /// 1-6位置 判断为好友的图像(按钮)
        /// </summary>

        protected ImageColor[,] P1_6_FriendOK_IC;
        /// <summary>
        /// 7-9位置 判断为好友的图像(按钮)
        /// </summary>
        protected ImageColor[,] P789_FriendOK_IC;
        public friend_char(List<String> SkinGamePageNameList)
        {
            Boolean first = true;
            foreach(var pn in SkinGamePageNameList)
            {
                LoadSkinGamePageName(pn, first);
                first = false;
            }
        }
        public static Dictionary<ArkChar, friend_char> FriendChars { get; protected set; }
        static friend_char()
        {
            FriendChars = new Dictionary<ArkChar, friend_char>();
            FriendChars[ArkChar.山] = new friend_char(new List<string>() { "山1",
                "山2","山3" });
            FriendChars[ArkChar.煌] = new friend_char(new List<string>() { "煌1",
                "煌2","煌3" });
            FriendChars[ArkChar.帕拉斯] = new friend_char(new List<string>() { "帕拉斯1",
                "帕拉斯2","帕拉斯3" });
            FriendChars[ArkChar.令] = new friend_char(new List<string>() { "令1",
                "令2" });
            //  FriendChars[ArkChar.帕拉斯] = new friend_char(new List<string>() {  "帕拉斯3" });
        }
        public  void GetChar(out double delta)
        {
            if (!IsCurPage())
            {
                throw new Exception("Time out 判断助战页面");
            }

            var bmp = mrfzGamePage.CatptureImg();
            var index = what_number_inpage(bmp,out delta);
            bmp.Dispose();
            while (true)
            {
                if (index <= 0)
                {
                    while (!CanRefresh())
                    {
                        wait(500);
                        break;
                    }
                    PageClick("rogue-sel-chars-friend", "refresh", false);
                    WaitToNotCurPage("rogue-sel-chars-friend",2000);
                    bmp.Dispose();
                    bmp = mrfzGamePage.CatptureImg();
                    index = what_number_inpage(bmp,out delta); 
                    continue;
                }
                else
                {
                    wait(500);
                    /*if(Program.Debug)
                    { 
                        mrfz_ScriptUnit.onMsg?.Invoke($"测试模式,暂停 ,等待按继续按钮");
                        mrfz_ScriptMachine.Pause = true;
                        while (mrfz_ScriptMachine.Pause)
                        {
                            waitSec(1);
                        }
                       // mrfzGamePage.CatptureImg().Save("D:\\ailini.png");
                       // if(index>6)
                       // {
                        //    SwipeTo1_6();
                        //}
                        //index = -1;
                       // continue;
                    }
                    */
                    
                    //如果是好友干员

                    PageClick("rogue-sel-chars-friend", $"P{index}", false);

                    bmp.Dispose();
                    if (mrfzGamePage.GamePageDic["rogue-sel-chars-friend-ready-to-get"].CheckPage(TimeOut))
                    {

                        PageClick("rogue-sel-chars-friend-ready-to-get", $"NEXT", false); 
                        return;
                    }
                    else
                    {
                        throw new Exception("非助战准备招募页面 判断超时");
                    }

                }
            }
        }

        public static Boolean IsHeadImageRight(RectColor rc, ImageColor[,] src_ic, ImageColor[,] dst_ic, out double AvgDelta)
        {
            AvgDelta = -1;

            try
            {
                var ic = src_ic; 
                var rcbmp_ic = dst_ic;
                var sum_delt = 0d;
                for (int x = 0; x < rc.rect.Width; x++)
                    for (int y = 0; y < rc.rect.Height; y++)
                    {
                        var ori = ic[x + rc.rect.X, y + rc.rect.Y];
                        var dst = rcbmp_ic[x, y];
                        var dr = dst.R - ori.R;
                        var dg = dst.G - ori.G;
                        var db = dst.B - ori.B;
                        var delt = Math.Sqrt((3 * dr * dr + 4 * dg * dg + 2 * db * db)) / 3d / 255d * 100;
                        sum_delt += delt;
                    }
                AvgDelta = sum_delt / (rc.rect.Width * rc.rect.Height); 
                if (AvgDelta > mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
            }

        }
        public  int what_number_inpage(Bitmap bmp,out double delta)
        {
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
            StringBuilder sb = new StringBuilder();
            ImageColor[,] ic = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            //if(false)
            {
                for (int i = 1; i <= 6; i++)
                {
                    var rc = Btn_OK_Region_Dic[i];
                    var dst = P1_6_FriendOK_IC; 
                    bool isRight = rc.IsRegionRight(ic, dst, out delta);
                    mrfz_ScriptUnit.onMsg?.Invoke($"编号{i} 干员 是否为好友:{(isRight ? "是" : "否")} DLT={delta}");
                    //如果是好友干员
                    if (mrfz_ScriptConfig.scriptConfig.GetFriendRole_NeedFriendShip)
                    {
                        if (isRight)
                        {
                            foreach (var kv in IC_dic)
                            {

                                var dst_ic_shan = kv.Value;
                                var region_shan = P_dic[kv.Key][i];
                                isRight =
                                IsHeadImageRight(region_shan,ic, dst_ic_shan, out delta); 

                                mrfz_ScriptUnit.onMsg?.Invoke($"编号{i} 干员 是否是{mrfz_ScriptConfig.scriptConfig.mainChar}:{(isRight?"是":"否")} DLT={delta}");
                                if (isRight)
                                { 
                                    return i;
                                }
                            }
                        }
                    }else
                    {
                        foreach (var kv in IC_dic)
                        {

                            var dst_ic_shan = kv.Value;
                            var region_shan = P_dic[kv.Key][i];
                            isRight = 
                            IsHeadImageRight(region_shan,ic, dst_ic_shan, out delta);
                            mrfz_ScriptUnit.onMsg?.Invoke($"编号{i} 干员 是否是{mrfz_ScriptConfig.scriptConfig.mainChar}:{(isRight ? "是" : "否")} DLT={delta}");

                            if (isRight)
                            { 
                                return i;
                            }
                        }
                    }
                }
            }
            List<int> indexList = new List<int>();
            for (int i = 7; i <= 9; i++)
            {
                foreach (var kv in IC_dic)
                {

                    var dst_ic_shan = kv.Value;
                    var region_shan = P_dic[kv.Key][i];  

                    var isRight = IsHeadImageRight(region_shan, ic, dst_ic_shan, out delta);
                    mrfz_ScriptUnit.onMsg?.Invoke($"编号{i} 干员 是否是{mrfz_ScriptConfig.scriptConfig.mainChar}:{(isRight ? "是" : "否")} DLT={delta}");

                    if (isRight)
                        {
                            indexList.Add(i);
                        }
                    
                        
                }
            }
            if (indexList.Count > 0)
            {
                SwipeTo789(); 
                bmp = mrfzGamePage.CatptureImg();
                ic = ImageColor.FromBitmap(bmp);
                bmp.Dispose();
                foreach (var i in indexList)
                {
                    var rc = Btn_OK_Region_Dic[i];
                    var dst = P789_FriendOK_IC; 
                    var isRight = rc.IsRegionRight(ic, dst, out delta);
                    mrfz_ScriptUnit.onMsg?.Invoke($"编号{i} 干员 是否为好友:{(isRight ? "是" : "否")} DLT={delta}");
                    //如果是好友干员

                    if (mrfz_ScriptConfig.scriptConfig.GetFriendRole_NeedFriendShip)
                    {
                        if (isRight)
                        {

                            return i;
                        }
                    }
                    else
                    {
                        return i;
                    }
                         
                }
                SwipeTo1_6();
            }
            delta = -999;
            return -1;
        }
        protected void LoadSkinGamePageName(String SkinGamePageName,Boolean IsFirst)
        {
            {
                RectColor[] P = new RectColor[10];
                P[4] = mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].regions[0]; 
                P[1] = new RectColor(new Rectangle(P[4].rect.X - 452, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height),
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
                P_dic[SkinGamePageName] = P;

                //Bitmap shan1 = new Bitmap(Environment.CurrentDirectory + "\\imgs\\region_imgs" + P[4].FileName);
                IC_dic[SkinGamePageName] = 
                    ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\freindhead\\" + SkinGamePageName
                    +".png");
            }
            {
                if (IsFirst)
                {
                    RectColor[] P = new RectColor[10];
                    P[4] = mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].regions[2];
                    P[1] = new RectColor(new Rectangle(P[4].rect.X - 452, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height),
                       P[4].FileName,
                         "P1");

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

                    //   mrfzGamePage.GamePageDic["P789_freind_next"].regions[0];
                    P[9] = mrfzGamePage.GamePageDic["P789_freind_next"].regions[0];
                    P[7] = new RectColor(new Rectangle(P[9].rect.X, P[9].rect.Y - 199 - 198, P[9].rect.Width, P[9].rect.Height),
                          P[9].FileName,
                        "P7");
                    P[8] = new RectColor(new Rectangle(P[7].rect.X, P[7].rect.Y + 199, P[7].rect.Width, P[7].rect.Height),
                      P[9].FileName,
                     "P8");
                    for (int i = 1; i <= 9; i++)
                    {
                        Btn_OK_Region_Dic[i] = P[i];
                    }
                    P1_6_FriendOK_IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\region_imgs" + P[4].FileName);
                    P789_FriendOK_IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\region_imgs" + P[9].FileName);
                }
            }
        }
        public static Boolean IsCurPage()
        {
            return mrfzGamePage.GamePageDic["rogue-sel-chars-friend"].CheckPage(2000);
        }
        public static Boolean CanRefresh()
        {
            return mrfzGamePage.GamePageDic["rogue-sel-chars-friend"].CheckPage(2000);
        }
        public static void SwipeTo1_6()
        {
            //助战-山皮肤1-P4 SWIPE_ST x-500
            var cnp = mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].NextPages[0];
            if (cnp.ClickName != "SWIPE_ST") throw new Exception("err");
            var ed = cnp.ClickPoint;
            var st = new Point(cnp.ClickPoint.X - 500, cnp.ClickPoint.Y);
            adb.Swipe(st, ed, 500);
            System.Threading.Thread.Sleep(500);
        }
        public static void SwipeTo789()
        {
            //助战-山皮肤1-P4 SWIPE_ST x-500
            var cnp = mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].NextPages[0];
            if (cnp.ClickName != "SWIPE_ST") throw new Exception("err");
            var st = cnp.ClickPoint;
            var ed = new Point(cnp.ClickPoint.X - 500, cnp.ClickPoint.Y);
            adb.Swipe(st, ed, 500);
            System.Threading.Thread.Sleep(500);

        }
        #region BASE_MODEL
        protected static bool WaitToNotCurPage(String PageName, int TimeOut_ms)
        {
            var first = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - first).TotalMilliseconds > TimeOut_ms)
                {
                    return true;
                }
                if (mrfzGamePage.GamePageDic[PageName].IsCurPage())
                {
                    wait(300);
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageName"></param>
        /// <param name="TimeOut_ms"></param>
        /// <returns>是否超时 true=超时 false=不超时</returns>
        protected static bool WaitToCurPage(String PageName, int TimeOut_ms)
        {
            var first = DateTime.Now;
            while (true)
            {

                if (mrfzGamePage.GamePageDic[PageName].IsCurPage())
                {
                    return false;
                }
                else
                {
                    wait(300);
                    if ((DateTime.Now - first).TotalMilliseconds > TimeOut_ms)
                    {
                        return true;
                    }
                }
            }
        }
        protected static void PageSwipe(String PageName, String startPoint, String EndPoint, int ms = 500)
        {
            var ST = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, startPoint);
            var ED = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, EndPoint);
            if (ST == null || ED == null)
            {
                throw new Exception($" 找不到{PageName} {startPoint} 或者{EndPoint}点击数据");
                return;
            }
            adb.Swipe(ST.ClickPoint, ED.ClickPoint, ms);
        }
        public enum Dir
        {
            上,
            下,
            左,
            右
        }
        protected static void PageSwipe(String PageName, String EndPoint, Dir dir, int ms = 500)
        {
            var ED = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, EndPoint);
            if (ED == null)
            {
                throw new Exception($" 找不到{PageName}{EndPoint}点击数据");
            }
            Point FinalPoinst = new Point();
            int delta = 300;
            switch (dir)
            {
                case Dir.上:
                    FinalPoinst = new Point(ED.ClickPoint.X, ED.ClickPoint.Y - delta);
                    break;
                case Dir.下:
                    FinalPoinst = new Point(ED.ClickPoint.X, ED.ClickPoint.Y + delta);
                    break;
                case Dir.右:
                    FinalPoinst = new Point(ED.ClickPoint.X + delta, ED.ClickPoint.Y);
                    break;
                case Dir.左:
                    FinalPoinst = new Point(ED.ClickPoint.X - delta, ED.ClickPoint.Y);
                    break;
            }
            if (FinalPoinst.X < 0) FinalPoinst.X = 0;
            if (FinalPoinst.X >= ImgW) FinalPoinst.X = ImgW - 1;
            if (FinalPoinst.Y < 0) FinalPoinst.Y = 0;
            if (FinalPoinst.Y >= ImgH) FinalPoinst.Y = ImgH - 1;
            adb.Swipe(ED.ClickPoint, FinalPoinst, ms);
        }
        protected static int ImgW = 1280, ImgH = 720;
        protected static int TimeOut = 10 * 1000;

        protected static bool PageClick(String PageName, String ClickName, Boolean Valid = false)
        {
            if (Valid)
            {
                if (mrfzGamePage.GamePageDic[PageName].IsCurPage())
                {
                    var CNP = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, ClickName);
                    if (CNP == null) { { throw new Exception($" 找不到{PageName} {ClickName}点击数据"); } }
                    ClickPage(CNP);
                    wait(200);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var CNP = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, ClickName);
                if (CNP == null) { { throw new Exception($" 找不到{PageName} {ClickName}点击数据"); } }
                ClickPage(CNP);
                wait(200);
                // this.onMsg?.Invoke($"page:{PageName} clickname:{ClickName}");
                return true;
            }


        }
        protected static ClickToNextPage SearchClickName(List<ClickToNextPage> list, String Name)
        {
            foreach (var cnp in list)
            {
                if (cnp.ClickName == Name)
                {
                    return cnp;
                }
            }
            return null;
        }
        protected static void ClickPage(ClickToNextPage cnp)
        {
            adb.Tap(cnp.ClickPoint);
            wait(100);
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

        #endregion
    }
}
