using lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GamePageScript.lib;
using GamePageScript.script.mrfz;
using MRFZ_Auto.script.mrfz;
using script;

namespace GamePageScript.ui
{
    public partial class ShotCutImgsForm : Form
    {
        public ShotCutImgsForm()
        {
            InitializeComponent();
            tb_imgpath.Text= Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器";
            //mrfz_ScriptUnit.GameWinName = "雷电模拟器";
            adb_list.SetData(null, null);
            
           //  CheckGameWindow();
        }

      
        
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckGameWindow(); 
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory+ "\\imgs\\adb_雷电模拟器"); 
            if (!di.Exists) di.Create();
            Hook.startListen((o, ee) => {
                if (ee.KeyCode.Equals(Keys.F12))
                {
                      
                    var Len = di.GetFiles("*.png").Length;
                    var FileName = di.FullName + "\\" + Len + ".png";
                   var BMP= mrfzGamePage.CatptureImg();
                    if (BMP == null) return;
                    BMP.Save(FileName);
                    BMP.Dispose();
                   // adb.ShotCut(FileName); 
                }
            });  
        }

        private void button4_Click(object sender, EventArgs e)
        {
             
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  mrfz_ScriptUnit.GameWinName = "明日方舟 - MuMu模拟器";
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器");
            if (!di.Exists) di.Create();
            Hook.startListen((o, ee) => {
                if (ee.KeyCode.Equals(Keys.F12))
                {

                    var Len = di.GetFiles("*.png").Length;
                    var FileName = di.FullName + "\\" + Len + ".png";
                    var BMP = mrfzGamePage.CatptureImg();
                    BMP.Save(FileName);
                    BMP.Dispose();
                    // adb.ShotCut(FileName);

                }
            });
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var NodeIndex = Convert.ToInt32(numericUpDown1.Value);
            var list = BranchNode.GetCurBranchNodes(NodeIndex, 1, mrfzGamePage.CatptureImg());
            return;
        }
        public Boolean IsPageRight
        {
            get
            {
                var IsKUIYING_Home =
                        mrfzGamePage.WhatPage(new string[] { "rogue-kuiying-home", "home" }, 4000) != null;
                return IsKUIYING_Home;
            }
        }
        public bool CheckGameWindow(String GameWinName="雷电模拟器")
        {
            Boolean CanRunScript = false;
            IntPtr GPR = IntPtr.Zero;
            if (GameWinName.StartsWith("雷电模拟器"))
            {
                GPR = Win.FindWindow(GameWinName, "LDPlayerMainFrame");
            }
            else if (GameWinName == ("明日方舟 - MuMu模拟器"))
            {
                GPR = Win.FindWindow(GameWinName, "Qt5QWindowIcon");
            }
            else
            {
         //    onMsg?.Invoke("不是雷电模拟器或者MUMU模拟器，雷电模拟器要求模拟器名以雷电模拟器开头");
                return CanRunScript;
            }
            mrfzGamePage.Hwnd = GPR;
            GamePage.Hwnd = GPR;
            Win.SetWindowPOS(mrfzGamePage.Hwnd, 0, 0);
            waitSec(1);
            Win.SetForegroundWindow(GPR);
            waitSec(1);
            if (GPR == null || GPR.ToInt32() == 0)
            {
              //  onMsg?.Invoke("请先启动雷电模拟器,或MUMU模拟器进入肉鸽准备界面后再启动本程序");
                return CanRunScript;
            }
            Rectangle rect = new Rectangle();
            WinLib.GetWindowRect(GPR, out rect);
            var SW = Screen.PrimaryScreen.Bounds.Width;
            var SH = Screen.PrimaryScreen.Bounds.Height;
            //  Boolean IsKUIYING_Home = false;
            if (GameWinName.StartsWith("雷电模拟器"))
            {
                if (rect.Width == 1322 && rect.Height == 756)
                {
                    return true;

                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(GPR, 0, 0, 1282, 756);
                        wait(500); 
                        if (IsPageRight)
                        {
                            return true;
                        }
                        else
                        {

                       //     onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                     //       onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                            return false;
                        }
                    }
                }
                else if (rect.Width == 1282 && rect.Height == 756)
                {

                    return true;
                    if (IsPageRight)
                    {
                        return true;
                    }
                    else
                    {
                        Win.SetWindowSize(GPR, 0, 0, 1322, 756);
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

                             //   onMsg?.Invoke("屏幕过小, 最低1366*768");
                            }
                            else
                            {

                             //   onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
//onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");

                            }
                            return false;
                        }
                    }
                }
                else
                {
                    Win.SetWindowSize(GPR, 0, 0, 1282, 756);
                    wait(500); 
                    if (IsPageRight)
                    {
                        return true;
                    }
                    return true;
                    Win.SetWindowSize(GPR, 0, 0, 1322, 756);
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

                           // onMsg?.Invoke("屏幕过小, 最低1366*768");
                        }
                        else
                        {

                         //   onMsg?.Invoke("不能启动,可能分辨率未设置正确,模拟器要求1280*720分辨率，设置完后需要重启模拟器(电脑的分辨率以及文本缩放100%改完后可能需要重启电脑才生效)");
                          //  onMsg?.Invoke("雷电模拟器可能需要收起右侧工具栏");
                        }
                        return false;
                    }
                }


            }
            else
            {
                bool small_screen = (SW < 1400 || SH < 900);
                if (small_screen)
                {

                //    onMsg?.Invoke("屏幕过小,该分辨率不支持MUMU，建议用雷电模拟器");
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
//onMsg?.Invoke("MUMU模拟器 窗口大小不正确,不要手动放大，缩小(设置1280*720后重启模拟器即可)");
                        return false;
                    }
                    //NOTHINGS
                }
            }
        }

        private void wait(int v)
        {
            System.Threading.Thread.Sleep(v);
        }

        private void waitSec(int v)
        {
            System.Threading.Thread.Sleep(v*1000);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            CheckGameWindow();
            mrfzGamePage.Hwnd = adb_list.ADB_ITEM.WIF.Handle;
            // mrfz_ScriptUnit.GameWinName = "雷电模拟器";
         //   ImageSave.SaveShopGold(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\164.png"));
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器");
            if (!di.Exists) di.Create();
            Hook.startListen((o, ee) => {
                if (ee.KeyCode.Equals(Keys.Space))
                {
                    ImageSave.SaveShopGold(null);
                   // ImageSave.SaveFrinedImgs();
                    // adb.ShotCut(FileName);

                }
            });
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ImageSave.SaveShopGoldFont();
        }
    }
}
