using script;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static MRFZ_Auto.script.mrfz.TeamLogo;
using static MRFZ_Auto.script.保全派驻.通用.TeamCharName;

namespace GamePageScript.script.mrfz
{
    public class mrfz_ScriptConfig: ScriptConfig
    {
        public ArkChar mainChar = ArkChar.山;
        public Diff diff= Diff.普通;
        public int RunCount = -1;
        public Policy policy = new Policy();
        public enum Priority
        {
            非战斗,
            普通作战,
            紧急作战,
        }
        public class Policy
        { 
            public Boolean AutoSaveGold = true;
             

            public Boolean MUJIANYUXING_First= false;
            public Boolean L1Exit = false;
            public Priority P1 = Priority.非战斗;
            public Priority P2 = Priority.普通作战;
            public Priority P3 = Priority.紧急作战;
            public Boolean GetItem=true;
            public Boolean GetSong=true;
            public Boolean GetPaper=true;
            public Boolean GetSupport=true;
            public Boolean GetGold=true;
            public Boolean BuyItem = true;
        }
        /// <summary>
        /// 助战 只选好友的/非好友也可
        /// </summary>
        public Boolean GetFriendRole_NeedFriendShip = true;
        public Boolean ShowTips = true;
        /// <summary>
        /// 是否使用助战
        /// </summary>
        public Boolean UseFriendRole = false;
        public Boolean UseADBCat = false;
        public enum Diff
        {
            简单,
            普通,
            灾厄,
        }
        public static mrfz_ScriptConfig scriptConfig;
        /// <summary>
        /// 选择team 分队的位置
        /// 
        /// 1 4 7
        /// 2 5 8
        /// 3 6 9
        /// </summary>
        public int SelectTeam_Loc = 5;
        public TeamType Team_Type = TeamType.指挥;
        /// <summary>
        /// 选择角色的位置
        /// 
        /// 1 4 7
        /// 2 5 8
        /// 3 6 9
        /// </summary>
        public int SelectChar_Loc=1;
        /// <summary>
        /// 选择heal 的位置
        /// 
        /// 1 
        /// 2 
        /// 3 
        /// 4
        /// </summary>
        public int SelectHEAL_Loc = 1;


        public double dlt_region = 5.68;
        public double dlt_page_check = 5;
        public double dlt_freind_char_get=5.68;
        public double dlt_battle_headimg = 5.68;
        public int DragRoleToBattleTime_ms = 1500;
        public Boolean enable_stoneeat = false;
        //保全派驻
        public Boolean UseFreindInBaoQuan = false;
        public Boolean NeedFriendShipsInBaoQuan = true;
        public RecChar FriendCharInBaoQuan = RecChar.泥岩;


        public enum ArkChar
        {
            山,
            煌,
            帕拉斯,
            令,
            棘刺,
            安赛尔,
            芙蓉,
            医疗预备干员,
            调香师,
            末药,
            苏苏洛,
            嘉维尔,
            清流,
            褐果, 
            未知,
        }
        static mrfz_ScriptConfig()
        {
            FileName = Environment.CurrentDirectory + "\\config\\ScriptConfig_mrfz.json";
            scriptConfig = load();
        }
        public void save()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\config"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\config");
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(jss.Serialize(this));
            sw.Close(); fs.Close(); sw.Dispose(); fs.Dispose();
        }
        public static mrfz_ScriptConfig load()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\config"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\config");
            }
            if (!File.Exists(FileName)) return new mrfz_ScriptConfig();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            mrfz_ScriptConfig sc = jss.Deserialize<mrfz_ScriptConfig>(sr.ReadToEnd());
            sr.Close(); sr.Dispose(); fs.Close(); fs.Dispose();
            return sc;
        }
    }
}
