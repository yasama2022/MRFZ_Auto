using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MRFZ_Auto.script.mrfz.map
{
   public class PutCharMap:map
    {/// <summary>
     /// key=mapname
     /// </summary>
        public static Dictionary<String, PutCharMap> Maps = new Dictionary<string, PutCharMap>();
        /// <summary>
        /// 高地 地形 用于判断是否可放置
        /// </summary>
        public Dictionary<String, RectColor> HighLand = new Dictionary<string, RectColor>();
        /// <summary>
        /// 地面 地形  用于判断是否可放置
        /// </summary>
        public Dictionary<String, RectColor> LowLand = new Dictionary<string, RectColor>();
        /// <summary>
        /// 用于判断状态的区域---不用于判断宝箱
        /// </summary>
        public Dictionary<String, RectColor> StateRegion = new Dictionary<string, RectColor>();
        /// <summary>
        /// 高地 地形 点击 点
        /// </summary>
        public Dictionary<String, CharPoint> HighLandPoints = new Dictionary<string, CharPoint>();
        /// <summary>
        /// 地面 地形 点击 点
        /// </summary>
        public Dictionary<String, CharPoint> LowLandPoints = new Dictionary<string, CharPoint>();

        public Boolean IsState(String state)
        {

            //return StateRegion.ContainsKey(state)? StateRegion[state].IsRegionRight()
            return false;
        }
        public PutCharMap(List<String> ImgFiles) : base(ImgFiles)
        {

        }
        public PutCharMap() { }
        static PutCharMap()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\config\\map");
            if (!di.Exists) { di.Create(); return; }
            var fn = Environment.CurrentDirectory + "\\config\\map\\putchar.json";
            if (File.Exists(fn))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Close(); fs.Dispose(); fs = null;
                Maps = jss.Deserialize<Dictionary<String, PutCharMap>>(Encoding.UTF8.GetString(bs));
                bs = null;
            }
            else
            {
                Maps = new Dictionary<string, PutCharMap>();
            }
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
            DirectoryInfo di_map = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\map");
            if (!di_map.Exists) di_map.Create();
            var dis = di_map.GetDirectories();
            foreach (var d in dis)
            {
                var MapName = d.Name;
                if (!Maps.ContainsKey(MapName))
                {
                    Maps.Add(MapName, new PutCharMap() { Name = MapName });
                }
                var ndi = new DirectoryInfo(d.FullName + "\\putchar");
                var fss = ndi.GetFiles("*.png");
                foreach (var fs in fss)
                {
                    if (Maps[MapName].ImgFiles.Contains(fs.Name))
                    {

                    }
                    else
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
            if (!di.Exists) { di.Create(); }
            var fn = Environment.CurrentDirectory
                
                + "\\config\\map\\putchar.json";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
            var text = jss.Serialize(Maps);
            byte[] bs = Encoding.UTF8.GetBytes(text);
            fs.Write(bs, 0, bs.Length);
            fs.Close(); fs.Dispose(); fs = null; bs = null;

        }
    }
}
