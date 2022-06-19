
namespace GamePageScript.ui
{
    partial class GamePageCreator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_pre = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.listbox_NextPages = new System.Windows.Forms.ListBox();
            this.tb_clickname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_REC_PPS = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.cb_紧急 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tb_mapname = new System.Windows.Forms.TextBox();
            this.cblist_page_catgory = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cblist_page_type = new System.Windows.Forms.ComboBox();
            this.lb_tips = new System.Windows.Forms.Label();
            this.cb_ban = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_pageName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tb_rcs = new System.Windows.Forms.RichTextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.listbox_regions = new System.Windows.Forms.ListBox();
            this.tb_region_name = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cvs = new System.Windows.Forms.Panel();
            this.button11 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_battle = new System.Windows.Forms.CheckBox();
            this.adb_list = new MRFZ_Auto.ui.Contorls.ADB_Form_List();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1054, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = " 图片存储";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1043, 69);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(302, 21);
            this.textBox2.TabIndex = 9;
            // 
            // btn_pre
            // 
            this.btn_pre.Location = new System.Drawing.Point(3, 191);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(53, 23);
            this.btn_pre.TabIndex = 12;
            this.btn_pre.Text = "<=";
            this.btn_pre.UseVisualStyleBackColor = true;
            this.btn_pre.Click += new System.EventHandler(this.btn_pre_click);
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(3, 220);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(53, 23);
            this.btn_next.TabIndex = 13;
            this.btn_next.Text = "=>";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.BTN_NEXT_CLICK);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "添加识别点";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 20);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "添加点击位置";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listbox_NextPages
            // 
            this.listbox_NextPages.FormattingEnabled = true;
            this.listbox_NextPages.ItemHeight = 12;
            this.listbox_NextPages.Location = new System.Drawing.Point(129, 12);
            this.listbox_NextPages.Name = "listbox_NextPages";
            this.listbox_NextPages.Size = new System.Drawing.Size(158, 112);
            this.listbox_NextPages.TabIndex = 16;
            // 
            // tb_clickname
            // 
            this.tb_clickname.Location = new System.Drawing.Point(6, 63);
            this.tb_clickname.Name = "tb_clickname";
            this.tb_clickname.Size = new System.Drawing.Size(100, 21);
            this.tb_clickname.TabIndex = 17;
            this.tb_clickname.TextChanged += new System.EventHandler(this.tb_clickname_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "点击名";
            // 
            // tb_REC_PPS
            // 
            this.tb_REC_PPS.Location = new System.Drawing.Point(18, 35);
            this.tb_REC_PPS.Name = "tb_REC_PPS";
            this.tb_REC_PPS.Size = new System.Drawing.Size(297, 51);
            this.tb_REC_PPS.TabIndex = 19;
            this.tb_REC_PPS.Text = "识别点位置-颜色";
            this.tb_REC_PPS.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.cb_紧急);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tb_mapname);
            this.groupBox1.Controls.Add(this.cblist_page_catgory);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cblist_page_type);
            this.groupBox1.Controls.Add(this.lb_tips);
            this.groupBox1.Controls.Add(this.cb_ban);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_pageName);
            this.groupBox1.Location = new System.Drawing.Point(1034, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 196);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "页信息";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(123, 174);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 35;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "技能";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(70, 174);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 34;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "放置";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(17, 174);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 33;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "初始";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // cb_紧急
            // 
            this.cb_紧急.AutoSize = true;
            this.cb_紧急.Location = new System.Drawing.Point(238, 144);
            this.cb_紧急.Name = "cb_紧急";
            this.cb_紧急.Size = new System.Drawing.Size(84, 16);
            this.cb_紧急.TabIndex = 32;
            this.cb_紧急.Text = "紧急[肉鸽]";
            this.cb_紧急.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 27;
            this.label11.Text = "地图名";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "分类";
            // 
            // tb_mapname
            // 
            this.tb_mapname.Location = new System.Drawing.Point(63, 142);
            this.tb_mapname.Name = "tb_mapname";
            this.tb_mapname.Size = new System.Drawing.Size(169, 21);
            this.tb_mapname.TabIndex = 26;
            // 
            // cblist_page_catgory
            // 
            this.cblist_page_catgory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cblist_page_catgory.FormattingEnabled = true;
            this.cblist_page_catgory.Items.AddRange(new object[] {
            "控制流程",
            "地图识别",
            "战斗地图"});
            this.cblist_page_catgory.Location = new System.Drawing.Point(63, 116);
            this.cblist_page_catgory.Name = "cblist_page_catgory";
            this.cblist_page_catgory.Size = new System.Drawing.Size(121, 20);
            this.cblist_page_catgory.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 29;
            this.label9.Text = "类型";
            // 
            // cblist_page_type
            // 
            this.cblist_page_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cblist_page_type.FormattingEnabled = true;
            this.cblist_page_type.Items.AddRange(new object[] {
            "肉鸽_愧影",
            "常规"});
            this.cblist_page_type.Location = new System.Drawing.Point(63, 90);
            this.cblist_page_type.Name = "cblist_page_type";
            this.cblist_page_type.Size = new System.Drawing.Size(121, 20);
            this.cblist_page_type.TabIndex = 28;
            // 
            // lb_tips
            // 
            this.lb_tips.AutoSize = true;
            this.lb_tips.Location = new System.Drawing.Point(173, 65);
            this.lb_tips.Name = "lb_tips";
            this.lb_tips.Size = new System.Drawing.Size(59, 12);
            this.lb_tips.TabIndex = 27;
            this.lb_tips.Text = "识别结果:";
            // 
            // cb_ban
            // 
            this.cb_ban.AutoSize = true;
            this.cb_ban.Location = new System.Drawing.Point(14, 61);
            this.cb_ban.Name = "cb_ban";
            this.cb_ban.Size = new System.Drawing.Size(72, 16);
            this.cb_ban.TabIndex = 4;
            this.cb_ban.Text = "禁用该图";
            this.cb_ban.UseVisualStyleBackColor = true;
            this.cb_ban.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(92, 61);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 26;
            this.button8.Text = "检测识别点";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "序号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "序号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "页名";
            // 
            // tb_pageName
            // 
            this.tb_pageName.Location = new System.Drawing.Point(63, 34);
            this.tb_pageName.Name = "tb_pageName";
            this.tb_pageName.Size = new System.Drawing.Size(239, 21);
            this.tb_pageName.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(68, 549);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(341, 131);
            this.tabControl1.TabIndex = 21;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.tb_REC_PPS);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 105);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "特征点识别";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(92, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 20;
            this.button7.Text = "清除识别点";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tb_rcs);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button9);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(333, 105);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "区域色彩相似匹配";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tb_rcs
            // 
            this.tb_rcs.Location = new System.Drawing.Point(6, 38);
            this.tb_rcs.Name = "tb_rcs";
            this.tb_rcs.Size = new System.Drawing.Size(321, 61);
            this.tb_rcs.TabIndex = 23;
            this.tb_rcs.Text = "区域:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(98, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(89, 23);
            this.button5.TabIndex = 22;
            this.button5.Text = "清除所有区域";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(20, 9);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(72, 23);
            this.button9.TabIndex = 21;
            this.button9.Text = "添加区域";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button10);
            this.groupBox3.Controls.Add(this.listbox_regions);
            this.groupBox3.Controls.Add(this.tb_region_name);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.listbox_NextPages);
            this.groupBox3.Controls.Add(this.tb_clickname);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(415, 549);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(594, 131);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "点击位置";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(294, 13);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(91, 23);
            this.button10.TabIndex = 19;
            this.button10.Text = "添加区域颜色";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // listbox_regions
            // 
            this.listbox_regions.FormattingEnabled = true;
            this.listbox_regions.ItemHeight = 12;
            this.listbox_regions.Location = new System.Drawing.Point(391, 12);
            this.listbox_regions.Name = "listbox_regions";
            this.listbox_regions.Size = new System.Drawing.Size(158, 100);
            this.listbox_regions.TabIndex = 20;
            // 
            // tb_region_name
            // 
            this.tb_region_name.Location = new System.Drawing.Point(293, 54);
            this.tb_region_name.Name = "tb_region_name";
            this.tb_region_name.Size = new System.Drawing.Size(91, 21);
            this.tb_region_name.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(314, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "区域名";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(10, 385);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(43, 23);
            this.button6.TabIndex = 23;
            this.button6.Text = "跳转";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(3, 358);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(59, 21);
            this.textBox4.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 329);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "序号";
            // 
            // cvs
            // 
            this.cvs.Location = new System.Drawing.Point(68, 3);
            this.cvs.Name = "cvs";
            this.cvs.Size = new System.Drawing.Size(960, 540);
            this.cvs.TabIndex = 26;
            this.cvs.Paint += new System.Windows.Forms.PaintEventHandler(this.cvs_Paint);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(981, 601);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(117, 23);
            this.button11.TabIndex = 27;
            this.button11.Text = "TEST";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1525, 465);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 30;
            this.label12.Text = "label12";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(1525, 437);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 32;
            this.label14.Text = "label14";
            // 
            // cb_battle
            // 
            this.cb_battle.AutoSize = true;
            this.cb_battle.Location = new System.Drawing.Point(10, 426);
            this.cb_battle.Name = "cb_battle";
            this.cb_battle.Size = new System.Drawing.Size(48, 16);
            this.cb_battle.TabIndex = 33;
            this.cb_battle.Text = "地图";
            this.cb_battle.UseVisualStyleBackColor = true;
            // 
            // adb_list
            // 
            this.adb_list.Location = new System.Drawing.Point(1048, 298);
            this.adb_list.Name = "adb_list";
            this.adb_list.Size = new System.Drawing.Size(245, 235);
            this.adb_list.TabIndex = 34;
            // 
            // GamePageCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 689);
            this.Controls.Add(this.adb_list);
            this.Controls.Add(this.cb_battle);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.cvs);
            this.Controls.Add(this.btn_pre);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.DoubleBuffered = true;
            this.Name = "GamePageCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GamePageCreator";
            this.Load += new System.EventHandler(this.GamePageCreator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_pre;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listbox_NextPages;
        private System.Windows.Forms.TextBox tb_clickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tb_REC_PPS;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_pageName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox cb_ban;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label lb_tips;
        private System.Windows.Forms.CheckBox cb_紧急;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tb_mapname;
        private System.Windows.Forms.ComboBox cblist_page_catgory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cblist_page_type;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox tb_rcs;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Panel cvs;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ListBox listbox_regions;
        private System.Windows.Forms.TextBox tb_region_name;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox cb_battle;
        private MRFZ_Auto.ui.Contorls.ADB_Form_List adb_list;
    }
}