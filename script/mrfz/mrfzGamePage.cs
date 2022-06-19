using GamePageScript.lib;
using lib;
using lib.image;
using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MRFZ_Auto.ui.Contorls.ADB_Form_List;

namespace GamePageScript.script.mrfz
{
    public class mrfzGamePage : GamePage
    {
        public PageType pageType = PageType.肉鸽_愧影;
        public enum PageType
        {
            肉鸽_愧影,
            常规
        }
        public PageCatgory pageCatgory = PageCatgory.控制流程;
        public String MapName = "";
        public Boolean 紧急 = false;
        public MapType mapType = MapType.初始;
        public enum MapType
        {
            初始,
            放置,
            技能,
        }
        public enum PageCatgory
        {
            控制流程,
            地图识别,
            战斗地图,
        }
        public mrfzGamePage() { }
       
        public static new Dictionary<string, mrfzGamePage> GamePageDic;
        public static new List<mrfzGamePage> gamePages = new List<mrfzGamePage>();
        public mrfzGamePage(int index, String ImgFile, Size ImgSize) : base(index, ImgFile, ImgSize)
        {
        }

       
        static mrfzGamePage()
        {
            SaveFileName = Environment.CurrentDirectory + "\\config\\gplist_mrfz.json";
            Load();
            mrfzGamePage.GamePageDic = new Dictionary<string, mrfzGamePage>();
            foreach (var gp in gamePages)
            {
                if (!gp.Baned && gp.Name != "")
                {
                    mrfzGamePage.GamePageDic[gp.Name] = gp;
                }
            }
            SH = Screen.PrimaryScreen.Bounds.Height; //1080
            SW = Screen.PrimaryScreen.Bounds.Width; //1920
            UpdatePageName = "update_0";
            if (!GamePageDic.ContainsKey(UpdatePageName))
                UpdatePageName = null;
        }
        static int SW;
        static int SH;
        static String UpdatePageName;
        static List<String> UpdatePageNameList = new List<string>();
        public static new void Load()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\config");
            if (!di.Exists) di.Create();
            if (!File.Exists(SaveFileName)) return;
            StreamReader sr = File.OpenText(SaveFileName);
            var text = sr.ReadToEnd();
            gamePages = jss.Deserialize<List<mrfzGamePage>>(text);
            sr.Close(); sr.Dispose();
        }
        public static new void Save()
        {
            FileStream FS = new FileStream(SaveFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(FS, Encoding.UTF8);
            sw.Write(jss.Serialize(gamePages));
            sw.Close();
            sw.Dispose();
        }
        public Boolean IsRegionRight(int index, out double AvgDelta)
        {
            AvgDelta = -1;
            if (index > regions.Count)
            {
                return false;
            }
            Bitmap bmp = null; 
            try
            {

                bmp = CatptureImg();
              //  var X = new Random().Next(10000, 1000000);
               // FileName = Environment.CurrentDirectory + $"\\imgs\\runtime\\{X}.png";
                // bmp.Save(FileName);
                //  bmp = adb.ShotCutRunning(FileName);

                if (bmp == null || bmp.Size.Width != this.ImgSize.Width || ImgSize.Height != bmp.Size.Height)
                {
                    throw new Exception("size error");
                }
                var ic = ImageColor.FromBitmap(bmp);
                var rc = this.regions[index];
                Bitmap rcbmp = new Bitmap(Environment.CurrentDirectory + @"\imgs\region_imgs" + rc.FileName);
                var rcbmp_ic = ImageColor.FromBitmap(rcbmp);
                var sum_delt = 0d;
                for (int x = 0; x < rc.rect.Width; x++)
                    for (int y = 0; y < rc.rect.Height; y++)
                    {
                        var ori = ic[x + rc.rect.X, y + rc.rect.Y];
                        var dst = rcbmp_ic[x, y];
                        var dr = dst.R - ori.R;
                        var dg = dst.G - ori.G;
                        var db = dst.B - ori.B;
                        var delt = Math.Sqrt(3*dr * dr + 4*dg * dg +2* db * db)/3d/255d*100d;
                        sum_delt += delt;
                    }
                AvgDelta = sum_delt /( rc.rect.Width * rc.rect.Height);
                rcbmp.Dispose(); 

                if (AvgDelta > mrfz_ScriptConfig.scriptConfig.dlt_region)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                bmp?.Dispose();
            }

        }
        public override bool CheckPage(int time_ms = 2000, Boolean Once = false, Bitmap curBmp = null)
        {
            var st = DateTime.Now;
            while (true)
            {

                if (IsCurPage(curBmp))
                {
                    return true;
                }
                if (Once) return false;
                if ((DateTime.Now - st).TotalMilliseconds > time_ms)
                {
                    return false;
                }
                else
                    System.Threading.Thread.Sleep(50);
            }
        }
        public static IntPtr GetHwndFromName(String Name)
        {
            IntPtr GPR = IntPtr.Zero;
            if (Name.StartsWith("雷电模拟器"))
            {
                GPR = Win.FindWindow(Name, "LDPlayerMainFrame");
            }
            else if (Name == ("明日方舟 - MuMu模拟器"))
            {
                GPR = Win.FindWindow(Name, "Qt5QWindowIcon");
            }else if(Name=="夜神模拟器")
            {

                GPR = Win.FindWindow(Name, "Qt5QWindowIcon");
            }
            return GPR;
            //MuMu模拟器
        }
       // static Boolean Test1366 = true;
        public static Bitmap CatptureImg(int index=0)
        {
            if(mrfz_ScriptConfig.scriptConfig.UseADBCat)
            {
               return adb.ShotCut(index);
            }else
            {
                if (Hwnd == null || Hwnd.ToInt32() == 0)
                {
                    Hwnd = mrfz_ScriptMachine.adb_item.WIF.Handle;
                    if (Hwnd == null || Hwnd.ToInt32() == 0)
                   return null;
                }

                Win.ShowWindow(Hwnd);
                if (mrfz_ScriptMachine.adb_item.adb_type== ADB_LIST_ITEM.ADB_TYPE.LD)
                { 
                    var bmp = Win.GetWindowClientCapture(Hwnd, 1, 34, 1280, 720); 
                    return bmp;
                }
                else if(mrfz_ScriptMachine.adb_item.adb_type ==  ADB_LIST_ITEM.ADB_TYPE.YS)
                {

                    var bmp = Win.GetWindowClientCapture(Hwnd, 2, 32, 1280, 720);  
                    return bmp;
                }
                else if(mrfz_ScriptMachine.adb_item.adb_type ==  ADB_LIST_ITEM.ADB_TYPE.MUMU)
                {

                    //var bmp = Win.GetWindowClientCapture(Hwnd, 1, 34, 1280, 720);
                    var bmp = Win.GetWindowClientCapture(Hwnd, 0, 36, 1280, 720);
                    return bmp;
                }else
                {
                    return null;
                }
            }
            // return adb.ShotCutRunning();
            
        }


        protected static List<String> LargerDeltaPage = new List<string>() {
        "rogue-kuiying-home-sel", "rogue-kuiying-home-normal","rogue-kuiying-home-nosel"
        };
        public static Boolean IsUpdatePage
        {
            get
            {
                if (UpdatePageName != null && GamePageDic.ContainsKey(UpdatePageName))
                {
                    return GamePageDic[UpdatePageName].IsCurPage();
                }
                else
                {
                    return false;
                }

            }
        }

        public override Boolean IsCurPage(Bitmap curBmp = null )
        {
            Bitmap bmp = null;
            try
            {

                if (curBmp != null)
                    bmp = curBmp;
                else
                {
                    bmp = CatptureImg();
                }
                if (bmp == null || bmp.Size.Width != this.ImgSize.Width || ImgSize.Height != bmp.Size.Height)
                {
                    throw new Exception("size error");
                }
                Boolean REC = true;
                var ic = ImageColor.FromBitmap(bmp);
                foreach (var pp in RecPagePPs)
                {
                    var cl = ic[pp.loc.X, pp.loc.Y];// bmp.GetPixel(pp.loc.X, pp.loc.Y);
                    if (cl.R != pp.color.R)
                    {
                        REC = false;
                        break;
                    }
                    if (cl.G != pp.color.G)
                    {
                        REC = false;
                        break;
                    }
                    if (cl.B != pp.color.B)
                    {
                        REC = false;
                        break;
                    }
                }

                //计算差值
                foreach (var rc in this.RecPageRectColors)
                {
                    Bitmap rcbmp = new Bitmap(Environment.CurrentDirectory + @"\imgs\rcimg" + rc.FileName);
                    var rcbmp_ic = ImageColor.FromBitmap(rcbmp);
                    var sum_delt = 0d;
                    for (int x = 0; x < rc.rect.Width; x++)
                        for (int y = 0; y < rc.rect.Height; y++)
                        {
                            var ori = ic[x + rc.rect.X, y + rc.rect.Y];
                            var dst = rcbmp_ic[x, y];
                            var dr = dst.R - ori.R;
                            var dg = dst.G - ori.G;
                            var db = dst.B - ori.B;
                            var delt = Math.Sqrt(3*dr * dr + 4*dg * dg + 2*db * db)/3d/255d*100;
                            sum_delt += delt;
                        }
                    var avg = sum_delt / (rc.rect.Width * rc.rect.Height);
                    rcbmp.Dispose();
                   /* if (LargerDeltaPage.Contains(this.Name))
                    {
                        if (avg > delt_max_Larger)
                        {
                            REC = false;
                            break;
                        }
                    }
                    else
                   */
                    {
                       // if (dlt == -1)
                        {
                            if (avg > mrfz_ScriptConfig.scriptConfig.dlt_page_check)
                            {
                                REC = false;
                                break;
                            }
                        }
                      //  else
                       // {
                          //  if (avg > dlt)
                          //  {
                             //   REC = false;
                            //    break;
                           // }
                       // }
                    }

                }
                var SaveLog = false;
                if (SaveLog && !REC)
                {
                    Log.SaveErrorLog(this, bmp);
                    //SAVE LOG
                }
                return REC;
            }
            catch (Exception EX)
            {

                return false;
            }
            finally
            {
                if (curBmp != null)
                {

                }
                else
                {
                    bmp?.Dispose();
                    bmp = null;

                }

            }
        }
        public  Boolean IsCurPage(ImageColor[,] ic  )
        { 
            try
            {

                
                Boolean REC = true; 
                foreach (var pp in RecPagePPs)
                {
                    var cl = ic[pp.loc.X, pp.loc.Y];// bmp.GetPixel(pp.loc.X, pp.loc.Y);
                    if (cl.R != pp.color.R)
                    {
                        REC = false;
                        break;
                    }
                    if (cl.G != pp.color.G)
                    {
                        REC = false;
                        break;
                    }
                    if (cl.B != pp.color.B)
                    {
                        REC = false;
                        break;
                    }
                }

                //计算差值
                foreach (var rc in this.RecPageRectColors)
                {
                    Bitmap rcbmp = new Bitmap(Environment.CurrentDirectory + @"\imgs\rcimg" + rc.FileName);
                    var rcbmp_ic = ImageColor.FromBitmap(rcbmp);
                    var sum_delt = 0d;
                    for (int x = 0; x < rc.rect.Width; x++)
                        for (int y = 0; y < rc.rect.Height; y++)
                        {
                            var ori = ic[x + rc.rect.X, y + rc.rect.Y];
                            var dst = rcbmp_ic[x, y];
                            var dr = dst.R - ori.R;
                            var dg = dst.G - ori.G;
                            var db = dst.B - ori.B;
                            var delt = Math.Sqrt(3 * dr * dr + 4 * dg * dg + 2 * db * db) / 3d / 255d * 100;
                            sum_delt += delt;
                        }
                    var avg = sum_delt / (rc.rect.Width * rc.rect.Height);
                    rcbmp.Dispose();
                    /* if (LargerDeltaPage.Contains(this.Name))
                     {
                         if (avg > delt_max_Larger)
                         {
                             REC = false;
                             break;
                         }
                     }
                     else
                    */
                    {
                        // if (dlt == -1)
                        {
                            if (avg > mrfz_ScriptConfig.scriptConfig.dlt_page_check)
                            {
                                REC = false;
                                break;
                            }
                        }
                        //  else
                        // {
                        //  if (avg > dlt)
                        //  {
                        //   REC = false;
                        //    break;
                        // }
                        // }
                    }

                }
                var SaveLog = false;
                 
                return REC;
            }
            catch (Exception EX)
            {

                return false;
            }
            finally
            {
                 

            }
        }
        public class Thread_Page_Judge
        {
            public String[] pagesNames; 
            public ThreadObject[] ThreadObjects;
            public String PageName;
            public Boolean RunCompelete = false;
            protected DateTime dstTime;
            public int CompelteThreadCount = 0;
             
            int TO; 
            public Thread_Page_Judge(String[] pagesNames, int TO)
            {
                this.pagesNames = pagesNames; 
                ThreadObjects = new ThreadObject[pagesNames.Length];
                this.TO = TO; 
            }
            public class ThreadObject
            {
                public String PageName;
                public Boolean RunCompelete = false;
                public DateTime dstTime;
                double dlt = 20;
                public Boolean IsCurpage;
                public void Run()
                {
                    Thread th=new Thread(()=>{
                        try
                        {
                            while (!RunCompelete)
                            {
                                if (DateTime.Now > dstTime)
                                {
                                    RunCompelete = true;
                                    return;
                                }


                                if (GamePageDic.ContainsKey(PageName))
                                {
                                    if (GamePageDic[PageName].IsCurPage())
                                    {
                                        RunCompelete = true;
                                        IsCurpage = true;
                                        return;
                                    }
                                }

                                System.Threading.Thread.Sleep(200);
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {

                        }
                    });
                    th.Start();
                     
                }
            }
            public String RunToGetPageName()
            { 

                dstTime = DateTime.Now.AddMilliseconds(TO);
                for (int i = 0; i < pagesNames.Length; i++)
                { 
                    var dstName = pagesNames[i];
                    ThreadObjects[i] = new ThreadObject()
                    {
                        PageName = dstName,
                         RunCompelete=false,
                        dstTime= dstTime,
                    };
                    ThreadObjects[i].Run(); 
                   
                }
                while(true)
                {
                    int CompelteThreadCount = 0;
                    foreach (var v in ThreadObjects)
                    {
                        if(v.RunCompelete)
                        {
                            CompelteThreadCount++;
                            if(v.IsCurpage)
                            {
                                this.PageName = v.PageName;
                                this.RunCompelete = true;
                                break; 
                            }
                        } 
                    }
                    if(DateTime.Now> dstTime)
                    {
                        return null;
                    }
                    if(RunCompelete||this.CompelteThreadCount >= pagesNames.Length)
                    {
                        return this.PageName; 
                    }else
                    {
                        Thread.Sleep(200);
                    }
                } 
            }
        }
        public static String WhatPageOnThread(String[] pagesNames, int TO)
        {  
            //bmp = CatptureImg(); 
            Thread_Page_Judge ths = new Thread_Page_Judge(pagesNames, TO);
            return ths.RunToGetPageName(); 
        }
   

        public static String WhatPage(String[] pagesNames,int TO)
        {
            Bitmap bmp = null;
            try
            {
                //bmp = CatptureImg();
                var dstTime = DateTime.Now.AddMilliseconds(TO);
                while (true)
                {
                    if (DateTime.Now > dstTime)
                    {
                        return null;
                    }
                    foreach (var p in pagesNames)
                    {
                        if (GamePageDic.ContainsKey(p))
                        {
                            if (GamePageDic[p].IsCurPage())
                            {
                                return p;
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }catch
            {
                return null;
            }
            finally
            {
                bmp?.Dispose();
            }
        }
        public static String WhatPage(String[] pagesNames, Bitmap bmp)
        {
            
            try
            {
                //bmp = CatptureImg(); 
                
                {
                    var ic = ImageColor.FromBitmap(bmp);
                    foreach (var p in pagesNames)
                    {
                        if (GamePageDic.ContainsKey(p))
                        {
                            
                            if (GamePageDic[p].IsCurPage(ic))
                            {
                                return p;
                            }
                        }
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                bmp?.Dispose();
            }
        }
        // int delt_max = 18;
        // int delt_max_Larger = 20;


        public static bool PageClick(String PageName, String ClickName, Boolean Valid = false)
        {
            if (Valid)
            {
                if (GamePageDic[PageName].IsCurPage())
                {
                    var CNP = SearchClickName(GamePageDic[PageName].NextPages, ClickName);
                    if (CNP == null) { { throw new Exception($" 找不到{PageName} {ClickName}点击数据"); } }
                    ClickPage(CNP);
                    Thread.Sleep(200);
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
                if (CNP == null) { { 
                        throw new Exception($" 找不到{PageName} {ClickName}点击数据");
                    } }
                ClickPage(CNP);
                System.Threading.Thread.Sleep(200);
                //onMsg?.Invoke($"page:{PageName} clickname:{ClickName}");
                return true;
            }


        }
        public static ClickToNextPage SearchClickName(List<ClickToNextPage> list, String Name)
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
        public static void ClickPage(ClickToNextPage cnp)
        {
            adb.Tap(cnp.ClickPoint);
            Thread.Sleep(100);
        }
    }
    
}
