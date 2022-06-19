using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Win32;

namespace lib
{

      class WinLib
    {

        [DllImport("user32.dll")]
        public static extern int MessageBeep(uint uType);
         [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(
            IntPtr hWnd,//窗口句柄
            StringBuilder lpString,//标题
            int nMaxCount //最大值
            ); 
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        uint beepI = 0x00000030;
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName); 
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
 
         
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);


        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point p);

         

        public static uint SWP_SHOWWINDOW = 64;
        public static uint SWP_NOMOVE = 2;
        public static uint SWP_NOSIZE = 1;
        public static IntPtr HWND_TOP = IntPtr.Zero;
        public static int WM_MOUSEMOVE = 512;
        public static int WM_LBUTTONDOWN = 513;
        public static int WM_LBUTTONUP = 514;
        public static int WM_RBUTTONDOWN = 0x204;
        public static int WM_RBUTTONUP = 0x205;
        public static int WM_MOUSEWHEEL = 522;
           public static int WM_ACTIVATE = 6; 
        public static int WA_ACTIVE = 1;
        public static int MK_LBUTTON = 1;
        public static int MK_RBUTTON = 2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter">0=</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx">W</param>
        /// <param name="cy">H</param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        
        public static   UInt32 MAKELONG(UInt32 cx, UInt32 cy)
        {
            return cx | (cy<<16);
        }



        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, string lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, UInt32 lParam);

        

        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto ,EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, UInt32 lParam);

        public static void PostMessageSafe(IntPtr hWnd, int msg, int wParam, UInt32 lParam)
        {
            bool returnValue = PostMessage(hWnd, msg, wParam, lParam);
            if (!returnValue)
            { 
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        #region GetWindowCapture的dll引用
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );
        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(
        IntPtr hwnd
        );
        [DllImport("user32.dll")]
        public static extern IntPtr GetClientDC(
        IntPtr hwnd
        );
        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DeviceContextValues flags);
        //HRGN 
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeedes, <c>false</c> otherwise. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);
        #endregion
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
       public static extern bool IsWindow(IntPtr hWnd);
       [ DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
         public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        #region bVk参数 常量定义
       

            #endregion
        
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)] 

        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);
        public const int KEYEVENTF_KEYUP = 2;

        public static void KeyBD(byte MainKey, byte secKey,Boolean combie)
        {
            if(combie)
            {
                 
                keybd_event(MainKey, 0, 0, 0);
                keybd_event(secKey, 0, 0, 0);
                keybd_event(MainKey, 0, KEYEVENTF_KEYUP, 0);
                keybd_event(secKey, 0, KEYEVENTF_KEYUP, 0);
            }
            else
            {

                keybd_event(MainKey, 0, 0, 0);
            } 
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        //取得前台窗口句柄函数
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        //取得桌面窗口句柄函数
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        
          [DllImport("user32.dll")]
          public static extern int SetCursorPos(int x, int y);
        //取得Shell窗口句柄函数
        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();
        //取得窗口大小函数
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowRect(IntPtr hwnd, out RECT rc);
         
        //取得窗口大小函数 
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static void ShowWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, 1);
        }
        public static void MouseMoveAndClick(int x,int y)
        {
            SetCursorPos(x,y);
          //   mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE,- x,- y, 0, 0);
         //   mouse_event( MOUSEEVENTF_MOVE, x, y, 0, 0);
          //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        public static bool IsFullScreen(IntPtr hWnd)
        {
            //桌面窗口句柄
          //  IntPtr desktopHandle; //Window handle for the desktop
                                  //Shell窗口句柄
         //   IntPtr shellHandle; //Window handle for the shell

            //        因为桌面窗口和Shell窗口也是全屏，要排除在其他全屏程序之外。

            //取得桌面和Shell窗口句柄
          //  desktopHandle = GetDesktopWindow();
           // shellHandle = GetShellWindow(); 
            //取得前台窗口句柄并判断是否全屏
            bool runningFullScreen = false;
            RECT appBounds;
            Rectangle screenBounds; 
            //取得前台窗口 
            if (hWnd != null && !hWnd.Equals(IntPtr.Zero))
            {
                //判断是否桌面或shell 
                {
                    //取得窗口大小
                    GetWindowRect(hWnd, out appBounds);
                    //判断是否全屏
                    screenBounds = Screen.FromHandle(hWnd).Bounds;
                    if ((appBounds.Bottom - appBounds.Top) == screenBounds.Height && (appBounds.Right - appBounds.Left) == screenBounds.Width)
                        runningFullScreen = true;
                }
            }
            return runningFullScreen;
        }

    }


    public class Win
    {
        public static List<WindowInformation> GetWinList()
        {
            List<WindowInformation> list = new List<WindowInformation>();
            var PT1 = WinLib.FindWindow("LDPlayerMainFrame", null);
            while(PT1!=null&&PT1.ToInt32()!=0)
            {
                var Tttle=GetWindowTitle(PT1);
                list.Add(new WindowInformation() { Caption = Tttle, Class = "LDPlayerMainFrame", Handle = PT1 });
                PT1 = WinLib.FindWindowEx(IntPtr.Zero, PT1, "LDPlayerMainFrame", null);
            }

          //  var list_YS = winlist.Where(wif => wif.Caption.Contains("夜神模拟器")
           //     && wif.Class == "Qt5QWindowIcon").ToList();
           // var list_MUMU = winlist.Where(wif => (wif.Caption.Contains("明日方舟 - MuMu模拟器")
          //  || wif.Caption.Contains("MuMu模拟器")) && wif.Class == "Qt5QWindowIcon").ToList();

          var  PT2 = WinLib.FindWindow("Qt5QWindowIcon", null);
            while (PT2 != null && PT2.ToInt32() != 0)
            {
                var Tttle = GetWindowTitle(PT2);
                if(Tttle.Contains("夜神模拟器"))
                {

                }else if(Tttle.Contains("MuMu模拟器"))
                {

                }else
                {
                    IntPtr child = IntPtr.Zero;
                    while (true)
                    {

                        child = WinLib.FindWindowEx(PT2, child, "Qt5QWindowIcon", null);
                        if (child == null || child.ToInt32() == 0)
                        {
                            break;
                        }
                        Tttle = GetWindowTitle(child);
                        if (Tttle.Contains("夜神模拟器"))
                        {
                            list.Add(new WindowInformation() { Caption = Tttle, Class = "Qt5QWindowIcon", Handle = child });
                        }
                        else if (Tttle.Contains("MuMu模拟器"))
                        {
                            list.Add(new WindowInformation() { Caption = Tttle, Class = "Qt5QWindowIcon", Handle = child });
                        }
                        else
                        {
                            continue;
                        }
                    }

                    PT2=WinLib.FindWindowEx(IntPtr.Zero, PT2, "Qt5QWindowIcon",null);
                    continue;
                }
                list.Add(new WindowInformation() { Caption = Tttle, Class = "Qt5QWindowIcon", Handle = PT2 });
                PT2 = WinLib.FindWindowEx(IntPtr.Zero, PT2, "Qt5QWindowIcon", null);
            }
            return list;
        }
        public static void ShowWindow(IntPtr hWnd)
        {
            WinLib.ShowWindow(hWnd);
        }
        public static void MouseClick(IntPtr hwnd,int x, int y,int y_add=32)
        {
            y += y_add;
            Point p = new Point();
            WinLib.GetCursorPos(ref p);
            WinLib.SetCursorPos(x,y);
            //WinLib.MouseMoveAndClick(x, y);
            LeftClick(hwnd, x, y); 
        }
        public static bool IsFullScreen(IntPtr hWnd)
        {
            return WinLib.IsFullScreen(hWnd);
        }
        public static   bool SetForegroundWindow(IntPtr hWnd)
        {
          //  ShowWindow(hWnd);
            return WinLib.SetForegroundWindow(hWnd.ToInt32());
        }
        public static void KeyBD(byte MainKey, byte secKey, Boolean combie)
        {
            WinLib.KeyBD(MainKey,   secKey,   combie);
        }
        public static void Beep()
        { 
            uint beepI = 0x00000030;

            //发出不同类型的声音的参数如下：  
            //Ok = 0x00000000,  
            //Error = 0x00000010,  
            //Question = 0x00000020,  
            //Warning = 0x00000030,  
            //Information = 0x00000040  
            WinLib.MessageBeep(0x00000010);
        }
        public static IntPtr FindWindow(String name,String className=null)
        {
            return WinLib.FindWindow(className, name);
        }

        public static IntPtr BindHwndByMousePoint()
        {
            Point p = new Point();
            if (WinLib.GetCursorPos(ref p))
            {
                return WinLib.WindowFromPoint(p);
            }
            else
                return IntPtr.Zero;
        }

        public static IntPtr BindHwndByMousePoint(Point p)
        { 
            if (WinLib.GetCursorPos(ref p))
            {
                return WinLib.WindowFromPoint(p);
            }
            else
                return IntPtr.Zero;
        }

        public static String GetWindowTitle(IntPtr hwnd)
        { 
                StringBuilder sb = new StringBuilder();
            if (WinLib.IsWindow(hwnd))
            {
                WinLib.GetWindowText(hwnd, sb, 99999);
                return sb.ToString();
            }
            else
                return "NO WINDOW";
        }
        public static String GetWindowClassName(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder();
            
           
                WinLib.GetClassName(hwnd, sb, 99999);
                return sb.ToString();
           
            
        }
        public static void SetWindowSize(IntPtr hwnd,int x, int y,int w,int h)
        {
            WinLib.SetWindowPos(hwnd, WinLib.HWND_TOP, x, y, w, h, WinLib.SWP_SHOWWINDOW  );
        }
        public static void SetWindowPOS(IntPtr hwnd, int x, int y)
        {
            WinLib.SetWindowPos(hwnd, WinLib.HWND_TOP, x, y, 1280, 720, WinLib.SWP_SHOWWINDOW| WinLib.SWP_NOSIZE);
        }
        /* public static void ShowWindow(IntPtr hwnd)
         {
             Rectangle rec = new Rectangle();
             WinLib.GetWindowRect(hwnd, out rec);

             WinLib.SetWindowPos(hwnd, WinLib.HWND_TOP, rec.X, rec.Y, rec.Width, rec.Height, WinLib.SWP_SHOWWINDOW|WinLib.SWP_NOMOVE);
         }
        */
        //子线程内可调用的
        //普通按钮项
        public static void LeftClick(IntPtr hwnd, int client_pos_x, int client_pos_y,int press_ms=0)
        { 
           UInt32 tmp = WinLib.MAKELONG((UInt32)client_pos_x, (UInt32)client_pos_y);
            if(press_ms!=0)
            {
                WinLib.SendMessage(hwnd, WinLib.WM_ACTIVATE, WinLib.WA_ACTIVE, 0);
                WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                Thread.Sleep(press_ms);
                WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
            else
            {
                WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
        }
        public static void LeftClick_Async(IntPtr hwnd, int client_pos_x, int client_pos_y, int press_ms = 0)
        {
            UInt32 tmp = WinLib.MAKELONG((UInt32)client_pos_x, (UInt32)client_pos_y);
            if (press_ms != 0)
            {
                WinLib.SendMessage(hwnd, WinLib.WM_ACTIVATE, WinLib.WA_ACTIVE, 0);
                WinLib.PostMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                Thread.Sleep(press_ms);
                WinLib.PostMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
            else
            {
                WinLib.PostMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                WinLib.PostMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
        }

        //子线程内可调用的
        //普通按钮项
        public static void RightClick(IntPtr hwnd, int client_pos_x, int client_pos_y, int press_ms = 0)
        {
            UInt32 tmp = WinLib.MAKELONG((UInt32)client_pos_x, (UInt32)client_pos_y);
            if (press_ms != 0)
            {
                WinLib.SendMessage(hwnd, WinLib.WM_ACTIVATE, WinLib.WA_ACTIVE, 0);
                WinLib.PostMessage(hwnd, WinLib.WM_RBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                Thread.Sleep(press_ms);
                WinLib.PostMessage(hwnd, WinLib.WM_RBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
            else
            {
                WinLib.SendMessage(hwnd, WinLib.WM_RBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                WinLib.SendMessage(hwnd, WinLib.WM_RBUTTONUP, WinLib.MK_LBUTTON, tmp);
            }
        }

        //MOUSE CLICK WIN32API 应该在主线程中调用
        //多线程用异步委托调用以让API在主线程中调用
        //使用SAFE的POSTMESSAGE，收集错误信息
        public static void LeftClickSafe(IntPtr hwnd, int client_pos_x, int client_pos_y, int press_ms = 0)
        {
            UInt32 tmp = WinLib.MAKELONG((UInt32)client_pos_x, (UInt32)client_pos_y);
            WinLib.PostMessageSafe(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
            WinLib.PostMessageSafe(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
        }
        //线程安全的
        public static void RightClickSafe(IntPtr hwnd, int client_pos_x, int client_pos_y, int press_ms = 0)
        {
            UInt32 tmp = WinLib.MAKELONG((UInt32)client_pos_x, (UInt32)client_pos_y);
            WinLib.PostMessageSafe(hwnd, WinLib.WM_RBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
            WinLib.PostMessageSafe(hwnd, WinLib.WM_RBUTTONUP, WinLib.MK_LBUTTON, tmp);
        }


        public static void WHEEL(IntPtr hwnd,Point px)
        {
            UInt32 tmp = WinLib.MAKELONG((UInt32)px.X, (UInt32)px.Y);
            WinLib.SendMessage(hwnd, WinLib.WM_MOUSEWHEEL, 120, tmp);
        }
        public static List<PointF> Drag(IntPtr hwnd,Point[] Line, int time_ms=500)
        {
            List<PointF> pfs = new List<PointF>();
            for (int i = 0; i < Line.Length; i++)
            {
                if (i == 0)
                {
                    pfs.Add(new PointF(Line[0].X, Line[0].Y));
                }
                else
                {
                    float cell_num = 20.0f;
                    var dx = (Line[i].X - Line[i - 1].X) / cell_num;
                    var dy = (Line[i].Y - Line[i - 1].Y) / cell_num;

                    for (int J = 0; J < cell_num; J++)
                    {
                        var p_x = dx * J + Line[i-1].X;
                        var p_y = dy * J + Line[i-1].Y;
                        pfs.Add(new PointF(p_x, p_y));
                    }
                }
                
            }

            //0 
            UInt32 tmp0 = WinLib.MAKELONG((UInt32)pfs[0].X, (UInt32)pfs[0].Y);
            WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp0);
            Thread.Sleep(15);
            for (int i=1; i<pfs.Count-1;i++)
            {
                UInt32 tmp = WinLib.MAKELONG((UInt32)pfs[i].X, (UInt32)pfs[i].Y);
                WinLib.SendMessage(hwnd, WinLib.WM_MOUSEMOVE, WinLib.MK_LBUTTON, tmp);
                Thread.Sleep(15);
            }
            Thread.Sleep(450);
            UInt32 tmpED = WinLib.MAKELONG((UInt32)pfs[pfs.Count - 1].X, (UInt32)pfs[pfs.Count - 1].Y);
            WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmpED);
            return pfs;
            /*
            for (int i = 0; i < Line.Length; i++)
            {
                UInt32 tmp = WinLib.MAKELONG((UInt32)Line[i].X, (UInt32)Line[i].Y);
                if (i == 0)
                {
                    WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONDOWN, WinLib.MK_LBUTTON, tmp);
                    Thread.Sleep(40);
                }
                else  
                {
                    if (i == Line.Length - 1)
                    {
                        Thread.Sleep(50);
                        WinLib.SendMessage(hwnd, WinLib.WM_LBUTTONUP, WinLib.MK_LBUTTON, tmp);
                        break;
                    }


                    //Thread.Sleep(40);
                    //INT SUB POINTS =10 NUM
                    float cell_num = 20.0f;
                    var dx = (Line[i].X - Line[i - 1].X)/ cell_num;
                    var dy = (Line[i].Y - Line[i - 1].Y)/ cell_num;

                    for(int J=0;J<cell_num ;J++)
                    {
                        var p_x = dx * J + Line[i].X;
                        var p_y = dy * J + Line[i].Y;
                        UInt32 ptmp = WinLib.MAKELONG((UInt32)p_x, (UInt32)p_y);
                        WinLib.SendMessage(hwnd, WinLib.WM_MOUSEMOVE, WinLib.MK_LBUTTON, ptmp);
                        Thread.Sleep(15);

                    }
                     
                }
                
            }*/
        }

        //对后台窗口的客户区截图
        public static Bitmap GetWindowClientCapture(IntPtr hWnd,int x=0,int y=0,int w=-1,int h=-1)
        {
            try
            {
                IntPtr hscrdc = WinLib.GetDC(hWnd);
                Rectangle clientRect = new Rectangle();
                Rectangle windowRect = new Rectangle();
                WinLib.GetClientRect(hWnd, out clientRect);
                WinLib.GetWindowRect(hWnd, out windowRect); 
                Point pf = new Point();
                WinLib.ClientToScreen(hWnd, ref pf);
                int startX = pf.X - windowRect.X;
                int startY = pf.Y - windowRect.Y;
                //客户区相对RECT
                Rectangle ClientRect = new Rectangle(startX, startY, clientRect.Width, clientRect.Height);

                Rectangle outputRect = new Rectangle(x, y, w, h);
                /*if (w > clientRect.Width || h > clientRect.Height)
                {
                    Rectangle srcRect= new Rectangle(startX, startY, clientRect.Width, clientRect.Height);
                    IntPtr hmemdc = WinLib.CreateCompatibleDC(hscrdc);
                    //创建和HWND句柄窗口DC 设备兼容的内存缓冲位图-完整窗口的大小
                    IntPtr hbitmap = WinLib.CreateCompatibleBitmap(hscrdc, windowRect.Width - windowRect.X, windowRect.Height - windowRect.Y);
                    //绑定输出 位图和缓冲DC
                    WinLib.SelectObject(hmemdc, hbitmap);
                    //打印窗口到内存DC
                    WinLib.PrintWindow(hWnd, hmemdc, 0);
                    //由一个指定设备的句柄创建一个新的Graphics对象
                    Bitmap srcbitmap = new Bitmap(srcRect.Width, srcRect.Height);
                    //输出Graphic
                    Graphics g2 = Graphics.FromImage(srcbitmap);
                    //输出DC
                    var outputhdc = g2.GetHdc();
                    //裁剪 SRCCOPY
                    WinLib.BitBlt(outputhdc, 0, 0, outputRect.Width, outputRect.Height, hmemdc, outputRect.X, outputRect.Y, TernaryRasterOperations.SRCCOPY);
                    WinLib.DeleteDC(hscrdc);//删除用过的对象
                    WinLib.DeleteDC(hmemdc);//删除用过的对象
                    WinLib.DeleteDC(outputhdc);//删除用过的对象
                    WinLib.DeleteObject(hbitmap);//删除用过的对象
                    g2.Dispose();
                    Bitmap output = new Bitmap(srcbitmap, w, h);
                    return output;
                }
                */
             //   else
                {
                    if (w == -1)
                    {
                        outputRect.Width = clientRect.Width - x;
                    }
                    if (h == -1)
                    {
                        outputRect.Height = clientRect.Height - y;
                    }
                    outputRect.X += ClientRect.X;
                    outputRect.Y += ClientRect.Y;
                    //创建和HWND句柄窗口DC 设备兼容的内存缓冲DC
                    IntPtr hmemdc = WinLib.CreateCompatibleDC(hscrdc);
                    //创建和HWND句柄窗口DC 设备兼容的内存缓冲位图-完整窗口的大小
                    IntPtr hbitmap = WinLib.CreateCompatibleBitmap(hscrdc, 
                        windowRect.Width - windowRect.X, 
                        windowRect.Height - windowRect.Y);
                    //绑定输出 位图和缓冲DC
                    WinLib.SelectObject(hmemdc, hbitmap);
                    //打印窗口到内存DC
                    WinLib.PrintWindow(hWnd, hmemdc, 0);
                    //由一个指定设备的句柄创建一个新的Graphics对象
                    Bitmap outputImg = new Bitmap(outputRect.Width, outputRect.Height);
                    //输出Graphic
                    Graphics g2 = Graphics.FromImage(outputImg);
                    //输出DC
                    var outputhdc = g2.GetHdc();
                    //裁剪 SRCCOPY
                    WinLib.BitBlt(outputhdc, 0, 0, outputRect.Width, outputRect.Height,
                        hmemdc, outputRect.X, outputRect.Y, 
                        TernaryRasterOperations.SRCCOPY);
                    WinLib.DeleteDC(hscrdc);//删除用过的对象
                    WinLib.DeleteDC(hmemdc);//删除用过的对象
                    WinLib.DeleteDC(outputhdc);//删除用过的对象
                    WinLib.DeleteObject(hbitmap);//删除用过的对象
                    g2.Dispose();
                    return outputImg;
                }
            }catch
            {
                return null;
            }finally
            {

            }
        }

    }

    public static class KEY
    {


        public const byte vbKeyLButton = 0x1;    // 鼠标左键
        public const byte vbKeyRButton = 0x2;    // 鼠标右键
        public const byte vbKeyCancel = 0x3;     // CANCEL 键
        public const byte vbKeyMButton = 0x4;    // 鼠标中键
        public const byte vbKeyBack = 0x8;       // BACKSPACE 键
        public const byte vbKeyTab = 0x9;        // TAB 键
        public const byte vbKeyClear = 0xC;      // CLEAR 键
        public const byte vbKeyEnter = 0xD;     // ENTER 键
        public const byte vbKeyShift = 0x10;     // SHIFT 键
        public const byte vbKeyControl = 0x11;   // CTRL 键
        public const byte vbKeyAlt = 18;         // Alt 键  (键码18)
        public const byte vbKeyMenu = 0x12;      // MENU 键
        public const byte vbKeyPause = 0x13;     // PAUSE 键
        public const byte vbKeyCapital = 0x14;   // CAPS LOCK 键
        public const byte vbKeyEscape = 0x1B;    // ESC 键
        public const byte vbKeySpace = 0x20;     // SPACEBAR 键
        public const byte vbKeyPageUp = 0x21;    // PAGE UP 键
        public const byte vbKeyEnd = 0x23;       // End 键
        public const byte vbKeyHome = 0x24;      // HOME 键
        public const byte vbKeyLeft = 0x25;      // LEFT ARROW 键
        public const byte vbKeyUp = 0x26;        // UP ARROW 键
        public const byte vbKeyRight = 0x27;     // RIGHT ARROW 键
        public const byte vbKeyDown = 0x28;      // DOWN ARROW 键
        public const byte vbKeySelect = 0x29;    // Select 键
        public const byte vbKeyPrint = 0x2A;     // PRINT SCREEN 键
        public const byte vbKeyExecute = 0x2B;   // EXECUTE 键
        public const byte vbKeySnapshot = 0x2C;  // SNAPSHOT 键
        public const byte vbKeyDelete = 0x2E;    // Delete 键
        public const byte vbKeyHelp = 0x2F;      // HELP 键
        public const byte vbKeyNumlock = 0x90;   // NUM LOCK 键

        //常用键 字母键A到Z
        public const byte vbKeyA = 65;
        public const byte vbKeyB = 66;
        public const byte vbKeyC = 67;
        public const byte vbKeyD = 68;
        public const byte vbKeyE = 69;
        public const byte vbKeyF = 70;
        public const byte vbKeyG = 71;
        public const byte vbKeyH = 72;
        public const byte vbKeyI = 73;
        public const byte vbKeyJ = 74;
        public const byte vbKeyK = 75;
        public const byte vbKeyL = 76;
        public const byte vbKeyM = 77;
        public const byte vbKeyN = 78;
        public const byte vbKeyO = 79;
        public const byte vbKeyP = 80;
        public const byte vbKeyQ = 81;
        public const byte vbKeyR = 82;
        public const byte vbKeyS = 83;
        public const byte vbKeyT = 84;
        public const byte vbKeyU = 85;
        public const byte vbKeyV = 86;
        public const byte vbKeyW = 87;
        public const byte vbKeyX = 88;
        public const byte vbKeyY = 89;
        public const byte vbKeyZ = 90;

        //数字键盘0到9
        public const byte vbKey0 = 48;    // 0 键
        public const byte vbKey1 = 49;    // 1 键
        public const byte vbKey2 = 50;    // 2 键
        public const byte vbKey3 = 51;    // 3 键
        public const byte vbKey4 = 52;    // 4 键
        public const byte vbKey5 = 53;    // 5 键
        public const byte vbKey6 = 54;    // 6 键
        public const byte vbKey7 = 55;    // 7 键
        public const byte vbKey8 = 56;    // 8 键
        public const byte vbKey9 = 57;    // 9 键


        public const byte vbKeyNumpad0 = 0x60;    //0 键
        public const byte vbKeyNumpad1 = 0x61;    //1 键
        public const byte vbKeyNumpad2 = 0x62;    //2 键
        public const byte vbKeyNumpad3 = 0x63;    //3 键
        public const byte vbKeyNumpad4 = 0x64;    //4 键
        public const byte vbKeyNumpad5 = 0x65;    //5 键
        public const byte vbKeyNumpad6 = 0x66;    //6 键
        public const byte vbKeyNumpad7 = 0x67;    //7 键
        public const byte vbKeyNumpad8 = 0x68;    //8 键
        public const byte vbKeyNumpad9 = 0x69;    //9 键
        public const byte vbKeyMultiply = 0x6A;   // MULTIPLICATIONSIGN(*)键
        public const byte vbKeyAdd = 0x6B;        // PLUS SIGN(+) 键
        public const byte vbKeySeparator = 0x6C;  // ENTER 键
        public const byte vbKeySubtract = 0x6D;   // MINUS SIGN(-) 键
        public const byte vbKeyDecimal = 0x6E;    // DECIMAL POINT(.) 键
        public const byte vbKeyDivide = 0x6F;     // DIVISION SIGN(/) 键


        //F1到F12按键
        public const byte vbKeyF1 = 0x70;   //F1 键
        public const byte vbKeyF2 = 0x71;   //F2 键
        public const byte vbKeyF3 = 0x72;   //F3 键
        public const byte vbKeyF4 = 0x73;   //F4 键
        public const byte vbKeyF5 = 0x74;   //F5 键
        public const byte vbKeyF6 = 0x75;   //F6 键
        public const byte vbKeyF7 = 0x76;   //F7 键
        public const byte vbKeyF8 = 0x77;   //F8 键
        public const byte vbKeyF9 = 0x78;   //F9 键
        public const byte vbKeyF10 = 0x79;  //F10 键
        public const byte vbKeyF11 = 0x7A;  //F11 键
        public const byte vbKeyF12 = 0x7B;  //F12 键
    }
}
