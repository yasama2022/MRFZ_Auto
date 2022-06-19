using GamePageScript.script.mrfz;
using lib.image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePageScript.script.mrfz.mrfz_ScriptUnit;

namespace MRFZ_Auto.script.mrfz
{
    public   class BranchNode
    {
        /// <summary>
        /// 该节点在第几层
        /// </summary>
        public int LayerNumber { get; private set; } = -1;
        /// <summary>
        /// 若是作战或者紧急作战,险路恶敌,地图名
        /// </summary>
        public String MapName;
        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NT;
        /// <summary>
        /// 进入该节点的 坐标
        /// </summary>
        public Point ClickPoint;
        /// <summary>
        /// 本节点总层数
        /// </summary>
        public int LayerCount;
        /// <summary>
        /// 本层节点序号 (1,2,3,.... 表示该L1-L6 当前层进行的第几个节点
        /// 前1,2个节点 坐标和后面节点有所不同
        /// </summary>

        public int NodeIndex;

        /// <summary>
        /// B3  ->B2层 Y轴差值
        /// </summary>
        public static int B3_TO_B2_Y_dis { get; } = 68;

        /// <summary>
        /// 各节点间 Y轴差值
        /// </summary>
        public static int B_Y_dis { get; } = 136;
        /// <summary>
        /// 下一个节点 X轴
        /// </summary>
        public static int ClickNode_X { get; } = 840;
        /// <summary>
        /// 当前节点 X轴(需要切换时用)
        /// </summary>
        public static int ActivNode_X { get; } = 410;
        /// <summary>
        /// 三层节点时, 第3层Y轴 点击(或者用来识别)
        /// </summary>

        public static int ClickNode_B3_L3_Y { get; } = 467;
        //81 19
        //861 495
        /// <summary>
        /// 识别NODE的矩形 大小
        /// </summary>
        public static Size RECNode_Size { get; } = new Size(81, 19);
        /// <summary>
        /// 识别NODE的矩形  在B3L3的坐标X
        /// </summary>
        public static int REC_Node_X_Second { get; } = 861;
        /// <summary>
        /// 识别NODE的矩形  在B3L3的坐标X
        /// </summary>
        public static int REC_Node_X_Third { get; } = 881;
        /// <summary>
        /// X偏移，第2个节点前无偏移,第三节点后偏移
        /// </summary>
        public static int RECNode__X_OFFSET { get; } = 20;

        public static int REC_Node_X_FirstNode { get; } = 411;
        /// <summary>
        /// 识别NODE的矩形  在B3L3的坐标Y
        /// </summary>
        public static int RECNode_B3_L3_Y { get; } = 495; 
        //121,60
        //167,45
        /// <summary>
        /// 相对于REC_NODE的偏移
        /// </summary>
        public static int REC_MapName_X_OFFSET { get; } = -64;
        public static int REC_MapName_Y_OFFSET { get; } = 29;
        /// <summary>
        /// 地图名大小
        /// </summary>
        public static Size REC_MapName_Size { get; } = new Size(90, 24);



        private static double CalcRegions_dis(ImageColor[,] ic1,ImageColor[,] ic2,int w,int h)
        {
            var sum_delt = 0d;
            var AvgDelta = 0d;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var ori = ic1[x, y];
                    var dst = ic2[x, y];
                    var dr = dst.R - ori.R;
                    var dg = dst.G - ori.G;
                    var db = dst.B - ori.B;
                    var delt = Math.Sqrt(dr * dr + dg * dg + db * db);
                    sum_delt += delt;
                }
            AvgDelta = sum_delt /w / h;
            return AvgDelta;
        }

