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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SparePartDetailForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsDetail = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbRestore = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ilvEquipment = new AnonManagementSystem.ImageListViewer();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.tsbAddImages = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteImage = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveImage = new System.Windows.Forms.ToolStripButton();
            this.gbBaseInfo = new System.Windows.Forms.GroupBox();
            this.tbUseType = new System.Windows.Forms.TextBox();
            this.ssbStoreSpot = new AnonManagementSystem.StoreSpotBox();
            this.nUdAmount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStoreDate = new System.Windows.Forms.DateTimePicker();
            this.cmbName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
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
            this.ofdImage = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.tsDetail.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.gbBaseInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdAmount)).BeginInit();
            this.SuspendLayout();
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
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 536);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.gbBaseInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "备件信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ilvEquipment);
            this.groupBox1.Controls.Add(this.toolStrip4);
            this.groupBox1.Location = new System.Drawing.Point(3, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 389);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "备件图片";
            // 
            // ilvEquipment
            // 
            this.ilvEquipment.DeleteImgKey = null;
            this.ilvEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilvEquipment.ImgDictionary = ((System.Collections.Generic.Dictionary<string, System.Drawing.Image>)(resources.GetObject("ilvEquipment.ImgDictionary")));
            this.ilvEquipment.Location = new System.Drawing.Point(3, 42);
            this.ilvEquipment.Name = "ilvEquipment";
            this.ilvEquipment.Size = new System.Drawing.Size(764, 344);
            this.ilvEquipment.TabIndex = 7;
            // 
            // toolStrip4
            // 
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddImages,
            this.tsbDeleteImage,
            this.tsbSaveImage});
            this.toolStrip4.Location = new System.Drawing.Point(3, 17);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(764, 25);
            this.toolStrip4.TabIndex = 6;
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
            // tsbSaveImage
            // 
            this.tsbSaveImage.Image = global::AnonManagementSystem.Properties.Resources.diskette1;
            this.tsbSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveImage.Name = "tsbSaveImage";
            this.tsbSaveImage.Size = new System.Drawing.Size(52, 22);
            this.tsbSaveImage.Text = "保存";
            this.tsbSaveImage.Click += new System.EventHandler(this.tsbSaveImage_Click);
            // 
            // gbBaseInfo
            // 
            this.gbBaseInfo.Controls.Add(this.tbUseType);
            this.gbBaseInfo.Controls.Add(this.ssbStoreSpot);
            this.gbBaseInfo.Controls.Add(this.nUdAmount);
            this.gbBaseInfo.Controls.Add(this.label1);
            this.gbBaseInfo.Controls.Add(this.dtpStoreDate);
            this.gbBaseInfo.Controls.Add(this.cmbName);
            this.gbBaseInfo.Controls.Add(this.label3);
            this.gbBaseInfo.Controls.Add(this.label8);
            this.gbBaseInfo.Controls.Add(this.label14);
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
            this.gbBaseInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBaseInfo.Location = new System.Drawing.Point(3, 3);
            this.gbBaseInfo.Name = "gbBaseInfo";
            this.gbBaseInfo.Size = new System.Drawing.Size(770, 104);
            this.gbBaseInfo.TabIndex = 1;
            this.gbBaseInfo.TabStop = false;
            this.gbBaseInfo.Text = "基本信息";
            // 
            // tbUseType
            // 
            this.tbUseType.Location = new System.Drawing.Point(519, 45);
            this.tbUseType.MaxLength = 15;
            this.tbUseType.Name = "tbUseType";
            this.tbUseType.Size = new System.Drawing.Size(190, 21);
            this.tbUseType.TabIndex = 6;
            // 
            // ssbStoreSpot
            // 
            this.ssbStoreSpot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ssbStoreSpot.Location = new System.Drawing.Point(89, 71);
            this.ssbStoreSpot.MaximumSize = new System.Drawing.Size(188, 20);
            this.ssbStoreSpot.MinimumSize = new System.Drawing.Size(188, 20);
            this.ssbStoreSpot.Name = "ssbStoreSpot";
            this.ssbStoreSpot.Size = new System.Drawing.Size(188, 20);
            this.ssbStoreSpot.TabIndex = 7;
            // 
            // nUdAmount
            // 
            this.nUdAmount.Location = new System.Drawing.Point(658, 71);
            this.nUdAmount.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nUdAmount.Name = "nUdAmount";
            this.nUdAmount.Size = new System.Drawing.Size(33, 21);
            this.nUdAmount.TabIndex = 3;
            this.nUdAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(283, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 60;
            this.label1.Text = "入库时间";
            // 
            // dtpStoreDate
            // 
            this.dtpStoreDate.CustomFormat = "yyyy年MM月dd日";
            this.dtpStoreDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStoreDate.Location = new System.Drawing.Point(342, 71);
            this.dtpStoreDate.Name = "dtpStoreDate";
            this.dtpStoreDate.Size = new System.Drawing.Size(130, 21);
            this.dtpStoreDate.TabIndex = 8;
            // 
            // cmbName
            // 
            this.cmbName.FormattingEnabled = true;
            this.cmbName.Location = new System.Drawing.Point(342, 18);
            this.cmbName.MaxLength = 20;
            this.cmbName.Name = "cmbName";
            this.cmbName.Size = new System.Drawing.Size(188, 20);
            this.cmbName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "备件名称";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 39;
            this.label8.Text = "库存位置";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(484, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 57;
            this.label14.Text = "备注";
            // 
            // tbSerialNo
            // 
            this.tbSerialNo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbSerialNo.Location = new System.Drawing.Point(89, 18);
            this.tbSerialNo.MaxLength = 20;
            this.tbSerialNo.Name = "tbSerialNo";
            this.tbSerialNo.Size = new System.Drawing.Size(188, 21);
            this.tbSerialNo.TabIndex = 0;
            this.tbSerialNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNumDig_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 54;
            this.label9.Text = "备件编号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(484, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 50;
            this.label2.Text = "状态";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(519, 71);
            this.cmbStatus.MaxLength = 6;
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(96, 20);
            this.cmbStatus.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(621, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 48;
            this.label13.Text = "数量";
            // 
            // cmbFactory
            // 
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.Location = new System.Drawing.Point(89, 45);
            this.cmbFactory.MaxLength = 20;
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(188, 20);
            this.cmbFactory.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 43;
            this.label10.Text = "生产厂家";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(283, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 41;
            this.label5.Text = "出厂时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 42;
            this.label4.Text = "型号";
            // 
            // dtpOemDate
            // 
            this.dtpOemDate.CustomFormat = "yyyy年MM月dd日";
            this.dtpOemDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpOemDate.Location = new System.Drawing.Point(342, 45);
            this.dtpOemDate.Name = "dtpOemDate";
            this.dtpOemDate.Size = new System.Drawing.Size(130, 21);
            this.dtpOemDate.TabIndex = 5;
            // 
            // cmbModel
            // 
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(571, 18);
            this.cmbModel.MaxLength = 20;
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(138, 20);
            this.cmbModel.TabIndex = 2;
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
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "SparePartDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "备件详细信息";
            this.Load += new System.EventHandler(this.SparePartDetailForm_Load);
            this.Shown += new System.EventHandler(this.SparePartDetailForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tsDetail.ResumeLayout(false);
            this.tsDetail.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.gbBaseInfo.ResumeLayout(false);
            this.gbBaseInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip tsDetail;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbRestore;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.OpenFileDialog ofdImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbBaseInfo;
        private System.Windows.Forms.NumericUpDown nUdAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpStoreDate;
        private System.Windows.Forms.ComboBox cmbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
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
        private StoreSpotBox ssbStoreSpot;
        private System.Windows.Forms.TextBox tbUseType;
        private System.Windows.Forms.GroupBox groupBox1;
        private ImageListViewer ilvEquipment;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton tsbAddImages;
        private System.Windows.Forms.ToolStripButton tsbDeleteImage;
        private System.Windows.Forms.ToolStripButton tsbSaveImage;
    }
}