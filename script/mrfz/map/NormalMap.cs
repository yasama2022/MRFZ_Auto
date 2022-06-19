using MRFZ_Auto.script.mrfz.map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;
namespace MRFZ_Auto.script
{
    public class NormalMap:map
    {
        /// <summary>
        /// key=mapname
        /// </summary>
        public static Dictionary<String, NormalMap> Maps = new Dictionary<string, NormalMap>();
        /// <summary>
        /// 高地 地形
        /// </summary>
        public Dictionary<String, RectColor> HighLand = new Dictionary<string, RectColor>();
        /// <summary>
        /// 地面 地形
        /// </summary>
        public Dictionary<String, RectColor> LowLand = new Dictionary<string, RectColor>();
        /// <summary>
        /// 用于判断状态的区域---不用于判断宝箱
        /// </summary>
        public Dictionary<String, RectColor> StateRegion = new Dictionary<string, RectColor>();
        /// <summary>
        /// 高地 地形 点击 点
        /// </summary>
        public Dictionary<String, Point> HighLandPoints = new Dictionary<string, Point>();
        /// <summary>
        /// 地面 地形 点击 点
        /// </summary>
        public Dictionary<String, Point> LowLandPoints = new Dictionary<string, Point>();

        public Boolean IsState(String state)
        {
            //return StateRegion.ContainsKey(state)? StateRegion[state].IsRegionRight()
            return false;
        }
        public NormalMap(List<String> ImgFiles):base( ImgFiles)
        {

        }
        public NormalMap() { }
        static NormalMap()
        {
            DirectoryInfo di=new DirectoryInfo( Environment.CurrentDirectory + "\\config\\map");
            if (!di.Exists) { di.Create();return; }
            var fn = Environment.CurrentDirectory + "\\config\\map\\normal.json";
            if (File.Exists(fn))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Close();fs.Dispose();fs = null;
                Maps = jss.Deserialize<Dictionary<String, NormalMap>>(Encoding.UTF8.GetString(bs));
                bs = null;
            }else
            {
                Maps = new Dictionary<string, NormalMap>();
            }
            DirectoryInfo di_map = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\map");
            if (!di_map.Exists) di_map.Create();
             var dis=di_map.GetDirectories();
            foreach (var v in Maps)
            {
                List<string> dellist = new List<string>();
                foreach (var img in v.Value.ImgFiles)
                {
                    if (!File.Exists(Environment.CurrentDirectory + "\\imgs\\map\\" + img))
                    {
                        dellist.Add(img);
                    }
                }
                foreach (var del in dellist)
                {
                    v.Value.ImgFiles.Remove(del);
                }
            }
            foreach(var d in dis)
            {
               var MapName= d.Name;
                if(!Maps.ContainsKey(MapName))
                {
                    Maps.Add(MapName, new NormalMap() {  Name=MapName});
                }
                DirectoryInfo ndi = new DirectoryInfo(d.FullName+ "\\normal");
                var fss = ndi.GetFiles("*.png");
                foreach(var fs in fss)
                {
                    if(Maps[MapName].ImgFiles.Contains(fs.Name))
                    {

                    }else
                    {
                        Maps[MapName].ImgFiles.Add(fs.Name);
                    }
                }
            }
            Save();
        }
        public static void Save()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\config\\map");
            if (!di.Exists) { di.Create();  }
            var fn = Environment.CurrentDirectory + "\\config\\map\\normal.json";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
            var text=jss.Serialize(Maps);
            byte[] bs = Encoding.UTF8.GetBytes(text);
            fs.Write(bs, 0, bs.Length);
            fs.Close(); fs.Dispose(); fs = null; bs = null;

        }
    }
}
