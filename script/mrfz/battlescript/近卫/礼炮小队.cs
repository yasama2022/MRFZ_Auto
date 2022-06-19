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
        protected rs_put_pos GetLH_近卫_礼炮小队()
        {


            //L7 L10
            //H4
            var L = CalcMindltOfTwoLowRegion(BattleMap.MapName, "L1", "L2");
            Dir dir_L = (L == "L1" ? Dir.右 : Dir.上);
            var H = L == "L1" ? "H1" : "H2";
            var dir_H = (H == "H1" ? Dir.下 : Dir.右);
            return new rs_put_pos() { L = L, H = H, dir_L = dir_L , dir_H= dir_H };

        }
         
    }
}
