using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MRFZ_Auto.script.CharPoint;

namespace GamePageScript.script.mrfz
{
    public partial class mrfz_ScriptUnit
    {
        protected rs_put_pos GetLH_近卫_死斗()
        {
            var L = CalcMindltOfTwoLowRegion(BattleMap.MapName, "L1", "L2");
            Dir dir_L = Dir.左;
            var H = "H2";
            var dir_H = Dir.左;
            return new rs_put_pos() { L = L, H = H, dir_L = dir_L, dir_H = dir_H };

        }
    }
}
