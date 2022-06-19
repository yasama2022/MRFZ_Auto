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
using MRFZ_Auto.script.mrfz.map;

namespace GamePageScript.ui
{
    public partial class UseSkillMapEditWin : Form
    {
        List<FileInfo> ImgFileList = new List<FileInfo>();
        int index = -1;
        int ImgIndex = 0;
        GamePageCreator.state State = GamePageCreator.state.None;
        private Bitmap CurBitmap;
        private Rectangle Rect;
        public UseSkillMap CurMap
        {
            get
            {
                if (index >= UseSkillMap.Maps.Count)
                {
                    index = UseSkillMap.Maps.Count - 1;
                }
                if (index < 0) index = 0;
                if (UseSkillMap.Maps.Count <= 0) return null;
                return UseSkillMap.Maps.ElementAt(index).Value;
            } 
        }
       
        
        public UseSkillMapEditWin()
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
                    ListBox1_SelectedIndexChanged(listBox4, null);
                    break;
                case 1:
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
                    if (listBox4.SelectedIndex >= CurMap.HighLandPoints.Count) return;
                      oldKey = CurMap.HighLandPoints.ElementAt(listBox4.SelectedIndex).Key;
                    CurMap.HighLandPoints.Add(NewText, CurMap.HighLandPoints.ElementAt(listBox4.SelectedIndex).Value); 
                    CurMap.HighLandPoints.Remove(oldKey);
                    S();
                    listBox4.DataSource = null;
                    listBox4.DataSource = new BindingSource(CurMap.HighLandPoints, null);
                    DrawCVS();
                    break;
                case 1:
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
                  if (listbox == listBox4 && listbox.SelectedIndex >= 0)
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
             
            if (UseSkillMap.Maps.Count > 0)
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

                UseSkillMap.Save(); 
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
                "\\imgs\\map\\"+CurMap.Name+ "\\useskill\\" +
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
                    foreach (var cnp in CurMap.HighLandPoints)
                    {

                        var p = new Point(cnp.Value.Point.X, cnp.Value.Point.Y);
                        p.Offset(-5, -5);
                        g.DrawString(cnp.Key, new Font("宋体", 24, FontStyle.Bold), Brushes.Red,
                            p);
                    }
                    break;
                case 1:
                    foreach (var cnp in CurMap.LowLandPoints)
                    {

                        var p = new Point(cnp.Value.Point.X, cnp.Value.Point.Y);
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
            if (index++ <= UseSkillMap.Maps.Count)
            {
                if (index >= UseSkillMap.Maps.Count)
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
                    win = new PictureBoxWin(CurBitmap, GamePageCreator.state.ClickToNext);
                    dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var p = win.result.p;
                         
                        var Name = "H" + (CurMap.HighLandPoints.Count+1); 
                        CurMap.HighLandPoints.Add(Name, new CharPoint() { Point = p, name = Name });
                        S();
                        listBox4.DataSource = null;
                        listBox4.DataSource = new BindingSource(CurMap.HighLandPoints, null);
                        listBox4.SelectedIndex = 0;
                    }
                    break;
                case 1:
                    win = new PictureBoxWin(CurBitmap, GamePageCreator.state.ClickToNext);
                    dr = win.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        var p = win.result.p;

                        var Name = "L" +( CurMap.LowLandPoints.Count+1);
                        CurMap.LowLandPoints.Add(Name, new CharPoint() { Point=p, name=Name});
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
