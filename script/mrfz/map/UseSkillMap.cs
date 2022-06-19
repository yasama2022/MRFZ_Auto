using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MRFZ_Auto.script.mrfz.map
{
    public class UseSkillMap:map
    {
        /// <summary>
        /// key=mapname
        /// </summary>
        public static Dictionary<String, UseSkillMap> Maps = new Dictionary<string, UseSkillMap>(); 
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
        public UseSkillMap(List<String> ImgFiles) : base(ImgFiles)
        {

        }
        public UseSkillMap() { }
        static UseSkillMap()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\config\\map");
            if (!di.Exists) { di.Create(); return; }
            var fn = Environment.CurrentDirectory + "\\config\\map\\useskill.json";
            if (File.Exists(fn))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
                byte[] bs = new byte[fs.Length];
                fs.Read(bs, 0, bs.Length);
                fs.Close(); fs.Dispose(); fs = null;
                Maps = jss.Deserialize<Dictionary<String, UseSkillMap>>(Encoding.UTF8.GetString(bs));
                bs = null;
            }
            else
            {
                Maps = new Dictionary<string, UseSkillMap>();
            }
            DirectoryInfo di_map = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\map");
            if (!di_map.Exists) di_map.Create();
            var dis = di_map.GetDirectories();
            foreach (var d in dis)
            {
                var MapName = d.Name;
                if (!Maps.ContainsKey(MapName))
                {
                    Maps.Add(MapName, new UseSkillMap() { Name = MapName });
                }
                var ndi = new DirectoryInfo(d.FullName+ "\\useskill");
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

                + "\\config\\map\\useskill.json";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
            var text = jss.Serialize(Maps);
            byte[] bs = Encoding.UTF8.GetBytes(text);
            fs.Write(bs, 0, bs.Length);
            fs.Close(); fs.Dispose(); fs = null; bs = null;

        }
    }
} 
