using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GamePageScript.lib;
using GamePageScript.script.mrfz;
using lib;
using MRFZ_Auto;
using MRFZ_Auto.script.mrfz;
using MRFZ_Auto.script.mrfz.battle;
using MRFZ_Auto.script.mrfz.shop;
using MRFZ_Auto.script.保全派驻.通用;
using MRFZ_Auto.script.普通关卡挂机;
using MRFZ_Auto.ui;
using script;
using Win32;
using static GamePageScript.script.mrfz.mrfz_ScriptUnit;
using static MRFZ_Auto.ui.Contorls.ADB_Form_List;

namespace GamePageScript.ui
{
    public partial class mrfzScriptWin : Form
    {
        private const int W = 1200, H = 705;
         
        public OnMsg onMsg;
        public OnMsg onMsg_nor;
        public mrfz_ScriptMachine scriptMachine = new mrfz_ScriptMachine();
        public int MAX = 100;
        public List<String> msg_stack = new List<string>();
        public List<String> msg_stack_nor = new List<string>();

        public mrfzScriptWin()
        {
            InitializeComponent();
            button1.Visible = Program.Debug;
            button4.Visible = Program.Debug;
            button4.Visible = Program.Debug;
            checkBox4.Visible= Program.Debug;
            this.Text = MRFZ_Auto.Properties.Resources.gametitle ;
            this.FormClosed += MrfzScriptWin_FormClosed;
            this.FormClosing += MrfzScriptWin_FormClosing;
            richTextBox1.Text = MRFZ_Auto.Properties.Resources.str_proto;
            richTextBox2.Text = MRFZ_Auto.Properties.Resources.update_pro;
            tb_nor_tips.Text= MRFZ_Auto.Properties.Resources.nor_tips;
            onMsg += (msg) =>
            {
                if (msg == "showtips")
                {
                    if (mrfz_ScriptConfig.scriptConfig.ShowTips)
                    {
                        var dr = MessageBox.Show(MRFZ_Auto.Properties.Resources.tips, "新版提示,点击'是'永久不显示该提示",  MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            
                            mrfz_ScriptConfig.scriptConfig.ShowTips = false;
                            mrfz_ScriptConfig.scriptConfig.save();
                        }
                    }
                    return;
                }
                msg_stack.Add(msg);
                if(msg_stack.Count>MAX)
                {
                    msg_stack.RemoveAt(0);
                }
                StringBuilder sb = new StringBuilder();
                foreach (var m in msg_stack)
                    sb.AppendLine(m);
                tb_msg.Text=(sb.ToString());
                sb.Clear();
                //  richTextBox1.AppendText(msg+"\r\n");
            };
            onMsg_nor += (x) =>
            {
                msg_stack_nor.Add(x);
                if (msg_stack_nor.Count > MAX)
                {
                    msg_stack_nor.RemoveAt(0);
                }
                StringBuilder sb = new StringBuilder();
                foreach (var m in msg_stack_nor)
                    sb.AppendLine(m);
                tb_nor_msg.Text = (sb.ToString());
                sb.Clear();
            };
           /* scriptMachine.onMsg += (x) =>
            {
                this.BeginInvoke(onMsg, x);
            };*/
           // richTextBox1.Text = MRFZ_Auto.Properties.Resources.str_turtol;
            this.tb_msg.ContentsResized += (o, e) =>
            {
                tb_msg.SelectionStart = tb_msg.Text.Length;                    //rtbReceive为控件的名字（自己取）
                tb_msg.ScrollToCaret();

            };
            try
            {
                
                ERRMsg += (msg) =>
                {

                    MessageBox.Show(msg, "出现错误异常");
                };
                initUI();
                onMsg?.Invoke(MRFZ_Auto.Properties.Resources.tips);
                Thread th = new Thread(() =>
                  {
                      while(!this.IsHandleCreated)
                      {
                          Thread.Sleep(500);
                      }
                      this.BeginInvoke(onMsg, "showtips");
                  });
                th.Start();
                adb_list.SetData(new List<OnMsg>() { this.onMsg ,this.onMsg_nor},new List<TextBox>() { this.tb_win , tb_win_nor });
               
            }catch(Exception ex)
            {
                MessageBox.Show("错误",ex.Message+"--"+ex.ToString()+"--"+ex.StackTrace);
            }
        }
        private void MrfzScriptWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.scriptMachine?.Stop();
                Environment.Exit(0);
            }catch
            {

            }
        }

        private void MrfzScriptWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.scriptMachine?.Stop();
                Environment.Exit(0);
            }
            catch
            {

            }
        }
          
        public delegate void onERRMsg(String msg);
        onERRMsg ERRMsg;
     

       

        private void initUI()
        {
            gb_stone.Visible = mrfz_ScriptConfig.scriptConfig.enable_stoneeat;
            bool canLing = false;
            if(!canLing)
            {
              //  T6.Visible = false;
              //  tableLayoutPanel2.ColumnCount = 5;
                label20.Text = "(后勤或指挥或突击战术,突击队一般排第5";
                groupBox5.Text = "是否用助战(山/煌/帕拉斯)";
            }
            
            Label[] lbs = new Label[] { label1, label2,
            label3,
            label4,
            label5,
            label6,
            label7,label8,label9,label10,label11,label12,};
            for(int i=0; i<12;i++)
            {
                FontFamily fontFamily = new FontFamily("黑体");
                lbs[i].Font = new Font(fontFamily, 12, FontStyle.Bold);
                lbs[i].Dock = DockStyle.Fill;
                lbs[i].TextAlign = ContentAlignment.MiddleCenter;
                lbs[i].ForeColor = Color.Black;
                lbs[i].BackColor = Color.Transparent;
                lbs[i].BorderStyle = BorderStyle.FixedSingle;
                lbs[i].Click += (o, e) =>
                {
                    int index = int.Parse((o as Label).Text);
                    for (int v = 0; v < 12; v++)
                    {
                        lbs[v].BackColor = Color.Transparent;
                    }
                   (o as Label).BackColor = Color.OrangeRed;
                    mrfz_ScriptConfig.scriptConfig.SelectChar_Loc = index;
                    mrfz_ScriptConfig.scriptConfig.save();

                };
            }
             
            lbs[mrfz_ScriptConfig.scriptConfig.SelectChar_Loc-1].BackColor = Color.OrangeRed;
            //heal
            Label[] H  = new Label[] {h1,h2,h3,h4 ,h5,h6,h7,h8,h9,h10,h11,h12};
            for (int i = 0; i < 12; i++)
            {
                FontFamily fontFamily = new FontFamily("黑体");
                H[i].Font = new Font(fontFamily, 12, FontStyle.Bold);
                H[i].Dock = DockStyle.Fill;
                H[i].TextAlign = ContentAlignment.MiddleCenter;
                H[i].ForeColor = Color.Black;
                H[i].BackColor = Color.Transparent;
                H[i].BorderStyle = BorderStyle.FixedSingle;
                H[i].Click += (o, e) =>
                {
                    int index = int.Parse((o as Label).Name.Replace("h", ""));
                    for (int v = 0; v <12; v++)
                    {
                        H[v].BackColor = Color.Transparent;
                    }
                   (o as Label).BackColor = Color.OrangeRed;
                    mrfz_ScriptConfig.scriptConfig.SelectHEAL_Loc = index;
                    mrfz_ScriptConfig.scriptConfig.save();

                };
            }
            H[mrfz_ScriptConfig.scriptConfig.SelectHEAL_Loc - 1].BackColor = Color.OrangeRed;
            //TEAM
            Label[] T = new Label[] { T1, T2, T3, T4 ,T5};
            for (int i = 0; i < T.Length; i++)
            {
                FontFamily fontFamily = new FontFamily("黑体");
                T[i].Font = new Font(fontFamily, 12, FontStyle.Bold);
                T[i].Dock = DockStyle.Fill;
                T[i].TextAlign = ContentAlignment.MiddleCenter;
                T[i].ForeColor = Color.Black;
                T[i].BackColor = Color.Transparent; 
                T[i].BorderStyle = BorderStyle.FixedSingle;
                T[i].Click += (o, e) =>
                {
                    int index = int.Parse((o as Label).Name.Replace("T", ""));
                    for (int v = 0; v < T.Length; v++)
                    {
                        T[v].BackColor = Color.Transparent;
                    }
                   (o as Label).BackColor = Color.OrangeRed;
                   // if(index!=5)
                    this.gb_heal.Visible = true;
                    mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc = index;
                   /* mrfz_ScriptConfig.scriptConfig.Team_Type=
                     (TeamLogo.TeamType)( index-1);*/
                    mrfz_ScriptConfig.scriptConfig.save();

                };
            }

            T[mrfz_ScriptConfig.scriptConfig.SelectTeam_Loc-1].BackColor = Color.OrangeRed;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.mainChar;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
            comboBox2.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.diff;
            if (mrfz_ScriptConfig.scriptConfig.RunCount == -1)
            {
                radioButton1.Checked = true;
                numericUpDown1.Visible = false;
                rb_nolimit.Checked = true;
                nud_count.Visible = false;
            }else
            {
                radioButton2.Checked = true;
                numericUpDown1.Visible = true;
                numericUpDown1.Value = mrfz_ScriptConfig.scriptConfig.RunCount;

                rb_count.Checked = true;
                nud_count.Visible = true;
                nud_count.Value = mrfz_ScriptConfig.scriptConfig.RunCount;
            }
            radioButton1.CheckedChanged += RadioButton1_CheckedChanged;
            radioButton2.CheckedChanged += RadioButton1_CheckedChanged;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            nud_count.ValueChanged += Nud_count_ValueChanged;
            checkBox1.Checked = mrfz_ScriptConfig.scriptConfig.policy.AutoSaveGold;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
           // checkBox2.Checked = mrfz_ScriptConfig.scriptConfig.policy.NoMoreNormalBattle;
          //  checkBox2.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox3.Checked = mrfz_ScriptConfig.scriptConfig.policy.L1Exit;
            checkBox3.CheckedChanged += CheckBox1_CheckedChanged;
          //  checkBox4.Checked = mrfz_ScriptConfig.scriptConfig.policy.NoMUJIANYUXING;
          //  checkBox4.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox5.Checked = mrfz_ScriptConfig.scriptConfig.policy.GetItem;
            checkBox5.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox6.Checked = mrfz_ScriptConfig.scriptConfig.policy.GetPaper;
            checkBox6.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox7.Checked = mrfz_ScriptConfig.scriptConfig.policy.GetSong;
            checkBox7.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox8.Checked = mrfz_ScriptConfig.scriptConfig.policy.GetSupport;
            checkBox8.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox9.Checked = mrfz_ScriptConfig.scriptConfig.policy.GetGold;
            checkBox9.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox10.Checked = mrfz_ScriptConfig.scriptConfig.policy.BuyItem;
            checkBox10.CheckedChanged += CheckBox1_CheckedChanged; 
            cb1.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.policy.P1;
            cb2.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.policy.P2;
            cb3.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.policy.P3;
            rb_count.CheckedChanged += Rb_count_CheckedChanged;
            rb_nolimit.CheckedChanged += Rb_nolimit_CheckedChanged;
            //0,1,2=rb_avoid_hard
            //0,2,1=rb_noavoid_hard
            //2,0,1=rb_hard_first
            //ELSE = rb_custom
            if(cb1.SelectedIndex==0&& cb2.SelectedIndex==1&& cb3.SelectedIndex==2)
            {
                rb_avoid_hard.Checked = true;
            }
            else if (cb1.SelectedIndex == 0 && cb2.SelectedIndex == 2 && cb3.SelectedIndex == 1)
            {
                rb_noavoid_hard.Checked = true;
            }
            else if (cb1.SelectedIndex == 2 && cb2.SelectedIndex == 0&& cb3.SelectedIndex == 1)
            {
                rb_hard_first.Checked = true;
            }else
            {
                rb_custom.Checked = true;
            }
            rb_avoid_hard.CheckedChanged += Rb_avoid_hard_CheckedChanged;
            rb_noavoid_hard.CheckedChanged += Rb_avoid_hard_CheckedChanged;
            rb_hard_first.CheckedChanged += Rb_avoid_hard_CheckedChanged;
            rb_custom.CheckedChanged += Rb_avoid_hard_CheckedChanged;
            cb1.SelectedIndexChanged += (x1, x2) =>
            {
                
                var I = cb1.SelectedIndex;
                if (cb2.SelectedIndex==cb1.SelectedIndex)
                {
                    for(int i=0; i<3;i++)
                    {
                        if(i!=I&&i!=cb3.SelectedIndex)
                        {
                            cb2.SelectedIndex = i;
                        }
                    }
                }
                if (cb3.SelectedIndex == cb1.SelectedIndex)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != I && i != cb2.SelectedIndex)
                        {
                            cb3.SelectedIndex = i;
                        }
                    }
                }

                mrfz_ScriptConfig.scriptConfig.policy.P1 = (mrfz_ScriptConfig.Priority)cb1.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P2 = (mrfz_ScriptConfig.Priority)cb2.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P3 = (mrfz_ScriptConfig.Priority)cb3.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.save();
            };
            cb3.SelectedIndexChanged += (x1, x2) =>
            {

                var I = cb3.SelectedIndex;
                if (cb2.SelectedIndex == cb3.SelectedIndex)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != I && i != cb1.SelectedIndex)
                        {
                            cb2.SelectedIndex = i;
                        }
                    }
                }
                if (cb3.SelectedIndex == cb1.SelectedIndex)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != I && i != cb2.SelectedIndex)
                        {
                            cb1.SelectedIndex = i;
                        }
                    }
                }
                mrfz_ScriptConfig.scriptConfig.policy.P1 = (mrfz_ScriptConfig.Priority)cb1.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P2 = (mrfz_ScriptConfig.Priority)cb2.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P3 = (mrfz_ScriptConfig.Priority)cb3.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.save();
            };
            cb2.SelectedIndexChanged += (x1, x2) =>
            {

                var I = cb2.SelectedIndex;
                if (cb3.SelectedIndex == cb2.SelectedIndex)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != I && i != cb1.SelectedIndex)
                        {
                            cb3.SelectedIndex = i;
                        }
                    }
                }
                if (cb1.SelectedIndex == cb2.SelectedIndex)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != I && i != cb3.SelectedIndex)
                        {
                            cb1.SelectedIndex = i;
                        }
                    }
                }

                mrfz_ScriptConfig.scriptConfig.policy.P1 = (mrfz_ScriptConfig.Priority)cb1.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P2 = (mrfz_ScriptConfig.Priority)cb2.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.policy.P3 = (mrfz_ScriptConfig.Priority)cb3.SelectedIndex;
                mrfz_ScriptConfig.scriptConfig.save();
            };
            rb_friendshan.CheckedChanged += Rb_friendshan_CheckedChanged;
            rb_no_friendshan.CheckedChanged += Rb_friendshan_CheckedChanged;
            if (mrfz_ScriptConfig.scriptConfig.UseFriendRole)
                rb_friendshan.Checked = true;
            else
                rb_no_friendshan.Checked = true; 
             
             
            cb_onlyusefriend.Checked = mrfz_ScriptConfig.scriptConfig.GetFriendRole_NeedFriendShip;
            cb_onlyusefriend.CheckedChanged += Cb_onlyusefriend_CheckedChanged;

        }

        private void Nud_count_ValueChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.RunCount = Convert.ToInt32(nud_count.Value);
            mrfz_ScriptConfig.scriptConfig.save();
        }

        private void Rb_nolimit_CheckedChanged(object sender, EventArgs e)
        {
            nud_count.Visible = !rb_nolimit.Checked;
            if(rb_nolimit.Checked)
            {

                mrfz_ScriptConfig.scriptConfig.RunCount = -1;
                mrfz_ScriptConfig.scriptConfig.save();
            }else
            {
                mrfz_ScriptConfig.scriptConfig.RunCount = Convert.ToInt32( nud_count.Value);
            }
        }

        private void Rb_count_CheckedChanged(object sender, EventArgs e)
        {
            nud_count.Visible = !rb_nolimit.Checked;
            if (rb_nolimit.Checked)
            {

                mrfz_ScriptConfig.scriptConfig.RunCount = -1;
                mrfz_ScriptConfig.scriptConfig.save();
            }else
            {

                mrfz_ScriptConfig.scriptConfig.RunCount = Convert.ToInt32(nud_count.Value);
            }
        }

        private void Rb_avoid_hard_CheckedChanged(object sender, EventArgs e)
        { 
            if (rb_avoid_hard.Checked)
            {
                cb1.Enabled = false;
                cb2.Enabled = false;
                cb3.Enabled = false;
                cb1.SelectedIndex = 0;
                cb2.SelectedIndex = 1;
                cb3.SelectedIndex = 2; 
            }
            else if (rb_noavoid_hard.Checked)
            { 
                cb1.Enabled = false;
                cb2.Enabled = false;
                cb3.Enabled = false;
                cb1.SelectedIndex = 0;
                cb2.SelectedIndex = 2;
                cb3.SelectedIndex = 1;
            }
            else if (rb_hard_first.Checked)
            {
                 
                cb1.Enabled = false;
                cb2.Enabled = false;
                cb3.Enabled = false;
                cb1.SelectedIndex = 2;
                cb2.SelectedIndex = 0;
                cb3.SelectedIndex = 1;
            }else if(rb_custom.Checked)
            {
                 
                cb1.Enabled = true;
                cb2.Enabled = true;
                cb3.Enabled = true;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.policy.AutoSaveGold = checkBox1.Checked; 
            mrfz_ScriptConfig.scriptConfig.policy.L1Exit = checkBox3.Checked; 
            mrfz_ScriptConfig.scriptConfig.policy.GetItem = checkBox5.Checked;
            mrfz_ScriptConfig.scriptConfig.policy.GetPaper = checkBox6.Checked;
            mrfz_ScriptConfig.scriptConfig.policy.GetSong = checkBox7.Checked;
            mrfz_ScriptConfig.scriptConfig.policy.GetSupport = checkBox8.Checked;
            mrfz_ScriptConfig.scriptConfig.policy.GetGold = checkBox9.Checked;
            mrfz_ScriptConfig.scriptConfig.policy.BuyItem = checkBox10.Checked; 
            mrfz_ScriptConfig.scriptConfig.save();

        }

        private void Cb_onlyusefriend_CheckedChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.GetFriendRole_NeedFriendShip = cb_onlyusefriend.Checked;
            mrfz_ScriptConfig.scriptConfig.save();
        }

       

        private void Rb_friendshan_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_friendshan.Checked)
            {
                mrfz_ScriptConfig.scriptConfig.UseFriendRole = true;
               // mrfz_ScriptConfig.scriptConfig.mainChar = mrfz_ScriptConfig.MainChar.山;
              //  comboBox1.SelectedIndex = (int)mrfz_ScriptConfig.scriptConfig.mainChar;
              //  comboBox1.Enabled = false;
                mrfz_ScriptConfig.scriptConfig.save();
            }
            else
            {
                mrfz_ScriptConfig.scriptConfig.UseFriendRole = false;
              //  comboBox1.Enabled = true;
                mrfz_ScriptConfig.scriptConfig.save();
            }
        }

        

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        { 
            mrfz_ScriptConfig.scriptConfig.RunCount = Convert.ToInt32(numericUpDown1.Value);
            mrfz_ScriptConfig.scriptConfig.save();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        { 
            if(radioButton1.Checked)
            {
                numericUpDown1.Visible = false;
                mrfz_ScriptConfig.scriptConfig.RunCount = -1;
                mrfz_ScriptConfig.scriptConfig.save();
            }
            else
            { 
                numericUpDown1.Visible = true;
                mrfz_ScriptConfig.scriptConfig.RunCount =Convert.ToInt32(numericUpDown1.Value);
                mrfz_ScriptConfig.scriptConfig.save();
            }
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.diff = (mrfz_ScriptConfig.Diff)comboBox2.SelectedIndex;
            mrfz_ScriptConfig.scriptConfig.save();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            mrfz_ScriptConfig.scriptConfig.mainChar =(mrfz_ScriptConfig.ArkChar) comboBox1.SelectedIndex;
          //  this.gb_more_cost.Visible = mrfz_ScriptConfig.scriptConfig.mainChar == mrfz_ScriptConfig.ArkChar.帕拉斯;
          //  this.gb_heal.Visible = mrfz_ScriptConfig.scriptConfig.mainChar != mrfz_ScriptConfig.MainChar.山;
            mrfz_ScriptConfig.scriptConfig.save();
            
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if(adb_list.ADB_ITEM!=null )
            {
                if (scriptMachine.Running)
                {
                    MessageBox.Show("脚本已经在运行，请等待脚本停止或者手动点击终止运行");
                    return;
                }
                else
                {
                    
                    scriptMachine.RunInMainForm(this, adb_list.ADB_ITEM);
                } 
            }
            else
            {
                MessageBox.Show("未检测到模拟器与模拟器窗口,请等待右侧模拟器检测完毕后再点开始" +
                    "\r\n,请先开启模拟器进入明日方舟-肉鸽准备界面后再启动");
                return;
            } 
            
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            scriptMachine.Stop();
        }

       

       

        
        private void button3_Click(object sender, EventArgs e)
        {
         var cfw=new    mrfz_config_Win();
         var dr=   cfw.ShowDialog();
            if(dr== DialogResult.OK)
            {
                mrfz_ScriptConfig.scriptConfig.save();
                gb_stone.Visible = mrfz_ScriptConfig.scriptConfig.enable_stoneeat;
                 
                 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mrfz_ScriptMachine.Pause = false;
        }

        private void adb_list_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TipsWin tw = new TipsWin();
            tw.Show();

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void T2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_start_nor_Click(object sender, EventArgs e)
        {
            if (adb_list.ADB_ITEM != null)
            {
                if (scriptMachine.Running)
                {
                    MessageBox.Show("脚本已经在运行，请等待脚本停止或者手动点击终止运行");
                    return;
                }
                else
                {
                    if(mrfz_ScriptConfig.scriptConfig.enable_stoneeat)
                    {
                        int eatstone = -1;
                        if (cb_eat_stone.Checked)
                        {
                            eatstone = Convert.ToInt32(nud_eatstone.Value);
                        }
                        int eatmdi = -1;
                        if (cb_eat_medi.Checked)
                        {
                            eatmdi = Convert.ToInt32(nud_eatmedi.Value);
                        }
                        scriptMachine.RunInMainForm_Normal(this, adb_list.ADB_ITEM
                            , eatstone, eatmdi);
                    }else
                    {
                        scriptMachine.RunInMainForm_Normal(this, adb_list.ADB_ITEM
                           , -1, -1);
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("未检测到模拟器与模拟器窗口,请等待右侧模拟器检测完毕后再点开始" +
                    "\r\n,请先开启模拟器进入明日方舟-关卡准备界面后再启动");
                return;
            }
        }

        private void btn_stop_nor_Click(object sender, EventArgs e)
        {
            scriptMachine.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mrfz_ScriptUnit.onMsg == null)
            {
                mrfz_ScriptUnit.onMsg += (o) =>
                {
                    this.BeginInvoke(onMsg,o);
                   // this.onMsg?.Invoke(o);
                };
            }
            Thread th2 = new Thread(() =>
            {
                //  var bf= MapDeBuff.CurMapDebuff();
                //98.png 2BUFF 180
                //109
                //  var listAB= equip.GetEquip();
               ImageSave.SaveBAOQUANFriendHeadImgInFriendsSelPage();
                return;
                var chlist = TeamCharName.GetCharNameInTeamEdit(); 
                return;
                int NI = 1;int L = 1;
                var Lnodes = BranchNode.GetCurBranchNodes(NI, L, mrfzGamePage.CatptureImg());
                var U = new mrfz_ScriptUnit(TaskType.Rogue_KuiYing);
                U.DealNode(1,1, Lnodes[0]);
                //var xxxx = BranchNode.GetCurBranchNodes(1, 1, mrfzGamePage.CatptureImg());
                //  ImageSave.SaveTeamCharName();
                //    double dlt = 0;

                //   FriendSel.GetChar(out dlt);
                return;
              var bf=  MapDeBuff.CurMapDebuff(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\98.png"));
            var sf=    MapDeBuff.CurMapDebuff(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\180.png"));
             //   ImageSave.SaveMapDebuff(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\98.png"));
              //  ImageSave.SaveMapDebuff(new Bitmap(@"F:\VS2019Project\MRFZ_Auto_1280_FRIENDSHAN_修改过_以及分辨率WH\bin\Debug\imgs\adb_雷电模拟器\180.png")); 
                //ImageSave.SaveFrinedImgs();
                // ImageSave.Save(ImageSave.SaveType.SaveHeadImgInCharSelPage);
                //  ImageSave.SaveBattleHeadImg();
                return;
                Point p = new Point();
               if( TeamLogo.FindTeam(TeamLogo.TeamType.堡垒战术, out p))
                {
                    adb.Tap(p);
                    Thread.Sleep(1500);

                    adb.Tap(p);
                }
              //  adb.Swipe(new Point(1172, 440), new Point(600, 440), 1000);
               // ImageSave.SaveTeamLogo();
              //  adb.Swipe(new Point(1172, 440), new Point(600, 440), 1000);
              //  ImageSave.SaveTeamLogo();

                //var listxxx= shop_item.RecCurShopItemList(true);
                return;
               // ImageSave.Save( ImageSave.SaveType.SaveBattleHeadImg);
               // mrfz_ScriptUnit uu = new mrfz_ScriptUnit(TaskType.Rogue_KuiYing);
           // // Add_Role_Sel.FindHasHopeIndex();
               // uu.ShopTest();
            });
            th2.Start();
            return;

            var list = shop_item.RecCurShopItemList(false);
            return;
            ImageSave.SaveShopItem();
            return;
            var GOLD =shop_gold.RecCurGolds(null);
            this.onMsg?.Invoke(GOLD.ToString());
            return;//
            //  button1.Enabled = false;
            Thread th = new Thread(() =>
            {
                mrfz_ScriptUnit uu = new mrfz_ScriptUnit(TaskType.Rogue_KuiYing);
                uu.Deal幕间余兴();
            });
            th.Start();

          //  var ItemList = WinItem.GetCurItemList();
            //  scriptMachine.CheckGameWindow();
     //   mrfz_ScriptUnit u = new mrfz_ScriptUnit( mrfz_ScriptUnit.TaskType.Rogue_KuiYing);
 // u.DEAL_TEST();
            return;
            
            //
            // 
            //  adb.ShotCut().Save("D:\\X.png");


            //scriptMachine.RunInMainForm(this,true); 
            // pictureBox1.BackgroundImage = bmp;
        }

    }
}
