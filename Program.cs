using MRFZ_Auto.lib.image;
using MRFZ_Auto.script.mrfz;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script.mrfz.map;
using MRFZ_Auto.script.mrfz.shop;
using MRFZ_Auto.ui;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static MRFZ_Auto.lib.image.CIE_DE_2000;

namespace MRFZ_Auto
{
    static class Program
    {
        static APP_T appType;
      public  static Boolean Debug = false;
        enum APP_T
        { 
            IMGSCUT,
            GAMEPAGECREATOR,
            MRFZ,
            MapEdit,
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.CurrentDirectory.EndsWith("Debug"))
            {
                Debug = true;
                double delta = 0;
                //  ImageSave.SaveShopItemName_Gray();
                // return;
               
              //  ImageSave.SaveShopItem(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\146.png"));
              // ImageSave.SaveShopItem(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\146.png"));
              //  return;
              // var GOLD2 = shop_gold.RecCurGolds(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\62.png"));

                //var GOLD = shop_gold.RecCurGolds(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\146.png"));

                //  return;
                // ImageSave.SaveShopGoldFont();return;
                // friend_char.FriendChars[ GamePageScript.script.mrfz.mrfz_ScriptConfig.ArkChar.山]
                //     .what_number_inpage(new Bitmap(@"D:\ailini.png"), out delta);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (HaveRunningInstance())
            {
                if (!Debug)
                {
                    var dr = MessageBox.Show("为防止恶意利用,本程序不允许多开,谢谢。点击'是'可以关闭本程序的其他进程,点击'否'本程序退出,", "提示", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
                        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
                        foreach (Process thisProc in processes)
                        {
                            if (thisProc.Id != current.Id)
                            {
                                // MessageBox.Show("关闭多余的程序");
                                thisProc.Kill(); //当发送关闭窗口命令无效时强行结束进程
                                thisProc.WaitForExit();
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            } 
            new BattleCharRec();
            new WinItem(); 
            var ms=PutCharMap.Maps;
            //  shop_gold.GrayN();
            // shop_gold.RecCurGolds(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\146.png"));
            ////.SaveShopGoldFont();

#if DEBUG


#endif
            appType = APP_T.MRFZ;
            switch (appType)
            {
                case APP_T.IMGSCUT:
                    Application.Run(new GamePageScript.ui.ShotCutImgsForm());
                    break;
                case APP_T.GAMEPAGECREATOR:
                    Application.Run(new GamePageScript.ui.GamePageCreator());
                    break;
                case APP_T.MRFZ:
                    Application.Run(new GamePageScript.ui.mrfzScriptWin());
                    break;
                case APP_T.MapEdit: 
                    Application.Run(new MapEditMainWin());
                    break;
                default:
                    break;
            }
        }


        #region 判断是否已经存在运行的实例

        /// <summary>
        /// 判断是否已经存在运行的实例
        /// </summary>
        /// <returns>存在返回true，不存在返回false</returns>
        public static bool HaveRunningInstance()
        {
            System.Diagnostics.Process current = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(current.ProcessName);
            if (processes.Length >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);
        
        private static bool closeProc(string processName)
        {
            bool result = false;

            foreach (System.Diagnostics.Process thisProc in System.Diagnostics.Process.GetProcessesByName(processName))
            {
                thisProc.Kill(); //当发送关闭窗口命令无效时强行结束进程
                thisProc.WaitForExit();
                result = true;

            }
            return result;
        }

    }
}
