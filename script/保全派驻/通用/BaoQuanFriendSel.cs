using GamePageScript.lib;
using GamePageScript.script.mrfz;
using lib.image;
using MRFZ_Auto.script.保全派驻.通用;
using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;
using static MRFZ_Auto.script.保全派驻.通用.TeamCharName;

namespace MRFZ_Auto.script.保全派驻.通用
{
    public class BaoQuanFriendSel
    {
        static BaoQuanFriendSel()
        {
            CHAR_NAME = new Dictionary<RecChar, ImageColor[,]>();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\friends");
            if (!di.Exists) di.Create();
            var fss= di.GetFiles("*.png");
            foreach(var fs in fss)
            {
                switch(fs.Name)
                {
                    case "OK.png":
                        BTN_OK = ImageColor.FromFile(fs.FullName);
                        break;
                    case "NO.png":
                        BTN_NO = ImageColor.FromFile(fs.FullName);
                        break;
                    case "OWN.png":
                        BTN_OWN = ImageColor.FromFile(fs.FullName);
                        break;
                    case "山.png":
                        CHAR_NAME[RecChar.山] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "泥岩.png":
                        CHAR_NAME[RecChar.泥岩] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "铃兰.png":
                        CHAR_NAME[RecChar.铃兰] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "橙闪.png":
                        CHAR_NAME[RecChar.橙闪] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "拉普兰德.png":
                        CHAR_NAME[RecChar.拉普兰德] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "归溟幽灵鲨.png":
                        CHAR_NAME[RecChar.归溟幽灵鲨] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "风笛.png":
                        CHAR_NAME[RecChar.风笛] = ImageColor.FromFile(fs.FullName);
                        break;
                    case "推进之王.png":
                        CHAR_NAME[RecChar.推进之王] = ImageColor.FromFile(fs.FullName);
                        break;

                }
            }
        }
        /// <summary>
        /// name
        /// </summary>
        protected  static Dictionary<RecChar, ImageColor[,]> CHAR_NAME;
        /// <summary>
        /// ok
        /// </summary>
        protected static ImageColor[,] BTN_OK;
        /// <summary>
        /// ok
        /// </summary>
        protected static ImageColor[,] BTN_NO;
        /// <summary>
        /// BTN_OWN
        /// </summary>
        protected static ImageColor[,] BTN_OWN;
        public  static int what_number_inpage(ImageColor[,] IC, out double delta)
        { 
            StringBuilder sb = new StringBuilder(); 
            for (int i = 0; i < 8; i++)
            {
                 
                {
                    var X = 128 + i * 139;
                    var Y = 635;
                    var W = 30;
                    var H = 20;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    double dlt = 0;
                    var isOK = (dlt= ImageColor.CalcDeltaOfTwoImg(IC, BTN_OK, rect)) < mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get;
                    var isNO = ImageColor.CalcDeltaOfTwoImg(IC, BTN_NO, rect) < mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get;
                    var isOWN = ImageColor.CalcDeltaOfTwoImg(IC, BTN_OWN, rect) < mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get;
                    IS_UI_DOWN = false;
                    if(isOWN)
                    {
                        continue;
                    }
                     
                    IS_UI_DOWN = false;
                    mrfz_ScriptUnit.onMsg?.Invoke($"编号{i+1} 干员 是否为好友:{(isOK ? "是" : "否")} DLT={dlt}");
                    if ((mrfz_ScriptConfig.scriptConfig.NeedFriendShipsInBaoQuan && isOK)|| !mrfz_ScriptConfig.scriptConfig.NeedFriendShipsInBaoQuan) 
                    {
                        {
                              X = 143 + i * 139;
                              W = 99;
                              Y = 581;
                            H = 19;
                              rect = new Rectangle(new Point(X, Y), new Size(W, H));
                            var dstic = CHAR_NAME[mrfz_ScriptConfig.scriptConfig.FriendCharInBaoQuan];
                            Color fontColor = Color.FromArgb(56, 56, 56);
                            var isCurChar = (delta = ImageColor.CalcDeltaOfTwoImgInFont(IC, dstic, fontColor, rect,8)) < mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get;
                            mrfz_ScriptUnit.onMsg?.Invoke($"编号{i+1} 干员 是否是{mrfz_ScriptConfig.scriptConfig.mainChar}:{(isCurChar ? "是" : "否")} DLT={delta}");
                            if (isCurChar)
                            {
                                return i + 1;
                            }
                        }
                    }
                }
                

            }

            delta = -1;
            return -1;
        }
      //  TeamCharName
        public static void GetChar(out double delta)
        {
            if (!IsCurPage())
            {
                throw new Exception("Time out 判断助战页面");
            }
            var bmp = mrfzGamePage.CatptureImg();
            var IC = ImageColor.FromBitmap(bmp);
            bmp.Dispose(); 
            var index = what_number_inpage(IC, out delta);
            while (true)
            {
                if (index <= 0)
                {
                    while (!CanRefresh())
                    {
                        wait(500);
                        break;
                    }
                    PageClick("baoquan_friends", "refresh", false);
                    WaitToNotCurPage("baoquan_friends", 2000);
                    bmp.Dispose();
                    bmp = mrfzGamePage.CatptureImg();
                    IC = ImageColor.FromBitmap(bmp);
                    bmp.Dispose();
                    index = what_number_inpage(IC, out delta);
                    
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
                       // index = -1;
                       //  continue;
                    }*/
                    
                    //如果是好友干员 
                   
                   
                    {
                        PageClick("baoquan_friends", $"P{index}", false);
                        waitSec(1);
                    }
                    

                }
            }
            
        }
        public static Boolean IS_UI_DOWN = false;
        
        public static Boolean IsCurPage()
        {
            return mrfzGamePage.GamePageDic["baoquan_friends"].CheckPage(2000);
        }
        public static Boolean CanRefresh()
        {
            return mrfzGamePage.GamePageDic["baoquan_friends"].CheckPage(2000);
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
