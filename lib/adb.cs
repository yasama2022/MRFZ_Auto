using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;

namespace GamePageScript.lib
{
    public class adb
    { 
        static adb()
        {
            //init();
        }
        public static String dvName;
        public static void init()
        {
            startserver();
            //var list=devices();
          //  if(list.Count==1)
             //   connect(list[0]);
            //listapk();
            if(!Directory.Exists(Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器");
            }
           // ShotCut(Environment.CurrentDirectory+ "\\imgs\\adb_雷电模拟器\\cur.png");
           // Tap(new Point(1141,525));
        }
        public static void Tap(Point point)
        {
            //adb shell input tap 300 500
            exc($"shell input tap {point.X} {point.Y}");
        }
        public static void testMRFZ()
        {
            //1657,987 1131，646，
           // Tap(new Point(1657, 987));
           Swipe(new Point(1657, 987), new Point(1131,646), 2000);
          //  Swipe(new Point(1131, 646), new Point(1131+200, 646), 800);
        }
        public static void Drag(Point st,Point end,int ms)
        {
            //adb shell input draganddrop 100 1220 500 620 2000
            exc($"shell input draganddrop {st.X} {st.Y} {end.X} {end.Y} {ms}");
        }
        public static void Swipe(Point st, Point end, int ms)
        {
            //adb shell input draganddrop 100 1220 500 620 2000
            exc($"shell input swipe {st.X} {st.Y} {end.X} {end.Y} {ms}");
        }
        public static void Press(Point st, int ms)
        {
            //adb shell input draganddrop 100 1220 500 620 2000
            exc($"shell input swipe {st.X} {st.Y} {st.X} {st.Y} {ms}");
        }
        public static Bitmap ShotCutRunning(String FileName=null)
        {
            if(FileName==null)
              FileName = Environment.CurrentDirectory + "\\imgs\\cur.png";
            //adb shell rm /sdcard/text.txt
            // exc("shell rm -f /sdcard/cur_capture.png");
            String SRC_FILE = "/sdcard/cur_capture.png";
            exc($"shell screencap {SRC_FILE}");
            exc($"pull {SRC_FILE} {FileName}");
            Bitmap bmp = new Bitmap(FileName);
            return bmp;
        }
        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);

