using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 
using GamePageScript.lib;
using lib;
using lib.image;
using MRFZ_Auto;
using MRFZ_Auto.script.mrfz;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script.mrfz.shop;
using script;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;
using static MRFZ_Auto.script.CharPoint;
using static MRFZ_Auto.script.mrfz.battle.BattleCharRec;

namespace GamePageScript.script.mrfz
{
    public partial class mrfz_ScriptUnit : ScriptUnit
    {
        public String RogueName = "愧影";
        new int W = 1280, H = 720;
        public TaskType taskType;
        public Boolean RunError = false;
        public mrfz_ScriptMachine scriptMachine;
        protected new static Dictionary<string, mrfzGamePage> GamePageDic;
        /// <summary>
        /// 超时 标准化 10秒普通页面， 战斗30秒
        /// </summary>
        public static int TimeOut_NORMAL = 10 * 1000;
        public static int TimeOut_BATTLE = 60 * 1000;
        public static int SH = 10 * 1000;
        public static int SW = 40 * 1000;
        
        public enum TaskType
        {
            Rogue_KuiYing,
            AntiUpdate,
            HomeToRogue,
            /// <summary>
            /// 普通关卡挂机
            /// </summary>
            NormalStart
        }
        static mrfz_ScriptUnit()
        {

            GamePageDic = mrfzGamePage.GamePageDic;
            SH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height; //1080
            SW = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width; //1920
            if (Environment.CurrentDirectory.EndsWith("Debug"))
            {
                Debug = true;
            }
        }
        public mrfz_ScriptUnit(TaskType taskType)
        {
            //  GameWinName = "雷电模拟器"; 
            this.taskType = taskType;
            if(taskType== TaskType.NormalStart)
            {

            }else
            {

            ClcBattleMapNode();
            }
        }
      /*  public new static String GameWinName = "雷电模拟器";
        public IntPtr GetHwndFromName()
        {
            if (GameWinName.StartsWith("雷电模拟器"))
            {
                GPR = Win.FindWindow(GameWinName, "LDPlayerMainFrame");
            }
            else if (GameWinName == ("明日方舟 - MuMu模拟器"))
            {
                GPR = Win.FindWindow(GameWinName, "Qt5QWindowIcon");
            }
            return GPR;
            //MuMu模拟器
        }
      */
        public override void RunTest()
        {
            /* MRFZCheckState CS = null; 
             onMsg?.Invoke("设置游戏窗口大小");
             //
              CheckGameWindow();

             //DealNode(1);
             scriptMachine.ScriptRunning = false;
             //BEGIN
             //DO TEST
             */
        }

        public override void Run()
        {
             
            switch (taskType)
            {
                case TaskType.Rogue_KuiYing:
                    Rogue_KuiKing();
                    break;
                case TaskType.AntiUpdate:
                    var ar=AntiUpdate();
                    if(ar.IsUpdate&&ar.SuccessToHome)
                    {
                        scriptMachine.NextTaskType = TaskType.HomeToRogue;
                    }else
                    {
                        StopScript("不能重新回到HOME 主界面");
                    }
                    break;
                case TaskType.HomeToRogue:
                    HomeToRogue(); 
                    scriptMachine.NextTaskType = TaskType.Rogue_KuiYing;
                    break;
                case TaskType.NormalStart:
                    RunNormal();
                    scriptMachine.NextTaskType = TaskType.NormalStart;
                    break;
            }
            onMsg("一轮脚本完成");
            scriptMachine.ScriptRunning = false;
        }
        Boolean HasEditTeam = false;
        Boolean hasSong = false;
        Boolean hasSURPPORT = false;
        protected GameItem SelectWinItem(List<GameItem> list,out int index)
        {
            index = 0;
            if (list == null || list.Count == 0) return null; 
            var policy=mrfz_ScriptConfig.scriptConfig.policy;
            Boolean hasExit= list.Count(x => x.itemType == ItemType.EXIT)>0;
            for (; index<list?.Count;index++)
            {
                switch(list[index].itemType)
                {
                    case ItemType.GOLD:
                        if (policy.GetGold||!hasExit)
                            return list[index];
                        break;
                    case ItemType.ITEM:
                        if (policy.GetItem||!hasExit)
                            return list[index];
                        break;
                    case ItemType.SURPPORT:
                        if(!hasSURPPORT)
                        {
                            if (policy.GetSupport || !hasExit)
                            {

                                hasSURPPORT = true;
                                return list[index];
                            }
                        } 
                        break;
                    case ItemType.SONG:
                        if (!hasSong)
                        {
                            if (policy.GetSong || !hasExit)
                            {

                                hasSong = true;
                                return list[index];
                            }
                        }  
                        break;
                    case ItemType.PAPER:
                        if (policy.GetPaper||!hasExit)
                            return list[index];
                        break;
                    case ItemType.EXIT:
                        return list[index];

                } 
            }
            return list[index];
        }

