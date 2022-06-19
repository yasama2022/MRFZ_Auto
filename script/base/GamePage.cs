using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web.Script.Serialization;
using System.IO; 
using lib;
using lib.image;
using GamePageScript.script.mrfz;
using MRFZ_Auto.script;

namespace script
{
    public class GamePage
    {
        public String ImgFile;
        public GamePage(int index,String ImgFile, Size ImgSize)
        {
            this.Index = index;
            this.ImgFile = ImgFile;
            this.ImgSize = ImgSize;
        }
        public GamePage() { }
        public int Index;
        public String Name="";
        public Size ImgSize;
        public Boolean Baned = false;
        /// <summary>
        /// 本页面的特征点 信息
        /// </summary>
        public List<PP> RecPagePPs = new List<PP>();
        public List<RectColor> RecPageRectColors = new List<RectColor>();
        public List<RectColor> regions = new List<RectColor>(); 
        public Dictionary<String,RectColor> GetRegions()
        {
            Dictionary<String, RectColor> dic = new Dictionary<string, RectColor>();
            foreach(var r in regions)
            {
                dic.Add(r.Name, r);
            }
            return dic;
        }
        /// <summary>
        /// 点击跳转下一页
        /// </summary>
        public List<ClickToNextPage> NextPages = new List<ClickToNextPage>();
        public static List<GamePage> gamePages = new List<GamePage>();
        public static Dictionary<string, GamePage> GamePageDic;
        public static String SaveFileName = Environment.CurrentDirectory + "\\config\\gplist.json";
        public static JavaScriptSerializer jss = new JavaScriptSerializer();
        public static IntPtr Hwnd;

        public virtual bool CheckPage(int time_ms=2000,Boolean Once=false,Bitmap curbmp=null)
        {
            throw new NotImplementedException("NO");
            var st=DateTime.Now;
            while(true)
            {
                if((DateTime.Now-st).TotalMilliseconds>time_ms)
                {
                    return false; 
                }
                if (IsCurPage()) return true;
                if (Once) return false;
                System.Threading.Thread.Sleep(50);
            }
        }
        public virtual Boolean IsCurPage(Bitmap curBmp = null
            )
        {
            throw new NotImplementedException("NO");
            Bitmap bmp = null;
            try
            {
                bmp = Win.GetWindowClientCapture(Hwnd);
                if (bmp == null || bmp.Size.Width != this.ImgSize.Width || ImgSize.Height != bmp.Size.Height)
                {
                    throw new Exception("size error");
                }
                var ic = ImageColor.FromBitmap(bmp);
                Boolean REC = true;
                foreach (var pp in RecPagePPs)
                {
                    var cl = bmp.GetPixel(pp.loc.X, pp.loc.Y);
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
                            var delt = Math.Sqrt(3*dr * dr + 4*dg * dg +2* db * db)/3d/255d*100;
                            sum_delt += delt;
                        }
                    var avg = sum_delt /( rc.rect.Width * rc.rect.Height);
                    rcbmp.Dispose();
                    
                    {
                       
                        {
                            if (avg > delt_max)
                            {
                                REC = false;
                                break;
                            }
                        }
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
        private static int  delt_max=12;
       
        static GamePage()
        {
            Load();
            GamePageDic = new Dictionary<string, GamePage>();
            foreach (var gp in GamePage.gamePages)
            {
                if (!gp.Baned && gp.Name != "")
                {
                    GamePageDic[gp.Name] = gp;
                }
            }
        }
        public   static  void Load()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\config");
            if (!di.Exists) di.Create();
            if (!File.Exists(SaveFileName)) return;
            StreamReader sr = File.OpenText(SaveFileName);
            var text = sr.ReadToEnd();
            gamePages = jss.Deserialize<List<GamePage>>(text);
            sr.Close(); sr.Dispose();
        }
        public   static void Save()
        {
            FileStream FS = new FileStream(SaveFileName,FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(FS, Encoding.UTF8); 
            sw.Write(jss.Serialize(gamePages));
            sw.Close();
            sw.Dispose();
        } 
        public override string ToString()
        {
            return Index.ToString() + ":" + Name;
        }
    }
    /// <summary>
    /// Point and pixiv
    /// </summary>
    public class PP
    {
        public Point loc;
        public ImageColor color;
         [System.Web.Script.Serialization.ScriptIgnore]
        public String Name { get { return ToString(); } }
        public override string ToString()
        {
            return loc.ToString() + "->{" + color.R+',' + color.G +','+ color.B+'}';
        }
    }
     
    /// <summary>
    /// 点击跳转下一页
    /// </summary>
    public class ClickToNextPage
    {
        public Point ClickPoint;
        public String ClickName = "";
        public List<int> NextPages = new List<int>();
        [System.Web.Script.Serialization.ScriptIgnore]
        public String Name { get { return ToString(); } }
        public override string ToString()
        {
            return ClickName+':'+ClickPoint.ToString()  ;
        }
    }
}
