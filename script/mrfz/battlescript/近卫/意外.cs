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
        protected rs_put_pos GetLH_近卫_意外()
        {

            var L = CalcMindltOfTwoLowRegion(BattleMap.MapName, "L2", "L3");
            Dir dir_L = Dir.左;// (L == "L2" ? Dir.左 : Dir.下);
            var H = L == "L2" ? "H2" : "H3";
            var dir_H =H=="H2"?Dir.左:Dir.下;
            return new rs_put_pos() { L = L, H = H, dir_L = dir_L, dir_H = dir_H };

        } 
    }
}