        protected shop_item SelectShopItem(List<shop_item> list)
        { 
            if (list == null||list.Count==0) return null;
            if (list.Count == 1) return list[0];
            if(Program.Debug)
            {

                list = list.OrderByDescending(x => x.PriceRatio).ToList();
                return list[0];
                var list_paper2 = list.Where(x => (x.Item_Type == shop_item.ItemType.paper)).ToList();
                if (list_paper2.Count > 0)
                {
                    list_paper2 = list_paper2.OrderBy((x => x.Price)).ToList();
                    return list_paper2[0];
                } 
            }

            list = list.OrderByDescending(x => x.PriceRatio).ToList();
            return list[0];
            //...
            var list_item=list.Where(x => (x.Item_Type == shop_item.ItemType.Item)).ToList();
            if(list_item.Count>0)
            {
                list_item = list_item.OrderBy((x => x.Price)).ToList();
                if(list_item[0].Price>8)
                {
                    var list_sp2 = list.Where(x => (x.Item_Type == shop_item.ItemType.supportItem)).ToList();
                    if (list_sp2.Count > 0)
                    {
                        return list_sp2[0];
                        //list_sp = list_sp.OrderBy((x => x.Price)).ToList();
                        // return list_sp[0];
                    }else
                    {
                        return list_item[0];
                    }
                }
                else
                { 
                    return list_item[0];
                } 
            }
            var list_sp = list.Where(x => (x.Item_Type == shop_item.ItemType.supportItem)).ToList();
            if (list_sp.Count > 0)
            {
                return list_sp[0];
                //list_sp = list_sp.OrderBy((x => x.Price)).ToList();
               // return list_sp[0];
            }

            var list_paper= list.Where(x => (x.Item_Type == shop_item.ItemType.paper)).ToList();
            if (list_paper.Count > 0)
            {
                list_paper = list_paper.OrderBy((x => x.Price)).ToList();
                return list_paper[0];
            }
            return list_item[0];
        }
        protected BranchNode SelectNode(List<BranchNode> list)
        {
            if (list == null) return null;
            if (list.Count == 1) return list[0];
            foreach(var v in list)
            {
                if(v.NT== NodeType.诡异行商)
                {
                    return v;
                }
            }
            foreach (var v in list)
            {
                if (v.NT == NodeType.安全的角落)
                {
                    return v;
                }
            }
            var 紧急作战_list = list.Where(x => x.NT == NodeType.紧急作战).ToList();
            var 作战_list = list.Where(x => x.NT == NodeType.作战).ToList();
            var 不期而遇_list = list.Where(x => x.NT == NodeType.不期而遇).ToList();
            var 幕间余兴_list = list.Where(x => x.NT == NodeType.幕间余兴).ToList();
            switch(mrfz_ScriptConfig.scriptConfig.policy.P1)
            {
                case Priority.非战斗:
                    if (不期而遇_list.Count > 0) return 不期而遇_list[0];
                    if (幕间余兴_list.Count > 0) return 幕间余兴_list[0];
                    break;
                case Priority.普通作战:
                    if (作战_list.Count > 0) return 作战_list[0];
                    break;
                case Priority.紧急作战:
                    if (紧急作战_list.Count > 0) return 紧急作战_list[0];
                    break;
            }
            switch (mrfz_ScriptConfig.scriptConfig.policy.P2)
            {
                case Priority.非战斗:
                    if (不期而遇_list.Count > 0) return 不期而遇_list[0];
                    if (幕间余兴_list.Count > 0) return 幕间余兴_list[0];
                    break;
                case Priority.普通作战:
                    if (作战_list.Count > 0) return 作战_list[0];
                    break;
                case Priority.紧急作战:
                    if (紧急作战_list.Count > 0) return 紧急作战_list[0];
                    break;
            }
            switch (mrfz_ScriptConfig.scriptConfig.policy.P3)
            {
                case Priority.非战斗:
                    if (不期而遇_list.Count > 0) return 不期而遇_list[0];
                    if (幕间余兴_list.Count > 0) return 幕间余兴_list[0];
                    break;
                case Priority.普通作战:
                    if (作战_list.Count > 0) return 作战_list[0];
                    break;
                case Priority.紧急作战:
                    if (紧急作战_list.Count > 0) return 紧急作战_list[0];
                    break;
            }
            return list[0];
            
              
        }
        public class ScriptException:Exception
        {
            public Boolean StopThisUnit;
            public Boolean StopMachine;
            public Boolean IsUpdate;
            public ScriptException(Boolean StopThisUnit,  Boolean StopMachine,string msg):base(msg)
            {
                this.StopMachine = StopMachine;
                this.StopThisUnit = StopThisUnit;
            }
        }
        protected void Rogue_KuiKing()
        {
            try
            {
                HasEditTeam = false;
                 onMsg?.Invoke($"肉鸽-愧影 任务即将开始");

             
                var bmp = mrfzGamePage.CatptureImg();
                if (bmp == null || bmp.Width != W || bmp.Height != H)
                {
                    bmp?.Dispose();
                    throw new ScriptException(true,true,$"获取图像错误,可能是游戏窗口被最小化了或者手动改变过窗口,分辨率/DPI,错误或者adb没有连接模拟器 " +
                    $"(核定配置:分辨率1280*720 DPI=240 雷电模拟器 安卓7.1.2 平板型)");
                }
                else
                {
                 //   bmp.Dispose();
                    
                }

                bmp?.Dispose();

                /*if(GamePageDic["home"].IsCurPage())
                {
                    HomeToRogue();
                }
                */
                GiveUpCurRogue();  
                //阶段1-难度选择-开始
                CheckHomeToNormalToStart();
                
                //ANTI UPDATE
                //阶段2-选择队伍 干员--进入第一层
                SelTeamToGoL1(); 
                int NodeIndex = 1;
                int L = 1;
                int retryC = 30;
                while (true)
                {
                    if(retryC<0)
                    {
                        { StopScript("不能回到节点抉择界面,30次重试超时,请联系作者"); return; } 
                    }
                    var list = BranchNode.GetCurBranchNodes(NodeIndex, L, mrfzGamePage.CatptureImg());
                    if (NodeIndex == 1)
                    {
                        if (list == null || list.Count == 0)
                        {
                            waitSec(1);
                            retryC--;
                            continue;
                        }
                    } 

                    if (list == null)
                    {
                        waitSec(1);
                        retryC--;
                        continue;
                    }
                    retryC = 30;
                    var BN = SelectNode(list);

                    if (BN.NT == NodeType.Unkown) { StopScript("未识别的节点类型"); return; }
                    var result = DealNode(NodeIndex, L, BN);

                    if (result.Endding) break;
                    NodeIndex++;
                    if (L == 1 && BN.NT == NodeType.诡异行商)
                    {
                        L++; NodeIndex = 1;
                    }
                    else if (L == 2 && BN.NT == NodeType.古堡馈赠)
                    {
                        L++; NodeIndex = 1;
                    }
                    else if (L == 3 && BN.NT == NodeType.险路恶敌)
                    {
                        L++; NodeIndex = 1;
                    }
                    else if (L == 4 && BN.NT == NodeType.古堡馈赠 && NodeIndex > 4)
                    {
                        L++; NodeIndex = 1;
                    }
                    else if (L == 5 && BN.NT == NodeType.险路恶敌)
                    {
                        L++; NodeIndex = 1;
                    }
                     
                }
                onMsg?.Invoke("完成一轮");
            }catch(ScriptException excpetion)
            {
                if(excpetion.StopMachine)
                {
                    StopScript(excpetion.Message);
                }
                else
                {
                     if(excpetion.IsUpdate)
                    {
                        scriptMachine.NextTaskType = TaskType.AntiUpdate;
                        return;
                    }
                }
            }catch(Exception ex)
            {
                StopScript(ex.Message);
            }
            finally
            {

            }
        }
        //#0
        public void GiveUpCurRogue()
        {
            Bitmap bitmap = mrfzGamePage.CatptureImg();
            if(GamePageDic["rogue-givup"].IsCurPage(bitmap))
            {
                PageToClick("rogue-givup", "NEXT", TimeOut_NORMAL);
                WaitToNotCurPage("rogue-givup", 2000);
                wait(500);
                if (GamePageDic["giveupchenck"].IsCurPage())
                {
                    PageToClick("giveupchenck", "NEXT", TimeOut_NORMAL);
                    WaitToNotCurPage("giveupchenck", 2000);

                    DealGameOver();
                }
            }else
            if(GamePageDic["rogue-home-end-giveup"].IsCurPage(bitmap))
            {
                PageToClick("rogue-home-end-giveup", "NEXT", TimeOut_NORMAL);
                WaitToNotCurPage("rogue-home-end-giveup", 2000);
                DealGameOver();
            }
        }
        //#1 
        public void CheckHomeToNormalToStart()
        {
            var PN = mrfzGamePage.WhatPage(new string[] { "rogue-kuiying-home-sel",
                        "rogue-kuiying-home-normal","rogue-kuiying-home-nosel"}, TimeOut_NORMAL);
            if (PN == null)
            {
                mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                StopScript("开始界面错误,请切换到肉鸽-愧影,难度选择-正式调查 界面");
                return;
            }
            if (PN == "rogue-kuiying-home-nosel")
            {
                PageClick("rogue-kuiying-home-nosel", "NEXT", false);
               var iselpage=  GamePageDic["rogue-kuiying-home-sel"].CheckPage(3000);
                if(!iselpage)
                {
                    mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                    StopScript("开始界面错误,请切换到肉鸽-愧影,难度选择-正式调查 界面");
                }

                PageClick("rogue-kuiying-home-sel", "NORMAL", false);
                WaitToNotCurPage("rogue-kuiying-home-sel", TimeOut_NORMAL);
                //NORMAL 
                var flag = GamePageDic["rogue-kuiying-home-normal"].CheckPage(TimeOut_NORMAL);
                if (flag)
                {
                    while (GamePageDic["rogue-home-wait"].IsCurPage())
                    {
                        wait(500);
                    }
                    PageClick("rogue-kuiying-home-normal", "NEXT", false);
                    WaitToNotCurPage("rogue-kuiying-home-normal", TimeOut_NORMAL);
                    //NORMAL
                }
                else
                {
                    mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                    StopScript("开始界面错误,请切换到肉鸽-愧影,难度选择界面[可能识别错误]");
                    return;
                }
            }


           if (PN == "rogue-kuiying-home-sel")
            {
                PageClick("rogue-kuiying-home-sel", "NORMAL", false);
                WaitToNotCurPage("rogue-kuiying-home-sel", TimeOut_NORMAL);
                //NORMAL 
                var flag = GamePageDic["rogue-kuiying-home-normal"].CheckPage(TimeOut_NORMAL);
                if (flag)
                {
                    while(GamePageDic["rogue-home-wait"].IsCurPage())
                    {
                        wait(500);
                    }
                    PageClick("rogue-kuiying-home-normal", "NEXT", false);
                    WaitToNotCurPage("rogue-kuiying-home-normal", TimeOut_NORMAL);
                    //NORMAL
                }
                else
                {
                    mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                    StopScript("开始界面错误,请切换到肉鸽-愧影,难度选择界面[可能识别错误]");
                    return;
                }
            }
            else if (PN == "rogue-kuiying-home-normal")
            {
                while (GamePageDic["rogue-home-wait"].IsCurPage())
                {
                    wait(500);
                }
                PageClick("rogue-kuiying-home-normal", "NEXT", false);
                WaitToNotCurPage("rogue-kuiying-home-normal", TimeOut_NORMAL);
            }

        }
        //#2 
        public void SelTeamToGoL1()
        {

            //rogue-sel-team  
            var flag = WaitToCurPage("rogue-sel-team", TimeOut_NORMAL);
            if (flag)
            {
                StopScript("选择队伍 识别错误 CODE=1");
                return;
            }
            Point nxtpoint;
            //选队伍 需要+ 第6
            if(mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc==6)
            {
                adb.Swipe(new Point(700,540),new Point(500, 540),600);
              //  PageSwipe("rogue-sel-jinwei", "SWIPE6_ST", "SWIPE6_ED");
            }
            PageClick("rogue-sel-jinwei", $"T{mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc}");
          //  flag = TeamLogo.FindTeam(mrfz_ScriptConfig.scriptConfig.Team_Type, out nxtpoint, null, true);
          //  if(!flag)
            {
                PageClick("rogue-sel-jinwei", $"T{mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc}");
            //    flag = TeamLogo.FindTeam(mrfz_ScriptConfig.scriptConfig.Team_Type, out nxtpoint, null, true);
            //    PageClick("", $"P{mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc}");
            }
            /*if (flag)
            {
                adb.Tap(nxtpoint);
                flag = GamePageDic["rogue-sel-jinwei"].CheckPage(TimeOut_NORMAL);
                if(!flag)
                {
                    StopScript("选择队伍 识别错误 CODE=2");
                    return;
                }
                adb.Tap(nxtpoint);
            }else
            {
                StopScript("  识别分队错误 CODE=110");
                return;
            }*/
            flag = GamePageDic["rogue-sel-chars-papertype"].CheckPage(TimeOut_NORMAL);
            if (flag)
            {
                PageClick("rogue-sel-chars-papertype", "NEXT", false);
                flag = GamePageDic["rogue-sel-chars-papertype-jinwei"].CheckPage(TimeOut_NORMAL);
                if (flag)
                {

                    PageClick("rogue-sel-chars-papertype-jinwei", "NEXT", false);
                    if(mrfz_ScriptConfig.scriptConfig.mainChar== ArkChar.令)
                    {

                        //LING
                        //...
                        SelMainChar(2);
                        SelHealChar(3);
                        GiveUpAddRole(1);
                        flag = GamePageDic["rogue-ready-start"].CheckPage(TimeOut_NORMAL);
                        if (flag)
                        {

                            PageClick("rogue-ready-start", "NEXT", false);
                            waitSec(5);
                            return;
                        }
                        else
                        {
                            StopScript("队伍准备 开始  超时  CODE=10");
                            return;
                        }
                    }
                    else
                    { 
                        //JINWEI
                        SelMainChar(1);
                        GiveUpAddRole(2);
                        SelHealChar(3);
                        flag = GamePageDic["rogue-ready-start"].CheckPage(TimeOut_NORMAL);
                        if (flag)
                        {

                            PageClick("rogue-ready-start", "NEXT", false);
                            waitSec(5);
                            return;
                        }
                        else
                        {
                            StopScript("队伍准备 开始  超时  CODE=10");
                            return;
                        }
                    } 
                }
                else
                {

                    StopScript("选择招募类别 识别错误  CODE=4");
                    return;
                }
            }
            else
            {
                StopScript("选择招募类别 识别错误  CODE=3");
                return;
            }

          
        }

