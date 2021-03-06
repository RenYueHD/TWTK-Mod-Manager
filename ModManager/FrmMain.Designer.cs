﻿namespace ModManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReload = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCheckLine = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiIgnoreUnActive = new System.Windows.Forms.ToolStripMenuItem();
            this.设置PFM路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置RPFM路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listMods = new System.Windows.Forms.ListBox();
            this.listTables = new System.Windows.Forms.ListBox();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pFMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用RPFM打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listWorkShop = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tsmiHideNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.ctxMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.选项ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1092, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRoot,
            this.toolStripMenuItem1,
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItem1.Text = "打开创意工坊";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.tsmiWorkshop_Click);
            // 
            // tsmiReload
            // 
            this.tsmiReload.Name = "tsmiReload";
            this.tsmiReload.Size = new System.Drawing.Size(150, 22);
            this.tsmiReload.Text = "重新加载";
            this.tsmiReload.Click += new System.EventHandler(this.tsmiReload_Click);
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckLine,
            this.tsmiIgnoreUnActive,
            this.tsmiHideNormal,
            this.设置PFM路径ToolStripMenuItem,
            this.设置RPFM路径ToolStripMenuItem});
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.选项ToolStripMenuItem.Text = "选项";
            // 
            // tsmiCheckLine
            // 
            this.tsmiCheckLine.CheckOnClick = true;
            this.tsmiCheckLine.Name = "tsmiCheckLine";
            this.tsmiCheckLine.Size = new System.Drawing.Size(189, 22);
            this.tsmiCheckLine.Text = "显示词条重复";
            this.tsmiCheckLine.Click += new System.EventHandler(this.tsmiCheckLine_Click);
            // 
            // tsmiIgnoreUnActive
            // 
            this.tsmiIgnoreUnActive.CheckOnClick = true;
            this.tsmiIgnoreUnActive.Name = "tsmiIgnoreUnActive";
            this.tsmiIgnoreUnActive.Size = new System.Drawing.Size(189, 22);
            this.tsmiIgnoreUnActive.Text = "不检测未启用的Mod";
            this.tsmiIgnoreUnActive.Click += new System.EventHandler(this.tsmiIgnoreUnActive_Click);
            // 
            // 设置PFM路径ToolStripMenuItem
            // 
            this.设置PFM路径ToolStripMenuItem.Name = "设置PFM路径ToolStripMenuItem";
            this.设置PFM路径ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.设置PFM路径ToolStripMenuItem.Text = "设置PFM路径";
            this.设置PFM路径ToolStripMenuItem.Click += new System.EventHandler(this.设置PFM路径ToolStripMenuItem_Click);
            // 
            // 设置RPFM路径ToolStripMenuItem
            // 
            this.设置RPFM路径ToolStripMenuItem.Name = "设置RPFM路径ToolStripMenuItem";
            this.设置RPFM路径ToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.设置RPFM路径ToolStripMenuItem.Text = "设置RPFM路径";
            this.设置RPFM路径ToolStripMenuItem.Click += new System.EventHandler(this.设置RPFM路径ToolStripMenuItem_Click);
            // 
            // listMods
            // 
            this.listMods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMods.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listMods.FormattingEnabled = true;
            this.listMods.Location = new System.Drawing.Point(0, 0);
            this.listMods.Name = "listMods";
            this.listMods.Size = new System.Drawing.Size(209, 316);
            this.listMods.TabIndex = 3;
            this.listMods.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listMods_DrawItem);
            this.listMods.SelectedIndexChanged += new System.EventHandler(this.listMods_SelectedIndexChanged);
            this.listMods.DoubleClick += new System.EventHandler(this.listMods_DoubleClick);
            this.listMods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMods_MouseDown);
            // 
            // listTables
            // 
            this.listTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listTables.FormattingEnabled = true;
            this.listTables.HorizontalExtent = 1200;
            this.listTables.HorizontalScrollbar = true;
            this.listTables.ItemHeight = 12;
            this.listTables.Location = new System.Drawing.Point(0, 0);
            this.listTables.Name = "listTables";
            this.listTables.ScrollAlwaysVisible = true;
            this.listTables.Size = new System.Drawing.Size(879, 571);
            this.listTables.TabIndex = 5;
            this.listTables.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listTables_DrawItem);
            this.listTables.DoubleClick += new System.EventHandler(this.listTables_DoubleClick);
            this.listTables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listTables_MouseDown);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pFMToolStripMenuItem,
            this.用RPFM打开ToolStripMenuItem,
            this.打开文件夹ToolStripMenuItem});
            this.ctxMenu.Name = "ctxLeft";
            this.ctxMenu.Size = new System.Drawing.Size(146, 70);
            // 
            // pFMToolStripMenuItem
            // 
            this.pFMToolStripMenuItem.Name = "pFMToolStripMenuItem";
            this.pFMToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.pFMToolStripMenuItem.Text = "用PFM打开";
            this.pFMToolStripMenuItem.Click += new System.EventHandler(this.pFMToolStripMenuItem_Click);
            // 
            // 用RPFM打开ToolStripMenuItem
            // 
            this.用RPFM打开ToolStripMenuItem.Name = "用RPFM打开ToolStripMenuItem";
            this.用RPFM打开ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.用RPFM打开ToolStripMenuItem.Text = "用RPFM打开";
            this.用RPFM打开ToolStripMenuItem.Click += new System.EventHandler(this.用RPFM打开ToolStripMenuItem_Click);
            // 
            // 打开文件夹ToolStripMenuItem
            // 
            this.打开文件夹ToolStripMenuItem.Name = "打开文件夹ToolStripMenuItem";
            this.打开文件夹ToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.打开文件夹ToolStripMenuItem.Text = "打开文件夹";
            this.打开文件夹ToolStripMenuItem.Click += new System.EventHandler(this.打开文件夹ToolStripMenuItem_Click);
            // 
            // listWorkShop
            // 
            this.listWorkShop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listWorkShop.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listWorkShop.FormattingEnabled = true;
            this.listWorkShop.Location = new System.Drawing.Point(0, 0);
            this.listWorkShop.Name = "listWorkShop";
            this.listWorkShop.Size = new System.Drawing.Size(209, 251);
            this.listWorkShop.TabIndex = 4;
            this.listWorkShop.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listWorkShop_DrawItem);
            this.listWorkShop.SelectedIndexChanged += new System.EventHandler(this.listMods_SelectedIndexChanged);
            this.listWorkShop.DoubleClick += new System.EventHandler(this.listMods_DoubleClick);
            this.listWorkShop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMods_MouseDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listTables);
            this.splitContainer1.Size = new System.Drawing.Size(1092, 571);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.TabStop = false;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listMods);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listWorkShop);
            this.splitContainer2.Size = new System.Drawing.Size(209, 571);
            this.splitContainer2.SplitterDistance = 316;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // tsmiHideNormal
            // 
            this.tsmiHideNormal.CheckOnClick = true;
            this.tsmiHideNormal.Name = "tsmiHideNormal";
            this.tsmiHideNormal.Size = new System.Drawing.Size(189, 22);
            this.tsmiHideNormal.Text = "不显示正常目录/词条";
            this.tsmiHideNormal.Click += new System.EventHandler(this.tsmiHideNormal_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 596);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mod管理器 v1.0.2 - Power by RenYueHD";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ctxMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckLine;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem 打开文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pFMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置PFM路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置RPFM路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用RPFM打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiIgnoreUnActive;
        private System.Windows.Forms.ListBox listWorkShop;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStripMenuItem tsmiHideNormal;
    }
}

