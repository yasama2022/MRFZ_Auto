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
using MRFZ_Auto.script.mrfz;
using System.Threading;
using MRFZ_Auto.ui;
using MRFZ_Auto.script.mrfz.battle; 
using MRFZ_Auto.script;

namespace GamePageScript.ui
{
    public partial class NormalMapEdit : Form
    {
        List<FileInfo> ImgFileList = new List<FileInfo>();
        int index = -1;
        int ImgIndex = 0;
        GamePageCreator.state State = GamePageCreator.state.None;
        private Bitmap CurBitmap;
        private Rectangle Rect;
        public NormalMap CurMap
        {
            get
            {
                if (index >= NormalMap.Maps.Count)
                {
                    index = NormalMap.Maps.Count - 1;
                }
                if (index < 0) index = 0;
                if (NormalMap.Maps.Count <= 0) return null;
                return NormalMap.Maps.ElementAt(index).Value;
            } 
        }
       
        
        public NormalMapEdit()
        {
            InitializeComponent();
            InitUI();
            LoadMaps();
            this.SetStyle(ControlStyles.DoubleBuffer |

         ControlStyles.UserPaint |
         ControlStyles.AllPaintingInWmPaint,
         true);
            this.UpdateStyles(); 
            
            cvs.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).
                SetValue(cvs, true, null); 
        }
        private void InitUI()
        {
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listBox1.MouseDoubleClick += ListBox1_MouseDoubleClick;
            listBox2.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listBox2.MouseDoubleClick += ListBox1_MouseDoubleClick;
            listBox3.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listBox3.MouseDoubleClick += ListBox1_MouseDoubleClick;
            listBox4.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listBox4.MouseDoubleClick += ListBox1_MouseDoubleClick;
            listBox5.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            listBox5.MouseDoubleClick += ListBox1_MouseDoubleClick;
            tb_r_name.LostFocus += Tb_r_name_LostFocus;
            tb_r_name.GotFocus += Tb_r_name_GotFocus;
            tabControl2.SelectedIndexChanged += TabControl2_SelectedIndexChanged;
        }

        private void Tb_r_name_GotFocus(object sender, EventArgs e)
        {
            OLDTEXT = tb_r_name.Text;
        }

