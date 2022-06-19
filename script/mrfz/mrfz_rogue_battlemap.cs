using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamePageScript.script.mrfz;
namespace MRFZ_Auto.script.mrfz
{
    public class mrfz_rogue_battlemap
    {
        public String MapName;
        /// <summary>
        /// 紧急
        /// </summary>
        public Boolean urgent;
        public mrfzGamePage RecPage;
        public mrfzGamePage Battle_First;
        public mrfzGamePage Battle_PutChar;
        public mrfzGamePage Battle_Skill;
        public int CharIndex = 1;
        public int PutCharPointCount = 0;
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
                if (this.MapName == "驯兽小屋")
                {
                    return altar_pos.left;
                }
                else
                    return altar_pos.none;
            }
        }
        static mrfz_rogue_battlemap()
        {
            for (int i = 0; i < 5; i++)
            {
                PointList.Add(new Point(1183 - i * 119-20, 631+20));
            }
        }
        public static Point GetCharPOS_Start(int index)
        {
            return  PointList[index];
        }
          static List<Point> PointList = new List<Point>();

        public mrfz_rogue_battlemap(String MapName,Boolean urgent, mrfzGamePage RecPage
            , mrfzGamePage Battle_First, mrfzGamePage Battle_PutChar, mrfzGamePage Battle_Skill)
        {
            this.MapName = MapName;
            this.RecPage = RecPage;
            this.Battle_First = Battle_First;
            this.Battle_PutChar = Battle_PutChar;
            this.Battle_Skill = Battle_Skill;
            this.urgent = urgent;
        }
        public Boolean IsCurMap() {
            return RecPage.CheckPage(200);
        }




    }
}