        /// <summary>
        /// 选择主要角色,第几个角色
        /// </summary>
        /// <param name="PIndex"></param>
        public void SelMainChar(int PIndex)
        {
            var flag = GamePageDic["rogue-sel-chars"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript("MainChar招募选择 超时 CODE=8");
                return;
            }
            PageClick("rogue-sel-chars", $"P{PIndex}", false);
           var PN= mrfzGamePage.WhatPage(new string[] { "rogue-sel-chars-jinwei" ,"rogue-help0"}, TimeOut_NORMAL);
            if(PN== "rogue-help0")
            {
                PageClick("rogue-help0", $"NEXT", false);
            }
            flag = GamePageDic["rogue-sel-chars-jinwei"].CheckPage(TimeOut_NORMAL);
            if (flag)
            {
              // ImageSave.Save(ImageSave.SaveType.SaveHeadImgInCharSelPage); 
                if (mrfz_ScriptConfig.scriptConfig.UseFriendRole)
                {
                    //friend
                  //  friendChar = mrfz_ScriptConfig.MainChar.山;
                    PageClick("rogue-sel-chars-jinwei", $"friend", false);
                    WaitToCurPage("rogue-sel-chars-friend", 5000);
                    try
                    {
                        double freind_dlt = -9990d;
                        FriendSel.GetChar(out freind_dlt);
                      //  friend_char.FriendChars[mrfz_ScriptConfig.scriptConfig.mainChar].GetChar(out freind_dlt);
                        onMsg?.Invoke($"助战图像判断因子:{freind_dlt}");
                    }
                    catch (Exception ex)
                    {
                        StopScript(ex.Message);
                        return;
                    }
                }
                else
                {

                    PageClick("rogue-sel-chars-jinwei", $"P{mrfz_ScriptConfig.scriptConfig.SelectChar_Loc}", false);
                    if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.山)
                    {
                        flag = GamePageDic["rogue-sel-chars-shan"].CheckPage(TimeOut_NORMAL);
                    }
                    else if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.煌)
                    {
                        flag = GamePageDic["rogue-sel-chars-huang"].CheckPage(TimeOut_NORMAL);
                    }
                    else if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.棘刺)
                    {
                        flag = GamePageDic["rogue-sel-chars-jici"].CheckPage(TimeOut_NORMAL);
                    }
                    else if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.帕拉斯)
                    {
                        flag = GamePageDic["rogue-sel-chars-palasi"].CheckPage(TimeOut_NORMAL);
                    }else if(mrfz_ScriptConfig.scriptConfig.mainChar== ArkChar.令)
                    {
                        flag = GamePageDic["rogue-sel-chars-ling"].CheckPage(TimeOut_NORMAL);
                   
                    }
                    if (flag)
                    {
                        PageClick("rogue-sel-chars-shan", "NEXT", false);
                    }
                    else
                    {
                        StopScript($"主要干员 识别错误 {mrfz_ScriptConfig.scriptConfig.mainChar}---  招募时的位置要正确  CODE=7");
                        return;
                    }
                }

                //if(flag)
                {
                    flag = GamePageDic["get-char"].CheckPage(TimeOut_NORMAL);
                    if (!flag)
                    {
                        StopScript("获取角色界面错误 code=11");
                        return;
                    }
                    PageClick("get-char", "NEXT", false);
                    WaitToNotCurPage("get-char", TimeOut_NORMAL);
                    PageClick("get-char", "NEXT", false);

                    //get-char
                    flag = GamePageDic["rogue-sel-chars"].CheckPage(TimeOut_NORMAL);


                }

                //rogue-sel-chars-shan
            }
            else
            {

                StopScript("选择招募类别 识别错误 CODE=6");
                return;
            }
        }

        public void GiveUpAddRole(int PIndex)
        {
            var flag = GamePageDic["rogue-sel-chars"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript("返回招募选择 超时 CODE=8");
                return;
            }
            PageClick("rogue-sel-chars", $"P{PIndex}", false);
            flag = GamePageDic["rogue-add-role-list"].CheckPage(TimeOut_NORMAL);
            if (flag)
            {

                PageClick("rogue-add-role-list", "GIVEUP", false);
                flag = GamePageDic["add-role-giveup"].CheckPage(TimeOut_NORMAL);
                if (flag)
                {

                    PageClick("add-role-giveup", "NEXT", false);

                }
            }
            else
            {

                StopScript("选择角色 界面错误");
            }
        }

        public void SelHealChar(int PIndex)
        {
            var flag = GamePageDic["rogue-sel-chars"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript("SelHealChar 返回招募选择 超时 CODE=8");
                return;
            } 
            PageClick("rogue-sel-chars", $"P{PIndex}", false);
            //P3- 选择医疗干员 
            {
                var PN = mrfzGamePage.WhatPage(new string[] { "rogue-sel-chars-jinwei", "rogue-help0" }, TimeOut_NORMAL);
                if (PN == "rogue-help0")
                {
                    PageClick("rogue-help0", $"NEXT", false);
                }
              //  ImageSave.Save(ImageSave.SaveType.SaveHeadImgInCharSelPage);
                PageToClick("rogue-sel-chars-jinwei",
                    $"P{mrfz_ScriptConfig.scriptConfig.SelectHEAL_Loc}", TimeOut_NORMAL);
                waitSec(1);
                PageClick("rogue-sel-chars-shan", "NEXT", false);
                flag = GamePageDic["get-char"].CheckPage(TimeOut_NORMAL);
                if (!flag)
                {
                    StopScript("获取角色界面错误 code=11");
                    return;
                }
                PageClick("get-char", "NEXT", false);
                WaitToNotCurPage("get-char", TimeOut_NORMAL);
                PageClick("get-char", "NEXT", false);

            }
             

        }

        public class AntiUpdateResult
        {
            public Boolean IsUpdate;
            public Boolean SuccessToHome;
            public Boolean TimeOut;
        }
        public AntiUpdateResult AntiUpdate()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                AntiUpdateResult ar = new AntiUpdateResult(); 
                List<String> updateList = new List<string>();
                int i = 0;
                while (true)
                {
                    if (GamePageDic.ContainsKey($"update_{i}"))
                    {
                        if (!GamePageDic[$"update_{i}"].Baned)
                        {
                            updateList.Add($"update_{i}");
                            sb.AppendLine($"更新页面加入:update_{i}");
                        }
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }
                var dstTime = DateTime.Now.AddSeconds(5);
                while (true)
                {
                    var bmp = mrfzGamePage.CatptureImg();
                    foreach (var p in updateList)
                    {
                        if (GamePageDic[p].IsCurPage(bmp))
                        {
                            ar.IsUpdate = true;
                            sb.AppendLine($"已经检测到更新页面");
                            break;
                        }
                    }
                    if (ar.IsUpdate)
                    {
                        break;
                    }
                    if (DateTime.Now > dstTime)
                    {
                        //..
                        ar.TimeOut = true;
                        return ar;
                    }
                    else
                    {

                    }
                    bmp.Dispose();
                    System.Threading.Thread.Sleep(500);
                }
                updateList.Add("home");
                dstTime = DateTime.Now.AddSeconds(80); 
                while (true)
                {
                    List<String> list = new List<string>();
                   var PN= mrfzGamePage.WhatPageOnThread(updateList.ToArray(),6000);
                    if (PN == "home")
                    {
                        sb.AppendLine($"已经检测到home页面-主界面");
                        System.Threading.Thread.Sleep(2000);
                        if (GamePageDic["home"].IsCurPage())
                        {
                            ar.SuccessToHome = true;
                            return ar;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (PN !=null)
                    {
                        sb.AppendLine($"已经检测到更新{PN}页面-next");
                        waitSec(1);
                        PageClick(PN, "NEXT", false);
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        sb.AppendLine($"非更新页面/home页面-next");
                        System.Threading.Thread.Sleep(500);
                    } 
                    if (DateTime.Now > dstTime)
                    {
                        //..
                        ar.TimeOut = true;
                        return ar;
                    }
                }
            }
            catch (Exception ex)
            { 
                    sb.AppendLine($"异常终止:{ex.Message} \r\n{ex.ToString()}");
                    StopScript(ex.Message);
                return new AntiUpdateResult()
                {
                    SuccessToHome = false
                };
            }
            finally
            {
                try
                {
                    var bmp = mrfzGamePage.CatptureImg();
                    bmp.Save(Environment.CurrentDirectory + "\\凌晨4点画面.png");
                    bmp.Dispose();
                    onMsg?.Invoke("程序文件夹下 凌晨4点画面.png已保存,如果不能过4点更新继续挂机,请把该图片发给作者");
                    sb.AppendLine($"程序文件夹下 凌晨4点画面.png已保存");
                    FileStream fs = new FileStream(Environment.CurrentDirectory + "\\凌晨4点.txt",FileMode.Create,FileAccess.Write);
                    var bs = Encoding.UTF8.GetBytes(sb.ToString());
                    fs.Write(bs,0,bs.Length);
                    fs.Close(); 
                }
                catch
                {

                }
            }
        }
        public void HomeToRogue()
        {
            PageToClick("home", "NEXT", TimeOut_NORMAL);
            PageToClick("ui-select-battle", "NEXT-ROGUE", TimeOut_NORMAL);
            PageToClick("ui-rogue-home", "NEXT_KUIYING", TimeOut_NORMAL);
            bool TO=WaitToCurPage("rogue-kuiying-home", TimeOut_NORMAL);
            if(TO)
            { 
                return;
            }
        }
        protected static bool Debug = false;
        protected mrfz_rogue_battlemap BattleMap;
        //战斗结束后退回节点选择界面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BattleMap"></param>
        /// <returns>战斗是否成功，失败则保证回到开始界面</returns>
        private Boolean BattleMapExcute(mrfz_rogue_battlemap BattleMap)
        {
            //DATE LOAD 
           
            this.BattleMap = BattleMap;
            //选人
            PageClick(BattleMap.RecPage.Name, "NEXT", false);
            onMsg?.Invoke("战斗地图:" + BattleMap.RecPage.Name);
            onMsg?.Invoke("紧急:" + BattleMap.RecPage.紧急);
            var flag = GamePageDic["rougue-team-edit"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript("进入队伍编辑 cuowu  rougue-team-edit");
                return false;
            }
            //编辑地图-队伍
            if (!this.HasEditTeam)
            {

                PageClick("rougue-team-edit", "EDIT", false);
                flag = GamePageDic["rougue-team-char-sel"].CheckPage(TimeOut_NORMAL);
                if (!flag)
                {
                    StopScript("进入队伍编辑 选人错误 rougue-team-char-sel");
                    return false;
                }

                PageClick("rougue-team-char-sel", "P1", false);
                wait(500);
                //  if (mrfz_ScriptConfig.scriptConfig.mainChar != mrfz_ScriptConfig.MainChar.山
                // ||mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc!=5)
                {
                    PageClick("rougue-team-char-sel", "P2", false);
                    wait(500);
                }

                PageClick("rougue-team-char-sel", "NEXT", false);
                flag = GamePageDic["rogue-team-edit-has-sel"].CheckPage(TimeOut_NORMAL);
                if (!flag)
                {
                    StopScript("不能返回队伍编辑界面  rogue-team-edit-has-sel");
                    return false;
                }
                if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.棘刺)
                {

                    PageClick("rogue-team-edit-has-sel", "SKILL3", false);
                }
                else if (mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.帕拉斯)
                {

                    //   PageClick("rogue-team-edit-has-sel", "SKILL1", false);
                }
                else
                {
                    PageClick("rogue-team-edit-has-sel", "SKILL2", false);
                }
                wait(500);
                PageClick("rogue-team-edit-has-sel", "NEXT", false);

            }
            else
            {
                PageClick("rogue-team-edit-has-sel", "NEXT", false);
            }

            onMsg?.Invoke("队伍编辑完毕");
            HasEditTeam = true;
            WaitToNotCurPage("rougue-team-edit", 200);
            //战斗
            flag = BattleMap.Battle_First.CheckPage(TimeOut_BATTLE);
            if (!flag)
            {
                StopScript("进入战斗超时 ----CODE =20");
                return false;
            }
            onMsg?.Invoke("进入战斗");
            waitSec(1);
            PageClick(BattleMap.Battle_First.Name, "2X", false);
            onMsg?.Invoke("2X");
           
            //观察
            /*   if (Debug&&GamePageDic[BattleMap.Battle_First.Name].regions.Count>0)
                { 
                    Bitmap fmap = mrfzGamePage.CatptureImg();
                   var srcIC= ImageColor.FromBitmap(fmap);
                    fmap.Dispose();
                   var R= GamePageDic[BattleMap.Battle_First.Name].GetRegions();
                    ImageColor[,] dst;
                    double dlt = -99;
                    if (BattleMap.MapName=="驯兽小屋")
                    {
                        dst = R["right_region"].GetRegionIC(); 
                        R["right_region"].IsRegionRight(srcIC, dst, out dlt);
                        onMsg?.Invoke("驯兽小屋 右侧 差异值:" + dlt);
                    }
                    int PW_I = 1;
                    for (; ; PW_I++)
                    {
                        if (R.ContainsKey($"PW{PW_I}"))
                        {
                            dst = R[$"PW{PW_I}"].GetRegionIC();
                            R[$"PW{PW_I}"].IsRegionRight(srcIC, dst, out dlt);

                            onMsg?.Invoke($"PW{PW_I} 差异值:" + dlt);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                */
            if (mrfz_ScriptConfig.scriptConfig.mainChar == ArkChar.令)
            {
                switch (BattleMap.MapName)
                {
                    case "驯兽小屋":
                        令_驯兽小屋();
                        break;
                    case "礼炮小队":
                        令_礼炮小队();
                        break;
                    case "意外":
                        令_意外();
                        break;
                    case "分赃不均":
                        令_分赃不均();
                        break;
                    case "巡逻队":
                        令_巡逻队();
                        break;
                    case "与虫相伴":
                        令_与虫相伴();
                        break;
                    case "落魄骑士":
                        令_落魄骑士();
                        break;
                    case "压轴登场":
                        令_压轴登场();
                        break;
                }
            }else
            {
                BattleRun_近卫();
                
            }
             


            var TimeOut = DateTime.Now.AddMinutes(8);
            onMsg?.Invoke($"RUNNING ");
            while (true)
            {
                if (DateTime.Now > TimeOut)
                {

                    StopScript("战斗 超时----CODE =22");
                    break;
                }
                var bmp2=mrfzGamePage.CatptureImg();
               var PN2= mrfzGamePage.WhatPage(new string[] { "rogue-end1", "rogue-end2" ,
                "rogue-end4","rogue-end3","GameWin","battle-over"
                }, bmp2);

                bmp2.Dispose();
                switch (PN2)
                {
                    case "rogue-end1":
                    case "rogue-end2":
                    case "rogue-end4":
                    case "rogue-end3":
                        DealGameOver();
                        return false; 
                    case "GameWin":
                        if (GamePageDic["GameWin"].IsCurPage())
                        {
                            //WIN
                            waitSec(2);
                            PageClick("GameWin", $"NEXT", false);
                            flag = GamePageDic["GameWinReward"].CheckPage(TimeOut_NORMAL);
                            if (!flag)
                            {
                                StopScript("战斗结束界面->奖励界面 错误---CODE =22");
                                return false;
                            }
                            onMsg?.Invoke("进入物品奖励界面");
                            //ImageSave.Save(ImageSave.SaveType.SaveWinItem);
                            var dstTime = DateTime.Now.AddSeconds(60);
                            while (true)
                            {

                                if (GamePageDic["get-all-rewards"].IsCurPage())
                                {
                                    PageClick("get-all-rewards", "NEXT", false);
                                    return true;
                                }
                                if (DateTime.Now > dstTime)
                                {
                                    StopScript("获取物品结算超时60秒,请联系作者");
                                    break;
                                }
                                int index = 0;
                                GameItem CurItem = null;
                                var ItemList = WinItem.GetCurItemList();
                                // var CurItem=WinItem.GetCurItem();

                                CurItem = SelectWinItem(ItemList, out index);
                                waitSec(1);
                                if (ItemList == null || ItemList.Count == 0
                                    || CurItem == null || CurItem.itemType == ItemType.UNKNOWN)
                                {
                                    onMsg?.Invoke("判断物品类型为空，尝试进行其他判断");
                                    //get_item_noplan NEXT
                                    if (GamePageDic["rogue-addrolelist-no-char"].IsCurPage())
                                    {
                                        PageClick("rogue-addrolelist-no-char", $"NEXT", false);
                                        PageToClick("add-role-giveup", "NEXT", TimeOut_NORMAL);

                                    }
                                    else if (GamePageDic["rogue-add-role-list"].IsCurPage())
                                    {
                                        PageClick("rogue-add-role-list", $"P1", false);
                                        waitSec(1);
                                        PageClick("rogue-add-role-list", $"NEXT", false);
                                        //...
                                        String[] TwoPages = new string[] { "add-role-giveup" ,
                                    "get-char"
                                };
                                        var PN = mrfzGamePage.WhatPage(TwoPages, TimeOut_NORMAL);
                                        if (PN == "add-role-giveup")
                                        {
                                            PageClick("add-role-giveup", "NEXT", false);
                                        }
                                        else if (PN == "get-char")
                                        {
                                            PageClick("get-char", "NEXT", false);
                                            WaitToNotCurPage("get-char", TimeOut_NORMAL);
                                            PageClick("get-char", "NEXT", false);
                                        }
                                        else
                                        {

                                            StopScript("获取招募卷异常,请联系作者");
                                            break;
                                        }
                                    }
                                    else if (GamePageDic["get_item_noplan"].IsCurPage())
                                    {
                                        PageClick("get_item_noplan", $"NEXT", false);
                                        wait(500);
                                        continue;
                                    }else if(GamePageDic["getsong-ok"].IsCurPage())
                                    {
                                        PageClick("getsong-ok", $"NEXT", false);
                                        wait(500);
                                        continue;
                                    }else
                                    {

                                        onMsg?.Invoke("判断物品类型为未知");
                                    }
                                    wait(500);
                                    continue;
                                }else
                                { 
                                    onMsg?.Invoke($"当前物品为{CurItem.itemType}");
                                } 

                                Point nextClick = WinItem.CurClickPoint(index, CurItem);
                                switch (CurItem.itemType)
                                {
                                    case ItemType.EXIT:
                                        adb.Tap(nextClick);
                                        onMsg?.Invoke($"点击退出");
                                        wait(200);
                                        var dstIC = GamePageDic["rogue-script-exit-itemget-check"]
                                            .regions[0].GetRegionIC();
                                        var offset_rect = new Rectangle(
                                            new Point(
                                           GamePageDic["rogue-script-exit-itemget-check"]
                                            .regions[0].rect.X + WinItem.Offset_X * (index - 1),
                                             GamePageDic["rogue-script-exit-itemget-check"]
                                            .regions[0].rect.Y),
                                             GamePageDic["rogue-script-exit-itemget-check"]
                                            .regions[0].rect.Size
                                            );
                                        DateTime exit_timeout = DateTime.Now.AddMilliseconds(TimeOut_NORMAL);
                                        while (true)
                                        {
                                            if (DateTime.Now > exit_timeout)
                                            {
                                                StopScript("退出 超时");
                                                return false;
                                            }
                                            var bmp = mrfzGamePage.CatptureImg();
                                            var srcIC = ImageColor.FromBitmap(bmp);
                                            //   offset_rect.X = dstIC.rect.X + WinItem.Offset_X * (index-1);
                                            bmp.Dispose();
                                            var dlt = ImageColor.CalcDeltaOfTwoImg(srcIC, dstIC, offset_rect);
                                            if (dlt < mrfz_ScriptConfig.scriptConfig.dlt_region)
                                            {
                                                adb.Tap(new Point(offset_rect.X + 5, offset_rect.Y + 5));
                                                //退出 
                                                return true;
                                            }
                                            wait(200);
                                        }
                                        //rogue-script-exit-itemget-check
                                        break;
                                    case ItemType.SURPPORT:
                                        onMsg?.Invoke($"点击支援道具 获取");
                                        //支援可能重复
                                        adb.Tap(nextClick);
                                        // PageClick("rogue-get-supportitem", $"NEXT", false);
                                        break;
                                    case ItemType.GOLD:
                                        onMsg?.Invoke($"点击金币 获取");
                                        adb.Tap(nextClick);
                                        //  PageClick("rougue-get-gold", $"NEXT", false); 
                                        break;
                                    case ItemType.SONG:
                                        onMsg?.Invoke($"点击剧目 获取");
                                        adb.Tap(nextClick);
                                        //   PageClick("rogue-getsong", $"NEXT", false);
                                        waitSec(1);
                                        String pn = mrfzGamePage.WhatPage(new string[] { "SONG_REPEAT", "getsong-ok" },
                                            TimeOut_NORMAL);

                                        onMsg?.Invoke($" 剧目 pn={pn}");
                                        if (pn == "SONG_REPEAT")
                                        {
                                            PageClick("SONG_REPEAT", $"OK", false);
                                            waitSec(1);
                                            if (GamePageDic["getsong-ok"].CheckPage(TimeOut_NORMAL))
                                            {
                                                flag = PageClickToUntilNextPage("getsong-ok", $"NEXT", TimeOut_NORMAL);
                                                // PageClick("getsong-ok", $"NEXT", false); 
                                                if (!flag)
                                                {

                                                    this.StopScript("获取剧目超时,你可以取消获取它或者报告给作者");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                //?
                                                this.StopScript("未收录的画面-请加入ta");
                                                return false;
                                            }
                                        }
                                        else if (pn == "getsong-ok")
                                        {
                                            onMsg?.Invoke($"获取剧目OK");
                                            PageClick("getsong-ok", $"NEXT", false);
                                            wait(1000);
                                            flag = PageClickToUntilNextPage("getsong-ok", $"NEXT", TimeOut_NORMAL);
                                            // PageClick("getsong-ok", $"NEXT", false); 
                                            if (!flag)
                                            {

                                                this.StopScript("获取剧目超时,你可以取消获取它或者报告给作者");
                                                return false;
                                            }

                                        }
                                        else
                                        {
                                            this.StopScript("非rogue-getsong和getsong-ok,可以截图发给作者");
                                            return false;
                                        }
                                        break;
                                    case ItemType.ITEM:
                                        onMsg?.Invoke($" 收藏品获取 点击 ");
                                        adb.Tap(nextClick);
                                        //PageClick("rogue-win-getitem", $"NEXT", false); 
                                        //..可能是升级卷
                                        break;
                                    case ItemType.PAPER:
                                        onMsg?.Invoke($" 招募卷虎丘 点击 ");
                                        //NEXT按钮是公用的
                                        adb.Tap(nextClick);
                                        //  PageClick("rogue-win-getitem", $"NEXT", false); 
                                        //..升级或者新的干员
                                        break;
                                }
                                wait(500);
                                continue;
                            }
                        }
                        break;
                    case "battle-over":
                        PageClick("battle-over", "NEXT", false);
                        waitSec(2);
                        WaitToNotCurPage("battle-over", 2000);
                        DealGameOver();
                        return false;
                    default:
                        waitSec(2);
                        break;
                } 
                
                 
            }
            return false;
            //rougue-team-edit
        }
        public class DealNodeResult
        {
            public Boolean BattleSucess = false;
            public Boolean IsBattle = false;
            public Boolean Endding = false;
        }
        MapDeBuff.DeBuff[] MapDebuff  = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NodeIndex">本层第几个节点,1,2,3,...</param>
        /// <param name="L">第几层 1,2,3,...</param>
        /// <param name="branchNode"></param>
        /// <returns></returns>
        public DealNodeResult DealNode(int NodeIndex, int L, BranchNode branchNode)
        {
            DealNodeResult dr = new DealNodeResult();

            adb.Tap(branchNode.ClickPoint);
            waitSec(1);
            wait(500);
            switch (branchNode.NT)
            {
                case NodeType.作战:
                case NodeType.紧急作战:
                case NodeType.险路恶敌:
                    Boolean MapSearch = false;
                    if (branchNode.MapName == null)
                    {
                        MapSearch = false;
                    }
                    else
                    {
                        Boolean urgent = branchNode.NT == NodeType.紧急作战;
                        foreach (var map in mrfz_Rogue_Battlemaps)
                        {
                            if (map.MapName == branchNode.MapName && map.urgent == urgent)
                            {
                                MapSearch = true;
                                dr.IsBattle = true;
                                //MapDebuff= MapDeBuff.CurMapDebuff();
                                dr.BattleSucess = BattleMapExcute(map);
                                if (!dr.BattleSucess)
                                {
                                    dr.Endding = true;
                                }
                                break;
                            }
                        }
                    }

                    if (!MapSearch)
                        StopScript("作战节点 不能识别,请到rec_mapname添加节点识别 及后续战斗");

                    break;
                case NodeType.幕间余兴:
                    Deal幕间余兴();
                    break;
                case NodeType.安全的角落:
                    Deal安全屋();
                    break;
                case NodeType.不期而遇:
                    Deal不期而遇();
                    break;
                case NodeType.诡异行商:
                    DealShopToEndding();
                    dr.Endding = true;
                    break;
                default:

                    break;
            }
            return dr;
        }

        Boolean SaveMoney = true;
        public void ShopTest()
        {
            var flag = GamePageDic["shop"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript($"shop 识别错误");
                return;
            }

            //rogue-shop-has-bank
            var HasBank = GamePageDic["rogue-shop-has-bank"].CheckPage(800);
            if (mrfz_ScriptConfig.scriptConfig.policy.AutoSaveGold && HasBank)
            {
                {

                    PageClick("rogue-shop-has-bank", "BANK", false);

                    PageToClick("rogue-shop-bank", "SAVE", TimeOut_NORMAL);

                    WaitToNotCurPage("rogue-shop-bank", TimeOut_NORMAL);
                    int turnGold = 16;
                    while (true)
                    {
                        if (GamePageDic["rogue-bank-save-ok"].IsCurPage())
                        {

                            turnGold = shop_gold.RecCurGolds(null);
                            onMsg?.Invoke("当前金币:" + turnGold);
                            if (turnGold < 0)
                            {
                                PageClick("rogue-bank-save-ok", "BACK", false);
                                waitSec(2);
                                PageClick("rogue-shop-bank", "BACK", false);
                                break;
                            }
                            PageClick("rogue-bank-save-ok", "OK", false);
                            wait(200);
                            continue;
                        }
                        else
                        {
                            if (GamePageDic["rogue-bank-save-no"].IsCurPage())
                            {
                                PageClick("rogue-bank-save-no", "BACK", false);

                                PageToClick("rogue-bank-error", "BACK", TimeOut_NORMAL);
                                //回到shop 
                                //BACK
                                break;
                            }
                            else
                            {

                                wait(200);
                            }
                        }
                    }

                };
            }
            ImageSave.Save(ImageSave.SaveType.SaveShopItem);
            if (mrfz_ScriptConfig.scriptConfig.policy.BuyItem)
            { 
                
                {
                    while (true)
                    {
                        var curGold = shop_gold.RecCurGolds(null); 
                        if (curGold <= 2)
                        {
                            break;
                        }
                        var ShopItemList = shop_item.RecCurShopItemList(HasBank);

                        var shopitem = SelectShopItem(ShopItemList);
                        if (shopitem == null)
                        {
                            onMsg?.Invoke("无商品可买");
                            break;
                        } 
                        var index = shopitem.index;
                        onMsg?.Invoke($"第{index}个商品");
                        //GET ITEM INDEX
                        PageClick("rogue-shop-has-bank", $"NEXT{index}");
                        PageToClick("rouge-shop-buycheck", "NEXT", TimeOut_NORMAL);
                        //可能招募,也可能无招募
                        if (shopitem.Item_Type == shop_item.ItemType.paper)
                        {
                            onMsg?.Invoke($"招募");
                            //get_item_noplan,NEXT
                            var PN = mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list", "rogue-addrolelist-no-char", "get_item_noplan" }, TimeOut_NORMAL);
                            if (PN == "get_item_noplan")
                            {
                                PageClick("get_item_noplan", $"NEXT", false);
                                wait(500);
                            }
                            PN = mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list", "rogue-addrolelist-no-char" }, TimeOut_NORMAL);
                            if (PN == "rogue-addrolelist-no-char")
                            {
                                PageClick("rogue-addrolelist-no-char", $"NEXT", false);
                                PageToClick("add-role-giveup", "NEXT", TimeOut_NORMAL);
                             

                            }
                            else if (PN == "rogue-add-role-list")
                            {
                                int Pindex = Add_Role_Sel.FindHasHopeIndex();
                                PageClick("rogue-add-role-list", $"P{Pindex}", false); 
                                waitSec(1);
                                PageClick("rogue-add-role-list", $"NEXT", false);
                                //...
                                String[] TwoPages = new string[] { "add-role-giveup" ,
                                    "get-char"
                                };
                                PN = mrfzGamePage.WhatPage(TwoPages, TimeOut_NORMAL);
                                if (PN == "add-role-giveup")
                                {
                                    PageClick("add-role-giveup", "NEXT", false);
                                }
                                else if (PN == "get-char")
                                {
                                    PageClick("get-char", "NEXT", false);
                                    WaitToNotCurPage("get-char", TimeOut_NORMAL);
                                    PageClick("get-char", "NEXT", false);
                                }
                                else
                                {

                                    StopScript("获取招募卷异常,请联系作者");
                                    break;
                                } 
                            }
                            else
                            {
                                StopScript("获取招募卷异常,请联系作者");
                                //...
                            }
                            waitSec(1);

                        }
                        else
                        {
                            onMsg?.Invoke($"物品");
                            var to = 1200;
                            var PN = mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list", "shop", "rogue-addrolelist-no-char", "get_item_noplan" }, to);
                            if (PN == "get_item_noplan")
                            {
                                PageClick("get_item_noplan", $"NEXT", false);
                                wait(500);
                            }
                            PN = mrfzGamePage.WhatPage(new string[] { "shop", "rogue-add-role-list", "rogue-addrolelist-no-char" }, to);
                            if (PN == "rogue-addrolelist-no-char")
                            {
                                PageClick("rogue-addrolelist-no-char", $"NEXT", false);
                                PageToClick("add-role-giveup", "NEXT", TimeOut_NORMAL);

                            }
                            else if (PN == "rogue-add-role-list")
                            {
                                ///111->248 Y=135
                                ///613->1003 X= 390 
                               int Pindex= Add_Role_Sel.FindHasHopeIndex();
                                PageClick("rogue-add-role-list", $"P{Pindex}", false);
                                waitSec(1);
                                PageClick("rogue-add-role-list", $"NEXT", false);
                                //...
                                String[] TwoPages = new string[] { "add-role-giveup" ,
                                    "get-char"
                                };
                                PN = mrfzGamePage.WhatPage(TwoPages, TimeOut_NORMAL);
                                if (PN == "add-role-giveup")
                                {
                                    PageClick("add-role-giveup", "NEXT", false);
                                }
                                else if (PN == "get-char")
                                {
                                    PageClick("get-char", "NEXT", false);
                                    WaitToNotCurPage("get-char", TimeOut_NORMAL);
                                    PageClick("get-char", "NEXT", false);
                                }
                                else
                                {

                                    StopScript("获取招募卷异常,请联系作者");
                                    break;
                                }
                            }
                            else
                            {
                                //CONTINUE RIGHT
                            }
                             
                        }
                         
                    }
                    //BUY 
                }
            }
            if (mrfz_ScriptConfig.scriptConfig.policy.L1Exit)
            {

            }
            else
            {//NO BANK----exit bank 
             //rogue-shop-has-bank-error NEXT
             //rogue-shop-exit-check
                waitSec(1);
                PageClick("rogue-shop-has-bank-error", "NEXT", false);
                PageToClick("rogue-shop-exit-check", "OK", TimeOut_NORMAL);
                WaitToNotCurPage("rogue-shop-exit-check", TimeOut_NORMAL);
                //rogue-shop-exit-check
                //L2
                //EXIT
                while (true)
                {
                    var list = BranchNode.GetCurBranchNodes(1, 2, mrfzGamePage.CatptureImg());
                    if (list != null && list.Count > 0)
                    {
                        break;
                    }
                    waitSec(1);
                }
            }
        }
        private void DealShopToEndding()
        {

            PageClick("诡意行商", "NEXT", false);
            WaitToNotCurPage("诡意行商", TimeOut_NORMAL);
            var flag = GamePageDic["shop"].CheckPage(TimeOut_NORMAL);
            if (!flag)
            {
                StopScript($"shop 识别错误");
                return;
            }

            //rogue-shop-has-bank
            var HasBank = GamePageDic["rogue-shop-has-bank"].CheckPage(800);
            if (mrfz_ScriptConfig.scriptConfig.policy.AutoSaveGold && HasBank)
            {
                {

                    PageClick("rogue-shop-has-bank", "BANK", false);

                    PageToClick("rogue-shop-bank", "SAVE", TimeOut_NORMAL);

                    WaitToNotCurPage("rogue-shop-bank", TimeOut_NORMAL);
                    int MAXCOUNT = 18;
                    while (true)
                    { 
                        if (GamePageDic["rogue-bank-save-ok"].IsCurPage())
                        {
                            var turnGold = shop_gold.RecCurGolds(null);
                            onMsg?.Invoke("当前金币:"+turnGold);
                            if (turnGold <= 0|| MAXCOUNT<=0)
                            {
                                PageClick("rogue-bank-save-ok", "BACK", false);
                                waitSec(2);
                                PageClick("rogue-shop-bank", "BACK", false);
                                break;
                            }
                            PageClick("rogue-bank-save-ok", "OK", false);
                            wait(200);
                            MAXCOUNT--;
                            continue;
                        }
                        else
                        {
                            if (GamePageDic["rogue-bank-save-no"].IsCurPage())
                            {
                                PageClick("rogue-bank-save-no", "BACK", false);

                                PageToClick("rogue-bank-error", "BACK", TimeOut_NORMAL);
                                //回到shop 
                                //BACK
                                break;
                            }
                            else
                            {

                                wait(200);
                                var turnGold = shop_gold.RecCurGolds(null);
                                onMsg?.Invoke("当前金币:" + turnGold);
                                if (turnGold <=0)
                                {
                                    PageClick("rogue-bank-save-ok", "BACK", false);
                                    waitSec(2);
                                    PageClick("rogue-shop-bank", "BACK", false);
                                    break;
                                }
                            }
                        }
                    }

                };
            }
            //ImageSave.Save(ImageSave.SaveType.SaveShopItem);
            if(mrfz_ScriptConfig.scriptConfig.policy.BuyItem )
            { 
                var curGold = shop_gold.RecCurGolds(null);
                onMsg?.Invoke("金钱:" + curGold);
                if (curGold >= 2)
                {
                    while(true)
                    {
                        var ShopItemList = shop_item.RecCurShopItemList(HasBank); 
                        var shopitem = SelectShopItem(ShopItemList);
                        if(shopitem==null)
                        {
                        //    onMsg?.Invoke("金钱:" + curGold);
                            onMsg?.Invoke("无商品可买");
                         // 
                            break;
                        }
                        var index = shopitem.index;
                        //GET ITEM INDEX
                        PageClick("rogue-shop-has-bank",$"NEXT{index}");
                        onMsg?.Invoke($"第{index}个商品");
                        PageToClick("rouge-shop-buycheck", "NEXT", TimeOut_NORMAL);
                        //可能招募,也可能无招募
                        if(shopitem.Item_Type== shop_item.ItemType.paper)
                        {
                            //get_item_noplan,NEXT
                            var PN=mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list", "rogue-addrolelist-no-char", "get_item_noplan" },TimeOut_NORMAL);
                            if (PN == "get_item_noplan")
                            {
                                PageClick("get_item_noplan", $"NEXT", false);
                                wait(500);
                            }
                            PN = mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list", "rogue-addrolelist-no-char" }, TimeOut_NORMAL);
                            if (PN=="rogue-addrolelist-no-char" )
                            {
                                PageClick("rogue-addrolelist-no-char", $"NEXT", false);
                                PageToClick("add-role-giveup", "NEXT", TimeOut_NORMAL);

                            }
                            else if (PN=="rogue-add-role-list")
                            {
                                int Pindex = Add_Role_Sel.FindHasHopeIndex();
                                PageClick("rogue-add-role-list", $"P{Pindex}", false);
                                waitSec(1);
                                PageClick("rogue-add-role-list", $"NEXT", false);
                                //...
                                String[] TwoPages = new string[] { "add-role-giveup" ,
                                    "get-char"
                                };
                                  PN = mrfzGamePage.WhatPage(TwoPages, TimeOut_NORMAL);
                                if (PN == "add-role-giveup")
                                {
                                    PageClick("add-role-giveup", "NEXT", false);
                                }
                                else if (PN == "get-char")
                                {
                                    PageClick("get-char", "NEXT", false);
                                    WaitToNotCurPage("get-char", TimeOut_NORMAL);
                                    PageClick("get-char", "NEXT", false);
                                }
                                else
                                {

                                    StopScript("获取招募卷异常,请联系作者");
                                    break;
                                }
                            }else
                            {
                                StopScript("获取招募卷异常,请联系作者");
                                //...
                            }
                             
                        }
                        else
                        {
                            var to = 1200;
                            var PN = mrfzGamePage.WhatPage(new string[] { "rogue-add-role-list","shop", "rogue-addrolelist-no-char", "get_item_noplan" }, to);
                            if (PN == "get_item_noplan")
                            {
                                PageClick("get_item_noplan", $"NEXT", false);
                                wait(500);
                            }
                            PN = mrfzGamePage.WhatPage(new string[] {"shop", "rogue-add-role-list", "rogue-addrolelist-no-char" }, to);
                            if (PN == "rogue-addrolelist-no-char")
                            {
                                PageClick("rogue-addrolelist-no-char", $"NEXT", false);
                                PageToClick("add-role-giveup", "NEXT", TimeOut_NORMAL);

                            }
                            else if (PN == "rogue-add-role-list")
                            {
                                PageClick("rogue-add-role-list", $"P1", false);
                                waitSec(1);
                                PageClick("rogue-add-role-list", $"NEXT", false);
                                //...
                                String[] TwoPages = new string[] { "add-role-giveup" ,
                                    "get-char"
                                };
                                PN = mrfzGamePage.WhatPage(TwoPages, TimeOut_NORMAL);
                                if (PN == "add-role-giveup")
                                {
                                    PageClick("add-role-giveup", "NEXT", false);
                                }
                                else if (PN == "get-char")
                                {
                                    PageClick("get-char", "NEXT", false);
                                    WaitToNotCurPage("get-char", TimeOut_NORMAL);
                                    PageClick("get-char", "NEXT", false);
                                }
                                else
                                {

                                    StopScript("获取招募卷异常,请联系作者");
                                    break;
                                }
                            }
                            else
                            { 
                                //CONTINUE RIGHT
                            }
                        }

                        waitSec(1);
                        WaitToCurPage("shop", TimeOut_NORMAL);
                    } 
                    //BUY 
                }
            }
            if (mrfz_ScriptConfig.scriptConfig.policy.L1Exit)
            {

            }else
            {//NO BANK----exit bank 
             //rogue-shop-has-bank-error NEXT
             //rogue-shop-exit-check
                waitSec(1);
                PageClick("rogue-shop-has-bank-error", "NEXT", false);
                PageToClick("rogue-shop-exit-check", "OK", TimeOut_NORMAL);
                WaitToNotCurPage("rogue-shop-exit-check", TimeOut_NORMAL);
                //rogue-shop-exit-check
                //L2
                //EXIT
                while (true)
                {
                    var list = BranchNode.GetCurBranchNodes(1, 2, mrfzGamePage.CatptureImg());
                    if (list != null && list.Count > 0)
                    {
                        break;
                    }
                    waitSec(1);
                }
            }
            

            PageToClick("rogue-L2", "BACK", TimeOut_NORMAL);

            PageToClick("rogue-givup", "NEXT", TimeOut_NORMAL);


            PageToClick("giveupchenck", "NEXT", TimeOut_NORMAL);
            WaitToNotCurPage("giveupchenck",2000);
            //rogue-end1
            //rogue-givup
            DealGameOver();
        }
        private void PageToClick(String page, String ClickName, int ms)
        {
            var flag = GamePageDic[page].CheckPage(ms);
            if (!flag)
            {
                StopScript($"{page} 识别错误");
                return;
            }
            PageClick(page, ClickName, false);
        }
       
        protected override void StopScript(String msg)
        {
            if (mrfzGamePage.IsUpdatePage)
            {
                throw new ScriptException(true, false, "next") {  IsUpdate
                =true}; 
            }
            onMsg?.Invoke(msg);
            ////..
            /// 
            this.RunError = true;
            this.scriptMachine.ScriptRunning = false;
            try
            {
                mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                onMsg?.Invoke("如有疑问可把本程序文件夹下最后一张图片.png发送给作者");
            }
            catch
            {

            } 
            scriptMachine?.Stop();

        }
        private void DealGameOver()
        {



            DateTime last_time = DateTime.Now.AddSeconds(30);
            wait(500);
            while (true)
            {
                if (GamePageDic["rogue-end1"].IsCurPage())
                {
                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);

                    //    PageClick("rogue-end1", "NEXT", false);
                }
                else if (GamePageDic["rogue-end2"].IsCurPage())
                {
                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);

                    //   PageClick("rogue-end2", "NEXT", false);
                }
                else if (GamePageDic["rogue-end4"].IsCurPage())
                {
                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);

                    //   PageClick("rogue-end4", "NEXT", false);
                }
                else if (GamePageDic["rogue-end3"].IsCurPage())
                {
                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);

                    //  PageClick("rogue-end3", "NEXT", false);
                }
                else if (GamePageDic["rogue-end5"].IsCurPage())
                {

                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);
                    // PageClick("rogue-end5", "NEXT", false);
                }
                else if (GamePageDic["rogue-kuiying-home-normal"].CheckPage(800))
                {
                    break;
                }
                else
                {
                    if (DateTime.Now > last_time)
                    {
                        try
                        {
                            mrfzGamePage.CatptureImg().Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                        }
                        catch
                        {

                        }
                        StopScript("战斗结算超时,请把本程序文件夹下 最后一张图片.png发给作者本人");

                        return;
                    }
                    PageClick("rogue-kuiying-home-normal", "END_NEXT", false);
                }
                waitSec(1);

            }

        } 
        

        private void Deal_NoSP_不期而遇()
        {
            var nps = mrfzGamePage.GamePageDic["不期而遇-其他2"].NextPages;
            var flag = false;
            foreach (var np in nps)
            {
                var p = np.ClickPoint;
                for (int i = 0; i < 5; i++)
                {
                    adb.Tap(p);
                    wait(300);
                    if (mrfzGamePage.GamePageDic["rogue-next-node"].IsCurPage())
                    {
                        //  p=mrfzGamePage.GamePageDic["不期而遇-END"].NextPages[0].ClickPoint;
                        flag = true;
                        //  adb.Tap(p);
                        return;
                    }
                }
            }
            if (!flag)
            {
                StopScript($"不期而遇 不能回到 节点选择");
                return;
            }
        }
        public void DEAL_TEST()
        {
            var PN = mrfzGamePage.WhatPage(new string[] { "幕间余兴1-1", "幕间余兴2-1", "幕间余兴3-1" }, TimeOut_NORMAL);
            if (PN == "幕间余兴1-1")
            {
                PageToClick("幕间余兴1-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-4", "NEXT", TimeOut_NORMAL);
            }
            else if (PN == "幕间余兴2-1")
            {
                PageToClick("幕间余兴2-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-4", "NEXT", TimeOut_NORMAL);

            }
            else if (PN == "幕间余兴3-1")
            {

                PageToClick("幕间余兴3-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-4", "NEXT", TimeOut_NORMAL);
            }
            else
            {
                StopScript("未识别的 幕间余兴");
            }
        }
        public   void Deal幕间余兴()
        { 
            if(Program.Debug)
            {
                onMsg?.Invoke("幕间余兴 暂停,DEBUG, 继续");
                mrfz_ScriptMachine.Pause = true;
                while (mrfz_ScriptMachine.Pause)
                {
                    waitSec(1); 
                }
            }
             
            PageClick("rogue-幕间余兴", "NEXT", false);
            WaitToNotCurPage("rogue-幕间余兴", TimeOut_NORMAL);

           var PN= mrfzGamePage.WhatPage(new string[] { "幕间余兴1-1", "幕间余兴2-1", "幕间余兴3-1" }, TimeOut_NORMAL);
             
            if(PN== "幕间余兴1-1")
            {
                PageToClick("幕间余兴1-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴1-4", "NEXT", TimeOut_NORMAL);
                var dtt= DateTime.Now.AddMilliseconds(TimeOut_NORMAL); 
                while (!GamePageDic["rogue-next-node"].IsCurPage()|| DateTime.Now< dtt)
                {

                    PageClick("幕间余兴2-3", "NEXT", false);
                    waitSec(1);
                }
            }
            else if(PN == "幕间余兴2-1")
            {
                PageToClick("幕间余兴2-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴2-4", "NEXT", TimeOut_NORMAL);
                var dtt = DateTime.Now.AddMilliseconds(TimeOut_NORMAL);
                while (!GamePageDic["rogue-next-node"].IsCurPage() || DateTime.Now < dtt)
                {

                    PageClick("幕间余兴2-3", "NEXT", false);
                    waitSec(1);
                }
                //幕间余兴2-3

            }
            else if(PN=="幕间余兴3-1")
            {

                PageToClick("幕间余兴3-1", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-2", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-3", "NEXT", TimeOut_NORMAL);
                PageToClick("幕间余兴3-4", "NEXT", TimeOut_NORMAL);
                var dtt = DateTime.Now.AddMilliseconds(TimeOut_NORMAL);
                while (!GamePageDic["rogue-next-node"].IsCurPage() || DateTime.Now < dtt)
                {

                    PageClick("幕间余兴2-3", "NEXT", false);
                    waitSec(1);
                }
                // flag = GamePageDic["rogue-next-node"].CheckPage(5000);
            }
            else
            {
                Deal_NoSP_不期而遇();
                return;
              //  StopScript("未识别的 幕间余兴");
            }
          
        }
        private void Deal安全屋()
        {
            PageClick("rogue-安全的角落", "NEXT", false);
            WaitToNotCurPage("rogue-安全的角落", TimeOut_NORMAL);
            Boolean NoSP = true;
            Boolean flag = false;
            if (NoSP)
            {
                Deal_NoSP_不期而遇();
                return;
            }
        }
        private void Deal不期而遇()
        { 
            PageClick("rogue-不期而遇", "NEXT", false);
            WaitToNotCurPage("rogue-不期而遇",TimeOut_NORMAL);
            Boolean NoSP = true;
            Boolean flag = false;
            if(NoSP)
            {
                Deal_NoSP_不期而遇();
                return;
            }
            //解脱1

              flag=GamePageDic["解脱1"].CheckPage(500);
            if (!flag)
            {
                waitSec(1);
                PageClick("解脱1", "NEXT", false);


                PageToClick("解脱2", "NEXT", 5000); 

                PageToClick("解脱3", "NEXT",5000);

                PageToClick("解脱-getitem", "NEXT", 4000);

                PageToClick("解脱-getitem-ok", "NEXT", 3000);

                PageToClick("解脱-ok", "NEXT", 3000);
                PageToClick("解脱-changeL5", "NEXT", 5000);
                 
                flag = GamePageDic["rogue-next-node"].CheckPage(5000);
                if (!flag)
                {
                    StopScript($"解脱4 不能回到 节点选择");
                    return;
                }

            }
            else
            {
                flag = GamePageDic["赴宴1"].CheckPage(500);
                if(flag)
                {
         
                    waitSec(1);
                    PageClick("赴宴1", "NEXT", false); 
                    PageToClick("赴宴2", "NEXT", 5000);
                    PageToClick("赴宴3", "NEXT", 5000);
                    PageToClick("赴宴4", "NEXT", 4000);
                     
                    flag = GamePageDic["rogue-next-node"].CheckPage(4000);
                    if (!flag)
                    {
                        StopScript($"赴宴4 不能回到 节点选择");
                        return;
                    }
                }
                else
                {
                    //普通不期而遇
                    Deal_NoSP_不期而遇();  
                }
            }
            //赴宴1
            //  GamePageDic["rogue-不期而遇"]
        }
        List<mrfz_rogue_battlemap> mrfz_Rogue_Battlemaps = new List<mrfz_rogue_battlemap>();
        private void ClcBattleMapNode()
        {
            String[] MapNames = new string[] { "驯兽小屋","意外","与虫为伴","礼炮小队" , "死斗" }; 
            foreach (var mapName in MapNames)
            {
                mrfzGamePage rec=null, first = null, putchar = null, skill = null;
                mrfzGamePage rec_2 = null, first_2 = null, putchar_2 = null, skill_2 = null;
              
                foreach (var kv in GamePageDic)
                {
                    var gp = kv.Value;
                    if (!gp.Baned &&  mapName==gp.MapName&&gp.pageCatgory!= mrfzGamePage.PageCatgory.控制流程)
                    {
                        if (gp.pageType == mrfzGamePage.PageType.肉鸽_愧影 &&
                             gp.pageCatgory == mrfzGamePage.PageCatgory.地图识别)
                        {
                            if (gp.紧急)
                            {
                                rec_2 = gp;
                            }
                            else
                            {
                                rec = gp;
                            }
                        } else
                        {
                            if (gp.pageType == mrfzGamePage.PageType.肉鸽_愧影 &&
                             gp.pageCatgory == mrfzGamePage.PageCatgory.战斗地图)
                            {
                                if(gp.mapType== mrfzGamePage.MapType.初始)
                                {
                                    first = gp;
                                    first_2 = gp;
                                }else if(gp.mapType== mrfzGamePage.MapType.放置)
                                {
                                    putchar = gp;
                                    putchar_2 = gp;
                                }else if(gp.mapType== mrfzGamePage.MapType.技能)
                                {
                                    skill = gp;
                                    skill_2 = gp;
                                }
                            }
                        }
                    }

                }
                if(rec!=null)
                {
                    mrfz_Rogue_Battlemaps.Add(new mrfz_rogue_battlemap(mapName, false, rec, first, putchar, skill));
                }
                if (rec_2 != null)
                {
                    mrfz_Rogue_Battlemaps.Add(new mrfz_rogue_battlemap(mapName, true, rec_2, first_2, putchar_2, skill_2));
                }
               // if(rec==null&&rec_2==null)
                {
                    //死斗
                   // mrfz_Rogue_Battlemaps.Add(new mrfz_rogue_battlemap(mapName, false, new mrfzGamePage() { Name=mapName,
                    // NextPages=new List<ClickToNextPage>() { new ClickToNextPage() { Name="NEXT",P } }
                   // }, first, putchar, skill));
                   // mrfz_Rogue_Battlemaps.Add(new mrfz_rogue_battlemap(mapName, true, rec_2, first_2, putchar_2, skill_2));
                }
            }
             
        }
      
        //public void 
        public enum NodeType
        {
            不期而遇,
            作战,
            紧急作战,
            诡异行商,
            幕间余兴,
            安全的角落,
            险路恶敌,
            古堡馈赠,
            Unkown 
        }


        #region BASE_MODEL
        protected override bool WaitToNotCurPage(String PageName, int TimeOut_ms)
        {
            var first = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - first).TotalMilliseconds > TimeOut_ms)
                {
                    return true;
                }
                if (mrfz_ScriptUnit. GamePageDic[PageName].IsCurPage())
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
        protected override bool WaitToCurPage(String PageName, int TimeOut_ms,int interval_ms=300)
        {
            var first = DateTime.Now;
            while (true)
            {
                
                if (GamePageDic[PageName].IsCurPage())
                {
                    return false;
                }
                else
                {
                    wait(interval_ms);
                    if ((DateTime.Now - first).TotalMilliseconds > TimeOut_ms)
                    {
                        return true;
                    }
                }
            }
        }
        protected void PageSwipe(String PageName, String startPoint,String EndPoint,int ms=500)
        {
            var ST = SearchClickName(GamePageDic[PageName].NextPages, startPoint);
            var ED = SearchClickName(GamePageDic[PageName].NextPages, EndPoint);
            if (ST == null || ED == null)
            {
               StopScript($" 找不到{PageName} {startPoint} 或者{EndPoint}点击数据");
                return;
            }
            adb.Swipe(ST.ClickPoint, ED.ClickPoint, ms);
        }
       
        protected void PageSwipe(String PageName, String EndPoint, Dir dir, int ms = 500)
        { 
            var ED = SearchClickName(GamePageDic[PageName].NextPages, EndPoint);
            if (  ED == null)
            {
                StopScript($" 找不到{PageName}{EndPoint}点击数据");
            }
            Point FinalPoinst = new Point();
            int delta = 300;
            switch(dir)
            {
                case Dir.上:
                    FinalPoinst = new Point(ED.ClickPoint.X, ED.ClickPoint.Y - delta);
                    break;
                case Dir.下:
                    FinalPoinst = new Point(ED.ClickPoint.X, ED.ClickPoint.Y + delta);
                    break;
                case Dir.右:
                    FinalPoinst = new Point(ED.ClickPoint.X+delta, ED.ClickPoint.Y );
                    break;
                case Dir.左:
                    FinalPoinst = new Point(ED.ClickPoint.X -delta, ED.ClickPoint.Y);
                    break;
            }
            if (FinalPoinst.X < 0) FinalPoinst.X = 0;
            if (FinalPoinst.X >= ImgW) FinalPoinst.X = ImgW - 1;
            if (FinalPoinst.Y < 0) FinalPoinst.Y = 0;
            if (FinalPoinst.Y >= ImgH) FinalPoinst.Y = ImgH - 1;
            adb.Swipe(ED.ClickPoint, FinalPoinst, ms);
        }
        /// <summary>
        /// 返回正确到下一页 ,
        /// TRUE=对
        /// FALSE=超时 错误 需要停止
        /// </summary>
        /// <param name="PageName"></param>
        /// <param name="ClickName"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        protected   bool PageClickToUntilNextPage(String PageName, String ClickName,int Timeout)
        {
            var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
            var dst=DateTime.Now.AddMilliseconds(Timeout);
            if (CNP == null) { { StopScript($" 找不到{PageName} {ClickName}点击数据"); } }
            while (true)
            {
                if (DateTime.Now > dst) return false;
                if(GamePageDic[PageName].IsCurPage())
                {
                    ClickPage(CNP);
                    wait(1000);
                }else
                {
                    return true;
                }
            }
             


        }
        protected override bool PageClick(String PageName, String ClickName, Boolean Valid = false)
        {
            if (Valid)
            {
                if (GamePageDic[PageName].IsCurPage())
                {
                    var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
                    if (CNP == null) { { StopScript($" 找不到{PageName} {ClickName}点击数据"); } }
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
                var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
                if (CNP == null) { { StopScript($" 找不到{PageName} {ClickName}点击数据"); } }
                ClickPage(CNP);
                wait(200);
                onMsg?.Invoke($"page:{PageName} clickname:{ClickName}");
                return true;
            }


        }
        protected override ClickToNextPage SearchClickName(List<ClickToNextPage> list, String Name)
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
        protected override void ClickPage(ClickToNextPage cnp)
        { 
            adb.Tap(cnp.ClickPoint); 
            wait(100);
        }
        #endregion
    }
    public class MRFZCheckState :  CheckState
    { 
    }
}
