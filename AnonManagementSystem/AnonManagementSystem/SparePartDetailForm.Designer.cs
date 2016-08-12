namespace AnonManagementSystem
{
    partial class SparePartDetailForm
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
            this.components = new System.ComponentModel.Container();
            this.cmsPicture = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.更新图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.图片另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsDetail = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbRestore = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gbBaseInfo = new System.Windows.Forms.GroupBox();
            this.nUdAmount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStoreDate = new System.Windows.Forms.DateTimePicker();
            this.cmbName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStoreSpot = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbUseType = new System.Windows.Forms.ComboBox();
            this.tbSerialNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbFactory = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpOemDate = new System.Windows.Forms.DateTimePicker();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.ilvEquipment = new AnonManagementSystem.ImageListViewer();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.tsbAddImages = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteImage = new System.Windows.Forms.ToolStripButton();
            this.ofdImage = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cmsPicture.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tsDetail.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbBaseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdAmount)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsPicture
            // 
            this.cmsPicture.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更新图片ToolStripMenuItem,
            this.图片另存为ToolStripMenuItem});
            this.cmsPicture.Name = "cmsPicture";
            this.cmsPicture.Size = new System.Drawing.Size(137, 48);
            this.cmsPicture.Opening += new System.ComponentModel.CancelEventHandler(this.cmsPicture_Opening);
            // 
            // 更新图片ToolStripMenuItem
            // 
            this.更新图片ToolStripMenuItem.Name = "更新图片ToolStripMenuItem";
            this.更新图片ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.更新图片ToolStripMenuItem.Text = "更新图片";
            // 
            // 图片另存为ToolStripMenuItem
            // 
            this.图片另存为ToolStripMenuItem.Name = "图片另存为ToolStripMenuItem";
            this.图片另存为ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.图片另存为ToolStripMenuItem.Text = "图片另存为";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tsDetail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 25);
            this.panel1.TabIndex = 1;
            // 
            // tsDetail
            // 
            this.tsDetail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbRestore});
            this.tsDetail.Location = new System.Drawing.Point(0, 0);
            this.tsDetail.Name = "tsDetail";
            this.tsDetail.Size = new System.Drawing.Size(784, 25);
            this.tsDetail.TabIndex = 0;
            this.tsDetail.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            this.tsbSave.Image = global::AnonManagementSystem.Properties.Resources.diskette1;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(52, 22);
            this.tsbSave.Text = "保存";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbRestore
            // 
            this.tsbRestore.Image = global::AnonManagementSystem.Properties.Resources.clock1;
            this.tsbRestore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRestore.Name = "tsbRestore";
            this.tsbRestore.Size = new System.Drawing.Size(52, 22);
            this.tsbRestore.Text = "恢复";
            this.tsbRestore.Click += new System.EventHandler(this.tsbRestore_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 536);
            this.panel2.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 536);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gbBaseInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "备件信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gbBaseInfo
            // 
            this.gbBaseInfo.Controls.Add(this.nUdAmount);
            this.gbBaseInfo.Controls.Add(this.label1);
            this.gbBaseInfo.Controls.Add(this.dtpStoreDate);
            this.gbBaseInfo.Controls.Add(this.cmbName);
            this.gbBaseInfo.Controls.Add(this.label3);
            this.gbBaseInfo.Controls.Add(this.cmbStoreSpot);
            this.gbBaseInfo.Controls.Add(this.label8);
            this.gbBaseInfo.Controls.Add(this.label14);
            this.gbBaseInfo.Controls.Add(this.cmbUseType);
            this.gbBaseInfo.Controls.Add(this.tbSerialNo);
            this.gbBaseInfo.Controls.Add(this.label9);
            this.gbBaseInfo.Controls.Add(this.label2);
            this.gbBaseInfo.Controls.Add(this.cmbStatus);
            this.gbBaseInfo.Controls.Add(this.label13);
            this.gbBaseInfo.Controls.Add(this.cmbFactory);
            this.gbBaseInfo.Controls.Add(this.label10);
            this.gbBaseInfo.Controls.Add(this.label5);
            this.gbBaseInfo.Controls.Add(this.label4);
            this.gbBaseInfo.Controls.Add(this.dtpOemDate);
            this.gbBaseInfo.Controls.Add(this.cmbModel);
            this.gbBaseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBaseInfo.Location = new System.Drawing.Point(3, 3);
            this.gbBaseInfo.Name = "gbBaseInfo";
            this.gbBaseInfo.Size = new System.Drawing.Size(770, 504);
            this.gbBaseInfo.TabIndex = 1;
            this.gbBaseInfo.TabStop = false;
            // 
            // nUdAmount
            // 
            this.nUdAmount.Location = new System.Drawing.Point(505, 85);
            this.nUdAmount.Name = "nUdAmount";
            this.nUdAmount.Size = new System.Drawing.Size(52, 21);
            this.nUdAmount.TabIndex = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 60;
            this.label1.Text = "入库时间";
            // 
            // dtpStoreDate
            // 
            this.dtpStoreDate.Location = new System.Drawing.Point(396, 137);
            this.dtpStoreDate.Name = "dtpStoreDate";
            this.dtpStoreDate.Size = new System.Drawing.Size(124, 21);
            this.dtpStoreDate.TabIndex = 59;
            // 
            // cmbName
            // 
            this.cmbName.FormattingEnabled = true;
            this.cmbName.Location = new System.Drawing.Point(173, 85);
            this.cmbName.Name = "cmbName";
            this.cmbName.Size = new System.Drawing.Size(154, 20);
            this.cmbName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "备件名称";
            // 
            // cmbStoreSpot
            // 
            this.cmbStoreSpot.FormattingEnabled = true;
            this.cmbStoreSpot.Location = new System.Drawing.Point(173, 137);
            this.cmbStoreSpot.Name = "cmbStoreSpot";
            this.cmbStoreSpot.Size = new System.Drawing.Size(154, 20);
            this.cmbStoreSpot.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(112, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 39;
            this.label8.Text = "库存位置";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(335, 89);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 57;
            this.label14.Text = "用于型号";
            // 
            // cmbUseType
            // 
            this.cmbUseType.FormattingEnabled = true;
            this.cmbUseType.Location = new System.Drawing.Point(396, 85);
            this.cmbUseType.Name = "cmbUseType";
            this.cmbUseType.Size = new System.Drawing.Size(71, 20);
            this.cmbUseType.TabIndex = 10;
            // 
            // tbSerialNo
            // 
            this.tbSerialNo.Location = new System.Drawing.Point(173, 58);
            this.tbSerialNo.Name = "tbSerialNo";
            this.tbSerialNo.Size = new System.Drawing.Size(154, 21);
            this.tbSerialNo.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(112, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 54;
            this.label9.Text = "备件编号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(528, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 50;
            this.label2.Text = "状态";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(563, 137);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(71, 20);
            this.cmbStatus.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(473, 89);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 48;
            this.label13.Text = "数量";
            // 
            // cmbFactory
            // 
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(173, 111);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(154, 20);
            this.cmbFactory.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(112, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 43;
            this.label10.Text = "生产厂家";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(335, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "出厂时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(528, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "型号";
            // 
            // dtpOemDate
            // 
            this.dtpOemDate.Location = new System.Drawing.Point(396, 111);
            this.dtpOemDate.Name = "dtpOemDate";
            this.dtpOemDate.Size = new System.Drawing.Size(124, 21);
            this.dtpOemDate.TabIndex = 6;
            // 
            // cmbModel
            // 
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(563, 111);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(71, 20);
            this.cmbModel.TabIndex = 3;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.ilvEquipment);
            this.tabPage6.Controls.Add(this.toolStrip4);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(776, 510);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "装备图片";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // ilvEquipment
            // 
            this.ilvEquipment.DeleteImgKey = null;
            this.ilvEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilvEquipment.ImgDictionary = null;
            this.ilvEquipment.Location = new System.Drawing.Point(3, 28);
            this.ilvEquipment.Name = "ilvEquipment";
            this.ilvEquipment.Size = new System.Drawing.Size(770, 479);
            this.ilvEquipment.TabIndex = 6;
            // 
            // toolStrip4
            // 
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddImages,
            this.tsbDeleteImage});
            this.toolStrip4.Location = new System.Drawing.Point(3, 3);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(770, 25);
            this.toolStrip4.TabIndex = 5;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // tsbAddImages
            // 
            this.tsbAddImages.Image = global::AnonManagementSystem.Properties.Resources.file_add1;
            this.tsbAddImages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddImages.Name = "tsbAddImages";
            this.tsbAddImages.Size = new System.Drawing.Size(52, 22);
            this.tsbAddImages.Text = "添加";
            this.tsbAddImages.Click += new System.EventHandler(this.tsbAddImages_Click);
            // 
            // tsbDeleteImage
            // 
            this.tsbDeleteImage.Image = global::AnonManagementSystem.Properties.Resources.file_delete1;
            this.tsbDeleteImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteImage.Name = "tsbDeleteImage";
            this.tsbDeleteImage.Size = new System.Drawing.Size(52, 22);
            this.tsbDeleteImage.Text = "删除";
            this.tsbDeleteImage.Click += new System.EventHandler(this.tsbDeleteImage_Click);
            // 
            // ofdImage
            // 
            this.ofdImage.Filter = "图片文件|*.jpg|图片文件|*.png";
            this.ofdImage.Title = "请选择图片";
            // 
            // SparePartDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SparePartDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备详细信息";
            this.Load += new System.EventHandler(this.SparePartDetailForm_Load);
            this.Shown += new System.EventHandler(this.SparePartDetailForm_Shown);
            this.cmsPicture.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tsDetail.ResumeLayout(false);
            this.tsDetail.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbBaseInfo.ResumeLayout(false);
            this.gbBaseInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdAmount)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip tsDetail;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbRestore;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip cmsPicture;
        private System.Windows.Forms.ToolStripMenuItem 更新图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图片另存为ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage6;
        private ImageListViewer ilvEquipment;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton tsbAddImages;
        private System.Windows.Forms.ToolStripButton tsbDeleteImage;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbBaseInfo;
        private System.Windows.Forms.NumericUpDown nUdAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStoreDate;
        private System.Windows.Forms.ComboBox cmbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStoreSpot;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbUseType;
        private System.Windows.Forms.TextBox tbSerialNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbFactory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpOemDate;
        private System.Windows.Forms.ComboBox cmbModel;
    }
}