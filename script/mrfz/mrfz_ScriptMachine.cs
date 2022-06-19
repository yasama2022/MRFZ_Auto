using GamePageScript.lib;
using GamePageScript.ui;
using lib;
using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32;
using static MRFZ_Auto.ui.Contorls.ADB_Form_List;

namespace GamePageScript.script.mrfz
{
    public class mrfz_ScriptMachine : ScriptMachine
    {

        //public static String GameWinName;
        mrfz_ScriptConfig  ScriptConfig;
        public static Boolean Pause = false;
        /// <summary>
        /// 窗口 宽高
        /// </summary>
        protected const int W = 1200, H = 705;
        public mrfz_ScriptUnit RunningUnit;
        public Boolean ScriptRunning = false;
        public Boolean Running = false;
        public Dictionary<string, mrfzGamePage> Pages { get; private set; }
        Thread mainThread;
        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint SC_MONITORPOWER = 0xF170;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, int lParam);
        private void OpenMonitor(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_SYSCOMMAND, SC_MONITORPOWER, -1); //打开显示器;
        }

        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
        protected   override void LoadConfig()
        {
            ScriptConfig = mrfz_ScriptConfig.scriptConfig;
           // GameWinName = "雷电模拟器";
            Pages = mrfzGamePage.GamePageDic;

        }
        public mrfz_ScriptMachine()
        {
            LoadConfig();
        }
        static mrfz_ScriptMachine()
        {
        }
        protected void waitSec(int sec)
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
        int TimeOut = 3000;
        public Boolean IsPageRight { get
            {
                var  IsKUIYING_Home =
                        mrfzGamePage.WhatPage(new string[] { "rogue-kuiying-home", "home" }, TimeOut) != null;
                return IsKUIYING_Home;
            }
        }
        public Boolean IsPageRight_Nor
        {
            get
            {
                var IsKUIYING_Home =
                        mrfzGamePage.WhatPage(new string[] { "norpage_start", "norpage_noproxy" }, TimeOut) != null;
                return IsKUIYING_Home;
            }
        }
        public bool CheckGameWindow()
        {
            if (mrfz_ScriptConfig.scriptConfig.UseADBCat)
            {
                return true;
            }
            Boolean CanRunScript = false;
            var Handle = adb_item.WIF.Handle;
            Win.SetWindowPOS(Handle, 0, 0);
            waitSec(1);
            Win.SetForegroundWindow(Handle);
            waitSec(1);
            if (Handle == null || Handle.ToInt32() == 0)
            {
                onMsg?.Invoke("请先启动雷电模拟器,或MUMU模拟器,夜神模拟器进入肉鸽准备界面后再启动本程序");
                return CanRunScript;
            }
            Rectangle rect = new Rectangle();
            WinLib.GetWindowRect(Handle, out rect);
            var SW = Screen.PrimaryScreen.Bounds.Width;
            var SH = Screen.PrimaryScreen.Bounds.Height;
            //  Boolean IsKUIYING_Home = false;
            if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.LD)
            {
                if (rect.Width == 1322 && rect.Height == 756)
                {
                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(Handle, 0, 0, 1282, 756);
                        wait(500);

                        if (IsPageRight)
                        {
                            return true;
                        }
                        else
                        {

                            onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                            onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                            return false;
                        }
                    }
                }
                else if (rect.Width == 1282 && rect.Height == 756)
                {

                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(Handle, 0, 0, 1322, 756);
                        wait(500);

                        if (IsPageRight)
                        {
                            return true;
                        }
                        else
                        {

                            bool small_screen = (SW < 1400 || SH < 800);
                            if (SW < 1366 || SH < 768)
                            {

                                onMsg?.Invoke("屏幕过小, 最低1366*768");
                            }
                            else
                            {

                                onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                                onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");

                            }
                            return false;
                        }
                    }
                }
                else
                {
                    Win.SetWindowSize(Handle, 0, 0, 1282, 756);
                    wait(500);

                    if (IsPageRight)
                    {
                        return true;
                    }

                    Win.SetWindowSize(Handle, 0, 0, 1322, 756);
                    wait(500);

                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {

                        bool small_screen = (SW < 1400 || SH < 800);
                        if (SW < 1366 || SH < 768)
                        {

                            onMsg?.Invoke("屏幕过小, 最低1366*768");
                        }
                        else
                        {

                            onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                            onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                        }
                        return false;
                    }
                }


            }
            else if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.YS)
            {

                Win.ShowWindow(Handle);
                Thread.Sleep(300);
                Win.SetWindowSize(Handle, 0, 0, 1280 + 40 + 2, 720 + 32 + 1);
                Thread.Sleep(300);
                if (IsPageRight)
                {
                    return true;
                }
                else
                {
                    onMsg?.Invoke("夜神模拟器 窗口大小不正确,不要手动放大，缩小(设置1280*720后重启模拟器即可)");
                    return false;
                }

            }
            else if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.MUMU)
            {
                bool small_screen = (SW < 1400 || SH < 900);
                if (small_screen)
                {

                    onMsg?.Invoke("屏幕过小,该分辨率不支持MUMU，建议用雷电模拟器/夜神");
                    return false;

                }
                else
                {
                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {
                        onMsg?.Invoke("MUMU模拟器 窗口大小不正确,不要手动放大，缩小(设置1280*720后重启模拟器即可)");
                        return false;
                    }
                    //NOTHINGS
                }
            }
            else
            {
                onMsg?.Invoke("模拟器类型 不正确");
                return false;
            }
        }
        public bool CheckGameWindow_Nor()
        {
            if (mrfz_ScriptConfig.scriptConfig.UseADBCat)
            {
                return true;
            }
            Boolean CanRunScript = false;
            var Handle = adb_item.WIF.Handle;
            Win.SetWindowPOS(Handle, 0, 0);
            waitSec(1);
            Win.SetForegroundWindow(Handle);
            waitSec(1);
            if (Handle == null || Handle.ToInt32() == 0)
            {
                onMsg?.Invoke("请先启动雷电模拟器,或MUMU模拟器,夜神模拟器进入肉鸽准备界面后再启动本程序");
                return CanRunScript;
            }
            Rectangle rect = new Rectangle();
            WinLib.GetWindowRect(Handle, out rect);
            var SW = Screen.PrimaryScreen.Bounds.Width;
            var SH = Screen.PrimaryScreen.Bounds.Height;
            //  Boolean IsKUIYING_Home = false;
            if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.LD)
            {
                if (rect.Width == 1322 && rect.Height == 756)
                {
                    if (IsPageRight_Nor)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(Handle, 0, 0, 1282, 756);
                        wait(500);

                        if (IsPageRight_Nor)
                        {
                            return true;
                        }
                        else
                        {

                            onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                            onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                            return false;
                        }
                    }
                }
                else if (rect.Width == 1282 && rect.Height == 756)
                {

                    if (IsPageRight_Nor)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(Handle, 0, 0, 1322, 756);
                        wait(500);

                        if (IsPageRight_Nor)
                        {
                            return true;
                        }
                        else
                        {

                            bool small_screen = (SW < 1400 || SH < 800);
                            if (SW < 1366 || SH < 768)
                            {

                                onMsg?.Invoke("屏幕过小, 最低1366*768");
                            }
                            else
                            {

                                onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                                onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");

                            }
                            return false;
                        }
                    }
                }
                else
                {
                    Win.SetWindowSize(Handle, 0, 0, 1282, 756);
                    wait(500);

                    if (IsPageRight_Nor)
                    {
                        return true;
                    }

                    Win.SetWindowSize(Handle, 0, 0, 1322, 756);
                    wait(500);

                    if (IsPageRight_Nor)
                    {
                        return true;
                    }
                    else
                    {

                        bool small_screen = (SW < 1400 || SH < 800);
                        if (SW < 1366 || SH < 768)
                        {

                            onMsg?.Invoke("屏幕过小, 最低1366*768");
                        }
                        else
                        {

                            onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                            onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                        }
                        return false;
                    }
                }


            }
            else if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.YS)
            {

                Win.ShowWindow(Handle);
                Thread.Sleep(300);
                Win.SetWindowSize(Handle, 0, 0, 1280 + 40 + 2, 720 + 32 + 1);
                Thread.Sleep(300);
                if (IsPageRight_Nor)
                {
                    return true;
                }
                else
                {
                    onMsg?.Invoke("夜神模拟器 窗口大小不正确,不要手动放大，缩小(设置1280*720后重启模拟器即可)");
                    return false;
                }

            }
            else if (adb_item.adb_type == ADB_LIST_ITEM.ADB_TYPE.MUMU)
            {
                bool small_screen = (SW < 1400 || SH < 900);
                if (small_screen)
                {

                    onMsg?.Invoke("屏幕过小,该分辨率不支持MUMU，建议用雷电模拟器/夜神");
                    return false;

                }
                else
                {
                    if (IsPageRight_Nor)
                    {
                        return true;
                    }
                    else
                    {
                        onMsg?.Invoke("MUMU模拟器 窗口大小不正确,不要手动放大，缩小(设置1280*720后重启模拟器即可)");
                        return false;
                    }
                    //NOTHINGS
                }
            }
            else
            {
                onMsg?.Invoke("模拟器类型 不正确");
                return false;
            }
        }
        public static ADB_LIST_ITEM adb_item;
        public void RunInMainForm(mrfzScriptWin mainForm, ADB_LIST_ITEM adb_item)
        {
            Running = true;
           
            if (onMsg == null)
                this.onMsg += (x) =>
                {
                    mainForm.BeginInvoke(mainForm.onMsg, x);
                };
            mainThread = new Thread(() =>
            {
                bool canrun = CheckGameWindow();
                if (canrun)
                {
                    if (mrfz_ScriptConfig.scriptConfig.UseADBCat)
                    {
                        onMsg?.Invoke("使用adb截图,不需要对窗口检测");

                    }
                    else
                    {
                        onMsg?.Invoke("窗口检测通过");
                    }
                    Run();
                    Running = false;
                }
                else
                {
                    onMsg?.Invoke("窗口检测不通过");
                    if (mrfzGamePage.Hwnd != null && mrfzGamePage.Hwnd.ToInt32() != 0)
                    {
                        try
                        {
                            var bmp = Win.GetWindowClientCapture(mrfzGamePage.Hwnd);
                            bmp.Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                            bmp.Dispose();
                            bmp = mrfzGamePage.CatptureImg();
                            bmp.Save(Environment.CurrentDirectory + "\\最后一张图片2.png");
                            bmp.Dispose();
                            onMsg?.Invoke("不能运行，请根据上述提示调整设置，如有疑问,可把本程序文件夹下最后一张图片.png和最后一张图片2.png发给作者(Q群)");
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        onMsg?.Invoke("窗口句柄为空");

                    }
                }
                Running = false;
            });
            mainThread.Start(); 
        }
        public int eatstone = -1;
        public int eatmedi = -1;
        public int curstone = 0;
        public int curmedi = 0;
        public void RunInMainForm_Normal(mrfzScriptWin mainForm, ADB_LIST_ITEM adb_item
            ,int eatstone=-1,int eatmedi=-1)
        {
            this.eatstone = eatstone;
            this.eatmedi = eatmedi;
            curmedi = 0;
            curstone = 0;
            Running = true;

            if (onMsg == null)
                this.onMsg += (x) =>
                {
                    mainForm.BeginInvoke(mainForm.onMsg_nor, x);
                };
            mainThread = new Thread(() =>
            {
                bool canrun = CheckGameWindow_Nor();
                if (canrun)
                {
                    if (mrfz_ScriptConfig.scriptConfig.UseADBCat)
                    {
                        onMsg?.Invoke("使用adb截图,不需要对窗口检测");
                    }
                    else
                    {
                        onMsg?.Invoke("窗口检测通过");
                    }
                    NextTaskType = mrfz_ScriptUnit.TaskType.NormalStart;
                    Run();
                    Running = false;
                }
                else
                {
                    onMsg?.Invoke("窗口检测不通过");
                    if (mrfzGamePage.Hwnd != null && mrfzGamePage.Hwnd.ToInt32() != 0)
                    {
                        try
                        {
                            var bmp = Win.GetWindowClientCapture(mrfzGamePage.Hwnd);
                            bmp.Save(Environment.CurrentDirectory + "\\最后一张图片.png");
                            bmp.Dispose();
                            bmp = mrfzGamePage.CatptureImg();
                            bmp.Save(Environment.CurrentDirectory + "\\最后一张图片2.png");
                            bmp.Dispose();
                            onMsg?.Invoke("不能运行，请根据上述提示调整设置，如有疑问,可把本程序文件夹下最后一张图片.png和最后一张图片2.png发给作者(Q群)");
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        onMsg?.Invoke("窗口句柄为空");

                    }
                }
                Running = false;
            });
            mainThread.Start();
        }
        public class LOCK_THREAD
        {
            public string name;
        }
        public LOCK_THREAD LOCK_RUNNING = new LOCK_THREAD();
        public LOCK_THREAD LOCK_ScriptList = new LOCK_THREAD();
        public LOCK_THREAD LOCK_Project = new LOCK_THREAD();
        Queue<mrfz_ScriptUnit> ScriptList = new Queue<mrfz_ScriptUnit>();
        protected mrfz_ScriptUnit GetCurScriptUnit()
        {
            lock (LOCK_ScriptList)
            {
                if (ScriptList.Count > 0)
                    return ScriptList.Dequeue();
                else return null;
            }
        }
        protected void AddScriptUnit(mrfz_ScriptUnit unit)
        {
            lock (LOCK_ScriptList)
            {
                ScriptList.Enqueue(unit);
            }
        } /// <summary>
          /// 下次运行的类型
          /// </summary>
        public  mrfz_ScriptUnit.TaskType NextTaskType = mrfz_ScriptUnit.TaskType.Rogue_KuiYing;
        public void Run()
        {
            int Count = 0;
            while(true)
            {
                /*if(ScriptRunning)
                {
                    wait(1000);
                    continue;
                }
                */
                if(RunningUnit!=null&& RunningUnit.RunError)
                { 
                    onMsg?.Invoke($"运行中出错,停止脚本");
                    this.Stop();
                    return;
                }

                Count++;
                onMsg?.Invoke($"开始第{Count}次脚本");
                /**
           *  var ar= AntiUpdate();
              if(ar.IsUpdate&&ar.SuccessToHome)
              { 
                  this.HomeToRogue();
                  throw new ScriptException(true,false,"back to next unit");
              }
              else
              {
                  StopScript("凌晨更新,重回主界面失败");return;
              }
           * */
                mrfz_ScriptUnit unit = new mrfz_ScriptUnit(NextTaskType);
                if (mrfz_ScriptUnit.onMsg == null)
                {
                    mrfz_ScriptUnit.onMsg += (o) =>
                  {
                      this.onMsg?.Invoke(o);
                  };
                }
                unit.scriptMachine = this;
                RunningUnit = unit; 
                unit.Run(); 
                System.Threading.Thread.Sleep(300);
                if (ScriptConfig.RunCount == -1)
                {
                    continue;
                }
                if (Count >= ScriptConfig.RunCount)
                {
                    break;
                }
            }

            onMsg?.Invoke($"结束脚本");
        }

        
        protected void wait(int ms)
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

        public void Stop()
        {
            this.RunningUnit?.Stop();
            
            onMsg?.Invoke("终止运行脚本");
            this.RunningUnit = null; 
            Running = false;
            this.mainThread?.Abort(); 
        }
    }
}
