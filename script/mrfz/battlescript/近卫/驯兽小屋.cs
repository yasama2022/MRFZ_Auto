using GamePageScript.lib;
using lib.image;
using MRFZ_Auto.script;
using MRFZ_Auto.script.mrfz;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script.mrfz.map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptConfig;
using static MRFZ_Auto.script.CharPoint;
using static MRFZ_Auto.script.mrfz.battle.BattleCharRec;

namespace GamePageScript.script.mrfz
{
    public partial class mrfz_ScriptUnit
    {

        /// <summary>
        /// 判断祭坛是否在左边
        /// </summary>
        /// <returns>祭坛位置是否在左边</returns>
        protected Boolean IsLeftJT()
        {
            int LEFT = 0;
            for(int i=0; i<5;i++)
            {
                if(IsLeftJT_Once())
                {
                    LEFT++;
                }
                wait(100);
            }
            if (LEFT >= 3)
            {
                return true;
            }
            else
                return false;
        }
        protected Boolean IsLeftJT_Once()
        {
            Bitmap bmp = mrfzGamePage.CatptureImg();
            var src = ImageColor.FromBitmap(bmp);
            bmp.Dispose();

            // onMsg?.Invoke("jt_r");
            var jtr = NormalMap.Maps["驯兽小屋"].StateRegion["JT_R"].srcIC;
            //  onMsg?.Invoke("jt_l");
            var jtl = NormalMap.Maps["驯兽小屋"].StateRegion["JT_L"].srcIC;
            var d1 = ImageColor.CalcDeltaOfTwoImg(src, jtr, NormalMap.Maps["驯兽小屋"].StateRegion["JT_R"].rect);
            var d2 = ImageColor.CalcDeltaOfTwoImg(src, jtl, NormalMap.Maps["驯兽小屋"].StateRegion["JT_L"].rect);

         //   onMsg?.Invoke($"JTR(右边无祭坛):{d1},JTL(左边无祭坛):{d2}");
            if (d1 < d2)
            {
             //   onMsg?.Invoke($"祭坛在左边");
                //LEFT
                return true;
            }
            else
            {
              //  onMsg?.Invoke($"祭坛在右边");
                return false;
            }
        }
        protected void Pre_近卫_驯兽小屋()
        {
            JT_LEFT = IsLeftJT();
        }
        protected rs_put_pos GetLH_近卫_驯兽小屋()
        {
            String L = "L1";
            String H = "H1";
            if (JT_LEFT)
            {
                //L1,L2
                L = CalcMindltOfTwoLowRegion(BattleMap.MapName, "L1", "L2");
                //H2
                H = "H2";
            }
            else
            {
                //L7 L10
                //H4
                L = CalcMindltOfTwoLowRegion(BattleMap.MapName, "L7", "L10");
                H = "H4";
            }
            return new rs_put_pos() { L=L,H=H,
            dir_L= Dir.右, dir_H= Dir.下};
        }
         
        Boolean JT_LEFT = false;
         
    }
}
