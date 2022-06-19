using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRFZ_Auto.script.mrfz.shop
{
    public class shop_item
    {
        public Boolean HasBank;
        /// <summary>
        /// 1-8
        /// </summary>
        public int index;
        public int offset_x = 206;
        public int offset_y = 211;
        public Size ItemNameSize = new Size(142, 22);
        public Size SupportItemSize = new Size(42, 22);
        public Size ItemExpSize = new Size(50,18);
        public Point ClickPointToBuy;
        public Boolean canBuy; 
        public String ItemName;
        public ItemType Item_Type;
        public float PriceRatio = 0;
       // public shop_item() { }
        public shop_item(ItemType t) { Item_Type = t; }
        public enum ItemType
        {
            Item,
            supportItem,
            paper, 
        }
        public int Price;
        public static List<shop_item> RecCurShopItemList(Boolean HasBank,Bitmap bmp =null)
        { 
            if (bmp == null)
                    bmp = mrfzGamePage.CatptureImg();
            ImageColor[,] srcIC = ImageColor.FromBitmap(bmp);
            int index = 1;
            if (HasBank) index = 2;
            List<shop_item> itemlist = new List<shop_item>();
            for (;index<=8;index++)
            {
                Dictionary<int, double> price_dlts = new Dictionary<int, double>();
                foreach(var kv in PriceICList)
                {

                    var price_dlt=  GetGrayDeltaOfTwoIC(srcIC, kv.Value, PriceRect[index],117);
                    price_dlts.Add(kv.Key, price_dlt);
                }
                var price = price_dlts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
              //  var price_kv= price_dlts.Max( );
                if (price == 999)
                {
                    continue;
                }
                  
                {
                    Dictionary<String, double> paper_dlts = new Dictionary<String, double>();
                    foreach (var kv in PaperICList)
                    {

                        var paper_dlt = GetGrayDeltaOfTwoIC(srcIC, kv.Value, ShopItemNameRect[index],117);
                        paper_dlts.Add(kv.Key, paper_dlt);
                    }
                    var peper_dlt_max_kv = paper_dlts.Aggregate((l, r) => l.Value > r.Value ? l : r);
                 //   var peper_dlt_min_kv = paper_dlts.Max(x => x);
                    var paperName = peper_dlt_max_kv.Key;
                    var peper_dlt_min = peper_dlt_max_kv.Value;
                    var sp_dlt = GetGrayDeltaOfTwoIC(srcIC, SupportItemIC, SupportItemRect[index],117);

                    if (sp_dlt >= peper_dlt_min&& sp_dlt>90)
                    {
                        itemlist.Add(new shop_item(ItemType.supportItem)
                        {
                            index = index,
                            canBuy = true,
                            Price = price,
                            ItemName = "支援道具",
                            PriceRatio = 5f / price
                        }); 
                        continue;
                    }
                    if (peper_dlt_min>=90)
                    {
                        if(price>=8)
                        {
                            itemlist.Add(new shop_item(ItemType.Item)
                            {
                                index = index,
                                canBuy = true,
                                Price = price,
                                ItemName = paperName,
                                PriceRatio = 5f / price
                            });
                        }
                        else
                        {
                            itemlist.Add(new shop_item(ItemType.paper)
                            {
                                index = index,
                                canBuy = true,
                                Price = price,
                                ItemName = paperName,
                                PriceRatio = 2f / price
                            });
                        }
                        
                    }
                    else
                    {
                        if (price < 8)
                        {
                            //???
                            itemlist.Add(new shop_item(ItemType.paper)
                            {
                                index = index,
                                canBuy = true,
                                Price = price,
                                ItemName = "未知招募卷",
                                PriceRatio = 2f / price
                            });
                        }
                        else
                        {
                            itemlist.Add(new shop_item(ItemType.Item)
                            {
                                index = index,
                                canBuy = true,
                                Price = price,
                                ItemName = "收藏品*",
                                PriceRatio = 5f / price
                            });
                        }
                      
                    }
                }
            }
             
            return itemlist;
        }
        static double GetGrayDeltaOfTwoIC(ImageColor[,] srcIC,ImageColor[,] dstIC,Rectangle rect,int gray_min=0)
        {
            double dlt_not_eq = 0;
            int sum = 0;
            for(int x=0;x< rect.Width; x++)
            {
                for(int y=0; y<rect.Height;y++)
                {
                    var col = srcIC[x+rect.X, y+rect.Y];
                    var dst = dstIC[x, y];
                    if (col.G> gray_min&& col.R > 100 && col.G > 100 & col.B > 100
                           && Math.Abs(col.R - col.G) < 30 && Math.Abs(col.B - col.R) < 30 & Math.Abs(col.G - col.B) < 30)
                    {
                        //cur=font
                        //font
                        if(dst.R==255)
                        {
                            sum++;
                        }
                        else
                        {
                            dlt_not_eq++;
                        }
                         
                       // bmp.SetPixel(x, y, fontCol);
                    }
                    else
                    {
                        //cur=back
                        //font
                        if (dst.R == 255)
                        {
                            dlt_not_eq++;
                            //sum++;
                        }
                        else
                        { 
                        }
                        
                       // bmp.SetPixel(x, y, backCol);
                    }
                }
            }
            //not eq percent
            dlt_not_eq = dlt_not_eq/(sum) * 100;
            if (dlt_not_eq > 100) dlt_not_eq = 100;
            var dlt_eq_percent = 100 - dlt_not_eq;
            return dlt_eq_percent;
        }
        static shop_item()
        {
            DirectoryInfo di_paper = new DirectoryInfo(Environment.CurrentDirectory+ @"\imgs\shop\ItemNameFonts\\招募卷");
            var fss=di_paper.GetFiles("*.png");
            foreach(var fs in fss)
            {
                PaperICList.Add(fs.Name, ImageColor.FromFile(fs.FullName));
            }
           var di_price = new DirectoryInfo(Environment.CurrentDirectory + @"\imgs\shop\\itemprice_fonts");
              fss = di_price.GetFiles("*.png");
            foreach (var fs in fss)
            {
                PriceICList.Add(int.Parse(fs.Name.Split('.')[0]), ImageColor.FromFile(fs.FullName));
            }
            String SpFileName = Environment.CurrentDirectory + @"\imgs\shop\ItemNameFonts\\支援\\支援.png";
            SupportItemIC = ImageColor.FromFile(SpFileName);
            //ITEM NAME
            for(int i=0; i<4;i++)
            {
                ShopItemNameRect.Add(i+1, new Rectangle(new Point(418 + 206 * i, 106), new Size(142, 22)));
            }
            for (int i = 0; i < 4; i++)
            {
                ShopItemNameRect.Add(i + 5, new Rectangle(new Point(418 + 206 * i, 317), new Size(142, 22)));
            }
            //PRICE

            for (int i = 0; i < 4; i++)
            {
                PriceRect.Add(i + 1, new Rectangle(new Point(1162 - 206 * (3 - i), 258), new Size(50, 18)));
            }
            for (int i = 0; i < 4; i++)
            {
                PriceRect.Add(i + 5, new Rectangle(new Point(1162 - 206 * (3 - i), 258 + 211), new Size(50, 18)));
            }
            //SURPPORT
            for (int i = 0; i < 4; i++)
            {
                SupportItemRect.Add(i + 1, new Rectangle(new Point(418 + 206 * i, 106), new Size(42, 22)));
            }
            for (int i = 0; i < 4; i++)
            {
                SupportItemRect.Add(i + 5, new Rectangle(new Point(418 + 206 * i, 317), new Size(42, 22)));
            }

        }
        /// <summary>
        /// KEY=INDEX
        /// </summary>
        protected static Dictionary<int, Rectangle> ShopItemNameRect = new Dictionary<int, Rectangle>();
        /// <summary>
        /// KEY = FILENAME
        /// </summary>
        protected static Dictionary<String, ImageColor[,]> PaperICList = new Dictionary<string, ImageColor[,]>();
        /// <summary>
        /// KEY=PRICE 
        /// </summary>
        protected static Dictionary<int, ImageColor[,]> PriceICList = new Dictionary<int, ImageColor[,]>();
        /// <summary>
        /// KEY = INDEX 1-8
        /// </summary>
        protected static Dictionary<int, Rectangle> PriceRect = new Dictionary<int, Rectangle>();
        /// <summary>
        /// 
        /// </summary>
        protected static ImageColor[,] SupportItemIC;
        /// <summary>
        /// KEY=INDEX 
        /// </summary>
        protected static Dictionary<int, Rectangle> SupportItemRect = new Dictionary<int, Rectangle>();

    }
}
