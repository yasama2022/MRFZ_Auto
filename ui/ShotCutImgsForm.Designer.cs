
namespace GamePageScript.ui
{
    partial class ShotCutImgsForm
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
            this.tb_imgpath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.adb_list = new MRFZ_Auto.ui.Contorls.ADB_Form_List();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_imgpath
            // 
            this.tb_imgpath.Location = new System.Drawing.Point(77, 6);
            this.tb_imgpath.Name = "tb_imgpath";
            this.tb_imgpath.Size = new System.Drawing.Size(302, 21);
            this.tb_imgpath.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = " 图片存储";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(51, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(123, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "开始截图";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(475, 360);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "地图名";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(51, 312);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(223, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = "雷电模拟器开始截图--GOLD";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // adb_list
            // 
            this.adb_list.Location = new System.Drawing.Point(530, 22);
            this.adb_list.Name = "adb_list";
            this.adb_list.Size = new System.Drawing.Size(245, 235);
            this.adb_list.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(64, 360);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(223, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "shopgold";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(530, 313);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown1.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(465, 315);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "NODEINDEX";
            // 
            // ShotCutImgsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.adb_list);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_imgpath);
            this.Name = "ShotCutImgsForm";
            this.Text = "ShotCutImgsForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_imgpath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private MRFZ_Auto.ui.Contorls.ADB_Form_List adb_list;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
    }
}