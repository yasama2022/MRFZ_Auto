
namespace MRFZ_Auto.ui.Contorls
{
    partial class ADB_Form_List
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listbox_adb = new System.Windows.Forms.ListBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listbox_adb);
            this.groupBox2.Controls.Add(this.btn_refresh);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 235);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模拟器-窗口选择";
            // 
            // listbox_adb
            // 
            this.listbox_adb.FormattingEnabled = true;
            this.listbox_adb.ItemHeight = 12;
            this.listbox_adb.Location = new System.Drawing.Point(6, 15);
            this.listbox_adb.Name = "listbox_adb";
            this.listbox_adb.Size = new System.Drawing.Size(229, 184);
            this.listbox_adb.TabIndex = 0;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(6, 205);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(108, 23);
            this.btn_refresh.TabIndex = 16;
            this.btn_refresh.Text = "刷新模拟器列表";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // ADB_Form_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "ADB_Form_List";
            this.Size = new System.Drawing.Size(245, 235);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listbox_adb;
        private System.Windows.Forms.Button btn_refresh;
    }
}
