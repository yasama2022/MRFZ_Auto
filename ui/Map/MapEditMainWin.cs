using GamePageScript.ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRFZ_Auto.ui
{
    public partial class MapEditMainWin : Form
    {
        public MapEditMainWin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NormalMapEdit nwin = new NormalMapEdit();
            nwin.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            PutCharMapEdit nwin = new PutCharMapEdit();
            nwin.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SkillReadyMapWin nwin = new SkillReadyMapWin();
            nwin.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            UseSkillMapEditWin nwin = new UseSkillMapEditWin();
            nwin.Show();
        }
    }
}
