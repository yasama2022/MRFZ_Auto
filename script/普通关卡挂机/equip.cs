using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.普通关卡挂机
{
    public class equip
    {
        public enum eqType
        {
            A,
            B
        }
        public enum CharClass
        {
            先锋=0,
            重装,
            术士,
            辅助,
            近卫,
            狙击,
            医疗,
            特种
        }
        public static Dictionary<CharClass,eqType> GetEquip()
        { 
            Bitmap bmp = mrfzGamePage.CatptureImg();
            var srcIC = ImageColor.FromBitmap(bmp);
            var A=ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\baoquan\\equip\\A.png");
           var B= ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\baoquan\\equip\\B.png");
            var backColor=Color.FromArgb(49, 49, 49);
            Dictionary<CharClass, eqType> eqTypeList = new Dictionary<CharClass, eqType>();
            bmp.Dispose();
            {
                 
                for (int i = 0; i < 4; i++)
                {
                    var X = 143 + i * 315;
                    var Y = 214;
                    var W = 15;
                    var H = 16;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    var dA=ImageColor.CalcDeltaOfTwoImgInFont(srcIC, A, backColor, rect);
                    var dB = ImageColor.CalcDeltaOfTwoImgInFont(srcIC, B, backColor, rect);
                    if(dA<dB)
                    {
                        eqTypeList.Add((CharClass)i,eqType.A);
                    }else
                    {
                        eqTypeList.Add((CharClass)i,eqType.B);
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    var X = 143 + i * 315;
                    var Y = 214 + 229;
                    var W = 15;
                    var H = 16;
                    Rectangle rect = new Rectangle(new Point(X, Y), new Size(W, H));
                    var dA = ImageColor.CalcDeltaOfTwoImgInFont(srcIC, A, backColor, rect);
                    var dB = ImageColor.CalcDeltaOfTwoImgInFont(srcIC, B, backColor, rect);
                    if (dA < dB)
                    {
                        eqTypeList.Add((CharClass)i, eqType.A);
                    }
                    else
                    {
                        eqTypeList.Add((CharClass)i, eqType.B);
                    }
                } 
            }
            return eqTypeList;
        }
    }
}
