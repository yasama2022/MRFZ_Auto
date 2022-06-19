using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MRFZ_Auto.script.保全派驻.通用.TeamCharName;

namespace MRFZ_Auto.script.保全派驻.通用
{
    public class Team
    { 
        public class TeamChar
        {
            /// <summary>
            /// 编号,序号
            /// </summary>
            public int order;
            public CharClass ChClass;
            public RecChar ChName;
        }

        public  static List<TeamChar> GetAllCharInTeam()
        {
            var list=TeamClassInTeamEdit.GetCharClassInTeamEdit();
            var list_name=TeamCharName.GetCharNameInTeamEdit();
            List<TeamChar> list_chs = new List<TeamChar>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 != CharClass.未知)
                {
                    if (list_name.Count >= i) continue;
                    if(list_name[i].Item1== RecChar.归溟幽灵鲨)
                    {
                        if(list[i].Item1== CharClass.特种)
                        {
                            list_chs.Add(new TeamChar()
                            {
                                ChClass = CharClass.特种,
                                ChName = RecChar.归溟幽灵鲨,
                                order = i
                            });
                        }else
                        {
                            list_chs.Add(new TeamChar()
                            {
                                ChClass = list[i].Item1,
                                ChName = RecChar.未知,
                                order = i
                            });
                        }
                    }else
                    {
                        list_chs.Add(new TeamChar()
                        {
                            ChClass = list[i].Item1,
                            ChName = list_name[i].Item1,
                            order = i
                        });
                    }
                }
            }
            return list_chs;
        }
    }
}
