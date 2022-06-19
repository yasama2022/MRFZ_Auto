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
        /// <summary>
        /// 保全派驻 多伦矿场
        /// </summary>
        public void RunBaoQuanDuoLun()
        {
            onMsg?.Invoke($"保全派驻 多伦矿场 任务即将开始");

            var bmp = mrfzGamePage.CatptureImg();
            if (bmp == null || bmp.Width != W || bmp.Height != H)
            {
                bmp?.Dispose();
                throw new ScriptException(true, true, $"获取图像错误,可能是游戏窗口被最小化了或者手动改变过窗口,分辨率/DPI,错误或者adb没有连接模拟器 " +
                $"(核定配置:分辨率1280*720 DPI=240 雷电模拟器 安卓7.1.2 平板型)");
            }
            bmp?.Dispose();
            
        }
    }
}
