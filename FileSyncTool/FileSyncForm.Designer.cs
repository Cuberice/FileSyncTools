namespace FileSyncTool
{
	partial class FileSyncForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSyncForm));
			this.btnSync = new System.Windows.Forms.Button();
			this.btnAddSyncDir = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.clDirUpdate = new FileSyncTool.ColorIndicator();
			this.lstbSyncPaths = new System.Windows.Forms.ListBox();
			this.clCopied = new FileSyncTool.ColorIndicator();
			this.clLocked = new FileSyncTool.ColorIndicator();
			this.clError = new FileSyncTool.ColorIndicator();
			this.clHasUpdate = new FileSyncTool.ColorIndicator();
			this.btnRemoveSyncDir = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenProfile = new System.Windows.Forms.ToolStripMenuItem();
			this.testDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.testSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteOldFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.UpdateGUITimer = new System.Windows.Forms.Timer(this.components);
			this.cbxShowCopyResults = new System.Windows.Forms.CheckBox();
			this.UpdateFileWatcherTimer = new System.Windows.Forms.Timer(this.components);
			this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbxNotificationsEnabled = new System.Windows.Forms.CheckBox();
			this.btnCheck = new System.Windows.Forms.Button();
			this.SyncPathItemToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.changeBaseDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSync
			// 
			this.btnSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSync.Location = new System.Drawing.Point(366, 300);
			this.btnSync.Name = "btnSync";
			this.btnSync.Size = new System.Drawing.Size(140, 28);
			this.btnSync.TabIndex = 0;
			this.btnSync.Text = "Sync";
			this.btnSync.UseVisualStyleBackColor = true;
			this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
			// 
			// btnAddSyncDir
			// 
			this.btnAddSyncDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddSyncDir.Location = new System.Drawing.Point(2, 271);
			this.btnAddSyncDir.Name = "btnAddSyncDir";
			this.btnAddSyncDir.Size = new System.Drawing.Size(140, 28);
			this.btnAddSyncDir.TabIndex = 1;
			this.btnAddSyncDir.Text = "Add Directory to Profile";
			this.btnAddSyncDir.UseVisualStyleBackColor = true;
			this.btnAddSyncDir.Click += new System.EventHandler(this.btnAddSyncDir_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.clDirUpdate);
			this.groupBox1.Controls.Add(this.lstbSyncPaths);
			this.groupBox1.Controls.Add(this.clCopied);
			this.groupBox1.Controls.Add(this.clLocked);
			this.groupBox1.Controls.Add(this.clError);
			this.groupBox1.Controls.Add(this.clHasUpdate);
			this.groupBox1.Location = new System.Drawing.Point(2, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(504, 238);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Directories to Sync";
			// 
			// clDirUpdate
			// 
			this.clDirUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clDirUpdate.Location = new System.Drawing.Point(402, 213);
			this.clDirUpdate.Name = "clDirUpdate";
			this.clDirUpdate.Size = new System.Drawing.Size(94, 19);
			this.clDirUpdate.TabIndex = 17;
			// 
			// lstbSyncPaths
			// 
			this.lstbSyncPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstbSyncPaths.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.lstbSyncPaths.FormattingEnabled = true;
			this.lstbSyncPaths.Location = new System.Drawing.Point(6, 16);
			this.lstbSyncPaths.Name = "lstbSyncPaths";
			this.lstbSyncPaths.Size = new System.Drawing.Size(492, 187);
			this.lstbSyncPaths.TabIndex = 0;
			this.lstbSyncPaths.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstSyncPaths_Click);
			this.lstbSyncPaths.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
			this.lstbSyncPaths.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSyncPaths_DoubleClick);
			// 
			// clCopied
			// 
			this.clCopied.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clCopied.Location = new System.Drawing.Point(185, 213);
			this.clCopied.Name = "clCopied";
			this.clCopied.Size = new System.Drawing.Size(94, 19);
			this.clCopied.TabIndex = 16;
			// 
			// clLocked
			// 
			this.clLocked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clLocked.Location = new System.Drawing.Point(93, 213);
			this.clLocked.Name = "clLocked";
			this.clLocked.Size = new System.Drawing.Size(94, 19);
			this.clLocked.TabIndex = 15;
			// 
			// clError
			// 
			this.clError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clError.Location = new System.Drawing.Point(8, 213);
			this.clError.Name = "clError";
			this.clError.Size = new System.Drawing.Size(94, 19);
			this.clError.TabIndex = 13;
			// 
			// clHasUpdate
			// 
			this.clHasUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clHasUpdate.Location = new System.Drawing.Point(285, 213);
			this.clHasUpdate.Name = "clHasUpdate";
			this.clHasUpdate.Size = new System.Drawing.Size(94, 19);
			this.clHasUpdate.TabIndex = 14;
			// 
			// btnRemoveSyncDir
			// 
			this.btnRemoveSyncDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRemoveSyncDir.Location = new System.Drawing.Point(2, 300);
			this.btnRemoveSyncDir.Name = "btnRemoveSyncDir";
			this.btnRemoveSyncDir.Size = new System.Drawing.Size(140, 28);
			this.btnRemoveSyncDir.TabIndex = 3;
			this.btnRemoveSyncDir.Text = "Remove Sync Directory";
			this.btnRemoveSyncDir.UseVisualStyleBackColor = true;
			this.btnRemoveSyncDir.Click += new System.EventHandler(this.btnRemoveSyncDir_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(510, 24);
			this.menuStrip1.TabIndex = 4;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenProfile,
            this.testDatabaseToolStripMenuItem,
            this.addFilesToolStripMenuItem,
            this.testSeriesToolStripMenuItem,
            this.deleteOldFilesToolStripMenuItem,
            this.changeBaseDirectoryToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuOpenProfile
			// 
			this.mnuOpenProfile.Name = "mnuOpenProfile";
			this.mnuOpenProfile.Size = new System.Drawing.Size(193, 22);
			this.mnuOpenProfile.Text = "Load From File";
			this.mnuOpenProfile.Click += new System.EventHandler(this.OpenProfile);
			// 
			// testDatabaseToolStripMenuItem
			// 
			this.testDatabaseToolStripMenuItem.Name = "testDatabaseToolStripMenuItem";
			this.testDatabaseToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.testDatabaseToolStripMenuItem.Text = "Test Database";
			this.testDatabaseToolStripMenuItem.Click += new System.EventHandler(this.testDatabaseToolStripMenuItem_Click);
			// 
			// addFilesToolStripMenuItem
			// 
			this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
			this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.addFilesToolStripMenuItem.Text = "Add Files";
			this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
			// 
			// testSeriesToolStripMenuItem
			// 
			this.testSeriesToolStripMenuItem.Name = "testSeriesToolStripMenuItem";
			this.testSeriesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.testSeriesToolStripMenuItem.Text = "Test Series";
			this.testSeriesToolStripMenuItem.Click += new System.EventHandler(this.testSeriesToolStripMenuItem_Click);
			// 
			// deleteOldFilesToolStripMenuItem
			// 
			this.deleteOldFilesToolStripMenuItem.Name = "deleteOldFilesToolStripMenuItem";
			this.deleteOldFilesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.deleteOldFilesToolStripMenuItem.Text = "Delete Old Files";
			this.deleteOldFilesToolStripMenuItem.Click += new System.EventHandler(this.DeleteOldFiles_Click);
			// 
			// UpdateGUITimer
			// 
			this.UpdateGUITimer.Interval = 500;
			this.UpdateGUITimer.Tick += new System.EventHandler(this.UpdateGUITimer_Tick);
			// 
			// cbxShowCopyResults
			// 
			this.cbxShowCopyResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbxShowCopyResults.AutoSize = true;
			this.cbxShowCopyResults.Location = new System.Drawing.Point(163, 271);
			this.cbxShowCopyResults.Name = "cbxShowCopyResults";
			this.cbxShowCopyResults.Size = new System.Drawing.Size(118, 17);
			this.cbxShowCopyResults.TabIndex = 6;
			this.cbxShowCopyResults.Text = "Show Copy Results";
			this.cbxShowCopyResults.UseVisualStyleBackColor = true;
			// 
			// UpdateFileWatcherTimer
			// 
			this.UpdateFileWatcherTimer.Interval = 10000;
			this.UpdateFileWatcherTimer.Tick += new System.EventHandler(this.UpdateFileWatcherTimer_Tick);
			// 
			// NotifyIcon
			// 
			this.NotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
			this.NotifyIcon.Text = "Directory Sync File Watcher";
			this.NotifyIcon.Visible = true;
			this.NotifyIcon.BalloonTipClicked += new System.EventHandler(this.NotifyIcon_Clicked);
			this.NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClicked);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.cbxNotificationsEnabled);
			this.panel1.Location = new System.Drawing.Point(56, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(454, 24);
			this.panel1.TabIndex = 12;
			// 
			// cbxNotificationsEnabled
			// 
			this.cbxNotificationsEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbxNotificationsEnabled.AutoSize = true;
			this.cbxNotificationsEnabled.Location = new System.Drawing.Point(360, 3);
			this.cbxNotificationsEnabled.Name = "cbxNotificationsEnabled";
			this.cbxNotificationsEnabled.Size = new System.Drawing.Size(84, 17);
			this.cbxNotificationsEnabled.TabIndex = 14;
			this.cbxNotificationsEnabled.Text = "Notifications";
			this.cbxNotificationsEnabled.UseVisualStyleBackColor = true;
			this.cbxNotificationsEnabled.CheckedChanged += new System.EventHandler(this.cbxNotificationsEnabled_Checked);
			// 
			// btnCheck
			// 
			this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCheck.Location = new System.Drawing.Point(366, 271);
			this.btnCheck.Name = "btnCheck";
			this.btnCheck.Size = new System.Drawing.Size(140, 28);
			this.btnCheck.TabIndex = 17;
			this.btnCheck.Text = "Check Files";
			this.btnCheck.UseVisualStyleBackColor = true;
			this.btnCheck.Click += new System.EventHandler(this.BtnCheckNow_Click);
			// 
			// SyncPathItemToolTip
			// 
			this.SyncPathItemToolTip.AutomaticDelay = 200;
			this.SyncPathItemToolTip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.SyncPathItemToolTip.IsBalloon = true;
			this.SyncPathItemToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.ToolTip_Popup);
			// 
			// changeBaseDirectoryToolStripMenuItem
			// 
			this.changeBaseDirectoryToolStripMenuItem.Name = "changeBaseDirectoryToolStripMenuItem";
			this.changeBaseDirectoryToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.changeBaseDirectoryToolStripMenuItem.Text = "Change Base Directory";
			this.changeBaseDirectoryToolStripMenuItem.Click += new System.EventHandler(this.ChangeBaseDir_Click);
			// 
			// FileSyncForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(510, 337);
			this.Controls.Add(this.btnCheck);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cbxShowCopyResults);
			this.Controls.Add(this.btnRemoveSyncDir);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnAddSyncDir);
			this.Controls.Add(this.btnSync);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(526, 375);
			this.Name = "FileSyncForm";
			this.Text = "Directory Synchronizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileSyncForm_Closing);
			this.Load += new System.EventHandler(this.FileSyncForm_Load);
			this.Shown += new System.EventHandler(this.FileSyncForm_Shown);
			this.SizeChanged += new System.EventHandler(this.FileSyncForm_SizeChanged);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form_DragEnter);
			this.groupBox1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.Button btnAddSyncDir;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox lstbSyncPaths;
		private System.Windows.Forms.Button btnRemoveSyncDir;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenProfile;
		private System.Windows.Forms.Timer UpdateGUITimer;
		private System.Windows.Forms.CheckBox cbxShowCopyResults;
		private System.Windows.Forms.Timer UpdateFileWatcherTimer;
		private System.Windows.Forms.NotifyIcon NotifyIcon;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox cbxNotificationsEnabled;
		private ColorIndicator clError;
		private ColorIndicator clHasUpdate;
		private ColorIndicator clLocked;
		private ColorIndicator clCopied;
		private System.Windows.Forms.Button btnCheck;
		private System.Windows.Forms.ToolStripMenuItem testDatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem testSeriesToolStripMenuItem;
		private ColorIndicator clDirUpdate;
		protected System.Windows.Forms.ToolTip SyncPathItemToolTip;
		private System.Windows.Forms.ToolStripMenuItem deleteOldFilesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem changeBaseDirectoryToolStripMenuItem;
	}
}

