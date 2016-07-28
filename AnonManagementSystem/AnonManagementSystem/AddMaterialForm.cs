using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EquipmentInformationData;

namespace AnonManagementSystem
{

    public partial class AddMaterialForm : Form
    {
        public delegate void SaveMaterial(Material material);
        public event SaveMaterial SaveMaterialSucess;

        public AddMaterialForm()
        {
            InitializeComponent();
        }
    }
}
