namespace AnonManagementSystem
{
    partial class MdiParent
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.EquipMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SparePartsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单条数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据整理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据备份ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeUserMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.tsbExportExcel = new System.Windows.Forms.ToolStripButton();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EquipMenu,
            this.SparePartsMenu,
            this.导出数据ToolStripMenuItem,
            this.数据整理ToolStripMenuItem,
            this.SettingMenu,
            this.ChangeUserMenu,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(884, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // EquipMenu
            // 
            this.EquipMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.EquipMenu.Name = "EquipMenu";
            this.EquipMenu.Size = new System.Drawing.Size(82, 21);
            this.EquipMenu.Text = "装备查询(&F)";
            this.EquipMenu.Click += new System.EventHandler(this.EquipMenu_Click);
            // 
            // SparePartsMenu
            // 
            this.SparePartsMenu.Name = "SparePartsMenu";
            this.SparePartsMenu.Size = new System.Drawing.Size(83, 21);
            this.SparePartsMenu.Text = "备件查询(&E)";
            this.SparePartsMenu.Click += new System.EventHandler(this.SparePartsMenu_Click);
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.单条数据ToolStripMenuItem,
            this.全部数据ToolStripMenuItem});
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.导出数据ToolStripMenuItem.Text = "数据导出";
            // 
            // 单条数据ToolStripMenuItem
            // 
            this.单条数据ToolStripMenuItem.Name = "单条数据ToolStripMenuItem";
            this.单条数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.单条数据ToolStripMenuItem.Text = "单条导出";
            this.单条数据ToolStripMenuItem.Click += new System.EventHandler(this.单条数据ToolStripMenuItem_Click);
            // 
            // 全部数据ToolStripMenuItem
            // 
            this.全部数据ToolStripMenuItem.Name = "全部数据ToolStripMenuItem";
            this.全部数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.全部数据ToolStripMenuItem.Text = "全部导出";
            this.全部数据ToolStripMenuItem.Click += new System.EventHandler(this.全部数据ToolStripMenuItem_Click);
            // 
            // 数据整理ToolStripMenuItem
            // 
            this.数据整理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据备份ToolStripMenuItem,
            this.导入数据ToolStripMenuItem});
            this.数据整理ToolStripMenuItem.Name = "数据整理ToolStripMenuItem";
            this.数据整理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.数据整理ToolStripMenuItem.Text = "数据整理";
            // 
            // 数据备份ToolStripMenuItem
            // 
            this.数据备份ToolStripMenuItem.Name = "数据备份ToolStripMenuItem";
            this.数据备份ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.数据备份ToolStripMenuItem.Text = "数据备份";
            this.数据备份ToolStripMenuItem.Click += new System.EventHandler(this.数据备份ToolStripMenuItem_Click);
            // 
            // 导入数据ToolStripMenuItem
            // 
            this.导入数据ToolStripMenuItem.Name = "导入数据ToolStripMenuItem";
            this.导入数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.导入数据ToolStripMenuItem.Text = "数据合并";
            this.导入数据ToolStripMenuItem.Click += new System.EventHandler(this.导入数据ToolStripMenuItem_Click);
            // 
            // SettingMenu
            // 
            this.SettingMenu.Name = "SettingMenu";
            this.SettingMenu.Size = new System.Drawing.Size(83, 21);
            this.SettingMenu.Text = "系统设置(&T)";
            this.SettingMenu.Click += new System.EventHandler(this.SettingMenu_Click);
            // 
            // ChangeUserMenu
            // 
            this.ChangeUserMenu.Name = "ChangeUserMenu";
            this.ChangeUserMenu.Size = new System.Drawing.Size(84, 21);
            this.ChangeUserMenu.Text = "切换用户(&V)";
            this.ChangeUserMenu.Click += new System.EventHandler(this.ChangeUserMenu_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(61, 21);
            this.helpMenu.Text = "帮助(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "关于(&A)";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 639);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(884, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel.Text = "状态";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.tsbAdd,
            this.tsbDelete,
            this.tsbExportExcel});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(884, 25);
            this.toolStrip.TabIndex = 13;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = global::AnonManagementSystem.Properties.Resources.file_search;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(52, 22);
            this.tsbRefresh.Text = "刷新";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbAdd
            // 
            this.tsbAdd.Image = global::AnonManagementSystem.Properties.Resources.file_add1;
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(52, 22);
            this.tsbAdd.Text = "添加";
            this.tsbAdd.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = global::AnonManagementSystem.Properties.Resources.file_delete1;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(52, 22);
            this.tsbDelete.Text = "删除";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // tsbExportExcel
            // 
            this.tsbExportExcel.Image = global::AnonManagementSystem.Properties.Resources.diskette1;
            this.tsbExportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportExcel.Name = "tsbExportExcel";
            this.tsbExportExcel.Size = new System.Drawing.Size(81, 22);
            this.tsbExportExcel.Text = "导出Excel";
            this.tsbExportExcel.Visible = false;
            this.tsbExportExcel.Click += new System.EventHandler(this.tsbExportExcel_Click);
            // 
            // fbd
            // 
            this.fbd.Description = "请选择指定目录";
            this.fbd.ShowNewFolderButton = false;
            // 
            // MdiParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AnonManagementSystem.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MdiParent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "装备和备件管理系统";
            this.Shown += new System.EventHandler(this.MDIParent_Shown);
            this.SizeChanged += new System.EventHandler(this.MDIParent_SizeChanged);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EquipMenu;
        private System.Windows.Forms.ToolStripMenuItem SparePartsMenu;
        private System.Windows.Forms.ToolStripMenuItem SettingMenu;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbAdd;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripButton tsbExportExcel;
        private System.Windows.Forms.ToolStripMenuItem ChangeUserMenu;
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单条数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据整理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据备份ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入数据ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog fbd;
    }
}



