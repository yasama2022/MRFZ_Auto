using GamePageScript.lib;
using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz
{
    public class TeamLogo
    {
        public TeamType teamType;
        public String ImgFile;
        public ImageColor[,] srcIC;
        public static Boolean FindTeam(TeamType teamType,out Point ClickPoint,Bitmap bmp=null,Boolean canSwipe=true)
        {
            if (bmp == null)
                bmp = mrfzGamePage.CatptureImg();
            ClickPoint = new Point();
            ImageColor[,] srcIC = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            if(FindTeam(teamType, out ClickPoint, srcIC))
            {
                return true;
            }else
            {
                //2
                if(canSwipe)
                {
                    adb.Swipe(new Point(946,436),new Point(500,436), 1200);
                    Thread.Sleep(1200);
                    bmp = mrfzGamePage.CatptureImg();
                    ClickPoint = new Point();
                    srcIC = ImageColor.FromBitmap(bmp);
                    bmp.Dispose();
                    if (FindTeam(teamType, out ClickPoint, srcIC))
                    {
                        return true;
                    }
                    else
                    {
                        //3
                        adb.Swipe(new Point(946,436),new Point(500,436), 1200);
                        Thread.Sleep(1200);
                        bmp = mrfzGamePage.CatptureImg();
                        ClickPoint = new Point();
                        srcIC = ImageColor.FromBitmap(bmp);
                        bmp.Dispose();

                    }
                }
                else
                {
                    return false;
                }
                
            }

            return false;
        }

        static Boolean FindTeam(TeamType teamType, out Point ClickPoint, ImageColor[,] srcIC)
        {
           var dstIC= Teams[teamType].srcIC;
            ClickPoint = new Point();
            for (int x = 1; x < 1280; x++)
            {
                var col = srcIC[x - 1, 487];
                //     var l2 = srcIC[x-1, 488];
                if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                  Math.Abs(col.B - col.G) <= 5)
                {
                    continue;

                }
                else
                {

                    col = srcIC[x, 487];
                    if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                 Math.Abs(col.B - col.G) <= 5)
                    {
                        col = srcIC[x, 488];
                        if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                        Math.Abs(col.B - col.G) <= 5)
                        {
                            int LEN = 10;
                            Boolean checkFlag = true;
                            for (int i = 0; i < LEN; i++)
                            {
                                col = srcIC[x + i, 487];
                                if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                       Math.Abs(col.B - col.G) <= 5)
                                {
                                    col = srcIC[x + i, 488];
                                    if (col.R > 50 && col.G > 50 && col.B > 50 && Math.Abs(col.R - col.G) <= 5 && Math.Abs(col.R - col.B) <= 5 &&
                      Math.Abs(col.B - col.G) <= 5)
                                    {

                                    }
                                    else
                                    {
                                        checkFlag = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    checkFlag = false;
                                    break;
                                }
                            }
                            if (checkFlag)
                            {
                                //
                                //108 270
                                //240 395
                                var x_start = x + 108 - 62;
                                var y_start = 270;
                                var h = 125;
                                var w = 132;
                                var rect = new Rectangle(new Point(x_start, y_start), new Size(w, h));
                                if(rect.X+w>1280)
                                {
                                    x += 276;
                                    continue;
                                }
                                var dlt=ImageColor.CalcDeltaOfTwoImg(srcIC, dstIC, rect);
                                if(dlt<mrfz_ScriptConfig.scriptConfig.dlt_region)
                                {
                                    ClickPoint = new Point(x+109, 487+113);
                                    return true;
                                }
                                //  bmplist.Add(bmp.Clone(new Rectangle(new Point(x_start, y_start), new Size(w, h)), System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                                x += 276;
                                continue;
                                //62->121
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {

                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }


            }
            return false;
        }
        public TeamLogo(TeamType teamType)
        {
            ImgFile=Environment.CurrentDirectory+ @"\imgs\teamlogo\" + teamType.ToString() + ".png";
            srcIC = ImageColor.FromFile(ImgFile);
        }
         static TeamLogo()
        {
            Teams = new Dictionary<TeamType, TeamLogo>();
            TeamType[] list = new TeamType[] {  TeamType.指挥, TeamType.后勤, TeamType.突击战术, TeamType.堡垒战术};
            foreach(var li in list)
            {

                Teams.Add(li, new TeamLogo(li));
            }
        }
          static Dictionary<TeamType, TeamLogo> Teams;
        public enum TeamType
        {
            指挥,
            后勤,
            突击战术,
            堡垒战术
        }
    }
}
