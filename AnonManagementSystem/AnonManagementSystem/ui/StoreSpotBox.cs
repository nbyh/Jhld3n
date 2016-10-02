using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class StoreSpotBox : UserControl
    {
        public StoreSpotBox()
        {
            InitializeComponent();
        }
        /** Set or Get the string that represents the value in the box */
        /// <summary>
        /// </summary>
        /// <returns>与该控件关联的文本。</returns>
        public override string Text
        {
            get
            {
                return cmb1.Text + cmb2.Text + cmb3.Text + tb4.Text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    cmb1.Text = value.Substring(0, 1);
                    cmb2.Text = value.Substring(1, 1);
                    cmb3.Text = value.Substring(2, 1);
                    tb4.Text = value.Substring(3);
                }
                else
                {
                    cmb1.Text = "";
                    cmb2.Text = "";
                    cmb3.Text = "";
                    tb4.Text = "";
                }
            }
        }

        public IEnumerable<string> DataSource1
        {
            set { cmb1.DataSource = value; }
        }

        public IEnumerable<string> DataSource2
        {
            set { cmb2.DataSource = value; }
        }

        public IEnumerable<string> DataSource3
        {
            set { cmb3.DataSource = value; }
        }

        public void Clear()
        {
            cmb1.Text = "";
            cmb2.Text = "";
            cmb3.Text = "";
            tb4.Text = "";
        }

        private void cmb1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cmb1.Text != "" && cmb1.Text.Length != cmb1.SelectionLength)
                {
                    cmb2.Focus();
                }
                e.Handled = true;
            }
        }

        private void cmb2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cmb2.Text != "" && cmb2.Text.Length != cmb2.SelectionLength)
                {
                    cmb3.Focus();
                }
                e.Handled = true;
            }
            if (e.KeyChar == 8 && cmb2.Text.Length == 0)
            {
                cmb1.Focus();
                cmb1.SelectionStart = cmb1.Text.Length;
            }
        }

        private void cmb3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cmb3.Text != "" && cmb3.Text.Length != cmb3.SelectionLength)
                {
                    tb4.Focus();
                }
                e.Handled = true;
            }
            if (e.KeyChar == 8 && cmb3.Text.Length == 0)
            {
                cmb2.Focus();
                cmb2.SelectionStart = cmb2.Text.Length;
            }
        }

        private void tb4_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void tb4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8 && tb4.Text.Length == 0)
            {
                cmb3.Focus();
                cmb3.SelectionStart = cmb3.Text.Length;
            }
        }
    }
}
