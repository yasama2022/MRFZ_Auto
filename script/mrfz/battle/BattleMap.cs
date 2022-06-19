using script;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz.battle
{
    public class BattleMap
    {
        /// <summary>
        /// 地图名
        /// </summary>
        public String Name;
        /// <summary>
        /// 紧急作战
        /// </summary>
        public Boolean urgent; 
        /// <summary>
        /// 是否已经进入本次战斗的判断
        /// </summary>

        public List<RectColor> CheckPageRectData;
        /// <summary>
        /// 技能-释放判断UI
        /// </summary>

        public List<RectColor> CheckSkillRectData;
        /// <summary>
        /// 地图可放置-位置的 RECT-COLORS----初始位置
        /// 命名规范:A1-AN,地面,B1-BN 高台,C1-CN:高/低均可,******不需要
        /// 其他为不可放置的地点,标记地形 如祭坛
        /// </summary>
        public Dictionary<String, RectColor> MapBlockRegions;
        /// <summary>
        /// 地图 char放置时的 RECT-COLORS---- PUT CHAR时, 判断宝箱时机
        /// M1-N
        /// H1-N 
        /// 该位置必须为可放置的。
        /// </summary>
        public Dictionary<String, RectColor> MapPutCharRegions;
        /// <summary>
        /// 地图  技能开启时的位置
        /// </summary>
        public Dictionary<String, Point> MapSkill_Points;
        /// <summary>
        /// 地图  技能准备完毕后 的判断------(与人物模型高矮有关)
        /// </summary>
        public Dictionary<String, RectColor> MapSkillOpenRegions;
        /// <summary>
        /// 主要角色的放置方向
        ///    M1-N 
        /// </summary>
        Dictionary<String, Dir> Map_MainChar_Dir = new Dictionary<string, Dir>();
        /// <summary>
        /// 治疗的的放置方向 
        /// H1-N 
        /// </summary>
        Dictionary<String, Dir> Map_Heal_Dir = new Dictionary<string, Dir>(); 

        /// <summary>
        /// 放置角色的朝向
        /// </summary>
        public enum Dir
        {
            上,
            下,
            左,
            右
        }
        /// <summary>
        /// 祭坛位置
        /// </summary>
        public enum altar_pos
        {
            left,
            right,
            none
        }
        /// <summary>
        /// 驯兽小屋 祭坛
        /// </summary>
        public altar_pos curPos
        {
            get
            {
                if (this.Name == "驯兽小屋")
                {
                    return altar_pos.left;
                }
                else
                    return altar_pos.none;
            }
        }
        static BattleMap()
        {
            for (int i = 0; i < 8; i++)
            {
                PointList.Add(new Point(1183 - i * 119 - 20, 631 + 20));
            }
        }
        /// <summary>
        /// 放置角色时的起点位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Point GetCharPOS_Start(int index)
        {
            return PointList[index];
        }
        static List<Point> PointList = new List<Point>();
        /// <summary>
        /// 2X 按钮位置
        /// </summary>

         static Point X2_Point; 
    }
}
