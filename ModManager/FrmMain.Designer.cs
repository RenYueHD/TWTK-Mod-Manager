namespace ModManager
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReload = new System.Windows.Forms.ToolStripMenuItem();
            this.listMods = new System.Windows.Forms.ListBox();
            this.listTables = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1071, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRoot,
            this.tsmiReload});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // loadRoot
            // 
            this.loadRoot.Name = "loadRoot";
            this.loadRoot.Size = new System.Drawing.Size(150, 22);
            this.loadRoot.Text = "打开data目录";
            this.loadRoot.Click += new System.EventHandler(this.loadRoot_Click);
            // 
            // tsmiReload
            // 
            this.tsmiReload.Name = "tsmiReload";
            this.tsmiReload.Size = new System.Drawing.Size(150, 22);
            this.tsmiReload.Text = "重新加载";
            this.tsmiReload.Click += new System.EventHandler(this.tsmiReload_Click);
            // 
            // listMods
            // 
            this.listMods.Dock = System.Windows.Forms.DockStyle.Left;
            this.listMods.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listMods.FormattingEnabled = true;
            this.listMods.Location = new System.Drawing.Point(0, 25);
            this.listMods.Name = "listMods";
            this.listMods.Size = new System.Drawing.Size(232, 557);
            this.listMods.TabIndex = 3;
            this.listMods.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listMods_DrawItem);
            this.listMods.SelectedIndexChanged += new System.EventHandler(this.listMods_SelectedIndexChanged);
            // 
            // listTables
            // 
            this.listTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listTables.FormattingEnabled = true;
            this.listTables.HorizontalExtent = 1200;
            this.listTables.HorizontalScrollbar = true;
            this.listTables.ItemHeight = 12;
            this.listTables.Location = new System.Drawing.Point(232, 25);
            this.listTables.Name = "listTables";
            this.listTables.ScrollAlwaysVisible = true;
            this.listTables.Size = new System.Drawing.Size(839, 557);
            this.listTables.TabIndex = 4;
            this.listTables.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listTables_DrawItem);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 582);
            this.Controls.Add(this.listTables);
            this.Controls.Add(this.listMods);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mod管理器 v0.3 - Power by RenYueHD";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadRoot;
        private System.Windows.Forms.ListBox listMods;
        private System.Windows.Forms.ListBox listTables;
        private System.Windows.Forms.ToolStripMenuItem tsmiReload;
    }
}

