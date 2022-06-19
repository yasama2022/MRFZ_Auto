using GamePageScript.script.mrfz;
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
    public partial class mrfz_config_Win : Form
    {
        public mrfz_config_Win()
        {
            InitializeComponent();
            // checkBox1.Checked= mrfz_ScriptConfig.scriptConfig.UseADBCat ;
            textBox1.Text = mrfz_ScriptConfig.scriptConfig.dlt_page_check.ToString(); 
            textBox2.Text = mrfz_ScriptConfig.scriptConfig.dlt_region.ToString();
            textBox3.Text = mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get.ToString();
            textBox4.Text = mrfz_ScriptConfig.scriptConfig.dlt_battle_headimg.ToString();
            numericUpDown1.Value = mrfz_ScriptConfig.scriptConfig.DragRoleToBattleTime_ms;
            textBox1.LostFocus += TextBox1_LostFocus;
            textBox2.LostFocus += TextBox1_LostFocus;
            textBox3.LostFocus += TextBox1_LostFocus;
            textBox4.LostFocus += TextBox1_LostFocus;
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;

            cb_enable_stoneeat.Checked= mrfz_ScriptConfig.scriptConfig.enable_stoneeat ;
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.DragRoleToBattleTime_ms = Convert.ToInt32( numericUpDown1.Value);
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            double dlt = -1d; 
            if(double.TryParse(tb.Text, out dlt))
            {
                if(dlt<4.0||dlt>8.0)
                {
                    if (tb.Equals(textBox1))
                    {

                        tb.Text = (5.00d).ToString();
                    }
                    else
                    {
                        tb.Text = (5.68d).ToString();
                    }
                    MessageBox.Show("超出合法范围: 4.0-8.0");
                }
                else
                {
                    if (tb.Equals(textBox1))
                    {
                        mrfz_ScriptConfig.scriptConfig.dlt_page_check = dlt;
                    }else
                    if (tb.Equals(textBox2))
                    {
                        mrfz_ScriptConfig.scriptConfig.dlt_region = dlt;
                    }
                    else
                    if (tb.Equals(textBox3))
                    {
                        mrfz_ScriptConfig.scriptConfig.dlt_freind_char_get = dlt;
                    }
                    else
                    if (tb.Equals(textBox4))
                    {
                        mrfz_ScriptConfig.scriptConfig.dlt_battle_headimg = dlt;
                    }
                    mrfz_ScriptConfig.scriptConfig.save();
                }
                 
            }
            else
            {
                if (tb.Equals(textBox1))
                {

                    tb.Text = (5.00d).ToString();
                }
                else
                {
                    tb.Text = (5.68d).ToString();
                }

                MessageBox.Show("超出合法范围: 4.0-8.0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            mrfz_ScriptConfig.scriptConfig.enable_stoneeat= cb_enable_stoneeat.Checked; 
        }
    }
}