        /// <summary>
        /// 获取当前的节点列表 
        /// 
        /// </summary>
        /// <param name="NodeIndex">当前层 节点序号 1,2,3,....</param>
        /// <param name="L">第几层,1,2,3,....</param>
        /// <returns></returns>
        public static List<BranchNode> GetCurBranchNodes(int NodeIndex,int L,Bitmap curBmp)
        {
            if (NodeIndex >= 3) NodeIndex = 3;
            var RDic=RectDic[NodeIndex];
            var R = new Random();
            for (int L_C=3; L_C<=4; L_C++)
            {
                List<Tuple<NodeType, String>> NTlist = new List<Tuple<NodeType, String>>(); 
                
                for(int i=1; i<= L_C; i++)
                {
                   var rect= RDic[L_C][i];
                    var curRegion=  curBmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    ImageColor[,] cur_ic = ImageColor.FromBitmap(curRegion);
                    double minDelta = 99999;
                    NodeType final_nt = NodeType.Unkown;
                    Dictionary<NodeType, double> nt_delta = new Dictionary<NodeType, double>();
                    String MapName = null;
                    foreach (var kv in REC_Imgs)
                    {
                        var nt = kv.Key;
                        var NT_REC_ImgColor = kv.Value;
                       var cur_d= CalcRegions_dis(NT_REC_ImgColor, cur_ic, rect.Width, rect.Height);
                        nt_delta[nt] = cur_d; 
                        if (cur_d<minDelta&&cur_d<50)
                        {
                            minDelta = cur_d;
                            final_nt = nt;
                            if(nt== NodeType.作战||nt== NodeType.紧急作战||nt== NodeType.险路恶敌)
                            {
                                Rectangle mapNameRec = new Rectangle(rect.X + REC_MapName_X_OFFSET,
                                       rect.Y + REC_MapName_Y_OFFSET, 
                                       REC_MapName_Size.Width, 
                                       REC_MapName_Size.Height);
                                //识别成功
                                  MapName=GetMapName2(curBmp, mapNameRec);
                                if(MapName==null)
                                {
                                    //识别失败
                                    //SAVE IMG
                                    var mbmp = curBmp.Clone(mapNameRec, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                                    mbmp.Save(Environment.CurrentDirectory + @"\imgs\rec_mapname\" + R.Next(10000, 1000000).ToString() + ".png");
                                    mbmp.Dispose();
                                    return null;
                                }
                                
                            }
                        }
                    }
                    NTlist.Add(new Tuple<NodeType, string>(final_nt, MapName));
                    curRegion.Dispose(); 
                }
                if (L_C == 3)
                {
                    if (NTlist[0].Item1 == NodeType.Unkown && NodeType.Unkown == NTlist[1].Item1
                      && NodeType.Unkown == NTlist[2].Item1)
                    {
                        //全空=不为3层和1层
                        continue;
                    }else
                    {
                        //1 不空 则表示为1层
                        if (NTlist[0].Item1 == NodeType.Unkown && NodeType.Unkown != NTlist[1].Item1
                     && NodeType.Unkown == NTlist[2].Item1)
                        {

                            return new List<BranchNode>() { new BranchNode() {
                                LayerCount=1,
                             LayerNumber=1,
                             NodeIndex=NodeIndex,
                             NT=NTlist[1].Item1,
                             ClickPoint=new Point(RDic[1][1].X,RDic[1][1].Y), 
                                MapName=NTlist[1].Item2,} };
                        }else
                        {
                            var bnlist= new List<BranchNode>();
                            for (int n = 0; n < NTlist.Count; n++)
                            {
                                if(NTlist[n].Item1!= NodeType.Unkown)
                                bnlist.Add(new BranchNode()
                                {
                                    LayerCount = 3,
                                    LayerNumber = n+1,
                                    NodeIndex = NodeIndex,
                                    NT = NTlist[n].Item1,
                                    ClickPoint = new Point(RDic[3][n+1].X, RDic[3][n+1].Y),
                                    MapName = NTlist[n].Item2 ,

                                });
                            }
                            return bnlist;
                        }
                    }
                }else
                {
                    //LC=4
                    var bnlist = new List<BranchNode>();
                    if (NTlist[0].Item1 == NodeType.Unkown && NodeType.Unkown == NTlist[1].Item1
                      && NodeType.Unkown == NTlist[2].Item1 && NodeType.Unkown == NTlist[3].Item1)
                    {
                        return null;
                    }
                    else
                    {
                        if (NTlist[0].Item1 == NodeType.Unkown
                            && NodeType.Unkown == NTlist[3].Item1)
                        {
                            if(NTlist[1].Item1 != NodeType.Unkown)
                            {
                                bnlist.Add(new BranchNode()
                                {
                                    LayerCount = 2,
                                    LayerNumber = 1,
                                    NodeIndex = NodeIndex,
                                    NT = NTlist[1].Item1,
                                    ClickPoint = new Point(RDic[2][1].X, RDic[2][1].Y),
                                    MapName = NTlist[1].Item2,

                                });
                            }
                            if (NTlist[2].Item1 != NodeType.Unkown)
                            {
                                bnlist.Add(new BranchNode()
                                {
                                    LayerCount = 2,
                                    LayerNumber = 2,
                                    NodeIndex = NodeIndex,
                                    NT = NTlist[2].Item1,
                                    ClickPoint = new Point(RDic[2][2].X, RDic[2][2].Y),
                                    MapName = NTlist[2].Item2,

                                });
                            }
                            if (bnlist.Count == 0) return null;
                            return bnlist; 
                        }else
                        {
                            for (int n = 0; n < NTlist.Count; n++)
                            {
                                if (NTlist[n].Item1 != NodeType.Unkown)
                                    bnlist.Add(new BranchNode()
                                    {
                                        LayerCount = 4,
                                        LayerNumber = n + 1,
                                        NodeIndex = NodeIndex,
                                        NT = NTlist[n].Item1,
                                        ClickPoint = new Point(RDic[4][n+1].X, RDic[4][n+1].Y),
                                        MapName = NTlist[n].Item2,

                                    });
                            }
                            return bnlist;
                        }
                    }
                }
            }
            return null;
        }
        public static Dictionary<NodeType, ImageColor[,]> REC_Imgs;
        /// <summary>
        /// 节点序号,总层数,第几层
        /// </summary>
        static Dictionary<int, Dictionary<int, Dictionary<int, Rectangle>>> RectDic = new Dictionary<int, Dictionary<int, Dictionary<int, Rectangle>>>();
         

        static BranchNode()
        {
            String path=Environment.CurrentDirectory+ @"\imgs\rec_imgs\";
            REC_Imgs = new Dictionary<NodeType, ImageColor[,]>(); 
            REC_Imgs.Add(NodeType.不期而遇, ImageColor.FromFile(path + "不期而遇.png"));
            REC_Imgs.Add(NodeType.作战, ImageColor.FromFile(path + "作战.png"));
            REC_Imgs.Add(NodeType.紧急作战, ImageColor.FromFile(path + "紧急作战.png"));
            REC_Imgs.Add(NodeType.幕间余兴,   ImageColor.FromFile(path + "幕间余兴.png"));
            REC_Imgs.Add(NodeType.诡异行商,   ImageColor.FromFile(path + "诡异行商.png"));
            REC_Imgs.Add(NodeType.安全的角落,   ImageColor.FromFile(path + "安全的角落.png"));
            REC_Imgs.Add(NodeType.古堡馈赠,   ImageColor.FromFile(path + "古堡馈赠.png"));
            REC_Imgs.Add(NodeType.险路恶敌,   ImageColor.FromFile(path + "险路恶敌.png")); 
            RectDic.Add(1, new Dictionary<int, Dictionary<int, Rectangle>>());
            RectDic.Add(2, new Dictionary<int, Dictionary<int, Rectangle>>());
            RectDic.Add(3, new Dictionary<int, Dictionary<int, Rectangle>>());
            for(int i=1; i<=4;i++)
                RectDic[1].Add(i, new Dictionary<int, Rectangle>());
            for (int i = 1; i <= 4; i++)
                RectDic[2].Add(i, new Dictionary<int, Rectangle>()); 
            for (int i = 1; i <= 4; i++)
                RectDic[3].Add(i, new Dictionary<int, Rectangle>()); 
            int[] Node_X = new int[] { 0, REC_Node_X_FirstNode, REC_Node_X_Second, REC_Node_X_Third };
            for (int NodeIndex = 1; NodeIndex <= 3; NodeIndex++)
            {


                RectDic[NodeIndex][1].Add(1, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B_Y_dis, RECNode_Size.Width, RECNode_Size.Height));

                RectDic[NodeIndex][2].Add(1, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B3_TO_B2_Y_dis- B_Y_dis, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][2].Add(2, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B3_TO_B2_Y_dis, RECNode_Size.Width, RECNode_Size.Height));

                RectDic[NodeIndex][3].Add(1, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B_Y_dis * 2, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][3].Add(2, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B_Y_dis, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][3].Add(3, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y, RECNode_Size.Width, RECNode_Size.Height));

                RectDic[NodeIndex][4].Add(1, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B_Y_dis * 2 - B3_TO_B2_Y_dis, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][4].Add(2, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B_Y_dis - B3_TO_B2_Y_dis, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][4].Add(3, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y - B3_TO_B2_Y_dis, RECNode_Size.Width, RECNode_Size.Height));
                RectDic[NodeIndex][4].Add(4, new Rectangle(Node_X[NodeIndex], RECNode_B3_L3_Y + B3_TO_B2_Y_dis, RECNode_Size.Width, RECNode_Size.Height));  
            }
            //\imgs\rec_mapname
            var fss = new System.IO.DirectoryInfo(Environment.CurrentDirectory + @"\imgs\rec_mapname").GetFiles("*.png");
            ImageColor fontColor = new ImageColor( Color.White);
            ImageColor backColor =new ImageColor( Color.Black); 
            foreach (var fs in fss)
            {
                var name = fs.Name.Split('.')[0];
                MapNames.Add(name);
                var srcIC = ImageColor.FromFile(fs.FullName);
                /*ImageColor[,] ic = new ImageColor[REC_MapName_Size.Width, REC_MapName_Size.Height];
                for (int x = 0; x < REC_MapName_Size.Width; x++)
                    for (int y = 0; y < REC_MapName_Size.Height; y++)
                    {
                        var dis_font = Math.Abs(srcIC[x, y].R - fontColor.R) + Math.Abs(srcIC[x, y].G - fontColor.G) +
                            Math.Abs(srcIC[x, y].B - fontColor.B);
                        var dis_back = Math.Abs(srcIC[x, y].R - backColor.R) + Math.Abs(srcIC[x, y].G - backColor.G) +
                            Math.Abs(srcIC[x, y].B - backColor.B);
                        ic[x, y] = dis_font > dis_back ? backColor : fontColor;
                    }
                    Maps.Add(name, ic);
                */
                Maps.Add(name, srcIC);
            }

        }
        public static String GetMapName2(Bitmap srcBmp,Rectangle rect)
        {
            var newbmp=srcBmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ImageColor[,] srcIC = ImageColor.FromBitmap(newbmp);
            newbmp.Dispose();
           // ImageColor[,] ic = new ImageColor[REC_MapName_Size.Width, REC_MapName_Size.Height];
            ImageColor fontColor = new ImageColor(Color.White);
            ImageColor backColor = new ImageColor(Color.Black);
           /* for (int x = 0; x < REC_MapName_Size.Width; x++)
                for (int y = 0; y < REC_MapName_Size.Height; y++)
                {
                    var dis_font = Math.Abs(srcIC[x, y].R - fontColor.R) + Math.Abs(srcIC[x, y].G - fontColor.G) +
                        Math.Abs(srcIC[x, y].B - fontColor.B);
                    var dis_back = Math.Abs(srcIC[x, y].R - backColor.R) + Math.Abs(srcIC[x, y].G - backColor.G) +
                        Math.Abs(srcIC[x, y].B - backColor.B);
                    ic[x, y] = dis_font > dis_back ? backColor : fontColor;
                }*/
            Dictionary<string, double> map_dis = new Dictionary<string, double>();
            double min_dis = 9999999;
            String min_name = null;
            foreach (var kv in Maps)
            {
                var MapName = kv.Key;
                var dstIC = kv.Value;
                double dis = 0;
                for (int x = 0; x < REC_MapName_Size.Width; x++)
                    for (int y = 0; y < REC_MapName_Size.Height; y++)
                    {
                        var dis_pix = Math.Abs(srcIC[x, y].R - dstIC[x, y].R)
                            + Math.Abs(srcIC[x, y].G - dstIC[x, y].G) +
                            Math.Abs(srcIC[x, y].B - dstIC[x, y].B);
                        var dr = dstIC[x, y].R - srcIC[x, y].R;
                        var dg = dstIC[x, y].G - srcIC[x, y].G;
                        var db = dstIC[x, y].B - srcIC[x, y].B;
                        var delt = Math.Sqrt(3 * dr * dr + 4 * dg * dg + 2 * db * db) / 3d / 255d * 100;
                        dis += delt;
                        //dis += dis_pix; 
                    }
                dis /= REC_MapName_Size.Width * REC_MapName_Size.Height;
                map_dis.Add(MapName, dis);
                if(dis< min_dis)
                {
                    min_dis = dis;
                    min_name = MapName;
                }

            } 
           // if(min_dis<18)
            { 
                return min_name;
            }
           // else
            {
           //     return null;
            } 
        }
        public static String GetMapName(  Rectangle rect)
        {
            var bmp=mrfzGamePage.CatptureImg();
            var newbmp = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ImageColor[,] srcIC = ImageColor.FromBitmap(newbmp);
            newbmp.Dispose();
            bmp.Dispose();
            ImageColor[,] ic = new ImageColor[REC_MapName_Size.Width, REC_MapName_Size.Height];
            ImageColor fontColor = new ImageColor(Color.White);
            ImageColor backColor = new ImageColor(Color.Black);
            for (int x = 0; x < REC_MapName_Size.Width; x++)
                for (int y = 0; y < REC_MapName_Size.Height; y++)
                {
                    //back
                    if((srcIC[x, y].G + srcIC[x, y].B + srcIC[x, y].R)/3<=20)
                    {

                    }else
                    {
                        //font
                    }
                    var dis_font = Math.Abs(srcIC[x, y].R - fontColor.R) + Math.Abs(srcIC[x, y].G - fontColor.G) +
                        Math.Abs(srcIC[x, y].B - fontColor.B);
                    var dis_back = Math.Abs(srcIC[x, y].R - backColor.R) + Math.Abs(srcIC[x, y].G - backColor.G) +
                        Math.Abs(srcIC[x, y].B - backColor.B);
                    ic[x, y] = dis_font > dis_back ? backColor : fontColor;
                }
            Dictionary<string, int> map_dis = new Dictionary<string, int>();
            int min_dis = 9999999;
            String min_name = null;
            foreach (var kv in Maps)
            {
                var MapName = kv.Key;
                var dstIC = kv.Value;
                int dis = 0;
                int pix_font_count = 0;
                for (int x = 0; x < REC_MapName_Size.Width; x++)
                    for (int y = 0; y < REC_MapName_Size.Height; y++)
                    {
                        //当前截图是backColor,灰度值小于20
                        if ((srcIC[x, y].G + srcIC[x, y].B + srcIC[x, y].R) / 3 <= 20)
                        {
                            //地图 backColor
                            if ((dstIC[x, y].G + dstIC[x, y].B + dstIC[x, y].R) / 3 <= 20)
                            {
                                //
                            }else
                            {
                                //有差异
                                dis++;
                                pix_font_count++;
                            }
                        }
                        else
                        {
                            //font
                            //地图 backColor
                            if ((dstIC[x, y].G + dstIC[x, y].B + dstIC[x, y].R) / 3 <= 20)
                            {
                                ////有差异
                                dis++;
                            }
                            else
                            {
                                pix_font_count++;
                            }
                        }
                        
                    } 
                dis = dis/(pix_font_count) * 100;
                map_dis.Add(MapName, dis);
                if (dis < min_dis)
                {
                    min_dis = dis;
                    min_name = MapName;
                }

            }
            var md = min_dis;
            return min_name;
        }
        public static List<String> MapNames = new List<string>();
        public static Dictionary<String, ImageColor[,]> Maps = new Dictionary<string, ImageColor[,]>();
        public static void SaveNode()
        {
            Rectangle rec = new Rectangle(REC_Node_X_Third,
            RECNode_B3_L3_Y   ,
            RECNode_Size.Width, RECNode_Size.Height);
            Bitmap src = new Bitmap(@"F:\VS2019Project\MRFZ_Auto\bin\Debug\imgs\adb_雷电模拟器\181.png");
            var B2JINGJI = src.Clone(rec, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            B2JINGJI.Save(Environment.CurrentDirectory + @"\imgs\rec_imgs\安全的角落.png");
            B2JINGJI.Dispose();
        }

         
       
    }
}
