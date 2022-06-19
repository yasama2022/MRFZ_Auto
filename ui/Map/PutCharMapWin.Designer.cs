
namespace GamePageScript.ui
{
    partial class PutCharMapEdit
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
            this.btn_pre = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.tb_imgname = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cvs = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.btn_add_pr = new System.Windows.Forms.Button();
            this.tb_r_name = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GB_DIR = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tb_mapname = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.GB_DIR.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_pre
            // 
            this.btn_pre.Location = new System.Drawing.Point(985, 75);
            this.btn_pre.Name = "btn_pre";
            this.btn_pre.Size = new System.Drawing.Size(53, 23);
            this.btn_pre.TabIndex = 12;
            this.btn_pre.Text = "<=";
            this.btn_pre.UseVisualStyleBackColor = true;
            this.btn_pre.Click += new System.EventHandler(this.btn_pre_click);
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(1183, 77);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(53, 23);
            this.btn_next.TabIndex = 13;
            this.btn_next.Text = "=>";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.BTN_NEXT_CLICK);
            // 
            // tb_imgname
            // 
            this.tb_imgname.Location = new System.Drawing.Point(1058, 77);
            this.tb_imgname.Name = "tb_imgname";
            this.tb_imgname.ReadOnly = true;
            this.tb_imgname.Size = new System.Drawing.Size(105, 21);
            this.tb_imgname.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1084, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 25;
            this.label8.Text = "地图名";
            // 
            // cvs
            // 
            this.cvs.Location = new System.Drawing.Point(3, 3);
            this.cvs.Name = "cvs";
            this.cvs.Size = new System.Drawing.Size(960, 540);
            this.cvs.TabIndex = 26;
            this.cvs.Paint += new System.Windows.Forms.PaintEventHandler(this.cvs_Paint);
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
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Location = new System.Drawing.Point(6, 24);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(296, 180);
            this.tabControl2.TabIndex = 33;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(288, 154);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "高台";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(282, 148);
            this.listBox1.TabIndex = 17;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listBox2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(288, 154);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "地面";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 12;
            this.listBox2.Location = new System.Drawing.Point(3, 3);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(282, 148);
            this.listBox2.TabIndex = 18;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.listBox3);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(288, 154);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "状态区域";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // listBox3
            // 
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(0, 0);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(288, 154);
            this.listBox3.TabIndex = 18;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.listBox4);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(288, 154);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "高台点";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // listBox4
            // 
            this.listBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox4.FormattingEnabled = true;
            this.listBox4.ItemHeight = 12;
            this.listBox4.Location = new System.Drawing.Point(3, 3);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(282, 148);
            this.listBox4.TabIndex = 18;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.listBox5);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(288, 154);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "地面点";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // listBox5
            // 
            this.listBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox5.FormattingEnabled = true;
            this.listBox5.ItemHeight = 12;
            this.listBox5.Location = new System.Drawing.Point(3, 3);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(282, 148);
            this.listBox5.TabIndex = 18;
            // 
            // btn_add_pr
            // 
            this.btn_add_pr.Location = new System.Drawing.Point(10, 210);
            this.btn_add_pr.Name = "btn_add_pr";
            this.btn_add_pr.Size = new System.Drawing.Size(117, 23);
            this.btn_add_pr.TabIndex = 18;
            this.btn_add_pr.Text = "添加区域/点";
            this.btn_add_pr.UseVisualStyleBackColor = true;
            this.btn_add_pr.Click += new System.EventHandler(this.btn_add_pr_Click);
            // 
            // tb_r_name
            // 
            this.tb_r_name.Location = new System.Drawing.Point(195, 212);
            this.tb_r_name.Name = "tb_r_name";
            this.tb_r_name.Size = new System.Drawing.Size(100, 21);
            this.tb_r_name.TabIndex = 19;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GB_DIR);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tabControl2);
            this.groupBox2.Controls.Add(this.tb_r_name);
            this.groupBox2.Controls.Add(this.btn_add_pr);
            this.groupBox2.Location = new System.Drawing.Point(969, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 360);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标准地图";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // GB_DIR
            // 
            this.GB_DIR.Controls.Add(this.button1);
            this.GB_DIR.Controls.Add(this.radioButton4);
            this.GB_DIR.Controls.Add(this.radioButton1);
            this.GB_DIR.Controls.Add(this.radioButton2);
            this.GB_DIR.Controls.Add(this.radioButton3);
            this.GB_DIR.Location = new System.Drawing.Point(16, 239);
            this.GB_DIR.Name = "GB_DIR";
            this.GB_DIR.Size = new System.Drawing.Size(276, 36);
            this.GB_DIR.TabIndex = 40;
            this.GB_DIR.TabStop = false;
            this.GB_DIR.Text = "默认放置方向";
            this.GB_DIR.Visible = false;
            this.GB_DIR.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(233, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(192, 14);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(35, 16);
            this.radioButton4.TabIndex = 38;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "右";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(67, 14);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 35;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "上";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(110, 14);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(35, 16);
            this.radioButton2.TabIndex = 36;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "下";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(151, 14);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(35, 16);
            this.radioButton3.TabIndex = 37;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "左";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(160, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "名称";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1183, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(53, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "=>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tb_mapname
            // 
            this.tb_mapname.Location = new System.Drawing.Point(1058, 34);
            this.tb_mapname.Name = "tb_mapname";
            this.tb_mapname.ReadOnly = true;
            this.tb_mapname.Size = new System.Drawing.Size(105, 21);
            this.tb_mapname.TabIndex = 37;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(985, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(53, 23);
            this.button3.TabIndex = 35;
            this.button3.Text = "<=";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // PutCharMapEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 561);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_mapname);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cvs);
            this.Controls.Add(this.tb_imgname);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn_pre);
            this.DoubleBuffered = true;
            this.Name = "PutCharMapEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图编辑器---放置角色";
            this.Load += new System.EventHandler(this.NormalMapEdit_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.GB_DIR.ResumeLayout(false);
            this.GB_DIR.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_pre;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.TextBox tb_imgname;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel cvs;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_add_pr;
        private System.Windows.Forms.TextBox tb_r_name;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tb_mapname;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.GroupBox GB_DIR;
        private System.Windows.Forms.Button button1;
    }
}