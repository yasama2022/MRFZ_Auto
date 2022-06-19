using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
namespace script
{
    public class Log
    {
        public  static String LogFile = "";
        public static String LastErrorImgFile = "";
        static Log()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\log\\imgs");
            if (!di.Exists) di.Create();
            DirectoryInfo di2 = new DirectoryInfo(Environment.CurrentDirectory + "\\log\\logtxt");
            if (!di2.Exists) di2.Create();
            var len_log=di2.GetFiles("*.json").Length;
            LogFile = Environment.CurrentDirectory + $"\\log\\logtxt\\{len_log}.json";
            var len=di.GetFiles("*.png").Length;
            LastErrorImgFile = Environment.CurrentDirectory + $"\\log\\imgs\\{len}.png";

        }
        public class LogText
        {
            public GamePage gamePage;
            public String ErrorBitMap;
            public LogText() { }
            public LogText(GamePage gamePage, String ErrorBitMap) { this.gamePage = gamePage;this.ErrorBitMap = ErrorBitMap; }

        }
        public static void SaveErrorLog(GamePage gamePage, Bitmap bitmap)
        {
           // bitmap.Save(LastErrorImgFile);
            return;
            FileStream fs = new FileStream(LogFile, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            LogText lt = null;
            try
            {
                if (bitmap != null)
                {
                    bitmap.Save(LastErrorImgFile);
                    lt = new LogText(gamePage, LastErrorImgFile);
                }
                else
                {

                    lt = new LogText(gamePage, "NO PIC");
                }
            }catch
            {


                lt = new LogText(gamePage, "NO PIC");
            } 
            JavaScriptSerializer JSS = new JavaScriptSerializer();
            sw.WriteLine(JSS.Serialize(lt));
            sw.Close();
            fs.Close();
        }
    }
}
