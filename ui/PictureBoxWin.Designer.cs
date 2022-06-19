
namespace MRFZ_Auto.ui
{
    partial class PictureBoxWin
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
            this.cvs = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.cvs)).BeginInit();
            this.SuspendLayout();
            // 
            // cvs
            // 
            this.cvs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cvs.Location = new System.Drawing.Point(0, 0);
            this.cvs.Name = "cvs";
            this.cvs.Size = new System.Drawing.Size(1280, 720);
            this.cvs.TabIndex = 0;
            this.cvs.TabStop = false;
            // 
            // PictureBoxWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.cvs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PictureBoxWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PictureBoxWin";
            ((System.ComponentModel.ISupportInitialize)(this.cvs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox cvs;
    }
}