
namespace GamePageScript.ui
{
    partial class SkillReadyMapWin
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
            this.btn_add_pr = new System.Windows.Forms.Button();
            this.tb_r_name = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tb_mapname = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tabControl2);
            this.groupBox2.Controls.Add(this.tb_r_name);
            this.groupBox2.Controls.Add(this.btn_add_pr);
            this.groupBox2.Location = new System.Drawing.Point(973, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 281);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标准地图";
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
            // SkillReadyMapWin
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
            this.Name = "SkillReadyMapWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图编辑器---技能准备完毕图"; 
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_add_pr;
        private System.Windows.Forms.TextBox tb_r_name;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tb_mapname;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox2;
    }
}