        private void TabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    ListBox1_SelectedIndexChanged(listBox1, null);
                    break;
                case 1:
                    ListBox1_SelectedIndexChanged(listBox2, null);
                    break;
                case 2:
                    ListBox1_SelectedIndexChanged(listBox3, null);
                    break;
                case 3:
                    ListBox1_SelectedIndexChanged(listBox4, null);
                    break;
                case 4:
                    ListBox1_SelectedIndexChanged(listBox5, null);
                    break;
            }
            DrawCVS();
        }
        String OLDTEXT = null;
        private void Tb_r_name_LostFocus(object sender, EventArgs e)
        {
            var NewText = tb_r_name.Text;
            if (OLDTEXT == null)
            {
                OLDTEXT = NewText;
            }else
            {
                if(NewText==OLDTEXT)
                {
                    return;
                }

                OLDTEXT = NewText;
            }
            String oldKey = "";
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    if (listBox1.SelectedIndex >= CurMap.HighLand.Count) return;
                      oldKey = CurMap.HighLand.ElementAt(listBox1.SelectedIndex).Key;

                    CurMap.HighLand.ElementAt(listBox1.SelectedIndex).Value.Name = NewText;
                    CurMap.HighLand.Add(NewText, CurMap.HighLand.ElementAt(listBox1.SelectedIndex).Value);
                    CurMap.HighLand.Remove(oldKey);
                    S();
                    listBox1.DataSource = null; 
                    listBox1.DataSource= new BindingSource(CurMap.HighLand, null);

                    DrawCVS();
                    break;
                case 1:
                    if (listBox2.SelectedIndex >= CurMap.LowLand.Count) return;
                      oldKey = CurMap.LowLand.ElementAt(listBox2.SelectedIndex).Key;
                    CurMap.LowLand.ElementAt(listBox2.SelectedIndex).Value.Name = NewText;
                    CurMap.LowLand.Add(NewText, CurMap.LowLand.ElementAt(listBox2.SelectedIndex).Value);
                    CurMap.LowLand.Remove(oldKey);
                    S();
                    listBox2.DataSource = null;
                    listBox2.DataSource = new BindingSource(CurMap.LowLand, null);
                    DrawCVS();
                    break;
                case 2:
                    if (listBox3.SelectedIndex >= CurMap.StateRegion.Count) return;
                      oldKey = CurMap.StateRegion.ElementAt(listBox3.SelectedIndex).Key;
                    CurMap.StateRegion.ElementAt(listBox3.SelectedIndex).Value.Name = NewText;
                    CurMap.StateRegion.Add(NewText, CurMap.StateRegion.ElementAt(listBox3.SelectedIndex).Value);
                    CurMap.StateRegion.Remove(oldKey);
                    S();
                    listBox3.DataSource = null;
                    listBox3.DataSource = new BindingSource(CurMap.StateRegion, null);
                    DrawCVS();
                    break;
                case 3:
                    if (listBox4.SelectedIndex >= CurMap.HighLandPoints.Count) return;
                      oldKey = CurMap.HighLandPoints.ElementAt(listBox4.SelectedIndex).Key;
                    CurMap.HighLandPoints.Add(NewText, CurMap.HighLandPoints.ElementAt(listBox4.SelectedIndex).Value); 
                    CurMap.HighLandPoints.Remove(oldKey);
                    S();
                    listBox4.DataSource = null;
                    listBox4.DataSource = new BindingSource(CurMap.HighLandPoints, null);
                    DrawCVS();
                    break;
                case 4:
                    if (listBox5.SelectedIndex >= CurMap.LowLandPoints.Count) return;
                    var oldKey2 = CurMap.LowLandPoints.ElementAt(listBox5.SelectedIndex).Key;
                    CurMap.LowLandPoints.Add(NewText, CurMap.LowLandPoints.ElementAt(listBox5.SelectedIndex).Value);
                    CurMap.LowLandPoints.Remove(oldKey2);
                    S(); 
                    listBox5.DataSource = null;
                    listBox5.DataSource = new BindingSource(CurMap.LowLandPoints, null);
                    DrawCVS();
                    break; 
            } 

        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var dr = MessageBox.Show(this, "是否删除此项", "是否删除此项", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No) return;
               var listbox=sender as ListBox;
                if (listbox==listBox1&& listBox1.SelectedIndex >= 0)
                {
                    if (CurMap.HighLand.Count > listBox1.SelectedIndex)
                    { 
                        String FileName = Environment.CurrentDirectory +
                            @"\imgs\rcimg" +
                            CurMap.HighLand.ElementAt(listBox1.SelectedIndex)
                            .Value.FileName;
                        File.Delete(FileName);
                        CurMap.HighLand.Remove(CurMap.HighLand.ElementAt(listBox1.SelectedIndex)
                            .Key);
                        listBox1.DataSource = null;
                        listBox1.DataSource = new BindingSource(CurMap.HighLand, null); 
                        if (CurMap.HighLand.Count > 0)
                            listBox1.SelectedIndex = 0;
                        S();
                        DrawCVS();
                    }
                }else if(listbox== listBox2 && listbox.SelectedIndex >= 0)
                {
                    if (CurMap.LowLand.Count > listbox.SelectedIndex)
                    { 
                        String FileName = Environment.CurrentDirectory +
                            @"\imgs\rcimg" +
                            CurMap.LowLand.ElementAt(listbox.SelectedIndex)
                            .Value.FileName;
                        File.Delete(FileName);
                        CurMap.LowLand.Remove(CurMap.LowLand.ElementAt(listbox.SelectedIndex)
                            .Key);
                        listbox.DataSource = null;
                        listbox.DataSource = new BindingSource(CurMap.LowLand, null);
                        if (CurMap.LowLand.Count > 0)
                            listbox.SelectedIndex = 0;
                        S();
                        DrawCVS();
                    }
                }
                else if (listbox == listBox3 && listbox.SelectedIndex >= 0)
                {
                    if (CurMap.StateRegion.Count > listbox.SelectedIndex)
                    { 
                        String FileName = Environment.CurrentDirectory +
                            @"\imgs\rcimg" +
                            CurMap.StateRegion.ElementAt(listbox.SelectedIndex)
                            .Value.FileName;
                        File.Delete(FileName);
                        CurMap.StateRegion.Remove(CurMap.StateRegion.ElementAt(listbox.SelectedIndex)
                            .Key);
                        listbox.DataSource = null;
                        listbox.DataSource = new BindingSource(CurMap.StateRegion, null);
                        if (CurMap.StateRegion.Count > 0)
                            listbox.SelectedIndex = 0;
                        S();
                        DrawCVS();
                    }
                }
                else if (listbox == listBox4 && listbox.SelectedIndex >= 0)
                {
                    if (CurMap.HighLandPoints.Count > listbox.SelectedIndex)
                    {  
                        CurMap.HighLandPoints.Remove(CurMap.HighLandPoints.ElementAt(listbox.SelectedIndex)
                            .Key);
                        listbox.DataSource = null;
                        listbox.DataSource = new BindingSource(CurMap.HighLandPoints, null);
                        if (CurMap.HighLandPoints.Count > 0)
                            listbox.SelectedIndex = 0;
                        S();
                        DrawCVS();
                    }
                }
                else if (listbox == listBox5 && listbox.SelectedIndex >= 0)
                {
                    if (CurMap.LowLandPoints.Count > listbox.SelectedIndex)
                    {
                        CurMap.LowLandPoints.Remove(CurMap.LowLandPoints.ElementAt(listbox.SelectedIndex)
                            .Key);
                        listbox.DataSource = null;
                        listbox.DataSource = new BindingSource(CurMap.LowLandPoints, null);
                        if (CurMap.LowLandPoints.Count > 0)
                            listbox.SelectedIndex = 0;
                        S();
                        DrawCVS();
                    }
                }
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //..
            var listbox = sender as ListBox;
            if (listbox == listBox1 && listbox.SelectedIndex >= 0 && listbox.SelectedIndex < CurMap.HighLand.Count)
            {

                tb_r_name.Text = CurMap.HighLand.ElementAt(listbox.SelectedIndex).Value.Name;
            }
            else if (listbox == listBox2 && listbox.SelectedIndex >= 0 && listbox.SelectedIndex < CurMap.LowLand.Count)
            {

                tb_r_name.Text = CurMap.LowLand.ElementAt(listbox.SelectedIndex).Value.Name;
            }else  if (listbox == listBox3 && listbox.SelectedIndex >= 0 && listbox.SelectedIndex < CurMap.StateRegion.Count)
            {

                tb_r_name.Text = CurMap.StateRegion.ElementAt(listbox.SelectedIndex).Value.Name;
            }
            else
            if (listbox == listBox4 && listbox.SelectedIndex >= 0 && listbox.SelectedIndex < CurMap.HighLandPoints.Count)
            {

                tb_r_name.Text = CurMap.HighLandPoints.ElementAt(listbox.SelectedIndex).Key;
            }
            else
            if (listbox == listBox5 && listbox.SelectedIndex >= 0 && listbox.SelectedIndex < CurMap.LowLandPoints.Count)
            {

                tb_r_name.Text = CurMap.LowLandPoints.ElementAt(listbox.SelectedIndex).Key;
            }
        } 
      
     
         
         
        private void LoadMaps()
        {
             
            if (NormalMap.Maps.Count > 0)
            {
                index = 0;
                LoadCurMap();
            } 

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
            if(CurMap!=null)
            {
              //  CurGamePage.Name = tb_pageName.Text;
              
                 NormalMap.Save(); 
            }
            
                  
        }
       
        
      
        private bool Loading = false;
        public void LoadCurMap()
        {
            //load img 
            Loading = true;
           // this.State = state.RecPoint;
            if(CurBitmap!=null)
            {
                CurBitmap.Dispose();
            }
            CurBitmap = new Bitmap(
                Environment.CurrentDirectory+ 
                "\\imgs\\map\\"+CurMap.Name+ "\\normal\\" +
                CurMap.ImgFiles[ImgIndex]
                );
            tb_imgname.Text = CurMap.ImgFiles[ImgIndex];
            this.cvs.BackgroundImageLayout = ImageLayout.Stretch;
            
            //  this.cvs.Width = CurBitmap.Width;
            //  this.cvs.Height =  CurBitmap.Height;   
            // textBox4.Text = index.ToString(); 
            DrawCVS();
            LoadCurMap_Data();
            Loading = false;

        }
        public void DrawCVS()
        { 
            Bitmap bmp = new Bitmap(1280, 720);
            var g=Graphics.FromImage(bmp); 
            g.DrawImage(CurBitmap, 0, 0); 
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    foreach (var cnp in CurMap.HighLand)
                    {
                       var p= new Point(cnp.Value.rect.Location.X, cnp.Value.rect.Location.Y);
                        p.Offset(-5, -5);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(127, 0, 122, 204)), cnp.Value.rect);
                        g.DrawString(cnp.Value.Name, new Font("宋体", 24, FontStyle.Regular), Brushes.Red,p   );
                    }
                    break;
                case 1:
                    foreach (var cnp in CurMap.LowLand)
                    {
                        var p = new Point(cnp.Value.rect.Location.X, cnp.Value.rect.Location.Y);
                        p.Offset(-5, -5);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(127, 0, 122, 204)), cnp.Value.rect);
                        g.DrawString(cnp.Value.Name, new Font("宋体", 24, FontStyle.Regular), Brushes.Red, p);
                    }
                    break;
                case 2:
                    foreach (var cnp in CurMap.StateRegion)
                    {
                        var p = new Point(cnp.Value.rect.Location.X, cnp.Value.rect.Location.Y);
                        p.Offset(-5, -5);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(127, 0, 122, 204)), cnp.Value.rect);
                        g.DrawString(cnp.Value.Name, new Font("宋体", 24, FontStyle.Regular), Brushes.Red, p);
                    }
                    break;
                case 3:
                    foreach (var cnp in CurMap.HighLandPoints)
                    {

                        var p = new Point(cnp.Value.X, cnp.Value.Y);
                        p.Offset(-5, -5);
                        g.DrawString(cnp.Key, new Font("宋体", 24, FontStyle.Bold), Brushes.Red,
                            p);
                    }
                    break;
                case 4:
                    foreach (var cnp in CurMap.LowLandPoints)
                    {

                        var p = new Point(cnp.Value.X, cnp.Value.Y);
                        p.Offset(-5, -5);
                        g.DrawString(cnp.Key, new Font("宋体", 24, FontStyle.Bold), Brushes.Red,
                            p);
                    }
                    break;
            } 
            /*  foreach(var cnp in CurGamePage.NextPages)
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
            */
            /*if(State== state.RecRect)
            {
                g.DrawRectangle(Pens.White, Rect);
            }*/
            cvs.BackgroundImageLayout = ImageLayout.Stretch;
             cvs.BackgroundImage = bmp;
            // g.Dispose();
        }
        private void LoadCurMap_Data()
        { 
            tb_mapname.Text = CurMap.Name;
            listBox1.DataSource = new BindingSource(CurMap.HighLand, null);
            listBox2.DataSource = new BindingSource(CurMap.LowLand, null);
            listBox3.DataSource = new BindingSource(CurMap.StateRegion, null);
            listBox4.DataSource = new BindingSource(CurMap.HighLandPoints, null);
            listBox5.DataSource = new BindingSource(CurMap.LowLandPoints, null); 


        }
        
       
        private void btn_pre_click(object sender, EventArgs e)
        {
            S();
            int cur = ImgIndex;
            if (ImgIndex-- >=0)
            {
                if (ImgIndex >= CurMap.ImgFiles.Count
                    ||ImgIndex<0)
                {
                    ImgIndex = cur; 
                }else
                {

                    LoadCurMap();
                } 
                
            }else
            {
                ImgIndex = cur;
            }
            

        }

        private void BTN_NEXT_CLICK(object sender, EventArgs e)
        {
            S();
            int cur = ImgIndex;
            if (ImgIndex++ <= CurMap.ImgFiles.Count)
            {
                if (ImgIndex >= CurMap.ImgFiles.Count)
                {
                    ImgIndex = cur; 
                }else
                {
                    LoadCurMap();
                }
                
            }else
            {
                ImgIndex = cur;
            }
             
           
        }  

       
       

        private void button2_Click(object sender, EventArgs e)
        {
            S();
            int cur = index;
            if (index++ <=NormalMap.Maps.Count)
            {
                if (index >= NormalMap.Maps.Count)
                {
                    index = cur; 
                }else
                {
                    ImgIndex = 0;
                    LoadCurMap();
                }  
                 
            }else
            {

                index = cur;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            S();
            int cur = index;
            if (index-- >= 0)
            {
                if (index < 0)
                {
                    index = cur; 
                } else

                    {
                    ImgIndex = 0;
                        LoadCurMap();
                    } 
                
            }else
            {

                index = cur;
            }

        }

        private void btn_add_pr_Click(object sender, EventArgs e)
        {
            PictureBoxWin win = null;
            DialogResult dr =  DialogResult.None;
            switch (tabControl2.SelectedIndex)
            {
                case 0:
                      win = new PictureBoxWin(CurBitmap, GamePageCreator.state.RegionBlock);
                      dr=win.ShowDialog();
                    if(dr== DialogResult.OK)
                    {
                       var rect= win.result.rect;
                        DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + @"\imgs\rcimg");
                        if (!di.Exists) di.Create();
                        var fn = $"\\normalmap_{index}_highland_{CurMap.HighLand.Count}.png";
                        String FileName = di.FullName + fn;
                        var rcimg = CurBitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        rcimg.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                        var Name = "H" + (CurMap.HighLand.Count+1);
                        RectColor rc = new RectColor(rect, fn
                            ,   Name);
                        CurMap.HighLand.Add(Name, rc);
                        S();
                        listBox1.DataSource = null;
                        listBox1.DataSource = new BindingSource(CurMap.HighLand, null);
                        listBox1.SelectedIndex = 0; 
                    }
                    break;
                case 1:
                      win = new PictureBoxWin(CurBitmap, GamePageCreator.state.RegionBlock);
                      dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var rect = win.result.rect;
                        DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + @"\imgs\rcimg");
                        if (!di.Exists) di.Create();
                        var fn = $"\\normalmap_{index}_LowLand_{CurMap.LowLand.Count}.png";
                        String FileName = di.FullName + fn;
                        var rcimg = CurBitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        rcimg.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                        var Name = "L" + (CurMap.LowLand.Count+1);
                        RectColor rc = new RectColor(rect, fn
                            ,  Name);
                        CurMap.LowLand.Add(Name, rc);
                        S();
                        listBox2.DataSource = null;
                        listBox2.DataSource = new BindingSource(CurMap.LowLand, null);
                        listBox2.SelectedIndex = 0;
                    }
                    break;
                case 2:
                    win = new PictureBoxWin(CurBitmap, GamePageCreator.state.RegionBlock);
                    dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var rect = win.result.rect;
                        DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + @"\imgs\rcimg");
                        if (!di.Exists) di.Create();
                        var fn = $"\\normalmap_{index}_StateRegion_{CurMap.StateRegion.Count}.png";
                        String FileName = di.FullName + fn;
                        var rcimg = CurBitmap.Clone(rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        rcimg.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                        var Name = "S" + (CurMap.StateRegion.Count+1);
                        RectColor rc = new RectColor(rect, fn
                            ,   Name);
                        CurMap.StateRegion.Add(Name, rc);
                        S();
                        listBox3.DataSource = null;
                        listBox3.DataSource = new BindingSource(CurMap.StateRegion, null);
                        listBox3.SelectedIndex = 0;
                    }
                    break;
                case 3:
                    win = new PictureBoxWin(CurBitmap, GamePageCreator.state.ClickToNext);
                    dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var p = win.result.p;
                         
                        var Name = "H" + (CurMap.HighLandPoints.Count+1); 
                        CurMap.HighLandPoints.Add(Name, p);
                        S();
                        listBox4.DataSource = null;
                        listBox4.DataSource = new BindingSource(CurMap.HighLandPoints, null);
                        listBox4.SelectedIndex = 0;
                    }
                    break;
                case 4:
                    win = new PictureBoxWin(CurBitmap, GamePageCreator.state.ClickToNext);
                    dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var p = win.result.p;

                        var Name = "L" +( CurMap.LowLandPoints.Count+1);
                        CurMap.LowLandPoints.Add(Name, p);
                        S();
                        listBox5.DataSource = null;
                        listBox5.DataSource = new BindingSource(CurMap.LowLandPoints, null);
                        listBox5.SelectedIndex = 0;
                    }
                    break;
            }
            DrawCVS();
        }

        private void NormalMapEdit_Load(object sender, EventArgs e)
        {

        }

        private void cvs_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
