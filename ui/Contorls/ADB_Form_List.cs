using GamePageScript.lib;
using GamePageScript.script.mrfz;
using script;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32;
using lib;
namespace MRFZ_Auto.ui.Contorls
{
    public partial class ADB_Form_List : UserControl
    {
        public delegate void oninit_connect();
        oninit_connect init_connect;
        int retry = 5; 
        public delegate void onEV_WIN(List<WindowInformation> winlist, List<String> list);
        onEV_WIN EV_WIN;

        public List<ADB_LIST_ITEM> List_ADB = new List<ADB_LIST_ITEM>();
        public class ADB_LIST_ITEM
        {
            public WindowInformation WIF;
            public String Device;
            public Boolean isEmulator;
            public ADB_TYPE adb_type;
            public enum ADB_TYPE
            {
                LD,
                MUMU,
                YS
            }
            public override string ToString()
            {

                return $"{WIF.Caption}({WIF.Handle.ToInt32()}) {Device}";
            }
        }
        public delegate void OnMsg(String msg);
        public OnMsg adb_on_msg;
        public ADB_LIST_ITEM ADB_ITEM { get
            {
                if(List_ADB.Count>0&&listbox_adb.SelectedIndex< List_ADB.Count)
                {
                    return List_ADB[listbox_adb.SelectedIndex];
                }else
                {
                    return null;
                }
            } }
        public ADB_Form_List()
        { 
            InitializeComponent(); 
        }
        List<TextBox> tb_win;

        public void SetData(List<OnMsg> onMsgs,List<TextBox> tb_wins)
        {
            this.tb_win = tb_wins;
            init_connect += () =>
            {
                if (List_ADB.Count > 0)
                {
                    listbox_adb.SelectedIndex = 0;
                }
            };
            adb_on_msg += (x) => {
                if(onMsgs!=null)
                foreach(var onmsg in onMsgs)
                {

                    onmsg?.Invoke(x);
                } 
                };
            listbox_adb.SelectedIndexChanged += Listbox_adb_SelectedIndexChanged;
            EV_WIN += (winlist,adblist) =>
            {
               
                this.List_ADB.Clear();
                var list_LD = winlist.Where(wif => wif.Class == "LDPlayerMainFrame").ToList();
                var list_YS = winlist.Where(wif => wif.Caption.Contains("夜神模拟器")
                && wif.Class == "Qt5QWindowIcon").ToList();
                var list_MUMU = winlist.Where(wif => (wif.Caption.Contains("明日方舟 - MuMu模拟器")
                || wif.Caption.Contains("MuMu模拟器")) && wif.Class == "Qt5QWindowIcon").ToList();

                Boolean hasMuMu = list_MUMU.Count > 0;// ptr_mumu != null && ptr_mumu.ToInt32() != 0;
                Boolean hasYeshen = list_YS.Count > 0;// ptr_yeshen != null && ptr_yeshen.ToInt32() != 0;


                Boolean hasLeiDian = list_LD.Count > 0;// ptr_leidian != null && ptr_leidian.ToInt32() != 0;

                //  if (adblist == null || adblist.Count == 0)

                if ((hasMuMu) && !adblist.Contains("127.0.0.1:7555"))
                {
                    adb_on_msg?.Invoke("未检测到MUMU模拟器");
                    adb_on_msg?.Invoke("检测到MUMU模拟器窗口有在运行,尝试连接");
                    adb.connectMUMU("127.0.0.1:7555");
                    adblist = adb.devices();

                }
                if (hasYeshen && (!adblist.Contains("127.0.0.1:62001") && !adblist.Contains("127.0.0.1:52001")))
                {
                    adb_on_msg?.Invoke("未检测到夜神模拟器");
                    adb_on_msg?.Invoke("检测到夜神模拟器窗口有在运行,尝试连接");
                    adb.connectYESHEN("127.0.0.1:62001");
                    adb.connectYESHEN("127.0.0.1:52001"); 
                    adblist = adb.devices();
                }
                    
           
                foreach (var de in adblist)
                {
                    if (de.StartsWith("emulator"))
                    {
                        if (hasLeiDian)
                        {
                            foreach (var LD in list_LD)
                            {
                                List_ADB.Add(new ADB_LIST_ITEM()
                                {
                                    WIF = LD,
                                    Device = de,
                                    isEmulator = true,
                                    adb_type = ADB_LIST_ITEM.ADB_TYPE.LD
                                });
                            }
                        }
                    }
                    else if (de.StartsWith("127.0.0.1:7555"))
                    {
                        if (hasMuMu)
                        {
                            foreach (var mumu in list_MUMU)
                            {
                                List_ADB.Add(new ADB_LIST_ITEM()
                                {
                                    WIF = mumu,
                                    Device = de,
                                    isEmulator = true,

                                    adb_type = ADB_LIST_ITEM.ADB_TYPE.MUMU
                                });
                            }
                            // tem_devices.Add(de);
                        }
                    }
                    else //if(de.StartsWith("127.0.0.1:62001"))
                    {
                        if (hasYeshen)
                        {
                            foreach (var ys in list_YS)
                            {
                                List_ADB.Add(new ADB_LIST_ITEM()
                                {
                                    WIF = ys,
                                    Device = de,
                                    isEmulator = true,

                                    adb_type = ADB_LIST_ITEM.ADB_TYPE.YS
                                });
                            }
                        }
                    }
                }
                listbox_adb.DataSource = null;
                listbox_adb.DataSource = this.List_ADB;
                // Thread th = new Thread(() =>
                //   {
                if (List_ADB.Count > 0)
                {
                    try
                    {
                        // List_ADB[0].Device
                        adb.connect(List_ADB[0].Device);
                        if (tb_win != null)
                            foreach(var tb in tb_win)
                            {

                                tb.Text = List_ADB[0].WIF.Caption;
                            }
                        this.BeginInvoke(init_connect);
                        this.BeginInvoke(adb_on_msg, "adb连接完毕");
                    }
                    catch (Exception EX)
                    {
                        adb_on_msg?.Invoke("adb连接错误," + EX.ToString() + "\r\n" + EX.Message);
                    }
                }
            };
           
            Thread th = new Thread(() =>
              {
                  while (!this.IsHandleCreated)
                  {
                      Thread.Sleep(500);
                  }
                  try
                  {
                      this.BeginInvoke(adb_on_msg, "读取模拟器/窗口列表中...,请等待");
                      var t1 = DateTime.Now;
                      adb.init();
                      var T = (DateTime.Now - t1).TotalSeconds;
                      var list = adb.devices();
                      List<WindowInformation> windowListBasic = Win.GetWinList();// WindowList.GetAllWindows();
                   //   var list = adb.devices();
                      this.BeginInvoke(EV_WIN, windowListBasic, list);

                    //  this.BeginInvoke(EV_INITMNQ, list); 
                  }
                  catch (Exception ex)
                  {
                     this.BeginInvoke(adb_on_msg, ex.Message + "\r\n" + ex.ToString() + "\r\n" + ex.StackTrace);
                  }
              });
            th.Start();
        }

