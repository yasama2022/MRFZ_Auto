using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MRFZ_Auto.script.保全派驻.通用
{
    public class TeamCharName
    {
        public enum CharClass
        {
            先锋 = 0,
            重装,
            术士,
            辅助,
            近卫,
            狙击,
            医疗,
            特种,
            未知
        }
        /// <summary>
        /// 需要识别的角色
        /// </summary>
        public enum RecChar
        {
            泥岩,
            铃兰,
            橙闪, 
            //
            拉普兰德,
            归溟幽灵鲨,
            风笛, 
            推进之王,
            山,
            //不可以充当助战
            安赛尔,
            未知,

        }
        public static Dictionary<RecChar, CharClass> CharClassDic = new Dictionary<RecChar, CharClass>();
        public static Dictionary<RecChar, ImageColor[,]> nameIC = new Dictionary<RecChar, ImageColor[,]>();
        static TeamCharName()
        {
            CharClassDic[RecChar.山] = CharClass.近卫;
            CharClassDic[RecChar.泥岩] = CharClass.重装;
            CharClassDic[RecChar.归溟幽灵鲨] = CharClass.特种;
            CharClassDic[RecChar.拉普兰德] = CharClass.近卫;
            CharClassDic[RecChar.推进之王] = CharClass.先锋;
            CharClassDic[RecChar.橙闪] = CharClass.术士;
            CharClassDic[RecChar.铃兰] = CharClass.辅助;
            CharClassDic[RecChar.风笛] = CharClass.先锋;
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\imgs\\baoquan\\team\\name");
            var fss=di.GetFiles("*.png"); 
            foreach (var fs in fss)
            {
                RecChar rc=(RecChar) Enum.Parse(typeof(RecChar), fs.Name.Split('.')[0]);
                nameIC.Add(rc, ImageColor.FromFile(fs.FullName));
            }
            return;
        }
        public static List<Tuple<RecChar,double>> GetCharNameInTeamEdit()
        {
            var bmp = mrfzGamePage.CatptureImg();
            var srcIC = ImageColor.FromBitmap(bmp);
            bmp.Dispose();
            List<Tuple<RecChar, double>> list = new List<Tuple<RecChar, double>>();
            for (int i = 0; i < 14; i++)
            {
                double dlt = 0d;
                list.Add(new Tuple<RecChar, double>( GetCharNameInTeamEdit(srcIC, i,out dlt),dlt));
            }
            return list;
        }
        
        public static RecChar GetCharNameInTeamEdit(ImageColor[,] srcIC, int Index,out double dlt)
        {
            RecChar recChar = RecChar.未知;
            Dictionary<RecChar, double> recDic = new Dictionary<RecChar, double>();
            var X = 116 + Index * 133;
            var Y = 323;
            var W = 40;
            var H = 19;
            Rectangle rect;
            if (Index < 7)
            {
                  X = 116 + Index * 133;
                  Y = 323;
                  W = 40;
                  H = 19; 
                  rect = new Rectangle(new Point(X, Y), new Size(W, H));
                
            }
            else
            {
               // Index -= 7;
                  X = 116 + (Index-7) * 133;
                  Y = 323 + 250;
                  W = 40;
                  H = 19;  
                  rect = new Rectangle(new Point(X, Y), new Size(W, H)); 
            }

            foreach(var kv in nameIC)
            {
                  dlt = ImageColor.CalcDeltaOfTwoImg(srcIC, kv.Value, rect);

               // var dlt = ImageColor.CalcDeltaOfTwoImgInFont(srcIC, kv.Value, Color.FromArgb(34, 34, 34), rect,10);
                recDic.Add(kv.Key, dlt);
            }
            var min = recDic.Aggregate((l, r) => l.Value < r.Value ? l : r);
            
            dlt = min.Value;
             if (min.Value<8)
            {
                return min.Key;
            }else
            {
                return RecChar.未知;
            }
        }
    
   }
}