        static int sindex = 0;
        public static Bitmap ShotCut_cmd(int index = 0)
        {
            try
            {
                String cmd = "exec-out screencap -p ";
                if (useEmulator)
                {
                    cmd = $"-s {dvName} " + cmd;
                }
                Process CmdProcess = new Process();
                CmdProcess.StartInfo.FileName = Environment.CurrentDirectory + "\\adb\\" + @"adb.exe";//可执行程序路径
                CmdProcess.StartInfo.Arguments = cmd;//参数以空格分隔，如果某个参数为空，可以传入""
                CmdProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\adb";
                CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
                CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
                                                                    //     CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
               CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
                CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出
                                                                    //  var writer = CmdProcess.StandardInput;
                                                                    //  writer.WriteLine(cmd ); //向cmd窗口发送输入信息  
                                                                    //  writer.AutoFlush = true;  //提交  
                                                                    // CmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived;                                          //   writer.Flush();
                                                                    //  writer.Close();
                CmdProcess.Start();//执行    )
                var stream=CmdProcess.StandardOutput.BaseStream;
                byte[] buffer = new byte[1024];
                MemoryStream ms = new MemoryStream();
                while (true)
                {
                    int rlen=stream.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, rlen);
                    if(rlen<buffer.Length)
                    {
                        break;
                    }
                } 
                ms.Position = 0;
                stream.Close();
                CmdProcess.Close();
                return new Bitmap(ms);
                { ;
                    // var str=   CmdProcess.StandardOutput.ReadToEnd();
                    // CmdProcess.WaitForExit(); 
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            //  process.StandardOutput.BaseStream.
        }
        public static Bitmap ShotCut(int index=0)
        {
            return ShotCut_cmd(index);

            String FileName =Environment.CurrentDirectory+$"\\adb\\sc{sindex}.png";
            if(sindex > 0)
            {
                CreatADBCat_Bat(sindex);
            }
            var EXE_FILE = Environment.CurrentDirectory + "\\adb\\" + @"cat.bat";
            if (!useEmulator)
            {

            //    cmd = $"adb exec-out screencap -p > sc{index}.png\r\n";
                EXE_FILE = $"{Environment.CurrentDirectory}\\adb\\cat{sindex}_mumu.bat";
            }
            else
            {

            //    cmd = $"adb -s {dvName} exec-out screencap -p > sc{index}.png\r\n";
                EXE_FILE = $"{Environment.CurrentDirectory}\\adb\\cat{sindex}_leidian.bat";
            }
             

           var Path_Luacher = Path.GetDirectoryName(EXE_FILE);
          // exc_get_bmp($"exec-out screencap -p > {FileName}");
         //   return null;
         ShellExecute(IntPtr.Zero, new StringBuilder("Open"),
                    new StringBuilder(EXE_FILE ), new StringBuilder($""),
                   new StringBuilder(Path_Luacher), (int)ShowCommands.SW_HIDE);
            sindex++;
            if (File.Exists(FileName))
            {
                var src = new Bitmap(FileName);
                var dst=(Bitmap)src.Clone();
                src.Dispose();
                return dst;// new Bitmap(FileName);
            }else
            {

                return null;
            } 

        }
        public static List<String> listapk()
        { 
            List<String> list = new List<string>();
            var lins = exc($"shell pm list packages -3");
            lins = lins.Replace("\r\n", "\n").Replace("\r", "\n");
            var ls = lins.Split('\n');
            foreach(var l in ls)
            {
                if(l.Contains("package"))
                {
                    list.Add(l.Split(':')[1]);
                }
            }
            //com.hypergryph.arknights
            //com.hypergryph.arknights.bilibili
            return list;
        }
        public static List<String> devices()
        {
            //List of devices attached
            //emulator-5554	offline
            try
            {
                List<String> dvs = new List<string>();
                var lins = exc("devices ");
                lins = lins.Replace("\r\n", "\n").Replace("\r", "\n");
                var ls = lins.Split('\n');
                if (ls.Length > 1)
                {
                    for (int i = 1; i < ls.Length; i++)
                    {
                        var ln = ls[i];
                        if (ln == "") continue;
                        var devicename = ln.Split('\t')[0];
                        dvs.Add(devicename);
                    }
                }
                return dvs;
            }catch(Exception ex)
            {
                throw new Exception("获取设备错误");
            }
        }
        public static void startserver()
        {
            //FileInfo fi = new FileInfo(Environment.CurrentDirectory + "\\start.bat");
          //  if(fi.Exists)
            //{
                    
          //  }
            // exc("nodaemon server ",true);
             exc("start-server");
        }
        static Boolean useEmulator = false;
        private static void CreatADBCat_Bat(int index)
        {
            String cmd = "";
            String FileName = $"{Environment.CurrentDirectory}\\adb\\cat{index}.bat";
            if (!useEmulator)
            {

                  cmd = $"adb exec-out screencap -p > sc{index}.png\r\n";
                FileName = $"{Environment.CurrentDirectory}\\adb\\cat{index}_mumu.bat";
            }
            else
            {

                  cmd = $"adb -s {dvName} exec-out screencap -p > sc{index}.png\r\n";
                FileName = $"{Environment.CurrentDirectory}\\adb\\cat{index}_leidian.bat";
            }

            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            var bs = Encoding.ASCII.GetBytes(cmd);
            fs.Write(bs,0,bs.Length);
            fs.Close();

        }
        public static void connect(String item)
        {
            if(item.Contains(":"))
            {
                useEmulator = false;
                connectMUMU(item);
               // CreatADBCat_Bat( 0);

            }
            else
            {
                useEmulator = true;
                connectLeiDian(item);
               // CreatADBCat_Bat( 0);
            } 
        }
        public static void connectLeiDian(String name)
        { 
                dvName = name;
            

           // exc($"-s {dvName} remount");
            // -s emulator-5556 remount
        }
        public static void connectMUMU(String ip_port)
        {
            exc($"connect {ip_port}");
        }
        public static void connectYESHEN(String ip_port)
        {
            exc($"connect {ip_port}");
        }
        public static String exc(String cmd,Boolean thread=false)
        {
            if(useEmulator)
            {
                cmd = $"-s {dvName} " + cmd;
            }
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = Environment.CurrentDirectory + "\\adb\\"+ @"adb.exe";//可执行程序路径
            CmdProcess.StartInfo.Arguments = cmd;//参数以空格分隔，如果某个参数为空，可以传入""
            CmdProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\adb";
            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
            CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出
          //  var writer = CmdProcess.StandardInput;
          //  writer.WriteLine(cmd ); //向cmd窗口发送输入信息  
          //  writer.AutoFlush = true;  //提交  
         //   writer.Flush();
          //  writer.Close();
            CmdProcess.Start();//执行   
            String ret=CmdProcess.StandardOutput.ReadToEnd();//输出   
            if (thread)
            {
                new Thread(() =>
                {
                    CmdProcess.WaitForExit();//等待程序执行完退出进程   
                    CmdProcess.Close();//结束
                }).Start();
            }
            else
            {
                CmdProcess.WaitForExit();//等待程序执行完退出进程   
                CmdProcess.Close();//结束
            }
            return ret;
        }

        public static void exc_get_bmp(String cmd, Boolean thread = false)
        {
            try
            {
                if (useEmulator)
                {
                    cmd = $"-s {dvName} " + cmd;
                }
                Process CmdProcess = new Process();
                CmdProcess.StartInfo.FileName = Environment.CurrentDirectory + "\\adb\\" + @"adb.exe";//可执行程序路径
                CmdProcess.StartInfo.Arguments = cmd;//参数以空格分隔，如果某个参数为空，可以传入""
                CmdProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\adb";
                CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口    
                CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程  
           //     CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入    
             //   CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出    
             //   CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出
                                                                    //  var writer = CmdProcess.StandardInput;
                                                                    //  writer.WriteLine(cmd ); //向cmd窗口发送输入信息  
                                                                    //  writer.AutoFlush = true;  //提交  
            // CmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived;                                          //   writer.Flush();
                                                                    //  writer.Close();
                CmdProcess.Start();//执行    )
                {
                    CmdProcess.Close();
                    // var str=   CmdProcess.StandardOutput.ReadToEnd();
                   // CmdProcess.WaitForExit(); 
                }
            }catch(Exception ex)
            {
                ;
            }
        }

        private static void CmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {  
        }
    }
}