        private void Listbox_adb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (List_ADB.Count <= 0) return;
            if (listbox_adb.SelectedIndex < 0) return;
            if (List_ADB.Count > listbox_adb.SelectedIndex)
            {
                mrfzGamePage.Hwnd = List_ADB[listbox_adb.SelectedIndex].WIF.Handle;
                GamePage.Hwnd = List_ADB[listbox_adb.SelectedIndex].WIF.Handle;
                mrfz_ScriptMachine.adb_item = List_ADB[listbox_adb.SelectedIndex];
                adb.connect(List_ADB[listbox_adb.SelectedIndex].Device);
                if (tb_win != null)
                {
                    foreach(var tb in tb_win)
                    tb.Text = List_ADB[listbox_adb.SelectedIndex].WIF.Caption;
                }

                this.BeginInvoke(adb_on_msg, "切换模拟器/窗口成功");
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(adb_on_msg, "刷新模拟器/窗口列表,请等待"); 
              retry = 5;
            List_ADB.Clear();
            listbox_adb.DataSource = null;
            listbox_adb.DataSource = List_ADB; 
            
            Thread th = new Thread(() =>
            { 
                adb.init();
                // var PT1 = WinLib.FindWindow("LDPlayerMainFrame", null);
                //  var intprt = WinLib.FindWindowEx(IntPtr.Zero,PT1,"LDPlayerMainFrame", null);
                List<WindowInformation> windowListBasic = Win.GetWinList();// WindowList.GetAllWindows();
                if(windowListBasic==null|| windowListBasic.Count==0)
                {
                  // var intprt= WinLib.FindWindow("LDPlayerMainFrame", null);
                    //win11 兼容
                  //  var list_LD = winlist.Where(wif => wif.Class == "LDPlayerMainFrame").ToList();
                  //  var list_YS = winlist.Where(wif => wif.Caption.Contains("夜神模拟器")
                  //  && wif.Class == "Qt5QWindowIcon").ToList();
                 //   var list_MUMU = winlist.Where(wif => (wif.Caption.Contains("明日方舟 - MuMu模拟器")
                 //   || wif.Caption.Contains("MuMu模拟器")) && wif.Class == "Qt5QWindowIcon").ToList();
                }
                var list = adb.devices();
                this.BeginInvoke(EV_WIN, windowListBasic, list);
            });
            th.Start();
        }
    }
}
