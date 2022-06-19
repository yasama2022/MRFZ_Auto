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
        public void RunNormal()
        {
            onMsg?.Invoke($"普通关卡挂机 任务即将开始");

            var bmp = mrfzGamePage.CatptureImg();
            if (bmp == null || bmp.Width != W || bmp.Height != H)
            {
                bmp?.Dispose();
                throw new ScriptException(true, true, $"获取图像错误,可能是游戏窗口被最小化了或者手动改变过窗口,分辨率/DPI,错误或者adb没有连接模拟器 " +
                $"(核定配置:分辨率1280*720 DPI=240 雷电模拟器 安卓7.1.2 平板型)");
            }
            bmp?.Dispose();
            if (GamePageDic["norpage_noproxy"].IsCurPage())
            {
                PageClick("norpage_noproxy","NEXT",false);
                waitSec(1);
            }
            PageToClick("norpage_start", "NEXT", TimeOut_NORMAL);
           var PN= mrfzGamePage.WhatPage(new String[]{ "norpage_team", "norpage_addhp", "norpage_addhp2" },TimeOut_NORMAL);
            if(PN== "norpage_addhp")
            {
                //norpage_addhp=吃药
                 if(scriptMachine.eatmedi>0&&scriptMachine.curmedi<scriptMachine.eatmedi)
                {

                    PageClick("norpage_addhp", "EAT", true);
                    scriptMachine.curmedi++;
                    
                    PageToClick("norpage_start", "NEXT", TimeOut_NORMAL);
                }
                else
                {
                    PageClick("norpage_addhp", "NEXT", true);
                    StopScript("理智耗尽 停止运行");
                    return;
                } 
               
            }
            else if(PN== "norpage_addhp2")
            {
                if (scriptMachine.eatstone > 0 && scriptMachine.curstone < scriptMachine.eatstone)
                {

                    PageClick("norpage_addhp2", "EAT", true);
                    scriptMachine.curstone++;
                    
                    PageToClick("norpage_start", "NEXT", TimeOut_NORMAL);

                }
                else
                {
                    PageClick("norpage_addhp2", "NEXT", true);
                    StopScript("理智耗尽 停止运行");
                    return;
                }
            }
            else
            {

            }
            PageToClick("norpage_team", "NEXT", TimeOut_NORMAL);
            if(WaitToCurPage("norpage_run", TimeOut_BATTLE))
            {
                StopScript("norpage_run 进入超时");
                return;
            }
           
            if (WaitToCurPage("norpage_end", 1000*60*8,1000))
            {
                StopScript("norpage_end 进入超时，关卡时间过长，8分钟超时。");
                return;
            }
            waitSec(2);
            PageClick("norpage_end", "NEXT", false);
            WaitToNotCurPage("norpage_end",3000);
            if(GamePageDic["norpage_end"].IsCurPage())
            {

                PageClick("norpage_end", "NEXT", false);
            }
            //norpage_start
            if (WaitToCurPage("norpage_start", TimeOut_NORMAL))
            {

                StopScript("norpage_start 进入超时，不能回到关卡开始页面");
                return;
            }
        }
    }
}
