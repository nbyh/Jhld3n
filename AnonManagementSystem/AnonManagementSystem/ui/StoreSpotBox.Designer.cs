namespace AnonManagementSystem
{
    partial class StoreSpotBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb1 = new System.Windows.Forms.ComboBox();
            this.cmb2 = new System.Windows.Forms.ComboBox();
            this.cmb3 = new System.Windows.Forms.ComboBox();
            this.tb4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "架";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "层";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "库";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "顺序";
            // 
            // cmb1
            // 
            this.cmb1.FormattingEnabled = true;
            this.cmb1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmb1.Location = new System.Drawing.Point(20, 0);
            this.cmb1.MaxLength = 1;
            this.cmb1.Name = "cmb1";
            this.cmb1.Size = new System.Drawing.Size(29, 20);
            this.cmb1.TabIndex = 8;
            this.cmb1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb1_KeyPress);
            // 
            // cmb2
            // 
            this.cmb2.FormattingEnabled = true;
            this.cmb2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmb2.Location = new System.Drawing.Point(66, 0);
            this.cmb2.MaxLength = 1;
            this.cmb2.Name = "cmb2";
            this.cmb2.Size = new System.Drawing.Size(29, 20);
            this.cmb2.TabIndex = 9;
            this.cmb2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb2_KeyPress);
            // 
            // cmb3
            // 
            this.cmb3.FormattingEnabled = true;
            this.cmb3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmb3.Location = new System.Drawing.Point(112, 0);
            this.cmb3.MaxLength = 1;
            this.cmb3.Name = "cmb3";
            this.cmb3.Size = new System.Drawing.Size(29, 20);
            this.cmb3.TabIndex = 10;
            this.cmb3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb3_KeyPress);
            // 
            // tb4
            // 
            this.tb4.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tb4.Location = new System.Drawing.Point(170, 0);
            this.tb4.MaxLength = 2;
            this.tb4.Name = "tb4";
            this.tb4.Size = new System.Drawing.Size(19, 21);
            this.tb4.TabIndex = 11;
            this.tb4.Enter += new System.EventHandler(this.tb4_Enter);
            this.tb4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb4_KeyPress);
            // 
            // StoreSpotBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tb4);
            this.Controls.Add(this.cmb3);
            this.Controls.Add(this.cmb2);
            this.Controls.Add(this.cmb1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(190, 20);
            this.MinimumSize = new System.Drawing.Size(190, 20);
            this.Name = "StoreSpotBox";
            this.Size = new System.Drawing.Size(188, 18);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb1;
        private System.Windows.Forms.ComboBox cmb2;
        private System.Windows.Forms.ComboBox cmb3;
        private System.Windows.Forms.TextBox tb4;
    }
}
