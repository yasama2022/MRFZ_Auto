using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 
using lib;
using script;
using lib.image; 
using GamePageScript.script.mrfz;
using GamePageScript.lib;
using System.Reflection;
using static GamePageScript.script.mrfz.mrfz_ScriptUnit;
using MRFZ_Auto.script.mrfz;
using System.Threading;
using MRFZ_Auto.ui;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script;

namespace GamePageScript.ui
{
    public partial class GamePageCreator : Form
    {
        List<FileInfo> ImgFileList = new List<FileInfo>();
        int index = -1;
        state State = state.None;
        private Bitmap CurBitmap;
        private Rectangle Rect;
        public mrfzGamePage CurGamePage
        {
            get
            {
                if (index >= mrfzGamePage.gamePages.Count)
                {
                    index = mrfzGamePage.gamePages.Count - 1;
                }
                if (index < 0) index = 0;
                if (mrfzGamePage.gamePages.Count <= 0) return null;
                return mrfzGamePage.gamePages[index];
            }
            set
            {
                if (index >= mrfzGamePage.gamePages.Count)
                {
                    index = mrfzGamePage.gamePages.Count - 1;
                }
                if (index < 0) index = 0;
                mrfzGamePage.gamePages[index] = value;
            }
        }
        public enum state
        {
            None,
            RecPoint,
            ClickToNext,
            RecRect,
            RegionBlock,
        }
        private void EnableRadio()
        {
            this.radioButton1.Enabled = true;
            this.radioButton2.Enabled = true;
            this.radioButton3.Enabled = true;
        }
        private void DisabledRadio()
        {
            this.radioButton1.Enabled = false;
            this.radioButton2.Enabled = false;
            this.radioButton3.Enabled = false;
        }
        public GamePageCreator()
        {
            InitializeComponent();
            InitUI(); 
            DisabledRadio();
            LoadGamePages();
            //GamePage. Load();
            this.SetStyle(ControlStyles.DoubleBuffer |

         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint,
         true);
            this.UpdateStyles(); 
            
            cvs.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).
                SetValue(cvs, true, null);
            adb_list.SetData(null, null);
        }
        private void InitUI()
        {
           // cvs.MouseDown += PictureBox1_MouseDown;
           // cvs.MouseMove += PictureBox1_MouseMove;
          //  cvs.MouseUp += PictureBox1_MouseUp; 
            listbox_NextPages.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listbox_regions.SelectedIndexChanged += Listbox_regions_SelectedIndexChanged;
            listbox_NextPages.MouseDoubleClick += ListBox1_MouseClick;
            listbox_regions.MouseDoubleClick += Listbox_regions_MouseDoubleClick; ;
            tb_pageName.LostFocus += tb_pageName_LostFocus;
            tb_clickname.LostFocus += tb_clickname_LostFocus;
            tb_region_name.LostFocus += Tb_region_name_LostFocus;
            cblist_page_type.SelectedIndex = 0;
            cblist_page_catgory.SelectedIndex = 0;
            cb_紧急.Enabled = false;
            cblist_page_type.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            cblist_page_catgory.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            tb_mapname.Text = "";
            tb_mapname.LostFocus += Tb_mapname_LostFocus;
            cb_紧急.CheckedChanged += CheckBox2_CheckedChanged;
            radioButton1.CheckedChanged += RadioButton1_CheckedChanged;
            radioButton2.CheckedChanged += RadioButton1_CheckedChanged;
            radioButton3.CheckedChanged += RadioButton1_CheckedChanged;
        }

