using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnonManagementSystem
{
    public partial class AddModifyUser : Form
    {
        private bool _modify = false;
        private int _id;
        public AddModifyUser()
        {
            InitializeComponent();
        }

        public bool Modify
        {
            set { _modify = value; }
        }

        public int Id
        {
            set { _id = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbAuthority_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds.X, e.Bounds.Y + 3);
        }

        private void AddModifyUser_Load(object sender, EventArgs e)
        {
            if (_modify)
            {
                
            }
        }
    }
}
