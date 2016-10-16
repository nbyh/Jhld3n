namespace AnonManagementSystem
{
    partial class AddMaterialForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbBrief = new System.Windows.Forms.TextBox();
            this.nUdCopybook = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.tbStoreSpot = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbDocNo = new System.Windows.Forms.TextBox();
            this.cmbShape = new System.Windows.Forms.ComboBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.nUdPages = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDocName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDocumentLink = new System.Windows.Forms.TextBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEdition = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ofdMaterial = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdCopybook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUdPages)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.tbBrief);
            this.groupBox1.Controls.Add(this.nUdCopybook);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbStoreSpot);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbDocNo);
            this.groupBox1.Controls.Add(this.cmbShape);
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Controls.Add(this.nUdPages);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbDocName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbDocumentLink);
            this.groupBox1.Controls.Add(this.btnBrowser);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 226);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(44, 191);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "备注";
            // 
            // tbBrief
            // 
            this.tbBrief.Location = new System.Drawing.Point(79, 187);
            this.tbBrief.MaxLength = 10;
            this.tbBrief.Name = "tbBrief";
            this.tbBrief.Size = new System.Drawing.Size(237, 21);
            this.tbBrief.TabIndex = 9;
            // 
            // nUdCopybook
            // 
            this.nUdCopybook.Location = new System.Drawing.Point(284, 76);
            this.nUdCopybook.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nUdCopybook.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUdCopybook.Name = "nUdCopybook";
            this.nUdCopybook.Size = new System.Drawing.Size(32, 21);
            this.nUdCopybook.TabIndex = 4;
            this.nUdCopybook.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "存放位置";
            // 
            // tbStoreSpot
            // 
            this.tbStoreSpot.Location = new System.Drawing.Point(79, 160);
            this.tbStoreSpot.MaxLength = 5;
            this.tbStoreSpot.Name = "tbStoreSpot";
            this.tbStoreSpot.Size = new System.Drawing.Size(237, 21);
            this.tbStoreSpot.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "册数";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 19;
            this.label9.Text = "资料编号";
            // 
            // tbDocNo
            // 
            this.tbDocNo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbDocNo.Location = new System.Drawing.Point(79, 76);
            this.tbDocNo.MaxLength = 8;
            this.tbDocNo.Name = "tbDocNo";
            this.tbDocNo.Size = new System.Drawing.Size(164, 21);
            this.tbDocNo.TabIndex = 3;
            this.tbDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbDocNo_KeyPress);
            // 
            // cmbShape
            // 
            this.cmbShape.FormattingEnabled = true;
            this.cmbShape.Location = new System.Drawing.Point(79, 104);
            this.cmbShape.MaxLength = 4;
            this.cmbShape.Name = "cmbShape";
            this.cmbShape.Size = new System.Drawing.Size(51, 20);
            this.cmbShape.TabIndex = 5;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "yyyy年MM月dd日";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(79, 133);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(131, 21);
            this.dtpDate.TabIndex = 7;
            // 
            // nUdPages
            // 
            this.nUdPages.Location = new System.Drawing.Point(171, 104);
            this.nUdPages.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nUdPages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUdPages.Name = "nUdPages";
            this.nUdPages.Size = new System.Drawing.Size(39, 21);
            this.nUdPages.TabIndex = 6;
            this.nUdPages.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "页数";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "形态";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "资料名称";
            // 
            // tbDocName
            // 
            this.tbDocName.Location = new System.Drawing.Point(79, 49);
            this.tbDocName.MaxLength = 20;
            this.tbDocName.Name = "tbDocName";
            this.tbDocName.Size = new System.Drawing.Size(237, 21);
            this.tbDocName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "存档链接";
            // 
            // tbDocumentLink
            // 
            this.tbDocumentLink.Location = new System.Drawing.Point(79, 22);
            this.tbDocumentLink.Name = "tbDocumentLink";
            this.tbDocumentLink.Size = new System.Drawing.Size(189, 21);
            this.tbDocumentLink.TabIndex = 0;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(274, 21);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(42, 23);
            this.btnBrowser.TabIndex = 1;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = false;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 12);
            this.label8.TabIndex = 16;
            // 
            // tbEdition
            // 
            this.tbEdition.Location = new System.Drawing.Point(59, 248);
            this.tbEdition.Name = "tbEdition";
            this.tbEdition.Size = new System.Drawing.Size(50, 21);
            this.tbEdition.TabIndex = 13;
            this.tbEdition.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 252);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "版本";
            this.label6.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(193, 244);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(274, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ofdMaterial
            // 
            this.ofdMaterial.Title = "请选择已存好位置的资料";
            // 
            // AddMaterialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 278);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbEdition);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMaterialForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加随机资料";
            this.Load += new System.EventHandler(this.AddMaterialForm_Load);
            this.Shown += new System.EventHandler(this.AddMaterialForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUdCopybook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUdPages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbBrief;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbDocNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbShape;
        private System.Windows.Forms.TextBox tbEdition;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.NumericUpDown nUdPages;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDocName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDocumentLink;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog ofdMaterial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbStoreSpot;
        private System.Windows.Forms.NumericUpDown nUdCopybook;
        private System.Windows.Forms.Label label11;
    }
}