        private void Listbox_regions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var dr = MessageBox.Show(this, "是否删除此项", "是否删除此项", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No) return;
                if (listbox_regions.SelectedIndex >= 0)
                {
                    if (CurGamePage.regions.Count > listbox_regions.SelectedIndex)
                    { 
                        File.Delete(Environment.CurrentDirectory+ @"\imgs\region_imgs" + CurGamePage.regions[listbox_regions.SelectedIndex].FileName); 
                        CurGamePage.regions.RemoveAt(listbox_regions.SelectedIndex);
                        listbox_regions.DataSource = null;
                        listbox_regions.DataSource = CurGamePage.regions;
                        if (CurGamePage.regions.Count > 0)
                            listbox_regions.SelectedIndex = 0;
                        S();
                    }
                }
            }
        }

        private void Listbox_regions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurGamePage != null && CurGamePage.regions .Count>0)
            {
                if (CurGamePage.regions.Count > listbox_regions.SelectedIndex && listbox_regions.SelectedIndex >= 0)
                {
                    var CNP = CurGamePage.regions[listbox_regions.SelectedIndex];
                    tb_region_name.Text = CNP.Name; 
                    S();
                }
            }
        }

        private void Tb_region_name_LostFocus(object sender, EventArgs e)
        { 
            if(CurGamePage?.regions.Count>0)
            {
                if(listbox_regions.SelectedIndex>=0&&listbox_regions.SelectedIndex< CurGamePage?.regions.Count)
                {
                    CurGamePage.regions[listbox_regions.SelectedIndex].Name= tb_region_name.Text;
                    listbox_regions.DataSource = null;
                    listbox_regions.DataSource = CurGamePage.regions;
                    S();
                }
            }
        }

     
        private String SaveRCImg()
        {
            //\imgs\rcimg
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory+ @"\imgs\rcimg");
            if (!di.Exists) di.Create();
            String FileName = di.FullName + $"\\{index}_{CurGamePage.RecPageRectColors.Count}.png";
            var rcimg=CurBitmap.Clone(Rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            rcimg.Save(FileName,System.Drawing.Imaging.ImageFormat.Png);
            return $"\\{index}_{CurGamePage.RecPageRectColors.Count}.png";
        }
        private String SaveRegionImg()
        {
            //\imgs\rcimg
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + @"\imgs\region_imgs");
            if (!di.Exists) di.Create();
            String FileName = di.FullName + $"\\{index}_{CurGamePage.regions.Count}.png";
            var rcimg = CurBitmap.Clone(Rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            rcimg.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
            return $"\\{index}_{CurGamePage.regions.Count}.png";
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (index < 0) return;
            if (State == state.RecPoint || State == state.ClickToNext)
            {

            }
            else
            {
                if (State == state.RecRect&& DownKey)
                {
                    ed = new Point(e.Location.X, e.Location.Y);  
                    Rect = GetRect(st, ed);
                    DrawCVS(); 
                }
            }
        }
        private Point st;
        private Point ed;
        private Boolean DownKey = false;
        private Rectangle GetRect(Point p1,Point p2)
        {
            int x = 0, y = 0, w = 0, h = 0;
            x = Math.Min(p1.X, p2.X);
            y = Math.Min(p1.Y, p2.Y);
            w = Math.Abs(p1.X - p2.X);
            h= Math.Abs(p1.Y - p2.Y);
            return new Rectangle(x, y, w, h);
        }
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (index < 0) return;
            if (State == state.RecPoint || State == state.ClickToNext)
            {

            }
            else
            {
                if (State == state.RecRect)
                {
                    DownKey = true;
                    st =new Point( e.Location.X,e.Location.Y);
                    ed = new Point(e.Location.X, e.Location.Y); 
                    Rect = GetRect(st, ed);
                    DrawCVS();
                   // CurBitmap
                        //Bitmap bmp = pictureBox1.BackgroundImage as Bitmap;
                    //var P = e.Location;
                }
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        { 
            if(radioButton1.Checked)
            {
                CurGamePage.mapType = mrfzGamePage.MapType.初始;
            }
            else if (radioButton2.Checked)
            {
                CurGamePage.mapType = mrfzGamePage.MapType.放置;

            }else
            {

                CurGamePage.mapType = mrfzGamePage.MapType.技能;
            }
            S();
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        { 
            S();
        }

        private void Tb_mapname_LostFocus(object sender, EventArgs e)
        {
            S(); 
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cblist_page_catgory.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    cb_紧急.Enabled = false;
                    break;
            }
            S();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { 
            switch(cblist_page_catgory.SelectedIndex)
            {
                case 0:
                    this.DisabledRadio();
                    cb_紧急.Enabled = false;
                    tb_mapname.Enabled = false;
                    break;
                case 1:
                case 2:
                    this.EnableRadio();
                    tb_mapname.Enabled = true;
                    if (cblist_page_type.SelectedIndex==0)
                    {

                        cb_紧急.Enabled = true;
                    }else
                        cb_紧急.Enabled = false;
                    break;
            }

            S();
        }

        private void LoadGamePages()
        {
            textBox2.Text = Environment.CurrentDirectory + "\\imgs\\adb_雷电模拟器";
            var di = new DirectoryInfo(textBox2.Text);
            var fss = di.GetFiles("*.png");
            ImgFileList = new List<FileInfo>(fss);
            ImgFileList.Sort((a, b) => { return int.Parse(a.Name.Split('.')[0]) - int.Parse(b.Name.Split('.')[0]); });
            if(mrfzGamePage.gamePages.Count< ImgFileList.Count)
            {
                for(int i= mrfzGamePage.gamePages.Count; i< ImgFileList.Count; i++)
                {
                    Bitmap bmp = new Bitmap(ImgFileList[i].FullName);
                    mrfzGamePage.gamePages.Add(new mrfzGamePage(i,
                        "\\imgs\\adb_雷电模拟器\\"+ ImgFileList[i].Name,new Size(bmp.Width,bmp.Height)));
                    bmp.Dispose();
                }
                mrfzGamePage.Save();
            }

            if (mrfzGamePage.gamePages.Count > 0)
            {
                index = 0;
                LoadCurGamePage();
            } 

        }
        private void tb_clickname_LostFocus(object sender, EventArgs e)
        {
            if (CurGamePage.NextPages.Count > listbox_NextPages.SelectedIndex)
            {
                //  var CNP = CurGamePage.NextPages[listBox1.SelectedIndex];
                //CurGamePage.NextPages.RemoveAt(listBox1.SelectedIndex);
                if (CurGamePage.NextPages.Count > 0)
                {
                    if (listbox_NextPages.SelectedIndex >= 0)
                    {
                       
                        CurGamePage.NextPages[listbox_NextPages.SelectedIndex].ClickName = tb_clickname.Text;

                    }
                }
                listbox_NextPages.DataSource = null;
                listbox_NextPages.DataSource = CurGamePage.NextPages;
                // if (CurGamePage.NextPages.Count > 0)
                //    listBox1.SelectedIndex = 0; 
            }
            else
            {

            }
            S();
           // LoadCurGamePage_Data();

        }

        private void tb_pageName_LostFocus(object sender, EventArgs e)
        {
            S();
        }
        enum SaveType
        {
            NONE,
            CLICKNAME,
            NAME,
        } 
        private void S(  )
        {
            if (Loading) return;
            if(CurGamePage!=null)
            {
                CurGamePage.Name = tb_pageName.Text;
                if (CurGamePage.NextPages.Count > 0)
                {
                    if (listbox_NextPages.SelectedIndex >= 0)
                    {
                      //  var CNP = CurGamePage.NextPages[listbox_NextPages.SelectedIndex];
                        CurGamePage.NextPages[listbox_NextPages.SelectedIndex].ClickName = tb_clickname.Text;
                    }
                }
                if (CurGamePage.regions.Count > 0)
                {
                    if (listbox_regions.SelectedIndex >= 0)
                    {
                        //  var CNP = CurGamePage.NextPages[listbox_NextPages.SelectedIndex];
                        CurGamePage.regions[listbox_regions.SelectedIndex].Name = tb_region_name.Text;
                    }
                }
                CurGamePage.Baned = cb_ban.Checked;
                CurGamePage.MapName = tb_mapname.Text;
                CurGamePage.mapType = mrfzGamePage.MapType.初始;
                if (radioButton2.Checked)
                    CurGamePage.mapType = mrfzGamePage.MapType.放置;
                if (radioButton3.Checked)
                    CurGamePage.mapType = mrfzGamePage.MapType.技能;
                CurGamePage.紧急 = cb_紧急.Checked; 
                CurGamePage.pageType = (mrfzGamePage.PageType)cblist_page_type.SelectedIndex;
                CurGamePage.pageCatgory = (mrfzGamePage.PageCatgory)cblist_page_catgory.SelectedIndex;
                mrfzGamePage.Save(); 
            }
            
                  
        }
       
        
        private void ListBox1_MouseClick(object sender, MouseEventArgs e)
        { 
            if(e.Button== MouseButtons.Left)
            {
                var dr=MessageBox.Show(this, "是否删除此项", "是否删除此项", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No) return;
                if(listbox_NextPages.SelectedIndex>=0)
                {
                    if (CurGamePage.NextPages.Count > listbox_NextPages.SelectedIndex)
                    { 
                      //  var CNP = CurGamePage.NextPages[listBox1.SelectedIndex];
                        CurGamePage.NextPages.RemoveAt(listbox_NextPages.SelectedIndex);
                        listbox_NextPages.DataSource = null;
                        listbox_NextPages.DataSource = CurGamePage.NextPages; 
                        if (CurGamePage.NextPages.Count > 0)
                            listbox_NextPages.SelectedIndex = 0;
                        S();
                    }
                }
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if(CurGamePage!=null&& CurGamePage.NextPages!=null)
            {
                if(CurGamePage.NextPages.Count>listbox_NextPages.SelectedIndex&& listbox_NextPages.SelectedIndex >= 0)
                {
                    var CNP = CurGamePage.NextPages[listbox_NextPages.SelectedIndex];
                    tb_clickname.Text = CNP.ClickName;
                   
                    S();
                }
            }
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
        private bool Loading = false;
        public void LoadCurGamePage()
        {
            //load img 
            Loading = true;
            this.State = state.RecPoint;
            if(CurBitmap!=null)
            {
                CurBitmap.Dispose();
            }
            CurBitmap = new Bitmap(Environment.CurrentDirectory+ CurGamePage.ImgFile);
            this.cvs.BackgroundImageLayout = ImageLayout.Stretch;
          //  this.cvs.Width = CurBitmap.Width;
          //  this.cvs.Height =  CurBitmap.Height;  
            DrawCVS();
            textBox4.Text = index.ToString();
            label5.Text = index.ToString(); 
            cb_ban.Checked = CurGamePage.Baned;
            LoadCurGamePage_Data();
            Loading = false;

        }
        public void DrawCVS()
        { 
            Bitmap bmp = new Bitmap(1280, 720);
            var g=Graphics.FromImage(bmp); 
            g.DrawImage(CurBitmap, 0, 0);
            foreach(var cnp in CurGamePage.NextPages)
            {
              
               g.DrawString( cnp.ClickName,new Font("宋体", 24,FontStyle.Bold),Brushes.Red,  cnp.ClickPoint);
            }
            foreach (var cnp in CurGamePage.RecPageRectColors)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(127, 0, 122, 204)), cnp.rect);
                g.DrawString("Rec", new Font("宋体", 24, FontStyle.Regular), Brushes.Red, cnp.rect.Location);
            }
            foreach (var cnp in CurGamePage.regions)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(127,244,197,31)), cnp.rect);
                g.DrawString(cnp.Name, new Font("宋体", 24, FontStyle.Regular), Brushes.Red, cnp.rect.Location);
            }
            /*if(State== state.RecRect)
            {
                g.DrawRectangle(Pens.White, Rect);
            }*/
            cvs.BackgroundImageLayout = ImageLayout.Stretch;
             cvs.BackgroundImage = bmp;
            // g.Dispose();
        }
        private void LoadCurGamePage_Data()
        {
            tb_pageName.Text = CurGamePage.Name;
            tb_REC_PPS.Text = "";
            tb_REC_PPS.ReadOnly = true;
            tb_rcs.Text = "";
            tb_rcs.ReadOnly = true;
            foreach (var pp in CurGamePage.RecPagePPs)
            {
                tb_REC_PPS.AppendText(pp.ToString() + "\r\n");
            }
            foreach (var rcs in CurGamePage.RecPageRectColors)
            {
                tb_rcs.AppendText(rcs.ToString() + "\r\n");
            }
            listbox_NextPages.DataSource = null;
            listbox_NextPages.DataSource = CurGamePage.NextPages;
            if(CurGamePage.NextPages.Count>0)
            {
                listbox_NextPages.SelectedIndex = 0;
                tb_clickname.Text = CurGamePage.NextPages[0].ClickName;
            }
            listbox_NextPages.DisplayMember = "Name";
            listbox_regions.DisplayMember = "Name";
            listbox_regions.DataSource = null;
            listbox_regions.DataSource = CurGamePage.regions;
            if (CurGamePage.regions.Count > 0)
            {
                listbox_regions.SelectedIndex = 0; 
                tb_region_name.Text =  CurGamePage.regions[0].Name;
            }
            cblist_page_type.SelectedIndex = (int)(CurGamePage.pageType);
            cblist_page_catgory.SelectedIndex = (int)(CurGamePage.pageCatgory);
            tb_mapname.Text = CurGamePage.MapName;
            cb_紧急.Checked = CurGamePage.紧急;
            switch(CurGamePage.mapType)
            {
                case mrfzGamePage.MapType.初始:
                    radioButton1.Checked = true;
                    break;
                case mrfzGamePage.MapType.放置:
                    radioButton2.Checked = true;
                    break;
                case mrfzGamePage.MapType.技能:
                    radioButton3.Checked = true;
                    break;
            } 
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int o = -1;
              bool isNumber= int.TryParse(textBox4.Text,out o);
                if(!isNumber)
                {
                    for (o = 0; o < mrfzGamePage.gamePages.Count; o++)
                    { 
                        if(mrfzGamePage.gamePages[o].Name==textBox4.Text)
                        {
                            this.index = o;
                            LoadCurGamePage();
                            return;
                        }
                              
                     }
                    throw new Exception("");
                }
                else
                {
                    if (o > 0 && o < ImgFileList.Count)
                    {
                        this.index = o;
                        LoadCurGamePage();
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                 
            }catch
            {
                MessageBox.Show("定位失败,序号超范围或者不存在Name相等的page");
            }
        }

        private void btn_pre_click(object sender, EventArgs e)
        {
            S();
            int cur = index;
            while (index-->=0)
            {
                if (index < 0)
                {
                    index = cur;
                    break;
                }
                if (!mrfzGamePage.gamePages[index].Baned)
                {
                    if (cb_battle.Checked)
                    {
                        if (mrfzGamePage.gamePages[index].pageCatgory
                            == mrfzGamePage.PageCatgory.战斗地图)
                        {
                            LoadCurGamePage();
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }else
                    {

                        LoadCurGamePage();
                    } 
                    break;
                }else
                {
                    continue;
                }
            }
            

        }

        private void BTN_NEXT_CLICK(object sender, EventArgs e)
        {
            S();
            int cur = index;
            while (index++ <= mrfzGamePage.gamePages.Count)
            {
                if (index >= mrfzGamePage.gamePages.Count)
                {
                    index = cur;
                    break;
                }
                if (!mrfzGamePage.gamePages[index].Baned)
                {
                    if (cb_battle.Checked)
                    {
                        if(mrfzGamePage.gamePages[index].pageCatgory
                            == mrfzGamePage.PageCatgory.战斗地图)
                        {
                            LoadCurGamePage();
                            break;
                        }else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        LoadCurGamePage();
                        break;
                    }
                }else
                {
                    continue;
                }
            }
             
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.State = state.RecPoint;
            PictureBoxWin win = new PictureBoxWin(this.CurBitmap, State);
            var dr = win.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //var col = CurBitmap.GetPixel(win.result.p.X, win.result.p.Y);
               // PP pp = new PP() { color = new ImageColor(col), loc = win.result.p };
                CurGamePage.RecPagePPs.Add(win.result.pp); 
                S();
                LoadCurGamePage_Data();
            }
            win.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.State = state.ClickToNext;
            PictureBoxWin win = new PictureBoxWin(this.CurBitmap, State);
            var dr=win.ShowDialog();
            if(dr== DialogResult.OK)
            { 
                CurGamePage.NextPages.Add(new ClickToNextPage() { ClickPoint = win.result.p, ClickName = "NEXT" });
                listbox_NextPages.DataSource = null;
                listbox_NextPages.DataSource = CurGamePage.NextPages;
                listbox_NextPages.DisplayMember = "Name";

                S();
                LoadCurGamePage_Data();
            }
            win.Dispose();
        }
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (index < 0) return;
            if (State == state.RecPoint || State == state.ClickToNext)
            {

                var P = e.Location;
                switch (State)
                {
                    case state.RecPoint:
                        var col = CurBitmap.GetPixel(P.X, P.Y);
                        PP pp = new PP() { color = new ImageColor(col), loc = P };
                        CurGamePage.RecPagePPs.Add(pp);
                        break;
                    case state.ClickToNext:
                        CurGamePage.NextPages.Add(new ClickToNextPage() { ClickPoint = P, ClickName = "NEXT" });
                        listbox_NextPages.DataSource = null;
                        listbox_NextPages.DataSource = CurGamePage.NextPages;
                        listbox_NextPages.DisplayMember = "Name";
                        break;
                }
                S();
                LoadCurGamePage_Data();
            }
            else
            {
                if (State == state.RecRect)
                {
                    // CurBitmap
                    ed = new Point(e.Location.X, e.Location.Y);
                    Rect = GetRect(st, ed);
                    if (regionType == RegionType.RecPage)
                    {
                        var fn = SaveRCImg();
                        this.CurGamePage.RecPageRectColors.Add(new RectColor(Rect, fn, fn));
                    }
                    else
                    {
                        var fn = SaveRegionImg();
                        this.CurGamePage.regions.Add(new RectColor(Rect, fn, fn));
                    }
                    //DO 
                    S();
                    LoadCurGamePage_Data();
                    DownKey = false;
                    this.State = state.None;
                    DrawCVS();

                }
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.CurGamePage.RecPagePPs.Clear();
            LoadCurGamePage_Data();
        }

        

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void GamePageCreator_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        { 
            S();
        }

        private void btn_savename_Click(object sender, EventArgs e)
        {
            S();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            lb_tips.Text = "识别中,,,"  ; 
            var flag = this.CurGamePage.CheckPage();
            lb_tips.Text = "识别" +( flag ? "成功" : "失败");

        }

        private const int W = 1200, H = 705;
        protected ClickToNextPage SearchClickName(List<ClickToNextPage> list, String Name)
        {
            foreach (var cnp in list)
            {
                if (cnp.ClickName == Name)
                {
                    return cnp;
                }
            }
            return null;
        }
        public void EXC_BATTLE_MAP(String name1, String name2, String name3)
        {
            var M1 = mrfzGamePage.GamePageDic[name1];
            var M2 = mrfzGamePage.GamePageDic[name2];
            var M3 = mrfzGamePage.GamePageDic[name3];
            if (M1.IsCurPage())
            {

                var list = M1.NextPages;
                var x2 = SearchClickName(list, $"2X");
                adb.Tap(x2.ClickPoint);
                System.Threading.Thread.Sleep(300);
                List<ClickToNextPage> P_List = new List<ClickToNextPage>();
                for (int i = 1; i < 5; i++)
                {

                    var c = SearchClickName(list, $"P{i}");
                    if (c == null) break;
                    P_List.Add(c);
                }
                List<ClickToNextPage> PW_List = new List<ClickToNextPage>();
                for (int i = 1; i < 5; i++)
                {

                    var c = SearchClickName(list, $"PW{i}");
                    if (c == null) break;
                    PW_List.Add(c);
                }
                adb.Tap(P_List[0].ClickPoint);

                if (M2.CheckPage())
                {
                    list = M2.NextPages;
                    List<ClickToNextPage> M2_P_List = new List<ClickToNextPage>();
                    for (int i = 1; i < 5; i++)
                    {

                        var c = SearchClickName(list, $"P{i}");
                        if (c == null) break;
                        M2_P_List.Add(c);
                    }
                    List<ClickToNextPage> M2_PW_List = new List<ClickToNextPage>();
                    for (int i = 1; i < 5; i++)
                    {

                        var c = SearchClickName(list, $"PW{i}");
                        if (c == null) break;
                        M2_PW_List.Add(c);
                    }
                    for (int i = 0; i < M2_PW_List.Count; i++)
                    {
                        adb.Swipe(M2_P_List[0].ClickPoint, M2_PW_List[i].ClickPoint, 500);
                        adb.Swipe(M2_PW_List[i].ClickPoint, new Point(M2_PW_List[i].ClickPoint.X + 300, M2_PW_List[i].ClickPoint.Y), 500);
                        for (int t = 0; t <= 3; t++)
                        {

                            System.Threading.Thread.Sleep(1000);
                        }
                        adb.Tap(PW_List[i].ClickPoint);
                        System.Threading.Thread.Sleep(500);

                        if (M3.CheckPage())
                        {
                            list = M3.NextPages;
                            List<ClickToNextPage> M3_SKILLS = new List<ClickToNextPage>();
                            for (int v = 1; v < 5; v++)
                            {

                                var c = SearchClickName(list, $"SKILL{v}");
                                if (c == null) break;
                                M3_SKILLS.Add(c);
                            }
                            adb.Tap(M3_SKILLS[i].ClickPoint);
                            return;
                        }
                    }
                }
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            EXC_BATTLE_MAP("","","");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.CurGamePage.RecPageRectColors.Clear();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory+ @"\imgs\rcimg");
            if(di.Exists)
            {
                var fss=di.GetFiles($"{index}_*.png");
                foreach (var fs in fss) fs.Delete();
            }
            LoadCurGamePage_Data();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            State = state.RecRect;
            regionType = RegionType.RecPage;
            PictureBoxWin win = new PictureBoxWin(this.CurBitmap, State);
            var dr = win.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (State == state.RecRect)
                {
                    // CurBitmap
                    ed = win.ed;
                    st = win.st;
                    Rect = win.result.rect;
                    if(Rect.Width==0||Rect.Height==0)
                    {
                        return;
                    }
                    //GetRect(st, ed);
                    if (regionType == RegionType.RecPage)
                    {
                        var fn = SaveRCImg();
                        this.CurGamePage.RecPageRectColors.Add(new RectColor(Rect, fn, fn));
                    }
                    else
                    {
                        var fn = SaveRegionImg();
                        this.CurGamePage.regions.Add(new RectColor(Rect, fn, fn));
                    }
                    //DO 
                    S();
                    LoadCurGamePage_Data();
                    DownKey = false;
                    this.State = state.None;
                   // DrawCVS();

                }
            }
            win.Dispose();

        }
        RegionType regionType;
        public enum RegionType
        {
            Region,
            RecPage,
        }

        private void tb_clickname_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

            State = state.RecRect;
            regionType = RegionType.Region;
            PictureBoxWin win = new PictureBoxWin(this.CurBitmap, State);
            var dr = win.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (State == state.RecRect)
                {
                    // CurBitmap
                    ed = win.ed;
                    st = win.st;
                    Rect = win.result.rect;
                    //GetRect(st, ed);
                    if (regionType == RegionType.RecPage)
                    {
                        var fn = SaveRCImg();
                        this.CurGamePage.RecPageRectColors.Add(new RectColor(Rect, fn, fn));
                    }
                    else
                    {
                        var fn = SaveRegionImg();
                        this.CurGamePage.regions.Add(new RectColor(Rect, fn, fn));
                    }
                    //DO 
                    S();
                    LoadCurGamePage_Data();
                    DownKey = false;
                    this.State = state.None;
                    // DrawCVS();

                }
            }
            win.Dispose();

        }
        public Boolean CalcRgeion_1(Bitmap srcBmp,Bitmap region1_bmp, Rectangle rect, out double AvgDelta)
        {
            AvgDelta = -1;
           
            String FileName = null;
            try
            {
                 
                  // bmp.Save(FileName);
                //  bmp = adb.ShotCutRunning(FileName);
                 
                var ic = ImageColor.FromBitmap(region1_bmp);
                var rc = rect;
                Bitmap rcbmp = srcBmp.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                rcbmp.Save("D:\\rg.png");
                var rcbmp_ic = ImageColor.FromBitmap(rcbmp);
                var sum_delt = 0d;
                for (int x = 0; x < rc.Width; x++)
                    for (int y = 0; y < rc.Height; y++)
                    {
                        var ori = ic[x , y ];
                        var dst = rcbmp_ic[x, y];
                        var dr = dst.R - ori.R;
                        var dg = dst.G - ori.G;
                        var db = dst.B - ori.B;
                        var delt = Math.Sqrt(dr * dr + dg * dg + db * db);
                        sum_delt += delt;
                    }
                AvgDelta = sum_delt / rc.Width / rc.Height;
                rcbmp.Dispose();
                var D_MAX = 30;
                if (AvgDelta > D_MAX)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                //srcBmp?.Dispose();
            }

        }
        private void CostFont()
        {
            var fontdi = Environment.CurrentDirectory + "\\imgs\\cost_font\\";
        //    adb.devices();
            Thread th = new Thread(() =>
              { 
                  
                  int C = 0;
                  var first = DateTime.Now;
                  while (true)
                  {
                      var  dt = first.AddSeconds(C);
                      while (true)
                      {
                          if (DateTime.Now >= dt)
                          {
                              break;
                          }
                          Thread.Sleep(20);
                      }
                      var bmp = mrfzGamePage.CatptureImg();
                      bmp.Save(fontdi + C.ToString() + ".png");
                      C++;
                      bmp.Dispose();
                      if (C >= 100)
                      {
                          break;
                      }
                  }

              });
            th.Start();
            return;
            var point = new Point(1230, 498);
            String fontfile =Environment.CurrentDirectory+ "\\imgs\\adb_雷电模拟器\\75.png"; 
            Size fontsize = new Size(32, 36);
            var point_2 = new Point(point.X-fontsize.Width, point.Y);
            Bitmap src = new Bitmap(fontfile);
            var nb=src.Clone(new Rectangle(point, fontsize), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            nb.Save(fontdi + "75_0.png");
            nb.Dispose();
           var nb2 = src.Clone(new Rectangle(point_2, fontsize), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            nb2.Save(fontdi + "75_1.png");
            nb2.Dispose();
            src.Dispose();
        }
        protected void wait(int ms)
        {
            if (ms > 1000)
            {
                int t = ms / 1000;
                for (int i = 0; i < t; i++)
                {

                    Thread.Sleep(ms);

                }
                if (ms - t * 1000 > 0)
                    Thread.Sleep(ms - t * 1000);
            }
            else
                Thread.Sleep(ms);
        }
        protected   bool PageClick(String PageName, String ClickName, Boolean Valid = false)
        {
            if (Valid)
            {
                if (mrfzGamePage. GamePageDic[PageName].IsCurPage())
                {
                    var CNP = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, ClickName);
                    if (CNP == null) { { 
                            return false; 
                        } }
                    ClickPage(CNP);
                    wait(200);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var CNP = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, ClickName);
                if (CNP == null) { { var ss = $" 找不到{PageName} {ClickName}点击数据"; } }
                ClickPage(CNP);
                wait(200);
             //   this.onMsg?.Invoke($"page:{PageName} clickname:{ClickName}");
                return true;
            }


        }
       
        protected   void ClickPage(ClickToNextPage cnp)
        {
            adb.Tap(cnp.ClickPoint);
            wait(100);
        }
        protected void PageSwipe(String PageName, String startPoint, String EndPoint, int ms = 500)
        {
            var ST = SearchClickName(mrfzGamePage. GamePageDic[PageName].NextPages, startPoint);
            var ED = SearchClickName(mrfzGamePage.GamePageDic[PageName].NextPages, EndPoint);
            if (ST == null || ED == null)
            {
               
                return;
            }
            adb.Swipe(ST.ClickPoint, ED.ClickPoint, ms);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap(@"D:\XX.png");
            var bxxxsdasdsa = this.CurGamePage.IsCurPage(bmp);

            return;
            Thread th = new Thread(() =>
              {
                  mrfz_ScriptUnit uu = new mrfz_ScriptUnit(TaskType.Rogue_KuiYing);
                  uu.Deal幕间余兴();
              });
            th.Start();
            
            return; 
            var ptr =Win.FindWindow("夜神模拟器", "Qt5QWindowIcon");
            if(ptr!=null&& ptr.ToInt32()!=0)
            {
                Win.SetWindowSize(ptr, 0, 0, 1280 + 40+2, 720 + 32+1);
                Thread.Sleep(300);
                var bmps = Win.GetWindowClientCapture(ptr, 2, 32, 1280, 720);
                bmps .Save(@"D:\\x.png");
                var bxxx = this.CurGamePage.IsCurPage(bmps);
                return;
            }
            return;
            Bitmap src = new Bitmap(@"D:\Program Files (x86)\Tencent\QQ\459900600\FileRecv\最后一张图片 (8).png");
           var b= this.CurGamePage.IsCurPage(src);
            return;
            
            var bcr = BattleCharRec.CharRecNow(false,src);
            mrfz_ScriptConfig.scriptConfig.mainChar = mrfz_ScriptConfig.ArkChar.煌;
            if (!bcr.Char_PointIndex.ContainsKey(mrfz_ScriptConfig.scriptConfig.mainChar))
            {

                return;
            }else
            {
                return;
            }
            //var bmp=  new Bitmap(@"D:\SD.png").Clone(new Rectangle(1,34,1280,720), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //  mrfzGamePage.GamePageDic["shop"].IsCurPage(false, bmp);
            //   mrfzGamePage.GamePageDic["update_1"].IsCurPage(false, bmp);
            return;
            PageClick("驯兽小屋2", "P2", false);
            wait(200);
            PageSwipe("驯兽小屋2", "P2", $"PW1", 1500);
            return;
            /*
            X:
P2-452 , P2,P2+451
。。。
Y:
P2,
P2+199
P2+397
                */ 
            RectColor[] P= new RectColor[10];
           P[4]= mrfzGamePage.GamePageDic["助战-山皮肤1-P4"].regions[0];
           P[1] = new RectColor(new Rectangle(P[4].rect.X-452, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height),
              P[4].FileName,
                "P1");
           P[7] = new RectColor(new Rectangle(P[4].rect.X+451, P[4].rect.Y, P[4].rect.Width, P[4].rect.Height), 
                P[4].FileName,
                "P7");
            P[5] = new RectColor(new Rectangle(P[4].rect.X , P[4].rect.Y+199, P[4].rect.Width, P[4].rect.Height),
                P[4].FileName,
                "P5");
            P[6] = new RectColor(new Rectangle(P[4].rect.X, P[4].rect.Y + 397, P[4].rect.Width, P[4].rect.Height),
               P[4].FileName,
               "P6");
            P[2] = new RectColor(new Rectangle(P[1].rect.X, P[1].rect.Y + 199, P[1].rect.Width, P[1].rect.Height),
               P[4].FileName,
               "P2");
            P[3] = new RectColor(new Rectangle(P[1].rect.X, P[1].rect.Y + 397, P[1].rect.Width, P[1].rect.Height),
              P[4].FileName,
              "P3");
            P[8] = new RectColor(new Rectangle(P[7].rect.X, P[7].rect.Y + 199, P[7].rect.Width, P[7].rect.Height),
             P[4].FileName,
             "P8");
            P[9] = new RectColor(new Rectangle(P[7].rect.X, P[7].rect.Y + 397, P[7].rect.Width, P[7].rect.Height),
            P[4].FileName,
            "P9");
            Bitmap shan1 = new Bitmap(Environment.CurrentDirectory + "\\imgs\\region_imgs" + P[4].FileName);  

            return;
            //B3:
            //cur x=840
            //ACTIVE X=410
            //ALWAYS:y=467 467-136,467-136*2

            //B2:B2-紧急作战 

            //B1:B1-诡异行商 
            //  TEST_B2();
            // TestB3_TOB2();
            var list = BranchNode.GetCurBranchNodes(3, 1, mrfzGamePage.CatptureImg());
            adb.Tap( list[0].ClickPoint);
         //   var list=BranchNode.GetCurBranchNodes(3, 1, mrfzGamePage.CatptureImg());
            return;
        }
        
        private void button12_Click(object sender, EventArgs e)
        {
            timer1.Tick += Timer1_Tick;
            timer1.Enabled = true;
            timer1.Start();
            return;
            //CostFont();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            return;

           /* var bmp = mrfzGamePage.CatptureImg();
            float delta = 0;
            int C = MAP_COST.CurCost(0,bmp,out delta);
            label7.Text = C.ToString();
            label12.Text = MAP_COST.secondDelta.ToString();
            label13.Text = delta.ToString();
            label14.Text = MAP_COST.fisrt_min_Delta_max.ToString();
           */
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var a=MAP_COST.secondDelta;
            return;
        }

        private void cvs_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var nps=mrfzGamePage.GamePageDic["不期而遇-其他2"].NextPages;
            Boolean flag = false;
            foreach(var np in nps)
            {
                var p=np.ClickPoint;
                for (int i = 0; i < 5; i++)
                {
                    adb.Tap(p);
                    System.Threading.Thread.Sleep(500);
                    if(mrfzGamePage.GamePageDic["rogue-next-node"].IsCurPage())
                    {
                      //  p=mrfzGamePage.GamePageDic["不期而遇-END"].NextPages[0].ClickPoint;
                        flag = true;
                      //  adb.Tap(p);
                        return;
                    }
                } 
            }
            return;
            //不期而遇-其他3
        }
    }
}
