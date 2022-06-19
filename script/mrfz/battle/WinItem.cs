using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRFZ_Auto.script.mrfz.battle
{
    public class WinItem
    {
        static WinItem()
        {
            ItemList.Add(ItemType.GOLD, new GameItem(ItemType.GOLD, "GOLD", "源石锭"));
            ItemList.Add(ItemType.ITEM, new GameItem(ItemType.ITEM, "ITEM"));
            ItemList.Add(ItemType.SONG, new GameItem(ItemType.SONG, "SONG", "剧目"));
            ItemList.Add(ItemType.PAPER, new GameItem(ItemType.PAPER, "PAPER"));
            ItemList.Add(ItemType.SURPPORT, new GameItem(ItemType.SURPPORT, "SURPPORT"));
            ItemList.Add(ItemType.EXIT, new GameItem(ItemType.EXIT, null,"EXIT"));
            PaperList.Add(PaperType.先锋, new GameItem(ItemType.PAPER, PaperType.先锋, "PAPER", "先锋招募卷"));
            PaperList.Add(PaperType.医疗, new GameItem(ItemType.PAPER, PaperType.医疗, "PAPER", "医疗招募卷"));
            PaperList.Add(PaperType.术师, new GameItem(ItemType.PAPER, PaperType.术师, "PAPER", "术师招募卷"));
            PaperList.Add(PaperType.特种, new GameItem(ItemType.PAPER, PaperType.特种, "PAPER", "特种招募卷"));
            PaperList.Add(PaperType.狙击, new GameItem(ItemType.PAPER, PaperType.狙击, "PAPER", "狙击招募卷"));
            PaperList.Add(PaperType.辅助, new GameItem(ItemType.PAPER, PaperType.辅助, "PAPER", "辅助招募卷"));
            PaperList.Add(PaperType.近卫, new GameItem(ItemType.PAPER, PaperType.近卫, "PAPER", "近卫招募卷"));
            PaperList.Add(PaperType.重装, new GameItem(ItemType.PAPER, PaperType.重装, "PAPER", "重装招募卷"));
            return;
        }
        static Dictionary<ItemType, GameItem> ItemList = new Dictionary<ItemType, GameItem>();
        static Dictionary<PaperType, GameItem> PaperList = new Dictionary<PaperType, GameItem>();

        static Rectangle ItemRec = new Rectangle(
            new Point(75 , 537),
                     new Size(45, 28));
        static Rectangle SubItemRec = new Rectangle(new Point(107 , 373),
                     new Size(114, 23));
        public static int Offset_X { get; } = 280;
        public static Point CurClickPoint(int index,GameItem item)
        {
            if(item.itemType== ItemType.EXIT)
            {
                return new Point(Offset_X*index+ SubItemRec.X, SubItemRec.Y+30);
            }else
            {

                return new Point(Offset_X * index + ItemRec.X+10, ItemRec.Y  +2);
            }
        }
        public static  GameItem GetCurItem()
        {
            var src = mrfzGamePage.CatptureImg();
            ImageColor[,] srcIc = ImageColor.FromBitmap(src);
            src.Dispose();
            foreach(var kv in ItemList)
            {
                var T = kv.Key;
                var dlt=ImageColor.CalcDeltaOfTwoImg(srcIc, kv.Value.IC,
                   ItemRec);
                if(dlt< mrfz_ScriptConfig.scriptConfig.dlt_region)
                {
                    GameItem searchItem = kv.Value;
                    if (T== ItemType.PAPER)
                    {
                        foreach(var paperKV in PaperList)
                        {
                            var dlt2 = ImageColor.CalcDeltaOfTwoImg(srcIc, paperKV.Value.sub_IC,
                            SubItemRec);
                            if(dlt2 < mrfz_ScriptConfig.scriptConfig.dlt_region)
                            {
                                return paperKV.Value;
                            }
                        }
                    } 
                    return searchItem;
                }
            }
            return null;

        }

        public static List<GameItem> GetCurItemList()
        {
            List<GameItem> list = new List<GameItem>();
            var src = mrfzGamePage.CatptureImg();
            ImageColor[,] srcIc = ImageColor.FromBitmap(src);
            src.Dispose();
            for(int i=0;i<4 ;i++)
            {
                var cur_item_rec_offset = new Rectangle(
                    new Point(ItemRec.X+i*Offset_X,ItemRec.Y)
                    , ItemRec.Size);
                var cur_subitem_rec_offset = new Rectangle(
                   new Point(SubItemRec.X + i * Offset_X, SubItemRec.Y)
                   , SubItemRec.Size);
                GameItem GI = null;
                double cur_DLT = 9999; 
                foreach (var kv in ItemList)
                { 
                    var T = kv.Key;
                    if (kv.Value.IC==null)
                    {
                        //EXIT
                        var dlt_ext = ImageColor.CalcDeltaOfTwoImg(srcIc, kv.Value.sub_IC,
                                                      cur_subitem_rec_offset);
                        if (dlt_ext < mrfz_ScriptConfig.scriptConfig.dlt_region
                            && dlt_ext < cur_DLT)
                        {

                            cur_DLT = dlt_ext;
                            GI = kv.Value;
                        }
                        continue;
                    }
                    var dlt = ImageColor.CalcDeltaOfTwoImg(srcIc, kv.Value.IC,
                       cur_item_rec_offset);
                    if (dlt < mrfz_ScriptConfig.scriptConfig.dlt_region
                        &&dlt<cur_DLT)
                    {
                        GameItem searchItem = kv.Value;
                        if (T == ItemType.PAPER)
                        {
                            Dictionary<PaperType, double> PaperDlts = new Dictionary<PaperType, double>();
                            foreach (var paperKV in PaperList)
                            {
                                var dlt2 = ImageColor.CalcDeltaOfTwoImg(srcIc, paperKV.Value.sub_IC,
                                cur_subitem_rec_offset);
                                if (dlt2 < mrfz_ScriptConfig.scriptConfig.dlt_region)
                                {
                                    PaperDlts[paperKV.Key] = dlt2; 
                                    break;
                                }
                            }
                            if(PaperDlts.Count==0)
                            {
                                continue;
                            }
                              PaperDlts = PaperDlts.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            var PT=PaperDlts.First().Key;
                            GI = PaperList[PT]; 
                        }else
                        {
                            if(searchItem.sub_IC!=null)
                            {
                               var sub_dlt = ImageColor.CalcDeltaOfTwoImg(srcIc, kv.Value.sub_IC,
                                                       cur_subitem_rec_offset);
                                if(sub_dlt < mrfz_ScriptConfig.scriptConfig.dlt_region)
                                {

                                    cur_DLT = dlt;
                                    GI = searchItem;
                                }
                            }else
                            {
                                cur_DLT = dlt;
                                GI = searchItem;
                            }
                        }
                    }else
                    {
                        continue;
                    }
                    
                }
                if(GI==null)
                {
                    //EXIT
                    //未识别出
                    break;
                    //list.Add(new GameItem(ItemType.UNKNOWN, null,null));
                   // break;
                }else
                {
                    list.Add(GI);
                }
            }
             
            return list;

        }
    }
    
    public class GameItem
    {
        public ItemType itemType;
        public PaperType paperType;
        public ImageColor[,] IC;
        public ImageColor[,] sub_IC;
        public GameItem(ItemType itemTyp, ImageColor[,] IC)
        {
            this.itemType = itemTyp;
            this.IC = IC;
        }
        public GameItem(ItemType itemTyp, String FileName)
        {
            this.itemType = itemTyp;
            if (FileName != null)
                this.IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\winitems\\"
                +FileName+".png"); 
        }
        public GameItem(ItemType itemTyp, String FileName,String SubFileName)
        {
            this.itemType = itemTyp; 
            if (FileName != null)
            this.IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\winitems\\"
                + FileName + ".png");
            if (SubFileName != null)
                this.sub_IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\winitems\\"
                + SubFileName + ".png");
        }
        public GameItem(ItemType itemTyp, PaperType paperType, String FileName
           , String FileName_Sub)
        {

            this.itemType = itemTyp;
            if (FileName != null)
                this.IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\winitems\\"
                + FileName + ".png");
            this.paperType = paperType;

            if (FileName_Sub != null)
                this.sub_IC = ImageColor.FromFile(Environment.CurrentDirectory + "\\imgs\\winitems\\"
                + FileName_Sub + ".png");
            if(sub_IC==null)
            {
                //.
                return;
            }
        }
         
    }
    public enum ItemType
    {
        GOLD,
        ITEM,
        PAPER,
        SONG,
        SURPPORT,
        EXIT,
        UNKNOWN,

    }
    public enum PaperType
    {
        先锋,
        近卫,
        重装,
        特种,
        狙击,
        辅助,
        术师,
        医疗
    }
}
