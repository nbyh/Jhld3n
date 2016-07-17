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
    public partial class SystemSetting : Form
    {
        public SystemSetting()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void tsbModify_Click(object sender, EventArgs e)
        {
            AddModifyUser addModifyUser = new AddModifyUser()
            {
                Modify = true,Id = 0
            };
            addModifyUser.ShowDialog();
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            AddModifyUser addModifyUser=new AddModifyUser();
            addModifyUser.ShowDialog();
        }
    }
}
